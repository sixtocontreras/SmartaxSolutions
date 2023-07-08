using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Modulos;

namespace Smartax.Web.Application.Controles.Modulos.Consulta
{
    public partial class CtrlConsultaLiquidacionIca : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        LiquidarImpuestos ObjLiqImpuesto = new LiquidarImpuestos();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Estado ObjEstado = new Estado();
        Usuario ObjUser = new Usuario();
        Utilidades ObjUtils = new Utilidades();

        protected void LstTipoImpuesto()
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de clasificacion. Motivo: " + ex.ToString();
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

        protected void LstUsuarios()
        {
            try
            {
                ObjUser.TipoConsulta = 2;
                ObjUser.IdUsuario = null;
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdEstado = 1;
                ObjUser.MostrarSeleccione = "SI";
                ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbUsuarios.DataSource = ObjUser.GetUsuarios();
                this.CmbUsuarios.DataValueField = "id_usuario";
                this.CmbUsuarios.DataTextField = "nombre_usuario";
                this.CmbUsuarios.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los usuarios. Motivo: " + ex.ToString();
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

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeLeer)
            {
                this.RadGrid1.Visible = false;
            }
            if (!objPermiso.PuedeLiqBorrador)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 4].Visible = false;
            }
            if (!objPermiso.PuedeLiqDefinitivo)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;
            }
            if (!objPermiso.PuedeVerFormulario)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
            if (!objPermiso.PuedeAnular)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Consulta Liquidación Impuestos";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                this.LstTipoImpuesto();
                this.LstUsuarios();
                //--
                this.ViewState["DtConsultar"] = null;
                this.TxtAnioGravable.Text = DateTime.Now.ToString("yyyy");
                this.CmbTipoImpuesto.Focus();
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtConsultar"];
                if (dtDatos != null)
                {
                    this.RadGrid1.DataSource = dtDatos; //--this.FuenteDatos;
                    this.RadGrid1.DataMember = "DtLiquidacionForm";
                }
                //this.RadGrid1.DataSource = this.FuenteDatos;
                //this.RadGrid1.DataMember = "DtLiquidacionForm";
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de moneda. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnVerInfo")
                {
                    #region VER INFORMACION DEL BORRADOR DEL FORMULARIO DEL ICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidImpuesto = Int32.Parse(item.GetDataKeyValue("idliquid_impuesto").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _NombreMunicipio = item["nombre_municipio"].Text.ToString().Trim();
                    string _PeriodoImpuesto = item["periodicidad_impuesto"].Text.ToString().Trim();
                    string _AnioGravable = item["anio_gravable"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());

                    #region MOSTRAR EL FORM DETALLE DEL IMPUESTO
                    this.UpdatePanel1.Update();
                    //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Modulos/Consulta/FrmVerDetalleLiquidacion.aspx?IdLiquidImpuesto=" + _IdLiquidImpuesto +
                    "&IdFormImpuesto=" + _IdFormularioImpuesto + "&AnioGravable=" + _AnioGravable + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(500);
                    Ventana.Width = Unit.Pixel(900);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Ver Detalle Liquidación IdMunicipio: " + _IdMunicipio + ", Municipio: " + _NombreMunicipio + ", Periodicidad: " + _PeriodoImpuesto;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                    //--                       
                    #endregion
                }
                else if (e.CommandName == "BtnVerBorrador")
                {
                    #region VER INFORMACION DEL BORRADOR DEL FORMULARIO DEL ICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidImpuesto = Int32.Parse(item.GetDataKeyValue("idliquid_impuesto").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _PeriodoImpuesto = item["periodo_impuesto"].Text.ToString().Trim();
                    string _IdPeriodicidadImpuesto = item["idperiodicidad_impuesto"].Text.ToString().Trim();
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    string _AnioGravable = item["anio_gravable"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());
                    string _IdFirmante1 = item.GetDataKeyValue("idfirmante_1").ToString().Trim();
                    string _IdFirmante2 = item.GetDataKeyValue("idfirmante_2").ToString().Trim();

                    if (_IdEstado == 2)   //--2 BORRADOR, 3 DEFINITIVO
                    {
                        //--AQUI VALIDAMOS EL FORM IMPUESTO: 1. ICA, 2. AUTO ICA
                        if (_IdFormularioImpuesto == 1)
                        {
                            #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                            this.UpdatePanel1.Update();
                            //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _TipoProceso = "2";
                            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarBorradorIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                            "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                            "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                            "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;

                            //Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarBorradorIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                            //"&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                            //"&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(700);
                            Ventana.Width = Unit.Pixel(1260);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Borrador del Impuesto ICA";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;
                            #endregion
                        }
                        else
                        {
                            #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO AUTORETENCION DEL ICA
                            this.UpdatePanel1.Update();
                            //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _TipoProceso = "2";
                            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarBorradorAutoIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                            "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                            "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                            "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(700);
                            Ventana.Width = Unit.Pixel(1260);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Borrador del Impuesto AutoIca";
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
                        this.UpdatePanel1.Update();
                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, no se puede mostrar información de la Liquidación ya que esta se encuentra en un estado diferente a BORRADOR. Contacte a soporte técnico !";
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
                    #endregion
                }
                else if (e.CommandName == "BtnVerDefinitivo")
                {
                    #region VER INFORMACION DEL BORRADOR DEL FORMULARIO DEL ICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidImpuesto = Int32.Parse(item.GetDataKeyValue("idliquid_impuesto").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _PeriodoImpuesto = item["periodo_impuesto"].Text.ToString().Trim();
                    string _IdPeriodicidadImpuesto = item["idperiodicidad_impuesto"].Text.ToString().Trim();
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    string _AnioGravable = item["anio_gravable"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());
                    string _IdFirmante1 = item.GetDataKeyValue("idfirmante_1").ToString().Trim();
                    string _IdFirmante2 = item.GetDataKeyValue("idfirmante_2").ToString().Trim();

                    if (_IdEstado == 2 || _IdEstado == 3)   //--2 BORRADOR, 3 DEFINITIVO
                    {
                        #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                        this.UpdatePanel1.Update();
                        //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;
                        string _TipoProceso = "";
                        string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();

                        if (_IdEstado == 2)
                        {
                            #region AQUI MOSTRAMOS EL FORM DE LIQUIDACION DEFINITIVA
                            //--AQUI VALIDAMOS EL FORM IMPUESTO: 1. ICA, 2. AUTO ICA
                            if (_IdFormularioImpuesto == 1)
                            {
                                _TipoProceso = "2";
                                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarDefinitivoIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                                    "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                                    "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                                    "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                                Ventana.Title = "Liquidar Definitivo del Impuesto ICA";
                            }
                            else
                            {
                                _TipoProceso = "2";
                                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarDefinitivoAutoIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                                    "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                                    "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                                    "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                                Ventana.Title = "Liquidar Definitivo del Impuesto Auto Ica";
                            }
                            #endregion
                        }
                        else if (_IdEstado == 3)
                        {
                            #region AQUI MOSTRAMOS EL FORM PARA VER LA LIQUIDACION DEFINITIVA
                            //--AQUI VALIDAMOS EL FORM IMPUESTO: 1. ICA, 2. AUTO ICA
                            if (_IdFormularioImpuesto == 1)
                            {
                                _TipoProceso = "3";
                                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerLiquidarDefinitivoIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                                    "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                                    "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                                    "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                                Ventana.Title = "Información del Impuesto Definitivo del ICA";
                            }
                            else
                            {
                                _TipoProceso = "3";
                                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerLiquidarDefinitivoAutoIca.aspx?IdFormImpuesto=" + _IdFormularioImpuesto +
                                    "&PeriodoImpuesto=" + _PeriodoImpuesto + "&IdPeriodicidadImpuesto=" + _IdPeriodicidadImpuesto + "&IdMunicipio=" + _IdMunicipio +
                                    "&IdCliente=" + _IdCliente + "&CodigoDane=" + _CodigoDane + "&AnioGravable=" + _AnioGravable + "&IdFirmante1=" + _IdFirmante1 +
                                    "&IdFirmante2=" + _IdFirmante2 + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                                Ventana.Title = "Información del Impuesto Definitivo del Auto Ica";
                            }
                            #endregion
                        }
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(700);
                        Ventana.Width = Unit.Pixel(1260);
                        Ventana.KeepInScreenBounds = true;
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
                        this.UpdatePanel1.Update();
                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, no se puede mostrar información de la Liquidación ya que esta se encuentra en un estado diferente a BORRADOR. Contacte a soporte técnico !";
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
                    #endregion
                }
                else if (e.CommandName == "BtnVerFormulario")
                {
                    #region ANULAR LIQUIDACION DEL IMPUESTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidImpuesto = Int32.Parse(item.GetDataKeyValue("idliquid_impuesto").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    string _IdPeriodicidadImpuesto = item["idperiodicidad_impuesto"].Text.ToString().Trim();
                    //string _PeriodicidadImpuesto = item["periodicidad_impuesto"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());
                    string _EstadoLiquidacion = item["estado_liquidacion"].Text.ToString().Trim();

                    #region MOSTRAR EL IMPUESTO EN PDF Y PARA IMPRIMIR
                    this.UpdatePanel1.Update();
                    //--AQUI HABILITAMOS EL FORM DEL IMPUESTO
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _TipoImpuesto = "";
                    switch (_IdFormularioImpuesto)
                    {
                        case 1:
                            _TipoImpuesto = "ICA\\UNO_A_UNO";
                            break;
                        case 2:
                            _TipoImpuesto = "AUTORETENCION_ICA\\UNO_A_UNO";
                            break;
                        default:
                            break;
                    }

                    string _TipoLiquidacion = _EstadoLiquidacion.ToString().Equals("PRELIMINAR") ? "BORRADOR" : "DEFINITIVO";
                    string _TipoEjecucion = "UNO_A_UNO";
                    string _CodigoPeriodicidad = _IdPeriodicidadImpuesto.ToString().PadLeft(2, '0');
                    Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerImpuesto.aspx?IdMunicipio=" + _IdMunicipio + "&IdFormImpuesto=" + _IdFormularioImpuesto + "&CodigoDane=" + _CodigoDane +
                        "&CodigoPeriodicidad=" + _CodigoPeriodicidad + "&TipoImpuesto=" + _TipoImpuesto + "&TipoLiquidacion=" + _TipoLiquidacion + "&TipoEjecucion=" + _TipoEjecucion;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(670);
                    Ventana.Width = Unit.Pixel(1100);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Visualizar Impuesto";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                    //--
                    #endregion
                }
                else if (e.CommandName == "BtnAnularLiquidacion")
                {
                    #region ANULAR LIQUIDACION DEL IMPUESTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidImpuesto = Int32.Parse(item.GetDataKeyValue("idliquid_impuesto").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    int _AnioGravable = Int32.Parse(item["anio_gravable"].Text.ToString().Trim());
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());

                    #region DEFINIR VALORES A CAMPOS DEL OBJETO DE CLASE
                    //--AQUI VALIDAMOS EL ESTADO DE LA LIQUIDACION
                    if (_IdEstado != 5)
                    {
                        #region AQUI ENVIAMOS A REALIZAR LA ANULACION DE LA LIQUIDACION
                        ObjLiqImpuesto.IdLiquidImpuesto = _IdLiquidImpuesto;
                        ObjLiqImpuesto.IdFormularioImpuesto = _IdFormularioImpuesto;
                        ObjLiqImpuesto.IdCliente = _IdCliente;
                        ObjLiqImpuesto.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                        ObjLiqImpuesto.CodigoDane = _CodigoDane;
                        ObjLiqImpuesto.AnioGravable = _AnioGravable;
                        ObjLiqImpuesto.DescripcionSancionOtro = "";
                        ObjLiqImpuesto.DestinoAportesVol = "";
                        ObjLiqImpuesto.IdEstado = 5;    //--_IdEstado == 2 ? 5 : 2;
                        ObjLiqImpuesto.IdUsuario = this.Session["IdUsuario"].ToString().Trim();
                        ObjLiqImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjLiqImpuesto.TipoProceso = 3; //--ANULAR LIQUIDACION

                        string _MsgError = "";
                        if (ObjLiqImpuesto.AddUpLiquidacionImpuestoIca(ref _IdLiquidImpuesto, ref _MsgError))
                        {
                            #region MOSTRAR MENSAJE DE USUARIO
                            this.UpdatePanel1.Update();
                            //MOSTRAR LA VENTANA DEL POPUP
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
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(600);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Mensaje del Sistema";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;

                            //--ACTUALIZAR LISTA
                            DataTable dtDatos = new DataTable();
                            dtDatos = (DataTable)this.ViewState["DtConsultar"];
                            if (dtDatos != null)
                            {
                                DataRow[] dataRows = dtDatos.Select("idliquid_impuesto = " + _IdLiquidImpuesto);
                                if (dataRows.Length == 1)
                                {
                                    dataRows[0]["estado_liquidacion"] = "ANULADO";
                                    dataRows[0]["fecha_anula"] = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                                    dtDatos.Rows[0].AcceptChanges();
                                    dtDatos.Rows[0].EndEdit();
                                    this.RadGrid1.Rebind();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region MOSTRAR MENSAJE DE USUARIO
                            this.UpdatePanel1.Update();
                            //MOSTRAR LA VENTANA DEL POPUP
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
                        #endregion
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        this.UpdatePanel1.Update();
                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, no podrá realizar la ANULACIÓN de esta liquidación porque ya se encuentra anulada !";
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
                        _log.Error(_MsgMensaje.Trim());
                        #endregion
                    }
                    #endregion
                    //--
                    #endregion
                }
                else
                {
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje.Trim());
                #endregion
            }
        }

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                this.RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del tipo de moneda. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENER AÑO GRAVABLE POR TIPO DE IMPUESTO
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;
                this.UpdatePanel1.Update();

                int _IdTipoImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbTipoImpuesto.SelectedValue.ToString().Trim()) : -1;
                //--TIPO IMPUESTOS: 1. ICA, 2. AUTO ICA
                if (_IdTipoImpuesto == 1)
                {
                    this.TxtAnioGravable.Text = DateTime.Now.AddYears(-1).ToString("yyyy");
                }
                else if (_IdTipoImpuesto == 2)
                {
                    this.TxtAnioGravable.Text = DateTime.Now.ToString("yyyy");
                }
                else
                {
                    this.TxtAnioGravable.Text = DateTime.Now.ToString("yyyy");
                }

                //--
                this.TxtCodDane.Text = "";
                this.CmbUsuarios.SelectedValue = "";
                this.CmbUsuarios.Focus();
                #endregion
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Error con la selección del tipo de impuesto. Motivo: " + ex.ToString();
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

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            this.GetConsultarLiquidacion();
        }

        protected void TxtCodDane_TextChanged(object sender, EventArgs e)
        {
            this.GetConsultarLiquidacion();
        }

        protected void TxtAnioGravable_TextChanged(object sender, EventArgs e)
        {
            this.GetConsultarLiquidacion();
        }

        private void GetConsultarLiquidacion()
        {
            DataTable DtDatos = new DataTable();
            try
            {
                ObjLiqImpuesto.TipoConsulta = 2;
                ObjLiqImpuesto.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                ObjLiqImpuesto.IdLiquidImpuesto = null;
                ObjLiqImpuesto.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjLiqImpuesto.CodigoDane = this.TxtCodDane.Text.ToString().Trim().Length > 0 ? this.TxtCodDane.Text.ToString().Trim() : null;
                ObjLiqImpuesto.AnioGravable = this.TxtAnioGravable.Text.ToString().Trim().Length > 0 ? this.TxtAnioGravable.Text.ToString().Trim() : DateTime.Now.AddYears(-1).ToString("yyyy");
                ObjLiqImpuesto.PeriodicidadImpuesto = null;
                ObjLiqImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--AQUI OBTENEMOS LOS DATOS DE LA DB
                DtDatos = ObjLiqImpuesto.GetAllLiquidacionForm();
                //--
                if (DtDatos != null)
                {
                    if (DtDatos != null)
                    {
                        //--Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        this.ViewState["DtConsultar"] = DtDatos;
                        DtDatos.PrimaryKey = new DataColumn[] { DtDatos.Columns["idliquid_impuesto, idformulario_impuesto, id_municipio, id_cliente, idcliente_establecimiento, id_estado"] };

                        //--AQUI VALIDAMOS EL FORM IMPUESTO A CONSULTAR 1. ICA, 2. AUTO ICA
                        if (Int32.Parse(ObjLiqImpuesto.IdFormularioImpuesto.ToString().Trim()) == 1)
                        {
                            this.RadGrid1.Columns[RadGrid1.Columns.Count - 18].Visible = false;
                            this.RadGrid1.Columns[RadGrid1.Columns.Count - 19].Visible = false;
                        }

                        this.UpdatePanel1.Update();
                        this.RadGrid1.Rebind();
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        this.UpdatePanel1.Update();
                        this.ViewState["DtConsultar"] = DtDatos;
                        this.RadGrid1.Rebind();

                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, no se encontro información de Liquidación con los filtros ingresados. Por favor validar nuevamente !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
                    this.UpdatePanel1.Update();
                    //MOSTRAR LA VENTANA DEL POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor usuario, ocurrio un error al realizar la consulta. Intentelo nuevamente por favor !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error al realizar la consulta. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
    }
}