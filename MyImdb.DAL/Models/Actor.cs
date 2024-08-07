using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Actor : BaseObject
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Surname { get; set; }
    }
}
