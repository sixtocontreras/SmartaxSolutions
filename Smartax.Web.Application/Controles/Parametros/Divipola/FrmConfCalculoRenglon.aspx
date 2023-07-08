<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfCalculoRenglon.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmConfCalculoRenglon" %>
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
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="CONFIGURAR OPERACION RENGLON FORMULARIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón Configurar</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón Operación</asp:Label>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="LblNumRenglon" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglonForm" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el renglon del formulario">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbRenglonForm" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center" colspan="2">
                            <telerik:RadTextBox ID="TxtDescripcion" runat="server" EmptyMessage="Descripción" Font-Size="15pt" Height="30px" MaxLength="120" TabIndex="3" Width="300px">
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="TxtDescripcion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador7">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodicidad</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Tarifa</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor tarifa</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbPeriodicidad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el valor">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbPeriodicidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoTarifa" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo tarifa">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="CmbTipoTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <telerik:RadNumericTextBox ID="TxtValorTarifa" runat="server" EmptyMessage="Tarifa" Font-Size="15pt" Height="30px" MaxLength="4" MaxValue="10000" MinValue="1" TabIndex="12" Width="100px">
                                <NegativeStyle Resize="None" />
                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="Validador8" runat="server" ControlToValidate="TxtValorTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador8_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador8">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el estado">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" Height="40px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" Height="40px" />
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
