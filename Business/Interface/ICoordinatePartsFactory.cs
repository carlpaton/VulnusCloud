using Business.CoordinatePart.Interface;

namespace Business.Interface
{
    public interface ICoordinatePartsFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension">
        /// NuGET |	packages.config (Legacy) | packages.config
        /// -------------------------------------------------------------------------
        /// NuGET | Package Reference in project file (.Net) | [project name].csproj
        /// -------------------------------------------------------------------------
        /// NPM   | \packages\AppName\web\client | package.json
        /// </param>
        /// <param name="type">
        /// NuGET, NPM
        /// </param>
        /// <returns></returns>
        ICoordinateParts Create(string extension, string type);
    }
}
