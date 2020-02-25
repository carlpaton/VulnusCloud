using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Business.Interface;
using Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Domain.Constants;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportLinesRepository _reportLinesRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IOssIndexVulnerabilitiesRepository _ossIndexVulnerabilitiesRepository;
        private readonly IScoreService _scoreService;
        private readonly IScoreClassService _scoreClassService;
        private readonly IBreadcrumbReportService _breadcrumbService;
        private readonly IOssIndexStatusService _ossIndexStatusService;
        private readonly IApiCallerService _apiCallerService;

        public ReportController(IReportRepository reportRepository, IReportLinesRepository reportLinesRepository, 
            IProjectRepository projectRepository, IOssIndexRepository ossIndexRepository,
            IComponentRepository componentRepository, IOssIndexVulnerabilitiesRepository ossIndexVulnerabilitiesRepository,
            IScoreService scoreService, IScoreClassService scoreClassService,
            IBreadcrumbReportService breadcrumbService, IOssIndexStatusService ossIndexStatusService,
            IApiCallerService apiCallerService)
        {
            _reportRepository = reportRepository;
            _reportLinesRepository = reportLinesRepository;
            _projectRepository = projectRepository;
            _ossIndexRepository = ossIndexRepository;
            _componentRepository = componentRepository;
            _ossIndexVulnerabilitiesRepository = ossIndexVulnerabilitiesRepository;
            _scoreService = scoreService;
            _scoreClassService = scoreClassService;
            _breadcrumbService = breadcrumbService;
            _ossIndexStatusService = ossIndexStatusService;
            _apiCallerService = apiCallerService;
        }

        // GET: Report
        public IActionResult Index()
        {
            _apiCallerService.ProcessOssRecords(DateTime.Now);

            // TODO this was duplicated to `HomeController.cs - Index`, consider a service that returns a list of `reportByProjectViewModel`
            var reportByProjectViewModel = new List<ReportByProjectViewModel>();
            var projectList = _projectRepository
                .SelectList()
                .OrderBy(x => x.ProjectName)
                .ToList();

            var reportList = _reportRepository.SelectList();

            foreach (var project in projectList)
            {
                if (reportList.Any(report => report.ProjectId == project.Id)) // TODO - consider using a view for this join
                {
                    reportByProjectViewModel.Add(new ReportByProjectViewModel()
                    {
                        ProjectId = project.Id,
                        ProjectName = project.ProjectName,
                        CurrentScore = _scoreService.GetCurrentScoreByProjectId(project.Id),
                        Status = _ossIndexStatusService.GetCurrentStatusByProjectId(project.Id)
                    });
                }
            }

            ViewData["Status_Helper"] = _ossIndexStatusService.GetHelperStatusByProjectId(reportByProjectViewModel.Select(x => x.ProjectId).ToList());
            SetTopNavSelected();
            return View(reportByProjectViewModel);
        }

        // GET: Report/Reports/5
        public IActionResult Reports(int id)
        {
            var project = _projectRepository.Select(id);
            if (project.Id == 0)
                return NotFound();

            var reportViewModel = new ReportViewModel
            {
                Reports = new List<ReportsViewModel>()
            };

            foreach (var report in _reportRepository.SelectByProjectId(id))
            {
                reportViewModel.Reports.Add(new ReportsViewModel() 
                {
                    Id = report.Id,
                    InsertDate = report.InsertDate,
                    Score = _scoreService.GetScoreByReportId(report.Id),
                    Status = _ossIndexStatusService.GetCurrentStatusByProjectId(project.Id)
                });
            }

            reportViewModel.Reports = reportViewModel.Reports
                .OrderByDescending(x => x.Id)
                .ToList();

            SetTopNavSelected();
            ViewData["Breadcrumbs"] = _breadcrumbService.GetReportReports(project.ProjectName);
            return View(reportViewModel);
        }

        // GET: Report/ReportLines/5
        public IActionResult ReportLines(int id)
        {
            var report = _reportRepository.Select(id);
            var project = _projectRepository.Select(report.ProjectId);

            var reportLineViewModel = new ReportLineViewModel
            {
                ProjectName = project.ProjectName,
                OssIndexs = new List<OssIndexViewModel>()
            };
            foreach (var reportLine in _reportLinesRepository.SelectListByReportId(report.Id))
            {
                var ossIndex = _ossIndexRepository.Select(reportLine.OssIndexId);
                var component = _componentRepository.Select(ossIndex.ComponentId);
                var score = _scoreService.GetScoreByOssIndexId(reportLine.OssIndexId);

                Enum.TryParse(ossIndex.HttpStatus.ToString(), out HttpStatusCode httpStatusCode);

                reportLineViewModel.OssIndexs.Add(new OssIndexViewModel() 
                {
                    Id = reportLine.OssIndexId,
                    ComponentName = component.Name,
                    Score = score,
                    ScoreFieldClass = _scoreClassService.SetScoreFieldClass(score),
                    Status = httpStatusCode.ToString()
                });
            }

            HttpContext.Session.SetString(SessionConstants.ProjectName, project.ProjectName);
            HttpContext.Session.SetInt32(SessionConstants.ProjectId, project.Id);
            HttpContext.Session.SetInt32(SessionConstants.ReportId, id);

            SetTopNavSelected();
            ViewData["Breadcrumbs"] = _breadcrumbService.GetReportLines(project.ProjectName, project.Id);
            return View(reportLineViewModel);
        }

        private void SetTopNavSelected()
        {
            ViewData["TopNav_IsSelected"] = "Report";
        }
    }
}
