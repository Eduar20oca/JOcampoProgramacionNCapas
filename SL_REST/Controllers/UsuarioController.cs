using SL_REST.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_REST.Controllers
{
    [RoutePrefix("api")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            ML.UsuarioML usuario = new ML.UsuarioML();
            usuario.Rol = new ML.Rol();

            usuario.Nombre = "";
            usuario.ApellidoMaterno = "";
            usuario.ApellidoPaterno = "";
            usuario.Rol.IdRol = 0;

            ML.Result result = BL.UsuarioBL.UsuarioGetAllSPEF(usuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("GetById/{idUsuario}")]
        public IHttpActionResult GetById(int idUsuario)
        {
            ML.Result result = BL.UsuarioBL.UsuarioGetByIdSPEF(idUsuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add([FromBody] ML.UsuarioML usuario)
        {
            ML.Result result = BL.UsuarioBL.UsuarioAddSPEF(usuario);

            //usuario.Imagen = Convert.FromBase64String(usuario.ImagenBase64);
            //usuario.ImagenBase64 = "";

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPut]
        [Route("Update/{IdUsuario}")]
        public IHttpActionResult Update(int IdUsuario, [FromBody] ML.UsuarioML usuario)
        {
            usuario.IdUsuario = IdUsuario;
            usuario.Imagen = Convert.FromBase64String(usuario.ImagenBase64);
            usuario.ImagenBase64 = "";

            ML.Result result = BL.UsuarioBL.UsuarioUpdateSPEF(usuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpDelete]
        [Route("Delete/{Idusuario}")]
        public IHttpActionResult Delete(int Idusuario)
        {
            ML.Result result = BL.UsuarioBL.UsuarioDeleteSPEF(Idusuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [Route("BusquedaAbierta")]
        public IHttpActionResult BusquedaAbierta([FromBody] ML.UsuarioML usuario)
        {
            ML.Result result = BL.UsuarioBL.UsuarioGetAllSPEF(usuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }

    }
}
