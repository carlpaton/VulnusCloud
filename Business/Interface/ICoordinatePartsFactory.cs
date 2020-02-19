using Business.CoordinatePart.Interface;

namespace Business.Interface
{
    public interface ICoordinatePartsFactory
    {
        ICoordinateParts GetCoordinatePart(string extension);
    }
}
