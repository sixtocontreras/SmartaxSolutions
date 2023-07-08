<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddDescuentoProntoPago.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAddDescuentoProntoPago" %>
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
.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput{vertical-align:middle}
.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.RadInput textarea.riTextBox{height:auto}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}
        .RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput .riTextBox{height:17px}.RadInput textarea{vertical-align:bottom;overflow:auto;resize:none;white-space:pre-wrap}
        .auto-style1 {
            height: 27px;
        }
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
                        <td align="center" bgcolor="#999999" colspan="6">
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR DESCUENTO DE PRONTO PAGO POR IMPUESTO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="300px">Formulario</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                        </td>
                        <td align="center" colspan="3">
                            <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Descripción del Descuento según Resol. Municipio</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el tipo documento">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el valor">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="Validador2_ValidatorCalloutExtender" TargetControlID="Validador2">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center" colspan="3">
                            <telerik:RadTextBox ID="TxtDescripcion" runat="server" EmptyMessage="Descripción" Font-Size="15pt" Height="50px" MaxLength="1000" TabIndex="3" TextMode="MultiLine" Width="450px">
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtDescripcion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 1</asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 2</asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 3</asp:Label>
                        </td>
                        <td align="center" class="auto-style1">
                            <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon1" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="4" ToolTip="Seleccione el No. del renglon del formulario" OnSelectedIndexChanged="CmbRenglon1_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbRenglon1" Display="None" ErrorMessage="Debe seleccionar por lo menos este campo !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion1" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="5" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon2" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="6" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion2" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="7" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon3" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="8" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon3_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion3" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="9" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion3_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 4</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 5</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 6</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon4" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="10" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon4_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion4" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="11" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion4_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon5" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="12" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon5_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion5" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion5_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon6" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="14" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon6_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion6" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="15" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion6_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 7</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 8</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 9</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Operación</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon7" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="16" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon7_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion7" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="17" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion7_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon8" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="18" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon8_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion8" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="19" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion8_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon9" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="20" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon9_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoOperacion9" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="21" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbTipoOperacion9_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Renglón 10</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                        </td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbRenglon10" runat="server" AutoPostBack="True" Font-Size="15pt" TabIndex="22" ToolTip="Seleccione el No. del renglon del formulario" Enabled="False" OnSelectedIndexChanged="CmbRenglon10_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="23" ToolTip="Seleccion el estado">
                            </asp:DropDownList>
                        </td>
                        <td align="center">&nbsp;</td>
                        <td align="center" colspan="3">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="9" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="10" Text="Salir" ToolTip="Salir" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
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
        </div>
    </form>
</body>
</html>
