<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddCalendarioTributario.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAddCalendarioTributario" %>
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
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
        </telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"/>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"/>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999" colspan="3">
                            <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR CALENDARIO TRIBUTARIO DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodicidad Impuesto</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Periodo del Impuesto</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo impuesto">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbPeriodicidadImpuesto" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione la periodicidad" AutoPostBack="True" OnSelectedIndexChanged="CmbPeriodicidadImpuesto_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="CmbPeriodoImpuesto" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione la periodicidad">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style1" colspan="3">
                            <asp:Panel ID="Panel1" runat="server">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Fecha Limite</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">% de Descuento</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="4" ToolTip="Seleccione el año gravable">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <telerik:RadDatePicker ID="dtFechaLimite" Runat="server">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td align="center">
                                            <telerik:RadNumericTextBox ID="TxtPorcentajeDesc" runat="server" EmptyMessage="% Descuento" Font-Size="15pt" Height="30px" MaxLength="6" MaxValue="10000" MinValue="0" TabIndex="6" Width="100px">
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
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="7" ToolTip="Seleccione el estado">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="8" Text="Guardar" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="120px" />
                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="9" Text="Salir" ToolTip="Salir" Width="120px" />
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
