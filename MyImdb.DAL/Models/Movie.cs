using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Movie : BaseObject
    {
        [Display(Name = "Film")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Name { get; set; }

        [Display(Name = "Çıkış Tarihi")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Yönetmen")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Director { get; set; }

        [Display(Name = "Film Resmi")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string ImagePath { get; set; }

        public ICollection<Movie_Actor>? Actors { get; set; }

        public List<Movie_Tag>? Movie_Tag { get; set; }

        public List<Movie_Comment>? Movie_Comment { get; set; }

        [Display(Name = "Etiketler")]
        [NotMapped]
        public string[] TagIdList { get; set; }

        [NotMapped]
        public IFormFile formFile { get; set; }


        [NotMapped]
        public double TotalPoint { get; set; }
    }
}
