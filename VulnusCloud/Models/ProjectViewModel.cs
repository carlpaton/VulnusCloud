using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
    }
}
