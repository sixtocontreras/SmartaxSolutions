using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using log4net;
using Telerik.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class FrmPermisosRol : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        SistemaRol ObjRol = new SistemaRol();
        SistemaNavegacion objNavegacion = new SistemaNavegacion();
        SistemaPermiso objPermiso = new SistemaPermiso();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();

        private void AplicarPermisos()
        {
            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnGuardar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                ViewState["MenuID"] = null;
                this.LblRolID.Text = Request.QueryString["RolID"].ToString().Trim();

                //Aqui llamamos la función que Muestra el menu de navegacion
                LoadMenuNavegacion();
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

        private void LoadMenuNavegacion()
        {
            try
            {
                DataSet MenuDS = new DataSet();

                objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                string DatosCache = Session["DtCacheMenu"].ToString().Trim();

                //Aqui validamos que el usuario que este logeado tenga el Rol de Usuario soporte
                if (Int32.Parse(Session["IdRol"].ToString().Trim()) == 1)
                {
                    MenuDS = objNavegacion.GetMenu();
                    Cache.Add(DatosCache, MenuDS, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
                else
                {
                    //En caso que el usuario tenga un rol diferente a super usuario le muestre sea las opciones en cache.
                    if (Cache.Get(DatosCache) == null)
                    {
                        MenuDS = objNavegacion.GetMenu();
                        Cache.Add(DatosCache, MenuDS, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                    }
                    else
                    {
                        MenuDS = (DataSet)Cache.Get(DatosCache);
                    }
                }

                //Mostramos las opciones de menu en el arbol.
                this.RadTreeView1.DataSource = MenuDS;
                this.RadTreeView1.DataMember = "DtMenu";
                this.RadTreeView1.DataTextField = "titulo_opcion";
                this.RadTreeView1.ToolTip = "descripcion_opcion";
                this.RadTreeView1.DataFieldID = "MenuID";
                this.RadTreeView1.DataFieldParentID = "ParentID";
                this.RadTreeView1.DataValueField = "MenuID";
                this.RadTreeView1.DataBind();

                if (MenuDS.Tables["DtMenu"].Columns[4] != null)
                {
                    this.RadTreeView1.DataFieldParentID = "ParentID";
                }
                this.RadTreeView1.DataBind();
            }
            catch (Exception ex)
            {
                //mostramos la ventana del mensaje del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                //Aqui mostramos el mensaje del proceso al usuario
                string _MsgError = "Error al listar las opciones de menu. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow1";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Info(_MsgError);
            }
        }

        protected void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                RadTreeNode ItemMenu = new RadTreeNode();
                ItemMenu = e.Node;
                this.ViewState["MenuID"] = Int32.Parse(ItemMenu.Value);
                this.ViewState["DescripcionMenu"] = ItemMenu.Text.ToString().Trim();

                //this.LblRol.Text = "Rol: " + ViewState["NombreRol"] + " Opción: " + ViewState["DescripcionMenu"];
                objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                if (objPermiso.DevolverPermisoMenu(Int32.Parse(this.LblRolID.Text), Int32.Parse(this.ViewState["MenuID"].ToString().Trim())) == true)
                {
                    this.ChkLeer.Checked = objPermiso.PuedeLeer;
                    this.ChkEscribir.Checked = objPermiso.PuedeRegistrar;
                    this.ChkModificar.Checked = objPermiso.PuedeModificar;
                    this.ChkEliminar.Checked = objPermiso.PuedeEliminar;
                    this.ChkBloquear.Checked = objPermiso.PuedeBloquear;
                    this.ChkAnular.Checked = objPermiso.PuedeAnular;
                    this.ChkExportar.Checked = objPermiso.PuedeExportar;
                    this.ChkConfigurar.Checked = objPermiso.PuedeConfigurar;
                    this.ChkImpBorrador.Checked = objPermiso.PuedeLiqBorrador;
                    this.ChkImpDefinitivo.Checked = objPermiso.PuedeLiqDefinitivo;
                    this.ChkVerFormulario.Checked = objPermiso.PuedeVerFormulario;
                }
                else
                {
                    this.ChkLeer.Checked = false;
                    this.ChkEscribir.Checked = false;
                    this.ChkModificar.Checked = false;
                    this.ChkEliminar.Checked = false;
                    this.ChkBloquear.Checked = false;
                    this.ChkAnular.Checked = false;
                    this.ChkExportar.Checked = false;
                    this.ChkConfigurar.Checked = false;
                    this.ChkImpDefinitivo.Checked = false;
                    this.ChkVerFormulario.Checked = false;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENASJE DE USUARIO
                //mostramos la ventana del mensaje del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                //Aqui mostramos el mensaje del proceso al usuario
                string _MsgError = "Error al seleccionar la opción de menu. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow1";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Info(_MsgError);
                #endregion
            }
        }

        protected void ChkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionarTodos.Checked == true)
            {
                this.ChkLeer.Checked = true;
                this.ChkEscribir.Checked = true;
                this.ChkModificar.Checked = true;
                this.ChkEliminar.Checked = true;
                this.ChkBloquear.Checked = true;
                this.ChkAnular.Checked = true;
                this.ChkExportar.Checked = true;
                this.ChkConfigurar.Checked = true;
                this.ChkImpBorrador.Checked = true;
                this.ChkImpDefinitivo.Checked = true;
                this.ChkVerFormulario.Checked = true;
            }
            else
            {
                this.ChkLeer.Checked = false;
                this.ChkEscribir.Checked = false;
                this.ChkModificar.Checked = false;
                this.ChkEliminar.Checked = false;
                this.ChkBloquear.Checked = false;
                this.ChkAnular.Checked = false;
                this.ChkExportar.Checked = false;
                this.ChkConfigurar.Checked = false;
                this.ChkImpBorrador.Checked = false;
                this.ChkImpDefinitivo.Checked = false;
                this.ChkVerFormulario.Checked = false;
            }
        }

        protected void ChkQuitarTodos_CheckedChanged(object sender, EventArgs e)
        {
            this.ChkLeer.Checked = false;
            this.ChkEscribir.Checked = false;
            this.ChkModificar.Checked = false;
            this.ChkEliminar.Checked = false;
            this.ChkBloquear.Checked = false;
            this.ChkAnular.Checked = false;
            this.ChkExportar.Checked = false;
            this.ChkConfigurar.Checked = false;
            this.ChkImpBorrador.Checked = false;
            this.ChkImpDefinitivo.Checked = false;
            this.ChkVerFormulario.Checked = false;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ViewState["MenuID"] != null)
                {
                    #region OBTENER DATOS PARA EL OBJETO DE CLASE
                    objPermiso.IdRol = Int32.Parse(this.LblRolID.Text.ToString().Trim());
                    objPermiso.IdNavegacion = Int32.Parse(ViewState["MenuID"].ToString().Trim());
                    objPermiso.NombreOpcionMenu = this.ViewState["DescripcionMenu"].ToString().Trim();
                    //--
                    objPermiso.PuedeLeer = this.ChkLeer.Checked == true ? true : false;
                    objPermiso.PuedeRegistrar = this.ChkEscribir.Checked == true ? true : false;
                    objPermiso.PuedeModificar = this.ChkModificar.Checked == true ? true : false;
                    objPermiso.PuedeEliminar = this.ChkEliminar.Checked == true ? true : false;
                    objPermiso.PuedeBloquear = this.ChkBloquear.Checked == true ? true : false;
                    objPermiso.PuedeAnular = this.ChkAnular.Checked == true ? true : false;
                    objPermiso.PuedeExportar = this.ChkExportar.Checked == true ? true : false;
                    objPermiso.PuedeConfigurar = this.ChkConfigurar.Checked == true ? true : false;
                    objPermiso.PuedeLiqBorrador = this.ChkImpBorrador.Checked == true ? true : false;
                    objPermiso.PuedeLiqDefinitivo = this.ChkImpDefinitivo.Checked == true ? true : false;
                    objPermiso.PuedeVerFormulario = this.ChkVerFormulario.Checked == true ? true : false;
                    objPermiso.IdUsuarioAdd = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                    objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    objPermiso.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(objPermiso);
                    #endregion

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (objPermiso.AddUpPermisos(ref _IdRegistro, ref _MsgError) == true)
                    {
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "PERMISOS_ROL";
                        //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                        ObjAuditoria.IdTipoEvento = 2;
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

                        #region QUITAR SELECCION DE CHECK
                        this.ChkSeleccionarTodos.Checked = false;
                        this.ChkQuitarTodos.Checked = false;
                        this.ChkLeer.Checked = false;
                        this.ChkEscribir.Checked = false;
                        this.ChkModificar.Checked = false;
                        this.ChkEliminar.Checked = false;
                        this.ChkBloquear.Checked = false;
                        this.ChkAnular.Checked = false;
                        this.ChkAnular.Checked = false;
                        this.ChkExportar.Checked = false;
                        this.ChkConfigurar.Checked = false;
                        this.ChkImpBorrador.Checked = false;
                        this.ChkImpDefinitivo.Checked = false;
                        this.ChkVerFormulario.Checked = false;
                        #endregion

                        #region MOSTRAR MENSAJE DE USUARIO
                        //mostramos la ventana del mensaje del sistema.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        //Aqui mostramos el mensaje del proceso al usuario
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow1";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        _log.Info(_MsgError);
                        #endregion
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        //mostramos la ventana del mensaje del sistema.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        //Aqui mostramos el mensaje del proceso al usuario
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow1";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        _log.Info(_MsgError);
                        #endregion
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //mostramos la ventana del mensaje del sistema.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    //Aqui mostramos el mensaje del proceso al usuario
                    string _MsgError = "Debe seleccionar una Opción de Menu la cual le va asignar el Permiso !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow1";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(230);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    _log.Info(_MsgError);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //mostramos la ventana del mensaje del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                //Aqui mostramos el mensaje del proceso al usuario
                string _MsgError = "Error al asignar el Permiso a la opción de menu [" + this.ViewState["DescripcionMenu"].ToString().Trim() + "]. Motivo: " + ex.Message.ToString().Trim();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow1";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Info(_MsgError);
                #endregion
            }

        }
    }
}