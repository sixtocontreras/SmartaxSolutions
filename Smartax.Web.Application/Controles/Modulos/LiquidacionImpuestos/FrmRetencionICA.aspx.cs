using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmRetencionICA : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Cliente ObjCliente = new Cliente();
        AdminRetencionICA _dataICA = new AdminRetencionICA();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
        private static double renglon36, renglon39;
        private string jsonRequest;

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

        private void CleanForm()
        {
            lblNombreDpto.Text = string.Empty;
            LblNombreMunicipio.Text = string.Empty;
            lblTotalRetenciones.Text = string.Empty;
            hdIdMunicipio.Value = string.Empty;
            hdIdRegistro.Value = string.Empty;
            lblTotalRetencionesPracticadas.Text = string.Empty;
            TxtCodDane.Text = string.Empty;
            lblFechaMaximaAno.Text = string.Empty;
            lblFechaMaximaMes.Text = string.Empty;
            lblFechaMaximaDia.Text = string.Empty;
            TxtAnioGravable.Text = string.Empty;
            rbMensual.Checked = false;
            rbBimestral.Checked = false;
            rbTrimestral.Checked = false;
            rbCuatrimestral.Checked = false;
            rbSemestral.Checked = false;
            rbAnual.Checked = false;
            ddlPeriodoDeclarar.ClearSelection();
            ddlPeriodoDeclarar.Visible = false;
            rbInicial.Checked = false;
            rbPago.Checked = false;
            rbCorreccion.Checked = false;
            txtFormularioAnterior.Text = string.Empty;
            lblRenglon15.Text = string.Empty;
            lblRenglon16.Text = string.Empty;
            lblRenglon17.Text = string.Empty;
            lblRenglon18.Text = string.Empty;
            lblRenglon19.Text = string.Empty;
            lblRenglon20.Text = string.Empty;
            lblRenglon21.Text = string.Empty;
            lblRenglon22.Text = string.Empty;
            lblRenglon23.Text = string.Empty;
            lblRenglon24.Text = string.Empty;
            lblRenglon26.Text = string.Empty;
            lblRenglon27.Text = string.Empty;
            lblRenglon28.Text = string.Empty;
            lblRenglon29.Text = string.Empty;
            lblRenglon30.Text = string.Empty;
            lblRenglon31.Text = string.Empty;
            lblRenglon32.Text = string.Empty;
            lblRenglon33.Text = string.Empty;
            lblRenglon34.Text = string.Empty;
            lblRenglon35.Text = string.Empty;
            txtRenglon37.Text = string.Empty;
            lblRenglon38.Text = string.Empty;
            lblRenglon39.Text = string.Empty;
            txtRenglon40.Text = string.Empty;
            txtRenglon41.Text = string.Empty;
            lblTotalPagos.Text = string.Empty;
            BtnGuardar.Enabled = false;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;
        }

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            var data = _dataICA.GetCliente(int.Parse(CmbCliente.SelectedValue));
            rblDocCliente.SelectedValue = data.Rows[0][0].ToString();
            lblNombres.Text = data.Rows[0][3].ToString();
            lblNumDoc.Text = data.Rows[0][1].ToString();
            lblDV2.Text = data.Rows[0][2].ToString();
            lblDireccion.Text = data.Rows[0][4].ToString();
            //lblDptoCliente.Text = lblNombreDpto.Text = data.Rows[0][5].ToString();
            //lblMunicipioCliente.Text = LblNombreMunicipio.Text = data.Rows[0][6].ToString();
            lblDptoCliente.Text = data.Rows[0][5].ToString();
            lblMunicipioCliente.Text = data.Rows[0][6].ToString();
            lblCorreo.Text = data.Rows[0][7].ToString();
            lblTelFijo.Text = data.Rows[0][8].ToString();
            lblFechaActual.Text = DateTime.UtcNow.AddHours(-5).ToString("dd-MM-yyyy");
            if (data.Rows[0][9].ToString() == "S")
            {
                RadioButton5.Checked = true;
            }
            else
            {
                RadioButton6.Checked = true;
            }

            var dataCo = _dataICA.GetClienteContribuyente(int.Parse(CmbCliente.SelectedValue));
            LblTA.Text = dataCo.Rows[0][2].ToString();

            var dataTA = _dataICA.GetClienteTA(int.Parse(CmbCliente.SelectedValue));
            if (dataTA.Rows[0][1].ToString() == "1") {
                RadioButton4.Checked = true;
            }
            else
            {
                RadioButton2.Checked = true;
            }

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
            setFirmas();
            pnlSelectCliente.Visible = false;
            PnlDatos.Visible = true;
            BtnGuardar.Enabled = false;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;
        }

        private void setFirmas()
        {
            var firmante = (DataTable)Session["firmantes"];
            var contador = (DataTable)Session["Contadores"];
            var rowf = 0;
            var rowc = 0;
            var row = 0;
            foreach (DataRow item in firmante.Rows)
            {
                if (item[0].ToString() == ddlFirmante.SelectedValue)
                    rowf = row;
                row++;
            }

            row = 0;
            foreach (DataRow item in contador.Rows)
            {
                if (item[0].ToString() == ddlContador.SelectedValue)
                    rowc = row;
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

        protected void TxtCodDane_TextChanged(object sender, EventArgs e)
        {
            LblNombreMunicipio.Text = "";
            lblNombreDpto.Text = "";
            hdIdMunicipio.Value = "";
            hdIdRegistro.Value = string.Empty;
            TxtAnioGravable.Text = "";
            lblFechaMaximaAno.Text = "";
            lblFechaMaximaMes.Text = "";
            lblFechaMaximaDia.Text = "";
            rbMensual.Checked = false;
            rbBimestral.Checked = false;
            rbTrimestral.Checked = false;
            rbCuatrimestral.Checked = false;
            rbSemestral.Checked = false;
            rbAnual.Checked = false;
            ddlPeriodoDeclarar.ClearSelection();
            ddlPeriodoDeclarar.Visible = false;
            BtnGuardar.Enabled = false;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;
            lblRenglon15.Text = "";
            lblRenglon16.Text = "";
            lblRenglon17.Text = "";
            lblRenglon18.Text = "";
            lblRenglon19.Text = "";
            lblRenglon20.Text = "";
            lblRenglon21.Text = "";
            lblRenglon22.Text = "";
            lblRenglon23.Text = "";
            lblRenglon24.Text = "";
            lblTotalRetenciones.Text = "";
            lblRenglon26.Text = "";
            lblRenglon27.Text = "";
            lblRenglon28.Text = "";
            lblRenglon29.Text = "";
            lblRenglon30.Text = "";
            lblRenglon31.Text = "";
            lblRenglon32.Text = "";
            lblRenglon33.Text = "";
            lblRenglon34.Text = "";
            lblRenglon35.Text = "";
            lblTotalRetencionesPracticadas.Text = "";
            txtRenglon37.Text = "";
            lblRenglon38.Text = "";
            lblRenglon39.Text = "";
            txtRenglon40.Text = "";
            txtRenglon41.Text = "";
            lblTotalPagos.Text = "";
            var data = _dataICA.GetUbicacion(TxtCodDane.Text);
            if (data.Rows.Count > 0)
            {
                LblNombreMunicipio.Text = data.Rows[0][0].ToString();
                lblNombreDpto.Text = data.Rows[0][1].ToString();
                hdIdMunicipio.Value = data.Rows[0][2].ToString();
                //if (TxtAnioGravable.Text != string.Empty)
                //{
                //    GetData();
                //}
            }
            else
            {
                TxtCodDane.Text = "";
            }
            /*
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
            */
        }

        protected void TxtAnioGravable_TextChanged(object sender, EventArgs e)
        {
            lblFechaMaximaAno.Text = "";
            lblFechaMaximaMes.Text = "";
            lblFechaMaximaDia.Text = "";
            hdIdRegistro.Value = string.Empty;
            rbMensual.Checked = false;
            rbBimestral.Checked = false;
            rbTrimestral.Checked = false;
            rbCuatrimestral.Checked = false;
            rbSemestral.Checked = false;
            rbAnual.Checked = false;
            ddlPeriodoDeclarar.ClearSelection();
            ddlPeriodoDeclarar.Visible = false;
            BtnGuardar.Enabled = false;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;
            lblRenglon15.Text = "";
            lblRenglon16.Text = "";
            lblRenglon17.Text = "";
            lblRenglon18.Text = "";
            lblRenglon19.Text = "";
            lblRenglon20.Text = "";
            lblRenglon21.Text = "";
            lblRenglon22.Text = "";
            lblRenglon23.Text = "";
            lblRenglon24.Text = "";
            lblTotalRetenciones.Text = "";
            lblRenglon26.Text = "";
            lblRenglon27.Text = "";
            lblRenglon28.Text = "";
            lblRenglon29.Text = "";
            lblRenglon30.Text = "";
            lblRenglon31.Text = "";
            lblRenglon32.Text = "";
            lblRenglon33.Text = "";
            lblRenglon34.Text = "";
            lblRenglon35.Text = "";
            lblTotalRetencionesPracticadas.Text = "";
            txtRenglon37.Text = "";
            lblRenglon38.Text = "";
            lblRenglon39.Text = "";
            txtRenglon40.Text = "";
            txtRenglon41.Text = "";
            lblTotalPagos.Text = "";

            if (TxtCodDane.Text != string.Empty)
            {
                GetData();
            }
            else
            {
                TxtAnioGravable.Text = "";
            }

            /*
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
                    ddlPeriodoDeclarar.Visible = false;
                    GetData();
                }
            }
            */
        }

        private void GetData()
        {
            if (string.IsNullOrEmpty(TxtAnioGravable.Text) || string.IsNullOrEmpty(hdIdMunicipio.Value))
            {
                lblFechaMaximaAno.Text = "";
                lblFechaMaximaMes.Text = "";
                lblFechaMaximaDia.Text = "";
                hdfechaMax.Value = "";
                //BtnGuardar.Enabled = false;
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
                    if (!GetDataSave(TxtAnioGravable.Text, hdIdMunicipio.Value, ddlPeriodoDeclarar.SelectedValue, int.Parse(CmbCliente.SelectedValue)))
                    {
                        SetDatosCalculados(data.Rows[0]);
                    }
                    else
                    {
                        BtnGuardar.Enabled = false;
                        BtnBorrar.Enabled = true;
                        btnPdf.Enabled = true;
                    }
                 }
                else
                {
                    lblFechaMaximaAno.Text = "";
                    lblFechaMaximaMes.Text = "";
                    lblFechaMaximaDia.Text = "";
                    hdfechaMax.Value = "";
                    BtnGuardar.Enabled = false;
                    BtnBorrar.Enabled = false;
                    btnPdf.Enabled = false;
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
                txtRenglon37.DisplayText = $"{row["renglon37"]}";
                lblRenglon38.Text = $"{row["renglon38"]}";
                lblRenglon39.Text = $"{row["renglon39"]}";
                txtRenglon40.DisplayText = $"{row["renglon40"]}";
                txtRenglon41.DisplayText = $"{row["renglon41"]}";
                lblTotalPagos.Text = $"{row["renglon42"]}";
                ddlFirmante.SelectedValue = row["id_firmante"].ToString();
                ddlContador.SelectedValue = row["id_contador"].ToString();
                hdIdRegistro.Value = row["id"].ToString();
                //BtnGuardar.Enabled = false;
                setFirmas();
                btnPdf.Enabled = true;
                BtnBorrar.Enabled = true;
            }
            return rta;
        }

        private void SetDatosCalculados(DataRow row)
        {
            lblRenglon24.Text = $"{0:N0}";
            //lblRenglon34.Text = $"{0:N0}";
            lblRenglon35.Text = $"{0:N0}";
            txtRenglon37.Text = "0";
            lblRenglon38.Text = $"{0:N0}";
            txtRenglon40.Text = "0";
            txtRenglon41.Text = "0";

            hdIdRegistro.Value = string.Empty;
            int anio = int.Parse(TxtAnioGravable.Text);
            int periodicidad = int.Parse(ddlPeriodoDeclarar.SelectedValue);

            DataTable dtReglonCuenta15_20;
            var results15_20 = _dataICA.GetFilas15_20(anio, periodicidad, TxtCodDane.Text, out dtReglonCuenta15_20);
            var dataRow = results15_20.Rows[0];

            double totalRenglon15 = (Math.Round(GetValor15_20(15, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon15.Text = $"{totalRenglon15:N0}";

            double totalRenglon16 = (Math.Round(GetValor15_20(16, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon16.Text = $"{totalRenglon16:N0}";

            double totalRenglon17 = (Math.Round(GetValor15_20(17, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon17.Text = $"{totalRenglon17:N0}";

            double totalRenglon18 = (Math.Round(GetValor15_20(18, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon18.Text = $"{totalRenglon18:N0}";

            double totalRenglon19 = (Math.Round(GetValor15_20(19, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon19.Text = $"{totalRenglon19:N0}";

            double totalRenglon20 = (Math.Round(GetValor15_20(20, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon20.Text = $"{totalRenglon20:N0}";

            DataTable dtReglonCuenta26_31;
            var results26_31 = _dataICA.GetFilas26_31(anio, periodicidad, TxtCodDane.Text, out dtReglonCuenta26_31);

            double totalRenglon26 = (Math.Round(GetValor26_31(26, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon26.Text = $"{totalRenglon26:N0}";

            double totalRenglon27 = (Math.Round(GetValor26_31(27, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon27.Text = $"{totalRenglon27:N0}";

            double totalRenglon28 = (Math.Round(GetValor26_31(28, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon28.Text = $"{totalRenglon28:N0}";

            double totalRenglon29 = (Math.Round(GetValor26_31(29, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon29.Text = $"{totalRenglon29:N0}";

            double totalRenglon30 = (Math.Round(GetValor26_31(30, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon30.Text = $"{totalRenglon30:N0}";

            double totalRenglon31 = (Math.Round(GetValor26_31(31, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon31.Text = $"{totalRenglon31:N0}";

            double renglon21_22_23 = totalRenglon26 + totalRenglon27 + totalRenglon28 + totalRenglon29 + totalRenglon30 + totalRenglon31;
            //lblRenglon21.Text = lblRenglon22.Text = lblRenglon23.Text = $"{renglon21_22_23:N0}";

            int municipioId = int.Parse(hdIdMunicipio.Value);
            double totalRenglon32 = (Math.Round(GetValorTarifa(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdAvisosTableros"]), anio) / 1000) * 1000);
            lblRenglon32.Text = $"{totalRenglon32:N0}";
            //--AQUI VALIDAMOS SI TIENE TARIFA CONFIGURADA EN LOS IMPUESTOS
            double totalRenglon21 = 0;
            if(totalRenglon32 > 0)
            {
                totalRenglon21 = renglon21_22_23;
                lblRenglon21.Text = $"{totalRenglon21:N0}";
            }
            else
            {
                totalRenglon21 = 0;
                lblRenglon21.Text = "0";
            }

            double totalRenglon33 = (Math.Round(GetValorTarifa(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdSobretasaBomberil"]), anio) / 1000) * 1000);
            lblRenglon33.Text = $"{totalRenglon33:N0}";
            //--AQUI VALIDAMOS SI TIENE TARIFA CONFIGURADA EN LOS IMPUESTOS
            double totalRenglon22 = 0;
            if (totalRenglon33 > 0)
            {
                totalRenglon22 = renglon21_22_23;
                lblRenglon22.Text = $"{totalRenglon22:N0}";
            }
            else
            {
                totalRenglon22 = 0;
                lblRenglon22.Text = "0";
            }

            double totalRenglon34 = (Math.Round(GetValorTarifa(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdReteAplicadasPesasMedidas"]), anio) / 1000) * 1000);
            lblRenglon34.Text = $"{totalRenglon34:N0}";
            //--AQUI VALIDAMOS SI TIENE TARIFA CONFIGURADA EN LOS IMPUESTOS
            double totalRenglon23 = 0;
            if (totalRenglon34 > 0)
            {
                totalRenglon23 = renglon21_22_23;
                lblRenglon23.Text = $"{totalRenglon23:N0}";
            }
            else
            {
                totalRenglon23 = 0;
                lblRenglon23.Text = "0";
            }

            double totalRenglon35 = (Math.Round(GetOtrasConfiguraciones(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdReteICA"]), anio) / 100) * 100);
            lblRenglon35.Text = $"{totalRenglon35:N0}";
            double totalRenglon24 = 0;

            double totalRenglones26_35 = totalRenglon26 + totalRenglon27 + totalRenglon28 + totalRenglon29 + totalRenglon30 + totalRenglon31 + totalRenglon32 + totalRenglon33 + totalRenglon34 + totalRenglon35;
            lblTotalRetencionesPracticadas.Text = $"{totalRenglones26_35:N0}";
            renglon36 = totalRenglones26_35;

            lblRenglon39.Text = $"{totalRenglones26_35:N0}";
            lblTotalPagos.Text = $"{totalRenglones26_35:N0}";

            //double totalRenglones15_20 = totalRenglon15 + totalRenglon16 + totalRenglon17 + totalRenglon18 + totalRenglon19 + totalRenglon20 + (renglon21_22_23 * 3);
            double totalRenglones15_20 = totalRenglon15 + totalRenglon16 + totalRenglon17 + totalRenglon18 + totalRenglon19 + totalRenglon20 + totalRenglon21 + totalRenglon22 + totalRenglon23 + totalRenglon24;
            lblTotalRetenciones.Text = $"{totalRenglones15_20:N0}";

            BtnGuardar.Enabled = true;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;

            //var sect = _dataICA.GetSector(hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
            //var especial = false;
            //if (sect.Rows.Count > 0)
            //{
            //    especial = sect.Rows[0][0].ToString() == "5";
            //}
            //if (especial)
            //{
            //    var data = _dataICA.GetDatosCalculados(hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
            //    if (data.Rows.Count > 0)
            //    {

            //        //txtMora.Text = "";
            //        //txtSanciones.Text = "";
            //        //rblSector.SelectedValue = "5";
            //        //lblRenglon2.Enabled = false;
            //        //lblRenglon3.Enabled = false;
            //        //lblClasificacion.Text = data.Rows[0][0].ToString();
            //        //lblBaseGravableEspecial.Text = double.Parse(data.Rows[0][1].ToString()).ToString();
            //        //var totalImp = Math.Round((double.Parse(data.Rows[0][1].ToString()) * double.Parse(hdUVT.Value)) / 1000) * 1000;
            //        //lblImpuestoEspecial.Text = totalImp.ToString("C2");
            //        //lblTotalImp.Text = lblImpuestoEspecial.Text;
            //        //var descuento = (DateTime.Parse(row[4].ToString()) >= DateTime.Now ? double.Parse(row[5].ToString()) : 0);
            //        //hddescuentoporcentaje.Value = descuento.ToString();
            //        //var descuentofinal = ((totalImp * descuento) / 100);
            //        //lblDescuento.Text = (Math.Round(descuentofinal / 1000) * 1000).ToString("C2");
            //        //var total = (Math.Round((totalImp - descuentofinal + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text)) / 1000) * 1000);
            //        //lblTotal.Text = total.ToString("C2");
            //        BtnGuardar.Enabled = true;
            //    }
            //}
            //else
            //{
            //    var data = _dataICA.GetDatosCalculados(double.Parse(hdUVT.Value), ddlPeriodicidad.SelectedValue, hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
            //    if (data.Rows.Count > 0)
            //    {
            //        //txtMora.Text = "";
            //        //txtSanciones.Text = "";
            //        //lblRenglon2.Enabled = true;
            //        //lblRenglon3.Enabled = true;
            //        //rblSector.SelectedValue = data.Rows[0][1].ToString();
            //        //lblClasificacion.Text = data.Rows[0][7].ToString();
            //        //lblRenglon2.Text = data.Rows[0][3].ToString();
            //        //lblRenglon3.Text = data.Rows[0][4].ToString();
            //        //lblBaseGravable.Text = double.Parse(data.Rows[0][5].ToString()).ToString("C2");
            //        //lblTarifaImpuesto.Text = $"{data.Rows[0][8].ToString()}%";
            //        //var impu = double.Parse(data.Rows[0][11].ToString());
            //        var pagomin = double.Parse(data.Rows[0][12].ToString());
            //        var pagomax = double.Parse(data.Rows[0][13].ToString());
            //        //lblImpuesto.Text = impu.ToString("C2");
            //        //lblUVTMin.Text = data.Rows[0][9].ToString();
            //        //lblUVTMax.Text = data.Rows[0][10].ToString();
            //        //lblPagoMinimo.Text = pagomin.ToString("C2");
            //        //lblPagoMax.Text = pagomax.ToString("C2");
            //        //var totalImp = (impu < pagomin ? pagomin : (impu > pagomax ? pagomax : impu));
            //        //lblTotalImp.Text = totalImp.ToString("C2");
            //        //var descuento = (DateTime.Parse(row[4].ToString()) >= DateTime.Now ? double.Parse(row[5].ToString()) : 0);
            //        //hddescuentoporcentaje.Value = descuento.ToString();
            //        //var descuentofinal = ((totalImp * descuento) / 100);
            //        //lblDescuento.Text = (Math.Round(descuentofinal / 1000) * 1000).ToString("C2");
            //        //var total = (Math.Round((totalImp - descuentofinal + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text)) / 1000) * 1000);
            //        //lblTotal.Text = total.ToString("C2");
            //        BtnGuardar.Enabled = true;
            //    }

            //    else
            //    {
            //        rblSector.ClearSelection();
            //        //lblClasificacion.Text = "";
            //        //lblRenglon2.Text = "";
            //        //lblRenglon3.Text = "";
            //        //lblBaseGravable.Text = "";
            //        //lblTarifaImpuesto.Text = "";
            //        //lblImpuesto.Text = "";
            //        //hddescuentoporcentaje.Value = "";
            //        //lblBaseGravableEspecial.Text = "";
            //        //lblImpuestoEspecial.Text = "";
            //        //lblUVTMin.Text = "";
            //        //lblUVTMax.Text = "";
            //        //lblPagoMinimo.Text = "";
            //        //lblPagoMax.Text = "";
            //        //lblTotalImp.Text = "";
            //        //lblDescuento.Text = "";
            //        //lblTotal.Text = "";
            //        //txtMora.Text = "";
            //        //txtSanciones.Text = "";
            //        BtnGuardar.Enabled = false;
            //    }
            //}

        }
        
        protected void ddlFirmante_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFirmas();
        }

        protected void ddlContador_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFirmas();
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {

        }

        protected void BtnSalirModal_Click(object sender, EventArgs e)
        {

        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            var hash = GenerarPdfReteICA(Server.MapPath("../../../Archivos/PDF/ReteICA.html"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "<script language='javascript'> window.open('" + ConfigurationManager.AppSettings["UrlBase"] + "/Archivos/PDF/pdfTemp" + hash + ".pdf', 'window','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes');</script>", false);
            Thread thread1 = new Thread(() => DoWork(Server.MapPath("../../../Archivos/PDF"), hash));
            thread1.Start();
           // GetData();
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
                        setDataPdf(hash, ref example_html);
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
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                bytes = AddWatermark(bytes, bfTimes, "BORRADOR");
            }
            var file = Server.MapPath($"../../../Archivos/PDF/pdfTemp{hash}.pdf");
            System.IO.File.WriteAllBytes(file, bytes);
            return hash;
        }

        private static byte[] AddWatermark(byte[] bytes, BaseFont baseFont, string watermarkText)
        {
            using (var ms = new MemoryStream(10 * 1024))
            {
                using (var reader = new PdfReader(bytes))
                using (var stamper = new PdfStamper(reader, ms))
                {
                    var pages = reader.NumberOfPages;
                    for (var i = 1; i <= pages; i++)
                    {
                        var dc = stamper.GetOverContent(i);
                        AddWaterMarkText(dc, watermarkText, baseFont, 72, 45, BaseColor.GRAY, reader.GetPageSizeWithRotation(i));
                    }
                    stamper.Close();
                }
                return ms.ToArray();
            }
        }

        public static void AddWaterMarkText(PdfContentByte pdfData, string watermarkText, BaseFont font, float fontSize, float angle, BaseColor color, Rectangle realPageSize)
        {
            var gstate = new PdfGState { FillOpacity = 0.35f, StrokeOpacity = 0.3f };
            pdfData.SaveState();
            pdfData.SetGState(gstate);
            pdfData.SetColorFill(color);
            pdfData.BeginText();
            pdfData.SetFontAndSize(font, fontSize);
            var x = (realPageSize.Right + realPageSize.Left) / 2;
            var y = (realPageSize.Bottom + realPageSize.Top) / 2;
            pdfData.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, angle);
            pdfData.EndText();
            pdfData.RestoreState();
        }

        private void setDataPdf(string hash, ref string pdf)
        {
            var data = _dataICA.GetDataPdf(int.Parse(hdIdRegistro.Value));
            var sancion = string.Empty;

            //switch (data.Rows[0][50].ToString())
            //{
            //    case "1":
            //        sancion = "Extemporaneidad";
            //        break;
            //    case "2":
            //        sancion = "Corrección";
            //        break;
            //    case "3":
            //        sancion = "Inexactitud";
            //        break;
            //    case "4":
            //        sancion = "Emplazamiento";
            //        break;
            //    case "5":
            //        sancion = "Otra";
            //        break;
            //}
            //var f1 = false;
            //var c1 = false;
            //var firmante = Server.MapPath($"../../../Archivos/PDF/firmante{hash}.png");
            //var contador = Server.MapPath($"../../../Archivos/PDF/contador{hash}.png");
            //if (data.Rows[0][7].ToString() != "")
            //{
            //    System.IO.File.WriteAllBytes(firmante, ((byte[])data.Rows[0][7]));
            //    f1 = true;
            //}
            //if (data.Rows[0][7].ToString() != "")
            //{
            //    System.IO.File.WriteAllBytes(firmante, ((byte[])data.Rows[0][13]));
            //    c1 = true;
            //}
            var row = data.Rows[0];

            pdf = pdf.Replace("@nombre_departamento", $"{row["nombre_departamento"]}");
            pdf = pdf.Replace("@cod_dane", $"{row["cod_dane"]}");
            pdf = pdf.Replace("@nombre_municipio", $"{row["nombre_municipio"]}");
            pdf = pdf.Replace("@anio_declarable", $"{row["anio_declarable"]}");
            pdf = pdf.Replace("@periodicidad", $"{row["periodicidad"]}");
            pdf = pdf.Replace("@fecha_maxima", $"{row["fecha_maxima"]}");
            pdf = pdf.Replace("@nombre_razon_social", $"{row["nombre_razon_social"]}");
            pdf = pdf.Replace("@num_ident_tributaria", $"{row["numero_iden"]}");
            pdf = pdf.Replace("@dv", $"{row["digito_v"]}");
            pdf = pdf.Replace("@dir_notificacion", $"{row["direccion_not"]}");
            pdf = pdf.Replace("@municipio", $"{row["ciu_not"]}");
            pdf = pdf.Replace("@correo_electronico", $"{row["correoe_not"]}");
            pdf = pdf.Replace("@telefono_not", $"{row["telefono_not"]}");
            pdf = pdf.Replace("@celular_not", " ");
            pdf = pdf.Replace("@tipo_cont", $"{row["tipo_cont"]}");
            pdf = pdf.Replace("@tipo_act", $"{row["tipo_act"]}");
            pdf = pdf.Replace("@gran_contribuyente", $"{row["gran_contribuyente"]}");
            pdf = pdf.Replace("@departamento", $"{row["depto_not"]}");
            pdf = pdf.Replace("@tipo_iden", $"{row["tipo_iden"]}");



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


            pdf = pdf.Replace("@nombreFirmante", $"{row["nombre_firmante"]}");
            pdf = pdf.Replace("@nombreContador", $"{row["nombre_contador"]}");
            pdf = pdf.Replace("@tipodoc_firmante", $"{row["tipo_iden_firmante"]}");
            pdf = pdf.Replace("@tipodoc_contador", $"{row["tipo_iden_contador"]}");
            pdf = pdf.Replace("@doc_firmante", $"{row["numero_iden_firmante"]}");
            pdf = pdf.Replace("@doc_contador", $"{row["numero_iden_contador"]}");
            pdf = pdf.Replace("@tp_contador", $"{row["tarjeta_profesional"]}");

            //pdf = pdf.Replace("@periodicidad", data.Rows[0][0].ToString())
            //    .Replace("@dataperiodicidad", data.Rows[0][1].ToString())
            //    .Replace("@tipodoc_cli", data.Rows[0][2].ToString())
            //    .Replace("@sector", data.Rows[0][3].ToString())
            //    .Replace("@tipodoc_firmante", data.Rows[0][4].ToString())
            //    .Replace("@doc_firmante", data.Rows[0][5].ToString())
            //    .Replace("@nombreFirmante", data.Rows[0][6].ToString())
            //    .Replace("@declaracioncorrige", "")
            //    .Replace("@imgFirmante", (f1 ? $"<img style='height:80px' src='{ConfigurationManager.AppSettings["UrlBase"]}/{$"Archivos/PDF/firmante{hash}.png"}' />" : ""))
            //    .Replace("@tipodoc_contador", data.Rows[0][8].ToString())
            //    .Replace("@doc_contador", data.Rows[0][9].ToString())
            //    .Replace("@contadorTitulo", data.Rows[0][10].ToString())
            //    .Replace("@tp_contador", data.Rows[0][11].ToString())
            //    .Replace("@nombreContador", data.Rows[0][12].ToString())
            //    .Replace("@imgContador", (c1 ? $"<img style='height:80px' src='{ConfigurationManager.AppSettings["UrlBase"]}/{$"Archivos/PDF/contador{hash}.png"}' />" : ""))
            //    //.Replace("@imgContador", Convert.ToBase64String(((byte[])data.Rows[0][13])))
            //    .Replace("@cod_dane", data.Rows[0][16].ToString())
            //    .Replace("@municipio", data.Rows[0][17].ToString())
            //    .Replace("@dpto_cli", data.Rows[0][30].ToString())
            //    .Replace("@dpto", data.Rows[0][19].ToString())
            //    .Replace("@anio", data.Rows[0][20].ToString())
            //    .Replace("@uvt", data.Rows[0][21].ToString())
            //    .Replace("@dd", DateTime.Parse(data.Rows[0][22].ToString()).Day.ToString())
            //    .Replace("@mm", DateTime.Parse(data.Rows[0][22].ToString()).Month.ToString())
            //    .Replace("@aaaa", DateTime.Parse(data.Rows[0][22].ToString()).Year.ToString())
            //    .Replace("@nombre_cli", data.Rows[0][24].ToString())
            //    .Replace("@numdoc_cli", data.Rows[0][26].ToString())
            //    .Replace("@dv_cli", data.Rows[0][27].ToString())
            //    .Replace("@dir_cli", data.Rows[0][28].ToString())
            //    .Replace("@muni_cli", data.Rows[0][29].ToString())
            //    .Replace("@mail_cli", data.Rows[0][31].ToString())
            //    .Replace("@tel_cli", data.Rows[0][32].ToString())
            //    .Replace("@cel_cli", data.Rows[0][33].ToString())
            //    .Replace("@renglon_1", data.Rows[0][35].ToString())
            //    .Replace("@renglon2", data.Rows[0][36].ToString())
            //    .Replace("@renglon3", data.Rows[0][37].ToString())
            //    .Replace("@renglon4", double.Parse(data.Rows[0][38].ToString() == "" ? "0" : data.Rows[0][38].ToString()).ToString("C2"))
            //    .Replace("@renglon5", double.Parse(data.Rows[0][39].ToString() == "" ? "0" : data.Rows[0][39].ToString()).ToString())
            //    .Replace("@renglon6", data.Rows[0][40].ToString())
            //    .Replace("@renglon7", double.Parse(data.Rows[0][41].ToString() == "" ? "0" : data.Rows[0][41].ToString()).ToString("C2"))
            //    .Replace("@renglon8_1", data.Rows[0][42].ToString())
            //    .Replace("@renglon8_2", double.Parse(data.Rows[0][43].ToString() == "" ? "0" : data.Rows[0][43].ToString()).ToString("C2"))
            //    .Replace("@renglon9_1", data.Rows[0][44].ToString())
            //    .Replace("@renglon9_2", double.Parse(data.Rows[0][45].ToString() == "" ? "0" : data.Rows[0][45].ToString()).ToString("C2"))
            //    .Replace("@renglon10", double.Parse(data.Rows[0][46].ToString() == "" ? "0" : data.Rows[0][46].ToString()).ToString("C2"))
            //    .Replace("@renglon11", double.Parse(data.Rows[0][47].ToString()).ToString("C2"))
            //    .Replace("@renglon12", double.Parse(data.Rows[0][48].ToString()).ToString("C2"))
            //    .Replace("@renglon13", double.Parse(data.Rows[0][49].ToString() == "" ? "0" : data.Rows[0][49].ToString()).ToString("C2"))
            //    .Replace("@renglon14_1", sancion)
            //    .Replace("@renglon14_2", double.Parse(data.Rows[0][51].ToString() == "" ? "0" : data.Rows[0][51].ToString()).ToString("C2"))
            //    .Replace("@renglon15", double.Parse(data.Rows[0][52].ToString()).ToString("C2"));
        }
        
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            FormularioReteICA _formReteICA = new FormularioReteICA();
            var anio = int.Parse(lblFechaMaximaAno.Text);
            var mes = int.Parse(lblFechaMaximaMes.Text);
            var dia = int.Parse(lblFechaMaximaDia.Text);

            /*
            foreach (var control in Controls)
            {
                var rb = control as RadioButton;
                if (rb != null && rb.GroupName == "cbperiodicidad" && rb.Checked)
                {
                    _formReteICA.Periodicidad = rb.Text;
                }
            }
            */

            if (rbMensual.Checked == true)
            {
                _formReteICA.Periodicidad = "MENSUAL";
            }
            if (rbBimestral.Checked == true)
            {
                _formReteICA.Periodicidad = "BIMESTRAL";
            }
            if (rbTrimestral.Checked == true)
            {
                _formReteICA.Periodicidad = "TRIMESTRAL";
            }

            if (rbCuatrimestral.Checked == true)
            {
                _formReteICA.Periodicidad = "CUATRIMESTRAL";
            }
            if (rbSemestral.Checked == true)
            {
                _formReteICA.Periodicidad = "SEMESTRAL";
            }
            if (rbAnual.Checked == true)
            {
                _formReteICA.Periodicidad = "ANUAL";
            }
            _formReteICA.Periodicidad = _formReteICA.Periodicidad + " - " + ddlPeriodoDeclarar.SelectedItem;
            _formReteICA.Id = string.IsNullOrEmpty(hdIdRegistro.Value) ? 0 : int.Parse(hdIdRegistro.Value);
            _formReteICA.AnioDeclarable = int.Parse(TxtAnioGravable.Text);
            _formReteICA.CodigoDane = TxtCodDane.Text;
            _formReteICA.FechaMaxima = new DateTime(anio, mes, dia);
            _formReteICA.IdCliente = int.Parse(CmbCliente.SelectedValue);
            _formReteICA.IdContador = int.Parse(ddlContador.SelectedValue);
            _formReteICA.nombre_firmante = ddlFirmante.SelectedItem.ToString();
            _formReteICA.nombre_contador = ddlContador.SelectedItem.ToString();
            _formReteICA.IdFirmador = int.Parse(ddlFirmante.SelectedValue);

            if(rbccFirma.Checked == true)
            {
                _formReteICA.tipo_iden_firmante = "CC";
            } else if (rbceFirma.Checked == true)
            {
                _formReteICA.tipo_iden_firmante = "CE";
            } else
            {
                _formReteICA.tipo_iden_firmante = "";
            }

            if (rbccConta.Checked == true)
            {
                _formReteICA.tipo_iden_contador = "CC";
            }
            else if (rbceConta.Checked == true)
            {
                _formReteICA.tipo_iden_contador = "CE";
            }
            else
            {
                _formReteICA.tipo_iden_contador = "";
            }

            _formReteICA.numero_iden_firmante = lblccfirmante.Text;
            _formReteICA.numero_iden_contador = lblccConta.Text;
            _formReteICA.tarjeta_profesional = lbltpConta.Text;


            _formReteICA.Periodicidad = _formReteICA.Periodicidad + " - " + ddlPeriodoDeclarar.SelectedItem;
            _formReteICA.IdMunicipio = int.Parse(hdIdMunicipio.Value);
            _formReteICA.IdPeriodo = int.Parse(ddlPeriodoDeclarar.SelectedValue);
            _formReteICA.NombreDepartamento = lblNombreDpto.Text.Trim();
            _formReteICA.NombreMunicipio = LblNombreMunicipio.Text.Trim();

            _formReteICA.nombre_razon_social = lblNombres.Text.Trim();

            //if (rblDocCliente[0])  AQUIIIIIII VOY

            _formReteICA.tipo_iden = "N";
            _formReteICA.numero_iden = lblNumDoc.Text.Trim();
            _formReteICA.digito_v = lblDV2.Text.Trim();
            _formReteICA.direccion_not = lblDireccion.Text.Trim();
            _formReteICA.depto_not = lblDptoCliente.Text.Trim();
            _formReteICA.ciu_not = lblMunicipioCliente.Text.Trim();
            _formReteICA.correoe_not = lblCorreo.Text.Trim();
            _formReteICA.telefono_not = lblTelFijo.Text.Trim();
            _formReteICA.celular_not = " ";
            _formReteICA.tipo_cont = LblTA.Text.Trim();
            _formReteICA.tipo_act = "FINANCIERA";
            _formReteICA.gran_contribuyente = "SI";


            _formReteICA.Renglon15 = lblRenglon15.Text;
            _formReteICA.Renglon16 = lblRenglon16.Text;
            _formReteICA.Renglon17 = lblRenglon17.Text;
            _formReteICA.Renglon18 = lblRenglon18.Text;
            _formReteICA.Renglon19 = lblRenglon19.Text;
            _formReteICA.Renglon20 = lblRenglon20.Text;
            _formReteICA.Renglon21 = lblRenglon21.Text;
            _formReteICA.Renglon22 = lblRenglon22.Text;
            _formReteICA.Renglon23 = lblRenglon23.Text;
            _formReteICA.Renglon24 = lblRenglon24.Text;
            _formReteICA.Renglon25 = lblTotalRetenciones.Text;
            _formReteICA.Renglon26 = lblRenglon26.Text; 
            _formReteICA.Renglon27 = lblRenglon27.Text; 
            _formReteICA.Renglon28 = lblRenglon28.Text; 
            _formReteICA.Renglon29 = lblRenglon29.Text; 
            _formReteICA.Renglon30 = lblRenglon30.Text; 
            _formReteICA.Renglon31 = lblRenglon31.Text; 
            _formReteICA.Renglon32 = lblRenglon32.Text; 
            _formReteICA.Renglon33 = lblRenglon33.Text; 
            _formReteICA.Renglon34 = lblRenglon34.Text;
            _formReteICA.Renglon35 = lblRenglon35.Text;
            _formReteICA.Renglon36 = lblTotalRetencionesPracticadas.Text;
            _formReteICA.Renglon37 = txtRenglon37.DisplayText;
            _formReteICA.Renglon38 = lblRenglon38.Text; 
            _formReteICA.Renglon39 = lblRenglon39.Text; 
            _formReteICA.Renglon40 = txtRenglon40.DisplayText;
            _formReteICA.Renglon41 = txtRenglon41.DisplayText;
            _formReteICA.Renglon42 = lblTotalPagos.Text;
           

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonRequest = js.Serialize(_dataICA);
            _log.Warn("REQUEST INSERT REG_IMPUESTO_RETEICA => " + jsonRequest);
            int idRegister = 0;
            var _MsgError = "";
            var rta = false;
            rta = _formReteICA.Id == 0 ? _dataICA.InsertData(_formReteICA, int.Parse(Session["IdUsuario"].ToString().Trim()), ref idRegister) : _dataICA.UpdateData(_formReteICA, int.Parse(Session["IdUsuario"].ToString().Trim()));
            if (rta)
            {
                _MsgError = "El formulario de Impuesto ha sido guardado con exito.";
                #region REGISTRO DE LOGS DE AUDITORIA
                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                ObjAuditoria.ModuloApp = "REG_IMPUESTO_RETEICA";
                ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                ObjAuditoria.DescripcionEvento = jsonRequest;
                ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                ObjAuditoria.TipoProceso = 1;
                BtnGuardar.Enabled = false;
                btnPdf.Enabled = true;
                BtnBorrar.Enabled = true;
                //'Agregar Auditoria del sistema
                string _MsgErrorLogs = "";
                if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                {
                    _log.Error(_MsgErrorLogs);
                }
                #endregion

                if (idRegister != 0)
                    hdIdRegistro.Value = idRegister.ToString();

                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "", "../../Imagenes/Iconos/16/check.png");
                _log.Info(_MsgError);
            }
            else
            {
                _MsgError = "Se ha presentado un error al guardar, por favor comuniquese con el administrador del sistema.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                _log.Error(_MsgError);
            }
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

        protected void BtnBorrar_Click(object sender, EventArgs e)
        {
            var registroId = string.IsNullOrEmpty(hdIdRegistro.Value) ? 0 : int.Parse(hdIdRegistro.Value);
            var rta = _dataICA.DeleteData(registroId);
            if (rta)
            {
                CleanForm();
                var _MsgError = "El formulario de Impuesto ha sido eliminado.";

                #region REGISTRO DE LOGS DE AUDITORIA
                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                ObjAuditoria.ModuloApp = "REG_IMPUESTO_RETEICA";
                ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                ObjAuditoria.DescripcionEvento = jsonRequest;
                ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                ObjAuditoria.TipoProceso = 1;
                BtnGuardar.Enabled = false;
                btnPdf.Enabled = false;
                BtnBorrar.Enabled = false;
                TxtCodDane.Text = "";
                TxtAnioGravable.Text = "";
                //'Agregar Auditoria del sistema
                string _MsgErrorLogs = "";
                if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                {
                    _log.Error(_MsgErrorLogs);
                }
                #endregion

                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "", "../../Imagenes/Iconos/16/check.png");
                _log.Info(_MsgError);
            }
            else
            {
                var _MsgError = "Se ha presentado un error al eliminar, por favor comuniquese con el administrador del sistema.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                _log.Error(_MsgError);
            }
        }

        protected void ddlPeriodoDeclarar_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdIdRegistro.Value = string.Empty;
            var dataddlSIC = _dataICA.GetData(TxtAnioGravable.Text, hdIdMunicipio.Value);
            if (dataddlSIC.Rows.Count > 0)
            {
                if (!GetDataSave(TxtAnioGravable.Text, hdIdMunicipio.Value, ddlPeriodoDeclarar.SelectedValue, int.Parse(CmbCliente.SelectedValue)))
                {
                    SetDatosCalculados(dataddlSIC.Rows[0]);
                }
                else
                {
                    BtnGuardar.Enabled = false;
                    BtnBorrar.Enabled = true;
                    btnPdf.Enabled = true;
                }
            }

            /*
            lblRenglon24.Text = $"{0:C2}";
            lblRenglon34.Text = $"{0:C2}";
            lblRenglon35.Text = $"{0:C2}";
          
            int anio = int.Parse(TxtAnioGravable.Text);
            int periodicidad = int.Parse(ddlPeriodoDeclarar.SelectedValue);

            DataTable dtReglonCuenta15_20;
            var results15_20 = _dataICA.GetFilas15_20(anio, periodicidad, TxtCodDane.Text, out dtReglonCuenta15_20);
            var dataRow = results15_20.Rows[0];

            double totalRenglon15 = (Math.Round(GetValor15_20(15, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon15.Text = $"{totalRenglon15:C2}";

            double totalRenglon16 = (Math.Round(GetValor15_20(16, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon16.Text = $"{totalRenglon16:C2}";

            double totalRenglon17 = (Math.Round(GetValor15_20(17, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon17.Text = $"{totalRenglon17:C2}";

            double totalRenglon18 = (Math.Round(GetValor15_20(18, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon18.Text = $"{totalRenglon18:C2}";

            double totalRenglon19 = (Math.Round(GetValor15_20(19, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon19.Text = $"{totalRenglon19:C2}";

            double totalRenglon20 = (Math.Round(GetValor15_20(20, dtReglonCuenta15_20, dataRow) / 1000) * 1000);
            lblRenglon20.Text = $"{totalRenglon20:C2}";

            DataTable dtReglonCuenta26_31;
            var results26_31 = _dataICA.GetFilas26_31(anio, periodicidad, TxtCodDane.Text, out dtReglonCuenta26_31);

            double totalRenglon26 = (Math.Round(GetValor26_31(26, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon26.Text = $"{totalRenglon26:C2}";
           
            double totalRenglon27 = (Math.Round(GetValor26_31(27, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon27.Text = $"{totalRenglon27:C2}";

            double totalRenglon28 = (Math.Round(GetValor26_31(28, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon28.Text = $"{totalRenglon28:C2}";

            double totalRenglon29 = (Math.Round(GetValor26_31(29, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon29.Text = $"{totalRenglon29:C2}";

            double totalRenglon30 = (Math.Round(GetValor26_31(30, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon30.Text = $"{totalRenglon30:C2}";

            double totalRenglon31 = (Math.Round(GetValor26_31(31, dtReglonCuenta26_31, results26_31) / 1000) * 1000);
            lblRenglon31.Text = $"{totalRenglon31:C2}";

            double renglon21_22_23 = totalRenglon26 + totalRenglon27 + totalRenglon28 + totalRenglon29 + totalRenglon30 + totalRenglon31;
            lblRenglon21.Text = lblRenglon22.Text = lblRenglon23.Text = $"{renglon21_22_23:C2}";

            int municipioId = int.Parse(hdIdMunicipio.Value);
            double totalRenglon32 = (Math.Round(GetValorTarifa(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdAvisosTableros"]), anio ) / 1000) * 1000);
            lblRenglon32.Text = $"{totalRenglon32:C2}";

            double totalRenglon33 = (Math.Round(GetValorTarifa(renglon21_22_23, municipioId, int.Parse(ConfigurationManager.AppSettings["IdSobretasaBomberil"]), anio) / 1000) * 1000);
            lblRenglon33.Text = $"{totalRenglon33:C2}";

            double totalRenglones26_35 = totalRenglon26 + totalRenglon27 + totalRenglon28 + totalRenglon29 + totalRenglon30 + totalRenglon31 + totalRenglon32 + totalRenglon33;
            lblTotalRetencionesPracticadas.Text = $"{totalRenglones26_35:C2}";
            renglon36 = totalRenglones26_35;

            lblRenglon39.Text = $"{totalRenglones26_35:C2}";
            lblTotalPagos.Text = $"{totalRenglones26_35:C2}";

            double totalRenglones15_20 = totalRenglon15 + totalRenglon16 + totalRenglon17 + totalRenglon18 + totalRenglon19 + totalRenglon20 + (renglon21_22_23 * 3);
            lblTotalRetenciones.Text = $"{totalRenglones15_20:C2}";
            BtnBorrar.Enabled = false;
            */
        }

        private double GetValor15_20(int renglon, DataTable dtReglonCuenta, DataRow drResults)
        {
            List<int> columns = new List<int>();
            foreach (DataRow row in dtReglonCuenta.Rows)
            {
                int idRow = 0;
                int renglonRow = int.Parse($"{row["renglon"]}");
                if (renglonRow == renglon)
                {
                    idRow = int.Parse($"{row["id"]}");
                    columns.Add(idRow - 1);
                }
            }

            return columns.Count < 1 ? 0D : columns.Sum(column => double.Parse($"{drResults[column]}"));
        }

        private double GetValor26_31(int renglon, DataTable dtReglonCuenta, DataTable dtResults)
        {
            List<double> values = new List<double>();
            foreach (DataRow row in dtReglonCuenta.Rows)
            {
                int renglonRow = int.Parse($"{row["renglon"]}");
                if (renglonRow == renglon)
                {
                    var cuenta = $"{row["cuenta"]}";
                    foreach (DataRow item in dtResults.Rows)
                    {
                        var cuentaResult = $"{item["cuenta"]}";
                        if (cuenta == cuentaResult)
                        {
                            var datoArchivo = int.Parse($"{row["datoarchivo"]}");
                            var debito = double.Parse($"{item["debito"]}");
                            var credito = double.Parse($"{item["credito"]}") * -1;

                            switch (datoArchivo)
                            {
                                case 2: // debito
                                    values.Add(debito);
                                    break;

                                case 3: // credito
                                    values.Add(credito);
                                    break;

                                case 5: // debito menos credito
                                    values.Add(debito - credito);
                                    break;

                                case 6: //credito menos debito
                                    values.Add(credito - debito);
                                    break;
                            }
                        }
                    }
                }
            }

            return values.Count < 1 ? 0D : values.Sum();
        }

        private double GetValorTarifa(double valorRenglon, int municipioId, int configuracionId, int anio)
        {
            var dr_tarifa = _dataICA.GetTipoTarifa(municipioId, configuracionId, anio);

            if (dr_tarifa == null)
            {
                return 0D;
            }

            var valorTarifa = double.Parse($"{dr_tarifa["valor_tarifa"]}");
            int _IdTipoTarifa = Int32.Parse($"{dr_tarifa["idtipo_tarifa"].ToString().Trim()}");
            var descripcionTarifa = $"{dr_tarifa["descripcion_tarifa"]}".TrimEnd();

            switch (_IdTipoTarifa)
            {
                case 1:
                    var porcentaje = valorTarifa / 100;
                    return (valorRenglon * porcentaje);

                case 2:
                    var milaje = valorTarifa / 100;
                    return valorRenglon * milaje;

                case 8:
                    return valorRenglon * valorTarifa;

                //case "PORCENTUAL":
                //    var porcentaje = valorTarifa / 100;
                //    return (valorRenglon * porcentaje);

                //case "POR MIL":
                //    var milaje = valorTarifa / 100;
                //    return valorRenglon * milaje;
                default:
                    return 0D;
            }
        }

        private double GetOtrasConfiguraciones(double valorRenglon, int municipioId, int formimpuestoId, int anio)
        {
            var dr_tarifa = _dataICA.GetOtrasConfiguraciones(municipioId, formimpuestoId, anio);

            if (dr_tarifa == null)
            {
                return 0D;
            }

            int _IdTipoTarifa = Int32.Parse($"{dr_tarifa["idtipo_tarifa"].ToString().Trim()}");
            var _CantidadMedida = double.Parse($"{dr_tarifa["cantidad_medida"]}");
            var _CantidadPeriodos = double.Parse($"{dr_tarifa["cantidad_periodos"]}");
            var _ValorUnidad = double.Parse($"{dr_tarifa["valor_unidad"]}");
            double _ValorRenglon = 0;

            switch (_IdTipoTarifa)
            {
                case 1:
                    _ValorRenglon = ((_ValorUnidad * _CantidadMedida) * _CantidadPeriodos);
                    return (_ValorRenglon / 100);

                case 2:
                    _ValorRenglon = ((_ValorUnidad * _CantidadMedida) * _CantidadPeriodos);
                    return (_ValorRenglon / 1000);

                case 8:
                    return ((_ValorUnidad * _CantidadMedida) * _CantidadPeriodos);

                //case "PORCENTUAL":
                //    var porcentaje = valorTarifa / 100;
                //    return (valorRenglon * porcentaje);

                //case "POR MIL":
                //    var milaje = valorTarifa / 100;
                //    return valorRenglon * milaje;
                default:
                    return 0D;
            }
        }

        protected void txtRenglon37_TextChanged(object sender, EventArgs e)
        {
            renglon39 = GetValor39();
            lblRenglon39.Text = $"{renglon39:N0}";
            lblTotalPagos.Text = $"{GetValor42():N0}";
        }

        protected void txtRenglon41_TextChanged(object sender, EventArgs e)
        {
            renglon39 = GetValor39();
            lblRenglon39.Text = $"{renglon39:N0}";
            lblTotalPagos.Text = $"{GetValor42():N0}";
        }

        protected void txtRenglon40_TextChanged(object sender, EventArgs e)
        {
            renglon39 = GetValor39();
            lblRenglon39.Text = $"{renglon39:N0}";
            lblTotalPagos.Text = $"{GetValor42():N0}";
        }

        protected void Unnamed_CheckedChanged(object sender, EventArgs e)
        {
            lblRenglon38.Enabled = rbCorreccion.Checked;

            if (!rbCorreccion.Checked)
            {
                lblRenglon38.Text = string.Empty;
                //lblRenglon38_TextChanged(null, EventArgs.Empty);
            }
        }

        private double GetValor42()
        {
            var valorRenglon41 = string.IsNullOrEmpty(txtRenglon41.Text) ? 0D : double.Parse(txtRenglon41.Text);
            var valorRenglon40 = string.IsNullOrEmpty(txtRenglon40.Text) ? 0D : double.Parse(txtRenglon40.Text);
            return renglon39 + valorRenglon40 + valorRenglon41;
        }

        private double GetValor39()
        {
            double valorRenglon37 = string.IsNullOrEmpty(txtRenglon37.Text) ? 0D : double.Parse(txtRenglon37.Text);
            //double valorRenglon38 = string.IsNullOrEmpty(txtRenglon38.Text) ? 0D : double.Parse(txtRenglon38.Text);
            return renglon36 - valorRenglon37; //- valorRenglon38;
        }
    }
}