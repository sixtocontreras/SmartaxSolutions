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
    public partial class FrmLoadOtrasConfig : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        const string quote = "\"";

        #region DEFINICION DE OBJETOS DE CLASES
        MunicipioTarifasMinima ObjMunTarMin = new MunicipioTarifasMinima();
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
                this.ViewState["DtOtrasConfiguraciones"] = null;
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
                DataTable DtOtrasConfiguraciones = new DataTable();
                DtOtrasConfiguraciones = (DataTable)this.ViewState["DtOtrasConfiguraciones"];
                if (DtOtrasConfiguraciones != null)
                {
                    dtDatos = DtOtrasConfiguraciones.Copy();
                }
                else
                {
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    DtOtrasConfiguraciones = new DataTable();
                    DtOtrasConfiguraciones.TableName = "DtOtrasConfiguraciones";
                    DtOtrasConfiguraciones.Columns.Add("id_registro", typeof(Int32));
                    DtOtrasConfiguraciones.PrimaryKey = new DataColumn[] { DtOtrasConfiguraciones.Columns["id_registro"] };
                    DtOtrasConfiguraciones.Columns.Add("id_municipio");
                    DtOtrasConfiguraciones.Columns.Add("municipio");
                    DtOtrasConfiguraciones.Columns.Add("id_formulario");
                    DtOtrasConfiguraciones.Columns.Add("formulario");
                    DtOtrasConfiguraciones.Columns.Add("idunidad_medida");
                    DtOtrasConfiguraciones.Columns.Add("idunidad_medida_baseg");
                    DtOtrasConfiguraciones.Columns.Add("idunidad_medida_aniofiscal");
                    DtOtrasConfiguraciones.Columns.Add("unidad_medida");
                    DtOtrasConfiguraciones.Columns.Add("anio_gravable");
                    DtOtrasConfiguraciones.Columns.Add("anio_fiscal");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_tarifa");
                    DtOtrasConfiguraciones.Columns.Add("valor_unidad");
                    DtOtrasConfiguraciones.Columns.Add("porcentaje_unidad");
                    DtOtrasConfiguraciones.Columns.Add("cantidad_periodos");
                    DtOtrasConfiguraciones.Columns.Add("idform_configuracion");
                    DtOtrasConfiguraciones.Columns.Add("descripcion_config");
                    //--
                    DtOtrasConfiguraciones.Columns.Add("para_calcular");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon1");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_operacion1");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon2");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_operacion2");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon3");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_operacion3");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon4");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_operacion4");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon5");
                    DtOtrasConfiguraciones.Columns.Add("idtipo_operacion5");
                    DtOtrasConfiguraciones.Columns.Add("idnum_renglon6");
                    DtOtrasConfiguraciones.Columns.Add("id_estado");
                    DtOtrasConfiguraciones.Columns.Add("estado");
                    dtDatos = DtOtrasConfiguraciones.Copy();
                }

                this.ViewState["DtOtrasConfiguraciones"] = dtDatos;
            }
            catch (Exception ex)
            {
                dtDatos = null;
                this.ViewState["DtOtrasConfiguraciones"] = dtDatos;
                _log.Error("Error al generar Tabla. Motivo: " + ex.Message);
            }

            return dtDatos;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.GetTablaDatos();
                RadGrid1.DataMember = "DtOtrasConfiguraciones";
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

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            DataTable DtOtrasConfiguraciones = new DataTable();
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
                            DtOtrasConfiguraciones = new DataTable();
                            DtOtrasConfiguraciones = (DataTable)this.ViewState["DtOtrasConfiguraciones"];

                            //Recorremos las filas del Datatable
                            foreach (DataRow rowItem in dtEtl.Rows)
                            {
                                #region ESCRIBIR DATOS EN EL DATATABLE
                                DataRow Fila = null;
                                Fila = DtOtrasConfiguraciones.NewRow();
                                //--
                                Fila["id_registro"] = DtOtrasConfiguraciones.Rows.Count + 1;
                                Fila["id_municipio"] = rowItem["Id Municipio"].ToString().Trim();
                                string _NombreMunicipio = rowItem["Municipio"].ToString().Trim();
                                Fila["municipio"] = _NombreMunicipio;
                                Fila["id_formulario"] = rowItem["Id Formulario"].ToString().Trim();
                                Fila["formulario"] = rowItem["Formulario"].ToString().Trim();
                                Fila["idunidad_medida"] = rowItem["Id Unidad de medida"].ToString().Trim();
                                Fila["idunidad_medida_baseg"] = rowItem["Id Unidad de medida BaseG"].ToString().Trim();
                                Fila["idunidad_medida_aniofiscal"] = rowItem["Id Unidad de medida AnioFiscal"].ToString().Trim();
                                Fila["unidad_medida"] = rowItem["Unidad de Medida"].ToString().Trim();
                                Fila["anio_gravable"] = rowItem["Anio Gravable"].ToString().Trim();
                                Fila["anio_fiscal"] = rowItem["Anio Fiscal"].ToString().Trim();
                                Fila["idtipo_tarifa"] = rowItem["Id Tipo Tarifa"].ToString().Trim();
                                //---
                                string _ValorUnidad1 = rowItem["Valor Unidad"].ToString().Trim();
                                string _CantidadMedida1 = rowItem["Porcentaje_Cantidad Unidad"].ToString().Trim().Replace(".", ",");
                                //--VARIABLE 1. SEPARADOR DE MILES, 2. SEPARADOR DECIMALES
                                string _CantidadPeriodos1 = rowItem["Cantidad Periodos"].ToString().Trim(); //--.Replace(FixedData.SeparadorMiles, "");
                                string _CantidadPeriodos2 = _CantidadPeriodos1.Replace(".", ",");
                                //--
                                _log.Warn("PASO 1 => MUNICIPIO: " + _NombreMunicipio + ", VALOR UNIDAD: " + _ValorUnidad1 + ", CANTIDAD MEDIDA: " + _CantidadMedida1 +
                                    ", CANTIDAD PERIODOS 1: " + _CantidadPeriodos1 + ", CANTIDAD PERIODOS 2: " + _CantidadPeriodos2);
                                //--
                                Fila["valor_unidad"] = _ValorUnidad1;
                                Fila["porcentaje_unidad"] = _CantidadMedida1;
                                Fila["cantidad_periodos"] = _CantidadPeriodos2;
                                //_log.Warn("DATA PROCESAR EN LA DB => MUNICIPIO: " + _NombreMunicipio + ", CANTIDAD PERIODO: " + _CantidadPeriodos2);

                                Fila["idform_configuracion"] = rowItem["Id No. Renglon"].ToString().Trim();
                                Fila["descripcion_config"] = ObjUtils.GetLimpiarCadena(rowItem["Descripcion de la Configuracion"].ToString().Trim().ToUpper());
                                //--
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
                                DtOtrasConfiguraciones.Rows.Add(Fila);
                                #endregion
                            }

                            //Aqui mandamos a mostrar en el Grid la Información.
                            if (DtOtrasConfiguraciones.Rows.Count > 0)
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
                                _MsgError = "El Proceso del archivo ha sido realizado correctamente con [" + DtOtrasConfiguraciones.Rows.Count + "] registros.";
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
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                DtOtrasConfiguraciones = null;
                this.ViewState["DtOtrasConfiguraciones"] = null;
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
                //Aqui limpiamos los datos del Grid.
                this.RadGrid1.Rebind();
                #endregion
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.ViewState["DtOtrasConfiguraciones"] = null;
            this.BtnCargar.Visible = true;
            this.BtnProcesar.Visible = false;
            this.BtnCancelar.Visible = false;
            this.RadGrid1.Rebind();
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            DataTable DtOtrasConfiguraciones = new DataTable();
            try
            {
                string _ErrorProcesar = "N";
                DtOtrasConfiguraciones = (DataTable)this.ViewState["DtOtrasConfiguraciones"];
                if (DtOtrasConfiguraciones != null)
                {
                    if (DtOtrasConfiguraciones.Rows.Count > 0)
                    {
                        int ContadorRow = DtOtrasConfiguraciones.Rows.Count;
                        ObjMunTarMin.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjMunTarMin.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        string _ArrayData = "";
                        int _ContadorRow = 0;
                        int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                        foreach (DataRow rowItem in DtOtrasConfiguraciones.Rows)
                        {
                            #region OBTENER DATOS DEL DATATABLE
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            string _NombreMunicipio = rowItem["municipio"].ToString().Trim();
                            int _IdFormulario = Int32.Parse(rowItem["id_formulario"].ToString().Trim());
                            int _IdUnidadMedida = Int32.Parse(rowItem["idunidad_medida"].ToString().Trim());
                            int _IdValorUnidMedida = Int32.Parse(rowItem["idunidad_medida_aniofiscal"].ToString().Trim());
                            int _IdUnidadMedidaBaseG = Int32.Parse(rowItem["idunidad_medida_baseg"].ToString().Trim());
                            int _IdTipoTarifa = Int32.Parse(rowItem["idtipo_tarifa"].ToString().Trim());
                            //---
                            string _ValorUnidad1 = rowItem["valor_unidad"].ToString().Trim();
                            string _CantidadMedida1 = rowItem["porcentaje_unidad"].ToString().Trim();
                            string _CantidadPeriodos1 = rowItem["cantidad_periodos"].ToString().Trim();
                            _log.Warn("PASO 2 => MUNICIPIO: " + _NombreMunicipio + ", VALOR UNIDAD: " + _ValorUnidad1 + ", CANTIDAD MEDIDA: " + _CantidadMedida1 +
                                ", CANTIDAD PERIODOS: " + _CantidadPeriodos1);
                            //--
                            int ValorConcepto = 0;
                            //double _ValorUnidad = Double.Parse(rowItem["valor_unidad"].ToString().Trim());
                            //double _CantidadMedida = Double.Parse(rowItem["porcentaje_unidad"].ToString().Trim());
                            //double _CantidadPeriodos = Double.Parse(rowItem["cantidad_periodos"].ToString().Trim());
                            string _ValorUnidad = rowItem["valor_unidad"].ToString().Trim().Replace(FixedData.SeparadorDecimalesFile, ".");
                            string _CantidadMedida = rowItem["porcentaje_unidad"].ToString().Trim().Replace(FixedData.SeparadorDecimalesFile, ".");
                            string _CantidadPeriodos = rowItem["cantidad_periodos"].ToString().Trim().Replace(FixedData.SeparadorDecimalesFile, ".");
                            string _IdFormConfiguracion = rowItem["idform_configuracion"].ToString().Trim();
                            string _Descripcion = rowItem["descripcion_config"].ToString().Trim().Length > 0 ? ObjUtils.GetLimpiarCadena(rowItem["descripcion_config"].ToString().Trim()) : "NA";
                            //---
                            _log.Warn("PASO 3 => MUNICIPIO: " + _NombreMunicipio + ", VALOR UNIDAD: " + _ValorUnidad + ", CANTIDAD MEDIDA: " + _CantidadMedida +
                                ", CANTIDAD PERIODOS: " + _CantidadPeriodos);
                            //--
                            string _Calcular = rowItem["para_calcular"].ToString().Trim();
                            int _IdFormConfiguracion1 = rowItem["idnum_renglon1"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon1"].ToString().Trim()) : 0;
                            int _IdTipoOperacion1 = rowItem["idtipo_operacion1"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion1"].ToString().Trim()) : 0;
                            int _IdFormConfiguracion2 = rowItem["idnum_renglon2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon2"].ToString().Trim()) : 0;
                            int _IdTipoOperacion2 = rowItem["idtipo_operacion2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion2"].ToString().Trim()) : 0;
                            int _IdFormConfiguracion3 = rowItem["idnum_renglon3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon3"].ToString().Trim()) : 0;
                            int _IdTipoOperacion3 = rowItem["idtipo_operacion3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion3"].ToString().Trim()) : 0;
                            int _IdFormConfiguracion4 = rowItem["idnum_renglon4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon4"].ToString().Trim()) : 0;
                            int _IdTipoOperacion4 = rowItem["idtipo_operacion4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion4"].ToString().Trim()) : 0;
                            int _IdFormConfiguracion5 = rowItem["idnum_renglon5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon5"].ToString().Trim()) : 0;
                            int _IdTipoOperacion5 = rowItem["idtipo_operacion5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion5"].ToString().Trim()) : 0;
                            int _IdFormConfiguracion6 = rowItem["idnum_renglon6"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idnum_renglon6"].ToString().Trim()) : 0;
                            int _IdEstado = Int32.Parse(rowItem["id_estado"].ToString().Trim());

                            //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                            if (_ArrayData.ToString().Trim().Length > 0)
                            {
                                _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _IdMunicipio + "," + _IdFormulario + "," + _IdUnidadMedida + "," + _IdValorUnidMedida + "," + _IdTipoTarifa + "," + _CantidadMedida + "," + _CantidadPeriodos + "," + _IdUnidadMedidaBaseG + "," + _ValorUnidad + "," + _Descripcion + "," + _Calcular + "," + _IdFormConfiguracion + "," + _IdFormConfiguracion1 + "," + _IdTipoOperacion1 + "," + _IdFormConfiguracion2 + "," + _IdTipoOperacion2 + "," + _IdFormConfiguracion3 + "," + _IdTipoOperacion3 + "," + _IdFormConfiguracion4 + "," + _IdTipoOperacion4 + "," + _IdFormConfiguracion5 + "," + _IdTipoOperacion5 + "," + _IdFormConfiguracion6 + "," + _IdEstado + ")" + quote;
                            }
                            else
                            {
                                _ArrayData = quote + "(" + _IdMunicipio + "," + _IdFormulario + "," + _IdUnidadMedida + "," + _IdValorUnidMedida + "," + _IdTipoTarifa + "," + _CantidadMedida + "," + _CantidadPeriodos + "," + _IdUnidadMedidaBaseG + "," + _ValorUnidad + "," + _Descripcion + "," + _Calcular + "," + _IdFormConfiguracion + "," + _IdFormConfiguracion1 + "," + _IdTipoOperacion1 + "," + _IdFormConfiguracion2 + "," + _IdTipoOperacion2 + "," + _IdFormConfiguracion3 + "," + _IdTipoOperacion3 + "," + _IdFormConfiguracion4 + "," + _IdTipoOperacion4 + "," + _IdFormConfiguracion5 + "," + _IdTipoOperacion5 + "," + _IdFormConfiguracion6 + "," + _IdEstado + ")" + quote;
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
                                ObjMunTarMin.ArrayData = _ArrayData.ToString().Trim();
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunTarMin.GetLoadOtrasConfig(ref _IdRegistro, ref _MsgError))
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
                                ObjMunTarMin.ArrayData = _ArrayData.ToString().Trim();
                                //ObjMunTarMin.ArrayData = "(604, 1, 12, 8, IMPUESTO DE AVISOS Y TABLEROS 15 DEL RENGLON 20, NULL, 1, 15.00, S, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)";
                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjMunTarMin.GetLoadOtrasConfig(ref _IdRegistro, ref _MsgError))
                                {
                                    #region MOSTRAR MENSAJE DE USUARIO
                                    DtOtrasConfiguraciones = null;
                                    this.ViewState["DtOtrasConfiguraciones"] = null;
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

    }
}