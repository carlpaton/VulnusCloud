namespace Business.Model
{
    public class CoordinatePartsModel
    {
        /// <summary>
        /// The component "type" or "format" such as maven, npm, nuget, gem, pypi, etc.
        /// Required
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Some name prefix such as a Maven group-id, a NPM package scope, or a Docker image owner.
        /// Optional and type-specific
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// The name of the component.
        /// Required
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version of the component.
        /// Required
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Extra qualifying data for a component such as an OS, architecture, a distro, etc.
        /// Optional, type-specific, ignored
        /// </summary>
        //public string Qualifiers { get; set; }

        /// <summary>
        /// Extra sub-path within a component, relative to the package root.
        /// Optional, ignored
        /// </summary>
        //public string Subpath { get; set; }
    }
}
