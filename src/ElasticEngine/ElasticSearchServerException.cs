using System;
using System.Globalization;

namespace ElasticEngine
{
    public class ElasticSearchServerException : Exception
    {
        public ElasticSearchServerException(string error, string exceptionType, int status)
        {
            Error = error;
            ExceptionType = exceptionType;
            Status = status;
        }

        public override string Message => string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Error, ExceptionType);

        public string Error { get; set; }
        public string ExceptionType { get; set; }
        public int Status { get; set; }
    }
}