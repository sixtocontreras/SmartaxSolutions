using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using System.Web.Script.Serialization;
using System.Text;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmLoadEstadoFinanciero : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        const string quote = "\"";

        ClienteEstadosFinanciero ObjClienteEF = new ClienteEstadosFinanciero();
        Lista ObjLista = new Lista();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        EnvioCorreo ObjCorreo = new EnvioCorreo();

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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void LstAniosGravables()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = ObjLista.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "descripcion_anio";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años gravables. Motivo: " + ex.ToString();
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

        protected void LstMeses()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbMesEf.DataSource = ObjLista.GetMeses();
                this.CmbMesEf.DataValueField = "id_mes";
                this.CmbMesEf.DataTextField = "nombre_mes";
                this.CmbMesEf.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los meses. Motivo: " + ex.ToString();
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

        protected void LstVersionEf()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbVersionEf.DataSource = ObjLista.GetVersionEf();
                this.CmbVersionEf.DataValueField = "id_version";
                this.CmbVersionEf.DataTextField = "version_ef";
                this.CmbVersionEf.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar las versiones. Motivo: " + ex.ToString();
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
                this.BtnPrecargar.Visible = false;
                this.BtnProcesar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                this.LstAniosGravables();
                this.LstMeses();
                this.LstVersionEf();
                this.LstTipoCaracter();
                this.CmbAnioGravable.SelectedValue = DateTime.Now.AddYears(-1).ToString("yyyy");
                this.CmbMesEf.SelectedValue = DateTime.Now.ToString("MM");

                //Limpiamos los datos en el DataTable.
                this.ViewState["DtEstadoFinanciero"] = null;
                this.GetTablaDatos();

                //--Aqui capturamos los parametros enviados.
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();

                string _DirectorioVirtual = HttpContext.Current.Server.MapPath("\\" + this.Session["DirectorioVirtual"].ToString().Trim() + "\\");
                this.ViewState["_PathDirectorio"] = _DirectorioVirtual + "\\" + "ESTADOS_FINANCIEROS" + "\\" + "CLIENTE_" + this.ViewState["IdCliente"].ToString().Trim();

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

        #region DEFINICION DE METODOS DEL GRID 
        private DataTable GetTablaDatos()
        {
            DataTable dtDatos = new DataTable();
            try
            {
                DataTable DtEstadoFinanciero = new DataTable();
                DtEstadoFinanciero = (DataTable)this.ViewState["DtEstadoFinanciero"];

                if (DtEstadoFinanciero != null)
                {
                    dtDatos = DtEstadoFinanciero.Copy();
                    DtEstadoFinanciero.Clear();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtEstadoFinanciero = new DataTable();
                    DtEstadoFinanciero.TableName = "DtEstadoFinanciero";
                    DtEstadoFinanciero.Columns.Add("id_registro", typeof(Int32));
                    DtEstadoFinanciero.PrimaryKey = new DataColumn[] { DtEstadoFinanciero.Columns["id_registro"] };
                    DtEstadoFinanciero.Columns.Add("codigo_cuenta");
                    DtEstadoFinanciero.Columns.Add("codigo_oficina");
                    DtEstadoFinanciero.Columns.Add("saldo_inicial_mn");
                    DtEstadoFinanciero.Columns.Add("debito_mn");
                    DtEstadoFinanciero.Columns.Add("credito_mn");
                    DtEstadoFinanciero.Columns.Add("saldo_final_mn");
                    DtEstadoFinanciero.Columns.Add("saldo_inicial_me");
                    DtEstadoFinanciero.Columns.Add("debito_me");
                    DtEstadoFinanciero.Columns.Add("credito_me");
                    DtEstadoFinanciero.Columns.Add("saldo_final_me");
                    //DtEstadoFinanciero.Columns.Add("fecha_ef");
                    dtDatos = DtEstadoFinanciero.Copy();
                }

                this.ViewState["DtEstadoFinanciero"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtEstadoFinanciero"] = dtDatos;

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
                string _MsgError = "Error con la tabla de datos. Motivo: " + ex.ToString();
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

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable DtEstadoFinanciero = new DataTable();
                DtEstadoFinanciero = (DataTable)this.ViewState["DtEstadoFinanciero"];
                //--
                this.RadGrid1.DataSource = DtEstadoFinanciero;  //--this.GetTablaDatos();
                this.RadGrid1.DataMember = "DtEstadoFinanciero";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del lote para despacho de inventario. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del lote para despacho de inventario. Motivo: " + ex.ToString();
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
        #endregion

        #region DEFINICION DE EVENTOS DE BOTONES
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.BtnPrecargar.Enabled = true;
            this.BtnProcesar.Enabled = false;
            this.BtnCancelar.Enabled = false;
            //--Aqui mandamos actualizar el Grid con los datos del inventario.
            this.ViewState["DtEstadoFinanciero"] = null;
            this.RadGrid1.Rebind();
        }

        protected void BtnPrecargar_Click(object sender, EventArgs e)
        {
            DataTable DtEstadoFinanciero = new DataTable();
            try
            {
                if (this.ViewState["IdCliente"].ToString().Trim().Length > 0)
                {
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    //--Aqui obtenemos el nombre del archivo
                    this.ViewState["_NombreArchivo"] = this.FileExaminar.FileName.ToString().Trim();
                    string _PathDirectorio = this.ViewState["_PathDirectorio"].ToString().Trim() + "\\" + this.ViewState["_NombreArchivo"].ToString().Trim();
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
                    //char _TipoSeparador = char.Parse(this.CmbTipoCaracter.SelectedValue.ToString().Trim());
                    //char[] delimiters = new char[] { '\t' };

                    if (FixedData.GetListExtensionesCons.Contains(_TipoExtension.ToString()))
                    {
                        //Eliminamos el archivo del directorio
                        File.Delete(_PathDirectorio);

                        //Guardamos el archivo en el directorio
                        this.FileExaminar.SaveAs(_PathDirectorio);

                        int _ContadorRow = 0;
                        string _MsgError = "";
                        DataTable dtEtl = new DataTable();
                        dtEtl = ObjUtils.GetEtl(_PathDirectorio, _TipoSeparador, ref _MsgError);

                        if (dtEtl != null)
                        {
                            if (dtEtl.Rows.Count > 0)
                            {
                                #region RECORRER EL DATATABLE DE LA INFORMACION
                                _ContadorRow = dtEtl.Rows.Count;
                                DtEstadoFinanciero = new DataTable();
                                DtEstadoFinanciero = (DataTable)this.ViewState["DtEstadoFinanciero"];
                                int _ContadorRows = 0;

                                foreach (DataRow rowItem in dtEtl.Rows)
                                {
                                    #region OBTENER DATOS DEL DATATABLE A CARGAR
                                    _ContadorRows++;
                                    string _CodigoCuenta = rowItem["COD_CUENTA_PUC"].ToString().Trim();
                                    string _CodigoOficina = rowItem["COD_OFICINA"].ToString().Trim();
                                    //--VARIABLE 1. SEPARADOR DE MILES, 2. SEPARADOR DECIMALES
                                    string _SaldoInicialMn1 = rowItem["SALDO_INICIAL_MN"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "");
                                    string _SaldoInicialMn2 = _SaldoInicialMn1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _SaldoInicialMn = _SaldoInicialMn2;
                                    string _DebitoMn1 = rowItem["DEBITO_MN"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "");
                                    string _DebitoMn2 = _DebitoMn1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _DebitoMn = _DebitoMn2;
                                    string _CreditoMn1 = rowItem["CREDITO_MN"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "");
                                    string _CreditoMn2 = _CreditoMn1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _CreditoMn = _CreditoMn2;
                                    string _SaldoFinalMn1 = rowItem["SALDO_FINAL_MN"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "");
                                    string _SaldoFinalMn2 = _SaldoFinalMn1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _SaldoFinalMn = _SaldoFinalMn2;
                                    string _SaldoInicialMe1 = rowItem["SALDO_INICIAL_ME"].ToString().Trim().Length > 0 ? rowItem["SALDO_INICIAL_ME"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "") : "0";
                                    string _SaldoInicialMe2 = _SaldoInicialMe1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _SaldoInicialMe = _SaldoInicialMe2;
                                    string _DebitoMe1 = rowItem["DEBITO_ME"].ToString().Trim().Length > 0 ? rowItem["DEBITO_ME"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "") : "0";
                                    string _DebitoMe2 = _DebitoMe1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _DebitoMe = _DebitoMe2;
                                    string _CreditoMe1 = rowItem["CREDITO_ME"].ToString().Trim().Length > 0 ? rowItem["CREDITO_ME"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "") : "0";
                                    string _CreditoMe2 = _CreditoMe1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _CreditoMe = _CreditoMe2;
                                    string _SaldoFinalMe1 = rowItem["SALDO_FINAL_ME"].ToString().Trim().Length > 0 ? rowItem["SALDO_FINAL_ME"].ToString().Trim().Replace(FixedData.SeparadorMilesFile, "") : "0";
                                    string _SaldoFinalMe2 = _SaldoFinalMe1.Replace(FixedData.SeparadorDecimalesFile, ".");
                                    string _SaldoFinalMe = _SaldoFinalMe2;
                                    //string _FechaEF = DateTime.Parse(rowItem["FECHA_EF"].ToString().Trim()).ToString("yyyy-MM-dd");
                                    #endregion

                                    #region AQUI VALIDAMOS LOS VALORES OBTENIDOS DEL DATATABLE
                                    //--AQUI VALIDAMOS QUE LOS VALORES DEL ESTADO FINANCIERO VENGA POR LO MENOS EN CERO                            
                                    if (_SaldoInicialMn.ToString().Trim().Length > 0)
                                    {
                                        if (_DebitoMn.ToString().Trim().Length > 0)
                                        {
                                            if (_CreditoMn.ToString().Trim().Length > 0)
                                            {
                                                if (_SaldoFinalMn.ToString().Trim().Length > 0)
                                                {
                                                    #region AGREGAR DATOS AL DATATABLE
                                                    //--AQUI AGREGAMOS EL REGISTRO EN EL DATATABLE
                                                    DataRow Fila = null;
                                                    Fila = DtEstadoFinanciero.NewRow();
                                                    //--OBTENER DATOS DEL ESTADO FINANCIERO
                                                    Fila["id_registro"] = DtEstadoFinanciero.Rows.Count + 1;
                                                    Fila["codigo_cuenta"] = _CodigoCuenta;
                                                    Fila["codigo_oficina"] = _CodigoOficina;
                                                    Fila["saldo_inicial_mn"] = _SaldoInicialMn;
                                                    Fila["debito_mn"] = _DebitoMn;
                                                    Fila["credito_mn"] = _CreditoMn;
                                                    Fila["saldo_final_mn"] = _SaldoFinalMn;
                                                    Fila["saldo_inicial_me"] = _SaldoInicialMe;
                                                    Fila["debito_me"] = _DebitoMe;
                                                    Fila["credito_me"] = _CreditoMe;
                                                    Fila["saldo_final_me"] = _SaldoFinalMe;
                                                    //Fila["fecha_ef"] = _FechaEF;
                                                    DtEstadoFinanciero.Rows.Add(Fila);
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
                                                    _MsgError = "Error el valor de la Columna SALDO FINAL de moneda nacional en la linea [" + _ContadorRows + "] viene en blanco. Por favor validar el archivo e intente cargarlo nuevamente !";
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

                                                    //--ACTUALIZAR LISTA
                                                    if (DtEstadoFinanciero.Rows.Count > 0)
                                                    {
                                                        DtEstadoFinanciero.Clear();
                                                        this.ViewState["DtEstadoFinanciero"] = null;
                                                        this.ViewState["DtEstadoFinanciero"] = DtEstadoFinanciero.Clone();
                                                        //--Aqui mandamos actualizar el Grid con los datos del inventario.
                                                        this.RadGrid1.Rebind();
                                                    }
                                                    return;
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
                                                _MsgError = "Error el valor de la Columna CREDITO de moneda nacional en la linea [" + _ContadorRows + "] viene en blanco. Por favor validar el archivo e intente cargarlo nuevamente !";
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
                                                //--ACTUALIZAR LISTA
                                                if (DtEstadoFinanciero.Rows.Count > 0)
                                                {
                                                    DtEstadoFinanciero.Clear();
                                                    this.ViewState["DtEstadoFinanciero"] = null;
                                                    this.ViewState["DtEstadoFinanciero"] = DtEstadoFinanciero.Clone();
                                                    //--Aqui mandamos actualizar el Grid con los datos del inventario.
                                                    this.RadGrid1.Rebind();
                                                }
                                                return;
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
                                            _MsgError = "Error el valor de la Columna DEBITO de moneda nacional en la linea [" + _ContadorRows + "] viene en blanco. Por favor validar el archivo e intente cargarlo nuevamente !";
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
                                            //--ACTUALIZAR LISTA
                                            if (DtEstadoFinanciero.Rows.Count > 0)
                                            {
                                                DtEstadoFinanciero.Clear();
                                                this.ViewState["DtEstadoFinanciero"] = null;
                                                this.ViewState["DtEstadoFinanciero"] = DtEstadoFinanciero.Clone();
                                                //--Aqui mandamos actualizar el Grid con los datos del inventario.
                                                this.RadGrid1.Rebind();
                                            }
                                            return;
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
                                        _MsgError = "Error el valor de la Columna SALDO INICIAL de moneda nacional en la linea [" + _ContadorRows + "] viene en blanco. Por favor validar el archivo e intente cargarlo nuevamente !";
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
                                        //--ACTUALIZAR LISTA
                                        if (DtEstadoFinanciero.Rows.Count > 0)
                                        {
                                            DtEstadoFinanciero.Clear();
                                            this.ViewState["DtEstadoFinanciero"] = null;
                                            this.ViewState["DtEstadoFinanciero"] = DtEstadoFinanciero.Clone();
                                            //--Aqui mandamos actualizar el Grid con los datos del inventario.
                                            this.RadGrid1.Rebind();
                                        }
                                        return;
                                    }
                                    #endregion
                                }

                                //--ACTUALIZAR LISTA
                                if (DtEstadoFinanciero.Rows.Count > 0)
                                {
                                    //--
                                    this.ViewState["DtEstadoFinanciero"] = DtEstadoFinanciero;
                                    this.BtnPrecargar.Enabled = false;
                                    this.BtnProcesar.Enabled = true;
                                    this.BtnCancelar.Enabled = true;
                                    //--Aqui mandamos actualizar el Grid con los datos del inventario.
                                    this.RadGrid1.Rebind();
                                }
                                #endregion
                            }
                            else
                            {
                                #region MOSTRAR MENSAJE DE USUARIO
                                //this.UpdatePanel3.Update();
                                //Mostramos el mensaje porque se produjo un error con la Trx.
                                this.RadWindowManager1.ReloadOnShow = true;
                                this.RadWindowManager1.DestroyOnClose = true;
                                this.RadWindowManager1.Windows.Clear();
                                this.RadWindowManager1.Enabled = true;
                                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                this.RadWindowManager1.Visible = true;

                                RadWindow Ventana = new RadWindow();
                                Ventana.Modal = true;
                                _MsgError = "Señor usuario, no se obtuvo datos para cargar ...!";
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
                            //this.UpdatePanel3.Update();
                            //Mostramos el mensaje porque se produjo un error con la Trx.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
                            _MsgError = "Señor usuario, ocurrio un error al realizar la Precarga del archivo ...!";
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
                        //this.UpdatePanel3.Update();
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, las extensiones validas para cargar los archivo debe ser .TXT ó .CSV ...!";
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
                    //this.UpdatePanel3.Update();
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor usuario, para realizar este proceso se requiere el Id del cliente. Intentelo nuevamente ...!";
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
                //this.UpdatePanel3.Update();
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con precarga del archivo. Motivo: " + ex.ToString();
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
                DtEstadoFinanciero = null;
                this.ViewState["DtEstadoFinanciero"] = null;
                this.RadGrid1.Rebind();
                #endregion
            }
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtEstadoFinanciero = new DataTable();
            try
            {
                string _ErrorProcesar = "N";
                DtEstadoFinanciero = (DataTable)this.ViewState["DtEstadoFinanciero"];
                if (DtEstadoFinanciero != null)
                {
                    if (DtEstadoFinanciero.Rows.Count > 0)
                    {
                        int ContadorRow = DtEstadoFinanciero.Rows.Count;
                        ObjClienteEF.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                        ObjClienteEF.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();
                        ObjClienteEF.MesEf = this.CmbMesEf.SelectedValue.ToString().Trim();
                        ObjClienteEF.VersionEf = this.CmbVersionEf.SelectedValue.ToString().Trim();
                        ObjClienteEF.NombreArchivo = this.ViewState["_NombreArchivo"].ToString().Trim();
                        ObjClienteEF.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjClienteEF.IdEstado = 1;
                        ObjClienteEF.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                        ObjClienteEF.TipoProceso = 1;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjClienteEF);

                        //--AQUI VALIDAMOS EL CARGUE DEL E.F.
                        if (!ObjClienteEF.GetValidarCargueEF())
                        {
                            #region OBTENER DATOS DEL ARCHIVO QUE SE ESTA PROCESANDO
                            string _ArrayEstadoFinanciero = "";
                            int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                            foreach (DataRow rowItem in DtEstadoFinanciero.Rows)
                            {
                                #region OBTENEMOS LOS VALORES DEL DATATABLE
                                string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                                string _CodigoOficina = rowItem["codigo_oficina"].ToString().Trim();
                                string _SaldoInicialMn = rowItem["saldo_inicial_mn"].ToString().Trim();
                                string _DebitoMn = rowItem["debito_mn"].ToString().Trim();
                                string _CreditoMn = rowItem["credito_mn"].ToString().Trim();
                                string _SaldoFinalMn = rowItem["saldo_final_mn"].ToString().Trim();
                                string _SaldoInicialMe = rowItem["saldo_inicial_me"].ToString().Trim();
                                string _DebitoMe = rowItem["debito_me"].ToString().Trim();
                                string _CreditoMe = rowItem["credito_me"].ToString().Trim();
                                string _SaldoFinalMe = rowItem["saldo_final_me"].ToString().Trim();
                                //string _FechaEF = rowItem["FECHA_EF"].ToString().Trim();

                                //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                if (_ArrayEstadoFinanciero.ToString().Trim().Length > 0)
                                {
                                    _ArrayEstadoFinanciero = _ArrayEstadoFinanciero.ToString().Trim() + "," + quote + "(" + _CodigoCuenta + "," + _CodigoOficina + "," + _SaldoInicialMn + "," + _DebitoMn + "," + _CreditoMn + "," + _SaldoFinalMn + "," + _SaldoInicialMe + "," + _DebitoMe + "," + _CreditoMe + "," + _SaldoFinalMe + ")" + quote;
                                }
                                else
                                {
                                    _ArrayEstadoFinanciero = quote + "(" + _CodigoCuenta + "," + _CodigoOficina + "," + _SaldoInicialMn + "," + _DebitoMn + "," + _CreditoMn + "," + _SaldoFinalMn + "," + _SaldoInicialMe + "," + _DebitoMe + "," + _CreditoMe + "," + _SaldoFinalMe + ")" + quote;
                                }
                                _CantidadReg++;
                                _CantidadTotalReg++;

                                //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                if (FixedData.CantidadRegProcesar == _CantidadReg)
                                {
                                    #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                    ObjClienteEF.ArrayEstadoFinanciero = _ArrayEstadoFinanciero.ToString().Trim();
                                    ObjClienteEF.CargueProceso = "N";
                                    int _IdRegistro = 0;
                                    string _CodError = "", _MsgError = "";
                                    if (ObjClienteEF.AddLoadEstadoFinanciero(ref _IdRegistro, ref _CodError, ref _MsgError))
                                    {
                                        _ArrayEstadoFinanciero = "";
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
                                        string _Mensaje = "";
                                        if (_CodError.Trim().Equals("03"))
                                        {
                                            _Mensaje = "Error al cargar el estado financiero. Motivo: " + _MsgError;
                                            this.GetOficinasCuentasCrear();
                                        }
                                        else
                                        {
                                            _Mensaje = "Señor usuario, ocurrio un error al cargar el lote No. " + _CantidadLoteProcesado + " del Estado financiero. Motivo: " + _MsgError;
                                        }
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
                                #endregion
                            }
                            #endregion

                            #region REALIZAMOS EL PROCESO DE CARGUE EN LA BASE DE DATOS
                            if (_ArrayEstadoFinanciero.ToString().Trim().Length > 0)
                            {
                                if (_ErrorProcesar.Equals("N"))
                                {
                                    ObjClienteEF.ArrayEstadoFinanciero = _ArrayEstadoFinanciero.ToString().Trim();
                                    ObjClienteEF.CargueProceso = "S";
                                    int _IdRegistro = 0;
                                    string _CodError = "", _MsgError = "";
                                    if (ObjClienteEF.AddLoadEstadoFinanciero(ref _IdRegistro, ref _CodError, ref _MsgError))
                                    {
                                        #region REGISTRO DE LOGS DE AUDITORIA
                                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                        ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                                        ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                                        ObjAuditoria.ModuloApp = "LOAD_ESTADO_FINANCIERO";
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
                                        this.BtnPrecargar.Visible = true;
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
                                        _MsgError = "El estado financiero ha sido cargado con " + _CantidadTotalReg + " registros de forma exitosa.";
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
                                        DtEstadoFinanciero = null;
                                        this.ViewState["DtEstadoFinanciero"] = null;
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
                                        int CantidadLote = (_CantidadLoteProcesado + 1);
                                        string _Mensaje = "";
                                        if (_CodError.Trim().Equals("03"))
                                        {
                                            _Mensaje = "Error al cargar el estado financiero. Motivo: " + _MsgError;
                                            this.GetOficinasCuentasCrear();
                                        }
                                        else
                                        {
                                            _Mensaje = "Señor usuario, ocurrio un error al cargar el lote No. " + CantidadLote + " del Estado financiero. Motivo: " + _MsgError;
                                        }
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
                                string _MsgError = "Error al procesar no se encontro información para procesar !";
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
                            string _MsgError = "Señor Usuario, la versión del estado financiero ya se encuentra cargada en el sistema. Por favor validar e intentar nuevamente !";
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
                        _log.Error(_MsgError);
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
                string _MsgError = "Señor usuario, ocurrio un error al cargar el archivo por favor vuelva intentarlo. Motivo: " + ex.Message;
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

        private void GetOficinasCuentasCrear()
        {
            try
            {
                ObjClienteEF.TipoConsulta = 1;
                ObjClienteEF.IdEstado = 1;
                ObjClienteEF.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                DataTable dtDatos = new DataTable();
                dtDatos = ObjClienteEF.GetOficinasCuentasCrear();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        StringBuilder strDetalleEmail = new StringBuilder();
                        string _AnioGravable = this.CmbAnioGravable.SelectedItem.Text.ToString().Trim();
                        string _MesEf = this.CmbMesEf.SelectedItem.Text.ToString().Trim();
                        //--
                        string _TituloTablaHtml = "LISTA DE OFICINAS Y CUENTAS POR CREAR EN EL SISTEMA CON EL E.F. AÑO GRAVABLE: " + _AnioGravable + ", MES: " + _MesEf;
                        string _TableHtml = GetTableHtml(dtDatos, _TituloTablaHtml);
                        strDetalleEmail.Append(_TableHtml.ToString() + "<br/><br/><br/>");

                        //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                        ObjCorreo.StrServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                        ObjCorreo.PuertoCorreo = FixedData.PuertoCorreoGmail;
                        ObjCorreo.StrEmailDe = FixedData.UsuarioEmail.ToString().Trim();
                        ObjCorreo.StrPasswordDe = FixedData.PasswordEmail.ToString().Trim();
                        //Aqui hacemos el envio del correo.
                        ObjCorreo.StrEmailPara = FixedData.EMAIL_PARA.ToString().Trim();
                        ObjCorreo.StrEmailCopia = FixedData.EMAILS_COPIA.ToString().Trim();
                        ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                        ObjCorreo.StrAsunto = "REF.: LISTA DE OFICINAS Y CUENTAS POR CREAR";
                        //--
                        string _MsgError = "";
                        if (ObjCorreo.SendEmailAlerta(ref _MsgError))
                        {
                            strDetalleEmail = new StringBuilder();
                            _log.Info(_MsgError);
                        }
                        else
                        {
                            _log.Error(_MsgError);
                        }
                    }
                    else
                    {
                        _log.Error("1. Error al enviar el correo de las oficinas y cuentas a crear no hay registros en el datatable");
                    }
                }
                else
                {
                    _log.Error("2. Error al enviar el correo de las oficinas y cuentas a crear no hay registros en el datatable");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al enviar el correo de las oficinas y cuentas a crear. Motivo: " + ex.Message);
            }
        }

        public static string GetTableHtml(DataTable DtDatos, string _TituloTablaHtml)
        {
            StringBuilder TableHtml = new StringBuilder();
            try
            {
                //Table start.
                TableHtml.Append("<table border = '1'>");
                TableHtml.Append("<tr align='center' valign='middle' >");
                TableHtml.Append("<th colspan=" + DtDatos.Columns.Count + "> " + _TituloTablaHtml + " </th> ");
                TableHtml.Append("</tr>");

                //Building the Header row.
                TableHtml.Append("<tr>");
                foreach (DataColumn column in DtDatos.Columns)
                {
                    TableHtml.Append("<th>");
                    TableHtml.Append(column.ColumnName.ToString().ToUpper());
                    TableHtml.Append("</th>");
                }
                TableHtml.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in DtDatos.Rows)
                {
                    TableHtml.Append("<tr>");
                    foreach (DataColumn column in DtDatos.Columns)
                    {
                        TableHtml.Append("<td>");
                        TableHtml.Append(row[column.ColumnName]);
                        TableHtml.Append("</td>");
                    }
                    TableHtml.Append("</tr>");
                }

                //Table end.
                TableHtml.Append("</table>");

            }
            catch (Exception ex)
            {
                TableHtml.Append("");
                _log.Error("Error al obtener la Tabla Html. Motivo: " + ex.Message);
            }

            return TableHtml.ToString();
        }
        #endregion
    }
}