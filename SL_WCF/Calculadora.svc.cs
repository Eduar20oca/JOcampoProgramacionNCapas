using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Calculadora" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Calculadora.svc or Calculadora.svc.cs at the Solution Explorer and start debugging.
    public class Calculadora : ICalculadora
    {
        public int Suma(int numero1, int numero2)
        {
            return numero1 + numero2;
        }

        public int Resta(int numero1, int numero2)
        {
            return numero1 - numero2;
        }
        public int Multiplicacion(int numero1, int numero2)
        {
            return numero1 * numero2;
        }
        public int Division(int numero1, int numero2)
        {
            if(numero2 <= 0)
            {
                Console.WriteLine("No se puede divir entre 0 o menores a 0");
            }
            return numero1 / numero2;
        }

    }
}
