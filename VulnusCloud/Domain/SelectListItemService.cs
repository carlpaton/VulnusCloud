using Data.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class SelectListItemService : ISelectListItemService
    {
        private readonly IProjectRepository _projectRepository;

        public SelectListItemService(IProjectRepository projectRepository) 
        {
            _projectRepository = projectRepository;
        }

        public List<SelectListItem> Project()
        {
            var projectSelectList = new List<SelectListItem>
            {
                new SelectListItem() { Value = "", Text = "-- Please Select --" }
            };
            foreach (var project in _projectRepository.SelectList())
            {
                projectSelectList.Add(new SelectListItem() 
                {
                    Value = project.Id.ToString(),
                    Text = project.ProjectName
                });
            }
            return projectSelectList;
        }

        public List<SelectListItem> PackageType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Value = "", Text = "-- Please Select --" },
                new SelectListItem() { Value = "1", Text = "Nuget" }
            };
        }
    }
}
