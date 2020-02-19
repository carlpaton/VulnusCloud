using System.Collections.Generic;
using Business.CoordinatePart.Interface;
using Business.Model;
using Common.Serialization.Interface;

namespace Business.CoordinatePart
{
    /// <summary>
    /// Reads components from `[project name].csproj`
    /// </summary>
    public class ByCsproj : ICoordinateParts
    {
        public List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type)
        {
            throw new System.NotImplementedException();
        }
    }
}
