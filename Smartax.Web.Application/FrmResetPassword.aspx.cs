using System;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Web.Caching;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Text.RegularExpressions;

namespace Smartax.Web.Application
{
    public partial class FrmResetPassword : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Usuario ObjUser = new Usuario();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //Aqui se recoge la informacion de los datos parametrizados en el web.config
                this.ViewState["MotorBaseDatos"] = ConfigurationManager.AppSettings["BaseDatosUtilizar"].ToString().Trim();

                //Datos de email para enviar los correos.
                this.ViewState["EmailSoporte"] = ConfigurationManager.AppSettings["UsuarioEmail"];
                this.ViewState["PasswordEmail"] = ConfigurationManager.AppSettings["PasswordEmail"];

                //Datos de servidor de correo.
                this.ViewState["ServerCorreo"] = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"];
                this.ViewState["PuertoCorreo"] = ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"];

                this.TxtIdentificacion.Text = "";
                this.TxtIdentificacion.Focus();
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

        protected void BtnResetPass_Click(object sender, EventArgs e)
        {
            try
            {
                #region DEFINICION DE VALORES A PASAR AL OBJETO CLASE
                ObjUser.IdUsuario = null;
                ObjUser.NombreUsuario = "NA";
                ObjUser.LoginUsuario = this.TxtIdentificacion.Text.ToString().Trim();
                ObjUser.EmailUsuario = this.TxtEmail.Text.ToString().Trim().ToUpper();
                string _ClaveDinamica = ObjUtils.GetRandom();
                ObjUser.PasswordUsuario = _ClaveDinamica;
                ObjUser.CambiarClave = "S";
                ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                ObjUser.IdEstado = 1;
                ObjUser.IdUsuarioUp = -1;
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.TipoProceso = 4;
                #endregion

                int _IdRegistro = 0;
                string _Mensaje = "";
                if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _Mensaje))
                {
                    #region ENVIO DE CORREO AL USUARIO
                    ObjCorreo.StrServerCorreo = this.ViewState["ServerCorreo"].ToString().Trim();
                    ObjCorreo.PuertoCorreo = Int32.Parse(this.ViewState["PuertoCorreo"].ToString().Trim());
                    ObjCorreo.StrEmailDe = this.ViewState["EmailSoporte"].ToString().Trim();
                    ObjCorreo.StrPasswordDe = this.ViewState["PasswordEmail"].ToString().Trim();
                    ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();
                    ObjCorreo.StrAsunto = "REF.: RESET CLAVE DE USUARIO";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    strDetalleEmail.Append("<h4>" + strTime + " Señor Usuario, para informarle que usted acaba de realizar el reset de su clave para el Ingreso al sistema [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                    "<h4>RESET CLAVE DE USUARIO</h2>" + "<br/>" +
                                    "Nuevo Password: " + _ClaveDinamica.ToString().Trim() + "<br/>" +
                                    "<br/><br/>" +
                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                    ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                    string _MsgErrorEmail = "";
                    if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                    {
                        _Mensaje = _Mensaje + " Pero " + _MsgErrorEmail.ToString().Trim();
                    }
                    #endregion

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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al resetear el password del usuario. Motivo: " + ex.ToString();
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