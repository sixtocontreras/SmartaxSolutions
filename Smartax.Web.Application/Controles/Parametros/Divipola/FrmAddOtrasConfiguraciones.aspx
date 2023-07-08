<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddOtrasConfiguraciones.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAddOtrasConfiguraciones" %>
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
                        <td align="center" bgcolor="#999999" colspan="4">
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR CONCEPTOS COMPLEMENTARIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Unidad de Medida</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo impuesto" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center" colspan="2">
                            <asp:DropDownList ID="CmbUnidadMedida" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione la unidad de medida" AutoPostBack="True" OnSelectedIndexChanged="CmbUnidadMedida_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbUnidadMedida" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el año">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Panel ID="Panel1" runat="server">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Fiscal</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:CheckBox ID="ChkValorUnidad" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkValorUnidad_CheckedChanged" TabIndex="7" Text="Modificar Valor Unid." />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Tarifa</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblCantidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad Unidad</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblCantidad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Cantidad Periodos</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbAnioFiscal" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbAnioFiscal_SelectedIndexChanged" TabIndex="3" ToolTip="Seleccione el año">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador26" runat="server" ControlToValidate="CmbAnioFiscal" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador26_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador26">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <telerik:RadNumericTextBox ID="TxtValorUnidad" runat="server" AutoPostBack="True" EmptyMessage="Valor Unid" Font-Size="15pt" Height="30px" MaxLength="8" MaxValue="10000000" MinValue="0" OnTextChanged="TxtValorUnidad_TextChanged" TabIndex="5" Visible="False" Width="130px">
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
                                            <asp:Label ID="LblValorUnidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt">$ 0.0</asp:Label>
                                            <asp:RequiredFieldValidator ID="Validador31" runat="server" ControlToValidate="TxtValorUnidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador31_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador31">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbTipoTarifa" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoTarifa_SelectedIndexChanged" TabIndex="4" ToolTip="Seleccione el tipo de tarifa">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador25" runat="server" ControlToValidate="CmbTipoTarifa" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador25_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador25">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <telerik:RadNumericTextBox ID="TxtCantidadUnidad" runat="server" AutoPostBack="True" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="6" MaxValue="10000" MinValue="0" OnTextChanged="TxtCantidad_TextChanged" TabIndex="5" Width="100px">
                                                <NegativeStyle Resize="None" />
                                                <NumberFormat DecimalDigits="4" ZeroPattern="n" />
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator ID="Validador24" runat="server" ControlToValidate="TxtCantidadUnidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador24_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador24">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <telerik:RadNumericTextBox ID="TxtCantPeriodos" runat="server" AutoPostBack="True" DataType="System.Int32" EmptyMessage="Cantidad" Font-Size="15pt" Height="30px" MaxLength="6" MaxValue="10000" MinValue="0" OnTextChanged="TxtCantPeriodos_TextChanged" TabIndex="5" Width="100px">
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
                                            <asp:RequiredFieldValidator ID="Validador23" runat="server" ControlToValidate="TxtCantPeriodos" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador23_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador23">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-weight: 700" colspan="4">
                            <asp:Panel ID="Panel2" runat="server">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Valor Concepto</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Renglón del Form.</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción de la Configuración</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblValorTarifaMinima" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt">$ 0.0</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="6" ToolTip="Seleccione el estado">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador20" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador20_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador20">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbRenglonForm" runat="server" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario" AutoPostBack="True" OnSelectedIndexChanged="CmbRenglonForm_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Validador21" runat="server" ControlToValidate="CmbRenglonForm" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador21_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador21">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                        <td align="center">
                                            <telerik:RadTextBox ID="TxtDescripcion" runat="server" EmptyMessage="Descripción" Font-Size="15pt" Height="50px" MaxLength="1000" TabIndex="8" TextMode="MultiLine" Width="300px">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="Validador22" runat="server" ControlToValidate="TxtDescripcion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador22_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador22">
                                            </ajaxToolkit:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" rowspan="3">
                            <asp:CheckBox ID="ChkCalcular" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" OnCheckedChanged="ChkCalcular_CheckedChanged" TabIndex="7" Text="Para Calcular" Width="130px" />
                        </td>
                        <td align="center">
                            <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Número Renglón 1</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center">
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
                            <asp:DropDownList ID="CmbSimbolo" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario" AutoPostBack="True" OnSelectedIndexChanged="CmbSimbolo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador14" runat="server" ControlToValidate="CmbSimbolo" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador14_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador14">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglonForm2" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador15" runat="server" ControlToValidate="CmbRenglonForm2" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador15_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador15">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Panel ID="Panel3" runat="server">
                                <table style="width:100%;">
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
                                            <asp:DropDownList ID="CmbSimbolo2" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario" AutoPostBack="True" OnSelectedIndexChanged="CmbSimbolo2_SelectedIndexChanged">
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
                                            <asp:DropDownList ID="CmbSimbolo3" runat="server" Enabled="false" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el renglon del formulario" AutoPostBack="True" OnSelectedIndexChanged="CmbSimbolo3_SelectedIndexChanged">
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
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="9" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="10" Text="Salir" ToolTip="Salir" Width="120px" />
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
