using MyImdb.DAL.Models;

namespace MyImdb.Api.Auth
{
    public interface ITokenService
    {
        public Task<GenerateTokenResponse> GenerateTokenAsync(AppUser user, List<string> roles);
    }
}
