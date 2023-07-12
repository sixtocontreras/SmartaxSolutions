using System;
using System.Collections.Generic;
using System.Text;
using log4net.Config;
using Smartax.Cronjob.Process.Clases.Utilidades;
using Smartax.Cronjob.Process.Clases.Transactions;
using Smartax.Cronjob.Process.Clases.Models;
using System.Threading.Tasks;
using System.Data;
using Smartax.Cronjob.Task.Clases.Transactions;
using Aspose.Cells;

namespace Smartax.Cronjob.Process
{
    class Program
    {
        public static Functions objFunctions = new Functions();
        public static EnviarEmails ObjEmails = new EnviarEmails();

        #region DEFINICION DE VARIABLES DEL SISTEMA
        //--DEFINICION DE VARIABLES PUBLICAS
        public static int _TipoProceso;
        public static string _NombreTarea;
        public static int _IdCliente;
        public static int _AnioGravable;
        public static string _MesEstadoFinanciero;
        public static int _IdFormularioImpuesto;
        public static string _VersionEf;
        public static int _IdEstado;
        public static int _IdUsuario;

        //public static int _IdFiltroEjecucion;
        //public static int _IdDepartamento;
        //public static int _IdMunicipio;
        //public static int _IdEstadoDeclaracion;
        const string quote = "\"";
        #endregion

