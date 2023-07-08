<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRptLogsAuditoria.ascx.cs" Inherits="Smartax.Web.Application.Controles.Reportes.Seguridad.CtrlRptLogsAuditoria" %>
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
    .RadPicker{vertical-align:middle}.rdfd_{position:absolute}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput .riTextBox{height:17px}.RadPicker_Default .rcCalPopup{background-position:0 0}.RadPicker_Default .rcCalPopup{background-image:url('mvwres://Telerik.Web.UI, Version=2020.1.219.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif')}.RadPicker .rcCalPopup{display:block;overflow:hidden;width:22px;height:22px;background-color:transparent;background-repeat:no-repeat;text-indent:-2222px;text-align:center;-webkit-box-sizing:content-box;-moz-box-sizing:content-box;box-sizing:content-box}.RadPicker td a{position:relative;outline:0;z-index:2;margin:0 2px;text-decoration:none}
    </style>
</head>
<body>
    <form id="form2">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="False" HorizontalAlign="NotSet">
            <asp:Panel ID="PanelDatos" runat="server" Width="800px">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">REPORTE LOGS DE AUDITORIA DEL SISTEMA</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label11" runat="server" Font-Size="15pt" Text="Fecha Inicial"></asp:Label>
                        </td>
                        <td align="center" class="auto-style1" colspan="2">
                            <asp:Label ID="Label10" runat="server" Font-Size="15pt" Text="Fecha Final"></asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label8" runat="server" Font-Size="15pt" Text="Evento"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1">
                            <telerik:RadDatePicker ID="DtFechaInicial" runat="server">
                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                </DateInput>
                            </telerik:RadDatePicker>
                        </td>
                        <td align="center" class="auto-style1" colspan="2">
                            <telerik:RadDatePicker ID="DtFechaFinal" runat="server">
                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                            </telerik:RadDatePicker>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:DropDownList ID="CmbEvento" runat="server" Font-Size="15pt">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="BtnGenerar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGenerar_Click" OnClientClick="ClickOnConfirmar(this, 'Procesando...')" Text="Generar reporte" ValidationGroup="ValidarDatos" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="auto-style2">
                        </td>
                        <td class="auto-style2" colspan="2"></td>
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
