using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Telerik.Web.UI;
using Smartax.Web.Application.Clases.Seguridad;
using System.Text;

namespace Smartax.Web.Application
{
    public partial class FrmValidarToken : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        TokenSecurity ObjToken = new TokenSecurity();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.GenerarToken();
                this.TxtNumeroToken.Text = "";
                this.TxtNumeroToken.Focus();
            }
        }

        protected void GenerarToken()
        {
            try
            {
                //Aqui Generamos el No. del Token para enviar al Usuario.
                ObjToken.IdToken = null;
                ObjToken.NumeroToken = ObjUtils.GetTokenRandom(100000, 999999);
                ObjToken.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjToken.FechaUso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ObjToken.EstadoToken = "DP";
                ObjToken.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjToken.TipoProceso = 1;

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjToken.GetGenerarToken(ref _IdRegistro, ref _MsgError))
                {
                    this.ViewState["IdToken"] = _IdRegistro;

                    //--1. SMS, 2. EMAIL
                    if (this.Session["IdMedioEnvio"].Equals("1"))
                    {
                        #region ENVIO DEL TOKEN POR SMS
                        this.TxtNumeroToken.Enabled = true;
                        this.BtnValidarToken.Enabled = true;

                        this.LblLeyenda.Text = "Señor usuario tenga en cuenta que tiene 2 minutos para poder ingresar al sistema con el No. del Token enviado al Celular configurado en el sistema. Si usted no alcanza ingresar el No. del Token enviado debera generar uno nuevo dando Click en el Link Generar nuevo Token el cual contara con el mismo tiempo antes mencionado para su Uso. Cualquier duda contacte a soporte técnico.";
                        //this.LblDescripcion.Text = "Señor usuario le fue enviado un mensaje de TEXTO con el No. de Token para que pueda ingresar al sistema.";
                        this.LblTipoEnvio.Text = "Medio de envio Celular";

                        string _Telefono = this.Session["TelefonoUser"].ToString().Trim();
                        this.LblMedioEnviado.Text = _Telefono.ToString().Trim().Substring(0, _Telefono.ToString().Trim().Length - 3) + "***";

                        //--AQUI ENVIAMOS EL SMS AL USUARIO
                        TokenSecurity objToken = new TokenSecurity();
                        objToken.NumeroTelefono = _Telefono;
                        objToken.MensajeSms = "PUNTO DE PAGO No. DE TOKEN: " + ObjToken.NumeroToken;
                        _MsgError = "";
                        if (objToken.GetEnvioTokenSms(ref _MsgError))
                        {
                            #region MOSTRAR MENSAJE AL USUARIO
                            //Mostrar el mensaje del error del sistema.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
                            string _Mensaje = "Señor usuario, le fue enviado un SMS con el No. de Token para ingresar al sistema.";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
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
                        else
                        {
                            #region ENVIO DEL TOKEN POR CORREO
                            this.LblTipoEnvio.Text = "Email:";
                            string[] _ArrayEmail = this.Session["EmailUsuario"].ToString().Trim().Split('@');
                            string _Email = _ArrayEmail[0].ToString().Trim().Substring(0, _ArrayEmail[0].ToString().Trim().Length - 3) + "***";
                            this.LblMedioEnviado.Text = _Email + "@" + _ArrayEmail[1].ToString().Trim();

                            //Aqui hacemos el envio del Email al usuario con el No. de Token asignado.
                            //Usuario registrado o editado de forma exitosa.
                            ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                            ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                            ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                            ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                            ObjCorreo.StrEmailPara = this.Session["EmailUsuario"].ToString().Trim();
                            ObjCorreo.StrAsunto = "REF.: No. TOKEN";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + "No. de Token: " + ObjToken.NumeroToken + ".</h4>");

                            ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                            if (!ObjCorreo.SendEmailUser(ref _MsgError))
                            {
                                #region MOSTRAR MENSAJE AL USUARIO
                                this.TxtNumeroToken.Enabled = false;
                                this.BtnValidarToken.Enabled = false;
                                //Mostrar el mensaje del error del sistema.
                                this.RadWindowManager1.ReloadOnShow = true;
                                this.RadWindowManager1.DestroyOnClose = true;
                                this.RadWindowManager1.Windows.Clear();
                                this.RadWindowManager1.Enabled = true;
                                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                this.RadWindowManager1.Visible = true;

                                RadWindow Ventana = new RadWindow();
                                Ventana.Modal = true;
                                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError + " Contacte a soporte técnico.";
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
                            #endregion
                            //--
                            #region MOSTRAR ERROR DEL SISTEMA
                            ////--MOSTRAR FORM DE POPUP
                            //this.RadWindowManager1.ReloadOnShow = true;
                            //this.RadWindowManager1.DestroyOnClose = true;
                            //this.RadWindowManager1.Windows.Clear();
                            //this.RadWindowManager1.Enabled = true;
                            //this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            //this.RadWindowManager1.Visible = true;

                            //RadWindow Ventana = new RadWindow();
                            //Ventana.Modal = true;
                            //string[] _Mensaje = _MsgError.ToString().Trim().Split('|');
                            //string _MsgMensaje = _Mensaje[0].ToString().Trim() + " - " + _Mensaje[1].ToString().Trim();
                            //Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                            //Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            //Ventana.VisibleOnPageLoad = true;
                            //Ventana.Visible = true;
                            //Ventana.Height = Unit.Pixel(300);
                            //Ventana.Width = Unit.Pixel(600);
                            //Ventana.KeepInScreenBounds = true;
                            //Ventana.Title = "Mensaje del Sistema";
                            //Ventana.VisibleStatusbar = false;
                            //Ventana.Behaviors = WindowBehaviors.Close;
                            //this.RadWindowManager1.Windows.Add(Ventana);
                            //this.RadWindowManager1 = null;
                            //Ventana = null;
                            #endregion
                        }

                        #endregion
                    }
                    else if (this.Session["IdMedioEnvio"].Equals("2"))
                    {
                        #region ENVIO DEL TOKEN POR CORREO
                        this.TxtNumeroToken.Enabled = true;
                        this.BtnValidarToken.Enabled = true;
                        this.LblLeyenda.Text = "Señor usuario tenga en cuenta que tiene 2 minutos para poder ingresar al sistema con el No. del Token enviado al Email configurado en el sistema. Si usted no alcanza ingresar el No. del Token enviado debera generar uno nuevo dando Click en el Link Generar nuevo Token el cual contara con el mismo tiempo antes mencionado para su Uso. Cualquier duda contacte a soporte técnico.";
                        //this.LblDescripcion.Text = "Señor usuario le fue enviado un Email con el No. de Token para que pueda ingresar al sistema.";
                        this.LblTipoEnvio.Text = "Medio de envio Email";

                        string[] _ArrayEmail = this.Session["EmailUsuario"].ToString().Trim().Split('@');
                        string _Email = _ArrayEmail[0].ToString().Trim().Substring(0, _ArrayEmail[0].ToString().Trim().Length - 3) + "***";
                        this.LblMedioEnviado.Text = _Email + "@" + _ArrayEmail[1].ToString().Trim();

                        //--AQUI PASAMOS LOS VALORES DE VARIABLES
                        ObjCorreo.StrServerCorreo = this.Session["ServerCorreo"].ToString().Trim();
                        ObjCorreo.PuertoCorreo = Int32.Parse(this.Session["PuertoCorreo"].ToString().Trim());
                        ObjCorreo.StrEmailDe = this.Session["EmailSoporte"].ToString().Trim();
                        ObjCorreo.StrPasswordDe = this.Session["PasswordEmail"].ToString().Trim();
                        ObjCorreo.StrEmailPara = this.Session["EmailUsuario"].ToString().Trim();
                        ObjCorreo.StrAsunto = "REF.: TOKEN DE AUTENTICACIÓN";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + "No. de Token: " + ObjToken.NumeroToken + ".</h4>");

                        ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                        if (!ObjCorreo.SendEmailUser(ref _MsgError))
                        {
                            #region MOSTRAR MENSAJE AL USUARIO
                            this.TxtNumeroToken.Enabled = false;
                            this.BtnValidarToken.Enabled = false;
                            //Mostrar el mensaje del error del sistema.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError + " Contacte a soporte técnico.";
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
                        this.TxtNumeroToken.Text = "";
                        this.TxtNumeroToken.Focus();
                        #endregion
                    }
                    else if (this.Session["IdMedioEnvio"].Equals("2.1"))
                    {
                        #region ENVIO DEL TOKEN POR CORREO
                        ////--
                        //this.TxtNumeroToken.Enabled = true;
                        //this.BtnValidarToken.Enabled = true;
                        //this.LblLeyenda.Text = "Señor usuario tenga en cuenta que tiene 2 minutos para poder ingresar al sistema con el No. del Token enviado al Email configurado en el sistema. Si usted no alcanza ingresar el No. del Token enviado debera generar uno nuevo dando Click en el Link Generar nuevo Token el cual contara con el mismo tiempo antes mencionado para su Uso. Cualquier duda contacte a soporte técnico.";
                        ////this.LblDescripcion.Text = "Señor usuario le fue enviado un Email con el No. de Token para que pueda ingresar al sistema.";
                        //this.LblTipoEnvio.Text = "Medio de envio Email";
                        //string[] _ArrayEmail = this.Session["EmailUsuario"].ToString().Trim().Split('@');
                        //string _Email = _ArrayEmail[0].ToString().Trim().Substring(0, _ArrayEmail[0].ToString().Trim().Length - 3) + "***";
                        //this.LblMedioEnviado.Text = _Email + "@" + _ArrayEmail[1].ToString().Trim();

                        //TrxPuntoPago objTrxApi = new TrxPuntoPago();
                        //objTrxApi.EmailUsuario = this.Session["EmailUsuario"].ToString().Trim();
                        //objTrxApi.AsuntoEmail = "REF.: No. TOKEN";
                        ////--
                        //string nHora = DateTime.Now.ToString("HH");
                        //string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        //StringBuilder strDetalleEmail = new StringBuilder();
                        //strDetalleEmail.Append("<h4>" + "No. de Token: " + ObjToken.NumeroToken + ".</h4>");
                        //objTrxApi.DetalleMensajeEmail = strDetalleEmail.ToString().Trim();

                        //_MsgError = "";
                        //if (objTrxApi.GetEnvioEmail(ref _MsgError))
                        //{
                        //    #region MOSTRAR MENASJE DE USUARIO
                        //    //this.UpdatePanel4.Update();
                        //    //Mostramos el mensaje porque se produjo un error con la Trx.
                        //    this.RadWindowManager1.ReloadOnShow = true;
                        //    this.RadWindowManager1.DestroyOnClose = true;
                        //    this.RadWindowManager1.Windows.Clear();
                        //    this.RadWindowManager1.Enabled = true;
                        //    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        //    this.RadWindowManager1.Visible = true;

                        //    RadWindow Ventana = new RadWindow();
                        //    Ventana.Modal = true;
                        //    string[] _MsgMensaje = _MsgError.ToString().Trim().Split('|');
                        //    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje[0].ToString().Trim() + " - " + _MsgMensaje[1].ToString().Trim();
                        //    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        //    Ventana.VisibleOnPageLoad = true;
                        //    Ventana.Visible = true;
                        //    Ventana.Height = Unit.Pixel(300);
                        //    Ventana.Width = Unit.Pixel(600);
                        //    Ventana.KeepInScreenBounds = true;
                        //    Ventana.Title = "Mensaje del Sistema";
                        //    Ventana.VisibleStatusbar = false;
                        //    Ventana.Behaviors = WindowBehaviors.Close;
                        //    this.RadWindowManager1.Windows.Add(Ventana);
                        //    this.RadWindowManager1 = null;
                        //    Ventana = null;
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region MOSTRAR MENASJE DE USUARIO
                        //    //this.UpdatePanel4.Update();
                        //    //Mostramos el mensaje porque se produjo un error con la Trx.
                        //    this.RadWindowManager1.ReloadOnShow = true;
                        //    this.RadWindowManager1.DestroyOnClose = true;
                        //    this.RadWindowManager1.Windows.Clear();
                        //    this.RadWindowManager1.Enabled = true;
                        //    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        //    this.RadWindowManager1.Visible = true;

                        //    RadWindow Ventana = new RadWindow();
                        //    Ventana.Modal = true;
                        //    string[] _MsgMensaje = _MsgError.ToString().Trim().Split('|');
                        //    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje[0].ToString().Trim() + " - " + _MsgMensaje[1].ToString().Trim();
                        //    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        //    Ventana.VisibleOnPageLoad = true;
                        //    Ventana.Visible = true;
                        //    Ventana.Height = Unit.Pixel(300);
                        //    Ventana.Width = Unit.Pixel(600);
                        //    Ventana.KeepInScreenBounds = true;
                        //    Ventana.Title = "Mensaje del Sistema";
                        //    Ventana.VisibleStatusbar = false;
                        //    Ventana.Behaviors = WindowBehaviors.Close;
                        //    this.RadWindowManager1.Windows.Add(Ventana);
                        //    this.RadWindowManager1 = null;
                        //    Ventana = null;
                        //    #endregion
                        //}
                        #endregion
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE ERROR
                    //Mostrar el mensaje del error del sistema.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError + " Contacte a soporte técnico.";
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
                _log.Error("Error: " + ex.StackTrace);
            }
        }

        protected void LnkGenerarToken_Click(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                this.GenerarToken();
                this.TxtNumeroToken.Focus();
            }
            catch (Exception ex)
            {
                _log.Error("Error al generar el Token. Motivo: " + ex.Message);
            }
        }

        protected void BtnValidarToken_Click(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                //Aqui vamos hacer uso del No. del Token para ingresar al sistema.
                ObjToken.IdToken = this.ViewState["IdToken"].ToString().Trim();
                ObjToken.NumeroToken = this.TxtNumeroToken.Text.ToString().Trim();
                ObjToken.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjToken.FechaUso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ObjToken.EstadoToken = "DP";
                ObjToken.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjToken.TipoProceso = 2;
                string _MsgError = "";

                for (int i = 0; i < FixedData.META_CHARS.Length; i++)
                {
                    if (ObjToken.NumeroToken.Contains(FixedData.META_CHARS[i].ToString().Trim()))
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        //Mostrar el mensaje del error del sistema.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;

                        _MsgError = "Alerta !!! Se detectaron datos no permitidos. !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError + ".";
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
                        return;
                    }
                }

                int _IdRegistro = 0;
                _MsgError = "";
                if (ObjToken.GetGenerarToken(ref _IdRegistro, ref _MsgError))
                {
                    this.Session["Login"] = 3;
                    //Response.Redirect("/");
                    Response.Redirect("/Index.aspx");
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    this.Session["Login"] = 1;
                    this.Session["SerialPV"] = "";
                    this.TxtNumeroToken.Text = "";
                    this.TxtNumeroToken.Focus();
                    //Mostrar el mensaje del error del sistema.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
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
                //Mostrar el mensaje del error del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error al realizar el uso del Token. Motivo: " + ex.StackTrace;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError + ". Contacte a soporte técnico.";
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