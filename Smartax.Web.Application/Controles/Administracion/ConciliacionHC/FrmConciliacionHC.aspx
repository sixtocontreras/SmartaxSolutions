<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConciliacionHC.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.ConciliacionHC.FrmConciliacionHC" %>
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
                                    <td align="center" bgcolor="#999999" colspan="4">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">PROCESO DE CONCILIACION HERRAMIENTA DE CUADRE</asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Aplicación</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Periodicidad</asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodo</asp:Label>
                                    </td>

                                </tr>
                                <tr>

                                    <td>
                                        <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el año Gravable">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CmbAplicativo" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo de aplicativo">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="CmbAplicativo" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CmbTipoPeriodicidad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el el tipo de periodicidad" AutoPostBack="True" OnSelectedIndexChanged="CmbTipoPeriodicidad_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validator3" runat="server" ControlToValidate="CmbTipoPeriodicidad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                        </cc1:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CmbPeriodo" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el mes">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="Validator4" runat="server" ControlToValidate="CmbPeriodo" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button ID="BtnEjecProceso" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnEjecProceso_Click" Text="Ejecutar Proceso" ToolTip="Click para ejecutar el proceso de conciliación" Width="250px" />
                                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="250px" />
                                        </td>
                                    </tr>
                            </table>
                        </asp:Panel>
                        <%--                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
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
