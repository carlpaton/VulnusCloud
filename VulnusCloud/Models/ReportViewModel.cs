using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VulnusCloud.Models
{
    public class ReportByProjectViewModel
    {
        public int ProjectId { get; set; }
        
        [Display(Name = "Project")]
        public string ProjectName { get; set; }
    }

    public class ReportViewModel
    {
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        public List<ReportsViewModel> Reports { get; set; }
    }

    public class ReportsViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InsertDate { get; set; }
    }

    public class ReportLineViewModel
    {
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        public List<OssIndexViewModel> OssIndexs { get; set; }
    }
}
