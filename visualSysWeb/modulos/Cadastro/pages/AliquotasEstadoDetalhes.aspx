<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AliquotasEstadoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.AliquotasEstadoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Classificação Fiscal </h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table style="margin-left: 30%;">
            <tr>
                <td>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                NCM</p>
                            <asp:TextBox ID="txtNcm" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Descrição</p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="400px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Marg Iva</p>
                            <asp:TextBox ID="txtMargemIva" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Marg Iva Ajustado</p>
                            <asp:TextBox ID="txtMargemIvaAjustado" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                (%)Imp Estad</p>
                            <asp:TextBox ID="txtImpEstadual" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                (%)Imp Fed Nac</p>
                            <asp:TextBox ID="txtImpFedNacional" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                (%)Imp Fed Import</p>
                            <asp:TextBox ID="txtImpFedImportacao" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div>
                            <p>Atualizar dados IBPT</p>
                            <asp:Button ID="btnIBPT" runat="server" with="80px" OnClick="btnIBPT_Click" >IBPT</asp:Button>
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
                    Substituição Tributaria
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="gridTable">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand" CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="UF" HeaderText="UF"></asp:BoundField>
                                <asp:BoundField DataField="icms_interestadual" HeaderText="Aliquota Destino">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="porc_estorno_st" HeaderText="(%) Estorno ST">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="nao_abate_aliq_origem" HeaderText="Não Abate Aliq Origem">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="mva" HeaderText="MVA">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="mva_cons_final" HeaderText="MVA Consumidor Final">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="condicao_icms" HeaderText="Condicao Icms"></asp:BoundField>
                                <asp:BoundField DataField="porc_icms" HeaderText="(%)Icms">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tipo_reducao" HeaderText="Tipo Redução"></asp:BoundField>
                                <asp:BoundField DataField="porc_reducao" HeaderText="(%)Redução">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CFOP" HeaderText="CFOP"></asp:BoundField>
                                <asp:BoundField DataField="porc_combate_pobresa" HeaderText="(%) Combate Pobresa">
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
                    PIS/CONFINS
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <div class="panel" style="width: 150px;">
                                 <div class="panelItem">
                                        <p>CST PIS</p>
                                        <asp:TextBox ID="txtCstPis" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_txtCstPis" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>(%)PIS</p>
                                        <asp:TextBox ID="txtPis" runat="server" Width="80px" CssClass="numero"></asp:TextBox>

                                    </div>
                                    <div class="panelItem">
                                        <p>(%)PIS Entrada</p>
                                        <asp:TextBox ID="txtPisEntrada" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                    </div>
                                   
                                </div>
                                <div class="panel" style="width: 150px;">
                                    <div class="panelItem">
                                        <p>CST COFINS</p>
                                        <asp:TextBox ID="txtCstCofins" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_txtCstCofins" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>(%)COFINS</p>
                                        <asp:TextBox ID="txtCofins" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <p>(%)COFINS Entrada</p>
                                        <asp:TextBox ID="txtCofinsEntrada" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                    </div>
                                    
                                </div>
                                <div class="panel" style="width: 150px;">
                                    <div class="panelItem">
                                        <p>CEST</p>
                                        <asp:TextBox ID="txtCest" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_txtCest" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
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
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound" >
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
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnExcluirCFOP" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir?"
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
    <asp:ModalPopupExtender ID="modalExluirCFOP" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirCFOP" TargetControlID="Label14">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnDetalhesUF" runat="server" CssClass="modalForm" Style="display: none;
        height: 300px; width:1100px; ">
        <asp:Panel ID="PnPagamentoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>
                    Detalhes UF</h1>
                <asp:Label ID="lblErroUF" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaUF" runat="server" ImageUrl="~/img/confirm.png" Width="25px"
                            OnClick="btnConfirmaUF_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaUF" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelaUF_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="margin-left:40px">
                <tr>
                    <td>
                        <div class="row">
                            <div class="panelItem">
                                <p>
                                    UF</p>
                                <asp:TextBox ID="txtUF" runat="server" Width="70px"></asp:TextBox>
                            </div>
                            <div class="panelItem" runat="server" visible="false">
                                <p>
                                    Tributacao</p>
                                <asp:TextBox ID="txtCodTributacao" runat="server" Width="60px" CssClass="numero"></asp:TextBox>
                                <asp:TextBox ID="txtDescTributacao" runat="server" Width="200px" ></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCodTributacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </div>
                            <div class="panelItem" >
                                <p>
                                    Aliq Destino</p>
                                <asp:TextBox ID="txticmsInterestadual" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    MVA</p>
                                <asp:TextBox ID="txtMva" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    MVA Cons Final</p>
                                <asp:TextBox ID="txtMvaConsumidorFinal" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>Condição ICMS</p>
                                <asp:DropDownList ID="ddlCondicoesICMS" runat="server">
                                <asp:ListItem Value="1" >Sem diferenciação</asp:ListItem>
                                <asp:ListItem Value="2" >Retido na Fonte</asp:ListItem>
                                <asp:ListItem Value="3" >Tributado</asp:ListItem>
                                <asp:ListItem Value="4" >Outros</asp:ListItem>

                                </asp:DropDownList>
                            </div>
                            <div class="panelItem" runat="server" visible="false">
                                <p>(%)ICMS</p>
                                <asp:TextBox ID="txtPorcICMS" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem" >
                                <p>Tipo Redução</p>
                                <asp:TextBox ID="txtTipoReducao" runat="server" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>(%)Redução</p>
                                <asp:TextBox ID="txtPorcReducao" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Texto NF</p>
                                <asp:TextBox ID="txtTextoNf" runat="server" Width="80px"></asp:TextBox>
                                 <asp:ImageButton ID="imgBtn_TxtTextoNf" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="ImgObs_Click" />
                            </div>
                             <div class="panelItem">
                                <p>
                                    (%)Combate Pobreza</p>
                                <asp:TextBox ID="txtPorcCombatePobreza" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                           
                        </div>
                        <div class="row">
                        
                            <div class="panelItem" style="margin-left: 80px;">
                                <p>
                                    Aliq Simples</p>
                                <asp:TextBox ID="txticmsEstadoSimples" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    MVA Simples</p>
                                <asp:TextBox ID="txtMvaSimples" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    (%) Estorno ST</p>
                                <asp:TextBox ID="txtPorcEstornoSt" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            
                            
                            <div class="panelItem" style="margin-left: 140px;">
                                <p>
                                    Tipo Red Simp</p>
                                <asp:TextBox ID="txtTipoReducaoSimples" runat="server" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    (%)Red Simples</p>
                                <asp:TextBox ID="txtPorcReducaoSimples" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Text NF Simp</p>
                                <asp:TextBox ID="txtTextoNfSimples" runat="server" Width="80px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtTextoNfSimples" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="ImgObs_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    CFOP</p>
                                <asp:TextBox ID="txtCfop" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                            </div>
                            
                        </div>
                        <div class="row">
                         <div class="panelItem" style="margin-left: 80px;">
                                <p>
                                    Protocolo</p>
                                <asp:TextBox ID="txtProtocolo" runat="server" Width="200px"></asp:TextBox>
                            </div>
                       
                            
                            <div class="panelItem" style="margin-left: 30px;">
                                <br />
                                <asp:CheckBox ID="chkNaoAbateAliqOrigem" runat="server" Text="Não Abate Aliquota Origem" />
                            </div>
                            <asp:Label ID="lblIndex" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalDetalhes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroUF" DropShadow="true" PopupControlID="PnDetalhesUF" TargetControlID="lblErroUF">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnObservacoesPadrao" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnObservaoFrame" runat="server" CssClass="frame" DefaultButton="imgBtnConfirmaObservacoes">
            <div class="cabMenu">
                <h1>
                    Observações Padrão</h1>
                <asp:Label ID="lblErroObservacao" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnConfirmaObservacoes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnConfirmaObservacoes_Click" />
                        <asp:Label ID="Label17" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelarObservacoes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelarObservacoes_Click" />
                        <asp:Label ID="Label18" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            <asp:Panel ID="pnNovaObservacao" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblcod" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtObservacaoAdd" TextMode="MultiLine" Width="600px" runat="server"
                                CssClass="SEM"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgConfirmaAddObservacao" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="imgConfirmaAddObservacao_Click" />
                            Confirma
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <hr />
            <asp:Panel ID="Panel6" runat="server" CssClass="lista">
                <asp:GridView ID="GridObservacao" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" OnRowCommand="GridObservacao_RowCommand" AutoGenerateColumns="false">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                               <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrObslistaItem" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="cod" HeaderText="Cod"></asp:BoundField>
                        <asp:BoundField DataField="Observacao" HeaderText="Observacao"></asp:BoundField>
                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/EDIT.png" CommandName="Alterar"
                            Text="Alterar">
                            <ControlStyle Height="20px" Width="20px" />
                            <ItemStyle Width="20px" />
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
            </asp:Panel>
            <asp:ImageButton ID="imgBtnAddObservacoes" runat="server" ImageUrl="~/img/add.png"
                Width="25px" OnClick="imgBtnAddObservacoes_Click" />
            <asp:Label ID="lblObservacao" runat="server" Text="  Adicionar nova Observacao"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalObservacoes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroObservacao" DropShadow="true" PopupControlID="PnObservacoesPadrao"
        TargetControlID="lblErroObservacao">
    </asp:ModalPopupExtender>

</asp:Content>
