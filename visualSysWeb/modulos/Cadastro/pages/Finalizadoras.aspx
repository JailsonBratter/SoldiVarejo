<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Finalizadoras.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Finalizadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Finalizadoras</h1></center>
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
                        Numero Finalizadora</p>
                    <asp:TextBox ID="txtPESQ1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Finalizadora</p>
                    <asp:TextBox ID="txtPESQ2" runat="server"></asp:TextBox>
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
                <asp:HyperLinkField DataTextField="Nro_Finalizadora" Text="Nro_Finalizadora" Visible="true"
                    HeaderText="Nro_Finalizadora" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/finalizadoraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="Codigo_centro_custo" Text="Codigo_centro_custo"
                    Visible="true" HeaderText="Codigo_centro_custo" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/finalizadoraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="Finalizadora" Text="Finalizadora" Visible="true"
                    HeaderText="Finalizadora" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/finalizadoraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nro_Finalizadora" />
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
