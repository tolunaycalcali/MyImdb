using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Dtos
{
    public class LoginDto
    {

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Username { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Password { get; set; }
    }
}
