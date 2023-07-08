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

namespace Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal
{
    public partial class FrmVerCalendarioTributario2 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        ModulosApp ObjModulo = new ModulosApp();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjModulo.TipoConsulta = Int32.Parse(this.ViewState["TipoConsulta"].ToString().Trim());
                ObjModulo.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjModulo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar vencimiento calendario tributario por municipio
                ObjetoDataTable = ObjModulo.GetCalendarioTributario();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["formulario, codigo_dane"] };
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
                //this.AplicarPermisos();
                this.ViewState["TipoConsulta"] = Request.QueryString["TipoConsulta"].ToString().Trim();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
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
                this.RadGrid1.DataMember = "DtCalendarioTributario";
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
                if (e.CommandName == "BtnDescCalendario")
                {
                    DataTable dtDatos = new DataTable();
                    dtDatos = this.FuenteDatos.Tables[0];

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
                    string strNombreEncabezadoReporte = "FECHAS VENCIMIENTOS CALENDARIOS TRIBUTARIOS";
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
                            if (ncol == 5)
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
    }
}