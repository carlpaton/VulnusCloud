using Business.Model;
using System;

namespace Business.Interface
{
    public interface IOssReportService
    {
        /// <summary>
        /// Creates dbo.Report
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        int CreateInitialReport(DateTime dateTime, int projectId);

        /// <summary>
        /// Creates dbo.Component (if not exist)
        /// Creates dbo.OssIndex, (if not exist)
        /// Create ReportLines
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="coordinatePart"></param>
        /// <returns></returns>
        void CreateInitialReportShell(int reportId, CoordinatePartsModel coordinatePart);

        /// <summary>
        /// Call remote OSS Index API
        /// Update local dbo.oss_index_vulnerabilities with data.
        /// </summary>
        /// <param name="ossIndexId"></param>
        /// <param name="databaseOssIndexVulnerabilities"></param>
        void GetVulnerability(int ossIndexId, bool databaseOssIndexVulnerabilities);
    }
}
