using Business.Interface;
using Business.Model;
using Data.Interface;
using Data.Schema;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class LookUpController : Controller
    {
        private readonly ISelectListItemService _selectListItemService;
        private readonly IComponentRepository _componentRepository;
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly ICoordinatesService _coordinatesService;
        private readonly IPackageTypeRepository _packageTypeRepository;

        public LookUpController(ISelectListItemService selectListItemService, IComponentRepository componentRepository,
            IOssIndexRepository ossIndexRepository, ICoordinatesService coordinatesService,
            IPackageTypeRepository packageTypeRepository) 
        {
            _selectListItemService = selectListItemService;
            _componentRepository = componentRepository;
            _ossIndexRepository = ossIndexRepository;
            _coordinatesService = coordinatesService;
            _packageTypeRepository = packageTypeRepository;
        }

        // GET: LookUp/Index
        public IActionResult Index()
        {
            ViewData["PackageType_SelectListItem"] = _selectListItemService.PackageType();
            return View();
        }

        // POST: FileUpload/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PackageTypeId,Namespace,Name,Version")] LookUpViewModel lookUpViewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO ~ extract this magic into `OssReportService` (where it was copied from lol)


                // create CoordinatePartsModel
                var coordinatePart = new CoordinatePartsModel
                {
                    Name = lookUpViewModel.Name,
                    Namespace = lookUpViewModel.Namespace,
                    Type = _packageTypeRepository.Select(lookUpViewModel.PackageTypeId).Name,
                    Version = lookUpViewModel.Version.ToString()
                };


                // create component
                var component = _componentRepository.SelectByName(lookUpViewModel.Name.Trim());
                var componentId = component.Id;
                if (componentId == 0)
                {
                    componentId = _componentRepository.Insert(new ComponentModel()
                    {
                        Name = lookUpViewModel.Name.Trim()
                    });
                }


                // create `[vulnuscloud].[dbo].[oss_index]`
                var ossIndex = _ossIndexRepository.SelectByComponentId(componentId);
                var ossIndexId = ossIndex.Id;
                if (ossIndexId == 0)
                {
                    ossIndex = new OssIndexModel()
                    {
                        ComponentId = componentId,
                        ExpireDate = DateTime.Now.AddMonths(1),
                        HttpStatus = (int)HttpStatusCode.Processing,
                        HttpStatusDate = DateTime.Now,
                        Coordinates = _coordinatesService.GetCoordinates(coordinatePart)
                };
                  
                    ossIndexId = _ossIndexRepository.Insert(ossIndex);
                    ossIndex = _ossIndexRepository.Select(ossIndexId);
                }


                // hack to reuse service
                //OssReportService.GetVulnerability


                // hack to read the results <3
                //IOssIndexVulnerabilitiesRepository.SelectlistByOssIndexId

            }

            return RedirectToAction("Index", "LookUp");
        }
    }
}