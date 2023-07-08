using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using log4net;

namespace Smartax.Web.Application.Controles.General
{
    public partial class CtrlFichaUsuario : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (Convert.ToString(this.Session["TemaTXL"]) == "")
                //{
                //    CargarTema();
                //}

                if (Convert.ToString(this.Session["Login"]) == "")
                {
                    if (Request.ServerVariables["SCRIPT_NAME"].ToLower() != "/index.aspx" && Request.ServerVariables["SCRIPT_NAME"] != "/")
                    {
                        Response.Redirect("/");
                    }
                }

                if (!(this.Page.IsPostBack))
                {
                    if (Session["NombreCompletoUsuario"] != null)
                    {
                        this.LabelTimeOut.Text = Session.Timeout + ":00";
                        this.Page.Title = FixedData.PlatformName;
                        this.LabelNombre.Text = this.Session["NombreCompletoUsuario"].ToString().Trim();

                        //if (Session["IdComercio"] != null)
                        //{
                        //    this.LblEmpresa.Text = this.Session["NombreComercio"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    this.LblEmpresa.Text = this.Session["NombreEmpresa"].ToString().Trim();
                        //}

                        Byte[] ImagenByte = null;
                        if (Session["ImagenUsuario"] != null)
                        {
                            this.RadBinaryImage1.Visible = true;
                            this.ImgUsuario.Visible = false;

                            ImagenByte = (Byte[])Session["ImagenUsuario"];
                            //System.Drawing.Image imgUsuario = ObjUtils.GetByteImagen(ImagenByte);
                            this.RadBinaryImage1.DataValue = ImagenByte;
                        }
                        else
                        {
                            this.ImgUsuario.Visible = false;
                            this.RadBinaryImage1.Visible = true;

                            string strImgDefault = "Imagenes\\Temas\\Gris\\Imagenes\\anonimo.gif";
                            string cRutaImagen = HttpContext.Current.Server.MapPath("/" + strImgDefault.ToString().Trim());

                            ImagenByte = ObjUtils.GetImagenBytes(cRutaImagen);
                            this.RadBinaryImage1.DataValue = ImagenByte;
                            //this.ImgUsuario.ImageUrl = "~/Temas/Azul/Imagenes/anonimo.gif";
                        }
                    }
                }

                if (Convert.ToString(this.Session["Login"]) == "")
                {
                    PnlLoginOff.Visible = true;
                    PnlLoginOn.Visible = false;
                }
                else
                {
                    PnlLoginOff.Visible = false;
                    PnlLoginOn.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //_log.Error("Error en la Ficha usuario. Motivo: " + ex.Message);
            }
        }

    }
}