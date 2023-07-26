using Millien.Domain.Entities;

namespace Milien_backend.Models.Responses
{
    public class AuthenticateResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Customer UserDTO { get; set; }
    }
}
