using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmConfiguracionImpuestos : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        FormConfiguracion ObjFormConfig = new FormConfiguracion();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjFormConfig.TipoConsulta = 1;
                ObjFormConfig.IdFormularioImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                ObjFormConfig.IdEstado = 1;
                ObjFormConfig.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los impuestos por municipio
                ObjetoDataTable = ObjFormConfig.GetAllFormConfiguracion();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idformulario_configuracion"] };
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
                string _MsgError = "Error al listar configuracion del formulario. Motivo: " + ex.ToString();
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
                this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
            if (!objPermiso.PuedeEliminar)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }

            //Ocultar la columna de empresa siempre y cuando el Rol sea diferente al de Soporte
            if (Int32.Parse(Session["IdRol"].ToString().Trim()) != 1)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 6].Visible = false;
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 7].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                //--AQUI OBTENEMOS LOS PARAMETROS
                this.ViewState["IdClienteImpuesto"] = Request.QueryString["IdClienteImpuesto"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["NombreImpuesto"] = Request.QueryString["NombreImpuesto"].ToString().Trim();
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                //--TITULO DEL FORMULARIO
                this.LbTitulo.Text = "HOJA DE CONFIGURACIÓN DE " + this.ViewState["NombreImpuesto"].ToString().Trim();
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

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtFormConfiguracion";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del valor de la unidad. Motivo: " + ex.ToString();
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAddConfigurar")
                {
                    #region INGRESAR A CONFIGURACION DEL IMPUESTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdFormConfiguracion = Convert.ToInt32(item.GetDataKeyValue("idformulario_configuracion").ToString().Trim());
                    string _NumeroRenglon = item["numero_renglon"].Text.ToString().Trim();
                    string _DescripcionRenglon = item["descripcion_renglon"].Text.ToString().Trim();
                    CheckBox ChkRenglonCalculado = item["renglon_calculado"].Controls[0] as CheckBox;
                    //--
                    if (ChkRenglonCalculado.Checked == false)
                    {
                        #region AQUI MOSTRAMOS EL FORMULARIO DE CONFIGURACION
                        //--Mandamos abrir el formulario de registro
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdFormConfiguracion +
                            "&NumeroRenglon=" + _NumeroRenglon +
                            "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                            "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                            "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                            "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
                        Ventana.ID = "RadWindow12";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(620);
                        Ventana.Width = Unit.Pixel(1140);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
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
                        string _MsgMensaje = "Señor usuario, No se puede Configurar este renglon ya que es Calculado. Por favor validar e intentar nuevamente !";
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
                        _log.Error(_MsgMensaje.Trim());
                        #endregion
                    }
                    #endregion
                }
                else if (e.CommandName == "BtnCargueMasivo")
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

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmLoadConfiguracionImp.aspx?IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(620);
                    Ventana.Width = Unit.Pixel(1130);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Realizar Cargue Masivo de Configuración";
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
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje.Trim());
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del valor de la unidad. Motivo: " + ex.ToString();
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