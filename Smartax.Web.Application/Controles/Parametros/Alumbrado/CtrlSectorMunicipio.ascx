<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlSectorMunicipio.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Alumbrado.CtrlSectorMunicipio" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

        <asp:HiddenField ID="hdMuni" runat="server" />
        <asp:HiddenField ID="hdMuniSelect" runat="server" />
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="ALUMBRADO PÚBLICO – CONFIGURACIÓN MUNICIPIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="True" AllowFilteringByColumn="True" Visible="true"
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="20" Skin="Default"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnItemDataBound="RadGrid1_ItemDataBound"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id" EditMode="PopUp" Name="DtSectorMunicipio" NoMasterRecordsText="No hay Registros para Mostrar">

                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                             <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id" HeaderText="Id"
                                    UniqueName="id" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cliente" DataSourceID="DatosCliente"
                                    HeaderText="Cliente" ListDataMember="DtClientes" ListTextField="nombre_cliente"
                                    ListValueField="id_estado" UniqueName="id_cliente" Visible="false">
                                </telerik:GridDropDownColumn>
                                  <telerik:GridBoundColumn DataField="nombre_dpto" HeaderText="Departamento"
                                    UniqueName="nombre_dpto" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                 <telerik:GridTemplateColumn DataField="id_dpto" HeaderText="Departamento" UniqueName="id_dpto" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("id_dpto") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="id_dptoCombo"  runat="server"  OnClientSelectedIndexChanged="changeDpto">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                               <%-- <telerik:GridDropDownColumn DataField="id_dpto" DataSourceID="DatosDpto"
                                    HeaderText="Departamento" ListDataMember="DtDptos" ListTextField="nombre_departamento"
                                    ListValueField="id_dpto" UniqueName="id_dpto" Visible="false">
                                </telerik:GridDropDownColumn>--%>
                                   <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Codigo DANE"
                                    UniqueName="codigo_dane" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                 <telerik:GridTemplateColumn DataField="id_municipio" HeaderText="Municipio" UniqueName="id_municipio" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("id_municipio") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <select ID="id_municipioCombo" onchange="changeMpio(this)">
                                            <option>           </option>
                                        </select>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                               <%-- <telerik:GridDropDownColumn DataField="id_municipio" DataSourceID="DatosMunicipio"
                                    HeaderText="Municipio" ListDataMember="DtMunicipios" ListTextField="nombre_municipio"
                                    ListValueField="id_municipio" UniqueName="id_municipio" Visible="false">
                                </telerik:GridDropDownColumn>--%>
                                 <telerik:GridNumericColumn DataField="vigencia" MaxLength="4" HeaderText="Vigencia"
                                    UniqueName="vigencia" >
                                </telerik:GridNumericColumn>
                                 <telerik:GridBoundColumn DataField="nombre_sector" HeaderText="Sector"
                                    UniqueName="nombre_sector" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_sector" DataSourceID="DatosSector"
                                    HeaderText="Sector" ListDataMember="DtSectores" ListTextField="nombre"
                                    ListValueField="id" UniqueName="id_sector" Visible="false">
                                </telerik:GridDropDownColumn>
                               
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <%--<telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>--%>
                                 <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnConfiguracion" Text="Configuración de tarifas"
                                    UniqueName="BtnConfiguracion" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Esta Seguro de Eliminar el registro Seleccionado de la Lista ....?"
                                    ConfirmTitle="Eliminar Registro" Text="Eliminar Registro">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id" CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
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
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
        <h3>Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
<script>

   
    var changeDpto = function (element) {
        debugger;
        var muni = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlSectorMunicipio_hdMuni").value);
        var data = "";
        data += "<option value=''>Seleccione</option>";
        for (var i = 0; i < muni.length; i++) {
            if (muni[i].dpto.toString() === element._value) {
                data += "<option value='"+muni[i].id+"'>" + muni[i].nombre + "</option>";
            }
        }
        document.getElementById("id_municipioCombo").innerHTML = data;
    }

    var changeMpio = function (element) {
        if (element.value === "") {
            document.getElementById("ContentPlaceHolder1_CtrlSectorMunicipio_hdMuniSelect").value = "";
        } else {
            var muni = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlSectorMunicipio_hdMuni").value);
            for (var i = 0; i < muni.length; i++) {
                if (muni[i].id.toString() === element.value) {
                    document.getElementById("ContentPlaceHolder1_CtrlSectorMunicipio_hdMuniSelect").value = element.value + "|" + muni[i].nombre.toString();
                }
            }
        }
    }
    function reloadPage() {
        radalert('An <br /><b>html</b> string.<br />');
    }
</script>