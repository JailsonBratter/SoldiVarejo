<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="TabelaPrecoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.TabelaPrecoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/TabelaPrecoDetalhes.css" rel="stylesheet" />
    <script src="../js/TabelaPrecoDetalhes.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>Detalhes da Tabela de Preço</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo Tabela
                        </p>
                        <asp:TextBox ID="txtcodigo_tabela" runat="server" MaxLength="6" Width="100px"></asp:TextBox>

                    </div>
                    <div class="panelItem">
                        <p>
                            Nro Tabela
                        </p>
                        <asp:TextBox ID="txtNro_Tabela" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Porc
                        </p>
                        <asp:TextBox ID="txtPorc" runat="server" Width="80px"
                            AutoPostBack="true"
                            OnTextChanged="txtPorc_TextChanged">
                        </asp:TextBox>

                    </div>
                    <asp:Panel ID="pnAcrescimo" runat="server" CssClass="panelItem" Style="width: 160px; margin-top: 15px">
                        <label class="switch">
                            <asp:CheckBox ID="chkAcrescimo" runat="server" AutoPostBack="true" OnCheckedChanged="chkAcrescimo_CheckedChanged" />
                            <span class="slider round"></span>
                        </label>
                    </asp:Panel>
                  
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens Adicionados
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItens" runat="server" DefaultButton="ImgBtn_txtPluAddRapido">
                        <table>
                            <tr>
                                <td>
                                    <div class="panelItem">
                                        <p>
                                            Plu
                                        </p>
                                        <asp:TextBox ID="txtPlu" runat="server" CssClass="sem" Width="80px"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtn_txtPluAddRapido" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="ImgBtnAddItens_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>
                                            Descrição
                                        </p>
                                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                    </div>

                                    <div class="panelItem">
                                        <p>
                                            Preço
                                        </p>
                                        <asp:TextBox ID="txtPreco" runat="server" Width="80px" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <p>Arredondamento <asp:ImageButton ID="btnHelpArredondamento" ImageUrl="~/img/help.png" Width="20" runat="server" style="position:absolute; margin-left:10px " OnClick="btnHelpArredondamento_Click" /></p>
                                        <asp:DropDownList ID="ddlTipoArredondamento" runat="server" Style="margin: 0px; 10px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoArredondamento_SelectedIndexChanged1">
                                            <asp:ListItem Value="1">NÃO ARREDONDA</asp:ListItem>
                                            <asp:ListItem Value="2">TRUNCAMENTO</asp:ListItem>
                                            <asp:ListItem Value="3">ARREDONDAMENTO</asp:ListItem>
                                            <asp:ListItem Value="4">ARR SIG 0,5</asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </div>
                                    <div id="divPrecoMargem" runat="server" class="panelItem" style="margin-top:-1px;">
                                        <div class="panelItem">
                                            <p>
                                                Preço Custo
                                            </p>
                                            <asp:TextBox ID="txtPrecoCusto" runat="server" Width="80px" ></asp:TextBox>
                                        </div>
                                        <div class="panelItem">
                                            <p>
                                                Margem
                                            </p>
                                            <asp:TextBox ID="TxtMargem" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="TxtMargem_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="panelItem">
                                            <p>
                                                Preço Tabela
                                            </p>
                                            <asp:TextBox ID="txtPrecoTabela" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtPrecoTabela_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="divPrecoDesconto" runat="server" class="panelItem" style="margin-top: -1px;">

                                        <div class="panelItem">
                                            <p>
                                                <asp:Label ID="lblDesconto" Text="Desconto" runat="server" />
                                            </p>
                                            <asp:TextBox ID="txtDesconto" runat="server" Width="80px"  AutoPostBack="true" OnTextChanged="txtDesconto_TextChanged"></asp:TextBox>
                                        </div>
                                        
                                        <div class="panelItem">
                                            <p>
                                                Preço Promoção 
                                            </p>
                                            <asp:TextBox ID="txtPrecoPromocao" runat="server" Width="80px" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);" AutoPostBack="true" OnTextChanged="txtPrecoPromocao_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="divAdd" runat="server" class="panelItem">
                                        <br />
                                        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                            OnClick="btnAdd_Click" />

                                        <asp:Label ID="Label4" runat="server" Text="Adicionar"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:GridView ID="gridProdutos" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridProdutos_RowCommand"
                        OnRowDataBound="gridItens_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                Text="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="descricao" HeaderText="Descrição">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Preco" HeaderText="Preço">
                                <ItemStyle Width="150px" />
                                <ItemStyle HorizontalAlign="Right" />

                            </asp:BoundField>
                            <asp:TemplateField HeaderText="%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescontoItem" runat="server" Text='<%# Eval("PositivoDesconto","{0:N2}") %>' 
                                        onchange="javascript:return calculaValor(this,'TabPanel1_gridProdutos')"
                                        Width="100px" CssClass="numero" 
                                        onkeypress="javascript:return formataDouble(this,event);" 
                                        onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arredodamento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoArredondamento" runat="server" Style="margin: 10px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoArredondamento_SelectedIndexChanged">
                                        <asp:ListItem Value="1">NÃO ARREDONDA</asp:ListItem>
                                        <asp:ListItem Value="2">TRUNCAMENTO</asp:ListItem>
                                        <asp:ListItem Value="3">ARREDONDAMENTO</asp:ListItem>
                                        <asp:ListItem Value="4">ARR SIG 0,5</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:Label ID="lblTipoConta" runat="server" Text='<%# Eval("tipo_arredondamento") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Preço Ajustado" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrecoPromocaoItem" runat="server" Text='<%# Eval("preco_promocao","{0:N2}") %>'
                                        onchange="javascript:return calculaDesconto(this,'TabPanel1_gridProdutos')"
                                        Width="100px" CssClass="numero"
                                        onkeypress="javascript:return formataDouble(this,event);"
                                        onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Desconto/Acréscimo" ControlStyle-Width="160px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Panel ID="pnAcrescimo" runat="server">
                                        <label class="switch">
                                            <asp:CheckBox ID="chkAcrescimo" Checked='<%#bool.Parse(Eval("Acrescimo").ToString())%>' runat="server" AutoPostBack="true" OnCheckedChanged="chkAcrescimo_CheckedChanged1" />
                                            <span class="slider round"></span>
                                        </label>
                                    </asp:Panel>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
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
                    <asp:GridView ID="gridTabelaPrecoMargem" runat="server" CellPadding="4" ForeColor="#333333"
                        AutoGenerateColumns="False" GridLines="None" OnRowCommand="gridTabelaPrecoMargem_RowCommand">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                Text="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="descricao" HeaderText="Descrição">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Preco" HeaderText="Preço">
                                <ItemStyle Width="150px" />
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PrecoCusto" HeaderText="Preço Custo">
                                <ItemStyle Width="150px" />
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Margem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMargemItem" runat="server" Text='<%# Eval("Margem","{0:N2}") %>'
                                        onchange="javascript:return calculaMargemPreco(this,'TabPanel1_gridTabelaPrecoMargem')"
                                        Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arredodamento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoArredondamento" runat="server" Style="margin: 10px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoArredondamento_SelectedIndexChanged">
                                        <asp:ListItem Value="1">NÃO ARREDONDA</asp:ListItem>
                                        <asp:ListItem Value="2">TRUNCAMENTO</asp:ListItem>
                                        <asp:ListItem Value="3">ARREDONDAMENTO</asp:ListItem>
                                        <asp:ListItem Value="4">ARR SIG 0,5</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:Label ID="lblTipoConta" runat="server" Text='<%# Eval("tipo_arredondamento") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Preço Tabela" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrecoPromocaoItem" runat="server" Text='<%# Eval("Preco_promocao","{0:N2}") %>'
                                        onchange="javascript:return calcularPrecoMargem(this,'TabPanel1_gridTabelaPrecoMargem')"
                                        Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Selecionar Itens
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="modalFormTelaCheiaframeDiv" Style="height: 700px;"
                        DefaultButton="imgPesquisaMercadoria">
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
                                    <td>Grupo:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>SubGrupo:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSubGrupo" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Departamento:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Linha
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlLinha" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlLinha_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fornecedor
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtFornecedor" runat="server" Width="150px"></asp:TextBox>
                                        <asp:ImageButton
                                            ID="imgFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                            Width="15px" OnClick="imgFornecedor_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Plu/EAN/Descricao:
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
                                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona_CheckedChanged" />
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
                        <div id="divIncluirSelecionado" runat="server" class="btnImprimirDireita">
                            <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                                Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />Incluir
                            item
                        </div>
                        <br />
                        <center><h3><b>Selecionados </b></h3></center>
                        <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv" Style="height: 300px;">

                            <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                GridLines="Vertical" AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound"
                                OnRowCommand="gridItens_RowCommand" CssClass="table">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                        Text="Excluir">
                                        <ControlStyle Height="20px" Width="20px" />
                                        <ItemStyle Width="20px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="plu" HeaderText="PLU">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descricao" HeaderText="Descrição">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Preco" HeaderText="Preço">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescontoItem" runat="server" Text='<%# Eval("PositivoDesconto","{0:N2}") %>'
                                                Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"
                                                onchange="javascript:return calculaValor(this,'TabPanel2_gridMercadoriasSelecionadas')"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Arredodamento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTipoArredondamento" runat="server" Style="margin: 10px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoArredondamento_SelectedIndexChanged">
                                                <asp:ListItem Value="1">NÃO ARREDONDA</asp:ListItem>
                                                <asp:ListItem Value="2">TRUNCAMENTO</asp:ListItem>
                                                <asp:ListItem Value="3">ARREDONDAMENTO</asp:ListItem>
                                                <asp:ListItem Value="4">ARR SIG 0,5</asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:Label ID="lblTipoConta" runat="server" Text='<%# Eval("tipo_arredondamento") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preço Promoção" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrecoPromocaoItem" runat="server" Text='<%# Eval("preco_promocao","{0:N2}") %>'
                                                Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"
                                                onchange="javascript:return calculaDesconto(this,'TabPanel2_gridMercadoriasSelecionadas')"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desconto/Acréscimo" ControlStyle-Width="160px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Panel ID="pnAcrescimo" runat="server">
                                                <label class="switch">
                                                    <asp:CheckBox ID="chkAcrescimo" Checked='<%#bool.Parse(Eval("Acrescimo").ToString())%>' runat="server" AutoPostBack="true" OnCheckedChanged="chkAcrescimo_CheckedChanged1" />
                                                    <span class="slider round"></span>
                                                </label>
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
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

                            <asp:GridView ID="gridMercadoriasSelecionadoMargem" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" GridLines="None" OnRowCommand="gridMercadoriasSelecionadoMargem_RowCommand">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                        Text="Excluir">
                                        <ControlStyle Height="20px" Width="20px" />
                                        <ItemStyle Width="20px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="plu" HeaderText="PLU">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descricao" HeaderText="Descrição">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Preco" HeaderText="Preço">
                                        <ItemStyle Width="150px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PrecoCusto" HeaderText="Preço Custo">
                                        <ItemStyle Width="150px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Margem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMargemItem" runat="server" Text='<%# Eval("Margem","{0:N2}") %>'
                                                onchange="javascript:return calculaMargemPreco(this,'TabPanel2_gridMercadoriasSelecionadoMargem')"
                                                Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Arredodamento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTipoArredondamento" runat="server" Style="margin: 10px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoArredondamento_SelectedIndexChanged">
                                                <asp:ListItem Value="1">NÃO ARREDONDA</asp:ListItem>
                                                <asp:ListItem Value="2">TRUNCAMENTO</asp:ListItem>
                                                <asp:ListItem Value="3">ARREDONDAMENTO</asp:ListItem>
                                                <asp:ListItem Value="4">ARR SIG 0,5</asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:Label ID="lblTipoConta" runat="server" Text='<%# Eval("tipo_arredondamento") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preço Tabela" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrecoPromocaoItem" runat="server" Text='<%# Eval("Preco_promocao","{0:N2}") %>'
                                                onchange="javascript:return calcularPrecoMargem(this,'TabPanel2_gridMercadoriasSelecionadoMargem')"
                                                Width="100px" CssClass="numero" onkeypress="javascript:return formataDouble(this,event);" onkeydown="javascript:return autoTab(this,event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
        <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
            <div class="cabMenu" style="overflow: auto; min-height: 100px;">
                <h2>
                    <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
                </h2>
            </div>
            <table class="frame" style="width: 100%;">
                <tr>
                    <td>
                        <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                            <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                                Font-Size="Larger" OnClick="btnOkError_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
        </asp:ModalPopupExtender>
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
            DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
        </asp:ModalPopupExtender>

        <asp:Panel ID="PnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none; padding: 5px">
            <h3>
                <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir a Tabela?"
                    CssClass="cabMenu"></asp:Label>
            </h3>
            <table>
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmarInativar" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaExclusao_Click" />
                        <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaInativar" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaExclusao_Click" />
                        <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupControlID="PnConfirmaExcluir" TargetControlID="Label14">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnHelpArredondar" runat="server" CssClass="frameModal" Style="display: none; padding: 5px; overflow:auto">
            <div class="row">
                <h3>TIPOS DE ARREDONDAMENTO</h3>
                <ol>
                    <li>SEM ARREDONDAMENTO </li>
                    <li>TRUNCAMENTO (9,96 = 9,90)</li>
                    <li>ARREDONDAMENTO (9,96 = 10,00)</li>
                    <li>ARR SIG 0,5 (9,93 = 10,00)</li>
                </ol>
            </div>
            <div class="row">
                <div class="col1">
                    <asp:ImageButton ID="ImgBtnFecharHelp" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnFecharHelp_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Fechar"></asp:Label>
                </div>

            </div>
                        
        </asp:Panel>
        <asp:ModalPopupExtender ID="modalHelpArredondar" runat="server" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupControlID="pnHelpArredondar" TargetControlID="Label6">
        </asp:ModalPopupExtender>
    </div>
</asp:Content>
