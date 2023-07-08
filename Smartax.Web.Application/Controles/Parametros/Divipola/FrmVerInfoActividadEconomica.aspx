<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerInfoActividadEconomica.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmVerInfoActividadEconomica" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACION DETALLADA DE LA ACTIVIDAD</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="idmun_act_economica" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DEL MUNICIPIO" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Act. Economica: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "idmun_act_economica") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="Año: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "id_anio") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="tipo_actividad: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tipo_actividad") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Cod. Agrupación: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_agrupacion") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Cod. Actividad: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_actividad") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Actividad Economica: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "descripcion_actividad") %>
                                                                <br />
                                                                <asp:Label ID="Label11" runat="server" ForeColor="Black" Font-Bold="true" Text="Tipo Tarifa: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "descripcion_tarifa") %>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Text="Tarifa de Ley: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tarifa_ley") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="Tarifa del Municipio: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tarifa_municipio") %>
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Text="No. Acuerdo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "numero_acuerdo") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="No. Articulo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "numero_articulo") %>
                                                                <br />
                                                                <asp:Label ID="Label21" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Registro: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_registro") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnFuncionario" HeaderText="ESTADO" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Font-Bold="true" Text='<%# DataBinder.Eval(Container.DataItem, "codigo_estado") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <GroupingSettings CollapseAllTooltip="Collapse all groups" />
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true"></Scrolling>
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">&nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" ToolTip="Salir" />
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
        </div>
    </form>
</body>
</html>
