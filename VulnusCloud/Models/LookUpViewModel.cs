using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class LookUpViewModel
    {
        [Required]
        [Display(Name = "Package Type")]
        public int PackageTypeId { get; set; }

        public string Namespace { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Version { get; set; }
    }
}
