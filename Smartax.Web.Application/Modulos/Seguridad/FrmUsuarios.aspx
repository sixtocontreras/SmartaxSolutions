<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmUsuarios.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Seguridad.FrmUsuarios" %>
<%@ Register src="../../Controles/Seguridad/CtrlUsuarios.ascx" tagname="CtrlUsuarios" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrlUsuarios ID="CtrlUsuarios1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
