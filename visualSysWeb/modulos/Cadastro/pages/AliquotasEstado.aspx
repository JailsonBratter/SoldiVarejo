<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AliquotasEstado.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.AliquotasEstado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Classificação Fiscal</h1>
    </center>
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
                        NCM</p>
                    <asp:TextBox ID="txtNCM" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição</p>
                    <asp:TextBox ID="txtDescricao" runat="server"></asp:TextBox>
                </td>
                
            </tr>
        </table>
    </div>
    
                <div class="panelItem" style="margin-bottom:15px;">
                    <h1>
                        <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
                </div>

    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
            CellPadding="5" ForeColor="#333333">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="cf" Text="---" Visible="true" HeaderText="NCM"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/AliquotasEstadoDetalhes.aspx?ncm={0}"
                    DataNavigateUrlFields="cf" />
                <asp:HyperLinkField DataTextField="Descricao" Text="---" Visible="true"
                    HeaderText="Descrição" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/AliquotasEstadoDetalhes.aspx?ncm={0}"
                    DataNavigateUrlFields="cf" />
                

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
