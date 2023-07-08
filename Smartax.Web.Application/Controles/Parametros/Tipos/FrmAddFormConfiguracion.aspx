<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddFormConfiguracion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Tipos.FrmAddFormConfiguracion" %>

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
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CONFIGURAR POSICIÓN FORMULARIO DE IMPUESTO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="14"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnItemCreated="RadGrid1_ItemCreated"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                OnInsertCommand="RadGrid1_InsertCommand"
                                OnUpdateCommand="RadGrid1_UpdateCommand"
                                OnDeleteCommand="RadGrid1_DeleteCommand">
                                <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idformulario_configuracion" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <EditFormSettings CaptionDataField="idformulario_configuracion"
                                        CaptionFormatString="Editar Registro: {0}"
                                        InsertCaption="Agregar Nuevo Registro">
                                        <EditColumn UniqueName="EditCommandColumn1">
                                        </EditColumn>
                                        <FormTemplate>
                                        </FormTemplate>
                                        <PopUpSettings Modal="True" />
                                    </EditFormSettings>
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                        <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
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
                                        <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                            HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                            ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                            UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                            InsertText="Insertar" UpdateText="Actualizar">
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddConfigurar" Text="Configurar calculo del campo"
                                            UniqueName="BtnAddConfigurar" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                            ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                            ConfirmTitle="Eliminar Configuración" Text="Eliminar" UniqueName="DeleteCommand">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                    <EditFormSettings PopUpSettings-Modal="true">
                                        <EditColumn UniqueName="EditCommandColumn1">
                                        </EditColumn>
                                        <FormTemplate>
                                        </FormTemplate>
                                        <PopUpSettings Modal="True" />
                                    </EditFormSettings>
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
