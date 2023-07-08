<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddPuc.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddPuc" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
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
                            <td align="center" bgcolor="#999999" colspan="3">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR PLAN UNICO DE CUENTA x IMPUESTO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo impuesto">
                                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">LISTADO DEL PLAN UNICO DE CUENTA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1" colspan="3">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="5"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                    OnItemCommand="RadGrid1_ItemCommand">
                                    <MasterTableView DataKeyNames="id_puc" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_puc" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="id_puc">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Código Cuenta"
                                                UniqueName="codigo_cuenta" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre Cuenta"
                                                UniqueName="nombre_cuenta">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_movimiento" HeaderText="Movimiento"
                                                UniqueName="tipo_movimiento" FilterControlWidth="40px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_nivel" HeaderText="Nivel"
                                                UniqueName="tipo_nivel" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_naturaleza" HeaderText="Naturaleza"
                                                UniqueName="tipo_naturaleza" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                                UniqueName="nombre_cliente">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsignar" Text="Asignar Cuenta"
                                                UniqueName="BtnAsignar" ImageUrl="/Imagenes/Iconos/16/img_add.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">LISTA DE CUENTAS ASIGNADAS x IMPUESTO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" PageSize="5"
                                    OnItemCommand="RadGrid2_ItemCommand"
                                    OnNeedDataSource="RadGrid2_NeedDataSource"
                                    OnPageIndexChanged="RadGrid2_PageIndexChanged"
                                    OnUpdateCommand="RadGrid2_UpdateCommand">
                                    <MasterTableView CommandItemDisplay="Top" EditMode="PopUp" DataKeyNames="idcliente_puc, idformulario_impuesto, id_puc, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <EditFormSettings CaptionDataField="idcliente_puc"
                                            CaptionFormatString="Id Registro: {0}">
                                            <EditColumn UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                            <FormTemplate>
                                            </FormTemplate>
                                            <PopUpSettings Modal="True" />
                                        </EditFormSettings>
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idcliente_puc" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" UniqueName="idcliente_puc" ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                                UniqueName="descripcion_formulario" ReadOnly="True" FilterControlWidth="100px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cod. Cuenta"
                                                UniqueName="codigo_cuenta" ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre Cuenta"
                                                UniqueName="nombre_cuenta">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCheckBoxColumn DataField="base_gravable" HeaderText="Base Gravable"
                                                UniqueName="base_gravable">
                                            </telerik:GridCheckBoxColumn>

                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado" ReadOnly="True">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                                InsertText="Insertar" UpdateText="Actualizar">
                                            </telerik:GridEditCommandColumn>
                                            <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBaseGravable" Text="Base Gravable por cuenta"
                                                UniqueName="BtnBaseGravable" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                            </telerik:GridButtonColumn>--%>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquer" Text="Bloquear/DesBloquear"
                                                UniqueName="BtnBloquer" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar la Cuenta"
                                                UniqueName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
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
    </form>
</body>
</html>
