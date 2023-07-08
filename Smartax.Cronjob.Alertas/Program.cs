using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using Smartax.Cronjob.Alertas.Clases;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;

namespace Smartax.Cronjob.Alertas
{
    public class Program
    {
        public static Functions objFunctions = new Functions();
        public static EnviarEmails ObjEmails = new EnviarEmails();

        #region DEFINICION DE VARIABLES DEL SISTEMA
        //--DEFINICION DE VARIABLES PUBLICAS
        public static int _TipoProceso;
        public static int _IdCliente;
        public static int _IdUsuario;
        public static int _IdFiltroEjecucion;
        public static int _IdFormularioImpuesto;
        public static int _IdDepartamento;
        public static int _IdMunicipio;
        public static int _IdEstadoDeclaracion;
        public static int _IdEstado;
        public static int _IdEmpresa;
        public static int _IdTipoEnvio;
        public static int _IdRedAliada;
        public static string _NombreRedAliada;
        public static string _IdComercioRedAliada;
        public static string _EmailsRedAliada;
        public static string _TituloTablaHtml;
        public static string _FechaInicial;
        public static string _FechaFinal;
        public static int _AnioGravable;
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
                ObjEmails.ServerCorreo = FixedData.SERVER_CORREO_GMAIL.ToString().Trim();
                ObjEmails.PuertoCorreo = Int32.Parse(FixedData.PUERTO_CORREO_GMAIL.Trim());
                ObjEmails.EmailDe = FixedData.EMAIL_ROOT_ACCOUNT.ToString().Trim();
                ObjEmails.PassEmailDe = FixedData.EMAIL_ROOT_PASSWORD.ToString().Trim();
                _IdEmpresa = 2;

                string _MsgError = "", _DiaSemana = "";
                if (FixedData.AmbienteTask.Trim().Equals("PRODUCCION"))
                {
                    #region PARAMETROS DE ENTRADA
                    //--Definicion para producción
                    _IdCliente = Int32.Parse(args[0].ToString());
                    _IdUsuario = Int32.Parse(args[1].ToString());
                    _IdFiltroEjecucion = Int32.Parse(args[2].ToString());
                    _IdFormularioImpuesto = Int32.Parse(args[3].ToString());
                    _IdDepartamento = Int32.Parse(args[4].ToString());
                    _IdMunicipio = args[5].ToString().Trim().Length > 0 ? Int32.Parse(args[5].ToString().Trim()) : -1;
                    _IdEstadoDeclaracion = Int32.Parse(args[6].ToString());
                    _IdEstado = 1;
                    _TipoProceso = Int32.Parse(args[7].ToString().Trim());
                    _AnioGravable = Int32.Parse(args[8].ToString().Trim());
                    //_IdCliente = Int32.Parse(args[2].ToString().Trim());
                    //_IdEstado = Int32.Parse(args[3].ToString());
                    //_IdTipoEnvio = Int32.Parse(args[4].ToString());
                    //_AnioPeriodo = Int32.Parse(DateTime.Now.ToString("yyyy"));
                    _DiaSemana = DateTime.Now.ToString("dddd").ToString().Trim().ToUpper();
                    #endregion
                }
                else if (FixedData.AmbienteTask.Trim().Equals("DESARROLLO"))
                {
                    #region PARAMETROS DE ENTRADA
                    //--Definicion para pruebas
                    _IdCliente = 4;
                    _IdUsuario = 2;
                    _IdFiltroEjecucion = 1;
                    _IdFormularioImpuesto = 1;
                    _IdDepartamento = 11;
                    _IdMunicipio = 474;
                    _IdEstadoDeclaracion = 2;
                    _IdEstado = 1;
                    _TipoProceso = 1;
                    _AnioGravable = 2018;
                    _DiaSemana = DateTime.Now.ToString("dddd").ToString().Trim().ToUpper();
                    #endregion
                }

                FixedData.LogApi.Warn("PROCESO A EJECUTAR => AMBIENTE = " + FixedData.AmbienteTask.Trim() + ", TAREA = " + _TipoProceso + ", DIA SEMANA = " + _DiaSemana + ", ID EMPRESA = " + _IdEmpresa + ", ID ESTADO = " + _IdEstado + ", TIPO TAREA = " + _IdTipoEnvio + ", AÑO = " + _AnioGravable);
                #endregion

