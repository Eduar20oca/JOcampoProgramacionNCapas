using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class EstadoBL
    {

        public static ML.Result GetAll()
        {
            ML.Result Result = new ML.Result();

            try
            {
                using(DL_EF.JOcampoProgramacionNCapasEntities1 Context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var estadoss = Context.EstadoGetAll().ToList();

                    if(estadoss.Count > 0)
                    {
                        Result.Objects = new List<object>();
                        
                        foreach(var estado in estadoss)
                        {
                            ML.Estado est = new ML.Estado();

                            est.IdEstado = estado.IdEstado;
                            est.Nombre = estado.Nombre;

                            Result.Objects.Add(est);
                        }
                        
                        Result.Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("No hay estaos registrados");
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
