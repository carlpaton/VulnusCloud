using Business.CoordinatePart;
using Business.CoordinatePart.Interface;
using Business.Interface;
using System;

namespace Business
{
    public class CoordinatePartsFactory : ICoordinatePartsFactory
    {
        public ICoordinateParts GetCoordinatePart(string extension)
        {
            switch (extension) 
            {
                case ".config":
                    return new ByConfig();
                case ".csproj":
                    return new ByCsproj();
                case ".json":
                    return new ByPackageJson();
                default:
                    throw new ApplicationException($"CoordinatePart for '{extension}' cannot be created.");
            }
        }
    }
}
