using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using log4net;

namespace Smartax.Cronjob.Process.Clases.Utilidades
{
    public class EnviarEmails
    {
        #region DEFINICION DE VARIABLES Y PROPIEDADES
        public string EmailDe { get; set; }
        public string PassEmailDe { get; set; }
        public string EmailPara { get; set; }
        public string EmailCopia { get; set; }
        public string Asunto { get; set; }
        public string Detalle { get; set; }
        public string RutaFile { get; set; }
        public string ServerCorreo { get; set; }
        public int PuertoCorreo { get; set; }
        #endregion

        //Metodo para el envio normal del correo
        public bool SendEmail(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(EmailDe, PassEmailDe);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(ServerCorreo, PuertoCorreo);

            try
            {
                msg.From = new MailAddress(EmailPara);
                msg.To.Add(new MailAddress(EmailPara.ToString().Trim()));

                msg.Subject = Asunto.ToString().Trim();
                msg.Body = Detalle.ToString().Trim();

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
                FixedData.LogApi.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

        //Metodo para el envio de correo con archivo adjunto
        public bool SendEmailConCopia(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(FixedData.UserCorreoGmail, FixedData.PassCorreoGmail);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(FixedData.ServerCorreoGmail, FixedData.PuertoCorreoGmail);

            try
            {
                msg.From = new MailAddress(EmailPara);
                msg.To.Add(new MailAddress(EmailPara.ToString().Trim()));

                msg.Subject = Asunto.ToString().Trim();
                msg.Body = Detalle.ToString().Trim();

                if (EmailCopia != null)
                {
                    if (EmailCopia.ToString().Trim().Length > 0)
                    {
                        string destino = EmailCopia.ToString().Trim();
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
                FixedData.LogApi.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

        //Metodo para el envio de correo con archivo adjunto
        public bool SendEmailAdjunto(ref string _MsgError)
        {
            bool Enviado = true;
            NetworkCredential loginInfo = new NetworkCredential(FixedData.UserCorreoGmail, FixedData.PassCorreoGmail);
            MailMessage msg = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient smtpClient = new SmtpClient(FixedData.ServerCorreoGmail, FixedData.PuertoCorreoGmail);

            try
            {
                msg.From = new MailAddress(EmailPara);
                msg.To.Add(new MailAddress(EmailPara.ToString().Trim()));

                Attachment data = new Attachment(RutaFile.ToString().Trim(), MediaTypeNames.Application.Octet);
                msg.Subject = Asunto.ToString().Trim();
                msg.Body = Detalle.ToString().Trim();

                //if (EmailCopia != null)
                //{
                //    if (EmailCopia.ToString().Trim().Length > 0)
                //    {
                //        string destino = EmailCopia.ToString().Trim();
                //        string[] destinatario = destino.Split(';');
                //        foreach (string destinos in destinatario)
                //        {
                //            msg.To.Add(new MailAddress(destinos));
                //        }
                //    }
                //}

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
                FixedData.LogApi.Error(_MsgError);
                return Enviado;
            }

            return Enviado;
        }

    }
}
