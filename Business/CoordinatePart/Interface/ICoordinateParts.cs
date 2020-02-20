using Business.Model;
using Common.Serialization.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.CoordinatePart.Interface
{
    public interface ICoordinateParts
    {
        List<CoordinatePartsModel> GetCoordinateParts(IJsonConvertService jsonConvertService, string type, IFormFile postedFile);
    }
}
