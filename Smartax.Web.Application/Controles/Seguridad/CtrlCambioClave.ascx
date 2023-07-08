<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlCambioClave.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlCambioClave" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="PanelPrincipal" runat="server" Width="1000px" DefaultButton="BtnGuardar">
        <table cellpadding="4" cellspacing="0" class="Tab" style="width:100%;">
            <tr>
                <td colspan="4" align="center" bgcolor="#999999">
                    <asp:Label ID="LblTitulo" runat="server" Font-Bold="True" Font-Size="16pt" Text="REALIZAR CAMBIO DE CLAVE" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="* Nombres:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtNombres" Runat="server" MaxLength="60" Width="250px" Enabled="False">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato1" runat="server" ControlToValidate="TxtNombres" Display="None" ErrorMessage="El Nombre es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato1_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato1">
                    </cc1:ValidatorCalloutExtender>
                </td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text="* Apellidos:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtApellidos" Runat="server" MaxLength="60" Width="250px" Enabled="False">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato2" runat="server" ControlToValidate="TxtApellidos" Display="None" ErrorMessage="El Apellido es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato2_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato2">
                    </cc1:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text="* Identificación:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtIdentificacion" Runat="server" MaxLength="20" Width="250px" Enabled="False">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato3" runat="server" ControlToValidate="TxtIdentificacion" Display="None" ErrorMessage="La Identificación es requerida !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato3_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato3">
                    </cc1:ValidatorCalloutExtender>
                </td>
                <td align="right">
                    <asp:Label ID="Label4" runat="server" Text="* Email:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtEmail" Runat="server" MaxLength="100" Width="250px" Enabled="False">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato4" runat="server" ControlToValidate="TxtEmail" Display="None" ErrorMessage="El email es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato4_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato4">
                    </cc1:ValidatorCalloutExtender>
                    <asp:RegularExpressionValidator ID="ValidadorEmailCorrecto" runat="server" ControlToValidate="TxtEmail" Display="None" ErrorMessage="Email no valido !" SetFocusOnError="True" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" ValidationGroup="ValidarDatos"></asp:RegularExpressionValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidadorEmailCorrecto_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidadorEmailCorrecto">
                    </cc1:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" Text="* Nueva Clave:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtNuevoPassword" Runat="server" MaxLength="16" Width="250px" TextMode="Password">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato5" runat="server" ControlToValidate="TxtNuevoPassword" Display="None" ErrorMessage="El nuevo password es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato5_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato5">
                    </cc1:ValidatorCalloutExtender>
                </td>
                <td align="right">
                    <asp:Label ID="Label6" runat="server" Text="* Confirmar Clave:"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="TxtConfirmarPassword" Runat="server" MaxLength="16" Width="250px" TextMode="Password">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="ValidarDato6" runat="server" ControlToValidate="TxtConfirmarPassword" Display="None" ErrorMessage="Debe confirmar el nuevo password !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidarDato6_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato6">
                    </cc1:ValidatorCalloutExtender>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TxtNuevoPassword" ControlToValidate="TxtConfirmarPassword" Display="None" ErrorMessage="La Clave confirmada no es igual a la nueva !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:CompareValidator>
                    <cc1:ValidatorCalloutExtender ID="CompareValidator1_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="CompareValidator1">
                    </cc1:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td>
                    <asp:Label ID="LblLogin" runat="server" Visible="False"></asp:Label>
                </td>
                <td align="right">&nbsp;</td>
                <td>
                    <asp:Label ID="LblIdUsuario" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="13pt" OnClick="BtnGuardar_Click" Text="Cambiar Clave" ToolTip="Click para guardar la información" ValidationGroup="ValidarDatos" Width="150px" />
                    &nbsp;<asp:Button ID="BtnCancelar" runat="server" Font-Bold="True" Font-Size="13pt" OnClick="BtnCancelar_Click" Text="Salir" ToolTip="Click para cancelar el registro" Width="150px" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Label ID="LblMensaje" runat="server" Font-Bold="True" Font-Size="13pt" ForeColor="#CC0000"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
    <div class="loading">
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="100px" />
        <h3>
            Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
