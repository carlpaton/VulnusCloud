using System.Collections.Generic;
using Business.CoordinatePart.Interface;
using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Http;

namespace Business.CoordinatePart
{
    // TODO ~ check bug in `\Business\CoordinatePart\ByCsproj.cs`, most likely exists here. so test with a `packages.config` with 1 component reference.

    /// <summary>
    /// Reads components from `packages.config`
    /// </summary>
    public class ByConfig : ICoordinateParts
    {
        public List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type, IFormFile postedFile)
        {
            var coordinateParts = new List<CoordinatePartsModel>();

            var packagesConfigFileModel = jsonConvertService.XmlFileToObject<PackagesConfigFileModel>(postedFile);
            foreach (var package in packagesConfigFileModel.packages.package)
            {
                coordinateParts.Add(new CoordinatePartsModel()
                {
                    Name = package.id,
                    Version = package.version,
                    Type = type
                });
            }

            return coordinateParts;
        }
    }
}
