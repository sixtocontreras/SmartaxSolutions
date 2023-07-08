<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlConfConceptosProv.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Provision.CtrlConfConceptosProv" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CONFIGURACIÓN CONCEPTOS ICA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_conf_conceptos_prov_ica" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_conf_conceptos_prov_ica"
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
                                <asp:LinkButton ID="LnkEjecProvision" runat="server" CommandName="BtnEjecProvision" ToolTip="Ejecutar Provisión"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/pos.png"/> EJECUTAR PROVISIÓN</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_conf_conceptos_prov_ica" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_conf_conceptos_prov_ica">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cliente" DataSourceID="DatosCliente"
                                    HeaderText="Cliente" ListDataMember="DtClientes" ListTextField="nombre_cliente"
                                    ListValueField="id_estado" UniqueName="id_cliente" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="vigencia" HeaderText="Vigencia"
                                    UniqueName="vigencia" MaxLength="4">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="desc_concepto" HeaderText="Concepto"
                                    UniqueName="desc_concepto" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_conceptos_prov_ica" DataSourceID="DatosConcepto"
                                    HeaderText="Concepto" ListDataMember="DtConceptosProv" ListTextField="desc_concepto"
                                    ListValueField="id_conceptos_prov_ica" UniqueName="id_conceptos_prov_ica" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="cta_debito" HeaderText="Cuenta Débito"
                                    UniqueName="cta_debito" MaxLength="20" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cta_debito" DataSourceID="DatosDebito"
                                    HeaderText="Cuenta Débito" ListDataMember="DtPlanUnicoCuentas" ListTextField="codigo_cuenta"
                                    ListValueField="id_puc" UniqueName="id_puc_debito" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="cta_credito" HeaderText="Cuenta Crédito"
                                    UniqueName="cta_credito" MaxLength="20" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cta_credito" DataSourceID="DatosCredito"
                                    HeaderText="Cuenta Crédito" ListDataMember="DtPlanUnicoCuentas" ListTextField="codigo_cuenta"
                                    ListValueField="id_puc" UniqueName="id_puc_credito" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="centro_costo" HeaderText="Centro de Costo"
                                    UniqueName="centro_costo" MaxLength="20">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_renglon" HeaderText="Renglón"
                                    UniqueName="numero_renglon" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idformulario_configuracion" DataSourceID="DatosFormConfiguracion"
                                    HeaderText="Renglón" ListDataMember="DtFormConfiguracion" ListTextField="numero_renglon"
                                    ListValueField="idformulario_configuracion" UniqueName="idformulario_configuracion" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Activar/Inactivar del Comercio"
                                    UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png"
                                    HeaderTooltip="Esta opción le permitirá Activar o Inactivar el Comercio">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar Tipo Documento" Text="Eliminar" UniqueName="DeleteCommand">
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
