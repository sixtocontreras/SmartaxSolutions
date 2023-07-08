<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerAcuerdosMunicipales.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmVerAcuerdosMunicipales" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False">
            <div>
                <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">VER ACUERDOS MUNICIPALES</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowMultiRowSelection="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="20" Width="100%">
                                    <MasterTableView ClientDataKeyNames="idacuerdo_municipal, idtipo_normatividad" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idacuerdo_municipal" EmptyDataText="" HeaderText="Id" UniqueName="idacuerdo_municipal">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_normatividad" FilterControlWidth="100px" HeaderText="Tipo Normatividad" UniqueName="tipo_normatividad">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_archivo" FilterControlWidth="180px" HeaderText="Nombre del Archivo" UniqueName="nombre_archivo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_acuerdo" FilterControlWidth="60px" HeaderText="Fecha Acuerdo" UniqueName="fecha_acuerdo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDownload" ImageUrl="/Imagenes/Iconos/16/img_load.png" Text="Descargar archivo del acuerdo" UniqueName="BtnDownload">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            </telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
            <div class="loading">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                <h3>Espere un momento por favor ...
                </h3>
            </div>
        </telerik:RadAjaxLoadingPanel>
        <%--<uc1:CtrlRadicacionCorrespondencia ID="CtrlRadicacionCorrespondencia1" runat="server" />--%>
    </form>
</body>
</html>
