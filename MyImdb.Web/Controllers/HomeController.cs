using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImdb.Business.Abstract;
using MyImdb.DAL.Context;
using MyImdb.DAL.Dtos;
using MyImdb.DAL.Models;
using MyImdb.Web.Models;
using System.Diagnostics;

namespace MyImdb.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly RoleManager<AppRole> _role;
        private readonly DataContext _dataContext;
        private IMovieReport _movieReport;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> role, DataContext dataContext = null, IMovieReport movieReport = null) : base(userManager, signInManager)
        {
            _role = role;
            _dataContext = dataContext;
            signInManager = _signInManager;
            userManager = _userManager;
            _movieReport = movieReport;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var result = await _dataContext.Movie
            .Include(x => x.Movie_Tag)
            .ThenInclude(x => x.Tag)
            .Where(x => x.isActive && !x.isDeleted)
            .ToListAsync();
            return View(result);
        }

        public IActionResult Dashboard ()
        {
            var report = _movieReport.MovieReports();
            return View(report);
        }


        [HttpPost]
        public async Task<IActionResult> Index(MovieFilter filter)
        {
            var result = await _dataContext.Movie
               .Include(x => x.Movie_Tag)
               .ThenInclude(x => x.Tag)
               .Where(x => x.isActive && !x.isDeleted && x.Name.ToLower().Contains(filter.Name == null ? "" : filter.Name.ToLower()))
               .ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Movie
            .Include(x => x.Movie_Tag)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Movie_Comment)
            .FirstOrDefaultAsync(x => x.isActive && !x.isDeleted && x.Id == id);

            if (result == null)
            {
                return RedirectToAction("Index");
            }


            double totalPoint = 0;
            foreach (var item in result.Movie_Comment)
            {
                totalPoint += item.Point;
            }

            if (result.Movie_Comment?.Count() > 0)
            {
                result.TotalPoint = totalPoint / result.Movie_Comment.Count();
            }
            else
            {
                result.TotalPoint = 0;
            }


            return View(result);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Setup()
        {
            AppRole role = new AppRole()
            {
                Name = "ADMIN"
            };
            await _role.CreateAsync(role);

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> SaveComment(Movie_Comment model)
        {

            ModelState.Remove("CreatedBy");
            ModelState.Remove("CreatedOn");
            ModelState.Remove("AppUserId");

            if (ModelState.IsValid)
            {

                model.CreatedBy = currentUser.UserName;
                model.CreatedOn = DateTime.Now;
                model.AppUserId = currentUser.Id;
                model.Point = model.Point;
                model.isActive = true;
                model.isDeleted = false;

                await _dataContext.Movie_Comment.AddAsync(model);
                if (await _dataContext.SaveChangesAsync() > 0)
                {
                    var result = await _dataContext.Movie_Comment.Where(x => x.MovieId == model.MovieId && x.isActive && !x.isDeleted).ToListAsync();
                    return PartialView("_Comments", result);
                }
            }

            return PartialView("_Commets",new Movie_Comment());
        }
    }
}
