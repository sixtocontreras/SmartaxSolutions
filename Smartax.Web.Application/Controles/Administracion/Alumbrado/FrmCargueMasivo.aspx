<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCargueMasivo.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Alumbrado.FrmCargueMasivo" %>

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
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <script>
                function download(filename, text) {
                    text = text.replaceAll('||', '\r\n');
                    var element = document.createElement('a');
                    element.setAttribute('href', 'data:text/plain+ parsed.join("\r\n\");charset=utf-8,' + text);
                    element.setAttribute('download', filename);

                    element.style.display = 'none';
                    document.body.appendChild(element);

                    element.click();

                    document.body.removeChild(element);
                }
                function reloadPage() {
                    window.parent.location.reload();
                }
            </script>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                    <table style="width: 100%">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CARGUE MASIVO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadAsyncUpload ID="upl" runat="server" MaxFileInputsCount="1"></telerik:RadAsyncUpload>
                                <asp:Button ID="btnCargar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Cargar" OnClick="btnCargar_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