                if (_TipoProceso == FixedData.TASK_LIQUIDACION_ICA_OFICINA)
                {
                    #region PROCESO DE LIQUIDACION DEL ICA POR OFICINA
                    Console.WriteLine("PROCESO LIQUIDACION ICA DE OFICINA");

                    //--AQUI OBTENEMOS EL DATATABLE RETORNADO EN LA LIQUIDACION
                    DataTable dtLiquidacion = new DataTable();
                    dtLiquidacion = GetLiquidacionOficXLote();
                    //--
                    if (dtLiquidacion != null)
                    {
                        if (dtLiquidacion.Rows.Count > 0)
                        {
                            int _AnioGravable = 0;
                            string _MesLiquidacion = "";
                            string _ArrayLiquidacion = "";
                            foreach (DataRow rowItem in dtLiquidacion.Rows)
                            {
                                #region OBTENER VALORES DEL DATATABLE PARA ENVIAR A LA DB
                                int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                int _IdFormularioImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                _MesLiquidacion = rowItem["mes_liquidacion"].ToString().Trim();
                                //--AQUI TOMAMOS LOS VALORES
                                double _ValorRenglon8 = Double.Parse(rowItem["valor_renglon8"].ToString().Trim());
                                double _ValorRenglon9 = Double.Parse(rowItem["valor_renglon9"].ToString().Trim());
                                double _ValorRenglon10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                double _ValorRenglon11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                double _ValorRenglon12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                double _ValorRenglon13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                double _ValorRenglon14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                double _ValorRenglon15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                double _ValorRenglon16 = Double.Parse(rowItem["valor_renglon16"].ToString().Trim());
                                double _ValorActividad1 = Double.Parse(rowItem["valor_actividad1"].ToString().Trim());
                                double _ValorActividad2 = Double.Parse(rowItem["valor_actividad2"].ToString().Trim());
                                double _ValorActividad3 = Double.Parse(rowItem["valor_actividad3"].ToString().Trim());
                                double _ValorOtrasAct = Double.Parse(rowItem["valor_otras_act"].ToString().Trim());
                                double _TotalIngresosGravado = Double.Parse(rowItem["total_ingresos_gravado"].ToString().Trim());
                                double _TotalImpuestos = Double.Parse(rowItem["total_impuestos"].ToString().Trim());
                                double _ValorRenglon17 = Double.Parse(rowItem["valor_renglon17"].ToString().Trim());
                                double _ValorRenglon18 = Double.Parse(rowItem["valor_renglon18"].ToString().Trim());
                                double _ValorRenglon19 = Double.Parse(rowItem["valor_renglon19"].ToString().Trim());
                                double _ValorRenglon20 = Double.Parse(rowItem["valor_renglon20"].ToString().Trim());
                                double _ValorRenglon21 = Double.Parse(rowItem["valor_renglon21"].ToString().Trim());
                                double _ValorRenglon22 = Double.Parse(rowItem["valor_renglon22"].ToString().Trim());
                                double _ValorRenglon23 = Double.Parse(rowItem["valor_renglon23"].ToString().Trim());
                                double _ValorRenglon24 = Double.Parse(rowItem["valor_renglon24"].ToString().Trim());
                                double _ValorRenglon25 = Double.Parse(rowItem["valor_renglon25"].ToString().Trim());
                                double _ValorRenglon26 = Double.Parse(rowItem["valor_renglon26"].ToString().Trim());
                                double _ValorRenglon27 = Double.Parse(rowItem["valor_renglon27"].ToString().Trim());
                                double _ValorRenglon28 = Double.Parse(rowItem["valor_renglon28"].ToString().Trim());
                                double _ValorRenglon29 = Double.Parse(rowItem["valor_renglon29"].ToString().Trim());
                                double _ValorRenglon30 = Double.Parse(rowItem["valor_renglon30"].ToString().Trim());
                                double _BaseGravBomberil = rowItem["base_grav_bomberil"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["base_grav_bomberil"].ToString().Trim()) : 0;
                                double _BaseGravSeguridad = rowItem["base_grav_seguridad"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["base_grav_seguridad"].ToString().Trim()) : 0;
                                double _Sanciones = rowItem["sanciones"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["sanciones"].ToString().Trim()) : 0;
                                string _DescSancionOtro = rowItem["descripcion_sancion_otro"].ToString().Trim();
                                double _ValorSancion = rowItem["valor_sancion"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_sancion"].ToString().Trim()) : 0;
                                double _ValorRenglon32 = rowItem["valor_renglon32"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon32"].ToString().Trim()) : 0;
                                double _ValorRenglon33 = rowItem["valor_renglon33"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon33"].ToString().Trim()) : 0;
                                double _ValorRenglon34 = rowItem["valor_renglon34"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon34"].ToString().Trim()) : 0;
                                double _ValorRenglon35 = rowItem["valor_renglon35"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon35"].ToString().Trim()) : 0;
                                double _ValorRenglon36 = rowItem["valor_renglon36"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon36"].ToString().Trim()) : 0;
                                double _InteresMora = rowItem["interes_mora"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["interes_mora"].ToString().Trim()) : 0;
                                double _ValorRenglon38 = rowItem["valor_renglon38"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon38"].ToString().Trim()) : 0;
                                double _ValorPagoVoluntario = rowItem["valor_pago_voluntario"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_pago_voluntario"].ToString().Trim()) : 0;
                                string _DestinoPagoVoluntario = rowItem["destino_pago_voluntario"].ToString().Trim();
                                double _ValorRenglon40 = rowItem["valor_renglon40"].ToString().Trim().Length > 0 ? Double.Parse(rowItem["valor_renglon40"].ToString().Trim()) : 0;
                                #endregion

                                #region AQUI CREAMOS EL ARRAY CON CADA UNO DE LOS VALORES DEL DATATABLE
                                //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                if (_ArrayLiquidacion.ToString().Trim().Length > 0)
                                {
                                    _ArrayLiquidacion = _ArrayLiquidacion.ToString().Trim() + "," + quote + "(" + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _MesLiquidacion + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," +
                                        _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorActividad1 + "," + _ValorActividad2 + "," + _ValorActividad3 + "," + _ValorOtrasAct + "," + _TotalIngresosGravado + "," + _TotalImpuestos + "," + _ValorRenglon17 + "," + _ValorRenglon18 + "," + _ValorRenglon19 + "," + _ValorRenglon20 + "," + _ValorRenglon21 + "," +
                                        _ValorRenglon22 + "," + _ValorRenglon23 + "," + _ValorRenglon24 + "," + _ValorRenglon25 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _BaseGravBomberil + "," + _BaseGravSeguridad + "," + _Sanciones + "," + _DescSancionOtro + "," + _ValorSancion + "," + _ValorRenglon32 + "," +
                                        _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _InteresMora + "," + _ValorRenglon38 + "," + _ValorPagoVoluntario + "," + _DestinoPagoVoluntario + "," + _ValorRenglon40 + ")" + quote;
                                }
                                else
                                {
                                    _ArrayLiquidacion = quote + "(" + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _MesLiquidacion + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," +
                                        _ValorActividad1 + "," + _ValorActividad2 + "," + _ValorActividad3 + "," + _ValorOtrasAct + "," + _TotalIngresosGravado + "," + _TotalImpuestos + "," + _ValorRenglon17 + "," + _ValorRenglon18 + "," + _ValorRenglon19 + "," + _ValorRenglon20 + "," + _ValorRenglon21 + "," + _ValorRenglon22 + "," + _ValorRenglon23 + "," +
                                         _ValorRenglon24 + "," + _ValorRenglon25 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _BaseGravBomberil + "," + _BaseGravSeguridad + "," + _Sanciones + "," + _DescSancionOtro + "," + _ValorSancion + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," +
                                          _ValorRenglon35 + "," + _ValorRenglon36 + "," + _InteresMora + "," + _ValorRenglon38 + "," + _ValorPagoVoluntario + "," + _DestinoPagoVoluntario + "," + _ValorRenglon40 + ")" + quote;
                                }
                                #endregion
                            }

                            if (_ArrayLiquidacion.ToString().Trim().Length > 0)
                            {
                                GetObtenerDatosDb objDatosDb = new GetObtenerDatosDb();
                                objDatosDb.IdFormularioImpuesto = _IdFormularioImpuesto;
                                objDatosDb.IdCliente = _IdCliente;
                                objDatosDb.AnioGravable = _AnioGravable;
                                //objDatosDb.MesLiquidacion = _MesLiquidacion;
                                objDatosDb.ArrayLiquidacion = _ArrayLiquidacion.ToString().Trim();
                                objDatosDb.IdUsuario = 1;
                                objDatosDb.MotorBaseDatos = FixedData.MotorBaseDatos;

                                int _IdRegistro = 0;
                                if (objDatosDb.AddLoadLiquidacionOficina(ref _IdRegistro, ref _MsgError))
                                {
                                    FixedData.LogApi.Error(_MsgError);
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    FixedData.LogApi.Error(_MsgError);

                                    ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                    ObjEmails.Asunto = "REF.: ERROR AL GUARDAR DATOS EN LA DB";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al guardar datos de la liquidación en la base de datos. Motivo: " + _MsgError + "</h4>" +
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
                        }
                    }
                    #endregion
                }
                else if (_TipoProceso == FixedData.TASK_LIQUIDACION_ICA_LOTE)
                {
                    #region PROCESO DE LIQUIDACION DEL ICA POR OFICINA
                    Console.WriteLine("PROCESO LIQUIDACION DEL ICA POR LOTE");

                    //--AQUI OBTENEMOS EL DATATABLE RETORNADO EN LA LIQUIDACION
                    DataTable dtLiquidacion = new DataTable();
                    dtLiquidacion = GetLiquidacionMasivaXLote();
                    //--
                    if (dtLiquidacion != null)
                    {
                        if (dtLiquidacion.Rows.Count > 0)
                        {
                            int _AnioGravable = 0;
                            string _ArrayLiquidacion = "";
                            foreach (DataRow rowItem in dtLiquidacion.Rows)
                            {
                                #region OBTENER VALORES DEL DATATABLE PARA ENVIAR A LA DB
                                int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                int _IdFormularioImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                string _FechaMaxPresentacion = rowItem["fecha_max_presentacion"].ToString().Trim();
                                string _FechaLiquidacion = rowItem["fecha_liquidacion"].ToString().Trim();
                                //--AQUI TOMAMOS LOS VALORES
                                int _PeriodoImpuesto = Int32.Parse(rowItem["periodo_impuesto"].ToString().Trim());
                                int _OpcionUso = Int32.Parse(rowItem["opcion_uso"].ToString().Trim());
                                string _NumDeclaracion = rowItem["num_declaracion"].ToString().Trim();
                                //--
                                double _ValorRenglon8 = Double.Parse(rowItem["valor_renglon8"].ToString().Trim());
                                double _ValorRenglon9 = Double.Parse(rowItem["valor_renglon9"].ToString().Trim());
                                double _ValorRenglon10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                double _ValorRenglon11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                double _ValorRenglon12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                double _ValorRenglon13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                double _ValorRenglon14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                double _ValorRenglon15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                double _ValorRenglon16 = Double.Parse(rowItem["valor_renglon16"].ToString().Trim());
                                double _ValorActividad1 = Double.Parse(rowItem["valor_actividad1"].ToString().Trim());
                                double _ValorActividad2 = Double.Parse(rowItem["valor_actividad2"].ToString().Trim());
                                double _ValorActividad3 = Double.Parse(rowItem["valor_actividad3"].ToString().Trim());
                                double _ValorOtrasAct = Double.Parse(rowItem["valor_otras_act"].ToString().Trim());
                                double _TotalIngresosGravado = Double.Parse(rowItem["total_ingresos_gravado"].ToString().Trim());
                                double _TotalImpuestos = Double.Parse(rowItem["total_impuestos"].ToString().Trim());
                                double _ValorRenglon17 = Double.Parse(rowItem["valor_renglon17"].ToString().Trim());
                                double _ValorRenglon18 = Double.Parse(rowItem["valor_renglon18"].ToString().Trim());
                                double _ValorRenglon19 = Double.Parse(rowItem["valor_renglon19"].ToString().Trim());
                                double _ValorRenglon20 = Double.Parse(rowItem["valor_renglon20"].ToString().Trim());
                                double _ValorRenglon21 = Double.Parse(rowItem["valor_renglon21"].ToString().Trim());
                                double _ValorRenglon22 = Double.Parse(rowItem["valor_renglon22"].ToString().Trim());
                                double _ValorRenglon23 = Double.Parse(rowItem["valor_renglon23"].ToString().Trim());
                                double _ValorRenglon24 = Double.Parse(rowItem["valor_renglon24"].ToString().Trim());
                                double _ValorRenglon25 = Double.Parse(rowItem["valor_renglon25"].ToString().Trim());
                                double _ValorRenglon26 = Double.Parse(rowItem["valor_renglon26"].ToString().Trim());
                                double _ValorRenglon27 = Double.Parse(rowItem["valor_renglon27"].ToString().Trim());
                                double _ValorRenglon28 = Double.Parse(rowItem["valor_renglon28"].ToString().Trim());
                                double _ValorRenglon29 = Double.Parse(rowItem["valor_renglon29"].ToString().Trim());
                                double _ValorRenglon30 = Double.Parse(rowItem["valor_renglon30"].ToString().Trim());
                                double _BaseGravBomberil = Double.Parse(rowItem["base_grav_bomberil"].ToString().Trim());
                                double _BaseGravSeguridad = Double.Parse(rowItem["base_grav_seguridad"].ToString().Trim());
                                double _Sanciones = Double.Parse(rowItem["sanciones"].ToString().Trim());
                                string _DescSancionOtro = rowItem["descripcion_sancion_otro"].ToString().Trim();
                                double _ValorSancion = Double.Parse(rowItem["valor_sancion"].ToString().Trim());
                                double _ValorRenglon32 = Double.Parse(rowItem["valor_renglon32"].ToString().Trim());
                                double _ValorRenglon33 = Double.Parse(rowItem["valor_renglon33"].ToString().Trim());
                                double _ValorRenglon34 = Double.Parse(rowItem["valor_renglon34"].ToString().Trim());
                                double _ValorRenglon35 = Double.Parse(rowItem["valor_renglon35"].ToString().Trim());
                                double _ValorRenglon36 = Double.Parse(rowItem["valor_renglon36"].ToString().Trim());
                                double _InteresMora = Double.Parse(rowItem["interes_mora"].ToString().Trim());
                                double _ValorRenglon38 = Double.Parse(rowItem["valor_renglon38"].ToString().Trim());
                                double _ValorPagoVoluntario = Double.Parse(rowItem["valor_pago_voluntario"].ToString().Trim());
                                string _DestinoPagoVoluntario = rowItem["destino_pago_voluntario"].ToString().Trim();
                                double _ValorRenglon40 = Double.Parse(rowItem["valor_renglon40"].ToString().Trim());
                                #endregion

                                #region AQUI CREAMOS EL ARRAY CON CADA UNO DE LOS VALORES DEL DATATABLE
                                //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                if (_ArrayLiquidacion.ToString().Trim().Length > 0)
                                {
                                    _ArrayLiquidacion = _ArrayLiquidacion.ToString().Trim() + "," + quote + "(" + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _FechaMaxPresentacion + "," + _FechaLiquidacion + "," + _PeriodoImpuesto + "," + _OpcionUso + "," + _NumDeclaracion + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," +
                                        _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorActividad1 + "," + _ValorActividad2 + "," + _ValorActividad3 + "," + _ValorOtrasAct + "," + _TotalIngresosGravado + "," + _TotalImpuestos + "," + _ValorRenglon17 + "," + _ValorRenglon18 + "," +
                                        _ValorRenglon19 + "," + _ValorRenglon20 + "," + _ValorRenglon21 + "," + _ValorRenglon22 + "," + _ValorRenglon23 + "," + _ValorRenglon24 + "," + _ValorRenglon25 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _BaseGravBomberil + "," + _BaseGravSeguridad + "," + _Sanciones + "," + _DescSancionOtro + "," +
                                        _ValorSancion + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _InteresMora + "," + _ValorRenglon38 + "," + _ValorPagoVoluntario + "," + _DestinoPagoVoluntario + "," + _ValorRenglon40 + ")" + quote;
                                }
                                else
                                {
                                    _ArrayLiquidacion = quote + "(" + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _FechaMaxPresentacion + "," + _FechaLiquidacion + "," + _PeriodoImpuesto + "," + _OpcionUso + "," + _NumDeclaracion + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," +
                                        _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorActividad1 + "," + _ValorActividad2 + "," + _ValorActividad3 + "," + _ValorOtrasAct + "," + _TotalIngresosGravado + "," + _TotalImpuestos + "," + _ValorRenglon17 + "," + _ValorRenglon18 + "," +
                                        _ValorRenglon19 + "," + _ValorRenglon20 + "," + _ValorRenglon21 + "," + _ValorRenglon22 + "," + _ValorRenglon23 + "," + _ValorRenglon24 + "," + _ValorRenglon25 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _BaseGravBomberil + "," + _BaseGravSeguridad + "," + _Sanciones + "," + _DescSancionOtro + "," +
                                        _ValorSancion + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _InteresMora + "," + _ValorRenglon38 + "," + _ValorPagoVoluntario + "," + _DestinoPagoVoluntario + "," + _ValorRenglon40 + ")" + quote;
                                }
                                #endregion
                            }

                            if (_ArrayLiquidacion.ToString().Trim().Length > 0)
                            {
                                GetObtenerDatosDb objDatosDb = new GetObtenerDatosDb();
                                objDatosDb.IdFormularioImpuesto = _IdFormularioImpuesto;
                                objDatosDb.IdCliente = _IdCliente;
                                objDatosDb.AnioGravable = _AnioGravable;
                                objDatosDb.ArrayLiquidacion = _ArrayLiquidacion.ToString().Trim();
                                objDatosDb.IdEstado = _IdEstadoDeclaracion;
                                objDatosDb.IdUsuario = 1;
                                objDatosDb.MotorBaseDatos = FixedData.MotorBaseDatos;

                                int _IdRegistro = 0;
                                if (objDatosDb.AddLoadLiquidacionMasivaXLote(ref _IdRegistro, ref _MsgError))
                                {
                                    FixedData.LogApi.Info(_MsgError);
                                    //--AQUI MANDAMOS A GENERAR LA LIQUIDACION EN PDF
                                    GenerarArchivoPdf(dtLiquidacion, _IdEstadoDeclaracion);
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    FixedData.LogApi.Error(_MsgError);

                                    ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                    ObjEmails.Asunto = "REF.: ERROR AL GUARDAR DATOS EN LA DB";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al guardar datos de la liquidación en la base de datos. Motivo: " + _MsgError + "</h4>" +
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
                        }
                    }
                    #endregion
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
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

        private static DataTable GetTablaDatos()
        {
            DataTable DtLiquidacionIca = new DataTable();
            try
            {
                #region DEFINIR COLUMNAS DEL DATATABLE
                //--DEFINIR COLUMNAS
                DtLiquidacionIca = new DataTable();
                DtLiquidacionIca.TableName = "DtLiquidacionIca";
                DtLiquidacionIca.Columns.Add("idliquid_impuesto", typeof(Int32));
                DtLiquidacionIca.PrimaryKey = new DataColumn[] { DtLiquidacionIca.Columns["idliquid_impuesto"] };
                DtLiquidacionIca.Columns.Add("id_municipio", typeof(Int32));
                DtLiquidacionIca.Columns.Add("idformulario_impuesto");
                DtLiquidacionIca.Columns.Add("id_cliente", typeof(Int32));
                DtLiquidacionIca.Columns.Add("idcliente_establecimiento", typeof(Int32));
                DtLiquidacionIca.Columns.Add("codigo_dane");
                DtLiquidacionIca.Columns.Add("anio_gravable", typeof(Int32));
                DtLiquidacionIca.Columns.Add("mes_liquidacion");
                DtLiquidacionIca.Columns.Add("valor_renglon8");
                DtLiquidacionIca.Columns.Add("valor_renglon9");
                DtLiquidacionIca.Columns.Add("valor_renglon10");
                DtLiquidacionIca.Columns.Add("valor_renglon11");
                DtLiquidacionIca.Columns.Add("valor_renglon12");
                DtLiquidacionIca.Columns.Add("valor_renglon13");
                DtLiquidacionIca.Columns.Add("valor_renglon14");
                DtLiquidacionIca.Columns.Add("valor_renglon15");
                DtLiquidacionIca.Columns.Add("valor_renglon16");
                DtLiquidacionIca.Columns.Add("valor_actividad1");
                DtLiquidacionIca.Columns.Add("valor_actividad2");
                DtLiquidacionIca.Columns.Add("valor_actividad3");
                DtLiquidacionIca.Columns.Add("valor_otras_act");
                DtLiquidacionIca.Columns.Add("total_ingresos_gravado");
                DtLiquidacionIca.Columns.Add("total_impuestos");
                DtLiquidacionIca.Columns.Add("valor_renglon17");
                DtLiquidacionIca.Columns.Add("valor_renglon18");
                DtLiquidacionIca.Columns.Add("valor_renglon19");
                DtLiquidacionIca.Columns.Add("valor_renglon20");
                DtLiquidacionIca.Columns.Add("valor_renglon21");
                DtLiquidacionIca.Columns.Add("valor_renglon22");
                DtLiquidacionIca.Columns.Add("valor_renglon23");
                DtLiquidacionIca.Columns.Add("valor_renglon24");
                DtLiquidacionIca.Columns.Add("valor_renglon25");
                DtLiquidacionIca.Columns.Add("valor_renglon26");
                DtLiquidacionIca.Columns.Add("valor_renglon27");
                DtLiquidacionIca.Columns.Add("valor_renglon28");
                DtLiquidacionIca.Columns.Add("valor_renglon29");
                DtLiquidacionIca.Columns.Add("valor_renglon30");
                DtLiquidacionIca.Columns.Add("tarifa_ica");
                DtLiquidacionIca.Columns.Add("base_grav_bomberil");
                DtLiquidacionIca.Columns.Add("base_grav_seguridad");
                DtLiquidacionIca.Columns.Add("sanciones");
                DtLiquidacionIca.Columns.Add("descripcion_sancion_otro");
                DtLiquidacionIca.Columns.Add("valor_sancion");
                DtLiquidacionIca.Columns.Add("valor_renglon32");
                DtLiquidacionIca.Columns.Add("valor_renglon33");
                DtLiquidacionIca.Columns.Add("valor_renglon34");
                DtLiquidacionIca.Columns.Add("valor_renglon35");
                DtLiquidacionIca.Columns.Add("valor_renglon36");
                DtLiquidacionIca.Columns.Add("interes_mora");
                DtLiquidacionIca.Columns.Add("valor_renglon38");
                DtLiquidacionIca.Columns.Add("valor_pago_voluntario");
                DtLiquidacionIca.Columns.Add("destino_pago_voluntario");
                DtLiquidacionIca.Columns.Add("valor_renglon40");
                #endregion
            }
            catch (Exception ex)
            {
                DtLiquidacionIca = null;
                FixedData.LogApi.Error("Error al generar el datatable de liquidación. Motivo: " + ex.Message);
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                ObjEmails.Asunto = "REF.: ERROR AL GENERAR LA TABLA LIQUIDACIÓN";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al generar la tabla de liquidación. Motivo: " + ex.Message + "</h4>" +
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

            return DtLiquidacionIca;
        }

        private static DataTable GetTablaDatosLiqLote()
        {
            DataTable DtLiquidacionIca = new DataTable();
            try
            {
                #region DEFINIR COLUMNAS DEL DATATABLE
                //--DEFINIR COLUMNAS
                DtLiquidacionIca = new DataTable();
                DtLiquidacionIca.TableName = "DtLiquidacionIca";
                DtLiquidacionIca.Columns.Add("idliquid_impuesto", typeof(Int32));
                DtLiquidacionIca.PrimaryKey = new DataColumn[] { DtLiquidacionIca.Columns["idliquid_impuesto"] };
                DtLiquidacionIca.Columns.Add("id_municipio", typeof(Int32));
                DtLiquidacionIca.Columns.Add("idformulario_impuesto");
                DtLiquidacionIca.Columns.Add("id_cliente", typeof(Int32));
                DtLiquidacionIca.Columns.Add("idcliente_establecimiento", typeof(Int32));
                DtLiquidacionIca.Columns.Add("nombre_oficina");
                DtLiquidacionIca.Columns.Add("codigo_dane");
                DtLiquidacionIca.Columns.Add("anio_gravable", typeof(Int32));
                //--INFO DEL MUNICIPIO Y DEL CLIENTE
                DtLiquidacionIca.Columns.Add("municipio_oficina");
                DtLiquidacionIca.Columns.Add("dpto_oficina");
                DtLiquidacionIca.Columns.Add("nombre_cliente");
                DtLiquidacionIca.Columns.Add("idtipo_identificacion");
                DtLiquidacionIca.Columns.Add("numero_documento");
                DtLiquidacionIca.Columns.Add("digito_verificacion");
                DtLiquidacionIca.Columns.Add("consorcio_union_temporal");
                DtLiquidacionIca.Columns.Add("actividad_patrim_autonomo");
                DtLiquidacionIca.Columns.Add("direccion_cliente");
                DtLiquidacionIca.Columns.Add("municipio_cliente");
                DtLiquidacionIca.Columns.Add("dpto_cliente");
                DtLiquidacionIca.Columns.Add("telefono_contacto");
                DtLiquidacionIca.Columns.Add("email_contacto");
                DtLiquidacionIca.Columns.Add("numero_puntos");
                DtLiquidacionIca.Columns.Add("tipo_clasificacion");
                //--DATOS DEL FORM ICA
                DtLiquidacionIca.Columns.Add("fecha_max_presentacion");
                DtLiquidacionIca.Columns.Add("fecha_liquidacion");
                DtLiquidacionIca.Columns.Add("periodo_impuesto");
                DtLiquidacionIca.Columns.Add("opcion_uso");
                DtLiquidacionIca.Columns.Add("num_declaracion");
                DtLiquidacionIca.Columns.Add("valor_renglon8");
                DtLiquidacionIca.Columns.Add("valor_renglon9");
                DtLiquidacionIca.Columns.Add("valor_renglon10");
                DtLiquidacionIca.Columns.Add("valor_renglon11");
                DtLiquidacionIca.Columns.Add("valor_renglon12");
                DtLiquidacionIca.Columns.Add("valor_renglon13");
                DtLiquidacionIca.Columns.Add("valor_renglon14");
                DtLiquidacionIca.Columns.Add("valor_renglon15");
                DtLiquidacionIca.Columns.Add("valor_renglon16");
                DtLiquidacionIca.Columns.Add("codigo_actividad1");
                DtLiquidacionIca.Columns.Add("valor_actividad1");
                DtLiquidacionIca.Columns.Add("tarifa_actividad1");
                DtLiquidacionIca.Columns.Add("codigo_actividad2");
                DtLiquidacionIca.Columns.Add("valor_actividad2");
                DtLiquidacionIca.Columns.Add("tarifa_actividad2");
                DtLiquidacionIca.Columns.Add("codigo_actividad3");
                DtLiquidacionIca.Columns.Add("valor_actividad3");
                DtLiquidacionIca.Columns.Add("tarifa_actividad3");
                DtLiquidacionIca.Columns.Add("valor_actividad4");
                DtLiquidacionIca.Columns.Add("tarifa_actividad4");
                DtLiquidacionIca.Columns.Add("valor_otras_act");
                DtLiquidacionIca.Columns.Add("total_ingresos_gravado");
                DtLiquidacionIca.Columns.Add("total_impuestos");
                DtLiquidacionIca.Columns.Add("valor_renglon17");
                DtLiquidacionIca.Columns.Add("valor_renglon18");
                DtLiquidacionIca.Columns.Add("valor_renglon19");
                DtLiquidacionIca.Columns.Add("valor_renglon20");
                DtLiquidacionIca.Columns.Add("valor_renglon21");
                DtLiquidacionIca.Columns.Add("valor_renglon22");
                DtLiquidacionIca.Columns.Add("valor_renglon23");
                DtLiquidacionIca.Columns.Add("valor_renglon24");
                DtLiquidacionIca.Columns.Add("valor_renglon25");
                DtLiquidacionIca.Columns.Add("valor_renglon26");
                DtLiquidacionIca.Columns.Add("valor_renglon27");
                DtLiquidacionIca.Columns.Add("valor_renglon28");
                DtLiquidacionIca.Columns.Add("valor_renglon29");
                DtLiquidacionIca.Columns.Add("valor_renglon30");
                DtLiquidacionIca.Columns.Add("tarifa_ica");
                DtLiquidacionIca.Columns.Add("base_grav_bomberil");
                DtLiquidacionIca.Columns.Add("base_grav_seguridad");
                DtLiquidacionIca.Columns.Add("sanciones");
                DtLiquidacionIca.Columns.Add("descripcion_sancion_otro");
                DtLiquidacionIca.Columns.Add("valor_sancion");
                DtLiquidacionIca.Columns.Add("valor_renglon32");
                DtLiquidacionIca.Columns.Add("valor_renglon33");
                DtLiquidacionIca.Columns.Add("valor_renglon34");
                DtLiquidacionIca.Columns.Add("valor_renglon35");
                DtLiquidacionIca.Columns.Add("valor_renglon36");
                DtLiquidacionIca.Columns.Add("interes_mora");
                DtLiquidacionIca.Columns.Add("valor_renglon38");
                DtLiquidacionIca.Columns.Add("valor_pago_voluntario");
                DtLiquidacionIca.Columns.Add("destino_pago_voluntario");
                DtLiquidacionIca.Columns.Add("valor_renglon40");
                #endregion
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                DtLiquidacionIca = null;
                FixedData.LogApi.Error("Error al generar el datatable de liquidación. Motivo: " + ex.Message);

                ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                ObjEmails.Asunto = "REF.: ERROR AL GENERAR LA TABLA LIQUIDACIÓN";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al generar la tabla de liquidación. Motivo: " + ex.Message + "</h4>" +
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

            return DtLiquidacionIca;
        }

        private static DataTable GetLiquidacionOficXLote()
        {
            //--AQUI CREAMOS EL DATATABLE DONDE SE GUARDA LA LIQUIDACIÓN DE LA OFICINA
            DataTable dtLiquidacionIca = new DataTable();
            dtLiquidacionIca = GetTablaDatos();
            try
            {
                #region REALIZAR EL PROCESO DE LIQUIDACION DEL CLIENTE
                //--AQUI INSTANCIAMOS EL OBJETO CLASE
                GetObtenerDatosDb objObtenerDatos = new GetObtenerDatosDb();
                objObtenerDatos.TipoConsulta = 1;
                objObtenerDatos.IdCliente = _IdCliente;
                objObtenerDatos.IdEstado = _IdEstado;
                objObtenerDatos.IdEmpresa = _IdEmpresa;
                objObtenerDatos.MotorBaseDatos = FixedData.MotorBaseDatos;
                string _MsgError = "";

                //--PASO 1: OBTENER DATOS DE ESTADOS FINANCIEROS CARGADOS AL SISTEMA
                DataTable dtDatosEf = new DataTable();
                dtDatosEf = objObtenerDatos.GetEstadosFinanSinProcesar(ref _MsgError);
                //--
                if (dtDatosEf != null)
                {
                    if (_MsgError.Trim().Length == 0)
                    {
                        if (dtDatosEf.Rows.Count > 0)
                        {
                            foreach (DataRow rowItemEf in dtDatosEf.Rows)
                            {
                                //--OBTENER VALORES DEL DATATABLE
                                int _IdClienteEf = Int32.Parse(rowItemEf["idcliente_ef"].ToString().Trim());
                                int _AnioGravable = Int32.Parse(rowItemEf["anio_gravable"].ToString().Trim());
                                string _MesEf = rowItemEf["mes_ef"].ToString().Trim();
                                _MsgError = "";
                                objObtenerDatos.TipoConsulta = 2;
                                //--PASO 2: OBTENER DATOS DE OFICINAS A REALIZAR EL PROCESO DE LIQUIDACIÓN
                                DataTable dtDatosOfic = new DataTable();
                                dtDatosOfic = objObtenerDatos.GetOficinasCliente(ref _MsgError);
                                //--
                                if (dtDatosOfic != null)
                                {
                                    if (_MsgError.Trim().Length == 0)
                                    {
                                        if (dtDatosOfic.Rows.Count > 0)
                                        {
                                            foreach (DataRow rowItemOfic in dtDatosOfic.Rows)
                                            {
                                                #region OBTENER DATOS DE LA OFICINA
                                                int _IdClienteEstablecimiento = Int32.Parse(rowItemOfic["idcliente_establecimiento"].ToString().Trim());
                                                int _IdMunicipio = Int32.Parse(rowItemOfic["id_municipio"].ToString().Trim());
                                                int _NumEstablecimiento = Int32.Parse(rowItemOfic["numero_puntos"].ToString().Trim());
                                                string _CodigoOficina = rowItemOfic["codigo_oficina"].ToString().Trim();
                                                string _CodigoDane = rowItemOfic["codigo_dane"].ToString().Trim();
                                                string _NombreOficina = rowItemOfic["nombre_oficina"].ToString().Trim();

                                                #region AQUI REGISTRAMOS EN EL DATATABLE LOS DATOS DE LA OFICINA A LIQUIDAR
                                                DataRow Fila = null;
                                                Fila = dtLiquidacionIca.NewRow();
                                                Fila["idliquid_impuesto"] = dtLiquidacionIca.Rows.Count + 1;
                                                Fila["id_municipio"] = _IdMunicipio;
                                                Fila["idformulario_impuesto"] = _IdFormularioImpuesto;
                                                Fila["id_cliente"] = _IdCliente;
                                                Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                                Fila["codigo_dane"] = _CodigoDane;
                                                Fila["anio_gravable"] = _AnioGravable;
                                                Fila["mes_liquidacion"] = _MesEf;
                                                Fila["valor_renglon8"] = 0;
                                                Fila["valor_renglon9"] = 0;
                                                Fila["valor_renglon10"] = 0;
                                                Fila["valor_renglon11"] = 0;
                                                Fila["valor_renglon12"] = 0;
                                                Fila["valor_renglon13"] = 0;
                                                Fila["valor_renglon14"] = 0;
                                                Fila["valor_renglon15"] = 0;
                                                Fila["valor_renglon16"] = 0;
                                                Fila["valor_actividad1"] = 0;
                                                Fila["valor_actividad2"] = 0;
                                                Fila["valor_actividad3"] = 0;
                                                Fila["valor_otras_act"] = 0;
                                                Fila["total_ingresos_gravado"] = 0;
                                                Fila["total_impuestos"] = 0;
                                                Fila["valor_renglon17"] = 0;
                                                Fila["valor_renglon18"] = 0;
                                                Fila["valor_renglon19"] = 0;
                                                Fila["valor_renglon20"] = 0;
                                                Fila["valor_renglon21"] = 0;
                                                Fila["valor_renglon22"] = 0;
                                                Fila["valor_renglon23"] = 0;
                                                Fila["valor_renglon24"] = 0;
                                                Fila["valor_renglon25"] = 0;
                                                Fila["valor_renglon26"] = 0;
                                                Fila["valor_renglon27"] = 0;
                                                Fila["valor_renglon28"] = 0;
                                                Fila["valor_renglon29"] = 0;
                                                Fila["valor_renglon30"] = 0;
                                                //Fila["tarifa_ica"] = 0;   //--EL VALOR DE ESTA COLUMNA ES IGUAL A LA COLUMNA total_ingresos_gravado                                                
                                                Fila["base_grav_bomberil"] = 0;
                                                Fila["base_grav_seguridad"] = 0;
                                                Fila["sanciones"] = 1;
                                                Fila["descripcion_sancion_otro"] = "NA";
                                                Fila["valor_sancion"] = 0;
                                                Fila["valor_renglon32"] = 0;
                                                Fila["valor_renglon33"] = 0;
                                                Fila["valor_renglon34"] = 0;
                                                Fila["valor_renglon35"] = 0;
                                                Fila["valor_renglon36"] = 0;
                                                Fila["interes_mora"] = 0;
                                                Fila["valor_renglon38"] = 0;
                                                Fila["valor_pago_voluntario"] = 0;
                                                Fila["destino_pago_voluntario"] = "NA";
                                                Fila["valor_renglon40"] = 0;
                                                dtLiquidacionIca.Rows.Add(Fila);
                                                #endregion

                                                #region AQUI REALIZAMOS EL PROCESO DE LIQUIDACION DE LA OFICINA
                                                //--PASO 3: OBTENER DATOS DE LA BASE GRAVABLE
                                                objObtenerDatos.TipoConsulta = 2;
                                                objObtenerDatos.IdCliente = _IdCliente;
                                                objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;
                                                objObtenerDatos.IdFormConfiguracion = null;
                                                objObtenerDatos.IdPuc = null;

                                                //--AQUI OBTENEMOS LOS DATOS DEL ESTABLECIMIENTO EN LIQUIDACION DEL IMPUESTO
                                                DataRow[] dataRows = dtLiquidacionIca.Select("idcliente_establecimiento = " + _IdClienteEstablecimiento + " AND TRIM(mes_liquidacion) = '" + _MesEf + "'");
                                                if (dataRows.Length == 1)
                                                {
                                                    #region OBTENEMOS LOS VALORES DE LA SESION (B. BASE GRAVABLE)
                                                    DataTable dtBaseGravable = new DataTable();
                                                    dtBaseGravable = objObtenerDatos.GetBaseGravable();
                                                    double _ValorRenglon16 = 0;
                                                    int _ContadorRow = 0;
                                                    //--
                                                    if (dtBaseGravable != null)
                                                    {
                                                        if (dtBaseGravable.Rows.Count > 0)
                                                        {
                                                            #region AQUI RECORREMOS LAS BASE GRAVABLE CONFIGURADAS POR EL CLIENTE
                                                            foreach (DataRow rowItemBg in dtBaseGravable.Rows)
                                                            {
                                                                #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                                                _ContadorRow++;
                                                                int _NumRenglon = Int32.Parse(rowItemBg["numero_renglon"].ToString().Trim());
                                                                string _CodigoCuenta = rowItemBg["codigo_cuenta"].ToString().Trim();
                                                                string _SaldoInicial = rowItemBg["saldo_inicial"].ToString().Trim();
                                                                string _MovDebito = rowItemBg["mov_debito"].ToString().Trim();
                                                                string _MovCredito = rowItemBg["mov_credito"].ToString().Trim();
                                                                string _SaldoFinal = rowItemBg["saldo_final"].ToString().Trim();
                                                                string _ValorExtracontable1 = rowItemBg["valor_extracontable"].ToString().Trim().Replace("-", "");
                                                                double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                                                //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                                                objObtenerDatos.TipoConsulta = 1;
                                                                objObtenerDatos.NumeroRenglon = _NumRenglon;
                                                                //objObtenerDatos.IdCliente = _IdCliente;
                                                                objObtenerDatos.AnioGravable = _AnioGravable;
                                                                //objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                                objObtenerDatos.SaldoInicial = _SaldoInicial;
                                                                objObtenerDatos.MovDebito = _MovDebito;
                                                                objObtenerDatos.MovCredito = _MovCredito;
                                                                objObtenerDatos.SaldoFinal = _SaldoFinal;
                                                                objObtenerDatos.CodigoCuenta = _CodigoCuenta;
                                                                objObtenerDatos.MesEf = _MesEf;

                                                                //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                                                double _ValorTotal = 0;
                                                                if (_SaldoInicial == "S" || _MovDebito == "S" ||
                                                                    _MovCredito == "S" || _SaldoFinal == "S")
                                                                {
                                                                    List<string> _ArrayDatos = objObtenerDatos.GetEstadoFinanciero();
                                                                    if (_ArrayDatos.Count > 0)
                                                                    {
                                                                        string _ValorCuenta = _ArrayDatos[1].ToString().Trim();
                                                                        _ValorTotal = (Double.Parse(_ValorCuenta) + _ValorExtracontable);
                                                                        FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    _ValorTotal = _ValorExtracontable;
                                                                }
                                                                #endregion

                                                                #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                                                double _SumValorRenglon = 0;
                                                                switch (_NumRenglon)
                                                                {
                                                                    case 8:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon8"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    //case 9:
                                                                    //    _SumValorRenglon = Double.Parse(this.LblValorRenglon9.Text.ToString().Trim().Replace("$ ", "").Replace(".", "")) + _ValorTotal;
                                                                    //    this.LblValorRenglon9.Text = String.Format(String.Format("{0:###,###,##0}", _SumValorRenglon));
                                                                    //    break;
                                                                    case 10:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon10"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 11:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon11"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 12:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon12"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 13:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon13"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 14:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon14"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 15:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon15"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 26:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon26"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 27:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon27"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 28:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon28"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 29:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon29"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 30:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon30"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 31:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["sanciones"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["sanciones"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 32:
                                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim()) + _ValorTotal;
                                                                        dataRows[0]["valor_renglon32"] = round(_SumValorRenglon);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                #endregion
                                                            }

                                                            #region AQUI CALCULAMOS EL VALOR DEL RENGLON 9 Y 16
                                                            //--AQUI CALCULAMOS EL VALOR DEL CAMPO 9
                                                            double _ValorRenglon8 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                            double _ValorRenglon10 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                            double _ValorRenglon9 = (_ValorRenglon8 - _ValorRenglon10);
                                                            //--
                                                            double _ValorRenglon11 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                            double _ValorRenglon12 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                            double _ValorRenglon13 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                            double _ValorRenglon14 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                            double _ValorRenglon15 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                            double _SumRenglones = (_ValorRenglon11 - _ValorRenglon12 - _ValorRenglon13 - _ValorRenglon14 - _ValorRenglon15);
                                                            _ValorRenglon16 = (_ValorRenglon10 - Double.Parse(_SumRenglones.ToString().Replace("-", "")));
                                                            //--
                                                            dataRows[0]["valor_renglon9"] = _ValorRenglon9;
                                                            dataRows[0]["valor_renglon16"] = round(_ValorRenglon16);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion

                                                            //--FIN DEL CALCULO DE LA BASE GRAVABLE
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                            ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                                            ObjEmails.Asunto = "REF.: ERROR AL OBTENER LA BASE GRAVABLE";

                                                            string nHora = DateTime.Now.ToString("HH");
                                                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                            StringBuilder strDetalleEmail = new StringBuilder();
                                                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: No se obtuvo información de la base gravable del Id_Cliente " + _IdCliente + " y Id_Oficina " + _IdClienteEstablecimiento + "</h4>" +
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
                                                            return null;
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER LA BASE GRAVABLE";

                                                        string nHora = DateTime.Now.ToString("HH");
                                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                        StringBuilder strDetalleEmail = new StringBuilder();
                                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: Error al obtener información de la base gravable del Id_Cliente " + _IdCliente + " y Id_Oficina " + _IdClienteEstablecimiento + "</h4>" +
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
                                                        return null;
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region OBTENEMOS LOS VALORES DE LA SESION (C. DISCRIMINACION DE ACTIVIDADES ECONOMICAS)
                                                    objObtenerDatos.TipoConsulta = 1;
                                                    objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;
                                                    objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;

                                                    DataTable dtActEconomica = new DataTable();
                                                    dtActEconomica = objObtenerDatos.GetConsultarActEconomica();
                                                    //this.ViewState["dtActEconomica"] = dtActEconomica;
                                                    double _TotalImpuesto = 0;

                                                    if (dtActEconomica != null)
                                                    {
                                                        if (dtActEconomica.Rows.Count > 0)
                                                        {
                                                            _ContadorRow = 1;
                                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                                            #region AQUI RECORREMOS EL DATATABLE
                                                            foreach (DataRow rowItem in dtActEconomica.Rows)
                                                            {
                                                                #region AQUI DEFINIMOS LOS VALORES DE CADA DE LAS 3 ACTIVIDADES DEL FORMULARIO
                                                                string _CodigoActividad = rowItem["codigo_actividad"].ToString().Trim();
                                                                int _IdCalcularTarifaPor = Int32.Parse(rowItem["idtipo_calculo"].ToString().Trim());
                                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                                double _TarifaLey = Double.Parse(rowItem["tarifa_ley"].ToString().Trim());
                                                                double _TarifaMunicipio = Double.Parse(rowItem["tarifa_municipio"].ToString().Trim());
                                                                string _SaldoFinal1 = rowItem["saldo_final"].ToString().Trim().Replace("-", "");
                                                                double _SaldoFinal = Double.Parse(_SaldoFinal1);
                                                                double _ValorActividad = 0;

                                                                switch (_ContadorRow)
                                                                {
                                                                    case 1:
                                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 1
                                                                        double _DefValorActividad = _SaldoFinal > 0 ? _SaldoFinal : _ValorRenglon16;

                                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_DefValorActividad * _TarifaLey) / 100);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 100);
                                                                            }

                                                                            dataRows[0]["valor_actividad1"] = round(_DefValorActividad);
                                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_DefValorActividad * _TarifaLey) / 1000);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                //_ValorActividad = (_DefValorActividad * _TarifaMunicipio);
                                                                                _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 1000);
                                                                            }

                                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = (_DefValorActividad * _TarifaLey);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = (_DefValorActividad * _TarifaMunicipio);
                                                                            }

                                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        break;
                                                                    #endregion

                                                                    case 2:
                                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 2
                                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 100);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                            }

                                                                            dataRows[0]["valor_actividad2"] = round(_SaldoFinal);
                                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 1000);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                            }

                                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaLey);
                                                                            }
                                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                            }

                                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        break;
                                                                    #endregion

                                                                    case 3:
                                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 3
                                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 100);
                                                                            }
                                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                            }

                                                                            dataRows[0]["valor_actividad3"] = round(_SaldoFinal);
                                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 1000);
                                                                            }
                                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                            }

                                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                                        {
                                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaLey);
                                                                            }
                                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                            {
                                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                            }

                                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        break;
                                                                    #endregion

                                                                    default:
                                                                        break;
                                                                }
                                                                _ContadorRow++;

                                                                #region AQUI CALCULAMOS LOS INGRESOS NO GRAVADOS Y TOTAL DEL IMPUESTO
                                                                //--AQUI CALCULAMOS LOS INGRESOS NO GRAVADOS
                                                                double _ValorIngAct1 = Double.Parse(dataRows[0]["valor_actividad1"].ToString().Trim());
                                                                double _ValorIngAct2 = Double.Parse(dataRows[0]["valor_actividad2"].ToString().Trim());
                                                                double _ValorIngAct3 = Double.Parse(dataRows[0]["valor_actividad3"].ToString().Trim());
                                                                double _TotalIngresosGravados = (_ValorIngAct1 + _ValorIngAct2 + _ValorIngAct3);

                                                                //--AQUI CALCULAMOS EL TOTAL DEL IMPUESTO
                                                                double _ValorRenglon17 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                double _ValorRenglon18 = Double.Parse(dataRows[0]["valor_renglon18"].ToString().Trim());
                                                                double _ValorRenglon19 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                double _ValorDesagregacion = 0;
                                                                _TotalImpuesto = (_ValorRenglon17 + _ValorRenglon18 + _ValorRenglon19 + _ValorDesagregacion);

                                                                //--ACTUALIZAR VALORES
                                                                dataRows[0]["total_ingresos_gravado"] = round(_TotalIngresosGravados);
                                                                dataRows[0]["total_impuestos"] = round(_TotalImpuesto);
                                                                dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                dtLiquidacionIca.Rows[0].EndEdit();
                                                                #endregion
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER ACTIVIDADES ECONOMICAS";

                                                        string nHora = DateTime.Now.ToString("HH");
                                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                        StringBuilder strDetalleEmail = new StringBuilder();
                                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener las actividades economicas de la oficina." + "</h4>" +
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
                                                        return null;
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region AQUI CALCULAMOS LOS VALORES DE LA SESION (D. LIQUIDACION PRIVADA)
                                                    _TotalImpuesto = Double.Parse(dataRows[0]["total_impuestos"].ToString().Trim());
                                                    double _TotalImpuestosLey = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                    double _ValorRenglon20 = (_TotalImpuesto + _TotalImpuestosLey);
                                                    //--
                                                    dataRows[0]["valor_renglon20"] = round(_ValorRenglon20);
                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                    dtLiquidacionIca.Rows[0].EndEdit();

                                                    objObtenerDatos.IdMunicipio = _IdMunicipio;
                                                    objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;

                                                    DataTable dtImpMunicipio = new DataTable();
                                                    dtImpMunicipio = objObtenerDatos.GetImpuestosMunicipio();
                                                    double _ValorLiquidacion = 0;

                                                    if (dtImpMunicipio != null)
                                                    {
                                                        if (dtImpMunicipio.Rows.Count > 0)
                                                        {
                                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                                            #region AQUI RECORREMOS EL DATATABLE
                                                            foreach (DataRow rowItem in dtImpMunicipio.Rows)
                                                            {
                                                                #region OBTENER DATOS DEL DATATABLE Y CALCULAR VALOR DE RENGLONES
                                                                int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                                                string _DescripcionRenglon = rowItem["descripcion_renglon"].ToString().Trim();
                                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                                double _ValorTarifa = Double.Parse(rowItem["valor_tarifa"].ToString().Trim());
                                                                dataRows[0]["base_grav_bomberil"] = round(_ValorRenglon20);
                                                                dataRows[0]["base_grav_seguridad"] = round(_ValorRenglon20);

                                                                //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                if (_IdTipoTarifa == 1)     //--TARIFA PORCENTUAL
                                                                {
                                                                    _ValorLiquidacion = ((_ValorRenglon20 * _ValorTarifa) / 100);
                                                                }
                                                                else if (_IdTipoTarifa == 2)    //--TARIFA POR MIL
                                                                {
                                                                    _ValorLiquidacion = ((_ValorRenglon20 * _ValorTarifa) / 1000);
                                                                }

                                                                #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                                switch (_NumRenglon)
                                                                {
                                                                    case 21:
                                                                        dataRows[0]["valor_renglon21"] = round(_ValorLiquidacion);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    //case 22:
                                                                    //    this.LblValorRenglon22.Text = String.Format(String.Format("{0:###,###,##0}", _ValorLiquidacion));
                                                                    //    break;
                                                                    case 23:
                                                                        dataRows[0]["valor_renglon23"] = round(_ValorLiquidacion);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    case 24:
                                                                        dataRows[0]["valor_renglon24"] = round(_ValorLiquidacion);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                    //case 31:
                                                                    //    this.TxtValorRenglon31.Text = String.Format(String.Format("{0:###,###,##0}", _ValorLiquidacion));
                                                                    //    break;
                                                                    default:
                                                                        break;
                                                                }
                                                                #endregion
                                                                //--
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER IMPUESTOS MUNICIPIOS";

                                                        string nHora = DateTime.Now.ToString("HH");
                                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                        StringBuilder strDetalleEmail = new StringBuilder();
                                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los impuestos del id municipio " + _IdMunicipio + "</h4>" +
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
                                                        return null;
                                                        #endregion
                                                    }
                                                    #endregion

                                                    ///---------------
                                                    #region AQUI CALCULAMOS EL VALOR DEL RENGLON 25 (TOTAL DE IMPUESTO A CARGO)
                                                    _ValorRenglon20 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                    double _ValorRenglon21 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                    double _ValorRenglon22 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                    double _ValorRenglon23 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                    double _ValorRenglon24 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                    double _ValorRenglon25 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                    //this.LblValorRenglon25.Text = String.Format(String.Format("{0:###,###,##0}", round(_ValorRenglon25)));
                                                    #endregion

                                                    #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                                    //--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                                    double _ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                    double _ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                    double _ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                    double _ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                    double _ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                    double _ValorRenglon31 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                    double _ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());

                                                    //double _TotalSaldoCargo = (_ValorRenglon25 - (_ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + (_ValorRenglon30 + _ValorRenglon31) + _ValorRenglon32);
                                                    double _TotalSaldoCargoAux = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                                    double _TotalSaldoCargo = _TotalSaldoCargoAux >= 0 ? _TotalSaldoCargoAux : 0;
                                                    double _TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                                    //--
                                                    dataRows[0]["valor_renglon33"] = round(_TotalSaldoCargo);
                                                    dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                                    dataRows[0]["valor_renglon35"] = round(_TotalSaldoCargo);
                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                    #endregion

                                                    #region OBTENEMOS LOS VALORES DE TARIFAS MINIMAS DEL MUNICIPIO
                                                    objObtenerDatos.AnioGravable = _AnioGravable;
                                                    objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;
                                                    objObtenerDatos.IdMunicipio = _IdMunicipio;
                                                    //--
                                                    DataTable dtTarifasMin = new DataTable();
                                                    dtTarifasMin = objObtenerDatos.GetTarifaMinMunicipio();
                                                    double _OperacionRenglon36 = 0, _TotalTarifaMinima = 0;

                                                    if (dtTarifasMin != null)
                                                    {
                                                        if (dtTarifasMin.Rows.Count > 0)
                                                        {
                                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                                            #region AQUI RECORREMOS EL DATATABLE
                                                            foreach (DataRow rowItem in dtTarifasMin.Rows)
                                                            {
                                                                #region AQUI OBNTENEMOS LOS VALORES DE LA PARAMETRIZACION DE OTRAS CONFIGURACIONES
                                                                string _NumeroRenglon = rowItem["numero_renglon"].ToString().Trim();
                                                                string _CalcularRenglon = rowItem["calcular_renglon"].ToString().Trim();
                                                                string _NumeroRenglon1 = rowItem["numero_renglon1"].ToString().Trim();
                                                                string _IdTipoOperacion = rowItem["idtipo_operacion"].ToString().Trim();
                                                                string _NumeroRenglon2 = rowItem["numero_renglon2"].ToString().Trim();

                                                                int _IdUnidadMedida = Int32.Parse(rowItem["idunidad_medida"].ToString().Trim());
                                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                                double _ValorUnidad = Double.Parse(rowItem["valor_unidad"].ToString().Trim());
                                                                double _CantidadMedida = Double.Parse(rowItem["cantidad_medida"].ToString().Trim());
                                                                #endregion

                                                                #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                                double _ValorTarifaMinima = 0;
                                                                double _OperacionRenglon = 0, _ValorNumRenglon1 = 0, _ValorNumRenglon2 = 0;
                                                                //--
                                                                #region OBTENER EL VALOR A CALCULAR DEL RENGLON 1
                                                                switch (Int32.Parse(_NumeroRenglon))
                                                                {
                                                                    ///--LOS RENGLONES 8, 9 Y 10 NO LO TENEMOS EN CUENTA PORQUE SON LOS INGRESOS PAIS Y DE LA OFICINA
                                                                    case 11:
                                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                        if (_CalcularRenglon.Equals("S"))
                                                                        {
                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon1 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon2 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                            if (_TipoOperacion == 1)
                                                                            {
                                                                                //_SumValorRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 2)
                                                                            {

                                                                            }
                                                                            else if (_TipoOperacion == 3)
                                                                            {

                                                                            }
                                                                            else if (_TipoOperacion == 4)
                                                                            {

                                                                            }
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            _ValorNumRenglon1 = 0;
                                                                        }
                                                                        #endregion
                                                                        break;
                                                                    case 12:
                                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                        if (_CalcularRenglon.Equals("S"))
                                                                        {
                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon1 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            _ValorNumRenglon1 = 0;
                                                                        }
                                                                        #endregion
                                                                        break;
                                                                    case 13:
                                                                        break;
                                                                    case 14:
                                                                        break;
                                                                    case 15:
                                                                        break;
                                                                    case 22:
                                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                        if (_CalcularRenglon.Equals("S"))
                                                                        {
                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon1 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon2 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                            if (_TipoOperacion == 1)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 2)
                                                                            {

                                                                            }
                                                                            else if (_TipoOperacion == 3)
                                                                            {

                                                                            }
                                                                            else if (_TipoOperacion == 4)
                                                                            {

                                                                            }
                                                                            else
                                                                            {

                                                                            }
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            //int _NumEstablecimiento = Int32.Parse(this.LblNumEstablecimientos.Text.ToString().Trim().Replace(".", ""));
                                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                            {
                                                                                _ValorTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                                            }
                                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                            {
                                                                                _ValorTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                                            }

                                                                            //--
                                                                            double _SumatoriaRenglon22 = 0;
                                                                            if (_NumEstablecimiento <= 1)
                                                                            {
                                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * _NumEstablecimiento);
                                                                            }
                                                                            else
                                                                            {
                                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * (_NumEstablecimiento - 1));  //--LE RESTAMOS UN ESTABLECIMIENTO
                                                                            }
                                                                            //--
                                                                            dataRows[0]["valor_renglon22"] = round(_SumatoriaRenglon22);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                        }
                                                                        #endregion

                                                                        #region AQUI CALCULAMOS EL VALOR DEL RENGLON 25 (TOTAL DE IMPUESTO A CARGO)
                                                                        _ValorRenglon20 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                        _ValorRenglon21 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                        _ValorRenglon22 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                        _ValorRenglon23 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                        _ValorRenglon24 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                        _ValorRenglon25 = (_ValorRenglon20 + _ValorRenglon21 + _ValorRenglon22 + _ValorRenglon23 + _ValorRenglon24);
                                                                        //--
                                                                        dataRows[0]["valor_renglon25"] = round(_ValorRenglon25);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        #endregion
                                                                        break;
                                                                    case 26:
                                                                        break;
                                                                    case 27:
                                                                        break;
                                                                    case 28:
                                                                        break;
                                                                    case 29:
                                                                        break;
                                                                    case 30:
                                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                        double _OperacionRenglon30 = 0;
                                                                        if (_CalcularRenglon.Equals("S"))
                                                                        {
                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                case 25:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                    break;
                                                                                case 26:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                    break;
                                                                                case 27:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                    break;
                                                                                case 28:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                    break;
                                                                                case 29:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon1 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                case 25:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                    break;
                                                                                case 26:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                    break;
                                                                                case 27:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                    break;
                                                                                case 28:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                    break;
                                                                                case 29:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon2 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                            if (_TipoOperacion == 1)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 2)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 3)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 4)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                                            }
                                                                            else
                                                                            {
                                                                                _OperacionRenglon = 0;
                                                                            }

                                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                            {
                                                                                _OperacionRenglon30 = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                                            }
                                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                            {
                                                                                _OperacionRenglon30 = (_OperacionRenglon * _CantidadMedida);
                                                                            }
                                                                            //--
                                                                            dataRows[0]["valor_renglon30"] = round(_OperacionRenglon30);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            //int _NumEstablecimiento = Int32.Parse(this.LblNumEstablecimientos.Text.ToString().Trim().Replace(".", ""));
                                                                            //--
                                                                            double _SumatoriaRenglon22 = 0;
                                                                            if (_NumEstablecimiento <= 1)
                                                                            {
                                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * _NumEstablecimiento);
                                                                            }
                                                                            else
                                                                            {
                                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * (_NumEstablecimiento - 1));  //--LE RESTAMOS UN ESTABLECIMIENTO
                                                                            }
                                                                            //--
                                                                            dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                        }
                                                                        #endregion

                                                                        #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                                                        //--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                                                        _ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                        _ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _ValorRenglon31 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                        _TotalSaldoCargo = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                                                        _TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                                                        //--
                                                                        dataRows[0]["valor_renglon33"] = round(_TotalSaldoCargo);
                                                                        dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                                                        dataRows[0]["valor_renglon35"] = round(_TotalSaldoCargo);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        #endregion
                                                                        break;
                                                                    case 31:
                                                                        break;
                                                                    case 32:
                                                                        break;
                                                                    case 36:
                                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                        _OperacionRenglon36 = 0;
                                                                        if (_CalcularRenglon.Equals("S"))
                                                                        {
                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                case 25:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                    break;
                                                                                case 26:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                    break;
                                                                                case 27:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                    break;
                                                                                case 28:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                    break;
                                                                                case 29:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon1 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                            //--
                                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                                            {
                                                                                case 8:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                    break;
                                                                                case 9:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                    break;
                                                                                case 10:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                    break;
                                                                                case 11:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                    break;
                                                                                case 12:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                    break;
                                                                                case 13:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                    break;
                                                                                case 14:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                    break;
                                                                                case 15:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                    break;
                                                                                case 16:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                    break;
                                                                                case 20:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                    break;
                                                                                case 21:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                    break;
                                                                                case 25:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                    break;
                                                                                case 26:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                    break;
                                                                                case 27:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                    break;
                                                                                case 28:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                    break;
                                                                                case 29:
                                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                    break;
                                                                                default:
                                                                                    _ValorNumRenglon2 = 0;
                                                                                    break;
                                                                            }
                                                                            #endregion

                                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                            if (_TipoOperacion == 1)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 2)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 3)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                                            }
                                                                            else if (_TipoOperacion == 4)
                                                                            {
                                                                                _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                                            }
                                                                            else
                                                                            {
                                                                                _OperacionRenglon = 0;
                                                                            }

                                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                            {
                                                                                _OperacionRenglon36 = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                                            }
                                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                            {
                                                                                _OperacionRenglon36 = (_OperacionRenglon * _CantidadMedida);
                                                                            }
                                                                            //--
                                                                            dataRows[0]["valor_renglon36"] = round(_OperacionRenglon36);
                                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                                            #endregion
                                                                        }
                                                                        #endregion

                                                                        #region AQUI CALCULAMOS EL VALOR A PAGAR
                                                                        //double _ValorRengln35 = 0;
                                                                        //_ValorRengln35 = Double.Parse(this.LblValorRenglon35.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));

                                                                        ////--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                        //if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                        //{
                                                                        //    _OperacionRenglon36 = ((_ValorRengln35 * _CantidadMedida) / 100);
                                                                        //}
                                                                        //else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                        //{
                                                                        //    _OperacionRenglon36 = (_ValorRengln35 * _CantidadMedida);
                                                                        //}

                                                                        //this.LblValorRenglon36.Text = String.Format(String.Format("{0:###,###,##0}", round(_OperacionRenglon36)));
                                                                        #endregion
                                                                        break;
                                                                    case 40:
                                                                        #region AQUI CALCULAMOS EL VALOR DE LA TARIFA MINIMA A PAGAR SI EL TOTAL A PAGAR ES CERO
                                                                        //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                        if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                        {
                                                                            _TotalTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                                        }
                                                                        else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                        {
                                                                            _TotalTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                                        }
                                                                        #endregion
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                #endregion

                                                                #endregion

                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER TARIFAS  MINIMAS";

                                                        string nHora = DateTime.Now.ToString("HH");
                                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                        StringBuilder strDetalleEmail = new StringBuilder();
                                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener las tarifas minimas del id municipio " + _IdMunicipio + "</h4>" +
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
                                                        return null;
                                                        #endregion
                                                    }
                                                    #endregion

                                                    #region CALCULAR RENGLONES 36, 37, 38 y 39
                                                    double _ValorRenglon35 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                    double _ValorRenglon36 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                    double _ValorRenglon37 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                    double _ValorRenglon38 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                    double _ValorRenglon39 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                    double _ValorRenglon40 = Double.Parse(dataRows[0]["valor_renglon40"].ToString().Trim());
                                                    double _TotalPagarVoluntario = (_ValorRenglon38 + _ValorRenglon39);
                                                    double _TotalPagar = (_ValorRenglon35 - _ValorRenglon36 + _ValorRenglon37);
                                                    //--
                                                    if (_ValorRenglon40 > 0)
                                                    {
                                                        dataRows[0]["valor_renglon40"] = _TotalPagarVoluntario;
                                                    }
                                                    else
                                                    {
                                                        dataRows[0]["valor_renglon40"] = _TotalTarifaMinima;
                                                    }
                                                    dataRows[0]["valor_renglon38"] = round(_TotalPagar);
                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                    #endregion
                                                }
                                                #endregion
                                                //--
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                            ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                            ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINA";

                                            string nHora = DateTime.Now.ToString("HH");
                                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                            StringBuilder strDetalleEmail = new StringBuilder();
                                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: el id cliente [" + _IdCliente + "] no tiene asociada oficinas." + "</h4>" +
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
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINA";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: " + _MsgError + "</h4>" +
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
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINA";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: No se obtuvo información de oficinas del cliente " + _IdCliente + "</h4>" +
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
                        }
                        else
                        {
                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                            ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                            ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: el id cliente [" + _IdCliente + "] no tiene asociada oficinas." + "</h4>" +
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
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: " + _MsgError + "</h4>" +
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
                else
                {
                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                    ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: No se obtuvo información de estados financieros cargados al idcliente " + _IdCliente + "</h4>" +
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
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                ObjEmails.Asunto = "REF.: ERROR PROCESO LIQUIDACION x LOTE";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al realizar el proceso de liquidación de la oficina. Motivo: " + ex.Message + "</h4>" +
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
                dtLiquidacionIca = null;
                #endregion
            }

            return dtLiquidacionIca;
        }

