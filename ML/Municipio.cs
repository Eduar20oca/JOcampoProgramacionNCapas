using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Municipio
    {
        [Key]
        public int? IdMunicipio { get; set; }    
        public String Nombre { get; set; }
        public ML.Estado Estado { get; set; }
        public List<object> Municipios { get; set; }
    }
}
