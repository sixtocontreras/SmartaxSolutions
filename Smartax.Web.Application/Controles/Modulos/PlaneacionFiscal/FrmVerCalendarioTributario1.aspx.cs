using System;
using System.Web.UI.WebControls;
using System.Data;
using NativeExcel;
using System.Drawing;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using System.Collections;

namespace Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal
{
    public partial class FrmVerCalendarioTributario1 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        ModulosApp ObjModulo = new ModulosApp();
        DiasFestivo ObjDiaFestivo = new DiasFestivo();
        Utilidades ObjUtils = new Utilidades();
        //private Hashtable Datafechas;

        public DataTable GetCalendarioTributario()
        {
            DataTable DtDatos = new DataTable();
            try
            {
                ObjModulo.TipoConsulta = 2;
                ObjModulo.IdMunicipio = null;
                ObjModulo.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjModulo.Anio = this.ViewState["Anio"].ToString().Trim();
                ObjModulo.Mes = this.ViewState["NumeroMes"].ToString().Trim();
                ObjModulo.IdEstado = 1;
                ObjModulo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--Mostrar vencimiento calendario tributario por municipio
                DtDatos = ObjModulo.GetCalendarioTributario();
                DtDatos.PrimaryKey = new DataColumn[] { DtDatos.Columns["idmun_calendario_trib"] };
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

            return DtDatos;
        }

        public DataTable GetDiasFestivos()
        {
            DataTable DtDatos = new DataTable();
            try
            {
                ObjDiaFestivo.TipoConsulta = 2;
                ObjDiaFestivo.Anio = this.ViewState["Anio"].ToString().Trim();
                ObjDiaFestivo.Mes = this.ViewState["NumeroMes"].ToString().Trim();
                ObjDiaFestivo.IdEstado = 1;
                ObjDiaFestivo.MostrarSeleccione = "NO";
                ObjDiaFestivo.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los dias festivos parametrizados
                DtDatos = ObjDiaFestivo.GetDiasFestivo();
                DtDatos.PrimaryKey = new DataColumn[] { DtDatos.Columns["iddia_festivo"] };
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
                string _MsgError = "Error al listar los dias festivos. Motivo: " + ex.ToString();
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

            return DtDatos;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                this.ViewState["Anio"] = Request.QueryString["Anio"].ToString().Trim();
                this.ViewState["NumeroMes"] = Request.QueryString["NumeroMes"].ToString().Trim();
                this.ViewState["NombreMes"] = Request.QueryString["NombreMes"].ToString().Trim();
                this.ViewState["TipoConsulta"] = Request.QueryString["TipoConsulta"].ToString().Trim();

                this.ViewState["DtDiasFestivo"] = null;
                this.ViewState["DtCalendarioTrib"] = null;
                this.ViewState["DtCalendarioExportar"] = null;

                //--AQUI DEFINIMOS LAS PROPIEDADES DEL CALENDARIO
                Calendar1.Caption = "CALENDARIO TRIBUTARIO";
                Calendar1.FirstDayOfWeek = FirstDayOfWeek.Sunday;
                Calendar1.NextPrevFormat = NextPrevFormat.ShortMonth;
                Calendar1.TitleFormat = TitleFormat.MonthYear;
                Calendar1.ShowGridLines = true;
                Calendar1.DayStyle.HorizontalAlign = HorizontalAlign.Left;
                Calendar1.DayStyle.VerticalAlign = VerticalAlign.Top;
                Calendar1.DayStyle.Height = new Unit(70);
                Calendar1.DayStyle.Width = new Unit(150);
                string _MesSeleccionado = "01/" + this.ViewState["NumeroMes"].ToString().Trim() + "/" + this.ViewState["Anio"].ToString().Trim();
                Calendar1.SelectedDate = Convert.ToDateTime(_MesSeleccionado.ToString().Trim());
                //Calendar1.SelectMonthText = this.ViewState["NombreMes"].ToString().Trim();
                //Calendar1.OtherMonthDayStyle.BackColor = Color.Cornsilk;

                //--AQUI OBTENEMOS DIAS FESTIVOS CONFIGURADOS.
                DataTable DtDatos = new DataTable();
                DtDatos = GetDiasFestivos();
                if (DtDatos != null)
                {
                    if (DtDatos.Rows.Count > 0)
                    {
                        //this.ViewState["DtDatos"] = DtDatos;
                        PintarDiasFestivos(DtDatos);
                    }
                }

                //--AQUI OBTENEMOS CALENDARIO TRIBUTARIO.
                DataTable DtCalendarioTrib = new DataTable();
                DtCalendarioTrib = GetCalendarioTributario();
                if (DtCalendarioTrib != null)
                {
                    if (DtCalendarioTrib.Rows.Count > 0)
                    {
                        this.ViewState["DtCalendarioExportar"] = DtCalendarioTrib;
                        //this.ViewState["DtDatos"] = DtDatos;
                        PintarCalendario(DtCalendarioTrib);
                    }
                }
            }
        }

        private void PintarDiasFestivos(DataTable DtDatos)
        {
            Hashtable dtDatos = new Hashtable();
            Hashtable Datafechas = new Hashtable();
            for (int nrow = 0; nrow < DtDatos.Rows.Count; nrow++)
            {
                DateTime _Fecha = Convert.ToDateTime(DtDatos.Rows[nrow]["fecha_festivo"].ToString().Trim());
                string _Descripcion = DtDatos.Rows[nrow]["descripcion_festivo"].ToString().Trim();

                //--AQUI LLENAMOS EL HASSTABLE
                dtDatos[_Fecha.ToString("dd/MM/yyyy")] = _Descripcion;
                Datafechas = dtDatos;

                Calendar1.SelectedDates.Add(_Fecha);
                Calendar1.SelectedDayStyle.BackColor = Color.Red;
            }

            this.ViewState["DtDiasFestivo"] = Datafechas;
        }

        private void PintarCalendario(DataTable DtDatos)
        {
            Hashtable dtDatosCal = new Hashtable();
            Hashtable DtFechaCalendario = new Hashtable();
            for (int nrow = 0; nrow < DtDatos.Rows.Count; nrow++)
            {
                DateTime _Fecha = Convert.ToDateTime(DtDatos.Rows[nrow]["fecha_limite"].ToString().Trim());
                string _Municipio = DtDatos.Rows[nrow]["nombre_municipio"].ToString().Trim();
                string _ValorDescuento = DtDatos.Rows[nrow]["valor_descuento"].ToString().Trim();

                //--AQUI LLENAMOS EL HASSTABLE
                dtDatosCal[_Fecha.ToString("dd/MM/yyyy")] = _Municipio + " - DESC.: " + _ValorDescuento + "%";
                DtFechaCalendario = dtDatosCal;

                //Calendar1.SelectedDates.Add(_Fecha);
                //Calendar1.SelectedDayStyle.BackColor = Color.Blue;
            }

            this.ViewState["DtCalendarioTrib"] = DtFechaCalendario;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            try
            {
                Hashtable Datafechas = new Hashtable();
                Datafechas = (Hashtable)this.ViewState["DtDiasFestivo"];

                //--AQUI RECORREMOS EL HASHTABLE DE LOS FESTIVOS.
                //ICollection keys = Datafechas.Keys;
                //foreach (Object f in keys)
                //{
                if (Datafechas[e.Day.Date.ToString("dd/MM/yyyy")] != null)
                {
                    //--AQUI AGREGAMOS EL CONTROL LABEL PARA MOSTRAR LA DESCRIPCION DEL DIA FESTIVO
                    Literal lit = new Literal();
                    lit.Text = "<br/>";
                    e.Cell.Controls.Add(lit);

                    Label lbl = new Label();
                    lbl.Text = (string)Datafechas[e.Day.Date.ToString("dd/MM/yyyy")];
                    //lbl.Text = (string)Datafechas[e.Day.Date.ToShortDateString()];
                    lbl.Font.Size = new FontUnit(FontSize.Small);
                    lbl.ForeColor = Color.White;
                    e.Cell.Controls.Add(lbl);
                }
                //}

                //--AQUI RECORREMOS EL HASHTABLE DEL CALENDARIO TRIBUTARIO.
                Hashtable DataCalend = new Hashtable();
                DataCalend = (Hashtable)this.ViewState["DtCalendarioTrib"];

                if (DataCalend[e.Day.Date.ToString("dd/MM/yyyy")] != null)
                {
                    //--AQUI AGREGAMOS EL CONTROL LABEL PARA MOSTRAR LA DESCRIPCION DEL DIA FESTIVO
                    Literal lit = new Literal();
                    lit.Text = "<br/>";
                    e.Cell.Controls.Add(lit);

                    Label lbl = new Label();
                    lbl.Text = (string)DataCalend[e.Day.Date.ToString("dd/MM/yyyy")];
                    //lbl.Text = (string)Datafechas[e.Day.Date.ToShortDateString()];
                    lbl.Font.Size = new FontUnit(FontSize.Small);
                    lbl.ForeColor = Color.Blue;
                    e.Cell.Controls.Add(lbl);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error. Motivo: " + ex.Message);
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

        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtCalendarioExportar"];

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        this.ExportarDatosExcel(dtDatos);
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
                        string _MsgError = "No hay información para exportar a Excel.";
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
                    string _MsgError = "No hay información para exportar a Excel.";
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
                string _MsgError = "Error al exportar información. Motivo: " + ex.Message;
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
                    string cNombreFileExcel = "RptVencimientosCalendario_" + _DiaActual + "_" + _MesActual + ".xlsx";

                    #region IMPRIMIR ENCABEZADO DEL ARCHIVO DE EXCEL
                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
                    string strNombreEncabezadoReporte = "FECHAS DE VENCIMIENTOS CALENDARIO TRIBUTARIO";
                    sheet.Range[2, 2, 2, CantidadCol].Value = strNombreEncabezadoReporte;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Size = 18;
                    sheet.Range[2, 2, 2, CantidadCol].ColumnWidth = 30;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Bold = true;
                    //sheet.Range[2, 2, 2, CantidadCol].Font.Color = Color.White;
                    sheet.Range[2, 2, 2, CantidadCol].Interior.Color = Color.Silver;
                    sheet.Range[2, 2, 2, CantidadCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.LineStyle = XlLineStyle.xlContinuous;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.Weight = XlBorderWeight.xlMedium;
                    #endregion

                    for (int ncol = 0; ncol < DtDatosExportar.Columns.Count; ncol++)
                    {
                        #region IMPRIMIR NOMBRE DE COLUMNAS DEL REPORTE
                        //AQUI OBTENEMOS LOS NOMBRES DE LAS COLUMNAS DEL DATATABLE
                        string strNombreColum = DtDatosExportar.Columns[ncol].ColumnName.ToString().Trim().ToUpper();
                        sheet.Range[ContadorRow, ContadorCol].Value = strNombreColum;
                        sheet.Range[ContadorRow, ContadorCol].Font.Bold = true;
                        sheet.Range[ContadorRow, ContadorCol].Font.Size = 12;
                        sheet.Range[ContadorRow, ContadorCol].ColumnWidth = 10;
                        sheet.Range[ContadorRow, ContadorCol].Interior.Color = Color.Silver;
                        sheet.Range[ContadorRow, ContadorCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        sheet.Range[ContadorRow, ContadorCol].Borders.LineStyle = XlLineStyle.xlDash;
                        sheet.Range[ContadorRow, ContadorCol].Borders.Weight = XlBorderWeight.xlMedium;
                        #endregion

                        Row = 4;
                        for (int nrow = 0; nrow < DtDatosExportar.Rows.Count; nrow++)
                        {
                            #region DETALLE DE LAS TRANSACCIONES EN EL ARCHIVO DE EXCEL
                            if (ncol == 4)
                            {
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim() + "%";
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }
                            else
                            {
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }
                            //sheet.Cells[Row, ContadorCol].Font.Color = Color.White;

                            Row++;
                            Contador++;
                            #endregion
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        #region DESCARGAR EL ARCHIVO DE EXCEL
                        //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = "VENCIMIENTOS_CALENDARIO";
                        book.Worksheets["VENCIMIENTOS_CALENDARIO"].UsedRange.Autofit();
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
                        string _MsgError = "No hay información para mostrar en el reporte de excel.";
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
                    string _MsgError = "No hay información para exportar a Excel.";
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
                string _Excepcion = ex.Message.ToString().Trim().Replace(".", "");
                _log.Error(_Excepcion);
                #endregion
            }
        }

    }
}