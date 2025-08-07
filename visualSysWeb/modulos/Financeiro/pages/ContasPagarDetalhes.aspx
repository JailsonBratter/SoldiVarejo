<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContasPagarDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ContasPagarDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do Contas a pagar</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="BtnBaixa" runat="server" class="direitaFechar">
        <asp:ImageButton ID="imgBtnBaixar" runat="server" ImageUrl="~/img/confirm.png" Height="25px"
            OnClick="imgBtnBaixar_Click" />
        Baixar
    </div>
    <div id="DivCodBarras" runat="server" class="direitaFechar" visible="false">
        <asp:ImageButton ID="ImgBtnIncluirPorCodBarra" runat="server" ImageUrl="~/img/BarCode.png" Height="25px"
            OnClick="ImgBtnIncluirPorCodBarra_Click" />
        Codigo barra
    </div>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table style="width: 840px; margin-left: 17%;">
            <tr>
                <td>
                    <div class="panelItem">
                        <asp:Panel ID="pnSimples" runat="server">
                            <asp:DropDownList ID="ddlLancamentoSimples" runat="server" Font-Size="Large" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlLancamentoSimples_SelectedIndexChanged">
                                <asp:ListItem Value="0">NAO</asp:ListItem>
                                <asp:ListItem Value="1">SIM</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label14" runat="server" Text="Lançamento Simplificado" Font-Size="X-Large"></asp:Label>
                        </asp:Panel>
                    </div>
                    <div class="panelItem" style="float: right; margin-right: 50px;">
                        <p>
                            Usuário
                        </p>
                        <asp:TextBox ID="txtusuario" runat="server" Width="200px" Style="font-size: 20px;"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divParcelas" runat="server" class="panelItem">
                        <div class="panelItem">
                            <p>Parcelas</p>
                            <asp:TextBox ID="txtQtdeParcelas" runat="server" Width="50px" Style="font-size: 20px;"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Por</p>
                            <asp:DropDownList ID="ddlTipoParcelas" runat="server" Height="30" Style="font-size: 21px;" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoParcelas_SelectedIndexChanged">
                                <asp:ListItem Text="Mês"></asp:ListItem>
                                <asp:ListItem Text="Dia"></asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div id="divQtdeDias" runat="server" class="panelItem">
                            <p>Qtd Dias</p>
                            <asp:TextBox ID="txtQtdeDias" runat="server"
                                Width="50px" Style="font-size: 20px;"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <br />
                            <br />
                            <asp:CheckBox ID="chkConsiderarFimDeSemana" runat="server"
                                Text="Forçar Vencimento em dia útil" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Documento
                        </p>
                        <asp:TextBox ID="txtDocumento" runat="server" Width="250px" MaxLength="50" Style="font-size: 20px;"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Serie Nf
                        </p>
                        <asp:TextBox ID="txtSerie" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p style="margin-left: 10px;">
                            Fornecedor
                        </p>
                        <asp:TextBox ID="txtFornecedor" runat="server" AutoPostBack="true" Width="400px" Style="font-size: 20px; margin-left: 10px;"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>



                </td>
            </tr>

            <tr>
                <td>
                    <div class="panelItem" style="width: 160px;">
                        <p>
                            Entrada
                        </p>
                        <asp:TextBox ID="txtentrada" runat="server" Width="120px" Style="font-size: 20px;"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtEntrada" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtEntrada"
                            TargetControlID="txtentrada">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem" style="width: 160px;">
                        <p style="margin-left: 10px;">
                            Emissao
                        </p>
                        <asp:TextBox ID="txtemissao" runat="server" Width="120px" Style="font-size: 20px; margin-left: 10px;"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtEmissao"
                            TargetControlID="txtemissao">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem" style="width: 160px;">
                        <p style="margin-left: 10px;">
                            Vencimento
                        </p>
                        <asp:TextBox ID="txtVencimento" runat="server" Width="120px" Style="font-size: 20px; margin-left: 10px;"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtVencimento" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtVencimento"
                            TargetControlID="txtVencimento">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p style="margin-left: 10px;">
                            Status
                        </p>
                        <asp:DropDownList ID="DllStatus" runat="server" Style="font-size: 20px; margin-left: 10px;">
                            <asp:ListItem Value="0">---- </asp:ListItem>
                            <asp:ListItem Value="1">ABERTO</asp:ListItem>
                            <asp:ListItem Value="2">CONCLUIDO</asp:ListItem>
                            <asp:ListItem Value="3">CANCELADO</asp:ListItem>
                            <asp:ListItem Value="4">LANCADO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p style="margin-left: 10px;">
                            Valor
                        </p>
                        <asp:TextBox ID="txtValor" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                            Style="font-size: 20px; margin-left: 10px; text-align: right;" CssClass="numero"
                            Width="120px"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="400px">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Geral
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="width: 840px; margin-left: 17%;">
                        <tr>
                            <td>
                                <div id="divDesconto" runat="server" class="panelItem">
                                    <p>
                                        Desconto
                                    </p>
                                    <asp:TextBox ID="txtDesconto" runat="server" CssClass="numero" Width="150px" AutoPostBack="True"
                                        OnTextChanged="txt_TextChanged"></asp:TextBox>
                                </div>
                                <div id="divAcrescimo" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Acréscimo
                                    </p>
                                    <asp:TextBox ID="txtAcrescimo" runat="server" CssClass="numero" Width="150px" AutoPostBack="True"
                                        OnTextChanged="txt_TextChanged"></asp:TextBox>
                                </div>
                                <div id="divPagamento" runat="server" class="panelItem" style="margin-left: 10px; width: 130px;">
                                    <p>
                                        Pagamento
                                    </p>
                                    <asp:TextBox ID="txtPagamento" runat="server" Width="100px"></asp:TextBox>
                                    <asp:ImageButton ID="ImgDtPagamento" ImageUrl="~/img/calendar.png" runat="server"
                                        Height="15px" />
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgDtPagamento"
                                        TargetControlID="txtPagamento" Enabled="True">
                                    </asp:CalendarExtender>
                                </div>
                                <div id="divValorPagar" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Valor a Pagar
                                    </p>
                                    <asp:TextBox ID="txtValorPagar" runat="server" CssClass="numero" Width="140px"></asp:TextBox>
                                </div>
                                <div id="divValorPago" runat="server" class="panelItem" style="margin-left: 20px;">
                                    <p>
                                        Valor Pago
                                    </p>
                                    <asp:TextBox ID="txtValor_Pago" runat="server" CssClass="numero" Width="150px" Style="text-align: right;"></asp:TextBox>
                                </div>
                                <div id="divParcial" runat="server" class="panelItem">
                                    <p>
                                        Parcial
                                    </p>
                                    <asp:TextBox ID="txtParcial" runat="server" CssClass="numero" Width="150px"></asp:TextBox>
                                </div>
                                <div id="divDuplicata" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Duplicata
                                    </p>
                                    <asp:TextBox ID="txtDuplicata" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                                <div id="divCentroCusto" runat="server" class="panelItem" style="margin-left: 10px; width: 370px;">
                                    <p>
                                        Centro de Custo
                                    </p>
                                    <asp:TextBox ID="txtCodigo_Centro_Custo" runat="server" Width="100px" OnTextChanged="txtCodigo_Centro_Custo_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:TextBox ID="txtDescCentroCusto" runat="server" Width="230px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnTxtCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                                <div id="divTipoPagamento" runat="server" class="panelItem">
                                    <p>
                                        Tipo Pagamento
                                    </p>
                                    <asp:TextBox ID="txtTipo_Pagamento" runat="server" Width="155px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnTxtTipo_Pagamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                                <div id="divIdConta" runat="server" class="panelItem">
                                    <p>
                                        ID Conta
                                    </p>
                                    <asp:TextBox ID="txtid_cc" runat="server" Width="150px" AutoPostBack="True" OnTextChanged="txtid_cc_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtntxtId_cc" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                                <div id="divBanco" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Banco
                                    </p>
                                    <asp:TextBox ID="txtBanco" runat="server" Width="150px"></asp:TextBox>
                                </div>
                                <div id="divAgencia" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Agencia
                                    </p>
                                    <asp:TextBox ID="txtAgencia" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div id="divConta" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Conta
                                    </p>
                                    <asp:TextBox ID="txtConta" runat="server" Width="150px"></asp:TextBox>
                                </div>
                                <div id="divNumeroCheque" runat="server" class="panelItem" style="margin-left: 10px;">
                                    <p>
                                        Numero cheque
                                    </p>
                                    <asp:TextBox ID="txtNumero_cheque" runat="server" Width="170px"></asp:TextBox>
                                </div>

                                <div class="panelItem">
                                    <br />
                                    <asp:CheckBox ID="chkConferido" runat="server" Text="Conferido" />
                                </div>
                                <div class="panelItem">
                                    <br />
                                    <asp:CheckBox ID="chkBaixa_Automatica" runat="server" Text="Baixa Automatica" AutoPostBack="True"
                                        OnCheckedChanged="chkBaixa_Automatica_CheckedChanged" />
                                </div>

                                <div class="row" id="divContabilEventos" runat="server">
                                    <p>
                                        Evento Contábil
                                    </p>
                                    <asp:TextBox ID="txtContabil_Eventos" runat="server" Width="100px" OnTextChanged="txtContabil_Eventos_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:TextBox ID="txtContabil_Eventos_Descricao" runat="server" Width="450px"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnIDContabilEventos" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>



                                <div class="row">
                                    <p>Codido de Barras</p>
                                    <asp:TextBox ID="txtCodBarras" runat="server" Width="98%" OnTextChanged="txtCodBarras_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                <div class="panelItem" style="width: 100%">
                                    <p>
                                        Observação
                                    </p>
                                    <asp:TextBox ID="txtobs" runat="server" Width="98%" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div id="divTitulosDoMesmoGrupo" runat="server" style="margin-bottom: 30px;">
                        <center><h2>
                            Títulos Relacionados
                            <br />  <asp:Label ID="lblparcelasCondicoes" runat="server"></asp:Label>

                                </h2>

                        </center>


                        <asp:GridView ID="gridTitulosGrupo" runat="server" AutoGenerateColumns="False" Width="50%"
                            CellPadding="5" ForeColor="#333333" GridLines="Vertical" AllowSorting="True"
                            DataKeyNames="Documento,fornecedor,valor">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:HyperLinkField DataTextField="Documento" Text="Documento" HeaderText="Documento"
                                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Documento" />
                                <asp:BoundField DataField="Valor" HeaderText="Valor" >
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StrVencimento" HeaderText="Vencimento" >
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" >
                                <ItemStyle HorizontalAlign="Center" />
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
                    Centro de Custos
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:GridView ID="gridCentroCusto" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" Width="80%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Codigo_centro_custo" HeaderText="Codigo"></asp:BoundField>
                            <asp:BoundField DataField="Descricao_centro_custo" HeaderText="Centro de Custo"></asp:BoundField>
                            <asp:BoundField DataField="Valor" ItemStyle-HorizontalAlign="Right" HeaderText="Total"></asp:BoundField>

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
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label1" runat="server" Text="Confirma Excluir" CssClass="cabMenu"></asp:Label>
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
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label1">
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


    <asp:Panel ID="pnConfirmaAlteracaoGrupo" runat="server" CssClass="frameModal" Style="display: none">
        <h2>
            <asp:Label ID="Label5" runat="server" Text="Gostaria de atribuir as alteração aos titulos relacionados que ainda estão abertos e com vencimento Maior?" CssClass="cabMenu" Style="height: 80px;"></asp:Label>
        </h2>

        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaGrupo" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaGrupo_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaGrupo" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaGrupo_Click" />
                    <asp:Label ID="Label7" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaAlteracaoGrupo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaAlteracaoGrupo" TargetControlID="Label5">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnCodBarras" runat="server" CssClass="frameModal" DefaultButton="ImgbtnConfirmaCodBarra" Style="display: none; width: 400px;">
        <div>

            <h2 class="cabMenu">Digite o codigo de barras
            </h2>
        </div>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="ImgbtnConfirmaCodBarra" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgbtnConfirmaCodBarra_Click" />
                    <asp:Label ID="Label9" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImgBtnCancelarCodBarra" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnCancelarCodBarra_Click" />
                    <asp:Label ID="Label10" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="frame" style="height: 50px;">
            <div class="row" style="text-align: center;">
                <asp:TextBox ID="txtlinhaDigitavel" runat="server" Width="90%" />
            </div>
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCodBarras" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnCodBarras" TargetControlID="Label10">
    </asp:ModalPopupExtender>

</asp:Content>
