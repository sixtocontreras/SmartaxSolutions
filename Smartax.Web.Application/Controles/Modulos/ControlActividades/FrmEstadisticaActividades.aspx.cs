using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Parametros.Tipos;

namespace Smartax.Web.Application.Controles.Modulos.ControlActividades
{
    public partial class FrmEstadisticaActividades : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        protected RadHtmlChart[] Chart;

        ControlActividadesAnalista ObjActividad = new ControlActividadesAnalista();
        TipoCtrlActividad ObjTipoCtrActividad = new TipoCtrlActividad();
        Usuario ObjUser = new Usuario();
        Lista objLista = new Lista();
        Utilidades ObjUtils = new Utilidades();

        private DataTable GetDatos()
        {
            DataTable dtDatos = new DataTable();
            try
            {
                ObjActividad.TipoConsulta = Int32.Parse(this.RbtTipoConsulta.SelectedValue.ToString().Trim());
                ObjActividad.IdTipoCtrlActividad = this.CmbTipoCtrlActividad.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoCtrlActividad.SelectedValue.ToString().Trim() : null;
                ObjActividad.IdFormularioImpuesto = null;
                ObjActividad.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : null;
                int _IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjActividad.IdUsuario = this.CmbUsuarios.SelectedValue.ToString().Trim().Length > 0 ? this.CmbUsuarios.SelectedValue.ToString().Trim() : null;
                ObjActividad.IdEstado = 1;
                ObjActividad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                dtDatos = ObjActividad.GetEstadisticaActividades();
                //--
                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        //--DATOS
                        this.ViewState["DtEstadisticaAct"] = dtDatos;
                    }
                    else
                    {
                        //--DATOS
                        this.ViewState["DtEstadisticaAct"] = dtDatos;
                    }
                }
                else
                {
                    //--DATOS
                    this.ViewState["DtEstadisticaAct"] = dtDatos;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                dtDatos = null;
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error al obtener datos de la grafica. Motivo: " + ex.ToString();
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

            return dtDatos;
        }

        public static double round(double input)
        {
            double _Result = 0;
            double rem = input % 1000;
            //return rem >= 5 ? (num - rem + 10) : (num - rem);
            if (rem >= 500)
            {
                _Result = (double)(1000 * Math.Ceiling(input / 1000));
            }
            else
            {
                _Result = (double)(1000 * Math.Round(input / 1000));
            }

            return _Result;
        }

