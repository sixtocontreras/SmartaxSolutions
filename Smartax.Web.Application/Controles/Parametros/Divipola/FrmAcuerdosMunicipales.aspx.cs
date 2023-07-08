using System;
using System.Web;
using System.Data;
using System.IO;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmAcuerdosMunicipales : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        AcuerdosMunicipales ObjAcuerdo = new AcuerdosMunicipales();
        TiposNormatividad ObjTipoNormatividad = new TiposNormatividad();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Lista ObjLista = new Lista();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjAcuerdo.TipoConsulta = 1;
                ObjAcuerdo.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjAcuerdo.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los impuestos por municipio
                ObjetoDataTable = ObjAcuerdo.GetAllAcuerdos();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idacuerdo_municipal, idtipo_normatividad"] };
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
                string _MsgError = "Error al listar los acuerdos. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
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

            return ObjetoDataSet;
        }

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
                Ventana.ID = "RadWindow2";
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

        protected void LstTipoNormatividad()
        {
            try
            {
                ObjTipoNormatividad.TipoConsulta = 2;
                ObjTipoNormatividad.IdEstado = 1;
                ObjTipoNormatividad.MostrarSeleccione = "SI";
                ObjTipoNormatividad.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
                ObjTipoNormatividad.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTipoNormatividad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoNormatividad.DataSource = ObjTipoNormatividad.GetTipoNormatividad();
                this.CmbTipoNormatividad.DataValueField = "idtipo_normatividad";
                this.CmbTipoNormatividad.DataTextField = "tipo_normatividad";
                this.CmbTipoNormatividad.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de normatividad. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
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

        private DataSet FuenteDatos
        {
            get
            {
                object obj = this.ViewState["_FuenteDatos"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetDatosGrilla();
                    this.ViewState["_FuenteDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatos"];
                }
            }
            set { this.ViewState["_FuenteDatos"] = value; }
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
            if (!objPermiso.PuedeLeer)
            {
                this.RadGrid1.Visible = false;
            }
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnCargar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                //--Listar los ComboBox
                this.LstTipoImpuesto();
                this.LstTipoNormatividad();
                string _AnioAcuerdo = DateTime.Now.ToString("yyyy");

                string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                this.ViewState["_PathDirectorio"] = _DirectorioVirtual + "\\" + "ACUERDOS_MUNICIPALES" + "\\" + "MUNICIPIO_" + this.ViewState["IdMunicipio"].ToString().Trim() + "\\" + _AnioAcuerdo;

                if (!Directory.Exists(this.ViewState["_PathDirectorio"].ToString().Trim()))
                {
                    //Creamos el directorio sino existe
                    Directory.CreateDirectory(this.ViewState["_PathDirectorio"].ToString().Trim());
                }
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
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

        #region DEFINICION DE EVENTOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtAcuerdos";
            }
            catch (Exception ex)
            {
                this.LblMensaje.Text = "Error al Intentar Cargar el NeedDataSource, Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnQuitar")
                {
                    #region DEFINICION DEL METODO PARA ELIMINAR EL ARCHIVO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdAcuerdo = Convert.ToInt32(item.GetDataKeyValue("idacuerdo_municipal").ToString().Trim());
                    string _IdTipoNormatividad = item.GetDataKeyValue("idtipo_normatividad").ToString().Trim();
                    string _NombreOrigFile = item["nombre_original_archivo"].Text.ToString().Trim();
                    string _NombreFile = item["nombre_archivo"].Text.ToString().Trim();
                    string FechaAcuerdo = item["fecha_acuerdo"].Text.ToString().Trim();
                    string _AnioAcuerdo = Convert.ToDateTime(FechaAcuerdo).ToString("yyyy");

                    ObjAcuerdo.IdAcuerdoMunicipal = _IdAcuerdo;
                    ObjAcuerdo.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    ObjAcuerdo.IdTipoNormatividad = _IdTipoNormatividad.ToString().Trim();
                    ObjAcuerdo.NombreOrigArchivo = _NombreOrigFile.ToString().Trim();
                    ObjAcuerdo.NombreArchivo = _NombreFile.ToString().Trim();
                    ObjAcuerdo.FechaAcuerdo = FechaAcuerdo;
                    ObjAcuerdo.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAcuerdo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAcuerdo.TipoProceso = 3;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjAcuerdo);
                    _log.Warn("REQUEST DELETE ACUERDOS MUNICIPALES => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjAcuerdo.AddUpAcuerdo(ref _IdRegistro, ref _MsgError))
                    {
                        #region AQUI OBTENEMOS EL DIRECTORIO PARA BORRAR EL ARCHIVO
                        string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                        this.ViewState["_PathDirectorio"] = _DirectorioVirtual + "\\" + "ACUERDOS_MUNICIPALES" + "\\" + "MUNICIPIO_" + this.ViewState["IdMunicipio"].ToString().Trim() + "\\" + _AnioAcuerdo;

                        if (Directory.Exists(this.ViewState["_PathDirectorio"].ToString().Trim()))
                        {
                            //Creamos el directorio sino existe
                            File.Delete(this.ViewState["_PathDirectorio"].ToString().Trim() + "\\" + _NombreFile);
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
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

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                        ObjAuditoria.ModuloApp = "ACUERDOS_MUNICIPALES";
                        ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        ObjAuditoria.DescripcionEvento = jsonRequest;
                        ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                        ObjAuditoria.TipoProceso = 1;

                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        string _MsgErrorLogs = "";
                        if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                        {
                            _log.Error(_MsgErrorLogs);
                        }
                        #endregion

                        //--Aqui actualizamos el grid
                        this.ViewState["_FuenteDatos"] = null;
                        this.RadGrid1.Rebind();
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
                        Ventana.ID = "RadWindow2";
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
                    #endregion
                }
                else if (e.CommandName == "BtnDownload")
                {
                    #region DEFINICION DE METODO PARA DESCARGAR ARCHIVOS
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdAcuerdo = Convert.ToInt32(item.GetDataKeyValue("idacuerdo_municipal").ToString().Trim());
                    string _IdTipoNormatividad = item.GetDataKeyValue("idtipo_normatividad").ToString().Trim();
                    string _NombreFile = item["nombre_archivo"].Text.ToString().Trim();
                    string _AnioAcuerdo = Convert.ToDateTime(item["fecha_registro"].Text.ToString().Trim()).ToString("yyyy");
                    //--AQUI OBTENEMOS EL DIRECTORIO DEL ARCHIVO A DESCARGAR
                    //string _RutaArchivo = this.ViewState["_PathDirectorio"].ToString().Trim() + "\\" + _NombreFile;
                    string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                    string _PathDirectorio = _DirectorioVirtual + "\\" + "ACUERDOS_MUNICIPALES" + "\\" + "MUNICIPIO_" + this.ViewState["IdMunicipio"].ToString().Trim() + "\\" + _AnioAcuerdo + "\\" + _NombreFile;
                    _log.Info("DIRECTORIO DESCARGAR ARCHIVO => " + _PathDirectorio);

                    ///--Aqui se inicia con la descarga del archivo
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    String Header = "Attachment; Filename=" + _NombreFile;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", Header);
                    FileInfo Dfile = new FileInfo(_PathDirectorio);
                    HttpContext.Current.Response.WriteFile(Dfile.FullName);
                    HttpContext.Current.Response.End();
                    #endregion
                }
                else if (e.CommandName == "BtnLogsAuditoria")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EDITAR IMPUESTO
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _ModuloApp = "ACUERDOS_MUNICIPALES";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(500);
                    Ventana.Width = Unit.Pixel(980);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Detalle Logs de Auditoria. Modulo: " + _ModuloApp;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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
                string _MsgError = "Error con el proceso del archivo. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
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
                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                this.LblMensaje.Text = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }
        }
        #endregion

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                string _NombreArchivo = HttpContext.Current.Server.UrlDecode(FileExaminar.FileName.ToString().Trim());
                string _NombreFile = _NombreArchivo;    //--FileExaminar.FileName.ToString().Trim();
                //string _AnioAcuerdo = Convert.ToDateTime(this.DtFechaAcuerdo.SelectedDate.ToString()).ToString("yyyy");

                if (_NombreFile.ToString().Trim().Length > 0)
                {
                    string _TipoExtension = Path.GetExtension(_NombreFile).ToString().ToUpper();
                    //--AQUI CREAMOS EL DIRECTORIO DONDE SE VA A GUARDAR EL ARCHIVO
                    string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                    string _PathDirectorio = this.ViewState["_PathDirectorio"].ToString().Trim();

                    if (!Directory.Exists(this.ViewState["_PathDirectorio"].ToString().Trim()))
                    {
                        //Creamos el directorio sino existe
                        Directory.CreateDirectory(this.ViewState["_PathDirectorio"].ToString().Trim());
                    }

                    this.ViewState["_PathDirectorio"] = this.ViewState["_PathDirectorio"].ToString().Trim() + "\\" + ObjUtils.GetLimpiarCadena2(_NombreFile.ToString().Trim().ToUpper());
                    if (File.Exists(this.ViewState["_PathDirectorio"].ToString().Trim()))
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
                        string _MsgError = "Señor usuario ya existe un archivo con este mismo nombre en el repositorio ..!";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
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
                    else
                    {
                        #region OBTENER DATOS PARA EL REGISTRO DE LA INFORMACION
                        ObjAcuerdo.IdAcuerdoMunicipal = null;
                        ObjAcuerdo.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                        ObjAcuerdo.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                        string _TipoNormatividad = this.CmbTipoNormatividad.SelectedItem.Text.ToString().Trim();
                        ObjAcuerdo.IdTipoNormatividad = this.CmbTipoNormatividad.SelectedValue.ToString().Trim();
                        //ObjAcuerdo.NombreOrigArchivo = ObjUtils.GetLimpiarCadena2(_NombreFile.ToString().Trim());
                        //ObjAcuerdo.NombreArchivo = _TipoNormatividad + "_" + _TipoExtension;
                        ObjAcuerdo.NombreOrigArchivo = ObjUtils.GetLimpiarCadena2(_NombreFile.ToString().Trim().ToUpper());
                        ObjAcuerdo.NombreArchivo = ObjUtils.GetLimpiarCadena2(_NombreFile.ToString().Trim().ToUpper());
                        ObjAcuerdo.FechaAcuerdo = DateTime.Parse(this.DtFechaAcuerdo.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                        ObjAcuerdo.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjAcuerdo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAcuerdo.TipoProceso = 1;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjAcuerdo);
                        _log.Warn("REQUEST ADD ACUERDOS MUNICIPALES => " + jsonRequest);

                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjAcuerdo.AddUpAcuerdo(ref _IdRegistro, ref _MsgError))
                        {
                            //string _NombreArchivoNew = _TipoNormatividad + "_" + _IdRegistro + _TipoExtension;
                            //string _PathNewFile = _PathDirectorio + "\\" + _NombreArchivoNew;
                            //this.FileExaminar.SaveAs(_PathNewFile.ToString().Trim());
                            ////--AQUI SE RENOMBRA EL NUEVO NOMBRE DEL ARCHIVO
                            //File.Move(_PathNewFile, _PathNewFile);

                            //ObjAcuerdo.IdAcuerdoMunicipal = _IdRegistro;
                            //ObjAcuerdo.NombreArchivo = _NombreArchivoNew;
                            //ObjAcuerdo.TipoProceso = 2;
                            //if (ObjAcuerdo.AddUpAcuerdo(ref _IdRegistro, ref _MsgError))
                            //{

                            //}

                            FileExaminar.SaveAs(this.ViewState["_PathDirectorio"].ToString().Trim());
                            //--Aqui actualizamos el grid
                            this.ViewState["_FuenteDatos"] = null;
                            this.RadGrid1.Rebind();
                            this.CmbTipoNormatividad.SelectedValue = "";

                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            if (ObjAcuerdo.TipoProceso == 1)
                            {
                                ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            }
                            else
                            {
                                ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                            }
                            ObjAuditoria.ModuloApp = "ACUERDOS_MUNICIPALES";
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
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                            Ventana.ID = "RadWindow2";
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
                            Ventana.ID = "RadWindow2";
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
                    string _MsgError = "Señor usuario debe seleccionar un archivo para cargar ..!";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow2";
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
                string _MsgError = "Error al cargar el archivo. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
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

    }
}