using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DL_EF;
using System.Runtime.CompilerServices;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;

namespace BL
{
    public class UsuarioBL
    {
        public static ML.Result UsuarioAdd(UsuarioML usuario)
        {
            string StringDeContext = UsuarioDL.GetContext();
            //Sin StoreProcedure
            //String query = "INSERT INTO Usuario (Nombre,ApellidoPaterno,ApellidoMaterno,UserName,Email,[Password],Sexo,Telefono,Celular,FechaNacimiento,CURP,IdRol)
            // VALUES(@Nombre, @ApellidoMaterno, @ApellidoMaterno, @UserName, @Email, @Password, @Sexo, @Telefono, @Celular, @FechaNacimiento, @CURP, @IdRol)"


            ML.Result Result = new ML.Result();
            //Validacion de campos obligatorios
            
            if (!ValidarCamposObligatorios(usuario, out string mensaje))
            {
                Console.WriteLine("Error al ingresar datos " + mensaje);
                Result.Correct = false;
                return Result ;
            }         
            
            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    SqlCommand cmd = new SqlCommand("UsuarioAdd", Context);
                    //Sin StoreProcedure = SqlCommand cmd = new SqlCommand("query", Context);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaDeNacimiento);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    
                    Context.Open();
                    int FilasAfectadas = cmd.ExecuteNonQuery();
                    
                    if(FilasAfectadas > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Ocurrio un error al ingresar el usuario";
                    }
                }
            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;

        }

