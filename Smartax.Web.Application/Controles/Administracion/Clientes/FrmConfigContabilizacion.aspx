<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfigContabilizacion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmConfigContabilizacion" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 1150px;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="HOJA DE CONFIGURACIÓN DE IMPUESTOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="17"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="idformulario_configuracion" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkAddLoad" runat="server" CommandName="BtnCargueMasivo" ToolTip="Realizar cargue masivo de configuración"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGUE MASIVO CONFIGURACIÓN</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idformulario_configuracion" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="idformulario_configuracion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridNumericColumn DataField="numero_renglon" HeaderText="No. Renglon" FilterControlWidth="70px"
                                                UniqueName="numero_renglon" DataType="System.Int32" NumericType="Number">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_renglon" HeaderText="Descripción"
                                                UniqueName="descripcion_renglon" FilterControlWidth="250px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridNumericColumn DataField="numero_orden" HeaderText="No. Orden" FilterControlWidth="70px"
                                                UniqueName="numero_orden" DataType="System.Int32" NumericType="Number">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridCheckBoxColumn DataField="renglon_calculado" HeaderText="Calculado"
                                                UniqueName="renglon_calculado">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridCheckBoxColumn DataField="renglon_contabilizar" HeaderText="Contabilizar"
                                                UniqueName="renglon_contabilizar">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado" ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddConfigurar" Text="Configurar calculo del campo"
                                                UniqueName="BtnAddConfigurar" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" />
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
        </div>
    </form>
</body>
</html>
