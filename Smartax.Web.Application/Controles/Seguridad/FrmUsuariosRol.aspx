<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUsuariosRol.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmUsuariosRol" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }

        .auto-style2 {
            height: 26px;
        }
    </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" class="auto-style2">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">LISTA DE USUARIOS ASIGNADOS AL PERFIL</asp:Label>
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
                                    <MasterTableView DataKeyNames="id_usuario" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_usuario" EmptyDataText=""
                                                UniqueName="id_usuario" HeaderText="ID">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="login_usuario" EmptyDataText=""
                                                UniqueName="login_usuario" HeaderText="Login Usuario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_usuario" EmptyDataText=""
                                                UniqueName="nombre_usuario" HeaderText="Nombre Usuario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_rol" EmptyDataText=""
                                                UniqueName="nombre_rol" HeaderText="Nombre Rol" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn CommandName="BtnEliminarUsuario" Text="Desasociar Usuario" UniqueName="BtnEliminarUsuario"
                                                FooterText="Eliminar la referencia del usuario con este rol" ConfirmText="¿ Se encuentra seguro de Desasociar el Usuario seleccionado del presente Rol ?"
                                                ConfirmTitle="Desasociar Usuario del Rol" HeaderTooltip="Utilize esta columna para Desasociar el usuario con el Presente rol"
                                                ConfirmDialogType="RadWindow">
                                            </telerik:GridButtonColumn>

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
