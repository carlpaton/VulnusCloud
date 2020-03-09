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
        private readonly IApiCallerService _apiCallerService;

        public FileUploadController(IPackageTypeRepository packageTypeRepository, IOssReportService ossReportService,
            ISelectListItemService selectListItemService, ICoordinatePartsFactory coordinatePartsFactory,
            IApiCallerService apiCallerService)
        {
            _packageTypeRepository = packageTypeRepository;
            _ossReportService = ossReportService;
            _selectListItemService = selectListItemService;
            _coordinatePartsFactory = coordinatePartsFactory;
            _apiCallerService = apiCallerService;
        }

        // GET: FileUpload/Create
        public IActionResult Create()
        {
            ViewData["TopNav_IsSelected"] = "FileUpload";
            ViewData["Project_SelectListItem"] = _selectListItemService.Project();
            ViewData["PackageType_SelectListItem"] = _selectListItemService.PackageType();
            return View();
        }

        /* TODO
         * 1. IActionResult Create should async
         * 2. await service/factory/repository methods
         * 3. refactor the awaited methods to also be async where possible
         */

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
                var reportId = _ossReportService.CreateInitialReport(DateTime.Now, fileUploadViewModel.ProjectId);

                var coordinateParts = _coordinatePartsFactory
                    .GetCoordinatePart(extension, type)
                    .GetCoordinateParts(type, postedFile);

                foreach (var coordinatePart in coordinateParts)
                {
                    _ossReportService.CreateInitialReportShell(reportId, coordinatePart);
                }
            }

            _apiCallerService.ProcessOssRecords(DateTime.Now.AddMinutes(1)); // Add a minute to force the new records to process now
            return RedirectToAction("Index", "Report");
        }
    }
}