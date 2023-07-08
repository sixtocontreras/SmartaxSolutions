<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageError.Master" AutoEventWireup="true" CodeBehind="FrmErrorPlataforma.aspx.cs" Inherits="Smartax.Web.Application.FrmErrorPlataforma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="center">
                <asp:Label ID="LblDescripcion" runat="server" Font-Size="13pt" ForeColor="Red" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">--------------------------------------------------------------------------</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                            <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
            </td>
        </tr>
    </table>
</asp:Content>
