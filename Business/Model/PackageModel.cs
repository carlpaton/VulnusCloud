namespace Business.Model
{
    public class PackageModel
    {
        /// <summary>
        /// `packages.config` this is `id`
        /// `[x].csproj` this is `Include`
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// `packages.config` this is `version`
        /// `[x].csproj` this is `Version`
        /// </summary>
        public string Version { get; set; }
    }
}
