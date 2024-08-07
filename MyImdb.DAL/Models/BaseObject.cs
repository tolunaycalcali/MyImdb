using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Models
{
    public class BaseObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public string? ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [Display(Name = "Aktif")]

        public bool isActive { get; set; }

        [ScaffoldColumn(false)]
        public bool isDeleted { get; set; } = false;

        public string isActiveString => isActive ? "Aktif" : "Pasif";
    }
}
