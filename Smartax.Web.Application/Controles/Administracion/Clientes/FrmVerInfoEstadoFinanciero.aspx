<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerInfoEstadoFinanciero.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmVerInfoEstadoFinanciero" %>
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
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACIÓN DETALLADA DEL ESTADO FINANCIERO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="90px">Mes del E.F.</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="130px">Versión del E.F.</asp:Label>
                                            </td>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="LblAnioGravable" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="LblMesEf" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="LblVersionEf" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="BtnExportar" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnExportar_Click" Text="Exportar a Excel" ToolTip="Click para exportar estado financiero a excel" Width="170px" />
                                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:Button ID="BtnProcesarBaseGMunicipios" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnProcesarBaseGMunicipios_Click" Text="Procesar Base G. x Municipios" ToolTip="Click para procesar la base gravable por municipios" Width="290px" />
                                                &nbsp;<asp:Button ID="BtnProcesarBaseGOficinas" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnProcesarBaseGOficinas_Click" Text="Procesar Base G. x Oficinas" ToolTip="Click para procesar la base gravable por oficinas" Width="290px" />
                                                <cc1:ConfirmButtonExtender ID="BtnProcesarBaseGOficinas_ConfirmButtonExtender" runat="server" BehaviorID="BtnProcesarBaseG_ConfirmButtonExtender" ConfirmText="¿Señor usuario, tenga en cuenta que para realizar este proceso debe contar con el ESTADO FINANCIERO cargado completamente al mes. Se encuentra seguro de realizar el Proceso ?" TargetControlID="BtnProcesarBaseGOficinas" />
                                                &nbsp;<cc1:ConfirmButtonExtender ID="BtnProcesarBaseGMunicipios_ConfirmButtonExtender" runat="server" BehaviorID="BtnProcesarBaseG_ConfirmButtonExtender" ConfirmText="¿Señor usuario, tenga en cuenta que para realizar este proceso debe contar con el ESTADO FINANCIERO cargado completamente al mes. Se encuentra seguro de realizar el Proceso ?" TargetControlID="BtnProcesarBaseGMunicipios" />
                                                <asp:Button ID="BtnProcesarProvIca" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnProcesarProvIca_Click" Text="Procesar Provisión de Ica" ToolTip="Click para procesar la provisión de Ica" Width="280px" />
                                                <cc1:ConfirmButtonExtender ID="BtnProcesarProvIca_ConfirmButtonExtender" runat="server" BehaviorID="BtnProcesarBaseG_ConfirmButtonExtender" ConfirmText="¿Señor usuario, tenga en cuenta que para realizar este proceso debe contar con la base gravable por oficinas ya generada ?" TargetControlID="BtnProcesarProvIca" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="idestado_financiero" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idestado_financiero" EmptyDataText="" FilterControlWidth="50px" HeaderText="Id" UniqueName="idestado_financiero">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_oficina" FilterControlWidth="50px" HeaderText="Cód. Oficina" UniqueName="codigo_oficina">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_oficina" FilterControlWidth="120px" HeaderText="Nombre Oficina" UniqueName="nombre_oficina">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" FilterControlWidth="60px" HeaderText="Cód. Cuenta" UniqueName="codigo_cuenta">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_inicial_mn" HeaderText="Saldo Inicial MN" UniqueName="saldo_inicial_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="debito_mn" HeaderText="Debito MN" UniqueName="debito_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="credito_mn" HeaderText="Credito MN" UniqueName="credito_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_final_mn" HeaderText="Saldo Final MN" UniqueName="saldo_final_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_inicial_me" HeaderText="Saldo Inicial ME" UniqueName="saldo_inicial_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="debito_me" HeaderText="Debito ME" UniqueName="debito_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="credito_me" HeaderText="Credito ME" UniqueName="credito_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_final_me" HeaderText="Saldo Final ME" UniqueName="saldo_final_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nota: Señor usuario tenga en cuenta que para generar el proceso de la PROVISIÓN DE ICA primero debe generar el proceso de la BASE GRAVABLE POR OFICINAS una vez termine este se debe procesar la provisión de Ica.</asp:Label>
                            </td>
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
