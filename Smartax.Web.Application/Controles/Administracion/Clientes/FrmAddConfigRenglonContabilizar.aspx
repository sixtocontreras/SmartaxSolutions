<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddConfigRenglonContabilizar.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddConfigRenglonContabilizar" %>
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
                            <td align="center" bgcolor="#999999" colspan="4">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">CONFIGURAR RENGLON A CONTABILIZAR DEL IMPUESTO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo</asp:Label>
                            </td>
                            <td align="center">
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
                            <td align="center">
                                <asp:RadioButtonList ID="RbTipo" runat="server" RepeatDirection="Horizontal" Width="100%" ValidationGroup="ValidarDatos">
                                    <asp:ListItem Value="D">Debito</asp:ListItem>
                                    <asp:ListItem Value="C">Crédito</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="RbTipo" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <telerik:RadAutoCompleteBox ID="TxtCuentaContable" runat="server" DataTextField="Text" DataValueField="Value" DropDownHeight="240px" DropDownWidth="400px" EmptyMessage="Digite la cuenta contable" Font-Bold="False" Font-Size="11pt" InputType="Text" TabIndex="1" Width="450px">
                                </telerik:RadAutoCompleteBox>
                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Adicionar cuenta" ValidationGroup="ValidarDatos" Width="100px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="100px" />
                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtCuentaContable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Label ID="LblDescripcion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="4">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="13pt" ForeColor="White">CUENTAS ASOCIADAS AL RENGLON DEL FORMULARIO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="5">
                                            <MasterTableView DataKeyNames="idconf_contabilizacion" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="idconf_contabilizacion" EmptyDataText="" HeaderText="Id" 
                                                        UniqueName="idconf_contabilizacion" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="tipo" HeaderText="Naturaleza Renglon" 
                                                        UniqueName="tipo" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Código" 
                                                        UniqueName="codigo_cuenta" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="tipo_naturaleza" HeaderText="Naturaleza Cuenta" 
                                                        UniqueName="tipo_naturaleza" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre" 
                                                        UniqueName="nombre_cuenta" FilterControlWidth="120px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="id_estado" HeaderText="Id Estado" 
                                                        UniqueName="id_estado" Visible="false" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado" 
                                                        UniqueName="codigo_estado" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddEstablecimiento" Text="Asignar Oficina"
                                                        ImageUrl="/Imagenes/Iconos/16/earth.png" UniqueName="BtnAddEstablecimiento">
                                                    </telerik:GridButtonColumn>--%>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Bloquear/DesBloquear"
                                                        ImageUrl="/Imagenes/Iconos/16/img_block.png" UniqueName="BtnBloquear">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar la Cuenta"
                                                        ImageUrl="/Imagenes/Iconos/16/error.png" UniqueName="BtnDisociar">
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
