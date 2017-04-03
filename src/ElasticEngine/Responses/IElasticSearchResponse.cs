using System.Collections.Generic;

namespace ElasticEngine
{
    public interface IElasticSearchResponse<T> where T : class
    {
        long Count { get; set; }
        double MaxScore { get; set; }
        long Page { get; set; }
        IEnumerable<T> Results { get; set; }
        long TotalCount { get; set; }
    }
}