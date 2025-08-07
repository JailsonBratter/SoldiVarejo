<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContasReceberDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ContasReceberDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do Conta a Receber</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
    <div id="BtnBaixa" runat="server" class="direitaFechar" style="right:40px;">
                        <asp:ImageButton ID="imgBtnBaixar" runat="server" ImageUrl="~/img/confirm.png" Height="25px"
                            OnClick="imgBtnBaixar_Click" />
                        Baixar
                    </div>
        <!--Coloque aqui os campos do cabe?alho    -->
        <table style="width: 910px;">
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Documento</p>
                        <asp:TextBox ID="txtDocumento" runat="server" Width="300px" MaxLength="50" Style="font-size: 20px;"></asp:TextBox>
                    </div>
                     

                    <div class="panelItem" style="float: right; margin-right: 20px;">
                        <p>
                            Usuário</p>
                        <asp:TextBox ID="txtusuario" runat="server" Width="150px" Style="font-size: 20px;"></asp:TextBox>
                    </div>
                   
                </td>
                
                
                
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Cliente</p>
                        <asp:TextBox ID="txtCodigo_Cliente" runat="server" Width="250px" AutoPostBack="true"
                            Style="font-size: 20px;" OnTextChanged=" txtCodigo_Cliente_TextChanged"></asp:TextBox>
                        <asp:TextBox ID="txtNome_Cliente" runat="server" Width="630px" AutoPostBack="true"
                            Style="font-size: 20px;" OnTextChanged="txtNomeCliente_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="true"
                            TargetControlID="txtNome_Cliente" ServiceMethod="GetNomesClientes" ServicePath="ContasReceberDetalhes.aspx"
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
                        <asp:ImageButton ID="imgBtnCliente" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem" style="width: 180px">
                        <p>
                            Entrada</p>
                        <asp:TextBox ID="txtEntrada" runat="server" Width="150px" CssClass="DATA" Style="font-size: 20px;"></asp:TextBox>
                    </div>
                    <div class="panelItem" style="margin-left: 10px; width: 180px;">
                        <p>
                            Emissao</p>
                        <asp:TextBox ID="txtEmissao" runat="server" Width="150px" CssClass="DATA" Style="font-size: 20px;"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataEmissao" runat="server" PopupButtonID="imgDtEmissao"
                            TargetControlID="txtEmissao">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem" style="margin-left: 10px; width: 180px;">
                        <p>
                            Vencimento</p>
                        <asp:TextBox ID="txtVencimento" runat="server" Width="150px" CssClass="DATA" Style="font-size: 20px;"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtVencimento" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="clnDataVencimento" runat="server" PopupButtonID="imgDtVencimento"
                            TargetControlID="txtVencimento">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem" style="margin-left: 10px;">
                        <p>
                            Status</p>
                        <asp:DropDownList ID="DDlStatus" runat="server" Style="font-size: 20px;">
                            <asp:ListItem>ABERTO</asp:ListItem>
                            <asp:ListItem>CONCLUIDO</asp:ListItem>
                            <asp:ListItem>CANCELADO</asp:ListItem>
                            <asp:ListItem>LANCADO</asp:ListItem>
                            <asp:ListItem>SUSPENSO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem" style="margin-left: 10px;">
                        <p>
                            Valor</p>
                        <asp:TextBox ID="txtValor" runat="server" Width="150px" AutoPostBack="true" OnTextChanged="txtCalcula_TextChanged"
                            CssClass="numero" Style="font-size: 20px; text-align: right;"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="400px">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Geral
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="width: 910px;">
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Desconto</p>
                                    <asp:TextBox ID="txtDesconto" runat="server" CssClass="numero" Width="150px" AutoPostBack="true"
                                        OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Acréscimo</p>
                                    <asp:TextBox ID="txtAcrescimo" runat="server" CssClass="numero" Width="150px" AutoPostBack="true"
                                        OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Taxa</p>
                                    <asp:TextBox ID="txtTaxa" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="width: 130px; margin-left: 10px;">
                                    <p>
                                        Recebimento</p>
                                    <asp:TextBox ID="txtPagamento" runat="server" Width="100px" CssClass="DATA"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtPagamento" ImageUrl="~/img/calendar.png" runat="server"
                                        Height="15px" />
                                    <asp:CalendarExtender ID="clnDataPagamento" runat="server" PopupButtonID="imgDtPagamento"
                                        TargetControlID="txtPagamento">
                                    </asp:CalendarExtender>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Total a Receber</p>
                                    <asp:TextBox ID="txtTotalPagar" runat="server" CssClass="numero" Width="140px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Valor Recebido</p>
                                    <asp:TextBox ID="txtValor_Pago" runat="server" CssClass="numero" Width="150px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Id Conta</p>
                                    <asp:TextBox ID="txtid_cc" runat="server" Width="150px" AutoPostBack="true" OnTextChanged="txtid_cc_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtntxtId_cc" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" Enabled="true"
                                        TargetControlID="txtid_cc" ServiceMethod="GetConta" ServicePath="ContasReceberDetalhes.aspx"
                                        MinimumPrefixLength="1" CompletionInterval="0" CompletionSetCount="20" EnableCaching="false"
                                        BehaviorID="AutoCompleteConta" CompletionListCssClass="autocomplete_completionListElement"
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
                                            var behavior = $find('AutoCompleteConta');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight -2;
                                                target.style.height = '0px';
                                            }" />
                            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteConta')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteConta')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                                        </Animations>
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Banco</p>
                                    <asp:TextBox ID="txtbanco" runat="server" Width="150px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Agencia</p>
                                    <asp:TextBox ID="txtagencia" runat="server" Width="150px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Conta</p>
                                    <asp:TextBox ID="txtconta" runat="server" Width="170px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Numero Cheque</p>
                                    <asp:TextBox ID="txtcheque" runat="server" Width="190px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Centro Custo</p>
                                    <asp:TextBox ID="txtCodigo_Centro_Custo" runat="server" Width="100px" AutoPostBack="true"
                                        OnTextChanged="txtCodigo_Centro_custo_TextChanged"></asp:TextBox>
                                    <asp:TextBox ID="txtCentroDescricao" runat="server" Width="280px" AutoPostBack="true"
                                        OnTextChanged="txtCentroDescricao_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" Enabled="true"
                                        TargetControlID="txtCentroDescricao" ServiceMethod="GetCentroCusto" ServicePath="ContasReceberDetalhes.aspx"
                                        MinimumPrefixLength="1" CompletionInterval="0" CompletionSetCount="20" EnableCaching="false"
                                        BehaviorID="AutoCompleteCentroCusto" CompletionListCssClass="autocomplete_completionListElement"
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
                                            var behavior = $find('AutoCompleteCentroCusto');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight -2;
                                                target.style.height = '0px';
                                            }" />
                            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteCentroCusto')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteCentroCusto')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                                        </Animations>
                                    </asp:AutoCompleteExtender>
                                    <asp:ImageButton ID="imgBtnCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        PDV</p>
                                    <asp:TextBox ID="txtpdv" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Operador</p>
                                    <asp:TextBox ID="txtoperador" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 15px;">
                                    <p>
                                        Finalizadora</p>
                                    <asp:TextBox ID="txtid_finalizadora" runat="server" Width="44px"></asp:TextBox>
                                    <asp:TextBox ID="txtfinalizadora" runat="server" Width="180px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Documento Emitido TEF</p>
                                    <asp:TextBox ID="txtdocumento_emitido" runat="server" Width="120px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Tipo de Recebimento</p>
                                    <asp:TextBox ID="txtTipoRecebimento" runat="server" Width="280px" MaxLength="20"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnTipoRecebimento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Tipo de Cartão</p>
                                    <asp:TextBox ID="txtTipoCartao" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <br />
                                    <asp:CheckBox ID="chkBaixa_Automatica" runat="server" Text="Baixa Automatica" />
                                </div>
                                <div class="panelItem" style="margin-left: 10px;">
                                    <asp:CheckBox ID="chkNota_servico" runat="server" Text="Nota de Serviço" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem" style="width: 100%;">
                                    <p>
                                        Observação</p>
                                    <asp:TextBox ID="txtObs" runat="server" Height="63px" CssClass="sem" TextMode="MultiLine"
                                        Width="100%"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                <HeaderTemplate>
                    Detalhes
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="gridnf">
                        <asp:GridView ID="gridCupons" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Documento" HeaderText="Documento">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Emissao" HeaderText="Emissao">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:N2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="taxa" HeaderText="Taxa" DataFormatString="{0:N2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:N2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Rede_cartao" HeaderText="Rede">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="id_Bandeira" HeaderText="Bandeira">
                                    <HeaderStyle HorizontalAlign="Center" />
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
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label1" runat="server" Text="Confirma Cancelamento" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="Label1">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaData" runat="server" CssClass="frameModal" Style="display: none">
        <center>
            <asp:Label ID="lblConfirmaTituloData" runat="server" Text="Foi informada uma Data de pagamento menor que a Data de emissão do titulo <br /> Gostaria de Salvar? "
                CssClass="titulobtn"></asp:Label>
        </center>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaData" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaData_Click" />
                    <asp:Label ID="lblConfirmaData" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaData" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="btnCancelaData_Click" />
                    <asp:Label ID="lblCcancelaData" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaData" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaData" TargetControlID="lblConfirmaTituloData">
    </asp:ModalPopupExtender>
</asp:Content>
