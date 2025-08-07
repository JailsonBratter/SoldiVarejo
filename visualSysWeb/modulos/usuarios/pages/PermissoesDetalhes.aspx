<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermissoesDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.usuarios.pages.PermissoesDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/PermissoesDetalhes1.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes das Permissões do Usuario</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>

    <div id="cabecalho" runat="server" class="frame">
        <table>

            <tr>
                <td>
                    <p>
                        id
                    </p>
                    <asp:TextBox ID="txtId" runat="server" Enabled="false"></asp:TextBox>
                </td>

                <td>
                    <p>
                        usuario
                    </p>
                    <asp:TextBox ID="txtusuario" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <p>
                        nome
                    </p>
                    <asp:TextBox ID="txtnome" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <p>
                        filial
                    </p>
                    <asp:TextBox ID="txtfilial" runat="server" Enabled="false"></asp:TextBox>


                </td>


                <td>
                    <asp:CheckBox ID="chkadm" runat="server" Text="adm" Enabled="false" />
                </td>
            </tr>

        </table>
    </div>
    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
            <HeaderTemplate>
                Telas
            </HeaderTemplate>
            <ContentTemplate>

                <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
                    <asp:Panel ID="PnModulos" runat="server" CssClass="inicial">
                    </asp:Panel>
                </div>

            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
            <HeaderTemplate>
                Detalhes
            </HeaderTemplate>
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <div class="row">
                                <h1>Telas</h1>
                                <hr />
                            </div>
                            <div class="panelItem" style="width: 200px; height: 400px; overflow: auto;">
                                <h2>Clientes</h2>
                                <asp:GridView ID="gridCliente" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                    GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                            </div>

                            <div class="panelItem" style="width: 200px; height: 400px; overflow: auto;">
                                <h2>Fornecedor</h2>
                                <asp:GridView ID="gridFornecedor" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                    GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                            </div>

                            <div class=" panelItem" style="width: 200px; height: 400px; overflow: auto;">
                                <h2>Produtos</h2>
                                <asp:GridView ID="gridProdutos" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                    GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                            </div>
                            <div class="panelItem" style="width: 200px; height: 400px; overflow: auto;">
                                <h2>Departamentos</h2>
                                <asp:GridView ID="gridDepartamento" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                    GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                            </div>
                            <div class="panelItem" style="width: 200px; height: 400px; overflow: auto;">
                                <h2>Plano de contas</h2>
                                <asp:GridView ID="gridPlanosDeContas" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                    GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                            </div>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <div class="row">
                                <h1>Relatorios</h1>
                                <hr />
                            </div>
                            <div class="row">

                                <div class=" panelItem" style="width: 500px; height: 500px; overflow: auto;">
                                    <h2>Vendas</h2>
                                    <asp:GridView ID="gridVendas" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>

                                <div class=" panelItem" style="width: 300px; height: 500px; overflow: auto;">
                                    <h2>Financeiro</h2>
                                    <asp:GridView ID="gridFinanceiro" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>

                                <div class=" panelItem" style="width: 300px; height: 500px; overflow: auto;">
                                    <h2>Fiscal</h2>
                                    <asp:GridView ID="gridFiscal" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>
                            </div>
                            <div class="row">
                                <div class=" panelItem" style="width: 400px; height: 500px; overflow: auto;">
                                    <h2>Estoque</h2>
                                    <asp:GridView ID="gridEstoque" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>

                                <div class=" panelItem" style="width: 300px; height: 500px; overflow: auto;">
                                    <h2>Cadastrais</h2>
                                    <asp:GridView ID="gridCadastrais" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>
                                <div class=" panelItem" style="width: 300px; height: 500px; overflow: auto;">
                                    <h2>Comanda</h2>
                                    <asp:GridView ID="gridComanda" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                                        GridLines="None" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSeleciona" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("acesso").ToString().Equals("1") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Todos" HeaderStyle-HorizontalAlign="Left" />
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
                                </div>
                            </div>
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

</asp:Content>
