<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerCalendarioTributario.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal.FrmVerCalendarioTributario" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="style/style1.css" media="screen" type="text/css" />
    <link rel='stylesheet' href='http://codepen.io/assets/libs/fullpage/jquery-ui.css'>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False" HorizontalAlign="NotSet">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="FECHAS DE VENCIMIENTOS CALENDARIOS TRIBUTARIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="BtnExportar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Exportar" ToolTip="Exportar calendario a excel" Width="140px" OnClick="BtnExportar_Click" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="140px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Table ID="tblTablaCalendario" runat="server" CellPadding="4" CellSpacing="0" class="Tab" Style="width: 100%;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnVencidos" runat="server" BackColor="OrangeRed" class="login login-submit" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnVencidos_Click" Text="Ver Vencidos" ToolTip="Click para ver detalle de municipios" Visible="False" Width="220px" Height="40px" />
                            &nbsp;<asp:Button ID="BtnVencimientosHoy" runat="server" BackColor="Yellow" class="login login-submit" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnVencimientosHoy_Click" Text="Vencimientos de hoy" ToolTip="Click para ver el detalle de los vencimientos de hoy" Visible="False" Width="220px" Height="40px" />
                            &nbsp;<asp:Button ID="BtnPorVencer" runat="server" BackColor="SlateBlue" class="login login-submit" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnPorVencer_Click" Text="Por Vencer" ToolTip="Click para ver detalle de proximos vencimientos" Visible="False" Width="220px" Height="40px" />
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
