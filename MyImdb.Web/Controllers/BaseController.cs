using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyImdb.DAL.Models;

namespace MyImdb.Web.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> _userManager { get; }
        protected SignInManager<AppUser> _signInManager { get; }
        protected AppUser currentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
    }
}
