using System.Collections.Generic;
using System.IO;
using System.Net;
using Business.Interface;
using Business.Model;
using Common.Http;
using Common.Http.Interface;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IJsonConvertService _jsonConvertService;
        private readonly IOssIndexService _ossIndexService;
        private readonly IHttpWebRequestFactory _httpWebRequestFactory;
        private readonly ISelectListItemService _selectListItemService;

        public FileUploadController(IJsonConvertService jsonConvertService, IOssIndexService ossIndexService, 
            IHttpWebRequestFactory httpWebRequestFactory, ISelectListItemService selectListItemService)
        {
            _jsonConvertService = jsonConvertService;
            _ossIndexService = ossIndexService;
            _httpWebRequestFactory = httpWebRequestFactory;
            _selectListItemService = selectListItemService;
        }

        // GET: FileUpload/Create
        public IActionResult Create()
        {
            ViewData["PackageType_SelectListItem"] = _selectListItemService.PackageType();
            return View();
        }

        // POST: FileUpload/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ComponentId,PackageTypeId,FormFiles")] FileUploadViewModel fileUploadViewModel)
        {
            if (ModelState.IsValid)
            {
                var postedFile = fileUploadViewModel.FormFiles[0];
                var extension = Path.GetExtension(postedFile.FileName);
                var coordinateParts = new List<CoordinatePartsModel>();
                var type = "nuget";

                // TODO - this can be a factory
                if (extension.Equals(".config")) 
                {
                    var packagesConfigFileModel = _jsonConvertService.XmlFileToObject<PackagesConfigFileModel>(postedFile);
                    foreach (var package in packagesConfigFileModel.packages.package)
                    {
                        coordinateParts.Add(new CoordinatePartsModel() 
                        {
                            Name = package.id,
                            Version = package.version,
                            Type = type
                        });
                    }
                } 
                else if (extension.Equals(".csproj")) 
                {
                    var csProjFileModel = _jsonConvertService.XmlFileToObject<CsProjFileModel>(postedFile);
                    foreach (var packageReference in csProjFileModel.Project.ItemGroup.PackageReference)
                    {
                        coordinateParts.Add(new CoordinatePartsModel()
                        {
                            Name = packageReference.Include,
                            Version = packageReference.Version,
                            Type = type
                        });
                    }
                }

                // TODO - this can be a service
                foreach (var coordinatePart in coordinateParts)
                {
                    var coordinates = _ossIndexService.GetCoordinates(coordinatePart);
                    var endPoint = $"https://ossindex.sonatype.org/api/v3/component-report/{coordinates}";

                    var request = _httpWebRequestFactory.Create(endPoint);
                    request.Method = WebRequestMethods.Http.Get;
                    request.ContentType = HttpConstants.JsonContentType;

                    using (var response = request.GetResponse())
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var responseString = streamReader.ReadToEnd();
                            var componentReport = _jsonConvertService.ToObject<ComponentReportModel>(responseString);

                            if (componentReport.vulnerabilities.Count > 0) 
                            {
                                var ohboy = "ohboy";
                            }
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Create));
        }
    }
}