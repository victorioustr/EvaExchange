using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;

namespace Business.Services.Authentication
{
    public class QTToken : AccessToken
    {
        public string ExternalUserId { get; set; }
        public AuthenticationProviderType Provider { get; set; }
        public string OnBehalfOf { get; set; }
    }
}