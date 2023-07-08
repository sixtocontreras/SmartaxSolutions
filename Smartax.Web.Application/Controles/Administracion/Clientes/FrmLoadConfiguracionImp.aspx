﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLoadConfiguracionImp.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmLoadConfiguracionImp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <script language="JavaScript" type="text/javascript">
        function deshabilitar(boton) {
            //alert('Click en el Boton.');
            document.getElementById(boton).style.visibility = 'hidden';
            document.getElementById('BtnCargar').style.visibility = 'hidden';
            document.getElementById('BtnSalir').style.visibility = 'hidden';
        }
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" EnableAJAX="False" Width="100%">
                <asp:Panel ID="Panel2" runat="server" DefaultButton="BtnCargar">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" colspan="4" bgcolor="#999999">
                                <asp:Label ID="Label1" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="15pt" Text="REALIZAR CARGUE MASIVO DE CONFIGURACIÓN IMPUESTO" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Separado Por</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="13pt">Archivo a Cargar</asp:Label>
                            </td>
                            <td align="center">
                                &nbsp;</td>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:DropDownList ID="CmbTipoCaracter" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo caracter">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbTipoCaracter" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <asp:FileUpload ID="FileExaminar" runat="server" Width="400px" />
                                <asp:RequiredFieldValidator ID="Validator4" runat="server" ControlToValidate="FileExaminar" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validator4_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="Validator4">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Button ID="BtnCargar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnCargar_Click" Text="Cargar" ToolTip="Cargar información del archivo" ValidationGroup="ValidarDatos" Width="120px" />
                                &nbsp;<asp:Button ID="BtnProcesar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnProcesar_Click" OnClientClick="deshabilitar(this.id)" Text="Procesar" ToolTip="Guardar informacion al sistema" Visible="False" Width="120px" />
                                &nbsp;<asp:Button ID="BtnCancelar" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="#CC0000" OnClick="BtnCancelar_Click" Text="Cancelar" ToolTip="Click para cancelar el proceso" Visible="False" Width="120px" />
                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" class="auto-style1">
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" ForeColor="Black">INFORMACIÓN A PROCESAR DEL ARCHIVO SELECCIONADO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" PageSize="13" 
                                    OnNeedDataSource="RadGrid1_NeedDataSource" 
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="idconfiguracion_imp" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idconfiguracion_imp" EmptyDataText="" HeaderText="Id" 
                                                UniqueName="idconfiguracion_imp" ReadOnly="True" FilterControlWidth="40px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable" 
                                                UniqueName="anio_gravable" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cod. Cuenta" 
                                                UniqueName="codigo_cuenta" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Cuenta" 
                                                UniqueName="nombre_cuenta" FilterControlWidth="100px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_inicial" HeaderText="S. Inicial" 
                                                UniqueName="saldo_inicial" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="mov_debito" HeaderText="Mov. Debito" 
                                                UniqueName="mov_debito" Visible="false" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="mov_credito" HeaderText="Mov. Credito" 
                                                UniqueName="mov_credito" Visible="false" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="saldo_final" HeaderText="S. Final" 
                                                UniqueName="saldo_final" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_extracontable" HeaderText="Valor Ext." 
                                                UniqueName="valor_extracontable" Visible="false" FilterControlWidth="50px">
                                            </telerik:GridBoundColumn>
                                        </Columns>
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
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