        public static ML.Result UsuarioDelete(UsuarioML usuario)
        {
            string StringDeContext = UsuarioDL.GetContext();
            //Sin Store Procedure
            //string query = "DELETE FROM Usuario WHERE IdUsuario = @IdUsuario"
            var Result = new ML.Result();

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    SqlCommand cmd = new SqlCommand("UsuarioDelete", Context);
                    //Sin SP  SqlCommand cmd = new SqlCommand("query", Context);
                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                        
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                    Context.Open();
                    int FilasAfectadas = cmd.ExecuteNonQuery();
                    
                    if (FilasAfectadas > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Ocurrio un error al eliminar al usuario";
                    }
                }
            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;        
        }

        public static ML.Result UsuarioUpdate(UsuarioML usuario)
        {
            string StringDeContext = UsuarioDL.GetContext();


            var Result = new ML.Result();

            if (!ValidarCamposObligatorios(usuario, out string mensaje))
            {
                Console.WriteLine("Error al ingresar datos " + mensaje);
                Result.Correct = false;
                return Result;
            }

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    SqlCommand cmd = new SqlCommand("UsuarioUpdate", Context);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaDeNacimiento);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    Context.Open();
                    int FilasAfectadas = cmd.ExecuteNonQuery();
                    
                    if (FilasAfectadas > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Ocurrio un error al actualizar al usuario";
                    }
                }
            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;

        }

        public static ML.Result UsuarioGetAll()
        {
            string StringDeContext = UsuarioDL.GetContext();
            string Query = "SELECT IdUsuario,Username,Nombre,ApellidoPaterno,ApellidoMaterno,Email,Password,Sexo,Telefono,Celular,FechaNacimiento,CURP,IdRol" +
                " FROM Usuario";
            
            var Result = new ML.Result();            

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    Context.Open();

                    SqlCommand cmd = new SqlCommand(Query, Context);
                    SqlDataReader lector = cmd.ExecuteReader();
                    Result.Objects = new List<object>();


                    while (lector.Read())
                    {
                                                
                        UsuarioML usuario = new UsuarioML();
                        usuario.IdUsuario = Convert.ToInt32(lector["IdUsuario"]);
                        usuario.UserName = lector["UserName"].ToString();
                        usuario.Nombre = lector["Nombre"].ToString();
                        usuario.ApellidoPaterno = lector["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = lector["ApellidoMaterno"].ToString();
                        usuario.Email = lector["Email"].ToString();
                        usuario.Password = lector["Password"].ToString();
                        usuario.Sexo = lector["Sexo"].ToString();
                        usuario.Telefono = lector["Telefono"] != DBNull.Value ? lector["Telefono"].ToString() : null;
                        usuario.Celular = lector["Celular"] != DBNull.Value ? lector["Celular"].ToString() : null;
                        //usuario.FechaDeNacimiento = lector["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(lector["FechaNacimiento"]): Convert.ToDateTime(null);
                        usuario.CURP = lector["CURP"] != DBNull.Value ? lector["CURP"].ToString(): null;
                        //usuario.IdRol = lector["IdRol"] != DBNull.Value ? (int?)Convert.ToInt32(lector["IdRol"]) : null;

                        Result.Objects.Add(usuario);

                    }

                    if (Result.Objects.Count > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Error al obtener a los usuarios";
                    }

                }

            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;
        }   

        public static ML.Result UsuarioGetById(UsuarioML usuario)
        {
            string StringDeContext = UsuarioDL.GetContext();
            string Query = "SELECT IdUsuario,Username,Nombre,ApellidoPaterno,ApellidoMaterno,Email,Password,Sexo,Telefono,Celular,FechaNacimiento,CURP,IdRol" +
                " FROM Usuario WHERE IdUsuario = @IdUsuario";

            var Result = new ML.Result();
            Result.Object = new UsuarioML();

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    Context.Open();

                    SqlCommand cmd = new SqlCommand(Query, Context);
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    SqlDataReader lector = cmd.ExecuteReader();

                    if (lector.Read())
                    {
                                             
                        usuario.IdUsuario = Convert.ToInt32(lector["IdUsuario"]);
                        usuario.UserName = lector["UserName"].ToString();
                        usuario.Nombre = lector["Nombre"].ToString();
                        usuario.ApellidoPaterno = lector["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = lector["ApellidoMaterno"].ToString();
                        usuario.Email = lector["Email"].ToString();
                        usuario.Password = lector["Password"].ToString();
                        usuario.Sexo = lector["Sexo"].ToString();
                        usuario.Telefono = lector["Telefono"] != DBNull.Value ? lector["Telefono"].ToString() : null;
                        usuario.Celular = lector["Celular"] != DBNull.Value ? lector["Celular"].ToString() : null;
                        //usuario.FechaDeNacimiento = lector["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(lector["FechaNacimiento"]) :Convert.ToDateTime(null);
                        usuario.CURP = lector["CURP"] != DBNull.Value ? lector["CURP"].ToString() : null;
                        //usuario.IdRol = lector["IdRol"] != DBNull.Value ? (int?)Convert.ToInt32(lector["IdRol"]) : null;

                        Result.Object = usuario;

                    }


                    if (Result.Object != null)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Error al obtener a los usuarios";
                    }
                }

            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;
        }         

        public static ML.Result SPGetAll()
        {
            string StringDeContext = UsuarioDL.GetContext();

            var Result = new ML.Result();

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    

                    SqlCommand cmd = new SqlCommand("UsuarioGetAll", Context);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    
                    Context.Open();
                    SqlDataReader lector = cmd.ExecuteReader();
                    Result.Objects = new List<object>();

                    while (lector.Read())
                    {
                      
                        UsuarioML usuario = new UsuarioML();
                        usuario.IdUsuario = Convert.ToInt32(lector["IdUsuario"]);
                        usuario.UserName = lector["UserName"].ToString();
                        usuario.Nombre = lector["Nombre"].ToString();
                        usuario.ApellidoPaterno = lector["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = lector["ApellidoMaterno"].ToString();
                        usuario.Email = lector["Email"].ToString();
                        usuario.Password = lector["Password"].ToString();
                        usuario.Sexo = lector["Sexo"].ToString();
                        usuario.Telefono = lector["Telefono"] != DBNull.Value ? lector["Telefono"].ToString() : null;
                        usuario.Celular = lector["Celular"] != DBNull.Value ? lector["Celular"].ToString() : null;
                        //usuario.FechaDeNacimiento = lector["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(lector["FechaNacimiento"]) : Convert.ToDateTime(null);
                        usuario.CURP = lector["CURP"] != DBNull.Value ? lector["CURP"].ToString() : null;
                        //usuario.IdRol = lector["IdRol"] != DBNull.Value ? (int?)Convert.ToInt32(lector["IdRol"]) : null;

                        Result.Objects.Add(usuario);

                    }

                    if (Result.Objects.Count > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Error al obtener a los usuarios";
                    }

                }

            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;

        }

        public static ML.Result SPGetById(UsuarioML usuario)
        {
            string StringDeContext = UsuarioDL.GetContext();
          

            var Result = new ML.Result();
            Result.Object = new UsuarioML();

            try
            {
                using (SqlConnection Context = new SqlConnection(StringDeContext))
                {
                    

                    SqlCommand cmd = new SqlCommand("UsuarioGetById", Context);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    Context.Open();
                    SqlDataReader lector = cmd.ExecuteReader();

                    if (lector.Read())
                    {

                        usuario.IdUsuario = Convert.ToInt32(lector["IdUsuario"]);
                        usuario.UserName = lector["UserName"].ToString();
                        usuario.Nombre = lector["Nombre"].ToString();
                        usuario.ApellidoPaterno = lector["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = lector["ApellidoMaterno"].ToString();
                        usuario.Email = lector["Email"].ToString();
                        usuario.Password = lector["Password"].ToString();
                        usuario.Sexo = lector["Sexo"].ToString();
                        usuario.Telefono = lector["Telefono"] != DBNull.Value ? lector["Telefono"].ToString() : null;
                        usuario.Celular = lector["Celular"] != DBNull.Value ? lector["Celular"].ToString() : null;
                        //usuario.FechaDeNacimiento = lector["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(lector["FechaNacimiento"]) : Convert.ToDateTime(null);
                        usuario.CURP = lector["CURP"] != DBNull.Value ? lector["CURP"].ToString() : null;
                        //usuario.IdRol = lector["IdRol"] != DBNull.Value ? (int?)Convert.ToInt32(lector["IdRol"]) : null;

                        Result.Object = usuario;

                    }


                    if (Result.Object != null)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Result.ErrorMessage = "Error al obtener a los usuarios";
                    }
                }

            }
            catch (Exception e)
            {
                Result.Ex = e;
                Result.ErrorMessage = e.Message;
                Result.Correct = false;
            }
            return Result;
        }

        public static ML.Result UsuarioAddEF(UsuarioML usuario)
        {
            ML.Result Result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new JOcampoProgramacionNCapasEntities1())
                {   
                    DL_EF.Usuario usuarioEF = new DL_EF.Usuario();

                    usuarioEF.Nombre = usuario.Nombre;
                    usuarioEF.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioEF.ApellidoMaterno = usuario.ApellidoMaterno;
                    usuarioEF.UserName = usuario.UserName;
                    usuarioEF.Password = usuario.Password;
                    usuarioEF.Email = usuario.Email;
                    usuarioEF.Sexo = usuario.Sexo;
                    //usuarioEF.FechaNacimiento = usuario.FechaDeNacimiento;
                    usuarioEF.Celular = usuario.Celular;
                    usuarioEF.Telefono = usuario.Telefono;
                    usuarioEF.CURP = usuario.CURP;
                    //usuarioEF.IdRol = usuario.IdRol;

                    context.Usuarios.Add(usuarioEF);
                    int FilasAfectadas = context.SaveChanges();

                    if(FilasAfectadas > 0)
                    {
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Console.WriteLine("No se pudo anadir al usuario");
                    }
                    
                }                    

            }
            catch (Exception ex)
            {
                Result.Correct = false;
                Result.ErrorMessage = "Error al anadir al usuario" + ex.Message;
                Result.Ex = ex;
            }
            return Result;
        }

        public static ML.Result UsuarioDeleteEF (int idUsuario)
        {
            ML.Result Result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 Context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {

                    var UsuarioGet = (from usuario in Context.Usuarios
                                where usuario.IdUsuario == idUsuario
                                select usuario).SingleOrDefault();

                    if(UsuarioGet != null)
                    {
                        Context.Usuarios.Remove(UsuarioGet);
                        Context.SaveChanges();

                        Result.Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("Usurio no Encontrado");
                        Result.Correct = false;
                    }                        
                               
                }                
            }
            catch (Exception ex)
            {
                Result.Correct = false;
                Result.ErrorMessage = "Ocurrio un error al eliminar el usuario" + ex.Message;
                Result.Ex = ex;
            }
            return Result;

        }

        public static ML.Result UsuarioUpdateEF(UsuarioML usuario)
        {
            ML.Result Result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var query = (from usuario1 in context.Usuarios
                                where usuario1.IdUsuario == usuario.IdUsuario
                                select usuario1).FirstOrDefault();

                    if(query != null)
                    {
                        query.Nombre = usuario.Nombre;
                        query.ApellidoMaterno = usuario.ApellidoMaterno;
                        query.ApellidoPaterno = usuario.ApellidoPaterno;
                        query.UserName = usuario.UserName;
                        query.Password = usuario.Password;
                        query.Email = usuario.Email;
                        query.Sexo = usuario.Sexo;
                        //query.FechaNacimiento = usuario.FechaDeNacimiento;
                        query.Celular = usuario.Celular;
                        query.Telefono = usuario.Telefono;
                        query.CURP = usuario.CURP;
                        //query.IdRol = usuario.IdRol;

                        context.Entry(query).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        if(query != null)
                        {
                            Console.WriteLine("Usuario Modificado Correctamente");
                        }
                        else
                        {
                            Console.WriteLine("No se encontro al usuario");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Result.Ex = ex;
                Result.ErrorMessage = "Ocurrio un error al actualizar el usuario" + ex.Message;
                Result.Correct = false;
            }
            
            return Result;

        }

        public static ML.Result UsuarioGetAllEF()
        {
            ML.Result Result = new ML.Result();


            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var query = (from usuario in context.Usuarios
                                 select new
                                 {
                                     IdUsuario = usuario.IdUsuario,
                                     Nombre = usuario.Nombre,
                                     ApellidoPaterno = usuario.ApellidoPaterno,
                                     ApellidoMaterno = usuario.ApellidoMaterno,
                                     UserName = usuario.UserName,
                                     Password = usuario.Password,
                                     FechaNacimiento = usuario.FechaNacimiento,
                                     Sexo = usuario.Sexo,
                                     CURP = usuario.CURP,
                                     Telefono = usuario.Telefono,
                                     Celular = usuario.Celular,
                                     IdROl = usuario.IdRol
                                 }).ToList();

                    if(query.Count > 0)
                    {
                        Result.Objects = new List<object>();

                        foreach(var usuario in query)
                        {
                            ML.UsuarioML usuario1 = new UsuarioML();
                            usuario1.IdUsuario = usuario.IdUsuario;
                            usuario1.Nombre = usuario.Nombre;
                            usuario1.ApellidoPaterno = usuario.ApellidoPaterno;
                            usuario1.ApellidoMaterno = usuario.ApellidoMaterno;
                            usuario1.UserName = usuario.UserName;
                            usuario1.Password = usuario.Password;
                            //usuario1.FechaDeNacimiento = usuario.FechaNacimiento;
                            usuario1.Sexo = usuario.Sexo;
                            usuario1.CURP = usuario.CURP;
                            usuario1.Telefono = usuario.Telefono;
                            usuario1.Celular = usuario.Celular;
                            //usuario1.IdRol = usuario.IdROl;

                            Result.Objects.Add(usuario1);

                        }

                        Result.Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("No hay usuarios registrados");
                        Result.Correct = false;
                    }
                }


            }
            catch (Exception ex)
            {
                Result.ErrorMessage = "Error al obtener a los usuarios";
                Result.Ex = ex;
                Result.Correct = false;
            }
            return Result;
        }

        public static ML.Result UsuarioGetByIDEF(int IdUsuario)
        {
            ML.Result Result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var query = (from usuario in context.Usuarios
                                 where usuario.IdUsuario == IdUsuario
                                 select new
                                 {
                                     IdUsuario = usuario.IdUsuario,
                                     Nombre = usuario.Nombre,
                                     ApellidoPaterno = usuario.ApellidoPaterno,
                                     ApellidoMaterno = usuario.ApellidoMaterno,
                                     UserName = usuario.UserName,
                                     Password = usuario.Password,
                                     FechaNacimiento = usuario.FechaNacimiento,
                                     Sexo = usuario.Sexo,
                                     CURP = usuario.CURP,
                                     Telefono = usuario.Telefono,
                                     Celular = usuario.Celular,
                                     IdRol = usuario.IdRol
                                 }).FirstOrDefault();

                    if (query != null)
                    {
                        ML.UsuarioML usuario = new ML.UsuarioML();

                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;
                        //usuario.FechaDeNacimiento = query.FechaNacimiento;
                        usuario.Sexo = query.Sexo;
                        usuario.CURP = query.CURP;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        //usuario.IdRol = query.IdRol;

                        Result.Object = usuario;
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Console.WriteLine("Usuario no encontrado");
                    }

                }
            }
            catch (Exception ex)
            {
                Result.Correct = false;
                Result.ErrorMessage = "Ocurrio un error al obtener al usuario" + ex.Message;
                Result.Ex = ex;
            }
            return Result;
        }

        public static ML.Result UsuarioAddSPEF(UsuarioML Usuario)
        {
            ML.Result result = new ML.Result();
            
            try
            {
                using(DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                                       
                    int filasafectadas = context.UsuarioADD(Usuario.Nombre, Usuario.ApellidoPaterno, Usuario.ApellidoMaterno, Usuario.UserName, Usuario.Email, Usuario.Password,
                        Usuario.Sexo, Usuario.Telefono,Usuario.Celular, Usuario.FechaDeNacimiento, Usuario.CURP, Usuario.Rol.IdRol,Usuario.Imagen ,Usuario.Direccion.Calle,Usuario.Direccion.NumeroInterior,
                        Usuario.Direccion.NumeroExterior, Usuario.Direccion.Colonia.IdColonia);

                    

                    if(filasafectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("Usuario no agregado");
                        result.Correct = false;
                    }
                }

            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Error al agregar al usuario";
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result UsuarioDeleteSPEF(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    int filasafectadas = context.UsuarioDelete(IdUsuario);

                    
                    if(filasafectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        Console.WriteLine("Usuario no actualizado");
                    }

                }

            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Error al eliminar usuario" + ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result UsuarioUpdateSPEF(UsuarioML Usuario)
        {
            ML.Result result = new ML.Result();

            try
            {

                using(DL_EF.JOcampoProgramacionNCapasEntities1 context = new JOcampoProgramacionNCapasEntities1())
                {

                    int filasafectadas = context.UsuarioUpdate(Usuario.IdUsuario, Usuario.Nombre, Usuario.ApellidoPaterno, Usuario.ApellidoMaterno, Usuario.UserName, Usuario.Email, Usuario.Password,
                        Usuario.Sexo, Usuario.Telefono, Usuario.Celular, Usuario.FechaDeNacimiento, Usuario.CURP, Usuario.Rol.IdRol, Usuario.Imagen, Usuario.Direccion.Calle , Usuario.Direccion.NumeroInterior, Usuario.Direccion.NumeroExterior, Usuario.Direccion.Colonia.IdColonia);

                    

                    if(filasafectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        Console.WriteLine("Error al actaulizar al usuario");
                    }
                }


            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }

        public static ML.Result UsuarioGetAllSPEF(ML.UsuarioML usuario)
        {
            ML.Result result = new ML.Result();
            
            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new JOcampoProgramacionNCapasEntities1())
                {
                    var usuariosList = context.UsuarioGetAll(usuario.Nombre,usuario.ApellidoPaterno,usuario.ApellidoMaterno ,usuario.Rol.IdRol).ToList();

                    if (usuariosList.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var usuarioL in usuariosList)
                        {
                            ML.UsuarioML usuario1 = new UsuarioML();
                            usuario1.Rol = new ML.Rol();
                            usuario1.Direccion = new ML.Direccion();
                            usuario1.Direccion.Colonia = new ML.Colonia();
                            usuario1.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario1.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                            usuario1.IdUsuario = usuarioL.IdUsuario;
                            usuario1.Nombre = usuarioL.Nombre;
                            usuario1.ApellidoPaterno = usuarioL.ApellidoPaterno;
                            usuario1.ApellidoMaterno = usuarioL.ApellidoMaterno;
                            usuario1.UserName = usuarioL.UserName;
                            usuario1.Email = usuarioL.Email;
                            usuario1.Password = usuarioL.Password;
                            usuario1.FechaDeNacimiento = usuarioL.FechaNacimiento;
                            usuario1.Sexo = usuarioL.Sexo;
                            usuario1.CURP = usuarioL.CURP;
                            usuario1.Telefono = usuarioL.Telefono;
                            usuario1.Celular = usuarioL.Celular;
                            usuario1.Imagen = usuarioL.Imagen;
                            usuario1.Rol.Descripcion = usuarioL.Descripcion;
                            usuario1.Estatus = usuarioL.Estatus;
                            usuario1.Direccion.Calle = usuarioL.Calle;
                            usuario1.Direccion.NumeroInterior = Convert.ToInt32(usuarioL.NumeroInterior);
                            usuario1.Direccion.NumeroExterior = Convert.ToInt32(usuarioL.NumeroExterior);
                            usuario1.Direccion.Colonia.Nombre = usuarioL.NombreColonia;
                            usuario1.Direccion.Colonia.Municipio.Nombre = usuarioL.NombreMunicipio;
                            usuario1.Direccion.Colonia.Municipio.Estado.Nombre = usuarioL.NombreEstado;

                            result.Objects.Add(usuario1);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay usuario registrados";
                    }                                      

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Error al obtener usuarios";
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result UsuarioGetByIdSPEF(int IdUsuario)
        {

            ML.Result Result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new JOcampoProgramacionNCapasEntities1())
                {
                    var usuario = context.UsuarioGetById(IdUsuario).FirstOrDefault();

                    if(usuario != null)
                    {                                                                     
                        
                        ML.UsuarioML usuario1 = new ML.UsuarioML();
                        usuario1.Rol = new ML.Rol();
                        usuario1.Direccion = new ML.Direccion();
                        usuario1.Direccion.Colonia = new ML.Colonia();
                        usuario1.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario1.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                        usuario1.IdUsuario = usuario.IdUsuario;
                        usuario1.Nombre = usuario.Nombre;
                        usuario1.ApellidoPaterno = usuario.ApellidoPaterno;
                        usuario1.ApellidoMaterno = usuario.ApellidoMaterno;
                        usuario1.UserName = usuario.UserName;
                        usuario1.Email = usuario.Email;
                        usuario1.Password = usuario.Password;
                        usuario1.FechaDeNacimiento = usuario.FechaNacimiento;
                        usuario1.Sexo = usuario.Sexo;
                        usuario1.CURP = usuario.CURP;
                        usuario1.Telefono = usuario.Telefono;
                        usuario1.Celular = usuario.Celular;
                        usuario1.Imagen = usuario.Imagen;
                        usuario1.Rol.IdRol = usuario.IdRol ?? 0;                        
                        usuario1.Direccion.Calle = usuario.Calle;
                        usuario1.Direccion.NumeroInterior = Convert.ToInt32(usuario.NumeroInterior);
                        usuario1.Direccion.NumeroExterior = Convert.ToInt32(usuario.NumeroExterior);
                        usuario1.Direccion.Colonia.IdColonia = usuario.IdColonia ?? 0;
                        usuario1.Direccion.Colonia.Municipio.IdMunicipio = usuario.IdMunicipio ?? 0;
                        usuario1.Direccion.Colonia.Municipio.Estado.IdEstado = usuario.IdEstado ?? 0;

                        Result.Object = usuario1;
                        Result.Correct = true;
                    }
                    else
                    {
                        Result.Correct = false;
                        Console.WriteLine("No se ecnontro al usuario");
                    }
                }

            }
            catch(Exception ex)
            {
                Result.Correct = false;
                Result.Ex = ex;
                Result.ErrorMessage = ex.Message;
            }
            return Result;
        }

        public static ML.Result UpdateEstatus(int IdUsuario , bool Estatus)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {
                    var filasAfectadas = context.UsuarioUpdateEstatus(IdUsuario, Estatus);

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        Console.WriteLine("No se actualizo el Estatus");
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result UsuarioGetAllView()
        {
            ML.Result result = new ML.Result();

            try
            {       
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new JOcampoProgramacionNCapasEntities1())
                {
                    var usuariosList = context.UsuarioGetAllViews.ToList();

                    if (usuariosList.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var usuarioL in usuariosList)
                        {
                            ML.UsuarioML usuario1 = new UsuarioML();
                            usuario1.Rol = new ML.Rol();
                            usuario1.Direccion = new ML.Direccion();
                            usuario1.Direccion.Colonia = new ML.Colonia();
                            usuario1.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario1.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                            usuario1.IdUsuario = usuarioL.IdUsuario;
                            usuario1.Nombre = usuarioL.Nombre;
                            usuario1.ApellidoPaterno = usuarioL.ApellidoPaterno;
                            usuario1.ApellidoMaterno = usuarioL.ApellidoMaterno;
                            usuario1.UserName = usuarioL.UserName;
                            usuario1.Email = usuarioL.Email;
                            usuario1.Password = usuarioL.Password;
                            usuario1.FechaDeNacimiento = usuarioL.FechaNacimiento;
                            usuario1.Sexo = usuarioL.Sexo;
                            usuario1.CURP = usuarioL.CURP;
                            usuario1.Telefono = usuarioL.Telefono;
                            usuario1.Celular = usuarioL.Celular;
                            usuario1.Imagen = usuarioL.Imagen;
                            usuario1.Rol.Descripcion = usuarioL.Descripcion;
                            usuario1.Estatus = usuarioL.Estatus;
                            usuario1.Direccion.Calle = usuarioL.Calle;
                            usuario1.Direccion.NumeroInterior = Convert.ToInt32(usuarioL.NumeroInterior);
                            usuario1.Direccion.NumeroExterior = Convert.ToInt32(usuarioL.NumeroExterior);
                            usuario1.Direccion.Colonia.Nombre = usuarioL.NombreColonia;
                            usuario1.Direccion.Colonia.Municipio.Nombre = usuarioL.NombreMunicipio;
                            usuario1.Direccion.Colonia.Municipio.Estado.Nombre = usuarioL.NombreEstado;

                            result.Objects.Add(usuario1);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay usuario registrados";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Error al obtener usuarios";
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result UsuarioAddTxtSPEF(UsuarioML Usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JOcampoProgramacionNCapasEntities1 context = new DL_EF.JOcampoProgramacionNCapasEntities1())
                {

                    int filasafectadas = context.UsuarioADDTxt(Usuario.Nombre, Usuario.ApellidoPaterno, Usuario.ApellidoMaterno, Usuario.UserName, Usuario.Email, Usuario.Password,
                        Usuario.Sexo, Usuario.Telefono, Usuario.Celular,Usuario.FechaDeNacimiento, Usuario.CURP, Usuario.Rol.IdRol);



                    if (filasafectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("Usuario no agregado");
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Error al agregar al usuario";
                result.Ex = ex;
            }
            return result;
        }



        public static bool ValidarCamposObligatorios(UsuarioML usuario, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(usuario.UserName))
            {
                mensaje = "El campo UserName es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                mensaje = "El campo Nombre es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.ApellidoPaterno))
            {
                mensaje = "El campo de Apellido Paterno es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.ApellidoMaterno))
            {
                mensaje = "El campo de Apellido Materno es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                mensaje = "El campo de Email es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.Password))
            {
                mensaje = "El campo de Password es Obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.Sexo))
            {
                mensaje = "El campo de Sexo es Obligatorio";
                return false;
            }else if (usuario.Sexo.Length > 2)
            {
                mensaje = "El campo 'Sexo' no puede tener mas de 2 caracteres (F/M)";
                return false;
            }

            mensaje = string.Empty;
            return true;

        }


    }
}
