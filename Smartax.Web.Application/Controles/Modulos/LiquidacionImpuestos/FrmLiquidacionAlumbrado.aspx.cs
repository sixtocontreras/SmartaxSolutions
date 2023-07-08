using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Parametros.Alumbrado;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmLiquidacionAlumbrado : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Cliente ObjCliente = new Cliente();
        AdminAlumbrado _dataAlumbrado = new AdminAlumbrado();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
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


            this.CmbCliente.DataSource = ObjCliente.GetClientes();
            this.CmbCliente.DataValueField = "id_cliente";
            this.CmbCliente.DataTextField = "nombre_cliente";
            this.CmbCliente.DataBind();
        }

        private void CleanForm()
        {
            rblSector.ClearSelection();
            lblClasificacion.Text = "";
            lblRenglon2.Text = "";
            lblRenglon3.Text = "";
            lblBaseGravable.Text = "";
            lblTarifaImpuesto.Text = "";
            lblImpuesto.Text = "";
            hddescuentoporcentaje.Value = "";
            lblBaseGravableEspecial.Text = "";
            lblImpuestoEspecial.Text = "";
            lblUVTMin.Text = "";
            lblUVTMax.Text = "";
            lblPagoMinimo.Text = "";
            lblPagoMax.Text = "";
            lblTotalImp.Text = "";
            lblDescuento.Text = "";
            lblTotal.Text = "";
            txtMora.Text = "";
            txtSanciones.Text = "";
            lblTituloPeriodicidad.Text = "";
            lblAnio.Text = "";
            lblMes.Text = "";
            lblDia.Text = "";
            hdfechaMax.Value = "";
            ddlPeriodicidad.Visible = false;
            LblNombreMunicipio.Text = "";
            LblNombreDpto.Text = "";
            hdIdMunicipio.Value = "";
            lblUVT.Text = "";
            hdUVT.Value = "";
            TxtCodDane.Text = "";
            TxtAnioGravable.Text = "";
            hdIdRegistro.Value = "";
            BtnGuardar.Enabled = false;
            BtnBorrar.Enabled = false;
            btnPdf.Enabled = false;
        }
        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            var data = _dataAlumbrado.GetCliente(int.Parse(CmbCliente.SelectedValue));
            rblDocCliente.SelectedValue = data.Rows[0][0].ToString();
            lblNombres.Text = data.Rows[0][3].ToString();
            lblNumDoc.Text = data.Rows[0][1].ToString();
            lblDV2.Text = data.Rows[0][2].ToString();
            lblDireccion.Text = data.Rows[0][4].ToString();
            lblDptoCliente.Text = data.Rows[0][5].ToString();
            lblMunicipioCliente.Text = data.Rows[0][6].ToString();
            lblCorreo.Text = data.Rows[0][7].ToString();
            lblTelFijo.Text = data.Rows[0][8].ToString();


            var firmante = _dataAlumbrado.GetFirmantes(false, CmbCliente.SelectedValue);
            var contador = _dataAlumbrado.GetFirmantes(true, CmbCliente.SelectedValue);
            Session["firmantes"] = firmante;
            Session["Contadores"] = contador;
            this.ddlFirmante.DataSource = firmante;
            this.ddlFirmante.DataValueField = "id_firmante";
            this.ddlFirmante.DataTextField = "nombre";
            this.ddlFirmante.DataBind();

            this.ddlContador.DataSource = contador;
            this.ddlContador.DataValueField = "id_firmante";
            this.ddlContador.DataTextField = "nombre";
            this.ddlContador.DataBind();
            setFirmas();
            pnlSelectCliente.Visible = false;
            PnlDatos.Visible = true;

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
            var data = _dataAlumbrado.GetUbicacion(TxtCodDane.Text);
            if (data.Rows.Count > 0)
            {
                LblNombreMunicipio.Text = data.Rows[0][0].ToString();
                LblNombreDpto.Text = data.Rows[0][1].ToString();
                hdIdMunicipio.Value = data.Rows[0][2].ToString();
                if (TxtAnioGravable.Text != string.Empty)
                {
                    GetData();
                }
            }
            else
            {
                LblNombreMunicipio.Text = "";
                LblNombreDpto.Text = "";
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
            var data = _dataAlumbrado.GetUVT(TxtAnioGravable.Text);
            if (data.Rows.Count > 0)
            {
                lblUVT.Text = double.Parse(data.Rows[0][0].ToString()).ToString("C0");
                hdUVT.Value = data.Rows[0][0].ToString();
                if (TxtCodDane.Text != string.Empty)
                {
                    GetData();
                }
            }
            else
            {
                lblUVT.Text = "";
                hdUVT.Value = "";
                BtnGuardar.Enabled = false;
                if (TxtAnioGravable.Text != string.Empty)
                {
                    GetData();
                }
            }
        }
        private void GetData()
        {
            if (string.IsNullOrEmpty(TxtAnioGravable.Text) || string.IsNullOrEmpty(hdIdMunicipio.Value))
            {
                lblTituloPeriodicidad.Text = "";
                lblAnio.Text = "";
                lblMes.Text = "";
                lblDia.Text = "";
                hdfechaMax.Value = "";
                ddlPeriodicidad.Visible = false;
                rblSector.ClearSelection();
                lblClasificacion.Text = "";
                lblRenglon2.Text = "";
                lblRenglon3.Text = "";
                lblBaseGravable.Text = "";
                lblTarifaImpuesto.Text = "";
                lblBaseGravableEspecial.Text = "";
                lblImpuestoEspecial.Text = "";
                lblImpuesto.Text = "";
                lblUVTMin.Text = "";
                lblUVTMax.Text = "";
                lblPagoMinimo.Text = "";
                lblPagoMax.Text = "";
                lblTotalImp.Text = "";
                lblDescuento.Text = "";
                lblTotal.Text = "";
                BtnGuardar.Enabled = false;
            }
            else
            {
                var data = _dataAlumbrado.GetData(TxtAnioGravable.Text, hdIdMunicipio.Value);
                if (data.Rows.Count > 0)
                {
                    lblTituloPeriodicidad.Text = "";
                    lblAnio.Text = "";
                    lblMes.Text = "";
                    lblDia.Text = "";
                    hdfechaMax.Value = "";
                    ddlPeriodicidad.Visible = false;
                    rblSector.ClearSelection();
                    lblClasificacion.Text = "";
                    lblRenglon2.Text = "";
                    lblRenglon3.Text = "";
                    lblBaseGravable.Text = "";
                    lblBaseGravableEspecial.Text = "";
                    lblImpuestoEspecial.Text = "";
                    lblTarifaImpuesto.Text = "";
                    lblImpuesto.Text = "";
                    lblUVTMin.Text = "";
                    lblUVTMax.Text = "";
                    lblPagoMinimo.Text = "";
                    lblPagoMax.Text = "";
                    lblTotalImp.Text = "";
                    lblDescuento.Text = "";
                    lblTotal.Text = "";

                    lblTituloPeriodicidad.Text = data.Rows[0][1].ToString();
                    var date = DateTime.Parse(data.Rows[0][4].ToString());
                    lblAnio.Text = date.Year.ToString();
                    lblMes.Text = date.Month.ToString();
                    lblDia.Text = date.Day.ToString();
                    hdfechaMax.Value = date.ToString("dd-MM-yyyy");
                    Session["dataPeriodicidad"] = data;
                    ddlPeriodicidad.DataSource = data;
                    ddlPeriodicidad.DataTextField = "periodicidad_impuesto";
                    ddlPeriodicidad.DataValueField = "idperiodicidad_impuesto";
                    ddlPeriodicidad.DataBind();
                    ddlPeriodicidad.Visible = true;
                    if (!GetDataSave(TxtAnioGravable.Text, hdIdMunicipio.Value, ddlPeriodicidad.SelectedValue, Int32.Parse(CmbCliente.SelectedValue)))
                    {
                        SetDatosCalculados(data.Rows[0]);
                        var days = (DateTime.Now.Date - date.Date).TotalDays;
                        if (days > 0)
                            RadWindowManager1.RadAlert($"La declaración tiene {days} días de vencida", 400, 200, "Alerta", "", "../../Imagenes/Iconos/16/img_info.png");
                    }
                }
                else
                {
                    lblTituloPeriodicidad.Text = "";
                    lblAnio.Text = "";
                    lblMes.Text = "";
                    lblDia.Text = "";
                    hdfechaMax.Value = "";
                    ddlPeriodicidad.Visible = false;
                    rblSector.ClearSelection();
                    lblClasificacion.Text = "";
                    lblRenglon2.Text = "";
                    lblRenglon3.Text = "";
                    lblBaseGravable.Text = "";
                    lblBaseGravableEspecial.Text = "";
                    lblImpuestoEspecial.Text = "";
                    lblTarifaImpuesto.Text = "";
                    lblImpuesto.Text = "";
                    lblUVTMin.Text = "";
                    lblUVTMax.Text = "";
                    lblPagoMinimo.Text = "";
                    lblPagoMax.Text = "";
                    lblTotalImp.Text = "";
                    lblDescuento.Text = "";
                    lblTotal.Text = "";
                    BtnGuardar.Enabled = false;
                }
            }
        }

        private bool GetDataSave(string anio, string municipio, string periodicidad, int cliente)
        {
            var data = _dataAlumbrado.GetData(anio, municipio, periodicidad, cliente);
            var rta = false;
            if (data.Rows.Count > 0)
            {
                rta = true;
                hdUVT.Value = data.Rows[0][7].ToString();
                lblUVT.Text = double.Parse(data.Rows[0][7].ToString()).ToString("C0");
                lblAnio.Text = DateTime.Parse(data.Rows[0][8].ToString()).Year.ToString();
                lblMes.Text = DateTime.Parse(data.Rows[0][8].ToString()).Month.ToString();
                lblDia.Text = DateTime.Parse(data.Rows[0][8].ToString()).Day.ToString();
                hdfechaMax.Value = DateTime.Parse(data.Rows[0][8].ToString()).ToString("dd-MM-yyyy");
                rblDocCliente.SelectedValue = data.Rows[0][11].ToString();
                lblNombres.Text = data.Rows[0][10].ToString();
                lblNumDoc.Text = data.Rows[0][12].ToString();
                lblDV2.Text = data.Rows[0][13].ToString();
                lblDireccion.Text = data.Rows[0][14].ToString();
                lblMunicipioCliente.Text = data.Rows[0][15].ToString();
                lblDptoCliente.Text = data.Rows[0][16].ToString();
                lblCorreo.Text = data.Rows[0][17].ToString();
                lblTelFijo.Text = data.Rows[0][18].ToString();
                rblSector.SelectedValue = data.Rows[0][20].ToString();
                var enable = rblSector.SelectedValue != "5";
                lblRenglon2.Enabled = enable;
                lblRenglon3.Enabled = enable;

                lblClasificacion.Text = data.Rows[0][21].ToString();
                lblRenglon2.Text = data.Rows[0][22].ToString();
                lblRenglon3.Text = data.Rows[0][23].ToString();
                if (!string.IsNullOrEmpty(data.Rows[0][24].ToString()))
                    lblBaseGravable.Text = double.Parse(data.Rows[0][24].ToString()).ToString("C0");
                if (!string.IsNullOrEmpty(data.Rows[0][25].ToString()))
                    lblBaseGravableEspecial.Text = double.Parse(data.Rows[0][25].ToString()).ToString();
                lblTarifaImpuesto.Text = data.Rows[0][26].ToString();
                var impu = double.Parse(data.Rows[0][27].ToString() == "" ? "0" : data.Rows[0][27].ToString());
                lblUVTMin.Text = data.Rows[0][28].ToString();
                var pagomin = double.Parse(data.Rows[0][29].ToString() == "" ? "0" : data.Rows[0][29].ToString());
                lblUVTMax.Text = data.Rows[0][30].ToString();
                var pagomax = double.Parse(data.Rows[0][31].ToString() == "" ? "0" : data.Rows[0][31].ToString());
                if (!string.IsNullOrEmpty(data.Rows[0][32].ToString()))
                    lblImpuestoEspecial.Text = double.Parse(data.Rows[0][32].ToString()).ToString("C0");
                lblImpuesto.Text = impu.ToString("C0");
                lblPagoMinimo.Text = pagomin.ToString("C0");
                lblPagoMax.Text = pagomax.ToString("C0");
                var totalImp = double.Parse(data.Rows[0][33].ToString() == "" ? "0" : data.Rows[0][33].ToString());
                lblTotalImp.Text = totalImp.ToString("C0");
                hddescuentoporcentaje.Value = data.Rows[0][46].ToString();
                lblDescuento.Text = double.Parse(data.Rows[0][34].ToString() == "" ? "0" : data.Rows[0][34].ToString()).ToString("C0");
                txtMora.Text = (string.IsNullOrEmpty(data.Rows[0][35].ToString()) ? "0" : (data.Rows[0][35].ToString()=="1"?"0": data.Rows[0][35].ToString()));
                rblSanciones.SelectedValue = data.Rows[0][36].ToString();
                txtSanciones.Text = (string.IsNullOrEmpty(data.Rows[0][37].ToString()) ? "0" : (data.Rows[0][37].ToString()=="1"?"0": data.Rows[0][37].ToString()));
                lblTotal.Text = double.Parse(data.Rows[0][38].ToString() == "" ? "0" : data.Rows[0][38].ToString()).ToString("C0");
                ddlFirmante.SelectedValue = data.Rows[0][39].ToString();
                ddlContador.SelectedValue = data.Rows[0][40].ToString();
                hdIdRegistro.Value = data.Rows[0][0].ToString();
                BtnGuardar.Enabled = true;
                setFirmas();
                btnPdf.Enabled = true;
                BtnBorrar.Enabled = true;
            }
            return rta;
        }
        private void SetDatosCalculados(DataRow row)
        {
            var sect = _dataAlumbrado.GetSector(hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
            var especial = false;
            if (sect.Rows.Count > 0)
            {
                especial = sect.Rows[0][0].ToString() == "5";
            }
            if (especial)
            {
                var data = _dataAlumbrado.GetDatosCalculados(hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
                if (data.Rows.Count > 0)
                {
                    if (data.Rows[0][2].ToString() != "4")
                     {
                        hdIdRegistro.Value = "";
                        txtMora.Text = "";
                        txtSanciones.Text = "";
                        rblSector.SelectedValue = "5";
                        lblRenglon2.Enabled = false;
                        lblRenglon3.Enabled = false;
                        lblClasificacion.Text = data.Rows[0][0].ToString();
                        lblBaseGravableEspecial.Text = double.Parse(data.Rows[0][1].ToString()).ToString();
                        var totalImp = Math.Round((double.Parse(data.Rows[0][4].ToString())) / 1000) * 1000;
                        if (double.Parse(data.Rows[0][2].ToString()).ToString() == "4")
                        {
                            totalImp = Math.Round((double.Parse(data.Rows[0][1].ToString())) / 1000) * 1000;
                        }
                        lblImpuestoEspecial.Text = totalImp.ToString("C0");
                        lblTotalImp.Text = lblImpuestoEspecial.Text;
                        var days = (DateTime.Parse(row[4].ToString()).Date - DateTime.Now.Date).TotalDays;
                        var descuento = (days >= 0 ? double.Parse(row[5].ToString()) : 0);
                        hddescuentoporcentaje.Value = descuento.ToString();
                        var descuentofinal = ((totalImp * descuento) / 100);
                        lblDescuento.Text = (Math.Round(descuentofinal / 1000) * 1000).ToString("C0");
                        var total = (Math.Round((totalImp - descuentofinal + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text)) / 1000) * 1000);
                        lblTotal.Text = total.ToString("C0");
                        BtnGuardar.Enabled = true;
                        BtnBorrar.Enabled = false;
                        btnPdf.Enabled = false;
                    }
                    else
                    {
                        var data_esp = _dataAlumbrado.GetDatosCalculados(double.Parse(hdUVT.Value), ddlPeriodicidad.SelectedValue, hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
                        if (data_esp.Rows.Count > 0)
                        {
                            rblSector.SelectedValue = "5";
                            var tot_consumo = Math.Round((double.Parse(data.Rows[0][1].ToString()) * double.Parse(data_esp.Rows[0][3].ToString())) / 1000) * 1000;
                            lblClasificacion.Text = data.Rows[0][0].ToString();
                            lblRenglon3.Text = data.Rows[0][1].ToString();
                            lblRenglon2.Text = data_esp.Rows[0][3].ToString();
                            lblBaseGravable.Text = tot_consumo.ToString("C0");
                            
                            lblTarifaImpuesto.Text = "1";
                            lblImpuesto.Text = lblBaseGravable.Text;
                            lblTotalImp.Text = lblBaseGravable.Text;
                            var totalImp = tot_consumo;

                            var days = (DateTime.Parse(row[4].ToString()).Date - DateTime.Now.Date).TotalDays;
                            var descuento = (days >= 0 ? double.Parse(row[5].ToString()) : 0);
                            hddescuentoporcentaje.Value = descuento.ToString();
                            var descuentofinal = ((totalImp * descuento) / 100);
                            lblDescuento.Text = (Math.Round(descuentofinal / 1000) * 1000).ToString("C0");
                            var total = (Math.Round((totalImp - descuentofinal + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text)) / 1000) * 1000);
                            lblTotal.Text = total.ToString("C0");


                            lblTotal.Text = lblBaseGravable.Text;
                            BtnGuardar.Enabled = true;
                            BtnBorrar.Enabled = false;
                            btnPdf.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                var data = _dataAlumbrado.GetDatosCalculados(double.Parse(hdUVT.Value), ddlPeriodicidad.SelectedValue, hdIdMunicipio.Value, TxtAnioGravable.Text, CmbCliente.SelectedValue);
                if (data.Rows.Count > 0)
                {
                    hdIdRegistro.Value = "";
                    txtMora.Text = "";
                    txtSanciones.Text = "";
                    lblRenglon2.Enabled = true;
                    lblRenglon3.Enabled = true;
                    rblSector.SelectedValue = data.Rows[0][1].ToString();
                    lblClasificacion.Text = data.Rows[0][7].ToString();
                    lblRenglon2.Text = data.Rows[0][3].ToString();
                    lblRenglon3.Text = data.Rows[0][4].ToString();
                    lblBaseGravable.Text = double.Parse(data.Rows[0][5].ToString()).ToString("C0");
                    lblTarifaImpuesto.Text = $"{data.Rows[0][8].ToString()}%";
                    var impu = double.Parse(data.Rows[0][11].ToString());
                    var pagomin = double.Parse(data.Rows[0][12].ToString());
                    var pagomax = double.Parse(data.Rows[0][13].ToString());
                    lblImpuesto.Text = impu.ToString("C0");
                    lblUVTMin.Text = data.Rows[0][9].ToString();
                    lblUVTMax.Text = data.Rows[0][10].ToString();
                    lblPagoMinimo.Text = pagomin.ToString("C0");
                    lblPagoMax.Text = pagomax.ToString("C0");
                    var totalImp = (impu < pagomin ? pagomin : (impu > pagomax ? pagomax : impu));
                    lblTotalImp.Text = totalImp.ToString("C0");
                    var days = (DateTime.Parse(row[4].ToString()).Date - DateTime.Now.Date).TotalDays;
                    var descuento = (days >= 0 ? double.Parse(row[5].ToString()) : 0);
                    hddescuentoporcentaje.Value = descuento.ToString();
                    var descuentofinal = ((totalImp * descuento) / 100);
                    lblDescuento.Text = (Math.Round(descuentofinal / 1000) * 1000).ToString("C0");
                    var total = (Math.Round((totalImp - descuentofinal + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text)) / 1000) * 1000);
                    lblTotal.Text = total.ToString("C0");
                    BtnGuardar.Enabled = true;
                    BtnBorrar.Enabled = false;
                    btnPdf.Enabled = false;
                }

                else
                {
                    rblSector.ClearSelection();
                    lblClasificacion.Text = "";
                    lblRenglon2.Text = "";
                    lblRenglon3.Text = "";
                    lblBaseGravable.Text = "";
                    lblTarifaImpuesto.Text = "";
                    lblImpuesto.Text = "";
                    hddescuentoporcentaje.Value = "";
                    lblBaseGravableEspecial.Text = "";
                    lblImpuestoEspecial.Text = "";
                    lblUVTMin.Text = "";
                    lblUVTMax.Text = "";
                    lblPagoMinimo.Text = "";
                    lblPagoMax.Text = "";
                    lblTotalImp.Text = "";
                    lblDescuento.Text = "";
                    lblTotal.Text = "";
                    txtMora.Text = "";
                    txtSanciones.Text = "";
                    BtnGuardar.Enabled = false;
                    BtnBorrar.Enabled = false;
                    btnPdf.Enabled = false;
                }
            }

        }


        protected void ddlPeriodicidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = (DataTable)Session["dataPeriodicidad"];
            foreach (DataRow item in data.Rows)
            {
                if (item[2].ToString() == ddlPeriodicidad.SelectedValue)
                {
                    var date = DateTime.Parse(item[4].ToString());
                    lblAnio.Text = date.Year.ToString();
                    lblMes.Text = date.Month.ToString();
                    lblDia.Text = date.Day.ToString();
                    hdfechaMax.Value = date.ToString("dd-MM-yyyy");
                    if (!GetDataSave(TxtAnioGravable.Text, hdIdMunicipio.Value, ddlPeriodicidad.SelectedValue, Int32.Parse(CmbCliente.SelectedValue)))
                    {
                        SetDatosCalculados(item);
                        var days = (DateTime.Now.Date - date.Date).TotalDays;
                        if (days>0)
                            RadWindowManager1.RadAlert($"La declaración tiene {days} días de vencida", 400, 200, "Alerta", "", "../../Imagenes/Iconos/16/img_info.png");

                    }
                }
            }
        }
        

        protected void txtMora_TextChanged(object sender, EventArgs e)
        {
            lblTotal.Text = (Math.Round(((double.Parse(lblTotalImp.Text.Replace("$", "").Replace(".", "")) - double.Parse(lblDescuento.Text.Replace("$", "").Replace(".", "")) + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text))) / 1000) * 1000).ToString("C0");

        }

        protected void txtSanciones_TextChanged(object sender, EventArgs e)
        {
            lblTotal.Text = (Math.Round(((double.Parse(lblTotalImp.Text.Replace("$", "").Replace(".", "")) - double.Parse(lblDescuento.Text.Replace("$", "").Replace(".", "")) + double.Parse(txtMora.Text == string.Empty ? "0" : txtMora.Text) + double.Parse(txtSanciones.Text == string.Empty ? "0" : txtSanciones.Text))) / 1000) * 1000).ToString("C0");

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
            var hash = GenerarPdfAlumbrado(Server.MapPath("../../../Archivos/PDF/Alumbrado.html"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "<script language='javascript'> window.open('" + ConfigurationManager.AppSettings["UrlBase"] + "/Archivos/PDF/pdfTemp" + hash + ".pdf', 'window','HEIGHT=600,WIDTH=820,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes');</script>", false);
            Thread thread1 = new Thread(() => DoWork(Server.MapPath("../../../Archivos/PDF"), hash));
            thread1.Start();
        }
        public string GenerarPdfAlumbrado(string url)
        {
            Byte[] bytes;

            var hash = DateTime.Now.ToString("ddMMyyyhhmm");
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        var example_html = File.ReadAllText(Server.MapPath("../../../Archivos/PDF/Alumbrado.html"));
                        setDataPdf(hash, ref example_html);
                        var example_css = File.ReadAllText(Server.MapPath("../../../Archivos/PDF/Alumbrado.css"));
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
            System.IO.File.WriteAllBytes(file, bytes);
            return hash;


        }
        private void setDataPdf(string hash, ref string pdf)
        {
            var data = _dataAlumbrado.GetDataPdf(int.Parse(hdIdRegistro.Value));
            var sancion = string.Empty;

            switch (data.Rows[0][50].ToString())
            {
                case "1":
                    sancion = "Extemporaneidad";
                    break;
                case "2":
                    sancion = "Corrección";
                    break;
                case "3":
                    sancion = "Inexactitud";
                    break;
                case "4":
                    sancion = "Emplazamiento";
                    break;
                case "5":
                    sancion = "Otra";
                    break;
            }
            var f1 = false;
            var c1 = false;
            var firmante = Server.MapPath($"../../../Archivos/PDF/firmante{hash}.png");
            var contador = Server.MapPath($"../../../Archivos/PDF/contador{hash}.png");
            if (data.Rows[0][7].ToString() != "")
            {
                System.IO.File.WriteAllBytes(firmante, ((byte[])data.Rows[0][7]));
                f1 = true;
            }
            if (data.Rows[0][13].ToString() != "")
            {
                System.IO.File.WriteAllBytes(contador, ((byte[])data.Rows[0][13]));
                c1 = true;
            }
            pdf = pdf.Replace("@periodicidad", data.Rows[0][0].ToString())
                .Replace("@dataperiodicidad", data.Rows[0][1].ToString())
                .Replace("@tipodoc_cli", data.Rows[0][2].ToString())
                .Replace("@sector", data.Rows[0][3].ToString())
                .Replace("@tipodoc_firmante", data.Rows[0][4].ToString())
                .Replace("@doc_firmante", data.Rows[0][5].ToString())
                .Replace("@nombreFirmante", data.Rows[0][6].ToString())
                .Replace("@declaracioncorrige", "")
                .Replace("@imgFirmante", (f1 ? $"<img style='height:80px' src='{ConfigurationManager.AppSettings["UrlBase"]}/{$"Archivos/PDF/firmante{hash}.png"}' />" : ""))
                .Replace("@tipodoc_contador", data.Rows[0][8].ToString())
                .Replace("@doc_contador", data.Rows[0][9].ToString())
                .Replace("@contadorTitulo", data.Rows[0][10].ToString())
                .Replace("@tp_contador", data.Rows[0][11].ToString())
                .Replace("@nombreContador", data.Rows[0][12].ToString())
                .Replace("@imgContador", (c1 ? $"<img style='height:80px' src='{ConfigurationManager.AppSettings["UrlBase"]}/{$"Archivos/PDF/contador{hash}.png"}' />" : ""))
                //.Replace("@imgContador", Convert.ToBase64String(((byte[])data.Rows[0][13])))
                .Replace("@cod_dane", data.Rows[0][16].ToString())
                .Replace("@municipio", data.Rows[0][17].ToString())
                .Replace("@dpto_cli", data.Rows[0][30].ToString())
                .Replace("@dpto", data.Rows[0][19].ToString())
                .Replace("@anio", data.Rows[0][20].ToString())
                .Replace("@uvt", data.Rows[0][21].ToString())
                .Replace("@dd", DateTime.Parse(data.Rows[0][22].ToString()).Day.ToString())
                .Replace("@mm", DateTime.Parse(data.Rows[0][22].ToString()).Month.ToString())
                .Replace("@aaaa", DateTime.Parse(data.Rows[0][22].ToString()).Year.ToString())
                .Replace("@nombre_cli", data.Rows[0][24].ToString())
                .Replace("@numdoc_cli", data.Rows[0][26].ToString())
                .Replace("@dv_cli", data.Rows[0][27].ToString())
                .Replace("@dir_cli", data.Rows[0][28].ToString())
                .Replace("@muni_cli", data.Rows[0][29].ToString())
                .Replace("@mail_cli", data.Rows[0][31].ToString())
                .Replace("@tel_cli", data.Rows[0][32].ToString())
                .Replace("@cel_cli", data.Rows[0][33].ToString())
                .Replace("@renglon_1", data.Rows[0][35].ToString())
                .Replace("@renglon2", data.Rows[0][36].ToString())
                .Replace("@renglon3", data.Rows[0][37].ToString())
                .Replace("@renglon4", double.Parse(data.Rows[0][38].ToString() == "" ? "0" : data.Rows[0][38].ToString()).ToString("C0"))
                .Replace("@renglon5", double.Parse(data.Rows[0][39].ToString() == "" ? "0" : data.Rows[0][39].ToString()).ToString())
                .Replace("@renglon6", data.Rows[0][40].ToString())
                .Replace("@renglon7", double.Parse(data.Rows[0][41].ToString() == "" ? "0" : data.Rows[0][41].ToString()).ToString("C0"))
                .Replace("@renglon8_1", data.Rows[0][42].ToString())
                .Replace("@renglon8_2", double.Parse(data.Rows[0][43].ToString() == "" ? "0" : data.Rows[0][43].ToString()).ToString("C0"))
                .Replace("@renglon9_1", data.Rows[0][44].ToString())
                .Replace("@renglon9_2", double.Parse(data.Rows[0][45].ToString() == "" ? "0" : data.Rows[0][45].ToString()).ToString("C0"))
                .Replace("@renglon10", double.Parse(data.Rows[0][46].ToString() == "" ? "0" : data.Rows[0][46].ToString()).ToString("C0"))
                .Replace("@renglon11", double.Parse(data.Rows[0][47].ToString()).ToString("C0"))
                .Replace("@renglon12", double.Parse(data.Rows[0][48].ToString()).ToString("C0"))
                .Replace("@renglon13", double.Parse(data.Rows[0][49].ToString() == "" ? "0" : data.Rows[0][49].ToString()).ToString("C0"))
                .Replace("@renglon14_1", sancion)
                .Replace("@renglon14_2", double.Parse(data.Rows[0][51].ToString() == "" ? "0" : data.Rows[0][51].ToString()).ToString("C0"))
                .Replace("@renglon15", double.Parse(data.Rows[0][52].ToString()).ToString("C0"));
        }

        protected void BtnBorrar_Click(object sender, EventArgs e)
        {
            var rta = _dataAlumbrado.DeleteData(int.Parse(hdIdRegistro.Value));
            if (rta)
            {
                var _MsgError = "El formulario de Impuesto ha sido eliminado.";
                #region REGISTRO DE LOGS DE AUDITORIA
                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                ObjAuditoria.ModuloApp = "REG_IMPUESTO_ALUMBRADO";
                ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                ObjAuditoria.DescripcionEvento = jsonRequest;
                ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                ObjAuditoria.TipoProceso = 1;

                btnPdf.Enabled = false;
                BtnBorrar.Enabled = false;
                BtnGuardar.Enabled = true;
                //'Agregar Auditoria del sistema
                string _MsgErrorLogs = "";
                if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                {
                    _log.Error(_MsgErrorLogs);
                }
                #endregion
                CleanForm();
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "", "../../Imagenes/Iconos/16/check.png");
                _log.Info(_MsgError);
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //Ventana = null;
                //_log.Info(_MsgError);
                //#endregion
            }
            else
            {
                var _MsgError = "Se ha presentado un error al eliminar, por favor comuniquese con el administrador del sistema.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                _log.Error(_MsgError);
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //Ventana = null;
                //_log.Error(_MsgError);
                //#endregion
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            var data = new AlumbradoDto
            {
                id_cliente = int.Parse(CmbCliente.SelectedValue),
                cod_dane = TxtCodDane.Text,
                municipio = LblNombreMunicipio.Text,
                id_municipio = int.Parse(hdIdMunicipio.Value),
                dpto = LblNombreDpto.Text,
                vigencia = int.Parse(TxtAnioGravable.Text),
                uvt = double.Parse(lblUVT.Text.Replace("$", "").Replace(".", "")),
                fechaMax = new DateTime(int.Parse(lblAnio.Text), int.Parse(lblMes.Text), int.Parse(lblDia.Text)),
                periodicidad = ddlPeriodicidad.SelectedValue,
                cliente_nombres = lblNombres.Text,
                cliente_id_doc = int.Parse(rblDocCliente.SelectedValue),
                cliente_numdoc = lblNumDoc.Text,
                cliente_dv = lblDV2.Text,
                cliente_direccion = lblDireccion.Text,
                cliente_municipio = lblMunicipioCliente.Text,
                cliente_dpto = lblDptoCliente.Text,
                cliente_mail = lblCorreo.Text,
                cliente_tel = lblTelFijo.Text,
                cliente_cel = lblCel.Text,
                id_sector = int.Parse(rblSector.SelectedValue),
                renglon1 = lblClasificacion.Text.Replace("$", "").Replace(".", ""),
                renglon2 = lblRenglon2.Text.Replace("$", "").Replace(".", ""),
                renglon3 = lblRenglon3.Text.Replace("$", "").Replace(".", ""),
                renglon4 = lblBaseGravable.Text.Replace("$", "").Replace(".", ""),
                renglon5 = lblBaseGravableEspecial.Text.Replace("$", "").Replace(".", ""),
                renglon6 = lblTarifaImpuesto.Text.Replace("$", "").Replace(".", ""),
                renglon7 = lblImpuesto.Text.Replace("$", "").Replace(".", ""),
                renglon8_1 = lblUVTMin.Text.Replace("$", "").Replace(".", ""),
                renglon8_2 = lblPagoMinimo.Text.Replace("$", "").Replace(".", ""),
                renglon9_1 = lblUVTMax.Text.Replace("$", "").Replace(".", ""),
                renglon9_2 = lblPagoMax.Text.Replace("$", "").Replace(".", ""),
                renglon10 = lblImpuestoEspecial.Text.Replace("$", "").Replace(".", ""),
                renglon11 = lblTotalImp.Text.Replace("$", "").Replace(".", ""),
                renglon12 = lblDescuento.Text.Replace("$", "").Replace(".", ""),
                renglon13 = txtMora.Text.Replace("$", "").Replace(".", ""),
                renglon14_1 = rblSanciones.SelectedValue,
                renglon14_2 = txtSanciones.Text.Replace("$", "").Replace(".", ""),
                renglon15 = lblTotal.Text.Replace("$", "").Replace(".", ""),
                id_contador = int.Parse(ddlContador.SelectedValue),
                id_firmante = int.Parse(ddlFirmante.SelectedValue),
                porcentaje = int.Parse(hddescuentoporcentaje.Value),
                id = int.Parse((hdIdRegistro.Value == "" ? "0" : hdIdRegistro.Value))
            };

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonRequest = js.Serialize(_dataAlumbrado);
            _log.Warn("REQUEST INSERT REG_IMPUESTO_ALUMBRADO => " + jsonRequest);
            int idRegister = 0;
            var _MsgError = "";
            var rta = false;
            rta = data.id == 0 ? _dataAlumbrado.InsertData(data, Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref idRegister) : _dataAlumbrado.UpdateData(data, Int32.Parse(Session["IdUsuario"].ToString().Trim()));
            if (rta)
            {
                _MsgError = "El formulario de Impuesto ha sido guardado con exito.";
                #region REGISTRO DE LOGS DE AUDITORIA
                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                ObjAuditoria.ModuloApp = "REG_IMPUESTO_ALUMBRADO";
                ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                ObjAuditoria.DescripcionEvento = jsonRequest;
                ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                ObjAuditoria.TipoProceso = 1;

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
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //_log.Info(_MsgError);
                //Ventana = null;
                //#endregion
            }
            else
            {
                _MsgError = "Se ha presentado un error al guardar, por favor comuniquese con el administrador del sistema.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                _log.Error(_MsgError);
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //Ventana = null;
                //_log.Error(_MsgError);
                //#endregion
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

        protected void btnBorrar2_Click(object sender, EventArgs e)
        {
            var rta = _dataAlumbrado.DeleteData(int.Parse(hdIdRegistro.Value));
            if (rta)
            {
                rblSector.ClearSelection();
                lblClasificacion.Text = "";
                lblRenglon2.Text = "";
                lblRenglon3.Text = "";
                lblBaseGravable.Text = "";
                lblTarifaImpuesto.Text = "";
                lblImpuesto.Text = "";
                hddescuentoporcentaje.Value = "";
                lblBaseGravableEspecial.Text = "";
                lblImpuestoEspecial.Text = "";
                lblUVTMin.Text = "";
                lblUVTMax.Text = "";
                lblPagoMinimo.Text = "";
                lblPagoMax.Text = "";
                lblTotalImp.Text = "";
                lblDescuento.Text = "";
                lblTotal.Text = "";
                txtMora.Text = "";
                txtSanciones.Text = "";
                lblTituloPeriodicidad.Text = "";
                lblAnio.Text = "";
                lblMes.Text = "";
                lblDia.Text = "";
                hdfechaMax.Value = "";
                ddlPeriodicidad.Visible = false;
                LblNombreMunicipio.Text = "";
                LblNombreDpto.Text = "";
                hdIdMunicipio.Value = "";
                lblUVT.Text = "";
                hdUVT.Value = "";
                var _MsgError = "El formulario de Impuesto ha sido eliminado.";

                #region REGISTRO DE LOGS DE AUDITORIA
                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                ObjAuditoria.ModuloApp = "REG_IMPUESTO_ALUMBRADO";
                ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                ObjAuditoria.DescripcionEvento = jsonRequest;
                ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                ObjAuditoria.TipoProceso = 1;

                btnPdf.Enabled = true;
                BtnBorrar.Enabled = true;
                //'Agregar Auditoria del sistema
                string _MsgErrorLogs = "";
                if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                {
                    _log.Error(_MsgErrorLogs);
                }
                #endregion

                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "", "../../Imagenes/Iconos/16/check.png");
                _log.Info(_MsgError);
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //Ventana = null;
                //_log.Info(_MsgError);
                //#endregion
            }
            else
            {
                var _MsgError = "Se ha presentado un error al eliminar, por favor comuniquese con el administrador del sistema.";
                RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "", "../../Imagenes/Iconos/16/delete.png");
                _log.Error(_MsgError);
                //#region MOSTRAR MENSAJE DE USUARIO
                ////Mostramos el mensaje porque se produjo un error con la Trx.
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
                //Ventana.Height = Unit.Pixel(250);
                //Ventana.Width = Unit.Pixel(500);
                //Ventana.KeepInScreenBounds = true;
                //Ventana.Title = "Mensaje del Sistema";
                //Ventana.VisibleStatusbar = false;
                //Ventana.Behaviors = WindowBehaviors.Close;
                //this.RadWindowManager1.Windows.Add(Ventana);
                //this.RadWindowManager1 = null;
                //Ventana = null;
                //_log.Error(_MsgError);
                //#endregion
            }
        }
    }
}