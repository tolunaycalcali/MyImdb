using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyImdb.DAL.Dtos;
using MyImdb.DAL.Models;

namespace MyImdb.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public IActionResult Login()
        {
            TempData["ReturnUrl"] = HttpContext.Request.Query["ReturnUrl"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);

                if (user is not null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);

                    if (result.Succeeded)
                    {
                        //var returnUrl = TempData["ReturnUrl"].ToString();
                        //if (Url.IsLocalUrl(returnUrl))
                        //    return Redirect(returnUrl);
                        //else
                            return Redirect("/");
                    }
                    else
                    {
                        return View(dto);
                    }
                }
            }

            return View(dto);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AppUserDto dto)
        {

            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = dto.Username,
                    Email = dto.Email,
                    Ad = dto.Ad,
                    Soyad = dto.Soyad,
                };

                var result = await _userManager.CreateAsync(appUser, dto.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return View(dto);
                }
            }

            return View(dto);
        }

        [HttpGet]
        public void Logout()
        {
            _signInManager.SignOutAsync();
        }
    }
}
