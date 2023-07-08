using System;
using System.Web;
using Telerik.Web.UI;
using NativeExcel;
using System.Drawing;
using System.Data;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Reportes;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Parametros.Tipos;

namespace Smartax.Web.Application.Controles.Reportes.Administrativos
{
    public partial class CtrlRptBancoCuentas : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        private static string FormatoMonto = "#,##0.00;(#,##0.00)";
        private static string FormatoCantidad = "#,##0.00;($#,##0.00)";

        //--DEFINICION DE OBJETOS DE CLASES
        ClaseReportes ObjReporte = new ClaseReportes();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Estado ObjEstado = new Estado();

        protected void LstEstado()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEstado = null;
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MostrarSeleccione = "SI";
                ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbEstado.DataSource = ObjEstado.GetEstados();
                this.CmbEstado.DataValueField = "id_estado";
                this.CmbEstado.DataTextField = "codigo_estado";
                this.CmbEstado.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los estados. Motivo: " + ex.ToString();
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
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeExportar)
            {
                this.BtnGenerar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Reporte de Banco Cuentas";
                //Aplicar los permisos 
                this.AplicarPermisos();

                this.LstEstado();
            }
        }

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjReporte.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEstado.SelectedValue.ToString().Trim() : null;
                ObjReporte.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                string _MsgError = "";
                dtDatos = ObjReporte.GetRptBancoCuentas(ref _MsgError);
                if (_MsgError.ToString().Trim().Length == 0)
                {
                    if (dtDatos != null)
                    {
                        if (dtDatos.Rows.Count > 0)
                        {
                            this.LblMensaje.Text = "";
                            this.ExportarDatosExcel(dtDatos);
                        }
                        else
                        {
                            this.LblMensaje.ForeColor = Color.Red;
                            this.LblMensaje.Text = "No hay registros de comercios para exportar a excel. !";
                        }
                    }
                    else
                    {
                        this.LblMensaje.ForeColor = Color.Red;
                        this.LblMensaje.Text = "No hay información de comercios para exportar a excel. !";
                    }
                }
                else
                {
                    this.LblMensaje.ForeColor = Color.Red;
                    this.LblMensaje.Text = _MsgError;
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Se produjo un error al exportar a excel. Motivo: " + ex.Message;
            }
        }

        //Metodo que permite exportar la informacion a excel.
        protected void ExportarDatosExcel(DataTable DtDatosExportar)
        {
            try
            {
                if (DtDatosExportar.Rows.Count > 0)
                {
                    //Aqui se comienza a escribir los datos en el archivo de excel que sera enviado por correo
                    DateTime FechaActual = DateTime.Now;
                    string _MesActual = Convert.ToString(FechaActual.ToString("MMMM"));
                    string _DiaActual = Convert.ToString(FechaActual.ToString("dd"));

                    string strEstado = "";
                    if (this.CmbEstado.SelectedValue.ToString().Trim().Length > 0)
                    {
                        strEstado = this.CmbEstado.SelectedItem.Text.ToString().Trim();
                    }
                    else
                    {
                        strEstado = "TODOS";
                    }

                    //Console.WriteLine("Generando el archivo de excel. \n");
                    string cNombreFileExcel = "RptBancoCuentas_" + _DiaActual + "_" + _MesActual + ".xlsx";

                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
                    string strNombreEncabezadoReporte = "REPORTE DE CALENDARIO TRIBUTARIO - ESTADO [" + strEstado + "]";
                    sheet.Range[2, 2, 2, CantidadCol].Value = strNombreEncabezadoReporte;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Size = 18;
                    sheet.Range[2, 2, 2, CantidadCol].ColumnWidth = 30;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Bold = true;
                    sheet.Range[2, 2, 2, CantidadCol].Interior.Color = Color.LightGray;
                    sheet.Range[2, 2, 2, CantidadCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.LineStyle = XlLineStyle.xlContinuous;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.Weight = XlBorderWeight.xlMedium;

                    for (int ncol = 0; ncol < DtDatosExportar.Columns.Count; ncol++)
                    {
                        //AQUI OBTENEMOS LOS NOMBRES DE LAS COLUMNAS DEL DATATABLE
                        string strNombreColum = DtDatosExportar.Columns[ncol].ColumnName.ToString().Trim().ToUpper();
                        sheet.Range[ContadorRow, ContadorCol].Value = strNombreColum;
                        sheet.Range[ContadorRow, ContadorCol].Font.Bold = true;
                        sheet.Range[ContadorRow, ContadorCol].Font.Size = 12;
                        sheet.Range[ContadorRow, ContadorCol].ColumnWidth = 10;
                        sheet.Range[ContadorRow, ContadorCol].Interior.Color = Color.LightGray;
                        sheet.Range[ContadorRow, ContadorCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        sheet.Range[ContadorRow, ContadorCol].Borders.LineStyle = XlLineStyle.xlDash;
                        sheet.Range[ContadorRow, ContadorCol].Borders.Weight = XlBorderWeight.xlMedium;

                        Row = 4;
                        for (int nrow = 0; nrow < DtDatosExportar.Rows.Count; nrow++)
                        {
                            sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                            sheet.Cells[Row, ContadorCol].Font.Size = 12;
                            sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            //--
                            Row++;
                            Contador++;
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = "BANCO_CUENTAS";
                        book.Worksheets["BANCO_CUENTAS"].UsedRange.Autofit();
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + cNombreFileExcel.ToString().Trim());
                        book.SaveAs(Response.OutputStream, XlFileFormat.xlOpenXMLWorkbook);
                        //book.SaveAs(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        this.LblMensaje.Text = "No hay información para mostrar en el reporte de excel.";
                        this.LblMensaje.ForeColor = Color.Red;
                        //UpdatePanelMsg.Update();
                    }
                }
                else
                {
                    this.LblMensaje.Text = "No hay información para exportar a Excel.";
                    this.LblMensaje.ForeColor = Color.Red;
                    //UpdatePanelMsg.Update();
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Black;
                this.LblMensaje.Text = "";
                //UpdatePanelMsg.Update();
            }
        }

    }
}