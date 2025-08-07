<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RelatoriosGraficos.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelatoriosGraficos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel2" runat="server" CssClass="filter">
        <div class="fontehyperlink">
            <table>
                <tr>
                    <td>
                        <asp:HyperLink ID="rel01" runat="server"  NavigateUrl="~/modulos/Relatorios/pages/RelComercial.aspx?tela=R007"> 01 - Comercial</asp:HyperLink>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HyperLink ID="rel02" runat="server" NavigateUrl="~/modulos/Relatorios/pages/RelImpostos.aspx?tela=R007"> 02 - Impostos</asp:HyperLink>
                    </td>
                </tr>
                
            </table>
        </div>
    </asp:Panel>
</asp:Content>
