using Microsoft.AspNetCore.Mvc.Testing;
using TransitNETesting.Utilities;

namespace TransitNETesting.Tests.UnitTests
{
    public class AuthTests : IClassFixture<TestDatabaseFixture>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public AuthTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }
        public TestDatabaseFixture Fixture { get; }



    }
}
