namespace VulnusCloud.Domain.Interface
{
    public interface IScoreClassService
    {
        string SetScoreFieldClass(decimal cvssScore);
    }
}
