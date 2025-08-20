using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Rol
    {
        [Key]
        [Required(ErrorMessage = "El Rol es requerido")]
        public int IdRol { get; set; }

        public String Descripcion { get; set; } 

        public List<object> Roles { get; set; }

    }
}
