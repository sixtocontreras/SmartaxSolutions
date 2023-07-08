using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Parametros;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmMunicipioActividadEcon : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        MunicipioActividadEconomica ObjMunActividad = new MunicipioActividadEconomica();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Lista ObjLista = new Lista();
        TiposActividad ObjTiposAct = new TiposActividad();
        TiposTarifa ObjTiposTarifa = new TiposTarifa();
        Estado ObjEstado = new Estado();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjMunActividad.TipoConsulta = 1;
                ObjMunActividad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunActividad.IdMunActividadEconomica = null;
                ObjMunActividad.IdEstado = null;
                ObjMunActividad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los impuestos por municipio
                ObjetoDataTable = ObjMunActividad.GetAllMunActEconomica();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idmun_act_economica"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //--Mostrar la lista de impuestos
                ObjetoDataTable = new DataTable();
                ObjFrmImpuesto.TipoConsulta = 2;
                ObjFrmImpuesto.IdEstado = 1;
                ObjFrmImpuesto.MostrarSeleccione = "NO";
                ObjFrmImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjFrmImpuesto.GetFormularioImpuesto();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar lista de años
                ObjetoDataTable = new DataTable();
                ObjLista.MostrarSeleccione = "NO";
                ObjetoDataTable = ObjLista.GetAnios();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de actividades economicas
                ObjetoDataTable = new DataTable();
                ObjTiposAct.TipoConsulta = 2;
                ObjTiposAct.MostrarSeleccione = "NO";
                ObjTiposAct.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTiposAct.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjTiposAct.GetTipoActividad();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar las Formulario de impuesto
                ObjetoDataTable = new DataTable();
                ObjTiposTarifa.TipoConsulta = 2;
                ObjTiposTarifa.MostrarSeleccione = "NO";
                ObjTiposTarifa.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTiposTarifa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjTiposTarifa.GetTipoTarifa();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Estados
                ObjetoDataTable = new DataTable();
                ObjEstado.TipoConsulta = 2;
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MostrarSeleccione = "NO";
                ObjEstado.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjEstado.GetEstados();
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
                string _MsgError = "Error al listar los Actividad economicas. Motivo: " + ex.ToString();
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
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //---OBTENER PARAMETROS 
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                if (this.ViewState["TipoProceso"].ToString().Trim().Equals("2"))
                {
                    this.LbTitulo.Text = "ACTIVIDADES ECONOMICAS DEL MUNICIPIO";
                    this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 4].Visible = false;
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 9].Visible = false;
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
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtMunActividadEconomica";

                GridDropDownColumn columna = new GridDropDownColumn();
                //--Listar los tipos de impuestos
                columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Impuesto";
                columna.DataField = "idformulario_impuesto";
                columna.ListTextField = "descripcion_formulario";
                columna.ListValueField = "idformulario_impuesto";
                columna.ListDataMember = "DtFormularioImpuesto";

                //--Listar los años gravable
                columna = (GridDropDownColumn)this.RadGrid1.Columns[4];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Año Gravable";
                columna.DataField = "id_anio";
                columna.ListTextField = "descripcion_anio";
                columna.ListValueField = "id_anio";
                columna.ListDataMember = "DtAnios";

                //--Listar los tipos de actividad economica
                columna = (GridDropDownColumn)this.RadGrid1.Columns[6];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Tipo Actividad";
                columna.DataField = "idtipo_actividad";
                columna.ListTextField = "tipo_actividad";
                columna.ListValueField = "idtipo_actividad";
                columna.ListDataMember = "DtTipoActividad";

                //--Listar los tipos de tarifas
                columna = (GridDropDownColumn)this.RadGrid1.Columns[11];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Tipo Tarifa";
                columna.DataField = "idtipo_tarifa";
                columna.ListTextField = "descripcion_tarifa";
                columna.ListValueField = "idtipo_tarifa";
                columna.ListDataMember = "DtTipoTarifa";

                //--Lista de Estados
                columna = (GridDropDownColumn)this.RadGrid1.Columns[17];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Estado";
                columna.DataField = "id_estado";
                columna.ListTextField = "codigo_estado";
                columna.ListValueField = "id_estado";
                columna.ListDataMember = "DtEstados";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Impuesto del municipio. Motivo: " + ex.ToString();
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
                if (e.CommandName == "BtnVerInfo")
                {
                    #region VER INFORMACION DETALLADA DE LA ACTIVIDAD ECONOMICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdMunActEconomica = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());

                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Parametros/Divipola/FrmVerInfoActividadEconomica.aspx?IdMunActEconomica=" + _IdMunActEconomica + "&IdMunicipio=" + this.ViewState["IdMunicipio"].ToString().Trim();
                    Ventana.ID = "RadWindow3";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(450);
                    Ventana.Width = Unit.Pixel(700);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "+ Información de la Actividad Id: " + _IdMunActEconomica;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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

                    string _ModuloApp = "ACT_ECONOMICA_MUNICIPIO";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(540);
                    Ventana.Width = Unit.Pixel(1250);
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

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    //--Validar campo codigo agrupacion
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("codigo_agrupacion");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    //--Validar campo codigo actividad
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("codigo_actividad");
                    TableCell cell2 = (TableCell)editor2.TextBoxControl.Parent;
                    RequiredFieldValidator validator2 = new RequiredFieldValidator();
                    validator2.ControlToValidate = editor2.TextBoxControl.ID;
                    validator2.ErrorMessage = "Campo Requerido";
                    validator2.Display = ValidatorDisplay.Dynamic;
                    cell2.Controls.Add(validator2);
                    editor2.Visible = true;

                    //--Validar campo descripcion actividad
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("descripcion_actividad");
                    TableCell cell3 = (TableCell)editor3.TextBoxControl.Parent;
                    RequiredFieldValidator validator3 = new RequiredFieldValidator();
                    validator3.ControlToValidate = editor3.TextBoxControl.ID;
                    validator3.ErrorMessage = "Campo Requerido";
                    validator3.Display = ValidatorDisplay.Dynamic;
                    cell3.Controls.Add(validator3);
                    editor3.Visible = true;

                    //--Validar campo valor tarifa
                    GridNumericColumnEditor editor4 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("tarifa_ley");
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated del Impuesto del municipio. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del Impuesto del municipio. Motivo: " + ex.ToString();
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
        #endregion

        #region DEFINICION DEL CRUD
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtMunActividadEconomica"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_act_economica"] };
            DataRow[] TodosValores = TablaDatos.Select("", "idmun_act_economica", DataViewRowState.CurrentRows); ;

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
                    string CmbTipoImpuesto = (editedItem["idformulario_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbAnio = (editedItem["id_anio"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbTipoActividad = (editedItem["idtipo_actividad"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbTipoTarifa = (editedItem["idtipo_tarifa"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                    ObjMunActividad.IdMunActividadEconomica = null;
                    ObjMunActividad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                    ObjMunActividad.IdTipoActividad = NuevaFila["idtipo_actividad"].ToString().Trim();
                    ObjMunActividad.CodigoActividad = NuevaFila["codigo_actividad"].ToString().Trim();
                    ObjMunActividad.DescripcionActividad = NuevaFila["descripcion_actividad"].ToString().Trim();
                    ObjMunActividad.IdTipoTarifa = NuevaFila["idtipo_tarifa"].ToString().Trim();
                    ObjMunActividad.TarifaLey = NuevaFila["tarifa_ley"].ToString().Trim();
                    ObjMunActividad.TarifaMunicipio = NuevaFila["tarifa_municipio"].ToString().Trim();
                    ObjMunActividad.NumeroAcuerdo = NuevaFila["numero_acuerdo"].ToString().Trim();
                    ObjMunActividad.NumeroArticulo = NuevaFila["numero_articulo"].ToString().Trim();
                    ObjMunActividad.IdEstado = NuevaFila["id_estado"].ToString().Trim();
                    ObjMunActividad.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjMunActividad.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjMunActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjMunActividad.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjMunActividad);
                    _log.Warn("REQUEST INSERT ACT. ECONOMICA MUNICIPIO => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjMunActividad.AddUpMunActEconomica(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        #region AQUI MOSTRAMOS LOS DATOS INGRESADOS EN EL GRID
                        NuevaFila["idmun_act_economica"] = _IdRegistro;
                        NuevaFila["descripcion_formulario"] = CmbTipoImpuesto.ToString().Trim();
                        NuevaFila["descripcion_anio"] = CmbAnio.ToString().Trim();
                        NuevaFila["tipo_actividad"] = CmbTipoActividad.ToString().Trim();
                        NuevaFila["codigo_agrupacion"] = NuevaFila["codigo_agrupacion"].ToString().Trim().ToUpper();
                        NuevaFila["codigo_actividad"] = NuevaFila["codigo_actividad"].ToString().Trim().ToUpper();
                        NuevaFila["descripcion_actividad"] = NuevaFila["descripcion_actividad"].ToString().Trim().ToUpper();
                        NuevaFila["descripcion_tarifa"] = CmbTipoTarifa.ToString().Trim();
                        NuevaFila["numero_acuerdo"] = NuevaFila["numero_acuerdo"].ToString().Trim().ToUpper();
                        NuevaFila["numero_articulo"] = NuevaFila["numero_articulo"].ToString().Trim().ToUpper();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim();
                        NuevaFila["fecha_registro"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                        this.FuenteDatos.Tables["DtMunActividadEconomica"].Rows.Add(NuevaFila);
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "ACT_ECONOMICA_MUNICIPIO";
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
                string _MsgError = "Error al registrar el Impuesto del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtMunActividadEconomica"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_act_economica"] };
            DataRow[] changedRows = TablaDatos.Select("idmun_act_economica = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idmun_act_economica"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idmun_act_economica"].ToString().Trim());

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

                string CmbTipoImpuesto = (editedItem["idformulario_impuesto"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbAnio = (editedItem["id_anio"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbTipoActividad = (editedItem["idtipo_actividad"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbTipoTarifa = (editedItem["idtipo_tarifa"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjMunActividad.IdMunActividadEconomica = _IdRegistro;
                ObjMunActividad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunActividad.IdTipoActividad = changedRows[0]["idtipo_actividad"].ToString().Trim();
                ObjMunActividad.CodigoActividad = changedRows[0]["codigo_actividad"].ToString().Trim();
                ObjMunActividad.DescripcionActividad = changedRows[0]["descripcion_actividad"].ToString().Trim();
                ObjMunActividad.IdTipoTarifa = changedRows[0]["idtipo_tarifa"].ToString().Trim();
                ObjMunActividad.TarifaLey = changedRows[0]["tarifa_ley"].ToString().Trim();
                ObjMunActividad.TarifaMunicipio = changedRows[0]["tarifa_municipio"].ToString().Trim();
                ObjMunActividad.NumeroAcuerdo = changedRows[0]["numero_acuerdo"].ToString().Trim();
                ObjMunActividad.NumeroArticulo = changedRows[0]["numero_articulo"].ToString().Trim();
                ObjMunActividad.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjMunActividad.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunActividad.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjMunActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjMunActividad.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunActividad);
                _log.Warn("REQUEST UPDATE ACT. ECONOMICA MUNICIPIO => " + jsonRequest);

                string _MsgError = "";
                if (ObjMunActividad.AddUpMunActEconomica(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    #region AQUI MOSTRAMOS LOS DATOS EDITADOS POR EL USUARIO EN EL GRID
                    changedRows[0]["descripcion_formulario"] = CmbTipoImpuesto.ToString().Trim();
                    changedRows[0]["descripcion_anio"] = CmbAnio.ToString().Trim();
                    changedRows[0]["tipo_actividad"] = CmbTipoActividad.ToString().Trim();
                    changedRows[0]["codigo_agrupacion"] = changedRows[0]["codigo_agrupacion"].ToString().Trim().ToUpper();
                    changedRows[0]["codigo_actividad"] = changedRows[0]["codigo_actividad"].ToString().Trim().ToUpper();
                    changedRows[0]["descripcion_actividad"] = changedRows[0]["descripcion_actividad"].ToString().Trim().ToUpper();
                    changedRows[0]["descripcion_tarifa"] = CmbTipoTarifa.ToString().Trim();
                    changedRows[0]["numero_acuerdo"] = changedRows[0]["numero_acuerdo"].ToString().Trim().ToUpper();
                    changedRows[0]["numero_articulo"] = changedRows[0]["numero_articulo"].ToString().Trim().ToUpper();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtMunActividadEconomica"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtMunActividadEconomica"].Rows[0].EndEdit();
                    #endregion

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "ACT_ECONOMICA_MUNICIPIO";
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
                string _MsgError = "Error al editar el Impuesto del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtMunActividadEconomica"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idmun_act_economica"] };
            DataRow[] changedRows = TablaDatos.Select("idmun_act_economica = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idmun_act_economica"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idmun_act_economica"].ToString().Trim());

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
                ObjMunActividad.IdMunActividadEconomica = _IdRegistro;
                ObjMunActividad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunActividad.IdTipoActividad = changedRows[0]["idtipo_actividad"].ToString().Trim();
                ObjMunActividad.CodigoActividad = changedRows[0]["codigo_actividad"].ToString().Trim();
                ObjMunActividad.DescripcionActividad = changedRows[0]["descripcion_actividad"].ToString().Trim();
                ObjMunActividad.IdTipoTarifa = changedRows[0]["idtipo_tarifa"].ToString().Trim();
                ObjMunActividad.TarifaLey = changedRows[0]["tarifa_ley"].ToString().Trim();
                ObjMunActividad.TarifaMunicipio = changedRows[0]["tarifa_municipio"].ToString().Trim();
                ObjMunActividad.NumeroAcuerdo = changedRows[0]["numero_acuerdo"].ToString().Trim();
                ObjMunActividad.NumeroArticulo = changedRows[0]["numero_articulo"].ToString().Trim();
                ObjMunActividad.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjMunActividad.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunActividad.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjMunActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjMunActividad.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunActividad);
                _log.Warn("REQUEST DELETE ACT. ECONOMICA MUNICIPIO => " + jsonRequest);

                if (ObjMunActividad.AddUpMunActEconomica(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtMunActividadEconomica"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtMunActividadEconomica"].AcceptChanges();
                    
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "ACT_ECONOMICA_MUNICIPIO";
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
                string _MsgError = "Error al eliminar el Impuesto del municipio. Motivo: " + ex.ToString();
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
                e.Canceled = true;
                #endregion
            }
        }
        #endregion
    }
}