using Business.Interface;
using Data.Interface;
using System.Net;
using System.Threading.Tasks;

namespace Business
{
    public class ApiCallerService : IApiCallerService
    {
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IOssReportService _ossReportService;

        public ApiCallerService(IOssIndexRepository ossIndexRepository, IOssReportService ossReportService) 
        {
            _ossIndexRepository = ossIndexRepository;
            _ossReportService = ossReportService;
        }

        public async Task ProcessOssRecords() 
        {
            await GetListByHttpStatusAndProcess((int)HttpStatusCode.Processing);
            await GetListByHttpStatusAndProcess((int)HttpStatusCode.TooManyRequests);
        }

        private async Task GetListByHttpStatusAndProcess(int httpStatus) 
        {
            var ossIndexList = await _ossIndexRepository.SelectByHttpStatusAsync(httpStatus);
            foreach (var ossIndex in ossIndexList)
            {
                _ossReportService.GetVulnerability(ossIndex.Id);
            }
        }
    }
}
