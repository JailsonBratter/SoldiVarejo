<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Filial.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Filial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center><h1>Filial</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Filial</p>
                    <asp:TextBox ID="txtFilial" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Razao social / Fantasia</p>
                    <asp:TextBox ID="txtRazaoSocial" runat="server" Width="300px"> </asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ</p>
                    <asp:TextBox ID="txtCnpj" runat="server" Width="180px"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Filial" Text="---" Visible="true"
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/manutencao/pages/Filialdetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="filial" />
                <asp:HyperLinkField DataTextField="Razao_social" Text="---" Visible="true"
                    HeaderText="Razão Social " DataNavigateUrlFormatString="~/modulos/manutencao/pages/Filialdetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="filial" />
                <asp:HyperLinkField DataTextField="Fantasia" Text="---" Visible="true" HeaderText="Fantasia"
                    DataNavigateUrlFormatString="~/modulos/manutencao/pages/Filialdetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="filial" />
                <asp:HyperLinkField DataTextField="Cnpj" Text="---" Visible="true" HeaderText="CNPJ"
                    DataNavigateUrlFormatString="~/modulos/manutencao/pages/Filialdetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="filial" />
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
