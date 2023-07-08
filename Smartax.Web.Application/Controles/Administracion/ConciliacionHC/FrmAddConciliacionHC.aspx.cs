using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Drawing.Charts;
using log4net;
using Microsoft.Win32.TaskScheduler;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.ProcessAPIs;
using Smartax.Web.Application.Clases.Seguridad;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Administracion.ConciliacionHC
{
    public partial class FrmAddConciliacionHC : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConciliacionesHerramientaCuadre ObjConciliacionHC = new ConciliacionesHerramientaCuadre();
        ProcessAPI ObjProcessAPI = new ProcessAPI();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();
        Combox ObjLista = new Combox();

        protected void LstAnioGravable()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = ObjLista.GetAnios();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años. Motivo: " + ex.ToString();
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

        protected void LstTipoPeriodicidad()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbTipoPeriodicidad.DataSource = ObjLista.GetTipoPeriodicidad();
                this.CmbTipoPeriodicidad.DataValueField = "idtipo_periodicidad";
                this.CmbTipoPeriodicidad.DataTextField = "tipo_periodicidad";
                this.CmbTipoPeriodicidad.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipo de periodicidad. Motivo: " + ex.ToString();
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

        protected void LstPeriodicidadMensual()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbPeriodo.DataSource = ObjLista.GetMeses();
                this.CmbPeriodo.DataValueField = "id_mes";
                this.CmbPeriodo.DataTextField = "numero_mes";
                this.CmbPeriodo.DataBind();

                this.BtnEjecProceso.Enabled = true;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un error al listar la periodicidad mensual. Motivo: " + ex.ToString();
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

        protected void LstPeriodicidadBimestral()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbPeriodo.DataSource = ObjLista.GetBimestral();
                this.CmbPeriodo.DataValueField = "id_periodicidad";
                this.CmbPeriodo.DataTextField = "periodicidad";
                this.CmbPeriodo.DataBind();

                this.BtnEjecProceso.Enabled = true;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un error al listar la periodicidad bimestral. Motivo: " + ex.ToString();
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

        protected void LstAplicativo()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbAplicativo.DataSource = ObjLista.GetAplicativo();
                this.CmbAplicativo.DataValueField = "id_aplicativo";
                this.CmbAplicativo.DataTextField = "aplicativo";
                this.CmbAplicativo.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años. Motivo: " + ex.ToString();
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
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            this.BtnEjecProceso.Enabled = false;
            if (!objPermiso.PuedeRegistrar)
            {
                //this.BtnGuardar.Enabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                //--LISTAR COMBOBOX
                this.LstAnioGravable();
                this.LstAplicativo();
                this.LstTipoPeriodicidad();
            }
        }

        protected void CmbTipoPeriodicidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim().Equals("1"))
                {
                    this.LstPeriodicidadMensual();
                }
                else
                {
                    this.LstPeriodicidadBimestral();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                #region DEFINICION DE VARIABLES OBJETO CLIENTE
                if (this.ViewState["TipoProceso"].Equals("INSERT"))
                {
                    ObjConciliacionHC.IdCliente = null;
                    ObjConciliacionHC.TipoProceso = 1;
                }
                else if (this.ViewState["TipoProceso"].Equals("UPDATE"))
                {
                    ObjConciliacionHC.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                    ObjConciliacionHC.TipoProceso = 2;
                }

                ObjConciliacionHC.IdAnio = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                ObjConciliacionHC.IdMes = Int32.Parse(this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim());
                ObjConciliacionHC.IdAplicativo = Int32.Parse(this.CmbAplicativo.SelectedValue.ToString().Trim());
                ObjConciliacionHC.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                #endregion

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjConciliacionHC.AddConciliacionHC(ref _IdRegistro, ref _MsgError))
                {
                    if (this.ViewState["TipoProceso"].Equals("INSERT"))
                    {
                        #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                        //--Definir valores para envio del email
                        ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                        ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                        ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                        ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                        ObjCorreo.StrEmailPara = "hcruz5785@gmial.com";//ObjCliente.EmailContacto;
                        ObjCorreo.StrAsunto = "REF.: REGISTRO DE USUARIO";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", Se informa que el registro de conciliacion HC fue creado a [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                        "<h4>DATOS DEL USUARIO REGISTRADO</h2>" + "<br/>" +
                                        "Nombre: " + ObjConciliacionHC.RazonSocial + "<br/>" +
                                        "Dirección: " + ObjConciliacionHC.DireccionCliente + "<br/>" +
                                        "Celular: " + ObjConciliacionHC.TelefonoContacto + "<br/>" +
                                        "Email: " + ObjConciliacionHC.EmailContacto + "<br/>" +

                                        "Usuario: " + "_LoginUser" + "<br/>" +
                                        "Password: " + "_ClaveDinamica.ToString().Trim()" + "<br/>" +
                                        "<br/><br/>" +

                                        "<h4>INFORMACIÓN DE LA EMPRESA</h4>" + "<br/>" +
                                        "Empresa: " + FixedData.NameEmpresa + "<br/>" +
                                        "Dirección: " + this.Session["DireccionEmpresa"].ToString().Trim() + "<br/>" +
                                        "Url Página: <a href=" + FixedData.PlatformUrlPagina + ">" + FixedData.PlatformUrlPagina + "</a>" + "<br/>" +
                                        "Servicio de atención al cliente : " + this.Session["TelefonoEmpresa"].ToString().Trim() + "<br/>" +
                                        "<br/><br/>" +
                                        "Si presenta algun problema o duda de como ingresar al sistema el Administrador del sistema le atendera y ayudara en sus comentarios." + "<br/>" +
                                        "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                        ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                        string _MsgErrorEmail = "";
                        if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                        {
                            _MsgError = _MsgError;//+ " Pero " + _MsgErrorEmail.ToString().Trim();
                        }
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "CLIENTE";
                        //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                        if (ObjConciliacionHC.TipoProceso == 1)
                        {
                            ObjAuditoria.IdTipoEvento = 2;
                        }
                        else
                        {
                            ObjAuditoria.IdTipoEvento = 3;
                        }
                        ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        ObjAuditoria.DescripcionEvento = "Se define el response del servicio"; //jsonRequest;
                        ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                        ObjAuditoria.TipoProceso = 1;

                        //'Agregar Auditoria del sistema
                        string _MsgErrorLogs = "";
                        if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                        {
                            _log.Error(_MsgErrorLogs);
                        }
                        #endregion
                    }


                    this.UpdatePanel1.Update();
                    //this.UpdatePanel3.Update();
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
                    _log.Info(_MsgError);
                    #endregion
                }
                else
                {
                    this.UpdatePanel1.Update();
                    //this.UpdatePanel3.Update();
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
                    _log.Error(_MsgError);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                this.UpdatePanel1.Update();
                //this.UpdatePanel3.Update();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al guardar los datos de la conciliacion. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void BtnEjecProceso_Click(object sender, EventArgs e)
        {
            try
            {
                int _TipoPeriodicidad = Int32.Parse(this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim());
                int _Periodo = Int32.Parse(this.CmbPeriodo.SelectedValue.ToString().Trim());
                ObjProcessAPI.AnioProcesar = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                ObjProcessAPI.MesProcesar = Int32.Parse(this.CmbPeriodo.SelectedValue.ToString().Trim());
                string _DataTarea = this.CmbAnioGravable.SelectedItem.Text.ToString().Trim() + "_" + this.CmbTipoPeriodicidad.SelectedItem.Text.ToString().Trim() + "_" + this.CmbPeriodo.SelectedValue.ToString().Trim();

                string _Mensaje = "";
                if (ObjProcessAPI.GetDownloadFileDavibox(_TipoPeriodicidad, _Periodo, ref _Mensaje))
                {
                    //--PROCESO EXITOSO MANDAMOS A CREAR LA TAREA PROGRAMADA
                    //--
                    int _TipoProceso = 5;
                    string _NombreTarea = "FILE_DAVIBOX_" + _DataTarea;
                    string _MsgError = "";
                    DeleteTaskSchedulerManual(_NombreTarea, ref _MsgError);
                    //--
                    if (CreateTaskSchedulerManual(_TipoProceso, _Periodo, _NombreTarea, ref _MsgError))
                    {
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "TASK_FILE_DAVIBOX";
                        ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        ObjAuditoria.DescripcionEvento = _TipoProceso + "|" + 3 + "|" + _NombreTarea;
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
                        _Mensaje = "Señor usuario, el proceso de la tarea programada fue creada de forma exitosa dentro algunos minutos le estara llegando un correo con la confirmación de que el proceso ha terminado. !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(650);
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
                        this.UpdatePanel1.Update();
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
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    this.UpdatePanel1.Update();
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
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al procesar el servicio. Motivo: " + ex.ToString();
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

        #region DEFINICION DE METODOS PARA CREAR Y ELIMINAR TAREAS PROGRAMADAS
        private bool CreateTaskSchedulerManual(int _TipoProceso, int _Periodo, string _NombreTarea, ref string _MsgError)
        {
            bool Result = false;
            string _NombreProveedor = "";
            try
            {
                //Obtener el servicio en la máquina local
                using (TaskService ts = new TaskService())
                {
                    //--AQUI DEFINIMOS LA FECHA CUANDO SE VA A EJECUTAR EL PROCESO
                    DateTime dtFechaInicio = DateTime.Now;
                    //Crear una nueva definición de tareas y asignar propiedades
                    TaskDefinition td = ts.NewTask();
                    //--AQUI VALIDAMOS EL TIPO DE PROCESO: 1. BASE GRAVABLE, 2. LIQUIDACION POR OFICINAS
                    td.RegistrationInfo.Description = "Tarea Programada de forma Manual para generar el proceso del servicio de davibox";

                    WeeklyTrigger wt = new WeeklyTrigger();
                    string _HoraGenerarTask = DateTime.Now.ToString("HH") + ":" + DateTime.Now.AddMinutes(Int32.Parse(FixedData.MinutosEjecutarTasks.ToString().Trim())).ToString("mm");
                    string[] ArrayHoraGeneracion = _HoraGenerarTask.Split(':');
                    wt.StartBoundary = new DateTime(dtFechaInicio.Year, dtFechaInicio.Month, dtFechaInicio.Day, int.Parse(ArrayHoraGeneracion[0].ToString().Trim()), int.Parse(ArrayHoraGeneracion[1].ToString().Trim()), 0);
                    _log.Error("LA TAREA PROGRAMADA [" + _NombreTarea + "] A LAS => " + _HoraGenerarTask);
                    //--
                    #region AQUI TOMAMOS LOS DIAS DE EJECUCION DE LA TAREA PROGRAMADA
                    //--AQUI OBTENEMOS EL NUMERO DE DIA DE LA SEMANA
                    DateTime dateValue = new DateTime(Int32.Parse(DateTime.Now.ToString("yyyy")), Int32.Parse(DateTime.Now.ToString("MM")), Int32.Parse(DateTime.Now.ToString("dd")));
                    int _NumroDiaSemana = (int)dateValue.DayOfWeek;
                    //--
                    switch (_NumroDiaSemana)
                    {
                        case 1:
                            wt.DaysOfWeek = DaysOfTheWeek.Monday;
                            break;
                        case 2:
                            wt.DaysOfWeek = DaysOfTheWeek.Tuesday;
                            break;
                        case 3:
                            wt.DaysOfWeek = DaysOfTheWeek.Wednesday;
                            break;
                        case 4:
                            wt.DaysOfWeek = DaysOfTheWeek.Thursday;
                            break;
                        case 5:
                            wt.DaysOfWeek = DaysOfTheWeek.Friday;
                            break;
                        case 6:
                            wt.DaysOfWeek = DaysOfTheWeek.Saturday;
                            break;
                        case 7:
                            wt.DaysOfWeek = DaysOfTheWeek.Sunday;
                            break;
                    }
                    #endregion

                    wt.WeeksInterval = 1;
                    wt.Repetition.Duration = new TimeSpan(0, 0, 0);
                    wt.Repetition.Interval = TimeSpan.FromMinutes(0);
                    td.Triggers.Add(wt);

                    #region AQUI OBTENEMOS EL MES ANTERIOR PARA EL BIMESTRE
                    //--
                    int _PrevMonth = 0;
                    switch (_Periodo)
                    {
                        case 1:
                            _PrevMonth = 1;
                            break;
                        case 2:
                            _PrevMonth = 3;
                            break;
                        case 3:
                            _PrevMonth = 5;
                            break;
                        case 4:
                            _PrevMonth = 7;
                            break;
                        case 5:
                            _PrevMonth = 9;
                            break;
                        case 6:
                            _PrevMonth = 11;
                            break;
                        default:
                            _PrevMonth = (_Periodo - 1);
                            break;
                    }
                    #endregion

                    int _IdCliente = Int32.Parse(this.Session["IdCliente"].ToString().Trim());
                    int _IdTipoPeriodicidad = Int32.Parse(this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim());
                    int _AnioGravable = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                    string _MesEf = this.CmbPeriodo.SelectedValue.ToString().Trim();
                    string _VersionEf = _PrevMonth.ToString();
                    int _IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    //--
                    td.Actions.Add(new ExecAction(FixedData.PathTasksProgramadas, _TipoProceso + " " + _NombreTarea + " " + _IdCliente + " " + _AnioGravable + " " + _MesEf + " " + _IdTipoPeriodicidad + " " + _VersionEf + " " + _IdUsuario));
                    ts.RootFolder.RegisterTaskDefinition(_NombreTarea.ToString(), td, TaskCreation.CreateOrUpdate, FixedData.UserCreateTasks, FixedData.PassCreateTasks);
                    Result = true;
                    _MsgError = "";
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "Error al generar la tarea programa de forma manual del proveedor [" + _NombreProveedor + "]. Motivo: " + ex.ToString();
                _log.Error(_MsgError);
            }

            return Result;
        }

        private bool DeleteTaskSchedulerManual(string _NombreTarea, ref string _MsgError)
        {
            bool Result = false;
            try
            {
                using (TaskService ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask(_NombreTarea.ToString().Trim());
                    _MsgError = "";
                    Result = true;
                    _MsgError = "Señor usuario, la tarea programada [" + _NombreTarea + "] ha sido borrada del sistema de forma exitosa.";
                    _log.Info(_MsgError);
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "Señor usuario, ocurrio un error al borrar la tarea programada [" + _NombreTarea + "] del sistema. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }

            return Result;
        }
        #endregion

    }
}