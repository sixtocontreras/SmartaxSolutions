using System;
using System.Web;
using System.Data;
using System.IO;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Drawing;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmVerAcuerdosMunicipales : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        AcuerdosMunicipales ObjAcuerdo = new AcuerdosMunicipales();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //--OBTENER PARAMETROS
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
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
                    string _NombreFile = item["nombre_archivo"].Text.ToString().Trim();
                    string FechaAcuerdo = item["fecha_acuerdo"].Text.ToString().Trim();
                    string _AnioAcuerdo = Convert.ToDateTime(FechaAcuerdo).ToString("yyyy");

                    ObjAcuerdo.IdAcuerdoMunicipal = _IdAcuerdo;
                    ObjAcuerdo.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    ObjAcuerdo.IdTipoNormatividad = _IdTipoNormatividad.ToString().Trim();
                    ObjAcuerdo.NombreArchivo = _NombreFile.ToString().Trim();
                    ObjAcuerdo.FechaAcuerdo = FechaAcuerdo;
                    ObjAcuerdo.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAcuerdo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAcuerdo.TipoProceso = 3;
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
                    string _AnioAcuerdo = Convert.ToDateTime(item["fecha_acuerdo"].Text.ToString().Trim()).ToString("yyyy");
                    //--AQUI OBTENEMOS EL DIRECTORIO DEL ARCHIVO A DESCARGAR
                    //string _RutaArchivo = this.ViewState["_PathDirectorio"].ToString().Trim() + "\\" + _NombreFile;
                    string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                    string _PathDirectorio = _DirectorioVirtual + "\\" + "ACUERDOS_MUNICIPALES" + "\\" + "MUNICIPIO_" + this.ViewState["IdMunicipio"].ToString().Trim() + "\\" + _AnioAcuerdo + "\\" + _NombreFile;

                    ///--Aqui se inicia con la descarga del archivo
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    String Header = "Attachment; Filename=" + _NombreFile;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", Header);
                    FileInfo Dfile = new FileInfo(_PathDirectorio);
                    HttpContext.Current.Response.WriteFile(Dfile.FullName);
                    HttpContext.Current.Response.End();
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
                string _MsgError = "Error al eliminar el archivo. Motivo: " + ex.Message;
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

    }
}