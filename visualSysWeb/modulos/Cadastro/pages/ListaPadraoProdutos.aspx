<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaPadraoProdutos.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ListaPadraoProdutos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>LISTA PADRAO DE PRODUTOS</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" style="margin-left:15px">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Nome
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="300px"> </asp:TextBox>
                    </div>
                <div class="panelItem">
                    <p>
                        Tipo</p>
                    <asp:DropDownList ID="ddlTipo" runat="server">
                        <asp:ListItem Value="TODOS">TODOS</asp:ListItem>
                        <asp:ListItem Value="INVENTARIO">INVENTARIO</asp:ListItem>
                        <asp:ListItem Value="PRODUCAO">PRODUCAO</asp:ListItem>
                        <asp:ListItem Value="COMPRAS">COMPRAS</asp:ListItem>
                    </asp:DropDownList>
                </div>
                   
                  
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"
            CellPadding="6" ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="id" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/ListaPadraoProdutosDetalhes.aspx?codigo={0}&tipo={1}"
                    DataNavigateUrlFields="id,tipo" />
                <asp:HyperLinkField DataTextField="descricao" Text="----" Visible="true" HeaderText="Nome"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/ListaPadraoProdutosDetalhes.aspx?codigo={0}&tipo={1}"
                    DataNavigateUrlFields="id,tipo" />
                <asp:HyperLinkField DataTextField="Tipo" Text="----" Visible="true" HeaderText="Tipo"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/ListaPadraoProdutosDetalhes.aspx?codigo={0}&tipo={1}"
                    DataNavigateUrlFields="id,tipo" />
                
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
