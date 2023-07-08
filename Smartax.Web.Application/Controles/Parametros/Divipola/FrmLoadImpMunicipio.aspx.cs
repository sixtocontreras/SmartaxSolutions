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
    public partial class FrmLoadImpMunicipio : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        const string quote = "\"";

        #region DEFINICION DE OBJETOS DE CLASES
        MunicipioImpuestos ObjMunImpuesto = new MunicipioImpuestos();
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
                this.ViewState["DtActEconomica"] = null;
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
                DataTable DtImpMunicipio = new DataTable();
                DtImpMunicipio = (DataTable)this.ViewState["DtImpMunicipio"];
                if (DtImpMunicipio != null)
                {
                    dtDatos = DtImpMunicipio.Copy();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtImpMunicipio = new DataTable();
                    DtImpMunicipio.TableName = "DtImpMunicipio";
                    DtImpMunicipio.Columns.Add("id_registro", typeof(Int32));
                    DtImpMunicipio.PrimaryKey = new DataColumn[] { DtImpMunicipio.Columns["id_registro"] };
                    DtImpMunicipio.Columns.Add("anio_gravable");
                    DtImpMunicipio.Columns.Add("id_municipio");
                    DtImpMunicipio.Columns.Add("municipio");
                    DtImpMunicipio.Columns.Add("id_formulario");
                    DtImpMunicipio.Columns.Add("formulario");
                    DtImpMunicipio.Columns.Add("idnum_renglon");
                    DtImpMunicipio.Columns.Add("descripcion");
                    DtImpMunicipio.Columns.Add("id_periodicidad");
                    DtImpMunicipio.Columns.Add("periodicidad");
                    DtImpMunicipio.Columns.Add("idtipo_tarifa");
                    DtImpMunicipio.Columns.Add("valor_tarifa");
                    DtImpMunicipio.Columns.Add("para_calcular");
                    DtImpMunicipio.Columns.Add("idnum_renglon1");
                    DtImpMunicipio.Columns.Add("idtipo_operacion1");
                    DtImpMunicipio.Columns.Add("idnum_renglon2");
                    DtImpMunicipio.Columns.Add("idtipo_operacion2");
                    DtImpMunicipio.Columns.Add("idnum_renglon3");
                    DtImpMunicipio.Columns.Add("idtipo_operacion3");
                    DtImpMunicipio.Columns.Add("idnum_renglon4");
                    DtImpMunicipio.Columns.Add("idtipo_operacion4");
                    DtImpMunicipio.Columns.Add("idnum_renglon5");
                    DtImpMunicipio.Columns.Add("idtipo_operacion5");
                    DtImpMunicipio.Columns.Add("idnum_renglon6");
                    DtImpMunicipio.Columns.Add("id_estado");
                    DtImpMunicipio.Columns.Add("estado");
                    dtDatos = DtImpMunicipio.Copy();
                }

                this.ViewState["DtImpMunicipio"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtImpMunicipio"] = dtDatos;
                _log.Error("Error al generar Tabla. Motivo: " + ex.Message);
            }

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.GetTablaDatos();
                RadGrid1.DataMember = "DtActEconomica";
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
            DataTable DtImpMunicipio = new DataTable();
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
                            DtImpMunicipio = new DataTable();
                            DtImpMunicipio = (DataTable)this.ViewState["DtImpMunicipio"];

                            //Recorremos las filas del Datatable
                            foreach (DataRow rowItem in dtEtl.Rows)
                            {
                                #region ESCRIBIR DATOS EN EL DATATABLE
                                DataRow Fila = null;
                                Fila = DtImpMunicipio.NewRow();
                                //--
                                Fila["id_registro"] = DtImpMunicipio.Rows.Count + 1;
                                Fila["anio_gravable"] = rowItem["Anio Gravable"].ToString().Trim();
                                Fila["id_municipio"] = rowItem["Id Municipio"].ToString().Trim();
                                Fila["municipio"] = rowItem["Municipio"].ToString().Trim();
                                Fila["id_formulario"] = rowItem["Id Formulario"].ToString().Trim();
                                Fila["formulario"] = rowItem["Formulario"].ToString().Trim();
                                Fila["idnum_renglon"] = rowItem["Id No. Renglon"].ToString().Trim();
                                Fila["descripcion"] = ObjUtils.GetLimpiarCadena(rowItem["Descripcion"].ToString().Trim());
                                Fila["id_periodicidad"] = rowItem["Id Periodicidad"].ToString().Trim();
                                Fila["periodicidad"] = rowItem["Periodicidad"].ToString().Trim();
                                Fila["idtipo_tarifa"] = rowItem["Id Tipo Tarifa"].ToString().Trim();
                                Fila["valor_tarifa"] = rowItem["Valor Tarifa"].ToString().Trim();
                                Fila["para_calcular"] = rowItem["Id Para calcular"].ToString().Trim();
                                Fila["idnum_renglon1"] = rowItem["Id Numero Renglon 1"].ToString().Trim();
                                Fila["idtipo_operacion1"] = rowItem["Id Tipo Operacion1"].ToString().Trim();
                                Fila["idnum_renglon2"] = rowItem["Id Numero Renglon 2"].ToString().Trim();
                                Fila["idtipo_operacion2"] = rowItem["Id Tipo Operacion2"].ToString().Trim();
                                Fila["idnum_renglon3"] = rowItem["Id Numero Renglon 3"].ToString().Trim();
                                Fila["idtipo_operacion3"] = rowItem["Id Tipo Operacion3"].ToString().Trim();
                                Fila["idnum_renglon4"] = rowItem["Id Numero Renglon 4"].ToString().Trim();
                                Fila["idtipo_operacion4"] = rowItem["Id Tipo Operacion4"].ToString().Trim();
                                Fila["idnum_renglon5"] = rowItem["Id Numero Renglon 5"].ToString().Trim();
                                Fila["idtipo_operacion5"] = rowItem["Id Tipo Operacion5"].ToString().Trim();
                                Fila["idnum_renglon6"] = rowItem["Id Numero Renglon 6"].ToString().Trim();
                                Fila["id_estado"] = rowItem["Id Estado"].ToString().Trim();
                                Fila["estado"] = rowItem["Estado"].ToString().Trim();
                                DtImpMunicipio.Rows.Add(Fila);
                                #endregion
                            }

                            //Aqui mandamos a mostrar en el Grid la Información.
                            if (DtImpMunicipio.Rows.Count > 0)
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
                                _MsgError = "El Proceso del archivo ha sido realizado correctamente con [" + DtImpMunicipio.Rows.Count + "] registros.";
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
                DtImpMunicipio = null;
                this.ViewState["DtImpMunicipio"] = null;
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
            this.ViewState["DtActEconomica"] = null;
            this.BtnCargar.Visible = true;
            this.BtnProcesar.Visible = false;
            this.BtnCancelar.Visible = false;
            this.RadGrid1.Rebind();
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtImpMunicipio = new DataTable();
            try
            {
                string _ErrorProcesar = "N";
                DtImpMunicipio = (DataTable)this.ViewState["DtImpMunicipio"];
                if (DtImpMunicipio != null)
                {
                    if (DtImpMunicipio.Rows.Count > 0)
                    {
                        int ContadorRow = DtImpMunicipio.Rows.Count;
                        ObjMunImpuesto.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjMunImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        string _ArrayData = "";
                        int _ContadorRow = 0;
                        int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                        foreach (DataRow rowItem in DtImpMunicipio.Rows)
                        {
                            #region OBTENER DATOS DEL DATATABLE
                            int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            int _IdFormulario = Int32.Parse(rowItem["id_formulario"].ToString().Trim());
                            string _IdFormConfiguracion = rowItem["idnum_renglon"].ToString().Trim();
                            string _IdPeriodicidad = rowItem["id_periodicidad"].ToString().Trim();
                            //string _Descripcion1 = rowItem["descripcion"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["descripcion"].ToString().Trim()) : "NA";
                            //string _Descripcion = _Descripcion1.ToString().Trim().Length > 60 ? _Descripcion1.ToString().Trim().Substring(0, 60) : _Descripcion1.ToString().Trim();
                            string _Descripcion = rowItem["descripcion"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["descripcion"].ToString().Trim()) : "NA";
                            int _OperacionRenglon = 0; //--DBNull.Value;
                            int _IdTipoTarifa = rowItem["idtipo_tarifa"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim()) : 0;
                            string _ValorTarifa = rowItem["valor_tarifa"].ToString().Trim().Length > 0 ? rowItem["valor_tarifa"].ToString().Trim().Replace(",", ".") : "0";
                            string _Calcular = rowItem["para_calcular"].ToString().Trim();
                            int _IdNumRenglon1 = rowItem["idnum_renglon1"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon1"].ToString().Trim()) : 0;
                            int _IdTipoOperacion1 = rowItem["idtipo_operacion1"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion1"].ToString().Trim()) : 0;
                            int _IdNumRenglon2 = rowItem["idnum_renglon2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon2"].ToString().Trim()) : 0;
                            int _IdTipoOperacion2 = rowItem["idtipo_operacion2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion2"].ToString().Trim()) : 0;
                            int _IdNumRenglon3 = rowItem["idnum_renglon3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon3"].ToString().Trim()) : 0;
                            int _IdTipoOperacion3 = rowItem["idtipo_operacion3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion3"].ToString().Trim()) : 0;
                            int _IdNumRenglon4 = rowItem["idnum_renglon4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon4"].ToString().Trim()) : 0;
                            int _IdTipoOperacion4 = rowItem["idtipo_operacion4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion4"].ToString().Trim()) : 0;
                            int _IdNumRenglon5 = rowItem["idnum_renglon5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon5"].ToString().Trim()) : 0;
                            int _IdTipoOperacion5 = rowItem["idtipo_operacion5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion5"].ToString().Trim()) : 0;
                            int _IdNumRenglon6 = rowItem["idnum_renglon6"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon6"].ToString().Trim()) : 0;
                            int _IdEstado = Int32.Parse(rowItem["id_estado"].ToString().Trim());

                            //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                            if (_ArrayData.ToString().Trim().Length > 0)
                            {
                                _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _AnioGravable + "," + _IdMunicipio + "," + _IdFormulario + "," + _IdFormConfiguracion + "," + _IdPeriodicidad + "," + _Descripcion + "," + _OperacionRenglon + "," + _IdTipoTarifa + "," + _ValorTarifa + "," + _Calcular + "," + _IdNumRenglon1 + "," + _IdTipoOperacion1 + "," + _IdNumRenglon2 + "," + _IdTipoOperacion2 + "," + _IdNumRenglon3 + "," + _IdTipoOperacion3 + "," + _IdNumRenglon4 + "," + _IdTipoOperacion4 + "," + _IdNumRenglon5 + "," + _IdTipoOperacion5 + "," + _IdNumRenglon6 + "," + _IdEstado + ")" + quote;
                            }
                            else
                            {
                                _ArrayData = quote + "(" + _AnioGravable + "," + _IdMunicipio + "," + _IdFormulario + "," + _IdFormConfiguracion + "," + _IdPeriodicidad + "," + _Descripcion + "," + _OperacionRenglon + "," + _IdTipoTarifa + "," + _ValorTarifa + "," + _Calcular + "," + _IdNumRenglon1 + "," + _IdTipoOperacion1 + "," + _IdNumRenglon2 + "," + _IdTipoOperacion2 + "," + _IdNumRenglon3 + "," + _IdTipoOperacion3 + "," + _IdNumRenglon4 + "," + _IdTipoOperacion4 + "," + _IdNumRenglon5 + "," + _IdTipoOperacion5 + "," + _IdNumRenglon6 + "," + _IdEstado + ")" + quote;
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
                                ObjMunImpuesto.ArrayData = _ArrayData.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunImpuesto.GetLoadImpMunicipio(ref _IdRegistro, ref _MsgError))
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
                                    string _Mensaje = "Señor usuario, ocurrio un error al cargar el lote No. " + _CantidadLoteProcesado + ", en la linea del archivo No. [ " + this.ViewState["Contador"].ToString().Trim() + " ] del cargue de impuestos. Motivo: " + _MsgError;
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
                                ObjMunImpuesto.ArrayData = _ArrayData.ToString().Trim();
                                //ObjMunImpuesto.ArrayData = "(604, 1, 12, 8, IMPUESTO DE AVISOS Y TABLEROS 15 DEL RENGLON 20, NULL, 1, 15.00, S, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)";
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunImpuesto.GetLoadImpMunicipio(ref _IdRegistro, ref _MsgError))
                                {
                                    #region MOSTRAR MENSAJE DE USUARIO
                                    DtImpMunicipio = null;
                                    this.ViewState["DtImpMunicipio"] = null;
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