<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmGestionUsuarios.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Seguridad.FrmGestionUsuarios" %>
<%@ Register src="../../Controles/Seguridad/CtrlGestionUsuarios.ascx" tagname="CtrlGestionUsuarios" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrlGestionUsuarios ID="CtrlGestionUsuarios1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
