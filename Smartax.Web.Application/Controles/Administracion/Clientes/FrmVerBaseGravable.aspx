<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerBaseGravable.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmVerBaseGravable" %>
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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False">
                <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="4" align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACIÓN DETALLADA DE LA BASE GRAVABLE PROCESADA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes del EF</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Versión del Estado Financiero</asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblAnioGravable" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="200px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblMesEf" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblVersionEf" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="250px"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;<asp:Button ID="BtnExportar" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnExportar_Click" Text="Exportar a Excel" ToolTip="Click para exportar estado financiero a excel" Width="170px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" Width="100%"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="idbase_gravable" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idbase_gravable" EmptyDataText=""
                                                HeaderText="Id" UniqueName="idbase_gravable" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cód. Dane"
                                                UniqueName="codigo_dane" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                                UniqueName="nombre_municipio" FilterControlWidth="120px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon8" HeaderText="V. Renglon 8"
                                                UniqueName="valor_renglon8" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon9" HeaderText="V. Renglon 9"
                                                UniqueName="valor_renglon9" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon10" HeaderText="V. Renglon 10"
                                                UniqueName="valor_renglon10" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon11" HeaderText="Credito MN"
                                                UniqueName="valor_renglon11" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon12" HeaderText="V. Renglon 12"
                                                UniqueName="valor_renglon12" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon13" HeaderText="V. Renglon 13"
                                                UniqueName="valor_renglon13" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon14" HeaderText="V. Renglon 14"
                                                UniqueName="valor_renglon14" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon15" HeaderText="V. Renglon 15"
                                                UniqueName="valor_renglon15" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_renglon26" HeaderText="V. Renglon 26"
                                                UniqueName="valor_renglon26" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
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
