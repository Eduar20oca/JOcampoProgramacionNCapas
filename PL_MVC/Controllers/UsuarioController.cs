using Microsoft.AspNet.Identity;
using ML;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI.Adapters;
using System.Data;
using System.Data.OleDb;
using Microsoft.Owin.Security.Provider;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Security;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using Antlr.Runtime;
using System.Security.Policy;
using System.Web.Management;

namespace PL_MVC.Controllers
{


    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.UsuarioML usuario = new UsuarioML();
            usuario.Rol = new Rol();

            //usuario = GetAllSOAP(usuario);
            usuario = GetAllREST();
            

            ML.Result resultRol = BL.RolBL.GetAll();
            if (resultRol.Correct)
            {
                usuario.Rol = new Rol();
                usuario.Rol.Roles = resultRol.Objects;
            }

            ViewBag.Listas = TempData["Mensaje"];
            return View(usuario);

            //UsuarioReference.CRUDUsuarioClient usuarioSOAP = new UsuarioReference.CRUDUsuarioClient();
            ////ML.Result result = BL.UsuarioBL.UsuarioGetAllSPEF(usuario);
            //var respueta = usuarioSOAP.GetAll(usuario);
            //if (respueta.Correct)
            //{
            //    usuario.Usuarios = respueta.Objects.ToList();            //}

        }

        [HttpPost]
        public ActionResult GetAll(UsuarioML Usuario, HttpPostedFileBase Archivo, string tipoArchivo)
        {            
            Usuario.Nombre = Usuario.Nombre ?? "";
            Usuario.ApellidoPaterno = Usuario.ApellidoPaterno ?? "";
            Usuario.ApellidoMaterno = Usuario.ApellidoMaterno ?? "";

            //ML.Result result = BL.UsuarioBL.UsuarioGetAllSPEF(Usuario);
            //UsuarioReference.CRUDUsuarioClient usuarioSOAP = new UsuarioReference.CRUDUsuarioClient();
            //var respuesta = usuarioSOAP.GetAll(Usuario);
            
            Usuario = BusquedaAbierta(Usuario);            

            ML.Result resultRol = BL.RolBL.GetAll();
            if (resultRol.Correct)
            {
                Usuario.Rol.Roles = resultRol.Objects;
            }


            if (tipoArchivo == "txt")
            {

                if (Archivo.FileName.Split('.')[1] == "txt")
                {
                    ML.CargaMasiva cargaMasiva = new ML.CargaMasiva();
                    cargaMasiva.Errores = new List<string>();
                    cargaMasiva.Correctos = new List<string>();

                    using (StreamReader reader = new StreamReader(Archivo.InputStream))
                    {
                        int contador = 0;
                        string encabezado = reader.ReadLine();
                        string linea;

                        while ((linea = reader.ReadLine()) != null)
                        {
                            contador++;
                            string[] campos = linea.Split('|');

                            BL.Validacion.Validar(campos, cargaMasiva.Errores);


                            if (cargaMasiva.Errores.Count == 0)
                            {
                                cargaMasiva.Correctos.Add($"{contador} El registro de {campos[1]} {campos[2]} {campos[3]} es valido");
                            }

                        }

                        if (cargaMasiva != null)
                        {
                            ViewBag.Listas = cargaMasiva;

                            if (cargaMasiva.Errores.Count == 0)
                            {
                                string path = Path.GetFileName(Archivo.FileName);
                                string fullPath = Server.MapPath("~/Content/txt/") + Path.GetFileNameWithoutExtension(Archivo.FileName) + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";

                                Session["RutaTxtValidado"] = fullPath;
                                if (!System.IO.File.Exists(fullPath))
                                {
                                    Archivo.SaveAs(fullPath);
                                }
                                ViewBag.Mensaje = "Archivo validado correctamente";
                            }
                            else
                            {
                                ViewBag.Guardar = true;

                                string fullPath = Server.MapPath("~/Content/txt/Errores/") + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";

                                using (StreamWriter writer = new StreamWriter(fullPath))
                                {

                                    foreach (string error in cargaMasiva.Errores)
                                    {
                                        writer.WriteLine($"{contador} {error}");
                                        contador++;
                                    }
                                }
                                Session["RutaTxtErrores"] = fullPath;
                            }
                        }
                    }
                }
                ViewBag.MostrarFormularioCarga = true;
            }

            else if (tipoArchivo == "xlsx")
            {

                string filePath = Path.GetFileName(Archivo.FileName);

                string fullPath = Server.MapPath("~/Content/Excel/") + Path.GetFileNameWithoutExtension(Archivo.FileName) + DateTime.Now.ToString("ddMMyyhhmmss") + ".xlsx";

                if (!System.IO.File.Exists(fullPath))
                {
                    Archivo.SaveAs(fullPath);
                }

                string conectionString = $"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=\"Excel 12.0 Xml;HDR=YES\"; Data Source={fullPath};";

                ML.Result resultExcel = BL.Validacion.LeerExcel(conectionString);

                ML.CargaMasiva cargaMasiva = new ML.CargaMasiva();
                cargaMasiva.Errores = new List<string>();
                cargaMasiva.Correctos = new List<string>();
                int contador = 0;

                foreach (ML.UsuarioML usuario in resultExcel.Objects)
                {
                    contador++;
                    string[] campos = new string[]
                    {

                 usuario.Nombre,
                 usuario.ApellidoPaterno,
                 usuario.ApellidoMaterno,
                 usuario.UserName,
                 usuario.Password,
                 usuario.Telefono,
                 usuario.Email,
                 usuario.Sexo,
                 usuario.Celular,
                 usuario.FechaDeNacimiento.ToString(),
                 usuario.CURP,
                 usuario.Rol.IdRol.ToString()
                    };

                    BL.Validacion.Validar(campos, cargaMasiva.Errores);

                    if (cargaMasiva.Errores.Count == 0)
                    {
                        cargaMasiva.Correctos.Add($"{contador} El registro de {campos[1]} {campos[2]} {campos[0]} es válido");
                    }
                }


                if (cargaMasiva != null)
                {
                    ViewBag.Listas = cargaMasiva;

                    if (cargaMasiva.Errores.Count == 0)
                    {

                        Session["RutaExcelValidado"] = fullPath;
                        ViewBag.Mensaje = "Archivo validado correctamente";
                        Session["UsuariosValidosExcel"] = resultExcel.Objects.Cast<ML.UsuarioML>().ToList();
                    }
                    else
                    {
                        ViewBag.Guardar = true;

                        string fullPathErrores = Server.MapPath("~/Content/Excel/ErroresExcel/") + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";

                        using (StreamWriter writer = new StreamWriter(fullPathErrores))
                        {

                            foreach (string error in cargaMasiva.Errores)
                            {
                                writer.WriteLine($"{contador} {error}");
                                contador++;
                            }
                        }

                        Session["RutaExcelErrores"] = fullPathErrores;
                    }
                    ViewBag.MostrarFormularioCarga = true;
                }
            }
            else
            {
                ViewBag.Mensaje = "No se seleccionó un archivo válido.";
            }

            return View(Usuario);
        }

        [HttpGet]
        public ActionResult Form(int? IdUsuario)
        {
            ML.UsuarioML usuario = new UsuarioML();
            usuario.Rol = new Rol();
            usuario.Direccion = new Direccion();
            usuario.Direccion.Colonia = new Colonia();
            usuario.Direccion.Colonia.Municipio = new Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new Estado();


            ML.Result resultRol = BL.RolBL.GetAll();

            if (resultRol.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;
            }

            ML.Result ResultEstado = BL.EstadoBL.GetAll();
            if (ResultEstado.Correct)
            {
                usuario.Direccion.Colonia.Municipio.Estado.Estados = ResultEstado.Objects;
            }

            if (IdUsuario > 0)
            {
                //ML.Result resultUsuario = BL.UsuarioBL.UsuarioGetByIdSPEF(IdUsuario.Value);
                //usuario = (ML.UsuarioML)resultUsuario.Object;

                //usuario = GetByIdSOAP(IdUsuario);
                usuario = GetByIdREST(IdUsuario);   

                if (usuario != null)
                {
                    ML.Result resultRol2 = BL.RolBL.GetAll();
                    usuario.Rol.Roles = resultRol2.Objects;

                    ML.Result resultEstado2 = BL.EstadoBL.GetAll();
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = ResultEstado.Objects;

                    ML.Result resultMunicipio = BL.MunicipioBL.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado.Value);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                    ML.Result resultColonia = BL.ColoniaBL.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
                }


            }
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Form(UsuarioML usuario, HttpPostedFileBase ArchivoImagen)
        {


            if (ModelState.IsValid)
            {
                HttpPostedFileBase imagenValida = Request.Files["ArchivoImagen"];
                if (ArchivoImagen != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        //copia el archivo a un memory stream (ms)
                        ArchivoImagen.InputStream.CopyTo(ms);
                        //convierte el memorystream un arreglo de bytes y asignamos el arreglo a la propiedad imagen
                        usuario.Imagen = ms.ToArray();
                    }
                }


                //ML.Result result = InsertUpdateSOAP(usuario);

                //UsuarioReference.CRUDUsuarioClient usuarioSOAP = new UsuarioReference.CRUDUsuarioClient();
                if (usuario.IdUsuario > 0)
                {
                    //    usuarioSOAP.Update(usuario);
                    UpdateREST(usuario);
                //    //BL.UsuarioBL.UsuarioUpdateSPEF(usuario);
                }
                else
                {
                    AddREST(usuario);
                //    usuarioSOAP.Add(usuario);
                //    //BL.UsuarioBL.UsuarioAddSPEF(usuario);
                }
            }
            else
            {
                usuario.Errores = new List<object>();
                usuario.Rol = new Rol();
                usuario.Direccion = new Direccion();
                usuario.Direccion.Colonia = new Colonia();
                usuario.Direccion.Colonia.Municipio = new Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new Estado();

                ML.Result resultRol = BL.RolBL.GetAll();

                if (resultRol.Correct)
                {
                    usuario.Rol.Roles = resultRol.Objects;
                }

                ML.Result ResultEstado = BL.EstadoBL.GetAll();
                if (ResultEstado.Correct)
                {
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = ResultEstado.Objects;
                }

                return View(usuario);
            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult Delete(int IdUsuario)
        {

            //UsuarioReference.CRUDUsuarioClient UsuarioSOAP = new UsuarioReference.CRUDUsuarioClient();

            //UsuarioSOAP.Delete(IdUsuario);
            //BL.UsuarioBL.UsuarioDeleteSPEF(IdUsuario);
            //ML.Result result = DeleteSOAP(IdUsuario);
            ML.Result result = DeleteREST(IdUsuario);

            return RedirectToAction("GetALL");
        }

        [HttpGet]
        public JsonResult GetByIdEstado(int IdEstado)
        {
            ML.Result ResultMunicipios = BL.MunicipioBL.GetByIdEstado(IdEstado);

            return Json(ResultMunicipios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByIdMunicipio(int? IdMunicipio)
        {
            if (IdMunicipio == null)
            {
                int IdMunicipioNull = 0;
                ML.Result ResultColonias = BL.ColoniaBL.GetByIdMunicipio(IdMunicipioNull);
                return Json(ResultColonias, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ML.Result ResultColonias = BL.ColoniaBL.GetByIdMunicipio(IdMunicipio);
                return Json(ResultColonias, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPost]
        public JsonResult UpdateEstatus(int IdUsuario, bool Estatus)
        {
            ML.Result resultUpdateEstatus = BL.UsuarioBL.UpdateEstatus(IdUsuario, Estatus);

            return Json(resultUpdateEstatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarErrores()
        {
            string path = Session["RutaTxtErrores"] as string;

            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
            {
                path = Session["RutaExcelErrores"] as string;
            }
            if ((string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)))
            {

                return Content("No hay archivo de errores disponible para descargar.");
            }

            string nombreArchivo = Path.GetFileName(path);
            Session.Remove("RutaTxtErrores");
            Session.Remove("RutaExcelErrores");
            return File(path, "text/plain", nombreArchivo);

        }

        [HttpPost]
        public ActionResult InsertarRegistros()
        {

            if (Session["RutaTxtValidado"] != null)
            {

                string ruta = Session["RutaTxtValidado"] as string;

                if (!string.IsNullOrEmpty(ruta) && System.IO.File.Exists(ruta))
                {
                    using (StreamReader reader = new StreamReader(ruta))
                    {
                        string encabezado = reader.ReadLine();
                        string linea;
                        ML.CargaMasiva cargaMasiva = new ML.CargaMasiva();
                        cargaMasiva.Errores = new List<string>();
                        cargaMasiva.Correctos = new List<string>();

                        while ((linea = reader.ReadLine()) != null)
                        {
                            string[] campos = linea.Split('|');

                            ML.UsuarioML usuario = new ML.UsuarioML();
                            usuario.Rol = new ML.Rol();

                            usuario.Nombre = campos[0];
                            usuario.ApellidoPaterno = campos[1];
                            usuario.ApellidoMaterno = campos[2];
                            usuario.UserName = campos[3];
                            usuario.Password = campos[4];
                            usuario.Telefono = campos[5];
                            usuario.Email = campos[6];
                            usuario.Sexo = campos[7];
                            usuario.Celular = campos[8];
                            usuario.FechaDeNacimiento = campos[9];
                            usuario.CURP = campos[10];
                            usuario.Rol.IdRol = Convert.ToInt32(campos[11]);

                            ML.Result result = BL.UsuarioBL.UsuarioAddTxtSPEF(usuario);


                            if (result.Correct)
                            {
                                cargaMasiva.Correctos.Add($"El usuario {usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno} se inserto correctamente");
                            }
                            else
                            {
                                cargaMasiva.Errores.Add($"El usuario {usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno} no se inserto, verifica los campos unicos");
                            }
                        }
                        if (cargaMasiva != null)
                        {
                            TempData["Mensaje"] = cargaMasiva;
                            Session.Remove("RutaTxtValidado");
                        }
                    }
                }
            }
            else if (Session["RutaExcelValidado"] != null)
            {
                var usuarios = Session["UsuariosValidosExcel"] as List<ML.UsuarioML>;


                ML.CargaMasiva cargaMasiva = new ML.CargaMasiva();
                cargaMasiva.Correctos = new List<string>();
                cargaMasiva.Errores = new List<string>();


                foreach (ML.UsuarioML usuario in usuarios)
                {
                    ML.Result result = BL.UsuarioBL.UsuarioAddTxtSPEF(usuario);

                    if (result.Correct)
                    {
                        cargaMasiva.Correctos.Add($"El usuario {usuario.Nombre} {usuario.ApellidoPaterno} se insertó correctamente.");
                    }
                    else
                    {
                        cargaMasiva.Errores.Add($"No se pudo insertar al usuario {usuario.Nombre} {usuario.ApellidoPaterno}.");
                    }
                }

                Session.Remove("UsuariosValidosExcel");
                Session.Remove("RutaExcelValidado");

                TempData["Mensaje"] = cargaMasiva;

            }

            return RedirectToAction("GetAll");
        }

        [NonAction]
        private ML.UsuarioML GetAllUsuarios(string xml)
        {
            var usuario1 = new ML.UsuarioML();
            usuario1.Usuarios = new List<object>();
            var xdoc = XDocument.Parse(xml);

            // Acceder a GetAllUsuarioResult
            var elementos = xdoc.Descendants("{http://schemas.microsoft.com/2003/10/Serialization/Arrays}anyType");

            foreach (var elem in elementos)
            {
                var usuario = new ML.UsuarioML();
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new Direccion();
                usuario.Direccion.Colonia = new Colonia();
                usuario.Direccion.Colonia.Municipio = new Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new Estado();

                // Manejo de IdUsuario null  
                //byte[]

                //if (elem.Element("{http://schemas.datacontract.org/2004/07/ML}IdUsuario")?.Value != null)
                //{
                //    idUsuario = int.Parse(elem.Element("{http://schemas.datacontract.org/2004/07/ML}IdUsuario")?.Value);
                //}
                //else
                //{
                //    idUsuario = 0;
                //}

                int idUsuario;
                int.TryParse(elem.Element("{http://schemas.datacontract.org/2004/07/ML}IdUsuario")?.Value, out idUsuario); //0 
                usuario.IdUsuario = idUsuario;

                // Acceso a otros campos 
                usuario.Nombre = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);
                usuario.ApellidoPaterno = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoPaterno")?.Value ?? string.Empty);
                usuario.ApellidoMaterno = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoMaerno")?.Value ?? string.Empty);
                usuario.UserName = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}UserName")?.Value ?? string.Empty);
                usuario.Email = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Email")?.Value ?? string.Empty);
                usuario.Password = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Password")?.Value ?? string.Empty);
                usuario.Sexo = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Sexo")?.Value ?? string.Empty);
                usuario.Telefono = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Telefono")?.Value ?? string.Empty);
                usuario.Celular = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Celular")?.Value ?? string.Empty);
                usuario.FechaDeNacimiento = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}FechaDeNacimiento")?.Value ?? string.Empty);
                usuario.CURP = (string)(elem.Element("{http://schemas.datacontract.org/2004/07/ML}FechaDeNacimiento")?.Value ?? string.Empty);

                bool Estatus;
                bool.TryParse(elem.Element("{http://schemas.datacontract.org/2004/07/ML}Estatus")?.Value, out Estatus);
                usuario.Estatus = Estatus;

                var propiedadRol = (elem.Element("{http://schemas.datacontract.org/2004/07/ML}Rol"));
                usuario.Rol.Descripcion = (string)(propiedadRol.Element("{http://schemas.datacontract.org/2004/07/ML}Descripcion")?.Value ?? string.Empty);

                var propiedadDireccion = elem.Element("{http://schemas.datacontract.org/2004/07/ML}Direccion");

                var propiedadColonia = propiedadDireccion.Element("{http://schemas.datacontract.org/2004/07/ML}Colonia");
                usuario.Direccion.Colonia.Nombre = (string)(propiedadColonia.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                var propiedadMunicipio = propiedadColonia.Element("{http://schemas.datacontract.org/2004/07/ML}Municipio");
                usuario.Direccion.Colonia.Municipio.Nombre = (string)(propiedadMunicipio.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                var propiedadEstado = propiedadMunicipio.Element("{http://schemas.datacontract.org/2004/07/ML}Estado");
                usuario.Direccion.Colonia.Municipio.Estado.Nombre = (string)(propiedadEstado.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                usuario1.Usuarios.Add(usuario);
            }
            return usuario1; // Devuelve el objeto completo 
        }

        [NonAction]
        private ML.UsuarioML GetAllSOAP(ML.UsuarioML usuario)
        {
            string action = "http://tempuri.org/ICRUDUsuario/GetAll";
            string url = "http://localhost:56986/CRUDUsuario.svc"; //Url del servicio


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST"; //Siempre post al usar SOAP


            //Se crea el sobreSOAP
            string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetAll>
         <!--Optional:-->
         <tem:usuario>
            <!--Optional:-->
            <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
            <!--Optional:-->
            <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
            <!--Optional:-->
            <ml:Nombre>{usuario.Nombre}</ml:Nombre>
            <!--Optional:-->           
            <ml:Rol>              
               <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>             
            </ml:Rol>            
         </tem:usuario>
      </tem:GetAll>
   </soapenv:Body>
</soapenv:Envelope>";

            //Envia la Solicitud
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                stream.Write(content, 0, content.Length);
            }
            // Obtener la respuesta 
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        // Deserializar el XML 
                        var usuarios = GetAllUsuarios(result); // Captura el objeto completo 
                        return usuarios; // Asegúrate de que tu vista esté lista para recibir este objeto 
                    }
                }
            }
            catch (WebException ex)
            {
                TempData["Mensaje"] = ex.Message; // Para mostrar en la vista si es necesario 
            }

            return null;
        }

        [NonAction]
        private ML.Result InsertUpdateSOAP(ML.UsuarioML usuario)
        {

            string url = "http://localhost:56986/CRUDUsuario.svc"; // URL del servicio 
            string soapEnvelope;
            string action;
            // Verificar si IdUsuario es null o 0 (o algún valor que determines como "nuevo") 
            if (usuario.IdUsuario == 0)
            {
                // Crear el sobre SOAP para agregar un nuevo usuario 
                action = "http://tempuri.org/ICRUDUsuario/Add";
                soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?> 
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:Add>
         <tem:Usuario>
            <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
            <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
            <ml:CURP>{usuario.CURP}</ml:CURP>
            <ml:Celular>{usuario.Celular}</ml:Celular>
            <ml:Direccion>
               <ml:Calle>{usuario.Direccion.Calle}</ml:Calle>
               <ml:Colonia>
                  <ml:IdColonia>{usuario.Direccion.Colonia.IdColonia}</ml:IdColonia>
                  <ml:Municipio>
                     <ml:Estado>
                        <ml:IdEstado>{usuario.Direccion.Colonia.Municipio.Estado.IdEstado}</ml:IdEstado>
                     </ml:Estado>
                     <ml:IdMunicipio>{usuario.Direccion.Colonia.Municipio.IdMunicipio}</ml:IdMunicipio>                  
                  </ml:Municipio>
               </ml:Colonia>
               <ml:NumeroExterior>{usuario.Direccion.NumeroExterior}</ml:NumeroExterior>
               <ml:NumeroInterior>{usuario.Direccion.NumeroInterior}</ml:NumeroInterior>
            </ml:Direccion>
            <ml:Email>{usuario.Email}</ml:Email>      
            <ml:FechaDeNacimiento>{usuario.FechaDeNacimiento}</ml:FechaDeNacimiento>
            <ml:IdUsuario>{usuario.IdUsuario}</ml:IdUsuario>
            <ml:Imagen>{usuario.Imagen}</ml:Imagen>
            <ml:Nombre>{usuario.Nombre}</ml:Nombre>
            <ml:Password>{usuario.Password}</ml:Password>
            <ml:Rol>
               <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>
            </ml:Rol>
            <ml:Sexo>{usuario.Sexo}</ml:Sexo>
            <ml:Telefono>{usuario.Telefono}</ml:Telefono>
            <ml:UserName>{usuario.UserName}</ml:UserName>
         </tem:Usuario>
      </tem:Add>
   </soapenv:Body>
</soapenv:Envelope>";
            }
            else
            {
                // Crear el sobre SOAP para actualizar un usuario existente 
                action = "http://tempuri.org/ICRUDUsuario/Update";
                soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?> 
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:Add>
         <tem:Usuario>
            <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
            <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
            <ml:CURP>{usuario.CURP}</ml:CURP>
            <ml:Celular>{usuario.Celular}</ml:Celular>
            <ml:Direccion>
               <ml:Calle>{usuario.Direccion.Calle}</ml:Calle>
               <ml:Colonia>
                  <ml:IdColonia>{usuario.Direccion.Colonia.IdColonia}</ml:IdColonia>
                  <ml:Municipio>
                     <ml:Estado>
                        <ml:IdEstado>{usuario.Direccion.Colonia.Municipio.Estado.IdEstado}</ml:IdEstado>
                     </ml:Estado>
                     <ml:IdMunicipio>{usuario.Direccion.Colonia.Municipio.IdMunicipio}</ml:IdMunicipio>                  
                  </ml:Municipio>
               </ml:Colonia>
               <ml:NumeroExterior>{usuario.Direccion.NumeroExterior}</ml:NumeroExterior>
               <ml:NumeroInterior>{usuario.Direccion.NumeroInterior}</ml:NumeroInterior>
            </ml:Direccion>
            <ml:Email>{usuario.Email}</ml:Email>      
            <ml:FechaDeNacimiento>{usuario.FechaDeNacimiento}</ml:FechaDeNacimiento>
            <ml:IdUsuario>{usuario.IdUsuario}</ml:IdUsuario>
            <ml:Imagen>{usuario.Imagen}</ml:Imagen>
            <ml:Nombre>{usuario.Nombre}</ml:Nombre>
            <ml:Password>{usuario.Password}</ml:Password>
            <ml:Rol>
               <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>
            </ml:Rol>
            <ml:Sexo>{usuario.Sexo}</ml:Sexo>
            <ml:Telefono>{usuario.Telefono}</ml:Telefono>
            <ml:UserName>{usuario.UserName}</ml:UserName>
         </tem:Usuario>
      </tem:Add>
   </soapenv:Body>
</soapenv:Envelope>";
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action); // Aquí ya existe la variable action 
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            // Enviar la solicitud 
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                stream.Write(content, 0, content.Length);
            }

            // Obtener la respuesta 
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine("Respuesta SOAP:");
                        Console.WriteLine(result);

                        ML.Result resultRespuesta = Result(result);

                        return resultRespuesta;
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                TempData["Mensaje"] = ex.Message;
            }
            return null;
        }

        [NonAction]
        private ML.UsuarioML GetByIdSOAP(int? IdUsuario)
        {
            ML.UsuarioML usuario = null;

            if (IdUsuario.HasValue)
            {
                // Obtener el usuario por ID 
                string action = "http://tempuri.org/ICRUDUsuario/GetById";
                string url = "http://localhost:56986/CRUDUsuario.svc";

                // Crear el sobre SOAP para obtener un usuario por su ID 
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?> 
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/""> 
   <soapenv:Header/> 
   <soapenv:Body> 
      <tem:GetUsuarioById>
         <!--Optional:--> 
         <tem:IdUsuario>{IdUsuario}</tem:IdUsuario> 
      </tem:GetUsuarioById> 
   </soapenv:Body> 
</soapenv:Envelope>";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", action);
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Enviar la solicitud 
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                    stream.Write(content, 0, content.Length);
                }

                // Obtener la respuesta 
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            Console.WriteLine("Respuesta SOAP:");
                            Console.WriteLine(result);

                            // Deserializar el usuario 
                            usuario = GetUsuarioById(result);
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ViewBag.Error = ex.Message; // Para mostrar en la vista si es necesario 
                }
            }
            return usuario;
        }
        [NonAction]
        private ML.UsuarioML GetUsuarioById(string xml)
        {
            var xdoc = XDocument.Parse(xml);
            // Acceder a GetUsuarioByIdResult usando el namespace correcto 

            var usuarioElement = xdoc.Descendants().FirstOrDefault(e =>
                e.Name.LocalName == "Object" &&
                e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/");

            if (usuarioElement != null)
            {
                ML.UsuarioML usuario = new ML.UsuarioML();
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new Direccion();
                usuario.Direccion.Colonia = new Colonia();
                usuario.Direccion.Colonia.Municipio = new Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new Estado();

                usuario.Nombre = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value); //0 
                usuario.ApellidoPaterno = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}ApellioPaterno")?.Value);
                usuario.ApellidoMaterno = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}ApellioMaterno")?.Value);
                usuario.UserName = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}UserName")?.Value ?? string.Empty);
                usuario.Email = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Email")?.Value ?? string.Empty);
                usuario.Password = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Password")?.Value ?? string.Empty);
                usuario.Sexo = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Sexo")?.Value ?? string.Empty);
                usuario.Telefono = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Telefono")?.Value ?? string.Empty);
                usuario.Celular = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Celular")?.Value ?? string.Empty);
                usuario.FechaDeNacimiento = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}FechaDeNacimiento")?.Value ?? string.Empty);
                usuario.CURP = (string)(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}FechaDeNacimiento")?.Value ?? string.Empty);

                var propiedadRol = (usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Rol"));
                usuario.Rol.Descripcion = (string)(propiedadRol.Element("{http://schemas.datacontract.org/2004/07/ML}Descripcion")?.Value ?? string.Empty);

                var propiedadDireccion = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Direccion");

                var propiedadColonia = propiedadDireccion.Element("{http://schemas.datacontract.org/2004/07/ML}Colonia");
                usuario.Direccion.Colonia.Nombre = (string)(propiedadColonia.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                var propiedadMunicipio = propiedadColonia.Element("{http://schemas.datacontract.org/2004/07/ML}Municipio");
                usuario.Direccion.Colonia.Municipio.Nombre = (string)(propiedadMunicipio.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                var propiedadEstado = propiedadMunicipio.Element("{http://schemas.datacontract.org/2004/07/ML}Estado");
                usuario.Direccion.Colonia.Municipio.Estado.Nombre = (string)(propiedadEstado.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty);

                return usuario;
            }



            return null; // O lanzar una excepción si no se encontró el usuario 

        }
        [NonAction]
        private ML.Result DeleteSOAP(int? IdUsuario)
        {
            if (IdUsuario.HasValue)
            {
                // Obtener el usuario por ID 
                string action = "http://tempuri.org/ICRUDUsuario/Delete";
                string url = "http://localhost:56986/CRUDUsuario.svc";

                // Crear el sobre SOAP para obtener un usuario por su ID 
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?> 
    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:Delete>
         <!--Optional:-->
         <tem:IdUsuario>{IdUsuario}</tem:IdUsuario>
      </tem:Delete>
   </soapenv:Body>
</soapenv:Envelope>";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", action);
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Enviar la solicitud 
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                    stream.Write(content, 0, content.Length);
                }

                // Obtener la respuesta 
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            Console.WriteLine("Respuesta SOAP:");
                            Console.WriteLine(result);

                            // Deserializar el result
                            ML.Result resultRespuesta = Result(result);
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ViewBag.Error = ex.Message; // Para mostrar en la vista si es necesario 
                }
            }
            return null;
        }

        [NonAction]
        private ML.Result Result(string xml)
        {
            ML.Result result = new ML.Result();
            var xdoc = XDocument.Parse(xml);

            var resultElement = xdoc.Descendants()
                .FirstOrDefault(e =>
                    e.Name.LocalName.EndsWith("Result") &&
                    e.Name.NamespaceName == "http://tempuri.org/");


            result.Correct = Convert.ToBoolean(resultElement.Element("{http://schemas.datacontract.org/2004/07/SL_WCF}Correct")?.Value);
            result.ErrorMessage = (string)(resultElement.Element("{http://schemas.datacontract.org/2004/07/SL_WCF}ErrorMessage")?.Value);

            return result;
        }

        [NonAction]
        private ML.UsuarioML GetAllREST()
        {
            ML.UsuarioML usuario = new ML.UsuarioML();            

            using (var client = new HttpClient())
            {
                //Leer la URL base desde Web.config
                string endPoint = ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                client.BaseAddress = new Uri(endPoint);

                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait();

                var resultado = responseTask.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var readTask = resultado.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    usuario.Usuarios = new List<object>();

                    //Convertir cada objeto en un UsuarioML
                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.UsuarioML usuarioitem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.UsuarioML>(resultItem.ToString());
                        usuario.Usuarios.Add(usuarioitem);
                    }
                }
                else
                {                    
                    return null;
                }
            }
            return usuario;
        }

        [NonAction]
        private ML.Result AddREST(ML.UsuarioML usuario)
        {
            ML.Result result = new ML.Result();
            //usuario.ImagenBase64 = Convert.ToBase64String(usuario.Imagen);
            //usuario.Imagen = new byte[0];

            using (var client = new HttpClient())
            {
                // Leer la URL base desde Web.config
                string endPoint = ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                client.BaseAddress = new Uri(endPoint);

                // POST JSON del objeto usuario
                var postTask = client.PostAsJsonAsync<ML.UsuarioML>("Add", usuario);
                postTask.Wait();

                var respuesta = postTask.Result;

                if (respuesta.IsSuccessStatusCode)
                {
                    var readTask = respuesta.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    result = readTask.Result;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al agregar usuario: ";
                }
            }

            return result;
        }

        [NonAction]
        private ML.Result UpdateREST(ML.UsuarioML usuario)
        {
            ML.Result result = new ML.Result();

            usuario.ImagenBase64 = Convert.ToBase64String(usuario.Imagen);
            usuario.Imagen = new byte[0];

            using (var client = new HttpClient())
            {
                string endPoint = ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                client.BaseAddress = new Uri(endPoint);

                // PUT al endpoint "Update/{IdUsuario}"
                var putTask = client.PutAsJsonAsync($"Update/{usuario.IdUsuario}", usuario);
                putTask.Wait();

                var resupuesta = putTask.Result;

                if (resupuesta.IsSuccessStatusCode)
                {
                    var readTask = resupuesta.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    result = readTask.Result;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al actualizar usuario: ";
                }
            }
            return result;
        }
        [NonAction]
        private ML.Result DeleteREST(int idUsuario)
        {
            ML.Result result = new ML.Result();

            using (var client = new HttpClient())
            {
                string endPoint = ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                client.BaseAddress = new Uri(endPoint);

                var deleteTask = client.DeleteAsync($"Delete/{idUsuario}");
                deleteTask.Wait();

                var response = deleteTask.Result;

                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    result = readTask.Result;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al eliminar usuario: " + response.ReasonPhrase;
                }
            }

            return result;
        }        
        [NonAction]
        private ML.UsuarioML GetByIdREST(int? idUsuario)
        {
            ML.UsuarioML usuario = new ML.UsuarioML();
            try
            {
                string endPoint = System.Configuration.ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(endPoint);

                    var responseTask = client.GetAsync("GetById/" + idUsuario);
                    responseTask.Wait();

                    var resultAPI = responseTask.Result;

                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        usuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.UsuarioML>(readTask.Result.Object.ToString());
                        return usuario;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return usuario;
        }

        [NonAction]
        private ML.UsuarioML BusquedaAbierta(ML.UsuarioML usuario)
        {
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
            {
                // Leer la URL base desde Web.config
                string endPoint = ConfigurationManager.AppSettings["UsuarioRest"].ToString();
                client.BaseAddress = new Uri(endPoint);

                // POST JSON del objeto usuario
                var postTask = client.PostAsJsonAsync<ML.UsuarioML>("BusquedaAbierta", usuario);
                postTask.Wait();

                var respuesta = postTask.Result;

                if (respuesta.IsSuccessStatusCode)
                {
                    var readTask = respuesta.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    usuario.Usuarios = new List<object>();

                    //Convertir cada objeto en un UsuarioML
                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.UsuarioML usuarioitem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.UsuarioML>(resultItem.ToString());
                        usuario.Usuarios.Add(usuarioitem);
                    }
                }
                else
                {
                    return null;
                }
            }
            return usuario;
        }
    }

}