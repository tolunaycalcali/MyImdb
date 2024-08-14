using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImdb.Api.Auth;

namespace MyImdb.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserLoginResponse>> Login(UserLoginRequest request)
        {
            try
            {
                var result = await _authService.LoginUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }
    }
}
