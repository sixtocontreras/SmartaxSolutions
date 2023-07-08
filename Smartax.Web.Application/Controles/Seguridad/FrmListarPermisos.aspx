<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmListarPermisos.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmListarPermisos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" LoadingPanelID="RadAjaxLoadingPanel1">
                <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">LISTADO DE PERMISOS POR ROL</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    GridLines="None">
                                    <MasterTableView DataKeyNames="id_navegacion" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_navegacion" EmptyDataText=""
                                                UniqueName="id_navegacion" HeaderText="Menu Id">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="titulo_opcion" EmptyDataText=""
                                                UniqueName="titulo_opcion" HeaderText="Título">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_opcion" EmptyDataText=""
                                                UniqueName="descripcion_opcion" HeaderText="Descripción" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridCheckBoxColumn DataField="puede_leer" HeaderText="Leer"
                                                UniqueName="puede_leer">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_registrar" HeaderText="Escribir"
                                                UniqueName="puede_registrar">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_modificar" HeaderText="Modificar"
                                                UniqueName="puede_modificar">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_eliminar" HeaderText="Eliminar"
                                                UniqueName="puede_eliminar">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_bloquear" HeaderText="Bloquear"
                                                UniqueName="puede_bloquear">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_anular" HeaderText="Anular"
                                                UniqueName="puede_anular">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_exportar" HeaderText="Exportar"
                                                UniqueName="puede_exportar">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_liq_borrador" HeaderText="Liq. Borrador"
                                                UniqueName="puede_liq_borrador">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_liq_definitivo" HeaderText="Liq. Def."
                                                UniqueName="puede_liq_definitivo">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_ver_formulario" HeaderText="Ver Form."
                                                UniqueName="puede_ver_formulario">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="puede_configurar" HeaderText="Configurar"
                                                UniqueName="puede_configurar">
                                            </telerik:GridCheckBoxColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEliminarMenu" Text="Desasociar el usuario"
                                                UniqueName="BtnEliminarMenu" ImageUrl="/Imagenes/Iconos/16/error.png" 
                                                ConfirmText="¿ Se encuentra seguro de quitar el permiso al Rol seleccionado ?"
                                                ConfirmTitle="Desasociar Opción de Menu" ConfirmDialogType="RadWindow">
                                            </telerik:GridButtonColumn>
                                            <%--<telerik:GridButtonColumn CommandName="BtnEliminarMenu" Text="Desasociar Opción" UniqueName="BtnEliminarMenu"
                                                FooterText="Eliminar la referencia de la opción de Menú con este rol" ConfirmText="¿ Se encuentra seguro de Desasociar la Opción de Menú seleccionada del presente Rol ?"
                                                ConfirmTitle="Desasociar Opción de Menu" ConfirmDialogType="RadWindow">
                                            </telerik:GridButtonColumn>--%>

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClientClick="window.close()" Text="Salir" ToolTip="Click para salir" Width="150px" />
                            </td>
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
