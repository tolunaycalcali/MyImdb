using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

namespace MyImdb.Web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class TagController : BaseController
    {
        private readonly DataContext _dataContext;
        public TagController(DataContext dataContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
            _dataContext = dataContext;
            signInManager = _signInManager;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {

            var result = await _dataContext.Tag.Where(x => !x.isDeleted).ToListAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag model)
        {
            ModelState.Remove("CreatedBy");
            ModelState.Remove("CratedOn");
            if (ModelState.IsValid)
            {
                if (await _dataContext.Tag.AnyAsync(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("Name", "Bu etiket zaten kayıtlı!");
                    return View(model);
                }
                model.CreatedBy = currentUser.UserName;
                model.CreatedOn = DateTime.Now;
                model.isDeleted = false;


                await _dataContext.Tag.AddAsync(model);
                if (await _dataContext.SaveChangesAsync() > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && !x.isDeleted);

            if (result == null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag model)
        {
            if (ModelState.IsValid)
            {
                if (await _dataContext.Tag.AnyAsync(x => x.Name == model.Name && x.Id != model.Id))
                {
                    ModelState.AddModelError("Name", "Bu etiket zaten kayıtlı!");
                    return View(model);
                }
                model.ModifiedBy = currentUser.UserName;
                model.ModifiedOn = DateTime.Now;

                _dataContext.Tag.Update(model);
                if (await _dataContext.SaveChangesAsync() > 0)
                {
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && !x.isDeleted);

            if (result == null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && !x.isDeleted);

            if (result == null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Index");
            }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && !x.isDeleted);

            if (result == null)
            {
                return RedirectToAction("Index");
            }

            result.ModifiedOn = DateTime.Now;
            result.ModifiedBy = currentUser.UserName;
            result.isDeleted = true;

            _dataContext.Tag.Update(result);
            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return RedirectToAction("Index");
            }

            return View(result);

        }
    }
}
