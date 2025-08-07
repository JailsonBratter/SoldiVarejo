<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Fornecedor.aspx.cs" Inherits="visualSysWeb.Cadastro.Fornecedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Fornecedores</h1></center>
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
                        CNPJ</p>
                    <input type="text" id="txtCNPJ" runat="server" size="15" />
                </td>
                <td>
                    <p>
                        Fornecedor / Razão social</p>
                    <input type="text" id="txtFornecedor" runat="server" size="50" />
                </td>
                <td>
                     <div class="panelItem">

                        <p>Tipo</p>
                        <asp:DropDownList ID="ddlTipoFornecedor" runat="server">
                            <asp:ListItem Text="TODOS">
                            </asp:ListItem>
                            <asp:ListItem Text="ADMINISTRATIVO">
                            </asp:ListItem>
                            <asp:ListItem Text="COMERCIAL">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <p>
                        Conta Contábil</p>
                    <input type="text" id="txtContaContabil" runat="server" size="15" />
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridFornecedor" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="True" OnSorting="gridFornecedor_Sorting">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="fornecedor" Text="fornecedor" Visible="true" HeaderText="Fornecedor"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?fornecedor={0}"
                    DataNavigateUrlFields="strfornecedor"  SortExpression="fornecedor"/>
                <asp:HyperLinkField DataTextField="Razao_social" Text="Razao Social" Visible="true"
                    HeaderText="Razão Social" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?fornecedor={0}"
                    DataNavigateUrlFields="strfornecedor"  SortExpression="Razao_social" />
                <asp:HyperLinkField DataTextField="CNPJ" Text="CNPJ" Visible="true" HeaderText="CNPJ"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?fornecedor={0}"
                    DataNavigateUrlFields="strfornecedor"  SortExpression="CNPJ" />
                <asp:HyperLinkField DataTextField="TIPO_FORNECEDOR" Text="---" Visible="true" HeaderText="TIPO"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?fornecedor={0}"
                    DataNavigateUrlFields="strfornecedor"  SortExpression="TIPO_FORNECEDOR" />
                <asp:HyperLinkField DataTextField="Conta_Contabil" Text="---" Visible="true" HeaderText="CTA Contábil"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/FornecedorDetalhes.aspx?fornecedor={0}"
                    DataNavigateUrlFields="strfornecedor"  SortExpression="Conta_Contabil" />

            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle    BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle  BackColor="#506C8C" />
            <SortedDescendingCellStyle   BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <br />
    </div>
</asp:Content>
