using System;
using System.Net;

namespace LACE.Core.Exceptions
{
    public class BadRequestException : HttpException
    {
        public BadRequestException()
            : this(Enum.GetName(HttpStatusCode.InternalServerError))
        {
        }

        public BadRequestException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(HttpStatusCode.BadRequest, message, innerException)
        {
        }
    }
}
