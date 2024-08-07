using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Movie_Tag : BaseObject
    {
        public Movie Movie { get; set; }
        public Tag Tag { get; set; }

        public int MovieId { get; set; }
        public int TagId { get; set; }
    }
}
