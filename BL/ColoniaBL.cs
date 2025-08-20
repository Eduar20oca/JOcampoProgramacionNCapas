using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ColoniaBL
    {

        public static ML.Result GetByIdMunicipio(int? IdMunicipio)
        {
            ML.Result Result = new ML.Result(); 

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 Context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var col = Context.ColoniaGetByIdMunicipio(IdMunicipio).ToList();

                    if (col.Count > 0)
                    {
                        Result.Objects = new List<object>();

                        foreach (var colonia in col)
                        {
                            ML.Colonia colonia1 = new ML.Colonia();

                            colonia1.IdColonia = colonia.IdColonia;
                            colonia1.Nombre = colonia.Nombre;

                            Result.Objects.Add(colonia1);
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
