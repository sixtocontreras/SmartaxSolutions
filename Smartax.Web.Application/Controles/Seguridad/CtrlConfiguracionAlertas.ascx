<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlConfiguracionAlertas.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlConfiguracionAlertas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CONFIGURACIÓN DE TAREAS PROGRAMADAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="100%">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="id_configuracion, idtipo_tarea, idtipo_envio, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LnkConfiguracion" runat="server" CommandName="BtnAddConfiguracion" ToolTip="Realizar cargue masivo de Calendario"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_comportamiento.png"/> CREAR NUEVA CONFIGURACIÓN</asp:LinkButton>
                                <asp:LinkButton ID="LnkActualizar" runat="server" CommandName="BtnActualizarLista" ToolTip="Realizar cargue masivo de Actividades Economicas"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_configuracion" EmptyDataText="" FilterControlWidth="40px"
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_configuracion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_tarea" EmptyDataText=""
                                    UniqueName="tipo_tarea" HeaderText="Tipo Tarea" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="dias_seguimiento" EmptyDataText=""
                                    UniqueName="dias_seguimiento" HeaderText="Días Seguimiento" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_inicio" EmptyDataText=""
                                    UniqueName="fecha_inicio" HeaderText="Fecha Inicio">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="hora_inicio" EmptyDataText=""
                                    UniqueName="hora_inicio" HeaderText="Hora Inicio">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="intervalo" EmptyDataText=""
                                    UniqueName="intervalo" HeaderText="Intervalo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_fin" EmptyDataText=""
                                    UniqueName="fecha_fin" HeaderText="Fecha Fin">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="hora_fin" EmptyDataText=""
                                    UniqueName="hora_fin" HeaderText="Hora Fin">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_envio" EmptyDataText=""
                                    UniqueName="tipo_envio" HeaderText="Tipo Envío">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_estado" EmptyDataText=""
                                    UniqueName="codigo_estado" HeaderText="Estado">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" Text="Editar informacion de la configuracion"
                                    UniqueName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/style_edit.png"
                                    HeaderTooltip="Esta opción le permitirá editar información de la configuracion">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Activar/Inactivar la Configuracion"
                                    UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png"
                                    HeaderTooltip="Esta opción le permitirá Activar o Inactivar la Configuracion">
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
