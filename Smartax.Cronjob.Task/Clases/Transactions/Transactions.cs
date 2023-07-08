using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Smartax.Cronjob.Process.Clases.Utilidades;
using Smartax.Cronjob.Process.Clases.Models;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Win32.TaskScheduler;

namespace Smartax.Cronjob.Process.Clases.Transactions
{
    public class Transactions
    {
        public static Functions objFunctions = new Functions();
        public static EnviarEmails ObjEmails = new EnviarEmails();
        const string quote = "\"";

        #region DEFINICION DE METODOS PARA PROCESAR BASE GRAVABLE
        public bool ProcessBaseGravable(BaseGravable_Req objBase)
        {
            bool _Result = false;
            try
            {
                //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
                ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
                ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();

                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.AnioGravable = objBase.anio_gravable;
                objProcessDb.MesEf = objBase.mes_ef;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    if (dtEstablecimientos.Rows.Count > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--
                        Console.WriteLine("GENERAR BASE GRAVABLE => INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE ESTABLECIMIENTOS A PROCESAR: " + dtEstablecimientos.Rows.Count);
                        FixedData.LogApi.Info("PROCESO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE ESTABLECIMIENTOS A PROCESAR: " + dtEstablecimientos.Rows.Count);

                        int _ContadorEstablecimiento = 0;
                        foreach (DataRow rowEst in dtEstablecimientos.Rows)
                        {
                            #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                            //--
                            _ContadorEstablecimiento++;
                            int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                            string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                            //--
                            objProcessDb.TipoConsulta = 2;  //--objBase.tipo_consulta;
                            objProcessDb.IdCliente = objBase.id_cliente;
                            objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                            objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                            objProcessDb.AnioGravable = objBase.anio_gravable;
                            objProcessDb.CodigoDane = _CodigoDane;
                            objProcessDb.IdFormConfiguracion = null;
                            objProcessDb.IdPuc = null;
                            objProcessDb.MesEf = objBase.mes_ef;
                            objProcessDb.IdEstado = 1;
                            //--
                            DataTable dtBaseGravableDb = new DataTable();
                            dtBaseGravableDb = objProcessDb.GetBaseGravable();
                            int _ContadorRow = 0;
                            //--
                            if (dtBaseGravableDb != null)
                            {
                                if (dtBaseGravableDb.Rows.Count > 0)
                                {
                                    #region AQUI REALIZAMOS EL PROCESO DE GUARDAR EN LA BASE DE DATOS
                                    //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                    double _SumValorRenglon8 = 0, _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                    double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                    double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                    double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                    double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                    double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                    double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                    //--
                                    foreach (DataRow rowItem in dtBaseGravableDb.Rows)
                                    {
                                        #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                        _ContadorRow++;
                                        int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                        string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                                        string _SaldoInicial = rowItem["saldo_inicial"].ToString().Trim();
                                        string _MovDebito = rowItem["mov_debito"].ToString().Trim();
                                        string _MovCredito = rowItem["mov_credito"].ToString().Trim();
                                        string _SaldoFinal = rowItem["saldo_final"].ToString().Trim();
                                        string _ValorExtracontable1 = rowItem["valor_extracontable"].ToString().Trim().Replace("-", "");
                                        double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                        //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                        objProcessDb.TipoConsulta = 1;
                                        objProcessDb.NumeroRenglon = _NumRenglon;
                                        objProcessDb.IdCliente = objBase.id_cliente != null ? objBase.id_cliente.ToString().Trim() : null;
                                        objProcessDb.AnioGravable = objBase.anio_gravable;
                                        objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                        //objProcessDb.IdClienteEstablecimiento = objBase.idcliente_establecimiento;
                                        objProcessDb.SaldoInicial = _SaldoInicial;
                                        objProcessDb.MovDebito = _MovDebito;
                                        objProcessDb.MovCredito = _MovCredito;
                                        objProcessDb.SaldoFinal = _SaldoFinal;
                                        objProcessDb.CodigoCuenta = _CodigoCuenta;
                                        objProcessDb.CodigoDane = _CodigoDane;
                                        objProcessDb.MesEf = objBase.mes_ef.ToString().Trim();

                                        //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                        double _ValorTotal = 0;
                                        if (_SaldoInicial == "S" || _MovDebito == "S" || _MovCredito == "S" || _SaldoFinal == "S")
                                        {
                                            List<string> _ArrayDatos = objProcessDb.GetEstadoFinanciero();
                                            if (_ArrayDatos.Count > 0)
                                            {
                                                string _ValorCuentaSeparadorMiles = _ArrayDatos[1].ToString().Trim().Replace(FixedData.SeparadorMilesAp, "");
                                                string _ValorCuenta = _ValorCuentaSeparadorMiles.Replace(FixedData.SeparadorDecimalesAp, ".");
                                                _ValorTotal = (Double.Parse(_ValorCuenta.Replace(".", FixedData.SeparadorDecimalesAp)) + _ValorExtracontable);
                                                //--
                                                FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                            }
                                        }
                                        else
                                        {
                                            _ValorTotal = _ValorExtracontable;
                                        }
                                        #endregion

                                        #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                        //--AQUI OBTENEMOS LOS DATOS 
                                        switch (_NumRenglon)
                                        {
                                            case 8:
                                                _SumValorRenglon8 = _SumValorRenglon8 + _ValorTotal;
                                                break;
                                            case 9:
                                                _SumValorRenglon9 = _SumValorRenglon9 + _ValorTotal;
                                                break;
                                            case 11:
                                                _SumValorRenglon11 = _SumValorRenglon11 + _ValorTotal;
                                                break;
                                            case 12:
                                                _SumValorRenglon12 = _SumValorRenglon12 + _ValorTotal;
                                                break;
                                            case 13:
                                                _SumValorRenglon13 = _SumValorRenglon13 + _ValorTotal;
                                                break;
                                            case 14:
                                                _SumValorRenglon14 = _SumValorRenglon14 + _ValorTotal;
                                                break;
                                            case 15:
                                                _SumValorRenglon15 = _SumValorRenglon15 + _ValorTotal;
                                                break;
                                            case 26:
                                                _SumValorRenglon26 = _SumValorRenglon26 + _ValorTotal;
                                                break;
                                            case 27:
                                                _SumValorRenglon27 = _SumValorRenglon27 + _ValorTotal;
                                                break;
                                            case 28:
                                                _SumValorRenglon28 = _SumValorRenglon28 + _ValorTotal;
                                                break;
                                            case 29:
                                                _SumValorRenglon29 = _SumValorRenglon29 + _ValorTotal;
                                                break;
                                            case 30:
                                                _SumValorRenglon30 = _SumValorRenglon30 + _ValorTotal;
                                                break;
                                            case 31:
                                                _SumValorRenglon31 = _SumValorRenglon31 + _ValorTotal;
                                                break;
                                            case 32:
                                                _SumValorRenglon32 = _SumValorRenglon32 + _ValorTotal;
                                                break;
                                            case 33:
                                                _SumValorRenglon33 = _SumValorRenglon33 + _ValorTotal;
                                                break;
                                            case 34:
                                                _SumValorRenglon34 = _SumValorRenglon34 + _ValorTotal;
                                                break;
                                            case 35:
                                                _SumValorRenglon35 = _SumValorRenglon35 + _ValorTotal;
                                                break;
                                            case 36:
                                                _SumValorRenglon36 = _SumValorRenglon36 + _ValorTotal;
                                                break;
                                            case 37:
                                                _SumValorRenglon37 = _SumValorRenglon37 + _ValorTotal;
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                    }

                                    #region GUARDAR LOS DATOS DE LA BG EN LA DB
                                    //--
                                    objProcessDb.IdMunicipio = _IdMunicipio;
                                    objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                    objProcessDb.IdCliente = objBase.id_cliente;
                                    objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                    objProcessDb.CodigoDane = _CodigoDane;
                                    objProcessDb.AnioGravable = objBase.anio_gravable;
                                    objProcessDb.MesEf = objBase.mes_ef;
                                    //--VALORES DE LA BASE GRAVABLE
                                    objProcessDb.ValorRenglon8 = _SumValorRenglon8;
                                    double _SumValorRenglon9Aux = (_SumValorRenglon8 - _SumValorRenglon9);
                                    objProcessDb.ValorRenglon9 = _SumValorRenglon9Aux;
                                    _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9Aux);
                                    objProcessDb.ValorRenglon10 = _SumValorRenglon10;
                                    //Console.WriteLine("\nVALORES: RENGLON 8 => " + _SumValorRenglon8 + ", RENGLON 9 => " + _SumValorRenglon9Aux + ", RENGLON 10 => " + _SumValorRenglon10);
                                    //--
                                    objProcessDb.ValorRenglon11 = _SumValorRenglon11;
                                    objProcessDb.ValorRenglon12 = _SumValorRenglon12;
                                    objProcessDb.ValorRenglon13 = _SumValorRenglon13;
                                    objProcessDb.ValorRenglon14 = _SumValorRenglon14;
                                    objProcessDb.ValorRenglon15 = _SumValorRenglon15;
                                    objProcessDb.ValorRenglon16 = 0;
                                    objProcessDb.ValorRenglon26 = _SumValorRenglon26;
                                    objProcessDb.ValorRenglon27 = _SumValorRenglon27;
                                    objProcessDb.ValorRenglon28 = _SumValorRenglon28;
                                    objProcessDb.ValorRenglon29 = _SumValorRenglon29;
                                    objProcessDb.ValorRenglon30 = _SumValorRenglon30;
                                    objProcessDb.ValorRenglon31 = _SumValorRenglon31;
                                    objProcessDb.ValorRenglon32 = _SumValorRenglon32;
                                    objProcessDb.ValorRenglon33 = _SumValorRenglon33;
                                    objProcessDb.ValorRenglon34 = _SumValorRenglon34;
                                    objProcessDb.ValorRenglon35 = _SumValorRenglon35;
                                    objProcessDb.ValorRenglon36 = _SumValorRenglon36;
                                    objProcessDb.ValorRenglon37 = _SumValorRenglon37;
                                    objProcessDb.VersionEf = objBase.version_ef;
                                    objProcessDb.IdEstado = 1;
                                    objProcessDb.IdUsuario = objBase.id_usuario;
                                    //objProcessDb.ArrayData = _ArrayData;
                                    objProcessDb.IdUsuario = objBase.id_usuario;

                                    int _IdRegistro = 0;
                                    string _MsgError = "";
                                    if (objProcessDb.AddLoadBaseGravable_New(ref _IdRegistro, ref _MsgError))
                                    {
                                        FixedData.LogApi.Info(_MsgError);

                                        #region PROCESO PARA EL ENVIO DEL CORREO
                                        //--
                                        //ObjEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                        //ObjEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                        //ObjEmails.Asunto = "REF.: PROCESO BASE GRAVABLE";
                                        //ObjEmails.Detalle = "Señor usuario, para informarle que el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], ha sido procesado de forma exitosa.";

                                        //string _MsgErrorEmail = "";
                                        //if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                        //{
                                        //    FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                        //}

                                        ////--
                                        //FixedData.LogApi.Info("PROCESO FINALIZADO: CANTIDAD ESTABLECIMIENTOS: [" + _ContadorEstablecimiento + "] FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                        //_ContadorEstablecimiento = 0;
                                        //return _Result;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region PROCESO PARA EL ENVIO DEL CORREO
                                        //--
                                        ObjEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                        ObjEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                        ObjEmails.Asunto = "REF.: ERROR PROCESO BASE GRAVABLE";
                                        ObjEmails.Detalle = "Señor usuario, ocurrio un error con el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], por favor validar con soporte técnico para ver que sucede. Posible Motivo: " + _MsgError;

                                        string _MsgErrorEmail = "";
                                        if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                        {
                                            FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                        }

                                        //--
                                        _ContadorEstablecimiento = 0;
                                        FixedData.LogApi.Error("PROCESO FINALIZADO PERO OCURRIO UN ERROR. MOTIVO: " + _MsgError + ", FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                        #endregion
                                    }
                                    //--
                                    #endregion

                                    //--MOSTRAR EL MENSAJE DEL PROCESO GUARDADO EN EL DATATABLE
                                    Console.WriteLine("\nFECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", ID MUNICIPIO: " + _IdMunicipio + ", ID ESTABLECIMIENTO: " + _IdClienteEstablecimiento + ", No. DE PROCESO GENERADO => " + _ContadorEstablecimiento);
                                    //--
                                    #endregion
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                    ObjEmails.Asunto = "REF.: ERROR AL GENERAR LA BASE GRAVABLE";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se encontro información para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                ObjEmails.Asunto = "REF.: ERROR AL GENERAR LA BASE GRAVABLE";

                                string nHora = DateTime.Now.ToString("HH");
                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                StringBuilder strDetalleEmail = new StringBuilder();
                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario ocurrio un error al obtener los datos para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                        #endregion
                    }
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER ESTABLECIMIENTOS";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener la lista de establecimientos para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: EXCEPCION AL GENERAR LA BASE GRAVABLE";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el proceso de la BASE GRAVABLE. Motivo: " + ex.Message + "</h4>" +
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

            return _Result;
        }

        public bool ProcessBaseGravablePorMunicipio(BaseGravable_Req objBase)
        {
            bool _Result = false;
            try
            {
                //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
                ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
                ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();

                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdMunicipio = null;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.AnioGravable = objBase.anio_gravable;
                objProcessDb.MesEf = objBase.mes_ef;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtMunicipios = new DataTable();
                dtMunicipios = objProcessDb.GetMunicipios();
                if (dtMunicipios != null)
                {
                    int _TotalMunicipios = dtMunicipios.Rows.Count;
                    if (_TotalMunicipios > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--
                        Console.WriteLine("GENERAR BASE GRAVABLE POR MUNICIPIO => INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE MUNICIPIOS A PROCESAR: " + dtMunicipios.Rows.Count);
                        FixedData.LogApi.Info("PROCESO DE BASE GRAVABLE POR MUNICIPIO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE MUNICIPIOS A PROCESAR: " + dtMunicipios.Rows.Count);

                        int _ContadorMunicipios = 0, _ContadoMunicipiosProcesados = 0;
                        foreach (DataRow rowEst in dtMunicipios.Rows)
                        {
                            #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                            //--
                            _ContadorMunicipios++;
                            //int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                            string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                            //--
                            objProcessDb.TipoConsulta = 2;
                            objProcessDb.IdCliente = objBase.id_cliente;
                            objProcessDb.IdMunicipio = _IdMunicipio;
                            objProcessDb.IdClienteEstablecimiento = null;   //--_IdClienteEstablecimiento;
                            objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                            objProcessDb.AnioGravable = objBase.anio_gravable;
                            objProcessDb.CodigoDane = _CodigoDane;
                            objProcessDb.IdFormConfiguracion = null;
                            objProcessDb.IdPuc = null;
                            objProcessDb.MesEf = objBase.mes_ef;
                            objProcessDb.IdEstado = 1;
                            //--
                            DataTable dtValorRenglon8 = new DataTable();
                            dtValorRenglon8 = objProcessDb.GetValorRenglon();
                            //--
                            if (dtValorRenglon8 != null)
                            {
                                if (dtValorRenglon8.Rows.Count > 0)
                                {
                                    foreach (DataRow rowItem in dtValorRenglon8.Rows)
                                    {
                                        double _SaldoInicial = Double.Parse(rowItem["saldo_inicial"].ToString().Trim());
                                        double _MovDebito = Double.Parse(rowItem["mov_debito"].ToString().Trim());
                                        double _MovCredito = Double.Parse(rowItem["mov_credito"].ToString().Trim());
                                        double _SaldoFinal = Double.Parse(rowItem["saldo_final"].ToString().Trim());
                                        double _SumValorRenglon8 = Double.Parse(rowItem["saldo_final"].ToString().Trim());
                                        double _ValorExtracontable1 = Double.Parse(rowItem["valor_extracontable"].ToString().Trim());

                                        string[] _ArrayRenglones = null;
                                        if (objBase.idform_impuesto == 1)
                                        {
                                            _ArrayRenglones = FixedData.RENGLONES_CONF_ICA.ToString().Trim().Split(',');
                                        }
                                        else
                                        {
                                            _ArrayRenglones = FixedData.RENGLONES_CONF_AUTOICA.ToString().Trim().Split(',');
                                        }

                                        if (_ArrayRenglones.Length > 0)
                                        {
                                            #region AQUI RECORREMOS LA LISTA DE LOS RENGLONES POR IMPUESTO
                                            //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                            double _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                            double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                            double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                            double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                            double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                            double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                            double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                            //--
                                            foreach (string ItemRenglon in _ArrayRenglones)
                                            {
                                                #region REALIZAR PROCESO DE BASE GRAVABLE
                                                int _NumeroRenglon = Int32.Parse(ItemRenglon.ToString().Trim());
                                                //--
                                                objProcessDb.TipoConsulta = 3;
                                                objProcessDb.IdMunicipio = _IdMunicipio;
                                                objProcessDb.IdClienteEstablecimiento = null;
                                                objProcessDb.NumeroRenglon = _NumeroRenglon;
                                                objProcessDb.TipoProceso = objBase.tipo_proceso;
                                                //--
                                                DataTable dtValorRenglon = new DataTable();
                                                dtValorRenglon = objProcessDb.GetValorRenglon();
                                                //--
                                                if (dtValorRenglon != null)
                                                {
                                                    if (dtValorRenglon.Rows.Count > 0)
                                                    {
                                                        #region AQUI OBTENEMOS EL VALOR DEL RENGLON EN EL E.F.
                                                        double _ValorRenglon = Double.Parse(dtValorRenglon.Rows[0]["saldo_final"].ToString().Trim());
                                                        switch (_NumeroRenglon)
                                                        {
                                                            case 9:
                                                                _SumValorRenglon9 = _SumValorRenglon9 + _ValorRenglon;
                                                                _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9);
                                                                break;
                                                            case 11:
                                                                _SumValorRenglon11 = _SumValorRenglon11 + _ValorRenglon;
                                                                break;
                                                            case 12:
                                                                _SumValorRenglon12 = _SumValorRenglon12 + _ValorRenglon;
                                                                break;
                                                            case 13:
                                                                _SumValorRenglon13 = _SumValorRenglon13 + _ValorRenglon;
                                                                break;
                                                            case 14:
                                                                _SumValorRenglon14 = _SumValorRenglon14 + _ValorRenglon;
                                                                break;
                                                            case 15:
                                                                _SumValorRenglon15 = _SumValorRenglon15 + _ValorRenglon;
                                                                break;
                                                            default:
                                                                //_ValorRenglon = _ValorRenglon;
                                                                break;
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region AQUI OBTENEMOS EL VALOR DEL RENGLON EN EL E.F.
                                                        double _ValorRenglon = 0;   //--Double.Parse(dtValorRenglon.Rows[0]["saldo_final"].ToString().Trim());
                                                        switch (_NumeroRenglon)
                                                        {
                                                            case 9:
                                                                _SumValorRenglon9 = _SumValorRenglon9 + _ValorRenglon;
                                                                _SumValorRenglon10 = _SumValorRenglon9;
                                                                break;
                                                            case 11:
                                                                _SumValorRenglon11 = _SumValorRenglon11 + _ValorRenglon;
                                                                break;
                                                            case 12:
                                                                _SumValorRenglon12 = _SumValorRenglon12 + _ValorRenglon;
                                                                break;
                                                            case 13:
                                                                _SumValorRenglon13 = _SumValorRenglon13 + _ValorRenglon;
                                                                break;
                                                            case 14:
                                                                _SumValorRenglon14 = _SumValorRenglon14 + _ValorRenglon;
                                                                break;
                                                            case 15:
                                                                _SumValorRenglon15 = _SumValorRenglon15 + _ValorRenglon;
                                                                break;
                                                            default:
                                                                //_ValorRenglon = _ValorRenglon;
                                                                break;
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region GUARDAR LOS DATOS DE LA BG EN LA DB
                                            //--
                                            objProcessDb.IdMunicipio = _IdMunicipio;
                                            objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                            objProcessDb.IdCliente = objBase.id_cliente;
                                            objProcessDb.IdClienteEstablecimiento = -1; //--_IdClienteEstablecimiento;
                                            objProcessDb.CodigoDane = _CodigoDane;
                                            objProcessDb.AnioGravable = objBase.anio_gravable;
                                            objProcessDb.MesEf = objBase.mes_ef;
                                            //--VALORES DE LA BASE GRAVABLE
                                            objProcessDb.ValorRenglon8 = _SumValorRenglon8;
                                            //double _SumValorRenglon9Aux = (_SumValorRenglon8 - _SumValorRenglon9);
                                            objProcessDb.ValorRenglon9 = _SumValorRenglon9;
                                            //_SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9);
                                            objProcessDb.ValorRenglon10 = _SumValorRenglon10;
                                            //Console.WriteLine("\nVALORES: RENGLON 8 => " + _SumValorRenglon8 + ", RENGLON 9 => " + _SumValorRenglon9Aux + ", RENGLON 10 => " + _SumValorRenglon10);
                                            //--
                                            objProcessDb.ValorRenglon11 = _SumValorRenglon11;
                                            objProcessDb.ValorRenglon12 = _SumValorRenglon12;
                                            objProcessDb.ValorRenglon13 = _SumValorRenglon13;
                                            objProcessDb.ValorRenglon14 = _SumValorRenglon14;
                                            objProcessDb.ValorRenglon15 = _SumValorRenglon15;
                                            objProcessDb.ValorRenglon16 = 0;
                                            objProcessDb.ValorRenglon26 = _SumValorRenglon26;
                                            objProcessDb.ValorRenglon27 = _SumValorRenglon27;
                                            objProcessDb.ValorRenglon28 = _SumValorRenglon28;
                                            objProcessDb.ValorRenglon29 = _SumValorRenglon29;
                                            objProcessDb.ValorRenglon30 = _SumValorRenglon30;
                                            objProcessDb.ValorRenglon31 = _SumValorRenglon31;
                                            objProcessDb.ValorRenglon32 = _SumValorRenglon32;
                                            objProcessDb.ValorRenglon33 = _SumValorRenglon33;
                                            objProcessDb.ValorRenglon34 = _SumValorRenglon34;
                                            objProcessDb.ValorRenglon35 = _SumValorRenglon35;
                                            objProcessDb.ValorRenglon36 = _SumValorRenglon36;
                                            objProcessDb.ValorRenglon37 = _SumValorRenglon37;
                                            objProcessDb.VersionEf = objBase.version_ef;
                                            objProcessDb.IdEstado = 1;
                                            objProcessDb.IdUsuario = objBase.id_usuario;
                                            objProcessDb.TipoProceso = objBase.tipo_proceso;

                                            int _IdRegistro = 0;
                                            string _MsgError = "";
                                            if (objProcessDb.AddLoadBaseGravable_New(ref _IdRegistro, ref _MsgError))
                                            {
                                                FixedData.LogApi.Info(_MsgError);
                                            }
                                            else
                                            {
                                                #region PROCESO PARA EL ENVIO DEL CORREO
                                                //--
                                                ObjEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                                ObjEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                                ObjEmails.Asunto = "REF.: ERROR CON EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO";
                                                ObjEmails.Detalle = "Señor usuario, ocurrio un error al registrar la Base Gravable por Municipio [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + ", Id Municipio: " + _IdMunicipio + "], por favor validar con soporte técnico para ver que sucede. Posible Motivo: " + _MsgError;

                                                string _MsgErrorEmail = "";
                                                if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                                {
                                                    FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                                }

                                                //--
                                                //_ContadorEstablecimiento = 0;
                                                //FixedData.LogApi.Error("PROCESO FINALIZADO PERO OCURRIO UN ERROR. MOTIVO: " + _MsgError + ", FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                                #endregion
                                            }
                                            //--
                                            #endregion

                                            //--MOSTRAR EL MENSAJE DEL PROCESO GUARDADO EN EL DATATABLE
                                            _ContadoMunicipiosProcesados = _TotalMunicipios - _ContadorMunicipios;
                                            Console.WriteLine("\nFECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", ID MUNICIPIO: " + _IdMunicipio + ", MUNICIPIOS PROCESADOS: " + _ContadorMunicipios + ", DE: " + _ContadoMunicipiosProcesados);

                                            #region AQUI VALIDAMOS SI TERMINO EL PROCESO PARA ENVIAR EL CORREO DE NOTIFICACION
                                            //--
                                            if (_ContadorMunicipios == _TotalMunicipios)
                                            {
                                                #region AQUI ENVIAMO EL CORREO DE NOTIFICACION
                                                //--
                                                #region BORRAR LA TAREA PROGRAMADA Y CAMBIAR EL ESTADO DE LA TAREA
                                                //--MANDAMOS A BORRAR LA TAREA PROGRAMADA
                                                DeleteTaskSchedulerManual(objBase.nombre_tarea.ToString().Trim());

                                                //--INSTANCIAMOS EL OBJETO DE CLASE
                                                ProcessDb objProcess = new ProcessDb();
                                                objProcess.TipoProceso = objBase.tipo_proceso;
                                                objProcess.IdEstadoProceso = 10;
                                                objProcess.TipoConsulta = 1;
                                                _IdRegistro = 0;
                                                string _MsgProcess = "";
                                                bool _Result5 = objProcess.AddProcesoBaseGravable(ref _IdRegistro, ref _MsgProcess);
                                                FixedData.LogApi.Warn("PROCESO TERMINADO => " + _MsgProcess);
                                                #endregion

                                                //--
                                                ObjEmails.EmailPara = FixedData.EnvioEmail;
                                                ObjEmails.EmailCopia = FixedData.EmailCopiaProcesos;
                                                ObjEmails.Asunto = "REF.: PROCESO DE BASE GRAVABLE POR MUNICIPIO";

                                                string nHora = DateTime.Now.ToString("HH");
                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario el proceso de la base gravable por municipio ha terminado de forma exitosa con [" + _ContadorMunicipios + " Municipios]" + "</h4>" +
                                                            "<br/><br/>" +
                                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                                string _MsgErrorEmail = "";
                                                if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                                {
                                                    FixedData.LogApi.Error(_MsgErrorEmail);
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            //--
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                    ObjEmails.Asunto = "REF.: NO SE ENCONTRO VALOR PARA EL RENGLON 8";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se encontro información para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER EL VALOR DEL RENGLON 8";

                                string nHora = DateTime.Now.ToString("HH");
                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                StringBuilder strDetalleEmail = new StringBuilder();
                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario ocurrio un error al obtener los datos para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                        #endregion
                    }
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER MUNICIPIOS";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener la lista de municipios para realizar el proceso de la BASE GRAVABLE POR MUNICIPIOS." + "</h4>" +
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
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: EXCEPCION AL GENERAR LA BASE GRAVABLE";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el proceso de la BASE GRAVABLE. Motivo: " + ex.Message + "</h4>" +
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

            return _Result;
        }

        public bool ProcessBaseGravablePorOficina(BaseGravable_Req objBase)
        {
            bool _Result = false;
            try
            {
                //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
                ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
                ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();

                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 6;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdMunicipio = null;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.AnioGravable = objBase.anio_gravable;
                objProcessDb.MesEf = objBase.mes_ef;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    int _TotalEstablecimiento = dtEstablecimientos.Rows.Count;
                    if (_TotalEstablecimiento > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--
                        Console.WriteLine("GENERAR BASE GRAVABLE => INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE ESTABLECIMIENTOS A PROCESAR: " + dtEstablecimientos.Rows.Count);
                        FixedData.LogApi.Info("PROCESO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", CANTIDAD DE ESTABLECIMIENTOS A PROCESAR: " + dtEstablecimientos.Rows.Count);

                        int _ContadorEstablecimiento = 0, _ContadorOficProcesadas = 0;
                        foreach (DataRow rowEst in dtEstablecimientos.Rows)
                        {
                            #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                            //--
                            _ContadorEstablecimiento++;
                            int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                            string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                            //--
                            objProcessDb.TipoConsulta = 1;
                            objProcessDb.IdCliente = objBase.id_cliente;
                            objProcessDb.IdMunicipio = _IdMunicipio;
                            objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                            objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                            objProcessDb.AnioGravable = objBase.anio_gravable;
                            objProcessDb.CodigoDane = _CodigoDane;
                            objProcessDb.IdFormConfiguracion = null;
                            objProcessDb.IdPuc = null;
                            objProcessDb.MesEf = objBase.mes_ef;
                            objProcessDb.IdEstado = 1;
                            //--AQUI VALIDAMOS SI EL ESTABLECIMIENTO EXISTE EN EL E.F.
                            bool _ExisteOficina = objProcessDb.GetValidarEstablecimientoEF();

                            if (_ExisteOficina == true)
                            {
                                objProcessDb.TipoConsulta = 2;
                                DataTable dtValorRenglon8 = new DataTable();
                                dtValorRenglon8 = objProcessDb.GetValorRenglon();
                                //--
                                if (dtValorRenglon8 != null)
                                {
                                    if (dtValorRenglon8.Rows.Count > 0)
                                    {
                                        foreach (DataRow rowItem in dtValorRenglon8.Rows)
                                        {
                                            double _SaldoInicial = Double.Parse(rowItem["saldo_inicial"].ToString().Trim());
                                            double _MovDebito = Double.Parse(rowItem["mov_debito"].ToString().Trim());
                                            double _MovCredito = Double.Parse(rowItem["mov_credito"].ToString().Trim());
                                            double _SaldoFinal = Double.Parse(rowItem["saldo_final"].ToString().Trim());
                                            double _SumValorRenglon8 = Double.Parse(rowItem["saldo_final"].ToString().Trim());
                                            double _ValorExtracontable1 = Double.Parse(rowItem["valor_extracontable"].ToString().Trim());

                                            string[] _ArrayRenglones = null;
                                            if (objBase.idform_impuesto == 1)
                                            {
                                                _ArrayRenglones = FixedData.RENGLONES_CONF_ICA.ToString().Trim().Split(',');
                                            }
                                            else
                                            {
                                                _ArrayRenglones = FixedData.RENGLONES_CONF_AUTOICA.ToString().Trim().Split(',');
                                            }

                                            if (_ArrayRenglones.Length > 0)
                                            {
                                                #region AQUI RECORREMOS LA LISTA DE LOS RENGLONES POR IMPUESTO
                                                //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                                double _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                                double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                                double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                                double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                                double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                                double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                                double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                                //--
                                                foreach (string ItemRenglon in _ArrayRenglones)
                                                {
                                                    #region REALIZAR PROCESO DE BASE GRAVABLE
                                                    int _NumeroRenglon = Int32.Parse(ItemRenglon.ToString().Trim());
                                                    //--
                                                    objProcessDb.TipoConsulta = 3;
                                                    objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                    objProcessDb.NumeroRenglon = _NumeroRenglon;
                                                    objProcessDb.TipoProceso = objBase.tipo_proceso;
                                                    //--
                                                    DataTable dtValorRenglon = new DataTable();
                                                    dtValorRenglon = objProcessDb.GetValorRenglon();
                                                    //--
                                                    if (dtValorRenglon != null)
                                                    {
                                                        if (dtValorRenglon.Rows.Count > 0)
                                                        {
                                                            #region AQUI OBTENEMOS EL VALOR DEL RENGLON EN EL E.F.
                                                            double _ValorRenglon = Double.Parse(dtValorRenglon.Rows[0]["saldo_final"].ToString().Trim());
                                                            switch (_NumeroRenglon)
                                                            {
                                                                case 9:
                                                                    _SumValorRenglon9 = _SumValorRenglon9 + _ValorRenglon;
                                                                    _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9);
                                                                    break;
                                                                case 11:
                                                                    _SumValorRenglon11 = _SumValorRenglon11 + _ValorRenglon;
                                                                    break;
                                                                case 12:
                                                                    _SumValorRenglon12 = _SumValorRenglon12 + _ValorRenglon;
                                                                    break;
                                                                case 13:
                                                                    _SumValorRenglon13 = _SumValorRenglon13 + _ValorRenglon;
                                                                    break;
                                                                case 14:
                                                                    _SumValorRenglon14 = _SumValorRenglon14 + _ValorRenglon;
                                                                    break;
                                                                case 15:
                                                                    _SumValorRenglon15 = _SumValorRenglon15 + _ValorRenglon;
                                                                    break;
                                                                default:
                                                                    //_ValorRenglon = _ValorRenglon;
                                                                    break;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region AQUI OBTENEMOS EL VALOR DEL RENGLON EN EL E.F.
                                                            double _ValorRenglon = 0;   //--Double.Parse(dtValorRenglon.Rows[0]["saldo_final"].ToString().Trim());
                                                            switch (_NumeroRenglon)
                                                            {
                                                                case 9:
                                                                    _SumValorRenglon9 = _SumValorRenglon9 + _ValorRenglon;
                                                                    _SumValorRenglon10 = _SumValorRenglon9;
                                                                    break;
                                                                case 11:
                                                                    _SumValorRenglon11 = _SumValorRenglon11 + _ValorRenglon;
                                                                    break;
                                                                case 12:
                                                                    _SumValorRenglon12 = _SumValorRenglon12 + _ValorRenglon;
                                                                    break;
                                                                case 13:
                                                                    _SumValorRenglon13 = _SumValorRenglon13 + _ValorRenglon;
                                                                    break;
                                                                case 14:
                                                                    _SumValorRenglon14 = _SumValorRenglon14 + _ValorRenglon;
                                                                    break;
                                                                case 15:
                                                                    _SumValorRenglon15 = _SumValorRenglon15 + _ValorRenglon;
                                                                    break;
                                                                default:
                                                                    //_ValorRenglon = _ValorRenglon;
                                                                    break;
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                }

                                                #region GUARDAR LOS DATOS DE LA BG EN LA DB
                                                //--
                                                objProcessDb.IdMunicipio = _IdMunicipio;
                                                objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                                objProcessDb.IdCliente = objBase.id_cliente;
                                                objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                objProcessDb.CodigoDane = _CodigoDane;
                                                objProcessDb.AnioGravable = objBase.anio_gravable;
                                                objProcessDb.MesEf = objBase.mes_ef;
                                                //--VALORES DE LA BASE GRAVABLE
                                                objProcessDb.ValorRenglon8 = _SumValorRenglon8;
                                                //double _SumValorRenglon9Aux = (_SumValorRenglon8 - _SumValorRenglon9);
                                                objProcessDb.ValorRenglon9 = _SumValorRenglon9;
                                                //_SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9);
                                                objProcessDb.ValorRenglon10 = _SumValorRenglon10;
                                                //Console.WriteLine("\nVALORES: RENGLON 8 => " + _SumValorRenglon8 + ", RENGLON 9 => " + _SumValorRenglon9Aux + ", RENGLON 10 => " + _SumValorRenglon10);
                                                //--
                                                objProcessDb.ValorRenglon11 = _SumValorRenglon11;
                                                objProcessDb.ValorRenglon12 = _SumValorRenglon12;
                                                objProcessDb.ValorRenglon13 = _SumValorRenglon13;
                                                objProcessDb.ValorRenglon14 = _SumValorRenglon14;
                                                objProcessDb.ValorRenglon15 = _SumValorRenglon15;
                                                objProcessDb.ValorRenglon16 = 0;
                                                objProcessDb.ValorRenglon26 = _SumValorRenglon26;
                                                objProcessDb.ValorRenglon27 = _SumValorRenglon27;
                                                objProcessDb.ValorRenglon28 = _SumValorRenglon28;
                                                objProcessDb.ValorRenglon29 = _SumValorRenglon29;
                                                objProcessDb.ValorRenglon30 = _SumValorRenglon30;
                                                objProcessDb.ValorRenglon31 = _SumValorRenglon31;
                                                objProcessDb.ValorRenglon32 = _SumValorRenglon32;
                                                objProcessDb.ValorRenglon33 = _SumValorRenglon33;
                                                objProcessDb.ValorRenglon34 = _SumValorRenglon34;
                                                objProcessDb.ValorRenglon35 = _SumValorRenglon35;
                                                objProcessDb.ValorRenglon36 = _SumValorRenglon36;
                                                objProcessDb.ValorRenglon37 = _SumValorRenglon37;
                                                objProcessDb.VersionEf = objBase.version_ef;
                                                objProcessDb.IdEstado = 1;
                                                objProcessDb.IdUsuario = objBase.id_usuario;
                                                objProcessDb.TipoProceso = objBase.tipo_proceso;

                                                int _IdRegistro = 0;
                                                string _MsgError = "";
                                                if (objProcessDb.AddLoadBaseGravable_New(ref _IdRegistro, ref _MsgError))
                                                {
                                                    FixedData.LogApi.Info(_MsgError);
                                                }
                                                else
                                                {
                                                    #region PROCESO PARA EL ENVIO DEL CORREO
                                                    //--
                                                    ObjEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                                    ObjEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                                    ObjEmails.Asunto = "REF.: ERROR CON EL PROCESO BASE GRAVABLE";
                                                    ObjEmails.Detalle = "Señor usuario, ocurrio un error al registrar la Base Gravable [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + ", IdCliente Establecimiento: " + _IdClienteEstablecimiento + "], por favor validar con soporte técnico para ver que sucede. Posible Motivo: " + _MsgError;

                                                    string _MsgErrorEmail = "";
                                                    if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                                    {
                                                        FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                                    }
                                                    #endregion
                                                }
                                                //--
                                                #endregion

                                                //--MOSTRAR EL MENSAJE DEL PROCESO GUARDADO EN EL DATATABLE
                                                _ContadorOficProcesadas = _TotalEstablecimiento - _ContadorEstablecimiento;
                                                Console.WriteLine("\nFECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", ID MUNICIPIO: " + _IdMunicipio + ", ID ESTABLECIMIENTO: " + _IdClienteEstablecimiento + ", OFICINAS PROCESADAS: " + _ContadorEstablecimiento + ", DE: " + _ContadorOficProcesadas);

                                                #region AQUI VALIDAMOS SI TERMINO EL PROCESO PARA ENVIAR EL CORREO DE NOTIFICACION
                                                //--
                                                if (_ContadorEstablecimiento == _TotalEstablecimiento)
                                                {
                                                    #region AQUI ENVIAMO EL CORREO DE NOTIFICACION
                                                    //--
                                                    #region BORRAR LA TAREA PROGRAMADA Y CAMBIAR EL ESTADO DE LA TAREA
                                                    //--MANDAMOS A BORRAR LA TAREA PROGRAMADA
                                                    DeleteTaskSchedulerManual(objBase.nombre_tarea.ToString().Trim());

                                                    //--INSTANCIAMOS EL OBJETO DE CLASE
                                                    ProcessDb objProcess = new ProcessDb();
                                                    objProcess.TipoProceso = objBase.tipo_proceso;
                                                    objProcess.IdEstadoProceso = 10;
                                                    objProcess.TipoConsulta = 1;
                                                    _IdRegistro = 0;
                                                    string _MsgProcess = "";
                                                    bool _Result5 = objProcess.AddProcesoBaseGravable(ref _IdRegistro, ref _MsgProcess);
                                                    FixedData.LogApi.Warn("PROCESO TERMINADO => " + _MsgProcess);
                                                    #endregion

                                                    //--
                                                    ObjEmails.EmailPara = FixedData.EnvioEmail;
                                                    ObjEmails.EmailCopia = FixedData.EmailCopiaProcesos;
                                                    ObjEmails.Asunto = "REF.: PROCESO DE BASE GRAVABLE";

                                                    string nHora = DateTime.Now.ToString("HH");
                                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                    StringBuilder strDetalleEmail = new StringBuilder();
                                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario el proceso de la base gravable ha terminado de forma exitosa con [" + _ContadorEstablecimiento + " oficinas]" + "</h4>" +
                                                                "<br/><br/>" +
                                                                "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                                    ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                                    string _MsgErrorEmail = "";
                                                    if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                                    {
                                                        FixedData.LogApi.Error(_MsgErrorEmail);
                                                    }
                                                    #endregion
                                                }
                                                #endregion
                                                //--
                                                #endregion
                                            }
                                        }
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                        ObjEmails.Asunto = "REF.: NO SE ENCONTRO VALOR PARA EL RENGLON 8";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se encontro información para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER EL VALOR DEL RENGLON 8";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario ocurrio un error al obtener los datos para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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

                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER ESTABLECIMIENTOS";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener la lista de establecimientos para realizar el proceso de la BASE GRAVABLE." + "</h4>" +
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
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: EXCEPCION AL GENERAR LA BASE GRAVABLE";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al ejecutar el proceso de la BASE GRAVABLE. Motivo: " + ex.Message + "</h4>" +
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

            return _Result;
        }

        public DataTable GetTablaDatosBg()
        {
            DataTable DtBaseGravable = new DataTable();
            try
            {
                //--AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE PARA ALMACENAR LA INFO DE LA BASE GRAVABLE
                DtBaseGravable = new DataTable();
                DtBaseGravable.TableName = "DtBaseGravable";
                DtBaseGravable.Columns.Add("idbase_gravable", typeof(Int32));
                DtBaseGravable.PrimaryKey = new DataColumn[] { DtBaseGravable.Columns["idbase_gravable"] };
                DtBaseGravable.Columns.Add("id_municipio");
                DtBaseGravable.Columns.Add("idformulario_impuesto");
                DtBaseGravable.Columns.Add("id_cliente");
                DtBaseGravable.Columns.Add("idcliente_establecimiento");
                DtBaseGravable.Columns.Add("codigo_dane");
                DtBaseGravable.Columns.Add("anio_gravable");
                DtBaseGravable.Columns.Add("mes_ef");
                DtBaseGravable.Columns.Add("valor_renglon8", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon9", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon10", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon11", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon12", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon13", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon14", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon15", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon16", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon26", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon27", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon28", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon29", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon30", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon31", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon32", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon33", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon34", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon35", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon36", typeof(Double));
                DtBaseGravable.Columns.Add("valor_renglon37", typeof(Double));
                DtBaseGravable.Columns.Add("version_ef");
            }
            catch (Exception ex)
            {
                DtBaseGravable = null;
                FixedData.LogApi.Error("ERROR AL GENERAR EL DATA TABLE PARA LA BASE GRAVABLE. MOTIVO: " + ex.Message);
            }

            return DtBaseGravable;
        }

        public static double round(double input)
        {
            double ValorRenglo = Math.Abs(input);
            double _Result = 0;
            double rem = ValorRenglo % 1000;
            //return rem >= 5 ? (num - rem + 10) : (num - rem);
            if (rem >= 500)
            {
                _Result = (double)(1000 * Math.Ceiling(ValorRenglo / 1000));
            }
            else
            {
                _Result = (double)(1000 * Math.Round(ValorRenglo / 1000));
            }

            return _Result;
        }
        #endregion

        #region DEFINICION DE METODOS PARA PROCESAR LIQUIDACION POR OFICINAS
        public bool ProcessLiquidacionXOficinas(LiqOficinas_Req objImpustosOfic)
        {
            bool _Result = false;
            //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
            ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
            ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
            ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
            ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();

            //--AQUI CREAMOS EL DATATABLE DONDE SE GUARDA LA LIQUIDACIÓN DE LA OFICINA
            DataTable dtLiquidacionIca = new DataTable();
            dtLiquidacionIca = GetTablaDatosOficinas();
            int _IdMunicipio = 0;
            try
            {
                #region REALIZAR EL PROCESO DE LIQUIDACION DEL CLIENTE
                //--AQUI INSTANCIAMOS EL OBJETO CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 1;
                objProcessDb.IdCliente = objImpustosOfic.id_cliente;
                objProcessDb.AnioGravable = objImpustosOfic.anio_gravable;
                objProcessDb.MesEf = objImpustosOfic.mes_ef;
                objProcessDb.IdEstado = objImpustosOfic.id_estado;
                string _MsgError = "";

                //--PASO 1: OBTENER DATOS DE ESTADOS FINANCIEROS CARGADOS AL SISTEMA
                DataTable dtDatosEf = new DataTable();
                dtDatosEf = objProcessDb.GetEstadosFinanSinProcesar(ref _MsgError);
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
                                //int _AnioGravable = Int32.Parse(rowItemEf["anio_gravable"].ToString().Trim());
                                //string _MesEf = rowItemEf["mes_ef"].ToString().Trim();
                                _MsgError = "";

                                //--PASO 1: OBTENER DATOS DE MUNICIPIOS
                                DataTable dtMunicipios = new DataTable();
                                objProcessDb.TipoConsulta = 7;
                                objProcessDb.IdMunicipio = null;
                                dtMunicipios = objProcessDb.GetLstMunicipios();
                                //--
                                if (dtMunicipios != null)
                                {
                                    if (dtMunicipios.Rows.Count > 0)
                                    {
                                        foreach (DataRow rowItemMun in dtMunicipios.Rows)
                                        {
                                            _IdMunicipio = Int32.Parse(rowItemMun["id_municipio"].ToString().Trim());
                                            int _CantidadPuntos = Int32.Parse(rowItemMun["cantidad_puntos"].ToString().Trim());

                                            //--PASO 2: OBTENER DATOS DE OFICINAS A REALIZAR EL PROCESO DE LIQUIDACIÓN
                                            DataTable dtEstablecimientos = new DataTable();
                                            objProcessDb.TipoConsulta = 8;
                                            objProcessDb.IdMunicipio = _IdMunicipio;
                                            dtEstablecimientos = objProcessDb.GetLstEstablecimientos();
                                            //--
                                            if (dtEstablecimientos != null)
                                            {
                                                int _ContadorOficProcesadas = 0, _TotalEstablecimiento = 0, _ContadorEstablecimiento = 0;
                                                if (dtEstablecimientos.Rows.Count > 0)
                                                {
                                                    int _ContadorOficina = 0;
                                                    _TotalEstablecimiento = dtEstablecimientos.Rows.Count;
                                                    foreach (DataRow rowItemOfic in dtEstablecimientos.Rows)
                                                    {
                                                        #region OBTENER DATOS DE LA OFICINA
                                                        _ContadorEstablecimiento++;
                                                        int _IdClienteEstablecimiento = Int32.Parse(rowItemOfic["idcliente_establecimiento"].ToString().Trim());
                                                        int _IdTipoSector = Int32.Parse(rowItemOfic["idtipo_sector"].ToString().Trim());
                                                        //int _IdMunicipio = Int32.Parse(rowItemOfic["id_municipio"].ToString().Trim());
                                                        string _CodigoDane = rowItemOfic["codigo_dane"].ToString().Trim();
                                                        string _CodigoOficina = rowItemOfic["codigo_oficina"].ToString().Trim();
                                                        string _NombreOficina = rowItemOfic["nombre_oficina"].ToString().Trim();
                                                        int _NumEstablecimiento = Int32.Parse(rowItemOfic["numero_puntos"].ToString().Trim());

                                                        #region AQUI REGISTRAMOS EN EL DATATABLE LOS DATOS DE LA OFICINA A LIQUIDAR
                                                        DataRow Fila = null;
                                                        Fila = dtLiquidacionIca.NewRow();
                                                        Fila["idliquid_impuesto"] = dtLiquidacionIca.Rows.Count + 1;
                                                        Fila["id_municipio"] = _IdMunicipio;
                                                        Fila["idformulario_impuesto"] = objImpustosOfic.idform_impuesto;
                                                        Fila["id_cliente"] = objImpustosOfic.id_cliente;
                                                        Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                                        Fila["codigo_dane"] = _CodigoDane;
                                                        Fila["anio_gravable"] = objImpustosOfic.anio_gravable;
                                                        Fila["mes_liquidacion"] = objImpustosOfic.mes_ef;
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
                                                        Fila["tarifa_ica"] = 0;   //--EL VALOR DE ESTA COLUMNA ES IGUAL A LA COLUMNA total_ingresos_gravado                                                
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
                                                        objProcessDb.TipoConsulta = 5;
                                                        objProcessDb.IdCliente = objImpustosOfic.id_cliente;
                                                        objProcessDb.IdMunicipio = _IdMunicipio;
                                                        objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                        objProcessDb.IdFormularioImpuesto = objImpustosOfic.idform_impuesto;
                                                        objProcessDb.IdFormConfiguracion = null;
                                                        objProcessDb.IdPuc = null;
                                                        objProcessDb.AnioGravable = objImpustosOfic.anio_gravable;
                                                        //--objProcessDb.MesEf = objImpustosOfic.mes_ef;
                                                        objProcessDb.CodigoDane = _CodigoDane;
                                                        objProcessDb.IdEstado = objImpustosOfic.id_estado;
                                                        objProcessDb.TipoProceso = 3;   //--OBTENER DATOS DE LA BG POR OFICINA

                                                        //--AQUI OBTENEMOS LOS DATOS DEL ESTABLECIMIENTO EN LIQUIDACION DEL IMPUESTO
                                                        DataRow[] dataRows = dtLiquidacionIca.Select("idcliente_establecimiento = " + _IdClienteEstablecimiento + " AND TRIM(mes_liquidacion) = '" + objImpustosOfic.mes_ef + "'");
                                                        if (dataRows.Length == 1)
                                                        {
                                                            #region AQUI OBTENEMOS LA BASE GRAVABLE DEL MES ANTES
                                                            //--DEFINIMOS VARIABLES DE VALORES
                                                            double _ValorEFMesAntesR8 = 0, _ValorEFMesAntesR9 = 0, _ValorEFMesAntesR10 = 0, _ValorEFMesAntesR11 = 0, _ValorEFMesAntesR12 = 0, _ValorEFMesAntesR13 = 0;
                                                            double _ValorEFMesAntesR14 = 0, _ValorEFMesAntesR15 = 0, _ValorEFMesAntesR26 = 0, _ValorEFMesAntesR27 = 0, _ValorEFMesAntesR28 = 0;
                                                            double _ValorEFMesAntesR29 = 0, _ValorEFMesAntesR31 = 0, _ValorEFMesAntesR32 = 0, _ValorEFMesAntesR33 = 0, _ValorEFMesAntesR34 = 0;
                                                            double _ValorEFMesAntesR35 = 0, _ValorEFMesAntesR36 = 0, _ValorEFMesAntesR37 = 0;
                                                            DataTable dtBaseGravableMesAntes = new DataTable();

                                                            //--VALIDAR SI EL MES ES ENERO NO REALIZA ESTE PROCESO
                                                            if (!(objImpustosOfic.mes_ef.ToString().Trim().Equals("01") && objImpustosOfic.mes_ef.ToString().Trim().Equals("12")))
                                                            {
                                                                #region OBTENEMOS LOS VALORES DE LA SESION (B. BASE GRAVABLE)
                                                                //--
                                                                int _MesAntesEF = (Int32.Parse(objImpustosOfic.mes_ef.ToString().Trim()) - 1);
                                                                objProcessDb.MesEf = _MesAntesEF.ToString().PadLeft(2, '0');
                                                                dtBaseGravableMesAntes = objProcessDb.GetBaseGravableMesAntes();
                                                                //int _ContadorRow = 0;
                                                                if (dtBaseGravableMesAntes != null)
                                                                {
                                                                    if (dtBaseGravableMesAntes.Rows.Count > 0)
                                                                    {
                                                                        //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                                                        foreach (DataRow rowItem in dtBaseGravableMesAntes.Rows)
                                                                        {
                                                                            #region AQUI OBTENEMOS LOS VALORES DEL MES ANTERIOR
                                                                            int _IdBaseGravable = Int32.Parse(rowItem["idbase_gravable"].ToString().Trim());
                                                                            _ValorEFMesAntesR8 = Double.Parse(rowItem["valor_renglon8"].ToString().Trim());
                                                                            _ValorEFMesAntesR9 = Double.Parse(rowItem["valor_renglon9"].ToString().Trim());
                                                                            _ValorEFMesAntesR10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                                                            _ValorEFMesAntesR11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                                                            _ValorEFMesAntesR12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                                                            _ValorEFMesAntesR13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                                                            _ValorEFMesAntesR14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                                                            _ValorEFMesAntesR15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                                                            _ValorEFMesAntesR26 = Double.Parse(rowItem["valor_renglon26"].ToString().Trim());
                                                                            _ValorEFMesAntesR27 = Double.Parse(rowItem["valor_renglon27"].ToString().Trim());
                                                                            _ValorEFMesAntesR28 = Double.Parse(rowItem["valor_renglon28"].ToString().Trim());
                                                                            _ValorEFMesAntesR29 = Double.Parse(rowItem["valor_renglon29"].ToString().Trim());
                                                                            _ValorEFMesAntesR31 = Double.Parse(rowItem["valor_renglon31"].ToString().Trim());
                                                                            _ValorEFMesAntesR32 = Double.Parse(rowItem["valor_renglon32"].ToString().Trim());
                                                                            _ValorEFMesAntesR33 = Double.Parse(rowItem["valor_renglon33"].ToString().Trim());
                                                                            _ValorEFMesAntesR34 = Double.Parse(rowItem["valor_renglon34"].ToString().Trim());
                                                                            _ValorEFMesAntesR35 = Double.Parse(rowItem["valor_renglon35"].ToString().Trim());
                                                                            _ValorEFMesAntesR36 = Double.Parse(rowItem["valor_renglon36"].ToString().Trim());
                                                                            _ValorEFMesAntesR37 = Double.Parse(rowItem["valor_renglon37"].ToString().Trim());
                                                                            _Result = true;
                                                                            break;
                                                                            #endregion
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        #region AQUI DEFINIMOS EN CERO LOS VALORES DE LAS VARIABLES
                                                                        _Result = false;
                                                                        _ValorEFMesAntesR8 = 0;
                                                                        _ValorEFMesAntesR9 = 0;
                                                                        _ValorEFMesAntesR11 = 0;
                                                                        _ValorEFMesAntesR12 = 0;
                                                                        _ValorEFMesAntesR13 = 0;
                                                                        _ValorEFMesAntesR14 = 0;
                                                                        _ValorEFMesAntesR15 = 0;
                                                                        _ValorEFMesAntesR26 = 0;
                                                                        _ValorEFMesAntesR27 = 0;
                                                                        _ValorEFMesAntesR28 = 0;
                                                                        _ValorEFMesAntesR29 = 0;
                                                                        _ValorEFMesAntesR31 = 0;
                                                                        _ValorEFMesAntesR32 = 0;
                                                                        _ValorEFMesAntesR33 = 0;
                                                                        _ValorEFMesAntesR34 = 0;
                                                                        _ValorEFMesAntesR35 = 0;
                                                                        _ValorEFMesAntesR36 = 0;
                                                                        _ValorEFMesAntesR37 = 0;
                                                                        #endregion
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    #region AQUI DEFINIMOS EN CERO LOS VALORES DE LAS VARIABLES
                                                                    _Result = false;
                                                                    _ValorEFMesAntesR8 = 0;
                                                                    _ValorEFMesAntesR9 = 0;
                                                                    _ValorEFMesAntesR11 = 0;
                                                                    _ValorEFMesAntesR12 = 0;
                                                                    _ValorEFMesAntesR13 = 0;
                                                                    _ValorEFMesAntesR14 = 0;
                                                                    _ValorEFMesAntesR15 = 0;
                                                                    _ValorEFMesAntesR26 = 0;
                                                                    _ValorEFMesAntesR27 = 0;
                                                                    _ValorEFMesAntesR28 = 0;
                                                                    _ValorEFMesAntesR29 = 0;
                                                                    _ValorEFMesAntesR31 = 0;
                                                                    _ValorEFMesAntesR32 = 0;
                                                                    _ValorEFMesAntesR33 = 0;
                                                                    _ValorEFMesAntesR34 = 0;
                                                                    _ValorEFMesAntesR35 = 0;
                                                                    _ValorEFMesAntesR36 = 0;
                                                                    _ValorEFMesAntesR37 = 0;
                                                                    #endregion
                                                                }
                                                                #endregion
                                                            }
                                                            #endregion

                                                            #region OBTENEMOS LOS VALORES DE LA SESION (B. BASE GRAVABLE)
                                                            //--
                                                            objProcessDb.MesEf = objImpustosOfic.mes_ef.ToString().Trim();
                                                            DataTable dtBaseGravable = new DataTable();
                                                            dtBaseGravable = objProcessDb.GetBaseGravableConsolidada();
                                                            int _ContadorRow = 0;
                                                            double _ValorRenglon8 = 0, _ValorRenglon9 = 0, _ValorRenglon10 = 0, _ValorRenglon11 = 0;
                                                            double _ValorRenglon12 = 0, _ValorRenglon13 = 0, _ValorRenglon14 = 0, _ValorRenglon15 = 0;
                                                            double _ValorRenglon16 = 0, _ValorRenglon26 = 0, _ValorRenglon27 = 0, _ValorRenglon28 = 0;
                                                            double _ValorRenglon29 = 0, _ValorRenglon30 = 0, _ValorRenglon31 = 0, _ValorRenglon32 = 0;
                                                            double _ValorRenglon33 = 0, _ValorRenglon34 = 0, _ValorRenglon35 = 0, _ValorRenglon36 = 0, _ValorRenglon37 = 0;
                                                            double _SumRenglones = 0;
                                                            //--
                                                            if (dtBaseGravable != null)
                                                            {
                                                                if (dtBaseGravable.Rows.Count > 0)
                                                                {
                                                                    #region AQUI RECORREMOS LAS BASE GRAVABLE OBTENIDA AL MUNICIPIO
                                                                    //--
                                                                    foreach (DataRow rowItem in dtBaseGravable.Rows)
                                                                    {
                                                                        #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                                                        _ContadorRow++;
                                                                        int _IdBaseGravable = Int32.Parse(rowItem["idbase_gravable"].ToString().Trim());
                                                                        _ValorRenglon8 = Double.Parse(rowItem["valor_renglon8"].ToString().Trim());
                                                                        _ValorRenglon9 = Double.Parse(rowItem["valor_renglon9"].ToString().Trim());
                                                                        //_ValorRenglon10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                                                        //_ValorRenglon11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                                                        //_ValorRenglon12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                                                        //_ValorRenglon13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                                                        //_ValorRenglon14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                                                        //_ValorRenglon15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                                                        //_ValorRenglon16 = Double.Parse(rowItem["valor_renglon16"].ToString().Trim());
                                                                        _ValorRenglon26 = Double.Parse(rowItem["valor_renglon26"].ToString().Trim());
                                                                        _ValorRenglon27 = Double.Parse(rowItem["valor_renglon27"].ToString().Trim());
                                                                        _ValorRenglon28 = Double.Parse(rowItem["valor_renglon28"].ToString().Trim());
                                                                        _ValorRenglon29 = Double.Parse(rowItem["valor_renglon29"].ToString().Trim());
                                                                        //_ValorRenglon30 = Double.Parse(rowItem["valor_renglon30"].ToString().Trim());
                                                                        _ValorRenglon31 = Double.Parse(rowItem["valor_renglon31"].ToString().Trim());
                                                                        _ValorRenglon32 = Double.Parse(rowItem["valor_renglon32"].ToString().Trim());
                                                                        _ValorRenglon33 = Double.Parse(rowItem["valor_renglon33"].ToString().Trim());
                                                                        _ValorRenglon34 = Double.Parse(rowItem["valor_renglon34"].ToString().Trim());
                                                                        _ValorRenglon35 = Double.Parse(rowItem["valor_renglon35"].ToString().Trim());
                                                                        _ValorRenglon36 = Double.Parse(rowItem["valor_renglon36"].ToString().Trim());
                                                                        _ValorRenglon37 = Double.Parse(rowItem["valor_renglon37"].ToString().Trim());

                                                                        #region AQUI OBTENEMOS LA SUMATORIA DE LOS RENGLONES DEL 11 AL 15
                                                                        //--AQUI OBTENEMOS LA SUMATORIA DE LOS RENGLONES DEL 11 AL 15
                                                                        objProcessDb.TipoConsulta = 7;
                                                                        objProcessDb.IdMunicipio = _IdMunicipio;
                                                                        objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                                                        objProcessDb.AnioGravable = objImpustosOfic.anio_gravable;
                                                                        objProcessDb.MesEf = objImpustosOfic.mes_ef;
                                                                        objProcessDb.IdEstado = 1;
                                                                        objProcessDb.TipoProceso = 2;
                                                                        //--
                                                                        DataTable dtSumBaseGravable = new DataTable();
                                                                        dtSumBaseGravable = objProcessDb.GetSumaBaseGravable();
                                                                        if (dtSumBaseGravable != null)
                                                                        {
                                                                            if (dtSumBaseGravable.Rows.Count > 0)
                                                                            {
                                                                                foreach (DataRow rowItem2 in dtSumBaseGravable.Rows)
                                                                                {
                                                                                    _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                                                    _ValorRenglon11 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                                                    _ValorRenglon12 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                                                    _ValorRenglon13 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                                                    _ValorRenglon14 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                                                    _ValorRenglon15 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                _ValorRenglon10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                                                                _ValorRenglon11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                                                                _ValorRenglon12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                                                                _ValorRenglon13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                                                                _ValorRenglon14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                                                                _ValorRenglon15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            _ValorRenglon10 = Double.Parse(rowItem["valor_renglon10"].ToString().Trim());
                                                                            _ValorRenglon11 = Double.Parse(rowItem["valor_renglon11"].ToString().Trim());
                                                                            _ValorRenglon12 = Double.Parse(rowItem["valor_renglon12"].ToString().Trim());
                                                                            _ValorRenglon13 = Double.Parse(rowItem["valor_renglon13"].ToString().Trim());
                                                                            _ValorRenglon14 = Double.Parse(rowItem["valor_renglon14"].ToString().Trim());
                                                                            _ValorRenglon15 = Double.Parse(rowItem["valor_renglon15"].ToString().Trim());
                                                                        }
                                                                        #endregion

                                                                        //--AQUI CALCULAMOS EL VALOR DE LOS RENGLONES 8, 9 Y 10
                                                                        //double _TotalValorRenglon8 = (_ValorRenglon8 - FixedData.ValorRestarRenglon8);
                                                                        double _TotalValorRenglon8 = (_ValorRenglon8 - _ValorEFMesAntesR8);

                                                                        //--
                                                                        _SumRenglones = (_ValorRenglon11 + _ValorRenglon12 + _ValorRenglon13 + _ValorRenglon14 + _ValorRenglon15);
                                                                        double _SumRenglonesMesAntes = (_ValorEFMesAntesR11 + _ValorEFMesAntesR12 + _ValorEFMesAntesR13 + _ValorEFMesAntesR14 + _ValorEFMesAntesR15);
                                                                        double _ValorRenglon16_MesActual = (_ValorRenglon10 - _SumRenglones);
                                                                        double _ValorRenglon16_MesAntes = (_ValorEFMesAntesR10 - _SumRenglonesMesAntes);
                                                                        //_ValorRenglon16 = (_ValorRenglon10 - _SumRenglones);
                                                                        _ValorRenglon16 = (_ValorRenglon16_MesActual - _ValorRenglon16_MesAntes);

                                                                        //--
                                                                        dataRows[0]["valor_renglon8"] = _TotalValorRenglon8;  //--round(Math.Abs(_ValorRenglon8));
                                                                        dataRows[0]["valor_renglon9"] = (_ValorRenglon9 - _ValorEFMesAntesR9); //--round(Math.Abs(_ValorRenglon9));
                                                                        dataRows[0]["valor_renglon10"] = (_ValorRenglon10 - _ValorEFMesAntesR10);   //--round(Math.Abs(_ValorRenglon10));
                                                                        dataRows[0]["valor_renglon11"] = (_ValorRenglon11 - _ValorEFMesAntesR11);   //--round(Math.Abs(_ValorRenglon11));
                                                                        dataRows[0]["valor_renglon12"] = (_ValorRenglon12 - _ValorEFMesAntesR12);   //--round(Math.Abs(_ValorRenglon12));
                                                                        dataRows[0]["valor_renglon13"] = (_ValorRenglon13 - _ValorEFMesAntesR13);   //--round(Math.Abs(_ValorRenglon13));
                                                                        dataRows[0]["valor_renglon14"] = (_ValorRenglon14 - _ValorEFMesAntesR14);   //--round(Math.Abs(_ValorRenglon14));
                                                                        dataRows[0]["valor_renglon15"] = (_ValorRenglon15 - _ValorEFMesAntesR15);   //--round(Math.Abs(_ValorRenglon15));
                                                                        dataRows[0]["valor_renglon16"] = _ValorRenglon16;
                                                                        //dataRows[0]["valor_renglon16"] = _ValorRenglon16 >= 0 ? _ValorRenglon16 : (_ValorRenglon16 * -1);
                                                                        dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                        dtLiquidacionIca.Rows[0].EndEdit();
                                                                        break;
                                                                        #endregion
                                                                    }
                                                                    #endregion
                                                                }
                                                            }
                                                            else
                                                            {
                                                                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                                                //--
                                                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER LA BASE GRAVABLE";

                                                                string nHora = DateTime.Now.ToString("HH");
                                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de la BASE GRAVABLE esta llegando en NULL." + "</h4>" +
                                                                            "<br/><br/>" +
                                                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                                                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                                                string _MsgErrorEmail = "";
                                                                if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                                                                {
                                                                    FixedData.LogApi.Error(_MsgErrorEmail);
                                                                }
                                                                return false;
                                                                //--
                                                                #endregion
                                                            }
                                                            #endregion

                                                            #region OBTENEMOS LOS VALORES DE LA SESION (C. DISCRIMINACION DE ACTIVIDADES ECONOMICAS)
                                                            objProcessDb.TipoConsulta = 1;
                                                            objProcessDb.IdFormularioImpuesto = objImpustosOfic.idform_impuesto;
                                                            objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;

                                                            DataTable dtActEconomica = new DataTable();
                                                            dtActEconomica = objProcessDb.GetConsultarActEconomica();
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
                                                                                double _DefValorActividad = 0;  //--_SaldoFinal > 0 ? _SaldoFinal : _ValorRenglon16;

                                                                                //--AQUI VALIDAMOS EL TIPO DE SECTOR DEL CLIENTE
                                                                                //--EN CASO QUE SEA 1. FINANCIERO SE TOMA EL VALOR DEL RENGLON 16
                                                                                if (_IdTipoSector == 1)
                                                                                {
                                                                                    _DefValorActividad = _ValorRenglon16;
                                                                                }
                                                                                else
                                                                                {
                                                                                    _DefValorActividad = _SaldoFinal > 0 ? _SaldoFinal : _ValorRenglon16;
                                                                                }

                                                                                //--AQUI DEFINIMOS EL TIPO DE TARIFA
                                                                                if (_IdTipoTarifa == 1)      //--1. PORCENTUAL
                                                                                {
                                                                                    #region VALIDACION DE LA TARIFA PORCENTUAL
                                                                                    //--AQUI HACEMOS EL CALCULO DE LA TARIFA
                                                                                    if (_IdCalcularTarifaPor == 1)      //--1. TARIFA DE LEY
                                                                                    {
                                                                                        _ValorActividad = ((_DefValorActividad * _TarifaLey) / 100);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 100);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    //dataRows[0]["valor_actividad1"] = _DefValorActividad;   //--round(_DefValorActividad);
                                                                                    //dataRows[0]["valor_actividad1"] = _DefValorActividad >= 0 ? _DefValorActividad : (_DefValorActividad * -1);
                                                                                    dataRows[0]["valor_actividad1"] = (_DefValorActividad * -1);
                                                                                    //double _ValorTotalActividad = _ValorActividad >= 0 ? _ValorActividad : (_ValorActividad * -1);
                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["total_ingresos_gravado"] = _ValorTotalActividad;    //--round(_ValorActividad);
                                                                                    dataRows[0]["valor_renglon17"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        //_ValorActividad = (_DefValorActividad * _TarifaMunicipio);
                                                                                        _ValorActividad = ((_DefValorActividad * _TarifaMunicipio) / 1000);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    dataRows[0]["valor_actividad1"] = (_DefValorActividad * -1);
                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["total_ingresos_gravado"] = _ValorTotalActividad;    //--round(_ValorActividad);
                                                                                    dataRows[0]["valor_renglon17"] = _ValorTotalActividad;   //--round(_ValorActividad);
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

                                                                                    dataRows[0]["valor_actividad1"] = (_DefValorActividad * -1);
                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["total_ingresos_gravado"] = _ValorTotalActividad;    //--round(_ValorActividad);
                                                                                    dataRows[0]["valor_renglon17"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    dataRows[0]["valor_actividad2"] = (_SaldoFinal * -1);
                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["valor_renglon18"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["valor_renglon18"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdCalcularTarifaPor == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["valor_renglon18"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = ((_SaldoFinal * _TarifaMunicipio) / 100);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    dataRows[0]["valor_actividad3"] = _SaldoFinal;  //--round(_SaldoFinal);
                                                                                    dataRows[0]["valor_renglon19"] = _ValorActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["valor_renglon19"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaLey;
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 2)      //--1. TARIFA DEL MUNICIPIO
                                                                                    {
                                                                                        _ValorActividad = (_SaldoFinal * _TarifaMunicipio);
                                                                                        dataRows[0]["tarifa_ica"] = _TarifaMunicipio;
                                                                                    }

                                                                                    double _ValorTotalActividad = (_ValorActividad * -1);
                                                                                    dataRows[0]["valor_renglon19"] = _ValorTotalActividad;   //--round(_ValorActividad);
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
                                                                        dataRows[0]["total_ingresos_gravado"] = _TotalIngresosGravados; //--round(_TotalIngresosGravados);
                                                                        dataRows[0]["total_impuestos"] = _TotalImpuesto;    //--round(_TotalImpuesto);
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
                                                                return _Result;
                                                                //--
                                                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER LAS ACTIVIDADES ECONOMICAS";

                                                                string nHora = DateTime.Now.ToString("HH");
                                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de las ACTIVIDADES ECONOMICAS." + "</h4>" +
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

                                                            #region AQUI CALCULAMOS LOS IMPUESTOS DEL MUNICIPIO (D. LIQUIDACION PRIVADA)
                                                            _TotalImpuesto = Double.Parse(dataRows[0]["total_impuestos"].ToString().Trim());
                                                            double _TotalImpuestosLey = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                            double _ValorRenglon20 = (_TotalImpuesto + _TotalImpuestosLey);
                                                            //--
                                                            dataRows[0]["valor_renglon20"] = _ValorRenglon20;   //--round(_ValorRenglon20);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();

                                                            objProcessDb.IdMunicipio = _IdMunicipio;
                                                            objProcessDb.IdFormularioImpuesto = objImpustosOfic.idform_impuesto;
                                                            objProcessDb.AnioGravable = objImpustosOfic.anio_gravable;

                                                            DataTable dtImpMunicipio = new DataTable();
                                                            dtImpMunicipio = objProcessDb.GetImpuestosMunicipio();
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
                                                                        else
                                                                        {
                                                                            _ValorLiquidacion = 0;
                                                                        }

                                                                        #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                                        switch (_NumRenglon)
                                                                        {
                                                                            case 21:
                                                                                dataRows[0]["valor_renglon21"] = _ValorLiquidacion; //--round(_ValorLiquidacion);
                                                                                dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                dtLiquidacionIca.Rows[0].EndEdit();
                                                                                break;
                                                                            //case 22:
                                                                            //    this.LblValorRenglon22.Text = String.Format(String.Format("{0:###,###,##0}", _ValorLiquidacion));
                                                                            //    break;
                                                                            case 23:
                                                                                dataRows[0]["valor_renglon23"] = _ValorLiquidacion; //--round(_ValorLiquidacion);
                                                                                dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                dtLiquidacionIca.Rows[0].EndEdit();
                                                                                break;
                                                                            case 24:
                                                                                dataRows[0]["valor_renglon24"] = _ValorLiquidacion; //--round(_ValorLiquidacion);
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
                                                            #endregion

                                                            #region OBTENEMOS LOS VALORES DE OTRAS CONFIGURACIONES DE RENGLONES
                                                            //--DEFINIMOS LAS VARIABLES
                                                            double _ValorRenglon21 = 0, _ValorRenglon22 = 0, _ValorRenglon23 = 0, _ValorRenglon24 = 0, _ValorRenglon25 = 0;

                                                            objProcessDb.AnioGravable = objImpustosOfic.anio_gravable;
                                                            objProcessDb.IdFormularioImpuesto = objImpustosOfic.idform_impuesto;
                                                            objProcessDb.IdMunicipio = _IdMunicipio;
                                                            //--
                                                            DataTable dtTarifasMin = new DataTable();
                                                            //dtTarifasMin = objProcessDb.GetTarifaMinMunicipio();
                                                            dtTarifasMin = objProcessDb.GetOtrasConfMunicipio();
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
                                                                        string _NumeroRenglon1 = rowItem["numero_renglon1"].ToString().Trim().Length > 0 ? rowItem["numero_renglon1"].ToString().Trim() : "0";
                                                                        string _IdTipoOperacion1 = rowItem["idtipo_operacion1"].ToString().Trim().Length > 0 ? rowItem["idtipo_operacion1"].ToString().Trim() : "0";
                                                                        string _NumeroRenglon2 = rowItem["numero_renglon2"].ToString().Trim().Length > 0 ? rowItem["numero_renglon2"].ToString().Trim() : "0";
                                                                        string _IdTipoOperacion2 = rowItem["idtipo_operacion2"].ToString().Trim().Length > 0 ? rowItem["idtipo_operacion2"].ToString().Trim() : "0";
                                                                        string _NumeroRenglon3 = rowItem["numero_renglon3"].ToString().Trim().Length > 0 ? rowItem["numero_renglon3"].ToString().Trim() : "0";
                                                                        string _IdTipoOperacion3 = rowItem["idtipo_operacion3"].ToString().Trim().Length > 0 ? rowItem["idtipo_operacion3"].ToString().Trim() : "0";
                                                                        string _NumeroRenglon4 = rowItem["numero_renglon4"].ToString().Trim().Length > 0 ? rowItem["numero_renglon4"].ToString().Trim() : "0";
                                                                        string _IdTipoOperacion4 = rowItem["idtipo_operacion4"].ToString().Trim().Length > 0 ? rowItem["idtipo_operacion4"].ToString().Trim() : "0";
                                                                        string _NumeroRenglon5 = rowItem["numero_renglon5"].ToString().Trim().Length > 0 ? rowItem["numero_renglon5"].ToString().Trim() : "0";
                                                                        string _IdTipoOperacion5 = rowItem["idtipo_operacion5"].ToString().Trim().Length > 0 ? rowItem["idtipo_operacion5"].ToString().Trim() : "0";
                                                                        string _NumeroRenglon6 = rowItem["numero_renglon6"].ToString().Trim().Length > 0 ? rowItem["numero_renglon6"].ToString().Trim() : "0";

                                                                        int _IdUnidadMedida = Int32.Parse(rowItem["idunidad_medida"].ToString().Trim());
                                                                        int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                                                                        double _ValorUnidad = Double.Parse(rowItem["valor_unidad"].ToString().Trim());
                                                                        double _ValorUnidadMedida = Double.Parse(rowItem["valor_unid_medida"].ToString().Trim());
                                                                        double _CantidadMedida = Double.Parse(rowItem["cantidad_medida"].ToString().Trim());
                                                                        double _CantidadPeriodos = Double.Parse(rowItem["cantidad_periodos"].ToString().Trim());
                                                                        //--VALIDAR EL VALOR DE LA UNIDAD
                                                                        double _TotalUnidad = 0;
                                                                        if (_ValorUnidadMedida > 0)
                                                                        {
                                                                            _TotalUnidad = ((_ValorUnidadMedida * _CantidadMedida) * _CantidadPeriodos);
                                                                        }
                                                                        else
                                                                        {
                                                                            _TotalUnidad = ((_ValorUnidad * _CantidadMedida) * _CantidadPeriodos);
                                                                        }
                                                                        #endregion

                                                                        #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                                                                        double _ValorTarifaMinima = 0, _OperacionRenglon = 0, _ValorTotalNumRenglon = 0, _ValorNumRenglon1 = 0, _ValorNumRenglon2 = 0, _ValorNumRenglon3 = 0, _ValorNumRenglon4 = 0, _ValorNumRenglon5 = 0, _ValorNumRenglon6 = 0;
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
                                                                                    int _TipoOperacion = _IdTipoOperacion1.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion1.Trim()) : 0;
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
                                                                                    int _NumeroRenglonCalc = _NumeroRenglon1.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon1.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
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
                                                                                        case 17:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
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
                                                                                        case 30:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon1 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon2.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon2.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
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
                                                                                        case 17:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon2 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 3
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon3.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon3.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon3 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 4
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon4.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon4.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon4 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 5
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon5.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon5.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon6 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 5
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon6.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon6.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon6 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                                    int _TipoOperacion1 = _IdTipoOperacion1.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion1.Trim()) : 0;
                                                                                    int _TipoOperacion2 = _IdTipoOperacion2.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion2.Trim()) : 0;
                                                                                    int _TipoOperacion3 = _IdTipoOperacion3.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion3.Trim()) : 0;
                                                                                    int _TipoOperacion4 = _IdTipoOperacion4.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion4.Trim()) : 0;
                                                                                    int _TipoOperacion5 = _IdTipoOperacion5.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion5.Trim()) : 0;
                                                                                    //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 1
                                                                                    switch (_TipoOperacion1)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                                                            break;
                                                                                        default:
                                                                                            _OperacionRenglon = _ValorNumRenglon1;
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 2
                                                                                    switch (_TipoOperacion2)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon3);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 3
                                                                                    switch (_TipoOperacion3)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon4);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 4
                                                                                    switch (_TipoOperacion4)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon5);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 5
                                                                                    switch (_TipoOperacion5)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon6);
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    //--AQUI VALIDAMOS EL TIPO DE UNIDAD 5. INDICA QUE NO APLICA
                                                                                    //if (_IdUnidadMedida != 5)
                                                                                    //{
                                                                                    //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                                    if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                                    {
                                                                                        if (_CantidadMedida > 0)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _OperacionRenglon;
                                                                                        }
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                                    {
                                                                                        if (_CantidadMedida > 0)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = (_OperacionRenglon * _CantidadMedida);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _OperacionRenglon;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //--AQUI VALIDAMOS SI LA UNIDAD ES VALOR ABSOLUTO
                                                                                        if (_IdUnidadMedida == 4)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _TotalUnidad;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = 0;
                                                                                        }
                                                                                    }

                                                                                    double _SumatoriaRenglon22 = 0;
                                                                                    if (_NumEstablecimiento > 0)
                                                                                    {
                                                                                        _ContadorOficina++;
                                                                                        if (_CantidadPuntos > 1)
                                                                                        {
                                                                                            //--
                                                                                            _NumEstablecimiento = (_CantidadPuntos - 1);
                                                                                            //--
                                                                                            if (_ContadorOficina < _CantidadPuntos)
                                                                                            {
                                                                                                //--
                                                                                                _SumatoriaRenglon22 = (_ValorTotalNumRenglon / 12);
                                                                                                dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                dataRows[0]["valor_renglon22"] = 0;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //--
                                                                                            _SumatoriaRenglon22 = 0;    //--((_ValorTarifaMinima / 12) * _NumEstablecimiento);
                                                                                            dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                                        }
                                                                                    }
                                                                                    #endregion

                                                                                    //dataRows[0]["valor_renglon22"] = _ValorTotalNumRenglon;
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    #region OBTENER EL VALOR DE LAS UNIDADES ADICIONALES MENOS 1 OFICINA
                                                                                    //--ESCRIBIMOS EL ID MUNICIPIO
                                                                                    FixedData.LogApi.Warn("ID MUNICIPIO OTRAS CONFIGURACIONES => " + _IdMunicipio);

                                                                                    //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                                    if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                                    {
                                                                                        _ValorTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                                    {
                                                                                        _ValorTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //--AQUI VALIDAMOS SI LA UNIDAD ES VALOR ABSOLUTO
                                                                                        if (_IdUnidadMedida == 4)
                                                                                        {
                                                                                            _ValorTarifaMinima = _TotalUnidad;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTarifaMinima = 0;
                                                                                        }
                                                                                    }

                                                                                    double _SumatoriaRenglon22 = 0;
                                                                                    if (_NumEstablecimiento > 0)
                                                                                    {
                                                                                        _ContadorOficina++;
                                                                                        if (_CantidadPuntos > 1)
                                                                                        {
                                                                                            //--
                                                                                            _NumEstablecimiento = (_CantidadPuntos - 1);
                                                                                            //_SumatoriaRenglon22 = ((_ValorTarifaMinima / 12) * _NumEstablecimiento);
                                                                                            //dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;

                                                                                            //--
                                                                                            if (_ContadorOficina < _CantidadPuntos)
                                                                                            {
                                                                                                //--
                                                                                                _SumatoriaRenglon22 = (_ValorTarifaMinima / 12);
                                                                                                dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                dataRows[0]["valor_renglon22"] = 0;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //--
                                                                                            _SumatoriaRenglon22 = 0;    //--((_ValorTarifaMinima / 12) * _NumEstablecimiento);
                                                                                            dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //--
                                                                                        _SumatoriaRenglon22 = 0;    //--((_ValorTarifaMinima / 12) * _NumEstablecimiento);
                                                                                        dataRows[0]["valor_renglon22"] = _SumatoriaRenglon22;
                                                                                    }

                                                                                    FixedData.LogApi.Warn("VALOR RENGLON 22 => " + _SumatoriaRenglon22);
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                    #endregion
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
                                                                                dataRows[0]["valor_renglon25"] = _ValorRenglon25;   //--round(_ValorRenglon25);
                                                                                dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                dtLiquidacionIca.Rows[0].EndEdit();
                                                                                #endregion
                                                                                break;
                                                                            case 23:
                                                                                #region VALIDAMOS SI ES CALCULADO Y LA OPERACION
                                                                                if (_CalcularRenglon.Equals("S"))
                                                                                {
                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 1
                                                                                    //--
                                                                                    int _NumeroRenglonCalc = _NumeroRenglon1.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon1.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
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
                                                                                        case 17:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
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
                                                                                        case 30:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon1 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon1 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 2
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon2.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon2.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
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
                                                                                        case 17:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon2 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon2 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 3
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon3.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon3.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon3 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon3 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 4
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon4.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon4.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon4 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon4 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 5
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon5.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon5.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon5 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon6 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI VALIDAMOS EL VALOR QUE VA A TENER EL VALOR 5
                                                                                    //--
                                                                                    _NumeroRenglonCalc = _NumeroRenglon6.Trim().Length > 0 ? Int32.Parse(_NumeroRenglon6.Trim()) : 0;
                                                                                    switch (_NumeroRenglonCalc)
                                                                                    {
                                                                                        case 8:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                            break;
                                                                                        case 9:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon9"].ToString().Trim());
                                                                                            break;
                                                                                        case 10:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                            break;
                                                                                        case 11:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon11"].ToString().Trim());
                                                                                            break;
                                                                                        case 12:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon12"].ToString().Trim());
                                                                                            break;
                                                                                        case 13:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon13"].ToString().Trim());
                                                                                            break;
                                                                                        case 14:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon14"].ToString().Trim());
                                                                                            break;
                                                                                        case 15:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon15"].ToString().Trim());
                                                                                            break;
                                                                                        case 16:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon16"].ToString().Trim());
                                                                                            break;
                                                                                        case 17:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon17"].ToString().Trim());
                                                                                            break;
                                                                                        case 19:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon19"].ToString().Trim());
                                                                                            break;
                                                                                        case 20:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                                                            break;
                                                                                        case 21:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                                                            break;
                                                                                        case 22:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                                                            break;
                                                                                        case 23:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                                                            break;
                                                                                        case 24:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                                                            break;
                                                                                        case 25:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon25"].ToString().Trim());
                                                                                            break;
                                                                                        case 26:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                                                            break;
                                                                                        case 27:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                                                            break;
                                                                                        case 28:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                                                            break;
                                                                                        case 29:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                                                            break;
                                                                                        case 30:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                                                            break;
                                                                                        case 31:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                                                            break;
                                                                                        case 32:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());
                                                                                            break;
                                                                                        case 33:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon33"].ToString().Trim());
                                                                                            break;
                                                                                        case 34:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon34"].ToString().Trim());
                                                                                            break;
                                                                                        case 35:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                                                            break;
                                                                                        case 36:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                                                            break;
                                                                                        case 37:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                                                            break;
                                                                                        case 38:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_renglon38"].ToString().Trim());
                                                                                            break;
                                                                                        case 39:
                                                                                            _ValorNumRenglon6 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                                                            break;
                                                                                        default:
                                                                                            _ValorNumRenglon6 = 0;
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    #region AQUI REALIZAMOS EL TIPO DE OPERACION CONFIGURADO
                                                                                    int _TipoOperacion1 = _IdTipoOperacion1.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion1.Trim()) : 0;
                                                                                    int _TipoOperacion2 = _IdTipoOperacion2.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion2.Trim()) : 0;
                                                                                    int _TipoOperacion3 = _IdTipoOperacion3.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion3.Trim()) : 0;
                                                                                    int _TipoOperacion4 = _IdTipoOperacion4.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion4.Trim()) : 0;
                                                                                    int _TipoOperacion5 = _IdTipoOperacion5.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion5.Trim()) : 0;
                                                                                    //--1. SUMA, 2. RESTA, 3. MULTIPLICACION, 4. DIVISION
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 1
                                                                                    switch (_TipoOperacion1)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 + _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 - _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 * _ValorNumRenglon2);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_ValorNumRenglon1 / _ValorNumRenglon2);
                                                                                            break;
                                                                                        default:
                                                                                            _OperacionRenglon = _ValorNumRenglon1;
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 2
                                                                                    switch (_TipoOperacion2)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon3);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon3);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 3
                                                                                    switch (_TipoOperacion3)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon4);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon4);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 4
                                                                                    switch (_TipoOperacion4)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon5);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon5);
                                                                                            break;
                                                                                    }
                                                                                    #endregion
                                                                                    //--
                                                                                    #region AQUI VALIDAMOS EL TIPO DE OPERACION 5
                                                                                    switch (_TipoOperacion5)
                                                                                    {
                                                                                        case 1:
                                                                                            _OperacionRenglon = (_OperacionRenglon + _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 2:
                                                                                            _OperacionRenglon = (_OperacionRenglon - _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 3:
                                                                                            _OperacionRenglon = (_OperacionRenglon * _ValorNumRenglon6);
                                                                                            break;
                                                                                        case 4:
                                                                                            _OperacionRenglon = (_OperacionRenglon / _ValorNumRenglon6);
                                                                                            break;
                                                                                    }
                                                                                    #endregion

                                                                                    //--AQUI VALIDAMOS EL TIPO DE UNIDAD 5. INDICA QUE NO APLICA
                                                                                    //if (_IdUnidadMedida != 5)
                                                                                    //{
                                                                                    //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                                    if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                                    {
                                                                                        if (_CantidadMedida > 0)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = ((_OperacionRenglon * _CantidadMedida) / 100);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _OperacionRenglon;
                                                                                        }
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                                    {
                                                                                        if (_CantidadMedida > 0)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = (_OperacionRenglon * _CantidadMedida);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _OperacionRenglon;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //--AQUI VALIDAMOS SI LA UNIDAD ES VALOR ABSOLUTO
                                                                                        if (_IdUnidadMedida == 4)
                                                                                        {
                                                                                            _ValorTotalNumRenglon = _TotalUnidad;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTotalNumRenglon = 0;
                                                                                        }
                                                                                    }
                                                                                    #endregion

                                                                                    //--
                                                                                    dataRows[0]["valor_renglon23"] = _ValorTotalNumRenglon;
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    #region OBTENER EL VALOR DE LAS UNIDADES ADICIONALES MENOS 1 OFICINA
                                                                                    //--ESCRIBIMOS EL ID MUNICIPIO
                                                                                    FixedData.LogApi.Warn("ID MUNICIPIO OTRAS CONFIGURACIONES => " + _IdMunicipio);

                                                                                    //--AQUI VALIDAMOS EL TIPO DE TARIFA
                                                                                    if (_IdTipoTarifa == 1)         //--PORCENTUAL
                                                                                    {
                                                                                        _ValorTarifaMinima = ((_ValorUnidad * _CantidadMedida) / 100);
                                                                                    }
                                                                                    else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                                                                                    {
                                                                                        _ValorTarifaMinima = (_ValorUnidad * _CantidadMedida);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //--AQUI VALIDAMOS SI LA UNIDAD ES VALOR ABSOLUTO
                                                                                        if (_IdUnidadMedida == 4)
                                                                                        {
                                                                                            _ValorTarifaMinima = _TotalUnidad;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            _ValorTarifaMinima = 0;
                                                                                        }
                                                                                    }
                                                                                    //--
                                                                                    FixedData.LogApi.Warn("VALOR RENGLON 23 => " + _ValorTarifaMinima);
                                                                                    dataRows[0]["valor_renglon23"] = _ValorTarifaMinima;
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                    #endregion
                                                                                }
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
                                                                                    int _TipoOperacion = _IdTipoOperacion1.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion1.Trim()) : 0;
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
                                                                                    dataRows[0]["valor_renglon30"] = _OperacionRenglon30;   //--round(_OperacionRenglon30);
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                    #endregion
                                                                                }
                                                                                else
                                                                                {
                                                                                    //--
                                                                                    dataRows[0]["valor_renglon30"] = 0;
                                                                                    dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                    dtLiquidacionIca.Rows[0].EndEdit();
                                                                                }
                                                                                #endregion

                                                                                #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                                                                ////--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                                                                //_ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon8"].ToString().Trim());
                                                                                //_ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon31 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon10"].ToString().Trim());
                                                                                //_ValorRenglon33Aux = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                                                                //_TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                                                                ////--
                                                                                //dataRows[0]["valor_renglon33"] = round(_ValorRenglon33Aux);
                                                                                //dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                                                                //dataRows[0]["valor_renglon35"] = round(_ValorRenglon33Aux);
                                                                                //dtLiquidacionIca.Rows[0].AcceptChanges();
                                                                                //dtLiquidacionIca.Rows[0].EndEdit();
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
                                                                                    int _TipoOperacion = _IdTipoOperacion1.Trim().Length > 0 ? Int32.Parse(_IdTipoOperacion1.Trim()) : 0;
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
                                                                                    dataRows[0]["valor_renglon36"] = _OperacionRenglon36;   //--round(_OperacionRenglon36);
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
                                                                return _Result;
                                                                //--
                                                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER LAS TARIFAS MINIMAS";

                                                                string nHora = DateTime.Now.ToString("HH");
                                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de las TARIFAS  MINIMAS." + "</h4>" +
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

                                                            ///---------------
                                                            #region AQUI CALCULAMOS EL VALOR DEL RENGLON 25 (TOTAL DE IMPUESTO A CARGO)
                                                            _ValorRenglon20 = Double.Parse(dataRows[0]["valor_renglon20"].ToString().Trim());
                                                            _ValorRenglon21 = Double.Parse(dataRows[0]["valor_renglon21"].ToString().Trim());
                                                            _ValorRenglon22 = Double.Parse(dataRows[0]["valor_renglon22"].ToString().Trim());
                                                            _ValorRenglon23 = Double.Parse(dataRows[0]["valor_renglon23"].ToString().Trim());
                                                            _ValorRenglon24 = Double.Parse(dataRows[0]["valor_renglon24"].ToString().Trim());
                                                            _ValorRenglon25 = _ValorRenglon20 + _ValorRenglon21 + _ValorRenglon22 + _ValorRenglon23 + _ValorRenglon24;

                                                            //--ACTUALIZAR DATOS
                                                            dataRows[0]["valor_renglon25"] = _ValorRenglon25;   //--round(_ValorRenglon25);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion

                                                            #region AQUI CALCULAMOS EL VALOR DEL RENGLON 33 y 34 
                                                            //--AQUI CALCULAMOS EL VALOR DEL RENGLON 33 (TOTAL SALDO A CARGO)
                                                            _ValorRenglon26 = Double.Parse(dataRows[0]["valor_renglon26"].ToString().Trim());
                                                            _ValorRenglon27 = Double.Parse(dataRows[0]["valor_renglon27"].ToString().Trim());
                                                            _ValorRenglon28 = Double.Parse(dataRows[0]["valor_renglon28"].ToString().Trim());
                                                            _ValorRenglon29 = Double.Parse(dataRows[0]["valor_renglon29"].ToString().Trim());
                                                            _ValorRenglon30 = Double.Parse(dataRows[0]["valor_renglon30"].ToString().Trim());
                                                            _ValorRenglon31 = Double.Parse(dataRows[0]["valor_sancion"].ToString().Trim());
                                                            _ValorRenglon32 = Double.Parse(dataRows[0]["valor_renglon32"].ToString().Trim());

                                                            //double _TotalSaldoCargo = (_ValorRenglon25 - (_ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + (_ValorRenglon30 + _ValorRenglon31) + _ValorRenglon32);
                                                            double _TotalSaldoCargoAux = ((_ValorRenglon25 - _ValorRenglon26 - _ValorRenglon27 - _ValorRenglon28 - _ValorRenglon29) + _ValorRenglon30 + _ValorRenglon31 - _ValorRenglon32);
                                                            double _ValorRenglon33Aux = _TotalSaldoCargoAux >= 0 ? _TotalSaldoCargoAux : 0;
                                                            double _TotalSaldoFavor = (((-(_ValorRenglon25)) + _ValorRenglon26 + _ValorRenglon27 + _ValorRenglon28 + _ValorRenglon29) - (_ValorRenglon30 - _ValorRenglon31) + _ValorRenglon32);
                                                            //--
                                                            dataRows[0]["valor_renglon33"] = _ValorRenglon33Aux;    //--round(_ValorRenglon33Aux);
                                                            dataRows[0]["valor_renglon34"] = _TotalSaldoFavor >= 0 ? _TotalSaldoFavor : 0;
                                                            dataRows[0]["valor_renglon35"] = _ValorRenglon33Aux;    //--round(_ValorRenglon33Aux);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();
                                                            #endregion

                                                            #region CALCULAR RENGLONES 36, 37, 38 y 39
                                                            //--
                                                            _ValorRenglon35 = Double.Parse(dataRows[0]["valor_renglon35"].ToString().Trim());
                                                            _ValorRenglon36 = Double.Parse(dataRows[0]["valor_renglon36"].ToString().Trim());
                                                            _ValorRenglon37 = Double.Parse(dataRows[0]["interes_mora"].ToString().Trim());
                                                            double _ValorRenglon38 = (_ValorRenglon35 - _ValorRenglon36 + _ValorRenglon37);
                                                            double _ValorRenglon39 = Double.Parse(dataRows[0]["valor_pago_voluntario"].ToString().Trim());
                                                            double _ValorRenglon40 = (_ValorRenglon38 + _ValorRenglon39);
                                                            //--
                                                            if (_ValorRenglon40 > 0)
                                                            {
                                                                dataRows[0]["valor_renglon40"] = _ValorRenglon40;
                                                            }
                                                            else
                                                            {
                                                                dataRows[0]["valor_renglon40"] = _TotalTarifaMinima;
                                                            }
                                                            //-
                                                            dataRows[0]["valor_renglon38"] = _ValorRenglon38;   //--round(_ValorRenglon38);
                                                            dtLiquidacionIca.Rows[0].AcceptChanges();
                                                            dtLiquidacionIca.Rows[0].EndEdit();

                                                            //--MOSTRAR EL MENSAJE DEL PROCESO GUARDADO EN EL DATATABLE
                                                            _ContadorOficProcesadas = _TotalEstablecimiento - _ContadorEstablecimiento;
                                                            Console.WriteLine("\nFECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", ID MUNICIPIO: " + _IdMunicipio + ", ID ESTABLECIMIENTO: " + _IdClienteEstablecimiento + ", OFICINAS PROCESADAS: " + _ContadorEstablecimiento + ", DE: " + _ContadorOficProcesadas);
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
                                                    _Result = false;
                                                    //--
                                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINAS";

                                                    string nHora = DateTime.Now.ToString("HH");
                                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                    StringBuilder strDetalleEmail = new StringBuilder();
                                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de las oficinas." + "</h4>" +
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
                                                _Result = false;
                                                //--
                                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINAS";

                                                string nHora = DateTime.Now.ToString("HH");
                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de las oficinas." + "</h4>" +
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
                            }

                            //--AQUI ENVIAMOS LOS DATOS A LA DB
                            if (dtLiquidacionIca != null)
                            {
                                if (dtLiquidacionIca.Rows.Count > 0)
                                {
                                    #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE
                                    //--
                                    int _TotalOficinas = dtLiquidacionIca.Rows.Count;
                                    foreach (DataRow rowItem in dtLiquidacionIca.Rows)
                                    {
                                        #region OBTENER VALORES DEL DATATABLE PARA ENVIAR A LA DB
                                        int _ContadorRow = Int32.Parse(rowItem["idliquid_impuesto"].ToString().Trim());
                                        int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                        int _IdFormularioImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                        int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                        _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                        string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                        int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                        string _MesLiquidacion = rowItem["mes_liquidacion"].ToString().Trim();
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
                                        double _TotalIngGravado = Double.Parse(rowItem["total_ingresos_gravado"].ToString().Trim());
                                        //double _TotalImpuestos = Double.Parse(rowItem["total_impuestos"].ToString().Trim());
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
                                        double _TarifaIca = Double.Parse(rowItem["tarifa_ica"].ToString().Trim());
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

                                        #region AQUI PASAMOS LOS VALORES A GUARDAR EN LA DB
                                        objProcessDb.IdMunicipio = _IdMunicipio;
                                        objProcessDb.IdFormularioImpuesto = _IdFormularioImpuesto;
                                        objProcessDb.IdCliente = _IdCliente;
                                        objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                        objProcessDb.CodigoDane = _CodigoDane;
                                        objProcessDb.AnioGravable = _AnioGravable;
                                        objProcessDb.MesLiquidacion = _MesLiquidacion;
                                        objProcessDb.ValorRenglon8 = _ValorRenglon8;
                                        objProcessDb.ValorRenglon9 = _ValorRenglon9;
                                        objProcessDb.ValorRenglon10 = _ValorRenglon10;
                                        objProcessDb.ValorRenglon11 = _ValorRenglon11;
                                        objProcessDb.ValorRenglon12 = _ValorRenglon12;
                                        objProcessDb.ValorRenglon13 = _ValorRenglon13;
                                        objProcessDb.ValorRenglon14 = _ValorRenglon14;
                                        objProcessDb.ValorRenglon15 = _ValorRenglon15;
                                        objProcessDb.ValorRenglon16 = _ValorRenglon16;
                                        objProcessDb.ValorActividad1 = _ValorActividad1;
                                        objProcessDb.ValorActividad2 = _ValorActividad2;
                                        objProcessDb.ValorActividad3 = _ValorActividad3;
                                        objProcessDb.ValorOtrasAct = _ValorOtrasAct;
                                        objProcessDb.TotalIngGravado = _TotalIngGravado;
                                        objProcessDb.ValorRenglon17 = _ValorRenglon17;
                                        objProcessDb.ValorRenglon18 = _ValorRenglon18;
                                        objProcessDb.ValorRenglon19 = _ValorRenglon19;
                                        objProcessDb.ValorRenglon20 = _ValorRenglon20;
                                        objProcessDb.ValorRenglon21 = _ValorRenglon21;
                                        objProcessDb.ValorRenglon22 = _ValorRenglon22;
                                        objProcessDb.ValorRenglon23 = _ValorRenglon23;
                                        objProcessDb.ValorRenglon24 = _ValorRenglon24;
                                        objProcessDb.ValorRenglon25 = _ValorRenglon25;
                                        objProcessDb.ValorRenglon26 = _ValorRenglon26;
                                        objProcessDb.ValorRenglon27 = _ValorRenglon27;
                                        objProcessDb.ValorRenglon28 = _ValorRenglon28;
                                        objProcessDb.ValorRenglon29 = _ValorRenglon29;
                                        objProcessDb.ValorRenglon30 = _ValorRenglon30;
                                        objProcessDb.TarifaIca = _TarifaIca;
                                        objProcessDb.BaseGravBomberil = _BaseGravBomberil;
                                        objProcessDb.BaseGravSeguridad = _BaseGravSeguridad;
                                        objProcessDb.Sanciones = _Sanciones;
                                        objProcessDb.ValorSancion = _ValorSancion;
                                        objProcessDb.ValorRenglon32 = _ValorRenglon32;
                                        objProcessDb.ValorRenglon33 = _ValorRenglon33;
                                        objProcessDb.ValorRenglon34 = _ValorRenglon34;
                                        objProcessDb.ValorRenglon35 = _ValorRenglon35;
                                        objProcessDb.ValorRenglon36 = _ValorRenglon36;
                                        objProcessDb.InteresMora = _InteresMora;
                                        objProcessDb.ValorRenglon38 = _ValorRenglon38;
                                        objProcessDb.ValorPagoVoluntario = _ValorPagoVoluntario;
                                        objProcessDb.ValorRenglon40 = _ValorRenglon40;
                                        objProcessDb.IdEstado = 2;
                                        objProcessDb.IdUsuario = 2;
                                        objProcessDb.TipoProceso = 1;   //--objImpustosOfic.tipo_proceso;

                                        int _IdRegistro = 0;
                                        if (objProcessDb.AddLoadLiquidacionOficina_New(ref _IdRegistro, ref _MsgError))
                                        {
                                            _Result = true;
                                            Console.WriteLine("\nFECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ", ID MUNICIPIO: " + _IdMunicipio + ", ID ESTABLECIMIENTO: " + _IdClienteEstablecimiento + ", REGISTRADO EN LA DB => " + _ContadorRow);
                                            //--
                                            FixedData.LogApi.Info(_MsgError);

                                            #region AQUI VALIDAMOS SI TERMINO EL PROCESO PARA ENVIAR EL CORREO DE NOTIFICACION
                                            //--
                                            if (_ContadorRow == _TotalOficinas)
                                            {
                                                #region AQUI ENVIAMO EL CORREO DE NOTIFICACION
                                                //--AQUI BORRAMOS LA TAREA PROGRAMADA
                                                DeleteTaskSchedulerManual(objImpustosOfic.nombre_tarea.ToString().Trim());
                                                //--
                                                ObjEmails.EmailPara = FixedData.EnvioEmail;
                                                ObjEmails.EmailCopia = FixedData.EmailCopiaProcesos;
                                                ObjEmails.Asunto = "REF.: PROCESO LIQUIDACION OFICINAS";

                                                string nHora = DateTime.Now.ToString("HH");
                                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                                StringBuilder strDetalleEmail = new StringBuilder();
                                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario el proceso de liquidación de oficinas ha terminado de forma exitosa con [" + _ContadorRow + " oficinas]" + "</h4>" +
                                                            "<br/><br/>" +
                                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                                string _MsgErrorEmail = "";
                                                if (!ObjEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                                {
                                                    FixedData.LogApi.Error(_MsgErrorEmail);
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                            _Result = false;
                                            //--
                                            ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                            ObjEmails.Asunto = "REF.: ERROR AL GUARDAR DATOS EN LA DB";

                                            string nHora = DateTime.Now.ToString("HH");
                                            string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                            StringBuilder strDetalleEmail = new StringBuilder();
                                            strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al guardar los datos en la db de la liquidación por oficinas. Motivo: " + _MsgError + "</h4>" +
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
                                    #endregion
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    _Result = false;
                                    //--
                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                    ObjEmails.Asunto = "REF.: ERROR AL PROCESAR LA LIQUIDACION PO OFICINA";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se encontro información de liquidación por oficinas" + "</h4>" +
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
                                _Result = false;
                                //--
                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE OFICINAS";

                                string nHora = DateTime.Now.ToString("HH");
                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                StringBuilder strDetalleEmail = new StringBuilder();
                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de las oficinas." + "</h4>" +
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
                            //--FIN DEL PROCESO
                        }
                    }
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        _Result = false;
                        //--
                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                        ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE ESTADOS FINANCIERO";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de ESTADOS FINANCIEROS CLIENTE." + "</h4>" +
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
                    _Result = false;
                    //--
                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                    ObjEmails.Asunto = "REF.: ERROR AL OBTENER DATOS DE ESTADOS FINANCIERO";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de ESTADOS FINANCIEROS CLIENTE." + "</h4>" +
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
                _Result = false;
                //--
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: ERROR PROCESO LIQUIDACION x OFICINAS";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al realizar el proceso de LIQUIDACION x OFICINAS con el ID MUNICIPIO [" + _IdMunicipio + "]. Motivo: " + ex.Message + "</h4>" +
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

            return _Result;
        }

        private static DataTable GetTablaDatosOficinas()
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
                FixedData.LogApi.Error("ERROR AL GENERAR EL DATA TABLE PARA LA LIQUIDACION POR OFICINAS. MOTIVO: " + ex.Message);
            }

            return DtLiquidacionIca;
        }
        #endregion

        private static bool DeleteTaskSchedulerManual(string _NombreTarea)
        {
            bool Result = false;
            string _MsgError = "";
            try
            {
                using (TaskService ts = new TaskService())
                {
                    //string _NombreTarea = "TAREA MANUAL [" + _NombreProveedor + "]";
                    ts.RootFolder.DeleteTask(_NombreTarea.ToString().Trim());

                    Result = true;
                    _MsgError = "LA TAREA PROGRAMADA [" + _NombreTarea + "] HA SIDO BORRADA DEL SISTEMA.";
                    FixedData.LogApi.Info(_MsgError);
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "ERROR AL BORRAR LA TAREA PROGRAMADA [" + _NombreTarea + "] DEL SISTEMA OPERATIVO. MOTIVO: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }

            return Result;
        }

    }
}
