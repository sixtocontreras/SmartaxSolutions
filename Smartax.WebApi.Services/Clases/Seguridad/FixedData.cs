using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using log4net;

namespace Smartax.WebApi.Services.Clases.Seguridad
{
    public class FixedData
    {
        public static readonly string LOGS_AUDITORIA_NAME = "LogsWebApiSmartax_";
        public static readonly string CLAVE_ENCRYPT_DECRYPT = "KeySer2019*-";

        public static readonly ILog LogApi = LogManager.GetLogger(LOGS_AUDITORIA_NAME);
        public static readonly string MotorBaseDatos = ConfigurationManager.AppSettings["BaseDatosUtilizar"].ToString().Trim();
        public static readonly string AmbienteServicios = ConfigurationManager.AppSettings["AmbienteServicios"].ToString().Trim();

        //--DIRECTORIO VIRTUAL PLATAFORMA
        public static string ServerCorreoGmail = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"].ToString().Trim();
        public static int PuertoCorreoGmail = Int32.Parse(ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"].ToString().Trim());
        public static string UserCorreoGmail = ConfigurationManager.AppSettings["USER_ENVIO_EMAIL"].ToString().Trim();
        public static string PassCorreoGmail = ConfigurationManager.AppSettings["PASS_ENVIO_EMAIL"].ToString().Trim();
        public static string EnvioEmail = ConfigurationManager.AppSettings["ENVIO_EMAIL"].ToString().Trim();
        public static string EnvioEmailCopia = ConfigurationManager.AppSettings["ENVIO_EMAIL_COPIA"].ToString().Trim();
        public static string EmailDestinoError = ConfigurationManager.AppSettings["EMAIL_DESTINATION_ERROR"].ToString().Trim();

        //--DIRECTORIO VIRTUAL PLATAFORMA
        public static string DirectorioVirtual = ConfigurationManager.AppSettings["DirectorioVirtual"].ToString();
        public static string DirectorioArchivos = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString();
        public static string DirectorioTicket = ConfigurationManager.AppSettings["DirectorioTicket"].ToString();

    }
}