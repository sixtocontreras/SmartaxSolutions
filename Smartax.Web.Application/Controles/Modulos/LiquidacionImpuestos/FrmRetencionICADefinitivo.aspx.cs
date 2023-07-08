using iTextSharp.text;
using iTextSharp.text.pdf;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmRetencionICADefinitivo : System.Web.UI.Page
    {
        Cliente ObjCliente = new Cliente();
        AdminRetencionICA _dataICA = new AdminRetencionICA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
            }
        }

        private void LoadCombos()
        {
            ObjCliente.TipoConsulta = 2;
            ObjCliente.IdEstado = 1;
            ObjCliente.MostrarSeleccione = "SI";
            ObjCliente.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
            ObjCliente.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
            ObjCliente.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            CmbCliente.DataSource = ObjCliente.GetClientes();
            CmbCliente.DataValueField = "id_cliente";
            CmbCliente.DataTextField = "nombre_cliente";
            CmbCliente.DataBind();
        }

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            var data = _dataICA.GetCliente(int.Parse(CmbCliente.SelectedValue));
            rblDocCliente.SelectedValue = data.Rows[0][0].ToString();
            lblNombres.Text = data.Rows[0][3].ToString();
            lblNumDoc.Text = data.Rows[0][1].ToString();
            lblDV2.Text = data.Rows[0][2].ToString();
            lblDireccion.Text = data.Rows[0][4].ToString();
            lblDptoCliente.Text = lblNombreDpto.Text = data.Rows[0][5].ToString();
            lblMunicipioCliente.Text = LblNombreMunicipio.Text = data.Rows[0][6].ToString();
            lblCorreo.Text = data.Rows[0][7].ToString();
            lblTelFijo.Text = data.Rows[0][8].ToString();
            lblFechaActual.Text = DateTime.UtcNow.AddHours(-5).ToString("dd-MM-yyyy");
            var firmante = _dataICA.GetFirmantes(false, CmbCliente.SelectedValue);
            var contador = _dataICA.GetFirmantes(true, CmbCliente.SelectedValue);
            Session["firmantes"] = firmante;
            Session["Contadores"] = contador;
            ddlFirmante.DataSource = firmante;
            ddlFirmante.DataValueField = "id_firmante";
            ddlFirmante.DataTextField = "nombre";
            ddlFirmante.DataBind();

            ddlContador.DataSource = contador;
            ddlContador.DataValueField = "id_firmante";
            ddlContador.DataTextField = "nombre";
            ddlContador.DataBind();

            pnlSelectCliente.Visible = false;
            PnlDatos.Visible = true;
        }

        

        protected void BtnSalirModal_Click(object sender, EventArgs e)
        {

        }

        protected void TxtCodDane_TextChanged(object sender, EventArgs e)
        {
            var data = _dataICA.GetUbicacion(TxtCodDane.Text);
            if (data.Rows.Count > 0)
            {
                LblNombreMunicipio.Text = data.Rows[0][0].ToString();
                lblNombreDpto.Text = data.Rows[0][1].ToString();
                hdIdMunicipio.Value = data.Rows[0][2].ToString();
                if (TxtAnioGravable.Text != string.Empty)
                {
                    GetData();
                }
            }
            else
            {
                LblNombreMunicipio.Text = "";
                lblNombreDpto.Text = "";
                hdIdMunicipio.Value = "";
                BtnGuardar.Enabled = false;
                if (TxtAnioGravable.Text != string.Empty)
                {
                    GetData();
                }
            }
        }

        protected void TxtAnioGravable_TextChanged(object sender, EventArgs e)
        {
            var data = _dataICA.GetUVT(TxtAnioGravable.Text);
            if (data.Rows.Count > 0)
            {
                hdUVT.Value = data.Rows[0][0].ToString();
                if (TxtCodDane.Text != string.Empty)
                {
                    GetData();
                }
            }

            else
            {
                hdUVT.Value = "";
                BtnGuardar.Enabled = false;
                if (TxtAnioGravable.Text != string.Empty)
                {
                    GetData();
                }
            }
        }

        protected void ddlPeriodoDeclarar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GetData()
        {
            if (string.IsNullOrEmpty(TxtAnioGravable.Text) || string.IsNullOrEmpty(hdIdMunicipio.Value))
            {
                lblFechaMaximaAno.Text = "";
                lblFechaMaximaMes.Text = "";
                lblFechaMaximaDia.Text = "";
                hdfechaMax.Value = "";
                BtnGuardar.Enabled = false;
            }
            else
            {
                var data = _dataICA.GetData(TxtAnioGravable.Text, hdIdMunicipio.Value);
                if (data.Rows.Count > 0)
                {
                    switch (data.Rows[0][1].ToString().Trim())
                    {
                        case "MENSUAL": rbMensual.Checked = true; break;
                        case "BIMESTRAL": rbBimestral.Checked = true; break;
                        case "TRIMESTRAL": rbTrimestral.Checked = true; break;
                        case "CUATRIMESTRAL": rbCuatrimestral.Checked = true; break;
                        case "SEMESTRAL": rbSemestral.Checked = true; break;
                        case "ANUAL": rbAnual.Checked = true; break;
                    }

                    var date = DateTime.Parse(data.Rows[0][4].ToString());
                    lblFechaMaximaAno.Text = date.Year.ToString();
                    lblFechaMaximaMes.Text = date.Month.ToString();
                    lblFechaMaximaDia.Text = date.Day.ToString();
                    hdfechaMax.Value = date.ToString("dd-MM-yyyy");
                    Session["dataPeriodicidad"] = data;
                    ddlPeriodoDeclarar.DataSource = data;
                    ddlPeriodoDeclarar.DataTextField = "periodicidad_impuesto";
                    ddlPeriodoDeclarar.DataValueField = "idperiodicidad_impuesto";
                    ddlPeriodoDeclarar.DataBind();
                    ddlPeriodoDeclarar.Visible = true;
                    GetDataSave(TxtAnioGravable.Text, hdIdMunicipio.Value, ddlPeriodoDeclarar.SelectedValue, int.Parse(CmbCliente.SelectedValue));
                   
                }
                else
                {
                    lblFechaMaximaAno.Text = "";
                    lblFechaMaximaMes.Text = "";
                    lblFechaMaximaDia.Text = "";
                    hdfechaMax.Value = "";
                    BtnGuardar.Enabled = false;
                }
            }
        }
        
        private bool GetDataSave(string anio, string municipio, string periodicidad, int cliente)
        {
            BtnGuardar.Enabled = true;
            var data = _dataICA.GetData(anio, municipio, periodicidad, cliente);
            var rta = false;
            if (data.Rows.Count > 0)
            {
                rta = true;
                var row = data.Rows[0];

                lblRenglon15.Text = $"{row["renglon15"]}";
                lblRenglon16.Text = $"{row["renglon16"]}";
                lblRenglon17.Text = $"{row["renglon17"]}";
                lblRenglon18.Text = $"{row["renglon18"]}";
                lblRenglon19.Text = $"{row["renglon19"]}";
                lblRenglon20.Text = $"{row["renglon20"]}";
                lblRenglon21.Text = $"{row["renglon21"]}";
                lblRenglon22.Text = $"{row["renglon22"]}";
                lblRenglon23.Text = $"{row["renglon23"]}";
                lblRenglon24.Text = $"{row["renglon24"]}";
                lblTotalRetenciones.Text = $"{row["renglon25"]}";
                lblRenglon26.Text = $"{row["renglon26"]}";
                lblRenglon27.Text = $"{row["renglon27"]}";
                lblRenglon28.Text = $"{row["renglon28"]}";
                lblRenglon29.Text = $"{row["renglon29"]}";
                lblRenglon30.Text = $"{row["renglon30"]}";
                lblRenglon31.Text = $"{row["renglon31"]}";
                lblRenglon32.Text = $"{row["renglon32"]}";
                lblRenglon33.Text = $"{row["renglon33"]}";
                lblRenglon34.Text = $"{row["renglon34"]}";
                lblRenglon35.Text = $"{row["renglon35"]}";
                lblTotalRetencionesPracticadas.Text = $"{row["renglon36"]}";
                lblRenglon37.Text = $"{row["renglon37"]}";
                lblRenglon38.Text = $"{row["renglon38"]}";
                lblRenglon39.Text = $"{row["renglon39"]}";
                lblRenglon40.Text = $"{row["renglon40"]}";
                lblRenglon41.Text = $"{row["renglon41"]}";
                lblTotalPagos.Text = $"{row["renglon41"]}";
                ddlFirmante.SelectedValue = row["id_firmante"].ToString();
                ddlContador.SelectedValue = row["id_contador"].ToString();
                hdIdRegistro.Value = row["id"].ToString();
                SetFirmas(row["id_firmante"].ToString(), row["id_contador"].ToString());
            }
            return rta;
        }

        private void SetFirmas(string idFirmante, string idContador)
        {
            var firmante = (DataTable)Session["firmantes"];
            var contador = (DataTable)Session["Contadores"];
            var rowf = 0;
            var rowc = 0;
            var row = 0;

            foreach (DataRow item in firmante.Rows)
            {
                if (item[0].ToString() == idFirmante)
                {
                    rowf = row;
                    break;
                }
                row++;
            }

            row = 0;
            foreach (DataRow item in contador.Rows)
            {
                if (item[0].ToString() == idContador)
                {
                    rowc = row;
                    break;
                }
                row++;
            }

            if (firmante.Rows[rowf][6].ToString() == "1")
            {
                lblccfirmante.Text = firmante.Rows[rowf][2].ToString();
                lblcefirmante.Text = "";
                rbccFirma.Checked = true;
                rbceFirma.Checked = false;
            }
            else
            {
                lblccfirmante.Text = "";
                lblcefirmante.Text = firmante.Rows[rowf][2].ToString();
                rbccFirma.Checked = false;
                rbceFirma.Checked = true;
            }

            if (contador.Rows[rowc][6].ToString() == "1")
            {
                lblccConta.Text = contador.Rows[rowc][2].ToString();
                lblceConta.Text = "";
                rbccConta.Checked = true;
                rbceConta.Checked = false;
            }
            else
            {
                lblccConta.Text = "";
                lblceConta.Text = contador.Rows[rowc][2].ToString();
                rbccConta.Checked = false;
                rbceConta.Checked = true;
            }

            rblContador.SelectedValue = (contador.Rows[rowc][2].ToString() == "6" ? 1 : 2).ToString();
            rbtpConta.Checked = true;
            lbltpConta.Text = contador.Rows[rowc][3].ToString();
            if (firmante.Rows[rowf][4].ToString() != string.Empty)
            {
                imgFirmante.ImageUrl = "data:image;base64," + Convert.ToBase64String((byte[])firmante.Rows[rowf][4]);
            }
            else
            {
                imgFirmante.ImageUrl = "";
            }
            if (contador.Rows[rowc][4].ToString() != string.Empty)
            {
                imgContador.ImageUrl = "data:image;base64," + Convert.ToBase64String((byte[])contador.Rows[rowc][4]);
            }
            else
            {
                imgContador.ImageUrl = "";
            }

        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            var hash = GenerarPdfReteICA(Server.MapPath("../../../Archivos/PDF/ReteICA.html"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "<script language='javascript'> window.open('" + ConfigurationManager.AppSettings["UrlBase"] + "/Archivos/PDF/pdfTemp" + hash + ".pdf', 'window','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes');</script>", false);
            Thread thread1 = new Thread(() => DoWork(Server.MapPath("../../../Archivos/PDF"), hash));
            thread1.Start();
        }

        public string GenerarPdfReteICA(string url)
        {
            byte[] bytes;

            var hash = DateTime.Now.ToString("ddMMyyyhhmm");
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        var example_html = File.ReadAllText(Server.MapPath("../../../Archivos/PDF/ReteICA.html"));
                        SetDataPdf(hash, ref example_html);
                        var example_css = File.ReadAllText(Server.MapPath("../../../Archivos/PDF/ReteICA.css"));
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {
                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);

                            }
                        }


                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            var file = Server.MapPath($"../../../Archivos/PDF/pdfTemp{hash}.pdf");
            File.WriteAllBytes(file, bytes);
            return hash;
        }

        private void SetDataPdf(string hash, ref string pdf)
        {
            var data = _dataICA.GetDataPdf(int.Parse(hdIdRegistro.Value));
            var sancion = string.Empty;
            
            var row = data.Rows[0];
            pdf = pdf.Replace("@renglon_15", $"{row["renglon15"]}");
            pdf = pdf.Replace("@renglon_16", $"{row["renglon16"]}");
            pdf = pdf.Replace("@renglon_17", $"{row["renglon17"]}");
            pdf = pdf.Replace("@renglon_18", $"{row["renglon18"]}");
            pdf = pdf.Replace("@renglon_19", $"{row["renglon19"]}");
            pdf = pdf.Replace("@renglon_20", $"{row["renglon20"]}");
            pdf = pdf.Replace("@renglon_21", $"{row["renglon21"]}");
            pdf = pdf.Replace("@renglon_22", $"{row["renglon22"]}");
            pdf = pdf.Replace("@renglon_23", $"{row["renglon23"]}");
            pdf = pdf.Replace("@renglon_24", $"{row["renglon24"]}");
            pdf = pdf.Replace("@renglon_25", $"{row["renglon25"]}");
            pdf = pdf.Replace("@renglon_26", $"{row["renglon26"]}");
            pdf = pdf.Replace("@renglon_27", $"{row["renglon27"]}");
            pdf = pdf.Replace("@renglon_28", $"{row["renglon28"]}");
            pdf = pdf.Replace("@renglon_29", $"{row["renglon29"]}");
            pdf = pdf.Replace("@renglon_30", $"{row["renglon30"]}");
            pdf = pdf.Replace("@renglon_31", $"{row["renglon31"]}");
            pdf = pdf.Replace("@renglon_32", $"{row["renglon32"]}");
            pdf = pdf.Replace("@renglon_33", $"{row["renglon33"]}");
            pdf = pdf.Replace("@renglon_34", $"{row["renglon34"]}");
            pdf = pdf.Replace("@renglon_35", $"{row["renglon35"]}");
            pdf = pdf.Replace("@renglon_36", $"{row["renglon36"]}");
            pdf = pdf.Replace("@renglon_37", $"{row["renglon37"]}");
            pdf = pdf.Replace("@renglon_38", $"{row["renglon38"]}");
            pdf = pdf.Replace("@renglon_39", $"{row["renglon39"]}");
            pdf = pdf.Replace("@renglon_40", $"{row["renglon40"]}");
            pdf = pdf.Replace("@renglon_41", $"{row["renglon41"]}");
            pdf = pdf.Replace("@renglon_42", $"{row["renglon42"]}");
        }

        public static void DoWork(string url, string hash)
        {

            Thread.Sleep(10000);
            try
            {
                File.Delete($"{url}/pdfTemp{hash}.pdf");
                File.Delete($"{url}/firmante{hash}.png");
                File.Delete($"{url}/contador{hash}.png");

            }
            catch (Exception)
            {
            }
        }
    }
}