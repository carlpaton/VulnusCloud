using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class SelectListItemService : ISelectListItemService
    {
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
