
using Microsoft.AspNetCore.Identity;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

namespace MyImdb.Api.Auth
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthManager(UserManager<AppUser> userManager, ITokenService tokenService = null, SignInManager<AppUser> signInManager = null)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();
            var user = await _userManager.FindByNameAsync(request.Username);


            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

                if (result.Succeeded)
                {
                    var roles = _userManager.GetRolesAsync(user).Result.ToList();
                    var generatedToken = await _tokenService.GenerateTokenAsync(user, roles);

                    response.AuthenticateResult = true;
                    response.AuthToken = generatedToken.Token;
                    response.AccessTokenExpireDate = generatedToken.TokenExpireDate;

                    return response;

                }
            }

            response.AuthenticateResult = false;
            response.AuthToken = "";
            response.AccessTokenExpireDate = DateTime.Now;

            return response;
        }
    }
}
