using Data.Interface;
using System.Linq;
using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class ScoreService : IScoreService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportLinesRepository _reportLinesRepository;
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IOssIndexVulnerabilitiesRepository _ossIndexVulnerabilitiesRepository;

        public ScoreService(IReportRepository reportRepository, IReportLinesRepository reportLinesRepository,
            IOssIndexRepository ossIndexRepository, IOssIndexVulnerabilitiesRepository ossIndexVulnerabilitiesRepository) 
        {
            _reportRepository = reportRepository;
            _reportLinesRepository = reportLinesRepository;
            _ossIndexRepository = ossIndexRepository;
            _ossIndexVulnerabilitiesRepository = ossIndexVulnerabilitiesRepository;
        }

        public decimal GetCurrentScoreByProjectId(int id)
        {
            var lastReportId = _reportRepository
                .SelectByProjectId(id)
                .OrderByDescending(x => x.InsertDate)
                .First()
                .Id;

            return GetScoreByReportId(lastReportId);
        }

        public decimal GetScoreByReportId(int id)
        {
            var totalCvssScore = 0m;

            foreach (var reportLine in _reportLinesRepository.SelectListByReportId(id))
            {
                var ossIndexId = _ossIndexRepository
                    .Select(reportLine.OssIndexId)
                    .Id;

                totalCvssScore += GetScoreByOssIndexId(ossIndexId);
            }

            return totalCvssScore;
        }

        public decimal GetScoreByOssIndexId(int id)
        {
            return _ossIndexVulnerabilitiesRepository
                    .SelectlistByOssIndexId(id)
                    .Select(x => x.CvssScore)
                    .Sum();
        }
    }
}
