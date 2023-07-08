<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerInfoEntidad.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Empresa.FrmVerInfoEntidad" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <asp:Panel ID="PanelDatos" runat="server" Width="750px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">INFORMACION DETALLADA DE LA ENTIDAD</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Height="377px">
                                    <MasterTableView DataKeyNames="id_empresa" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DE LA ENTIDAD" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Entidad: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "id_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="No. Nit: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nit_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombre: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Emblema: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "emblema_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Dirección: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "direccion_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Teléfono: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "telefono_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Bold="true" Text="Email: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "email_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label11" runat="server" ForeColor="Black" Font-Bold="true" Text="Cantidad Registrar: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "cant_empresas_registrar") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="País: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_pais") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="Departamento: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_departamento") %>
                                                                <br />
                                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Font-Bold="true" Text="Ciudad: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_municipio") %>
                                                                <br />
                                                                <asp:Label ID="Label18" runat="server" ForeColor="Black" Font-Bold="true" Text="Tipo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tipo_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label16" runat="server" ForeColor="Black" Font-Bold="true" Text="Es Unica ?: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "empresa_unica") %>
                                                                <br />
                                                                <asp:Label ID="Label21" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Registro: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_registro") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnFuncionario" HeaderText="ESTADO" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="150">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Font-Bold="true" Text='<%# DataBinder.Eval(Container.DataItem, "codigo_estado") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
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
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
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
