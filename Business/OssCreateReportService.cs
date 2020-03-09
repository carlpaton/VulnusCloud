using Business.Interface;
using Business.Model;
using Common.Http.Interface;
using Common.Serialization.Interface;
using Data.Interface;
using Data.Schema;
using System;
using System.IO;
using System.Net;

namespace Business
{
    public class OssReportService : IOssReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IOssIndexRepository _ossIndexRepository;
        private readonly IReportLinesRepository _reportLinesRepository;
        private readonly ICoordinatesService _coordinatesService;
        private readonly IHttpWebRequestFactory _httpWebRequestFactory;
        private readonly IJsonConvertService _jsonConvertService;
        private readonly IOssIndexVulnerabilitiesRepository _ossIndexVulnerabilitiesRepository;
        private readonly string _contentType = "application/json; charset=utf-8";

        public OssReportService(IReportRepository reportRepository, IComponentRepository componentRepository,
            IOssIndexRepository ossIndexRepository, IReportLinesRepository reportLinesRepository,
            ICoordinatesService coordinatesService, IHttpWebRequestFactory httpWebRequestFactory,
            IJsonConvertService jsonConvertService, IOssIndexVulnerabilitiesRepository ossIndexVulnerabilitiesRepository)
        {
            _reportRepository = reportRepository;
            _componentRepository = componentRepository;
            _ossIndexRepository = ossIndexRepository;
            _reportLinesRepository = reportLinesRepository;
            _coordinatesService = coordinatesService;
            _httpWebRequestFactory = httpWebRequestFactory;
            _jsonConvertService = jsonConvertService;
            _ossIndexVulnerabilitiesRepository = ossIndexVulnerabilitiesRepository;
        }

        public int CreateInitialReport(DateTime dateTime, int projectId) 
        {
            return _reportRepository.Insert(new ReportModel()
            {
                InsertDate = dateTime,
                ProjectId = projectId
            });
        }

        public void CreateInitialReportShell(int reportId, CoordinatePartsModel coordinatePart)
        {
            // check dbo.component.name on coordinatePart.Name
            // if it exists, return the id
            // else create, return the id                  
            var component = _componentRepository.SelectByName(coordinatePart.Name.Trim());
            var componentId = component.Id;
            if (componentId == 0)
            {
                componentId = _componentRepository.Insert(new ComponentModel()
                {
                    Name = coordinatePart.Name.Trim()
                });
            }

            // check dbo.oss_index.component_id
            // if it exists, check `expire_date` 
            //    - if older than 30 days, call remote api
            //    - insert results if any to dbo.oss_index_vulnerabilities
            //    - update `expire_date` = NOW
            // else
            //    - create with `expire_date` = NOW
            //    - call remote api
            //    - insert results if any to dbo.oss_index_vulnerabilities
            var ossIndex = _ossIndexRepository.SelectByComponentId(componentId);
            var ossIndexId = ossIndex.Id;
            if (ossIndexId == 0)
            {
                ossIndex = new OssIndexModel()
                {
                    ComponentId = componentId,
                    ExpireDate = DateTime.Now.AddMonths(1),
                    HttpStatus = (int)HttpStatusCode.Processing,
                    HttpStatusDate = DateTime.Now
                };

                ossIndexId = _ossIndexRepository.Insert(ossIndex);
                ossIndex = _ossIndexRepository.Select(ossIndexId);
            }

            /* TODO
             * 
             * 1. this is always zero as we cannot pass things like `1.4.0` as a decimal, consider deprecating `[vulnuscloud].[dbo].[oss_index].[version]` as this data is already in `[vulnuscloud].[dbo].[oss_index].[coordinates]`
             * 2. [vulnuscloud].[dbo].[oss_index].[coordinates] should be normalized:
             *      `pkg:Nuget/BeITMemcached@1.4.0`
             *      > pkg: is known, comes from `_coordinatesService`
             *      > Nuget/ should rather be stored as `[vulnuscloud].[dbo].[oss_index].[package_type_id]` - then this links to PackageTypeRepository
             *      > BeITMemcached@ can be read from [vulnuscloud].[dbo].[component].[id] = [vulnuscloud].[dbo].[oss_index].[component_id]
             *      > 1.4.0 could then be stored as [vulnuscloud].[dbo].[oss_index].[version]
             *      
             *      [vulnuscloud].[dbo].[oss_index].[coordinates] could then be generated when needed.
             */

            if (decimal.TryParse(coordinatePart.Version, out decimal coordinatePartVersion))
                ossIndex.Version = coordinatePartVersion;

            ossIndex.Coordinates = _coordinatesService.GetCoordinates(coordinatePart);
            _ossIndexRepository.Update(ossIndex);

            _reportLinesRepository.Insert(new ReportLinesModel()
            {
                OssIndexId = ossIndexId,
                ReportId = reportId
            });
        }

        public void GetVulnerability(int ossIndexId)
        {
            var ossIndex = _ossIndexRepository.Select(ossIndexId);
            var coordinates = ossIndex.Coordinates;
            var endPoint = $"https://ossindex.sonatype.org/api/v3/component-report/{coordinates}"; // TODO ~ read from config

            var request = _httpWebRequestFactory.Create(endPoint);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = _contentType;

            try
            {
                using (var response = request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var responseString = streamReader.ReadToEnd();
                        var componentReport = _jsonConvertService.ToObject<ComponentReportModel>(responseString);

                        // TODO - consideration:
                        //    - perhaps update `dbo.oss_index` if the data has changed
                        
                        ossIndex.Description = componentReport.description;
                        ossIndex.Reference = componentReport.reference;
                        ossIndex.ExpireDate = DateTime.Now.AddMonths(1);
                        ossIndex.HttpStatus = (int)HttpStatusCode.OK;

                        _ossIndexRepository.Update(ossIndex);

                        foreach (var vulnerability in componentReport.vulnerabilities)
                        {
                            // TODO
                            // delete `[vulnuscloud].[dbo].[oss_index_vulnerabilities].[oss_index_id]`

                            var ossIndexVulnerabilitiesModel = new OssIndexVulnerabilitiesModel()
                            {
                                Cve = vulnerability.cve,
                                CvssScore = vulnerability.cvssScore,
                                CvssVector = vulnerability.cvssVector,
                                Description = vulnerability.description,
                                InsertDate = DateTime.Now,
                                OssId = vulnerability.id,
                                OssIndexId = ossIndex.Id,
                                Reference = vulnerability.reference,
                                Title = vulnerability.title
                            };

                            _ossIndexVulnerabilitiesRepository.Insert(ossIndexVulnerabilitiesModel);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        ossIndex.HttpStatus = (int)HttpStatusCode.TooManyRequests;
                        ossIndex.HttpStatusDate = DateTime.Now;
                        _ossIndexRepository.Update(ossIndex);

                        // TODO
                        // Consider
                        // 1. Throtteling at this point (this method is not async so the user wont know)
                        // 2. Halt all API calls, defer them for later

                        // 2 above would need an arcutectual change above
                        //      - a. the user's file content would be uploaded to `oss_index_queue`
                        //      - b. async process run to query for the data
                        //      - c. include this in the reports page so the user is aware the system is still `fetching data`
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO
                // 1. Consider that http_status would now be 0 for this record.
                // 2. logging
            }
        }
    }
}
