using System;

namespace MyLanguagePal.Shared.Exceptions
{
    public class MlpException : Exception
    {
        public MlpException()
        {

        }

        public MlpException(string message) :
            base(message)
        {

        }

        public MlpException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

        public MlpException(int code)
        {
            Code = code;
        }

        public MlpException(int code, string message) :
            base(message)
        {
            Code = code;
        }

        public MlpException(int code, string message, Exception innerException) :
            base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; private set; } = (int)MlpExceptionCodes.Unknown;
    }
}