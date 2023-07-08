using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartax.Cronjob.Alertas.Clases
{
    public class FixedData
    {
        //Log Appender Name
        public static readonly string LOGS_AUDITORIA_NAME = "LogsTaskSmartax";
        public static readonly ILog LogApi = LogManager.GetLogger(LOGS_AUDITORIA_NAME);

        //--DEFINICION DE VARIABLES DEL SISTEMA
        public static string MotorBaseDatos = ConfigurationManager.AppSettings["BASE_DATOS_UTILIZAR"].ToString();
        public static string AmbienteTask = ConfigurationManager.AppSettings["AMBIENTE_TASK"].ToString();

        //--DEFINICION DE VARIABLES DEL SISTEMA
        public static string REPOSITORIO_LIQUIDACION = ConfigurationManager.AppSettings["REPOSITORIO_LIQUIDACION"].ToString();

        public static string SERVER_CORREO_GMAIL = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"].ToString();
        public static string PUERTO_CORREO_GMAIL = ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"].ToString();
        public static string EMAIL_ROOT_ACCOUNT = ConfigurationManager.AppSettings["EMAIL_ROOT_ACCOUNT"].ToString();
        public static string EMAIL_ROOT_PASSWORD = ConfigurationManager.AppSettings["EMAIL_ROOT_PASSWORD"].ToString();
        public static string EMAIL_DESTINATION_ERROR = ConfigurationManager.AppSettings["EMAIL_DESTINATION_ERROR"].ToString().Trim();

        //--AQUI DEFINIMOS LOS NOMBRES DE LOS PROCESOS A EJECUTAR
        public static readonly int TASK_LIQUIDACION_ICA_OFICINA = 1;           //--ACTUALIZAR PRODUCTOS DE RECARGAS A CELULAR (PUNTO RED)
        public static readonly int TASK_LIQUIDACION_ICA_LOTE = 2;           //--ACTUALIZAR PRODUCTOS DE RECARGAS A CELULAR (PUNTO RED)

    }
}
