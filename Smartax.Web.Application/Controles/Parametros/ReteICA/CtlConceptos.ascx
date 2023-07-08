<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtlConceptos.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.ReteICA.CtlConceptos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="RETEICA – ASOCIACIÓN DE CUENTAS CONTABLES A MODULOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="false" AllowFilteringByColumn="false" 
                        Visible="true" AutoGenerateColumns="False" AllowPaging="false" PageSize="20" 
                        Skin="Default" GridLines="None" OnItemCreated="RadGrid1_ItemCreated" 
                        OnNeedDataSource="RadGrid1_NeedDataSource" OnUpdateCommand="RadGrid1_UpdateCommand">

                        <MasterTableView runat="server" DataKeyNames="id" EditMode="PopUp" 
                            Name="DtConsumos" NoMasterRecordsText="No hay Registros para Mostrar" >
                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn Visible="false" DataField="id" HeaderText="Id"
                                    UniqueName="id" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="concepto" HeaderText="Concepto"
                                    UniqueName="concepto"
                                    EmptyDataText="">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="cuenta" HeaderText="Cuenta"
                                    UniqueName="cuenta" EmptyDataText="">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id" CaptionFormatString="Editar Registro: {0}">
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
