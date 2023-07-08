using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Administracion;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion
{
    public partial class FrmAddActividadAnalista : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        ControlActividadesAnalista ObjActAnalista = new ControlActividadesAnalista();
        ControlActividades ObjActividad = new ControlActividades();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        protected void LstEstado()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEstado = null;
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MostrarSeleccione = "SI";
                ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbEstado.DataSource = ObjEstado.GetEstados();
                this.CmbEstado.DataValueField = "id_estado";
                this.CmbEstado.DataTextField = "codigo_estado";
                this.CmbEstado.DataBind();
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
                string _MsgMensaje = "Señor usuario. Error al listar los estados. Motivo: " + ex.ToString();
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

        private void GetInfoActividad()
        {
            try
            {
                ObjActAnalista.TipoConsulta = 1;
                ObjActAnalista.IdControlActividad = this.ViewState["IdControlActividad"].ToString().Trim();
                ObjActAnalista.IdTipoCtrlActividad = this.ViewState["IdTipoCtrlActividad"].ToString().Trim();
                ObjActAnalista.IdFormularioImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                ObjActAnalista.AnioGravable = this.ViewState["AnioGravable"].ToString().Trim();
                //--AQUI VALIDAMOS QUE EL USUARIO SEA UN ANALISTA
                int _IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                if (_IdRol == 7)
                {
                    ObjActAnalista.IdUsuario = this.Session["IdUsuario"].ToString().Trim();
                }
                else
                {
                    ObjActAnalista.IdUsuario = null;
                }
                ObjActAnalista.IdEstado = null;
                ObjActAnalista.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjActAnalista.GetAllActividades();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        #region OBTENER DATOS DEL DATATABLE
                        this.ViewState["IdCtrlActividadAnalista"] = dtDatos.Rows[0]["idctrl_actividades_analista"].ToString().Trim();
                        this.LblTipoCtrlActividad.Text = dtDatos.Rows[0]["tipo_ctrl_actividad"].ToString().Trim();
                        this.LblTipoImpuesto.Text = dtDatos.Rows[0]["descripcion_formulario"].ToString().Trim();
                        this.LblAnioGravable.Text = dtDatos.Rows[0]["anio_gravable"].ToString().Trim();
                        this.LblCodigoActividad.Text = dtDatos.Rows[0]["codigo_actividad"].ToString().Trim();
                        this.LblDescripcionActividad.Text = dtDatos.Rows[0]["descripcion_actividad"].ToString().Trim();
                        this.CmbEstado.SelectedValue = dtDatos.Rows[0]["id_estado"].ToString().Trim();

                        //--AQUI VALIDAMOS EL TIPO CONTROL ACTIVIDAD PARA EL MANEJO DE LAS CANTIDADES
                        string _ManejaDobleCantidad = dtDatos.Rows[0]["doble_cantidad"].ToString().Trim();
                        //--
                        if (_ManejaDobleCantidad.Equals("S"))
                        {
                            this.LblCantidad1.Text = "Cantidad ( Si )";
                            this.LblCantidad2.Text = "Cantidad ( No )";
                            this.TxtCantidad1.Text = dtDatos.Rows[0]["cantidad_si"].ToString().Trim();
                            this.Validador1.Enabled = true;
                            this.TxtCantidad2.Text = dtDatos.Rows[0]["cantidad_no"].ToString().Trim();
                            this.Validador2.Enabled = true;
                            this.TxtCantidad1.Focus();
                        }
                        else
                        {
                            this.LblCantidad1.Text = "Cantidad";
                            this.LblCantidad2.Text = "Cantidad ( No )";
                            this.LblCantidad2.Visible = false;
                            this.TxtCantidad1.Text = dtDatos.Rows[0]["cantidad"].ToString().Trim();
                            this.Validador1.Enabled = true;
                            this.TxtCantidad2.Text = "0";
                            this.TxtCantidad2.Visible = false;
                            this.TxtCantidad2.Enabled = false;
                            this.Validador2.Enabled = false;
                            this.TxtCantidad1.Focus();
                        }
                        #endregion
                    }
                    else
                    {
                        #region SI NO AY REGISTRO DE LA ACTIVIDAD CONFIGURADA SE OBTIENE DEL CONTROL DE ACTIVIDADES
                        this.ViewState["IdCtrlActividadAnalista"] = "";
                        ObjActividad.TipoConsulta = 3;
                        ObjActividad.IdControlActividad = this.ViewState["IdControlActividad"].ToString().Trim();
                        ObjActividad.IdTipoCtrlActividad = this.ViewState["IdTipoCtrlActividad"].ToString().Trim();
                        ObjActividad.IdFormularioImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                        ObjActividad.AnioGravable = this.ViewState["AnioGravable"].ToString().Trim();
                        ObjActividad.IdEstado = null;
                        ObjActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        dtDatos = ObjActividad.GetInfoActividad();
                        if (dtDatos != null)
                        {
                            if (dtDatos.Rows.Count > 0)
                            {
                                #region OBTENER DATOS DEL DATATABLE
                                this.LblTipoCtrlActividad.Text = dtDatos.Rows[0]["tipo_ctrl_actividad"].ToString().Trim();
                                this.LblTipoImpuesto.Text = dtDatos.Rows[0]["descripcion_formulario"].ToString().Trim();
                                this.LblAnioGravable.Text = dtDatos.Rows[0]["anio_gravable"].ToString().Trim();
                                this.LblCodigoActividad.Text = dtDatos.Rows[0]["codigo_actividad"].ToString().Trim();
                                this.LblDescripcionActividad.Text = dtDatos.Rows[0]["descripcion_actividad"].ToString().Trim();

                                //--AQUI VALIDAMOS EL TIPO CONTROL ACTIVIDAD PARA EL MANEJO DE LAS CANTIDADES
                                string _ManejaDobleCantidad = dtDatos.Rows[0]["doble_cantidad"].ToString().Trim();
                                //--
                                if (_ManejaDobleCantidad.Equals("S"))
                                {
                                    this.LblCantidad1.Text = "Cantidad ( Si )";
                                    this.LblCantidad2.Text = "Cantidad ( No )";
                                    this.TxtCantidad1.Text = "";
                                    this.Validador1.Enabled = true;
                                    this.TxtCantidad2.Text = "";
                                    this.Validador2.Enabled = true;
                                    this.TxtCantidad1.Focus();
                                }
                                else
                                {
                                    this.LblCantidad1.Text = "Cantidad";
                                    this.LblCantidad2.Text = "Cantidad ( No )";
                                    this.LblCantidad2.Visible = false;
                                    this.TxtCantidad1.Text = "";
                                    this.Validador1.Enabled = true;
                                    this.TxtCantidad2.Text = "0";
                                    this.TxtCantidad2.Visible = false;
                                    this.TxtCantidad2.Enabled = false;
                                    this.Validador2.Enabled = false;
                                    this.TxtCantidad1.Focus();
                                }
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
                                string _MsgMensaje = "No se encontro información con la tarifa seleccionada... !";
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
                        else
                        {

                        }
                        #endregion
                    }
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar los datos de la tarifa. Motivo: " + ex.ToString();
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

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = this.ViewState["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            //if (!objPermiso.PuedeLeer)
            //{
            //    this.BtnGuardar.Visible = false;
            //}
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnGuardar.Visible = false;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.BtnGuardar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--AQUI OBTENEMOS LOS PARAMETROS ENVIADOS
                this.ViewState["IdControlActividad"] = Request.QueryString["IdControlActividad"].ToString().Trim();
                this.ViewState["IdTipoCtrlActividad"] = Request.QueryString["IdTipoCtrlActividad"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["AnioGravable"] = Request.QueryString["AnioGravable"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();
                this.ViewState["IdCtrlActividadAnalista"] = "";
                this.AplicarPermisos();

                //--LISTAR
                this.LstEstado();
                this.CmbEstado.SelectedValue = "1";
                this.GetInfoActividad();

                //if (this.ViewState["TipoProceso"].ToString().Trim().Equals("UPDATE"))
                //{
                //    this.LblTitulo.Text = "EDITAR DATOS DE LA ACTIVIDAD DEL ANALISTA";
                //}
                //else
                //{
                //    this.LblTitulo.Text = "REGISTRAR DATOS DE LA ACTIVIDAD DEL ANALISTA";
                //}
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

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //--AQUI VALIDAMOS QUE EL USUARIO SEA UN ANALISTA
                //int _IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                string _IdRol = this.Session["IdRol"].ToString().Trim();
                bool _Result2 = FixedData.IdRolCtrlActividades.Contains(_IdRol);
                if (_Result2 == true)
                //if (_IdRol == 7 || _IdRol == 11)
                {
                    #region AQUI ENVIAMOS LOS DATOS A LA BASE DE DATOS
                    ObjActAnalista.IdCtrlActividadAnalista = this.ViewState["IdCtrlActividadAnalista"].ToString().Trim().Length > 0 ? this.ViewState["IdCtrlActividadAnalista"].ToString().Trim() : null;
                    ObjActAnalista.IdControlActividad = this.ViewState["IdControlActividad"].ToString().Trim();
                    ObjActAnalista.IdTipoCtrlActividad = this.ViewState["IdTipoCtrlActividad"].ToString().Trim();
                    ObjActAnalista.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                    ObjActAnalista.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjActAnalista.CantidadSi = this.TxtCantidad1.Text.ToString().Trim();
                    ObjActAnalista.CantidadNo = this.TxtCantidad2.Text.ToString().Trim();
                    ObjActAnalista.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                    ObjActAnalista.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjActAnalista.TipoProceso = this.ViewState["IdCtrlActividadAnalista"].ToString().Trim().Length > 0 ? 2 : 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjActAnalista);
                    _log.Warn("REQUEST INSERT ACTIVIDAD ANALISTA => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjActAnalista.AddUpControlActividades(ref _IdRegistro, ref _MsgError))
                    {
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "ACTIVIDADES_ANALISTA";
                        if (ObjActAnalista.TipoProceso == 1)
                        {
                            ObjAuditoria.IdTipoEvento = 2;
                        }
                        else
                        {
                            ObjAuditoria.IdTipoEvento = 3;
                        }
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

                        #region MOSTRAR MENSAJE DE USUARIO
                        //--LISMPIAR OBJETOS DEL FORM
                        //this.CmbTipoImpuesto.SelectedValue = "";
                        //this.CmbAnioGravable.SelectedValue = "";
                        //this.CmbEstado.SelectedValue = "";
                        //this.TxtCodActividad.Text = "";
                        //this.TxtCantidad2.Text = "";
                        //this.TxtDescripcionActividad.Text = "";

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
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor usuario, el perfil permitido para realizar este proceso debe ser ANALISTA !";
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
                string _MsgMensaje = "Señor usuario. Error al guardar la actividad del analista. Motivo: " + ex.ToString();
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
    }
}