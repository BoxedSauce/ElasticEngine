namespace ElasticEngine
{
    public abstract class AbstractPagedSearchQuery<T> : AbstractSearchQuery<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        protected AbstractPagedSearchQuery<T> AddPaging()
        {
            Size(PageSize).From((Page-1)*PageSize);
            return this;
        }
    }
}