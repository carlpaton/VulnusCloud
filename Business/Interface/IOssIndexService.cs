using Business.Model;

namespace Business.Interface
{
    public interface IOssIndexService
    {
        /// <summary>
        /// A package-url (or purl) is a URI composed of six coordinate parts prefixed by pkg scheme
        /// Example:  pkg:type/namespace/name@version?qualifiers#subpath
        /// </summary>
        /// <param name="coordinatePartsModel"></param>
        /// <returns></returns>
        string GetCoordinates(CoordinatePartsModel coordinatePartsModel);
    }
}
