using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using Microsoft.Win32.TaskScheduler;
using System.Data;
using NativeExcel;
using System.Drawing;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.ProcessAPIs;
using Smartax.Web.Application.Clases.Parametros;
using static Smartax.Web.Application.Clases.ProcessAPIs.ModelApiSmartax;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Modulos;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmProcesoContabilizacion : System.Web.UI.Page
    {
        #region AQUI INSTANCIAMOS OBJETO DE CLASE
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        private static string FormatoMonto = "#,##0.00;(#,##0.00)";

        ProcessAPI objProceso = new ProcessAPI();
        ConfContabilizacion ObjContabilizacion = new ConfContabilizacion();
        EjecucionXLoteUser ObjEjecUser = new EjecucionXLoteUser();
        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        PeriodicidadPagos ObjPeriodicidad = new PeriodicidadPagos();
        LiquidarImpuestos ObjLiqImpuesto = new LiquidarImpuestos();
        Combox ObjCombox = new Combox();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        #endregion

        #region AQUI LLENAMOS LAS LISTAS PARA LOS FILTROS
        protected void LstAnioGravable()
        {
            try
            {
                ObjCombox.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = ObjCombox.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "numero_anio";
                this.CmbAnioGravable.DataBind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar el año gravable. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void LstTipoImpuestos()
        {
            try
            {
                ObjFrmImpuesto.TipoConsulta = 2;
                ObjFrmImpuesto.IdEstado = 1;
                ObjFrmImpuesto.MostrarSeleccione = "SI";
                ObjFrmImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjFrmImpuesto.GetFormularioImpuesto();
                this.CmbTipoImpuesto.DataValueField = "idformulario_impuesto";
                this.CmbTipoImpuesto.DataTextField = "descripcion_formulario";
                this.CmbTipoImpuesto.DataBind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de impuestos. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void LstPeriodicidad()
        {
            try
            {
                ObjPeriodicidad.TipoConsulta = 2;
                ObjPeriodicidad.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjPeriodicidad.IdEstado = 1;
                ObjPeriodicidad.MostrarSeleccione = "SI";
                ObjPeriodicidad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbPeriodicidad.DataSource = ObjPeriodicidad.GetPeriodicidadPagos();
                this.CmbPeriodicidad.DataValueField = "id_periodicidad";
                this.CmbPeriodicidad.DataTextField = "descripcion_periodicidad";
                this.CmbPeriodicidad.DataBind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar las periodicidades. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void LstMeses()
        {
            try
            {
                ObjCombox.MostrarSeleccione = "SI";
                this.CmbMeses.DataSource = ObjCombox.GetMeses();
                this.CmbMeses.DataValueField = "id_mes";
                this.CmbMeses.DataTextField = "numero_mes";
                this.CmbMeses.DataBind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los meses. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                this.ViewState["DtFiltros"] = null;
                this.ViewState["DataProcesar"] = "";

                //--AQUI LISTAMOS LOS COMBOBOX
                this.LstAnioGravable();
                this.LstTipoImpuestos();
                this.LstMeses();
                //--
                this.ViewState["DtContabilizacion"] = null;
                this.GetTablaDatos();
            }
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            string str = string.Format("VS_{0}_{1}", Request.UserHostAddress, DateTime.Now.Ticks);
            Cache.Add(str, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", str);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            string str = Request.Form["__VIEWSTATE_KEY"];
            if (!str.StartsWith("VS_"))
            {
                throw new Exception("Invalid ViewState");
            }
            return Cache[str];
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LstPeriodicidad();
        }

        protected void CmbMeses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObjLiqImpuesto.TipoConsulta = 1;
                ObjLiqImpuesto.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjLiqImpuesto.PeriodicidadImpuesto = this.CmbPeriodicidad.SelectedValue.ToString().Trim();
                ObjLiqImpuesto.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();
                ObjLiqImpuesto.MesImpuesto = this.CmbMeses.SelectedValue.ToString().Trim();
                ObjLiqImpuesto.IdEstado = 1;
                ObjLiqImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //--
                int _Cantidad = ObjLiqImpuesto.GetContadorImpuestos();
                this.LblCantidad1.Text = ObjUtils.GetFormatNumberSinSigno(_Cantidad.ToString());
                //--
                ObjLiqImpuesto.TipoConsulta = 2;
                ObjLiqImpuesto.IdEstado = 3;
                _Cantidad = ObjLiqImpuesto.GetContadorImpuestos();
                this.LblCantidad2.Text = ObjUtils.GetFormatNumberSinSigno(_Cantidad.ToString());
                this.BtnEjecutar.Enabled = true;
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al obtener el contador de impuestos. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void BtnEjecutar_Click_Old(object sender, EventArgs e)
        {
            try
            {
                if (this.ViewState["DataProcesar"].ToString().Trim().Length > 0)
                {
                    #region AQUI OBTENEMOS LO VALORES A ENVIAR AL SERIVICIO
                    LiquidarImpuesto_Req objData = new LiquidarImpuesto_Req();
                    //objData.estado_liquidacion = this.RbEstadoDeclaracion.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.RbEstadoDeclaracion.SelectedValue.ToString().Trim()) : -1;
                    //objData.idejecucion_lote = Int32.Parse(this.CmbTipoFiltro.SelectedValue.ToString().Trim());
                    objData.tipo_impuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbTipoImpuesto.SelectedValue.ToString().Trim()) : 0;
                    objData.data_procesar = this.ViewState["DataProcesar"].ToString().Trim();
                    //objData.emails_confirmar = this.TxtEmailsConfirmacion.Text.ToString().Trim().ToUpper();
                    objData.id_usuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    objData.nombre_usuario = this.Session["NombreCompletoUsuario"].ToString().Trim();
                    //--OBTENEMOS LOS DATOS DEL FIRMANTE 1
                    //firmante objFirmante = new firmante();
                    //objFirmante.id_firmante = Int32.Parse(this.ViewState["IdFirmante1"].ToString().Trim()) > 0 ? this.ViewState["IdFirmante1"].ToString().Trim() : null;
                    //objFirmante.nombre_firmante = this.ViewState["NombreFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["NombreFirmante1"].ToString().Trim() : "";
                    //objFirmante.tipo_documento = this.ViewState["TipoDocFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["TipoDocFirmante1"].ToString().Trim() : "";
                    //objFirmante.numero_documento = this.ViewState["DocFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["DocFirmante1"].ToString().Trim() : "";
                    //objFirmante.numero_tp = this.ViewState["TpFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["TpFirmante1"].ToString().Trim() : "";
                    //objFirmante.id_rol = this.ViewState["IdRolFirmante1"].ToString().Trim().Length > 0 ? Int32.Parse(this.ViewState["IdRolFirmante1"].ToString().Trim()) : 0;
                    //objFirmante.imagen_firma = (byte[])this.ViewState["ImagenFirma1"];
                    //objData.info_firmante1 = objFirmante;

                    //--OBTENEMOS LOS DATOS DEL FIRMANTE 2
                    //objFirmante = new firmante();
                    //objFirmante.id_firmante = Int32.Parse(this.ViewState["IdFirmante2"].ToString().Trim()) > 0 ? this.ViewState["IdFirmante2"].ToString().Trim() : null;
                    //objFirmante.nombre_firmante = this.ViewState["NombreFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["NombreFirmante2"].ToString().Trim() : "";
                    //objFirmante.tipo_documento = this.ViewState["TipoDocFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["TipoDocFirmante2"].ToString().Trim() : "";
                    //objFirmante.numero_documento = this.ViewState["DocFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["DocFirmante2"].ToString().Trim() : "";
                    //objFirmante.numero_tp = this.ViewState["TpFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["TpFirmante2"].ToString().Trim() : "";
                    //objFirmante.id_rol = this.ViewState["IdRolFirmante2"].ToString().Trim().Length > 0 ? Int32.Parse(this.ViewState["IdRolFirmante2"].ToString().Trim()) : 0;
                    //objFirmante.imagen_firma = (byte[])this.ViewState["ImagenFirma2"];
                    //objData.info_firmante2 = objFirmante;
                    #endregion

                    string _MsgError = "";
                    if (objProceso.GetProcesoLiquidacion(objData, ref _MsgError))
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        string[] _ArrayData = _MsgError.ToString().Trim().Split('|');
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _ArrayData[0].ToString().Trim() + " - " + _ArrayData[1].ToString().Trim();
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        string[] _ArrayData = _MsgError.ToString().Trim().Split('|');
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _ArrayData[0].ToString().Trim() + " - " + _ArrayData[1].ToString().Trim();
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _Mensaje = "Señor usuario, no hay item seleccionado para realizar el proceso por lote. Por favor validar los filtros !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar la Ejecución por Lote. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void BtnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjContabilizacion.TipoConsulta = 1;
                string _FormImpuesto = this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim();
                ObjContabilizacion.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjContabilizacion.IdPeriodicidadImpuesto = this.CmbPeriodicidad.SelectedValue.ToString().Trim();
                string _AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();
                ObjContabilizacion.AnioGravable = _AnioGravable;
                string _MesImpuesto = this.CmbMeses.SelectedItem.Text.ToString().Trim();
                ObjContabilizacion.MesImpuesto = this.CmbMeses.SelectedValue.ToString().Trim();
                ObjContabilizacion.IdEstado = 1;
                ObjContabilizacion.NumeroRenglon = null;
                ObjContabilizacion.CodigoCuenta = null;
                ObjContabilizacion.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //--
                DataTable dtRenglonesCont = new DataTable();
                dtRenglonesCont = ObjContabilizacion.GetRenglonesContabilizar();
                if (dtRenglonesCont != null)
                {
                    if (dtRenglonesCont.Rows.Count > 0)
                    {
                        #region AQUI RECORREMOS LOS RENGLONES A CONTABILIZAR
                        foreach (DataRow rowItem in dtRenglonesCont.Rows)
                        {
                            string _NaturalezaRenglon = rowItem["naturaleza_renglon"].ToString().Trim();
                            int _NumeroRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                            string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                            //--
                            #region AQUI OBTENEMOS DATOS DEL MUNICIPIO
                            ObjContabilizacion.TipoConsulta = 2;
                            ObjContabilizacion.IdEstado = 3;
                            ObjContabilizacion.Tipo = _NaturalezaRenglon;
                            ObjContabilizacion.NumeroRenglon = _NumeroRenglon;
                            ObjContabilizacion.CodigoCuenta = _CodigoCuenta;
                            //--
                            string _MsgError = "";
                            DataTable dtValoresCont = new DataTable();
                            dtValoresCont = ObjContabilizacion.GetValoresContabilizar(ref _MsgError);
                            //--
                            if (_MsgError.ToString().Trim().Length == 0)
                            {
                                if (dtValoresCont != null)
                                {
                                    if (dtValoresCont.Rows.Count > 0)
                                    {
                                        #region AQUI RECORREMOS LOS VALORES DEL DATATABLE
                                        //--
                                        DataTable DtContabilizacion = new DataTable();
                                        foreach (DataRow rowItem2 in dtValoresCont.Rows)
                                        {
                                            #region AQUI ARMAMOS EL DATATABLE QUE SE VA EXPORTAR A EXCEL
                                            //--
                                            double _ValorRenglon = Double.Parse(rowItem2["valor_renglon"].ToString().Trim());
                                            string _NumeroNit = rowItem2["numero_nit"].ToString().Trim();
                                            string _Sucursal = rowItem2["sucursal"].ToString().Trim();
                                            string _CodigoOficina = rowItem2["codigo_oficina"].ToString().Trim();
                                            string _FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                                            //--SI EL VALOR DEL RENGLON ES MAYOR A CERO SE CONTABILIZA DE LO CONTRARIO NO
                                            if (_ValorRenglon > 0)
                                            {
                                                DtContabilizacion = (DataTable)this.ViewState["DtContabilizacion"];
                                                if (DtContabilizacion != null)
                                                {
                                                    #region AQUI INGRESAMOS LOS DATOS AL DATATABLE
                                                    DataRow Fila = null;
                                                    Fila = DtContabilizacion.NewRow();
                                                    //--
                                                    Fila["num_linea"] = DtContabilizacion.Rows.Count + 1;
                                                    Fila["fecha"] = _FechaActual;
                                                    Fila["UniNeg"] = "DAVIV";
                                                    Fila["Contabilidad"] = "BIFRS";
                                                    Fila["Cuenta"] = _CodigoCuenta;
                                                    Fila["Tipo CCV"] = "";
                                                    Fila["Moneda"] = "COP";
                                                    //--AQUI VALIDAMOS LA NATURALEZA DEL RENGLON
                                                    if (_NaturalezaRenglon.Equals("D"))
                                                    {
                                                        Fila["Importe"] = _ValorRenglon;
                                                    }
                                                    else
                                                    {
                                                        Fila["Importe"] = "-" + _ValorRenglon;
                                                    }
                                                    Fila["Cls Cambio"] = "";
                                                    Fila["Cotizacion"] = "";
                                                    Fila["Importe Base"] = "";
                                                    Fila["Referencia"] = "";
                                                    Fila["Clv Item Abierto"] = _NumeroNit;
                                                    Fila["Descripcion"] = "CONTABILIZACION " + _FormImpuesto + " RENGLON " + _NumeroRenglon + " " + _MesImpuesto + " " + _AnioGravable;
                                                    Fila["OFICINA"] = "N/A";
                                                    Fila["SUCURSAL"] = _Sucursal;
                                                    Fila["DEPARTAMENTO"] = _CodigoOficina;
                                                    Fila["PROYECTOS"] = "GR001";
                                                    Fila["SUBPROYECTO"] = "";
                                                    Fila["Ref Ppto"] = "";
                                                    Fila["Sucursales&Agencias"] = "";
                                                    Fila["Cuenta Alt"] = "";
                                                    Fila["Producto"] = "";
                                                    Fila["Evento Entrd"] = "";
                                                    Fila["Valor Estdco"] = "";
                                                    Fila["Cd Estdco"] = "";
                                                    Fila["Proyecto"] = "";
                                                    Fila["Cd Libro"] = "";
                                                    Fila["CC1"] = "";
                                                    Fila["CC2"] = "";
                                                    Fila["CC3"] = "";
                                                    Fila["Filial"] = "";
                                                    Fila["Filial Fnc"] = "";
                                                    Fila["Escenario"] = "";
                                                    Fila["UN Proy"] = "";
                                                    Fila["Actividad"] = "";
                                                    Fila["Tp Recurso"] = "";
                                                    Fila["Categoria"] = "";
                                                    Fila["Subcateg"] = "";
                                                    Fila["Analisis"] = "";
                                                    Fila["Grupo IU"] = "";
                                                    Fila["IU Fija"] = "";
                                                    DtContabilizacion.Rows.Add(Fila);
                                                    //--AQUI ASIGNAMOS EL DATATABLE
                                                    this.ViewState["DtContabilizacion"] = DtContabilizacion;
                                                    #endregion
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region MOSTRAR MENSAJE DE USUARIO
                                //Mostramos el mensaje porque se produjo un error con la Trx.
                                this.RadWindowManager1.ReloadOnShow = true;
                                this.RadWindowManager1.DestroyOnClose = true;
                                this.RadWindowManager1.Windows.Clear();
                                this.RadWindowManager1.Enabled = true;
                                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                this.RadWindowManager1.Visible = true;

                                RadWindow Ventana = new RadWindow();
                                Ventana.Modal = true;
                                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                                Ventana.VisibleOnPageLoad = true;
                                Ventana.Visible = true;
                                Ventana.Height = Unit.Pixel(330);
                                Ventana.Width = Unit.Pixel(600);
                                Ventana.KeepInScreenBounds = true;
                                Ventana.Title = "Mensaje del Sistema";
                                Ventana.VisibleStatusbar = false;
                                Ventana.Behaviors = WindowBehaviors.Close;
                                this.RadWindowManager1.Windows.Add(Ventana);
                                this.RadWindowManager1 = null;
                                Ventana = null;
                                return;
                                #endregion
                            }
                            #endregion
                        }

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjContabilizacion);

                        //--
                        #region AQUI EXPORTAMOS LA INFORMACION A EXCEL
                        DataTable DtExportar = (DataTable)this.ViewState["DtContabilizacion"];
                        if (DtExportar != null)
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.ModuloApp = "PROCESO_CONTABILIZACION";
                            ObjAuditoria.IdTipoEvento = 5;
                            ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            ObjAuditoria.DescripcionEvento = jsonRequest;
                            ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                            ObjAuditoria.TipoProceso = 1;

                            //'Agregar Auditoria del sistema
                            string _MsgErrorLogs = "";
                            if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                            {
                                _log.Error(_MsgErrorLogs);
                            }
                            #endregion

                            ExportarDatosExcel(DtExportar);
                        }
                        #endregion
                        //--
                        #endregion
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario. no se encontro información de renglones para contabilizar !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(330);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgMensaje = "Señor usuario. Ocurrio un Error al obtener los renglones para contabilizar !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(330);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar el proceso de contabilización. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        private DataTable GetTablaDatos()
        {
            DataTable dtDatos = new DataTable();
            try
            {
                DataTable DtContabilizacion = new DataTable();
                DtContabilizacion = (DataTable)this.ViewState["DtContabilizacion"];
                if (DtContabilizacion != null)
                {
                    dtDatos = DtContabilizacion.Copy();
                }
                else
                {
                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtContabilizacion = new DataTable();
                    DtContabilizacion.TableName = "DtContabilizacion";
                    DtContabilizacion.Columns.Add("fecha");
                    DtContabilizacion.Columns.Add("num_linea", typeof(Int32));
                    DtContabilizacion.PrimaryKey = new DataColumn[] { DtContabilizacion.Columns["num_linea"] };
                    DtContabilizacion.Columns.Add("UniNeg");
                    DtContabilizacion.Columns.Add("Contabilidad");
                    DtContabilizacion.Columns.Add("Cuenta");
                    DtContabilizacion.Columns.Add("Tipo CCV");
                    DtContabilizacion.Columns.Add("Moneda");
                    DtContabilizacion.Columns.Add("Importe");
                    DtContabilizacion.Columns.Add("Cls Cambio");
                    DtContabilizacion.Columns.Add("Cotizacion");
                    DtContabilizacion.Columns.Add("Importe Base");
                    DtContabilizacion.Columns.Add("Referencia");
                    DtContabilizacion.Columns.Add("Clv Item Abierto");
                    DtContabilizacion.Columns.Add("Descripcion");
                    DtContabilizacion.Columns.Add("OFICINA");
                    DtContabilizacion.Columns.Add("SUCURSAL");
                    DtContabilizacion.Columns.Add("DEPARTAMENTO");
                    DtContabilizacion.Columns.Add("PROYECTOS");
                    DtContabilizacion.Columns.Add("SUBPROYECTO");
                    DtContabilizacion.Columns.Add("Ref Ppto");
                    DtContabilizacion.Columns.Add("Sucursales&Agencias");
                    DtContabilizacion.Columns.Add("Cuenta Alt");
                    DtContabilizacion.Columns.Add("Producto");
                    DtContabilizacion.Columns.Add("Evento Entrd");
                    DtContabilizacion.Columns.Add("Valor Estdco");
                    DtContabilizacion.Columns.Add("Cd Estdco");
                    DtContabilizacion.Columns.Add("Proyecto");
                    DtContabilizacion.Columns.Add("Cd Libro");
                    DtContabilizacion.Columns.Add("CC1");
                    DtContabilizacion.Columns.Add("CC2");
                    DtContabilizacion.Columns.Add("CC3");
                    DtContabilizacion.Columns.Add("Filial");
                    DtContabilizacion.Columns.Add("Filial Fnc");
                    DtContabilizacion.Columns.Add("Escenario");
                    DtContabilizacion.Columns.Add("UN Proy");
                    DtContabilizacion.Columns.Add("Actividad");
                    DtContabilizacion.Columns.Add("Tp Recurso");
                    DtContabilizacion.Columns.Add("Categoria");
                    DtContabilizacion.Columns.Add("Subcateg");
                    DtContabilizacion.Columns.Add("Analisis");
                    DtContabilizacion.Columns.Add("Grupo IU");
                    DtContabilizacion.Columns.Add("IU Fija");
                    dtDatos = DtContabilizacion.Copy();
                    #endregion
                }

                this.ViewState["DtContabilizacion"] = dtDatos;
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                dtDatos = null;
                this.ViewState["DtContabilizacion"] = dtDatos;

                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Error al generar Tabla de Contabilización. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }

            return dtDatos;
        }

        //Metodo que permite exportar la informacion a excel.
        protected void ExportarDatosExcel(DataTable DtDatosExportar)
        {
            try
            {
                if (DtDatosExportar.Rows.Count > 0)
                {
                    //Aqui se comienza a escribir los datos en el archivo de excel que sera enviado por correo
                    DateTime FechaActual = DateTime.Now;
                    string _MesActual = Convert.ToString(FechaActual.ToString("MMMM"));
                    string _DiaActual = Convert.ToString(FechaActual.ToString("dd"));
                    //--AQUI DEFINIMOS EL NOMBRE DEL ARCHIVO
                    string cNombreFileExcel = "RptContabilizacion_" + _DiaActual + "_" + _MesActual + ".xlsx";

                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
                    string strNombreEncabezadoReporte = "REPORTE DE CONTABILIZACIÓN " + this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim();
                    sheet.Range[2, 2, 2, CantidadCol].Value = strNombreEncabezadoReporte;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Size = 18;
                    sheet.Range[2, 2, 2, CantidadCol].ColumnWidth = 30;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Bold = true;
                    sheet.Range[2, 2, 2, CantidadCol].Interior.Color = Color.LightGray;
                    sheet.Range[2, 2, 2, CantidadCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.LineStyle = XlLineStyle.xlContinuous;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.Weight = XlBorderWeight.xlMedium;

                    for (int ncol = 0; ncol < DtDatosExportar.Columns.Count; ncol++)
                    {
                        //AQUI OBTENEMOS LOS NOMBRES DE LAS COLUMNAS DEL DATATABLE
                        string strNombreColum = DtDatosExportar.Columns[ncol].ColumnName.ToString().Trim().ToUpper();
                        sheet.Range[ContadorRow, ContadorCol].Value = strNombreColum;
                        sheet.Range[ContadorRow, ContadorCol].Font.Bold = true;
                        sheet.Range[ContadorRow, ContadorCol].Font.Size = 12;
                        sheet.Range[ContadorRow, ContadorCol].ColumnWidth = 10;
                        sheet.Range[ContadorRow, ContadorCol].Interior.Color = Color.LightGray;
                        sheet.Range[ContadorRow, ContadorCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        sheet.Range[ContadorRow, ContadorCol].Borders.LineStyle = XlLineStyle.xlDash;
                        sheet.Range[ContadorRow, ContadorCol].Borders.Weight = XlBorderWeight.xlMedium;

                        Row = 4;
                        for (int nrow = 0; nrow < DtDatosExportar.Rows.Count; nrow++)
                        {
                            //AQUI OBTENEMOS CADA UNO DE LOS DATOS DEL DATATABLE
                            if (ncol == 7)
                            {
                                sheet.Cells[Row, ContadorCol].NumberFormat = FormatoMonto;
                                sheet.Cells[Row, ContadorCol].Value = Convert.ToDouble(DtDatosExportar.Rows[nrow][ncol].ToString().Trim());
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }
                            else
                            {
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }

                            Row++;
                            Contador++;
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = "CONTABILIZACION";
                        book.Worksheets["CONTABILIZACION"].UsedRange.Autofit();
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + cNombreFileExcel.ToString().Trim());
                        book.SaveAs(Response.OutputStream, XlFileFormat.xlOpenXMLWorkbook);
                        //book.SaveAs(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        this.LblMensaje.Text = "No hay información para mostrar en el reporte de excel.";
                        this.LblMensaje.ForeColor = Color.Red;
                        //UpdatePanelMsg.Update();
                    }
                }
                else
                {
                    this.LblMensaje.Text = "No hay información para exportar a Excel.";
                    this.LblMensaje.ForeColor = Color.Red;
                    //UpdatePanelMsg.Update();
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Black;
                this.LblMensaje.Text = "";
                //UpdatePanelMsg.Update();
            }
        }

    }
}