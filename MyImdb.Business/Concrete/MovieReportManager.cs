using Microsoft.EntityFrameworkCore;
using MyImdb.Business.Abstract;
using MyImdb.DAL.Context;
using MyImdb.DAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.Business.Concrete
{
    public class MovieReportManager(DataContext dataContext) : IMovieReport
    {
        private readonly DataContext _dataContext = dataContext;

        public List<MovieReport> MovieReports()
        {
            var result = _dataContext.Movie_Tag.Include(x => x.Tag).GroupBy(x => x.TagId).ToList();

            List<MovieReport> report = new List<MovieReport>();

            foreach (var item in result)
            {
                report.Add(new MovieReport()
                {
                    Tur = item.FirstOrDefault().Tag.Name,
                    Count = item.Count()
                });
            }

            return report;
        }
    }
}
