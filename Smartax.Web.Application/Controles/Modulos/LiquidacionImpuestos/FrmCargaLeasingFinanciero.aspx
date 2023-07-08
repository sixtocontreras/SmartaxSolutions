<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageGeneral.Master" CodeBehind="FrmCargaLeasingFinanciero.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmCargaLeasingFinanciero" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <script>
                function download(filename, text) {
                    text = text.replaceAll('||', '\r\n');
                    var element = document.createElement('a');
                    element.setAttribute('href', 'data:text/plain+ parsed.join("\r\n\");charset=utf-8,' + text);
                    element.setAttribute('download', filename);

                    element.style.display = 'none';
                    document.body.appendChild(element);

                    element.click();

                    document.body.removeChild(element);
                }
                function reloadPage() {
                    window.parent.location.reload();
                }
            </script>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                    <table style="width: 100%">
                        <tr>
                            <td align="center" colspan="3" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CARGUE MASIVO LEASING FINANCIERO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadNumericTextBox Label="Año" ID="txtAnio" runat="server" EmptyMessage="Año" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="1" TabIndex="1" Width="120px" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="txtAnio" Display="None" ErrorMessage="Este campo es requerido!" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <telerik:RadNumericTextBox Label="Mes" ID="txtMes" MaxValue="12" runat="server" EmptyMessage="Mes" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="1" TabIndex="1" Width="120px" DataType="System.Int32">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="txtMes" Display="None" ErrorMessage="Este campo es requerido!" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                <telerik:RadAsyncUpload ID="upl" runat="server" MaxFileInputsCount="1"></telerik:RadAsyncUpload>
                                
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="btnCargar" OnClick="btnCargar_Click" runat="server" Font-Bold="True" Font-Size="14pt" Text="Cargar" ValidationGroup="ValidarDatos" />
                                <asp:Button ID="btnSalir" runat="server" Font-Bold="True" OnClick="btnSalir_Click" Font-Size="14pt" Text="Salir" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>


