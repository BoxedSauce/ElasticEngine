using System;

namespace ElasticEngine
{
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException()
        {
            
        }

        public ElasticSearchException(string message) 
            : base(message)
        {
            
        }

        public ElasticSearchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
