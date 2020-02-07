using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VulnusCloud.Models
{
    public class FileUploadViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// npm, nuget ect
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// packages.config
        /// [x].csproj
        /// </summary>
        [NotMapped]
        public List<IFormFile> FormFiles { get; set; }
    }
}
