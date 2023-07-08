<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddMunicipioImpuestos.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAddMunicipioImpuestos" %>
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
    <style type="text/css">
.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}
        </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999" colspan="3">
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR IMPUESTOS DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el formulario de impuesto" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglonForm" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbRenglonForm_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el renglon del formulario">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbRenglonForm" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <telerik:RadTextBox ID="TxtDescripcion" runat="server" EmptyMessage="Descripción" Font-Size="15pt" Height="30px" MaxLength="120" TabIndex="3" Width="300px">
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="TxtDescripcion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador7">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Panel ID="Panel2" runat="server">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label45" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label46" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodicidad</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label47" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Tarifa</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label48" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor tarifa</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label49" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el año">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador35" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador35_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador35">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbPeriodicidad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el valor">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador31" runat="server" ControlToValidate="CmbPeriodicidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador31_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador31">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbTipoTarifa" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoTarifa_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el tipo tarifa">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador32" runat="server" ControlToValidate="CmbTipoTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador32_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador32">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <telerik:RadNumericTextBox ID="TxtValorTarifa" runat="server" EmptyMessage="Tarifa" Font-Size="15pt" Height="30px" MaxLength="6" MaxValue="10000" MinValue="0" TabIndex="12" Width="100px">
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
                                            <asp:RequiredFieldValidator ID="Validador33" runat="server" ControlToValidate="TxtValorTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador33_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador33">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el estado">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador34" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador34_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador34">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Panel ID="Panel1" runat="server">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center" rowspan="6">
                                            <asp:CheckBox ID="ChkCalcular" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkCalcular_CheckedChanged" TabIndex="7" Text="Para Calcular" Width="130px" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 1</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 2</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm1" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador13" runat="server" ControlToValidate="CmbRenglonForm1" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador13_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador13">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbSimbolo" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbSimbolo_SelectedIndexChanged" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador14" runat="server" ControlToValidate="CmbSimbolo" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador14_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador14">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:DropDownList ID="CmbRenglonForm2" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador15" runat="server" ControlToValidate="CmbRenglonForm2" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador15_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador15">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 3</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 4</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbSimbolo2" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbSimbolo2_SelectedIndexChanged" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm3" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador27" runat="server" ControlToValidate="CmbRenglonForm3" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador27_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador27">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbSimbolo3" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbSimbolo3_SelectedIndexChanged" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm4" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador28" runat="server" ControlToValidate="CmbRenglonForm4" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador28_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador28">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 5</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label44" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 6</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbSimbolo4" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbSimbolo4_SelectedIndexChanged" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm5" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador29" runat="server" ControlToValidate="CmbRenglonForm5" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador29_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador29">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbSimbolo5" runat="server" AutoPostBack="True" Enabled="false" Font-Size="15pt" OnSelectedIndexChanged="CmbSimbolo5_SelectedIndexChanged" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm6" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador30" runat="server" ControlToValidate="CmbRenglonForm6" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador30_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador30">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
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
