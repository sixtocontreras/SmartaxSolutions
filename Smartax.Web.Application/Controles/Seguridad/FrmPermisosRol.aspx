<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPermisosRol.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmPermisosRol" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" HorizontalAlign="NotSet" Width="100%">
                <asp:Panel ID="PanelNavegacion" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;" border="1">
                        <tr>
                            <td rowspan="2">&nbsp;
                        <asp:Panel ID="PanelArbol" runat="server">
                            <h3>
                                <asp:Label ID="LblNavegacion" runat="server" CssClass="FormLabels">Menú de navegación</asp:Label>
                            </h3>
                            <telerik:RadTreeView ID="RadTreeView1" runat="server" OnNodeClick="RadTreeView1_NodeClick" Font-Bold="True" Font-Size="12pt" ForeColor="#0066FF">
                                <DataBindings>
                                    <telerik:RadTreeNodeBinding ToolTipField="descripcion_opcion" />
                                </DataBindings>
                            </telerik:RadTreeView>

                        </asp:Panel>
                            </td>
                            <td align="center">
                                <asp:Panel ID="PanelPermisos" runat="server" DefaultButton="BtnGuardar" Style="text-align: center" Width="100%">
                                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;" border="1">
                                        <tr>
                                            <td colspan="6" bgcolor="#999999">
                                                <h2 style="text-align: center">
                                                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR PERMISOS A ROLES DEL SISTEMA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                                                </h2>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkSeleccionarTodos" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkSeleccionarTodos_CheckedChanged" Text="Seleccionar todos" ToolTip="Marcar todos las opciones" />
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkQuitarTodos" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkQuitarTodos_CheckedChanged" Text="Quitar selección" ToolTip="Desmarcar todas las opciones" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Leer</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Registrar</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Modificar</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Eliminar</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Bloquear</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkLeer" runat="server" Font-Size="16pt" ToolTip="Permite visualizar interfaz de menu en el sistema" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEscribir" runat="server" Font-Size="16pt" ToolTip="Permite registrar informacion en el sistema" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkModificar" runat="server" Font-Size="16pt" ToolTip="Permite modificar información del sistema" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEliminar" runat="server" Font-Size="16pt" ToolTip="Permite borrar información del sistema" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkBloquear" runat="server" Font-Size="16pt" ToolTip="Permite bloquear registros en el sistema" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Anular</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Exportar</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Poder realizar movimientos de saldos Proveedores u Oficinas">Aplicar Configuración</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Poder realizar movimientos de saldos Proveedores u Oficinas">Liq. Borrador Impuestos</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Poder realizar movimientos de saldos Proveedores u Oficinas">Liq. Definitivo Impuestos</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkAnular" runat="server" Font-Size="16pt" ToolTip="Permite anular procesos realizados en el sistema" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkExportar" runat="server" Font-Size="16pt" ToolTip="Permite exportar información del sistema" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkConfigurar" runat="server" Font-Size="16pt" ToolTip="Permite realizar configuración de información" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkImpBorrador" runat="server" Font-Size="16pt" ToolTip="Permite realizar movimientos de saldos de proveedores u oficinas" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkImpDefinitivo" runat="server" Font-Size="16pt" ToolTip="Permite aprobar o rechazar consignaciones de comercios" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Poder realizar movimientos de saldos Proveedores u Oficinas">Ver Formularios</asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkVerFormulario" runat="server" Font-Size="16pt" ToolTip="Permite visualizar formularios de impuestos" />
                                            </td>
                                            <td>&nbsp;</td>
                                            <td colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td colspan="2">
                                                &nbsp;</td>
                                            <td>
                                                <asp:Label ID="LblRolID" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000" Visible="False"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <%-- <tr>
                                        <td colspan="5">
                                            <asp:Button ID="BtnAceptar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Aceptar" Width="120px" OnClick="BtnAceptar_Click" ToolTip="Click para guardar cambios del permiso" />
                                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" ToolTip="Click para salir" />
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar cambios del permiso" Width="150px" />
                                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClientClick="window.close()" Text="Salir" ToolTip="Click para salir" Width="150px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
