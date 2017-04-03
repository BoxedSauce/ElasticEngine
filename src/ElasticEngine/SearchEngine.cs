using System;
using System.Collections.Generic;
using ElasticEngine.Queries;
using Nest;

namespace ElasticEngine
{
    public class SearchEngine
    {
        private const int Retry = 4;

        private readonly Uri _uri;
        private readonly IConnectionSettingsValues _connectionSettings;
        
        public SearchEngine(Uri uri)
        {
            _uri = uri;
        }

        public SearchEngine(IConnectionSettingsValues connectionSettings)
        {
            _connectionSettings = connectionSettings;
        }


        public IEnumerable<T> Search<T>(AbstractSearchQuery<T> query) where T : class
        {
            ElasticClient client = GetClient();
            SearchDescriptor<T> search = query.Build();

            int retryCount = 0;

            while (retryCount < Retry)
            {
                try
                {
                    ISearchResponse<T> searchResponse = client.Search<T>(search);
                    return searchResponse.Documents;
                }
                catch (Exception)
                {
                    retryCount++;
                }
            }


            throw new ElasticEngineException();
        }




        private ElasticClient GetClient()
        {
            ElasticClient client = new ElasticClient(new ConnectionSettings());

            return client;
        }
    }
}
