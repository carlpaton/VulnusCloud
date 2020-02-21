using System;
using System.IO;
using Business.Interface;
using Common.Serialization.Interface;
using Data.Interface;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IPackageTypeRepository _packageTypeRepository;
        private readonly IOssReportService _ossReportService;
        private readonly ISelectListItemService _selectListItemService;
        private readonly ICoordinatePartsFactory _coordinatePartsFactory;
        private readonly IJsonConvertService _jsonConvertService;

        public FileUploadController(IPackageTypeRepository packageTypeRepository, IOssReportService ossReportService,
            ISelectListItemService selectListItemService, ICoordinatePartsFactory coordinatePartsFactory,
            IJsonConvertService jsonConvertService)
        {
            _packageTypeRepository = packageTypeRepository;
            _ossReportService = ossReportService;
            _selectListItemService = selectListItemService;
            _coordinatePartsFactory = coordinatePartsFactory;
            _jsonConvertService = jsonConvertService;
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

                var type = _packageTypeRepository.Select(fileUploadViewModel.PackageTypeId).Name;
                var coordinatePartsFactory = _coordinatePartsFactory.GetCoordinatePart(extension);

                var reportId = _ossReportService.CreateInitialReport(DateTime.Now, fileUploadViewModel.ProjectId);
                var coordinateParts = coordinatePartsFactory.GetCoordinateParts(_jsonConvertService, type, postedFile);
                foreach (var coordinatePart in coordinateParts)
                {
                    var ossIndex = _ossReportService.CreateInitialReportShell(reportId, coordinatePart);
                    _ossReportService.GetVulnerability(ossIndex, coordinatePart);
                }
            }

            return RedirectToAction("Index", "Report");
        }
    }
}