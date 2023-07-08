<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerDesagregacion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmVerDesagregacion" %>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="DESAGREGACIÓN DE ACTIVIDADES ECONOMICAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                <MasterTableView DataKeyNames="idestab_act_economica" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idestab_act_economica" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idestab_act_economica">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cód. Actividad"
                                            UniqueName="codigo_actividad" FilterControlWidth="80px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tipo_tarifa" HeaderText="Tipo Tarifa"
                                            UniqueName="tipo_tarifa" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tarifa_ley" HeaderText="Tarifa Ley"
                                            UniqueName="tarifa_ley" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tarifa_municipio" HeaderText="Tarifa Municipio"
                                            UniqueName="tarifa_municipio" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cód. Cuenta"
                                            UniqueName="codigo_cuenta" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="saldo_inicial" HeaderText="Saldo Inicial"
                                            UniqueName="saldo_inicial" FilterControlWidth="70px" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="mov_debito" HeaderText="Mov. Débito"
                                            UniqueName="mov_debito" FilterControlWidth="70px" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="mov_credito" HeaderText="Mov. Crédito"
                                            UniqueName="mov_credito" FilterControlWidth="70px" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="saldo_final2" HeaderText="Saldo Final"
                                            UniqueName="saldo_final2" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
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
