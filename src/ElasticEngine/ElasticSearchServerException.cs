using System;
using System.Globalization;
using Elasticsearch.Net;

namespace ElasticEngine
{
    public class ElasticSearchServerException : Exception
    {
        public ElasticSearchServerException(Error error)
        {
            Error = error;
        }

        public override string Message
            => string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Error.CausedBy, Error.Reason);

        public Error Error { get; set; }
    }
}