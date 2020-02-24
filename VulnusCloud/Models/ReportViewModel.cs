using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class ReportByProjectViewModel
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        [Display(Name = "Current Score")]
        public decimal CurrentScore { get; set; }

        public string Status { get; set; }
    }

    public class ReportViewModel
    {
        //[Display(Name = "Project")]
        //public string ProjectName { get; set; }

        public List<ReportsViewModel> Reports { get; set; }
    }

    public class ReportsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Upload Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime InsertDate { get; set; }

        public decimal Score { get; set; }
    }

    public class ReportLineViewModel
    {
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        public List<OssIndexViewModel> OssIndexs { get; set; }
    }
}
