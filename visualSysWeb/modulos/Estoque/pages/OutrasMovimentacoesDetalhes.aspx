<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OutrasMovimentacoesDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.OutrasMovimentacoesDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/OutrasMovimentacoesDetalhes.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Outras Movimentações Detalhes</h1>
        </center>
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
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigo_inventario" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Tipo de Mov
                    </p>
                    <asp:DropDownList ID="ddTipo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddTipo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Descrição
                    </p>
                    <asp:TextBox ID="txtDescricao_inventario" runat="server" Width="300px" MaxLength="50" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data
                    </p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtUsuario" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Status
                    </p>
                    <asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click"
                        CssClass="submitButton" Height="40px" />
                    <div id="divBtnImprimirContagem" runat="server" class="panelItem">
                        <asp:ImageButton ID="imgBtnImprimir" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                            Height="50px" OnClick="imgBtnImprimir_Click" />
                        Contagem
                    </div>
                    <div id="divBtnImprimirConferencia" runat="server" class="panelItem">
                        <asp:ImageButton ID="imgBtnConferencia" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                            Height="50px" OnClick="imgBtnConferencia_Click" />
                        Conferencia
                    </div>
                    <div id="divBtnImprimirEncerrado" runat="server" class="panelItem">
                        <asp:ImageButton ID="imgBtnImpEncerrado" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                            Height="50px" OnClick="imgBtnImpEncerrado_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="row">
        <asp:UpdatePanel ID="updPnl" runat="server">
            <ContentTemplate>
                <div id="progressContainer" style="width: 100%; background-color: #ddd; border-radius: 4px;">
                    <asp:Panel ID="progressBar" runat="server" Style="height: 30px; width: 0%; background-color: #f0ad4e; text-align: center; color: white; line-height: 30px; border-radius: 4px;">
                        <asp:Literal ID="litPercentual" runat="server" />
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick" Enabled="false" />
    </div>

    <div id="divAvisoTipo" runat="server" visible="true" class="row">
        <div style="border-style: solid; border-width: 1px; float: left; background: #FFFACD; margin-left: 30px; margin-top: 30px; margin-bottom: 20px;">
            <b>ATENÇÃO!</b> Para a adicionar os itens escolha primeiro o tipo de movimentação.
        </div>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens Adicionados
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItens" runat="server" CssClass="filter" DefaultButton="imgPlu">
                        <table>
                            <tr>
                                <td>
                                    <p>
                                        PLU/EAN
                                    </p>
                                    <asp:TextBox ID="txtPlu" runat="server" CssClass="sem" Width="150px"></asp:TextBox><asp:ImageButton
                                        ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                        OnClick="imgPlu_Click" />
                                </td>
                                <td>
                                    <p>
                                        Descrição
                                    </p>
                                    <asp:TextBox ID="txtDescricao" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        SaldoAtual
                                    </p>
                                    <asp:TextBox ID="TxtSaldoAtual" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        Vlr Unit
                                    </p>
                                    <asp:TextBox ID="txtCusto" runat="server" CssClass="sem" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        <asp:Label ID="lblContado" runat="server" Text="Contado"></asp:Label>
                                    </p>
                                    <asp:TextBox ID="txtContado" runat="server" CssClass="sem" Width="80px"></asp:TextBox>
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
                        <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                        <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                        <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                        <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
                    </div>
                    <div class="gridTable">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" 
                            OnRowCommand="gridItens_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="ID" HeaderText="Ordem"></asp:BoundField>
                                <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                <asp:BoundField DataField="Saldo_atual" HeaderText="Saldo Atual">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Custo" HeaderText="Vlr Unit">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Contado">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txtGridContado" runat="server" Text='<%# Eval("Contada", "{0:N3}") %>' Width="80px" OnChange="atualizarDiferenca(this)"
                                            CssClass="numero"   ></asp:TextBox></center>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtde">
                                    <ItemTemplate>
                                        <center><asp:TextBox ID="txtGridQtde" runat="server" Text='<%# Eval("Qtde", "{0:N3}") %>' Width="80px" OnKeyPress="javascript:return autoTab(this,event);"  
                                            CssClass="numero"   ></asp:TextBox></center>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Diferenca" HeaderText="Diferenca">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                    Text="Alterar">
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Coletor
                </HeaderTemplate>
                <ContentTemplate>

                    <div style="border-style: solid; border-width: 1px; width: 300px; float: left; background: #FFFACD; margin-left: 20px; margin-top: 30px;">
                        <b>ATENÇÃO!</b> Para a utilização do coletor é necessário a configuração previa do arquivo
                        <a href="javascript:abrePopupCofigColetor();">Configurar Agora</a><br />
                        <br />
                        Duvidas entrar em contato com a Bratter.
                   </div>
                    <div class="panel" style="width: 55%; float: left; margin-left: 20px;">
                        <h3>Passos para importar Arquivo:</h3>
                        <div class="panelItem">
                            <asp:ImageButton ID="imgBuscaArquivo" runat="server" ImageUrl="~/img/arquivo-txt-Grande.png"
                                Width="50px" OnClick="imgBuscaArquivo_Click" />
                        </div>
                        <div class="panelItem" style="line-height: 50px;">
                            <asp:Label ID="lblIdentificarArquivo" runat="server" Text="1º Identificar Arquivo"
                                Width="200px"></asp:Label>
                            <asp:DropDownList ID="ddlArquivos" runat="server" Visible="false" Width="200px">
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <asp:ImageButton ID="imgLeArquivo" runat="server" ImageUrl="~/img/arquivo-download-grande.png"
                                Width="50px" OnClick="imgLeArquivo_Click" />
                        </div>
                        <div class="panelItem" style="line-height: 50px;">
                            2º Ler Arquivo Selec
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel5">
                <HeaderTemplate>
                    Comanda Eletrônica
                </HeaderTemplate>
                <ContentTemplate>

                    <div style="border-style: solid; border-width: 1px; width: 300px; float: left; background: #FFFACD; margin-left: 20px; margin-top: 30px;">
                        <b>ATENÇÃO!</b> Não esquecer de LIMPAR a comanda após conclusão da tarefa.
                        <br />
                        Duvidas entrar em contato com a Bratter.
                   </div>
                    <div class="panel" style="width: 55%; float: left; margin-left: 20px;">
                        <h3>Passos para importar a comanda:</h3>
                        <div class="panelItem">
                            <asp:TextBox ID="txtNumeroComanda" runat="server" Width="100px"/>
                            <p>1º Digite o número da comanda na caixa de texto.</p>
                        </div>
                        <div class="panelItem">
                            <asp:ImageButton ID="imgLerComanda" runat="server" ImageUrl="~/img/arquivo-download-grande.png"
                                Width="50px" OnClick="imgLerComanda_Click" />
                        </div>
                        <div class="panelItem" style="line-height: 50px;">
                            2º Clique na imagem para importar os itens da comanda
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3">
                <HeaderTemplate>
                    Opções
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:CheckBox ID="chkDepartamento" runat="server" AutoPostBack="True" Text="Quebrar Itens Departamento"
                        OnCheckedChanged="chkDepartamento_CheckedChanged" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div id="divInventarioCompleto" runat="server" class="panelItem">
                        <p>
                            <asp:CheckBox ID="chkInventarioCompleto" runat="server" Text="Zerar Itens não informados no inventario" />
                            <div id="divAvisoInventarioCompleto" runat="server" style="border-style: solid; border-width: 1px; width: 50%; float: left; background: #FFFACD;">
                                <b>ATENÇÃO!</b> Ao selecionar essa opção todos os produtos que não estão informados
                                no inventario terão o estoque zerado.
                            </div>
                        </p>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
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
                                    <td>Categoria:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlCategoria" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Seguimento:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSeguimento" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlSeguimento_SelectedIndexChanged">
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
                                    <td>Marca
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtMarca" runat="server" Width="150px"></asp:TextBox>
                                        <asp:ImageButton
                                            ID="imgMarca" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                            Width="15px" OnClick="imgMarca_Click" />
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
                                    <td>Listas Salvas
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtcodListaPadrao" runat="server" Width="20%" AutoPostBack="true" OnTextChanged="txtcodListaPadrao_TextChanged" />
                                        <asp:TextBox ID="txtDescricaoListaPadrao" runat="server" Width="60%" />
                                        <asp:ImageButton
                                            ID="imgBtnListaPadrao" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                            Width="15px" OnClick="imgBtnListaPadrao_Click" />
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
                                        <asp:Button ID="btnCriarInventarioCompleto" runat="server" Height="40px" OnClick="btnCriarInventarioCompleto_Click"
                                            Text="Inventario Completo" />
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
                        <div class="btnImprimirDireita">
                            <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                                Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />Incluir
                            item
                        </div>
                        <br />
                        <center><h3><b>Selecionados </b></h3></center>
                        <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv" Style="height: 300px;">
                            <div class="gridTableTelaCheia">
                                <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                    GridLines="Vertical" AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound"
                                    OnRowCommand="gridItens_RowCommand" CssClass="table">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                        <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                        <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                        <asp:BoundField DataField="Saldo_atual" HeaderText="Saldo Atual">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Custo" HeaderText="Vlr Unit">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Contada" HeaderText="Contado">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qtde" HeaderText="Qtde">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Diferenca" HeaderText="Diferenca">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Total">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                            Text="Alterar">
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
        </asp:TabContainer>
    </div>
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
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Encerrar A Movimentacao?"
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

    <asp:Panel ID="pnIventarioCompleto" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <b>
                <asp:Label ID="Label4" runat="server" Text="Tem certeza que gostaria de incluir todos os itens cadastrados no iventario ?"
                    CssClass="cabMenu"></asp:Label>
            </b>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaIventarioCompleto_Click" />
                    <asp:Label ID="Label5" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnConfirmaIventarioCompleto_Click" />
                    <asp:Label ID="Label6" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalIventarioCompleto" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnIventarioCompleto" TargetControlID="Label4">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnAguarde" runat="server" CssClass="frameModal" Style="display: none">
        <div class="frame">
            <h1>
                <asp:Label ID="lblDetalhesAguarde" runat="server"></asp:Label></h1>
            <br />
            <center><img src="../../../img/aguarde.gif" /></center>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalaguarde" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnAguarde" TargetControlID="lblDetalhesAguarde">
    </asp:ModalPopupExtender>
    <asp:Timer ID="TimerAguarde" runat="server" OnTick="TimerAguarde_Tick" Enabled="false"
        Interval="30000">
    </asp:Timer>
    <asp:Timer ID="TimerImportaColetor" runat="server" OnTick="TimerImportaColetor_Tick" Enabled="false"
        Interval="1000">
    </asp:Timer>
</asp:Content>
