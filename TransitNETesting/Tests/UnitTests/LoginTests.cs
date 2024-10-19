using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TransitNE.Data;
using TransitNETesting.Utilities;

namespace TransitNETesting.Tests.UnitTests
{
    public class LoginTests : IClassFixture<TestDatabaseFixture>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public LoginTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }
        public TestDatabaseFixture Fixture { get; }
        
        [Fact]
        public async Task GetAsyncTest()
        {

        }

    }
}
