using Nest;

namespace ElasticEngine
{
    public abstract class AbstractSearchQuery<T> : SearchDescriptor<T> where T : class
    {
        protected QueryContainer SearchQueryContainer;

        public abstract SearchDescriptor<T> Build();


        protected void AndQuery(QueryContainer query)
        {
            SearchQueryContainer = SearchQueryContainer == null ? query : SearchQueryContainer && query;
        }

        protected void OrQuery(QueryContainer query)
        {
            SearchQueryContainer = SearchQueryContainer == null ? query : SearchQueryContainer || query;
        }
    }
}