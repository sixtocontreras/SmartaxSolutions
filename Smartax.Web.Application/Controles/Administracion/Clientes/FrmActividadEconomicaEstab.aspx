<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmActividadEconomicaEstab.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmActividadEconomicaEstab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
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
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <div>
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR ACTIVIDADES ECONOMICAS DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">ACTIVIDADES ECONOMICAS SIN ASIGNAR</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="5">
                                    <MasterTableView DataKeyNames="idmun_act_economica" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idmun_act_economica" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="idmun_act_economica">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                                UniqueName="descripcion_formulario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="anio_actividad" HeaderText="Año Gravable"
                                                UniqueName="anio_actividad" FilterControlWidth="40px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_actividad" HeaderText="Tipo Actividad"
                                                UniqueName="tipo_actividad">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="codigo_agrupacion" HeaderText="Cód. Agrupaciín"
                                                UniqueName="codigo_agrupacion" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cód. Actividad"
                                                UniqueName="codigo_actividad" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Descripción"
                                                UniqueName="descripcion_actividad" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="descripcion_tarifa" HeaderText="Tipo Tarifa"
                                                UniqueName="descripcion_tarifa" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="tarifa_ley" HeaderText="Valor Ley"
                                                UniqueName="tarifa_ley" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tarifa_municipio" HeaderText="Valor Municipio"
                                                UniqueName="tarifa_municipio" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsignar" Text="Asignar Actividad"
                                                UniqueName="BtnAsignar" ImageUrl="/Imagenes/Iconos/16/img_add.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">ACTIVIDADES ECONOMICAS ASIGNADAS</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                            GridLines="None" PageSize="5"
                                            OnItemCommand="RadGrid2_ItemCommand"
                                            OnNeedDataSource="RadGrid2_NeedDataSource"
                                            OnPageIndexChanged="RadGrid2_PageIndexChanged"
                                            OnUpdateCommand="RadGrid2_UpdateCommand">
                                            <MasterTableView CommandItemDisplay="Top" EditMode="PopUp" DataKeyNames="idestab_act_economica, idmun_act_economica, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                <CommandItemTemplate>
                                                    <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                                </CommandItemTemplate>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="idestab_act_economica" EmptyDataText="" HeaderText="Id"
                                                        UniqueName="idestab_act_economica" FilterControlWidth="40px" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                                        UniqueName="descripcion_formulario" FilterControlWidth="100px" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="anio_actividad" HeaderText="Año Gravable"
                                                        UniqueName="anio_actividad" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="tipo_actividad" HeaderText="Tipo Actividad"
                                                        UniqueName="tipo_actividad" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="codigo_agrupacion" HeaderText="Cód. Agrupaciín"
                                                        UniqueName="codigo_agrupacion" Visible="false" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cód. Actividad"
                                                        UniqueName="codigo_actividad" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <%--<telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Descripción"
                                                        UniqueName="descripcion_actividad" Visible="false">
                                                     </telerik:GridBoundColumn>--%>

                                                    <telerik:GridBoundColumn DataField="descripcion_tarifa" HeaderText="Tipo Tarifa"
                                                        UniqueName="descripcion_tarifa" ReadOnly="true" Visible="false">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="tarifa_ley" HeaderText="Valor Ley" FilterControlWidth="70px"
                                                        UniqueName="tarifa_ley" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="tarifa_municipio" HeaderText="Valor Municipio" FilterControlWidth="70px"
                                                        UniqueName="tarifa_municipio" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="tipo_calculo" HeaderText="Tipo Calculo"
                                                        UniqueName="tipo_calculo" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridDropDownColumn DataField="idtipo_calculo" DataSourceID="DtTipoCalculos"
                                                        HeaderText="Tipo Calculo" ListDataMember="DtTipoCalculo" ListTextField="tipo_calculo"
                                                        ListValueField="idtipo_calculo" UniqueName="idtipo_calculo" Visible="false">
                                                    </telerik:GridDropDownColumn>

                                                    <telerik:GridBoundColumn DataField="id_puc" HeaderText="Id Puc"
                                                        UniqueName="id_puc" ReadOnly="true" Visible="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="# Cuenta"
                                                        UniqueName="codigo_cuenta" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                        UniqueName="codigo_estado" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                                        InsertText="Insertar" UpdateText="Actualizar">
                                                    </telerik:GridEditCommandColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDuplicar" Text="Duplicar Actividad"
                                                        UniqueName="BtnDuplicar" ImageUrl="/Imagenes/Iconos/16/img_add.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquer" Text="Bloquear/DesBloquear"
                                                        UniqueName="BtnBloquer" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnCuenta" Text="Asociar cuentas contables"
                                                        UniqueName="BtnCuenta" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar la Cuenta"
                                                        UniqueName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                        </telerik:RadWindowManager>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>

        <asp:HiddenField ID="HiddenField1" runat="server" />
        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
            PopupControlID="Panel2"
            TargetControlID="HiddenField1"
            CancelControlID="BtnSalir1"
            BackgroundCssClass="backgroundColor">
        </cc1:ModalPopupExtender>

        <asp:Panel ID="Panel2" runat="server" BackColor="#E6E6E6" Width="950px" HorizontalAlign="Center">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="popupcontainer" style="width: 100%">
                        <asp:Panel ID="Panel3" runat="server" Width="950px" BorderWidth="1">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center" bgcolor="#999999" colspan="5">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" CssClass="FormLabels" Font-Size="14pt" ForeColor="White">LISTADO DE CUENTAS CONTABLES</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="LblTitulo3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Código Cuenta:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <telerik:RadTextBox ID="TxtCodCuenta" runat="server" EmptyMessage="Código" Font-Size="15pt" Height="30px" MaxLength="20" TabIndex="6" Width="160px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="LblTitulo4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nombre Cuenta:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <telerik:RadTextBox ID="TxtNombreCuenta" runat="server" EmptyMessage="Nombre de la cuenta" Font-Size="15pt" Height="30px" MaxLength="250" TabIndex="6" Width="260px">
                                            <EmptyMessageStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Buscar" ToolTip="Buscar cuenta" Width="120px" OnClick="BtnConsultar_Click" />
                                        &nbsp;<asp:Button ID="BtnSalir1" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnSalir1_Click" Text="Salir" ToolTip="Salir" Width="120px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <telerik:RadGrid ID="RadGrid4" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Culture="es-ES" OnItemCommand="RadGrid4_ItemCommand" OnNeedDataSource="RadGrid4_NeedDataSource" OnPageIndexChanged="RadGrid4_PageIndexChanged">
                                            <MasterTableView DataKeyNames="idpuc_impuesto, id_puc" Name="Grilla" NoMasterRecordsText="No hay registros para mostrar">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="idpuc_impuesto" EmptyDataText="" FilterControlWidth="40px"
                                                        HeaderText="Id" ReadOnly="True" UniqueName="idpuc_impuesto">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_formulario" FilterControlWidth="100px"
                                                        HeaderText="Impuesto" ReadOnly="True" UniqueName="descripcion_formulario">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cod. Cuenta"
                                                        ReadOnly="True" UniqueName="codigo_cuenta">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre Cuenta"
                                                        UniqueName="nombre_cuenta">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridCheckBoxColumn DataField="base_gravable" HeaderText="Base Gravable"
                                                        UniqueName="base_gravable">
                                                    </telerik:GridCheckBoxColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                        ReadOnly="True" UniqueName="codigo_estado">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="BtnAsociar" Text="Asociar cuenta"
                                                        CommandName="BtnAsociar" ImageUrl="/Imagenes/Iconos/16/check.png">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
            <div class="loading">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                <h3>Espere un momento por favor ...</h3>
            </div>
        </telerik:RadAjaxLoadingPanel>

    </form>
</body>
</html>
