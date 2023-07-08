<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerDetalleCalendarioMunicipio.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal.FrmVerDetalleCalendarioMunicipio" %>
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" EnableAJAX="false" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="LISTADO DE MUNICIPIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                <MasterTableView CommandItemDisplay="Top" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkAddExport" runat="server" CommandName="BtnExportarExcel" ToolTip="Exportar información a excel"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/money_add.png"/> EXPORTAR INFORMACIÓN A EXCEL</asp:LinkButton>
                                    </CommandItemTemplate>
                                    <Columns>
                                       <%-- <telerik:GridBoundColumn DataField="id_registro" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" UniqueName="id_registro">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                            UniqueName="descripcion_formulario">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                            UniqueName="nombre_municipio" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable"
                                            UniqueName="anio_gravable" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="periodicidad_impuesto" HeaderText="Periodicidad"
                                            UniqueName="periodicidad_impuesto" FilterControlWidth="80px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_limite" HeaderText="Fecha Limite"
                                            UniqueName="fecha_limite" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_descuento" HeaderText="% Descuento"
                                            UniqueName="valor_descuento" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="descuento_ganado" HeaderText="Desc. Pronto Pago"
                                            UniqueName="descuento_ganado">
                                        </telerik:GridBoundColumn>--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
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
