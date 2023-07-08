<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRetencionICA.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmRetencionICA" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="JavaScript">
        var msg = "¡El botón derecho está desactivado para este sitio !";
        function disableIE() {
            if (document.all) {
                //alert(msg);
                return false;
            }
        }

        function disableNS(e) {
            if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2)) {
                //alert(msg);
                return false;
            } else if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2)) {
                //alert(msg);
                return false;
            }

            //alert('El Navegador es: ' + navigator.appName);
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = disableNS;
        } else {
            document.onmouseup = disableNS;
            document.oncontextmenu = disableIE;
        }
        document.oncontextmenu = new Function("return false")
        //document.oncontextmenu = new Function("alert(msg);return false")
    </script>
    <style type="text/css">
        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
            height: 25px;
        }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

        .auto-style2 {
            width: 268435456px;
        }
    </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:HiddenField ID="hdCliente" runat="server" />
                <asp:Panel ID="pnlSelectCliente" runat="server" Width="1200px" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="0" cellspacing="0" class="Tab" border="0" style="width: 1200px;">
                        <tr>
                            <td colspan="4" align="center" bgcolor="#999999">
                                <asp:Label ID="Label4" runat="server" CssClass="SubTitle" Text="LIQUIDACIÓN DE RETENCIÓN DE INDUSTRIA Y COMERCIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <br />
                                <asp:Label ID="Label8" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" Text="Cliente"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:DropDownList ID="CmbCliente" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione Cliente">
                                </asp:DropDownList>
                           
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="BtnCargar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Cargar" ToolTip="Click para cargar la liquidacion" Width="120px" OnClick="BtnCargar_Click" ValidationGroup="ValidarDatos" />
                                <asp:Button ID="BtnSalirModal" runat="server" Font-Bold="True" Font-Size="14pt" Text="Salir" ToolTip="Click para cerrar la ventana" Width="120px" OnClick="BtnSalirModal_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PnlDatos" runat="server" Visible="false" Width="1250px" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="1" style="width: 1200px;">
                        <tr>
                            <td colspan="11" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="LIQUIDACIÓN DE RETENCIÓN DE INDUSTRIA Y COMERCIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="11">
                                <asp:Label ID="LbTitulo0" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="FORMULARIO UNICO NACIONAL DE DECLARACIÓN Y PAGO DEL IMPUESTO DE INDUSTRIA Y COMERCIO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Label ID="Label48" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">DEPARTAMENTO:</asp:Label>
                            </td>
                            <td align="center" colspan="5">
                                <asp:Label ID="lblNombreDpto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="400px"></asp:Label>
                            </td>

                            <td align="center" colspan="2">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="155px">Código DANE</asp:Label>
                            </td>
                            <td colspan="2">
                                <telerik:RadNumericTextBox ID="TxtCodDane" runat="server" AutoPostBack="True" EmptyMessage="" Font-Bold="False" Font-Size="15pt" Height="26px" MaxLength="10" MinValue="1" OnTextChanged="TxtCodDane_TextChanged" TabIndex="1" Width="120px" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
  
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="3" align="right">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">MUNICIPIO O DISTRITO:</asp:Label>
                            </td>

                            <td rowspan="3" align="center" colspan="4">
                                <asp:HiddenField ID="hdIdRegistro" runat="server" />
                                <asp:HiddenField ID="hdIdMunicipio" runat="server" />
                                <asp:HiddenField ID="hdfechaMax" runat="server" />
                                <asp:Label ID="LblNombreMunicipio" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="center" colspan="5">
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">Fecha máxima presentación</asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td align="center" colspan="2">
                                <asp:Label ID="Label49" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">DD</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label53" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">MM</asp:Label>
                            </td>
                            <td align="center" class="auto-style2">
                                <asp:Label ID="Label54" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">AAAA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblFechaMaximaDia" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px"></asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblFechaMaximaMes" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px"></asp:Label>
                            </td>
                            <td align="center" class="auto-style2">
                                <asp:Label ID="lblFechaMaximaAno" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">1. AÑO GRAVABLE:</asp:Label>
                            </td>

                            <td align="center">
                                <telerik:RadNumericTextBox ID="TxtAnioGravable" OnTextChanged="TxtAnioGravable_TextChanged" runat="server" EmptyMessage="" Font-Bold="False" Font-Size="15pt" Height="26px" MaxLength="4" MinValue="1" TabIndex="2" Width="120px" AutoPostBack="True" >
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                              <asp:HiddenField runat="server" ID="hdUVT" />
                            </td>

                            <td align="center">
                                <asp:Label ID="Label14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">2. PERIODICIDAD:</asp:Label>
                            </td>

                            <td colspan="5" align="center">
                                <asp:RadioButton ID="rbMensual" Enabled="false" Text="Mensual" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbBimestral" Enabled="false" Text="Bimestral" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbTrimestral" Enabled="false" Text="Trimestral" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbCuatrimestral" Enabled="false" Text="Cuatrimestral" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbSemestral" Enabled="false" Text="Semestral" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbAnual" Enabled="false" Text="Anual" GroupName="cbperiodicidad" runat="server" Checked="false"></asp:RadioButton>
                            </td>

                            <td align="center">
                                <asp:Label ID="Label55" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">PERIODO A DECLARAR:</asp:Label>
                            </td>

                            <td align="center" colspan="2">
                                <asp:DropDownList ID="ddlPeriodoDeclarar" AutoPostBack="true" Width="70%" Visible="false" runat="server" Font-Size="13pt" TabIndex="1" ToolTip="Seleccione" OnSelectedIndexChanged ="ddlPeriodoDeclarar_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">3. OPCIÓN DE USO: </asp:Label>
                            </td>
                            <td colspan="6" align="center">
                                <asp:RadioButton ID="rbInicial" Enabled="false"  GroupName="opcionuso" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Declaración inicial  " runat="server" Checked="true"></asp:RadioButton>
                                <asp:RadioButton ID="rbPago" Enabled="false" GroupName="opcionuso" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Pago   " runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="rbCorreccion" Enabled="false" GroupName="opcionuso" AutoPostBack="true" Text="Corrección" runat="server" Checked="false" OnCheckedChanged="Unnamed_CheckedChanged"></asp:RadioButton>
 
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="9pt">Formulario anterior:</asp:Label>
                                                            
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtFormularioAnterior" runat="server" EmptyMessage="Formulario anterior" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="4" MinValue="1" TabIndex="2" Width="120px" AutoPostBack="false" Visible="false">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="9pt">Fecha de presentación</asp:Label>
                                                         
                            </td>
                            <td class="auto-style2">
                                <asp:Label ID="lblFechaActual" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt"></asp:Label>
                               
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6" align="center">
                                <label class="FormLabels" style="font-weight: bold; font-size: 10pt; transform: rotate(180deg); writing-mode: vertical-rl;">
                                    DATOS<br />
                                    GENERALES</label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombres y Apellidos ó Razón Social del Establecimiento</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="lblNombres" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:RadioButtonList runat="server" ID="rblDocCliente" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="3" Enabled="false" Font-Bold="False" Font-Size="12pt">
                                    <asp:ListItem Value="1">Cédula Ciudadania</asp:ListItem>
                                    <asp:ListItem Value="2">NIT</asp:ListItem>
                                    <asp:ListItem Value="3">Cédula Extranjeria</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">N°</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblNumDoc" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">DV</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDV2" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">Dirección de Notificación</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDireccion" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Departamento</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDptoCliente" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">Municipio</asp:Label>
                            </td>
                            <td class="auto-style2">
                                <asp:Label ID="lblMunicipioCliente" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Correo Electrónico de Notificación</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblCorreo" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Teléfono Fijo</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTelFijo" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label122" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Teléfono Celular</asp:Label>
                            </td>
                            <td class="auto-style2">
                                <asp:Label ID="lblCel" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">12. Tipo de contribuyente</asp:Label>
                            </td>
                            <td align="center" colspan="6">
                                <asp:Label ID="Label22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">13. Tipo de actividad</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">14. Gran Contribuyente</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="LblTA" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="6" align="center">
                                <asp:RadioButton ID="RadioButton1" Enabled="false" GroupName="tipoactividad" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Indusria" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="RadioButton2" Enabled="false" GroupName="tipoactividad" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Comercial" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="RadioButton3" Enabled="false" GroupName="tipoactividad" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Servicios" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="RadioButton4" Enabled="false" GroupName="tipoactividad" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Financiera" runat="server" Checked="false"></asp:RadioButton>
                            </td>
                            <td colspan="2" align="center">
                                <asp:RadioButton ID="RadioButton5" Enabled="false" GroupName="grancontribuyente" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="Si" runat="server" Checked="false"></asp:RadioButton>
                                <asp:RadioButton ID="RadioButton6" Enabled="false" GroupName="grancontribuyente" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" Text="No" runat="server" Checked="false"></asp:RadioButton>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="11" align="center">
                                <label class="FormLabels" style="font-weight: bold; font-size: 10pt; transform: rotate(180deg); writing-mode: vertical-rl;">BASE DE RETENCIÓN</label>
                            </td>

                            <td colspan="7">
                                <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por actividades industriales</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">15</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por actividades comerciales</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">16</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por actividades de servicios</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">17</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por actividades financieras</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">18</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención sobre rendimientos financieros</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">19</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label72" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención en pagos con Tarjeta débito y crédito</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label73" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">20</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label75" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por Avisos y Tableros</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label76" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">21</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label78" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por Sobretasa Bomberil</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label79" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">22</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label81" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por Pesas y Medidas</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label82" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">23</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label84" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base de retención por Otros Conceptos</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label85" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">24</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label74" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Total base retenciones (Suma renglones 15+16+17+18+19+20+21+22+23+24)</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label77" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">25</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblTotalRetenciones" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="11" align="center">
                                <label class="FormLabels" style="font-weight: bold; font-size: 10pt; transform: rotate(180deg); writing-mode: vertical-rl;">RETENCIONES APLICADAS</label>
                            </td>
                            <td colspan="7">
                                <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">
                                    Retenciones practicadas por actividades industriales
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">26</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por actividades comerciales
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">27</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por actividades de servicios</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">28</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label80" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por actividades financieras</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label83" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">29</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label87" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas sobre rendimientos financieros</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label88" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">30</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label90" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas en pagos con Tarjeta débito y crédito</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label91" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">31</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label93" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por conceptos de Avisos y Tableros</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label94" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">32</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label96" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por concepto de Sobretasa Bomberil</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label97" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">33</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label99" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por concepto de Pesas y Medidas</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label100" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">34</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label102" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Retenciones practicadas por Otros Conceptos</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label103" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">35</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label105" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Total retenciones practicadas (Suma renglones 26+27+28+29+30+31+32+33+34+35)</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label106" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">36</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblTotalRetencionesPracticadas" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3" align="center">
                                <label class="FormLabels" style="font-weight: bold; font-size: 10pt; transform: rotate(180deg); writing-mode: vertical-rl;">LIQUIDACIÓN</label>
                            </td>
                            <td colspan="7">
                                <asp:Label ID="Label44" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">
                                    (-) Menos: Descuentos por devoluciòn de retenciones
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">37</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <telerik:RadNumericTextBox ID="txtRenglon37" runat="server" OnTextChanged="txtRenglon37_TextChanged" AutoPostBack="True" EmptyMessage="" Font-Bold="False" Font-Size="12pt" Height="26px" MaxLength="10" MinValue="0" TabIndex="1" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Right" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label52" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">
                                    (-) Menos: Pago declaraciòn (En caso de ser una correcciòn)
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label50" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">38</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label60" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Total retenciones a declarar (Suma Renglones 36-37-38)</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label57" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">39</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblRenglon39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3" align="center">
                                <label class="FormLabels" style="font-weight: bold; font-size: 10pt; transform: rotate(180deg); writing-mode: vertical-rl;">PAGOS</label>
                            </td>
                            <td colspan="7">
                                <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Pago sanciones</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">40</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <telerik:RadNumericTextBox ID="txtRenglon40" runat="server" OnTextChanged="txtRenglon40_TextChanged" AutoPostBack="True" EmptyMessage="" Font-Bold="False" Font-Size="12pt" Height="26px" MaxLength="10" MinValue="0" TabIndex="1" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Right" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label46" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Pago intereses de mora</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label47" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">41</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <telerik:RadNumericTextBox ID="txtRenglon41" runat="server" OnTextChanged="txtRenglon41_TextChanged" AutoPostBack="True" EmptyMessage="" Font-Bold="False" Font-Size="12pt" Height="26px" MaxLength="10" MinValue="0" TabIndex="1" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Right" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="Label62" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">TOTAL A PAGAR (Suma Renglones 39+40+41)</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label61" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">42</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblTotalPagos" align="right" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Firma del declarante</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:RadioButtonList runat="server" ID="rblContador" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="5" Enabled="false">
                                    <asp:ListItem Value="1">Contador Público</asp:ListItem>
                                    <asp:ListItem Value="2">Revisor Fiscal</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                                <asp:Image ID="imgFirmante" runat="server" /><br />
                            </td>
                            <td colspan="6">
                                <br />
                                <asp:Image ID="imgContador" runat="server" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombres</asp:Label>
                                <asp:DropDownList ID="ddlFirmante" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFirmante_SelectedIndexChanged" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione...">
                                </asp:DropDownList>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombre</asp:Label>
                                <asp:DropDownList ID="ddlContador" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlContador_SelectedIndexChanged" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione...">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:RadioButton Text="CC" Enabled="false" runat="server" ID="rbccFirma" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lblccfirmante" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="CE" Enabled="false" runat="server" ID="rbceFirma" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lblcefirmante" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="TP" Enabled="false" runat="server" ID="rbctpFirma" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lbltpfirmante" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:RadioButton Text="CC" Enabled="false" runat="server" ID="rbccConta" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lblccConta" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="CE" Enabled="false" runat="server" ID="rbceConta" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lblceConta" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="TP" Enabled="false" runat="server" ID="rbtpConta" Checked="false"></asp:RadioButton>
                                <asp:Label ID="lbltpConta" runat="server" CssClass="FormLabels" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11">
                                <asp:Label ID="Label45" runat="server" CssClass="FormLabels" Font-Size="11pt">EN CONCORDANCIA CON LO ESTABLECIDO EN EL ACUERDO MUNICIPAL No. ___________ DEL _______ DE____________ DEL AÑO _________</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" align="center">
                                <asp:Label ID="Label51" runat="server" CssClass="FormLabels" Font-Size="11pt">SE REQUIERE INFORMACIÓN EXÓGENA: SI ______ NO ______</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="11">
                                <asp:Panel ID="Panel3" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label120" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">ACCIONES DE BOTONES</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" Text="Guardar" ToolTip="Click para guardar el definitivo del formularion del alumbrado" Enabled="false" ValidationGroup="ValidarDatos" Width="200px" OnClick="BtnGuardar_Click" />
                                                <telerik:RadButton Font-Bold="True" Text="Borrar" ID="BtnBorrar" runat="server" Font-Size="14pt" Height="40px" ToolTip="Click para borrar el formulario" Width="200px" Enabled="true" OnClick="BtnBorrar_Click">
                                                    <ConfirmSettings Title="Confirmación" ConfirmText="¿Esta seguro que desea eliminar este formulario de liquidación?" />
                                                </telerik:RadButton>
                                                &nbsp;<asp:Button ID="btnPdf" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" Text="Descargar pdf" ToolTip="Click para descargar el formulartio" Width="200px" Enabled="false" OnClick="btnPdf_Click" />
                                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" OnClick="BtnSalir_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
