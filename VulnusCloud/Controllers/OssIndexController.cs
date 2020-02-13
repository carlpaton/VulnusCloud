using Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VulnusCloud.Domain.Constants;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class OssIndexController : Controller
    {
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IOssIndexVulnerabilitiesRepository _ossIndexVulnerabilitiesRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IScoreClassService _scoreClassService;
        private readonly IBreadcrumbReportService _breadcrumbService;

        public OssIndexController(IOssIndexRepository ossIndexRepository, IOssIndexVulnerabilitiesRepository ossIndexVulnerabilitiesRepository,
            IComponentRepository componentRepository, IScoreClassService scoreClassService, IBreadcrumbReportService breadcrumbService)
        {
            _ossIndexRepository = ossIndexRepository;
            _ossIndexVulnerabilitiesRepository = ossIndexVulnerabilitiesRepository;
            _componentRepository = componentRepository;
            _scoreClassService = scoreClassService;
            _breadcrumbService = breadcrumbService;
        }

        // GET: OssIndex/Details/5
        public IActionResult Details(int id)
        {
            var ossIndex = _ossIndexRepository.SelectByComponentId(id);
            if (ossIndex.Id == 0)
                return NotFound();

            var component = _componentRepository.Select(ossIndex.ComponentId);

            var ossIndexDetailsViewModel = new OssIndexDetailsViewModel
            {
                ComponentName = component.Name,
                Coordinates = ossIndex.Coordinates,
                Description = ossIndex.Description,
                ExpireDate = ossIndex.ExpireDate,
                HttpStatus = ossIndex.HttpStatus,
                Reference = ossIndex.Reference
            };

            ossIndexDetailsViewModel.Vulnerabilities = new List<OssIndexVulnerabilitiesViewModel>();
            var vulnerabilityList = _ossIndexVulnerabilitiesRepository
                .SelectlistByOssIndexId(id)
                .OrderByDescending(x => x.CvssScore)
                .ToList();

            foreach (var vulnerability in vulnerabilityList)
            {
                ossIndexDetailsViewModel.Vulnerabilities.Add(new OssIndexVulnerabilitiesViewModel() 
                {
                    CvssScore = vulnerability.CvssScore,
                    Description = vulnerability.Description,
                    Reference = vulnerability.Reference,
                    Title = vulnerability.Title,
                    ScoreFieldClass = _scoreClassService.SetScoreFieldClass(vulnerability.CvssScore)
                });
            }

            var projectName = HttpContext.Session.GetString(SessionConstants.ProjectName);
            var projectId = HttpContext.Session.GetInt32(SessionConstants.ProjectId) ?? 0;
            var reportId = HttpContext.Session.GetInt32(SessionConstants.ReportId) ?? 0;

            ViewData["TopNav_IsSelected"] = "Report";
            ViewData["Breadcrumbs"] = _breadcrumbService.GetOssIndexDetails(projectName, projectId, reportId);
            return View(ossIndexDetailsViewModel);
        }
    }
}
