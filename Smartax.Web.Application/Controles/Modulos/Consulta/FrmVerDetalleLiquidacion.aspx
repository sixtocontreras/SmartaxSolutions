﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerDetalleLiquidacion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.Consulta.FrmVerDetalleLiquidacion" %>
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
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACION DETALLADA DE LA LIQUIDACIÓN</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="idliquid_impuesto" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DE LA LIQUIDACIÓN" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Liquidación: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "idliquid_impuesto") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="Código Dane Dpto: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_dane_dpto") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombre Departamento: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_dpto") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Código Dane Municipio: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_dane_munic") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Municipio: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_municipio") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Impuesto: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "formulario_impuesto") %>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Text="Total a Pagar: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "total_pagar") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombre Firmante 1: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_firmante1") %>
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombre Firmante 2: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_firmante2") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="Usuario Borrador: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "usuario_borrador") %>
                                                                <br />
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Borrador: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_borrador") %>
                                                                <br />
                                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Font-Bold="true" Text="Usuario Definitivo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "usuario_definitivo") %>
                                                                <br />
                                                                <asp:Label ID="Label14" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Definitivo: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_definitivo") %>
                                                                <br />
                                                                <asp:Label ID="Label15" runat="server" ForeColor="Black" Font-Bold="true" Text="Usuario Anula: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "usuario_anula") %>
                                                                <br />
                                                                <asp:Label ID="Label21" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Anulación: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_anula") %>
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
