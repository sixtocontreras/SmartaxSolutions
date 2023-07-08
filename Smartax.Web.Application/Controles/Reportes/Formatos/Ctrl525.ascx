<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctrl525.ascx.cs" Inherits="Smartax.Web.Application.Controles.Reportes.Formatos.Ctrl525" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <form id="form1">
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%">
                <asp:Panel ID="PnlTrxGeneral" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="5" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="VALORES CONTRARIOS F 525" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cliente</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="CmbCliente" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione Cliente">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbCliente" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Vigencia</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadNumericTextBox ID="TxtVigencia" runat="server" EmptyMessage="Vigencia" Font-Size="15pt" Height="30px" MaxLength="4" TabIndex="2" Width="260px">
                                    <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="TxtVigencia" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                       
                                                <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodo</asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                        <telerik:RadTextBox ID="TxtPeriodoImpuesto" runat="server" EmptyMessage="Periodo Impuesto" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="3" Width="80px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtPeriodoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="BtnGenerar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Generar" ToolTip="Click para crear el reporte" Width="120px" OnClick="BtnGenerar_Click" ValidationGroup="ValidarDatos" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    </telerik:RadWindowManager>
                </div>
            </telerik:RadAjaxPanel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
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
