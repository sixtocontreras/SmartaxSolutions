using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartax.Cronjob.Process.Clases.Utilidades
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
        public static int MAXIMA_CONCURRENCIA = Int32.Parse(ConfigurationManager.AppSettings["MAXIMA_CONCURRENCIA"].ToString().Trim());
        public static int CANTIDAD_LOTE = Int32.Parse(ConfigurationManager.AppSettings["CANTIDAD_LOTE"].ToString().Trim());
        public static int IDTIPO_PROCESO = Int32.Parse(ConfigurationManager.AppSettings["IDTIPO_PROCESO"].ToString().Trim());
        public static int IDFORMULARIO_IMPUESTO = Int32.Parse(ConfigurationManager.AppSettings["IDFORMULARIO_IMPUESTO"].ToString().Trim());
        public static int ANIO_GRAVABLE = Int32.Parse(ConfigurationManager.AppSettings["ANIO_GRAVABLE"].ToString().Trim());
        public static string MES_EF = ConfigurationManager.AppSettings["MES_EF"].ToString();
        public static string RENGLONES_CONF_ICA = ConfigurationManager.AppSettings["RENGLONES_CONF_ICA"].ToString();
        public static string RENGLONES_CONF_AUTOICA = ConfigurationManager.AppSettings["RENGLONES_CONF_AUTOICA"].ToString();
        public static readonly double ValorRestarRenglon8 = Double.Parse(ConfigurationManager.AppSettings["VALOR_RESTAR_RENGLON8"].ToString().Trim());
        public static int DIAS_INACTIVIDAD_USUARIO = Int32.Parse(ConfigurationManager.AppSettings["DIAS_INACTIVIDAD_USUARIO"].ToString().Trim());

        //--DEFINICION DE VARIABLES PARA EL ENVIO DE CORREOS
        public static string ServerCorreoGmail = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"].ToString().Trim();
        public static int PuertoCorreoGmail = Int32.Parse(ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"].ToString().Trim());
        public static string UserCorreoGmail = ConfigurationManager.AppSettings["USER_ENVIO_EMAIL"].ToString().Trim();
        public static string PassCorreoGmail = ConfigurationManager.AppSettings["PASS_ENVIO_EMAIL"].ToString().Trim();

        //--DEFINICION DE VARIABLES PARA EL EMAIL DE NOTIFICACION
        public static string EnvioEmail = ConfigurationManager.AppSettings["ENVIO_EMAIL"].ToString().Trim();
        public static string EnvioEmailCopia = ConfigurationManager.AppSettings["ENVIO_EMAIL_COPIA"].ToString().Trim();
        public static string EmailDestinoError = ConfigurationManager.AppSettings["EMAIL_DESTINATION_ERROR"].ToString().Trim();
        public static string EmailCopiaProcesos = ConfigurationManager.AppSettings["EMAIL_COPIA_PROCESOS"].ToString().Trim();

        //--VARIABLES DE CONFIGURACION DE SEPARADOR DE MILES Y DECIMALES
        public static readonly string SeparadorMilesAp = ConfigurationManager.AppSettings["SEPARADOR_MILES_AP"].ToString().Trim();
        public static readonly string SeparadorMilesFile = ConfigurationManager.AppSettings["SEPARADOR_MILES_FILE"].ToString().Trim();
        public static readonly string SeparadorDecimalesAp = ConfigurationManager.AppSettings["SEPARADOR_DECIMALES_AP"].ToString().Trim();
        public static readonly string SeparadorDecimalesFile = ConfigurationManager.AppSettings["SEPARADOR_DECIMALES_FILE"].ToString().Trim();

        //--AQUI DEFINIMOS LOS NOMBRES DE LOS PROCESOS A EJECUTAR
        public static readonly int TASK_LIQUIDACION_BG_MUNICIPIO = 1;           //--GENERAR BASE GRAVABLE POR MUNICIPIO
        public static readonly int TASK_LIQUIDACION_BG_OFICINA = 2;             //--GENERAR BASE GRAVABLE POR OFICINA
        public static readonly int TASK_LIQUIDACION_IMPUESTO_OFICINA = 3;       //--LIQUIDACION POR OFICINAS
        public static readonly int TASK_LIQUIDACION_POR_LOTES = 4;              //--LIQUIDACION IMPUESTO POR OFICINA
        public static readonly int TASK_PROCESAR_FILE_DAVIBOX = 5;              //--PROCESAR LOS ARCHIVOS DESCARGADOS DEL DAVIBOX
        public static readonly int TASK_ACTIVIDAD_USUARIOS = 6;                 //--VALIDAR LA ACTIVIDAD DEL USUARIO EN EL SISTEMA

    }
}
