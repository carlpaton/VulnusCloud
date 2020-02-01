using System.Threading.Tasks;
using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IJsonConvertService _jsonConvertService;

        public FileUploadController(IJsonConvertService jsonConvertService)
        {
            _jsonConvertService = jsonConvertService;
        }

        // GET: FileUpload/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FileUpload/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PackageType,FormFiles")] FileUploadViewModel fileUploadViewModel)
        {
            if (ModelState.IsValid)
            {
                var postedFile = fileUploadViewModel.FormFiles[0];
                var packagesConfigFileModel = _jsonConvertService.XmlFileToObject<PackagesConfigFileModel>(postedFile);
                var csProjFileModel = _jsonConvertService.XmlFileToObject<CsProjFileModel>(postedFile);
            }

            return RedirectToAction(nameof(Create));
        }
    }
}