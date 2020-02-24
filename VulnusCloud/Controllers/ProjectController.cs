using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interface;
using Data.Interface;
using Data.Schema;
using Microsoft.AspNetCore.Mvc;
using VulnusCloud.Models;

namespace VulnusCloud.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IApiCallerService _apiCallerService;

        public ProjectController(IProjectRepository projectRepository, IApiCallerService apiCallerService)
        {
            _projectRepository = projectRepository;
            _apiCallerService = apiCallerService;
        }

        // GET: Project
        public IActionResult Index()
        {
            _apiCallerService.ProcessOssRecords();

            var projectViewModelList = new List<ProjectViewModel>();
            var projectList = _projectRepository
                .SelectList()
                .OrderBy(x => x.ProjectName)
                .ToList();

            // TODO ~ mapping concearn outside of the controllers scope
            foreach (var project in projectList)
            {
                projectViewModelList.Add(new ProjectViewModel() 
                {
                    Id = project.Id,
                    ProjectName = project.ProjectName
                });
            }

            SetTopNavSelected();
            return View(projectViewModelList);
        }

        // GET: Project/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var project = _projectRepository.Select(id ?? 0);
            if (project.Id == 0)
                return NotFound();

            // TODO ~ mapping
            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName
            };

            SetTopNavSelected();
            return View(projectViewModel);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ProjectName")] ProjectViewModel projectViewModel)
        {
            if (id != projectViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var project = _projectRepository.Select(id);
                if (project.Id == 0)
                    return NotFound();

                project.ProjectName = projectViewModel.ProjectName;
                _projectRepository.Update(project);

                return RedirectToAction(nameof(Index));
            }

            SetTopNavSelected();
            return View(projectViewModel);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            SetTopNavSelected();
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProjectName")] ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                _projectRepository.Insert(new ProjectModel() 
                {
                    ProjectName = projectViewModel.ProjectName
                });

                return RedirectToAction(nameof(Index));
            }

            SetTopNavSelected();
            return View(projectViewModel);
        }

        // DELETE: Project/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var jsonResponseModel = new JsonResponseModel() 
            { 
                Pass = false,
                Message = "Something went wrong please try again later."
            };

            if (id == 0)
                return Json(jsonResponseModel);

            var project = _projectRepository.Select(id);
            if (project.Id == 0)
                return Json(jsonResponseModel);

            try 
            {
                _projectRepository.Delete(id);

                jsonResponseModel.Pass = true;
                jsonResponseModel.Message = "Record deleted ok.";
            }
            catch (Exception ex)
            {
                // TODO loggingService?

                jsonResponseModel.Message = ex.Message;
                return Json(jsonResponseModel);
            }

            return Json(jsonResponseModel);
        }

        private void SetTopNavSelected()
        {
            ViewData["TopNav_IsSelected"] = "Project";
        }
    }
}
