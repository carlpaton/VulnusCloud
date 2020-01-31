using System;

namespace Business.Exceptions
{
    public class CoordinateNotFoundException : Exception
    {
        public CoordinateNotFoundException()
        {
        }

        public CoordinateNotFoundException(string message)
            : base(message)
        {
        }

        public CoordinateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
