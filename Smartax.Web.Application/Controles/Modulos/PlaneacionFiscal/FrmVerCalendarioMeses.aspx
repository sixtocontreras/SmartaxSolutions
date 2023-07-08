<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerCalendarioMeses.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal.FrmVerCalendarioMeses" %>
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
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" HorizontalAlign="NotSet">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Table ID="tblTablaMeses" runat="server" CellPadding="4" CellSpacing="0" class="Tab" Style="width: 100%;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#999999" colspan="3">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="FECHAS LIMITES DE PAGOS Y DESCUENTOS POR PRONTO PAGO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                        </td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo de impuesto">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el año">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                        </td>
                        <td align="center">
                            <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnConsultar_Click" TabIndex="3" Text="Consultar" ToolTip="Click para consultar información" ValidationGroup="ValidarDatos" Width="120px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="10" Text="Salir" ToolTip="Salir" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">&nbsp;</td>
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
