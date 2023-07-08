<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerTarifasExcesivas.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal.FrmVerTarifasExcesivas" %>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False" HorizontalAlign="NotSet">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="TARIFAS DE LEY Y EXCESIVAS POR MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="25"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="idmun_act_economica" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkDownloadFile" runat="server" CommandName="BtnDownloadFile" ToolTip="Cargar nuevo estado financiero"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_load.png"/> DESCARGAR TARIFAS EXCESIVAS</asp:LinkButton>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="anio_actividad" EmptyDataText="" FilterControlWidth="50px"
                                            HeaderText="Año" UniqueName="anio_actividad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="formulario_impuesto" HeaderText="Form. Impuesto"
                                            UniqueName="formulario_impuesto" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Código Dane"
                                            UniqueName="codigo_dane" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                            UniqueName="nombre_municipio" FilterControlWidth="120px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tipo_actividad" HeaderText="Actividad"
                                            UniqueName="tipo_actividad" FilterControlWidth="120px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cod. Actividad"
                                            UniqueName="codigo_actividad" FilterControlWidth="120px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="tarifa_ley" HeaderText="Tarifa Ley"
                                            UniqueName="tarifa_ley" FilterControlWidth="80px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tarifa_municipio" HeaderText="Tarifa Municipio"
                                            UniqueName="tarifa_municipio" FilterControlWidth="80px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="numero_acuerdo" HeaderText="No. Acuerdo"
                                            UniqueName="numero_acuerdo" FilterControlWidth="150px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_articulo" HeaderText="No. Articulo"
                                            UniqueName="numero_articulo" FilterControlWidth="80px">
                                        </telerik:GridBoundColumn>
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
