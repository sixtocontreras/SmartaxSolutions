<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRenglonConcepto.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.ReteICA.CtrlRenglonConcepto" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="RETEICA – CONFIGURACIÓN RENGLONES" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="false" AllowFilteringByColumn="false" 
                        Visible="true" AutoGenerateColumns="False" AllowPaging="false" PageSize="20" 
                        Skin="Default" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCreated="RadGrid1_ItemCreated" OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand"
                        OnItemDataBound="RadGrid1_ItemDataBound">

                        <MasterTableView runat="server" CommandItemDisplay="Top" EditMode="PopUp" 
                             NoMasterRecordsText="No hay Registros para Mostrar">
                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="renglon" DataType="System.Int32" HeaderText="Renglon"
                                    UniqueName="renglon">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="cuenta" HeaderText="Cuenta"
                                    UniqueName="cuenta"
                                    EmptyDataText="">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="nom_datoarchivo" ReadOnly="true" HeaderText="Dato a obtener" 
                                    UniqueName="nom_datoarchivo" >
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn DataField="datoarchivo" HeaderText="Dato a obtener" UniqueName="datoarchivo" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("datoarchivo") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="datoarchivoCombo" runat="server">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" ButtonType="ImageButton" EditText="Editar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>

                                <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ButtonType="ImageButton"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar" Text="Eliminar" UniqueName="DeleteCommand">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" InsertCaption="Agregar Nuevo Registro" CaptionFormatString="Editar Registro">
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