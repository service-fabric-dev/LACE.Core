using System;
using System.Net;

namespace LACE.Core.Exceptions
{
    public class InternalServerErrorException : HttpException
    {
        public InternalServerErrorException()
            : this(Enum.GetName(HttpStatusCode.InternalServerError))
        {
        }

        public InternalServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException)
            : base(HttpStatusCode.BadRequest, message, innerException)
        {

        }
    }
}
