<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRoles.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlRoles" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRO DE ROLES O PERFILES DEL SISTEMA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="id_rol" EditMode="PopUp" Name="Grilla"
                            NoMasterRecordsText="No hay Registros para Mostrar">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <EditFormSettings CaptionDataField="id_rol"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_rol" EmptyDataText="" FilterControlWidth="40px"
                                    UniqueName="id_rol" HeaderText="Id" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_rol" EmptyDataText="" FilterControlWidth="160px"
                                    UniqueName="nombre_rol" HeaderText="Nombre" MaxLength="50">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="rol_sistema" HeaderText="Sistema"
                                    UniqueName="rol_sistema">
                                </telerik:GridCheckBoxColumn>
                                
                                <telerik:GridBoundColumn DataField="nombre_cliente" EmptyDataText=""
                                    UniqueName="nombre_cliente" HeaderText="Cliente" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" EmptyDataText=""
                                    UniqueName="codigo_estado" HeaderText="Estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_modificacion" EmptyDataText=""
                                    UniqueName="fecha_modificacion" HeaderText="F. Modificación" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" EmptyDataText=""
                                    UniqueName="fecha_registro" HeaderText="F. Registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Rol" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsinarPermisos" Text="Asignar Permisos al Rol"
                                    UniqueName="BtnAsinarPermisos" ImageUrl="/Imagenes/Iconos/16/user1_add.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddModulos" Text="Asignar Modulos al Rol"
                                    UniqueName="BtnAddModulos" ImageUrl="/Imagenes/Iconos/16/index_add.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddActividad" Text="Asignar Control de Actividades"
                                    UniqueName="BtnAddActividad" ImageUrl="/Imagenes/Iconos/16/check.png">
                                </telerik:GridButtonColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnListarUsuarios" Text="Listar usuarios asignados al rol"
                                    UniqueName="BtnListarUsuarios" ImageUrl="/Imagenes/Iconos/16/script_edit.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnListarMenus" Text="Listar permisos asignados al rol"
                                    UniqueName="BtnListarMenus" ImageUrl="/Imagenes/Iconos/16/users2.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar este Rol ....?"
                                    ConfirmTitle="Eliminar Rol" Text="Eliminar Rol">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEliminarUsuarios" Text="Eliminar Usuarios asignados al rol"
                                    UniqueName="BtnEliminarUsuarios" ImageUrl="/Imagenes/Iconos/16/img_delete_user.png"
                                    ConfirmText="¿ Se encuentra seguro de Eliminar todos los USUARIOS ASOCIADOS al ROL seleccionado ?"
                                    ConfirmTitle="Eliminar Todos los Usuarios" ConfirmDialogType="RadWindow">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEliminarMenus" Text="Eliminar Permisos asignados al rol"
                                    UniqueName="BtnEliminarMenus" ImageUrl="/Imagenes/Iconos/16/img_delete_list.png"
                                    ConfirmText="¿ Se encuentra seguro de Eliminar todas las OPCIONES DE MENU ASOCIADAS al ROL seleccionado ?"
                                    ConfirmTitle="Eliminar Todos los Permisos" ConfirmDialogType="RadWindow">
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
            <%--<tr>
                <td align="center">
                    <uc1:Aviso ID="Aviso1" runat="server" />
                </td>
            </tr>--%>
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
