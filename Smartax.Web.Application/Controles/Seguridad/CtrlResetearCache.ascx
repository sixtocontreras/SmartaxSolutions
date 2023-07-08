<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlResetearCache.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlResetearCache" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="RESETEAR DATOS DE CACHE DEL ROL" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        GridLines="None">
                        <MasterTableView DataKeyNames="id_rol" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_rol" EmptyDataText="" HeaderText="Código"
                                    UniqueName="id_rol">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_rol" EmptyDataText=""
                                    HeaderText="Nombre" UniqueName="nombre_rol">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn CommandName="BtnResetear" UniqueName="BtnResetear"
                                    ConfirmText="¿Esta Seguro de Eliminar la Cache del Rol seleccionado ..?"
                                    ConfirmTitle="Resetear Cache" Text="Resetear Cache">
                                </telerik:GridButtonColumn>
                            </Columns>
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
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
        <h3>Procesando solicitud ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
