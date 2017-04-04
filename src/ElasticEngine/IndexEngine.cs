using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Nest;

namespace ElasticEngine
{
    public class IndexEngine
    {
        private const int MaxRetry = 4;

        private readonly IElasticClient _client;

        public IndexEngine(IElasticClient client)
        {
            _client = client;
        }


        public ElasticIndexResponse Index<T>(T entity) where T : class
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < MaxRetry)
            {
                try
                {
                    IIndexResponse indexResponse = _client.Index(entity);
                    if (indexResponse != null && indexResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(indexResponse.ServerError.Error);
                    }
                    
                    ElasticIndexResponse response = new ElasticIndexResponse();

                    return response;
                }
                catch (WebException wex)
                {
                    lastException = wex;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                retryCount++;

                Thread.Sleep(500);
            }
            throw new ElasticSearchException("There was an error occured while indexing", lastException);
        }

        public ElasticBulkIndexResponse BulkIndex<T>(IEnumerable<T> entities) where T : class
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < MaxRetry)
            {
                try
                {
                    IBulkResponse indexResponse = _client.IndexMany(entities);
                    if (indexResponse != null && indexResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(indexResponse.ServerError.Error);
                    }
                    
                    ElasticBulkIndexResponse response = new ElasticBulkIndexResponse();
                    return response;
                }
                catch (WebException wex)
                {
                    lastException = wex;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                retryCount++;

                Thread.Sleep(500);
            }
            throw new ElasticSearchException("There was an error occured while indexing", lastException);
        }
    }
}
