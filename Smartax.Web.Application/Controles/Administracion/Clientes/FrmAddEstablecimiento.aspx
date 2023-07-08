<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddEstablecimiento.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddEstablecimiento" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="JavaScript">
        var msg = "¡El botón derecho está desactivado para este sitio !";
        function disableIE() {
            if (document.all) {
                //alert(msg);
                return false;
            }
        }

        function disableNS(e) {
            if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2)) {
                //alert(msg);
                return false;
            } else if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2)) {
                //alert(msg);
                return false;
            }

            //alert('El Navegador es: ' + navigator.appName);
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = disableNS;
        } else {
            document.onmouseup = disableNS;
            document.oncontextmenu = disableIE;
        }
        document.oncontextmenu = new Function("return false")
        //document.oncontextmenu = new Function("alert(msg);return false")
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR ESTABLECIMIENTOS PRINCIPALES" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                    OnItemCreated="RadGrid1_ItemCreated"
                                    OnInsertCommand="RadGrid1_InsertCommand"
                                    OnUpdateCommand="RadGrid1_UpdateCommand"
                                    OnDeleteCommand="RadGrid1_DeleteCommand">
                                    <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idcliente_establecimiento, id_municipio" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <EditFormSettings CaptionDataField="idcliente_establecimiento"
                                            CaptionFormatString="Editar Registro: {0}"
                                            InsertCaption="Agregar Nuevo Registro">
                                            <EditColumn UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                            <FormTemplate>
                                            </FormTemplate>
                                            <PopUpSettings Modal="True" />
                                        </EditFormSettings>
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                            <asp:LinkButton ID="LnkAddLoad1" runat="server" CommandName="BtnLoadOficinas" ToolTip="Realizar cargue masivo de establecimientos"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGUE MASIVO DE OFICINAS</asp:LinkButton>
                                            <asp:LinkButton ID="LnkAddLoad2" runat="server" CommandName="BtnLoadActividades" ToolTip="Realizar cargue masivo de actividades"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGUE MASIVO ACT. ECONOMICAS</asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="BtnCalcularPuntos" ToolTip="Calcular Cantidad de Puntos x Municipios"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/media_play.png"/> CALCULAR CANTIDAD OFICINAS</asp:LinkButton>
                                            <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idcliente_establecimiento" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="idcliente_establecimiento">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Departamento"
                                                UniqueName="nombre_departamento" FilterControlWidth="80px" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cód. Dane"
                                                UniqueName="codigo_dane" FilterControlWidth="50px" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                                UniqueName="nombre_municipio" FilterControlWidth="80px" ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="codigo_oficina" HeaderText="Cód. Oficina"
                                                UniqueName="codigo_oficina" MaxLength="10">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Oficina"
                                                UniqueName="nombre_oficina" MaxLength="60">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="nombre_contacto" HeaderText="Contacto"
                                                UniqueName="nombre_contacto" MaxLength="60" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="direccion_contacto" HeaderText="Dirección"
                                                UniqueName="direccion_contacto" MaxLength="100" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="Teléfono"
                                                UniqueName="telefono_contacto" MaxLength="30" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn DataField="inscrito_rit" HeaderText="Inscrito Rit"
                                                UniqueName="inscrito_rit" Visible="false">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridNumericColumn DataField="numero_puntos" HeaderText="No. Establecimientos" FilterControlWidth="70px"
                                                UniqueName="numero_puntos" DataType="System.Int32" NumericType="Number">
                                            </telerik:GridNumericColumn>

                                            <telerik:GridBoundColumn DataField="numero_placa_municipal" HeaderText="No. Placa Municipal"
                                                UniqueName="numero_placa_municipal" MaxLength="20" Visible="false" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="numero_matricula_ic" HeaderText="No. Matricula IC"
                                                UniqueName="numero_matricula_ic" MaxLength="20" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="numero_rit" HeaderText="No. RIT"
                                                UniqueName="numero_rit" MaxLength="20" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn DataField="avisos_tablero" HeaderText="Avisos y Tableros"
                                                UniqueName="avisos_tablero">
                                            </telerik:GridCheckBoxColumn>
                                            <telerik:GridDateTimeColumn DataField="fecha_inicio_actividades" HeaderText="Fecha Inicio Actividad"
                                                UniqueName="fecha_inicio_actividades" Visible="false">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="sucursal" HeaderText="Sucursal"
                                                UniqueName="sucursal" MaxLength="10" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn DataField="oficina_pagadora" HeaderText="Ofic. Pagadora"
                                                UniqueName="oficina_pagadora"  FilterControlWidth="60px">
                                            </telerik:GridCheckBoxColumn>

                                            <telerik:GridDropDownColumn DataField="id_dpto" DataSourceID="Datos"
                                                HeaderText="Departamento" ListDataMember="DtDptos" ListTextField="nombre_departamento"
                                                ListValueField="id_dpto" UniqueName="id_dpto" Visible="false">
                                            </telerik:GridDropDownColumn>
                                            <telerik:GridDropDownColumn DataField="id_municipio" DataSourceID="DatosMun"
                                                HeaderText="Municipio" ListDataMember="DtMunicipios" ListTextField="nombre_municipio"
                                                ListValueField="id_municipio" UniqueName="id_municipio" Visible="false">
                                            </telerik:GridDropDownColumn>

                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                                HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                                ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                            </telerik:GridDropDownColumn>

                                            <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                                UniqueName="fecha_registro" ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                                InsertText="Insertar" UpdateText="Actualizar">
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddActEconomicas" Text="Agregar actividades economicas"
                                                UniqueName="BtnAddActEconomicas" ImageUrl="/Imagenes/Iconos/16/contract.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddEstablecimiento" Text="Agregar establecimientos"
                                                UniqueName="BtnAddEstablecimiento" ImageUrl="/Imagenes/Iconos/16/earth.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                                ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                                ConfirmTitle="Eliminar Establecimiento" Text="Eliminar" UniqueName="DeleteCommand">
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
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnProcesar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Procesar" Width="120px" OnClick="BtnProcesar_Click" Visible="False" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" />
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
