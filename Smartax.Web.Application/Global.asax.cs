using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PryInventario
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Tiempo de la sesión 20 minutos
            Session.Timeout = 120;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                if (ex.GetBaseException() != null)
                {
                    ex = ex.GetBaseException();
                    Application["TheException"] = ex; //store the error for later
                    string myParams = "MensajeError=" + ex.Message.ToString().Trim();
                    //myParams = Server.UrlEncode(myParams);

                    Server.ClearError(); //borrar el error para que podamos seguir adelante
                    try
                    {
                        Response.Redirect("../../FrmErrorPlataforma.aspx?MensajeError=" + ex.Message.ToString().Trim());
                    }
                    catch (Exception ex2)
                    {
                        Response.Redirect("../../FrmErrorPlataforma.aspx?MensajeError=" + ex2.Message.ToString().Trim());
                    }
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}