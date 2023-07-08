<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlRegistroConsumos.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Alumbrado.CtrlRegistroConsumos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

        
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:HiddenField ID="hdMuni" runat="server" />
        <asp:HiddenField ID="hdMuniSelect" runat="server" />
        <asp:HiddenField ID="hdOfi" runat="server" />
        <asp:HiddenField ID="hdOfiSelected" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR CONSUMOS Y COSTOS DE ALUMBRADO PUBLICO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
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
                        <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id" EditMode="PopUp" Name="DtConsumos" NoMasterRecordsText="No hay Registros para Mostrar">

                            <ExpandCollapseColumn Visible="True">
                            </ExpandCollapseColumn>
                             <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                 <asp:LinkButton ID="lnkCargue" runat="server" CommandName="btnCargueMasivo" ToolTip="Realizar un cargue masivo"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> CARGUE MASIVO</asp:LinkButton>
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
                               
                                   <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                 <telerik:GridTemplateColumn DataField="id_municipio" HeaderText="Municipio" UniqueName="id_municipio" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("id_municipio") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <select ID="id_municipioCombo"  style="width:100%" runat="server" onchange="changeMpio()">
                                            <option>           </option>
                                        </select>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                               <telerik:GridBoundColumn DataField="nombre_oficina" HeaderText="Oficina"
                                    UniqueName="nombre_oficina" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="id_oficina" HeaderText="Oficina" UniqueName="id_oficina" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("id_oficina") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <select ID="id_oficinaCombo" style="width:100%" runat="server" onchange="changeOfi()">
                                            <option>           </option>
                                        </select>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridNumericColumn DataField="vigencia" MaxLength="4" HeaderText="Vigencia"
                                    UniqueName="vigencia" >
                                </telerik:GridNumericColumn> 
                                <telerik:GridBoundColumn DataField="nombre_mes" HeaderText="Mes"
                                    UniqueName="nombre_mes" ReadOnly="true" EmptyDataText="">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_mes" DataSourceID="DatosMes"
                                    HeaderText="Mes" ListDataMember="DtMes" ListTextField="nombre"
                                    ListValueField="id" UniqueName="id_mes" Visible="false">
                                </telerik:GridDropDownColumn>
                                <telerik:GridNumericColumn DataField="kw_consumo" HeaderText="Kilovatios consumidos"
                                    UniqueName="kw_consumo" >
                                </telerik:GridNumericColumn> 
                                <telerik:GridNumericColumn DataField="kw_hora" HeaderText="Costo Kilovatio por hora"
                                    UniqueName="kw_hora" >
                                </telerik:GridNumericColumn> 
                               
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha Registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
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

   
    var changeDpto = function () {
        var dptos =JSON.parse(document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_dptoCombo_ClientState").value).value 
        if (dptos === "") {
            document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuniSelect").value = "";
            document.getElementById("id_municipioCombo").innerHTML = "";

        } else {
            var muni = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuni").value);
            var data = "";
            data += "<option value=''>Seleccione</option>";
            for (var i = 0; i < muni.length; i++) {
                if (muni[i].dpto.toString() === dptos) {
                    data += "<option value='" + muni[i].id + "'>" + muni[i].nombre + "</option>";
                }
            }
            document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_municipioCombo").innerHTML = data;
        }
    }

    var changeMpio = function () {
        debugger;
        if (document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_municipioCombo").value === "") {
            document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuniSelect").value = "";
             document.getElementById("id_oficinaCombo").innerHTML = "";
        } else {
            var muni = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuni").value);
            for (var i = 0; i < muni.length; i++) {
                if (muni[i].id.toString() ===document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_municipioCombo").value) {
                    document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuniSelect").value = document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_municipioCombo").value + "|" + muni[i].nombre.toString();
                }
            }
            var ofi = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfi").value);
            var data = "";
            data += "<option value=''>Seleccione</option>";
            for (var i = 0; i < ofi.length; i++) {
                if (ofi[i].municipio.toString() === document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_municipioCombo").value) {
                    data += "<option value='"+ofi[i].id+"'>" + ofi[i].nombre + "</option>";
                }
            }
            document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_oficinaCombo").innerHTML = data;

        }
    }

    var setMpioOfiData = function (dpto, mpio,ofi) {
        document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfiSelected").value = ofi;        
        document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuniSelect").value = mpio;
    }

    //var setOfi = function (mpio, ofi) {

    //}

    var changeOfi = function () {
        debugger;
        if (document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_oficinaCombo").value === "") {
            document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfiSelected").value = "";
        } else {
            var ofi = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfi").value);
            for (var i = 0; i < ofi.length; i++) {
                if (ofi[i].id.toString() === document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_oficinaCombo").value) {
                    document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfiSelected").value = document.getElementById("ctl00_ContentPlaceHolder1_CtrlRegistroConsumos_RadGrid1_ctl00_ctl02_ctl04_id_oficinaCombo").value + "|" + ofi[i].nombre.toString();
                }
            }
        }
    }
</script>