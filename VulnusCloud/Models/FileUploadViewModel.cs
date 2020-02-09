using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VulnusCloud.Models
{
    public class FileUploadViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Id links to npm, nuget ect
        /// </summary>
        [Required]
        [Display(Name = "Package Type")]
        public int PackageTypeId { get; set; }

        /// <summary>
        /// packages.config
        /// [x].csproj
        /// </summary>
        [NotMapped]
        [Required]
        [Display(Name = "Upload File")]
        public List<IFormFile> FormFiles { get; set; }
    }
}
