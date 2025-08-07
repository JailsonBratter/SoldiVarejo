<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PedidoCompraDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoCompraDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do pedido De Compra</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <asp:HyperLink ID="btnImpressao" runat="server" NavigateUrl="~/modulos/Pedidos/pages/PedidoPrintCompra.aspx"
            CssClass="btnImprimirDireita" Target="_blank">
            <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" />
        </asp:HyperLink>
        <table>
            <tr>
                <td>
                    <p>
                        Pedido</p>
                    <asp:TextBox ID="txtPedido" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="1">Aberto</asp:ListItem>
                        <asp:ListItem Value="2">Fechado</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Comprador</p>
                    <asp:TextBox ID="txtfuncionario" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFuncionario" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td>
                    <p>
                        Data cadastro</p>
                    <asp:TextBox ID="txtData_cadastro" runat="server" Width="80px" MaxLength="15"></asp:TextBox>
                    <asp:ImageButton ID="imgDtCadastro" ImageUrl="~/img/calendar.png" runat="server"
                        Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtCadastro"
                        TargetControlID="txtData_cadastro">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Data entrega</p>
                    <asp:TextBox ID="txtData_entrega" runat="server" Width="80px" MaxLength="15" CssClass="DATA"></asp:TextBox>
                    <asp:ImageButton ID="imgDtEntrega" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtEntrega"
                        TargetControlID="txtData_entrega">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Hora</p>
                    <asp:TextBox ID="txthora" runat="server" Width="70px" MaxLength="5" CssClass="HORA"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Natureza da Operação</p>
                    <asp:TextBox ID="txtCFOP" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCfop" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="Img_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtCliente_Fornec" runat="server" Width="300px"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="true"
                        TargetControlID="txtCliente_Fornec" ServiceMethod="GetNomesFornecedor" ServicePath="PedidoCompraDetalhes.aspx"
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
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExCli')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteExCli')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                        </Animations>
                    </asp:AutoCompleteExtender>
                    <asp:ImageButton ID="imgBtnFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td>
                    <p>
                        Usuario</p>
                    <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Desconto</p>
                    <asp:TextBox ID="txtDesconto" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Total</p>
                    <asp:TextBox ID="txtTotal" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItens" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                            Width="20px" OnClick="ImgBtnAddItens_Click" />
                        Incluir item
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
                                <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                <asp:BoundField DataField="CodReferencia" HeaderText="Referencia"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false"></asp:BoundField>
                                <asp:BoundField DataField="qtde" HeaderText="Qtde">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Embalagem" HeaderText="Emb">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Unitario" HeaderText="Preço">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total">
                                    <ItemStyle HorizontalAlign="Right" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Observações
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:TextBox ID="txtObs" runat="server" Width="800px" Height="200px" TextMode="MultiLine"></asp:TextBox>
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
    <asp:Panel ID="PnAddPagamento" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="PnPagamentoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>
                    Dados do Pagamento</h1>
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
                            Tipo Pagamento</p>
                        <asp:TextBox ID="txtTipoPg" runat="server" Width="200px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtTipoPg" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </td>
                    <td>
                        <p>
                            Vencimento</p>
                        <asp:TextBox ID="txtVencimentoPg" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgVencimentoPg" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgVencimentoPg"
                            TargetControlID="txtVencimentoPg">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Valor</p>
                        <asp:TextBox ID="txtValorPg" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPagamentos" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroPg" DropShadow="true" PopupControlID="pnAddPagamento"
        TargetControlID="lblErroPg">
    </asp:ModalPopupExtender>:
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none;height:180px;">
        <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>
                    Dados do Item</h1>
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
            <asp:Label ID="lblIndex" runat="server" Text="" Visible="false"></asp:Label>
            <table>
                <tr>
                    <td>
                        <p>
                            PLU</p>
                        <asp:TextBox ID="txtPLU" runat="server" Width="50px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtPLU" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </td>
                    <td colspan="2">
                        <p>
                            Descricao</p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Qtde</p>
                        <asp:TextBox ID="txtQtde" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Embalagem</p>
                        <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Preço</p>
                        <asp:TextBox ID="txtUnitario" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            TOTAL :</p>
                        <asp:TextBox ID="TxtTotalItem" runat="server" CssClass="numero" Width="100px" Enabled="false"></asp:TextBox>
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
                    GridLines="None" OnRowDataBound="GridMercadoria1_RowDataBound">
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
                                Adicionar</p>
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
                        <asp:BoundField DataField="Descricao" HeaderText="DESCRICAO"></asp:BoundField>
                        <asp:TemplateField HeaderText="QTD">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQtd" runat="server" Text='<%# Eval("QTD") %>' Width="30px" onkeypress="return numeros(this,event);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="embalagem" HeaderText="EMB"></asp:BoundField>
                        <asp:TemplateField HeaderText="PRECO">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPreco" runat="server" Text='<%# Eval("Preco") %>' Width="60px"
                                    onkeypress="return numeros(this,event);"></asp:TextBox>
                            </ItemTemplate>
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
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMercadorialista" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMercadoriaLista" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label7" runat="server" Text="Confirma Exclusão" CssClass="cabMenu"></asp:Label>
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
    <asp:Panel ID="PnManterDados" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label18" runat="server" Text="Gostaria de Manter os Dados do ultimo Cadastro?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaManter" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaManter_Click" />
                    <asp:Label ID="Label19" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaManter" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaManter_Click" />
                    <asp:Label ID="Label20" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalManterDados" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnManterDados" TargetControlID="Label18">
    </asp:ModalPopupExtender>
</asp:Content>
