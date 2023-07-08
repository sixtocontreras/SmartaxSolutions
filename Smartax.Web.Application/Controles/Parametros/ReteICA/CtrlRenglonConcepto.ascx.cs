using log4net;
using Smartax.Web.Application.Clases.Parametros.ReteICA;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Parametros.ReteICA
{
    public partial class CtrlRenglonConcepto : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        ConfRenglonConcepto confReglonConcepto = new ConfRenglonConcepto();
        Utilidades ObjUtils = new Utilidades();
        List<DatoArchivo> datoArchivos = new List<DatoArchivo>
        {
            new DatoArchivo(0, "Producto"),
            new DatoArchivo(1, "Saldo inicial"),
            new DatoArchivo(2, "Débito"),
            new DatoArchivo(3, "Crédito"),
            new DatoArchivo(4, "Saldo final"),
            new DatoArchivo(5, "Débitos - Créditos"),
            new DatoArchivo(6, "Créditos - Débitos"),
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Page.Title + "REGISTRAR RENGLON CONCEPTOS RETEICA";
            ObjUtils.CambiarGrillaAEspanol(RadGrid1);

            if (!IsPostBack)
            {
                RadGrid1.DataSource = GetDatosGrilla();
                RadGrid1.DataBind();
            }
        }

        public DataTable GetDatosGrilla()
        {
            try
            {
                return confReglonConcepto.GetAll();
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
                string _MsgError = "Error al listar los tipos de convenio. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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
            return null;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = GetDatosGrilla();
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("renglon");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator
                    {
                        ControlToValidate = editor.TextBoxControl.ID,
                        ErrorMessage = "Campo Requerido",
                        Display = ValidatorDisplay.Dynamic
                    };
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    GridTextBoxColumnEditor editor1 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("cuenta");
                    TableCell cell2 = (TableCell)editor1.TextBoxControl.Parent;
                    RequiredFieldValidator validator1 = new RequiredFieldValidator
                    {
                        ControlToValidate = editor1.TextBoxControl.ID,
                        ErrorMessage = "Campo Requerido",
                        Display = ValidatorDisplay.Dynamic
                    };
                    cell2.Controls.Add(validator1);
                    editor1.Visible = true;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated del tipo de comision. Motivo: " + ex.ToString();
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow2";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                var idUsuario = $"{Session["IdUsuario"]}";
                var renglon = int.Parse($"{newValues["renglon"]}");
                var cuenta = $"{newValues["cuenta"]}";
                var combo = editedItem.FindControl("datoarchivoCombo") as RadComboBox;
                var dato = string.IsNullOrEmpty(combo.SelectedValue) ? (int?)null : int.Parse($"{combo.SelectedValue}");
                confReglonConcepto.Insert(idUsuario, renglon, cuenta, dato);
            }
            catch (Exception ex)
            {
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
            }
        }

        protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                var idUsuario = $"{Session["IdUsuario"]}";
                var renglon = int.Parse($"{newValues["renglon"]}");
                var cuenta = $"{newValues["cuenta"]}";
                var combo = editedItem.FindControl("datoarchivoCombo") as RadComboBox;
                var dato = string.IsNullOrEmpty(combo.SelectedValue) ? (int?)null : int.Parse($"{combo.SelectedValue}");
                var oldRenglon = int.Parse(editedItem.OwnerTableView.Items[editedItem.ItemIndex]["renglon"].Text);
                var oldCuenta = editedItem.OwnerTableView.Items[editedItem.ItemIndex]["cuenta"].Text;
                confReglonConcepto.Update(idUsuario, renglon, cuenta, dato, oldRenglon, oldCuenta);
            }
            catch (Exception ex)
            {
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
            }
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                var renglon = int.Parse($"{newValues["renglon"]}");
                var cuenta = $"{newValues["cuenta"]}";
                confReglonConcepto.Delete(renglon, cuenta);
            }
            catch (Exception ex)
            {
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadComboBox Field1Combo = e.Item.FindControl("datoarchivoCombo") as RadComboBox;
                Field1Combo.DataSource = datoArchivos;
                Field1Combo.DataValueField = "Id";
                Field1Combo.DataTextField = "Value";
                Field1Combo.DataBind();
                Field1Combo.EmptyMessage = "Seleccione";

                var item = e.Item.DataItem;
                var selectedItem = $"{(item as DataRowView)?.Row["datoarchivo"]}";
                Field1Combo.SelectedValue = selectedItem;
            }
        }
    }

    class DatoArchivo
    {
        public DatoArchivo(int id, string value)
        {
            Id = id;
            Value = value;
        }
        public int Id { get; private set; }

        public string Value { get; private set; }
    }
}