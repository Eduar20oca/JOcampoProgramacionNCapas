using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class UsuarioViewController : Controller
    {
        [HttpGet]
        public ActionResult UsuarioGetAllView()
        {
            ML.UsuarioML usuario = new ML.UsuarioML();
            
            ML.Result result = BL.UsuarioBL.UsuarioGetAllView();

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
            }


            return View(usuario);
        }
    }
}