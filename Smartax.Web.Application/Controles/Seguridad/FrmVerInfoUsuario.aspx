<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerInfoUsuario.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmVerInfoUsuario" %>
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
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">INFORMACION DETALLADA DEL USUARIO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="id_usuario" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DEL USUARIO" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Usuario: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "id_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombres: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_usuario") %> <%# DataBinder.Eval(Container.DataItem, "apellido_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="Identificación: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "identificacion_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Text="Login: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "login_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Dirección: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "direccion_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Teléfono Fijo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "telefono_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Email: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "email_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Bold="true" Text="Cambiar Clave: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "cambiar_clave") %>
                                                                <br />
                                                                <asp:Label ID="Label11" runat="server" ForeColor="Black" Font-Bold="true" Text="Fuera Oficina: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "manejar_fuera_oficina") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="IP PC: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "ip_equipo_oficina") %>
                                                                <br />
                                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Font-Bold="true" Text="Rol o Perfil: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_rol") %>
                                                                <br />
                                                                <asp:Label ID="Label16" runat="server" ForeColor="Black" Font-Bold="true" Text="Empresa: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_empresa") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="Estado Usuario: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_estado") %>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Text="Observación Usuario: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "observacion_usuario") %>
                                                                <br />
                                                                <asp:Label ID="Label14" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Registro: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_registro") %>
                                                                <br />
                                                                <asp:Label ID="Label19" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Ult. Ingreso: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_ult_ingreso") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
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
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="true"></Scrolling>
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">&nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" />
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
