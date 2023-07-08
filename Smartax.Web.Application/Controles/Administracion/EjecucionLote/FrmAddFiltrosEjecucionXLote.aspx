<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddFiltrosEjecucionXLote.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.EjecucionLote.FrmAddFiltrosEjecucionXLote" %>

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
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR FILTROS DE EJECUCIÓN POR DEPARTAMENTO Y MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="Panel2" runat="server">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Formulario</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                            </td>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" OnSelectedIndexChanged="CmbDpto_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el formulario aplicar en el filtro">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                                </cc1:ValidatorCalloutExtender>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbDpto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbDpto_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el departamento">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="BtnAddDpto0" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnAddDpto_Click" Text="Agregar Departamento" ToolTip="Permite asociar solo el Departamento seleccionado" ValidationGroup="ValidarDatos" Width="240px" />
                                                &nbsp;<asp:Button ID="BtnSalir0" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">LISTADO DE MUNICIPIOS POR DEPARTAMENTO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="5">
                                    <MasterTableView DataKeyNames="id_municipio, id_dpto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_municipio" EmptyDataText="" FilterControlWidth="40px" HeaderText="Id" ReadOnly="True" UniqueName="id_municipio">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane_mun" FilterControlWidth="70px" HeaderText="Código Dane" UniqueName="codigo_dane_mun">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" FilterControlWidth="100px" HeaderText="Nombre Municipio" UniqueName="nombre_municipio">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_estado" FilterControlWidth="60px" HeaderText="Estado" ReadOnly="True" UniqueName="codigo_estado">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsignar" ImageUrl="/Imagenes/Iconos/16/img_add.png" Text="Asignar Municipio" UniqueName="BtnAsignar">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">DEPARTAMENTOS O MUNICIPIOS ASIGNADOS</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid2_ItemCommand" OnNeedDataSource="RadGrid2_NeedDataSource" OnPageIndexChanged="RadGrid2_PageIndexChanged" PageSize="5">
                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="idejec_lote_filtro, idformulario_impuesto, id_dpto, idestado_declaracion, idestado_filtro" EditMode="PopUp" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <EditFormSettings CaptionDataField="idejec_lote_filtro" CaptionFormatString="Id Registro: {0}">
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
                                            <telerik:GridBoundColumn DataField="idejec_lote_filtro" EmptyDataText="" FilterControlWidth="40px" HeaderText="Id" ReadOnly="True" UniqueName="idejec_lote_filtro">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_formulario" FilterControlWidth="70px" HeaderText="Formulario" ReadOnly="true" UniqueName="descripcion_formulario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_dpto" FilterControlWidth="100px" HeaderText="Departamento" ReadOnly="True" UniqueName="nombre_dpto">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane" FilterControlWidth="70px" HeaderText="Código Dane" ReadOnly="True" UniqueName="codigo_dane">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" FilterControlWidth="100px" HeaderText="Municipio" UniqueName="nombre_municipio">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="estado_declaracion" FilterControlWidth="70px" HeaderText="Estado Declar." ReadOnly="true" UniqueName="estado_declaracion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="estado_filtro" FilterControlWidth="60px" HeaderText="Estado" UniqueName="estado_filtro">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_registro" FilterControlWidth="70px" HeaderText="F. Registro" ReadOnly="True" UniqueName="fecha_registro">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_actualizacion" FilterControlWidth="70px" HeaderText="F. Actualización" ReadOnly="True" UniqueName="fecha_actualizacion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAprobacion" ImageUrl="/Imagenes/Iconos/16/check.png" Text="Aprobar o Rechazar Declaración" UniqueName="BtnAprobacion">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquer" ImageUrl="/Imagenes/Iconos/16/img_block.png" Text="Bloquear/DesBloquear" UniqueName="BtnBloquer">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png" Text="Desasociar Municipio" UniqueName="BtnDisociar">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;</td>
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
