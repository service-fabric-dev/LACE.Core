//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata.Ecma335;
//using System.Text;
//using System.Threading.Tasks;

//namespace LACE.Core.Exceptions
//{
//    public class ValidatedException : Exception
//    {
//        public ValidatedException()
//            : this(true)
//        {
//        }

//        public ValidatedException(bool isValid)
//        {
//            IsValid = isValid;
//        }

//        public bool IsValid { get; }

//        public static implicit operator bool(ValidatedException exception) => false;

//        public static readonly ValidatedException Valid = new ValidatedException(true);
//    }
//}
