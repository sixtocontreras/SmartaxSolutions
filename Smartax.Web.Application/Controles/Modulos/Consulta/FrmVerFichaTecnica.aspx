<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerFichaTecnica.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.Consulta.FrmVerFichaTecnica" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" Width="100%">
                <asp:Panel ID="Panel1" runat="server">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="0" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="3">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="FICHA TÉCNICA DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="13pt" Text="No. de Item"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="13pt" Text="Concepto"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="13pt" Text="Información"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Size="13pt" Text="1"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label31" runat="server" Font-Bold="False" Font-Size="13pt" Text="Nombre del Departamento"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNomDepartamento" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="13pt" Text="2"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Size="13pt" Text="Nombre del Municipio"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNomMunicipio" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="13pt" Text="3"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label14" runat="server" Font-Bold="False" Font-Size="13pt" Text="NIT del Municipio"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNitMunicipio" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="13pt" Text="4"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label15" runat="server" Font-Bold="False" Font-Size="13pt" Text="Codigo DANE"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblCodigoDane" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="13pt" Text="5"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Size="13pt" Text="N° de establecimientos "></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNumEstablecimiento" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="13pt" Text="6"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label17" runat="server" Font-Bold="False" Font-Size="13pt" Text="Inscrito en el RIT"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblInscritoRit" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="13pt" Text="7"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label18" runat="server" Font-Bold="False" Font-Size="13pt" Text="Email Contacto"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblEmailContacto" runat="server" Font-Bold="False" Font-Size="13pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="13pt" Text="8"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label20" runat="server" Font-Bold="False" Font-Size="13pt" Text="Información de Bancos"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerInfoBancos" runat="server" OnClick="LnkVerInfoBancos_Click" ToolTip="Click para listar los bancos">Ver Detalle</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Size="13pt" Text="9"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label19" runat="server" Font-Bold="False" Font-Size="13pt" Text="Impuestos del Municipio"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerImpuestos" runat="server" OnClick="LnkVerImpuestos_Click" ToolTip="Click para listar los impuestos del municipio">Ver Detalle</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Size="13pt" Text="10"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label22" runat="server" Font-Bold="False" Font-Size="13pt" Text="Actividades Economicas"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerActEconomicas" runat="server" OnClick="LnkVerActEconomicas_Click" ToolTip="Click para listar las actividades economicas">Ver Detalle</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="13pt" Text="11"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label27" runat="server" Font-Bold="False" Font-Size="13pt" Text="Calendarío Tributario"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerCalendarioTrib" runat="server" OnClick="LnkVerCalendarioTrib_Click" ToolTip="Click para listar el calendario tributario">Ver Detalle</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="13pt" Text="12"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label29" runat="server" Font-Bold="False" Font-Size="13pt" Text="Normatividad"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerAcuerdosMun" runat="server" OnClick="LnkVerAcuerdosMun_Click" ToolTip="Click para listar los acuerdos municipales">Ver Detalle</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
