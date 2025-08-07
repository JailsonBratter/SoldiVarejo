<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Transportadoras.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Transportadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Transportadoras</h1></center>
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
                        Nome/Razao Social
                    </p>
                    <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ</p>
                    <asp:TextBox ID="txtCNPJ" runat="server" CssClass="CNPJ" Width="150px" MaxLength="17"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="9"
            ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Nome_transportadora" Text="---" Visible="true"
                    HeaderText="Transportadora" DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
                <asp:HyperLinkField DataTextField="cnpj" Text="---" Visible="true" HeaderText="CNPJ"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
                <asp:HyperLinkField DataTextField="Razao_social" Text="---" Visible="true" HeaderText="Razao Social"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
                <asp:HyperLinkField DataTextField="cidade" Text="---" Visible="true" HeaderText="Cidade"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
                <asp:HyperLinkField DataTextField="estado" Text="---" Visible="true" HeaderText="UF"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
                  <asp:HyperLinkField DataTextField="dpadrao" Text="---" Visible="true" HeaderText="Default"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TransportadorasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Nome_Transportadora" />
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
