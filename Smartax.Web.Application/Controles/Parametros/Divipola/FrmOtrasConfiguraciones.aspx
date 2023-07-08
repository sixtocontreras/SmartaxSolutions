<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmOtrasConfiguraciones.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmOtrasConfiguraciones" %>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="OTRAS CONFIGURACIONES DE IMPUESTOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idmun_tarifa_minima" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkAddTarifa" runat="server" CommandName="BtnOtrasConfig" ToolTip="Ingresar nueva Configuración"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/money_add.png"/> NUEVA CONFIGURACIÓN</asp:LinkButton>
                                        <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA TARIFAS</asp:LinkButton>
                                        <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idmun_tarifa_minima" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idmun_tarifa_minima">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Formulario"
                                            UniqueName="descripcion_formulario" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="anio_tarifa" HeaderText="Año Fiscal"
                                            UniqueName="anio_tarifa" ReadOnly="true" FilterControlWidth="40px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable"
                                            UniqueName="anio_gravable" ReadOnly="true" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_configuracion" HeaderText="Descripción"
                                            UniqueName="descripcion_configuracion" ReadOnly="true" FilterControlWidth="250px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="unidad_medida" HeaderText="Medida"
                                            UniqueName="unidad_medida" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_renglon" HeaderText="Renglon Princ."
                                            UniqueName="numero_renglon" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad_medida" HeaderText="Cantidad"
                                            UniqueName="cantidad_medida" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_unidad" HeaderText="Valor Unid."
                                            UniqueName="valor_unidad" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tarifa_minima" HeaderText="Tarifa Mínima"
                                            UniqueName="tarifa_minima" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_renglon1" HeaderText="No. Renglon"
                                            UniqueName="numero_renglon1" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="simbolo1" HeaderText="Operación"
                                            UniqueName="simbolo1" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_renglon2" HeaderText="No. Renglon"
                                            UniqueName="numero_renglon2" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                            UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                            UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" Text="Editar informacion"
                                            UniqueName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/edit.png">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"  Text="Eliminar"
                                            ConfirmDialogType="RadWindow" ConfirmText="¿Se Encuentra Seguro de eliminar el registro Seleccionado ...!"
                                            ConfirmTitle="Eliminar Otra Configuración" UniqueName="DeleteCommand">
                                        </telerik:GridButtonColumn>
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
