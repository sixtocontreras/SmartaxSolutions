<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlFichaTecnicaMunicipio.ascx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.Consulta.CtrlFichaTecnicaMunicipio" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
        <asp:Panel ID="Panel1" runat="server">
            <table cellpadding="4" cellspacing="0" class="Tab" border="0" style="width: 100%;">
                <tr>
                    <td align="center" bgcolor="#999999" colspan="5">
                        <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="VER FICHA TÉCNICA POR MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="5">
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None"
                            OnNeedDataSource="RadGrid1_NeedDataSource"
                            OnItemCommand="RadGrid1_ItemCommand"
                            OnPageIndexChanged="RadGrid1_PageIndexChanged">
                            <MasterTableView DataKeyNames="id_registro, id_municipio" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id_municipio" EmptyDataText="" HeaderText="Id"
                                        UniqueName="id_municipio" FilterControlWidth="40px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Departamento"
                                        UniqueName="nombre_departamento" FilterControlWidth="120px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cod. Dane"
                                        UniqueName="codigo_dane" FilterControlWidth="50px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                        UniqueName="nombre_municipio" FilterControlWidth="120px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo_oficina" HeaderText="Cod. Oficina"
                                        UniqueName="codigo_oficina" FilterControlWidth="50px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Oficina"
                                        UniqueName="nombre_oficina" FilterControlWidth="100px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="estado_oficina" HeaderText="Estado"
                                        UniqueName="estado_oficina" FilterControlWidth="60px">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver ficha técnica del municipio"
                                        UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/window_edit.png">
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
            <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
        </div>
    </telerik:RadAjaxLoadingPanel>
</body>
</html>
