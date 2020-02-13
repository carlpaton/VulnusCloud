namespace VulnusCloud.Domain.Interface
{
    public interface IBreadcrumbReportService
    {
        string GetReportReports(string projectName);

        string GetReportLines(string projectName, int projectId);

        string GetOssIndexDetails(string projectName, int projectId, int reportId);
    }
}
