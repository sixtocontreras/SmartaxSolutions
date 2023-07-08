<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddPucImpuesto.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Tipos.FrmAddPucImpuesto" %>
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
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="ASIGNACIÓN DE IMPUESTO A CUENTA CONTABLE" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                OnInsertCommand="RadGrid1_InsertCommand"
                                OnUpdateCommand="RadGrid1_UpdateCommand"
                                OnDeleteCommand="RadGrid1_DeleteCommand">
                                <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idpuc_impuesto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <EditFormSettings CaptionDataField="idpuc_impuesto"
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
                                        <telerik:GridBoundColumn DataField="idpuc_impuesto" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idpuc_impuesto">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                            UniqueName="descripcion_formulario" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDropDownColumn DataField="idformulario_impuesto" DataSourceID="DtFormImpuesto"
                                            HeaderText="Impuesto" ListDataMember="DtFormularioImpuesto" ListTextField="descripcion_formulario"
                                            ListValueField="idformulario_impuesto" UniqueName="idformulario_impuesto" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                            UniqueName="codigo_estado" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                            HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                            ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                            UniqueName="fecha_registro" ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                            InsertText="Insertar" UpdateText="Actualizar">
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                            ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                            ConfirmTitle="Eliminar Valor Unidad" Text="Eliminar" UniqueName="DeleteCommand">
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
