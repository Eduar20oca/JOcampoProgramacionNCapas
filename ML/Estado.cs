using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Estado
    {
        [Key]
        public int? IdEstado { get; set; }
        public String Nombre { get; set; }
        public List<object> Estados { get; set; }

    }
}
