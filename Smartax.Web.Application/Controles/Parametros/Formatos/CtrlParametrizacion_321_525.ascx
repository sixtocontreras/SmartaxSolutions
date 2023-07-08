<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlParametrizacion_321_525.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Formatos.CtrlParametrizacion_321_525" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="PARAMETRIZACIÓN F321 525" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
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
                        <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id_param_f321_f525" EditMode="PopUp" Name="DtEmpresas" NoMasterRecordsText="No hay Registros para Mostrar">

                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                             <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_param_f321_f525" HeaderText="Id"
                                    UniqueName="id_param_f321_f525" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cliente" DataSourceID="DatosCliente"
                                    HeaderText="Cliente" ListDataMember="DtClientes" ListTextField="nombre_cliente"
                                    ListValueField="id_estado" UniqueName="id_cliente" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridNumericColumn DataField="columna_321" MaxLength="4" HeaderText="Columna 321"
                                    UniqueName="columna_321" >
                                </telerik:GridNumericColumn>
                                <telerik:GridBoundColumn DataField="etiqueta" HeaderText="Etiqueta"
                                    UniqueName="etiqueta" EmptyDataText="" MaxLength="20">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="base_resultante" HeaderText="Base Resultante"
                                    UniqueName="base_resultante" EmptyDataText="" MaxLength="1">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="subcuenta_f525" HeaderText="Subcuenta F-525"
                                    UniqueName="subcuenta_f525" EmptyDataText=""  MaxLength="3">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cuenta_f525" HeaderText="Nombre Cuenta F-525"
                                    UniqueName="nombre_cuenta_f525" EmptyDataText="" MaxLength="120" >
                                </telerik:GridBoundColumn>
                               
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                 <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnConfiguracion" Text="Configuración de cuentas"
                                    UniqueName="BtnConfiguracion" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Esta Seguro de Eliminar el registro Seleccionado de la Lista ....?"
                                    ConfirmTitle="Eliminar Registro" Text="Eliminar Registro">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id_param_f321_f525" CaptionFormatString="Editar Registro: {0}"
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

