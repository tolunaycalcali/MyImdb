using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Movie_Comment : BaseObject
    {
        [Display(Name = "Yorum")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(250,ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
        public string Comment { get; set; }

        [Display(Name = "Puan")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public double Point { get; set; }

        public Movie? Movie { get; set; }
        public int MovieId { get; set; }

        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
