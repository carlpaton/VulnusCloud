using System.Collections.Generic;
using Business.CoordinatePart.Interface;
using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Http;

namespace Business.CoordinatePart
{
    // TODO ~ bug fix
    // if only one `PackageReference` so `<ItemGroup><PackageReference Include="Newtonsoft.Json" Version="12.0.3" />...` exists in the .csproj file `jsonConvertService` will throw as `CsProjFileModel...PackageReference` is a list

    /// <summary>
    /// Reads components from `[project name].csproj`
    /// </summary>
    public class ByCsproj : ICoordinateParts
    {
        public List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type, IFormFile postedFile)
        {
            var coordinateParts = new List<CoordinatePartsModel>();

            var csProjFileModel = jsonConvertService.XmlFileToObject<CsProjFileModel>(postedFile);
            foreach (var packageReference in csProjFileModel.Project.ItemGroup.PackageReference)
            {
                coordinateParts.Add(new CoordinatePartsModel()
                {
                    Name = packageReference.Include,
                    Version = packageReference.Version,
                    Type = type
                });
            }

            return coordinateParts;
        }
    }
}
