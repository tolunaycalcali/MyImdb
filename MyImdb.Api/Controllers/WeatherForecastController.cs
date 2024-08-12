using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

namespace MyImdb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DataContext _dataContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext dataContext = null)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetMovies")]
        public async Task<IEnumerable<Movie>> GetMovieList()
        {
            return await _dataContext.Movie
            .Include(x => x.Movie_Tag)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Movie_Comment).ToListAsync();
        }
    }
}
