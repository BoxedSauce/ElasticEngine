using System;
using System.Linq;
using System.Net;
using System.Threading;
using Nest;

namespace ElasticEngine
{
    public class SearchEngine : ISearchEngine
    {
        private const int MaxRetry = 4;

        private readonly IElasticClient _client;
        
        public SearchEngine(Uri uri)
        {
            _client = new ElasticClient(uri);
        }

        public SearchEngine(IConnectionSettingsValues connectionSettings)
        {
            _client = new ElasticClient(connectionSettings);
        }

        public SearchEngine(IElasticClient client)
        {
            _client = client;
        }

        public ElasticSearchResponse<T> Search<T>(AbstractSearchQuery<T> query) where T : class
        {
            SearchDescriptor<T> searchQuery = query.Build();

            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < MaxRetry)
            {
                try
                {
                    ISearchResponse<T> searchResponse = _client.Search<T>(searchQuery);
                    if (!searchResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(searchResponse.ServerError.Error);
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
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                retryCount++;

                Thread.Sleep(500);
            }
            
            throw new ElasticSearchException("There was an error occured while performing a search", lastException);
        }

        /// <summary>
        /// Get a single document by it's Id. Uses the clients default index
        /// </summary>
        /// <typeparam name="T">Type of Document to return</typeparam>
        /// <param name="id">Id of document</param>
        /// <returns><see cref="T"/>T</returns>
        public T Get<T>(string id) where T : class
        {
            int retryCount = 0;
            Exception lastException = null;
            
            while (retryCount < MaxRetry)
            {
                try
                {
                    IGetRequest getRequest = new GetRequest<T>(_client.ConnectionSettings.DefaultIndex, typeof(T).Name, new Id(id));
                    IGetResponse<T> searchResponse = _client.Get<T>(getRequest);
                    if (!searchResponse.IsValid)
                    {
                        throw new ElasticSearchServerException(searchResponse.ServerError.Error);
                    }

                    return searchResponse.Source;
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
            
            throw new ElasticSearchException("There was an error occured while performing a search", lastException);
        }
    }
}