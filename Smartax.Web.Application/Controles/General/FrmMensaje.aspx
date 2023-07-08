<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMensaje.aspx.cs" Inherits="Smartax.Web.Application.Controles.General.FrmMensaje" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="~/Styles/styles.css" />
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
        <div>
            <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                <table style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999" class="auto-style1">
                            <asp:Label ID="LblTitulo" runat="server" Font-Bold="True" ForeColor="White" Font-Size="16pt">MENSAJE DEL SISTEMA</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="LblMensaje" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClientClick="window.close()" Text="Salir" Width="150px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
