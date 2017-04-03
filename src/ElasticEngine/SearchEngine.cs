using System;
using System.Linq;
using System.Net;
using Elasticsearch.Net.Connection;
using Nest;

namespace ElasticEngine
{
    public class SearchEngine : ISearchEngine
    {
        private const int Retry = 4;

        private readonly IElasticClient _client;
        
        public SearchEngine(Uri uri)
        {
            _client = new ElasticClient(new ConnectionSettings(uri));
        }

        public SearchEngine(IConnectionSettingsValues connectionSettings, IConnection connection = null, INestSerializer serializer = null)
        {
            _client = new ElasticClient(connectionSettings, connection, serializer);
        }

        public SearchEngine(IElasticClient client)
        {
            _client = client;
        }

        public ElasticSearchResponse<T> Search<T>(AbstractSearchQuery<T> query) where T : class
        {
            SearchDescriptor<T> search = query.Build();

            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < Retry)
            {
                try
                {
                    ISearchResponse<T> searchResponse = _client.Search<T>(search);

                    if (!searchResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(
                            searchResponse.ServerError.Error,
                            searchResponse.ServerError.ExceptionType,
                            searchResponse.ServerError.Status);
                    }

                    ElasticSearchResponse<T> response = new ElasticSearchResponse<T>
                    {
                        Count = searchResponse.Documents.Count(),
                        TotalCount = searchResponse.Total,
                        MaxScore = searchResponse.MaxScore,
                        Results = searchResponse.Documents
                    };
                    return response;
                }
                catch (WebException wex)
                {
                    lastException = wex;
                    retryCount++;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    retryCount++;
                }
            }
            
            throw new ElasticSearchException("There was an error occured while performing a search", lastException);
        }

        public T Get<T>(string id) where T : class
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < Retry)
            {
                try
                {
                    IGetResponse<T> searchResponse = _client.Get<T>(id);
                    if (!searchResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(
                            searchResponse.ServerError.Error,
                            searchResponse.ServerError.ExceptionType,
                            searchResponse.ServerError.Status);
                    }

                    return searchResponse.Source;
                }
                catch (WebException wex)
                {
                    lastException = wex;
                    retryCount++;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    retryCount++;
                }
            }

            throw new ElasticSearchException("There was an error occured while performing a search", lastException);
        }
    }
}