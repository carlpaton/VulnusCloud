using System;

namespace Data.Schema
{
    public class OssIndexVulnerabilitiesModel
    {
        public int Id { get; set; }
        public int OssIndexId { get; set; }
        public DateTime InsertDate { get; set; }
        public string OssId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Decimal CvssScore { get; set; }
        public string CvssVector { get; set; }
        public string Cve { get; set; }
        public string Reference { get; set; }
    }
}
