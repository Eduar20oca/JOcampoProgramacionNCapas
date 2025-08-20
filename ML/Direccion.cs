using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Direccion
    {
        [Key]  
        public int IdDireccion { get; set; }

        [StringLength (50)]
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        public string Calle { get; set; }

        [Range(0,99999, ErrorMessage = "Maximo 5 caracteres")]
        public int NumeroExterior { get; set; }

        [Range(0, 99999, ErrorMessage = "Maximo 5 caracteres")]
        public int NumeroInterior { get; set; }
        
        public ML.Colonia Colonia { get; set; }
    }
}
