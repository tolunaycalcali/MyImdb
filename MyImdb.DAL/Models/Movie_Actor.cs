using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Movie_Actor : BaseObject
    {
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }

        public int MovieId { get; set; }
        public int ActorId { get; set; }
    }
}
