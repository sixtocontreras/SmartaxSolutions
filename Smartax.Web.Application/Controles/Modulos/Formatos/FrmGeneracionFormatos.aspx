<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGeneracionFormatos.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.Formatos.FrmGeneracionFormatos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

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
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                            <table cellpadding="3" cellspacing="0" class="Tab" style="width: 100%;">
                                <tr>
                                    <td align="center" colspan="3" bgcolor="#999999">
                                        <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="EJECUTAR FORMATOS 321 - 525" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cliente</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Vigencia</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodo</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbCliente" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione Cliente">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbCliente" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="TxtVigencia" runat="server" EmptyMessage="Vigencia" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" TabIndex="2" Width="100px">
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
                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="TxtVigencia" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="TxtPeriodoImpuesto" runat="server" EmptyMessage="Periodo Impuesto" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="3" Width="80px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtPeriodoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="BtnEjecutar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Ejecutar" ToolTip="Click para ejecutar el proceso de generacion de F-321/525" Width="120px" OnClick="BtnEjecutar_Click" ValidationGroup="ValidarDatos" />
                                        &nbsp;<asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" ToolTip="Salir" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </telerik:RadAjaxPanel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                    <h3>Espere un momento por favor ...</h3>
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
