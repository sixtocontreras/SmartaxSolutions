using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartax.Cronjob.Process.Clases.Utilidades
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
