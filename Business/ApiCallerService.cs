using Business.Interface;
using Data.Interface;
using System;
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

        public async Task ProcessOssRecords(DateTime dateTimeOfMethodCall) 
        {
            // TODO - its probably better to have a list deligates to await
            await GetListByHttpStatusAndProcess((int)HttpStatusCode.Processing, dateTimeOfMethodCall);
            await GetListByHttpStatusAndProcess((int)HttpStatusCode.TooManyRequests, dateTimeOfMethodCall);
        }

        private async Task GetListByHttpStatusAndProcess(int httpStatus, DateTime dateTimeOfMethodCall) 
        {
            var ossIndexList = await _ossIndexRepository.SelectByHttpStatusAsync(httpStatus);
            foreach (var ossIndex in ossIndexList)
            {
                if (dateTimeOfMethodCall > ossIndex.HttpStatusDate.AddMinutes(1))
                    _ossReportService.GetVulnerability(ossIndex.Id);
            }
        }
    }
}
