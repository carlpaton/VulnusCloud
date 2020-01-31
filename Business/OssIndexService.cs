using System;
using Business.Exceptions;
using Business.Interface;
using Business.Model;

namespace Business
{
    public class OssIndexService : IOssIndexService
    {
        public string GetCoordinates(CoordinatePartsModel coordinatePartsModel) 
        {
            Validate(coordinatePartsModel);

            return $"pkg:{coordinatePartsModel.Type}/{GetOptionNameSpace(coordinatePartsModel.Namespace)}{coordinatePartsModel.Name}@{coordinatePartsModel.Version}";
        }

        private object GetOptionNameSpace(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
                return "";

            return $"{nameSpace}/";
        }

        private void Validate(CoordinatePartsModel coordinatePartsModel)
        {
            if (string.IsNullOrEmpty(coordinatePartsModel.Type))
                ThrowCoordinateNotFoundException("Type");

            if (string.IsNullOrEmpty(coordinatePartsModel.Name))
                ThrowCoordinateNotFoundException("Name");

            if (string.IsNullOrEmpty(coordinatePartsModel.Version))
                ThrowCoordinateNotFoundException("Version");
        }

        private void ThrowCoordinateNotFoundException(string messagePart) 
        {
            throw new CoordinateNotFoundException($"CoordinatePartsModel {messagePart} is required.");
        }
    }
}
