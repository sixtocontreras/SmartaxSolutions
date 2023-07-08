<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMunicipioImpuestos.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmMunicipioImpuestos" %>
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
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="LISTA DE IMPUESTOS ASIGNADOS AL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                OnDeleteCommand="RadGrid1_DeleteCommand">
                                <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idmun_impuesto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkAddImpuesto" runat="server" CommandName="BtnAddImpuesto" ToolTip="Agregar nuevo impuesto"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/money_add.png"/> AGREGAR NUEVO IMPUESTO</asp:LinkButton>
                                        <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                                        <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idmun_impuesto" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idmun_impuesto">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Formulario"
                                            UniqueName="descripcion_formulario" ReadOnly="true" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable"
                                            UniqueName="anio_gravable" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_periodicidad" HeaderText="Periodicidad"
                                            UniqueName="descripcion_periodicidad" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_impuesto" HeaderText="Descripción"
                                            UniqueName="descripcion_impuesto">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="descripcion_tarifa" HeaderText="Tipo Tarifa"
                                            UniqueName="descripcion_tarifa" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_tarifa" HeaderText="Valor Tarifa"
                                            UniqueName="valor_tarifa" ReadOnly="true" FilterControlWidth="70px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                            UniqueName="codigo_estado" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                            UniqueName="fecha_registro" ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" Text="Editar informacion"
                                            UniqueName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/edit.png">
                                        </telerik:GridButtonColumn>
                                        <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddConfigurar" Text="Adicionar nueva versión del registro"
                                            UniqueName="BtnAddConfigurar" ImageUrl="/Imagenes/Iconos/16/index_add.png">
                                        </telerik:GridButtonColumn>--%>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                            ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                            ConfirmTitle="Eliminar Impuesto" Text="Eliminar" UniqueName="DeleteCommand">
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
