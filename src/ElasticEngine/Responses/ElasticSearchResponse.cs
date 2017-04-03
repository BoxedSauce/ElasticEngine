using System.Collections.Generic;

namespace ElasticEngine
{
    public class ElasticSearchResponse<T> : IElasticSearchResponse<T> where T : class
    {
        public double MaxScore { get; set; }
        public long Page { get; set; }
        public long Count { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
