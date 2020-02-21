using Business.Model;
using Data.Schema;
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
        /// return OssIndex
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="coordinatePart"></param>
        /// <returns></returns>
        OssIndexModel CreateInitialReportShell(int reportId, CoordinatePartsModel coordinatePart);

        void GetVulnerability(OssIndexModel ossIndex, CoordinatePartsModel coordinatePart);
    }
}
