<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddConfiguracionAlerta.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmAddConfiguracionAlerta" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server"></telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                <tr>
                                    <td colspan="3" align="center" bgcolor="#999999">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">CONFIGURAR TAREAS PROGRAMADAS</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Tarea</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Envio</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado de la Tarea</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbTipoTarea" runat="server" Font-Size="13pt" ToolTip="Seleccione el tipo de tarea">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbTipoTarea" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="Validador3">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbTipoEnvio" runat="server" AutoPostBack="True" Font-Size="13pt" OnSelectedIndexChanged="CmbTipoEnvio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="CmbTipoEnvio" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="Validador7">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="13pt" ToolTip="Seleccione un estado">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbEstado" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="Validador4">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Fecha Inicio</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Días Seguimiento</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Fecha Fin</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <telerik:RadDatePicker ID="dtFechaInicio" runat="server">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td align="center">
                                        <telerik:RadNumericTextBox ID="TxtNumeroDias" runat="server" Enabled="False" MaxLength="2" Value="0" Width="80px">
                                        </telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="Validador8" runat="server" ControlToValidate="TxtNumeroDias" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador8_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="Validador8">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td align="center">
                                        <telerik:RadDatePicker ID="dtFechaFin" runat="server">
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Hora Inicio</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Intervalos (min)</asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="LblTitulo18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Hora Fin</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbInicio" runat="server" Font-Size="13pt">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbIntervalo" runat="server" Font-Size="13pt">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="CmbHoraFin" runat="server" Font-Size="13pt">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Label ID="LblTitulo19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Dias Ejecución de la Tarea Programada</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:CheckBoxList ID="ChkDiasEnvio" runat="server" Font-Size="13pt" RepeatDirection="Horizontal" TabIndex="11" Width="700px">
                                            <asp:ListItem Value="1">LUNES</asp:ListItem>
                                            <asp:ListItem Value="2">MARTES</asp:ListItem>
                                            <asp:ListItem Value="3">MIÉRCOLES</asp:ListItem>
                                            <asp:ListItem Value="4">JUEVES</asp:ListItem>
                                            <asp:ListItem Value="5">VIERNES</asp:ListItem>
                                            <asp:ListItem Value="6">SÁBADO</asp:ListItem>
                                            <asp:ListItem Value="7">DOMINGO</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="BtnGuardarDatos" runat="server" Font-Bold="True" Font-Size="13pt" Height="35px" OnClick="BtnGuardarDatos_Click" Text="Guardar Configuración" ValidationGroup="ValidarDatos" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="35px" OnClientClick="window.close()" TabIndex="27" Text="Salir" ToolTip="Salir" Width="120px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">&nbsp;</td>
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
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
