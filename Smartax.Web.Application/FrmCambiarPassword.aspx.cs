using System;
using System.Web.UI.WebControls;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Text;
using System.Text.RegularExpressions;

namespace Smartax.Web.Application
{
    public partial class FrmCambiarPassword : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Usuario ObjUser = new Usuario();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.LblNombreUsuario.Text = this.Session["NombreCompletoUsuario"].ToString().Trim();
                this.TxtNuevoPass.Text = "";
                this.TxtNuevoPass.Focus();
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

        protected void BtnCambiarPass_Click(object sender, EventArgs e)
        {
            try
            {
                #region DEFINICION DE VALORES A PASAR AL OBJETO CLASE
                ObjUser.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.NombreUsuario = this.Session["NombreCompletoUsuario"].ToString().Trim();
                ObjUser.LoginUsuario = this.Session["LoginUsuario"].ToString().Trim();
                ObjUser.EmailUsuario = this.Session["EmailUsuario"].ToString().Trim();
                ObjUser.PasswordUsuario = this.TxtNuevoPass.Text.ToString().Trim();
                ObjUser.CambiarClave = "N";
                ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                ObjUser.IdEstado = 1;
                ObjUser.IdUsuarioUp = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.TipoProceso = 3;
                #endregion

                if (ObjUser.PasswordUsuario.ToString().Trim().Length >= 8 && ObjUser.PasswordUsuario.ToString().Trim().Length <= FixedData.LongitudClaveUsuario)
                {
                    if (ContrasenaSegura(ObjUser.PasswordUsuario))
                    {
                        int _IdRegistro = 0;
                        string _Mensaje = "";
                        if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _Mensaje))
                        {
                            #region ENVIO DE CORREO AL USUARIO
                            ObjCorreo.StrServerCorreo = this.Session["ServerCorreo"].ToString().Trim();
                            ObjCorreo.PuertoCorreo = Int32.Parse(this.Session["PuertoCorreo"].ToString().Trim());
                            ObjCorreo.StrEmailDe = this.Session["EmailSoporte"].ToString().Trim();
                            ObjCorreo.StrPasswordDe = this.Session["PasswordEmail"].ToString().Trim();
                            ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();
                            ObjCorreo.StrAsunto = "REF.: CAMBIO DE CLAVE USUARIO";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + " Señor Usuario, para informarle que usted acaba de realizar el reset de su clave para el Ingreso al sistema [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                            "<h4>CAMBIO DE CLAVE AL USUARIO</h2>" +
                                            "Nombre: " + ObjUser.NombreUsuario.ToString().Trim() + "<br/>" +
                                            "Usuario: " + ObjUser.LoginUsuario.ToString().Trim() + "<br/>" +
                                            "Password: " + ObjUser.PasswordUsuario + "<br/>" +
                                            "<br/><br/>" +
                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                            ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                            string _MsgErrorEmail = "";
                            if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                            {
                                _Mensaje = _Mensaje + " Pero " + _MsgErrorEmail.ToString().Trim();
                            }
                            #endregion

                            //Cambio de clave realizado de forma exitosa.
                            Session.Contents.RemoveAll();
                            Session.Clear();
                            Session.Abandon();
                            ViewState.Clear();
                            Response.Redirect("/Default.aspx");
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
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
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
                        string _MsgMensaje = "Señor usuario, la clave debe ser alfanumerica debe contener minimo una letra, numeros y caracteres especiales !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
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
                    string _MsgMensaje = "Señor usuario, la clave debe tener una longitud minima de 8 y maximo " + FixedData.LongitudClaveUsuario + " caracteres !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(230);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar el cambio de clave del usuario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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

        public Boolean ContrasenaSegura(String _PasswordUser)
        {
            //letras de la A a la Z, mayusculas y minusculas
            Regex letras = new Regex(@"[a-zA-z]");
            //digitos del 0 al 9
            Regex numeros = new Regex(@"[0-9]");
            //cualquier caracter del conjunto
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");

            Boolean cumpleCriterios = false;

            //si no contiene las letras, regresa false
            if (!letras.IsMatch(_PasswordUser))
            {
                return false;
            }
            //si no contiene los numeros, regresa false
            if (!numeros.IsMatch(_PasswordUser))
            {
                return false;
            }

            //si no contiene los caracteres especiales, regresa false
            if (!caracEsp.IsMatch(_PasswordUser))
            {
                return false;
            }

            //si cumple con todo, regresa true
            return true;
        }
    }
}