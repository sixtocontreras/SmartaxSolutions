<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAprobarLiquidacionPorLote.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmAprobarLiquidacionPorLote" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center" colspan="4" bgcolor="#999999">
                                        <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="APROBAR O RECHAZAR LIQUIDACIÓN DEL IMPUESTO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Año Gravable</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Estado Liquidación</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Tipo de Impuesto</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Periodicidad</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LblAnioGravable" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblEstadoLiquidacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTipoImpuesto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblPeriodicidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Departamento</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Código Dane</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Municipio</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Estado de la Aprobación</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LblDepartamento" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblCodDane" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblMunicipio" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbEstadoAprobacion" runat="server" RepeatDirection="Horizontal" TabIndex="5" ToolTip="Es consorcio unión temporal ?">
                                            <asp:ListItem Value="1">APROBADA</asp:ListItem>
                                            <asp:ListItem Value="2">RECHAZADA</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="RbEstadoAprobacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="Validador1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Panel ID="Panel2" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt" Width="350px">Analista Procesa</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label44" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Emails Notificación Validación</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="LblAnalista" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadTextBox ID="TxtEmailsNotificacion" runat="server" EmptyMessage="email1@dominio.com;email2@dominio.com.co;email3@dominio.net;email4@dominio.gov" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="7" TextMode="MultiLine" Width="500px">
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="TxtEmailsNotificacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="Validador2_ValidatorCalloutExtender" TargetControlID="Validador2">
                                                        </ajaxToolkit:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="Label45" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Observación Validación Impuesto</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <telerik:RadTextBox ID="TxtObservacion" runat="server" EmptyMessage="Observación de la Validación" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="7" TextMode="MultiLine" Width="700px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtObservacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="Validador3_ValidatorCalloutExtender" TargetControlID="Validador3">
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
                                        <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="45px" OnClick="BtnGuardar_Click" Text="Enviar Notificación" ToolTip="Click para guardar validación y enviar notificación" ValidationGroup="ValidarDatos" Width="200px" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="45px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
