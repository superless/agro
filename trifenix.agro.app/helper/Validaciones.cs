using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.agro.app.helper
{
    public static class Validaciones
    {

        /// <summary>
        /// Valida si un número entero es válido, true es no válido
        /// esto es debido a que es para ser utilizado con e.cancel
        /// </summary>        
        /// <param name="nuevaLetra">letra que se esta añadiendo</param>
        /// <returns>true si no es entero</returns>
        public static bool ValidaEnteroParaKeyPress(char nuevaLetra)
        {
            if (!char.IsNumber(nuevaLetra) && !char.IsControl(nuevaLetra))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidaTextoSinEspacios(char nuevaLetra)
        {
            if (!char.IsLetter(nuevaLetra) && !char.IsNumber(nuevaLetra) && !char.IsControl(nuevaLetra))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool ValidaDecimalParaKeyPress(string textoactual, char nuevaLetra, int precision, int scale)
        {
            int nroEnteros = precision - scale;

            var decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if (!char.IsNumber(nuevaLetra) && !char.IsControl(nuevaLetra) && nuevaLetra != decimalSeparator)
            {
                return true;
            }


            if (nuevaLetra == ',')
            {

                if (textoactual.Contains(decimalSeparator))
                {
                    return true;
                }
            }
            else
            {
                if (!char.IsControl(nuevaLetra) && textoactual.Count() == nroEnteros && !textoactual.Contains(decimalSeparator))
                {
                    return true;
                }
            }

            if (!char.IsControl(nuevaLetra) && (textoactual.Count()) >= precision)
            {
                return true;
            }
            return false;
        }


    }
}
