using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class EnvioCorreo
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE VARIABLES Y PROPIEDADES
        private string _strEmailDe;
        private string _strPasswordDe;
        private string _strEmailPara;
        private string _strEmailCopia;
        private string _strAsunto;
        private string _strDetalle;
        private string _strRutaFile;
        private string _strServerCorreo;
        private int _PuertoCorreo;

        public string StrEmailDe
        {
            get { return _strEmailDe; }
            set { _strEmailDe = value; }
        }

        public string StrPasswordDe
        {
            get { return _strPasswordDe; }
            set { _strPasswordDe = value; }
        }

        public string StrEmailPara
        {
            get { return _strEmailPara; }
            set { _strEmailPara = value; }
        }

        public string StrEmailCopia
        {
            get { return _strEmailCopia; }
            set { _strEmailCopia = value; }
        }

        public string StrAsunto
        {
            get { return _strAsunto; }
            set { _strAsunto = value; }
        }

        public string StrDetalle
        {
            get { return _strDetalle; }
            set { _strDetalle = value; }
        }

        public string StrServerCorreo
        {
            get { return _strServerCorreo; }
            set { _strServerCorreo = value; }
        }

        public int PuertoCorreo
        {
            get { return _PuertoCorreo; }
            set { _PuertoCorreo = value; }
        }

        public string strRutaFile
        {
            get
            {
                return _strRutaFile;
            }

            set
            {
                _strRutaFile = value;
            }
        }

        #endregion

        //Metodo para el envio de correo con Archivo Adjunto
        public bool SendEmail(string cPathFile, ref string _MsgError)
        {
            bool Enviado = true;
            string PathFile = Directory.GetCurrentDirectory() + "\\correos_enviar.txt";

            //StringBuilder sMensaje = new StringBuilder();
            string sMensaje = "";
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(StrEmailPara);

                //string destino = "sixtocf@hotmail.com;danielh.gomez@telefonica.com";
                //string[] destinatario = destino.Split(';');

                //foreach (string destinos in destinatario)
                //{
                //    msg.To.Add(new MailAddress(destinos));
                //}

                StreamReader objReader = new StreamReader(PathFile.ToString().Trim());
                string sLine = "";
                do
                {
                    sLine = objReader.ReadLine();
                    if ((sLine != null))
                    {
                        msg.To.Add(new MailAddress(sLine.ToString().Trim()));
                    }
                } while (!(sLine == null));
                objReader.Close();

                Attachment data = new Attachment(cPathFile.ToString().Trim(), MediaTypeNames.Application.Octet);
                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();
                msg.Attachments.Add(data);

                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = "Se peodujo un Error. Motivo:. " + ex.Message.ToString().Trim();
                _log.Error(_MsgError);
                return Enviado;
            }
            return Enviado;

        }

        //Metodo para el envio normal del correo
        public bool SendEmail(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(StrEmailPara);
                msg.To.Add(new MailAddress(StrEmailPara.ToString().Trim()));

                if (StrEmailCopia.ToString().Trim().Length > 0)
                {
                    //Envia correo con copia
                    msg.CC.Add(new MailAddress(StrEmailCopia.ToString().Trim()));
                    //Envia correo con copia oculta
                    //msg.Bcc.Add(new MailAddress(StrEmailCopia.ToString().Trim()));
                }

                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();

                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = "Se produjo un Error. Motivo:. " + ex.Message.ToString().Trim();
                _log.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

        //Metodo para el envio normal del correo
        public bool SendEmailUser(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(StrEmailPara);
                msg.To.Add(new MailAddress(StrEmailPara.ToString().Trim()));

                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();

                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = "Error al enviar el correo. Motivo. " + ex.Message;
                _log.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

        //Metodo para el envio de correo con archivo adjunto
        public bool SendEmailConCopia(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(StrEmailPara);
                msg.To.Add(new MailAddress(StrEmailPara.ToString().Trim()));

                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();

                if (StrEmailCopia != null)
                {
                    if (StrEmailCopia.ToString().Trim().Length > 0)
                    {
                        string destino = StrEmailCopia.ToString().Trim();
                        string[] destinatario = destino.Split(';');
                        foreach (string destinos in destinatario)
                        {
                            msg.To.Add(new MailAddress(destinos));
                        }
                    }
                }

                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = "Se produjo un Error. Motivo:. " + ex.Message.ToString().Trim();
                _log.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

        //Metodo para el envio de correo con archivo adjunto
        public bool SendEmailAdjunto(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(StrEmailPara);
                msg.To.Add(new MailAddress(StrEmailPara.ToString().Trim()));

                Attachment data = new Attachment(strRutaFile.ToString().Trim(), MediaTypeNames.Application.Octet);
                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();

                if (StrEmailCopia != null)
                {
                    if (StrEmailCopia.ToString().Trim().Length > 0)
                    {
                        string destino = StrEmailCopia.ToString().Trim();
                        string[] destinatario = destino.Split(';');
                        foreach (string destinos in destinatario)
                        {
                            msg.To.Add(new MailAddress(destinos));
                        }
                    }
                }
                //Aqui adjuntamos el archivo ah enviar
                msg.Attachments.Add(data);
                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = "Se produjo un Error. Motivo:. " + ex.Message.ToString().Trim();
                _log.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }


        //Metodo para el envio normal del correo
        public bool SendEmailAlerta(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(StrEmailDe, StrPasswordDe);
            MailMessage msg = new MailMessage();
            SmtpClient smtpClient = new SmtpClient(StrServerCorreo, PuertoCorreo);

            try
            {
                //msg.From = new MailAddress(StrEmailPara);
                msg.From = new MailAddress(StrEmailDe);
                string destino = StrEmailPara.ToString().Trim();
                string[] destinatario = destino.Split(';');

                foreach (string destinos in destinatario)
                {
                    msg.To.Add(new MailAddress(destinos));
                }
                //msg.To.Add(new MailAddress(StrEmailPara.ToString().Trim()));

                string destinoCC = StrEmailCopia.ToString().Trim();
                string[] destinatarioCC = destinoCC.Split(';');

                foreach (string destinosCC in destinatarioCC)
                {
                    msg.CC.Add(new MailAddress(destinosCC));
                }
                //msg.CC.Add(new MailAddress(StrEmailCopia.ToString().Trim()));

                msg.Subject = StrAsunto.ToString().Trim();
                msg.Body = StrDetalle.ToString().Trim();

                msg.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                _MsgError = "La(s) Alerta ha sido enviada correctamente al Email [" + StrEmailPara + "]";
                Enviado = true;
            }
            catch (Exception ex)
            {
                Enviado = false;
                _MsgError = ex.Message.ToString().Trim();
            }

            return Enviado;
        }

    }
}