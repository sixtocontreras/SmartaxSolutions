<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlInfoEntidad.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Formatos.CtrlInfoEntidad" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="PARAMETRIZACIÓN INFORMACIÓN ENTIDAD" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="True" AllowFilteringByColumn="True" Visible="true"
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="20" Skin="Default"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id_informacion_cliente_sfc" EditMode="PopUp" Name="DtEmpresas" NoMasterRecordsText="No hay Registros para Mostrar">

                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                             <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_informacion_cliente_sfc" HeaderText="Id"
                                    UniqueName="id_informacion_cliente_sfc" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cliente" DataSourceID="DatosCliente"
                                    HeaderText="Cliente" ListDataMember="DtClientes" ListTextField="nombre_cliente"
                                    ListValueField="id_estado" UniqueName="id_cliente" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="tipo_entidad" MaxLength="2" HeaderText="Tipo Entidad"
                                    UniqueName="tipo_entidad" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_entidad" MaxLength="6" HeaderText="Código Entidad"
                                    UniqueName="codigo_entidad" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_entidad" MaxLength="40" HeaderText="Nombre Entidad"
                                    UniqueName="nombre_entidad" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="palabra_clave" MaxLength="11" HeaderText="Palabra Clave"
                                    UniqueName="palabra_clave" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="area_informacion" MaxLength="2" HeaderText="Area Información"
                                    UniqueName="area_informacion" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_informe" MaxLength="2" HeaderText="Tipo Informe"
                                    UniqueName="tipo_informe" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_oficina" MaxLength="4" HeaderText="Codigo Oficina"
                                    UniqueName="codigo_oficina" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_moneda" MaxLength="1" HeaderText="Tipo Moneda"
                                    UniqueName="tipo_moneda" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_informacion" MaxLength="1" HeaderText="Tipo Información"
                                    UniqueName="tipo_informacion" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_fideicomiso_fondo" MaxLength="1" HeaderText="Tipo Fideicomiso Fondo"
                                    UniqueName="tipo_fideicomiso_fondo" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_fideicomiso_fondo" MaxLength="4" HeaderText="Codigo Fideicomiso Fondo"
                                    UniqueName="codigo_fideicomiso_fondo" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="numero_columnas_f321" MaxLength="4" HeaderText="Numero Columnas F-321"
                                    UniqueName="numero_columnas_f321" Visible="false">
                                </telerik:GridNumericColumn>
                                <telerik:GridBoundColumn DataField="unidad_captura_tn_f321" MaxLength="2"  HeaderText="Unidad captura Total Nacional F-321"
                                    UniqueName="unidad_captura_tn_f321" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="unidad_captura_f525" MaxLength="2" HeaderText="Unidad captura F-525"
                                    UniqueName="unidad_captura_f525">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="Datos"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Esta Seguro de Eliminar la Entidad Seleccionada de la Lista ....?"
                                    ConfirmTitle="Eliminar Entidad" Text="Eliminar Entidad">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id_informacion_cliente_sfc" CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
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
        </table>
    </asp:Panel>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
    <div class="loading">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
        <h3>Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>

