<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAcuerdosMunicipales.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmAcuerdosMunicipales" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False">
            <div>
                <asp:Panel ID="PanelDatos" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">&nbsp;</td>
                            <td align="center" bgcolor="#999999" colspan="4">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="White">CARGAR ARCHIVO DE ACUERDOS MUNICIPALES</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Impuesto</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Normatividad</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Fecha del Documento</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Documento</asp:Label>
                            </td>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el formulario de impuesto">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <asp:DropDownList ID="CmbTipoNormatividad" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el tipo normatividad">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbTipoNormatividad" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <telerik:RadDatePicker ID="DtFechaAcuerdo" runat="server" Font-Size="15pt" TabIndex="3">
                                    <Calendar Culture="es-ES" EnableWeekends="True" FastNavigationNextText="&amp;lt;&amp;lt;" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                    </Calendar>
                                    <DateInput DateFormat="d/MM/yyyy" DisplayDateFormat="d/MM/yyyy" LabelWidth="40%" TabIndex="3">
                                        <EmptyMessageStyle Resize="None" />
                                        <ReadOnlyStyle Resize="None" />
                                        <FocusedStyle Resize="None" />
                                        <DisabledStyle Resize="None" />
                                        <InvalidStyle Resize="None" />
                                        <HoveredStyle Resize="None" />
                                        <EnabledStyle Resize="None" />
                                    </DateInput>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="3" />
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="DtFechaAcuerdo" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <asp:FileUpload ID="FileExaminar" runat="server" Width="300px" TabIndex="4" />
                                <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="FileExaminar" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnCargar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnCargar_Click" Text="Cargar" ToolTip="Click para adjuntar documento" Width="120px" TabIndex="5" />
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1" colspan="5">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowMultiRowSelection="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="20" Width="100%">
                                    <MasterTableView ClientDataKeyNames="idacuerdo_municipal, idtipo_normatividad" CommandItemDisplay="Top" EditMode="PopUp" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idacuerdo_municipal" EmptyDataText="" HeaderText="Id"
                                                UniqueName="idacuerdo_municipal" FilterControlWidth="40px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_formulario" FilterControlWidth="100px" HeaderText="Impuesto"
                                                UniqueName="descripcion_formulario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_normatividad" FilterControlWidth="100px" HeaderText="Tipo Normatividad"
                                                UniqueName="tipo_normatividad">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_original_archivo" HeaderText="Nombre del Archivo"
                                                UniqueName="nombre_original_archivo" Visible="false" FilterControlWidth="180px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_archivo" HeaderText="Nombre del Archivo"
                                                UniqueName="nombre_archivo" FilterControlWidth="180px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_acuerdo" FilterControlWidth="60px" HeaderText="F. Documento"
                                                UniqueName="fecha_acuerdo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="F. Registro"
                                                UniqueName="fecha_registro">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDownload" Text="Descargar Documento"
                                                ImageUrl="/Imagenes/Iconos/16/img_load.png" UniqueName="BtnDownload">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnQuitar" Text="Eliminar archivo del acuerdo"
                                                ImageUrl="/Imagenes/Iconos/16/delete.png" UniqueName="BtnQuitar">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
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
        <%--<uc1:CtrlRadicacionCorrespondencia ID="CtrlRadicacionCorrespondencia1" runat="server" />--%>
    </form>
</body>
</html>
