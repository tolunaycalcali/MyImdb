using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

namespace MyImdb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public TagController(DataContext dataContext = null)
        {
            _dataContext = dataContext;
        }

        [HttpGet("GetList")]
        [Authorize]
        public async Task<IEnumerable<Tag>> GetList()
        {
            return await _dataContext.Tag.Where(x => x.isActive && !x.isDeleted).ToListAsync();
        }

        [HttpGet("GetById")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Tag>> GetById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && x.isActive && !x.isDeleted);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add(Tag model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataContext.Tag.Add(model);
            await _dataContext.SaveChangesAsync();

            return Ok("Başarılı");
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update(Tag model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isExist = await _dataContext.Tag.AnyAsync(x => x.Id == model.Id);

            if (isExist)
            {
                _dataContext.Tag.Update(model);
                await _dataContext.SaveChangesAsync();

                return Ok("Başarılı");
            }

            return BadRequest();
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) { return BadRequest(); }

            var result = await _dataContext.Tag.FirstOrDefaultAsync(x => x.Id == id && x.isActive && !x.isDeleted);

            if (result != null)
            {
                result.isDeleted = true;
                result.ModifiedOn = DateTime.Now;
                result.ModifiedBy = "asd";

                _dataContext.Update(result);
                await _dataContext.SaveChangesAsync();

                return Ok(true);
            }

            return BadRequest();
        }
    }
}
