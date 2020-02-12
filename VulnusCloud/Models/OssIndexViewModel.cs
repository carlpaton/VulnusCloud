using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class OssIndexViewModel
    {
        [Display(Name = "Component Id")]
        public int Id { get; set; }

        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }

        public decimal Score { get; set; }

        public string ScoreFieldClass { get; set; }
    }

    public class OssIndexDetailsViewModel : OssIndexViewModel
    {
        [Display(Name = "Co-ordinates")]
        public string Coordinates { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        [Display(Name = "Expire Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "Http Status")]
        public int HttpStatus { get; set; }

        public List<OssIndexVulnerabilitiesViewModel> Vulnerabilities { get; set; }
    }

    public class OssIndexVulnerabilitiesViewModel 
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Score")]
        public decimal CvssScore { get; set; }

        public string Reference { get; set; }

        public string ScoreFieldClass { get; set; }
    }
}
