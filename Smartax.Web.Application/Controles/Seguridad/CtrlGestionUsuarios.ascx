<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlGestionUsuarios.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlGestionUsuarios" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CAMBIO DE CLAVE, ACTIVACION Y BLOQUEO DE USUARIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemDataBound="RadGrid1_ItemDataBound"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        GridLines="None">
                        <MasterTableView EditMode="PopUp" DataKeyNames="id_usuario, id_rol" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_usuario"
                                CaptionFormatString="Cambio de Clave: {0}">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_usuario" EmptyDataText="" FilterControlWidth="40px"
                                    HeaderText="Código" UniqueName="id_usuario" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="identificacion_usuario" EmptyDataText=""
                                    UniqueName="identificacion_usuario" HeaderText="Identificacion" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_completo_usuario" EmptyDataText="" FilterControlWidth="160px"
                                    UniqueName="nombre_completo_usuario" HeaderText="Nombre Usuario" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_usuario" EmptyDataText=""
                                    HeaderText="Nombres" UniqueName="nombre_usuario" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="apellido_usuario" EmptyDataText=""
                                    HeaderText="Apellidos" UniqueName="apellido_usuario" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="login_usuario" EmptyDataText="" HeaderText="Login"
                                    UniqueName="login_usuario">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_usuario" EmptyDataText=""
                                    HeaderText="Email" UniqueName="email_usuario">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_rol" EmptyDataText=""
                                    HeaderText="Perfil" UniqueName="nombre_rol" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="id_estado" EmptyDataText=""
                                    HeaderText="Estado" UniqueName="id_estado" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_estado" EmptyDataText=""
                                    HeaderText="Estado" UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="password_nuevo" EmptyDataText=""
                                    HeaderText="Nuevo Password" UniqueName="password_nuevo" MaxLength="16" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Realizar cambio de clave" 
                                    ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnCambioClave" Text="Realizar Cambio de Calve"
                                    UniqueName="BtnCambioClave" ImageUrl="/Imagenes/Iconos/16/key.png">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Bloqueo de Usuario"
                                    UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDesBloquear" Text="DesBloqueo de Usuario"
                                    UniqueName="BtnDesBloquear" ImageUrl="/Imagenes/Iconos/16/check.png">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" PopUpSettings-Width="500">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
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
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
        <h3>Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
