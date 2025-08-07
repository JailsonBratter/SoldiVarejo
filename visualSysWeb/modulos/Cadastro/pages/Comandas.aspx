<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Comandas.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Comandas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Comandas</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" visible="true">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Comanda</p>
                    <asp:TextBox ID="txtPESQ1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="129px">
                        <asp:ListItem Value="">---</asp:ListItem>
                        <asp:ListItem Value="0">LIVRE</asp:ListItem>
                        <asp:ListItem Value="2">ABERTA</asp:ListItem>
                        <asp:ListItem Value="4">BLOQUEADA</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
            CellPadding="5" ForeColor="#333333">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Comanda" Text="Comanda" Visible="true" HeaderText="Comanda"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
                <asp:HyperLinkField DataTextField="Status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
                <asp:HyperLinkField DataTextField="Qtd_itens" Text="---" Visible="true" HeaderText="Qtde itens"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
                <asp:HyperLinkField DataTextField="Valor" Text="---" Visible="true" HeaderText="Valor"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
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
        <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>
    </div>
</asp:Content>
