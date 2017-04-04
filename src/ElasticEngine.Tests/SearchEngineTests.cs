using System;
using System.Net;
using ElasticEngine.Tests.Models;
using Elasticsearch.Net;
using Moq;
using Nest;
using NUnit.Framework;

namespace ElasticEngine.Tests
{
    [TestFixture]
    public class SearchEngineTests
    {
        private Mock<IElasticClient> _client;
        private Mock<IConnectionSettingsValues> _connectionSettings;

        [SetUp]
        public void SetUp()
        {
            _connectionSettings = new Mock<IConnectionSettingsValues>();
            _connectionSettings.SetupGet(x => x.DefaultIndex).Returns("testindex");

            _client = new Mock<IElasticClient>();
            _client.SetupGet(x => x.ConnectionSettings)
                .Returns(_connectionSettings.Object);
        }


        [Test]
        public void Get_Returns_Entity()
        {
            var id = "123";

            var searchResponse = new Mock<IGetResponse<TestEntity>>();
            searchResponse.SetupGet(x => x.Source).Returns(new TestEntity {Id = id, Name = "Test Entity"});
            searchResponse.SetupGet(x => x.IsValid).Returns(true);
            
            _client.Setup(x => x.Get<TestEntity>(It.IsAny<IGetRequest>()))
                .Returns(searchResponse.Object);

            SearchEngine se = new SearchEngine(_client.Object);
            var result = se.Get<TestEntity>(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public void Get_Throws_WebException()
        {
            var id = "123";

            _client.Setup(x => x.Get<TestEntity>(It.IsAny<IGetRequest>()))
                .Throws<WebException>();

            SearchEngine se = new SearchEngine(_client.Object);

            var result = Assert.Throws<ElasticSearchException>(() => se.Get<TestEntity>(id));
            
            Assert.IsNotNull(result);
            Assert.AreEqual("There was an error occured while performing a search", result.Message);
            Assert.IsNotNull(result.InnerException);
            Assert.IsInstanceOf<WebException>(result.InnerException);
        }

        [Test]
        public void Search_Returns_Entities()
        {
            var query = new TestPagedAndQuery
            {
                Id = "123",
                Name = "Test",
                Page = 1,
                PageSize = 10
            };
            
            var searchResponse = new SearchResponse<TestEntity>();

            _client.Setup(x => x.Search<TestEntity>(It.IsAny<TestPagedAndQuery>()))
                .Returns(searchResponse);

            SearchEngine se = new SearchEngine(_client.Object);
            var result = se.Search(query);

            Assert.IsNotNull(result);
        }
    }
}