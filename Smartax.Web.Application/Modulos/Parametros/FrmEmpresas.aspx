<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmEmpresas.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Parametros.FrmEmpresas" %>
<%@ Register src="../../Controles/Parametros/Empresa/CtrlEmpresa.ascx" tagname="CtrlEmpresa" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrlEmpresa ID="CtrlEmpresa1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
