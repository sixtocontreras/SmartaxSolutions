<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmProcesoContabilizacion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmProcesoContabilizacion" %>

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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False" HorizontalAlign="NotSet">
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" class="auto-style1" colspan="4">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">PROCESO COMPROBANTE DE CONTABILIZACIÓN</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodicidad</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes del Impuesto</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione un año de la lista">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el tipo de impuesto" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbPeriodicidad" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione una periodicidad de la lista">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbPeriodicidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbMeses" runat="server" Font-Size="15pt" TabIndex="4" ToolTip="Seleccione un mes de la lista" AutoPostBack="True" OnSelectedIndexChanged="CmbMeses_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbMeses" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Impuestos según Calendario Tributario</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Impuestos según Liquidación</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Visible="False">Estado de Aprobación</asp:Label>
                                            </td>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="LblCantidad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="22pt" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="LblCantidad2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="22pt" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td align="center">&nbsp;</td>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <%--<tr>
                                                    <td align="center" colspan="4">
                                                        <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Ingrese los Correos separados por Punto y Coma (;) donde llegara la Notificación</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <telerik:RadTextBox ID="TxtEmailsConfirmacion" runat="server" EmptyMessage="email1@dominio.com;email2@dominio.com.co;email3@dominio.net;email4@dominio.gov" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="5" TextMode="MultiLine" Width="600px">
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="TxtEmailsConfirmacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>--%>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Visible="False">Nota: </asp:Label>
                                <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="11pt" Visible="False">Señor usuario, recuerde que una vez da clic sobre el botón “Ejecutar Proceso”, el mismo se realizará en “Background”, cuando el proceso finalice recibirá un correo de confirmación.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="BtnEjecutar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnEjecutar_Click" Text="Ejecutar Proceso" ToolTip="Click para ejecutar el proceso por Lote" ValidationGroup="ValidarDatos" Width="180px" Height="40px" TabIndex="6" Enabled="False" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="180px" Height="40px" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </telerik:RadAjaxPanel>
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
