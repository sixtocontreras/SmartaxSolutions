<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLiquidacionOficina.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmLiquidacionOficina" %>
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
                            <td colspan="5" align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">REALIZAR LIQUIDACIÓN DE IMPUESTO DE ICA x OFICINA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes del EF</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Versión del Estado Financiero</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes a Liquidar</asp:Label>
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
                                <asp:Label ID="LblVersionEf" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="CmbMes" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el mes">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbMes" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;<asp:Button ID="BtnProcesar" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnProcesar_Click" Text="Procesar" ToolTip="Click para realizar el proceso de Liquidación" Width="130px" ValidationGroup="ValidarDatos" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="130px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" Width="100%"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="idestado_financiero" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idestado_financiero" EmptyDataText=""
                                                HeaderText="Id" UniqueName="idestado_financiero" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_oficina" HeaderText="Cód. Oficina"
                                                UniqueName="codigo_oficina" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Nombre Oficina"
                                                UniqueName="nombre_oficina" FilterControlWidth="120px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cód. Cuenta"
                                                UniqueName="codigo_cuenta" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_inicial_mn" HeaderText="Saldo Inicial MN"
                                                UniqueName="saldo_inicial_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="debito_mn" HeaderText="Debito MN"
                                                UniqueName="debito_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="credito_mn" HeaderText="Credito MN"
                                                UniqueName="credito_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_final_mn" HeaderText="Saldo Final MN"
                                                UniqueName="saldo_final_mn">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_inicial_me" HeaderText="Saldo Inicial ME"
                                                UniqueName="saldo_inicial_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="debito_me" HeaderText="Debito ME"
                                                UniqueName="debito_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="credito_me" HeaderText="Credito ME"
                                                UniqueName="credito_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_final_me" HeaderText="Saldo Final ME"
                                                UniqueName="saldo_final_me" Visible="false">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
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