        protected void LstUsuarios()
        {
            try
            {
                ObjUser.TipoConsulta = 2;
                ObjUser.IdUsuario = null;
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdEstado = null;
                ObjUser.MostrarSeleccione = "SI";
                ObjUser.IdRol = Convert.ToInt32(this.Session["IdRol"].ToString().Trim());
                ObjUser.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjUser.GetUsuarios();
                //--AÑO GRAVABLE
                this.CmbUsuarios.DataSource = dtDatos;
                this.CmbUsuarios.DataValueField = "id_usuario";
                this.CmbUsuarios.DataTextField = "nombre_usuario";
                this.CmbUsuarios.DataBind();
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
                string _MsgMensaje = "Señor usuario. Error al listar los usuarios analistas. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void LstAnioGravable()
        {
            try
            {
                objLista.MostrarSeleccione = "SI";

                DataTable dtDatos = new DataTable();
                dtDatos = objLista.GetAnios();
                //--AÑO GRAVABLE
                this.CmbAnioGravable.DataSource = dtDatos;
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
                string _MsgMensaje = "Señor usuario. Error al listar los año gravable. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void LstTipoCtrlActividad()
        {
            try
            {
                ObjTipoCtrActividad.TipoConsulta = 4;
                ObjTipoCtrActividad.ManejaEstadistica = null;
                ObjTipoCtrActividad.IdEstado = 1;
                ObjTipoCtrActividad.MostrarSeleccione = "SI";
                ObjTipoCtrActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoCtrlActividad.DataSource = ObjTipoCtrActividad.GetTipoActividades();
                this.CmbTipoCtrlActividad.DataValueField = "idtipo_ctrl_actividades";
                this.CmbTipoCtrlActividad.DataTextField = "tipo_ctrl_actividad";
                this.CmbTipoCtrlActividad.DataBind();
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
                string _MsgMensaje = "Señor usuario. Error al listar los tipos de actividades. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--DATOS
                this.ViewState["DtEstadisticaAct"] = null;
                //--
                this.LstAnioGravable();
                this.LstUsuarios();
                this.LstTipoCtrlActividad();
                //--
                int _IdEncontrado = 0;
                string _IdRol = this.Session["IdRol"].ToString().Trim();
                string[] _IdsArray = FixedData.IdRolCtrlActividades.ToString().Trim().Split(',');
                //--
                for (int i = 0; i < _IdsArray.Length; i++)
                {
                    if (_IdsArray[i].ToString().Trim() == _IdRol)
                    {
                        //--HABILITAMOS AMBAS OPCIONES
                        this.RbtTipoConsulta.Enabled = false;
                        this.CmbUsuarios.Enabled = false;
                        this.CmbUsuarios.SelectedValue = this.Session["IdUsuario"].ToString().Trim();
                        _IdEncontrado = 1;
                        break;
                    }
                    else
                    {
                        _IdEncontrado = 2;
                    }
                }

                #region AQUI OBTENEMOS LOS TIPOS DE CONTROLES DE ACTIVIDADES
                ObjTipoCtrActividad.TipoConsulta = 4;
                ObjTipoCtrActividad.ManejaEstadistica = "S";
                ObjTipoCtrActividad.IdEstado = 1;
                ObjTipoCtrActividad.MostrarSeleccione = "NO";
                ObjTipoCtrActividad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtTipoCtrActividad = new DataTable();
                dtTipoCtrActividad = ObjTipoCtrActividad.GetTipoActividades();
                int _ContadorAct = 0;
                foreach (DataRow rowItem in dtTipoCtrActividad.Rows)
                {
                    int _IdTipoCtrlActividades = Int32.Parse(rowItem["idtipo_ctrl_actividades"].ToString().Trim());

                    //--OBTENEMOS LOS DATOS DE LA ACTIVIDADES DE ANALISTAS
                    DataTable dtDatos = new DataTable();
                    DataTable dtActividades = new DataTable();
                    dtDatos = this.GetDatos();
                    dtActividades = dtDatos.Clone();
                    //--
                    DataRow[] dataRows = dtDatos.Select("idtipo_ctrl_actividades = " + _IdTipoCtrlActividades);
                    foreach (DataRow rowItem2 in dataRows)
                    {
                        //Copiar los datos de las factura del usuario logeado.
                        dtActividades.ImportRow(rowItem2);
                    }

                    if (dtActividades.Rows.Count > 0)
                    {
                        switch (_ContadorAct)
                        {
                            case 0:
                                //--
                                this.RadHtmlChart1.DataSource = null;
                                this.RadHtmlChart1 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            case 1:
                                //--
                                this.RadHtmlChart2.DataSource = null;
                                this.RadHtmlChart2 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            case 2:
                                //--
                                this.RadHtmlChart3.DataSource = null;
                                this.RadHtmlChart3 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            case 3:
                                //--
                                this.RadHtmlChart4.DataSource = null;
                                this.RadHtmlChart4 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            case 4:
                                //--
                                this.RadHtmlChart5.DataSource = null;
                                this.RadHtmlChart5 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            case 5:
                                //--
                                this.RadHtmlChart6.DataSource = null;
                                this.RadHtmlChart6 = this.MostrarGrafica(dtActividades, _ContadorAct, "");
                                break;
                            default:
                                break;
                        }
                    }
                    _ContadorAct++;
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

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtEstadisticaAct"];
                //--
                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.RadGrid1.DataSource = dtDatos;
                        this.RadGrid1.DataMember = "DtEstadisticaAct";
                    }
                    else
                    {
                        this.RadGrid1.DataSource = dtDatos;
                        this.RadGrid1.DataMember = "DtEstadisticaAct";
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
                string _MsgError = "Error con el evento NeedDataSource del tipo de moneda. Motivo: " + ex.ToString();
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

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                this.RadGrid1.Rebind();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del tipo de moneda. Motivo: " + ex.ToString();
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

        private RadHtmlChart MostrarGrafica(DataTable dtDatos, int i, string cNombreKpi)
        {
            RadHtmlChart ChartData = new RadHtmlChart();
            try
            {
                Chart = new RadHtmlChart[]
                                    {
                                        this.RadHtmlChart1, this.RadHtmlChart2, this.RadHtmlChart3,
                                        this.RadHtmlChart4, this.RadHtmlChart5, this.RadHtmlChart6
                                    };

                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (dtDatos.Rows.Count > 0)
                {
                    Chart[i].Visible = true;
                    Chart[i].DataSource = null;
                    Chart[i].DataSource = dtDatos;
                    Chart[i].DataBind();
                    Chart[i].Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Top;
                    ColumnSeries series = null;
                    CategorySeriesItem item1 = null;

                    ArrayList ArrayNombres = new ArrayList { };
                    string aux;
                    double val;
                    int _ContadorColor = 0;
                    Chart[i].PlotArea.Series.Clear();
                    for (int nrow = 0; nrow < dtDatos.Rows.Count; nrow++)
                    {
                        series = new ColumnSeries();
                        //LineSeries series = new LineSeries();
                        for (int col = 0; col < dtDatos.Columns.Count; col++)
                        {
                            item1 = new CategorySeriesItem();
                            //--AQUI SOLO OBTENEMOS LOS CALUMNAS 7, 9, 7
                            if (col == 7)
                            {
                                aux = dtDatos.Rows[nrow][col].ToString().Trim();
                                string _CodigoActividad = dtDatos.Rows[nrow]["codigo_actividad"].ToString().Trim();
                                string _NombreFila = dtDatos.Rows[nrow]["descripcion_actividad"].ToString().Trim();

                                bool contains = ArrayNombres.Contains(_NombreFila);
                                if (contains == false)
                                {
                                    ArrayNombres.Add(_NombreFila.ToString().Trim());
                                }

                                if (aux == "")
                                {
                                    item1.Y = null;
                                }
                                else
                                {
                                    val = Double.Parse(aux);
                                    item1.Y = (decimal)val;
                                }

                                //series.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                //series.MarkersAppearance.Size = 3;
                                //series.MarkersAppearance.BorderWidth = 2;
                                //series.TooltipsAppearance.ClientTemplate = "Sector: #=series.name# <br/> Valor: #=value# <br/> Fecha: #=category#";

                                //Aqui enviamos los datos para armar las Columnas
                                item1.BackgroundColor = getLineColor(_ContadorColor);
                                series.Name = _NombreFila;
                                series.TooltipsAppearance.ClientTemplate = "Tipo: #=series.name# <br/> Código: " + _CodigoActividad + " <br/> Valor: #=value#";
                                series.Stacked = false;
                                series.Gap = 1.5;
                                series.Spacing = 0.4;
                                series.SeriesItems.Add(item1);
                                _ContadorColor++;
                            }
                        }

                        ////--
                        series.LabelsAppearance.Visible = true;
                        series.TooltipsAppearance.Color = Color.White;
                        Chart[i].PlotArea.Series.Add(series);
                    }

                    //AQUI SE ARMA LA SECUENCIA DE FECHAS EN LA PARTE INFERIOR DE LA GRAFICA
                    //Chart[i].PlotArea.XAxis.Items.Clear();
                    //for (int c = 0; c < ArrayNombres.Count; c++)
                    //{
                    //    string strNombre = ArrayNombres[c].ToString().Trim();
                    //    Chart[i].PlotArea.XAxis.Items.Add(strNombre.ToString().Trim());
                    //}

                    //AQUI SE DETERMINA EL TIEMPO ENTRE UN DIA DE OTRO PARA QUE NO LOS MUESTRE TAN PEGADOS EN EL AXIS
                    //Chart[i].PlotArea.XAxis.LabelsAppearance.Step = 30;
                    //Chart[i].PlotArea.XAxis.LabelsAppearance.RotationAngle = -70;

                    //AQUI SE AGREGAN LOS NOMBRES DE LA PARTE INFERIOR Y DEL LADO IZQUIERDO DE LA GRAFICA
                    Chart[i].PlotArea.YAxis.TitleAppearance.Text = "Valores";
                    //Chart[i].PlotArea.XAxis.TitleAppearance.Text = "Tiempo, Dias";
                    //Chart[i].PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //Chart[i].PlotArea.XAxis.TitleAppearance.RotationAngle = 0;

                    ChartData = Chart[i];
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    Chart[i].PlotArea.Series.Clear();
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor usuario, no se encontro información para mostrar en la grafica !";
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
                string _MsgError = "Error al mostrar la Grafica. Motivo: " + ex.Message;
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

            return ChartData;
        }

        #region DEFINICION DE FUNCIONES PARA EL COLOR DE LA LINEAS DE LA GRAFICA
        protected virtual Color getLineColor(int nContadorRow)
        {
            try
            {
                //para almacenar el ultimo caraceter del sector
                return lineColors[nContadorRow];
            }
            catch (Exception EX)
            {
                return Color.Black;
            }
        }

        protected virtual Color getCircleColor(int nContadorRow)
        {
            try
            {
                //para almacenar el ultimo caraceter del sector
                return circleColors[nContadorRow];
            }
            catch (Exception EX)
            {
                return Color.Black;
            }
        }


        protected Color[] lineColors = {
                  Color.Coral,
                  Color.Red,
                  Color.Blue,
                  Color.Orange,

                  Color.Yellow,
                  Color.DarkViolet,
                  Color.DarkRed,

                  Color.GreenYellow,
                  //Color.LightSteelBlue,
                  Color.DarkMagenta,
                  Color.OliveDrab,                  
                  
                  //Color.LightGoldenrodYellow,                                    
                  Color.Coral,
                  Color.DarkBlue,
                  Color.IndianRed,

                  Color.DarkGreen,
                  Color.DarkMagenta,
                  Color.DarkOrange,

                  Color.DarkKhaki,
                  Color.DarkOliveGreen,
                  Color.DarkOrchid,

                  Color.DarkSalmon,
                  Color.DarkSeaGreen,
                  Color.DarkSlateBlue,
                  };


        protected Color[] circleColors = {
                  Color.Blue,
                  Color.Orange,
                  Color.Red,

                  Color.Yellow,
                  Color.DarkViolet,
                  Color.DarkRed,

                  Color.GreenYellow,
                  //Color.LightSteelBlue,
                  Color.DarkMagenta,
                  Color.OliveDrab,                  
                  
                  //Color.LightGoldenrodYellow,                                    
                  Color.Coral,
                  Color.DarkBlue,
                  Color.IndianRed,

                  Color.DarkGreen,
                  Color.DarkMagenta,
                  Color.DarkOrange,

                  Color.DarkKhaki,
                  Color.DarkOliveGreen,
                  Color.DarkOrchid,

                  Color.DarkSalmon,
                  Color.DarkSeaGreen,
                  Color.DarkSlateBlue,
                  };
        #endregion

        protected void RbtTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.RbtTipoConsulta.SelectedValue.ToString().Trim().Equals("3"))
                {
                    this.CmbUsuarios.Enabled = true;
                    this.Validador1.Enabled = true;
                    this.CmbUsuarios.SelectedValue = "";
                }
                else
                {
                    this.CmbUsuarios.Enabled = false;
                    this.Validador1.Enabled = false;
                    this.CmbUsuarios.SelectedValue = "";
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
                string _MsgError = "Error con la selección del tipo de consulta. Motivo: " + ex.ToString();
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

        protected void BtnConsultar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                //--
                DataTable dtDatos = new DataTable();
                dtDatos = this.GetDatos();
                this.UpdatePanel1.Update();
                this.RadHtmlChart1.DataSource = null;
                this.RadHtmlChart1 = this.MostrarGrafica(dtDatos, 0, "");
                this.RadGrid1.Rebind();
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
                string _MsgError = "Error al realizar la consulta. Motivo: " + ex.ToString();
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
    }
}