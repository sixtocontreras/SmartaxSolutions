using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Smartax.WebApi.Services.Clases.Seguridad
{
    public class Functions
    {
        public string GetTime(int nHora)
        {
            string ResultTime = "";
            try
            {
                if (nHora <= 12)
                {
                    ResultTime = "Buenos días";
                }
                else if (nHora <= 18)
                {
                    ResultTime = "Buenas tarde";
                }
                else
                {
                    ResultTime = "Buenas noche";
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener el Time. Motivo: " + ex.Message);
            }

            return ResultTime;
        }

        public string GetRandom()
        {
            Random rnd = new Random();
            string strRandom = "";
            try
            {
                strRandom = Convert.ToString(rnd.Next(1, 9999999));
            }
            catch (Exception ex)
            {
                strRandom = Convert.ToString(rnd.Next(1, 9999999));
                FixedData.LogApi.Error("Error al generar el Random del Password. Motivo: " + ex.Message.ToString().Trim());
            }

            return strRandom.ToString().Trim();
        }

        public string Randomizer(int iStart, int iEnd)
        {
            int iRandomValue = 0;
            try
            {
                Random rnd = new Random();
                Int32 numRandom = rnd.Next(iStart, iEnd);

                //iRandomValue = iStart + (numRandom * (iEnd - iStart));
                iRandomValue = numRandom;
            }
            catch (Exception ex)
            {
                iRandomValue = 0;
            }

            return iRandomValue.ToString().Trim().PadLeft(6, '0');
        }

        //Método para convertir la Imagen a Bytes
        public Byte[] GetImagenBytes(string strRutaImagen)
        {
            Byte[] Arreglo = null;
            try
            {
                FileStream Imagen = new FileStream(strRutaImagen, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Arreglo = new Byte[Imagen.Length];
                BinaryReader reader = new BinaryReader(Imagen);
                Arreglo = reader.ReadBytes(Convert.ToInt32(Imagen.Length));
                reader.Close();
            }
            catch (Exception ex)
            {
                //Aqui hacemos el llamado de la Función que guardar los Logs de Errores que suceden en el sistema
                FixedData.LogApi.Error("Error al pasar la Imagen a Binario. Motivo: " + ex.Message.ToString().Trim());
            }

            return Arreglo;
        }

    }
}