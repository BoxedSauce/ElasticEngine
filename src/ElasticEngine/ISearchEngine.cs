namespace ElasticEngine
{
    public interface ISearchEngine
    {
        ElasticSearchResponse<T> Search<T>(AbstractSearchQuery<T> query) where T : class;
    }
}