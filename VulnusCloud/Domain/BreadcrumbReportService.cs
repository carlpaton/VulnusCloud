using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class BreadcrumbReportService : IBreadcrumbReportService
    {
        private readonly string _prefix = "<span class='report-breadcrumbs'>";
        private readonly string _suffix = "</span>";

        public string GetReportReports(string projectName) 
        {
            return $"{_prefix}<a href='/Report'>Report</a> - <span class='report-breadcrumbs--last'>{projectName}</span>{_suffix}";
        }

        public string GetReportLines(string projectName, int projectId)
        {
            return $"{_prefix}<a href='/Report'>Report</a> - <a href='/Report/Reports/{projectId}'>{projectName}</a> - Report Lines{_suffix}";
        }

        public string GetOssIndexDetails(string projectName, int projectId, int reportId)
        {
            return $"{_prefix}<a href='/Report'>Report</a> - <a href='/Report/Reports/{projectId}'>{projectName}</a> - <a href='/Report/ReportLines/{reportId}'>Report Lines</a> - Oss Index{_suffix}";
        }
    }
}
