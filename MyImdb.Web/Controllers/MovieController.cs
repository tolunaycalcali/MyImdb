using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

namespace MyImdb.Web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MovieController : BaseController
    {
        private readonly DataContext _dataContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public MovieController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext dataContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment) : base(userManager, signInManager)
        {
            _dataContext = dataContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _dataContext.Movie.Where(x => !x.isDeleted).ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            var tags = await _dataContext.Tag.Where(x => !x.isDeleted && x.isActive).ToListAsync();
            ViewBag.Tags = new SelectList(tags, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie model)
        {
            var tags = await _dataContext.Tag.Where(x => !x.isDeleted && x.isActive).ToListAsync();

            ModelState.Remove("CreatedBy");
            ModelState.Remove("ImagePath");
            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.formFile.FileName);
                        var path = Path.Combine(_environment.WebRootPath, "MovieImage", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.formFile.CopyToAsync(stream);
                        }

                        model.CreatedBy = currentUser.UserName;
                        model.CreatedOn = DateTime.Now;
                        model.isDeleted = false;
                        model.ImagePath = fileName;


                        await _dataContext.Movie.AddAsync(model);
                        await _dataContext.SaveChangesAsync();

                        foreach (var item in model.TagIdList)
                        {
                            await _dataContext.Movie_Tag.AddAsync(new Movie_Tag()
                            {
                                CreatedBy = currentUser.UserName,
                                CreatedOn = DateTime.Now,
                                isActive = true,
                                isDeleted = false,
                                MovieId = model.Id,
                                TagId = Convert.ToInt32(item)
                            });
                        }

                        await _dataContext.SaveChangesAsync();
                        await transaction.Result.CommitAsync();

                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        ViewBag.Tags = new SelectList(tags, "Id", "Name");
                        return View(model);
                    }
                }
            }
            ViewBag.Tags = new SelectList(tags, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Movie.Include(x => x.Movie_Tag).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.isActive && !x.isDeleted && x.Id == id);

            if (result == null)
            {
                return RedirectToAction("Index");
            }

            var tags = await _dataContext.Tag.Where(x => x.isActive && !x.isDeleted).ToListAsync();

            ViewBag.Tags = new SelectList(tags, "Id", "Name");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie model)
        {
            ModelState.Remove("formFile");
            if (ModelState.IsValid)
            {
                using (var transaction = _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        model.ModifiedBy = currentUser.UserName;
                        model.ModifiedOn = DateTime.Now;
                        model.isDeleted = false;

                        if (model.formFile != null)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.formFile.FileName);
                            var path = Path.Combine(_environment.WebRootPath, "MovieImage", fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await model.formFile.CopyToAsync(stream);
                            }

                            model.ImagePath = fileName;
                        }

                        _dataContext.Update(model);

                        var tags = await _dataContext.Movie_Tag.Where(x => x.MovieId == model.Id).ToListAsync();
                        _dataContext.Movie_Tag.RemoveRange(tags);
                        _dataContext.SaveChanges();

                        foreach (var item in model.TagIdList)
                        {
                            await _dataContext.Movie_Tag.AddAsync(new Movie_Tag()
                            {
                                CreatedBy = currentUser.UserName,
                                CreatedOn = DateTime.Now,
                                isActive = true,
                                isDeleted = false,
                                MovieId = model.Id,
                                TagId = Convert.ToInt32(item)
                            });
                        }

                        await _dataContext.SaveChangesAsync();
                        await transaction.Result.CommitAsync();

                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        return View(model);
                    }
                }
            }

            return View(model);
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
            .FirstOrDefaultAsync(x => x.isActive && !x.isDeleted && x.Id == id);

            if (result is null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Movie
            .Include(x => x.Movie_Tag)
            .ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.isActive && !x.isDeleted && x.Id == id);

            if (result is null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var result = _dataContext.Movie
            .Include(x => x.Movie_Tag)
            .ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Id == id);

            if (result is null)
            {
                return RedirectToAction("Index");
            }

            result.isDeleted = true;

            _dataContext.Entry(result).State = EntityState.Modified;
            if (_dataContext.SaveChanges() > 0)
            {
                return RedirectToAction("Index");
            }
            return View(result);
        }
    }
}
