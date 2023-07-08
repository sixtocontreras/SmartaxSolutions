<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddTarifasMinima.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAddTarifasMinima" %>
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
.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}
        </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999" colspan="4">
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR TARIFAS MINIMAS DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Unidad de Medida</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo documento" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center" colspan="2">
                            <asp:DropDownList ID="CmbUnidadMedida" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione la unidad de medida" AutoPostBack="True" OnSelectedIndexChanged="CmbUnidadMedida_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbUnidadMedida" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el valor" AutoPostBack="True" OnSelectedIndexChanged="CmbAnioGravable_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Fiscal</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor Unidad</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Tarifa</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="LblCantidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad Unidad</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbAnioFiscal" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbAnioGravable_SelectedIndexChanged" TabIndex="3" ToolTip="Seleccione el valor">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador11" runat="server" ControlToValidate="CmbAnioFiscal" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador11_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador11">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:Label ID="LblValorUnidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt">$ 0.0</asp:Label>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoTarifa" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoTarifa_SelectedIndexChanged" TabIndex="4" ToolTip="Seleccione el tipo de tarifa">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="CmbTipoTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador7">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <telerik:RadNumericTextBox ID="TxtCantidad" runat="server" AutoPostBack="True" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="4" MaxValue="10000" MinValue="1" OnTextChanged="TxtCantidad_TextChanged" TabIndex="5" Width="100px">
                                <NegativeStyle Resize="None" />
                                <NumberFormat DecimalDigits="2" ZeroPattern="n" />
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="TxtCantidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tarifa Mínima</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                        </td>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblValorTarifaMinima" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt">$ 0.0</asp:Label>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="6" ToolTip="Seleccione el estado">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador10" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador10_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador10">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="ChkCalcular" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkCalcular_CheckedChanged" TabIndex="7" Text="Para Calcular Renglón" />
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglonForm" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador8" runat="server" ControlToValidate="CmbRenglonForm" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador8_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador8">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" Height="40px" TabIndex="9" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" Height="40px" TabIndex="10" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            </telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
            <div class="loading">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
            </div>
        </telerik:RadAjaxLoadingPanel>
    </form>
</body>
</html>
