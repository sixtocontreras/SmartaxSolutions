<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmClientes.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Administracion.Clientes.FrmClientes" %>
<%@ Register src="../../../Controles/Administracion/Clientes/CtrlClientes.ascx" tagname="CtrlClientes" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrlClientes ID="CtrlClientes1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
