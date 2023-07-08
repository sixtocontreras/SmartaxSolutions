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

namespace Smartax.Web.Application.Controles.Reportes.Administrativos
{
    public partial class CtrlRptRetencionIca : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        private static string FormatoMonto = "$#,##0.00;($#,##0.00)";
        private static string FormatoCantidad = "#,##0.00;($#,##0.00)";

        //--DEFINICION DE OBJETOS DE CLASES
        ClaseReportes ObjReporte = new ClaseReportes();
        Estado ObjEstado = new Estado();
        Combox ObjCmb = new Combox();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Reportes de Retencion de Ica";
                //Aplicar los permisos 
                this.AplicarPermisos();
                //Listar Combos
                this.LstTipoReporte();
                this.LstAnio();
                this.LstMes();

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

        protected void LstTipoReporte()
        {
            try
            {

                ObjCmb.MostrarSeleccione = "SI";

                this.CmbTipoReporte.DataSource = ObjCmb.GetTipoRptRetencionIca();
                this.CmbTipoReporte.DataValueField = "idrpt_retencionica";
                this.CmbTipoReporte.DataTextField = "rpt_retencionica";
                this.CmbTipoReporte.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de reporte. Motivo: " + ex.ToString();
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
        protected void LstAnio()
        {
            try
            {
                ObjCmb.MostrarSeleccione = "SI";

                this.CmbAnio.DataSource = ObjCmb.GetAnios();
                this.CmbAnio.DataValueField = "id_anio";
                this.CmbAnio.DataTextField = "numero_anio";
                this.CmbAnio.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años. Motivo: " + ex.ToString();
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
        protected void LstMes()
        {
            try
            {
                ObjCmb.MostrarSeleccione = "SI";

                this.CmbMes.DataSource = ObjCmb.GetMeses();
                this.CmbMes.DataValueField = "id_mes";
                this.CmbMes.DataTextField = "numero_mes";
                this.CmbMes.DataBind();
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


        protected void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjReporte.TipoProceso = Convert.ToInt32(this.CmbTipoReporte.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoReporte.SelectedValue.ToString().Trim() : null);
                ObjReporte.AnioGravable = Convert.ToInt32(this.CmbAnio.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnio.SelectedValue.ToString().Trim() : null);
                if (ObjReporte.TipoProceso == 2)
                {
                    ObjReporte.Mes = this.CmbMes.SelectedItem.ToString().Trim().Length > 0 ? this.CmbMes.SelectedItem.ToString().Trim() : null;

                }
                else
                {
                    ObjReporte.Mes = this.CmbMes.SelectedValue.ToString().Trim().Length > 0 ? this.CmbMes.SelectedValue.ToString().Trim() : null;
                }

                ObjReporte.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                string _MsgError = "";
                dtDatos = ObjReporte.GetRptRetencionIca(ref _MsgError);
                if (_MsgError.ToString().Trim().Length == 0)
                {
                    if (dtDatos != null)
                    {
                        if (dtDatos.Rows.Count > 0)
                        {
                            this.LblMensaje.Text = "";
                            this.ExportarDatosExcel(dtDatos, ObjReporte.TipoProceso);
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
        protected void ExportarDatosExcel(DataTable DtDatosExportar, int tipo_proceso)
        {
            try
            {
                if (DtDatosExportar.Rows.Count > 0)
                {
                    //Aqui se comienza a escribir los datos en el archivo de excel que sera enviado por correo
                    DateTime FechaActual = DateTime.Now;
                    string _MesActual = Convert.ToString(FechaActual.ToString("MMMM"));
                    string _DiaActual = Convert.ToString(FechaActual.ToString("dd"));

                    string cNombreFileExcel = "";
                    string strNombreEncabezadoReporte = "";
                    string infoSheet = "";


                    if (tipo_proceso == 1)
                    {
                        cNombreFileExcel = "RptPagaduria_" + _DiaActual + "_" + _MesActual + ".xlsx";
                        strNombreEncabezadoReporte = "REPORTE PAGADURIA";
                        infoSheet = "REPORTE_PAGADURIA";
                    }
                    else if(tipo_proceso == 2)
                    {
                        cNombreFileExcel = "RptLeasingHabitacional_" + _DiaActual + "_" + _MesActual + ".xlsx";
                        strNombreEncabezadoReporte = "REPORTE LEASING HABITACIONAL";
                        infoSheet = "REPORTE_LEASING_HABITACIONAL";
                    }
                    else if (tipo_proceso == 3)
                    {
                        cNombreFileExcel = "RptLeasingFinanciero_" + _DiaActual + "_" + _MesActual + ".xlsx";
                        strNombreEncabezadoReporte = "REPORTE LEASING FINANCIERO";
                        infoSheet = "REPORTE_LEASING_FINANCIERO";
                    }
                    else if (tipo_proceso == 4)
                    {
                        cNombreFileExcel = "RptTarjetadeCredito_" + _DiaActual + "_" + _MesActual + ".xlsx";
                        strNombreEncabezadoReporte = "REPORTE TARJETA DE CREDITO";
                        infoSheet = "REPORTE_TARJETA_DE_CREDITO";
                    }
                    else if (tipo_proceso == 5)
                    {
                        cNombreFileExcel = "RptInfoContable_" + _DiaActual + "_" + _MesActual + ".xlsx";
                        strNombreEncabezadoReporte = "REPORTE INFORMACION CONTABLE";
                        infoSheet = "REPORTE_INFORMACION_CONTABLE";
                    }

                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
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
                            //AQUI OBTENEMOS CADA UNO DE LOS DATOS DEL DATATABLE
                            //if (ncol == 12)
                            //{
                                //sheet.Cells[Row, ContadorCol].NumberFormat = FormatoCantidad;
                                //sheet.Cells[Row, ContadorCol].Value = Convert.ToDouble(DtDatosExportar.Rows[nrow][ncol].ToString().Trim());
                                //sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                //sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            //}
                            //else
                            //{
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            //}

                            Row++;
                            Contador++;
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = infoSheet;
                        book.Worksheets[infoSheet].UsedRange.Autofit();
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