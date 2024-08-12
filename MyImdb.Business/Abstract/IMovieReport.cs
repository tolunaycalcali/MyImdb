using MyImdb.DAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.Business.Abstract
{
    public interface IMovieReport
    {
        List<MovieReport> MovieReports();
    }
}