        static void Main(string[] args)
        {
            try
            {
                //--AQUI INSTACIAMOS EL OBJETO PARA ESCRIBIR EN EL ARCHIVO DE LOS LOGS
                XmlConfigurator.Configure();

                #region DEFINICION DE VARIABLES PARA EJECUTAR LOS PROCESOS DE LA TAREA
                //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
                ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
                ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();

                string _MsgError = "", _DiaSemana = "";
                if (FixedData.AmbienteTask.Trim().Equals("PRODUCCION"))
                {
                    #region PARAMETROS DE ENTRADA
                    //--OBTENER DATOS DEL PROCESO A REALIZAR
                    _TipoProceso = Int32.Parse(args[0].ToString().Trim());
                    _NombreTarea = args[1].ToString().Trim();
                    _IdCliente = Int32.Parse(args[2].ToString());
                    _AnioGravable = Int32.Parse(args[3].ToString().Trim());
                    _MesEstadoFinanciero = args[4].ToString().Trim();
                    _IdFormularioImpuesto = Int32.Parse(args[5].ToString());
                    _VersionEf = args[6].ToString().Trim();
                    _IdEstado = 1;
                    _IdUsuario = Int32.Parse(args[7].ToString());

                    //_IdClienteEf = 3;
                    //_IdFiltroEjecucion = Int32.Parse(args[2].ToString());
                    //_IdDepartamento = Int32.Parse(args[4].ToString());
                    //_IdMunicipio = args[5].ToString().Trim().Length > 0 ? Int32.Parse(args[5].ToString().Trim()) : -1;
                    //_IdEstadoDeclaracion = Int32.Parse(args[6].ToString());
                    //--
                    _DiaSemana = DateTime.Now.ToString("dddd").ToString().Trim().ToUpper();
                    #endregion
                }
                else if (FixedData.AmbienteTask.Trim().Equals("DESARROLLO_1"))
                {
                    #region PARAMETROS DE ENTRADA
                    //--DEFINICION DE VARUIABLES PARA PRUEBAS
                    _TipoProceso = FixedData.IDTIPO_PROCESO;
                    //--VALIDAR EL TIPO DE PROCESO
                    if (_TipoProceso == 1)
                    {
                        _NombreTarea = "TASK_BASE_GRAVABLE_MUNICIPIOS_" + DateTime.Now.ToString("ddMMyyyy");
                    }
                    else if (_TipoProceso == 2)
                    {
                        _NombreTarea = "TASK_BASE_GRAVABLE_OFICINAS_" + DateTime.Now.ToString("ddMMyyyy");
                    }
                    else if (_TipoProceso == 3)
                    {
                        _NombreTarea = "TASK_PROVISION_ICA_" + DateTime.Now.ToString("ddMMyyyy");
                    }
                    _IdCliente = 4;
                    _AnioGravable = FixedData.ANIO_GRAVABLE;
                    _MesEstadoFinanciero = FixedData.MES_EF;
                    _IdFormularioImpuesto = FixedData.IDFORMULARIO_IMPUESTO;
                    _VersionEf = "VERSION_1";
                    _IdEstado = 1;
                    _IdUsuario = 2;

                    //--VARIABLE UTILIZADAS PARA LAS DECLARACIONES POR LOTE
                    //_IdFiltroEjecucion = 1;
                    //_IdDepartamento = 11;
                    //_IdMunicipio = 474;
                    //_IdEstadoDeclaracion = 2;
                    _DiaSemana = DateTime.Now.ToString("dddd").ToString().Trim().ToUpper();
                    #endregion
                }
                else if (FixedData.AmbienteTask.Trim().Equals("DESARROLLO"))
                {
                    #region PARAMETROS DE ENTRADA
                    //--DEFINICION DE VARUIABLES PARA PRUEBAS
                    _TipoProceso = FixedData.IDTIPO_PROCESO;
                    _NombreTarea = "FILE_DAVIBOX_" + "2023" + "_MENSUAL" + "01";
                    _IdCliente = 4;
                    _AnioGravable = Int32.Parse(DateTime.Now.ToString("yyyy").ToString().Trim());
                    _MesEstadoFinanciero = "04";
                    _IdFormularioImpuesto = FixedData.IDFORMULARIO_IMPUESTO;
                    _VersionEf = "4e4e95bf-1738-41b0-872d-d0478344e3d6";
                    _IdEstado = 1;
                    _IdUsuario = 2;
                    _DiaSemana = DateTime.Now.ToString("dddd").ToString().Trim().ToUpper();
                    #endregion
                }

                FixedData.LogApi.Warn("PROCESO A EJECUTAR => AMBIENTE = " + FixedData.AmbienteTask.Trim() + ", TIPO PROCESO = " + _TipoProceso + ", NOMBRE TAREA = " + _NombreTarea + ", DIA SEMANA = " + _DiaSemana + ", AÑO = " + _AnioGravable);
                #endregion

                #region EJECUCION DE LOS PROCESOS
                //--TIPO PROCESO: 1.BASE GRAVABLE, 2.LIQUIDACION x OFICINA-->
                if (_TipoProceso == FixedData.TASK_LIQUIDACION_BG_MUNICIPIO ||
                    _TipoProceso == FixedData.TASK_LIQUIDACION_BG_OFICINA)
                {
                    #region REALIZAR EL PROCESO DE LA BASE GRAVABLE
                    //--INSTANCIAMOS EL OBJETO DE CLASE
                    ProcessDb objProcess = new ProcessDb();
                    objProcess.IdFormularioImpuesto = _IdFormularioImpuesto;
                    objProcess.AnioGravable = _AnioGravable;
                    objProcess.MesEf = _MesEstadoFinanciero;

                    //--PASO 1: TABLA TEMPORAL PARA LOS CODIGO DE CUENTAS
                    objProcess.TipoProceso = 1;
                    int _IdRegistro = 0;
                    string _MsgProcess = "";
                    bool _Result1 = objProcess.GetBorrarTablas(ref _MsgProcess);
                    bool _Result = objProcess.GetGenerarTablasTemp(ref _IdRegistro, ref _MsgProcess);

                    //--PASO 2: TABLA TEMPORAL PARA OBTENER EL VALOR DEL RENGLON 8
                    objProcess.TipoProceso = 2;
                    _IdRegistro = 0;
                    _MsgProcess = "";
                    bool _Result2 = objProcess.GetBorrarTablas(ref _MsgProcess);
                    _Result = objProcess.GetGenerarTablasTemp(ref _IdRegistro, ref _MsgProcess);

                    //--PASO 3: TABLA TEMPORAL PARA OBTENER TODOS LOS VALORES DEL E.F.
                    objProcess.TipoProceso = 3;
                    _IdRegistro = 0;
                    _MsgProcess = "";
                    bool _Result3 = objProcess.GetBorrarTablas(ref _MsgProcess);
                    _Result = objProcess.GetGenerarTablasTemp(ref _IdRegistro, ref _MsgProcess);

                    Transactions objTransac = new Transactions();
                    BaseGravable_Req objBaseGrav = new BaseGravable_Req();
                    objBaseGrav.tipo_proceso = _TipoProceso;
                    objBaseGrav.nombre_tarea = _NombreTarea;
                    objBaseGrav.id_cliente = _IdCliente;
                    objBaseGrav.idform_impuesto = _IdFormularioImpuesto;
                    objBaseGrav.anio_gravable = _AnioGravable;
                    objBaseGrav.mes_ef = _MesEstadoFinanciero;
                    objBaseGrav.version_ef = _VersionEf;
                    objBaseGrav.id_usuario = _IdUsuario;

                    //--VALIDAR EL TIPO DE PROCESO EN LA DB
                    objProcess.TipoProceso = _TipoProceso;
                    objProcess.IdEstadoProceso = 1;
                    objProcess.TipoConsulta = 1;
                    _IdRegistro = 0;
                    _MsgProcess = "";
                    bool _Result4 = objProcess.AddProcesoBaseGravable(ref _IdRegistro, ref _MsgProcess);

                    //--VALIDAR EL TIPO DE PROCESO
                    if (_TipoProceso == FixedData.TASK_LIQUIDACION_BG_MUNICIPIO)
                    {
                        //--PASO 4: BORRAR LA TABLA DE LA BASE GRAVABLE
                        objProcess.TipoProceso = 4;
                        objProcess.MesEf = _MesEstadoFinanciero;
                        _IdRegistro = 0;
                        _MsgProcess = "";
                        bool _Result5 = objProcess.GetBorrarTablas(ref _MsgProcess);
                        //objProcess.TipoProceso = 5;
                        //_Result = objProcess.GetGenerarTablas(ref _IdRegistro, ref _MsgProcess);

                        objTransac.ProcessBaseGravablePorMunicipio(objBaseGrav);
                    }
                    else if (_TipoProceso == FixedData.TASK_LIQUIDACION_BG_OFICINA)
                    {
                        //--PASO 4: BORRAR LA TABLA DE LA BASE GRAVABLE
                        objProcess.TipoProceso = 5;
                        objProcess.MesEf = _MesEstadoFinanciero;
                        _IdRegistro = 0;
                        _MsgProcess = "";
                        bool _Result5 = objProcess.GetBorrarTablas(ref _MsgProcess);

                        objTransac.ProcessBaseGravablePorOficina(objBaseGrav);
                    }
                    #endregion
                }
                else if (_TipoProceso == FixedData.TASK_LIQUIDACION_IMPUESTO_OFICINA)
                {
                    #region REALIZAR EL PROCESO DE LIQUIDACION POR OFICINAS
                    //--AQUI REALIZAMOS EL PROCESO DE BORRAR 1ero LA TABLA
                    ProcessDb objProcess = new ProcessDb();
                    objProcess.IdFormularioImpuesto = _IdFormularioImpuesto;
                    objProcess.AnioGravable = _AnioGravable;
                    objProcess.MesEf = _MesEstadoFinanciero;
                    objProcess.TipoProceso = 6;
                    //int _IdRegistro = 0;
                    string _MsgProcess = "";
                    bool _Result5 = objProcess.GetBorrarTablas(ref _MsgProcess);
                    //objProcess.TipoProceso = 3;
                    //bool _Result = objProcess.GetGenerarTablas(ref _IdRegistro, ref _MsgProcess);

                    //--INSTANCIAMOS EL OBJETO DE CLASE
                    LiqOficinas_Req objImpustosOfic = new LiqOficinas_Req();
                    objImpustosOfic.tipo_proceso = _TipoProceso;
                    objImpustosOfic.tipo_consulta = 1;
                    objImpustosOfic.nombre_tarea = _NombreTarea;
                    objImpustosOfic.id_cliente = _IdCliente;
                    objImpustosOfic.idform_impuesto = _IdFormularioImpuesto;
                    objImpustosOfic.anio_gravable = _AnioGravable;
                    objImpustosOfic.mes_ef = _MesEstadoFinanciero;
                    objImpustosOfic.id_estado = _IdEstado;
                    //--
                    Transactions objTransac = new Transactions();
                    objTransac.ProcessLiquidacionXOficinas(objImpustosOfic);
                    #endregion
                }
                else if (_TipoProceso == FixedData.TASK_PROCESAR_FILE_DAVIBOX)
                {
                    #region REALIZAR EL PROCESO DE LIQUIDACION POR OFICINAS
                    //--INSTANCIAMOS EL OBJETO DE CLASE
                    FileDavibox_Req objFileDavibox = new FileDavibox_Req();
                    objFileDavibox.anio_gravable = _AnioGravable;
                    objFileDavibox.mes_procesar = _MesEstadoFinanciero.ToString().Trim();
                    objFileDavibox.uuid = _VersionEf.ToString().Trim();
                    //--
                    ProcessFileDavibox objTransac = new ProcessFileDavibox();
                    objTransac.ProcesarArchivosDavibox(objFileDavibox);
                    #endregion
                }
                else if (_TipoProceso == FixedData.TASK_PROCESAR_FILE_DAVIBOX_2)
                {
                    #region REALIZAR EL PROCESO DE LIQUIDACION POR OFICINAS
                    // Cargar archivo de Excel
                    Workbook wb = new Workbook("D:\\ARCHIVOS DE PRUEBAS\\File_Davibox\\LH_04_2023.xlsx");

                    // Obtener todas las hojas de trabajo
                    WorksheetCollection collection = wb.Worksheets;

                    // Recorra todas las hojas de trabajo
                    for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
                    {

                        // Obtener hoja de trabajo usando su índice
                        Worksheet worksheet = collection[worksheetIndex];

                        // Imprimir el nombre de la hoja de trabajo
                        Console.WriteLine("Worksheet: " + worksheet.Name);

                        // Obtener el número de filas y columnas
                        int rows = worksheet.Cells.MaxDataRow;
                        int cols = worksheet.Cells.MaxDataColumn;

                        // Bucle a través de filas
                        for (int i = 0; i < rows; i++)
                        {

                            // Recorra cada columna en la fila seleccionada
                            for (int j = 0; j < cols; j++)
                            {
                                if (i == 0)
                                {
                                    // Valor de la celda de impresión
                                    Console.Write(worksheet.Cells[i, j].Value + " | ");
                                }
                                else if (i >= 1)
                                {
                                    // Valor de la celda de impresión
                                    Console.Write(worksheet.Cells[i, j].Value + " | ");
                                }
                            }
                            // Salto de línea de impresión
                            Console.WriteLine(" ");
                        }
                    }
                    Console.WriteLine(" ");
                    #endregion
                }
                else if (_TipoProceso == FixedData.TASK_ACTIVIDAD_USUARIOS)
                {
                    #region VALIDAMOS TIEMPO DE INACTIVIDAD DE USUARIOS
                    //--INSTANCIAMOS EL OBJETO DE CLASE
                    ProcessDb objProcess = new ProcessDb();
                    objProcess.TipoConsulta = 4;
                    objProcess.IdUsuario = null;
                    objProcess.IdCliente = 4;
                    objProcess.IdEstado = 1;

                    DataTable dtDatos = new DataTable();
                    dtDatos = objProcess.GetActividadUsuarios();
                    if (dtDatos != null)
                    {
                        if (dtDatos.Rows.Count > 0)
                        {
                            foreach (DataRow rowEst in dtDatos.Rows)
                            {
                                int _IdUsuario = Int32.Parse(rowEst["id_usuario"].ToString().Trim());
                                string _NombreUsuario = rowEst["nombre_usuario"].ToString().Trim();
                                string _EmailUsuario = rowEst["email_usuario"].ToString().Trim();
                                DateTime _FechaUltIngreso = DateTime.Parse(rowEst["fecha_ult_ingreso"].ToString().Trim());

                                // Difference in days, hours, and minutes.
                                TimeSpan ts = DateTime.Now - _FechaUltIngreso;
                                // Difference in days.
                                int _DiasObtenido = ts.Days;
                                //--VALIDAR LA CANTIDAD DE DIAS DE INACTIVIDAD
                                if (_DiasObtenido > FixedData.DIAS_INACTIVIDAD_USUARIO)
                                {
                                    #region AQUI INACTIVAMOS EL USUARIO EN EL SISTEMA
                                    //--
                                    objProcess.TipoProceso = 5;
                                    objProcess.IdUsuario = _IdUsuario;
                                    objProcess.NombreUsuario = _NombreUsuario;
                                    objProcess.IdEstado = 0;
                                    objProcess.IdUsuarioUp = 1;

                                    int _IdRegistro = 0;
                                    _MsgError = "";
                                    bool _Result = objProcess.SetProcesoUsuario(ref _IdRegistro, ref _MsgError);
                                    if (_Result == true)
                                    {
                                        #region AQUI ENVIAMOS LA NOTIFICACION POR CORREO
                                        //--
                                        ObjEmails.Asunto = "REF.: USUARIO INACTIVO";
                                        ObjEmails.EmailPara = _EmailUsuario;

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario, para informarle que su usuario ha sido INACTIVO en el sistema de SMARTAX por no haber tenido actividad en el sistema. Cualquier inquietud por favor contactar a soporte técnico." + "</h4>" +
                                                    "<br/><br/>" +
                                                    "para cualquier reclamo o inquietud se debe presentar el soporte de pago generado y por favor comuniquese con el area de servicio de atención al cliente." + "<br/>" +
                                                    "<br/><br/>" +
                                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                        ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                        string _MsgErrorEmail = "";
                                        if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                                        {
                                            FixedData.LogApi.Error(_MsgErrorEmail);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {

                }
                #endregion
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: ERROR AL EJECUTAR EL PROCESO";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: " + ex.Message + "</h4>" +
                            "<br/><br/>" +
                            "para cualquier reclamo o inquietud se debe presentar el soporte de pago generado y por favor comuniquese con el area de servicio de atención al cliente." + "<br/>" +
                            "<br/><br/>" +
                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                string _MsgErrorEmail = "";
                if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                {
                    FixedData.LogApi.Error(_MsgErrorEmail);
                }
                #endregion
            }
        }

        //private static async Task MainAsync(InfoEstablecimientos objBase)
        //{
        //    Console.WriteLine("--------| EL PROCESO DE LA TAREA CONCURRENTE INICIO |---------");
        //    //--AQUI DEFINIMOS LA CANTIDAD DE LOTES POR PROCESO

        //    ProcessConcurrentBag objProcess = new ProcessConcurrentBag();
        //    await objProcess.EjecutarProcessAsync(objBase, "carpetTemporal", 1);
        //    if (objProcess.ErrorMessage != "")
        //    {
        //        Console.WriteLine(objProcess.ErrorMessage);
        //    }
        //    Console.WriteLine("--------| EL PROCESO DE LA TAREA CONCURRENTE FINALIZO |---------");
        //}

    }
}
