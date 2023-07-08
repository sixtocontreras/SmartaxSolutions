<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerInfoValidacion.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmVerInfoValidacion" %>
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
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">INFORMACION DETALLADA DE LA VALIDACIÓN</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                                    <MasterTableView DataKeyNames="idliquidacion_lote" Name="Grilla" NoMasterRecordsText="No hay información para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="INFORMACION DETALLADA DE LA VALIDACIÓN" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="400">
                                                        <tr>
                                                            <td width="100%" align="left">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="Id Liquidacion Lote: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "idliquidacion_lote") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="Tipo Impuesto: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "tipo_impuesto") %>
                                                                <br />
                                                                <asp:Label ID="Label4" runat="server" ForeColor="Black" Font-Bold="true" Text="Año Gravable: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "anio_gravable") %>
                                                                <br />
                                                                <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Bold="true" Text="Departamento: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_dpto") %>
                                                                <br />
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Black" Font-Bold="true" Text="Código Dane: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "codigo_dane") %>
                                                                <br />
                                                                <asp:Label ID="Label8" runat="server" ForeColor="Black" Font-Bold="true" Text="Municipio: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_municipio") %>
                                                                <br />
                                                                <asp:Label ID="Label11" runat="server" ForeColor="Black" Font-Bold="true" Text="Periodicidad: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "periodicidad_impuesto") %>
                                                                <br />
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Font-Bold="true" Text="Estado Liquidacion: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "estado_liquidacion") %>
                                                                <br />
                                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Font-Bold="true" Text="Revision Jefe: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "revision_jefe") %>
                                                                <br />
                                                                <asp:Label ID="Label5" runat="server" ForeColor="Black" Font-Bold="true" Text="Vencida: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "vencida") %>
                                                                <br />
                                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Limite: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_limite") %>
                                                                <br />
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Font-Bold="true" Text="Nombre Analista: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_analista") %>
                                                                <br />
                                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Font-Bold="true" Text="Email Analista: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "email_analista") %>
                                                                <br />
                                                                <asp:Label ID="Label14" runat="server" ForeColor="Black" Font-Bold="true" Text="Observacion: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "observacion_validacion") %>
                                                                <br />
                                                                <asp:Label ID="Label15" runat="server" ForeColor="Black" Font-Bold="true" Text="Usuario Valida: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "usuario_valida") %>
                                                                <br />
                                                                <asp:Label ID="Label16" runat="server" ForeColor="Black" Font-Bold="true" Text="Email Valida: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "email_valida") %>
                                                                <br />
                                                                <asp:Label ID="Label18" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Validacion: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_validacion") %>
                                                                <br />
                                                                <asp:Label ID="Label21" runat="server" ForeColor="Black" Font-Bold="true" Text="Fecha Registro: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "fecha_registro") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnFuncionario" HeaderText="APROBACION JEFE" FilterControlWidth="100px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Font-Bold="true" Text='<%# DataBinder.Eval(Container.DataItem, "aprobacion_jefe") %>'></asp:Label>
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
