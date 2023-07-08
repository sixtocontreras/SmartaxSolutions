using System;
using System.Web.UI.WebControls;
using System.Data;
using NativeExcel;
using System.Drawing;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using System.Collections;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Parametros.Tipos;

namespace Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal
{
    public partial class FrmVerCalendarioTributario : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static int[] ArrayMes = null;

        ModulosApp ObjModulo = new ModulosApp();
        DiasFestivo ObjDiaFestivo = new DiasFestivo();
        Utilidades ObjUtils = new Utilidades();
        //private Hashtable Datafechas;

        public DataTable GetCalendarioTributario()
        {
            DataTable DtDatos = new DataTable();
            try
            {
                ObjModulo.TipoConsulta = 3;
                ObjModulo.IdMunicipio = null;
                ObjModulo.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjModulo.Anio = this.ViewState["Anio"].ToString().Trim();
                ObjModulo.Mes = this.ViewState["NumeroMes"].ToString().Trim();
                ObjModulo.IdFormImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim().Length > 0 ? this.ViewState["IdFormImpuesto"].ToString().Trim() : null;
                ObjModulo.IdEstado = 1;
                ObjModulo.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--Mostrar vencimiento calendario tributario por municipio
                DtDatos = ObjModulo.GetCalendarioTributario();
                DtDatos.PrimaryKey = new DataColumn[] { DtDatos.Columns["idmun_calendario_trib"] };
                this.ViewState["DtCalendarioTrib"] = DtDatos;
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
                this.ViewState["DtDiasFestivo"] = DtDatos;
                //DtDatos.PrimaryKey = new DataColumn[] { DtDatos.Columns["iddia_festivo"] };
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
                this.ViewState["PosicionDiaSemana"] = Request.QueryString["PosicionDiaSemana"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["FormImpuesto"] = Request.QueryString["FormImpuesto"].ToString().Trim();

                this.ViewState["DtDiasFestivo"] = null;
                this.ViewState["DtCalendarioTrib"] = null;
                this.ViewState["DtCalendarioExportar"] = null;
                //--AQUI OBTENEMOS LOS DATOS DEL CALENDARIO TRIBUTARIO
                this.GetCalendarioTributario();

                //--VALIDAR SI EL AÑO ES BISIESTO
                int _Anio = Int32.Parse(this.ViewState["Anio"].ToString().Trim());
                if (_Anio % 4 == 0 && (_Anio % 100 != 0 || _Anio % 400 == 0))
                {
                    this.ViewState["DiaBisiesto"] = "S";
                }
                else
                {
                    this.ViewState["DiaBisiesto"] = "N";
                }

                //--AQUI LLAMAMOS LOS METODOS DE LOS DATATABLES
                this.GetDiasFestivos();

                #region OBTENER EL ARRAY DE DIAS DEL MES SELECCIONADO
                //--AQUI VALIDAMOS EL MES AL CUAL SE VA MOSTRAR EL CALENDARIO
                if (this.ViewState["NumeroMes"].ToString().Trim().Equals("01"))
                {
                    this.GetPintarTabla(FixedData.ArrayEnero);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("02"))
                {
                    //Aqui calculamos si el año es bisiesto para obtener los dias del año.
                    //int _Anio = Int32.Parse(DateTime.Now.ToString("yyyy"));
                    if (_Anio % 4 == 0 && (_Anio % 100 != 0 || _Anio % 400 == 0))
                    {
                        this.GetPintarTabla(FixedData.ArrayFebreroBisiesto);
                    }
                    else
                    {
                        this.GetPintarTabla(FixedData.ArrayFebrero);
                    }
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("03"))
                {
                    this.GetPintarTabla(FixedData.ArrayMarzo);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("04"))
                {
                    this.GetPintarTabla(FixedData.ArrayAbril);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("05"))
                {
                    this.GetPintarTabla(FixedData.ArrayMayo);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("06"))
                {
                    this.GetPintarTabla(FixedData.ArrayJunio);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("07"))
                {
                    this.GetPintarTabla(FixedData.ArrayJulio);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("08"))
                {
                    this.GetPintarTabla(FixedData.ArrayAgosto);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("09"))
                {
                    this.GetPintarTabla(FixedData.ArraySeptiembre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("10"))
                {
                    this.GetPintarTabla(FixedData.ArrayOctubre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("11"))
                {
                    this.GetPintarTabla(FixedData.ArrayNoviembre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("12"))
                {
                    this.GetPintarTabla(FixedData.ArrayDiciembre);
                }
                #endregion
            }
            else
            {
                #region OBTENER EL ARRAY DE DIAS DEL MES SELECCIONADO
                //--AQUI VALIDAMOS EL MES AL CUAL SE VA MOSTRAR EL CALENDARIO
                if (this.ViewState["NumeroMes"].ToString().Trim().Equals("01"))
                {
                    this.GetPintarTabla(FixedData.ArrayEnero);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("02"))
                {
                    //Aqui calculamos si el año es bisiesto para obtener los dias del año.
                    int _Anio = Int32.Parse(this.ViewState["Anio"].ToString().Trim());
                    if (_Anio % 4 == 0 && (_Anio % 100 != 0 || _Anio % 400 == 0))
                    {
                        this.GetPintarTabla(FixedData.ArrayFebreroBisiesto);
                    }
                    else
                    {
                        this.GetPintarTabla(FixedData.ArrayFebrero);
                    }
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("03"))
                {
                    this.GetPintarTabla(FixedData.ArrayMarzo);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("04"))
                {
                    this.GetPintarTabla(FixedData.ArrayAbril);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("05"))
                {
                    this.GetPintarTabla(FixedData.ArrayMayo);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("06"))
                {
                    this.GetPintarTabla(FixedData.ArrayJunio);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("07"))
                {
                    this.GetPintarTabla(FixedData.ArrayJulio);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("08"))
                {
                    this.GetPintarTabla(FixedData.ArrayAgosto);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("09"))
                {
                    this.GetPintarTabla(FixedData.ArraySeptiembre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("10"))
                {
                    this.GetPintarTabla(FixedData.ArrayOctubre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("11"))
                {
                    this.GetPintarTabla(FixedData.ArrayNoviembre);
                }
                else if (this.ViewState["NumeroMes"].ToString().Trim().Equals("12"))
                {
                    this.GetPintarTabla(FixedData.ArrayDiciembre);
                }
                #endregion
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

        //Aqui mostramos los datos en la Lista del tablero de control
        private void GetPintarTabla(int[] ArrayDias)
        {
            try
            {
                ////Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                //this.RadWindowManager1.Enabled = false;
                //this.RadWindowManager1.EnableAjaxSkinRendering = false;
                //this.RadWindowManager1.Visible = false;

                //string[] ArrayMes = FixedData.ArrayMeses;
                if (ArrayDias.Length > 0)
                {
                    tblTablaCalendario.Controls.Clear();

                    //Colocar los Encabezados de cada una de las Columnas de la Tabla
                    int rowMeses = 7;
                    int rows = 1;
                    int Columns = FixedData.ArrayDiasSemana.Length;

                    #region PINTAR TITULO DEL CALENDARIO
                    //Aqui creamos los titulos del encabezado de la tabla html
                    for (int Fila1 = 0; Fila1 < rows; Fila1++)
                    {
                        TableRow rowTitulo = new TableRow();
                        tblTablaCalendario.Controls.Add(rowTitulo);

                        TableCell CellTitulo = new TableCell();
                        Label LblTitulo = new Label();
                        //--VALIDAR SI EL IMPUESTO FUE SELECCIONADO
                        if (this.ViewState["IdFormImpuesto"].ToString().Trim().Length > 0)
                        {
                            if (Int32.Parse(this.ViewState["IdFormImpuesto"].ToString().Trim()) == 1)
                            {
                                LblTitulo.Text = "DÍAS CALENDARIO TRIBUTARIO MES DE " + this.ViewState["NombreMes"].ToString().Trim() + " DEL " + (Int32.Parse(this.ViewState["Anio"].ToString().Trim()) + 1) + " ( " + this.ViewState["FormImpuesto"].ToString().Trim() + " )";
                                CellTitulo.Controls.Add(LblTitulo);
                            }
                            else
                            {
                                LblTitulo.Text = "DÍAS CALENDARIO TRIBUTARIO MES DE " + this.ViewState["NombreMes"].ToString().Trim() + " DEL " + this.ViewState["Anio"].ToString().Trim() + " ( " + this.ViewState["FormImpuesto"].ToString().Trim() + " )";
                                CellTitulo.Controls.Add(LblTitulo);
                            }
                        }
                        else
                        {
                            LblTitulo.Text = "DÍAS CALENDARIO TRIBUTARIO MES DE " + this.ViewState["NombreMes"].ToString().Trim() + " DEL " + this.ViewState["Anio"].ToString().Trim();
                            CellTitulo.Controls.Add(LblTitulo);
                        }

                        CellTitulo.ColumnSpan = 7;  // Columns;
                        CellTitulo.Font.Bold = true;
                        CellTitulo.Font.Size = 12;
                        CellTitulo.BackColor = Color.White;
                        CellTitulo.ForeColor = Color.Black;
                        CellTitulo.HorizontalAlign = HorizontalAlign.Center;
                        CellTitulo.BorderWidth = Unit.Pixel(1);

                        rowTitulo.Controls.Add(CellTitulo);
                    }
                    #endregion

                    #region PINTAR TITULO DEL CALENDARIO
                    TableRow rowDias = new TableRow();
                    tblTablaCalendario.Controls.Add(rowDias);
                    //Aqui creamos los titulos del encabezado de la tabla html
                    foreach (string _DiaMes in FixedData.ArrayDiasSemana)
                    {
                        TableCell CellDias = new TableCell();
                        Label LblTitulo = new Label();

                        LblTitulo.Text = _DiaMes.ToString().Trim();
                        CellDias.Controls.Add(LblTitulo);

                        //CellDias.ColumnSpan = 7;  // Columns;
                        CellDias.Font.Bold = true;
                        CellDias.Font.Size = 12;
                        CellDias.BackColor = Color.White;
                        CellDias.ForeColor = Color.Black;
                        CellDias.HorizontalAlign = HorizontalAlign.Center;
                        CellDias.BorderWidth = Unit.Pixel(1);
                        rowDias.Controls.Add(CellDias);
                    }
                    #endregion

                    #region PINTAR CADA UNO DE LOS DÍAS DEL MES
                    TableRow rowNew = new TableRow();
                    tblTablaCalendario.Controls.Add(rowNew);
                    int _ContadorFilaDiaMes = 0;
                    int _ContadorFila = 0;
                    int _ContadorMes = 1;

                    #region AQUI VALIDAMOS EL DIA DE INICIO DE LA SEMANA PARA PINTAR EN EL CALENDARIO
                    //--AQUI VALIDAMOS SI EL IMPUESTO ES EL ICA
                    int _AnioSleccionado = 0;
                    if (Int32.Parse(this.ViewState["IdFormImpuesto"].ToString().Trim()) == 1)
                    {
                        _AnioSleccionado = Int32.Parse(this.ViewState["Anio"].ToString().Trim()) + 1;
                    }
                    else
                    {
                        _AnioSleccionado = Int32.Parse(this.ViewState["Anio"].ToString().Trim());
                    }
                    int _MesSleccionado = Int32.Parse(this.ViewState["NumeroMes"].ToString().Trim());
                    int _DiasSemana = Int32.Parse(this.ViewState["PosicionDiaSemana"].ToString().Trim());
                    int _PosicionDiaSemana = 0;
                    int _DiasSemMover = (_AnioSleccionado - FixedData._AnioBase) + 1;
                    //--AQUI VALIDAMOS EL AÑO DEL CALENDARIO
                    if (_AnioSleccionado > FixedData._AnioBase)
                    {
                        #region AÑO MAYOR AL AÑO BASE (2019)
                        if (this.ViewState["DiaBisiesto"].ToString().Trim().Equals("N"))
                        {
                            #region CALENDARIO AÑO NO BISIESTO
                            if (_DiasSemana >= 7)
                            {
                                _PosicionDiaSemana = 2;
                            }
                            else
                            {
                                _DiasSemMover = (_AnioSleccionado - FixedData._AnioBase);
                                if ((_DiasSemana + _DiasSemMover) >= 8)
                                {
                                    _PosicionDiaSemana = _DiasSemMover;
                                }
                                else
                                {
                                    _PosicionDiaSemana = (_DiasSemMover + _DiasSemana) + 1;
                                    if (_PosicionDiaSemana >= 8)
                                    {
                                        _PosicionDiaSemana = 1;
                                    }
                                    //_PosicionDiaSemana = _DiasSemana + _DiasSemMover;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region CALENDARIO AÑO BISIESTO
                            if (_DiasSemana >= 7)
                            {
                                _PosicionDiaSemana = 1;
                            }
                            else
                            {
                                if (_MesSleccionado > 2)
                                {
                                    if (_DiasSemana >= 6)
                                    {
                                        _PosicionDiaSemana = 1;
                                    }
                                    else
                                    {
                                        _PosicionDiaSemana = _DiasSemana + 2;
                                    }
                                    //_PosicionDiaSemana = (_DiasSemana + _DiasSemMover) - 2;
                                }
                                else
                                {
                                    _PosicionDiaSemana = (_DiasSemana + _DiasSemMover) - 1;
                                }
                            }
                            #endregion                            
                        }
                        #endregion
                    }
                    else
                    {
                        _PosicionDiaSemana = _DiasSemana - 1;
                    }
                    #endregion

                    #region AQUI PINTAMOS LOS DATOS EN BLANCO EN EL CALENDARIO
                    TableCell cellNew = null;
                    if (_PosicionDiaSemana < 7)
                    {
                        for (int i = 1; i <= _PosicionDiaSemana; i++)
                        {
                            cellNew = new TableCell();
                            Label LblText = new Label();
                            LblText.Text = "";

                            cellNew.BorderStyle = BorderStyle.Inset;
                            cellNew.Height = 70;
                            cellNew.Width = 75;
                            cellNew.Font.Bold = true;
                            cellNew.Font.Size = 15;
                            cellNew.BackColor = Color.LightGray;
                            cellNew.HorizontalAlign = HorizontalAlign.Center;
                            cellNew.BorderWidth = Unit.Pixel(1);

                            cellNew.Controls.Add(LblText);
                            rowNew.Controls.Add(cellNew);
                            _ContadorFila++;
                        }
                    }
                    #endregion

                    foreach (int _DiaMes in ArrayDias)
                    {
                        #region AQUI RECORREMOS TODOS LOS DIAS DEL MES
                        if (_ContadorFila == rowMeses)
                        {
                            _ContadorFila = 0;
                            rowNew = new TableRow();
                            tblTablaCalendario.Controls.Add(rowNew);
                        }

                        string _DescripcionText = "";
                        string _NumeroDia = _DiaMes.ToString().Trim();
                        if (_NumeroDia.Length <= 1)
                        {
                            _NumeroDia = "0" + _NumeroDia;
                        }

                        cellNew = new TableCell();
                        LinkButton LnkButton = new LinkButton();
                        LnkButton.Text = _DiaMes.ToString().Trim();
                        string _IdButton = _DiaMes.ToString().Trim() + "|" + this.ViewState["NombreMes"].ToString().Trim();
                        LnkButton.ID = _IdButton;
                        LnkButton.ForeColor = Color.Black;

                        #region AQUI PINTAMOS LOS DIAS FESTIVOS DEL AÑO
                        DataTable dtDiasFestido = new DataTable();
                        dtDiasFestido = (DataTable)this.ViewState["DtDiasFestivo"];
                        DataRow[] dataRowsFest = dtDiasFestido.Select("dia_festivo = '" + _NumeroDia.ToString().Trim() + "' AND mes_festivo = '" + this.ViewState["NumeroMes"].ToString().Trim() + "'");
                        Label LblText = new Label();
                        int _ContadorFestivos = 0;
                        if (dataRowsFest.Length == 1)
                        {
                            string _DiaFestivo = dataRowsFest[0]["descripcion_festivo"].ToString().Trim();
                            //if (_DescripcionText.ToString().Trim().Length > 0)
                            //{
                            //    _DescripcionText = _DescripcionText + "\r" + _DiaFestivo;
                            //}
                            _ContadorFestivos++;
                            LnkButton.ToolTip = _DiaFestivo;
                            LblText.ForeColor = Color.Red;
                            LnkButton.ForeColor = Color.Red;
                            cellNew.Font.Bold = true;
                        }
                        #endregion

                        #region AQUI PINTAMOS LOS DIAS DEL CALENDARIO TRIBUTARIO
                        DataTable dtDiasCalendario = new DataTable();
                        dtDiasCalendario = (DataTable)this.ViewState["DtCalendarioTrib"];
                        DataRow[] dataRowsCal = dtDiasCalendario.Select("dia_limite = '" + _NumeroDia.ToString().Trim() + "'");
                        //DataRow[] dataRowsCal = dtDiasFestido.Select("dia_limite = '" + _NumeroDia.ToString().Trim() + "' AND mes_festivo = '" + this.ViewState["NumeroMes"].ToString().Trim() + "'");
                        //Label LblText = new Label();

                        if (dataRowsCal.Length >= 1)
                        {
                            string _Vencido = dataRowsCal[0]["vencido"].ToString().Trim();
                            string _ValorDescuento = dataRowsCal[0]["valor_descuento"].ToString().Trim();
                            //--
                            if (_Vencido.Equals("S"))
                            {
                                this.BtnVencidos.Visible = true;
                                LnkButton.ToolTip = "Fecha Vencida, Descuento por pronto pago ( " + _ValorDescuento + " )";
                                cellNew.BackColor = Color.OrangeRed;
                            }
                            else if (_Vencido.Equals("P"))
                            {
                                this.BtnVencimientosHoy.Visible = true;
                                LnkButton.ToolTip = "Vence el día de hoy, Descuento por pronto pago ( " + _ValorDescuento + " )";
                                cellNew.BackColor = Color.Yellow;
                            }
                            else if (_Vencido.Equals("N"))
                            {
                                this.BtnPorVencer.Visible = true;
                                LnkButton.ToolTip = "Proxima fecha de Vencimiento, Descuento por pronto pago ( " + _ValorDescuento + " )";
                                cellNew.BackColor = Color.SlateBlue;
                            }

                            cellNew.Font.Bold = true;
                        }
                        else
                        {
                            if (_ContadorFestivos <= 0)
                            {
                                LnkButton.ToolTip = _DiaMes.ToString().Trim() + " - " + this.ViewState["NombreMes"].ToString().Trim();
                            }

                            cellNew.Font.Size = 15;
                            cellNew.BackColor = Color.LightGray;
                        }
                        #endregion

                        cellNew.Controls.Add(LnkButton);

                        //LblText.Text = _DescripcionText.ToString().Trim();
                        //cellNew.Controls.Add(LblText);

                        #region AQUI LE COLOCAMOS FORMATO Y STYLO DE LA CELDA DE LA TABLA
                        //--AQUI PINTAMOS DE AMARILLO EL MES ACTUAL.
                        //string _MesActual = DateTime.Now.ToString("MMMM").ToUpper();
                        //if (_MesActual.Equals(_NombreMes))
                        //{
                        //    LnkButton.ForeColor = Color.Blue;
                        //}
                        //else
                        //{
                        //    LnkButton.ForeColor = Color.Black;
                        //}

                        cellNew.BorderStyle = BorderStyle.Inset;
                        cellNew.Height = 65;
                        cellNew.Width = 70;
                        cellNew.Font.Size = 25;
                        //cellNew.Font.Bold = true;
                        //cellNew.BackColor = Color.LightGray;

                        cellNew.HorizontalAlign = HorizontalAlign.Center;
                        cellNew.BorderWidth = Unit.Pixel(1);
                        #endregion

                        rowNew.Controls.Add(cellNew);
                        _ContadorFila++;
                        _ContadorMes++;
                        _ContadorFilaDiaMes++;
                        #endregion
                    }
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
                string _MsgError = "Error al mostrar la lista de meses del año. Motivo: " + ex.Message;
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

        protected void BtnVencidos_Click(object sender, EventArgs e)
        {
            #region MOSTRAR EL FORM DE CALENDARIO TRIBUTARIO
            //--HABILITAR OBJETO DE POPUP
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;

            //--AQUI FILTRAMOS EL DATATABLE
            DataTable dtDatos = new DataTable();
            DataTable DtFacturasPagar = new DataTable();
            DtFacturasPagar = (DataTable)this.ViewState["DtCalendarioTrib"];
            dtDatos = DtFacturasPagar.Clone();

            DataRow[] dataRows = DtFacturasPagar.Select("vencido = 'S'");
            foreach (DataRow rowItem in dataRows)
            {
                //--Copiar los datos de los registros
                dtDatos.ImportRow(rowItem);
            }
            this.Session["DtCalendarioTrib"] = dtDatos;
            string _Tipo = "1";

            RadWindow Ventana = new RadWindow();
            Ventana.Modal = true;
            Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerDetalleCalendarioMunicipio.aspx?AnioGravable=" + this.ViewState["Anio"].ToString().Trim() + "&Mes=" + this.ViewState["NumeroMes"].ToString().Trim() + "&Tipo=" + _Tipo;
            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(500);
            Ventana.Width = Unit.Pixel(1000);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Municipios con Fechas Vencidas. Mes de " + this.ViewState["NombreMes"].ToString().Trim();
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnVencimientosHoy_Click(object sender, EventArgs e)
        {
            #region MOSTRAR EL FORM DE CALENDARIO TRIBUTARIO
            //--HABILITAR OBJETO DE POPUP
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;

            //--AQUI FILTRAMOS EL DATATABLE
            DataTable dtDatos = new DataTable();
            DataTable DtFacturasPagar = new DataTable();
            DtFacturasPagar = (DataTable)this.ViewState["DtCalendarioTrib"];
            dtDatos = DtFacturasPagar.Clone();

            DataRow[] dataRows = DtFacturasPagar.Select("vencido = 'P'");
            foreach (DataRow rowItem in dataRows)
            {
                //--Copiar los datos de los registros
                dtDatos.ImportRow(rowItem);
            }
            this.Session["DtCalendarioTrib"] = dtDatos;
            string _Tipo = "2";

            RadWindow Ventana = new RadWindow();
            Ventana.Modal = true;
            Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerDetalleCalendarioMunicipio.aspx?AnioGravable=" + this.ViewState["Anio"].ToString().Trim() + "&Mes=" + this.ViewState["NumeroMes"].ToString().Trim() + "&Tipo=" + _Tipo;
            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(500);
            Ventana.Width = Unit.Pixel(800);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Municipios con Vencimientos día actual. Mes de " + this.ViewState["NombreMes"].ToString().Trim();
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnPorVencer_Click(object sender, EventArgs e)
        {
            #region MOSTRAR EL FORM DE CALENDARIO TRIBUTARIO
            //--HABILITAR OBJETO DE POPUP
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;

            //--AQUI FILTRAMOS EL DATATABLE
            DataTable dtDatos = new DataTable();
            DataTable DtFacturasPagar = new DataTable();
            DtFacturasPagar = (DataTable)this.ViewState["DtCalendarioTrib"];
            dtDatos = DtFacturasPagar.Clone();

            DataRow[] dataRows = DtFacturasPagar.Select("vencido = 'N'");
            foreach (DataRow rowItem in dataRows)
            {
                //--Copiar los datos de los registros
                dtDatos.ImportRow(rowItem);
            }
            this.Session["DtCalendarioTrib"] = dtDatos;
            string _Tipo = "3";

            RadWindow Ventana = new RadWindow();
            Ventana.Modal = true;
            Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerDetalleCalendarioMunicipio.aspx?AnioGravable=" + this.ViewState["Anio"].ToString().Trim() + "&Mes=" + this.ViewState["NumeroMes"].ToString().Trim() + "&Tipo=" + _Tipo;
            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(500);
            Ventana.Width = Unit.Pixel(800);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Municipios con Fechas por Vencer. Mes de " + this.ViewState["NombreMes"].ToString().Trim();
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtCalendarioTrib"];

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
                            //if (ncol == 5)
                            //{
                            //    sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim() + "%";
                            //    sheet.Cells[Row, ContadorCol].Font.Size = 12;
                            //    sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            //}
                            //else
                            //{
                            sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                            sheet.Cells[Row, ContadorCol].Font.Size = 12;
                            sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            //}
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