        private static DataTable GetLiquidacionMasivaXLote()
        {
            //--AQUI CREAMOS EL DATATABLE DONDE SE GUARDA LA LIQUIDACIÓN DE LA OFICINA
            DataTable dtLiquidacionIca = new DataTable();
            dtLiquidacionIca = GetTablaDatosLiqLote();
            try
            {
                #region REALIZAR EL PROCESO DE LIQUIDACION DEL CLIENTE
                //--AQUI INSTANCIAMOS EL OBJETO CLASE
                GetObtenerDatosDb objObtenerDatos = new GetObtenerDatosDb();
                //objObtenerDatos.TipoConsulta = 1;
                objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;
                objObtenerDatos.IdCliente = _IdCliente;
                objObtenerDatos.IdDepartamento = _IdDepartamento;
                objObtenerDatos.IdMunicipio = _IdMunicipio > 0 ? _IdMunicipio.ToString() : null;
                objObtenerDatos.MotorBaseDatos = FixedData.MotorBaseDatos;
                string _MsgError = "";

                //--PASO 1: OBTENER DATOS DE ESTADOS FINANCIEROS CARGADOS AL SISTEMA
                DataTable dtCliente = new DataTable();
                dtCliente = objObtenerDatos.GetConsultarDatos(ref _MsgError);
                //--
                if (dtCliente != null)
                {
                    if (_MsgError.Trim().Length == 0)
                    {
                        if (dtCliente.Rows.Count > 0)
                        {
                            foreach (DataRow rowItemOfic in dtCliente.Rows)
                            {
                                #region OBTENER DATOS DE LA OFICINA
                                int _IdClienteEstablecimiento = Int32.Parse(rowItemOfic["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowItemOfic["idmun_oficina"].ToString().Trim());
                                int _NumEstablecimiento = Int32.Parse(rowItemOfic["numero_puntos"].ToString().Trim());
                                //string _CodigoOficina = rowItemOfic["codigo_oficina"].ToString().Trim();
                                string _CodigoDane = rowItemOfic["codigo_dane"].ToString().Trim();
                                string _NombreOficina = rowItemOfic["nombre_oficina"].ToString().Trim();

                                #region AQUI REGISTRAMOS EN EL DATATABLE LOS DATOS DE LA OFICINA A LIQUIDAR
                                DataRow Fila = null;
                                Fila = dtLiquidacionIca.NewRow();
                                Fila["idliquid_impuesto"] = dtLiquidacionIca.Rows.Count + 1;
                                Fila["id_municipio"] = _IdMunicipio;
                                Fila["idformulario_impuesto"] = _IdFormularioImpuesto;
                                Fila["id_cliente"] = _IdCliente;
                                Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                Fila["nombre_oficina"] = _NombreOficina;
                                Fila["codigo_dane"] = _CodigoDane;
                                Fila["anio_gravable"] = _AnioGravable;
                                //--INFO DEL MUNICIPIO Y DEL CLIENTE
                                Fila["dpto_oficina"] = rowItemOfic["dpto_oficina"].ToString().Trim();
                                Fila["municipio_oficina"] = rowItemOfic["municipio_oficina"].ToString().Trim();
                                Fila["nombre_cliente"] = rowItemOfic["nombre_cliente"].ToString().Trim();
                                Fila["idtipo_identificacion"] = rowItemOfic["idtipo_identificacion"].ToString().Trim();
                                Fila["numero_documento"] = rowItemOfic["numero_documento"].ToString().Trim();
                                Fila["digito_verificacion"] = rowItemOfic["digito_verificacion"].ToString().Trim();
                                Fila["consorcio_union_temporal"] = rowItemOfic["consorcio_union_temporal"].ToString().Trim();
                                Fila["actividad_patrim_autonomo"] = rowItemOfic["actividad_patrim_autonomo"].ToString().Trim();
                                Fila["direccion_cliente"] = rowItemOfic["direccion_cliente"].ToString().Trim();
                                Fila["dpto_cliente"] = rowItemOfic["dpto_cliente"].ToString().Trim();
                                Fila["municipio_cliente"] = rowItemOfic["municipio_cliente"].ToString().Trim();
                                Fila["telefono_contacto"] = rowItemOfic["telefono_contacto"].ToString().Trim();
                                Fila["email_contacto"] = rowItemOfic["email_contacto"].ToString().Trim();
                                Fila["numero_puntos"] = _NumEstablecimiento;
                                Fila["tipo_clasificacion"] = rowItemOfic["tipo_clasificacion"].ToString().Trim();

                                Fila["fecha_max_presentacion"] = DateTime.Now.ToString("dd-MM-yyyy");
                                Fila["fecha_liquidacion"] = DateTime.Now.ToString("dd MM yyyy");
                                Fila["periodo_impuesto"] = 7;   //--PAGO ANUAL
                                Fila["opcion_uso"] = 2;         //--SOLO PAGO
                                Fila["num_declaracion"] = "NA"; //--NUMERO DE FORMULARIO

                                Fila["valor_renglon8"] = 0;
                                Fila["valor_renglon9"] = 0;
                                Fila["valor_renglon10"] = 0;
                                Fila["valor_renglon11"] = 0;
                                Fila["valor_renglon12"] = 0;
                                Fila["valor_renglon13"] = 0;
                                Fila["valor_renglon14"] = 0;
                                Fila["valor_renglon15"] = 0;
                                Fila["valor_renglon16"] = 0;
                                Fila["codigo_actividad1"] = "";
                                Fila["valor_actividad1"] = 0;
                                Fila["tarifa_actividad1"] = 0;
                                Fila["codigo_actividad2"] = "";
                                Fila["valor_actividad2"] = 0;
                                Fila["tarifa_actividad2"] = 0;
                                Fila["codigo_actividad3"] = "";
                                Fila["valor_actividad3"] = 0;
                                Fila["tarifa_actividad3"] = 0;
                                Fila["valor_actividad4"] = 0;
                                Fila["tarifa_actividad4"] = 0;
                                Fila["valor_otras_act"] = 0;
                                Fila["total_ingresos_gravado"] = 0;
                                Fila["total_impuestos"] = 0;

                                Fila["valor_renglon17"] = 0;
                                Fila["valor_renglon18"] = 0;
                                Fila["valor_renglon19"] = 0;
                                Fila["valor_renglon20"] = 0;
                                Fila["valor_renglon21"] = 0;
                                Fila["valor_renglon22"] = 0;
                                Fila["valor_renglon23"] = 0;
                                Fila["valor_renglon24"] = 0;
                                Fila["valor_renglon25"] = 0;
                                Fila["valor_renglon26"] = 0;
                                Fila["valor_renglon27"] = 0;
                                Fila["valor_renglon28"] = 0;
                                Fila["valor_renglon29"] = 0;
                                Fila["valor_renglon30"] = 0;
                                //Fila["tarifa_ica"] = 0;   //--EL VALOR DE ESTA COLUMNA ES IGUAL A LA COLUMNA total_ingresos_gravado
                                Fila["base_grav_bomberil"] = 0;
                                Fila["base_grav_seguridad"] = 0;
                                Fila["sanciones"] = 1;
                                Fila["descripcion_sancion_otro"] = "NA";
                                Fila["valor_sancion"] = 0;
                                Fila["valor_renglon32"] = 0;
                                Fila["valor_renglon33"] = 0;
                                Fila["valor_renglon34"] = 0;
                                Fila["valor_renglon35"] = 0;
                                Fila["valor_renglon36"] = 0;
                                Fila["interes_mora"] = 0;
                                Fila["valor_renglon38"] = 0;
                                Fila["valor_pago_voluntario"] = 0;
                                Fila["destino_pago_voluntario"] = "NA";
                                Fila["valor_renglon40"] = 0;
                                dtLiquidacionIca.Rows.Add(Fila);
                                #endregion

                                #region AQUI REALIZAMOS EL PROCESO DE LIQUIDACION DE LA OFICINA
                                //--PASO 3: OBTENER DATOS DE LA BASE GRAVABLE
                                objObtenerDatos.TipoConsulta = 2;
                                objObtenerDatos.IdCliente = _IdCliente;
                                objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                objObtenerDatos.IdFormConfiguracion = null;
                                objObtenerDatos.IdPuc = null;
                                //--AQUI OBTENEMOS LOS DATOS DEL ESTABLECIMIENTO EN LIQUIDACION DEL IMPUESTO
                                DataRow[] dataRows = dtLiquidacionIca.Select("idcliente_establecimiento = " + _IdClienteEstablecimiento);
                                if (dataRows.Length == 1)
                                {
                                    #region OBTENEMOS LOS VALORES DE LA SESION (B. BASE GRAVABLE)
                                    DataTable dtBaseGravable = new DataTable();
                                    dtBaseGravable = objObtenerDatos.GetBaseGravable();
                                    double _ValorRenglon16 = 0;
                                    int _ContadorRow = 0;
                                    //--
                                    if (dtBaseGravable != null)
                                    {
                                        if (dtBaseGravable.Rows.Count > 0)
                                        {
                                            #region AQUI RECORREMOS LAS BASE GRAVABLE CONFIGURADAS POR EL CLIENTE
                                            foreach (DataRow rowItemBg in dtBaseGravable.Rows)
                                            {
                                                #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                                _ContadorRow++;
                                                int _NumRenglon = Int32.Parse(rowItemBg["numero_renglon"].ToString().Trim());
                                                string _CodigoCuenta = rowItemBg["codigo_cuenta"].ToString().Trim();
                                                string _SaldoInicial = rowItemBg["saldo_inicial"].ToString().Trim();
                                                string _MovDebito = rowItemBg["mov_debito"].ToString().Trim();
                                                string _MovCredito = rowItemBg["mov_credito"].ToString().Trim();
                                                string _SaldoFinal = rowItemBg["saldo_final"].ToString().Trim();
                                                string _ValorExtracontable1 = rowItemBg["valor_extracontable"].ToString().Trim().Replace("-", "");
                                                double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                                //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                                objObtenerDatos.TipoConsulta = 1;
                                                objObtenerDatos.NumeroRenglon = _NumRenglon;
                                                //objObtenerDatos.IdCliente = _IdCliente;
                                                objObtenerDatos.AnioGravable = _AnioGravable;
                                                //objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                objObtenerDatos.SaldoInicial = _SaldoInicial;
                                                objObtenerDatos.MovDebito = _MovDebito;
                                                objObtenerDatos.MovCredito = _MovCredito;
                                                objObtenerDatos.SaldoFinal = _SaldoFinal;
                                                objObtenerDatos.CodigoCuenta = _CodigoCuenta;

                                                //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                                double _ValorTotal = 0;
                                                if (_SaldoInicial == "S" || _MovDebito == "S" ||
                                                    _MovCredito == "S" || _SaldoFinal == "S")
                                                {
                                                    List<string> _ArrayDatos = objObtenerDatos.GetEstadoFinanciero();
                                                    if (_ArrayDatos.Count > 0)
                                                    {
                                                        string _ValorCuenta = _ArrayDatos[1].ToString().Trim();
                                                        _ValorTotal = (Double.Parse(_ValorCuenta) + _ValorExtracontable);
                                                        FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                                    }
                                                }
                                                else
                                                {
                                                    _ValorTotal = _ValorExtracontable;
                                                }
                                                #endregion

                                                #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                                double _SumValorRenglon = 0;
                                                switch (_NumRenglon)
                                                {
                                                    case 8:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon8"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    //case 9:
                                                    //    _SumValorRenglon = Double.Parse(this.LblValorRenglon9.Text.ToString().Trim().Replace("$ ", "").Replace(".", "")) + _ValorTotal;
                                                    //    this.LblValorRenglon9.Text = String.Format(String.Format("{0:###,###,##0}", _SumValorRenglon));
                                                    //    break;
                                                    case 10:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon10"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 11:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon11"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 12:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon12"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 13:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon13"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 14:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon14"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 15:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon15"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 26:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon26"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 27:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon27"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 28:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon28"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 29:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon29"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 30:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon30"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 31:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["sanciones"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["sanciones"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 32:
                                                        _SumValorRenglon = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim()) + _ValorTotal;
                                                        dataRows[0]["valor_renglon32"] = round(_SumValorRenglon);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                #endregion
                                            }

                                            #region AQUI CALCULAMOS EL VALOR DEL RENGLON 9 Y 16
                                            //--AQUI CALCULAMOS EL VALOR DEL CAMPO 9
                                            double _ValorRenglon8 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                            double _ValorRenglon10 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                            double _ValorRenglon9 = (_ValorRenglon8 - _ValorRenglon10);
                                            //--
                                            double _ValorRenglon11 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                            double _ValorRenglon12 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                            double _ValorRenglon13 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                            double _ValorRenglon14 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                            double _ValorRenglon15 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                            double _SumRenglones = (_ValorRenglon11 - _ValorRenglon12 - _ValorRenglon13 - _ValorRenglon14 - _ValorRenglon15);
                                            _ValorRenglon16 = (_ValorRenglon10 - Double.Parse(_SumRenglones.ToString().Replace("-", "")));
                                            //--
                                            dataRows[0]["valor_renglon9"] = _ValorRenglon9;
                                            dataRows[0]["valor_renglon16"] = round(_ValorRenglon16);
                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                            dtLiquidacionIca.Rows[0].EndEdit();
                                            #endregion

                                            //--FIN DEL CALCULO DE LA BASE GRAVABLE
                                            #endregion
                                        }
                                        else
                                        {
                                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                            ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                            ObjEmails.Asunto = "REF.: ERROR AL OBTENER LA BASE GRAVABLE";

                                            string nHora = DateTime.Now.ToString("HH");
                                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                            StringBuilder strDetalleEmail = new StringBuilder();
                                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: No se obtuvo información de la base gravable del Id_Cliente " + _IdCliente + " y Id_Oficina " + _IdClienteEstablecimiento + "</h4>" +
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
                                            return null;
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER LA BASE GRAVABLE";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: Error al obtener información de la base gravable del Id_Cliente " + _IdCliente + " y Id_Oficina " + _IdClienteEstablecimiento + "</h4>" +
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
                                        return null;
                                        #endregion
                                    }
                                    #endregion

                                    #region OBTENEMOS LOS VALORES DE LA SESION (C. DISCRIMINACION DE ACTIVIDADES ECONOMICAS)
                                    objObtenerDatos.TipoConsulta = 1;
                                    //ObjConsulta.AnioGravable = 1;
                                    objObtenerDatos.IdClienteEstablecimiento = _IdClienteEstablecimiento;

                                    DataTable dtActEconomica = new DataTable();
                                    dtActEconomica = objObtenerDatos.GetConsultarActEconomica();
                                    //this.ViewState["dtActEconomica"] = dtActEconomica;
                                    double _TotalImpuesto = 0;

                                    if (dtActEconomica != null)
                                    {
                                        if (dtActEconomica.Rows.Count > 0)
                                        {
                                            _ContadorRow = 1;
                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                            #region AQUI RECORREMOS EL DATATABLE
                                            foreach (DataRow rowItem in dtActEconomica.Rows)
                                            {
                                                #region AQUI DEFINIMOS LOS VALORES DE CADA DE LAS 3 ACTIVIDADES DEL FORMULARIO
                                                string _CodigoActividad = rowItem["codigo_actividad"].ToString().Trim();
                                                int _IdCalcularTarifaPor = Int32.Parse(rowItem["idtipo_calculo"].ToString().Trim());
                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                double _TarifaLey = Double.Parse(rowItem["tarifa_ley"].ToString().Trim());
                                                double _TarifaMunicipio = Double.Parse(rowItem["tarifa_municipio"].ToString().Trim());
                                                string _SaldoFinal1 = rowItem["saldo_final"].ToString().Trim().Replace("-", "");
                                                double _SaldoFinal = Double.Parse(_SaldoFinal1);
                                                double _ValorActividad = 0;

                                                switch (_ContadorRow)
                                                {
                                                    case 1:
                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 1
                                                        double _DefValorActividad = _SaldoFinal > 0 ? _SaldoFinal : _ValorRenglon16;

                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                        {
                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_DefValorActividad * _TarifaLey) / 100);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 100);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad1"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad1"] = round(_DefValorActividad);
                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_DefValorActividad * _TarifaLey) / 1000);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                //_ValorActividad = (_DefValorActividad * _TarifaMunicipio);
                                                                _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 1000);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad1"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad1"] = round(_DefValorActividad);
                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = (_DefValorActividad * _TarifaLey);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = (_DefValorActividad * _TarifaMunicipio);
                                                                dataRows[0]["tarifa_actividad1"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad1"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad1"] = round(_DefValorActividad);
                                                            dataRows[0]["valor_renglon17"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        break;
                                                    #endregion

                                                    case 2:
                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 2
                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                        {
                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 100);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad2"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad2"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 1000);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad2"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad2"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaLey);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaLey;
                                                            }
                                                            else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                dataRows[0]["tarifa_actividad2"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad2"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad2"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon18"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        break;
                                                    #endregion

                                                    case 3:
                                                        #region AQUI CALCULAMOS EL VALOR DE LA ACTIVIDAD ECONOMICA 3
                                                        //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                        if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                        {
                                                            #region VALIDACION DE LA TARIFA PORCENTUAL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 100);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaLey;
                                                            }
                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad3"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad3"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 2)    //--2. POR MIL
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR MIL
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = ((_SaldoFinal * _TarifaLey) / 1000);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaLey;
                                                            }
                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad3"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad3"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else if (_IdTipoTarifa == 8)    //--8. POR UNIDAD
                                                        {
                                                            #region VALIDACION DE LA TARIFA POR UNIDAD
                                                            //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                            if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaLey);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaLey;
                                                            }
                                                            else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                            {
                                                                _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                dataRows[0]["tarifa_actividad3"] = _TarifaMunicipio;
                                                            }

                                                            dataRows[0]["codigo_actividad3"] = _CodigoActividad;
                                                            dataRows[0]["valor_actividad3"] = round(_SaldoFinal);
                                                            dataRows[0]["valor_renglon19"] = round(_ValorActividad);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        break;
                                                    #endregion

                                                    default:
                                                        break;
                                                }
                                                _ContadorRow++;

                                                #region AQUI CALCULAMOS LOS INGRESOS NO GRAVADOS Y TOTAL DEL IMPUESTO
                                                //--AQUI CALCULAMOS LOS INGRESOS NO GRAVADOS
                                                double _ValorIngAct1 = Double.Parse(dataRows[0]["valor_actividad1"].ToString().Trim());
                                                double _ValorIngAct2 = Double.Parse(dataRows[0]["valor_actividad2"].ToString().Trim());
                                                double _ValorIngAct3 = Double.Parse(dataRows[0]["valor_actividad3"].ToString().Trim());
                                                double _TotalIngresosGravados = (_ValorIngAct1 + _ValorIngAct2 + _ValorIngAct3);

                                                //--AQUI CALCULAMOS EL TOTAL DEL IMPUESTO
                                                double _ValorRenglon17 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                double _ValorRenglon18 = Double.Parse(dataRows[0]["valor_renglon18"].ToString().Trim());
                                                double _ValorRenglon19 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                double _ValorDesagregacion = 0;
                                                _TotalImpuesto = (_ValorRenglon17 + _ValorRenglon18 + _ValorRenglon19 + _ValorDesagregacion);

                                                //--ACTUALIZAR VALORES
                                                dataRows[0]["total_ingresos_gravado"] = round(_TotalIngresosGravados);
                                                dataRows[0]["total_impuestos"] = round(_TotalImpuesto);
                                                dtLiquidacionIca.Rows[0].AcceptChanges();
                                                dtLiquidacionIca.Rows[0].EndEdit();
                                                #endregion
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER ACTIVIDADES ECONOMICAS";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener las actividades economicas de la oficina." + "</h4>" +
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
                                        return null;
                                        #endregion
                                    }
                                    #endregion

                                    #region AQUI CALCULAMOS LOS VALORES DE LA SESION (D. LIQUIDACION PRIVADA)
                                    _TotalImpuesto = Double.Parse(dataRows[0]["total_impuestos"].ToString().Trim());
                                    double _TotalImpuestosLey = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                    double _ValorRenglon20 = (_TotalImpuesto + _TotalImpuestosLey);
                                    //--
                                    dataRows[0]["valor_renglon20"] = round(_ValorRenglon20);
                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                    dtLiquidacionIca.Rows[0].EndEdit();

                                    objObtenerDatos.IdMunicipio = _IdMunicipio;
                                    objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;

                                    DataTable dtImpMunicipio = new DataTable();
                                    dtImpMunicipio = objObtenerDatos.GetImpuestosMunicipio();
                                    double _ValorLiquidacion = 0;

                                    if (dtImpMunicipio != null)
                                    {
                                        if (dtImpMunicipio.Rows.Count > 0)
                                        {
                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                            #region AQUI RECORREMOS EL DATATABLE
                                            foreach (DataRow rowItem in dtImpMunicipio.Rows)
                                            {
                                                #region OBTENER DATOS DEL DATATABLE Y CALCULAR VALOR DE RENGLONES
                                                int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                                string _DescripcionRenglon = rowItem["descripcion_renglon"].ToString().Trim();
                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                double _ValorTarifa = Double.Parse(rowItem["valor_tarifa"].ToString().Trim());
                                                dataRows[0]["base_grav_bomberil"] = round(_ValorRenglon20);
                                                dataRows[0]["base_grav_seguridad"] = round(_ValorRenglon20);

                                                //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                if (_IdTipoTarifa == 1)     //--TARIFA PORCENTUAL
                                                {
                                                    _ValorLiquidacion = ((_ValorRenglon20 * _ValorTarifa) / 100);
                                                }
                                                else if (_IdTipoTarifa == 2)    //--TARIFA POR MIL
                                                {
                                                    _ValorLiquidacion = ((_ValorRenglon20 * _ValorTarifa) / 1000);
                                                }

                                                #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                switch (_NumRenglon)
                                                {
                                                    case 21:
                                                        dataRows[0]["valor_renglon21"] = round(_ValorLiquidacion);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    //case 22:
                                                    //    this.LblValorRenglon22.Text = String.Format(String.Format("{0:###,###,##0}", _ValorLiquidacion));
                                                    //    break;
                                                    case 23:
                                                        dataRows[0]["valor_renglon23"] = round(_ValorLiquidacion);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    case 24:
                                                        dataRows[0]["valor_renglon24"] = round(_ValorLiquidacion);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        break;
                                                    //case 31:
                                                    //    this.TxtValorRenglon31.Text = String.Format(String.Format("{0:###,###,##0}", _ValorLiquidacion));
                                                    //    break;
                                                    default:
                                                        break;
                                                }
                                                #endregion
                                                //--
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER IMPUESTOS MUNICIPIOS";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los impuestos del id municipio " + _IdMunicipio + "</h4>" +
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
                                        return null;
                                        #endregion
                                    }
                                    #endregion

                                    ///---------------
                                    #region AQUI CALCULAMOS EL VALOR DEL RENGLON 25 (TOTAL DE IMPUESTO A CARGO)
                                    _ValorRenglon20 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                    double _ValorRenglon21 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                    double _ValorRenglon22 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                    double _ValorRenglon23 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                    double _ValorRenglon24 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                    double _ValorRenglon25 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                    //this.LblValorRenglon25.Text = String.Format(String.Format("{0:###,###,##0}", round(_ValorRenglon25)));
                                    #endregion

                                    #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                    //--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                    double _ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                    double _ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                    double _ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                    double _ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                    double _ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                    double _ValorRenglon31 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                    double _ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());

                                    //double _TotalSaldoCargo = (_ValorRenglon25 - (_ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + (_ValorRenglon30 + _ValorRenglon31) + _ValorRenglon32);
                                    double _TotalSaldoCargoAux = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                    double _TotalSaldoCargo = _TotalSaldoCargoAux >= 0 ? _TotalSaldoCargoAux : 0;
                                    double _TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                    //--
                                    dataRows[0]["valor_renglon33"] = round(_TotalSaldoCargo);
                                    dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                    dataRows[0]["valor_renglon35"] = round(_TotalSaldoCargo);
                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                    dtLiquidacionIca.Rows[0].EndEdit();
                                    #endregion

                                    #region OBTENEMOS LOS VALORES DE TARIFAS MINIMAS DEL MUNICIPIO
                                    objObtenerDatos.AnioGravable = _AnioGravable;
                                    objObtenerDatos.IdFormularioImpuesto = _IdFormularioImpuesto;
                                    objObtenerDatos.IdMunicipio = _IdMunicipio;
                                    //--
                                    DataTable dtTarifasMin = new DataTable();
                                    dtTarifasMin = objObtenerDatos.GetTarifaMinMunicipio();
                                    double _OperacionRenglon36 = 0, _TotalTarifaMinima = 0;

                                    if (dtTarifasMin != null)
                                    {
                                        if (dtTarifasMin.Rows.Count > 0)
                                        {
                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                            #region AQUI RECORREMOS EL DATATABLE
                                            foreach (DataRow rowItem in dtTarifasMin.Rows)
                                            {
                                                #region AQUI OBNTENEMOS LOS VALORES DE LA PARAMETRIZACION DE OTRAS CONFIGURACIONES
                                                string _NumeroRenglon = rowItem["numero_renglon"].ToString().Trim();
                                                string _CalcularRenglon = rowItem["calcular_renglon"].ToString().Trim();
                                                string _NumeroRenglon1 = rowItem["numero_renglon1"].ToString().Trim();
                                                string _IdTipoOperacion = rowItem["idtipo_operacion"].ToString().Trim();
                                                string _NumeroRenglon2 = rowItem["numero_renglon2"].ToString().Trim();

                                                int _IdUnidadMedida = Int32.Parse(rowItem["idunidad_medida"].ToString().Trim());
                                                int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                double _ValorUnidad = Double.Parse(rowItem["valor_unidad"].ToString().Trim());
                                                double _CantidadMedida = Double.Parse(rowItem["cantidad_medida"].ToString().Trim());
                                                #endregion

                                                #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                double _ValorTarifaMinima = 0;
                                                double _OperacionRenglon = 0, _ValorNumRenglon1 = 0, _ValorNumRenglon2 = 0;
                                                //--
                                                #region OBTENER EL VALOR A CALCULAR DEL RENGLON 1
                                                switch (Int32.Parse(_NumeroRenglon))
                                                {
                                                    ///--LOS RENGLONES 8, 9 Y 10 NO LO TENEMOS EN CUENTA PORQUE SON LOS INGRESOS PAIS Y DE LA OFICINA
                                                    case 11:
                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                        if (_CalcularRenglon.Equals("S"))
                                                        {
                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon1 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon2 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                            if (_TipoOperacion == 1)
                                                            {
                                                                //_SumValorRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 2)
                                                            {

                                                            }
                                                            else if (_TipoOperacion == 3)
                                                            {

                                                            }
                                                            else if (_TipoOperacion == 4)
                                                            {

                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            _ValorNumRenglon1 = 0;
                                                        }
                                                        #endregion
                                                        break;
                                                    case 12:
                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                        if (_CalcularRenglon.Equals("S"))
                                                        {
                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon1 = 0;
                                                                    break;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            _ValorNumRenglon1 = 0;
                                                        }
                                                        #endregion
                                                        break;
                                                    case 13:
                                                        break;
                                                    case 14:
                                                        break;
                                                    case 15:
                                                        break;
                                                    case 22:
                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                        if (_CalcularRenglon.Equals("S"))
                                                        {
                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon1 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon2 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                            if (_TipoOperacion == 1)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 2)
                                                            {

                                                            }
                                                            else if (_TipoOperacion == 3)
                                                            {

                                                            }
                                                            else if (_TipoOperacion == 4)
                                                            {

                                                            }
                                                            else
                                                            {

                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            //int _NumEstablecimiento = Int32.Parse(this.LblNumEstablecimientos.Text.ToString().Trim().Replace(".", ""));
                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                            {
                                                                _ValorTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                            }
                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                            {
                                                                _ValorTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                            }

                                                            //--
                                                            double _SumatoriaRenglon22 = 0;
                                                            if (_NumEstablecimiento <= 1)
                                                            {
                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * _NumEstablecimiento);
                                                            }
                                                            else
                                                            {
                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * (_NumEstablecimiento - 1));  //--LE RESTAMOS UN ESTABLECIMIENTO
                                                            }
                                                            //--
                                                            dataRows[0]["valor_renglon22"] = round(_SumatoriaRenglon22);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                        }
                                                        #endregion

                                                        #region AQUI CALCULAMOS EL VALOR DEL RENGLON 25 (TOTAL DE IMPUESTO A CARGO)
                                                        _ValorRenglon20 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                        _ValorRenglon21 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                        _ValorRenglon22 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                        _ValorRenglon23 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                        _ValorRenglon24 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                        _ValorRenglon25 = (_ValorRenglon20 + _ValorRenglon21 + _ValorRenglon22 + _ValorRenglon23 + _ValorRenglon24);
                                                        //--
                                                        dataRows[0]["valor_renglon25"] = round(_ValorRenglon25);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        #endregion
                                                        break;
                                                    case 26:
                                                        break;
                                                    case 27:
                                                        break;
                                                    case 28:
                                                        break;
                                                    case 29:
                                                        break;
                                                    case 30:
                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                        double _OperacionRenglon30 = 0;
                                                        if (_CalcularRenglon.Equals("S"))
                                                        {
                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                case 25:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                    break;
                                                                case 26:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                    break;
                                                                case 27:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                    break;
                                                                case 28:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                    break;
                                                                case 29:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon1 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                case 25:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                    break;
                                                                case 26:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                    break;
                                                                case 27:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                    break;
                                                                case 28:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                    break;
                                                                case 29:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon2 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                            if (_TipoOperacion == 1)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 2)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 3)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 4)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                            }
                                                            else
                                                            {
                                                                _OperacionRenglon = 0;
                                                            }

                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                            {
                                                                _OperacionRenglon30 = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                            }
                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                            {
                                                                _OperacionRenglon30 = (_OperacionRenglon * _CantidadMedida);
                                                            }
                                                            //--
                                                            dataRows[0]["valor_renglon30"] = round(_OperacionRenglon30);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            //int _NumEstablecimiento = Int32.Parse(this.LblNumEstablecimientos.Text.ToString().Trim().Replace(".", ""));
                                                            //--
                                                            double _SumatoriaRenglon22 = 0;
                                                            if (_NumEstablecimiento <= 1)
                                                            {
                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * _NumEstablecimiento);
                                                            }
                                                            else
                                                            {
                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima * (_NumEstablecimiento - 1));  //--LE RESTAMOS UN ESTABLECIMIENTO
                                                            }
                                                            //--
                                                            dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                        }
                                                        #endregion

                                                        #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                                        //--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                                        _ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                        _ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _ValorRenglon31 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                        _TotalSaldoCargo = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                                        _TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                                        //--
                                                        dataRows[0]["valor_renglon33"] = round(_TotalSaldoCargo);
                                                        dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                                        dataRows[0]["valor_renglon35"] = round(_TotalSaldoCargo);
                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                        #endregion
                                                        break;
                                                    case 31:
                                                        break;
                                                    case 32:
                                                        break;
                                                    case 36:
                                                        #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                        _OperacionRenglon36 = 0;
                                                        if (_CalcularRenglon.Equals("S"))
                                                        {
                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon1))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                case 25:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                    break;
                                                                case 26:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                    break;
                                                                case 27:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                    break;
                                                                case 28:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                    break;
                                                                case 29:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon1 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                            //--
                                                            switch (Int32.Parse(_NumeroRenglon2))
                                                            {
                                                                case 8:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                    break;
                                                                case 9:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                    break;
                                                                case 10:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                    break;
                                                                case 11:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                    break;
                                                                case 12:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                    break;
                                                                case 13:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                    break;
                                                                case 14:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                    break;
                                                                case 15:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                    break;
                                                                case 16:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                    break;
                                                                case 20:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                    break;
                                                                case 21:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                    break;
                                                                case 25:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                    break;
                                                                case 26:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                    break;
                                                                case 27:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                    break;
                                                                case 28:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                    break;
                                                                case 29:
                                                                    _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                    break;
                                                                default:
                                                                    _ValorNumRenglon2 = 0;
                                                                    break;
                                                            }
                                                            #endregion

