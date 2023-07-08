using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Drawing;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddFirma : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        Firmantes ObjFirmante = new Firmantes();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetImagenFirmante()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar imagen del modulo
                ObjFirmante.TipoConsulta = 3;
                ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjFirmante.IdFirmante = this.ViewState["IdFirmante"].ToString().Trim();
                ObjFirmante.IdEstado = null;
                ObjFirmante.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                ObjetoDataTable = ObjFirmante.GetImagenFirma();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_firmante, nombre_archivo"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                if (ObjetoDataTable != null)
                {
                    if (ObjetoDataTable.Rows.Count > 0)
                    {
                        this.BtnAdicionar.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al listar la imagen. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }

            return ObjetoDataSet;
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
                    ConjuntoDatos = GetImagenFirmante();
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
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnAdicionar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                //Informacion de la empresa seleccionada
                this.ViewState["IdFirmante"] = Request.QueryString["IdFirmante"].ToString().Trim();
                this.ViewState["IdRol"] = Request.QueryString["IdRol"].ToString().Trim();
            }
        }

        #region DEFINICION DE EVENTOS DE LA PAGINA
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

        #region DEFINICION DE EVENTOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatos;
                RadGrid1.DataMember = "DtImagenFirma";
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el NeedDataSource, Motivo: " + ex.ToString();
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnEliminar")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdFirmante = Convert.ToInt32(item.GetDataKeyValue("id_firmante").ToString().Trim());
                    string _NombreArchivo = item.GetDataKeyValue("nombre_archivo").ToString().Trim();

                    string _RutaVirtual = HttpContext.Current.Server.MapPath("\\" + Session["DirectorioVirtual"].ToString().Trim() + "\\");
                    string _PathDirectorio = _RutaVirtual + "IMAGEN_FIRMA\\" + _NombreArchivo;

                    ObjFirmante.IdFirmante = _IdFirmante;
                    ObjFirmante.IdTipoIdentificacion = -1;
                    ObjFirmante.NumeroDocumento = "";
                    ObjFirmante.NombreFuncionario = "";
                    ObjFirmante.ApellidoFuncionario = "";
                    ObjFirmante.NumeroContacto = "";
                    ObjFirmante.EmailContacto = "";
                    ObjFirmante.ImagenFirma = null;
                    ObjFirmante.NombreArchivo = null;
                    ObjFirmante.IdRol = Int32.Parse(this.ViewState["IdRol"].ToString().Trim());
                    ObjFirmante.PasswordUsuario = "";
                    ObjFirmante.IdEstado = 1;
                    ObjFirmante.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                    ObjFirmante.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjFirmante.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                    ObjFirmante.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjFirmante.TipoProceso = 3;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjFirmante);

                    string _MsgError = "";
                    int _IdRegistro = 0;
                    if (ObjFirmante.AddUpFirmante(ref _IdRegistro, ref _MsgError))
                    {
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "IMAGEN_FIRMA";
                        //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                        ObjAuditoria.IdTipoEvento = 4;
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

                        this.BtnAdicionar.Enabled = true;
                        this.LblMensaje.ForeColor = Color.Black;
                        this.LblMensaje.Text = _MsgError;
                        _log.Info(_MsgError);

                        if (File.Exists(_PathDirectorio.ToString().Trim()))
                        {
                            File.Delete(_PathDirectorio.ToString().Trim());
                        }

                        this.ViewState["_FuenteDatos"] = null;
                        this.RadGrid1.Rebind();
                    }
                    else
                    {
                        this.LblMensaje.ForeColor = Color.Red;
                        this.LblMensaje.Text = _MsgError;
                        _log.Error(_MsgError);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al Cargar el ItemCommand. Motivo: " + ex.ToString();
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
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al Cargar el PageIndexChanged. Motivo: " + ex.ToString();
            }
        }
        #endregion

        protected void BtnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                string _RutaVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                string cPathDirectorio = _RutaVirtual + "IMAGEN_FIRMA\\";

                string _RutaFile = cPathDirectorio.ToString().Trim();
                string _NombreFile = FileExaminar.FileName.ToString().Trim();

                if (_NombreFile.ToString().Trim().Length > 0)
                {
                    this.ViewState["strPath"] = cPathDirectorio.ToString().Trim() + _NombreFile.ToString().Trim();
                    if (Directory.Exists(cPathDirectorio.ToString().Trim()))
                    {
                        FileExaminar.SaveAs(this.ViewState["strPath"].ToString().Trim());
                    }
                    else
                    {
                        Directory.CreateDirectory(cPathDirectorio.ToString().Trim());
                        FileExaminar.SaveAs(this.ViewState["strPath"].ToString().Trim());
                    }

                    //Aqui Hacemos el Llamado del Método GetToImagen para retornar un Objeto Image                             
                    System.Drawing.Image imgParametro = null;
                    byte[] ImagenBytes = ObjUtils.GetImagenBytes(this.ViewState["strPath"].ToString().Trim());
                    imgParametro = ObjUtils.GetByteImagen(ImagenBytes);

                    if (imgParametro != null)
                    {
                        ObjFirmante.IdFirmante = this.ViewState["IdFirmante"].ToString().Trim();
                        ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                        ObjFirmante.IdTipoIdentificacion = -1;
                        ObjFirmante.NumeroDocumento = "";
                        ObjFirmante.NombreFuncionario = "";
                        ObjFirmante.ApellidoFuncionario = "";
                        ObjFirmante.NumeroContacto = "";
                        ObjFirmante.EmailContacto = "";
                        ObjFirmante.PermitirFirma = "S";
                        ObjFirmante.ImagenFirma = ImagenBytes;
                        ObjFirmante.NombreArchivo = _NombreFile;
                        ObjFirmante.IdRol = Int32.Parse( this.ViewState["IdRol"].ToString().Trim());
                        ObjFirmante.PasswordUsuario = "";
                        ObjFirmante.IdEstado = 1;
                        ObjFirmante.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                        ObjFirmante.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                        ObjFirmante.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                        ObjFirmante.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjFirmante.TipoProceso = 3;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjFirmante);

                        string _MsgError = "";
                        int _IdRegistro = 0;
                        if (ObjFirmante.AddUpFirmante(ref _IdRegistro, ref _MsgError))
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.ModuloApp = "IMAGEN_FIRMA";
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

                            //Aqui mandamos actualizar el Grid de las Imagenes
                            this.ViewState["_FuenteDatos"] = null;
                            this.RadGrid1.Rebind();

                            this.LblMensaje.ForeColor = Color.Black;
                            this.LblMensaje.Text = _MsgError;
                        }
                        else
                        {
                            this.LblMensaje.ForeColor = Color.Red;
                            this.LblMensaje.Text = _MsgError;
                        }
                    }
                }
                else
                {
                    this.LblMensaje.ForeColor = Color.Red;
                    this.LblMensaje.Text = "Debe seleccionar la imagen del Modulo para adicionar. !";
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al Cargar la imagen del Modulo. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text);
            }
        }

    }
}