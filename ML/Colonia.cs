using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Colonia
    {
        [Key]
        public int? IdColonia { get; set; }
        public String Nombre { get; set; }
        public ML.Municipio Municipio { get; set; } 
        public List<object> Colonias { get; set; }
    }
}   
