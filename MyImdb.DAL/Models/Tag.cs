using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class Tag : BaseObject
    {
        [Display(Name = "Etiket")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        public string Name { get; set; }

    }
}
