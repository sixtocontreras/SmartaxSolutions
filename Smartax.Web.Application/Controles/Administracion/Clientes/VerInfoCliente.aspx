<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerInfoCliente.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.VerInfoCliente" %>
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
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACION DETALLADA DEL CLIENTE</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="id_cliente" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DEL CLIENTE" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Cliente: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "id_cliente") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="Tipo Documento: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tipo_identificacion") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="No. Documento: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "numero_documento") %>-<%# DataBinder.Eval(Container.DataItem, "digito_verificacion") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Ubicación Principal: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_municipio") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Razón Social: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "razon_social") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Inscrito en el Rit: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "inscrito_rit") %>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Text="Dirección: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "direccion_cliente") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="Contacto: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_contacto") %>
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Text="Teléfono: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "telefono_contacto") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="Email: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "email_contacto") %>
                                                                <br />
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Bold="true" Text="No. de Establecimientos: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "numero_puntos") %>
                                                                <br />
                                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Font-Bold="true" Text="Tiene Presencia Nacional: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tiene_presencia_nacional") %>
                                                                <br />
                                                                <asp:Label ID="Label14" runat="server" ForeColor="Black" Font-Bold="true" Text="Consorcio Unión Temporal: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "consorcio_union_temporal") %>
                                                                <br />
                                                                <asp:Label ID="Label15" runat="server" ForeColor="Black" Font-Bold="true" Text="Actividad Patrimonio Autónomo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "actividad_patrim_autonomo") %>
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
