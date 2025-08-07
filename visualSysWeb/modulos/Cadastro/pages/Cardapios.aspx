<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cardapios.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Cardapios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Cardapios</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtns" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="panelItem" style="margin-bottom: 15px;">
        <div class="panelItem" style="margin-bottom: 0px;">
            <h1>
                <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
        </div>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridCardapios" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="True" >
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="id" Text="Codigo" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/Cardapio.aspx?tela=C034&ID={0}"
                    DataNavigateUrlFields="ID" />
                <asp:HyperLinkField DataTextField="Titulo" Text="Titulo_Cardapio" Visible="true"
                    HeaderText="Título do Cardápio" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/Cardapio.aspx?tela=C034&ID={0}"
                    DataNavigateUrlFields="ID"  />
                <asp:HyperLinkField DataTextField="data_ultima_alteracao" Text="Alteracao" Visible="true" HeaderText="Alteração"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/Cardapio.aspx?tela=C034ID={0}"
                    DataNavigateUrlFields="ID" />
                <asp:HyperLinkField DataTextField="usuario_alteracao" Text="Usuario_Alteracao" Visible="true" HeaderText="Usuário Alteração"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/Cardapio.aspx?tela=C034ID={0}"
                    DataNavigateUrlFields="ID" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <br />
    </div>
</asp:Content>
