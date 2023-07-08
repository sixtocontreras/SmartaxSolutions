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

namespace Smartax.Web.Application.Controles.Reportes.Seguridad
{
    public partial class CtrlRptLogsAuditoria : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        private static string FormatoMonto = "#,##0.00;(#,##0.00)";
        private static string FormatoCantidad = "#,##0.00;($#,##0.00)";

        //--DEFINICION DE OBJETOS DE CLASES
        ClaseReportes ObjReporte = new ClaseReportes();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        TipoEvento objTipoEvento = new TipoEvento();
        Estado ObjEstado = new Estado();

        protected void LstTipoEvento()
        {
            try
            {
                objTipoEvento.TipoConsulta = 2;
                objTipoEvento.IdEstado = 1;
                objTipoEvento.MostrarSeleccione = "SI";
                objTipoEvento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbEvento.DataSource = objTipoEvento.GetTipoEvento();
                this.CmbEvento.DataValueField = "idtipo_evento";
                this.CmbEvento.DataTextField = "tipo_evento";
                this.CmbEvento.DataBind();
                LblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                LblMensaje.Text = "Error al Listar los tipos de eventos. Motivo: " + ex.Message.ToString().Trim();
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
                this.Page.Title = "Reporte Logs de Auditoria";
                this.LstTipoEvento();

                DateTime dFechaActual = DateTime.Now;
                this.DtFechaInicial.MaxDate = DateTime.Now;
                this.DtFechaInicial.SelectedDate = dFechaActual;
                this.DtFechaFinal.SelectedDate = dFechaActual;
            }
        }

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjAuditoria.TipoConsulta = 2;
                ObjAuditoria.IdTipoEvento = this.CmbEvento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEvento.SelectedValue.ToString().Trim() : null;
                ObjAuditoria.ModuloApp = null;
                DateTime dtFechaInicial = Convert.ToDateTime(this.DtFechaInicial.SelectedDate);
                DateTime dtFechaFinal = Convert.ToDateTime(this.DtFechaFinal.SelectedDate);
                ObjAuditoria.FechaInicial = Convert.ToString(dtFechaInicial.ToString("yyyy-MM-dd")) + " 00:00:00";
                ObjAuditoria.FechaFinal = Convert.ToString(dtFechaFinal.ToString("yyyy-MM-dd")) + " 23:59:59";
                ObjAuditoria.IdUsuario = null;  //--Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdEmpresa = null;  //--this.CmbEmpresa.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbEmpresa.SelectedValue.ToString().Trim()) : Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjAuditoria.GetLogsAuditoria();
                //--
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
                        this.LblMensaje.Text = "No hay registros para exportar a excel. !";
                    }
                }
                else
                {
                    this.LblMensaje.ForeColor = Color.Red;
                    this.LblMensaje.Text = "No hay información para exportar a excel. !";
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
                    //Console.WriteLine("Generando el archivo de excel. \n");
                    string cNombreFileExcel = "RptLogsAuditoria_" + _DiaActual + "_" + _MesActual + ".xlsx";

                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
                    string strNombreEncabezadoReporte = "REPORTE DE LOGS DE AUDITORIA DEL SISTEMA";
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

                            Row++;
                            Contador++;
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = "LOGS_AUDITORIA";
                        book.Worksheets["LOGS_AUDITORIA"].UsedRange.Autofit();
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