using System.Collections.Generic;

namespace Business.Model
{
    /// <summary>
    /// packages.config
    /// 
    /// .Net project packages file (3rd party nuget)
    /// </summary>
    public class PackagesConfigFileModel
    {
        public Packages packages { get; set; }
    }

    public class Packages
    {
        public List<Package> package { get; set; }
    }

    public class Package
    {
        public string id { get; set; }
        public string version { get; set; }
    }
}
