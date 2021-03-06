﻿using System;
using System.Threading.Tasks;

namespace Business.Interface
{
    /// <summary>
    /// Looks for [vulnuscloud].[dbo].[oss_index].[http_status] == 0/429 and do the call, then update as `_ossReportService.GetVulnerability` would
    /// </summary>
    public interface IApiCallerService
    {
        /// <summary>
        /// Looks for the oldest 10 records in [vulnuscloud].[dbo].[oss_index].[http_status] that are in (102-Processing; 429-TooManyRequests) and tries to update them with data.
        /// </summary>
        /// <param name="dateTimeOfMethodCall">
        /// Used to validate that the records read from the database are not recent.
        /// </param>
        /// <returns></returns>
        Task ProcessOssRecords(DateTime dateTimeOfMethodCall);
    }
}
