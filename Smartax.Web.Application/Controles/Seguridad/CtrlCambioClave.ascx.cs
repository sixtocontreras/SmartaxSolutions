using System;
using System.Text;
using System.Drawing;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlCambioClave : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CtrlCambioClave));

        Usuario ObjUser = new Usuario();
        Utilidades ObjUtils = new Utilidades();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = "Cambio de Clave";
                //--
                this.LblIdUsuario.Text = this.Session["IdUsuario"].ToString().Trim();
                this.LblLogin.Text = this.Session["LoginUsuario"].ToString().Trim();
                this.TxtNombres.Text = this.Session["NombreUsuario"].ToString().Trim();
                this.TxtApellidos.Text = this.Session["ApellidoUsuario"].ToString().Trim();
                this.TxtIdentificacion.Text = this.Session["Identificacion"].ToString().Trim();
                this.TxtEmail.Text = this.Session["EmailUsuario"].ToString().Trim();

                if (this.Session["CambiarClave"].ToString().Trim().Equals("S"))
                {
                    this.BtnCancelar.Visible = true;
                }
                else
                {
                    this.BtnCancelar.Visible = false;
                }

                this.TxtNuevoPassword.Focus();
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjUser.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.IdUsuario = Int32.Parse(this.LblIdUsuario.Text.ToString().Trim());
                ObjUser.NombreUsuario = this.TxtNombres.Text.ToString().Trim().ToUpper();
                ObjUser.ApellidoUsuario = this.TxtApellidos.Text.ToString().Trim().ToUpper();
                ObjUser.IdentificacionUsuario = this.TxtIdentificacion.Text.ToString().Trim().ToUpper();
                ObjUser.EmailUsuario = this.TxtEmail.Text.ToString().Trim();
                ObjUser.LoginUsuario = this.LblLogin.Text.ToString().Trim();
                string _PasswordUser = this.TxtNuevoPassword.Text.ToString().Trim();
                string _ConfPassUser = this.TxtConfirmarPassword.Text.ToString().Trim();
                //ObjUser.PasswordUsuario = ObjUtils.GetHashPassword(ObjUser.LoginUsuario, _PasswordUser);
                ObjUser.PasswordUsuario = _PasswordUser;
                ObjUser.CambiarClave = "N";
                int _DiasExpiraClave = Int32.Parse(this.Session["DiasExpClave"].ToString().Trim());
                DateTime dtFechaActual = DateTime.Now.AddDays(1);
                //ObjUser.FechaExpira = dtFechaActual.AddDays(_DiasExpiraClave).ToString("yyyy-MM-dd");
                ObjUser.IdEstado = 1;
                ObjUser.TipoProceso = 4;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjUser);
                _log.Warn("REQUEST CAMBIO CLAVE => " + jsonRequest);

                if (_PasswordUser.ToString().Trim().Length >= 8 && _PasswordUser.ToString().Trim().Length <= FixedData.LongitudClaveUsuario)
                {
                    if (ContrasenaSegura(_PasswordUser))
                    {
                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjUser.SetProcesoUsuario(ref _IdRegistro, ref _MsgError))
                        {
                            this.BtnGuardar.Enabled = false;
                            this.LblMensaje.ForeColor = Color.Black;
                            this.LblMensaje.Text = _MsgError;

                            #region ENVIO DE EMAIL AL USUARIO
                            _MsgError = "";
                            ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                            ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                            ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                            ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                            ObjCorreo.StrEmailPara = ObjUser.EmailUsuario.ToString().Trim();
                            ObjCorreo.StrAsunto = "Ref.: Cambio de Clave";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + ", Para informarle que Usted acaba de realizar el cambio de Clave para el Ingreso a [" + FixedData.PlatformName.ToString().Trim() + "].</h4>" +
                                            "<h4>DATOS DEL USUARIO</h4>" + "<br/>" +
                                            "Nombre de Usuario: " + ObjUser.NombreUsuario.ToString().Trim() + " " + ObjUser.ApellidoUsuario.ToString().Trim() + "<br/>" +
                                            "Identificación: " + ObjUser.IdentificacionUsuario.ToString().Trim() + "<br/>" +
                                            "Email: " + ObjUser.EmailUsuario.ToString().Trim() + "<br/>" +
                                            "Login Usuario: " + ObjUser.LoginUsuario.ToString().Trim() + "<br/>" +
                                            "Nuevo Password: " + this.TxtNuevoPassword.Text.ToString().Trim() + "<br/>" +
                                            "En caso de no haber sido usted por favor comuniquese de forma urgente con SOPORTE TÉCNICO." + "<br/>" +
                                            "<br/><br/>" +
                                            "<h4>INFORMACIÓN DE LA EMPRESA</h4>" +
                                            "Empresa: " + this.Session["NombreEmpresa"].ToString().Trim() + "<br/>" +
                                            "Dirección: " + this.Session["DireccionEmpresa"].ToString().Trim() + "<br/>" +
                                            "Url Página: " + FixedData.PlatformUrlPagina.ToString().Trim() + "<br/>" +
                                            "<br/><br/>" +
                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                            ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                            if (!ObjCorreo.SendEmailUser(ref _MsgError))
                            {
                                this.LblMensaje.Text = this.LblMensaje.Text + " Pero " + _MsgError.ToString().Trim();
                            }
                            #endregion

                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.ModuloApp = "CAMBIO_CLAVE";
                            ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            ObjAuditoria.DescripcionEvento = jsonRequest;
                            ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                            ObjAuditoria.TipoProceso = 1;

                            //'Agregar Auditoria del sistema
                            string _MsgErrorLogs = "";
                            if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                            {
                                _log.Error(_MsgErrorLogs);
                            }
                            #endregion
                        }
                        else
                        {
                            this.BtnGuardar.Enabled = true;
                            this.LblMensaje.ForeColor = Color.Red;
                            this.LblMensaje.Text = _MsgError;
                        }
                    }
                    else
                    {
                        this.BtnGuardar.Enabled = true;
                        this.LblMensaje.ForeColor = Color.Red;
                        this.LblMensaje.Text = "Señor usuario, la clave debe ser alfanumerica debe contener minimo una letra, numeros y caracteres especiales !";
                    }
                }
                else
                {
                    this.BtnGuardar.Enabled = true;
                    this.LblMensaje.ForeColor = Color.Red;
                    this.LblMensaje.Text = "Señor usuario, la clave debe tener una longitud minima de 8 y maximo " + FixedData.LongitudClaveUsuario + " caracteres !";
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al realizar el cambio de clave. Motivo: " + ex.Message.ToString().Trim();
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

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }
    }
}