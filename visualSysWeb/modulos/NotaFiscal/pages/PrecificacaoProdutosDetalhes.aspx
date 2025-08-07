<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PrecificacaoProdutosDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PrecificacaoProdutosDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/PrecificacaoProdutos.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes Precificação de Produtos </h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Panel ID="cabecalho" runat="server" CssClass="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição</p>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data</p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtUsuarioPrecificacao" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:TextBox ID="txtStatus" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td rowspan="2">
                    <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar/Atualizar" OnClick="btnEncerrar_Click"
                        CssClass="submitButton" Height="40px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <asp:CheckBox ID="chkTodasFilias" runat="server" Text="Aplicar preços a Todas as Filiais"
                        Checked="false" />
                </td>
                <td colspan="3">
                    <br />
                    <asp:TextBox ID="txtPorc" runat="server" Width="80px" Style="font-size: 20px; text-align: right;"></asp:TextBox>
                    (%)
                    <asp:Button ID="btnAplicarPorc" runat="server" Text="Aplicar" OnClick="btnAplicarPorc_Click"
                        CssClass="submitButton" Height="20px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
            <HeaderTemplate>
                Itens Adicionados
            </HeaderTemplate>
            <ContentTemplate>
                <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
                    <h1><asp:Label ID="lblRegistros" runat="server"></asp:Label></h1>
                </div>
                <div class="direitaFechar" >
                    <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
                </div>
                <div class="gridTable">
                    <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridItens_RowCommand"
                        OnSorting="gridItens_Sorting" AllowSorting="True">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="true" SortExpression="1">
                            </asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" ReadOnly="true" SortExpression="2">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Custo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCusto" runat="server" Text='<%# Eval("Custo","{0:N2}") %>' Width="100px"
                                        CssClass="numero" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Margem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMargem" runat="server" Text='<%# Eval("Margem","{0:N4}") %>' Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtMargem" runat="server" CssClass="numero" style="text-align: right;" Text='<%# Eval("Margem","{0:N4}") %>'
                                        Width="100px"  OnKeyPress="numerosGrid(this,event);"
                                        onchange="calculaPreco(this)"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Preco_anterior" HeaderText="Prc Anterior" ItemStyle-HorizontalAlign="Right"
                                DataFormatString="{0:N2}" ReadOnly="true">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Preco novo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPreco_novo" runat="server" CssClass="numero" style="text-align: right;"  Text='<%# Eval("Preco_novo", "{0:N2}") %>'
                                        Width="100px" onchange="calculaMargem(this);" OnKeyPress="numerosGrid(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Codigo_Familia" HeaderText="Familia" ReadOnly="true">
                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                Text="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                            </asp:ButtonField>
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
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
            <HeaderTemplate>
                Selecionar Itens</HeaderTemplate>
            <ContentTemplate>
                <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="modalFormTelaCheiaframeDiv"
                    Style="height: 700px;" DefaultButton="imgPesquisaMercadoria">
                    <asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" CssClass="cabMenuLista"></asp:Label><br />
                    <div id="Div1" runat="server" class="modalFormTelaCheiaframeMini" style="height: 250px;">
                        <table>
                            <tr>
                                <td colspan="3">
                                    <center><b>Filtrar</b></center>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Grupo:
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" Width="200px"
                                        OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SubGrupo:
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlSubGrupo" runat="server" AutoPostBack="True" Width="200px"
                                        OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Departamento:
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Width="200px"
                                        OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Linha
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlLinha" runat="server" AutoPostBack="True" Width="200px"
                                        OnSelectedIndexChanged="ddlLinha_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fornecedor
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFornecedor" runat="server" Width="150px"></asp:TextBox><asp:ImageButton
                                        ID="imgFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                        Width="15px" OnClick="imgFornecedor_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Plu/EAN/Descricao:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                                        autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="20px" OnClick="ImgPesquisaMercadoria_Click" />Filtrar
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgLimpar" runat="server" ImageUrl="~/img/botao-apagar.png"
                                        Height="20px" OnClick="imgLimpar_Click" />Limpar
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe"
                        Style="height: 250px;">
                        <div class="gridTableTelaCheia">
                            <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelecionaMercadoria_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                    </asp:Panel>
                    <div class="btnImprimirDireita">
                        <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                            Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />Incluir
                        item</div>
                    <br />
                    <center><h3><b>Selecionados </b></h3></center>
                    <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv"
                        Style="height: 300px;">
                        <div class="gridTableTelaCheia">
                            <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand"
                                CssClass="table">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="true" SortExpression="1">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" ReadOnly="true" SortExpression="2">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Custo" HeaderText="Custo" ItemStyle-HorizontalAlign="Right"
                                        DataFormatString="{0:N2}" ReadOnly="true">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Margem" HeaderText="Margem" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField DataField="Preco_anterior" HeaderText="Preco_anterior" ReadOnly="true">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Preco_novo" HeaderText="Preco_anterior" ReadOnly="true">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Codigo_Familia" HeaderText="Familia" ReadOnly="true">
                                    </asp:BoundField>
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
                    </asp:Panel>
                </asp:Panel>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label>
                </h1>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            Filtrar
            <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="600px" CssClass="SEM"></asp:TextBox>
            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaLista_Click" />
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrlistaItem" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label2" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Encerrar A Precificação?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarEncerrar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaEncerrar_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarEncerrar_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEncerrar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEncerrar" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
