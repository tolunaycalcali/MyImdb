using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Dtos
{
    public class AppUserDto
    {

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Username { get; set; }

        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Password { get; set; }
    }
}
