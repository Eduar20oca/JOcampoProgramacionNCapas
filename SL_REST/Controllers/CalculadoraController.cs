using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SL_REST.Models;

namespace SL_REST.Controllers
{
    [RoutePrefix("api")]
    public class CalculadoraController : ApiController
    {
        [HttpGet]
        [Route("Suma/{numero1}/{numero2}")]
        public IHttpActionResult Suma(int numero1 , int numero2)
        {
            return Content(HttpStatusCode.OK, numero1 + numero2);
        }

        [HttpGet]
        [Route("Resta/{numero1}/{numero2}")]
        public IHttpActionResult Resta(int numero1, int numero2)
        {
            return Content(HttpStatusCode.OK, numero1 + numero2);
        }

        [HttpGet]
        [Route("Multiplicacion/{numero1}/{numero2}")]
        public IHttpActionResult Multiplicacion(int numero1, int numero2)
        {
            return Content(HttpStatusCode.OK, numero1 * numero2);   
        }

        [HttpGet]
        [Route("Division/{numero1}/{numero2}")] 
        public IHttpActionResult Division(int numero1, int numero2)
        {
            if(numero2 > 0)
            {
                return Content(HttpStatusCode.OK, numero1 / numero2);
            }
            return Content(HttpStatusCode.OK, "No se puede dividir entre 0");
        }

        [HttpPost]
        [Route("Suma")]
        public IHttpActionResult Suma([FromBody] Numeros numeros)
        {
            return Content(HttpStatusCode.OK, numeros.Numero1 + numeros.Numero2);
        }

        [HttpPost]
        [Route("Resta")]
        public IHttpActionResult Resta([FromBody] Numeros numeros)
        {
            return Content(HttpStatusCode.OK, numeros.Numero1 - numeros.Numero2);
        }

        [HttpPost]
        [Route("Multiplicacion")]
        public IHttpActionResult Multiplicacion([FromBody] Numeros numeros)
        {
            return Content(HttpStatusCode.OK, numeros.Numero1 * numeros.Numero2);
        }

        [HttpPost]
        [Route("Division")]
        public IHttpActionResult Division([FromBody] Numeros numeros)
        {
            if (numeros.Numero2 <= 0)
                return Content(HttpStatusCode.BadRequest, "No se puede dividir entre 0");

            return Content(HttpStatusCode.OK, numeros.Numero1 / numeros.Numero2);
        }

    }
}
