using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Data;
using System.Text;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros;
using Newtonsoft.Json;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmValidarLiquidacionesPorLote : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        const string quote = "\"";

        EjecucionXLoteUser ObjEjecUser = new EjecucionXLoteUser();
        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();
        Estado ObjEstado = new Estado();
        Combox ObjCombox = new Combox();
        Utilidades ObjUtils = new Utilidades();
        EnvioCorreo ObjCorreo = new EnvioCorreo();

        #region AQUI LISTAMOS LOS COMBOBOX
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

        protected void LstTipoFiltros()
        {
            try
            {
                ObjEjecUser.TipoConsulta = 2;
                ObjEjecUser.IdEjecucionLote = null;
                int _IdRol = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                if (_IdRol == 1 || _IdRol == 2)
                {
                    ObjEjecUser.IdUsuario = null;
                }
                else
                {
                    ObjEjecUser.IdUsuario = this.Session["IdUsuario"] != null ? this.Session["IdUsuario"].ToString().Trim() : null;
                }
                ObjEjecUser.IdEstado = 1;
                ObjEjecUser.MostrarSeleccione = "SI";
                ObjEjecUser.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbTipoFiltro.DataSource = ObjEjecUser.GetEjecucionXLoteUser();
                this.CmbTipoFiltro.DataValueField = "idejecucion_lote";
                this.CmbTipoFiltro.DataTextField = "nombre_proceso";
                this.CmbTipoFiltro.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de filtros. Motivo: " + ex.ToString();
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

        protected void LstTipoImpuestosFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 2;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                ObjEjecFiltros.IdFormImpuesto = null;
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjEjecFiltros.GetEjecucionXLoteFiltroTipoImp();
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

        protected void LstEstadoImpuesto()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.TipoEstado = "LIQ_IMPUESTO";
                ObjEstado.MostrarSeleccione = "SI";
                ObjEstado.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbEstadoLiquidacion.DataSource = ObjEstado.GetEstados();
                this.CmbEstadoLiquidacion.DataValueField = "id_estado";
                this.CmbEstadoLiquidacion.DataTextField = "codigo_estado";
                this.CmbEstadoLiquidacion.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los estados. Motivo: " + ex.ToString();
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

        protected void LstDptosFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 3;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroDpto();
                this.CmbDepartamento.DataSource = dtDatos;
                this.CmbDepartamento.DataValueField = "id_dpto";
                this.CmbDepartamento.DataTextField = "nombre_dpto";
                this.CmbDepartamento.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los departamentos. Motivo: " + ex.ToString();
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

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

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
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                //this.AplicarPermisos();
                this.ViewState["DtFiltros"] = null;
                this.ViewState["DataProcesar"] = "";
                this.GetTablaDatos();

                //--Llenar los combox
                this.LstAnioGravable();
                this.LstTipoFiltros();
                this.LstEstadoImpuesto();
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION DE EVENTOS DEL PAGE
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
        #endregion

        #region DEFINICION DE EVENTOS CONTROLES DEL FORM
        protected void CmbAnioGravable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbEstadoLiquidacion.Enabled = true;
                this.CmbTipoFiltro.Enabled = true;
                this.CmbTipoFiltro.SelectedValue = "";
                this.CmbTipoFiltro.Focus();
            }
            else
            {
                this.CmbEstadoLiquidacion.Enabled = false;
                this.CmbTipoFiltro.Enabled = false;
                this.CmbTipoFiltro.SelectedValue = "";
            }
        }

        protected void CmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            if (this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbTipoImpuesto.Enabled = true;
                this.CmbTipoImpuesto.SelectedValue = "";
                this.CmbDepartamento.SelectedValue = "";
                //--
                this.LstTipoImpuestosFiltro();
                this.CmbTipoImpuesto.Focus();
            }
            else
            {
                this.CmbTipoImpuesto.Enabled = false;
                this.CmbTipoImpuesto.SelectedValue = "";
                this.CmbDepartamento.Enabled = false;
                this.CmbDepartamento.SelectedValue = "";
            }
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            if (this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbDepartamento.Enabled = true;
                this.CmbDepartamento.SelectedValue = "";
                //--
                this.LstDptosFiltro();
                this.CmbDepartamento.Focus();
            }
            else
            {
                this.CmbDepartamento.Enabled = false;
                this.CmbDepartamento.SelectedValue = "";
            }
        }
        #endregion

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtFiltros"];
                //--
                if (dtDatos != null)
                {
                    this.RadGrid1.DataSource = dtDatos; //--this.FuenteDatos;
                    this.RadGrid1.DataMember = "DtLiquidacionXLote";
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
                if (e.CommandName == "BtnAprobar")
                {
                    #region VER INFORMACION DEL BORRADOR DEL FORMULARIO DEL ICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidacionLote = Int32.Parse(item.GetDataKeyValue("idliquidacion_lote").ToString().Trim());
                    int _AnioGravable = Int32.Parse(item.GetDataKeyValue("anio_gravable").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdUsuario = Int32.Parse(item.GetDataKeyValue("id_usuario").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    int _IdEstadoLiquidacion = Int32.Parse(item.GetDataKeyValue("idestado_liquidacion").ToString().Trim());
                    string _PeriodicidadImpuesto = item["periodicidad_impuesto"].Text.ToString().Trim();
                    string _TipoImpuesto = item["tipo_impuesto"].Text.ToString().Trim();
                    string _NombreDpto = item["nombre_dpto"].Text.ToString().Trim();
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    string _NombreMunicipio = item["nombre_municipio"].Text.ToString().Trim();
                    string _EstadoLiquidacion = item["estado_liquidacion"].Text.ToString().Trim();
                    string _RevisionJefe = item["revision_jefe"].Text.ToString().Trim();
                    string _AprobacionJefe = item["aprobacion_jefe"].Text.ToString().Trim();
                    string _NombreAnalista = item["nombre_analista"].Text.ToString().Trim();
                    string _EmailAnalista = item["email_analista"].Text.ToString().Trim();
                    string _FechaLimite = item["fecha_limite"].Text.ToString().Trim();

                    if (_RevisionJefe.Trim().Equals("NO") && (_AprobacionJefe.Trim().Equals("PENDIENTE") ||
                        _AprobacionJefe.Trim().Equals("NO")))
                    {
                        #region MOSTRAR EL FORM DE APROBAR O RECAHZAR
                        this.UpdatePanel1.Update();
                        //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        //--string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmAprobarLiquidacionPorLote.aspx?IdLiquidacionLote=" + _IdLiquidacionLote +
                        "&AnioGravable=" + _AnioGravable + "&EstadoLiquidacion=" + _EstadoLiquidacion + "&TipoImpuesto=" + _TipoImpuesto +
                        "&Periodicidad=" + _PeriodicidadImpuesto + "&NombreDpto=" + _NombreDpto + "&CodigoDane=" + _CodigoDane + "&NombreMunicipio=" + _NombreMunicipio +
                        "&NombreAnalista=" + _NombreAnalista + "&EmailAnalista=" + _EmailAnalista + "&FechaLimite=" + _FechaLimite;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(550);
                        Ventana.Width = Unit.Pixel(1100);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Aprobar o Rechazar la Liquidación Id: " + _IdLiquidacionLote + ", Tipo IMpuesto: " + _TipoImpuesto;
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
                        string _MsgMensaje = "Señor usuario, no se puede realizar la aprobación o rechazo de la liquidación porque esta ya se encuentra APROBADA por el Jefe !";
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
                else if (e.CommandName == "BtnActualizarLista")
                {
                    #region ACTUALIAZR LISTA DE IMPUESTOS
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    //--Actualizar la lista de productos.
                    this.GetConsultarLiquidacionesXLote();
                    this.RadGrid1.Rebind();
                    #endregion
                }
                else if (e.CommandName == "BtnVerFormulario")
                {
                    #region VISUALIZAR EL FORM DEL IMPUESTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidacionLote = Int32.Parse(item.GetDataKeyValue("idliquidacion_lote").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    int _PeriodoImpuesto = Int32.Parse(item.GetDataKeyValue("periodo_impuesto").ToString().Trim());
                    int _CodigoPeriodicidad = Int32.Parse(item.GetDataKeyValue("codigo_periodicidad").ToString().Trim());
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("idestado_liquidacion").ToString().Trim());
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
                            _TipoImpuesto = "ICA\\POR_LOTE";
                            break;
                        case 2:
                            _TipoImpuesto = "AUTORETENCION_ICA\\POR_LOTE";
                            break;
                        default:
                            break;
                    }

                    string _TipoLiquidacion = _EstadoLiquidacion.ToString().Equals("PRELIMINAR") ? "BORRADOR" : "DEFINITIVO";
                    string _TipoEjecucion = "POR_LOTE";
                    Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerImpuesto.aspx?IdMunicipio=" + _IdMunicipio + "&IdFormImpuesto=" + _IdFormularioImpuesto + "&CodigoDane=" + _CodigoDane +
                        "&CodigoPeriodicidad=" + _CodigoPeriodicidad.ToString().PadLeft(2, '0') + "&TipoImpuesto=" + _TipoImpuesto + "&TipoLiquidacion=" + _TipoLiquidacion + "&TipoEjecucion=" + _TipoEjecucion;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(670);
                    Ventana.Width = Unit.Pixel(1100);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Visualizar Liquidación Impuesto";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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
                //Mostramos el mensaje porque se produjo un error con la Trx.
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
                #endregion
            }
        }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _JsonResponse = "";
            foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
            {
                if (item.Selected == true)
                {
                    string _DataItem = item.KeyValues.ToString().Trim();
                    if (_JsonResponse.ToString().Trim().Length > 0)
                    {
                        _JsonResponse = _JsonResponse + "," + _DataItem;
                    }
                    else
                    {
                        _JsonResponse = _DataItem;
                    }
                }
            }

            //--OBTENER LA DATA A PROCESAR
            this.ViewState["DataProcesar"] = @"[" + _JsonResponse + @"]";
            _log.Warn("DATA VALIDACION LIQUIDACION A NOTIFICAR => " + this.ViewState["DataProcesar"].ToString().Trim());
        }
        #endregion

        protected void BtnConsultar_Click(object sender, ImageClickEventArgs e)
        {
            this.GetConsultarLiquidacionesXLote();
        }

        private void GetConsultarLiquidacionesXLote()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 1;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdEstado = this.CmbEstadoLiquidacion.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEstadoLiquidacion.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                DataTable dtDatos = new DataTable();
                string _MsgError = "";
                dtDatos = ObjEjecFiltros.GetValidarLiquidacionXLote(ref _MsgError);
                //--
                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        #region AQUI OBTENEMOS LOS DATOS DE LA DB
                        //--Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        this.ViewState["DtFiltros"] = dtDatos;
                        dtDatos.PrimaryKey = new DataColumn[] { dtDatos.Columns["idliquidacion_lote, id_cliente, idformulario_impuesto, anio_gravable, id_dpto, nombre_dpto, codigo_dane, id_municipio, nombre_municipio, idcliente_establecimiento, periodicidad_impuesto, fecha_limite"] };
                        this.UpdatePanel1.Update();
                        this.PnlRespuesta.Visible = true;
                        this.RadGrid1.Rebind();
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
                        string _MsgMensaje = "Señor usuario, no se encontro información de liquidaciones por lote. Por favor valide los filtros y vuelva intentarlo !";
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
                    string _MsgMensaje = _MsgError.ToString().Trim().Length > 0 ? _MsgError.ToString().Trim() : "Señor usuario. Ocurrio un Error al listar los municipios. Por favor validar con soporte técnico !";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los municipios. Motivo: " + ex.ToString();
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

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ViewState["DataProcesar"].ToString().Trim().Length > 0)
                {
                    DataTable dtDatos = JsonConvert.DeserializeObject<DataTable>(this.ViewState["DataProcesar"].ToString().Trim());
                    if (dtDatos != null)
                    {
                        if (dtDatos.Rows.Count > 0)
                        {
                            #region AQUI REALIZAMOS EL PROCESO CON LA DB
                            //--
                            #region AQUI RECORREMOS EL DATATABLE
                            object _IdEjecucionLote = null;
                            string _ArrayDataLiq = "";
                            //--AQUI OBTENEMOS EL DATATABLE A MOSTRAR EN EL CORREO
                            DataTable dtDatosMostrar = new DataTable();
                            dtDatosMostrar = this.GetTablaDatos();
                            foreach (DataRow rowItem in dtDatos.Rows)
                            {
                                #region OBTENER DATOS DEL DATATABLE
                                int _IdLiquidacionLote = Int32.Parse(rowItem["idliquidacion_lote"].ToString().Trim());
                                int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                int _IdUsuario = Int32.Parse(rowItem["id_usuario"].ToString().Trim());
                                int _IdFormularioImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                string _TipoImpuesto = rowItem["tipo_impuesto"].ToString().Trim();
                                int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                int _IdDepartamento = Int32.Parse(rowItem["id_dpto"].ToString().Trim());
                                string _NombreDpto = rowItem["nombre_dpto"].ToString().Trim();
                                string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                string _NombreMunicipio = rowItem["nombre_municipio"].ToString().Trim();
                                int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                //int _PeriodoImpuesto = Int32.Parse(rowItem["periodo_impuesto"].ToString().Trim());
                                //int _CodigoPeriodicidad = Int32.Parse(rowItem["codigo_periodicidad"].ToString().Trim());
                                int _IdPeriodicidadImpuesto = Int32.Parse(rowItem["idperiodicidad_impuesto"].ToString().Trim());
                                string _Periodicidad = rowItem["periodicidad_impuesto"].ToString().Trim();
                                int _IdEstadoLiquidacion = Int32.Parse(rowItem["idestado_liquidacion"].ToString().Trim());
                                string _FechaVencimiento = DateTime.Parse(rowItem["fecha_limite"].ToString().Trim()).ToString("yyyy-MM-dd");
                                string _NombreAnalista = rowItem["nombre_analista"].ToString().Trim();
                                //--
                                DataRow Fila = null;
                                Fila = dtDatosMostrar.NewRow();
                                Fila["codigo_dane"] = _CodigoDane;
                                Fila["departamento"] = _NombreDpto;
                                Fila["municipio"] = _NombreMunicipio;
                                Fila["periodicidad"] = _Periodicidad;
                                Fila["fecha_vencimiento"] = _FechaVencimiento;
                                Fila["nombre_usuario"] = _NombreAnalista;
                                dtDatosMostrar.Rows.Add(Fila);

                                //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                if (_ArrayDataLiq.ToString().Trim().Length > 0)
                                {
                                    _ArrayDataLiq = _ArrayDataLiq.ToString().Trim() + "," + quote + "(" + _IdLiquidacionLote + "," + _IdCliente + "," + _IdUsuario + "," + _AnioGravable + "," + _IdEjecucionLote + "," + _IdFormularioImpuesto + "," + _IdDepartamento + "," + _CodigoDane + "," + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _IdEstadoLiquidacion + "," + _IdPeriodicidadImpuesto + "," + _Periodicidad + "," + _FechaVencimiento + ")" + quote;
                                }
                                else
                                {
                                    _ArrayDataLiq = quote + "(" + _IdLiquidacionLote + "," + _IdCliente + "," + _IdUsuario + "," + _AnioGravable + "," + _IdEjecucionLote + "," + _IdFormularioImpuesto + "," + _IdDepartamento + "," + _CodigoDane + "," + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _IdEstadoLiquidacion + "," + _IdPeriodicidadImpuesto + "," + _Periodicidad + "," + _FechaVencimiento + ")" + quote;
                                    //_ArrayDataLiq = quote + "(" + _IdLiquidacionLote + "," + _IdCliente + "," + _IdUsuario + "," + _AnioGravable + "," + _IdEjecucionLote + "," + _IdFormularioImpuesto + "," + _IdDepartamento + "," + _CodigoDane + "," + _IdMunicipio + "," + _IdClienteEstablecimiento + "," + _IdEstadoLiquidacion + "," + _PeriodoImpuesto + "," + _CodigoPeriodicidad + "," + _Periodicidad + "," + _FechaVencimiento + ")" + quote;
                                }
                                #endregion
                            }
                            #endregion

                            ObjEjecFiltros.IdEjecucionLote = null;
                            ObjEjecFiltros.ArrayData = _ArrayDataLiq.ToString().Trim();
                            ObjEjecFiltros.AprobacionJefe = this.RbEstadoAprobacion.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                            ObjEjecFiltros.Observacion = ObjUtils.GetLimpiarCadena(this.TxtObservacionValidacion.Text.ToString().Trim().ToUpper());
                            ObjEjecFiltros.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                            ObjEjecFiltros.TipoProceso = 3;
                            ObjEjecFiltros.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                            int _IdRegistro = 0;
                            string _MsgError = "";
                            if (ObjEjecFiltros.GetProcesarLiquidacionLote(ref _IdRegistro, ref _MsgError))
                            {
                                #region OBTENER DATOS DEL DATATABLE Y ENVIO DE CORREO
                                //--
                                #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                                //--Definir valores para envio del email
                                ObjCorreo.StrServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                                ObjCorreo.PuertoCorreo = FixedData.PuertoCorreoGmail;
                                ObjCorreo.StrEmailDe = FixedData.UsuarioEmail.ToString().Trim();
                                ObjCorreo.StrPasswordDe = FixedData.PasswordEmail.ToString().Trim();
                                string[] _ListaEmails = this.TxtEmailsNotificacion.Text.ToString().Trim().Split(';');
                                ObjCorreo.StrEmailPara = _ListaEmails[0].ToString().Trim();
                                ObjCorreo.StrEmailCopia = this.TxtEmailsNotificacion.Text.ToString().Trim();
                                ObjCorreo.StrAsunto = "REF.: VALIDACION LIQUIDACION x LOTE";

                                string nHora = DateTime.Now.ToString("HH");
                                string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                                StringBuilder strDetalleEmail = new StringBuilder();
                                string _TituloTablaHtml = "LISTA DE LIQUIDACIONES DE ICA PROCESADAS";
                                string _TableHtml = GetTableHtml(_TituloTablaHtml, dtDatosMostrar);

                                //strDetalleEmail.Append("<h4>" + strTime + ", Señor " + this.LblAnalista.Text.ToString().Trim() + ", el proceso de revisión a la ejecución masiva de las declaraciones del tipo de impuesto " + this.LblTipoImpuesto.Text.ToString().Trim() + " ha finalizado para los siguientes municipios." + "</h4>" + "<br/>");
                                if (ObjEjecFiltros.AprobacionJefe.Equals("S"))
                                {
                                    strDetalleEmail.Append("<h4>" + strTime + ", Señor analista, el proceso de validación de liquidación de impuestos ha finalizado para los siguientes municipios." + "</h4>" + "<br/>");
                                }
                                else
                                {
                                    strDetalleEmail.Append("<h4>" + strTime + ", Señor analista, para que por favor se haga una revisión de liquidación de impuestos para los siguientes municipios." + "</h4>" + "<br/>");
                                }

                                strDetalleEmail.Append("<h4>" + "Observación: " + this.TxtObservacionValidacion.Text.ToString().Trim() + "</h4>" + "<br/><br/>");
                                strDetalleEmail.Append(_TableHtml.ToString() + "<br/><br/><br/>");

                                ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                                string _MsgErrorEmail = "";
                                if (!ObjCorreo.SendEmailConCopia(ref _MsgErrorEmail))
                                {
                                    _MsgError = _MsgError + " Pero " + _MsgErrorEmail.ToString().Trim();
                                }
                                #endregion
                                //--
                                #endregion

                                #region MOSTRAR MENSAJE DE USUARIO
                                this.UpdatePanel1.Update();
                                this.PnlRespuesta.Visible = false;
                                this.RbEstadoAprobacion.ClearSelection();
                                this.TxtObservacionValidacion.Text = "";
                                this.TxtEmailsNotificacion.Text = "";
                                //this.BtnGuardar.Enabled = false;
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
                                this.RadGrid1.Rebind();
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
                            string _MsgError = "Señor usuario, no hay información seleccionada de la lista. Por favor validar nuevamente !";
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
                        string _MsgError = "Señor usuario, ocurrio un error con los item seleccionados de la lista. Por favor validar nuevamente !";
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
                    string _MsgError = "Señor usuario, para realizar este proceso primero debe seleccionar los item de las lista que se va notificar sobre la validación !";
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
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar el proceso de la validación por lote. Motivo: " + ex.ToString();
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
            DataTable DtLiquidaciones = new DataTable();
            try
            {
                #region DEFINIR COLUMNAS DEL DATATABLE
                //--DEFINIR COLUMNAS
                DtLiquidaciones = new DataTable();
                DtLiquidaciones.TableName = "DtLiquidaciones";
                DtLiquidaciones.Columns.Add("codigo_dane");
                DtLiquidaciones.Columns.Add("departamento");
                DtLiquidaciones.Columns.Add("municipio");
                DtLiquidaciones.Columns.Add("periodicidad");
                DtLiquidaciones.Columns.Add("fecha_vencimiento");
                DtLiquidaciones.Columns.Add("nombre_usuario");
                #endregion
            }
            catch (Exception ex)
            {
                DtLiquidaciones = null;
                _log.Error("Error al generar el datatable de datos. Motivo: " + ex.Message);
            }

            return DtLiquidaciones;
        }

        public static string GetTableHtml(string _TituloTablaHtml, DataTable DtDatos)
        {
            StringBuilder TableHtml = new StringBuilder();
            try
            {
                //Table start.
                TableHtml.Append("<table border = '1'>");
                TableHtml.Append("<tr align='center' valign='middle' >");
                TableHtml.Append("<th colspan=" + DtDatos.Columns.Count + "> " + _TituloTablaHtml + " </th> ");
                TableHtml.Append("</tr>");

                //Building the Header row.
                TableHtml.Append("<tr>");
                foreach (DataColumn column in DtDatos.Columns)
                {
                    TableHtml.Append("<th>");
                    TableHtml.Append(column.ColumnName.ToString().ToUpper());
                    TableHtml.Append("</th>");
                }
                TableHtml.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in DtDatos.Rows)
                {
                    TableHtml.Append("<tr>");
                    foreach (DataColumn column in DtDatos.Columns)
                    {
                        TableHtml.Append("<td>");
                        TableHtml.Append(row[column.ColumnName]);
                        TableHtml.Append("</td>");
                    }
                    TableHtml.Append("</tr>");
                }

                //Table end.
                TableHtml.Append("</table>");

            }
            catch (Exception ex)
            {
                TableHtml.Append("");
                _log.Error("Error al obtener la Tabla Html. Motivo: " + ex.Message);
            }

            return TableHtml.ToString();
        }

    }
}