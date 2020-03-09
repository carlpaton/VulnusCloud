using Business.CoordinatePart;
using Business.CoordinatePart.Interface;
using Business.Interface;
using Common.Serialization.Interface;
using System;

namespace Business
{
    public class CoordinatePartsFactory : ICoordinatePartsFactory
    {
        private readonly IJsonConvertService _jsonConvertService;

        public CoordinatePartsFactory(IJsonConvertService jsonConvertService) 
        {
            _jsonConvertService = jsonConvertService;
        }

        public ICoordinateParts GetCoordinatePart(string extension, string type)
        {
            if (CleanString(extension).Equals(".config") && CleanString(type).Equals("nuget"))
                return new ByConfig(_jsonConvertService);

            if (CleanString(extension).Equals(".csproj") && CleanString(type).Equals("nuget"))
                return new ByCsproj(_jsonConvertService);

            if (CleanString(extension).Equals(".json") && CleanString(type).Equals("npm"))
                return new ByPackageJson();

            throw new ApplicationException($"CoordinatePart for extension='{extension}', type='{type}' cannot be created.");
        }

        private string CleanString(string s) 
        {
            return s.ToLower().Trim();
        }
    }
}
