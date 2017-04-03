using Nest;

namespace ElasticEngine
{
    public abstract class AbstractSearchQuery<T> : SearchDescriptor<T> where T : class
    {
        public abstract SearchDescriptor<T> Build();
    }
}