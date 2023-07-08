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

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmMunicipioCalendarioTrib : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        MunicipioCalendarioTributario ObjMunCalendario = new MunicipioCalendarioTributario();
        FormularioImpuesto ObjFormImpuesto = new FormularioImpuesto();
        PeriodicidadPagos ObjPeriodicidad = new PeriodicidadPagos();
        Estado ObjEstado = new Estado();
        Lista ObjLista = new Lista();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjMunCalendario.TipoConsulta = 1;
                ObjMunCalendario.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunCalendario.IdCliente = null;
                ObjMunCalendario.IdEstado = null;
                ObjMunCalendario.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los impuestos por municipio
                ObjetoDataTable = ObjMunCalendario.GetAllMunCalendarioTrib();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idmun_calendario_trib"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar las Formulario de Calendario Tributario
                //ObjetoDataTable = new DataTable();
                //ObjFormImpuesto.TipoConsulta = 2;
                //ObjFormImpuesto.MostrarSeleccione = "NO";
                //ObjFormImpuesto.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                //ObjFormImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                //ObjetoDataTable = ObjFormImpuesto.GetFormularioImpuesto();
                //ObjetoDataSet.Tables.Add(ObjetoDataTable);

                ////Mostrar las Periodicidades de Pago
                //ObjetoDataTable = new DataTable();
                ////--
                //ObjPeriodicidad.TipoConsulta = 1;
                //ObjPeriodicidad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                //ObjPeriodicidad.IdFormularioImpuesto = null;
                //ObjPeriodicidad.IdEstado = 1;
                //ObjPeriodicidad.MostrarSeleccione = "NO";
                //ObjPeriodicidad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //DataTable dtPeriodicidad = new DataTable();
                //dtPeriodicidad = ObjPeriodicidad.GetPeriodicidadMunicipio();
                //if (dtPeriodicidad != null)
                //{
                //    if (dtPeriodicidad.Rows.Count > 0)
                //    {
                //        foreach (DataRow rowItem in dtPeriodicidad.Rows)
                //        {
                //            #region OBTENER DATOS DEL DATATABLE
                //            int _IdPeriodicidad = Int32.Parse(rowItem["id_periodicidad"].ToString().Trim());
                //            ObjPeriodicidad.TipoConsulta = 1;
                //            ObjPeriodicidad.IdPeriodicidad = _IdPeriodicidad;
                //            ObjPeriodicidad.IdEstado = 1;
                //            ObjPeriodicidad.MostrarSeleccione = "NO";
                //            ObjPeriodicidad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                //            ObjetoDataTable = ObjPeriodicidad.GetPeriodicidadImpuesto();
                //            ObjetoDataSet.Tables.Add(ObjetoDataTable);
                //            break;
                //            #endregion
                //        }
                //    }
                //    else
                //    {
                //        //--
                //        //DataTable TablaDatos = new DataTable();
                //        //TablaDatos.TableName = "DtPeriodicidadPagos";
                //        //TablaDatos.Columns.Add("id_periodicidad");
                //        //TablaDatos.Columns.Add("periodicidad_pago");
                //        //TablaDatos.Rows.Add("", "<< Seleccione >>");
                //        //DataRow Fila = null;
                //        //Fila = TablaDatos.NewRow();
                //        //Fila["id_periodicidad"] = "-1";
                //        //Fila["periodicidad_pago"] = "NA";
                //        //TablaDatos.Rows.Add(Fila);
                //        ObjetoDataSet.Tables.Add(dtPeriodicidad);
                //    }
                //}

                //Mostrar las Formulario de Calendario Tributario
                //ObjetoDataTable = new DataTable();
                //ObjLista.MostrarSeleccione = "NO";
                //ObjetoDataTable = ObjLista.GetAnios();
                //ObjetoDataSet.Tables.Add(ObjetoDataTable);

                ////Mostrar los Estados
                //ObjetoDataTable = new DataTable();
                //ObjEstado.TipoConsulta = 2;
                //ObjEstado.TipoEstado = "INTERFAZ";
                //ObjEstado.MostrarSeleccione = "NO";
                //ObjEstado.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                //ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                //ObjetoDataTable = ObjEstado.GetEstados();
                //ObjetoDataSet.Tables.Add(ObjetoDataTable);
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
                string _MsgError = "Error al listar el calendario tributario. Motivo: " + ex.ToString();
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
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //---OBTENER PARAMETROS 
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                this.ViewState["NombreMunicipio"] = Request.QueryString["NombreMunicipio"].ToString().Trim();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                if (this.ViewState["TipoProceso"].ToString().Trim().Equals("2"))
                {
                    this.LbTitulo.Text = "CALENDARIO TRIBUTARIO DEL MUNICIPIO";
                    this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;
                }
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtMunCalendarioTrib";

                //GridDropDownColumn columna = new GridDropDownColumn();
                ////--Lista de Formulario de impuestos
                //columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                //columna.DataSourceID = this.RadGrid1.DataSourceID;
                //columna.HeaderText = "Formulario";
                //columna.DataField = "idformulario_impuesto";
                //columna.ListTextField = "descripcion_formulario";
                //columna.ListValueField = "idformulario_impuesto";
                //columna.ListDataMember = "DtFormularioImpuesto";

                ////--Lista de Periodidad Pago
                //columna = (GridDropDownColumn)this.RadGrid1.Columns[4];
                //columna.DataSourceID = this.RadGrid1.DataSourceID;
                //columna.HeaderText = "Periodicidad";
                //columna.DataField = "idperiodicidad_impuesto";
                //columna.ListTextField = "periodicidad_impuesto";
                //columna.ListValueField = "idperiodicidad_impuesto";
                //columna.ListDataMember = "DtPeriodicidadImpuesto";

                ////--Lista de Formulario de impuestos
                //columna = (GridDropDownColumn)this.RadGrid1.Columns[6];
                //columna.DataSourceID = this.RadGrid1.DataSourceID;
                //columna.HeaderText = "Año Gravable";
                //columna.DataField = "id_anio";
                //columna.ListTextField = "descripcion_anio";
                //columna.ListValueField = "id_anio";
                //columna.ListDataMember = "DtAnios";

                ////--Lista de Estados
                //columna = (GridDropDownColumn)this.RadGrid1.Columns[11];
                //columna.DataSourceID = this.RadGrid1.DataSourceID;
                //columna.HeaderText = "Estado";
                //columna.DataField = "id_estado";
                //columna.ListTextField = "codigo_estado";
                //columna.ListValueField = "id_estado";
                //columna.ListDataMember = "DtEstados";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Calendario Tributario del municipio. Motivo: " + ex.ToString();
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAddCalendario")
                {
                    #region REGISTRAR CALENDARIO TRIBUTARIO
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _IdMunCalendarioTrib = "-1";
                    string _IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    string _TipoProceso = "INSERT";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Parametros/Divipola/FrmAddCalendarioTributario.aspx?IdMunCalendarioTrib=" + _IdMunCalendarioTrib + "&IdMunicipio=" + _IdMunicipio + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(440);
                    Ventana.Width = Unit.Pixel(950);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Registrar Calendario Tributario del Municipio: " + this.ViewState["NombreMunicipio"].ToString().Trim();
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnEditar")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EDITAR TARIFA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdMunCalendarioTrib = Int32.Parse(item.GetDataKeyValue("idmun_calendario_trib").ToString().Trim());

                    //--Mandamos abrir el formulario de registro
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    string _TipoProceso = "UPDATE";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Parametros/Divipola/FrmAddCalendarioTributario.aspx?IdMunCalendarioTrib=" + _IdMunCalendarioTrib + "&IdMunicipio=" + _IdMunicipio + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(440);
                    Ventana.Width = Unit.Pixel(950);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Editar Calendario Tributario Id: " + _IdMunCalendarioTrib;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnActualizarLista")
                {
                    #region ACTUALIAZR LISTA DE TARIFAS
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    //--Actualizar la lista de productos.
                    this.ViewState["_FuenteDatos"] = null;
                    this.RadGrid1.Rebind();
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

                    string _ModuloApp = "CALENDARIO_MUNICIPIO";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(440);
                    Ventana.Width = Unit.Pixel(950);
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
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje.Trim());
                #endregion
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (((e.Item is GridEditableItem) && (((GridEditableItem)e.Item).IsInEditMode)))
            {
                try
                {
                    //DataRowView row = (DataRowView)e.Item.DataItem;
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridEditableItem ItemEditado = (GridEditableItem)e.Item;
                    GridDropDownListColumnEditor CmbAnioGravable = (GridDropDownListColumnEditor)item.EditManager.GetColumnEditor("id_anio");

                    //if (row != null)
                    //{
                    //CmbAnioGravable.SelectedValue = DateTime.Now.ToString("yyyy");
                    //}

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
                    string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemDataBound. Motivo: " + ex.ToString();
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(230);
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

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    //--Validar campo fecha inicial
                    //GridDateTimeColumnEditor editor = (GridDateTimeColumnEditor)item.EditManager.GetColumnEditor("fecha_limite");
                    //TableCell cell1 = (TableCell)editor.SharedCalendar.Parent;
                    //RequiredFieldValidator validator = new RequiredFieldValidator();
                    //validator.ControlToValidate = editor.TextBoxControl.ID;
                    //validator.ErrorMessage = "Campo Requerido";
                    //validator.Display = ValidatorDisplay.Dynamic;
                    //cell1.Controls.Add(validator);
                    //editor.Visible = true;

                    ////--Validar campo fecha final
                    //GridDateTimeColumnEditor editor2 = (GridDateTimeColumnEditor)item.EditManager.GetColumnEditor("fecha_final");
                    //TableCell cell2 = (TableCell)editor2.SharedCalendar.Parent;
                    //RequiredFieldValidator validator2 = new RequiredFieldValidator();
                    //validator2.ControlToValidate = editor2.TextBoxControl.ID;
                    //validator2.ErrorMessage = "Campo Requerido";
                    //validator2.Display = ValidatorDisplay.Dynamic;
                    //cell2.Controls.Add(validator2);
                    //editor2.Visible = true;

                    //--Validar campo valor tarifa
                    GridNumericColumnEditor editor4 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("valor_descuento");
                    TableCell cell4 = (TableCell)editor4.NumericTextBox.Parent;
                    RequiredFieldValidator validator4 = new RequiredFieldValidator();
                    validator4.ControlToValidate = editor4.NumericTextBox.ID;
                    validator4.ErrorMessage = "Campo Requerido";
                    validator4.Display = ValidatorDisplay.Dynamic;
                    cell4.Controls.Add(validator4);
                    editor4.Visible = true;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated del Calendario Tributario del municipio. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del Calendario Tributario del municipio. Motivo: " + ex.ToString();
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

        #region DEFINICION DEL CRUD
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtMunCalendarioTrib"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_calendario_trib"] };
            DataRow[] TodosValores = TablaDatos.Select("", "idmun_calendario_trib", DataViewRowState.CurrentRows); ;

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    NuevaFila[(string)entry.Key] = entry.Value;
                }

                if ((NuevaFila != null))
                {
                    string CmbFormulario = (editedItem["idformulario_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbPeriodicidad = (editedItem["idperiodicidad_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                    ObjMunCalendario.IdMunCalendarioTrib = null;
                    ObjMunCalendario.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    ObjMunCalendario.IdFormularioImpuesto = NuevaFila["idformulario_impuesto"].ToString().Trim();
                    ObjMunCalendario.FechaLimite = NuevaFila["fecha_limite"].ToString().Trim();
                    ObjMunCalendario.ValorDescuento = NuevaFila["valor_descuento"].ToString().Trim();
                    ObjMunCalendario.IdEstado = NuevaFila["id_estado"].ToString().Trim();
                    ObjMunCalendario.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjMunCalendario.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjMunCalendario.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjMunCalendario.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjMunCalendario);
                    _log.Warn("REQUEST INSERT CALENDARIO MUNICIPIO => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjMunCalendario.AddUpMunCalendarioTrib(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        #region MOSTRAR DATOS REGISTRADOS EN EL GRID
                        NuevaFila["idmun_calendario_trib"] = _IdRegistro;
                        NuevaFila["descripcion_formulario"] = CmbFormulario.ToString().Trim();
                        NuevaFila["descripcion_anio"] = NuevaFila["id_anio"].ToString().Trim();
                        NuevaFila["fecha_limite"] = Convert.ToDateTime(NuevaFila["fecha_limite"].ToString().Trim()).ToString("dd-MM-yyyy");
                        //NuevaFila["fecha_final"] = Convert.ToDateTime(NuevaFila["fecha_final"].ToString().Trim()).ToString("yyyy-MM-dd");
                        NuevaFila["valor_descuento1"] = NuevaFila["valor_descuento"].ToString().Trim() + "%";
                        NuevaFila["valor_descuento"] = NuevaFila["valor_descuento"].ToString().Trim();
                        NuevaFila["periodicidad_impuesto"] = CmbPeriodicidad.ToString().Trim();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        this.FuenteDatos.Tables["DtMunCalendarioTrib"].Rows.Add(NuevaFila);
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "CALENDARIO_MUNICIPIO";
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
                        _log.Info(_MsgError);
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
                string _MsgError = "Error al registrar el Calendario Tributario del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtMunCalendarioTrib"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_calendario_trib"] };
            DataRow[] changedRows = TablaDatos.Select("idmun_calendario_trib = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idmun_calendario_trib"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idmun_calendario_trib"].ToString().Trim());

            if (changedRows.Length != 1)
            {
                e.Canceled = true;
                return;
            }

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            changedRows[0].BeginEdit();

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }

                string CmbFormulario = (editedItem["idformulario_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbPeriodicidad = (editedItem["idperiodicidad_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjMunCalendario.IdMunCalendarioTrib = _IdRegistro;
                ObjMunCalendario.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunCalendario.IdFormularioImpuesto = changedRows[0]["idformulario_impuesto"].ToString().Trim();
                ObjMunCalendario.FechaLimite = changedRows[0]["fecha_limite"].ToString().Trim();
                ObjMunCalendario.ValorDescuento = changedRows[0]["valor_descuento"].ToString().Trim();
                ObjMunCalendario.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjMunCalendario.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunCalendario.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunCalendario.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjMunCalendario.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunCalendario);
                _log.Warn("REQUEST UPDATE CALENDARIO MUNICIPIO => " + jsonRequest);

                string _MsgError = "";
                if (ObjMunCalendario.AddUpMunCalendarioTrib(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    #region MOSTRAR DATOS EDITADOS EN EL GRID
                    changedRows[0]["descripcion_formulario"] = CmbFormulario.ToString().Trim().ToUpper();
                    changedRows[0]["descripcion_anio"] = changedRows[0]["id_anio"].ToString().Trim();
                    changedRows[0]["fecha_limite"] = Convert.ToDateTime(changedRows[0]["fecha_limite"].ToString().Trim()).ToString("dd-MM-yyyy");
                    //changedRows[0]["fecha_final"] = Convert.ToDateTime(changedRows[0]["fecha_final"].ToString().Trim()).ToString("yyyy-MM-dd");
                    changedRows[0]["valor_descuento1"] = changedRows[0]["valor_descuento"].ToString().Trim() + "%";
                    changedRows[0]["valor_descuento"] = changedRows[0]["valor_descuento"].ToString().Trim();
                    changedRows[0]["periodicidad_impuesto"] = CmbPeriodicidad.ToString().Trim().ToUpper();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtMunCalendarioTrib"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtMunCalendarioTrib"].Rows[0].EndEdit();
                    #endregion

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "CALENDARIO_MUNICIPIO";
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
                    _log.Info(_MsgError);
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
                string _MsgError = "Error al editar el Calendario Tributario del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtMunCalendarioTrib"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_calendario_trib"] };
            DataRow[] changedRows = TablaDatos.Select("idmun_calendario_trib = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idmun_calendario_trib"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idmun_calendario_trib"].ToString().Trim());

            if (changedRows.Length != 1)
            {
                e.Canceled = true;
                return;
            }

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            changedRows[0].BeginEdit();

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }

                string _MsgError = "";
                ObjMunCalendario.IdMunCalendarioTrib = _IdRegistro;
                ObjMunCalendario.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunCalendario.IdFormularioImpuesto = changedRows[0]["idformulario_impuesto"].ToString().Trim();
                ObjMunCalendario.FechaLimite = changedRows[0]["fecha_limite"].ToString().Trim();
                ObjMunCalendario.ValorDescuento = changedRows[0]["valor_descuento"].ToString().Trim();
                ObjMunCalendario.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjMunCalendario.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunCalendario.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunCalendario.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjMunCalendario.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunCalendario);
                _log.Warn("REQUEST DELETE CALENDARIO MUNICIPIO => " + jsonRequest);

                if (ObjMunCalendario.AddUpMunCalendarioTrib(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtMunCalendarioTrib"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtMunCalendarioTrib"].AcceptChanges();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "CALENDARIO_MUNICIPIO";
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
                    _log.Info(_MsgError);
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
                string _MsgError = "Error al eliminar el Calendario Tributario del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }
        #endregion
    }
}