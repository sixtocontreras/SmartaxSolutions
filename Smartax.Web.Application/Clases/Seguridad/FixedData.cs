using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class FixedData
    {
        public static readonly string LOG_AUDITORIA_NAME = "LogPlataformaTrx";

        //--DEFINICION DE VARIABLES GENERALES DEL SISTEMA
        public static readonly string BaseDatosUtilizar = ConfigurationManager.AppSettings["BaseDatosUtilizar"].ToString().Trim();
        public static readonly string AmbienteSistema = ConfigurationManager.AppSettings["AmbienteSistema"].ToString().Trim();
        public static readonly string DirectorioVirtual = ConfigurationManager.AppSettings["DirectorioVirtual"].ToString().Trim();
        public static readonly string DirectorioArchivos = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString().Trim();
        public static readonly string DirectorioArchivosApi = ConfigurationManager.AppSettings["DirectorioArchivosApi"].ToString().Trim();
        public static readonly int CantidadRegProcesar = Int32.Parse(ConfigurationManager.AppSettings["CANTIDAD_REG_PROCESAR"].ToString().Trim());
        public static readonly int CantidadRegProcesarFiles = Int32.Parse(ConfigurationManager.AppSettings["CANTIDAD_REG_PROCESAR_FILES"].ToString().Trim());
        public static readonly int LongitudClaveUsuario = Int32.Parse(ConfigurationManager.AppSettings["LONGITUD_CLAVE_USUARIO"].ToString().Trim());
        public static readonly string IdRolCtrlActividades = ConfigurationManager.AppSettings["IDROL_CTRL_ACTIVIDADES"].ToString().Trim();

        //--DEFINICION DE VARIABLES PARA ENVIO DE CORREOS
        public static readonly string ServerCorreoGmail = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"].ToString().Trim();
        public static readonly int PuertoCorreoGmail = Int32.Parse(ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"].ToString().Trim());
        public static readonly string UsuarioEmail = ConfigurationManager.AppSettings["UsuarioEmail"].ToString().Trim();
        public static readonly string PasswordEmail = ConfigurationManager.AppSettings["PasswordEmail"].ToString().Trim();
        public static readonly string EMAIL_PARA = ConfigurationManager.AppSettings["EMAILS_PARA"].ToString().Trim();
        public static readonly string EMAILS_COPIA = ConfigurationManager.AppSettings["EMAILS_COPIA"].ToString().Trim();

        //--DEFINICION DE VARIABLES PARA ALMACENAR LOS DATOS EN CACHE.
        public static readonly string GetCachePais = "DtPaises";
        public static readonly string GetCacheDptos = "DtDptos";
        public static readonly string GetCacheMunicipios = "DtMunicipios";
        public static readonly string GetCacheCuentasCliente = "DtCuentasCliente";

        //--Google ReCaptcha
        public static readonly string GoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["GoogleRecaptchaSiteKey"].ToString().Trim();
        public static readonly string GoogleRecaptchaSecretKey = ConfigurationManager.AppSettings["GoogleRecaptchaSecretKey"].ToString().Trim();
        public static readonly string GoogleRecaptchaApiUrl = ConfigurationManager.AppSettings["GoogleRecaptchaApiUrl"].ToString().Trim();
        public static readonly string GoogleRecaptchaResponse = ConfigurationManager.AppSettings["GoogleRecaptchaResponse"].ToString().Trim();
        public static readonly string GoogleRecaptchaSuccess = ConfigurationManager.AppSettings["GoogleRecaptchaSuccess"].ToString().Trim();
        public static readonly string ValidRecaptcha = ConfigurationManager.AppSettings["ValidRecaptcha"].ToString().Trim();

        //--Datos del nombre de la Plataforma
        public static readonly string NameEmpresa = ConfigurationManager.AppSettings["NameEmpresa"].ToString().Trim();
        public static readonly string PlatformName = ConfigurationManager.AppSettings["NameSystem"].ToString().Trim();
        public static readonly string PlatformVersion = ConfigurationManager.AppSettings["VesrionSystem"].ToString().Trim();
        public static readonly string PlatformUrlPagina = ConfigurationManager.AppSettings["UrlPagina"].ToString().Trim();

        //--Datos de variables para crear y ejecutar tareas
        public static readonly string MinutosEjecutarTasks = ConfigurationManager.AppSettings["MinutosEjecutarTasks"].ToString().Trim();
        public static readonly string UserCreateTasks = ConfigurationManager.AppSettings["UsernameCreateTasks"].ToString().Trim();
        public static readonly string PassCreateTasks = ConfigurationManager.AppSettings["UserPassCreateTasks"].ToString().Trim();
        public static readonly string PathTasksProgramadas = ConfigurationManager.AppSettings["PathTasksProgramadas"].ToString().Trim();

        //--Datos de variables para Conectarse a las API de Smartax Services
        public static readonly string BaseUrlSmartax = ConfigurationManager.AppSettings["BASE_URL_SMARTAX"].ToString().Trim();
        public static readonly string ActionSegmentSmartax = ConfigurationManager.AppSettings["ACTION_SEGMENT_SMARTAX"].ToString().Trim();
        public static readonly string UserWsSmartax = ConfigurationManager.AppSettings["USER_WS_SMARTAX"].ToString().Trim();
        public static readonly string PassWsSmartax = ConfigurationManager.AppSettings["PASS_WS_SMARTAX"].ToString().Trim();
        public static readonly double ValorRestarRenglon8 = Double.Parse(ConfigurationManager.AppSettings["VALOR_RESTAR_RENGLON8"].ToString().Trim());
        public static string IDRENGLONES_CONFIGURACION = ConfigurationManager.AppSettings["IDRENGLONES_CONFIGURACION"].ToString();

        //--Datos de variables para Conectarse a las API de Smartax Services Davibox
        public static readonly string BaseUrlDavibox = ConfigurationManager.AppSettings["BASE_URL_DAVIBOX"].ToString().Trim();

        //--VARIABLES DE CONFIGURACION DE SEPARADOR DE MILES Y DECIMALES
        public static readonly string SeparadorMilesAp = ConfigurationManager.AppSettings["SEPARADOR_MILES_AP"].ToString().Trim();
        public static readonly string SeparadorMilesFile = ConfigurationManager.AppSettings["SEPARADOR_MILES_FILE"].ToString().Trim();
        public static readonly string SeparadorDecimalesAp = ConfigurationManager.AppSettings["SEPARADOR_DECIMALES_AP"].ToString().Trim();
        public static readonly string SeparadorDecimalesFile = ConfigurationManager.AppSettings["SEPARADOR_DECIMALES_FILE"].ToString().Trim();

        //--Extensiones de archivos para cargar inventario
        public static readonly List<string> GetListExtensionesCons = new List<string>(new string[] { ".TXT", ".CSV" });

        //--DEFINIR VARIABLES IDs TABLA DE FORMULARIOS DE IMPUESTOS.
        public static readonly int IDFORM_IMPUESTO_ICA = 1;
        public static readonly int IDFORM_IMPUESTO_AUTORETE_ICA = 2;

        //--ARRAY DE MESES DEL AÑO
        public static readonly int _AnioBase = 2019;
        //public static readonly string[] ArrayDiaInicioMes = new string[] { "ENERO|1", "FEBRERO|4", "MARZO|4", "ABRIL|0", "MAYO|2", "JUNIO|5", "JULIO|0", "AGOSTO|3", "SEPTIEMBRE|6", "OCTUBRE|1", "NOVIEMBRE|4", "DICIEMBRE|6" };
        public static readonly string[] ArrayMeses = new string[] { "ENERO|1", "FEBRERO|4", "MARZO|4", "ABRIL|0", "MAYO|2", "JUNIO|5", "JULIO|0", "AGOSTO|3", "SEPTIEMBRE|6", "OCTUBRE|1", "NOVIEMBRE|4", "DICIEMBRE|6" };
        //public static readonly string[] ArrayMeses = new string[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
        public static readonly string[] ArrayDiasSemana = new string[] { "L", "M", "M", "J", "V", "S", "D" };
        public static readonly List<string> GetListExtensionesBac = new List<string>(new string[] { ".TXT", ".CSV" });
        public static readonly char[] META_CHARS = { '+', '"', '<', '>', ';', '/', '=', '\\' };

        //Arrays con la cantidad de días.
        public static readonly int[] ArrayEnero = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArrayFebrero = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
        public static readonly int[] ArrayFebreroBisiesto = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
        public static readonly int[] ArrayMarzo = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArrayAbril = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
        public static readonly int[] ArrayMayo = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArrayJunio = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
        public static readonly int[] ArrayJulio = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArrayAgosto = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArraySeptiembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
        public static readonly int[] ArrayOctubre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static readonly int[] ArrayNoviembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
        public static readonly int[] ArrayDiciembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

    }
}