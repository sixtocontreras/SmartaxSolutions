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
using Smartax.Web.Application.Clases.Parametros.Divipola;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmLoadBancoCuentas : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        const string quote = "\"";

        #region DEFINICION DE OBJETOS DE CLASES
        MunicipioBanco ObjMunicipioBanco = new MunicipioBanco();
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

                //Limpiamos los datos en el DataTable.
                this.ViewState["DtBancoCuentas"] = null;
                this.ViewState["Contador"] = 0;

                string _DirectorioVirtual = HttpContext.Current.Server.MapPath("/" + Session["DirectorioVirtual"].ToString().Trim() + "/");
                //this.ViewState["_PathDirectorio"] = _DirectorioVirtual + "/" + "IDEMPRESA_" + this.Session["IdEmpresa"].ToString().Trim() + "/" + "LOAD_CALENDARIO";
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
                DataTable DtBancoCuentas = new DataTable();
                DtBancoCuentas = (DataTable)this.ViewState["DtBancoCuentas"];
                if (DtBancoCuentas != null)
                {
                    dtDatos = DtBancoCuentas.Copy();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtBancoCuentas = new DataTable();
                    DtBancoCuentas.TableName = "DtBancoCuentas";
                    DtBancoCuentas.Columns.Add("id_registro", typeof(Int32));
                    DtBancoCuentas.PrimaryKey = new DataColumn[] { DtBancoCuentas.Columns["id_registro"] };
                    DtBancoCuentas.Columns.Add("id_municipio");
                    DtBancoCuentas.Columns.Add("municipio");
                    DtBancoCuentas.Columns.Add("id_banco");
                    DtBancoCuentas.Columns.Add("banco");
                    DtBancoCuentas.Columns.Add("idtipo_cuenta");
                    DtBancoCuentas.Columns.Add("tipo_cuenta");
                    DtBancoCuentas.Columns.Add("idmedio_pago");
                    DtBancoCuentas.Columns.Add("medio_pago");
                    DtBancoCuentas.Columns.Add("numero_cuenta");
                    DtBancoCuentas.Columns.Add("cheque");
                    DtBancoCuentas.Columns.Add("version_registro");
                    DtBancoCuentas.Columns.Add("id_estado");
                    DtBancoCuentas.Columns.Add("estado");
                    dtDatos = DtBancoCuentas.Copy();
                }

                this.ViewState["DtBancoCuentas"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtBancoCuentas"] = dtDatos;
                _log.Error("Error al generar Tabla. Motivo: " + ex.Message);
            }

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.GetTablaDatos();
                RadGrid1.DataMember = "DtBancoCuentas";
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
            DataTable DtBancoCuentas = new DataTable();
            int _ContadorCargue = 0;
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
                            DtBancoCuentas = new DataTable();
                            DtBancoCuentas = (DataTable)this.ViewState["DtBancoCuentas"];

                            //Recorremos las filas del Datatable
                            foreach (DataRow rowItem in dtEtl.Rows)
                            {
                                DataRow Fila = null;
                                Fila = DtBancoCuentas.NewRow();
                                //--
                                Fila["id_registro"] = DtBancoCuentas.Rows.Count + 1;
                                Fila["id_municipio"] = rowItem["id_municipio"].ToString().Trim();
                                Fila["municipio"] = ObjUtils.GetLimpiarCadena(rowItem["Municipio"].ToString().Trim());
                                Fila["id_banco"] = rowItem["id_banco"].ToString().Trim();
                                Fila["banco"] = rowItem["BANCOS"].ToString().Trim();
                                Fila["idtipo_cuenta"] = rowItem["Idtipo_cuenta"].ToString().Trim();
                                Fila["tipo_cuenta"] = rowItem["Tipo de Cuenta"].ToString().Trim();
                                Fila["idmedio_pago"] = rowItem["id_medio_pago"].ToString().Trim();
                                Fila["medio_pago"] = ObjUtils.GetLimpiarCadena(rowItem["MEDIO DE PAGO"].ToString().Trim());
                                Fila["numero_cuenta"] = rowItem["No. CUENTA"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["No. CUENTA"].ToString().Trim()) : "NA";
                                Fila["cheque"] = "N"; //--rowItem["Periodicidad"].ToString().Trim();
                                Fila["version_registro"] = rowItem["version_registro"].ToString().Trim();
                                Fila["id_estado"] = rowItem["id_estado"].ToString().Trim();
                                Fila["estado"] = rowItem["Estado"].ToString().Trim();
                                DtBancoCuentas.Rows.Add(Fila);
                                _ContadorCargue++;
                            }

                            //Aqui mandamos a mostrar en el Grid la Información.
                            if (DtBancoCuentas.Rows.Count > 0)
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
                                _MsgError = "El Proceso del archivo ha sido realizado correctamente con [" + DtBancoCuentas.Rows.Count + "] registros.";
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
                            _MsgError = "Señor usuario, no se encontro información para cargar al sistema. Por favor validar con soporte técnico !";
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
                DtBancoCuentas = null;
                this.ViewState["DtBancoCuentas"] = null;
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
            this.ViewState["DtBancoCuentas"] = null;
            this.BtnCargar.Visible = true;
            this.BtnProcesar.Visible = false;
            this.BtnCancelar.Visible = false;
            this.RadGrid1.Rebind();
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtBancoCuentas = new DataTable();
            try
            {
                string _ErrorProcesar = "N";
                DtBancoCuentas = (DataTable)this.ViewState["DtBancoCuentas"];
                if (DtBancoCuentas != null)
                {
                    if (DtBancoCuentas.Rows.Count > 0)
                    {
                        int ContadorRow = DtBancoCuentas.Rows.Count;
                        ObjMunicipioBanco.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjMunicipioBanco.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        string _ArrayData = "";
                        int _ContadorRow = 0;
                        int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                        foreach (DataRow rowItem in DtBancoCuentas.Rows)
                        {
                            #region OBTENER DATOS DEL DATATABLE
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            int _IdBanco = Int32.Parse(rowItem["id_banco"].ToString().Trim());
                            int _IdTipoCuenta = Int32.Parse(rowItem["idtipo_cuenta"].ToString().Trim());
                            int _IdMedioPago = Int32.Parse(rowItem["idmedio_pago"].ToString().Trim());
                            string _NumeroCuenta = rowItem["numero_cuenta"].ToString().Trim();
                            string _Cheque = rowItem["cheque"].ToString().Trim();
                            string _VersionRegistro = rowItem["version_registro"].ToString().Trim();
                            int _IdEstado = Int32.Parse(rowItem["id_estado"].ToString().Trim());

                            //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                            if (_ArrayData.ToString().Trim().Length > 0)
                            {
                                _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _IdMunicipio + "," + _IdBanco + "," + _IdTipoCuenta + "," + _IdMedioPago + "," + _NumeroCuenta + "," + _Cheque + "," + _VersionRegistro + "," + _IdEstado + ")" + quote;
                            }
                            else
                            {
                                _ArrayData = quote + "(" + _IdMunicipio + "," + _IdBanco + "," + _IdTipoCuenta + "," + _IdMedioPago + "," + _NumeroCuenta + "," + _Cheque + "," + _VersionRegistro + "," + _IdEstado + ")" + quote;
                            }

                            //--
                            _ContadorRow++;
                            this.ViewState["Contador"] = _ContadorRow;
                            _CantidadReg++;
                            _CantidadTotalReg++;

                            //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                            if (FixedData.CantidadRegProcesarFiles == _CantidadReg)
                            {
                                #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                ObjMunicipioBanco.ArrayData = _ArrayData.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunicipioBanco.GetLoadBancoCuentas(ref _IdRegistro, ref _MsgError))
                                {
                                    _ArrayData = "";
                                    _CantidadReg = 0;
                                    _CantidadLoteProcesado++;
                                    _ErrorProcesar = "N";
                                }
                                else
                                {
                                    #region MOSTRAR MENSAJE DE USUARIO
                                    _ErrorProcesar = "S";
                                    //Mostramos el mensaje porque se produjo un error con la Trx.
                                    this.RadWindowManager1.ReloadOnShow = true;
                                    this.RadWindowManager1.DestroyOnClose = true;
                                    this.RadWindowManager1.Windows.Clear();
                                    this.RadWindowManager1.Enabled = true;
                                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                    this.RadWindowManager1.Visible = true;

                                    RadWindow Ventana = new RadWindow();
                                    Ventana.Modal = true;
                                    string _Mensaje = "Señor usuario, ocurrio un error al cargar el lote No. " + _CantidadLoteProcesado + ", en la linea del archivo No. [ " + this.ViewState["Contador"].ToString().Trim() + " ] del cargue de banco cuentas. Motivo: " + _MsgError;
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
                                    break;
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                _ErrorProcesar = "N";
                            }
                            #endregion
                        }

                        if (_ArrayData.ToString().Trim().Length > 0)
                        {
                            if (_ErrorProcesar.Equals("N"))
                            {
                                ObjMunicipioBanco.ArrayData = _ArrayData.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunicipioBanco.GetLoadBancoCuentas(ref _IdRegistro, ref _MsgError))
                                {
                                    #region MOSTRAR MENSAJE DE USUARIO
                                    DtBancoCuentas = null;
                                    this.ViewState["DtBancoCuentas"] = null;
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