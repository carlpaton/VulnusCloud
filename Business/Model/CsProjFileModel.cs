using System.Collections.Generic;

namespace Business.Model
{
    /// <summary>
    /// [x].csproj
    /// 
    /// This is the project file for a .net core project.
    /// </summary>
    public class CsProjFileModel
    {
        public Project Project { get; set; }
    }

    public class PackageReference
    {
        public string Include { get; set; }
        public string Version { get; set; }
    }

    public class ItemGroup
    {
        public List<PackageReference> PackageReference { get; set; }
    }

    public class Project
    {
        public ItemGroup ItemGroup { get; set; }
    }
}
