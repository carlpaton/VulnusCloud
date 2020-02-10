using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class OssIndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
    }
}
