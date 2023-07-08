<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmEjecucionPorLoteIca.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmEjecucionPorLoteIca" %>
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
.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}</style>
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
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">LIQUIDACION DE IMPUESTO DEL ICA POR LOTES</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Filtro</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbTipoFiltro" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo filtro" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoFiltro_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoFiltro" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">&nbsp;&nbsp;<asp:DropDownList ID="CmbTipoImpuesto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el formulario aplicar en el filtro">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="CmbMunicipio" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:DropDownList ID="CmbDepartamento" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbDepartamento_SelectedIndexChanged" TabIndex="2" ToolTip="Seleccione el departamento">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbDepartamento" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Municipio</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado de la Declaración a Ejecutar</asp:Label>
                                    </td>
                                    <td align="center" colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbMunicipio" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbMunicipio_SelectedIndexChanged" TabIndex="3" ToolTip="Seleccione el municipio">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbMunicipio" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbEstadoDeclaracion" runat="server" RepeatDirection="Horizontal" TabIndex="10" ToolTip="Es consorcio unión temporal ?" Enabled="False">
                                            <asp:ListItem Value="2">PRELIMINAR</asp:ListItem>
                                            <asp:ListItem Value="3">DEFINITIVO</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="RbEstadoDeclaracion" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center" colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Ingrese los Correos separados por Punto y Coma (;) donde llegara la Notificación</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <telerik:RadTextBox ID="TxtEmailsConfirmacion" runat="server" EmptyMessage="email1@dominio.com;email2@dominio.com.co;email3@dominio.net;email4@dominio.gov" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="3" TextMode="MultiLine" Width="600px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="TxtEmailsConfirmacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
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
                                        <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nota: </asp:Label>
                                        <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="11pt">Señor usuario tenga en cuenta que una vez se de click en el boton de EJECUTAR PROCESO este será ejecutado en Background una vez termine le será enviado un correo informando la terminación del proceso.</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="BtnEjecutar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnEjecutar_Click" Text="Ejecutar Proceso" ToolTip="Click para ejecutar el proceso por Lote" ValidationGroup="ValidarDatos" Width="180px" Height="40px" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="180px" Height="40px" />
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
