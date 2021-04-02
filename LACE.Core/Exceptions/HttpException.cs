using System;
using System.Net;

namespace LACE.Core.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode code)
            : this(code, Enum.GetName(code))
        {
            Code = code;
        }

        public HttpException(HttpStatusCode code, string message)
            : base(message)
        {
            Code = code;
        }

        public HttpException(HttpStatusCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public HttpStatusCode Code { get; }
    }
}
