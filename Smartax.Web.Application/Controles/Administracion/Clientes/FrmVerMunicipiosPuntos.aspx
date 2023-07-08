<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerMunicipiosPuntos.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmVerMunicipiosPuntos" %>
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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False">
                <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">CANTIDAD DE OFICINAS POR MUNICIPIO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" 
                                    OnNeedDataSource="RadGrid1_NeedDataSource" 
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="idmunicipio_cant_puntos, id_municipio" Name="Grilla" NoMasterRecordsText="No hay registros para mostrar">
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="BtnExportarExcel" ToolTip="Exportar información a excel"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/table_edit.png"/> EXPORTAR A EXCEL</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idmunicipio_cant_puntos" EmptyDataText="" FilterControlWidth="50px" HeaderText="Id" 
                                                UniqueName="idmunicipio_cant_puntos">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane" FilterControlWidth="50px" HeaderText="Cód. Dane" 
                                                UniqueName="codigo_dane">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" FilterControlWidth="120px" HeaderText="Municipio" 
                                                UniqueName="nombre_municipio">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="cantidad_puntos" FilterControlWidth="60px" HeaderText="Cantidad Oficinas" 
                                                UniqueName="cantidad_puntos">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_registro" FilterControlWidth="80px" HeaderText="F. Registro" 
                                                UniqueName="fecha_registro">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_actualizacion" FilterControlWidth="80px" HeaderText="F. Actualización" 
                                                UniqueName="fecha_actualizacion">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="BtnExportar" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" OnClick="BtnExportar_Click" Text="Exportar a Excel" ToolTip="Click para exportar estado financiero a excel" Width="170px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
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
