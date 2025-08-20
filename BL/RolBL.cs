using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RolBL
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {

           
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var query = context.RolGetAll().ToList();

                    if(query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach(var itemRol in query)
                        {
                            ML.Rol Rol = new ML.Rol();

                            Rol.IdRol = itemRol.IdRol;
                            Rol.Descripcion = itemRol.Descripcion;

                            result.Objects.Add(Rol);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        Console.WriteLine("No hay roles registrados");
                    }
                }

            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
