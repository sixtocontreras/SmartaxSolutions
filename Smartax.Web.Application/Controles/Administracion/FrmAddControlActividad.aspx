<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddControlActividad.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.FrmAddControlActividad" %>
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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                <tr>
                                    <td colspan="4" align="center" bgcolor="#999999">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">REGISTRAR O EDITAR ACTIVIDADES DEL ANALISTA</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Control Actividad</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:DropDownList ID="CmbTipoCtrlActividad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo ">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoCtrlActividad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="250px">Estado</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el impuesto">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el año gravable ">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                                        &nbsp;&nbsp;</td>
                                    <td colspan="2" align="center">
                                        <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="4" ToolTip="Seleccione el estado">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador10" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador10_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador10">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1" align="center" colspan="4">
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Código de la Actividad</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">% Cumplimiento</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad a Validar</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Del Sistema ?</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadTextBox ID="TxtCodActividad" runat="server" EmptyMessage="Código" Font-Size="15pt" Height="30px" MaxLength="10" TabIndex="5" Width="150px">
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="Validador11" runat="server" ControlToValidate="TxtCodActividad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador11_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador11">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadNumericTextBox ID="TxtPorcCumplimiento" runat="server" EmptyMessage="% Porcentaje" Font-Size="15pt" Height="30px" MaxLength="8" MaxValue="10000" MinValue="0" TabIndex="6" Value="1" Width="150px">
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat DecimalDigits="2" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="Validador12" runat="server" ControlToValidate="TxtPorcCumplimiento" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador12_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador12">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadNumericTextBox ID="TxtCantidadValidar" runat="server" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="5" MaxValue="10000" MinValue="0" TabIndex="6" Value="1" Width="120px">
                                                            <NegativeStyle Resize="None" />
                                                            <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="Validador13" runat="server" ControlToValidate="TxtCantidadValidar" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador13_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador13">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <asp:RadioButtonList ID="RbtDelSistema" runat="server" RepeatDirection="Horizontal" TabIndex="7">
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción o Nombre de la Tarea</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <telerik:RadTextBox ID="TxtDescripcionActividad" runat="server" EmptyMessage="Descripción Actividad" Font-Size="15pt" Height="60px" MaxLength="500" TabIndex="8" TextMode="MultiLine" Width="600px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="TxtDescripcionActividad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador7">
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
                                        <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="9" Text="Guardar" ToolTip="Click para ejecutar el proceso por Lote" ValidationGroup="ValidarDatos" Width="180px" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="10" Text="Salir" ToolTip="Salir" Width="180px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
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
