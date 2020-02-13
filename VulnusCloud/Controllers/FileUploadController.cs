using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Business.Interface;
using Business.Model;
using Common.Http;
using Common.Http.Interface;
using Common.Serialization.Interface;
using Data.Interface;
using Data.Schema;
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
        private readonly IComponentRepository _componentRepository;
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IOssIndexVulnerabilitiesRepository _ossIndexVulnerabilitiesRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IReportLinesRepository _reportLinesRepository;

        public FileUploadController(IJsonConvertService jsonConvertService, IOssIndexService ossIndexService,
            IHttpWebRequestFactory httpWebRequestFactory, ISelectListItemService selectListItemService,
            IComponentRepository componentRepository, IOssIndexRepository ossIndexRepository,
            IOssIndexVulnerabilitiesRepository ossIndexVulnerabilitiesRepository, IReportRepository reportRepository,
            IReportLinesRepository reportLinesRepository)
        {
            _jsonConvertService = jsonConvertService;
            _ossIndexService = ossIndexService;
            _httpWebRequestFactory = httpWebRequestFactory;
            _selectListItemService = selectListItemService;
            _componentRepository = componentRepository;
            _ossIndexRepository = ossIndexRepository;
            _ossIndexVulnerabilitiesRepository = ossIndexVulnerabilitiesRepository;
            _reportRepository = reportRepository;
            _reportLinesRepository = reportLinesRepository;
        }

        // GET: FileUpload/Create
        public IActionResult Create()
        {
            ViewData["TopNav_IsSelected"] = "FileUpload";
            ViewData["Project_SelectListItem"] = _selectListItemService.Project();
            ViewData["PackageType_SelectListItem"] = _selectListItemService.PackageType();
            return View();
        }

        // POST: FileUpload/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProjectId,PackageTypeId,FormFiles")] FileUploadViewModel fileUploadViewModel)
        {
            if (ModelState.IsValid)
            {
                var postedFile = fileUploadViewModel.FormFiles[0];
                var extension = Path.GetExtension(postedFile.FileName);
                var coordinateParts = new List<CoordinatePartsModel>();
                var type = "nuget"; // this needs to be read from `PackageTypeId`

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

                var reportId = _reportRepository.Insert(new ReportModel() 
                {
                    InsertDate = DateTime.Now,
                    ProjectId = fileUploadViewModel.ProjectId
                });

                // TODO - this can be a service
                foreach (var coordinatePart in coordinateParts)
                {
                    // check dbo.component.name on coordinatePart.Name
                    // if it exists, return the id
                    // else create, return the id                  
                    var component = _componentRepository.SelectByName(coordinatePart.Name.Trim());
                    var componentId = component.Id;
                    if (componentId == 0)
                    {
                        componentId = _componentRepository.Insert(new ComponentModel()
                        {
                            Name = coordinatePart.Name.Trim()
                        });
                    }

                    // check dbo.oss_index.component_id
                    // if it exists, check `expire_date` 
                    //    - if older than 30 days, call remote api
                    //    - insert results if any to dbo.oss_index_vulnerabilities
                    //    - update `expire_date` = NOW
                    // else
                    //    - create with `expire_date` = NOW
                    //    - call remote api
                    //    - insert results if any to dbo.oss_index_vulnerabilities
                    var callApi = false;
                    var ossIndex = _ossIndexRepository.SelectByComponentId(componentId);
                    var ossIndexId = ossIndex.Id;
                    if (ossIndexId == 0)
                    {
                        ossIndex = new OssIndexModel()
                        {
                            ComponentId = componentId,
                            ExpireDate = DateTime.Now.AddMonths(1)
                        };

                        ossIndexId = _ossIndexRepository.Insert(ossIndex);
                        ossIndex = _ossIndexRepository.Select(ossIndexId);
                        callApi = true;
                    }
                    else if (ossIndex.ExpireDate < DateTime.Now.AddMonths(1))
                        callApi = true;

                    _reportLinesRepository.Insert(new ReportLinesModel()
                    {
                        OssIndexId = ossIndexId,
                        ReportId = reportId
                    });

                    if (callApi)
                    {
                        var coordinates = _ossIndexService.GetCoordinates(coordinatePart);
                        var endPoint = $"https://ossindex.sonatype.org/api/v3/component-report/{coordinates}"; // TODO ~ read from config

                        var request = _httpWebRequestFactory.Create(endPoint);
                        request.Method = WebRequestMethods.Http.Get;
                        request.ContentType = HttpConstants.JsonContentType;

                        try
                        {
                            using (var response = request.GetResponse())
                            {
                                using (var streamReader = new StreamReader(response.GetResponseStream()))
                                {
                                    var responseString = streamReader.ReadToEnd();
                                    var componentReport = _jsonConvertService.ToObject<ComponentReportModel>(responseString);

                                    // TODO - consideration:
                                    //    - perhaps update `dbo.oss_index` if the data has changed

                                    ossIndex.Coordinates = coordinates;
                                    ossIndex.Description = componentReport.description;
                                    ossIndex.Reference = componentReport.reference;
                                    ossIndex.ExpireDate = DateTime.Now.AddMonths(1);
                                    ossIndex.HttpStatus = (int)HttpStatusCode.OK;

                                    if (decimal.TryParse(coordinatePart.Version, out decimal coordinatePartVersion))
                                        ossIndex.Version = coordinatePartVersion;

                                    _ossIndexRepository.Update(ossIndex);

                                    foreach (var vulnerability in componentReport.vulnerabilities)
                                    {
                                        // TODO
                                        // delete `[vulnuscloud].[dbo].[oss_index_vulnerabilities].[oss_index_id]`

                                        var ossIndexVulnerabilitiesModel = new OssIndexVulnerabilitiesModel()
                                        {
                                            Cve = vulnerability.cve,
                                            CvssScore = vulnerability.cvssScore,
                                            CvssVector = vulnerability.cvssVector,
                                            Description = vulnerability.description,
                                            InsertDate = DateTime.Now,
                                            OssId = vulnerability.id,
                                            OssIndexId = ossIndexId,
                                            Reference = vulnerability.reference,
                                            Title = vulnerability.title
                                        };

                                        _ossIndexVulnerabilitiesRepository.Insert(ossIndexVulnerabilitiesModel);
                                    }
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response is HttpWebResponse response)
                            {
                                if (response.StatusCode == HttpStatusCode.TooManyRequests) 
                                {
                                    ossIndex.HttpStatus = (int)HttpStatusCode.TooManyRequests;
                                    _ossIndexRepository.Update(ossIndex);

                                    // TODO
                                    // Consider
                                    // 1. Throtteling at this point (this method is not async so the user wont know)
                                    // 2. Halt all API calls, defer them for later

                                    // 2 above would need an arcutectual change above
                                    //      - a. the user's file content would be uploaded to `oss_index_queue`
                                    //      - b. async process run to query for the data
                                    //      - c. include this in the reports page so the user is aware the system is still `fetching data`
                                }
                            }
                        }
                        catch (Exception ex) 
                        {
                            // TODO
                            // 1. Consider that http_status would now be 0 for this record.
                            // 2. logging
                        }

                    }
                }
            }

            return RedirectToAction("Index", "Report");
        }
    }
}