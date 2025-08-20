using BL;
using Microsoft.SqlServer.Server;
using ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Program
    {
       //static void Main(string[] args)
        {

            //Program.CRUDCompleto();         
            //AddEF();
            //DeleteEF();
            //GetById();
            //AddSPEF();
            //DeleteSPEF();
            //GetAllSPEF();


        }

        public static void CRUDCompleto()
        {
            bool salir = false;
            try
            {
                while (!salir)
                {
                    int opcion = MostrarMenu();
                    salir = EjecutarCRUD(opcion);
                }                
            } 
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un Error Al Ejecutar la Opcion" + e.Message);
            }

        }

        public static int MostrarMenu()
        {
            Console.WriteLine("*** CRUD COMPLETO ***\n1. Añadir Usuario\n2. Eliminar Usuario\n3. Modificar Usuario\n4. Mostrar Usuarios\n5. Buscar Por Id\n6. SPGetAll\n7. SPGetById\n8. Salir\nSeleciona una Opcion: ");

            return Convert.ToInt32(Console.ReadLine()); 
        }

        public static bool EjecutarCRUD(int opcion)
        {
            bool salir = false;
            
            switch (opcion)
            {
                case 1:
                    
                    Add();
                    break;

                case 2:
                    Delete();
                    break;

                case 3:
                    Update();
                    break;
                
                case 4:
                    GetAll();
                    break;

                case 5:
                    GetById();
                    break;

                case 6:
                    SPGetAll();
                    break;

                case 7:
                    SPGetById();
                    break;

                case 8:
                    salir = true;
                    break;

                default:
                    Console.WriteLine("Opcion no Válida, Selecciona Otra Opcion");
                    break;
            }
            return salir;                                     

        }

        public static void Add()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el UserName: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa el Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Ingresa el Password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa el Sexo (F/M): ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa el Telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa el Celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa la Fecha de Nacimiento (YYYY/MM/dd): ");
            usuario.FechaDeNacimiento = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ingresa el CURP: ");
            usuario.CURP = Console.ReadLine();
            Console.WriteLine("Ingresa el IdROl: ");
            usuario.IdRol = Convert.ToInt32(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioAdd(usuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Agregado con Exito");
            }
            else
            {
                Console.WriteLine("Ocurrio un Error al Agregar el Usuario");
            }

        }

        public static void Delete()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el ID del usuario a Eliminar: ");
            usuario.IdUsuario = Convert.ToInt32(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioDelete(usuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Eliminado con Exito");
            }
            else
            {
                Console.WriteLine("Ocurrio un Error al Eliminar el Usuario");
            }

        }

        public static void Update()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el ID del usuario que deseas actualizar: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Ingresa el UserName: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa el Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Ingresa el Password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa el Sexo (F/M): ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa el Telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa el Celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa la Fecha de Nacimiento(YYYY/MM/dd): ");
            usuario.FechaDeNacimiento = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ingresa el CURP: ");
            usuario.CURP = Console.ReadLine();
            Console.WriteLine("Ingresa el IdROl: ");
            usuario.IdRol = Convert.ToInt32(Console.ReadLine());


            ML.Result Result = UsuarioBL.UsuarioUpdate(usuario);
            
            

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Actualizado con Exito");
            }
            else
            {
                Console.WriteLine("Ocurrio un Error al Actualizar el Usuario. Verifica el Id ingresado");
            }

        }

        public static void GetAll()
        {
            
            ML.Result Lista= UsuarioBL.UsuarioGetAll();

            if (!Lista.Correct)
            {
                Console.WriteLine("Ocurrio un Error al Obtener a los Usuarios");
            }
            else if(Lista.Objects.Count <= 0)
            {
                Console.WriteLine("No hay Usuarios Registrados en el Sistema");
            }
            else 
            { 
                Console.WriteLine(" *** Usuarios Obtenidos ***");
                foreach (UsuarioML usuario in Lista.Objects)
                {
                    Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
                }                                             
              
            }
       
            
        }

        public static void GetById()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el Id el usuario que deseas buscas: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioGetById(usuario);

            if (Result.Correct )
            {
                Console.WriteLine("Usuario Encontrado con Exito");

                UsuarioML UsuarioEncontrado = (UsuarioML)Result.Object;
                
                Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
            }
            else
            {
                Console.WriteLine("Error al encontrar el usuario");
            }
        }

        public static void SPGetAll()
        {
            ML.Result Lista = UsuarioBL.SPGetAll();

            if (!Lista.Correct)
            {
                Console.WriteLine("Ocurrio un Error al Obtener a los Usuarios");
            }
            else if (Lista.Objects.Count <= 0)
            {
                Console.WriteLine("No hay Usuarios Registrados en el Sitemas");
            }
            else
            {
                Console.WriteLine("*** Usuarios Obtenidos ***");
                foreach(UsuarioML usuario in Lista.Objects)
                {
                    Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
                }
            }
        }

        public static void SPGetById()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el Id el usuario que deseas buscas: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioGetById(usuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Encontrado con Exito");

                UsuarioML UsuarioEncontrado = (UsuarioML)Result.Object;

                Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
            }
            else
            {
                Console.WriteLine("Error al encontrar el usuario");
            }
        }

        public static void AddEF()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el UserName: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa el Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Ingresa el Password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa el Sexo (F/M): ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa el Telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa el Celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa la Fecha de Nacimiento (YYYY/MM/dd): ");
            usuario.FechaDeNacimiento = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ingresa el CURP: ");
            usuario.CURP = Console.ReadLine();
            Console.WriteLine("Ingresa el IdROl: ");
            usuario.IdRol = Convert.ToInt32(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioAddEF(usuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Anadido con Exito");
            }
            else
            {
                Console.WriteLine("No se pudo agregar al usuario");
            }
        }

        public static void DeleteEF()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingresa el ID del usuario a Eliminar: ");
            usuario.IdUsuario = Convert.ToInt32(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioDeleteEF(usuario.IdUsuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Eliminado con Exito");
            }
            else
            {
                Console.WriteLine("Ocurrio un Error al Eliminar el Usuario");
            }

        }

        public static void UpdateEF()
        {
            UsuarioML usuario = new UsuarioML();
            Console.WriteLine("Ingre el Id del usuario que deseas modifcar: ");
            usuario.IdUsuario = Convert.ToInt32(Console.ReadLine());

            ML.Result Result = UsuarioBL.UsuarioUpdateEF(usuario);

            if (Result.Correct)
            {
                Console.WriteLine("Usuario Actualizado con Exito");
            }
            else
            {
                Console.WriteLine("No se pudo actualizar al usuario");
            }

        }

        public static void GetAllEF()
        {
            ML.Result Lista = UsuarioBL.UsuarioGetAllEF();

            if (Lista.Correct)
            {
                Console.WriteLine(" *** Usuarios Obtenidos ***");
                foreach (UsuarioML usuario in Lista.Objects)
                {
                    Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
                }
            }
            else
            {
                Console.WriteLine("No se pudieron obtener los usuarios");
            }
                
        }

        public static void GetByIdEF()
        {

            UsuarioML usuario = new UsuarioML();
            
            Console.WriteLine("Ingresa el id que deseas consultar: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());

            
            ML.Result result = BL.UsuarioBL.UsuarioGetByIDEF(usuario.IdUsuario);

            if (result.Correct)
            {
                Console.WriteLine("Usuario Encontrado con Exito");

                UsuarioML UsuarioEncontrado = (UsuarioML)result.Object;

                Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.IdRol}");
            }
            else
            {
                Console.WriteLine("Error al encontrar al usuario");
            }
            
        }

        public static void AddSPEF()
        {
            
            ML.UsuarioML usuario = new ML.UsuarioML();
            usuario.Rol = new ML.Rol();
            Console.WriteLine("Ingresa el UserName: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa el Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Ingresa el Password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa el Sexo (F/M): ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa el Telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa el Celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa la Fecha de Nacimiento (YYYY/MM/dd): ");
            usuario.FechaDeNacimiento = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ingresa el CURP: ");
            usuario.CURP = Console.ReadLine();
            Console.WriteLine("Ingresa el IdROl: ");
            usuario.Rol.IdRol = Convert.ToInt32(Console.ReadLine());

            ML.Result resultAdd = BL.UsuarioBL.UsuarioAddSPEF(usuario);

            if(resultAdd.Correct)
            { Console.WriteLine("El usuario fue aregado correctamente"); 
            }
            else
            {
                Console.WriteLine("Error al agregar usuario" + resultAdd.ErrorMessage);
            }
        }

        public static void DeleteSPEF()
        {
            ML.UsuarioML usuario = new UsuarioML();

            Console.WriteLine("Ingresa el Id del Usuario que deseas eliminar: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());

            ML.Result resultDelete = BL.UsuarioBL.UsuarioDeleteSPEF(usuario.IdUsuario);

            if (resultDelete.Correct)
            {
                Console.WriteLine("El usuario fue eliminado correctamente");

            }
            else
            {
                Console.WriteLine("El usuario no se logro eliminar" + resultDelete.ErrorMessage);
            }
        }

        public static void UpdateSPEF()
        {
            UsuarioML usuario = new UsuarioML();
            usuario.Rol = new ML.Rol();
            Console.WriteLine("Ingresa el ID del usuario que deseas actualizar: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Ingresa el UserName: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa el Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Apellido Materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa el Email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Ingresa el Password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa el Sexo (F/M): ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa el Telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa el Celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa la Fecha de Nacimiento(YYYY/MM/dd): ");
            usuario.FechaDeNacimiento = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Ingresa el CURP: ");
            usuario.CURP = Console.ReadLine();
            Console.WriteLine("Ingresa el IdROl: ");
            usuario.Rol.IdRol = Convert.ToInt32(Console.ReadLine());

            ML.Result result = UsuarioBL.UsuarioUpdateSPEF(usuario);

            if(result.Correct)
            {
                Console.WriteLine("El usuario fue actualizado correctamente");
            }
            else
            {
                Console.WriteLine("El usuario no fue actualizado");
            }
        }

        public static void GetAllSPEF()
        {
            ML.Result result = BL.UsuarioBL.UsuarioGetAllSPEF();

            if (result.Correct) 
            { 
               Console.WriteLine(" *** Usuarios Obtenidos ***");
                foreach (UsuarioML usuario in result.Objects)
                {

                    Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.Rol.Descripcion}");
                }
            }
            else
            {
                Console.WriteLine("No se pudieron obtener los usuarios");
            }



}

        public static void GetByIdSPEF()
        {

            UsuarioML usuario = new UsuarioML();

            Console.WriteLine("Ingresa el id que deseas consultar: ");
            usuario.IdUsuario = Convert.ToInt16(Console.ReadLine());


            ML.Result result = BL.UsuarioBL.UsuarioGetByIDEF(usuario.IdUsuario);

            if (result.Correct)
            {
                Console.WriteLine("Usuario Encontrado con Exito");

                UsuarioML UsuarioEncontrado = (UsuarioML)result.Object;

                Console.WriteLine($"IdUsuario= {usuario.IdUsuario}, UserName={usuario.UserName}, Nombre= {usuario.Nombre}, ApellidoPaterno= {usuario.ApellidoPaterno}, " +
                        $"ApellidoMaterno= {usuario.ApellidoMaterno}, Email= {usuario.Email}, Password = {usuario.Password}, Sexo = {usuario.Sexo}, Telefono = {usuario.Telefono}" +
                        $", Celular = {usuario.Celular}, Fecha de Nacimiento= {usuario.FechaDeNacimiento}, CURP= {usuario.CURP} , IdRol= {usuario.Rol.Descripcion}");
            }
            else
            {
                Console.WriteLine("Error al encontrar al usuario");
            }
        }            
    }

            
 }

