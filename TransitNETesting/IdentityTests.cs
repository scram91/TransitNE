using Microsoft.AspNetCore.Mvc.Testing;

namespace TransitNETesting
{
    public class IdentityTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public IdentityTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }
    }
}
