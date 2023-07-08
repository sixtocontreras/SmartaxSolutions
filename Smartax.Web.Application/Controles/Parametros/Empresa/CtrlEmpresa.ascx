<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlEmpresa.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Empresa.CtrlEmpresa" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR INFORMACION DE EMPRESAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="True" AllowFilteringByColumn="True" Visible="true"
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="20" Skin="Default"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnPreRender="RadGrid1_PreRender"
                        OnItemDataBound="RadGrid1_ItemDataBound"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id_empresa" EditMode="PopUp" Name="DtEmpresas" NoMasterRecordsText="No hay Registros para Mostrar">
                            <DetailTables>
                                <telerik:GridTableView runat="server" CommandItemDisplay="Top" DataKeyNames="idempresa_hija"
                                    EditMode="PopUp" Name="DtEmpresasHijas" BackColor="ActiveCaption" Width="100%"
                                    BorderColor="Brown" Caption="Registro de empresas hijas" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <EditFormSettings CaptionDataField="idempresa_hija"
                                        CaptionFormatString="Editar datos empresa: {0}"
                                        InsertCaption="Agregar nueva Empresa">
                                        <EditColumn UniqueName="EditCommandColumn1">
                                        </EditColumn>
                                    </EditFormSettings>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idempresa_hija" HeaderText="Código"
                                            UniqueName="idempresa_hija" ReadOnly="true" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nit_empresa" HeaderText="Nit"
                                            UniqueName="nit_empresa" EmptyDataText="">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_empresa" HeaderText="Nombre"
                                            UniqueName="nombre_empresa" EmptyDataText="">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="emblema_empresa" HeaderText="Emblema"
                                            UniqueName="emblema_empresa" EmptyDataText="" MaxLength="150" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="direccion_empresa" HeaderText="Dirección"
                                            UniqueName="direccion_empresa" EmptyDataText="">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="telefono_empresa" HeaderText="Telefono"
                                            UniqueName="telefono_empresa" EmptyDataText="" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="email_empresa" HeaderText="Email"
                                            UniqueName="email_empresa" EmptyDataText="" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridNumericColumn DataField="cant_empresas_registrar" HeaderText="Cantidad"
                                            UniqueName="cant_empresas_registrar" DataType="System.Int32" NumericType="Number" Visible="false">
                                        </telerik:GridNumericColumn>
                                        <telerik:GridCheckBoxColumn DataField="tipo_empresa" HeaderText="Tipo"
                                            UniqueName="tipo_empresa" ReadOnly="true" Visible="false">
                                        </telerik:GridCheckBoxColumn>
                                        <telerik:GridCheckBoxColumn DataField="empresa_unica" HeaderText="Unica"
                                            UniqueName="empresa_unica" Visible="false">
                                        </telerik:GridCheckBoxColumn>

                                        <telerik:GridDropDownColumn DataField="id_pais" DataSourceID="DatosPaises"
                                            HeaderText="Pais" ListDataMember="DtPaises" ListTextField="nombre_pais"
                                            ListValueField="id_pais" UniqueName="id_pais" Visible="false">
                                        </telerik:GridDropDownColumn>
                                        <telerik:GridDropDownColumn DataField="id_dpto" DataSourceID="Datos"
                                            HeaderText="Departamento" ListDataMember="DtDptos" ListTextField="nombre_departamento"
                                            ListValueField="id_dpto" UniqueName="id_dpto" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                            UniqueName="nombre_municipio" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDropDownColumn DataField="id_municipio" DataSourceID="DatosMun"
                                            HeaderText="Municipio" ListDataMember="DtMunicipios" ListTextField="nombre_municipio"
                                            ListValueField="id_municipio" UniqueName="id_municipio" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                            UniqueName="codigo_estado" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="Datos"
                                            HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                            ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                        </telerik:GridDropDownColumn>

                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                            UniqueName="fecha_registro" ReadOnly="true" Visible="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                            InsertText="Insertar" UpdateText="Actualizar">
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver mas informacion de la entidad"
                                            UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                            HeaderTooltip="Esta opción le permitirá ver mas información">
                                        </telerik:GridButtonColumn>
                                        <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddLogo" Text="Asignar Logo de la entidad"
                                            UniqueName="BtnAddLogo" ImageUrl="/Imagenes/Iconos/16/img_picture.png"
                                            HeaderTooltip="Esta opción le permitirá asignar el logo de la entidad">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddMovimiento" Text="Asignar cupo general"
                                            UniqueName="BtnAddMovimiento" ImageUrl="/Imagenes/Iconos/16/img_payment.png"
                                            HeaderTooltip="Esta opción le permitirá Asignar el cupo general">
                                        </telerik:GridButtonColumn>--%>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                            ConfirmDialogType="RadWindow"
                                            ConfirmText="¿Esta Seguro de Eliminar la Entidad Seleccionada de la Lista ....?"
                                            ConfirmTitle="Eliminar Entidad" Text="Eliminar Entidad">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <%--<EditColumn ButtonType="ImageButton"/>--%>
                                    </EditFormSettings>
                                </telerik:GridTableView>

                            </DetailTables>
                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>

                            <%--<MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_empresa" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">--%>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_empresa" HeaderText="Código"
                                    UniqueName="id_empresa" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nit_empresa" HeaderText="Nit"
                                    UniqueName="nit_empresa" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_empresa" HeaderText="Nombre"
                                    UniqueName="nombre_empresa" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="emblema_empresa" HeaderText="Emblema"
                                    UniqueName="emblema_empresa" EmptyDataText="" MaxLength="150" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="direccion_empresa" HeaderText="Dirección"
                                    UniqueName="direccion_empresa" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="telefono_empresa" HeaderText="Telefono"
                                    UniqueName="telefono_empresa" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_empresa" HeaderText="Email"
                                    UniqueName="email_empresa" EmptyDataText="" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="cant_empresas_registrar" HeaderText="Cantidad"
                                    UniqueName="cant_empresas_registrar" DataType="System.Int32" NumericType="Number" FilterControlWidth="50px">
                                </telerik:GridNumericColumn>
                                <telerik:GridCheckBoxColumn DataField="tipo_empresa" HeaderText="Tipo"
                                    UniqueName="tipo_empresa" ReadOnly="true" Visible="false">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="empresa_unica" HeaderText="Unica"
                                    UniqueName="empresa_unica" Visible="false">
                                </telerik:GridCheckBoxColumn>

                                <telerik:GridDropDownColumn DataField="id_pais" DataSourceID="DatosPaises"
                                    HeaderText="Pais" ListDataMember="DtPaises" ListTextField="nombre_pais"
                                    ListValueField="id_pais" UniqueName="id_pais" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridDropDownColumn DataField="id_dpto" DataSourceID="Datos"
                                    HeaderText="Departamento" ListDataMember="DtDptos" ListTextField="nombre_departamento"
                                    ListValueField="id_dpto" UniqueName="id_dpto" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Ciudad"
                                    UniqueName="nombre_municipio" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_municipio" DataSourceID="DatosCiudad"
                                    HeaderText="Ciudad" ListDataMember="DtMunicipios" ListTextField="nombre_municipio"
                                    ListValueField="id_municipio" UniqueName="id_municipio" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="Datos"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver mas informacion de la entidad"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                    HeaderTooltip="Esta opción le permitirá ver mas información">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddLogo" Text="Asignar Logo de la entidad"
                                    UniqueName="BtnAddLogo" ImageUrl="/Imagenes/Iconos/16/img_picture.png"
                                    HeaderTooltip="Esta opción le permitirá asignar el logo de la entidad">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddMovimiento" Text="Asignar cupo general"
                                    UniqueName="BtnAddMovimiento" ImageUrl="/Imagenes/Iconos/16/img_payment.png"
                                    HeaderTooltip="Esta opción le permitirá Asignar el cupo general">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Esta Seguro de Eliminar la Entidad Seleccionada de la Lista ....?"
                                    ConfirmTitle="Eliminar Entidad" Text="Eliminar Entidad">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true">
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
