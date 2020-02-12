using VulnusCloud.Domain.Interface;

namespace VulnusCloud.Domain
{
    public class ScoreClassService : IScoreClassService
    {
        public string SetScoreFieldClass(decimal cvssScore)
        {
            if (cvssScore >= 7)
                return "table-danger";

            if (cvssScore >= 5)
                return "table-warning";

            if (cvssScore >= 2)
                return "table-info";

            return "table-light";
        }
    }
}
