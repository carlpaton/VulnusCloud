using Business.CoordinatePart;
using Business.CoordinatePart.Interface;
using Business.Interface;
using System;

namespace Business
{
    public class CoordinatePartsFactory : ICoordinatePartsFactory
    {
        public ICoordinateParts GetCoordinatePart(string extension, string type)
        {
            if (CleanString(extension).Equals(".config") && CleanString(type).Equals("nuget"))
                return new ByConfig();

            if (CleanString(extension).Equals(".csproj") && CleanString(type).Equals("nuget"))
                return new ByCsproj();

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
