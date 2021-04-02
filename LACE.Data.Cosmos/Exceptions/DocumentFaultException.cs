using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LACE.Core.Exceptions;

namespace LACE.Data.Cosmos.Exceptions
{
    public class DocumentFaultException : InternalServerErrorException
    {
        public DocumentFaultException(string message)
            : base(message)
        {
        }
    }
}
