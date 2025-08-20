using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MunicipioBL
    {

        public static ML.Result GetByIdEstado(int IdEstado)
        {
            ML.Result Result = new ML.Result();

            try
            {
                using(DL_EF.JOcampoProgramacionNCapasEntities1 Context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var muni = Context.MunicipioByIdEstado(IdEstado).ToList();

                    if(muni.Count > 0)
                    {
                        Result.Objects = new List<object>();

                        foreach(var municipio in muni)
                        {
                            ML.Municipio municipio1 = new ML.Municipio();
                            

                            municipio1.IdMunicipio = municipio.IdMunicipio;
                            municipio1.Nombre = municipio.Nombre;

                            Result.Objects.Add(municipio1);
                        }

                        Result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Result.Correct = false;
                Result.ErrorMessage = ex.Message;
                Result.Ex = ex;
            }

            return Result;
        }
    }
}
