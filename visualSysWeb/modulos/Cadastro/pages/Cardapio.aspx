<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cardapio.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Cardapio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/Cardapio.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Cardápio</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div id="divSemCardapio" runat="server" class="semCardapio">
        <h1>Ainda não foi cadastrado um Cardapio para essa Filial </h1>
        <asp:Button Text="Incluir Novo Cardapio?" runat="server" CssClass="btn btn-incluir" OnClick="btnIncluir_Click" />
    </div>
    <div id="divConteudo" runat="server" class="conteudo" visible="false">
        <div class="cabecalho">

            <div class="row">
                <div class="panelItem">
                    <p>Filial</p>
                    <asp:TextBox ID="txtFilial" runat="server" Width="90%" />
                </div>
                <div class="panelItem">
                    <p>Data Cadastro</p>
                    <asp:TextBox ID="txtDtCadastro" runat="server" Width="90%" />
                </div>
                <div class="panelItem">
                    <p>Data Ultima Alteração</p>
                    <asp:TextBox ID="TxtDtAlteracao" runat="server" Width="90%" />
                </div>
                <div class="panelItem">
                    <p>Usuario Cadastro</p>
                    <asp:TextBox ID="txtUsuarioCadastro" runat="server" Width="90%" />
                </div>
                <div class="panelItem">
                    <p>Usuario Ult Alteração</p>
                    <asp:TextBox ID="txtUsuarioUltAlteracao" runat="server" Width="90%" />
                </div>

            </div>
            <div class="row">
                <div class="panelItem url">
                    <p>Url Padrão Cardápio</p>
                    <asp:TextBox ID="txtUrlPadrao" runat="server" Width="100%" CssClass="sem" />
                </div>
                

                <div id="divAtualizarApi" runat="server" class="panelItem btn" >
                    <asp:ImageButton ImageUrl="~/img/arquivo-upload.png" ID="ImgBtnEnviarAPI" runat="server" OnClick="ImgBtnEnviarAPI_Click" />
                    Atualizar API
                </div>
                <div  runat="server" class="panelItem btn">
                    <asp:ImageButton ImageUrl="~/img/senha.png" ID="btnImgToken" runat="server" OnClick="ImgBtnToken_Click" />
                    Token
                </div>
            </div>
            <div id="divToken" runat="server" visible="false" class="row">
                <div class="panelItem url">
                    <p>Token Acesso</p>
                    <asp:TextBox ID="txtToken" runat="server" Width="95%" Height="100px" TextMode ="MultiLine" CssClass="sem" />
                </div>
            </div>

        </div>
        <div class="col colEsquerda">
            <div class="row">
                <h2>Categorias</h2>
            </div>
            <hr />
            <div class="row">
                <div id="divIncluirNovaCategoria" runat="server" class="panelItem">
                    <asp:ImageButton ImageUrl="~/img/add.png" ID="ImgAddCategoria" runat="server" OnClick="ImgAddCategoria_Click" />
                    Incluir Nova Categoria      
                </div>
            </div>
            <div id="divSemCategorias" runat="server" class="semCardapio semCategoria">
                Sem Categorias Cadastradas!
            </div>
            <div class="table">
                <asp:GridView ID="gridCategoria" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gridCategoria_RowDataBound" >
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="1%">
                            <ItemTemplate>
                                <asp:RadioButton ID="rdoCategoria" runat="server" GroupName="GrCategoria" AutoPostBack="true" OnCheckedChanged="rdoCategoria_CheckedChanged"></asp:RadioButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                        <asp:BoundField DataField="PizzaStr" HeaderText="Pizza" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                     
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
        <div class="col colDireita">
            <div>
                <h2>Detalhes 
                </h2>
                <hr />
                <div class="row">
                    <div class="panelItem">
                        <p>Id</p>
                        <asp:TextBox ID="txtIdCategoria" runat="server" Width="80px" />
                    </div>
                    <div class="panelItem">
                        <p>Categoria</p>
                        <asp:TextBox ID="txtCategoria" runat="server" Width="250px" AutoPostBack="true" OnTextChanged="txtCategoria_TextChanged" />
                    </div>
                    <div class="panelItem">
                        <p>Status</p>
                        <asp:DropDownList ID="ddlStatusCatgoria" runat="server" Height="20px" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusCatgoria_SelectedIndexChanged">
                            <asp:ListItem Text="Ativa" Value="ativa" />
                            <asp:ListItem Text="Oculta" Value="oculta" />
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>Pizza</p>
                        <asp:DropDownList ID="ddlPizza" runat="server" Height="20px" AutoPostBack="true" OnSelectedIndexChanged="ddlPizza_SelectedIndexChanged">
                            <asp:ListItem Text="Não" Value="0" />
                            <asp:ListItem Text="Meia" Value="2" />
                            <asp:ListItem Text="Terco" Value="3" />
                        </asp:DropDownList>
                    </div>
                    <div id="divCatMeia" runat="server" class="panelItem" visible="false">
                        <p>Categoria Meia</p>
                        <asp:TextBox ID="txtIdCategoriaMeia" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtCategoria_TextChanged" />
                        <asp:ImageButton ID="imgBtnCategoriaMeia" ImageUrl="~/img/pesquisaM.png" runat="server" CssClass="imgPesq" OnClick="imgBtnPesquisa_Click" />

                    </div>
                    <div id="divCatTerco" runat="server" class="panelItem" visible="false">
                        <p>Categoria Terco</p>
                        <asp:TextBox ID="txtIdCategoriaTerco" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtCategoria_TextChanged" />
                        <asp:ImageButton ID="imgBtnCategoriaTerco" ImageUrl="~/img/pesquisaM.png" runat="server" CssClass="imgPesq" OnClick="imgBtnPesquisa_Click" />
                    </div>
                </div>

            </div>
            <div class="row">
                <h2>
                    Produtos 
                </h2>
               
            </div>
            <hr>
            <div class="row">
                <div id="divIncluirProdutos" runat="server" class="panelItem">
                    <asp:ImageButton ID="ImgBtnAddProduto" OnClick="ImgBtnAddProduto_Click" ImageUrl="~/img/add.png" runat="server" />
                    Incluir produtos      
                </div>
            </div>
            <div id="divSemProdutos" runat="server" class="semCardapio">
                Sem Produtos Cadastrados!
            </div>
            <div class="table">
                <asp:GridView ID="gridProdutos" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="gridProdutos_RowCommand">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="PLU" HeaderText="PLU" />
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                        <asp:BoundField DataField="Preco" HeaderText="Preço" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField HeaderText="Obs" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkBtnObservacoes" runat="server" Text='<%# Eval("Obs") %>' OnClick="linkBtnObservacoes_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ativo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAtivo" Text="" runat="server" Checked='<%# Eval("Ativo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Preço por Obs" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPrecoPorObs" runat="server" SelectedValue='<%# Eval("precoPorObs") %>'>
                                    <asp:ListItem Text="Acrescenta valor" Value="1" />
                                    <asp:ListItem Text="Maior valor" Value="2" />
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
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
        </div>
    </div>

    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px; overflow:auto">
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
    <asp:ModalPopupExtender ID="modalError" runat="server"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnAddCategoria" runat="server" CssClass="frameModal" Style="display: none; width: 80%;" DefaultButton="ImgBtnConfirmaNovaCategoria">
        <div class="frame">

            <div class="cabMenu" style="height: 80px;">
                <h2>
                    <asp:Label ID="lblTituloCategoria" runat="server" Text="Nova Categoria"></asp:Label>
                </h2>
            </div>
            <div class="row">
                <div class="panel">

                    <div class="panelItem">
                        <p>Id</p>
                        <asp:TextBox ID="txtIdNovaCategoria" runat="server" Width="80px" />
                    </div>
                    <div class="panelItem">
                        <p>Nome</p>
                        <asp:TextBox ID="txtTituloNovaCategoria" runat="server" Width="300px" />
                    </div>
                    <div class="panelItem">
                        <p>Pizza</p>
                        <asp:DropDownList ID="DdlnovaCategoriaPizza" runat="server" Height="20px">
                            <asp:ListItem Text="Não" Value="0" />
                            <asp:ListItem Text="Meia" Value="2" />
                            <asp:ListItem Text="Terco" Value="3" />
                        </asp:DropDownList>
                    </div>
                    <div id="div1" runat="server" class="panelItem">
                        <p>Categoria Meia</p>
                        <asp:TextBox ID="txtNovaCategoriaMeia" runat="server" Width="80px" />
                        <asp:ImageButton ID="imgBtnNovaCategoriaMeia" ImageUrl="~/img/pesquisaM.png" runat="server" CssClass="imgPesq" OnClick="imgBtnPesquisa_Click" />

                    </div>
                    <div id="div2" runat="server" class="panelItem">
                        <p>Categoria Terco</p>
                        <asp:TextBox ID="txtNovaCategoriaTerco" runat="server" Width="80px" />
                        <asp:ImageButton ID="imgBtnNovaCategoriaTerco" ImageUrl="~/img/pesquisaM.png" runat="server" CssClass="imgPesq" OnClick="imgBtnPesquisa_Click" />
                    </div>
                </div>

            </div>

            <div class="row botoes">
                <div class="panelItem btn">
                        <asp:ImageButton ID="ImgBtnConfirmaNovaCategoria" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnIncluirNovaCategoria_Click" />
                        Incluir
                
                    </div>
                <div class="panelItem btn">
                    <asp:ImageButton ID="ImgBtnCancelaNovaCategoria" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarNovaCategoria_Click" />Cancelar
                </div>

            </div>
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalNovaCategoria" runat="server"
        DropShadow="true" PopupControlID="PnAddCategoria" TargetControlID="lblTituloCategoria">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
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
                        Width="25px" OnClick="btnFechar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
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
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="frameModal" Style="display: none;"
        DefaultButton="imgPesquisaMercadoria">
        <h2>
            <asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" CssClass="cabMenuLista"></asp:Label></h2>
        <br />
        <div class="row botoes">
            <div class="panelItem btn">
                <asp:ImageButton ID="ImgBtnConfirmarIncluirProdutos" runat="server" ImageUrl="~/img/confirm.png"
                    Width="25px" OnClick="BtnConfirmarIncluirProdutos_Click" />
                Confirmar

            </div>
            <div class="panelItem btn">
                <asp:ImageButton ID="ImgBtnCancelarIncluirProdutos" runat="server" ImageUrl="~/img/cancel.png"
                    Width="25px" OnClick="BtnCancelarIncluirProdutos_Click" />Cancelar
            </div>
        </div>
        <div id="divMerc" runat="server" class="modalFormTelaCheiaframeMini" style="height: 250px;">
            <table>
                <tr>
                    <td colspan="2">
                        <center><b>Filtrar</b></center>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>Grupo:
                    </td>
                    <td>
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
                    <td>
                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Width="200px"
                            OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Plu/EAN/Descricao:
                    </td>
                    <td>
                        <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                            autocomplete="off">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="20px" OnClick="ImgPesquisaMercadoria_Click" />
                        Filtrar
                    </td>
                    <td>
                        <asp:ImageButton ID="imgLimpar" runat="server" ImageUrl="~/img/botao-apagar.png"
                            Height="20px" OnClick="imgLimpar_Click" />
                        Limpar
                    </td>

                </tr>
            </table>
        </div>
        <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe"
            Style="height: 250px;">
            <div class="gridTableTelaCheia">
                <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="false">
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
                        <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                        <asp:BoundField DataField="PRC VENDA" HeaderText="PRC VENDA">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
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
        <div class="btnImprimirDireita">
            <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />
            Incluir item
        </div>
        <br />
        <center><h3><b>Selecionados </b></h3></center>
        <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv" Style="height: 300px;" >
            <div class="gridTableTelaCheia">
                <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                    GridLines="Vertical" AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridMercadoriasSelecionadas_RowCommand">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>

                        <asp:BoundField DataField="Preco" HeaderText="Preco">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir" HeaderStyle-HorizontalAlign="Center"
                            Text="Excluir">
                            <ControlStyle Height="20px" Width="20px"  />
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
    <asp:ModalPopupExtender ID="modalIncluirProdutos" runat="server"
        DropShadow="true" PopupControlID="pnMercadoriaListaFrame" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnObsProdutos" runat="server" CssClass="frameModal" Style="display: none; width:80% " DefaultButton="ImgBtnAddObservacao">

        <div id="divFecharObs" runat="server"  class="btnImprimirDireita">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/cancel.png"
                Height="20px" Width="20px" OnClick="btnAddObsCancela_Click" />
            Fechar
        </div>
        <div class="frame">
            <div class="cabMenu" style="height: 80px;">
                <h2>
                    <asp:Label ID="lblTextObs" runat="server" Text="Observações"></asp:Label>
                </h2>

            </div>
            <div id="divBotoesObservacoes" runat="server"  class="row botoes">
                <div class="panelItem btn">
                    <asp:ImageButton ID="ImgBtnConfirmaIncluirObservacoes" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConfirmaIncluirObservacoes_Click" />
                    Confirmar

                </div>
                <div class="panelItem btn">
                    <asp:ImageButton ID="ImgBtnCancelarObs" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnAddObsCancela_Click" />Cancelar
                </div>
            </div>
            <div id="divCamposAddObs" runat="server" class="row">
                <div class="panel" style="padding: 5px 5%;">

                    <div class="panelItem">
                        <p>Produto</p>
                        <asp:TextBox ID="txtAddObsPlu" runat="server" Width="80px" />
                    </div>

                    <div class="panelItem">
                        <p>Titulo</p>
                        <asp:TextBox ID="txtAddObsTitulo" runat="server" Width="200px" />
                    </div>
                    <div class="panelItem">
                        <p>Plu Add</p>
                        <asp:TextBox ID="txtAddObsPluAdd" runat="server" Width="80px" OnTextChanged="txtAddObsPluAdd_TextChanged"  AutoPostBack="true"/>
                        <asp:ImageButton ID="ImgBtnAddObsPluAdd" ImageUrl="~/img/pesquisaM.png" runat="server" CssClass="imgPesq" OnClick="imgBtnPesquisa_Click" />
                    </div>
                    <div class="panelItem">
                        <p>Preco</p>
                        <asp:TextBox ID="txtAddObsPreco" runat="server" Width="80px" CssClass="numero" />
                    </div>

                    <div class="panelItem">
                        <p>Tipo Obs</p>
                        <asp:DropDownList ID="ddlAddObsTipoObservacao" runat="server" Height="20px">
                            <asp:ListItem Text="escolha-obrigatoria" Value="escolha-unica" />
                            <asp:ListItem Text="escolha" value="escolha" />
                            <asp:ListItem Text="texto" Value="texto" />
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>Ordem Obrigatoria</p>
                        <asp:TextBox ID="txtOrdemObrigatoria" runat="server" Width="80px" CssClass="numero" />
                    </div>

                    <div id="divIncluirObs" runat="server" class="panelItem btn">
                        <asp:ImageButton ID="ImgBtnAddObservacao" OnClick="ImgBtnAddObservacao_Click" ImageUrl="~/img/add.png" runat="server" />
                        Incluir    
                    </div>
                </div>

            </div>
            <div class="row">
                <div style="max-height: 200px; padding: 20px ; flex:1; overflow: auto;">
                    <asp:GridView ID="gridObservacoesProduto" runat="server" ForeColor="#333333" Width="100%"
                        GridLines="Vertical" AutoGenerateColumns="False"  OnRowCommand="gridObservacoesProduto_RowCommand" >
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="titulo" HeaderText="Titulo"></asp:BoundField>

                            <asp:BoundField DataField="pluAdd" HeaderText="Plu Add">
                            </asp:BoundField>
                            <asp:BoundField DataField="tipo" HeaderText="Tipo"></asp:BoundField>
                            <asp:BoundField DataField="ObrigatorioOrdem" HeaderText="Ordem Obrigatorio"></asp:BoundField>
                            <asp:BoundField DataField="preco" HeaderText="Preço">
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
            </div>

        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalIncluirObs" runat="server"
        DropShadow="true" PopupControlID="pnObsProdutos" TargetControlID="lblTextObs">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaExcluirProduto" runat="server" CssClass="frameModal" Style="display: none;" DefaultButton="ImgBtnCancelaExcluirProdutos">

        <div class="frame">
            <div class="cabMenu" style="height: 80px;">
                <h2>Tem Certeza que gostaria de excluir o produto:
                    <asp:Label ID="lblTextConfirmaExcluirProduto" runat="server" Text=""></asp:Label>
                </h2>

            </div>
            <div  class="row botoes">
                <div class="panelItem btn">
                    <asp:ImageButton ID="ImgBtnConfirmaExcluirBotoes" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConfirmaExcluirBotoes_Click" />
                    Confirmar

                </div>
                <div class="panelItem btn">
                    <asp:ImageButton ID="ImgBtnCancelaExcluirProdutos" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnCancelaExcluirProdutos_Click" />Cancelar
                </div>
            </div>
            
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmarExcluirProduto" runat="server"
        DropShadow="true" PopupControlID="pnConfirmaExcluirProduto" TargetControlID="lblTextConfirmaExcluirProduto">
    </asp:ModalPopupExtender>
</asp:Content>
