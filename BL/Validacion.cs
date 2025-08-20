using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BL
{
    public class Validacion
    {

        public static void Validar(string[] campo, List<string> Errores)
        {
            string error = "";

            if (string.IsNullOrWhiteSpace(campo[0]) || !campo[0].All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                error += $"El Nombre '{campo[0]}' no puede estar vacío ni contener números o caracteres especiales.";
            }

            if (string.IsNullOrWhiteSpace(campo[1]) || !campo[1].All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                error += $"El Apellido Paterno '{campo[1]}' no puede estar vacío ni contener números o caracteres especiales.";

            }

            if (string.IsNullOrWhiteSpace(campo[2]) || !campo[2].All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                error += $"El Apellido Materno '{campo[2]}' no puede estar vacío ni contener números o caracteres especiales.";

            }

            if (string.IsNullOrWhiteSpace(campo[3]))
            {
                error += $"El UserName '{campo[3]}' no puede estar vacío ni contener solo espacios.";

            }

            if (string.IsNullOrWhiteSpace(campo[4]))
            {
                error += $"El Password no puede estar vacío ni contener solo espacios.";

            }

            if (string.IsNullOrWhiteSpace(campo[5]) || !Regex.IsMatch(campo[5], @"^\d{10}$"))
            {
                error += $"El Teléfono '{campo[5]}' debe contener exactamente 10 dígitos numéricos.";

            }

            if (string.IsNullOrWhiteSpace(campo[6]) || !Regex.IsMatch(campo[6], @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                error += $"El Email '{campo[6]}' tiene un formato incorrecto.";

            }


            if (string.IsNullOrWhiteSpace(campo[7]) || (campo[8].ToUpper() != "M" && campo[7].ToUpper() != "F"))
            {
                error += $"El Sexo '{campo[7]}' debe ser 'M' o 'F'.";

            }

            if (string.IsNullOrWhiteSpace(campo[8]) || !Regex.IsMatch(campo[8], @"^\d{10}$"))
            {
                error += $"El Celular '{campo[9]}' debe contener exactamente 10 dígitos numéricos.";

            }

            if (!DateTime.TryParse(campo[9], out DateTime fechaNacimiento))
            {
                error += $"La Fecha de Nacimiento '{campo[9]}' no tiene un formato válido (ej: dd/MM/yyyy).";

            }
            else if (fechaNacimiento > DateTime.Now)
            {
                error += $"La Fecha de Nacimiento '{fechaNacimiento.ToShortDateString()}' no puede ser en el futuro.";

            }

            if (string.IsNullOrWhiteSpace(campo[10]) || !Regex.IsMatch(campo[10], @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$"))
            {
                error += $"El CURP '{campo[10]}' debe tener un formato válido de 18 caracteres.";

            }

            if (!int.TryParse(campo[11], out int IdRol) || IdRol <= 0)
            {
                error += $"El IdRol '{campo[11]}' debe ser un número entero mayor que cero.";

            }

            if (error != "")
            {
                Errores.Add(error);
            }
        }


        public static ML.Result LeerExcel(string connectionString)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (OleDbConnection context = new OleDbConnection(connectionString))
                {
                    context.Open();

                    string query = "SELECT * FROM [Sheet1$]";

                    OleDbCommand oleDbCommand = new OleDbCommand();
                    oleDbCommand.Connection = context;
                    oleDbCommand.CommandText = query;

                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);

                    DataTable dataTable = new DataTable();
                    oleDbDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {

                        result.Objects = new List<object>();

                        foreach (DataRow itemOledb in dataTable.Rows)
                        {
                            ML.UsuarioML usuario = new ML.UsuarioML();
                            usuario.Rol = new ML.Rol();

                            usuario.Nombre = itemOledb[0].ToString();
                            usuario.ApellidoPaterno = itemOledb[1].ToString();
                            usuario.ApellidoMaterno = itemOledb[2].ToString();
                            usuario.UserName = itemOledb[3].ToString();
                            usuario.Password = itemOledb[4].ToString();
                            usuario.Telefono = itemOledb[5].ToString();
                            usuario.Email = itemOledb[6].ToString();
                            usuario.Sexo = itemOledb[7].ToString();
                            usuario.Celular = itemOledb[8].ToString();
                            usuario.FechaDeNacimiento = itemOledb[9].ToString();
                            usuario.CURP = itemOledb[10].ToString();
                            usuario.Rol.IdRol = Convert.ToInt16(itemOledb[11]);

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo leer el archivo";
                    }


                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }


    }

}
