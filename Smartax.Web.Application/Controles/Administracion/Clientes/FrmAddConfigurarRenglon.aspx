<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddConfigurarRenglon.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddConfigurarRenglon" %>
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">CONFIGURAR RENGLON DEL FORMULARIO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón</asp:Label>
                            </td>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Digite la Cuenta Contable</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblNombreFormulario" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNumRenglon" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt"></asp:Label>
                            </td>
                            <td align="center" colspan="3">
                                <telerik:RadAutoCompleteBox ID="TxtCuentaContable" runat="server" DataTextField="Text" DataValueField="Value" DropDownHeight="240px" DropDownWidth="400px" EmptyMessage="Digite la cuenta contable" Font-Bold="False" Font-Size="11pt" InputType="Text" TabIndex="1" Width="450px">
                                </telerik:RadAutoCompleteBox>
                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Adicionar cuenta" ValidationGroup="ValidarDatos" Width="100px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="100px" />
                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtCuentaContable" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Label ID="LblDescripcion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Label ID="Label15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Seleccione el valor a obtener del estado financiero por cuenta y oficina</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor Extracontable</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:CheckBox ID="ChkSaldoInicial" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkSaldoInicial_CheckedChanged" Text="Aplicar Saldo Inicial" TabIndex="2" />
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="ChkMovDebito" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkMovDebito_CheckedChanged" Text="Aplicar Mov. Débito" TabIndex="3" />
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="ChkMovCredito" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkMovCredito_CheckedChanged" Text="Aplicar Mov. Crédito" TabIndex="4" />
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="ChkSaldoFinal" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkSaldoFinal_CheckedChanged" Text="Aplicar Saldo Final" TabIndex="5" />
                            </td>
                            <td align="center">
                                <telerik:RadNumericTextBox ID="TxtValorExtracontable" runat="server" EmptyMessage="Valor" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="15" TabIndex="6" Width="170px">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="13pt" ForeColor="White">CUENTAS ASOCIADAS AL RENGLON DEL FORMULARIO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="5"
                                            AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                            OnNeedDataSource="RadGrid1_NeedDataSource"
                                            OnItemCommand="RadGrid1_ItemCommand"
                                            OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                            <MasterTableView DataKeyNames="idcliente_base_gravable" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="idcliente_base_gravable" EmptyDataText=""
                                                        HeaderText="Id" UniqueName="idcliente_base_gravable" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Código"
                                                        UniqueName="codigo_cuenta" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre"
                                                        UniqueName="nombre_cuenta" FilterControlWidth="120px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="saldo_inicial" HeaderText="S. Inicial"
                                                        UniqueName="saldo_inicial" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="mov_debito" HeaderText="Mov. Débito"
                                                        UniqueName="mov_debito" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="mov_credito" HeaderText="Mov. Crédito"
                                                        UniqueName="mov_credito" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="saldo_final" HeaderText="S. Final"
                                                        UniqueName="saldo_final" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="valor_extracontable" HeaderText="V. Ext."
                                                        UniqueName="valor_extracontable" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="idcliente_establecimiento" HeaderText="Id Oficina"
                                                        UniqueName="idcliente_establecimiento" Visible="false" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Oficina"
                                                        UniqueName="nombre_oficina" FilterControlWidth="70px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="id_estado" HeaderText="Id Estado"
                                                        UniqueName="id_estado" Visible="false" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                        UniqueName="codigo_estado" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddEstablecimiento" Text="Asignar Oficina"
                                                        UniqueName="BtnAddEstablecimiento" ImageUrl="/Imagenes/Iconos/16/earth.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Bloquear/DesBloquear"
                                                        UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar la Cuenta"
                                                        UniqueName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </telerik:RadAjaxPanel>

            <asp:HiddenField ID="HiddenField1" runat="server" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                PopupControlID="Panel2"
                TargetControlID="HiddenField1"
                CancelControlID="BtnSalir1"
                BackgroundCssClass="backgroundColor">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="Panel2" runat="server" BackColor="#E6E6E6" Width="950px" HorizontalAlign="Center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div id="popupcontainer" style="width: 100%">
                            <asp:Panel ID="Panel3" runat="server" Width="950px" BorderWidth="1">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" bgcolor="#999999" colspan="5">
                                            <asp:Label ID="Label4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">ASIGNACIÓN DE OFICINA A LA CUENTA PARAMETRIZADA </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="LblTitulo3" runat="server" Font-Bold="True" CssClass="FormLabels" Font-Size="12pt">Código:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <telerik:RadTextBox ID="TxtCodOficina" runat="server" EmptyMessage="Código" Font-Size="15pt" Height="30px" MaxLength="20" TabIndex="6" Width="160px">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="LblTitulo4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nombre:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <telerik:RadTextBox ID="TxtNombreOficina" runat="server" EmptyMessage="Nombre oficina" Font-Size="15pt" Height="30px" MaxLength="250" TabIndex="6" Width="260px">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnConsultar_Click" Text="Buscar" ToolTip="Buscar cuenta" Width="120px" />
                                            &nbsp;<asp:Button ID="BtnSalir1" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnSalir1_Click" Text="Salir" ToolTip="Salir" Width="120px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5">
                                            <telerik:RadGrid ID="RadGrid4" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Culture="es-ES" OnItemCommand="RadGrid4_ItemCommand" OnNeedDataSource="RadGrid4_NeedDataSource" OnPageIndexChanged="RadGrid4_PageIndexChanged">
                                                <MasterTableView DataKeyNames="idcliente_establecimiento" Name="Grilla" NoMasterRecordsText="No hay registros para mostrar">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="idcliente_establecimiento" EmptyDataText="" FilterControlWidth="40px"
                                                            HeaderText="Id" ReadOnly="True" UniqueName="idcliente_establecimiento">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="codigo_oficina" FilterControlWidth="70px"
                                                            UniqueName="codigo_oficina" HeaderText="Cod. Oficina">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="codigo_dane" FilterControlWidth="70px"
                                                            UniqueName="codigo_dane" HeaderText="Cod. Dane">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Nombre Oficina"
                                                            ReadOnly="True" UniqueName="nombre_oficina" FilterControlWidth="120px">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="BtnAsociar" Text="Asociar oficina"
                                                            CommandName="BtnAsociar" ImageUrl="/Imagenes/Iconos/16/check.png">
                                                        </telerik:GridButtonColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5">
                                            <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

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
