using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using WebAPI;

namespace Tests.Helpers.Token
{
    [TestFixture]
    public abstract class BaseIntegrationTest : WebApplicationFactory<Startup>
    {
        private static readonly JwtSecurityTokenHandler STokenHandler = new();

        public string Issuer { get; } = "eva.guru";
        public string Audience { get; } = "eva.guru";
        public SigningCredentials SigningCredentials { get; }

        protected HttpClient HttpClient { get; set; }

        protected WebApplicationFactory<Startup> Factory => new();

        [SetUp]
        public void Setup()
        {
            HttpClient = CreateClient();
        }
    }
}