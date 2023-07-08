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
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmEjecucionPorLote : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        EjecucionXLoteUser ObjEjecUser = new EjecucionXLoteUser();
        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();
        Combox ObjCombox = new Combox();

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
                ObjEjecUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

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
                Ventana.ID = "RadWindow2";
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
                ObjEjecFiltros.IdFormImpuesto = null;
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

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
                Ventana.ID = "RadWindow2";
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
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

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
                Ventana.ID = "RadWindow2";
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

        protected void LstMunicipioFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 4;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                String _MsgError = "";
                dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroMunicipio(ref _MsgError);
                this.CmbMunicipio.DataSource = dtDatos;
                this.CmbMunicipio.DataValueField = "id_municipio";
                this.CmbMunicipio.DataTextField = "nombre_municipio";
                this.CmbMunicipio.DataBind();

                //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                ObjEjecFiltros.TipoConsulta = 5;
                int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                    this.Validador6.Enabled = true;
                }
                else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                    this.Validador6.Enabled = true;
                    this.BtnEjecutar.Enabled = false;
                    this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                    this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.Validador6.Enabled = false;
                    this.BtnEjecutar.Enabled = false;
                    this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                    this.LblMensaje.ForeColor = System.Drawing.Color.Red;
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
                Ventana.ID = "RadWindow2";
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

        protected void LstAnioGravable()
        {
            try
            {
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
                Ventana.ID = "RadWindow2";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                //this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                //Llenar los combox
                this.LstTipoFiltros();
                this.LstAnioGravable();
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

        protected void CmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstTipoImpuestosFiltro();
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstDptosFiltro();
        }

        protected void CmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstMunicipioFiltro();
        }

        protected void CmbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region VALIDAR EL ESTADO DE LA DECLARACION
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;
                //--VALIDAR QUE EL MUNICIPIO SE ALLA SELECCIONADO
                if (this.CmbMunicipio.SelectedValue.ToString().Trim().Length > 0)
                {
                    #region OBTENER EL ESTADO DE LA DECLARACION 
                    ObjEjecFiltros.TipoConsulta = 5;
                    ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                    ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                    //--
                    string[] _ArrayMunicipio = this.CmbMunicipio.SelectedValue.ToString().Trim().Split('|');
                    ObjEjecFiltros.IdMunicipio = _ArrayMunicipio[0].ToString().Trim();
                    ObjEjecFiltros.IdEstado = 1;
                    ObjEjecFiltros.MostrarSeleccione = "SI";
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                    int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                    if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                    }
                    else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.Validador6.Enabled = false;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    #endregion
                }
                else
                {
                    #region OBTENER EL ESTADO DE LA DECLARACION 
                    ObjEjecFiltros.TipoConsulta = 5;
                    ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                    ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdMunicipio = null;
                    ObjEjecFiltros.IdEstado = 1;
                    ObjEjecFiltros.MostrarSeleccione = "SI";
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                    int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                    if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                    }
                    else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.Validador6.Enabled = false;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    #endregion
                }
                #endregion
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al validar el estado de la declaración. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
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
                if (CreateTaskScheduler())
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
                    string _MsgMensaje = "Señor usuario. La tarea fue creada de forma exitosa, tenga en cuenta que esta iniciara la ejecución en 2 minutos.";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow2";
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
                    string _MsgMensaje = "Señor usuario. Ocurrio un error al generar la tarea programada. Por favor validar con soporte técnico !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow2";
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

                // Get the service on the remote machine
                //using (TaskService ts = new TaskService(@"\\RemoteServer"))
                //using (TaskService ts = new TaskService())
                //{
                //    // Create a new task definition and assign properties
                //    TaskDefinition td = ts.NewTask();
                //    td.RegistrationInfo.Description = "Does something";

                //    // Create a trigger that will fire the task at this time every other day
                //    td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

                //    // Create an action that will launch Notepad whenever the trigger fires
                //    td.Actions.Add(new ExecAction("notepad.exe", "D:\\PRUEBA\\prueba.txt", null));

                //    // Register the task in the root folder
                //    ts.RootFolder.RegisterTaskDefinition(@"Test", td);
                //}

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
                Ventana.ID = "RadWindow2";
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
        
        private bool CreateTaskScheduler()
        {
            bool Result = false;
            string _MsgError = "";
            try
            {
                //Obtener el servicio en la máquina local
                using (TaskService ts = new TaskService())
                {
                    string _NombreTarea = this.Session["NombreCompletoUsuario"].ToString().Trim() + "_" + this.CmbTipoFiltro.SelectedValue.ToString().Trim();
                    string _TipoImpuesto = this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim();
                    string _Departamento = this.CmbDepartamento.SelectedItem.Text.ToString().Trim();
                    string _Municipio = this.CmbMunicipio.SelectedValue.ToString().Trim().Length > 0 ? this.CmbMunicipio.SelectedItem.Text.ToString().Trim() : "SIN_DEFINIR";
                    string _EstadoLiquidacion = this.RbEstadoDeclaracion.SelectedValue.ToString().Trim().Equals("2") ? "PRELIMINAR" : "DEFINITIVO";

                    //Crear una nueva definición de tareas y asignar propiedades
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Tarea Programada para la liquidación por lote del Impuesto: " + _TipoImpuesto + ", Dpto: " + _Departamento + ", Municipio: " + _Municipio + ", Estado: " + _EstadoLiquidacion + ".";

                    //WeeklyTrigger wt = new WeeklyTrigger();
                    //int idx = 0;

                    //string[] ArrayHoraInicio = this.CmbInicio.SelectedItem.Text.Split(':');
                    //wt.StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(ArrayHoraInicio[0]), int.Parse(ArrayHoraInicio[1]), 0);

                    //foreach (ListItem item in this.ChkDiasEnvio.Items)
                    //{
                    //    if (item.Selected)
                    //    {
                    //        switch (Int32.Parse(item.Value))
                    //        {
                    //            case 1:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Monday : wt.DaysOfWeek | DaysOfTheWeek.Monday);
                    //                break;
                    //            case 2:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Tuesday : wt.DaysOfWeek | DaysOfTheWeek.Tuesday);
                    //                break;
                    //            case 3:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Wednesday : wt.DaysOfWeek | DaysOfTheWeek.Wednesday);
                    //                break;
                    //            case 4:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Thursday : wt.DaysOfWeek | DaysOfTheWeek.Thursday);
                    //                break;
                    //            case 5:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Friday : wt.DaysOfWeek | DaysOfTheWeek.Friday);
                    //                break;
                    //            case 6:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Saturday : wt.DaysOfWeek | DaysOfTheWeek.Saturday);
                    //                break;
                    //            case 7:
                    //                wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Sunday : wt.DaysOfWeek | DaysOfTheWeek.Sunday);
                    //                break;
                    //        }
                    //        idx++;
                    //    }
                    //}

                    //wt.WeeksInterval = 1;

                    ////Configurar la hora de inicio y fin de la tarea.
                    //string[] ArrayHoraFin = this.CmbHoraFin.SelectedItem.Text.Split(':');
                    //int startHour = int.Parse(ArrayHoraInicio[0].ToString().Trim());
                    //int endHour = int.Parse(ArrayHoraFin[0].ToString().Trim());
                    //int startMinute = int.Parse(ArrayHoraInicio[1].ToString().Trim());
                    //int endMinute = int.Parse(ArrayHoraFin[1].ToString().Trim());

                    //int _IdTipoEnvio = Int32.Parse(this.CmbTipoEnvio.SelectedValue.ToString().Trim());
                    //if (_IdTipoEnvio == 1)
                    //{
                    //    wt.Repetition.Duration = new TimeSpan((endHour - startHour), Math.Abs(endMinute - startMinute), 0);
                    //    wt.Repetition.Interval = TimeSpan.FromMinutes(Int32.Parse(this.CmbIntervalo.SelectedItem.Text));
                    //}
                    //else
                    //{
                    //    //wt.Repetition.Duration = new TimeSpan((endHour - startHour), Math.Abs(endMinute - startMinute), 0);
                    //    //wt.Repetition.Interval = TimeSpan.FromMinutes(Int32.Parse(this.CmbIntervalo.SelectedItem.Text));
                    //}
                    //td.Triggers.Add(wt);
                    td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

                    //--AQUI DEFINIMOS LOS PARAMETROS CON LOS QUE SE VAN A CREAR LAS TAREAS
                    string _IdCliente = this.Session["IdCliente"].ToString().Trim();
                    string _IdUsuario = this.Session["IdUsuario"].ToString().Trim();
                    string _IdFiltroEjecucion = this.CmbTipoFiltro.SelectedValue.ToString().Trim();
                    string _IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                    string _IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim();
                    string _IdMunicipio = this.CmbMunicipio.SelectedValue.ToString().Trim();
                    string _EstadoDeclaracion = this.RbEstadoDeclaracion.SelectedValue.ToString().Trim();
                    int _TipoProceso = 2;  //--INDICA LA EJECUCION POR LOTE
                    string _AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();

                    //--AQUI CREAMOS LA TAREA CON LOS PARAMETROS DEFINIDOS
                    td.Actions.Add(new ExecAction(FixedData.PathTasksProgramadas.ToString().Trim(), _IdCliente + " " + _IdUsuario + " " + _IdFiltroEjecucion + " " + _IdFormImpuesto + " " + _IdDepartamento + " " + _IdMunicipio + " " + _EstadoDeclaracion + " " + _TipoProceso + " " + _AnioGravable));
                    ts.RootFolder.RegisterTaskDefinition(_NombreTarea.ToString(), td, TaskCreation.CreateOrUpdate, FixedData.UserCreateTasks, FixedData.PassCreateTasks);
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                Result = false;
                this.UpdatePanel1.Update();
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                _MsgError = "Error al crear la tarea. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/ControlUsuario/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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

            return Result;
        }
        
    }
}