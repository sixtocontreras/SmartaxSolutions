<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddCliente.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddCliente" %>
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
            height: 35px;
        }

        .auto-style2 {
            width: 13px;
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
                                    <td align="center" bgcolor="#999999" colspan="5">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">REGISTRAR INFORMACIÓN DE CLIENTES</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Documento</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. Documento</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="60px">DV</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nombre o Razón Social</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbTipoIdentificacion" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo documento">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoIdentificacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center" colspan="2">
                                        <telerik:RadNumericTextBox ID="TxtNumDocumento" runat="server" AutoPostBack="True" EmptyMessage="Valor" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" OnTextChanged="TxtNumDocumento_TextChanged" TabIndex="2" Width="150px">
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
                                        &nbsp;<asp:Label ID="LblSigno" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">-</asp:Label>
                                        &nbsp;<asp:Label ID="LblDv" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt"></asp:Label>
                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="TxtNumDocumento" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center" colspan="2">
                                        <telerik:RadTextBox ID="TxtRazonSocial" runat="server" EmptyMessage="Razón social" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="3" Width="300px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="TxtRazonSocial" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Distrito / Municipio</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Dirección de Notificación</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="LblUbicacionDpto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="LblUbicacionPrincipal" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                <asp:ImageButton ID="BtnAddUbicacion" runat="server" Height="20px" ImageUrl="~/Imagenes/Iconos/16/earth.png" OnClick="BtnAddUbicacion_Click" Width="20px" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <telerik:RadTextBox ID="TxtDireccion" runat="server" EmptyMessage="Dirección" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="5" Width="300px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="TxtDireccion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nombre Contacto</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Teléfono</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Email</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadTextBox ID="TxtContacto" runat="server" EmptyMessage="Contacto" Font-Size="15pt" Height="30px" MaxLength="100" TabIndex="6" Width="260px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="TxtContacto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td colspan="2">
                                        <telerik:RadTextBox ID="TxtTelefono" runat="server" EmptyMessage="Teléfono" Font-Size="15pt" Height="30px" MaxLength="15" TabIndex="7" Width="180px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="TxtTelefono" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador7">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td colspan="2">
                                        <telerik:RadTextBox ID="TxtEmail" runat="server" EmptyMessage="Email" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="8" Width="300px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="Validador8" runat="server" ControlToValidate="TxtEmail" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador8_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador8">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Presencia Nacional</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Consorcio Unión Temporal</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Inscripción en Rit</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Actividad Patrim Aut.</asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbPresenciaNacional" runat="server" RepeatDirection="Horizontal" TabIndex="9" ToolTip="Tiene presencia nacional ?">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:RadioButtonList ID="RbConsorcioUnionTemp" runat="server" RepeatDirection="Horizontal" TabIndex="10" ToolTip="Es consorcio unión temporal ?">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbInscripcionRit" runat="server" RepeatDirection="Horizontal" TabIndex="4" ToolTip="Se encuentra inscripto en el Rit ?">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="RbInscripcionRit" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbActividadPatrimAut" runat="server" RepeatDirection="Horizontal" TabIndex="11" ToolTip="Tiene actividad patrimonio autonomo ?">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Gran Contribuyente</asp:Label>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Sector</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Clasificación</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">No. de Establecimientos</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RadioButtonList ID="RbGranContribuyente" runat="server" RepeatDirection="Horizontal" TabIndex="9" ToolTip="Tiene presencia nacional ?">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:DropDownList ID="CmbTipoSector" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el estado">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador13" runat="server" ControlToValidate="CmbTipoSector" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador13_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador13">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbTipoClasificacion" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el tipo de clasificacion">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador14" runat="server" ControlToValidate="CmbTipoClasificacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador14_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador14">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <telerik:RadNumericTextBox ID="TxtNumPuntos" runat="server" EmptyMessage="No. Puntos" Font-Size="15pt" Height="30px" MaxLength="8" MaxValue="10000" MinValue="1" TabIndex="12" Value="1" Width="100px">
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
                                        <asp:RequiredFieldValidator ID="Validador16" runat="server" ControlToValidate="TxtNumPuntos" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador16_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador16">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el estado">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador12" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador12_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador12">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="2">&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </telerik:RadAjaxPanel>

            <asp:HiddenField ID="HiddenField1" runat="server" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                PopupControlID="PnlMunicipios"
                TargetControlID="HiddenField1"
                CancelControlID="BtnSalir2"
                BackgroundCssClass="backgroundColor">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="PnlMunicipios" runat="server" Width="850px" HorizontalAlign="Center" BackColor="#E6E6E6">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div id="popupcontainer" style="width: 850px">
                            <asp:Panel ID="Panel4" runat="server" Width="850px" BorderWidth="1">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="5" align="center" bgcolor="#999999">
                                            <asp:Label ID="LblTitulo2" runat="server" Font-Bold="True" Font-Size="16pt" CssClass="SubTitle" ForeColor="White" Text="SELECCIONE UBICACION PRINCIPAL"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Código Dane</asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Municipio</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="CmbDepartamento" runat="server" Font-Size="15pt" TabIndex="13" ToolTip="Seleccione el departamento">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <telerik:RadTextBox ID="TxtCodDane" runat="server" EmptyMessage="Código" Font-Size="15pt" Height="30px" MaxLength="8" TabIndex="7" Width="140px">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="TxtNomMunicipio" runat="server" EmptyMessage="Nombre" Font-Size="15pt" Height="30px" MaxLength="60" TabIndex="7" Width="180px">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="BtnConsultar" runat="server" ImageUrl="~/Imagenes/Iconos/32/img_search.png" OnClick="BtnConsultar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Culture="es-ES" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                                <MasterTableView DataKeyNames="id_municipio" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="id_municipio" EmptyDataText="" HeaderText="Id" ReadOnly="True" UniqueName="id_municipio">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Departamento" UniqueName="nombre_departamento">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="codigo_dane_mun" HeaderText="Cod. Dane" UniqueName="codigo_dane_mun">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio" UniqueName="nombre_municipio">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnSeleccion" ImageUrl="/Imagenes/Iconos/16/check.png" Text="Seleccionar municipio" UniqueName="BtnSeleccion">
                                                        </telerik:GridButtonColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="LblMsg" runat="server" Font-Bold="False" Font-Size="12pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Button ID="BtnSalir2" runat="server" Font-Bold="True" Font-Size="14pt" Text="Salir" Width="120px" OnClick="BtnSalir2_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td colspan="3">&nbsp;</td>
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
