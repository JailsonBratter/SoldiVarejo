<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PedidoOrcamentoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoOrcamentoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/PedidoVendaDetalhes.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/PedidoVendaDetalhes1.js" type="text/javascript"></script>
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do Orçamento
            </h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <div class="btnImprimirDireita">
            <div id="div1" runat="server" class="panelItem">
                <asp:HyperLink ID="btnImpressao" runat="server" NavigateUrl="~/modulos/Pedidos/pages/PedidoPrint.aspx"
                    Target="_blank">
                    <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" />
                </asp:HyperLink>
            </div>

        </div>
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Pedido
                        </p>
                        <asp:TextBox ID="txtPedido" runat="server" Width="100px"></asp:TextBox>

                    </div>

                    <div class="panelItem">
                        <p>
                            Status
                        </p>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Value="1">Aberto</asp:ListItem>
                            <asp:ListItem Value="2">Fechado</asp:ListItem>
                            <asp:ListItem Value="3">Cancelado</asp:ListItem>
                            <asp:ListItem Value="4">Pendente Entregra</asp:ListItem>
                            <asp:ListItem Value="5">Transito</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="panelItem">
                        <p>
                            Vendedor
                        </p>
                        <asp:TextBox ID="txtfuncionario" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFuncionario" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>

                    <div class="panelItem">
                        <p>
                            Usuario
                        </p>
                        <asp:TextBox ID="txtUsLogado" runat="server" Width="100px"></asp:TextBox>
                    </div>

                    <div class="panelItem">
                        <p>
                            Data cadastro
                        </p>
                        <asp:TextBox ID="txtData_cadastro" runat="server" Width="80px"></asp:TextBox>
                    </div>

                    <div class="panelItem">
                        <p>
                            hora
                        </p>
                        <asp:TextBox ID="txtHoraCadastro" runat="server" CssClass="hora"></asp:TextBox>

                    </div>



                    <div class="panelItem">
                        <p>
                            Natureza Operação
                        </p>
                        <asp:TextBox ID="txtCFOP" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnCfop" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                            OnClick="Img_Click" />
                    </div>


                    <div class="panelItem">
                        <p>
                            Centro Custo
                        </p>
                        <asp:TextBox ID="txtCentroCusto" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtcentro_custo" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Despesas
                        </p>
                        <asp:TextBox ID="txtDespesas" runat="server" Width="100px"></asp:TextBox>
                    </div>
                </td>
            </tr>

            <tr>
                <td>

                    <div class="panelItem">

                        <p>
                            Cliente
                        </p>
                        <asp:TextBox ID="txtCliente_Fornec" runat="server" Width="80px" OnTextChanged="txtCliente_Fornec_TextChanged"
                            CssClass="INTEIRO" AutoPostBack="true">
                        </asp:TextBox>
                        <asp:TextBox ID="txtNomeCliente" runat="server" Width="350px" AutoPostBack="true"
                            OnTextChanged=" txtNomeCliente_TextChanged">
                        </asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="true"
                            TargetControlID="txtNomeCliente" ServiceMethod="GetNomesClientes" ServicePath="PedidoVendaDetalhes.aspx"
                            MinimumPrefixLength="1" CompletionInterval="0" CompletionSetCount="20" EnableCaching="false"
                            BehaviorID="AutoCompleteExCli" CompletionListCssClass="autocomplete_completionListElement"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                            <Animations>
                                <OnShow>
                                    <Sequence>
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoCompleteExCli');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".0">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExCli')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".0">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteExCli')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </asp:AutoCompleteExtender>
                        <asp:ImageButton ID="imgBtnCliente" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                      
                        <asp:ImageButton ID="ImgbtnAddNovocliente" runat="server" ImageUrl="~/img/Add.png"
                            Height="15px" OnClick="ImgbtnAddNovocliente_Click" Visible="false" />
                    </div>

                    <div class="panelItem">
                        <p>
                            Tabela Desconto
                        </p>
                        <asp:TextBox ID="txtCodTbPreco" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="imgCodTbPreco" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>

                    <div class="panelItem">
                        <p>
                            Valor Desconto
                        </p>
                        <asp:TextBox ID="txtDesconto" runat="server" Width="100px" AutoPostBack="true" CssClass="numero"
                            OnTextChanged="txtDesconto_TextChanged">
                        </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Limite de Credito
                        </p>
                        <asp:TextBox ID="txtLimiteCredito" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Utilizado
                        </p>
                        <asp:TextBox ID="txtLimiteUtilizado" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Disponivel
                        </p>
                        <asp:TextBox ID="txtLimiteDisponivel" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                    </div>

                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>Endereco</p>
                        <asp:TextBox ID="txtEndereco" runat="server" Width="300px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Numero</p>
                        <asp:TextBox ID="txtEnderecoNumero" runat="server" Width="50px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Complemento</p>
                        <asp:TextBox ID="txtEnderecoComplemento" runat="server" Width="100px"></asp:TextBox>
                    </div>

                    <div class="panelItem">
                        <p>Bairro</p>
                        <asp:TextBox ID="txtBairro" runat="server" Width="200px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Cidade</p>
                        <asp:TextBox ID="txtCidade" runat="server" Width="200px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Uf</p>
                        <asp:TextBox ID="txtUf" runat="server" Width="50px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <br />
                        <asp:DropDownList ID="ddlEntrega" runat="server" Font-Size="X-Large" Height="40px" Style="margin-top: 4px;">
                            <asp:ListItem>ENTREGA</asp:ListItem>
                            <asp:ListItem>RETIRA</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data entrega
                        </p>
                        <asp:TextBox ID="txtData_entrega" runat="server" CssClass="DATA" Width="140px" Height="35px" Font-Size="X-Large"></asp:TextBox>
                        <asp:ImageButton ID="imgDtEntrega" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtEntrega"
                            TargetControlID="txtData_entrega">
                        </asp:CalendarExtender>
                    </div>

                    <div class="panelItem">
                        <p>
                            Hora
                        </p>
                        <asp:TextBox ID="txthora" runat="server" CssClass="hora" Font-Size="X-Large" Width="100px" Height="35px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Frete
                        </p>
                        <asp:TextBox ID="txtFrete" runat="server" Width="100px" CssClass="numero" Font-Size="X-Large"></asp:TextBox>
                    </div>
                    <div class="btnImprimirDireita">


                        <div class="panelItem">
                            <p>
                                Total Itens
                            </p>
                            <asp:TextBox ID="txtTotalBruto" runat="server" Width="150px" CssClass="numero" Font-Size="X-Large"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Total
                            </p>
                            <asp:TextBox ID="txtTotal" runat="server" Width="150px" CssClass="numero" Font-Size="X-Large"></asp:TextBox>
                        </div>

                    </div>
                </td>

            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens
                </HeaderTemplate>
                <ContentTemplate>

                    <asp:Panel ID="addItens" runat="server" CssClass="titulobtn" DefaultButton="ImgBtnAddItens">
                        <div id="divAdditensRapido" runat="server">
                            <div class="panelItem">
                                <p>Plu</p>
                                <asp:TextBox ID="txtPluAddRapido" runat="server" CssClass="sem"
                                    Width="80px" />
                                <asp:ImageButton ID="ImgBtn_txtPluAddRapido" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="ImgBtnAddItens_Click" />
                            </div>
                            <div class="panelItem" style="text-align: left;">
                                <p>Ref</p>
                                <asp:TextBox ID="txtRefAddRapido" runat="server" Width="80px" CssClass="sem" />
                            </div>
                            <div class="panelItem" style="text-align: left;">
                                <p>Descricao</p>
                                <asp:TextBox ID="txtDescricaoAddRapito" runat="server" Width="300px" />
                            </div>
                            <div class="panelItem">
                                <p>Emb</p>
                                <asp:TextBox ID="txtEmbAddRapito" runat="server" CssClass="numero" Width="50px" />
                            </div>
                            <div class="panelItem">
                                <p>Preço</p>
                                <asp:TextBox ID="txtPrecoAddRapito" runat="server" CssClass="numero" Width="80px" />
                            </div>
                            <div class="panelItem">
                                <p>Qtde</p>
                                <asp:TextBox ID="txtQtdeAddRapito" runat="server" CssClass="sem" Width="80px" />
                            </div>

                        </div>
                        <div id="divBtnAddItem" class="panelItem">

                            <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="ImgBtnAddItens_Click" />
                            Incluir item
                        
                        </div>
                    </asp:Panel>
                    <div class="gridTable">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound" OnRowCommand="gridItens_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                <asp:BoundField DataField="CodReferencia" HeaderText="REFERENCIA"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false"></asp:BoundField>
                                <asp:BoundField DataField="qtde" HeaderText="Qtde">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Embalagem" HeaderText="Emb">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Peso_bruto" HeaderText="Peso Bruto">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TabDesc" HeaderText="Tab Preco"></asp:BoundField>
                                <asp:BoundField DataField="Desc" HeaderText="Desc%">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Unitario" HeaderText="Preço">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Obs" HeaderText="Observações"></asp:BoundField>
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
                    Observações
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:TextBox ID="txtObs" CssClass="sem" runat="server" Width="800px" Height="200px"
                        TextMode="MultiLine"></asp:TextBox>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3">
                <HeaderTemplate>
                    Pagamentos
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addPg" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="btnAddPg" runat="server" ImageUrl="~/img/add.png" Height="20px"
                            Width="20px" OnClick="ImgBtnAddPagamentos_Click" />
                        Incluir Pagamento
                    </asp:Panel>
                    <div class="gridTable">
                        <asp:GridView ID="gridPagamentos" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowDataBound="gridPagamentos_RowDataBound" OnRowCommand="gridPagamentos_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                                    Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Tipo_pagamento" HeaderText="Tipo Pg"></asp:BoundField>
                                <asp:BoundField DataField="Vencimento" HeaderText="Vencimento"></asp:BoundField>
                                <asp:BoundField DataField="Valor" HeaderText="Valor"></asp:BoundField>
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


                        <div class="row">
                            <h3 style="text-align: center">Intermediador (MarketPlace)</h3>
                        </div>
                        <hr />
                        <div style="display: flex; justify-content: center; flex-direction: row; width: 80%; margin: auto">
                            <div style="flex: 2">
                                <p>
                                    Operação 
                                </p>
                                <asp:DropDownList ID="ddlIntermediador" runat="server" Width="90%">
                                    <asp:ListItem Value="0" Selected="True">Operação sem intermediador</asp:ListItem>
                                    <asp:ListItem Value="1">Operação em site ou plataforma de terceiros</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div style="flex: 1">
                                <p>CNPJ do Intermediador</p>
                                <asp:TextBox ID="txtIntermedCnpj" Style="width: 90%" runat="server" />
                            </div>
                            <div style="flex: 1">
                                <p>Indentificador</p>
                                <asp:TextBox ID="txtIdCadIntTran" Style="width: 90%" runat="server" />
                            </div>
                            <div style="flex: 1">
                                <p>CNPJ Pagamento</p>
                                <asp:TextBox ID="txtCnpjPagamento" Style="width: 90%" runat="server" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
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
                <asp:DropDownList ID="ddlTipoPesquisa" runat="server">
                    <asp:ListItem>PLU</asp:ListItem>
                    <asp:ListItem>DESCRICAO</asp:ListItem>
                </asp:DropDownList>
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
    <asp:Panel ID="PnAddPagamento" runat="server" CssClass="modalForm" Style="display: none; height: 200px;">
        <asp:Panel ID="PnPagamentoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Dados do Pagamento</h1>
                <asp:Label ID="lblErroPg" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaPagamentos" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaPagamentos_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaPagamentos" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaPagamentos_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Tipo Pagamento
                        </p>
                        <asp:TextBox ID="txtTipoPg" runat="server" Width="200px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtTipoPg" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </td>
                    <td>
                        <p>
                            Parcelas
                        </p>
                        <asp:TextBox ID="txtParcelas" runat="server" Width="80px"></asp:TextBox>

                    </td>
                    <td>
                        <p>
                            Dias
                        </p>
                        <asp:TextBox ID="txtDiasParcelas" runat="server" Width="80px"></asp:TextBox>

                    </td>
                    <td>
                        <p>
                            Vencimento
                        </p>
                        <asp:TextBox ID="txtVencimentoPg" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgVencimentoPg" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgVencimentoPg"
                            TargetControlID="txtVencimentoPg">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Valor
                        </p>
                        <asp:TextBox ID="txtValorPg" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPagamentos" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroPg" DropShadow="true" PopupControlID="pnAddPagamento"
        TargetControlID="lblErroPg">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none; height: 280px;">
        <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Dados do Item</h1>
                <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaItens" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaItens_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaItem" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelaItem_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgExcluiItem" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="ImgExcluiItem_Click" />
                        <asp:Label ID="Label6" runat="server" Text="Excluir Item"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblerroItem" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:Label ID="lblIndex" runat="server" Text="" Visible="false"></asp:Label>
            <asp:CheckBox ID="chkInserido" runat="server" Text ="" Visible="false" Checked="false" />
            <table>
                <tr>

                    <td>
                        <p>
                            PLU
                        </p>
                        <asp:TextBox ID="txtPLU" runat="server" Width="50px" ViewStateMode="Inherit"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtPLU" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </td>
                    <td colspan="2">
                        <p>
                            Descricao
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Qtde
                        </p>
                        <asp:TextBox ID="txtQtde" runat="server" CssClass="numero" Width="50px" OnChange="javascript:calculaTotal();"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Embalagem
                        </p>
                        <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="50px" OnChange="javascript:calculaTotal();"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Desconto %
                        </p>
                        <asp:TextBox ID="txtDescontoItem" runat="server" CssClass="numero" Width="100px"
                            OnChange="javascript:calculaTotal();">
                        </asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Preço
                        </p>
                        <asp:TextBox ID="txtUnitario" runat="server" CssClass="numero" Width="100px" OnChange="javascript:calculaTotal();"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            TOTAL :
                        </p>
                        <asp:TextBox ID="TxtTotalItem" runat="server" CssClass="numero" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <p>Observação </p>
                        <asp:TextBox ID="txtObsItem" runat="server" TextMode="MultiLine" onkeyup="limite_textarea(this.value)" MaxLength="255" Width="99%" Height="50px"></asp:TextBox>
                        <h6 style="margin-top: 0px;">*Observação pode conter até 255 caracteres falta <span id="cont">255</span> caracteres </h6>
                    </td>
                    <td colspan="2">
                        <div class="panelItem">
                            <p>Produzir</p>
                            <label class="switch">

                                <asp:CheckBox ID="chkProduzirItem" runat="server" OnCheckedChanged="chkProduzirItem_change" AutoPostBack="true" />

                                <span class="slider round"></span>
                            </label>

                        </div>
                        <div class="panelItem">
                            <p>Data </p>
                            <asp:TextBox ID="txtDataProducaoItem" runat="server" CssClass="DATA" Width="80px"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnDtHoraProducaoItem" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgBtnDtHoraProducaoItem"
                                TargetControlID="txtDataProducaoItem">
                            </asp:CalendarExtender>

                        </div>
                        <div class="panelItem">
                            <p>Hora</p>
                            <asp:TextBox ID="txtHoraProducaoItem" runat="server" CssClass="hora"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Agrupamento Produção</p>
                            <asp:DropDownList ID="ddlAgrupamento" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalItens" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblError" DropShadow="true" PopupControlID="pnItens" TargetControlID="lblError">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnMercadoriaLista" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="frameDiv" DefaultButton="imgPesquisaMercadoria"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar:
                <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="300px" OnTextChanged="txtfiltromercadoria_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaMercadoria_Click" />
                <asp:DropDownList ID="ddlTipoPesquisaMercadoria" runat="server" Width="180px">
                    <asp:ListItem>PLU</asp:ListItem>
                    <asp:ListItem Selected="True">DESCRICAO / EAN / REF</asp:ListItem>
                </asp:DropDownList>

            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnFecharMecadoria" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnFecharMecadoria_Click" />
                    <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelarMercadoria" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaMercadoria_Click" />
                    <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="listaDiv">
                <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" OnRowDataBound="GridMercadoria1_RowDataBound"
                    AutoGenerateColumns="false">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PLU" HeaderText="PLU" HtmlEncode="false" />
                        <asp:BoundField DataField="EAN" HeaderText="EAN" HtmlEncode="false" />
                        <asp:BoundField DataField="REFERENCIA" HeaderText="REFERENCIA" HtmlEncode="false" />
                        <asp:BoundField DataField="DESCRICAO" HeaderText="DESCRICAO" HtmlEncode="false" />
                        <asp:BoundField DataField="PRC VENDA" HeaderText="PRC VENDA" HtmlEncode="false" />
                        <asp:BoundField DataField="SALDO" HeaderText="SALDO" HtmlEncode="false" />
                        <asp:BoundField DataField="PESO BRUTO" HeaderText="PESO BRUTO" HtmlEncode="false" />


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
            <asp:Panel ID="Panel1" runat="server" CssClass="frameCentral">
                <table>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <p>
                                Adicionar
                            </p>
                            &nbsp;&nbsp;
                            <asp:ImageButton ID="AddSelecionado" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                OnClick="ImgBtnAddSelecionado_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="listaDiv">
                <asp:GridView ID="GridMercadoriaSelecionado" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="false" OnRowCommand="GridMercadoriaSelecionado_RowCommand"
                    OnRowDataBound="GridMercadoriaSelecionado_RowDataBound">
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Alterar"
                            Text="Alterar">
                            <ControlStyle Height="20px" Width="20px" />
                            <ItemStyle Width="20px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="Plu" HeaderText="PLU"></asp:BoundField>
                        <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false"></asp:BoundField>
                        <asp:TemplateField HeaderText="Qtd">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQtd" runat="server" Text='<%# Eval("QTD") %>' Width="30px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="embalagem" HeaderText="Emb"></asp:BoundField>
                        <asp:TemplateField HeaderText="Preço">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPreco" runat="server" Text='<%# Eval("Preco") %>' Width="60px"></asp:TextBox>
                                <asp:Label ID="lblPreco" runat="server" Text='<%# Eval("PrecoPadrao") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="peso_bruto" HeaderText="Peso Bruto"></asp:BoundField>
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
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMercadorialista" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMercadoriaLista" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label7" runat="server" Text="Confirma Cancelamento?" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label12" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label13" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label7">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnSenha" runat="server" CssClass="frameModal" Style="display: none"
        DefaultButton="imgBtnConfirmaDesconto">
        <asp:Label ID="lblTituloSenha" runat="server" Text="Digite a Senha para Liberação do Desconto"
            CssClass="cabMenu" Style="height: 80px; padding: 3px; font-size: medium;"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaDesconto" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaDesconto_Click" />
                    <asp:Label ID="Label16" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaDesconto" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaDesconto_Click" />
                    <asp:Label ID="Label17" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
            <tr>

                <td colspan="2" style="font-size: 25px;">
                    <center>
                        SENHA:
                         
                        <asp:TextBox ID="txtSAutorizacao" runat="server"    Width="200px" Height="50px"   Font-Size="X-Large" autocomplete="off" TextMode="password"  ></asp:TextBox>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSenha" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSenha" TargetControlID="lblTituloSenha">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnManterDados" runat="server" CssClass="frameModal" Style="display: none; height: 150px; width: 500px;">
        <h2>
            <b>
                <center>
            <asp:Label ID="Label18" runat="server" Text="Gostaria de Manter os Dados do ultimo Cadastro?"> </asp:Label>
                </center>
            </b>
        </h2>
        <hr />
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="btnConfirmaManter" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="btnConfirmaManter_Click" />
            <asp:Label ID="Label19" runat="server" Text="SIM"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 40%;">
            <asp:ImageButton ID="btnCancelaManter" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="btnCancelaManter_Click" />
            <asp:Label ID="Label20" runat="server" Text="NÃO"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalManterDados" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnManterDados" TargetControlID="Label18">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnTitulosAbertos" runat="server" CssClass="modalForm" Style="display: none;">
        <asp:Panel ID="Panel4" runat="server" CssClass="frame">
            <asp:Label ID="lblTitulosAbertos" runat="server" Text="Titulos em aberto!" CssClass="cabMenu"></asp:Label>
            <table class="frame">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnImprimirTitulos" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                            Width="25px" OnClick="btnImprimirTitulos_Click" />
                        <asp:Label ID="Label22" runat="server" Text="Imprimir"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnFecharTitulos" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" />
                        <asp:Label ID="Label23" runat="server" Text="Fechar"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="direita">
                <h2>
                    <b>Total:</b>
                    <asp:Label ID="lblTotalTitulos" runat="server" Text=""></asp:Label></h2>
            </div>
            <asp:Panel ID="Panel5" runat="server" CssClass="lista">
                <asp:GridView ID="gridTitulos" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Documento" HeaderText="DOCUMENTO"></asp:BoundField>
                        <asp:BoundField DataField="tipo" HeaderText="TIPO"></asp:BoundField>
                        <asp:BoundField DataField="Emissao" HeaderText="EMISSAO"></asp:BoundField>
                        <asp:BoundField DataField="Vencimento" HeaderText="VENCIMENTO"></asp:BoundField>
                        <asp:BoundField DataField="Valor" HeaderText="VALOR">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Dias" HeaderText="DIAS">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
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
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalTitulos" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnTitulosAbertos" TargetControlID="lblTitulosAbertos">
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

    <asp:Panel ID="pnNovoCliente" runat="server" CssClass="modalForm" Style="display: none; height: 280px;">
        <asp:Panel ID="Panel6" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Cadastro novo Cliente</h1>
                <asp:Label ID="lblErrorCliente" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaClientes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaClientes_Click" />
                        <asp:Label ID="Label15" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaClientes" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="ImgBtnCancelaClientes_Click" />
                        <asp:Label ID="Label21" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="Label25" runat="server" Text="" ForeColor="Red"></asp:Label>
            <table>
                <tr>

                    <td>
                        <div class="panelItem">
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoClienteNovo" runat="server" Width="50px" ViewStateMode="Inherit"></asp:TextBox>
                        </div>
                        <div class="panelItem">

                            <p>
                                Nome
                            </p>
                            <asp:TextBox ID="txtNomeClienteNovo" runat="server" Width="200px"></asp:TextBox>
                        </div>
                        <div class="panelItem">

                            <p>
                                Telefone
                            </p>
                            <asp:TextBox ID="txtTelefoneClienteNovo" runat="server" Width="150px"></asp:TextBox>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>Endereço</p>
                            <asp:TextBox ID="txtEnderecoClienteNovo" runat="server" Width="200px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Numero</p>
                            <asp:TextBox ID="txtNumeroClienteNovo" runat="server" Width="50px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Complemento</p>
                            <asp:TextBox ID="txtComplementoClienteNovo" runat="server" Width="150px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Bairro</p>
                            <asp:TextBox ID="txtBairroClienteNovo" runat="server" Width="150px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Cidade</p>
                            <asp:TextBox ID="txtCidadeClienteNovo" runat="server" Width="150px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>UF</p>
                            <asp:TextBox ID="txtUFClienteNovo" runat="server" Width="50px"></asp:TextBox>

                        </div>

                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalNovocliente" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label25" DropShadow="true" PopupControlID="pnNovoCliente" TargetControlID="Label25">
    </asp:ModalPopupExtender>

</asp:Content>
