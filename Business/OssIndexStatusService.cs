using Business.Interface;
using Data.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Business
{
    public class OssIndexStatusService : IOssIndexStatusService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportLinesRepository _reportLinesRepository;
        private readonly IOssIndexRepository _ossIndexRepository;

        private readonly List<int> KnownHttpStatusCodes = new List<int>() 
        {
            (int)HttpStatusCode.Processing, 
            (int)HttpStatusCode.OK,
            (int)HttpStatusCode.TooManyRequests
        };

        public OssIndexStatusService(IReportRepository reportRepository, IReportLinesRepository reportLinesRepository,
            IOssIndexRepository ossIndexRepository) 
        {
            _reportRepository = reportRepository;
            _reportLinesRepository = reportLinesRepository;
            _ossIndexRepository = ossIndexRepository;
        }

        public string GetCurrentStatusByProjectId(int id)
        {
            var sb = new StringBuilder();
            var httpStatusList = GetHttpStatusListByProjectId(id);
          
            if (httpStatusList.Contains((int)HttpStatusCode.Processing))
                sb.Append($"{HttpStatusCode.Processing}");

            if (httpStatusList.Contains((int)HttpStatusCode.OK)) 
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append($"{HttpStatusCode.OK}");
            }

            if (httpStatusList.Contains((int)HttpStatusCode.TooManyRequests))
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append($"{HttpStatusCode.TooManyRequests}");
            }

            return sb.ToString();
        }

        public string GetHelperStatusByProjectId(List<int> projectIds)
        {
            var httpStatusList = new List<int>();
            var sb = new StringBuilder();

            foreach (var projectId in projectIds)
            {
                httpStatusList.AddRange(GetHttpStatusListByProjectId(projectId));
            }

            if (httpStatusList.Contains((int)HttpStatusCode.Processing))
                sb.Append($"<i>Processing</i> - OSS API calls are still being made, these are automatic;");

            if (httpStatusList.Contains((int)HttpStatusCode.TooManyRequests))
            {
                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append($"<i>TooManyRequests</i> - Rate limiting on the OSS API was hit, these will automagically be retried;");
            }

            if (httpStatusList.Contains((int)HttpStatusCode.OK))
            {
                if (sb.Length > 0) 
                {
                    sb.Append($" <i>Partial OK</i> - Some but not all OSS API calls are complete;");
                }
                else 
                {
                    sb.Append($"<i>OK</i> - OSS API calls are complete;");
                }                
            }

            return sb.ToString();
        }

        private List<int> GetHttpStatusListByProjectId(int id) 
        {
            var httpStatusList = new List<int>();

            var lastReportId = _reportRepository
                .SelectByProjectId(id)
                .OrderByDescending(x => x.InsertDate)
                .First()
                .Id;


            foreach (var reportLine in _reportLinesRepository.SelectListByReportId(id))
            {
                var ossIndex = _ossIndexRepository
                    .Select(reportLine.OssIndexId);

                if (!httpStatusList.Contains(ossIndex.HttpStatus))
                    httpStatusList.Add(ossIndex.HttpStatus);
            }

            return httpStatusList;
        }
    }
}
