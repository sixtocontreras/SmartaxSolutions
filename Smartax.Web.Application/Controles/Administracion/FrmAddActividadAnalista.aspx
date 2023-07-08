<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddActividadAnalista.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.FrmAddActividadAnalista" %>
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
        .auto-style1 {
            height: 27px;
        }
    </style>
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
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Control Actividad</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="200px">Tipo de Impuesto</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="LblTipoCtrlActividad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="LblTipoImpuesto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Código de la Actividad</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción Actividad</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblAnioGravable" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblCodigoActividad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="LblDescripcionActividad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">
                                        <asp:Label ID="LblCantidad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad ( Si )</asp:Label>
                                    </td>
                                    <td align="center" class="auto-style1">
                                        <asp:Label ID="LblCantidad2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad ( No )</asp:Label>
                                    </td>
                                    <td align="center" colspan="2" class="auto-style1">
                                        <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="250px">Estado</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadNumericTextBox ID="TxtCantidad1" runat="server" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="8" MaxValue="10000" MinValue="0" TabIndex="5" Value="1" Width="150px">
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
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="TxtCantidad1" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <telerik:RadNumericTextBox ID="TxtCantidad2" runat="server" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="8" MaxValue="10000" MinValue="0" TabIndex="5" Value="1" Width="150px">
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
                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="TxtCantidad2" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="6" ToolTip="Seleccione el estado">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
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
                                        <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="8" Text="Guardar" ToolTip="Click para ejecutar el proceso por Lote" ValidationGroup="ValidarDatos" Width="180px" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="9" Text="Salir" ToolTip="Salir" Width="180px" />
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
