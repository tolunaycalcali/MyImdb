using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Dtos
{
    public class MovieFilter
    {
        [Display(Name = "Film Adı")]
        public string? Name { get; set; }
    }
}
