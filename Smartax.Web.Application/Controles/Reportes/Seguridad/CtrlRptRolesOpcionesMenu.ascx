<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRptRolesOpcionesMenu.ascx.cs" Inherits="Smartax.Web.Application.Controles.Reportes.Seguridad.CtrlRptRolesOpcionesMenu" %>
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
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }
        .auto-style2 {
            height: 27px;
        }
    </style>
</head>
<body>
    <form id="form2">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="False" HorizontalAlign="NotSet">
            <asp:Panel ID="PanelDatos" runat="server" Width="800px">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">REPORTE DE PERFILES Y OPCIONES ASIGNADAS</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" class="auto-style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label15" runat="server" Font-Size="14pt" Text="Perfil"></asp:Label>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label14" runat="server" Font-Size="14pt" Text="Estado"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:DropDownList ID="CmbPerfil" runat="server" Font-Size="14pt" ToolTip="Seleccione un Perfil de la lista">
                            </asp:DropDownList>
                        </td>
                        <td align="center" colspan="2">
                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="14pt" ToolTip="Estado del establecimiento">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="BtnGenerar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGenerar_Click" OnClientClick="ClickOnConfirmar(this, 'Procesando...')" Text="Generar reporte" ValidationGroup="ValidarDatos" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2"></td>
                        <td class="auto-style2" colspan="2">&nbsp;</td>
                        <td class="auto-style2"></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
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
