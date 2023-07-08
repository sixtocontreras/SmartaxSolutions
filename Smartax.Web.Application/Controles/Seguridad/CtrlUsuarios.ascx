<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlUsuarios.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlUsuarios" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR INFORMACION DE USUARIOS INTERNOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView CommandItemDisplay="Top" EditMode="PopUp" DataKeyNames="id_usuario" Name="Grilla"
                            NoMasterRecordsText="No hay Registros para Mostrar">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <EditFormSettings CaptionDataField="id_usuario"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_usuario" EmptyDataText="" FilterControlWidth="40px"
                                    UniqueName="id_usuario" HeaderText="Id" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="identificacion_usuario" HeaderText="Identificacion" FilterControlWidth="60px"
                                    UniqueName="identificacion_usuario" DataType="System.Int32" NumericType="Number" MaxLength="20">
                                </telerik:GridNumericColumn>
                                <telerik:GridBoundColumn DataField="nombre_completo_usuario" EmptyDataText="" FilterControlWidth="160px"
                                    UniqueName="nombre_completo_usuario" HeaderText="Nombre Usuario" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_usuario" EmptyDataText=""
                                    UniqueName="nombre_usuario" HeaderText="Nombres" MaxLength="20" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="apellido_usuario" EmptyDataText=""
                                    HeaderText="Apellidos" UniqueName="apellido_usuario" MaxLength="30" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="login_usuario" EmptyDataText="" HeaderText="Login"
                                    UniqueName="login_usuario" MaxLength="30" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="direccion_usuario" HeaderText="Dirección"
                                    UniqueName="direccion_usuario" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="telefono_usuario" EmptyDataText="" HeaderText="Teléfono"
                                    UniqueName="telefono_usuario" MaxLength="20" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_usuario" EmptyDataText="" HeaderText="Email"
                                    UniqueName="email_usuario" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridCheckBoxColumn DataField="cambiar_clave" HeaderText="Cambiar Clave"
                                    UniqueName="cambiar_clave" Visible="false" ReadOnly="true">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="manejar_fuera_oficina" HeaderText="Por Fuera"
                                    UniqueName="manejar_fuera_oficina" Visible="false">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn DataField="ip_equipo_oficina" EmptyDataText="" HeaderText="IP"
                                    UniqueName="ip_equipo_oficina" MaxLength="20" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="medio_envio" EmptyDataText="" HeaderText="Envio"
                                    UniqueName="medio_envio" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idmedio_envio_token" DataSourceID="Datos"
                                    HeaderText="Envio" ListDataMember="DtMedioEnvio" ListTextField="medio_envio"
                                    ListValueField="idmedio_envio_token" UniqueName="idmedio_envio_token" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="nombre_rol" EmptyDataText="" HeaderText="Perfil"
                                    UniqueName="nombre_rol" ReadOnly="true" FilterControlWidth="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_rol" DataSourceID="Datos"
                                    HeaderText="Perfil" ListDataMember="DtRoles" ListTextField="nombre_rol"
                                    ListValueField="id_rol" UniqueName="id_rol" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="nombre_empresa" EmptyDataText="" HeaderText="Empresa"
                                    UniqueName="nombre_empresa" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_empresa" DataSourceID="DatosEmpresas"
                                    HeaderText="Empresa" ListDataMember="DtEmpresas" ListTextField="nombre_empresa"
                                    ListValueField="id_empresa" UniqueName="id_empresa" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" EmptyDataText="" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_registro" EmptyDataText="" HeaderText="F. Registro"
                                    UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_ult_ingreso" EmptyDataText="" HeaderText="F. Ult. Ingreso"
                                    UniqueName="fecha_ult_ingreso" ReadOnly="true" FilterControlWidth="80px">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver mas informacion del usuario"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                    HeaderTooltip="Esta opción le permitirá ver mas información">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar este Usuario ....?"
                                    ConfirmTitle="Eliminar Usuario" Text="Eliminar Usuario">
                                </telerik:GridButtonColumn>--%>
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
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="164px" Height="155px" />
        <h3>Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
