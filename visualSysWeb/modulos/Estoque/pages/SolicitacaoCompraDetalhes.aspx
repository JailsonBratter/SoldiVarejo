<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SolicitacaoCompraDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.SolicitacaoCompraDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Solicitação de Compra Detalhes</h1>
        </center>
    </div>
    <div id="divPage" runat="server">
        <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
        </asp:Panel>
        <br />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div id="cabecalho" runat="server" class="frame">
            <table>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>
                                Tipo</p>
                            <asp:DropDownList ID="ddlTipoSolicitacao" runat="server" Font-Size="X-Large">
                                <asp:ListItem>Manual</asp:ListItem>
                                <asp:ListItem>Automatica</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <p>
                                Codigo</p>
                            <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Descrição</p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="250px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Data</p>
                            <asp:TextBox ID="txtDataCadastro" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Usuario</p>
                            <asp:TextBox ID="txtUsuarioCadastro" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Status</p>
                            <asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click"
                                CssClass="submitButton" Height="40px" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="conteudo" runat="server" class="conteudo">
            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                    <HeaderTemplate>
                        Itens Adicionados</HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="addItensDig" runat="server" CssClass="filter" DefaultButton="imgPlu">
                            <table>
                                <tr>
                                    <td>
                                        <p>
                                            PLU/EAN</p>
                                        <asp:TextBox ID="txtPlu" runat="server" Width="150px"></asp:TextBox><asp:ImageButton
                                            ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </td>
                                    <td>
                                        <p>
                                            Descrição</p>
                                        <asp:TextBox ID="txtDescricaoItem" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <p>
                                            SaldoAtual</p>
                                        <asp:TextBox ID="TxtSaldoAtual" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <p>
                                            Vlr Unit</p>
                                        <asp:TextBox ID="txtCusto" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <p>
                                            <asp:Label ID="lblContado" runat="server" Text="Qtde"></asp:Label></p>
                                        <asp:TextBox ID="txtContado" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            Width="20px" OnClick="ImgBtnAddItens_Click" />Incluir item
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
                            <h1>
                                <asp:Label ID="lblRegistros" runat="server"></asp:Label></h1>
                        </div>
                        <div class="direitaFechar">
                            <asp:Button ID="btnItensDigitados" runat="server" Text="Digitados" Font-Size="Large" OnClick="btnDigitado_Click" />
                            <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
                        </div>
                        <div class="gridTable">
                            <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                                AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand" CssClass="table"
                                OnRowEditing="gridItens_RowEditing">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="ordem" HeaderText="Ordem"></asp:BoundField>
                                    <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                    <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                    <asp:BoundField DataField="ref_fornecedor" HeaderText="Ref"></asp:BoundField>
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                    <asp:BoundField DataField="Und" HeaderText="Und"></asp:BoundField>
                                    <asp:BoundField DataField="Embalagem" HeaderText="Emb"></asp:BoundField>
                                    <asp:BoundField DataField="CTR" HeaderText="Contrato">
                                        <ItemStyle Font-Size="X-Large" Font-Bold="true"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Saldo" HeaderText="Saldo Atual">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Preco_Custo" HeaderText="Preco Custo ">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cob_cad" HeaderText="cob cad">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sugestao" HeaderText="Sugestao">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Qtde Comprar">
                                        <ItemTemplate>
                                            <center>
                                        <asp:TextBox ID="txtGridQtde" runat="server" Text='<%# Eval("Qtde_Comprar") %>' Width="80px" OnKeyPress="javascript:return autoTab(this,event);"
                                            CssClass="numero"   ></asp:TextBox></center>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecionaItens_CheckedChanged"
                                                Checked="true" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="true" AutoPostBack="true"
                                                OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                                            <asp:Label ID="lblaceitaSolicitacao" Text='<%# Eval("ACEITA_SUG") %>' runat="server"
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
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
                                            <asp:Label ID="lblGrupo" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtGrupo" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                Width="15px" OnClick="imgBtnGrupo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SubGrupo:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSubGrupo" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtSubGrupo" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" Width="15px" OnClick="imgBtnSubGrupo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Departamento:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblDepartamento" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtDepartamento" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" Width="15px" OnClick="imgBtnDepartamento_Click" />
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
                                            <asp:TextBox ID="txtFornecedor" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton
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
                                            Listas Salvas
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcodListaPadrao" runat="server" Width="30px" AutoPostBack="true" OnTextChanged="txtcodListaPadrao_TextChanged" />  
                                            <asp:TextBox ID="txtDescricaoListaPadrao" runat="server" Width="150px" />  
                                             <asp:ImageButton
                                                ID="imgBtnListaPadrao" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                Width="15px" OnClick="imgBtnListaPadrao_Click" />
                                        </td>
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
                                        <td>
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
                                <asp:Button ID="btnSalvarListaPadrao" Text="Salvar Lista" runat="server" OnClick="btnSalvarListaPadrao_Click"  style="margin-left:30px;"/>
                                <div class="gridTableTelaCheia">
                                    <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                        GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand="gridItensSelecao_RowCommand"
                                        CssClass="table">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ordem" HeaderText="Ordem"></asp:BoundField>
                                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                            <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                            <asp:BoundField DataField="ref_fornecedor" HeaderText="Ref"></asp:BoundField>
                                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                            <asp:BoundField DataField="Und" HeaderText="Und"></asp:BoundField>
                                            <asp:BoundField DataField="Embalagem" HeaderText="Emb"></asp:BoundField>
                                            <asp:BoundField DataField="CTR" HeaderText="Contrato"></asp:BoundField>
                                            <asp:BoundField DataField="Saldo" HeaderText="Saldo Atual">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
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
                            </asp:Panel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                    <HeaderTemplate>
                        Pedido/Cotacoes</HeaderTemplate>
                    <ContentTemplate>
                        <div id="Div2" runat="server" class="modalFormTelaCheiaframeMini" style="height: 250px;
                            width: 45%;">
                            <center><h3><b>Pedidos </b></h3></center>
                            <hr />
                            <asp:GridView ID="gridPedidos" runat="server" ForeColor="#333333" GridLines="Vertical"
                                AutoGenerateColumns="False" Width="100%" OnRowCommand="gridPedido_RowCommand"
                                CssClass="table">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:HyperLinkField DataTextField="Pedido" Text="---" Visible="true" HeaderText="Pedido"
                                        Target="_blank" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="pedido" />
                                    <asp:HyperLinkField DataTextField="Fornecedor" Text="---" Visible="true" HeaderText="Fornecedor"
                                        Target="_blank" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="pedido" />
                                    <asp:HyperLinkField DataTextField="status" Text="---" Visible="true" HeaderText="Status"
                                        Target="_blank" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="pedido" />
                                    <asp:HyperLinkField DataTextField="Data_cadastro" Text="---" Visible="true" HeaderText="Data Cadastro"
                                        Target="_blank" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="pedido" />
                                    <asp:HyperLinkField DataTextField="Total" Text="---" Visible="true" HeaderText="Total"
                                        Target="_blank" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="pedido" />
                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/email.png" CommandName="Email"
                                        HeaderText="Enviar Email" Text="Email"></asp:ButtonField>
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
                        <div id="Div3" runat="server" class="modalFormTelaCheiaframeMini" style="height: 250px;
                            width: 45%;">
                            <center><h3><b>Cotações </b></h3></center>
                            <hr />
                            <asp:GridView ID="gridCotacoes" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="false" Width="100%" GridLines="None">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:HyperLinkField DataTextField="Cotacao" Text="----" Visible="true" Target="_blank"
                                        HeaderText="Cotacao" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="cotacao" />
                                    <asp:HyperLinkField DataTextField="descricao" Text="----" Visible="true" Target="_blank"
                                        HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="cotacao" />
                                    <asp:HyperLinkField DataTextField="data" Text="----" Visible="true" Target="_blank"
                                        HeaderText="Data" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="cotacao" />
                                    <asp:HyperLinkField DataTextField="status" Text="----" Visible="true" Target="_blank"
                                        HeaderText="Status" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}"
                                        DataNavigateUrlFields="cotacao" />
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
            </asp:TabContainer>
        </div>
    </div>
    <div class="frame">
    <table style="width:100%">
        <tr>
            <td>
            
            
        <asp:Panel ID="pnCotacaoEscolha" runat="server" Visible="false" CssClass="modalFormTelaInteiraConfirma"
            Height="500px">
            <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
                <div style="width: 500px; float: left; ">
                    <h2>
                        Escolha as opções</h2>
                </div>
                
                <div class="Paneldireita" style="float:right;">
                    <asp:ImageButton ID="imgBtnFechar" ImageUrl="~/img/cancel.png" runat="server" Width="20px"
                        OnClick="imgBtnFechar_Click" />
                    Cancela
                </div>
                <div class="Paneldireita" style="float:right;">
                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/confirm.png"
                            Width="20px" OnClick="imgBtnConfirmaContratosAtivos_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Confirma"></asp:Label>
                </div>
            </div>
            <br />
            <div style="width: 45%; float: left;">
                <div>
                    <h3>
                        <asp:Label ID="Label9" runat="server" Text="Gerar pedidos para os Itens com contratos Ativos ?"
                            CssClass="cabMenu"></asp:Label>
                    </h3>
                </div>
                <div class="gridTable" style="overflow: auto; height: 300px">
                    <asp:GridView ID="gridContratos" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                        Checked="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ordem" HeaderText="Ordem"></asp:BoundField>
                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                            <asp:BoundField DataField="Embalagem" HeaderText="Emb"></asp:BoundField>
                            <asp:BoundField DataField="Preco_Custo" HeaderText="Preco"></asp:BoundField>
                            <asp:BoundField DataField="Qtde_Comprar" HeaderText="Qtde"></asp:BoundField>
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
                    <div id="divSemItensContrato" runat="server" visible ="false">
                        <h1>NÃO FOI ENCONTRADO ITENS COM CONTRATO!!</h1>
                    
                    </div>
                </div>
            </div>
            <div style="width: 50%; margin-left: 2%; float: left;">
                <h3>
                <asp:Label ID="Label10" runat="server" Text="Incluir em uma cotação"
                            CssClass="cabMenu"></asp:Label>
                    
                </h3>
                <div class="gridTable" style="overflow: auto; height: 300px">
                    <asp:GridView ID="gridCotacaoAberta" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                        GridLines="Vertical" AllowSorting="True" OnRowDataBound="gridCotacaoAberta_RowDataBound"
                        Width="100%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="COTACAO" HeaderText="COTAÇÃO" ReadOnly="true" SortExpression="PLU">
                            </asp:BoundField>
                            <asp:BoundField DataField="DATA" HeaderText="DATA" ReadOnly="true" SortExpression="EAN">
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRICAO" HeaderText="DESCRIÇÃO" ReadOnly="true" SortExpression="DESCRICAO">
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrCotacaoItem" />
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
            </div>
        </asp:Panel>

        </td>
        </tr>
    </table>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none; height:400px;"
        DefaultButton="ImgPesquisaLista" >
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" Style="height:90%" DefaultButton="ImgPesquisaLista">
           
            <center><h1><asp:Label ID="lbltituloLista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
               
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>

            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista" Style="height:60%" >
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
        CancelControlID="LblCancelaLista" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Encerrar A Solicitação?"
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
  
    <asp:Panel ID="pnConfirmaExcluirItem" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label6" runat="server" Text="Tem Certeza que gostaria de Excluir o Item?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <asp:Label ID="lblPluExcluir" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lbllinhaGrid" runat="server" Text="" Visible="false"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExluirItem" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExluirItem_Click" />
                    <asp:Label ID="Label7" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExluirItem" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExluirItem_Click" />
                    <asp:Label ID="Label8" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirItem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirItem" TargetControlID="Label6">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaEmail" runat="server" CssClass="frameModal" Style="display: none">
        <div class="btnImprimirDireita">
            <asp:ImageButton ID="imgFecharEmail" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />Fechar
        </div>
        <h2>
            <asp:Label ID="lblPedidoEmail" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblMensagemEmail" runat="server" Text="Tem Certeza que gostaria de Enviar o email "
                CssClass="cabMenu"></asp:Label>
        </h2>
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="btnEnviarEmail" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnEnviarEmail_Click" />
                    <asp:Label ID="lblSimEmail" runat="server" Text="SIM"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="btnCancelaEmail" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancela_Click" />
                    <asp:Label ID="lblNaoEmail" runat="server" Text="NÃO"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaEmail" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaEmail" TargetControlID="lblMensagemEmail"
        CancelControlID="imgFecharEmail">
    </asp:ModalPopupExtender>


     <asp:Panel ID="pnSalvarListaPadrao" runat="server" CssClass="frameModal" Style="display: none; width:400px;">
       
        <h2>
           
            <asp:Label ID="Label4" runat="server" Text="Informe o nome da nova Lista"
                CssClass="cabMenu"></asp:Label>
        </h2>
         
         <asp:TextBox ID="txtNomeNovaListaPadrao" runat="server" Width="95%" Font-Size="X-Large" style="margin-left:10px; margin-bottom:10px;" /> 
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="imgBtnSalvarListaPadrao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnSalvarListaPadrao_Click" />
                    <asp:Label ID="Label5" runat="server" Text="SALVAR"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="imgBtnCancelaListaPadrao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" />
                    <asp:Label ID="Label12" runat="server" Text="CANCELAR"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSalvarListaPadrao" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSalvarListaPadrao" TargetControlID="Label4"
        CancelControlID="imgBtnCancelaListaPadrao">
    </asp:ModalPopupExtender>
      <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
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
</asp:Content>
