using System;
using System.Drawing;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal
{
    public partial class FrmVerCalendarioMeses : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Lista ObjLista = new Lista();
        Utilidades ObjUtils = new Utilidades();

        protected void LstTipoImpuesto()
        {
            try
            {
                ObjFrmImpuesto.TipoConsulta = 2;
                ObjFrmImpuesto.IdEstado = 1;
                ObjFrmImpuesto.MostrarSeleccione = "SI";
                ObjFrmImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjFrmImpuesto.GetFormularioImpuesto();
                this.CmbTipoImpuesto.DataValueField = "idformulario_impuesto";
                this.CmbTipoImpuesto.DataTextField = "descripcion_formulario";
                this.CmbTipoImpuesto.DataBind();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void LstAnioGravable()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                //--LISTAR LOS TIPOS DE OPERACION 1
                this.CmbAnioGravable.DataSource = ObjLista.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "descripcion_anio";
                this.CmbAnioGravable.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años gravables. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.ViewState["TipoConsulta"] = Request.QueryString["TipoConsulta"].ToString().Trim();
                //--
                this.LstTipoImpuesto();
                this.LstAnioGravable();
            }

            //--PINTAR LOS 12 MESES DEL AÑO
            this.GetPintarTabla();
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
        private void GetPintarTabla()
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string[] ArrayMes = FixedData.ArrayMeses;
                if (ArrayMes.Length > 0)
                {
                    tblTablaMeses.Controls.Clear();

                    //Colocar los Encabezados de cada una de las Columnas de la Tabla
                    int rowMeses = 6;
                    int rows = 1;
                    int Columns = ArrayMes.Length;

                    #region PINTAR EL TITULO DE LA TABLA
                    //Aqui creamos los titulos del encabezado de la tabla html
                    for (int Fila1 = 0; Fila1 < rows; Fila1++)
                    {
                        TableRow rowTitulo = new TableRow();
                        tblTablaMeses.Controls.Add(rowTitulo);

                        TableCell CellTitulo = new TableCell();
                        Label LblTitulo = new Label();

                        LblTitulo.Text = "SELECCIONE EL MES DE LA LISTA PARA VER CALENDARIO";
                        CellTitulo.Controls.Add(LblTitulo);

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

                    TableRow rowNew = new TableRow();
                    tblTablaMeses.Controls.Add(rowNew);
                    int _ContadorFila = 0;
                    int _ContadorMes = 1;
                    foreach (string nMes in ArrayMes)
                    {
                        if (_ContadorFila == rowMeses)
                        {
                            _ContadorFila = 0;
                            rowNew = new TableRow();
                            tblTablaMeses.Controls.Add(rowNew);
                        }

                        TableCell cellNew = new TableCell();
                        string[] _NombreMes = nMes.ToString().Trim().Split('|');
                        LinkButton LnkButton = new LinkButton();
                        LnkButton.Text = _NombreMes[0].ToString().Trim();
                        string _IdButton = _ContadorMes + "|" + nMes.ToString().Trim();
                        LnkButton.ID = _IdButton;
                        LnkButton.ToolTip = LnkButton.Text.ToString().Trim();

                        //--AQUI PINTAMOS DE AMARILLO EL MES ACTUAL.
                        string _MesActual = DateTime.Now.ToString("MMMM").ToUpper();
                        if (_MesActual.Equals(LnkButton.Text.ToString().Trim()))
                        {
                            LnkButton.ForeColor = Color.Blue;
                        }
                        else
                        {
                            LnkButton.ForeColor = Color.Black;
                        }

                        LnkButton.Command += new CommandEventHandler(link_Click);
                        cellNew.Controls.Add(LnkButton);

                        cellNew.BorderStyle = BorderStyle.Inset;
                        cellNew.Height = 150;
                        cellNew.Font.Bold = true;
                        cellNew.Font.Size = 15;
                        cellNew.BackColor = Color.LightGray;

                        cellNew.HorizontalAlign = HorizontalAlign.Center;
                        cellNew.BorderWidth = Unit.Pixel(1);

                        rowNew.Controls.Add(cellNew);
                        _ContadorFila++;
                        _ContadorMes++;
                    }

                    //this.LblMensaje.ForeColor = Color.Black;
                    //this.LblMensaje.Text = "";
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
                #endregion
            }
        }

        void link_Click(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton LnkButton = sender as LinkButton;
                string[] ArrayIds = LnkButton.ID.ToString().Trim().Split('|');
                string _Mes = ArrayIds[0].ToString().Trim();
                string _NumeroMes = ArrayIds[0].ToString().Trim();
                string _NombreMes = ArrayIds[1].ToString().Trim();
                string _PosicionDiaSemana = ArrayIds[2].ToString().Trim();
                string _IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "";
                string _FormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim() : "";
                string _Anio = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                //string _Anio = "2022";

                if (_Mes.Length <= 1)
                {
                    _NumeroMes = "0" + _Mes;
                }

                #region MOSTRAR EL FORM DE CALENDARIO TRIBUTARIO
                //--VALIDAR EL FORM Y EL AÑO GRAVABLE
                if(this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length>0 ||
                    this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0)
                {
                    #region MOSTRAR FORM DE CALENDARIO TRIBUTARIO
                    //--HABILITAR OBJETO DE POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    //Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerCalendarioTributario1.aspx?Anio=" + _Anio + "&NumeroMes=" + _NumeroMes + "&NombreMes=" + _NombreMes + "&TipoConsulta=" + this.ViewState["TipoConsulta"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerCalendarioTributario.aspx?Anio=" + _Anio + "&NumeroMes=" + _NumeroMes + "&NombreMes=" + _NombreMes + "&PosicionDiaSemana=" + _PosicionDiaSemana + "&IdFormImpuesto=" + _IdFormImpuesto + "&FormImpuesto=" + _FormImpuesto;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(620);
                    Ventana.Width = Unit.Pixel(1050);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Calendario: Año: " + _Anio + " Mes: " + _NombreMes;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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
                    string _MsgMensaje = "Señor usuario, debe seleccionar el tipo de impuesto y el año gravable de la lista !";
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
                    _log.Error(_MsgMensaje);
                    #endregion
                }
                #endregion
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los días del mes. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            #region MOSTRAR EL FORM DE CALENDARIO TRIBUTARIO
            //--HABILITAR OBJETO DE POPUP
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;

            string _TipoImpuesto = this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim();
            string _AnioGravable = this.CmbAnioGravable.SelectedItem.Text.ToString().Trim();

            RadWindow Ventana = new RadWindow();
            Ventana.Modal = true;
            Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerFechaLimitePago.aspx?IdImpuesto=" + this.CmbTipoImpuesto.SelectedValue.ToString().Trim() + "&AnioGravable=" + this.CmbAnioGravable.SelectedValue.ToString().Trim();
            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(500);
            Ventana.Width = Unit.Pixel(900);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = _TipoImpuesto + " Año Gravable: " + _AnioGravable;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }
    }
}