using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DL
{
    public class UsuarioDL
    {

        public static string GetContext()
        {
            string context = ConfigurationManager.ConnectionStrings["JOcampoProgramacionNCapas"].ToString();
            return context;
        }

       

        }
    }

