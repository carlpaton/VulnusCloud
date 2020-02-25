using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Business.Interface;
using Data.Interface;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Domain.Interface;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiCallerService _apiCallerService;
        private readonly IProjectRepository _projectRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IScoreService _scoreService;
        private readonly IOssIndexStatusService _ossIndexStatusService;

        public HomeController(IApiCallerService apiCallerService, IProjectRepository projectRepository,
            IReportRepository reportRepository, IScoreService scoreService,
            IOssIndexStatusService ossIndexStatusService) 
        {
            _apiCallerService = apiCallerService;
            _projectRepository = projectRepository;
            _reportRepository = reportRepository;
            _scoreService = scoreService;
            _ossIndexStatusService = ossIndexStatusService;
        }

        public IActionResult Index()
        {
            _apiCallerService.ProcessOssRecords(DateTime.Now);

            // TODO this was copied from `ReportController - Index`, consider a service that returns a list of `reportByProjectViewModel`
            var projectList = _projectRepository
                .SelectList()
                .OrderBy(x => x.ProjectName)
                .ToList();

            var reportList = _reportRepository.SelectList();
            var sb = new StringBuilder();

            foreach (var project in projectList)
            {
                if (reportList.Any(report => report.ProjectId == project.Id))
                {
                    // TODO - inject as service
                    var score = _scoreService.GetCurrentScoreByProjectId(project.Id);
                    sb.AppendFormat("<button type='button' class='btn {0}'>", score == 0 ? "btn-primary" : "btn-danger");
                    sb.AppendFormat("{0} <span class='badge badge-light'>{1}</span>", project.ProjectName, score);
                    sb.Append("</button> ");
                }
            }

            ViewData["Buttons_Helper"] = sb.ToString();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