                                                            #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                            int _TipoOperacion = _IdTipoOperacion.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion.Trim()) : 0;
                                                            //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                            if (_TipoOperacion == 1)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 2)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 3)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                            }
                                                            else if (_TipoOperacion == 4)
                                                            {
                                                                _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                            }
                                                            else
                                                            {
                                                                _OperacionRenglon = 0;
                                                            }

                                                            //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                            if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                            {
                                                                _OperacionRenglon36 = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                            }
                                                            else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                            {
                                                                _OperacionRenglon36 = (_OperacionRenglon * _CantidadMedida);
                                                            }
                                                            //--
                                                            dataRows[0]["valor_renglon36"] = round(_OperacionRenglon36);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion
                                                        }
                                                        #endregion

                                                        #region AQUI CALCULAMOS EL VALOR A PAGAR
                                                        //double _ValorRengln35 = 0;
                                                        //_ValorRengln35 = Double.Parse(this.LblValorRenglon35.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));

                                                        ////--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                        //if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                        //{
                                                        //    _OperacionRenglon36 = ((_ValorRengln35 * _CantidadMedida) / 100);
                                                        //}
                                                        //else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                        //{
                                                        //    _OperacionRenglon36 = (_ValorRengln35 * _CantidadMedida);
                                                        //}

                                                        //this.LblValorRenglon36.Text = String.Format(String.Format("{0:###,###,##0}", round(_OperacionRenglon36)));
                                                        #endregion
                                                        break;
                                                    case 40:
                                                        #region AQUI CALCULAMOS EL VALOR DE LA TARIFA MINIMA A PAGAR SI EL TOTAL A PAGAR ES CERO
                                                        //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                        if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                        {
                                                            _TotalTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                        }
                                                        else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                        {
                                                            _TotalTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                        }
                                                        #endregion
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                #endregion

                                                #endregion

                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER TARIFAS  MINIMAS";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener las tarifas minimas del id municipio " + _IdMunicipio + "</h4>" +
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
                                        return null;
                                        #endregion
                                    }
                                    #endregion

                                    #region CALCULAR RENGLONES 36, 37, 38 y 39
                                    double _ValorRenglon35 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                    double _ValorRenglon36 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                    double _ValorRenglon37 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                    double _ValorRenglon38 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                    double _ValorRenglon39 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                    double _ValorRenglon40 = Double.Parse(dataRows[0]["valor_renglon40"].ToString().Trim());
                                    double _TotalPagarVoluntario = (_ValorRenglon38 + _ValorRenglon39);
                                    double _TotalPagar = (_ValorRenglon35 - _ValorRenglon36 + _ValorRenglon37);
                                    //--
                                    if (_ValorRenglon40 > 0)
                                    {
                                        dataRows[0]["valor_renglon40"] = _TotalPagarVoluntario;
                                    }
                                    else
                                    {
                                        dataRows[0]["valor_renglon40"] = _TotalTarifaMinima;
                                    }
                                    dataRows[0]["valor_renglon38"] = round(_TotalPagar);
                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                    dtLiquidacionIca.Rows[0].EndEdit();
                                    #endregion
                                }
                                #endregion
                                //--
                                #endregion
                            }
                        }
                        else
                        {
                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                            ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                            ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: el id cliente [" + _IdCliente + "] no tiene asociada oficinas." + "</h4>" +
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
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: " + _MsgError + "</h4>" +
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
                else
                {
                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                    ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE EF";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el TIPO DE PROCESO [" + _TipoProceso + "]. Motivo: No se obtuvo información de estados financieros cargados al idcliente " + _IdCliente + "</h4>" +
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
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EMAIL_DESTINATION_ERROR;
                ObjEmails.Asunto = "REF.: ERROR PROCESO LIQUIDACION x LOTE";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al realizar el proceso de liquidación de la oficina. Motivo: " + ex.Message + "</h4>" +
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
                dtLiquidacionIca = null;
                #endregion
            }

            return dtLiquidacionIca;
        }

        public static double round(double input)
        {
            double _Result = 0;
            double rem = input % 1000;
            //return rem >= 5 ? (num - rem + 10) : (num - rem);
            if (rem >= 500)
            {
                _Result = (double)(1000 * Math.Ceiling(input / 1000));
            }
            else
            {
                _Result = (double)(1000 * Math.Round(input / 1000));
            }

            return _Result;
        }

        public static bool GenerarArchivoPdf(DataTable dtLiquidacion, int _IdEstadoDeclaracion)
        {
            bool _Result = false;
            try
            {
                foreach (DataRow rowItem in dtLiquidacion.Rows)
                {
                    #region AQUI CREAMOS EL DIRECTORIO PARA GUARDAR LOS ARCHIVOS
                    //string _RutaMapPath = HttpContext.Current.Server.MapPath("\\");
                    //string _RutaVirtual = HttpContext.Current.Server.MapPath("/" + FixedData.REPOSITORIO_LIQUIDACION.ToString().Trim());
                    //string _PathDirectorio = _RutaVirtual + "\\" + this.Session["DirectorioArchivos"].ToString().Trim() + "\\" + DateTime.Now.ToString("yyyy") + "\\" + "ICA\\BORRADOR" + "\\" + "CLIENTE_" + this.Session["IdCliente"].ToString().Trim();
                    var CurrentDirectory = Directory.GetCurrentDirectory();
                    string _PathDirectorio = "";
                    int _IdFormularioImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                    if (_IdFormularioImpuesto == 1)
                    {
                        if (_IdEstadoDeclaracion == 2)  //--2. PRELIMINAR, 3. DEFINITIVO
                        {
                            _PathDirectorio = CurrentDirectory + "\\" + FixedData.REPOSITORIO_LIQUIDACION.ToString().Trim() + "\\" + DateTime.Now.ToString("yyyy") + "\\" + "ICA\\BORRADOR";
                        }
                        else
                        {
                            _PathDirectorio = CurrentDirectory + "\\" + FixedData.REPOSITORIO_LIQUIDACION.ToString().Trim() + "\\" + DateTime.Now.ToString("yyyy") + "\\" + "ICA\\DEFINITIVO";
                        }
                    }
                    else
                    {

                    }

                    if (!Directory.Exists(_PathDirectorio))
                    {
                        Directory.CreateDirectory(_PathDirectorio);
                    }

                    // Creamos el documento con el tamaño de página tradicional
                    Document doc = new Document(PageSize.LETTER);
                    //Document doc = new Document(PageSize.A5, 25, 25, 30, 30);
                    //Document doc = new Document();

                    // Indicamos donde vamos a guardar el documento
                    string _PathArchivoPdf = _PathDirectorio + "\\FORMULARIO_" + rowItem["id_municipio"].ToString().Trim() + ".pdf";

                    //--AQUI VALIDAMOS SI EXISTE EL ARCHIVO PARA BORRARLO.
                    if (File.Exists(_PathArchivoPdf))
                    {
                        File.Delete(_PathArchivoPdf);
                    }

                    FileStream fileStream = new FileStream(_PathArchivoPdf, System.IO.FileMode.OpenOrCreate);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fileStream);
                    doc.SetPageSize(PageSize.LETTER);
                    //doc.SetMargins(36f, 36f, 36f, 36f); // 0.5 inch margins
                    doc.Open();

                    //--AQUI COLOCAMOS LA MARCA DE AGUA EN EL DOCUMENTO
                    PdfContentByte cb = writer.DirectContentUnder;
                    if (_IdEstadoDeclaracion == 2)  //--2. PRELIMINAR, 3. DEFINITIVO
                    {
                        Image image = Image.GetInstance("Imagenes\\img_borrador.png");
                        float posicionX = (writer.PageSize.Top / 2) - (image.Width / 2);
                        float posicionY = (writer.PageSize.Right / 2) - (image.Height / 2);
                        image.SetAbsolutePosition(posicionX, posicionY);
                        PdfGState state = new PdfGState();
                        state.FillOpacity = 0.2f;
                        cb.SetGState(state);
                        cb.AddImage(image);
                    }
                    #endregion

                    #region DEFINIR TITULO DEL FORMULARIO
                    float _AnchoTabla = 560f;
                    PdfPTable tblTitulo = new PdfPTable(4);
                    tblTitulo.TotalWidth = _AnchoTabla;
                    tblTitulo.LockedWidth = true;
                    tblTitulo.WidthPercentage = 100;

                    Font _FontEncabezado1 = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK);
                    //--> 1. Titulo: del Formulario
                    Paragraph LblTitulo = new Paragraph("\nFORMULARIO ÚNICO NACIONAL DE DECLARACIÓN Y PAGO DEL IMPUESTO DE INDUSTRIA Y COMERCIO\n\n", _FontEncabezado1);
                    PdfPCell clTitulo = new PdfPCell(new Phrase(LblTitulo));
                    clTitulo.Colspan = 4;
                    //header.BorderWidth = 1;
                    clTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblTitulo.AddCell(clTitulo);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(tblTitulo);
                    #endregion

                    #region DEFINICION DE VALORES DE LA SESION 1 DEL FORMULARIO
                    PdfPTable TblSeccionMun = new PdfPTable(3);
                    TblSeccionMun.TotalWidth = _AnchoTabla;
                    TblSeccionMun.LockedWidth = true;
                    TblSeccionMun.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionMun.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionMun.SpacingAfter = 0;
                    float[] TblWidths0 = new float[3];
                    TblWidths0[0] = 250f;
                    TblWidths0[1] = 250f;
                    TblWidths0[2] = -210f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionMun.SetWidths(TblWidths0);
                    Font _standardFont1 = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.NORMAL, BaseColor.BLACK);

                    //--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 1.
                    string _TituloMunicipio = "MUNICIPIO O DISTRITO:".PadRight(150, ' ') + rowItem["nombre_oficina"].ToString().Trim().ToUpper();
                    PdfPCell clTituloMun = new PdfPCell(new Phrase(_TituloMunicipio, _standardFont1));
                    clTituloMun.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionMun.AddCell(clTituloMun);

                    string _FechaMaxPres = rowItem["fecha_max_presentacion"].ToString().Trim().Replace("-", "/");
                    string _FechaMaxPresentacion = "Fecha máxima presentación\n" + _FechaMaxPres;
                    PdfPCell clFechaMax = new PdfPCell(new Phrase(_FechaMaxPresentacion, _standardFont1));
                    clFechaMax.HorizontalAlignment = Element.ALIGN_CENTER;
                    clFechaMax.Rowspan = 2;
                    clFechaMax.Colspan = 2;
                    TblSeccionMun.AddCell(clFechaMax);

                    //--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 2.
                    string _TituloDpto = "DEPARTAMENTO:".PadRight(140, ' ') + rowItem["dpto_oficina"].ToString().Trim().ToUpper();
                    PdfPCell clTituloDpto = new PdfPCell(new Phrase(_TituloDpto, _standardFont1));
                    clTituloDpto.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionMun.AddCell(clTituloDpto);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionMun);
                    #endregion

                    #region SESION DEL AÑO GRAVABLE Y RECUADRO DE MESES
                    int _NumColumnas = 4;
                    PdfPTable TblSeccionAnio = new PdfPTable(_NumColumnas);
                    TblSeccionAnio.TotalWidth = _AnchoTabla;
                    TblSeccionAnio.LockedWidth = true;
                    TblSeccionAnio.WidthPercentage = 100;
                    ///-Left aLign
                    TblSeccionAnio.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionAnio.SpacingAfter = 0;
                    float[] TblWidths01 = new float[_NumColumnas];
                    TblWidths01[0] = 30f;
                    TblWidths01[1] = 50f;
                    TblWidths01[2] = 10f;
                    TblWidths01[3] = 10f;
                    //-Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionAnio.SetWidths(TblWidths01);
                    Font _FontAnio = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.NORMAL, BaseColor.BLACK);

                    //--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 1.
                    string _AnioGravable = "AÑO GRAVABLE:".PadRight(20, ' ') + rowItem["anio_gravable"].ToString().Trim();
                    PdfPCell clAnioGravable = new PdfPCell(new Phrase(_AnioGravable, _FontAnio));
                    clAnioGravable.PaddingTop = 9f;
                    //clTituloAnio.VerticalAlignment = Element.ALIGN_CENTER;
                    clAnioGravable.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionAnio.AddCell(clAnioGravable);

                    //PdfPTable TblSeccionAuxAnio = new PdfPTable(1);
                    ////float[] widths = new float[] { 10f };
                    ////TblSeccionAuxAnio.SetWidths(widths);
                    //TblSeccionAuxAnio.WidthPercentage = 100;
                    //string _Anio = "2020";
                    //PdfPCell clAnio = new PdfPCell(new Phrase(_Anio, _FontAnio));
                    //clAnio.HorizontalAlignment = Element.ALIGN_CENTER;
                    //TblSeccionAuxAnio.AddCell(clAnio);
                    //TblSeccionAnio.AddCell(TblSeccionAuxAnio);

                    ///--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 2.
                    //string _TituloMeses = "SOLAMENTE PARA BOGOTÁ, marque el Bimestre o periodo anual";
                    //PdfPCell clMeses = new PdfPCell(new Phrase(_TituloMeses, _FontAnio));
                    //clMeses.HorizontalAlignment = Element.ALIGN_LEFT;
                    //TblSeccionAnio.AddCell(clMeses);

                    //PdfPTable TblSeccionMeses = new PdfPTable(1);
                    //TblSeccionMeses.WidthPercentage = 10;
                    //string _MesUno = "ene-feb";
                    //PdfPCell clMesUno = new PdfPCell(new Phrase(_MesUno, _FontAnio));
                    //clMesUno.HorizontalAlignment = Element.ALIGN_CENTER;
                    //TblSeccionMeses.AddCell(clMesUno);
                    //TblSeccionAnio.AddCell(TblSeccionMeses);

                    //string _MesDos = "2";
                    //PdfPCell clMesDos = new PdfPCell(new Phrase(_MesDos, _FontAnio));
                    //clMesDos.HorizontalAlignment = Element.ALIGN_CENTER;
                    //TblSeccionMeses.AddCell(clMesDos);
                    //TblSeccionAnio.AddCell(TblSeccionMeses);

                    string _RutaImagen = CurrentDirectory + "\\Imagenes\\img_cuadro.png";
                    //--AQUI OBTENEMOS EL ARREGLO DE BYTE DE LA IMAGEN.
                    byte[] imageBytes = objFunctions.GetImagenBytes(_RutaImagen);
                    Image ImgMeses = Image.GetInstance(imageBytes);
                    ImgMeses.ScaleAbsolute(700f, 40f);
                    PdfPCell clImg3 = new PdfPCell(ImgMeses);
                    //clImg3.Border = 0;
                    clImg3.Colspan = 3;
                    clImg3.PaddingTop = 5f;
                    clImg3.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionAnio.AddCell(clImg3);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionAnio);
                    #endregion

                    #region SESION OPCION DE USO, DECLARACION INICIAL
                    PdfPTable TblSeccionOpcionUso = new PdfPTable(1);
                    TblSeccionOpcionUso.TotalWidth = _AnchoTabla;
                    TblSeccionOpcionUso.LockedWidth = true;
                    TblSeccionOpcionUso.WidthPercentage = 100;
                    ///-Left aLign
                    TblSeccionOpcionUso.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionOpcionUso.SpacingAfter = 0;
                    //float[] TblWidths01 = new float[2];
                    //TblWidths01[0] = 100f;
                    //TblWidths01[1] = 350f;
                    ////-Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    //TblSeccionAnio.SetWidths(TblWidths01);
                    Font _FontOpcionUso = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.NORMAL, BaseColor.BLACK);

                    string _OpcionUso = "OPCIÓN DE USO";
                    string _Opciones1 = "DECLARACIÓN INICIAL: X";
                    string _Opciones2 = "SOLO PAGO: ";
                    string _Opciones3 = "CORRECCIÓN: ";
                    string _Opciones4 = "Declaración que corrige No.:_____________________________";
                    string _Opciones5 = "Fecha: " + DateTime.Now.ToString("dd/MM/yyy");
                    PdfPCell clOpciones = new PdfPCell(new Phrase(_OpcionUso + "                  " + _Opciones1 + "     " + _Opciones2 + "     " + _Opciones3 + "     " + _Opciones4 + "        " + _Opciones5, _FontOpcionUso));
                    clOpciones.Colspan = 4;
                    clOpciones.PaddingTop = 5f;
                    clOpciones.VerticalAlignment = Element.ALIGN_CENTER;
                    TblSeccionOpcionUso.AddCell(clOpciones);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionOpcionUso);
                    #endregion

                    #region DEFINICION DE VALORES DE LA SESION 1 DEL FORMULARIO
                    //PdfPTable table = new PdfPTable(4);
                    //table.TotalWidth = _AnchoTabla;
                    //table.LockedWidth = true;
                    //table.WidthPercentage = 100;

                    ////--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 1.
                    //Font _standardFont1 = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.NORMAL, BaseColor.BLACK);
                    //string _TituloMunicipio = "MUNICIPIO O DISTRITO:";
                    //PdfPCell clTituloMun = new PdfPCell(new Phrase(_TituloMunicipio, _standardFont1));
                    //clTituloMun.HorizontalAlignment = Element.ALIGN_LEFT;
                    //table.AddCell(clTituloMun);

                    //string _NombreMunicipio = this.LblNombreMunicipio.Text.ToString().Trim().ToUpper();
                    //PdfPCell clNombreMunicipio = new PdfPCell(new Phrase(_NombreMunicipio, _standardFont1));
                    //clNombreMunicipio.BorderWidth = 0;
                    //clNombreMunicipio.Colspan = 2;
                    //clNombreMunicipio.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(clNombreMunicipio);

                    //string _FechaMaxPres = this.CmbFecha1.SelectedValue.ToString().Trim() + "/" + this.CmbFecha2.SelectedValue.ToString().Trim() + "/" + this.CmbFecha3.SelectedValue.ToString().Trim();
                    //string _FechaMaxPresentacion = "Fecha Máxima Presentación\n" + _FechaMaxPres;
                    //PdfPCell clFechaMax = new PdfPCell(new Phrase(_FechaMaxPresentacion, _standardFont1));
                    //clFechaMax.HorizontalAlignment = Element.ALIGN_CENTER;
                    //clFechaMax.Rowspan = 2;
                    //clFechaMax.Colspan = 2;
                    //table.AddCell(clFechaMax);

                    ////--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 2.
                    //string _TituloDpto = "DEPARTAMENTO:";
                    //PdfPCell clTituloDpto = new PdfPCell(new Phrase(_TituloDpto, _standardFont1));
                    //clTituloDpto.HorizontalAlignment = Element.ALIGN_LEFT;
                    //table.AddCell(clTituloDpto);

                    //string _NombreDpto = this.LblNombreDpto.Text.ToString().Trim().ToUpper();
                    //PdfPCell clNombreDpto = new PdfPCell(new Phrase(_NombreDpto, _standardFont1));
                    ////clTerminal.BorderWidth = 0;
                    //clNombreDpto.Colspan = 2;
                    //clNombreDpto.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(clNombreDpto);

                    ////--AQUI DEFINIMOS LOS DATOS DEL ENCABEZADO 2.
                    //string _TituloAnio = "AÑO GRAVABLE: " + this.TxtAnioGravable.Text.ToString().Trim();
                    //PdfPCell clTituloAnio = new PdfPCell(new Phrase(_TituloAnio, _standardFont1));
                    //clTituloAnio.PaddingTop = 9f;
                    //clTituloAnio.VerticalAlignment = Element.ALIGN_CENTER;
                    ////clTituloAnio.HorizontalAlignment = Element.ALIGN_LEFT;
                    //table.AddCell(clTituloAnio);

                    //string _ImgDefault = "Imagenes/Modulos/img_cuadro.png";
                    //string _RutaImagen = HttpContext.Current.Server.MapPath("/" + _ImgDefault.ToString().Trim());
                    ////--AQUI OBTENEMOS EL ARREGLO DE BYTE DE LA IMAGEN.
                    //byte[] imageBytes = ObjUtils.GetImagenBytes(_RutaImagen);
                    //iTextSharp.text.Image ImgHuella = iTextSharp.text.Image.GetInstance(imageBytes);
                    //ImgHuella.ScaleAbsolute(700f, 40f);
                    //PdfPCell clImg3 = new PdfPCell(ImgHuella);
                    ////clImg3.Border = 0;
                    //clImg3.Colspan = 3;
                    //clImg3.PaddingTop = 5f;
                    //clImg3.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(clImg3);

                    //Font _standardFont2 = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.NORMAL, BaseColor.BLACK);
                    //string _OpcionUso = "";
                    //if (this.RbDeclaracionInicial.Checked)
                    //{
                    //    _OpcionUso = "OPCIÓN DE USO            DECLARACIÓN INICIAL: X        SOLO PAGO:          CORRECCIÓN:          Declaración que corrige No.:";
                    //}
                    //if (this.RbSoloPago.Checked)
                    //{
                    //    _OpcionUso = "OPCIÓN DE USO            DECLARACIÓN INICIAL:          SOLO PAGO: X        CORRECCIÓN:          Declaración que corrige No.:";
                    //}
                    //if (this.RbCorreccion.Checked)
                    //{
                    //    _OpcionUso = "OPCIÓN DE USO            DECLARACIÓN INICIAL:          SOLO PAGO:          CORRECCIÓN: X        Declaración que corrige No.: " + this.TxtNumCorreccion.Text.ToString().Trim();
                    //}

                    //string _Opciones5 = "Fecha: " + DateTime.Now.ToString("dd/MM/yyy");
                    //PdfPCell clOpciones = new PdfPCell(new Phrase(_OpcionUso + "          " + _Opciones5, _standardFont2));
                    //clOpciones.Colspan = 4;
                    //clOpciones.PaddingTop = 5f;
                    //clOpciones.VerticalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(clOpciones);

                    ////--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    //doc.Add(table);
                    #endregion

                    //--AQUI DEFINIMOS LOS TIPOS DE LETRAS Y TAMAÑO DE LAS SESIONES
                    Font _FontSecciones = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.NORMAL, BaseColor.BLACK);
                    Font _FontFirmas = new Font(Font.FontFamily.TIMES_ROMAN, 5, Font.NORMAL, BaseColor.BLACK);
                    Font _FontDetalle = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.NORMAL, BaseColor.BLACK);
                    Font _FontCodigosQr = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL, BaseColor.BLACK);
                    Font _standardFontRenglon = new Font(Font.FontFamily.TIMES_ROMAN, 6, Font.BOLD, BaseColor.BLACK);

                    #region SESION A. INFORMACION DEL CONTRIBUYENTE
                    PdfPTable TblSeccionA = new PdfPTable(6);
                    TblSeccionA.TotalWidth = _AnchoTabla;
                    TblSeccionA.LockedWidth = true;
                    TblSeccionA.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionA.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.SpacingAfter = 0;
                    float[] TblWidthsA = new float[6];
                    TblWidthsA[0] = 24.8f;
                    TblWidthsA[1] = 11f;
                    TblWidthsA[2] = 100f;
                    TblWidthsA[3] = 100f;
                    TblWidthsA[4] = 70f;
                    TblWidthsA[5] = 70f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionA.SetWidths(TblWidthsA);

                    string _SeccionA = "A. INFORMACIÓN DEL\nCONTRIBUYENTE";
                    //PdfPCell clSeccionA = new PdfPCell(new Phrase(_SeccionA, _FontSecciones)) { Rotation = 90, PaddingTop = 6f, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                    PdfPCell clSeccionA = new PdfPCell(new Phrase(_SeccionA, _FontSecciones));
                    clSeccionA.Rowspan = 6;
                    clSeccionA.Colspan = 1;
                    //clSeccionB.BorderWidth = 1;
                    clSeccionA.Rotation = 90;
                    clSeccionA.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionA.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.AddCell(clSeccionA);

                    //--RENGLON 1
                    PdfPCell clRenglon1 = new PdfPCell(new Phrase("1", _standardFontRenglon));
                    clRenglon1.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.AddCell(clRenglon1);

                    string _NombreCliente = "NOMBRES Y APELLIDOS O RAZÓN SOCIAL\n" + rowItem["nombre_cliente"].ToString().Trim();
                    PdfPCell clNombreCliente = new PdfPCell(new Phrase(_NombreCliente, _FontDetalle));
                    clNombreCliente.Colspan = 4;
                    clNombreCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clNombreCliente);
                    //TblSeccionA.AddCell("A");

                    //--RENGLON 2
                    PdfPCell clRenglon2 = new PdfPCell(new Phrase("2", _standardFontRenglon));
                    clRenglon2.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.AddCell(clRenglon2);

                    #region MOSTRAR EL TIPO DE DOCUMENTO
                    string _DocCliente = "";
                    int _IdTipoDocumento = Int32.Parse(rowItem["idtipo_identificacion"].ToString().Trim());
                    if (_IdTipoDocumento == 1)
                    {
                        _DocCliente = "C.C.: X  NIT.:    TI.:    C.E.:    " + rowItem["numero_documento"].ToString().Trim() + "   " + rowItem["digito_verificacion"].ToString().Trim();
                    }
                    if (_IdTipoDocumento == 2)
                    {
                        _DocCliente = "C.C.:    NIT.: X  TI.:    C.E.:    " + rowItem["numero_documento"].ToString().Trim() + "   " + rowItem["digito_verificacion"].ToString().Trim();
                    }
                    if (_IdTipoDocumento == 3)
                    {
                        _DocCliente = "C.C.:    NIT.:    TI.:    C.E.: X  " + rowItem["numero_documento"].ToString().Trim() + "   " + rowItem["digito_verificacion"].ToString().Trim();
                    }

                    PdfPCell clDocumCliente = new PdfPCell(new Phrase(_DocCliente, _FontDetalle));
                    //clDocumCliente.Colspan = 2;
                    clDocumCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clDocumCliente);
                    #endregion

                    #region MOSTRAR SI ES UNION TEMPORAL
                    string _DescUnionTemporal = "";
                    string _UnionTemporal = rowItem["consorcio_union_temporal"].ToString().Trim();
                    string _PatrimonioAut = rowItem["actividad_patrim_autonomo"].ToString().Trim();
                    if (_UnionTemporal.Trim().Equals("S") && _PatrimonioAut.Trim().Equals("S"))
                    {
                        _DescUnionTemporal = "Es Consorcio o Unión Temporal:  X   Realiza actividades a través de Patrimonio Autónomo:  X";
                    }
                    else
                    {
                        if (_UnionTemporal.Trim().Equals("S"))
                        {
                            _DescUnionTemporal = "Es Consorcio o Unión Temporal:  X   Realiza actividades a través de Patrimonio Autónomo:";
                        }
                        else if (_PatrimonioAut.Trim().Equals("S"))
                        {
                            _DescUnionTemporal = "Es Consorcio o Unión Temporal:      Realiza actividades a través de Patrimonio Autónomo:  X";
                        }
                        else
                        {
                            _DescUnionTemporal = "Es Consorcio o Unión Temporal:      Realiza actividades a través de Patrimonio Autónomo:";
                        }
                    }

                    PdfPCell clUnionTemporal = new PdfPCell(new Phrase(_DescUnionTemporal, _FontDetalle));
                    clUnionTemporal.Colspan = 3;
                    clUnionTemporal.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clUnionTemporal);
                    //TblSeccionA.AddCell("B");
                    #endregion

                    //--RENGLON 3
                    PdfPCell clRenglon3 = new PdfPCell(new Phrase("3", _standardFontRenglon));
                    clRenglon3.Rowspan = 2;
                    clRenglon3.PaddingTop = 11f;
                    clRenglon3.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.AddCell(clRenglon3);

                    string _DirecCliente = "DIRECCIÓN DE NOTIFICACIÓN\n" + rowItem["direccion_cliente"].ToString().Trim();
                    PdfPCell clDirecCliente = new PdfPCell(new Phrase(_DirecCliente, _FontDetalle));
                    clDirecCliente.Colspan = 4;
                    clDirecCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clDirecCliente);

                    string _MunicipioDistrito = "MUNICIPIO O DISTRITO DE LA DIRECCIÓN\n" + rowItem["municipio_cliente"].ToString().Trim();
                    PdfPCell clMunDistrito = new PdfPCell(new Phrase(_MunicipioDistrito, _FontDetalle));
                    //clMunDistrito.Colspan = 2;
                    clMunDistrito.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clMunDistrito);

                    string _Dpto = "DEPARTAMENTO\n" + rowItem["dpto_cliente"].ToString().Trim();
                    PdfPCell clDpto = new PdfPCell(new Phrase(_Dpto, _FontDetalle));
                    clDpto.Colspan = 4;
                    clDpto.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clDpto);

                    ////--RENGLON 4
                    PdfPCell clRenglon4 = new PdfPCell(new Phrase("4", _standardFontRenglon));
                    clRenglon4.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionA.AddCell(clRenglon4);

                    string _TelefCliente = "TELÉFONO\n" + rowItem["telefono_contacto"].ToString().Trim();
                    PdfPCell clTelefCliente = new PdfPCell(new Phrase(_TelefCliente, _FontDetalle));
                    clTelefCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clTelefCliente);

                    string _EmailCliente = "5. CORREO ELECTRÓNICO\n" + rowItem["email_contacto"].ToString().Trim();
                    PdfPCell clEmailCliente = new PdfPCell(new Phrase(_EmailCliente, _FontDetalle));
                    clEmailCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clEmailCliente);

                    string _NumEstablecimiento = "6. N° DE ESTABLECIMIENTOS\n" + rowItem["numero_puntos"].ToString().Trim().PadLeft(30, ' ');
                    PdfPCell clNumEstablecimiento = new PdfPCell(new Phrase(_NumEstablecimiento, _FontDetalle));
                    clNumEstablecimiento.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clNumEstablecimiento);

                    string _ClasificacionCliente = "7. CLASIFICACIÓN\n" + rowItem["tipo_clasificacion"].ToString().Trim();
                    PdfPCell clClasificacionCliente = new PdfPCell(new Phrase(_ClasificacionCliente, _FontDetalle));
                    clClasificacionCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionA.AddCell(clClasificacionCliente);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionA);
                    #endregion

                    #region SECCION B. BASE GRAVABLE
                    PdfPTable TblSeccionB = new PdfPTable(4);
                    TblSeccionB.TotalWidth = _AnchoTabla;
                    TblSeccionB.LockedWidth = true;
                    TblSeccionB.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionB.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.SpacingAfter = 0;
                    float[] TblWidthsB = new float[4];
                    TblWidthsB[0] = 20.5f;
                    TblWidthsB[1] = 9f;
                    TblWidthsB[2] = 200f;
                    TblWidthsB[3] = 80f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionB.SetWidths(TblWidthsB);

                    string _SeccionB = "B. BASE GRAVABLE";
                    //PdfPCell clSeccionB = new PdfPCell(new Phrase(_SeccionB, _FontSecciones)) { Rotation = 90, PaddingTop = 6f, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                    PdfPCell clSeccionB = new PdfPCell(new Phrase(_SeccionB, _FontSecciones));
                    clSeccionB.Rowspan = 9;
                    clSeccionB.Colspan = 1;
                    //clSeccionB.BorderWidth = 1;
                    clSeccionB.Rotation = 90;
                    clSeccionB.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionB.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clSeccionB);

                    //--RENGLON 8
                    PdfPCell clRenglon8 = new PdfPCell(new Phrase("8", _standardFontRenglon));
                    clRenglon8.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon8);

                    string _DescRenglon8 = "TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS DEL PERIODO EN TODO EL PAÍS";
                    PdfPCell clDescRenglon8 = new PdfPCell(new Phrase(_DescRenglon8, _FontDetalle));
                    clDescRenglon8.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon8);

                    string _ValorRenglon8 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon8"].ToString().Trim())));
                    PdfPCell clValorRenglon8 = new PdfPCell(new Phrase(_ValorRenglon8, _FontDetalle));
                    clValorRenglon8.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon8);

                    //--RENGLON 9
                    PdfPCell clRenglon9 = new PdfPCell(new Phrase("9", _standardFontRenglon));
                    clRenglon9.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon9);

                    string _DescRenglon9 = "      MENOS INGRESOS FUERA DE ESTE MUNICIPIO O DISTRITO";
                    PdfPCell clDescRenglon9 = new PdfPCell(new Phrase(_DescRenglon9, _FontDetalle));
                    clDescRenglon9.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon9);

                    string _ValorRenglon9 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon9"].ToString().Trim())));
                    PdfPCell clValorRenglon9 = new PdfPCell(new Phrase(_ValorRenglon9, _FontDetalle));
                    clValorRenglon9.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon9);

                    //--RENGLON 10
                    PdfPCell clRenglon10 = new PdfPCell(new Phrase("10", _standardFontRenglon));
                    clRenglon10.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon10);

                    string _DescRenglon10 = "TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS EN ESTE MUNICIPIO (RENGLÓN 8 MENOS 9)";
                    PdfPCell clDescRenglon10 = new PdfPCell(new Phrase(_DescRenglon10, _FontDetalle));
                    clDescRenglon10.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon10);

                    string _ValorRenglon10 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon10"].ToString().Trim())));
                    PdfPCell clValorRenglon10 = new PdfPCell(new Phrase(_ValorRenglon10, _FontDetalle));
                    clValorRenglon10.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon10);

                    //--RENGLON 11
                    PdfPCell clRenglon11 = new PdfPCell(new Phrase("11", _standardFontRenglon));
                    clRenglon11.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon11);

                    string _DescRenglon11 = "     MENOS INGRESOS POR DEVOLUCIONES, REBAJAS, DESCUENTOS";
                    PdfPCell clDescRenglon11 = new PdfPCell(new Phrase(_DescRenglon11, _FontDetalle));
                    clDescRenglon11.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon11);

                    string _ValorRenglon11 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon11"].ToString().Trim())));
                    PdfPCell clValorRenglon11 = new PdfPCell(new Phrase(_ValorRenglon11, _FontDetalle));
                    clValorRenglon11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon11);

                    //--RENGLON 12
                    PdfPCell clRenglon12 = new PdfPCell(new Phrase("12", _standardFontRenglon));
                    clRenglon12.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon12);

                    string _DescRenglon12 = "     MENOS INGRESOS POR EXPORTACIONES";
                    PdfPCell clDescRenglon12 = new PdfPCell(new Phrase(_DescRenglon12, _FontDetalle));
                    clDescRenglon12.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon12);

                    string _ValorRenglon12 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon12"].ToString().Trim())));
                    PdfPCell clValorRenglon12 = new PdfPCell(new Phrase(_ValorRenglon12, _FontDetalle));
                    clValorRenglon12.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon12);

                    //--RENGLON 13
                    PdfPCell clRenglon13 = new PdfPCell(new Phrase("13", _standardFontRenglon));
                    clRenglon13.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon13);

                    string _DescRenglon13 = "     MENOS INGRESOS POR VENTAS DE ACTIVOS FIJOS";
                    PdfPCell clDescRenglon13 = new PdfPCell(new Phrase(_DescRenglon13, _FontDetalle));
                    clDescRenglon13.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon13);

                    string _ValorRenglon13 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon13"].ToString().Trim())));
                    PdfPCell clValorRenglon13 = new PdfPCell(new Phrase(_ValorRenglon13, _FontDetalle));
                    clValorRenglon13.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon13);

                    //--RENGLON 14
                    PdfPCell clRenglon14 = new PdfPCell(new Phrase("14", _standardFontRenglon));
                    clRenglon14.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon14);

                    string _DescRenglon14 = "     MENOS INGRESOS POR ACTIVIDADES EXCLUIDAS O NO SUJETAS Y OTROS INGRESOS NO GRAVADOS";
                    PdfPCell clDescRenglon14 = new PdfPCell(new Phrase(_DescRenglon14, _FontDetalle));
                    clDescRenglon14.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon14);

                    string _ValorRenglon14 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon14"].ToString().Trim())));
                    PdfPCell clValorRenglon14 = new PdfPCell(new Phrase(_ValorRenglon14, _FontDetalle));
                    clValorRenglon14.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon14);

                    //--RENGLON 15
                    PdfPCell clRenglon15 = new PdfPCell(new Phrase("15", _standardFontRenglon));
                    clRenglon15.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon15);

                    string _DescRenglon15 = "     MENOS INGRESOS POR OTRAS ACTIVIDADES EXENTAS EN ESTE MUNICIPIO O DISTRITO (POR ACUERDO)";
                    PdfPCell clDescRenglon15 = new PdfPCell(new Phrase(_DescRenglon15, _FontDetalle));
                    clDescRenglon15.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon15);

                    string _ValorRenglon15 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon15"].ToString().Trim())));
                    PdfPCell clValorRenglon15 = new PdfPCell(new Phrase(_ValorRenglon15, _FontDetalle));
                    clValorRenglon15.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon15);

                    //--RENGLON 16
                    PdfPCell clRenglon16 = new PdfPCell(new Phrase("16", _standardFontRenglon));
                    clRenglon16.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionB.AddCell(clRenglon16);

                    string _DescRenglon16 = "TOTAL INGRESOS GRAVABLES (RENGLÓN 10 MENOS 11, 12, 13, 14 Y 15)";
                    PdfPCell clDescRenglon16 = new PdfPCell(new Phrase(_DescRenglon16, _FontDetalle));
                    clDescRenglon16.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionB.AddCell(clDescRenglon16);

                    string _ValorRenglon16 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon16"].ToString().Trim())));
                    PdfPCell clValorRenglon16 = new PdfPCell(new Phrase(_ValorRenglon16, _FontDetalle));
                    clValorRenglon16.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionB.AddCell(clValorRenglon16);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionB);
                    #endregion

                    #region SECCION C. DISCRIMINACIÓN DE ACTIVIDADES GRAVADAS
                    PdfPTable TblSeccionC = new PdfPTable(6);
                    TblSeccionC.TotalWidth = _AnchoTabla;
                    TblSeccionC.LockedWidth = true;
                    TblSeccionC.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionC.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.SpacingAfter = 0;
                    float[] TblWidthsC = new float[6];
                    TblWidthsC[0] = 23f;
                    TblWidthsC[1] = 70f;
                    TblWidthsC[2] = 50f;
                    TblWidthsC[3] = 53f;
                    TblWidthsC[4] = 62f;
                    TblWidthsC[5] = 90f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionC.SetWidths(TblWidthsC);

                    string _SeccionC = "C. DISCRIMINACIÓN DE\nACTIVIDADES GRAVADAS";
                    PdfPCell clSeccionC = new PdfPCell(new Phrase(_SeccionC, _FontSecciones));
                    clSeccionC.Rowspan = 7;
                    clSeccionC.Colspan = 1;
                    //clSeccionB.BorderWidth = 1;
                    clSeccionC.Rotation = 90;
                    clSeccionC.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionC.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clSeccionC);

                    #region DEFINIR NOMBRE DE COLUMNAS DE LA TABLA
                    //--DEFINIR NOMBRE DE COLUMNAS DE LA TABLA
                    PdfPCell clActGravadas = new PdfPCell(new Phrase("ACTIVIDADES GRAVADAS", _FontDetalle));
                    clActGravadas.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clActGravadas);

                    string _CodigoAct = "CÓDIGO";
                    PdfPCell clCodigoAct = new PdfPCell(new Phrase(_CodigoAct, _FontDetalle));
                    clCodigoAct.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clCodigoAct);

                    string _IngGravados = "INGRESOS GRAVADOS";
                    PdfPCell clIngGravados = new PdfPCell(new Phrase(_IngGravados, _FontDetalle));
                    clIngGravados.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados);

                    string _TarifaxMil = "TARIFA (POR MIL)";
                    PdfPCell clTarifaxMil = new PdfPCell(new Phrase(_TarifaxMil, _FontDetalle));
                    //clTarifaxMil.Colspan = 2;
                    clTarifaxMil.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaxMil);

                    PdfPCell clValorAct = new PdfPCell(new Phrase("", _FontDetalle));
                    //clValorAct.Colspan = 2;
                    clValorAct.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clValorAct);
                    #endregion

                    #region DEFINIR VALORES DE COLUMNAS DE LA TABLA
                    //--DEFINIR VALORES CAMPO ACTIVIDAD 1
                    PdfPCell clActPrincipal1 = new PdfPCell(new Phrase("ACTIVIDAD 1 (PRINCIPAL)", _FontDetalle));
                    clActPrincipal1.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActPrincipal1);

                    string _CodigoAct1 = rowItem["codigo_actividad1"].ToString().Trim();
                    PdfPCell clCodigoAct1 = new PdfPCell(new Phrase(_CodigoAct1, _FontDetalle));
                    clCodigoAct1.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clCodigoAct1);

                    string _IngGravados1 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_actividad1"].ToString().Trim())));
                    PdfPCell clIngGravados1 = new PdfPCell(new Phrase(_IngGravados1, _FontDetalle));
                    clIngGravados1.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados1);

                    string _TarifaAct1 = rowItem["tarifa_actividad1"].ToString().Trim();
                    PdfPCell clTarifaAct1 = new PdfPCell(new Phrase(_TarifaAct1, _FontDetalle));
                    clTarifaAct1.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct1);

                    string _ValorAct1 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon17"].ToString().Trim())));
                    PdfPCell clValorAct1 = new PdfPCell(new Phrase(_ValorAct1, _FontDetalle));
                    clValorAct1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct1);

                    //--DEFINIR VALORES CAMPO ACTIVIDAD 2
                    PdfPCell clActividad2 = new PdfPCell(new Phrase("ACTIVIDAD 2", _FontDetalle));
                    clActividad2.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActividad2);

                    string _CodigoAct2 = rowItem["codigo_actividad2"].ToString().Trim();
                    PdfPCell clCodigoAct2 = new PdfPCell(new Phrase(_CodigoAct2, _FontDetalle));
                    clCodigoAct2.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clCodigoAct2);

                    string _IngGravados2 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_actividad2"].ToString().Trim())));
                    PdfPCell clIngGravados2 = new PdfPCell(new Phrase(_IngGravados2, _FontDetalle));
                    clIngGravados2.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados2);

                    string _TarifaAct2 = rowItem["tarifa_actividad2"].ToString().Trim();
                    PdfPCell clTarifaAct2 = new PdfPCell(new Phrase(_TarifaAct2, _FontDetalle));
                    clTarifaAct2.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct2);

                    string _ValorAct2 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon18"].ToString().Trim())));
                    PdfPCell clValorAct2 = new PdfPCell(new Phrase(_ValorAct2, _FontDetalle));
                    clValorAct2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct2);

                    //--DEFINIR VALORES CAMPO ACTIVIDAD 3
                    PdfPCell clActividad3 = new PdfPCell(new Phrase("ACTIVIDAD 3", _FontDetalle));
                    clActividad3.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActividad3);

                    string _CodigoAct3 = rowItem["codigo_actividad3"].ToString().Trim();
                    PdfPCell clCodigoAct3 = new PdfPCell(new Phrase(_CodigoAct3, _FontDetalle));
                    clCodigoAct3.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clCodigoAct3);

                    string _IngGravados3 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_actividad3"].ToString().Trim())));
                    PdfPCell clIngGravados3 = new PdfPCell(new Phrase(_IngGravados3, _FontDetalle));
                    clIngGravados3.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados3);

                    string _TarifaAct3 = rowItem["tarifa_actividad3"].ToString().Trim();
                    PdfPCell clTarifaAct3 = new PdfPCell(new Phrase(_TarifaAct3, _FontDetalle));
                    clTarifaAct3.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct3);

                    string _ValorAct3 = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(rowItem["valor_renglon19"].ToString().Trim())));
                    PdfPCell clValorAct3 = new PdfPCell(new Phrase(_ValorAct3, _FontDetalle));
                    clValorAct3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct3);

                    //--DEFINIR VALORES CAMPO ACTIVIDAD 4
                    PdfPCell clActividad4 = new PdfPCell(new Phrase("OTRAS ACTIVIDADES", _FontDetalle));
                    clActividad4.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActividad4);

                    string _CodigoAct4 = "VER DESAGREGACIÓN";
                    PdfPCell clCodigoAct4 = new PdfPCell(new Phrase(_CodigoAct4, _FontDetalle));
                    clCodigoAct4.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clCodigoAct4);

                    string _IngGravados4 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_actividad4"].ToString().Trim())));
                    PdfPCell clIngGravados4 = new PdfPCell(new Phrase(_IngGravados4, _FontDetalle));
                    clIngGravados4.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados4);

                    string _TarifaAct4 = rowItem["tarifa_actividad4"].ToString().Trim();
                    PdfPCell clTarifaAct4 = new PdfPCell(new Phrase(_TarifaAct4, _FontDetalle));
                    clTarifaAct4.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct4);

                    string _ValorAct4 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_otras_act"].ToString().Trim())));
                    PdfPCell clValorAct4 = new PdfPCell(new Phrase(_ValorAct4, _FontDetalle));
                    clValorAct4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct4);

                    //--DEFINIR VALORES TOTAL INGRESOS GRAVADOS
                    PdfPCell clActividad5 = new PdfPCell(new Phrase("TOTAL INGRESOS GRAVADOS", _FontDetalle));
                    clActividad5.Colspan = 2;
                    clActividad5.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActividad5);

                    string _IngGravados5 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["total_ingresos_gravado"].ToString().Trim())));
                    PdfPCell clIngGravados5 = new PdfPCell(new Phrase(_IngGravados5, _FontDetalle));
                    clIngGravados5.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clIngGravados5);

                    PdfPCell clTarifaAct5 = new PdfPCell(new Phrase("17. TOTAL IMPUESTO", _FontDetalle));
                    clTarifaAct5.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct5);

                    string _ValorAct5 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["total_impuestos"].ToString().Trim())));
                    PdfPCell clValorAct5 = new PdfPCell(new Phrase(_ValorAct5, _FontDetalle));
                    clValorAct5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct5);

                    //--DEFINIR VALORES GENERACIÓN DE ENERGIA
                    PdfPCell clActividad6 = new PdfPCell(new Phrase("18. GENERACIÓN DE ENERGIA CAPACIDAD INSTALADA", _FontDetalle));
                    clActividad6.Colspan = 3;
                    clActividad6.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionC.AddCell(clActividad6);

                    PdfPCell clTarifaAct6 = new PdfPCell(new Phrase("19. IMPUESTO LEY 56 DE 1981", _FontDetalle));
                    clTarifaAct6.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionC.AddCell(clTarifaAct6);

                    string _ValorAct6 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon19"].ToString().Trim())));
                    PdfPCell clValorAct6 = new PdfPCell(new Phrase(_ValorAct6, _FontDetalle));
                    clValorAct6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionC.AddCell(clValorAct6);
                    #endregion

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionC);
                    #endregion

                    #region SECCION D. LIQUIDACIÓN PRIVADA
                    PdfPTable TblSeccionD = new PdfPTable(4);
                    TblSeccionD.TotalWidth = _AnchoTabla;
                    TblSeccionD.LockedWidth = true;
                    TblSeccionD.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionD.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.SpacingAfter = 0;
                    float[] TblWidthsD = new float[4];
                    TblWidthsD[0] = 20.5f;
                    TblWidthsD[1] = 9f;
                    TblWidthsD[2] = 200f;
                    TblWidthsD[3] = 80f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionD.SetWidths(TblWidthsD);

                    string _SeccionD = "D. LIQUIDACIÓN PRIVADA";
                    PdfPCell clSeccionD = new PdfPCell(new Phrase(_SeccionD, _FontSecciones));
                    clSeccionD.Rowspan = 15;
                    clSeccionD.Colspan = 1;
                    //clSeccionD.BorderWidth = 1;
                    clSeccionD.Rotation = 90;
                    clSeccionD.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionD.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clSeccionD);

                    //--RENGLON 20
                    PdfPCell clRenglon20 = new PdfPCell(new Phrase("20", _standardFontRenglon));
                    clRenglon20.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon20);

                    string _DescRenglon20 = "TOTAL IMPUESTO DE INDUSTRIA Y COMERCIO (RENGLÓN 17 + 19)";
                    PdfPCell clDescRenglon20 = new PdfPCell(new Phrase(_DescRenglon20, _FontDetalle));
                    clDescRenglon20.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon20);

                    string _ValorRenglon20 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon20"].ToString().Trim())));
                    PdfPCell clValorRenglon20 = new PdfPCell(new Phrase(_ValorRenglon20, _FontDetalle));
                    clValorRenglon20.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon20);

                    //--RENGLON 21
                    PdfPCell clRenglon21 = new PdfPCell(new Phrase("21", _standardFontRenglon));
                    clRenglon21.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon21);

                    string _DescRenglon21 = "IMPUESTOS DE AVISOS Y TABLEROS (15% del renglón 20)";
                    PdfPCell clDescRenglon21 = new PdfPCell(new Phrase(_DescRenglon21, _FontDetalle));
                    clDescRenglon21.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon21);

                    string _ValorRenglon21 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon21"].ToString().Trim())));
                    PdfPCell clValorRenglon21 = new PdfPCell(new Phrase(_ValorRenglon21, _FontDetalle));
                    clValorRenglon21.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon21);

                    //--RENGLON 22
                    PdfPCell clRenglon22 = new PdfPCell(new Phrase("22", _standardFontRenglon));
                    clRenglon22.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon22);

                    string _DescRenglon22 = "PAGO POR UNIDADES COMERCIALES ADICIONALES DEL SECTOR FINANCIERO";
                    PdfPCell clDescRenglon22 = new PdfPCell(new Phrase(_DescRenglon22, _FontDetalle));
                    clDescRenglon22.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon22);

                    string _ValorRenglon22 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon22"].ToString().Trim())));
                    PdfPCell clValorRenglon22 = new PdfPCell(new Phrase(_ValorRenglon22, _FontDetalle));
                    clValorRenglon22.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon22);

                    //--RENGLON 23
                    PdfPCell clRenglon23 = new PdfPCell(new Phrase("23", _standardFontRenglon));
                    clRenglon23.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon23);

                    string _DescRenglon23 = "SOBRETASA BOMBERIL (Ley 1575 de 2012) (si la hay, liquide según el acuerdo Municipal o distrital)";
                    PdfPCell clDescRenglon23 = new PdfPCell(new Phrase(_DescRenglon23, _FontDetalle));
                    clDescRenglon23.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon23);

                    string _ValorRenglon23 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon23"].ToString().Trim())));
                    PdfPCell clValorRenglon23 = new PdfPCell(new Phrase(_ValorRenglon23, _FontDetalle));
                    clValorRenglon23.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon23);

                    //--RENGLON 24
                    PdfPCell clRenglon24 = new PdfPCell(new Phrase("24", _standardFontRenglon));
                    clRenglon24.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon24);

                    string _DescRenglon24 = "SOBRETASA DE SEGURIDAD (LEY 1421 de 2011) (SI la hay, liquídela según el acuerdo Municipal o distrital)";
                    PdfPCell clDescRenglon24 = new PdfPCell(new Phrase(_DescRenglon24, _FontDetalle));
                    clDescRenglon24.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon24);

                    string _ValorRenglon24 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon24"].ToString().Trim())));
                    PdfPCell clValorRenglon24 = new PdfPCell(new Phrase(_ValorRenglon24, _FontDetalle));
                    clValorRenglon24.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon24);

                    //--RENGLON 25
                    PdfPCell clRenglon25 = new PdfPCell(new Phrase("25", _standardFontRenglon));
                    clRenglon25.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon25);

                    string _DescRenglon25 = "TOTAL IMPUESTO A CARGO (RENGLÓN 20 + 21 + 22 + 23 + 24)";
                    PdfPCell clDescRenglon25 = new PdfPCell(new Phrase(_DescRenglon25, _FontDetalle));
                    clDescRenglon25.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon25);

                    string _ValorRenglon25 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon25"].ToString().Trim())));
                    PdfPCell clValorRenglon25 = new PdfPCell(new Phrase(_ValorRenglon25, _FontDetalle));
                    clValorRenglon25.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon25);

                    //--RENGLON 26
                    PdfPCell clRenglon26 = new PdfPCell(new Phrase("26", _standardFontRenglon));
                    clRenglon26.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon26);

                    string _DescRenglon26 = "     MENOS VALOR DE EXENCIÓN O EXONERACIÓN SOBRE EL IMPUESTO Y NO SOBRE LOS INGRESOS";
                    PdfPCell clDescRenglon26 = new PdfPCell(new Phrase(_DescRenglon26, _FontDetalle));
                    clDescRenglon26.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon26);

                    string _ValorRenglon26 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon26"].ToString().Trim())));
                    PdfPCell clValorRenglon26 = new PdfPCell(new Phrase(_ValorRenglon26, _FontDetalle));
                    clValorRenglon26.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon26);

                    //--RENGLON 27
                    PdfPCell clRenglon27 = new PdfPCell(new Phrase("27", _standardFontRenglon));
                    clRenglon27.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon27);

                    string _DescRenglon27 = "     MENOS RETENCIONES que le practicaron a favor de este municipio o distrito en este periodo";
                    PdfPCell clDescRenglon27 = new PdfPCell(new Phrase(_DescRenglon27, _FontDetalle));
                    clDescRenglon27.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon27);

                    string _ValorRenglon27 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon27"].ToString().Trim())));
                    PdfPCell clValorRenglon27 = new PdfPCell(new Phrase(_ValorRenglon27, _FontDetalle));
                    clValorRenglon27.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon27);

                    //--RENGLON 28
                    PdfPCell clRenglon28 = new PdfPCell(new Phrase("28", _standardFontRenglon));
                    clRenglon28.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon28);

                    string _DescRenglon28 = "     MENOS AUTORETENCIONES practicadas a favor de este municipio o distrito en este periodo";
                    PdfPCell clDescRenglon28 = new PdfPCell(new Phrase(_DescRenglon28, _FontDetalle));
                    clDescRenglon28.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon28);

                    string _ValorRenglon28 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon28"].ToString().Trim())));
                    PdfPCell clValorRenglon28 = new PdfPCell(new Phrase(_ValorRenglon28, _FontDetalle));
                    clValorRenglon28.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon28);

                    //--RENGLON 29
                    PdfPCell clRenglon29 = new PdfPCell(new Phrase("29", _standardFontRenglon));
                    clRenglon29.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon29);

                    string _DescRenglon29 = "     MENOS ANTICIPO LIQUIDADO EN EL AÑO ANTERIOR";
                    PdfPCell clDescRenglon29 = new PdfPCell(new Phrase(_DescRenglon29, _FontDetalle));
                    clDescRenglon29.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon29);

                    string _ValorRenglon29 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon29"].ToString().Trim())));
                    PdfPCell clValorRenglon29 = new PdfPCell(new Phrase(_ValorRenglon29, _FontDetalle));
                    clValorRenglon29.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon29);

                    //--RENGLON 30
                    PdfPCell clRenglon30 = new PdfPCell(new Phrase("30", _standardFontRenglon));
                    clRenglon30.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon30);

                    string _DescRenglon30 = "ANTICIPO DEL AÑO SIGUIENTE (Si existe, liquide porcentaje según Acuerdo Municipal o Distrital)";
                    PdfPCell clDescRenglon30 = new PdfPCell(new Phrase(_DescRenglon30, _FontDetalle));
                    clDescRenglon30.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon30);

                    string _ValorRenglon30 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon30"].ToString().Trim())));
                    PdfPCell clValorRenglon30 = new PdfPCell(new Phrase(_ValorRenglon30, _FontDetalle));
                    clValorRenglon30.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon30);

                    //--RENGLON 31
                    PdfPCell clRenglon31 = new PdfPCell(new Phrase("31", _standardFontRenglon));
                    clRenglon31.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon31);

                    #region AQUI VALIDAMOS LA OPCION DE SANCION
                    string _DescripcionSancion = rowItem["descripcion_sancion_otro"].ToString().Trim().ToUpper();
                    string _DescRenglon31 = "";

                    //--AQUI VALIDAMOS EL TIPO DE SANCION.
                    string _Sancion = rowItem["sanciones"].ToString().Trim();
                    if (_Sancion.Equals("1"))
                    {
                        _DescRenglon31 = "SANCIONES Extemporaneidad:  X   Corrección:     Inexactitud:        Otra Cuál:___________";
                    }
                    else if (_Sancion.Equals("2"))
                    {
                        _DescRenglon31 = "SANCIONES Extemporaneidad:     Corrección:  X   Inexactitud:        Otra Cuál:___________";
                    }
                    else if (_Sancion.Equals("3"))
                    {
                        _DescRenglon31 = "SANCIONES Extemporaneidad:     Corrección:     Inexactitud:   X     Otra Cuál:___________";
                    }
                    else if (_Sancion.Equals("4"))
                    {
                        _DescRenglon31 = "SANCIONES Extemporaneidad:     Corrección:     Inexactitud:        Otra X Cuál: " + _DescripcionSancion;
                    }
                    else
                    {
                        _DescRenglon31 = "SANCIONES Extemporaneidad:     Corrección:     Inexactitud:        Otra X Cuál: NO IDENTIFICADA";
                    }
                    #endregion

                    PdfPCell clDescRenglon31 = new PdfPCell(new Phrase(_DescRenglon31, _FontDetalle));
                    clDescRenglon31.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon31);

                    string _ValorRenglon31 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_sancion"].ToString().Trim())));
                    PdfPCell clValorRenglon31 = new PdfPCell(new Phrase(_ValorRenglon31, _FontDetalle));
                    clValorRenglon31.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon31);

                    //--RENGLON 32
                    PdfPCell clRenglon32 = new PdfPCell(new Phrase("32", _standardFontRenglon));
                    clRenglon32.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon32);

                    string _DescRenglon32 = "     MENOS SALDO A FAVOR DEL PERIODO ANTERIOR SIN SOLICITUD DE DEVOLUCIÓN O COMPENSACIÓN";
                    PdfPCell clDescRenglon32 = new PdfPCell(new Phrase(_DescRenglon32, _FontDetalle));
                    clDescRenglon32.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon32);

                    string _ValorRenglon32 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon32"].ToString().Trim())));
                    PdfPCell clValorRenglon32 = new PdfPCell(new Phrase(_ValorRenglon32, _FontDetalle));
                    clValorRenglon32.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon32);

                    //--RENGLON 33
                    PdfPCell clRenglon33 = new PdfPCell(new Phrase("33", _standardFontRenglon));
                    clRenglon33.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon33);

                    string _DescRenglon33 = "TOTAL SALDO A CARGO (RENGLÓN 25-26-27-28-29+30+31-32)";
                    PdfPCell clDescRenglon33 = new PdfPCell(new Phrase(_DescRenglon33, _FontDetalle));
                    clDescRenglon33.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon33);

                    string _ValorRenglon33 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon33"].ToString().Trim())));
                    PdfPCell clValorRenglon33 = new PdfPCell(new Phrase(_ValorRenglon33, _FontDetalle));
                    clValorRenglon33.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon33);

                    //--RENGLON 34
                    PdfPCell clRenglon34 = new PdfPCell(new Phrase("34", _standardFontRenglon));
                    clRenglon34.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionD.AddCell(clRenglon34);

                    string _DescRenglon34 = "TOTAL SALDO A FAVOR (RENGLÓN 25-26-27-28-29+30+31-32) si el resultado es menor a cero";
                    PdfPCell clDescRenglon34 = new PdfPCell(new Phrase(_DescRenglon34, _FontDetalle));
                    clDescRenglon34.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionD.AddCell(clDescRenglon34);

                    string _ValorRenglon34 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon34"].ToString().Trim())));
                    PdfPCell clValorRenglon34 = new PdfPCell(new Phrase(_ValorRenglon34, _FontDetalle));
                    clValorRenglon34.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionD.AddCell(clValorRenglon34);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionD);
                    #endregion

                    #region SECCION E. PAGO
                    PdfPTable TblSeccionE = new PdfPTable(4);
                    TblSeccionE.TotalWidth = _AnchoTabla;
                    TblSeccionE.LockedWidth = true;
                    TblSeccionE.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionE.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.SpacingAfter = 0;
                    float[] TblWidthsE = new float[4];
                    TblWidthsE[0] = 20.5f;
                    TblWidthsE[1] = 9f;
                    TblWidthsE[2] = 200f;
                    TblWidthsE[3] = 80f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionE.SetWidths(TblWidthsE);

                    string _SeccionE = "E. PAGO";
                    PdfPCell clSeccionE = new PdfPCell(new Phrase(_SeccionE, _FontSecciones));
                    clSeccionE.Rowspan = 4;
                    clSeccionE.Colspan = 1;
                    //clSeccionE.BorderWidth = 1;
                    clSeccionE.Rotation = 90;
                    clSeccionE.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionE.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.AddCell(clSeccionE);

                    //--RENGLON 35
                    PdfPCell clRenglon35 = new PdfPCell(new Phrase("35", _standardFontRenglon));
                    clRenglon35.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.AddCell(clRenglon35);

                    string _DescRenglon35 = "VALOR A PAGAR";
                    PdfPCell clDescRenglon35 = new PdfPCell(new Phrase(_DescRenglon35, _FontDetalle));
                    clDescRenglon35.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionE.AddCell(clDescRenglon35);

                    string _ValorRenglon35 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon35"].ToString().Trim())));
                    PdfPCell clValorRenglon35 = new PdfPCell(new Phrase(_ValorRenglon35, _FontDetalle));
                    clValorRenglon35.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionE.AddCell(clValorRenglon35);

                    //--RENGLON 36
                    PdfPCell clRenglon36 = new PdfPCell(new Phrase("36", _standardFontRenglon));
                    clRenglon36.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.AddCell(clRenglon36);

                    string _DescRenglon36 = "DESCUENTO POR PRONTO PAGO (Si existe, liquídelo según el Acuerdo Municipal o Distrital)";
                    PdfPCell clDescRenglon36 = new PdfPCell(new Phrase(_DescRenglon36, _FontDetalle));
                    clDescRenglon36.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionE.AddCell(clDescRenglon36);

                    string _ValorRenglon36 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon36"].ToString().Trim())));
                    PdfPCell clValorRenglon36 = new PdfPCell(new Phrase(_ValorRenglon36, _FontDetalle));
                    clValorRenglon36.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionE.AddCell(clValorRenglon36);

                    //--RENGLON 37
                    PdfPCell clRenglon37 = new PdfPCell(new Phrase("37", _standardFontRenglon));
                    clRenglon37.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.AddCell(clRenglon37);

                    string _DescRenglon37 = "INTERESES DE MORA";
                    PdfPCell clDescRenglon37 = new PdfPCell(new Phrase(_DescRenglon37, _FontDetalle));
                    clDescRenglon37.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionE.AddCell(clDescRenglon37);

                    string _ValorRenglon37 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["interes_mora"].ToString().Trim())));
                    PdfPCell clValorRenglon37 = new PdfPCell(new Phrase(_ValorRenglon37, _FontDetalle));
                    clValorRenglon37.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionE.AddCell(clValorRenglon37);

                    //--RENGLON 38
                    PdfPCell clRenglon38 = new PdfPCell(new Phrase("38", _standardFontRenglon));
                    clRenglon38.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionE.AddCell(clRenglon38);

                    string _DescRenglon38 = "TOTAL A PAGAR (RENGLÓN 35-36+37)";
                    PdfPCell clDescRenglon38 = new PdfPCell(new Phrase(_DescRenglon38, _FontDetalle));
                    clDescRenglon38.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionE.AddCell(clDescRenglon38);

                    string _ValorRenglon38 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon38"].ToString().Trim())));
                    PdfPCell clValorRenglon38 = new PdfPCell(new Phrase(_ValorRenglon38, _FontDetalle));
                    clValorRenglon38.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionE.AddCell(clValorRenglon38);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionE);
                    #endregion

                    #region SECCION PAGO VOLUNTARIO
                    PdfPTable TblSeccionP = new PdfPTable(4);
                    TblSeccionP.TotalWidth = _AnchoTabla;
                    TblSeccionP.LockedWidth = true;
                    TblSeccionP.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionP.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionP.SpacingAfter = 0;
                    float[] TblWidthsP = new float[4];
                    TblWidthsP[0] = 60f;
                    TblWidthsP[1] = 9f;
                    TblWidthsP[2] = 160f;
                    TblWidthsP[3] = 80f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionP.SetWidths(TblWidthsP);

                    string _SeccionP = "SECCIÓN PAGO VOLUNTARIO\n(Solamente donde exista esta opción)";
                    PdfPCell clSeccionP = new PdfPCell(new Phrase(_SeccionP, _FontSecciones));
                    clSeccionP.Rowspan = 3;
                    clSeccionP.Colspan = 1;
                    clSeccionP.Padding = 8f;
                    clSeccionP.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionP.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionP.AddCell(clSeccionP);

                    //--RENGLON 39
                    PdfPCell clRenglon39 = new PdfPCell(new Phrase("39", _standardFontRenglon));
                    clRenglon39.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionP.AddCell(clRenglon39);

                    string _DescRenglon39 = "LIQUIDE EL VALOR DEL PAGO VOLUNTARIO (Según instrucciones del municipio/distrito)";
                    PdfPCell clDescRenglon39 = new PdfPCell(new Phrase(_DescRenglon39, _FontDetalle));
                    clDescRenglon39.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionP.AddCell(clDescRenglon39);

                    string _ValorRenglon39 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_pago_voluntario"].ToString().Trim())));
                    PdfPCell clValorRenglon39 = new PdfPCell(new Phrase(_ValorRenglon39, _FontDetalle));
                    clValorRenglon39.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionP.AddCell(clValorRenglon39);

                    //--RENGLON 40
                    PdfPCell clRenglon40 = new PdfPCell(new Phrase("40", _standardFontRenglon));
                    clRenglon40.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionP.AddCell(clRenglon40);

                    string _DescRenglon40 = "TOTAL A PAGAR CON PAGO VOLUNTARIO (Renglón 38 + 39)";
                    PdfPCell clDescRenglon40 = new PdfPCell(new Phrase(_DescRenglon40, _FontDetalle));
                    clDescRenglon40.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionP.AddCell(clDescRenglon40);

                    string _ValorRenglon40 = String.Format(String.Format("{0:###,###,##0}", Double.Parse(rowItem["valor_renglon40"].ToString().Trim())));
                    PdfPCell clValorRenglon40 = new PdfPCell(new Phrase(_ValorRenglon40, _FontDetalle));
                    clValorRenglon40.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionP.AddCell(clValorRenglon40);

                    //--RENGLON 41
                    PdfPCell clRenglon41 = new PdfPCell(new Phrase("41", _standardFontRenglon));
                    clRenglon41.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionP.AddCell(clRenglon41);

                    string _DescRenglon41 = "Destino de mi aporte voluntario: " + rowItem["destino_pago_voluntario"].ToString().Trim().ToUpper();
                    PdfPCell clDescRenglon41 = new PdfPCell(new Phrase(_DescRenglon41, _FontDetalle));
                    clDescRenglon41.Colspan = 2;
                    clDescRenglon41.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionP.AddCell(clDescRenglon41);

                    //string _ValorRenglon41 = "0";
                    //PdfPCell clValorRenglon41 = new PdfPCell(new Phrase(_ValorRenglon41, _FontDetalle));
                    //clValorRenglon41.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //TblSeccionP.AddCell(clValorRenglon41);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionP);
                    #endregion

                    #region SECCION F. FIRMAS
                    PdfPTable TblSeccionF = new PdfPTable(3);
                    TblSeccionF.TotalWidth = _AnchoTabla;
                    TblSeccionF.LockedWidth = true;
                    TblSeccionF.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionF.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionF.SpacingAfter = 0;
                    float[] TblWidthsF = new float[3];
                    TblWidthsF[0] = 23f;
                    TblWidthsF[1] = 160f;
                    TblWidthsF[2] = 160f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionF.SetWidths(TblWidthsF);

                    string _SeccionF = "F. FIRMAS";
                    PdfPCell clSeccionF = new PdfPCell(new Phrase(_SeccionF, _FontSecciones));
                    clSeccionF.Rowspan = 3;
                    clSeccionF.Colspan = 1;
                    clSeccionF.Rotation = 90;
                    clSeccionF.VerticalAlignment = Element.ALIGN_CENTER;
                    clSeccionF.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionF.AddCell(clSeccionF);

                    //--RENGLON DATOS FIRMA DECLARANTE 1
                    string _DescFirma1 = "FIRMA DEL DECLARANTE\n\n\n\n";
                    PdfPCell clRenglonFirma1 = new PdfPCell(new Phrase(_DescFirma1, _FontFirmas));
                    clRenglonFirma1.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonFirma1);

                    string _DescFirma2 = "FIRMA DEL CONTADOR:       REVISOR FISCAL:\n\n\n\n";
                    PdfPCell clRenglonFirma2 = new PdfPCell(new Phrase(_DescFirma2, _FontFirmas));
                    clRenglonFirma2.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonFirma2);

                    //--RENGLON DATOS FIRMA DECLARANTE 2
                    string _DescLineaFirma1 = "NOMBRE: " + "";  //--this.LblNombreFirmante1.Text.ToString().Trim();
                    PdfPCell clRenglonLineaFirma1 = new PdfPCell(new Phrase(_DescLineaFirma1, _FontFirmas));
                    clRenglonLineaFirma1.BorderWidthBottom = 1f;
                    clRenglonLineaFirma1.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonLineaFirma1);

                    string _DescLineaFirma2 = "NOMBRE: " + "";  //--this.LblNombreFirmante2.Text.ToString().Trim();
                    PdfPCell clRenglonLineaFirma2 = new PdfPCell(new Phrase(_DescLineaFirma2, _FontFirmas));
                    clRenglonLineaFirma2.BorderWidthBottom = 1f;
                    clRenglonLineaFirma2.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonLineaFirma2);

                    //--RENGLON DATOS FIRMA DECLARANTE 3
                    string _DescDocFirma1 = "";
                    //if (this.RbCedulaCiudFirm1.Checked)
                    //{
                    _DescDocFirma1 = "C.C.:     C.E.:       T.I.:      ";
                    //_DescDocFirma1 = "C.C.:  X   C.E.:       T.I.:      " + this.LblNumeroFirm1.Text.ToString().Trim();
                    //}
                    //if (this.RbCedulaExtrFirm1.Checked)
                    //{
                    //    _DescDocFirma1 = "C.C.:      C.E.:  X    T.I.:      " + this.LblNumeroFirm1.Text.ToString().Trim();
                    //}
                    //if (this.RbTarjetaProfFirm1.Checked)
                    //{
                    //    _DescDocFirma1 = "C.C.:      C.E.:       T.I.:  X   " + this.LblNumeroFirm1.Text.ToString().Trim();
                    //}

                    PdfPCell clRenglonDocFirma1 = new PdfPCell(new Phrase(_DescDocFirma1, _FontFirmas));
                    clRenglonDocFirma1.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonDocFirma1);

                    string _DescDocFirma2 = "";
                    //if (this.RbCedulaCiudFirm2.Checked)
                    //{
                    _DescDocFirma2 = "C.C.:     C.E.:       T.I.:      ";
                    //_DescDocFirma2 = "C.C.:  X   C.E.:       T.I.:      " + this.LblNumeroFirm2.Text.ToString().Trim();
                    //}
                    //if (this.RbCedulaExtrFirm2.Checked)
                    //{
                    //    _DescDocFirma2 = "C.C.:      C.E.:  X    T.I.:      " + this.LblNumeroFirm2.Text.ToString().Trim();
                    //}
                    //if (this.RbTarjetaProfFirm2.Checked)
                    //{
                    //    _DescDocFirma2 = "C.C.:      C.E.:       T.I.:  X   " + this.LblNumeroFirm2.Text.ToString().Trim();
                    //}

                    PdfPCell clRenglonDocFirma2 = new PdfPCell(new Phrase(_DescDocFirma2, _FontFirmas));
                    clRenglonDocFirma2.HorizontalAlignment = Element.ALIGN_LEFT;
                    TblSeccionF.AddCell(clRenglonDocFirma2);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionF);
                    #endregion

                    #region SECCION CODIGO DE BARRAS 1
                    PdfPTable TblSeccionCB = new PdfPTable(2);
                    TblSeccionCB.TotalWidth = _AnchoTabla;
                    TblSeccionCB.LockedWidth = true;
                    TblSeccionCB.WidthPercentage = 100;
                    /// Left aLign
                    TblSeccionCB.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB.SpacingAfter = 0;
                    float[] TblWidthsCB = new float[2];
                    TblWidthsCB[0] = 200f;
                    TblWidthsCB[1] = 200f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionCB.SetWidths(TblWidthsCB);

                    //--ESPACIO PARA CÓDIGO DE BARRAS
                    PdfPTable TblSeccionCB1 = new PdfPTable(1);
                    TblSeccionCB1.WidthPercentage = 100;
                    string _DescCodBarras1 = "\n\nESPACIO PARA CÓDIGO DE BARRAS\n\n\n\n";
                    PdfPCell clCodBarras1 = new PdfPCell(new Phrase(_DescCodBarras1, _FontCodigosQr));
                    clCodBarras1.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB1.AddCell(clCodBarras1);
                    TblSeccionCB.AddCell(TblSeccionCB1);

                    string _DescCodBarras2 = "\n\nESPACIO PARA NÚMERO DE REFERENCIA RECAUDO\n\nFORMULARIO No.";
                    PdfPCell clCodBarras2 = new PdfPCell(new Phrase(_DescCodBarras2, _FontCodigosQr));
                    clCodBarras2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    TblSeccionCB.AddCell(clCodBarras2);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionCB);
                    #endregion

                    #region SECCION CODIGO DE BARRAS 2
                    PdfPTable TblSeccionCB2 = new PdfPTable(3);
                    TblSeccionCB2.TotalWidth = _AnchoTabla;
                    TblSeccionCB2.LockedWidth = true;
                    TblSeccionCB2.WidthPercentage = 150;
                    /// Left aLign
                    TblSeccionCB2.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB2.SpacingAfter = 0;
                    float[] TblWidthsCB2 = new float[3];
                    TblWidthsCB2[0] = 80f;
                    TblWidthsCB2[1] = 120f;
                    TblWidthsCB2[2] = 350f;
                    // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                    TblSeccionCB2.SetWidths(TblWidthsCB2);

                    PdfPTable TblSeccionCB21 = new PdfPTable(1);
                    TblSeccionCB21.WidthPercentage = 100;
                    //--ESPACIO PARA CÓDIGO DE BARRAS
                    string _DescCodigoQr = "\n\n\nESPACIO PARA\nCÓDIGO QR\n\n\n";
                    PdfPCell clCodigoQr = new PdfPCell(new Phrase(_DescCodigoQr, _FontCodigosQr));
                    clCodigoQr.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB21.AddCell(clCodigoQr);
                    TblSeccionCB2.AddCell(TblSeccionCB21);

                    string _DescSelloTimbre = "\n\n\nESPACIO PARA\nSELLO O TIMBRE\n\n\n";
                    PdfPCell clSelloTimbre = new PdfPCell(new Phrase(_DescSelloTimbre, _FontCodigosQr));
                    clSelloTimbre.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB2.AddCell(clSelloTimbre);

                    string _DescSerialAutom = "\n\n\nESPACIO PARA SERIAL AUTOMÁTICO DE TRANSACCIÓN O MECANISMO DE\nIDENTIFICACIÓN DE RECAUDO\n\n\n";
                    PdfPCell clSerialAutom = new PdfPCell(new Phrase(_DescSerialAutom, _FontCodigosQr));
                    clSerialAutom.HorizontalAlignment = Element.ALIGN_CENTER;
                    TblSeccionCB2.AddCell(clSerialAutom);

                    //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                    doc.Add(TblSeccionCB2);
                    #endregion

                    #region MOSTRAR LAS ACTIVIDADES ECONOMISCAS AL RESPALDO DE LA PAGINA
                    GetObtenerDatosDb objObtenerDatos = new GetObtenerDatosDb();
                    objObtenerDatos.TipoConsulta = 1;
                    objObtenerDatos.AnioGravable = rowItem["anio_gravable"].ToString().Trim();
                    objObtenerDatos.IdClienteEstablecimiento = rowItem["idcliente_establecimiento"].ToString().Trim();
                    objObtenerDatos.MotorBaseDatos = FixedData.MotorBaseDatos.ToString().Trim();

                    DataTable dtActEconomica = new DataTable();
                    dtActEconomica = objObtenerDatos.GetConsultarActEconomica();
                    if (dtActEconomica != null)
                    {
                        //--SOLO SE IOMPRIMEN AL RESPALDO SIEMPRE Y CUANDO LA LISTA ES MAYOR A 3 REGISTROS
                        if (dtActEconomica.Rows.Count > 3)
                        {
                            //--AQUI ESCRIBIMOS EL RESTO DE LAS ACTIVIDADES EN OTRA OTRA PARA QUE SEA IMPRESA AL RESPALDO DEL IMPUESTO
                            #region SECCION C. DISCRIMINACIÓN DE ACTIVIDADES GRAVADAS
                            #region AQUI DEFINIMOS EL NOMBRE Y ROTACION DEL LABEL
                            PdfPTable TblSeccionCAux = new PdfPTable(6);
                            TblSeccionCAux.TotalWidth = _AnchoTabla;
                            TblSeccionCAux.LockedWidth = true;
                            TblSeccionCAux.WidthPercentage = 100;
                            /// Left aLign
                            TblSeccionCAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.SpacingAfter = 10;
                            float[] TblWidthsCAux = new float[6];
                            TblWidthsCAux[0] = 25f;
                            TblWidthsCAux[1] = 100f;
                            TblWidthsCAux[2] = 70f;
                            TblWidthsCAux[3] = 70f;
                            TblWidthsCAux[4] = 70f;
                            TblWidthsCAux[5] = 70f;
                            // Set the column widths on table creation. Unlike HTML cells cannot be sized.
                            TblSeccionCAux.SetWidths(TblWidthsCAux);

                            string _SeccionCAux = "C. DISCRIMINACIÓN DE\nACTIVIDADES GRAVADAS";
                            PdfPCell clSeccionCAux = new PdfPCell(new Phrase(_SeccionCAux, _FontSecciones));
                            clSeccionCAux.Rowspan = 14;
                            clSeccionCAux.Colspan = 1;
                            //clSeccionB.BorderWidth = 1;
                            clSeccionCAux.Rotation = 90;
                            clSeccionCAux.VerticalAlignment = Element.ALIGN_CENTER;
                            clSeccionCAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clSeccionCAux);
                            #endregion

                            #region DEFINIR NOMBRE DE COLUMNAS DE LA TABLA
                            //--DEFINIR NOMBRE DE COLUMNAS DE LA TABLA
                            PdfPCell clActGravadasAux = new PdfPCell(new Phrase("ACTIVIDADES GRAVADAS", _FontDetalle));
                            clActGravadasAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clActGravadasAux);

                            //string _CodigoActAux = "CÓDIGO";
                            PdfPCell clCodigoActAux = new PdfPCell(new Phrase("CÓDIGO", _FontDetalle));
                            clCodigoActAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clCodigoActAux);

                            //string _IngGravados = "INGRESOS GRAVADOS";
                            PdfPCell clIngGravadosAux = new PdfPCell(new Phrase("INGRESOS GRAVADOS", _FontDetalle));
                            clIngGravadosAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clIngGravadosAux);

                            //string _TarifaxMil = "TARIFA (POR MIL)";
                            PdfPCell clTarifaxMilAux = new PdfPCell(new Phrase("TARIFA (POR MIL)", _FontDetalle));
                            //clTarifaxMilAux.Colspan = 2;
                            clTarifaxMilAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clTarifaxMilAux);

                            PdfPCell clValorActAux = new PdfPCell(new Phrase("", _FontDetalle));
                            //clValorAct.Colspan = 2;
                            clValorActAux.HorizontalAlignment = Element.ALIGN_CENTER;
                            TblSeccionCAux.AddCell(clValorActAux);
                            #endregion
                            #endregion

                            int _ContadorRowsAct = 4;
                            double _ValorActividad = 0;
                            foreach (DataRow rowItemAct in dtActEconomica.Rows)
                            {
                                #region AQUI PINTAMOS LAS ACTIVIDADES ECONOMICAS RESTANTES SI LAS HAY EN EL DATATABLE
                                //--SI EL CONTADOR DEL DATAROW ES IGUAL A 4 SE PROCEDE A PINTAR LAS DE
                                if (_ContadorRowsAct == 4)
                                {
                                    //--AQUI OBTENEMOS LOS DATOS DE LA ACTIVIDAD ECONOMICA
                                    string _CodigoActividad = rowItemAct["codigo_actividad"].ToString().Trim();
                                    int _IdCalcularTarifaPor = Int32.Parse(rowItemAct["idtipo_calculo"].ToString().Trim());
                                    int _IdTipoTarifa = Int32.Parse(rowItemAct["idtipo_tarifa"].ToString().Trim());
                                    double _TarifaLey = Double.Parse(rowItemAct["tarifa_ley"].ToString().Trim());
                                    double _TarifaMunicipio = Double.Parse(rowItemAct["tarifa_municipio"].ToString().Trim());
                                    string _SaldoFinal1 = rowItemAct["saldo_final"].ToString().Trim().Replace("-", "");
                                    double _SaldoFinal = Double.Parse(_SaldoFinal1);

                                    #region DEFINIR VALORES DE COLUMNAS DE LA TABLA
                                    //--DEFINIR VALORES CAMPO ACTIVIDAD 1
                                    PdfPCell clActPrincipalAux = new PdfPCell(new Phrase("ACTIVIDAD " + _ContadorRowsAct, _FontDetalle));
                                    clActPrincipalAux.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TblSeccionCAux.AddCell(clActPrincipalAux);

                                    //string _CodigoAct1Aux = this.LblCodActividad1.Text.ToString().Trim();
                                    PdfPCell clCodigoAct1Aux = new PdfPCell(new Phrase(_CodigoActividad, _FontDetalle));
                                    clCodigoAct1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TblSeccionCAux.AddCell(clCodigoAct1Aux);

                                    string _IngGravados1Aux = String.Format(String.Format("{0:###,###,##0}", round(_SaldoFinal)));
                                    PdfPCell clIngGravados1Aux = new PdfPCell(new Phrase(_IngGravados1Aux, _FontDetalle));
                                    clIngGravados1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TblSeccionCAux.AddCell(clIngGravados1Aux);

                                    string _TarifaAct1Aux = _TarifaMunicipio.ToString().Trim();
                                    PdfPCell clTarifaAct1Aux = new PdfPCell(new Phrase(_TarifaAct1Aux, _FontDetalle));
                                    clTarifaAct1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TblSeccionCAux.AddCell(clTarifaAct1Aux);

                                    //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                    if (_IdTipoTarifa == 1)      //--1. TARIFA DE LEY
                                    {
                                        _ValorActividad = ((_SaldoFinal * _TarifaLey) / 100);
                                    }
                                    else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                    {
                                        _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                    }

                                    string _ValorAct1Aux = String.Format(String.Format("{0:###,###,##0}", round(_ValorActividad)));
                                    PdfPCell clValorAct1Aux = new PdfPCell(new Phrase(_ValorAct1Aux, _FontDetalle));
                                    clValorAct1Aux.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TblSeccionCAux.AddCell(clValorAct1Aux);
                                    #endregion
                                }
                                _ContadorRowsAct++;
                                #endregion
                            }

                            //--AQUI PINTAMOS LAS ACTIVIDADES QUE HAGAN FALTA POR PINTAR
                            for (int i = _ContadorRowsAct; i <= 15; i++)
                            {
                                #region DEFINIR VALORES DE COLUMNAS DE LA TABLA
                                //--DEFINIR VALORES CAMPO ACTIVIDAD 1
                                PdfPCell clActPrincipal1Aux = new PdfPCell(new Phrase("ACTIVIDAD " + i, _FontDetalle));
                                clActPrincipal1Aux.HorizontalAlignment = Element.ALIGN_LEFT;
                                TblSeccionCAux.AddCell(clActPrincipal1Aux);

                                PdfPCell clCodigoAct1Aux = new PdfPCell(new Phrase("", _FontDetalle));
                                clCodigoAct1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                TblSeccionCAux.AddCell(clCodigoAct1Aux);

                                string _IngGravados1Aux = "";
                                PdfPCell clIngGravados1Aux = new PdfPCell(new Phrase(_IngGravados1Aux, _FontDetalle));
                                clIngGravados1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                TblSeccionCAux.AddCell(clIngGravados1Aux);

                                string _TarifaAct1Aux = "";
                                PdfPCell clTarifaAct1Aux = new PdfPCell(new Phrase(_TarifaAct1Aux, _FontDetalle));
                                clTarifaAct1Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                TblSeccionCAux.AddCell(clTarifaAct1Aux);

                                string _ValorAct1Aux = "";
                                PdfPCell clValorAct1Aux = new PdfPCell(new Phrase(_ValorAct1Aux, _FontDetalle));
                                clValorAct1Aux.HorizontalAlignment = Element.ALIGN_RIGHT;
                                TblSeccionCAux.AddCell(clValorAct1Aux);
                                #endregion

                                if (i == 15)
                                {
                                    #region TOTAL DE INGRESOS GRAVADOS ACT ECONOMICAS
                                    //--DEFINIR VALORES TOTAL INGRESOS GRAVADOS
                                    PdfPCell clActividad5Aux = new PdfPCell(new Phrase("TOTAL INGRESOS GRAVADOS", _FontDetalle));
                                    clActividad5Aux.Colspan = 2;
                                    clActividad5Aux.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TblSeccionCAux.AddCell(clActividad5Aux);

                                    //string _IngGravados5Aux = this.LblTotalIngresosGravados.Text.ToString().Trim();
                                    PdfPCell clIngGravados5Aux = new PdfPCell(new Phrase("", _FontDetalle));
                                    clIngGravados5Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TblSeccionCAux.AddCell(clIngGravados5Aux);

                                    //PdfPCell clTarifaAct5Aux = new PdfPCell(new Phrase("17. TOTAL IMPUESTO", _FontDetalle));
                                    PdfPCell clTarifaAct5Aux = new PdfPCell(new Phrase("17. TOTAL IMPUESTO", _FontDetalle));
                                    clTarifaAct5Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TblSeccionCAux.AddCell(clTarifaAct5Aux);

                                    //string _ValorAct5Aux = this.LblTotalImpuesto.Text.ToString().Trim();
                                    PdfPCell clValorAct5Aux = new PdfPCell(new Phrase("", _FontDetalle));
                                    clValorAct5Aux.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TblSeccionCAux.AddCell(clValorAct5Aux);

                                    //--DEFINIR VALORES GENERACIÓN DE ENERGIA
                                    //PdfPCell clActividad6Aux = new PdfPCell(new Phrase("18. GENERACIÓN DE ENERGIA CAPACIDAD INSTALADA", _FontDetalle));
                                    PdfPCell clActividad6Aux = new PdfPCell(new Phrase("", _FontDetalle));
                                    clActividad6Aux.Colspan = 3;
                                    clActividad6Aux.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TblSeccionCAux.AddCell(clActividad6Aux);

                                    //PdfPCell clTarifaAct6Aux = new PdfPCell(new Phrase("19. IMPUESTO LEY 56 DE 1981", _FontDetalle));
                                    //clTarifaAct6Aux.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //TblSeccionCAux.AddCell(clTarifaAct6Aux);

                                    //string _ValorAct6Aux = this.LblTotalImpuestosLey.Text.ToString().Trim();
                                    PdfPCell clValorAct6Aux = new PdfPCell(new Phrase("", _FontDetalle));
                                    clValorAct6Aux.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TblSeccionCAux.AddCell(clValorAct6Aux);
                                    #endregion
                                }
                            }

                            //--AQUI ADICIONAMOS LA TABLA AL DOCUMENTO
                            //doc.NewPage();
                            doc.Add(TblSeccionCAux);
                        }
                    }
                    #endregion

                    doc.Close();
                    writer.Close();
                }
                _Result = true;
            }
            catch (Exception ex)
            {
                _Result = false;
                FixedData.LogApi.Error("Error al generar el PDF del ICA. motivo: " + ex.Message);
            }

            return _Result;
        }

    }
}
