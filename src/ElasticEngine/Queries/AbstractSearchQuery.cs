using Nest;

namespace ElasticEngine.Queries
{
    public abstract class AbstractSearchQuery<T> : SearchDescriptor<T> where T : class
    {
        public abstract SearchDescriptor<T> Build();
    }
}