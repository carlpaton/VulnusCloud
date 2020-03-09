using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.CoordinatePart.Interface
{
    public interface ICoordinateParts
    {
        /// <summary>
        /// Maps the given components file (nuget, npm ect) to a list of CoordinatePartsModel.
        /// </summary>
        /// <param name="jsonConvertService">
        /// Service to de-serialze the posted file from XML or JSON.
        /// </param>
        /// <param name="type">
        /// The component "type" or "format" such as maven, npm, nuget, gem, pypi, etc.
        /// </param>
        /// <param name="postedFile">
        /// Represents a file sent with the HttpRequest.
        /// </param>
        /// <returns></returns>
        List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type, IFormFile postedFile);
    }
}
