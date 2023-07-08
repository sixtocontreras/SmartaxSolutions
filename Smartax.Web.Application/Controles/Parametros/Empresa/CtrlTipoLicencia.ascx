<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlTipoLicencia.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Empresa.CtrlTipoLicencia" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR TIPOS DE LICENCIAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idtipo_licencia" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <EditFormSettings CaptionDataField="idtipo_licencia"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="idtipo_licencia" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="idtipo_licencia">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_licencia" HeaderText="Descripción"
                                    UniqueName="tipo_licencia" MaxLength="60">
                                </telerik:GridBoundColumn>

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

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteCommand" ConfirmDialogType="RadWindow"
                                    ConfirmText="Esta Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar Tipo Licencia" Text="Eliminar">
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
