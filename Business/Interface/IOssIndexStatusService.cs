using System.Collections.Generic;

namespace Business.Interface
{
    public interface IOssIndexStatusService
    {
        /// <summary>
        /// For the given project id's most recent file upload, check the http status of oss_index
        /// 102 - Processing
        /// 200 - OK
        /// 429 - TooManyRequests
        /// 
        /// There is rate limiting on the OSS API, if the application spams them they will return 429.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCurrentStatusByProjectId(int id);

        /// <summary>
        /// Helper text for `status` that appears alert box above reports. This translates the HTTP status code being appended into something human readable.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetHelperStatusByProjectId(List<int> projectIds);
    }
}
