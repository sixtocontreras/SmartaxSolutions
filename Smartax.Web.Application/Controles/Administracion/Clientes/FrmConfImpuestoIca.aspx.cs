using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Telerik.Web.UI;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Parametros.Tipos;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmConfImpuestoIca : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        FormConfiguracion ObjFormConfig = new FormConfiguracion();
        LiquidarImpuestos ObjLiqImpuesto = new LiquidarImpuestos();
        ConsultaLiqImpuesto ObjConsulta = new ConsultaLiqImpuesto();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();

        private void GetFormConfiguracion()
        {
            try
            {
                ObjFormConfig.TipoConsulta = 1;
                ObjFormConfig.IdFormularioImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                ObjFormConfig.IdEstado = 1;
                ObjFormConfig.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjFormConfig.GetAllFormConfiguracion();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        foreach (DataRow rowItem in dtDatos.Rows)
                        {
                            string _IdFormConfiguracion = rowItem["idformulario_configuracion"].ToString().Trim();
                            int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                            bool _RenglonCalculado = Convert.ToBoolean(rowItem["renglon_calculado"].ToString().Trim()) == false ? true : false;

                            #region AQUI DEFINIMOS EL VALOR MEDIANTE EL SWITCH
                            switch (_NumRenglon)
                            {
                                case 8:
                                    this.LblIdConfImpuesto8.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon8.Enabled = _RenglonCalculado;
                                    break;
                                case 9:
                                    this.LblIdConfImpuesto9.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon9.Enabled = _RenglonCalculado;
                                    break;
                                case 10:
                                    this.LblIdConfImpuesto10.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon10.Enabled = _RenglonCalculado;
                                    break;
                                case 11:
                                    this.LblIdConfImpuesto11.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon11.Enabled = _RenglonCalculado;
                                    break;
                                case 12:
                                    this.LblIdConfImpuesto12.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon12.Enabled = _RenglonCalculado;
                                    break;
                                case 13:
                                    this.LblIdConfImpuesto13.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon13.Enabled = _RenglonCalculado;
                                    break;
                                case 14:
                                    this.LblIdConfImpuesto14.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon14.Enabled = _RenglonCalculado;
                                    break;
                                case 15:
                                    this.LblIdConfImpuesto15.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon15.Enabled = _RenglonCalculado;
                                    break;
                                case 16:
                                    this.LblIdConfImpuesto16.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon16.Enabled = _RenglonCalculado;
                                    break;
                                case 20:
                                    this.LblIdConfImpuesto20.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon20.Enabled = _RenglonCalculado;
                                    break;
                                case 21:
                                    this.LblIdConfImpuesto21.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon21.Enabled = _RenglonCalculado;
                                    break;
                                case 22:
                                    this.LblIdConfImpuesto22.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon22.Enabled = _RenglonCalculado;
                                    break;
                                case 23:
                                    this.LblIdConfImpuesto23.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon23.Enabled = _RenglonCalculado;
                                    break;
                                case 24:
                                    this.LblIdConfImpuesto24.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon24.Enabled = _RenglonCalculado;
                                    break;
                                case 25:
                                    this.LblIdConfImpuesto25.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon25.Enabled = _RenglonCalculado;
                                    break;
                                case 26:
                                    this.LblIdConfImpuesto26.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon26.Enabled = _RenglonCalculado;
                                    break;
                                case 27:
                                    this.LblIdConfImpuesto27.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon27.Enabled = _RenglonCalculado;
                                    break;
                                case 28:
                                    this.LblIdConfImpuesto28.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon28.Enabled = _RenglonCalculado;
                                    break;
                                case 29:
                                    this.LblIdConfImpuesto29.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon29.Enabled = _RenglonCalculado;
                                    break;
                                case 30:
                                    this.LblIdConfImpuesto30.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon30.Enabled = _RenglonCalculado;
                                    break;
                                case 32:
                                    this.LblIdConfImpuesto32.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon32.Enabled = _RenglonCalculado;
                                    break;
                                case 33:
                                    this.LblIdConfImpuesto33.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon33.Enabled = _RenglonCalculado;
                                    break;
                                case 34:
                                    this.LblIdConfImpuesto34.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon34.Enabled = _RenglonCalculado;
                                    break;
                                case 35:
                                    this.LblIdConfImpuesto35.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon35.Enabled = _RenglonCalculado;
                                    break;
                                case 36:
                                    this.LblIdConfImpuesto36.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon36.Enabled = _RenglonCalculado;
                                    break;
                                case 39:
                                    this.LblIdConfImpuesto39.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon39.Enabled = _RenglonCalculado;
                                    break;
                                case 40:
                                    this.LblIdConfImpuesto40.Text = _IdFormConfiguracion;
                                    this.BtnConfRenglon40.Enabled = _RenglonCalculado;
                                    break;
                                default:
                                    break;
                            }
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
                        string _MsgMensaje = "No se encontro información de configuración del formulario... !";
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
                string _MsgError = "Error al listar configuracion del formulario. Motivo: " + ex.ToString();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--AQUI OBTENEMOS LOS PARAMETROS
                this.ViewState["IdClienteImpuesto"] = Request.QueryString["IdClienteImpuesto"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["NombreImpuesto"] = Request.QueryString["NombreImpuesto"].ToString().Trim();
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.GetFormConfiguracion();

            }
        }

        #region EVENTOS CHECKED
        protected void RbExtemporaneidad_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.RbExtemporaneidad.Checked == true)
            {
                this.ViewState["Sancion"] = "1";
                this.RbCorreccion2.Checked = false;
                this.RbInexactitud.Checked = false;
                this.RbOtra.Checked = false;
                this.Validator3.Enabled = false;
                this.TxtSancion.Enabled = false;
                this.TxtSancion.Text = "";
            }
        }

        protected void RbCorreccion2_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.RbCorreccion2.Checked == true)
            {
                this.ViewState["Sancion"] = "2";
                this.RbExtemporaneidad.Checked = false;
                this.RbInexactitud.Checked = false;
                this.RbOtra.Checked = false;
                this.Validator3.Enabled = false;
                this.TxtSancion.Enabled = false;
                this.TxtSancion.Text = "";
            }
        }

        protected void RbInexactitud_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.RbInexactitud.Checked == true)
            {
                this.ViewState["Sancion"] = "3";
                this.RbExtemporaneidad.Checked = false;
                this.RbCorreccion2.Checked = false;
                this.RbOtra.Checked = false;
                this.Validator3.Enabled = false;
                this.TxtSancion.Enabled = false;
                this.TxtSancion.Text = "";
            }
        }

        protected void RbOtra_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.RbOtra.Checked == true)
            {
                this.ViewState["Sancion"] = "4";
                this.RbExtemporaneidad.Checked = false;
                this.RbCorreccion2.Checked = false;
                this.RbInexactitud.Checked = false;
                this.Validator3.Enabled = true;
                this.TxtSancion.Enabled = true;
                this.TxtSancion.Text = "";
                this.TxtSancion.Focus();
            }
        }

        protected void TxtValorRenglon37_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //--AQUI CALCULAMOS EL VALOR DEL RENGLON 38 (TOTAL A PAGAR)
                //double _ValorRenglon35 = Double.Parse(this.LblValorRenglon35.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
                //double _ValorRenglon36 = Double.Parse(this.LblValorRenglon36.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
                //double _ValorRenglon37 = Double.Parse(this.TxtValorRenglon37.Text.ToString().Trim().Replace(".", ","));
                //double _TotalPagar = (_ValorRenglon35 - _ValorRenglon36 + _ValorRenglon37);
                //this.LblValorRenglon38.Text = String.Format(String.Format("{0:$ ###,###,##0}", _TotalPagar));
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
                string _MsgError = "Error al calcular el Total a Pagar. Motivo: " + ex.Message;
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

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {

        }

        #region EVENTOS CLICK DE BOTONES DE CONFIGURAR RENGLON
        protected void BtnConfRenglon8_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "8";
            string _DescripcionRenglon = this.LblDescRenglon8.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto8.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon9_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "9";
            string _DescripcionRenglon = this.LblDescRenglon9.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto9.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon10_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "10";
            string _DescripcionRenglon = this.LblDescRenglon10.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto10.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon11_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "11";
            string _DescripcionRenglon = this.LblDescRenglon11.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto11.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon12_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "12";
            string _DescripcionRenglon = this.LblDescRenglon12.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto12.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon13_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "13";
            string _DescripcionRenglon = this.LblDescRenglon13.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto13.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon14_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "14";
            string _DescripcionRenglon = this.LblDescRenglon14.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto14.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon15_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "15";
            string _DescripcionRenglon = this.LblDescRenglon15.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto15.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon16_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "16";
            string _DescripcionRenglon = this.LblDescRenglon16.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto16.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon20_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "20";
            string _DescripcionRenglon = this.LblDescRenglon20.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto20.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon21_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "21";
            string _DescripcionRenglon = this.LblDescRenglon21.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto21.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon22_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "22";
            string _DescripcionRenglon = this.LblDescRenglon22.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto22.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon23_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "23";
            string _DescripcionRenglon = this.LblDescRenglon23.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto23.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon24_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "24";
            string _DescripcionRenglon = this.LblDescRenglon24.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto24.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon25_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "25";
            string _DescripcionRenglon = this.LblDescRenglon25.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto25.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon26_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "26";
            string _DescripcionRenglon = this.LblDescRenglon26.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto26.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon27_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "27";
            string _DescripcionRenglon = this.LblDescRenglon27.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto27.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon28_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "28";
            string _DescripcionRenglon = this.LblDescRenglon28.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto28.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon29_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "29";
            string _DescripcionRenglon = this.LblDescRenglon29.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto29.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon30_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "30";
            string _DescripcionRenglon = this.LblDescRenglon30.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto30.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon32_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "32";
            string _DescripcionRenglon = this.LblDescRenglon32.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto32.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon33_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "33";
            string _DescripcionRenglon = this.LblDescRenglon33.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto33.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon34_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "34";
            string _DescripcionRenglon = this.LblDescRenglon34.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto34.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon35_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "35";
            string _DescripcionRenglon = this.LblDescRenglon35.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto35.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon36_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "36";
            string _DescripcionRenglon = this.LblDescRenglon36.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto36.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon39_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "39";
            string _DescripcionRenglon = this.LblDescRenglon39.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto39.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }

        protected void BtnConfRenglon40_Click(object sender, EventArgs e)
        {
            #region VISUALIZAR FORMULARIO CONFIGURACION DE IMPUESTO
            //--Mandamos abrir el formulario de registro
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;
            Ventana.Modal = true;

            string _NumeroRenglon = "40";
            string _DescripcionRenglon = this.LblDescRenglon40.Text.ToString().Trim();
            string _IdConfImpuesto = this.LblIdConfImpuesto40.Text.ToString().Trim();
            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddConfigurarRenglon.aspx?IdConfImpuesto=" + _IdConfImpuesto +
                "&NumeroRenglon=" + _NumeroRenglon +
                "&IdFormImpuesto=" + this.ViewState["IdFormImpuesto"].ToString().Trim() +
                "&NombreImpuesto=" + this.ViewState["NombreImpuesto"].ToString().Trim() +
                "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() +
                "&DescripcionRenglon=" + _DescripcionRenglon + "&PathUrl=" + _PathUrl;
            Ventana.ID = "RadWindow12";
            Ventana.VisibleOnPageLoad = true;
            Ventana.Visible = true;
            Ventana.Height = Unit.Pixel(620);
            Ventana.Width = Unit.Pixel(1100);
            Ventana.KeepInScreenBounds = true;
            Ventana.Title = "Configurar No. de Renglon: " + _NumeroRenglon + " del Formulario: " + this.ViewState["NombreImpuesto"].ToString(); ;
            Ventana.VisibleStatusbar = false;
            Ventana.Behaviors = WindowBehaviors.Close;
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;
            #endregion
        }
        #endregion

    }
}