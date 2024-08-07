using MyImdb.DAL.Dtos;
using MyImdb.DAL.Models;

namespace MyImdb.Web.Models
{
    public class MoviesVm
    {
        public MovieFilter MovieFilter { get; set; }
        public List<Movie> Movie { get; set; }
    }
}
