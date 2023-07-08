<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlDepartamentos.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.CtrlDepartamentos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR INFORMACIÓN DE DEPARTAMENTOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_dpto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_dpto"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_dpto" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_dpto">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_pais" HeaderText="País"
                                    UniqueName="nombre_pais" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_pais" DataSourceID="DatosPais"
                                    HeaderText="País" ListDataMember="DtPaises" ListTextField="nombre_pais"
                                    ListValueField="id_pais" UniqueName="id_pais" Visible="false">
                                </telerik:GridDropDownColumn>
                                
                                <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cod. Dane"
                                    UniqueName="codigo_dane" MaxLength="10">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Nombre Dpto"
                                    UniqueName="nombre_departamento" MaxLength="30">
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
                                <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar Departamento" Text="Eliminar" UniqueName="DeleteCommand">
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
