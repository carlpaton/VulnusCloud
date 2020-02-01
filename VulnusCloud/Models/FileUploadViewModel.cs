using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace VulnusCloud.Models
{
    public class FileUploadViewModel
    {
        /// <summary>
        /// npm, nuget ect
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// packages.config
        /// [x].csproj
        /// </summary>
        public List<IFormFile> FormFiles { get; set; }
    }
}
