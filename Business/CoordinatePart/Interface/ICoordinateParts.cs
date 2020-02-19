using Business.Model;
using Common.Serialization.Interface;
using System.Collections.Generic;

namespace Business.CoordinatePart.Interface
{
    public interface ICoordinateParts
    {
        List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type);
    }
}
