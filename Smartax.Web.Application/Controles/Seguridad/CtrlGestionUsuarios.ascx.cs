using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Text;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Seguridad;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlGestionUsuarios : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE CLASE
        Usuario ObjUser = new Usuario();
        SistemaRol ObjRoles = new SistemaRol();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        #endregion

        public DataSet GetObtenerDatos()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar usuarios
                ObjUser.TipoConsulta = 1;
                ObjUser.IdUsuario = null;
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdEstado = null;
                ObjUser.IdRol = Convert.ToInt32(this.Session["IdRol"].ToString().Trim());
                ObjUser.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                ObjetoDataTable = ObjUser.GetAllUsuarios();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_usuario, id_rol"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al cargar los Datos de usuarios. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(270);
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

            return ObjetoDataSet;
        }

        private DataSet FuenteDatosGrillaDatos
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosGrillaDatos"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetObtenerDatos();
                    this.ViewState["_FuenteDatosGrillaDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosGrillaDatos"];
                }
            }
            set { this.ViewState["_FuenteDatosGrillaDatos"] = value; }
        }

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            int _IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeLeer)
            {
                RadGrid1.Visible = false;
            }

            if (_IdRol > 2)
            {
                RadGrid1.MasterTableView.CommandItemDisplay = 0;
            }

            if (!objPermiso.PuedeModificar)
            {
                RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
                RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
                RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;
            }
            //if (!objPermiso.PuedeEliminar)
            //{
            //    RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Gestión de Usuarios";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION DE EVENTOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatosGrillaDatos;
                RadGrid1.DataMember = "DtUsuarios";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo NeedDataSource. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                #endregion
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnBloquear")
                {
                    #region DEFINICION DEL METODO PARA BLOQUEAR USUARIOS
                    GridDataItem item = (GridDataItem)e.Item;
                    ObjUser.IdUsuario = Int32.Parse(item["id_usuario"].Text.ToString().Trim());
                    ObjUser.NombreUsuario = item["nombre_completo_usuario"].Text.ToString().Trim();
                    ObjUser.IdentificacionUsuario = item["identificacion_usuario"].Text.ToString().Trim();
                    ObjUser.IdEstado = Int32.Parse(item["id_estado"].Text.ToString().Trim());
                    ObjUser.LoginUsuario = item["login_usuario"].Text.ToString().Trim();
                    ObjUser.PasswordUsuario = "";
                    ObjUser.CambiarClave = "";
                    ObjUser.FechaExpClave = "";
                    ObjUser.EmailUsuario = item["email_usuario"].Text.ToString().Trim();

                    //--Aqui buscamos el id del usuario en el datatable
                    DataTable TablaDatos = new DataTable();
                    TablaDatos = this.FuenteDatosGrillaDatos.Tables["DtUsuarios"];
                    DataRow[] changedRows = TablaDatos.Select("id_usuario = " + ObjUser.IdUsuario);

                    if (changedRows.Length == 1)
                    {
                        ObjUser.IdUsuarioUp = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjUser.TipoProceso = 2;

                        if (ObjUser.IdEstado.Equals(1))
                        {
                            ObjUser.IdEstado = 0;

                            //--AQUI SERIALIZAMOS EL OBJETO CLASE
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            string jsonRequest = js.Serialize(ObjUser);

                            int _IdRegistro = 0;
                            string _MsgError = "";
                            if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _MsgError))
                            {
                                #region REGISTRO DE LOGS DE AUDITORIA
                                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                                ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                                ObjAuditoria.ModuloApp = "GESTION_BLOQUEAR_USUARIOS";
                                //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                                ObjAuditoria.IdTipoEvento = 3;
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

                                #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                                //Datos para el envio del correo
                                ObjCorreo.StrServerCorreo = this.Session["ServerCorreo"].ToString().Trim();
                                ObjCorreo.PuertoCorreo = Int32.Parse(this.Session["PuertoCorreo"].ToString().Trim());
                                ObjCorreo.StrEmailDe = this.Session["EmailSoporte"].ToString().Trim();
                                ObjCorreo.StrPasswordDe = this.Session["PasswordEmail"].ToString().Trim();
                                ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();

                                if (ObjCorreo.StrEmailPara.ToString().Trim().Length > 0)
                                {
                                    ObjCorreo.StrAsunto = "REF.: BLOQUEO DE USUARIO";
                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + " Señor Usuario, para informarle que su usuario ha sido DesBloqueado para el Ingreso al [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                                    "<h4>DATOS DEL USUARIO</h2>" + "<br/>" +
                                                    "Nombre del usuario: " + ObjUser.NombreUsuario + "<br/>" +
                                                    "Identificación: " + ObjUser.IdentificacionUsuario + "<br/>" +
                                                    "Estado de su usuario: INACTIVO<br/>" +
                                                    "<br/><br/>" +
                                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                    ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                                    string _MsgErrorEmail = "";
                                    if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                                    {
                                        _log.Error(_MsgErrorEmail.ToString().Trim());
                                    }
                                }
                                #endregion

                                this.ViewState["_FuenteDatosGrillaDatos"] = null;
                                this.RadGrid1.Rebind();

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
                                Ventana.Height = Unit.Pixel(230);
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
                                Ventana.Height = Unit.Pixel(230);
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
                            string _MsgMensaje = "El Usuario [" + ObjUser.NombreUsuario + "] ya se encuentra Bloqueado.";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        string _MsgMensaje = "El Id del Usuario no pudo ser identificado. Intentelo nuevamente por favor !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        #endregion
                    }
                    #endregion                    
                }
                else if (e.CommandName == "BtnDesBloquear")
                {
                    #region DEFINICION DEL METODO PARA BLOQUEAR USUARIOS
                    GridDataItem item = (GridDataItem)e.Item;
                    ObjUser.IdUsuario = Int32.Parse(item["id_usuario"].Text.ToString().Trim());
                    ObjUser.NombreUsuario = item["nombre_completo_usuario"].Text.ToString().Trim();
                    ObjUser.IdentificacionUsuario = item["identificacion_usuario"].Text.ToString().Trim();
                    ObjUser.IdEstado = Int32.Parse(item["id_estado"].Text.ToString().Trim());
                    ObjUser.LoginUsuario = item["login_usuario"].Text.ToString().Trim();
                    ObjUser.PasswordUsuario = "";
                    ObjUser.CambiarClave = "";
                    ObjUser.FechaExpClave = "";
                    ObjUser.EmailUsuario = item["email_usuario"].Text.ToString().Trim();

                    //--Aqui buscamos el id del usuario en el datatable
                    DataTable TablaDatos = new DataTable();
                    TablaDatos = this.FuenteDatosGrillaDatos.Tables["DtUsuarios"];
                    DataRow[] changedRows = TablaDatos.Select("id_usuario = " + ObjUser.IdUsuario);

                    if (changedRows.Length == 1)
                    {
                        ObjUser.IdUsuarioUp = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjUser.TipoProceso = 2;

                        if (ObjUser.IdEstado.Equals(0))
                        {
                            ObjUser.IdEstado = 1;

                            //--AQUI SERIALIZAMOS EL OBJETO CLASE
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            string jsonRequest = js.Serialize(ObjUser);

                            int _IdRegistro = 0;
                            string _MsgError = "";
                            if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _MsgError))
                            {
                                #region REGISTRO DE LOGS DE AUDITORIA
                                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                                ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                                ObjAuditoria.ModuloApp = "GESTION_DESBLOQUEAR_USUARIOS";
                                //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                                ObjAuditoria.IdTipoEvento = 3;
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

                                #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                                //Datos para el envio del correo
                                ObjCorreo.StrServerCorreo = this.Session["ServerCorreo"].ToString().Trim();
                                ObjCorreo.PuertoCorreo = Int32.Parse(this.Session["PuertoCorreo"].ToString().Trim());
                                ObjCorreo.StrEmailDe = this.Session["EmailSoporte"].ToString().Trim();
                                ObjCorreo.StrPasswordDe = this.Session["PasswordEmail"].ToString().Trim();
                                ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();

                                if (ObjCorreo.StrEmailPara.ToString().Trim().Length > 0)
                                {
                                    ObjCorreo.StrAsunto = "REF.: BLOQUEO DE USUARIO";
                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + " Señor Usuario, para informarle que su usuario ha sido DesBloqueado para el Ingreso al [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                                    "<h4>DATOS DEL USUARIO</h2>" + "<br/>" +
                                                    "Nombre del usuario: " + ObjUser.NombreUsuario + "<br/>" +
                                                    "Identificación: " + ObjUser.IdentificacionUsuario + "<br/>" +
                                                    "Estado de su usuario: ACTIVO<br/>" +
                                                    "<br/><br/>" +
                                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                    ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                                    string _MsgErrorEmail = "";
                                    if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                                    {
                                        _log.Error(_MsgErrorEmail.ToString().Trim());
                                    }
                                }
                                #endregion

                                this.ViewState["_FuenteDatosGrillaDatos"] = null;
                                this.RadGrid1.Rebind();

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
                                Ventana.Height = Unit.Pixel(230);
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
                                Ventana.Height = Unit.Pixel(230);
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
                            string _MsgMensaje = "El Usuario [" + ObjUser.NombreUsuario + "] ya se encuentra Desbloqueado.";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        string _MsgMensaje = "El Id del Usuario no pudo ser identificado. Intentelo nuevamente por favor !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        #endregion
                    }
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
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemCommand. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                #endregion
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (((e.Item is GridEditableItem) && (((GridEditableItem)e.Item).IsInEditMode)))
            {
                try
                {
                    DataRowView row = (DataRowView)e.Item.DataItem;
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridEditableItem ItemEditado = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor CajaText = item.EditManager.GetColumnEditor("nombre_usuario") as GridTextBoxColumnEditor;

                    if (row != null)
                    {
                        CajaText.Text = row["nombre_usuario"].ToString().Trim();
                        CajaText.TextBoxControl.Enabled = false;

                        CajaText = new GridTextBoxColumnEditor();
                        CajaText = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("apellido_usuario") as GridTextBoxColumnEditor;
                        CajaText.Text = row["apellido_usuario"].ToString().Trim();
                        CajaText.TextBoxControl.Enabled = false;

                        CajaText = new GridTextBoxColumnEditor();
                        CajaText = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("login_usuario") as GridTextBoxColumnEditor;
                        CajaText.Text = row["login_usuario"].ToString().Trim();
                        CajaText.TextBoxControl.Enabled = false;

                        CajaText = new GridTextBoxColumnEditor();
                        CajaText = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("email_usuario") as GridTextBoxColumnEditor;
                        CajaText.Text = row["email_usuario"].ToString().Trim();
                        CajaText.TextBoxControl.Enabled = false;

                        CajaText = new GridTextBoxColumnEditor();
                        CajaText = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("password_nuevo") as GridTextBoxColumnEditor;
                        //CajaText.Text = "";
                        CajaText.TextBoxMode = TextBoxMode.Password;
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
                    string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemDataBound. Motivo: " + ex.ToString();
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    #endregion
                }
            }
        }

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("password_nuevo");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;
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
                    string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemCreated. Motivo: " + ex.ToString();
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    #endregion
                }
            }
        }

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid1.Rebind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo PageIndexChanged. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                #endregion
            }
        }
        #endregion

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            #region OBTENER DATOS DEL REGISTRO SELECCIONADO EN EL GRID
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosGrillaDatos.Tables["DtUsuarios"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_usuario"] };
            DataRow[] changedRows = TablaDatos.Select("id_usuario = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_usuario"].ToString());

            if (changedRows.Length != 1)
            {
                e.Canceled = true;
                return;
            }

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            changedRows[0].BeginEdit();
            #endregion

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }

                #region DEFINICION DE VALORES A PASAR AL OBJETO CLASE
                ObjUser.IdUsuario = Convert.ToInt32(changedRows[0]["id_usuario"].ToString().Trim());
                ObjUser.NombreUsuario = changedRows[0]["nombre_usuario"].ToString().Trim() + " " + changedRows[0]["apellido_usuario"].ToString().Trim();
                ObjUser.LoginUsuario = changedRows[0]["login_usuario"].ToString().Trim();
                ObjUser.EmailUsuario = changedRows[0]["email_usuario"].ToString().Trim();
                string _NewPassword = changedRows[0]["password_nuevo"].ToString().Trim();
                ObjUser.PasswordUsuario = _NewPassword;
                ObjUser.CambiarClave = "S";
                ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                ObjUser.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjUser.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjUser.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.TipoProceso = 1;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjUser);
                #endregion

                if (_NewPassword.ToString().Trim().Length >= 8 && _NewPassword.ToString().Trim().Length <= FixedData.LongitudClaveUsuario)
                {
                    if (ContrasenaSegura(_NewPassword))
                    {
                        int _IdRegistro = 0;
                        string _Mensaje = "";
                        if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _Mensaje))
                        {
                            changedRows[0].EndEdit();

                            #region ENVIO DE CORREO AL USUARIO
                            ObjCorreo.StrServerCorreo = this.Session["ServerCorreo"].ToString().Trim();
                            ObjCorreo.PuertoCorreo = Int32.Parse(this.Session["PuertoCorreo"].ToString().Trim());
                            ObjCorreo.StrEmailDe = this.Session["EmailSoporte"].ToString().Trim();
                            ObjCorreo.StrPasswordDe = this.Session["PasswordEmail"].ToString().Trim();
                            ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();
                            ObjCorreo.StrAsunto = "REF.: CAMBIO DE CLAVE";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + " Señor Usuario, para informarle que el Administrador del sistema le acaba de realizar el cambio de clave para el Ingreso al sistema [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                            "<h4>DATOS DEL USUARIO</h2>" + "<br/>" +
                                            "Nombre: " + ObjUser.NombreUsuario + " " + ObjUser.ApellidoUsuario + "<br/>" +
                                            "Usuario: " + ObjUser.LoginUsuario.ToString().Trim() + "<br/>" +
                                            "Nuevo Password: " + _NewPassword.ToString().Trim() + "<br/>" +
                                            "<br/><br/>" +
                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                            ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                            string _MsgErrorEmail = "";
                            if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                            {
                                _Mensaje = _Mensaje + " Pero " + _MsgErrorEmail.ToString().Trim();
                            }
                            #endregion

                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.ModuloApp = "CAMBIO_CLAVE_GESTION_USUARIOS";
                            //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                            ObjAuditoria.IdTipoEvento = 3;
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
                            //Mostramos el mensaje porque se produjo un error con la Trx.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
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
                        string _MsgMensaje = "Señor usuario, la clave debe ser alfanumerica debe contener minimo una letra, numeros y caracteres especiales !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        e.Canceled = true;
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
                    string _MsgMensaje = "Señor usuario, la clave debe tener una longitud minima de 8 y maximo " + FixedData.LongitudClaveUsuario + " caracteres !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    e.Canceled = true;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al editar los datos del usuario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                e.Canceled = true;
                #endregion
            }
        }

        public Boolean ContrasenaSegura(String _PasswordUser)
        {
            //letras de la A a la Z, mayusculas y minusculas
            Regex letras = new Regex(@"[a-zA-z]");
            //digitos del 0 al 9
            Regex numeros = new Regex(@"[0-9]");
            //cualquier caracter del conjunto
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");

            Boolean cumpleCriterios = false;

            //si no contiene las letras, regresa false
            if (!letras.IsMatch(_PasswordUser))
            {
                return false;
            }
            //si no contiene los numeros, regresa false
            if (!numeros.IsMatch(_PasswordUser))
            {
                return false;
            }

            //si no contiene los caracteres especiales, regresa false
            if (!caracEsp.IsMatch(_PasswordUser))
            {
                return false;
            }

            //si cumple con todo, regresa true
            return true;
        }

    }
}