<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlFichaUsuario.ascx.cs" Inherits="Smartax.Web.Application.Controles.General.CtrlFichaUsuario" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Panel ID="PnlLoginOff" runat="server">
</asp:Panel>
<asp:Panel ID="PnlLoginOn" runat="server" BorderStyle="None">
    <script type="text/javascript">
        /* This script and many more are available free online at
        The JavaScript Source :: http://javascript.internet.com
        Created by: Neill Broderick :: http://www.bespoke-software-solutions.co.uk/downloads/downjs.php */

        var mins
        var secs;

        function cd() {
            mins = 1 * m("<%=Session.Timeout%>"); // change minutes here
            secs = 0 + s(":01"); // change seconds here (always add an additional second to your total)
            redo();
        }

        function m(obj) {
            for (var i = 0; i < obj.length; i++) {
                if (obj.substring(i, i + 1) == ":")
                    break;
            }
            return (obj.substring(0, i));
        }

        function s(obj) {
            for (var i = 0; i < obj.length; i++) {
                if (obj.substring(i, i + 1) == ":")
                    break;
            }
            return (obj.substring(i + 1, obj.length));
        }

        function dis(mins, secs) {
            var disp;
            if (mins <= 9) {
                disp = " 0";
            } else {
                disp = " ";
            }
            disp += mins + ":";
            if (secs <= 9) {
                disp += "0" + secs;
            } else {
                disp += secs;
            }
            return (disp);
        }

        function redo() {
            secs--;
            if (secs == -1) {
                secs = 59;
                mins--;
            }
            //document.form1.disp.value = dis(mins, secs); // setup additional displays here.
            if (navigator.appName == "Microsoft Internet Explorer") {
                document.getElementById("<%=LabelTimeOut.ClientID %>").innerText = dis(mins, secs);
            }
            if (navigator.appName != "Microsoft Internet Explorer") {
                document.getElementById("<%=LabelTimeOut.ClientID %>").textContent = dis(mins, secs);
            }
            if ((mins == 0) && (secs == 0)) {
                //window.alert("Time is up. Press OK to continue."); // change timeout message as required
                window.location = "/Salir.aspx" // redirects to specified page once timer ends and ok button is pressed
            } else {
                cd = setTimeout("redo()", 1000);
            }
        }

        function init() {
            cd();
        }

        window.onload = init;
    </script>
    <table cellpadding="4" cellspacing="0">
        <tr>
            <td>
                <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" Height="100px" Width="100px" Visible="false" ResizeMode="Fill" />
                <asp:Image ID="ImgUsuario" runat="server" AlternateText="Image text" Width="80px" Visible="false" />
            </td>
            <td class="style1">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="text-align: left" class="auto-style1">Bienvenido<br />
                            <asp:Label ID="LabelNombre" runat="server"
                                Style="font-weight: 700; font-size: medium"></asp:Label>
                            <br />
                            Su sesión se cerrará en&nbsp;<asp:Label ID="LabelTimeOut" runat="server"
                                Style="font-weight: 700;"></asp:Label>
                            &nbsp;minutos
                            <tooltip id="Tooltip1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;
            </td>
        </tr>
    </table>
</asp:Panel>
