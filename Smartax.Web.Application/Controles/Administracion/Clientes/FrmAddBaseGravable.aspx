<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddBaseGravable.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddBaseGravable" %>
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
    <style type="text/css">
        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

        </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="6">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">CONFIGURAR VALORES DE LA CUENTA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Digite el nombre de la Oficina</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblNombreFormulario" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:DropDownList ID="CmbAnioGravable" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbAnioGravable_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el año gravable del estado financiero">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbRenglonForm" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td align="center" colspan="2">
                                <asp:DropDownList ID="CmbRenglonForm" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbRenglonForm_SelectedIndexChanged" TabIndex="2" ToolTip="Seleccione el renglon del formulario">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbRenglonForm" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td align="center" colspan="2">
                                <telerik:RadAutoCompleteBox ID="TxtMunicipio" runat="server"
                                    DataTextField="Text" DataValueField="Value"
                                    DropDownHeight="240px" DropDownWidth="400px"
                                    EmptyMessage="Digite el nombre de la oficina"
                                    Font-Bold="False" Font-Size="11pt" InputType="Text" Enabled="false"
                                    OnTextChanged="TxtMunicipio_TextChanged" TabIndex="3" Width="340px">
                                </telerik:RadAutoCompleteBox>
                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtMunicipio" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Label ID="LblDescripcion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="100%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:CheckBox ID="ChkSaldoInicial" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkSaldoInicial_CheckedChanged" Text="Aplicar Saldo Inicial" />
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="ChkMovDebito" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkMovDebito_CheckedChanged" Text="Aplicar Mov. Débito" />
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="ChkMovCredito" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkMovCredito_CheckedChanged" Text="Aplicar Mov. Crédito" />
                            </td>
                            <td align="center" colspan="2">
                                <asp:CheckBox ID="ChkSaldoFinal" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkSaldoFinal_CheckedChanged" Text="Aplicar Saldo Final" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor Extracontable</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblSaldoInicial" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt">$ 0.0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblMovDebito" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt">$ 0.0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblMovCredito" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt">$ 0.0</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="LblSaldoFinal" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt">$ 0.0</asp:Label>
                            </td>
                            <td align="center">
                                <telerik:RadNumericTextBox ID="TxtValorExtracontable" runat="server" EmptyMessage="Valor" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="15" TabIndex="2" Width="170px" AutoPostBack="True" OnTextChanged="TxtValorExtracontable_TextChanged">
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
                            <td align="center">
                                <asp:Label ID="Label11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor Total</asp:Label>
                            </td>
                            <td align="center">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                            <td align="center" colspan="2">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblValorTotal" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="15pt">$ 0.0</asp:Label>
                            </td>
                            <td align="center">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                            <td align="center" colspan="2">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                            <td align="center" colspan="2">&nbsp;</td>
                            <td align="center">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
