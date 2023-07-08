using System;
using System.Web;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using log4net;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Parametros.Tipos
{
    public partial class FrmLoadPlanUnicoCuentas : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        const string quote = "\"";

        #region DEFINICION DE OBJETOS DE CLASES
        PlanUnicoCuenta ObjPuc = new PlanUnicoCuenta();
        Lista ObjLista = new Lista();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        protected void LstTipoCaracter()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbTipoCaracter.DataSource = ObjLista.GetTipoCaracter();
                this.CmbTipoCaracter.DataValueField = "id_caracter";
                this.CmbTipoCaracter.DataTextField = "tipo_caracter";
                this.CmbTipoCaracter.DataBind();
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
            if (!objPermiso.PuedeRegistrar)
            {
                //this.BtnLoadFile.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                //this.Aviso1.OcultarAviso();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                this.LstTipoCaracter();

                //--Aqui capturamos los parametros enviados.
                //this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();

                //Limpiamos los datos en el DataTable.
                this.ViewState["DtPlanUnicoCuenta"] = null;
                this.ViewState["Contador"] = 0;

                string _DirectorioVirtual = HttpContext.Current.Server.MapPath("/" + Session["DirectorioVirtual"].ToString().Trim() + "/");
                this.ViewState["_PathDirectorio"] = _DirectorioVirtual + "//" + "CARGUES_MASIVO";

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

        private DataTable GetTablaDatos()
        {
            DataTable dtDatos = new DataTable();
            try
            {
                DataTable DtPlanUnicoCuenta = new DataTable();
                DtPlanUnicoCuenta = (DataTable)this.ViewState["DtPlanUnicoCuenta"];
                if (DtPlanUnicoCuenta != null)
                {
                    dtDatos = DtPlanUnicoCuenta.Copy();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtPlanUnicoCuenta = new DataTable();
                    DtPlanUnicoCuenta.TableName = "DtPlanUnicoCuenta";
                    DtPlanUnicoCuenta.Columns.Add("id_registro", typeof(Int32));
                    DtPlanUnicoCuenta.PrimaryKey = new DataColumn[] { DtPlanUnicoCuenta.Columns["id_registro"] };
                    DtPlanUnicoCuenta.Columns.Add("codigo_cuenta");
                    DtPlanUnicoCuenta.Columns.Add("cod_cuenta_padre");
                    DtPlanUnicoCuenta.Columns.Add("nombre_cuenta");
                    DtPlanUnicoCuenta.Columns.Add("idbase_gravable");
                    DtPlanUnicoCuenta.Columns.Add("base_gravable");
                    DtPlanUnicoCuenta.Columns.Add("id_movimiento");
                    DtPlanUnicoCuenta.Columns.Add("movimiento");
                    DtPlanUnicoCuenta.Columns.Add("idtipo_nivel");
                    DtPlanUnicoCuenta.Columns.Add("idtipo_naturaleza");
                    DtPlanUnicoCuenta.Columns.Add("idtipo_moneda");
                    DtPlanUnicoCuenta.Columns.Add("id_cliente");
                    DtPlanUnicoCuenta.Columns.Add("id_estado");
                    DtPlanUnicoCuenta.Columns.Add("estado");
                    dtDatos = DtPlanUnicoCuenta.Copy();
                }

                this.ViewState["DtPlanUnicoCuenta"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtPlanUnicoCuenta"] = dtDatos;
                _log.Error("Error al generar Tabla. Motivo: " + ex.Message);
            }

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.GetTablaDatos();
                RadGrid1.DataMember = "DtPlanUnicoCuenta";
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
                string _MsgError = "Error con el evento NeedDataSource. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

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
                string _MsgError = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            DataTable DtPlanUnicoCuenta = new DataTable();
            try
            {
                //Información del directorio donde se guardaran los archivos.
                string _NombreArchivo = this.FileExaminar.FileName.ToString().Trim();
                string _PathDirectorio = this.ViewState["_PathDirectorio"].ToString().Trim() + "/" + _NombreArchivo;
                //Aqui obtenemos la extension del archivo y la validamos con el Array de Extensiones soportadas por el sistema.
                string _TipoExtension = Path.GetExtension(_PathDirectorio).ToString().ToUpper();
                //--VALIDAMOS EL SEPARADOR DEL ARCHIVO
                char[] _TipoSeparador = null;
                if (this.CmbTipoCaracter.SelectedValue.ToString().Trim().Equals("1"))
                {
                    _TipoSeparador = new char[] { ';' };
                }
                else if (this.CmbTipoCaracter.SelectedValue.ToString().Trim().Equals("2"))
                {
                    _TipoSeparador = new char[] { '|' };
                }
                else if (this.CmbTipoCaracter.SelectedValue.ToString().Trim().Equals("3"))
                {
                    _TipoSeparador = new char[] { '\t' };
                }

                if (FixedData.GetListExtensionesBac.Contains(_TipoExtension.ToString()))
                {
                    //Eliminamos el archivo del directorio
                    File.Delete(_PathDirectorio);

                    //Guardamos el archivo en el directorio
                    this.FileExaminar.SaveAs(_PathDirectorio);

                    string _MsgError = "";
                    DataTable dtEtl = new DataTable();
                    dtEtl = ObjUtils.GetEtl(_PathDirectorio, _TipoSeparador, ref _MsgError);

                    if (dtEtl != null)
                    {
                        if (dtEtl.Rows.Count > 0)
                        {
                            int ContadorRow = dtEtl.Rows.Count;
                            DtPlanUnicoCuenta = new DataTable();
                            DtPlanUnicoCuenta = (DataTable)this.ViewState["DtPlanUnicoCuenta"];

                            //Recorremos las filas del Datatable
                            foreach (DataRow rowItem in dtEtl.Rows)
                            {
                                #region OBTENER DATOS DEL ARCHIVO PARA EL DATATABLE
                                DataRow Fila = null;
                                Fila = DtPlanUnicoCuenta.NewRow();
                                //--
                                Fila["id_registro"] = DtPlanUnicoCuenta.Rows.Count + 1;
                                Fila["codigo_cuenta"] = rowItem["codigo_cuenta"].ToString().Trim();
                                Fila["cod_cuenta_padre"] = rowItem["cod_cuenta_padre"].ToString().Trim();
                                Fila["nombre_cuenta"] = ObjUtils.GetLimpiarCadena(rowItem["nombre_cuenta"].ToString().Trim());
                                Fila["idbase_gravable"] = rowItem["ID_BASE GRAVABLE"].ToString().Trim();
                                Fila["base_gravable"] = rowItem["BASE_GRAVABLE"].ToString().Trim();
                                Fila["id_movimiento"] = rowItem["ID_MOVIMIENTO"].ToString().Trim();
                                Fila["movimiento"] = rowItem["movimiento"].ToString().Trim();
                                Fila["idtipo_nivel"] = rowItem["idtipo_nivel"].ToString().Trim();
                                Fila["idtipo_naturaleza"] = rowItem["idtipo_naturaleza"].ToString().Trim();
                                Fila["idtipo_moneda"] = rowItem["idtipo_moneda"].ToString().Trim();
                                Fila["id_cliente"] = rowItem["id_cliente"].ToString().Trim();
                                Fila["id_estado"] = rowItem["id_estado"].ToString().Trim();
                                Fila["estado"] = rowItem["ESTADO"].ToString().Trim();
                                DtPlanUnicoCuenta.Rows.Add(Fila);
                                #endregion
                            }

                            //Aqui mandamos a mostrar en el Grid la Información.
                            if (DtPlanUnicoCuenta.Rows.Count > 0)
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
                                _MsgError = "El Proceso del archivo ha sido realizado correctamente con [" + DtPlanUnicoCuenta.Rows.Count + "] registros.";
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
                                //--
                                this.BtnCargar.Visible = false;
                                this.BtnProcesar.Visible = true;
                                this.BtnCancelar.Visible = true;
                                this.RadGrid1.Rebind();
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
                            _MsgError = "Señor usuario, no se encontró información de establecimientos para cargar al sistema. Por favor validar el separador del archivo e intente nuevamente !";
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
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor Usuario. La Extensión del archivo de los Subsidios no es valido. Debe ser [.TXT, .CSV]";
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
                DtPlanUnicoCuenta = null;
                this.ViewState["DtPlanUnicoCuenta"] = null;
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Señor Usuario. Ocurrio un error al leer datos del archivo. Motivo: " + ex.Message;
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
                //Aqui limpiamos los datos del Grid.
                this.RadGrid1.Rebind();
                #endregion
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.ViewState["DtPlanUnicoCuenta"] = null;
            this.BtnCargar.Visible = true;
            this.BtnProcesar.Visible = false;
            this.BtnCancelar.Visible = false;
            this.RadGrid1.Rebind();
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtPlanUnicoCuenta = new DataTable();
            try
            {
                DtPlanUnicoCuenta = (DataTable)this.ViewState["DtPlanUnicoCuenta"];
                if (DtPlanUnicoCuenta != null)
                {
                    if (DtPlanUnicoCuenta.Rows.Count > 0)
                    {
                        int ContadorRow = DtPlanUnicoCuenta.Rows.Count;
                        ObjPuc.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                        ObjPuc.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjPuc.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        string _ArrayPuc = "";
                        int _ContadorRow = 0;
                        foreach (DataRow rowItem in DtPlanUnicoCuenta.Rows)
                        {
                            #region OBTENER DATOS DEL DATATABLE
                            string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                            string _CodCuentaPadre = rowItem["cod_cuenta_padre"].ToString().Trim();
                            string _NombreCuenta = rowItem["nombre_cuenta"].ToString().Trim();
                            string _BaseGravable = rowItem["idbase_gravable"].ToString().Trim();
                            string _Movimiento = rowItem["id_movimiento"].ToString().Trim();
                            int _IdTipoNivel = Int32.Parse(rowItem["idtipo_nivel"].ToString().Trim());
                            int _IdTipoNaturaleza = Int32.Parse(rowItem["idtipo_naturaleza"].ToString().Trim());
                            int _IdTipoMoneda = Int32.Parse(rowItem["idtipo_moneda"].ToString().Trim());
                            int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                            int _IdEstado = Int32.Parse(rowItem["id_estado"].ToString().Trim());

                            //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                            if (_ArrayPuc.ToString().Trim().Length > 0)
                            {
                                _ArrayPuc = _ArrayPuc.ToString().Trim() + "," + quote + "(" + _CodigoCuenta + "," + _CodCuentaPadre + "," + _NombreCuenta + "," + _BaseGravable + "," + _Movimiento + "," + _IdTipoNivel + "," + _IdTipoNaturaleza + "," + _IdTipoMoneda + "," + _IdCliente + "," + _IdEstado + ")" + quote;
                            }
                            else
                            {
                                _ArrayPuc = quote + "(" + _CodigoCuenta + "," + _CodCuentaPadre + "," + _NombreCuenta + "," + _BaseGravable + "," + _Movimiento + "," + _IdTipoNivel + "," + _IdTipoNaturaleza + "," + _IdTipoMoneda + "," + _IdCliente + "," + _IdEstado + ")" + quote;
                            }

                            //--
                            _ContadorRow++;
                            this.ViewState["Contador"] = _ContadorRow;
                            #endregion
                        }

                        if (_ArrayPuc.ToString().Trim().Length > 0)
                        {
                            ObjPuc.ArrayPuc = _ArrayPuc.ToString().Trim();
                            int _IdRegistro = 0;
                            string _MsgError = "";
                            if (ObjPuc.GetLoadPuc(ref _IdRegistro, ref _MsgError))
                            {
                                #region MOSTRAR MENSAJE DE USUARIO
                                DtPlanUnicoCuenta = null;
                                this.ViewState["DtPlanUnicoCuenta"] = null;
                                this.CmbTipoCaracter.SelectedValue = "";
                                this.BtnCargar.Visible = true;
                                this.BtnProcesar.Visible = false;
                                this.BtnCancelar.Visible = false;

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
                                string _Mensaje = _MsgError + " Por favor revise la información del Archivo o Contacte a soporte técnico !";
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
                        string _MsgError = "Señor Usuario. No hay información para guardar al Sistema. !";
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
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor Usuario. No hay información para guardar al Sistema. !";
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
                string _MsgError = "Señor Usuario. Ocurrio un error en la linea [" + this.ViewState["Contador"].ToString().Trim() + "] al guadar la información. Motivo: " + ex.Message;
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

    }
}