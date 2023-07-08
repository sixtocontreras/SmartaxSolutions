<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRptMunicipioAutoretencion.ascx.cs" Inherits="Smartax.Web.Application.Controles.Reportes.Administrativos.CtrlRptMunicipioAutoretencion" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script  language="JavaScript" type="text/javascript">
        function ClickOnConfirmar(btn, msg) {
            // Comprobamos si se está haciendo una validación
            if (typeof (Page_ClientValidate) == 'function') {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }

            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button') {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg = 'undefined')) { msg = 'Procesando...'; }
                btn.value = msg;
                // La magia verdadera :D
                btn.disabled = true;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form2">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="False" HorizontalAlign="NotSet">
            <asp:Panel ID="PanelDatos" runat="server" Width="800px">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">REPORTE DE AUTORETENCIONES POR MUNICIPIO</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label12" runat="server" Font-Size="14pt" Text="Tipo de Impuesto"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label13" runat="server" Font-Size="14pt" Text="Estado"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="14pt" ToolTip="Tipo de Impuesto">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="14pt" ToolTip="Estado del establecimiento">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="BtnGenerar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGenerar_Click" OnClientClick="ClickOnConfirmar(this, 'Procesando...')" Text="Generar reporte" ValidationGroup="ValidarDatos" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="LblMensaje" runat="server" Font-Size="14pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            </telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        </telerik:RadAjaxLoadingPanel>
    </form>
</body>
</html>
