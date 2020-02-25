using System;
using System.Diagnostics;
using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiCallerService _apiCallerService;

        public HomeController(IApiCallerService apiCallerService) 
        {
            _apiCallerService = apiCallerService;
        }

        public IActionResult Index()
        {
            _apiCallerService.ProcessOssRecords(DateTime.Now);
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
