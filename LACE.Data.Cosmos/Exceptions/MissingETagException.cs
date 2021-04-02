using LACE.Core.Exceptions;

namespace LACE.Data.Cosmos.Exceptions
{
    public class MissingETagException : InternalServerErrorException
    {
        public MissingETagException(string message)
            : base(message)
        {
        }
    }
}
