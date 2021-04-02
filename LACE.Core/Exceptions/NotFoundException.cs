using System;
using System.Net;

namespace LACE.Core.Exceptions
{
    public class NotFoundException : HttpException
    {
        public NotFoundException()
            : this(Enum.GetName(HttpStatusCode.InternalServerError))
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(HttpStatusCode.BadRequest, message, innerException)
        {

        }
    }
}
