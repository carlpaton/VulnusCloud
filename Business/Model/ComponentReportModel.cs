using System.Collections.Generic;

namespace Business.Model
{
    /// <summary>
    /// Response model from `ossindex.sonatype.org/api/v3/component-report/`
    /// </summary>
    public class ComponentReportModel
    {
        public string coordinates { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public List<Vulnerability> vulnerabilities { get; set; }
    }

    public class Vulnerability
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public double cvssScore { get; set; }
        public string cvssVector { get; set; }
        public string cve { get; set; }
        public string reference { get; set; }
    }
}
