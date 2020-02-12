namespace VulnusCloud.Domain.Interface
{
    public interface IScoreService
    {
        /// <summary>
        /// For the given project id' most recent file upload, with OSS index vulnerabilities, return the score sum.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        decimal GetCurrentScoreByProjectId(int id);

        /// <summary>
        /// For the given report id, sum and return OSS index vulnerabilities score.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        decimal GetScoreByReportId(int id);

        /// <summary>
        /// Sum score for given OSS index vulnerabilities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        decimal GetScoreByOssIndexId(int id);
    }
}
