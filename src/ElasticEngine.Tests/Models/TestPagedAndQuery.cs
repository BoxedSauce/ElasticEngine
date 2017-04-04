using Nest;

namespace ElasticEngine.Tests.Models
{
    public class TestPagedAndQuery : AbstractPagedSearchQuery<TestEntity>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override SearchDescriptor<TestEntity> Build()
        {
            return AndId()
                .AndName()
                .AddPaging()
                .Query(q => SearchQueryContainer);
        }


        public TestPagedAndQuery AndId()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                AndQuery(new QueryContainerDescriptor<TestEntity>().Term(f => f.Id, Id));
            }

            return this;
        }

        public TestPagedAndQuery AndName()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                AndQuery(new QueryContainerDescriptor<TestEntity>().Term(f => f.Name, Name));
            }

            return this;
        }
    }
}
