using Data.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class SelectListItemService : ISelectListItemService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPackageTypeRepository _packageTypeRepository;

        public SelectListItemService(IProjectRepository projectRepository, IPackageTypeRepository packageTypeRepository) 
        {
            _projectRepository = projectRepository;
            _packageTypeRepository = packageTypeRepository;
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
            var projectSelectList = new List<SelectListItem>
            {
                new SelectListItem() { Value = "", Text = "-- Please Select --" }
            };
            foreach (var project in _packageTypeRepository.SelectList())
            {
                projectSelectList.Add(new SelectListItem()
                {
                    Value = project.Id.ToString(),
                    Text = project.Name
                });
            }
            return projectSelectList;
        }
    }
}
