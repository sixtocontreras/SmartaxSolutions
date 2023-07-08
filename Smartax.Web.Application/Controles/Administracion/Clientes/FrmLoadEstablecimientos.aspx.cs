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
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmLoadEstablecimientos : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        const string quote = "\"";

        #region DEFINICION DE OBJETOS DE CLASES
        ClienteEstablecimiento ObjEstablecimiento = new ClienteEstablecimiento();
        Lista ObjLista = new Lista();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
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
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();

                //Limpiamos los datos en el DataTable.
                this.ViewState["DtEstablecimientos"] = null;
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
                DataTable DtEstablecimientos = new DataTable();
                DtEstablecimientos = (DataTable)this.ViewState["DtEstablecimientos"];
                if (DtEstablecimientos != null)
                {
                    dtDatos = DtEstablecimientos.Copy();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtEstablecimientos = new DataTable();
                    DtEstablecimientos.TableName = "DtEstablecimientos";
                    DtEstablecimientos.Columns.Add("id_establecimiento", typeof(Int32));
                    DtEstablecimientos.PrimaryKey = new DataColumn[] { DtEstablecimientos.Columns["id_establecimiento"] };
                    DtEstablecimientos.Columns.Add("Codigo");
                    DtEstablecimientos.Columns.Add("Oficina");
                    DtEstablecimientos.Columns.Add("Contacto");
                    DtEstablecimientos.Columns.Add("Direccion");
                    DtEstablecimientos.Columns.Add("Telefono");
                    DtEstablecimientos.Columns.Add("InscritoRit");
                    DtEstablecimientos.Columns.Add("NoEstablecimientos");
                    DtEstablecimientos.Columns.Add("NoPlacaMunicipal");
                    DtEstablecimientos.Columns.Add("NoMatriculaIC");
                    DtEstablecimientos.Columns.Add("NoRIT");
                    DtEstablecimientos.Columns.Add("AvisosyTableros");
                    DtEstablecimientos.Columns.Add("sucursal");
                    DtEstablecimientos.Columns.Add("oficina_pagadora");
                    DtEstablecimientos.Columns.Add("FechaInicioActividad");
                    DtEstablecimientos.Columns.Add("IdDepartamento");
                    DtEstablecimientos.Columns.Add("Departamento");
                    DtEstablecimientos.Columns.Add("IdMunicipio");
                    DtEstablecimientos.Columns.Add("Municipio");
                    DtEstablecimientos.Columns.Add("IdEstado");
                    DtEstablecimientos.Columns.Add("Estado");
                    dtDatos = DtEstablecimientos.Copy();
                }

                this.ViewState["DtEstablecimientos"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtEstablecimientos"] = dtDatos;
                _log.Error("Error al generar Tabla. Motivo: " + ex.Message);
            }

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.GetTablaDatos();
                RadGrid1.DataMember = "DtEstablecimientos";
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
            DataTable DtEstablecimientos = new DataTable();
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
                            DtEstablecimientos = new DataTable();
                            DtEstablecimientos = (DataTable)this.ViewState["DtEstablecimientos"];

                            //Recorremos las filas del Datatable
                            foreach (DataRow rowItem in dtEtl.Rows)
                            {
                                DataRow Fila = null;
                                Fila = DtEstablecimientos.NewRow();
                                //--
                                Fila["id_establecimiento"] = DtEstablecimientos.Rows.Count + 1;
                                Fila["Codigo"] = rowItem["Codigo"].ToString().Trim();
                                Fila["Oficina"] = rowItem["Oficina"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["Oficina"].ToString().Trim().ToUpper()) : "NO REGISTRA";
                                Fila["Contacto"] = rowItem["Contacto"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["Contacto"].ToString().Trim().ToUpper()) : "NO REGISTRA";
                                Fila["Direccion"] = rowItem["Direccion"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["Direccion"].ToString().Trim().ToUpper()) : "NO REGISTRA";
                                Fila["Telefono"] = rowItem["Telefono"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["Telefono"].ToString().Trim().ToUpper()) : "NO REGISTRA";
                                Fila["InscritoRit"] = rowItem["Inscrito Rit"].ToString().Trim();
                                Fila["NoEstablecimientos"] = rowItem["No. Establecimientos"].ToString().Trim();
                                Fila["NoPlacaMunicipal"] = rowItem["No. Placa Municipal"].ToString().Trim();
                                Fila["NoMatriculaIC"] = rowItem["No. Matricula IC"].ToString().Trim();
                                Fila["NoRIT"] = rowItem["No. RIT"].ToString().Trim();
                                Fila["AvisosyTableros"] = rowItem["Avisos y Tableros"].ToString().Trim().ToUpper();
                                Fila["sucursal"] = rowItem["Sucursal"].ToString().Trim().ToUpper();
                                Fila["oficina_pagadora"] = rowItem["ID_Oficina_Pagadora"].ToString().Trim().ToUpper();
                                Fila["FechaInicioActividad"] = rowItem["Fecha Inicio Actividad"].ToString().Trim().Length > 0 ? Convert.ToDateTime(rowItem["Fecha Inicio Actividad"].ToString().Trim()).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                                Fila["IdDepartamento"] = rowItem["ID DEPARTAMENTO"].ToString().Trim().ToUpper();
                                Fila["Departamento"] = rowItem["Departamento"].ToString().Trim().ToUpper();
                                Fila["IdMunicipio"] = rowItem["ID MUNICIPIO"].ToString().Trim();
                                Fila["Municipio"] = rowItem["Municipio"].ToString().Trim();
                                Fila["IdEstado"] = rowItem["ID ESTADO"].ToString().Trim();
                                Fila["Estado"] = rowItem["Estado"].ToString().Trim();
                                DtEstablecimientos.Rows.Add(Fila);
                            }

                            //Aqui mandamos a mostrar en el Grid la Información.
                            if (DtEstablecimientos.Rows.Count > 0)
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
                                _MsgError = "El Proceso del archivo ha sido realizado correctamente con [" + DtEstablecimientos.Rows.Count + "] registros.";
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
                DtEstablecimientos = null;
                this.ViewState["DtEstablecimientos"] = null;
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
            this.ViewState["DtEstablecimientos"] = null;
            this.BtnCargar.Visible = true;
            this.BtnProcesar.Visible = false;
            this.BtnCancelar.Visible = false;
            this.RadGrid1.Rebind();
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtEstablecimientos = new DataTable();
            try
            {
                string _ErrorProcesar = "N";
                DtEstablecimientos = (DataTable)this.ViewState["DtEstablecimientos"];
                if (DtEstablecimientos != null)
                {
                    if (DtEstablecimientos.Rows.Count > 0)
                    {
                        int ContadorRow = DtEstablecimientos.Rows.Count;
                        ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                        ObjEstablecimiento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjEstablecimiento);

                        string _ArrayEstablecimientos = "";
                        int _ContadorRow = 0;
                        int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                        foreach (DataRow rowItem in DtEstablecimientos.Rows)
                        {
                            #region OBTENER DATOS DEL DATATABLE
                            string _Codigo = rowItem["Codigo"].ToString().Trim();
                            string _Oficina = rowItem["Oficina"].ToString().Trim();
                            string _Contacto = rowItem["Contacto"].ToString().Trim();
                            string _Direccion = rowItem["Direccion"].ToString().Trim();
                            string _Telefono = rowItem["Telefono"].ToString().Trim();
                            string _InscritoRit = rowItem["InscritoRit"].ToString().Trim().Length > 0 ? rowItem["InscritoRit"].ToString().Trim().Substring(0, 1) : "N";
                            int _NoEstablecimientos = rowItem["NoEstablecimientos"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["NoEstablecimientos"].ToString().Trim()) : 0;
                            string _NoPlacaMunicipal = rowItem["NoPlacaMunicipal"].ToString().Trim();
                            string _NoMatriculaIC = rowItem["NoMatriculaIC"].ToString().Trim();
                            string _NoRIT = rowItem["NoRIT"].ToString().Trim();
                            string _AvisosyTableros = rowItem["AvisosyTableros"].ToString().Trim().Length > 0 ? rowItem["AvisosyTableros"].ToString().Trim().Substring(0, 1) : "N";
                            string _FechaInicioActividad = rowItem["FechaInicioActividad"].ToString().Trim();
                            string _Sucursal = rowItem["sucursal"].ToString().Trim();
                            string _OficinaPagadora = rowItem["oficina_pagadora"].ToString().Trim();
                            int _IdDepartamento = Int32.Parse(rowItem["IdDepartamento"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowItem["IdMunicipio"].ToString().Trim());
                            int _IdEstado = rowItem["IdEstado"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["IdEstado"].ToString().Trim()) : 0;

                            //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                            if (_ArrayEstablecimientos.ToString().Trim().Length > 0)
                            {
                                _ArrayEstablecimientos = _ArrayEstablecimientos.ToString().Trim() + "," + quote + "(" + _Codigo + "," + _Oficina + "," + _Contacto + "," + _Direccion + "," + _Telefono + "," + _InscritoRit + "," + _NoEstablecimientos + "," + _NoPlacaMunicipal + "," + _NoMatriculaIC + "," + _NoRIT + "," + _AvisosyTableros + "," + _FechaInicioActividad + "," + _Sucursal + "," + _OficinaPagadora + "," + _IdDepartamento + "," + _IdMunicipio + "," + _IdEstado + ")" + quote;
                            }
                            else
                            {
                                _ArrayEstablecimientos = quote + "(" + _Codigo + "," + _Oficina + "," + _Contacto + "," + _Direccion + "," + _Telefono + "," + _InscritoRit + "," + _NoEstablecimientos + "," + _NoPlacaMunicipal + "," + _NoMatriculaIC + "," + _NoRIT + "," + _AvisosyTableros + "," + _FechaInicioActividad + "," + _Sucursal + "," + _OficinaPagadora + "," + _IdDepartamento + "," + _IdMunicipio + "," + _IdEstado + ")" + quote;
                            }

                            //--
                            _ContadorRow++;
                            this.ViewState["Contador"] = _ContadorRow;
                            _CantidadReg++;
                            _CantidadTotalReg++;

                            //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                            if (FixedData.CantidadRegProcesarFiles == _CantidadReg)
                            {
                                ObjEstablecimiento.ArrayEstablecimientos = _ArrayEstablecimientos.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjEstablecimiento.GetLoadEstablecimiento(ref _IdRegistro, ref _MsgError))
                                {
                                    _ArrayEstablecimientos = "";
                                    _CantidadReg = 0;
                                    _CantidadLoteProcesado++;
                                    _ErrorProcesar = "N";
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
                            else
                            {
                                _ErrorProcesar = "N";
                            }
                            #endregion
                        }

                        if (_ArrayEstablecimientos.ToString().Trim().Length > 0)
                        {
                            if (_ErrorProcesar.Equals("N"))
                            {
                                ObjEstablecimiento.ArrayEstablecimientos = _ArrayEstablecimientos.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjEstablecimiento.GetLoadEstablecimiento(ref _IdRegistro, ref _MsgError))
                                {
                                    #region REGISTRO DE LOGS DE AUDITORIA
                                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                    ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                                    ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                                    ObjAuditoria.ModuloApp = "LOAD_ESTABLECIMIENTOS";
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

                                    #region MOSTRAR MENSAJE DE USUARIO
                                    DtEstablecimientos = null;
                                    this.ViewState["DtEstablecimientos"] = null;
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