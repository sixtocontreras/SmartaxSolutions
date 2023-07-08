using Smartax.Web.Application.Clases.Parametros.Alumbrado;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Administracion.Alumbrado
{
    public partial class FrmCargueMasivo : System.Web.UI.Page
    {
        ConfConsumos consumos = new ConfConsumos();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            List<string> lista = new List<string>();
            var inputs = upl.UploadedFiles;
            UploadedFile file;
            if (inputs.Count > 0) {
                file = inputs[0];
                StreamReader reader = new StreamReader(file.InputStream);
                do
                {
                    lista.Add(reader.ReadLine());

                    // do your coding 
                    //Loop trough txt file and add lines to ListBox1  

                } while (reader.Peek() != -1);
                reader.Close();
            }

            var listaOk = new List<string>();
            var listaErrores = new List<string>();
            var row = 0;
            //DataTable TablaDatos = this.FuenteDatos.Tables["DtConsumos"];
            foreach (var item in lista)
            {
                if (row > 0)
                {

                    var fila = item.Split(';');
                    var error = false;
                    try
                    {
                        var data = double.Parse(fila[3]);
                        var list = fila[3].Split(',');
                        if (list.Length > 1)
                        {
                            if (list[1].Length > 2)
                            {
                                listaErrores.Add(item + ";El consumo debe ser un numero con maximo 2 decimales.");
                                error = true;
                            }
                        }
                        else {

                            list = fila[3].Split('.');
                            if (list.Length > 1)
                            {
                                if (list[1].Length > 2)
                                {
                                    listaErrores.Add(item + ";El consumo debe ser un numero con maximo 2 decimales.");
                                    error = true;
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                        listaErrores.Add(item + ";El costo debe ser un numero con maximo 2 decimales.");
                        error = true;
                    }
                    try
                    {
                        var data = double.Parse(fila[4]);
                        var list = fila[4].Split(',');
                        if (list.Length > 1)
                        {
                            if (list[1].Length > 2)
                            {
                                listaErrores.Add(item + ";El costo debe ser un numero con maximo 2 decimales.");
                                error = true;
                            }
                        }
                        else
                        {

                            list = fila[4].Split('.');
                            if (list.Length > 1)
                            {
                                if (list[1].Length > 2)
                                {
                                    listaErrores.Add(item + ";El costo debe ser un numero con maximo 2 decimales.");
                                    error = true;
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                        listaErrores.Add(item + ";El costo debe ser un numero con maximo 2 decimales.");
                        error = true;
                    }
                    if (!error)
                    {
                        if (lista.Count(x=>x.StartsWith($"{fila[0]};{fila[1]};{fila[2]};"))>1) {
                            listaErrores.Add(item + ";El registro esta duplicado en el archivo por oficina - vigencia - mes.");
                        }
                        else if(consumos.GetRow(ConfigurationManager.AppSettings["idCliente"], fila[0], fila[1], fila[2]).Rows.Count > 0)
                        {
                            listaErrores.Add(item + ";Ya existe registro para el municipio – oficina - vigencia - mes.");
                        }
                        else if (int.Parse(fila[1]) < 2000 || int.Parse(fila[1]) > 2100)
                        {
                            listaErrores.Add(item + ";La vigencia debe estar entre el 2000 y el 2100.");
                        }
                        else if (int.Parse(fila[2]) < 1 || int.Parse(fila[2]) > 12)
                        {
                            listaErrores.Add(item + ";El numero del mes debe ser entre 1 y 12.");
                        }
                        else if (consumos.GetOficina(fila[0]).Rows.Count == 0) {

                            listaErrores.Add(item + ";El codigo de oficina no existe.");
                        }
                        else
                        {
                            listaOk.Add(item);
                        }
                    }
                }
                row++;
            }
            if (listaErrores.Count == 0)
            {
                foreach (var item in listaOk)
                {
                    var fila = item.Split(';');
                    consumos.AddConfEntidad(fila[0], ConfigurationManager.AppSettings["idCliente"], fila[1], fila[2], fila[3], fila[4], Session["IdUsuario"].ToString().Trim());
                }
                //this.RadWindowManager1.ReloadOnShow = true;
                //this.RadWindowManager1.DestroyOnClose = true;
                //this.RadWindowManager1.Windows.Clear();
                //this.RadWindowManager1.Enabled = true;
                //this.RadWindowManager1.EnableAjaxSkinRendering = true;
                //this.RadWindowManager1.Visible = true;

                var _MsgError = "Cargue finalizado con exito.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Cargue exitoso", "reloadPage", "../../Imagenes/Iconos/16/check.png");
                //RadWindow Ventana = new RadWindow();
                //Ventana.Modal = true;
                //Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                //Ventana.ID = "RadWindow2";
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
            }
            else {

                var st = string.Join("||", listaErrores.ToArray());//.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('errores.csv', '{st}') ", true);

                var _MsgError = "El archivo presenta errores y no pudo ser cargado.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                //this.RadWindowManager1.ReloadOnShow = true;
                //this.RadWindowManager1.DestroyOnClose = true;
                //this.RadWindowManager1.Windows.Clear();
                //this.RadWindowManager1.Enabled = true;
                //this.RadWindowManager1.EnableAjaxSkinRendering = true;
                //this.RadWindowManager1.Visible = true;

                //RadWindow Ventana = new RadWindow();
                //Ventana.Modal = true;
                //Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                //Ventana.ID = "RadWindow2";
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

                //Response.Clear();
                //Response.ContentType = "application/CSV";
                //Response.AddHeader("content-disposition", "attachment;    filename=errores.csv");
                //Response.BinaryWrite(bytes);
                //Response.End();
            }


        }
    }
}