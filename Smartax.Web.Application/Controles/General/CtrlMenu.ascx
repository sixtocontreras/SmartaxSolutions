<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlMenu.ascx.cs" Inherits="Smartax.Web.Application.Controles.General.CtrlMenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadMenu ID="MenuDinamico" runat="server" BorderStyle="None" Font-Size="16pt"
    EnableOverlay="False" Style="z-index: 2900" Width="100%">
    <CollapseAnimation Duration="200" Type="OutQuint" />
    <DataBindings>
        <telerik:RadMenuItemBinding ToolTipField="descripcion_opcion" />
    </DataBindings>
</telerik:RadMenu>
