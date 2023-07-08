<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlConciliacionHC.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.ConciliacionHC.CtrlConciliacionHC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
        <asp:Panel ID="Panel1" runat="server">
            <table cellpadding="4" cellspacing="0" class="Tab" border="0" style="width: 100%;">
                <tr>
                    <td align="center" bgcolor="#999999" colspan="5">
                        <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="VER CONCILIACIONES HC" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="5">
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                            OnNeedDataSource="RadGrid1_NeedDataSource"
                            OnItemCommand="RadGrid1_ItemCommand"
                            OnPageIndexChanged="RadGrid1_PageIndexChanged">


                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="idconciliacion_hc" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                <CommandItemTemplate>
                                    <asp:LinkButton ID="LnkAddConciliacionHC" runat="server" CommandName="BtnAddConciliacionHC" ToolTip="Agregar nueva conciliación HC"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/user1_add.png"/> CREAR NUEVA CONCILIACION HC</asp:LinkButton>
                                    <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista de conciliaciones"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA CONCILIACIONES</asp:LinkButton>

                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="idconciliacion_hc" EmptyDataText="" HeaderText="Id"
                                        UniqueName="idconciliacion_hc" FilterControlWidth="40px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable"
                                        UniqueName="anio_gravable" FilterControlWidth="120px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="idtipo_aplicativo" HeaderText="Tipo Aplicativo"
                                        UniqueName="idtipo_aplicativo" FilterControlWidth="50px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mes_conciliacion" HeaderText="Mes Conciliación"
                                        UniqueName="mes_conciliacion" FilterControlWidth="120px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="id_estado" HeaderText="Estado"
                                        UniqueName="id_estado" FilterControlWidth="50px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="idusuario_add" HeaderText="Usuario Crea"
                                        UniqueName="idusuario_add" FilterControlWidth="100px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="idusuario_up" HeaderText="Usuario Actualiza"
                                        UniqueName="idusuario_up" FilterControlWidth="60px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha_modificacion" HeaderText="Fecha Modificación"
                                        UniqueName="fecha_modificacion" FilterControlWidth="60px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                        UniqueName="fecha_registro" FilterControlWidth="60px">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Activar/Inactivar"
                                        UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png">
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
