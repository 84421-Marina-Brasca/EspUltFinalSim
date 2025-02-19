using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ejercicio112
{
    internal class GeneradorNros
    {
        public static double Truncar(double nro)
        {
            return Math.Truncate(nro * 10000) / 10000;
        }

        public static double Uniforme(double inf, double sup, double rnd)
        {
            return Truncar(inf + rnd * (sup - inf));
        }

        public static double Exponencial(double media, double rnd)
        {
            return Truncar(-media * Math.Log(1 - rnd));
        }
    }
}
