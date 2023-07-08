<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLoadEstadoFinanciero.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmLoadEstadoFinanciero" %>
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
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">CARGAR ESTADO FINANCIERO DEL CLIENTE</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes del EF</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Versión del EF</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Separado Por</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Seleccione el archivo</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione año gravable">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos2"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="CmbMesEf" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el mes del estado financiero">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="CmbMesEf" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos2"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="CmbVersionEf" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione la versión del estado financiero">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbVersionEf" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos2"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="CmbTipoCaracter" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo caracter">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbTipoCaracter" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos1"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileExaminar" runat="server" Width="300px" />
                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="FileExaminar" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos1"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                </cc1:ValidatorCalloutExtender>
                                &nbsp;&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="BtnPrecargar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnPrecargar_Click" Text="Precargar" ToolTip="Click para realizar precarga del archivo" ValidationGroup="ValidarDatos1" Width="120px" />
                                &nbsp;<asp:Button ID="BtnProcesar" runat="server" Enabled="False" Font-Bold="True" Font-Size="14pt" OnClick="BtnProcesar_Click" Text="Procesar" ToolTip="Click para cargar los datos" Width="120px" ValidationGroup="ValidarDatos2"/>
                                &nbsp;<asp:Button ID="BtnCancelar" runat="server" Enabled="False" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnCancelar_Click" Text="Cancelar" ToolTip="Click para cancelar el proceso" Width="120px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="13pt" ForeColor="Black">INFORMACIÓN DEL ESTADO FINANCIERO A CARGAR</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="id_registro" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_registro" EmptyDataText="" HeaderText="Id"
                                                 UniqueName="id_registro" FilterControlWidth="40px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cód. Cuenta"
                                                UniqueName="codigo_cuenta" FilterControlWidth="70px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_oficina" HeaderText="Cód. Oficina"
                                                UniqueName="codigo_oficina" FilterControlWidth="70px">
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
