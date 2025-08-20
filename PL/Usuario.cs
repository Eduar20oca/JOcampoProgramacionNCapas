using BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    public class Usuario
    {
        static void Main(string[] args)
        {

            string ruta = @"C:\Users\digis\OneDrive\Documentos\JESUSEDUARDOOCAMPONIÑO\Registrs.txt";

            if (File.Exists(ruta))
            {
                using (StreamReader reader = new StreamReader(ruta))
                {
                    string linea1 = reader.ReadLine();

                    List<string> erroresEcontrados = new List<string>();
                    int contador = 0;                    
                    string linea;

                    while ((linea = reader.ReadLine()) != null)
                    {
                        ML.UsuarioML usuario = new ML.UsuarioML();
                        usuario.Rol = new ML.Rol();
                        string[] campos = linea.Split('|');
                        contador++;

                        string Id = campos[0];
                        string nombre = campos[1];
                        string apellidoPaterno = campos[2];
                        string apellidoMaterno = campos[3];
                        string Username = campos[4];
                        string Password = campos[5];
                        string Telefono = campos[6];
                        string Email = campos[7];
                        string Sexo = campos[8];
                        string Celular = campos[9];
                        string CURP = campos[10];
                        string IdRol = campos[11];
                                               

                        usuario.IdUsuario = Convert.ToInt16(Id);
                        usuario.Nombre = nombre;
                        usuario.ApellidoPaterno = apellidoPaterno;
                        usuario.ApellidoMaterno = apellidoMaterno;
                        usuario.UserName = Username;
                        usuario.Password = Password;
                        usuario.Telefono = Telefono;
                        usuario.Email= Email;
                        usuario.Sexo = Sexo;
                        usuario.Celular = Celular;
                        usuario.CURP = CURP;
                        usuario.Rol.IdRol= Convert.ToInt16(IdRol);

                        string confirmacion = Validar(usuario);

                        if(confirmacion != "")
                        {
                          erroresEcontrados.Add(String.Join(",", confirmacion));
                        }
                        
                        foreach(var Error in erroresEcontrados)
                        {
                            Console.WriteLine($"Linea {contador}: {Error}");
                        }

                    }
                }
            }
        }

        public static string Validar(ML.UsuarioML usuario)
        {
            usuario.Errores = new List<object>();

            if(usuario.IdUsuario <= 0)
            {
                string Error = $"El Id {usuario.IdUsuario} No puede ser menor que cero";
                usuario.Errores.Add(Error);

                return Error;
            }
            if (string.IsNullOrEmpty(usuario.Nombre) || !usuario.Nombre.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                string Error = $"El Nombre {usuario.Nombre} No puede ser vacio o contener numeros";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(string.IsNullOrWhiteSpace(usuario.ApellidoPaterno) || !usuario.ApellidoPaterno.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                string Error = $"El Apellido {usuario.ApellidoPaterno} No puede ser vacio o contener numeros";
                usuario.Errores.Add(Error);
                return Error;
            }
            if (string.IsNullOrWhiteSpace(usuario.ApellidoMaterno) || !usuario.ApellidoMaterno.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                string Error = $"El Apellido {usuario.ApellidoMaterno} No puede ser vacio o contener numeros";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(string.IsNullOrWhiteSpace(usuario.UserName))
            {
                string Error = $"El UserName {usuario.UserName} No puede ser vacio o Contener Espacios";
                usuario.Errores.Add(Error);
                return Error;
            }   
            if(string.IsNullOrWhiteSpace(usuario.Password))
            {
                string Error = $"El Password {usuario.Password} No puede ser vacia o Contener Espacios";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(string.IsNullOrWhiteSpace(usuario.Telefono) || !Regex.IsMatch(usuario.Telefono, @"^\d{10}$"))
            {
                string Error = $"El Telefono {usuario.Password} Debe contener 10 numeros";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(string.IsNullOrWhiteSpace(usuario.Email) || !Regex.IsMatch(usuario.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                string Error = $"El Email {usuario.Email} Es Incorrecto";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(usuario.Sexo == null || usuario.Sexo.ToUpper() != "M" && usuario.Sexo.ToUpper() != "F")
            {
                string Error = $"El Genero {usuario.Sexo} debe ser 'M' o 'F'";
                usuario.Errores.Add(Error);
                return Error;
            }
            if (string.IsNullOrWhiteSpace(usuario.Celular) || !Regex.IsMatch(usuario.Celular, @"^\d{10}$"))
            {
                string Error = $"El Celular {usuario.Celular} debe contener 10 numeros";
                usuario.Errores.Add(Error);
                return Error;
            }
            if(string.IsNullOrWhiteSpace(usuario.CURP) || !Regex.IsMatch(usuario.CURP, @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$"))
            {
                string Error = $"El CURP {usuario.CURP} debe contener 18 Caracteres";
                usuario.Errores.Add(Error);
                return Error;
            }
            if (usuario.Rol.IdRol <= 0)
            {
                string Error = $"El Id del Rol {usuario.IdUsuario} No puede ser menor que cero";
                usuario.Errores.Add(Error);
                return Error;
            }
            return "";

        }
           
          
        
    }
}
