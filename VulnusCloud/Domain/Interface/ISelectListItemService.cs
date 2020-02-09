using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VulnusCloud.Domain.Interface
{
    public interface ISelectListItemService
    {
        List<SelectListItem> Project();
        List<SelectListItem> PackageType();
    }
}
