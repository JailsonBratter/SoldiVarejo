<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfEntradaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfEntradaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>Detalhes da Nota de Entrada</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Panel ID="cabecalho" runat="server" CssClass="frame">
        <asp:HyperLink ID="btnImpressao" runat="server" NavigateUrl="~/modulos/NotaFiscal/pages/NotaPrint.aspx"
            CssClass="btnImprimirDireita" Target="_blank">
            <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" />
        </asp:HyperLink>
        <table>
            <tr>
                <td>
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Serie
                    </p>
                    <asp:TextBox ID="txtSerie" runat="server" Width="50px" CssClass="numero"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ
                    </p>
                    <asp:TextBox ID="txtFornecedor_CNPJ" runat="server" AutoPostBack="true" OnTextChanged="txtFornecedor_CNPJ_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtFornecedor_CNPJ" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
                <td>
                    <p>
                        Emissao
                    </p>
                    <asp:TextBox ID="txtEmissao" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDtEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataEmissao" runat="server" PopupButtonID="imgDtEmissao"
                        TargetControlID="txtEmissao">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Data
                    </p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDtCalendario" ImageUrl="~/img/calendar.png" runat="server"
                        Height="15px" />
                    <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="imgDtCalendario"
                        TargetControlID="txtData">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:CheckBox ID="chknf_Canc" runat="server" Text="Cancelada" />
                </td>
                <td>
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtusuario" runat="server"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td colspan="3">
                    <p>
                        Fornecedor
                    </p>
                    <asp:TextBox ID="txtCliente_Fornecedor" runat="server" Width="100px"></asp:TextBox>
                    <asp:TextBox ID="TxtNomeFornecedor" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Pedido
                    </p>
                    <asp:TextBox ID="txtPedido" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>Centro Custo</p>
                    <asp:TextBox ID="txtcentro_custo" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtcentro_custo" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
                <td>
                    <p>
                        Natureza Operacao
                    </p>
                    <asp:TextBox ID="txtCodigo_operacao" runat="server" CssClass="numero" Width="100px" OnTextChanged="txtCodigo_operacao_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtCodigo_operacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
                <td>
                    <p>
                        Usuario Ult Alteracao
                    </p>
                    <asp:TextBox ID="txtusuario_Alteracao" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">Chave
                    <asp:TextBox ID="txtid" runat="server" Width="500px"></asp:TextBox>
                </td>

                <td>
                    <div id="divCorrigirPorc" runat="server" visible="false">
                        <asp:ImageButton ID="imgBtnMostraPorcCredito" runat="server" ImageUrl="~/img/edit.png"
                            Height="15px" OnClick="imgBtnMostraPorcCredito_Click" />(%)Simples
                    </div>
                </td>

            </tr>
        </table>
    </asp:Panel>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="400px">
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
                    <asp:Panel ID="Panel1" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="ImgBtnAddReferencia" runat="server" ImageUrl="~/img/add.png"
                            Height="20px" Width="20px" OnClick="ImgBtnAddReferencia_Click" />
                        Incluir P/Referencia Fornecedor
                    </asp:Panel>
                    <div class="gridnf">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound" OnRowCommand="gridItens_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                    <ItemStyle Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="PLU" HeaderText="PLU">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CODIGO_REFERENCIA" HeaderText="Cod Forn">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="False">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DescricaoNotaDeEntrada" HeaderText="Descrição NF" HtmlEncode="False">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="qtde" HeaderText="Qtde">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Embalagem" HeaderText="Emb">
                                    <ItemStyle Width="40px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Unitario" HeaderText="Preço">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Codigo_Tributacao" HeaderText="Trib">
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Desconto" HeaderText="Desc%">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ipi" HeaderText="IPI">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ipiv" HeaderText="Ipiv">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Despesas" HeaderText="Desp.">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Margem_iva" HeaderText="M.IVA%">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="iva" HeaderText="IVA">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="codigo_operacao" HeaderText="CFOP">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aliquota_icms" HeaderText="ICMS">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="redutor_base" HeaderText="Redutor">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="pisv" HeaderText="PIS Vlr">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="COFINSV" HeaderText="COFINS Vlr">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="num_item" HeaderText="Item NF">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="pCredSN" HeaderText="%Cred SN">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vCredicmssn" HeaderText="V Cred icms SN ">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="data_validade" HeaderText="Data Validade">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
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
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Pagamentos
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="AddPg" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="AddPagamento" runat="server" ImageUrl="~/img/add.png" Height="20px"
                            Width="20px" OnClick="AddPagamento_Click" />
                        Incluir pagamento
                    </asp:Panel>
                    <div class="gridnf">
                        <asp:GridView ID="gridPagamentos" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowCommand="gridPagamentos_RowCommand">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                    <ItemStyle Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Tipo_pagamento" HeaderText="Tipo Pagamento"></asp:BoundField>
                                <asp:BoundField DataField="Vencimento" HeaderText="Vencimento"></asp:BoundField>
                                <asp:BoundField DataField="Valor" HeaderText="Valor"></asp:BoundField>
                                <asp:BoundField DataField="Cod_barra" HeaderText="Cod Barra"></asp:BoundField>
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
                        <div class="rodapeRelatorio">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkBoletoRecebido" runat="server" Text="Boleto Recebido" />
                                    </td>
                                    <td>Pagamentos:
                                        <asp:Label ID="lblTotalPagamentos" runat="server" Text="0,00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                <HeaderTemplate>
                    Observação
                </HeaderTemplate>
                <ContentTemplate>
                    <td>
                        <p>
                            Observacao
                        </p>
                        <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Height="150px"
                            Width="99%"></asp:TextBox>
                    </td>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tabCentroCusto" runat="server" HeaderText="tabCentroCusto">
                <HeaderTemplate>
                    Centro de custos
                </HeaderTemplate>
                <ContentTemplate>

                    <h4 style="text-align: center">Centros de Custos
                    </h4>
                    <hr>
                    <div class="gridnf">
                        <asp:GridView ID="gridCentroCusto" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" Width="80%">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Codigo_centro_custo" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="Descricao_centro_custo" HeaderText="Centro de Custo"></asp:BoundField>
                                <asp:BoundField DataField="Valor" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="Total"></asp:BoundField>

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
            <asp:TabPanel ID="tabHistoricoEdicao" runat="server" HeaderText="tabHistoricoEdicao">
                <HeaderTemplate>
                    Histórico Edição
                </HeaderTemplate>
                <ContentTemplate>

                    <h4 style="text-align: center">Alterações
                    </h4>
                    <hr>
                    <div class="gridnf">
                        <asp:GridView ID="gridHistorico" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" Width="80%">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="usuario" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="data_alteracao" HeaderText="Data Alteração"></asp:BoundField>
                                <asp:BoundField DataField="justificativa" HeaderText="Justificativa"></asp:BoundField>

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
            <asp:TabPanel ID="tabCTe" runat="server" HeaderText="CT-e" Visible="false">
                <HeaderTemplate>
                    CT-e (Conhecimento de Transportes Eletrônico)
                </HeaderTemplate>
                <ContentTemplate>

                    <h4 style="text-align: center">CT-e
                    </h4>
                    <hr>
                    <div>
                        <table class="table">
                            <tr>
                                <td>Código da situação do documento</td>
                                <td>
                                    <asp:DropDownList ID="ddlCTeSitDOC" runat="server">
                                        <asp:ListItem Value=""> </asp:ListItem>
                                        <asp:ListItem Value="00">Documento Regular </asp:ListItem>
                                        <asp:ListItem Value="02">Documento Cancelado</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Código do participante (Fornecedor Frete)</td>
                                <td>
                                    <asp:TextBox ID="txtCTeCodigoFornecedor" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                    <asp:ImageButton ID="btnimg_txtCTeCodigoFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>Número do documento</td>
                                <td>
                                    <asp:TextBox ID="txtCTeNumeroDocumento" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Série do documento</td>
                                <td>
                                    <asp:TextBox ID="txtCTeSerie" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Data de emissão</td>
                                <td>
                                    <asp:TextBox ID="txtCTeEmissao" runat="server" Width="80px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtCTeEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="cldCTeEmissao" runat="server" PopupButtonID="imgDtCTeEmissao"
                                        TargetControlID="txtCTeEmissao">
                                    </asp:CalendarExtender>
                                </td>
                                <td>Data de aquisição/prestação do serviço</td>
                                <td>
                                    <asp:TextBox ID="txtCTeAquisicao" runat="server" Width="80px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtCTeAquisicao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="cldCTeAquisicao" runat="server" PopupButtonID="imgDtCTeAquisicao"
                                        TargetControlID="txtCTeAquisicao">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Data de Vencimento Financeiro</td>
                                <td>
                                    <asp:TextBox ID="txtCTeVencimento" runat="server" Width="80px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDTCTeVencimento" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="cIdCTeVencimento" runat="server" PopupButtonID="imgDTCTeVencimento"
                                        TargetControlID="txtCTeVencimento">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Chave do Conhecimento de Transporte Eletrônico</td>
                                <td>
                                    <asp:TextBox ID="txtCTeChave" runat="server" Text="" Width="400px"  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Tipo de CT-e ou BP-e</td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoCTeBPe" runat="server">
                                        <asp:ListItem Value=""> </asp:ListItem>
                                        <asp:ListItem Value="0">CT-e ou BP-e Normal</asp:ListItem>
                                        <asp:ListItem Value="1">CT-e de Complemento de Valores</asp:ListItem>
                                        <asp:ListItem Value="2">Ct-e emitido em hipótese de anulação</asp:ListItem>
                                        <asp:ListItem Value="3">CT-e substituto do CT-e anulado</asp:ListItem>
                                        <asp:ListItem Value="4">BP-e TM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Chave do BP-e substituído</td>
                                <td>
                                    <asp:TextBox ID="txtCTeChaveSubstituicao" runat="server" Text="" Width="400px"  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Tipo de Frete</td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoCTeFrete" runat="server">
                                        <asp:ListItem Value=""> </asp:ListItem>
                                        <asp:ListItem Value="0">Por conta do emitente</asp:ListItem>
                                        <asp:ListItem Value="1">Por conta do destinatário/remetente</asp:ListItem>
                                        <asp:ListItem Value="2">Por conta de terceiros</asp:ListItem>
                                        <asp:ListItem Value="3">Sem cobrança de frete</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Base de cálculo do ICMS</td>
                                <td>
                                    <asp:TextBox ID="txtCTeBCICMS" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Redução de Base</td>
                                <td>
                                    <asp:TextBox ID="txtCTeReducao" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Alíquota ICMS</td>
                                <td>
                                    <asp:TextBox ID="txtCTeAliquota" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Valor do ICMS</td>
                                <td>
                                    <asp:TextBox ID="txtCTeValorICMS" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Valor do documento fiscal</td>
                                <td>
                                    <asp:TextBox ID="txtCTeValorDocFiscal" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Valor total da prestaçã de serviço</td>
                                <td>
                                    <asp:TextBox ID="txtCTeValorTotalPrestServico" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Valor total do desconto</td>
                                <td>
                                    <asp:TextBox ID="txtCTeValorTotalDesconto" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                                <td>Valor não tributado</td>
                                <td>
                                    <asp:TextBox ID="txtCTeValorNaoTributado" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Código do município de origem (IBGE)</td>
                                <td>
                                    <asp:TextBox ID="txtCTeMunicipioOrigem" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                    <asp:ImageButton ID="btnimg_txtCTeMunicipioOrigem" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                                </td>
                                <td>Código do município de destino (IBGE)</td>
                                <td>
                                    <asp:TextBox ID="txtCTeMunicipioDestino" runat="server" Text=""  CssClass="numero" ></asp:TextBox>
                                    <asp:ImageButton ID="btnimg_txtCTeMunicipioDestino" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>

        </asp:TabContainer>
    </div>
    <div id="rodape" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Frete
                    </p>
                    <asp:TextBox ID="txtFrete" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                        OnTextChanged="txtFrete_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Seguro
                    </p>
                    <asp:TextBox ID="txtSeguro" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                        OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        IPI
                    </p>
                    <asp:TextBox ID="txtIPI_Nota" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Base de Calculo ICMS
                    </p>
                    <asp:TextBox ID="txtBase_Calculo" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>

                <td>
                    <p>FCP ST </p>
                    <asp:TextBox ID="txtTotal_vFCPST" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Base de Calculo Substituição
                    </p>
                    <asp:TextBox ID="txtBase_Calc_Subst" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>

                <td colspan="2">
                    <p>
                        Desconto Rateio
                    </p>
                    <asp:TextBox ID="txtDesconto" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        Despesas
                    </p>
                    <asp:TextBox ID="txtDespesas_financeiras" runat="server" CssClass="numero" Width="100px"
                        AutoPostBack="true" OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Outras
                    </p>
                    <asp:TextBox ID="txtOutras" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                        OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Desconto
                    </p>
                    <asp:TextBox ID="txtDesconto_geral" runat="server" CssClass="numero" Width="100px"
                        AutoPostBack="true" OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Valor ICMS
                    </p>
                    <asp:TextBox ID="txtICMS_Nota" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>Valor FCP</p>
                    <asp:TextBox ID="txtTotal_vFCP" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Valor ICMS Substituição
                    </p>
                    <asp:TextBox ID="txtICMS_ST" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Total Produtos
                    </p>
                    <asp:TextBox ID="txtTotalProdutos" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Total Nota
                    </p>
                    <asp:TextBox ID="txtTotal" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none; height: 450px;">
        <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Dados do Item</h1>
                <asp:Label ID="lblErroItem" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaItens" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaItens_Click" />
                        <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaItem" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelaItem_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>
                                PLU
                            </p>
                            <asp:TextBox ID="txtPLU" runat="server" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)" Width="50px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtPLU" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                            <asp:Label ID="lblInativo" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div class="panelItem">

                            <p>
                                Cod Fornecedor
                            </p>
                            <asp:TextBox ID="txtCODIGO_REFERENCIA" runat="server" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricao" runat="server" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)" Width="400px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Und
                            </p>
                            <asp:TextBox ID="txtUnd" runat="server" Width="30px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Qtde
                            </p>
                            <asp:TextBox ID="txtQtde" runat="server" CssClass="numero" Width="75px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Embalagem
                            </p>
                            <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Preço
                            </p>
                            <asp:TextBox ID="txtUnitario" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>

                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    Desconto %
                                </p>
                                <asp:TextBox ID="txtDescontoItem" runat="server" CssClass="numero" Width="100px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Desconto Valor
                                </p>
                                <asp:TextBox ID="txtDescValorItem" runat="server" CssClass="numero" Width="100px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    IPI%
                                </p>
                                <asp:TextBox ID="txtIPI" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    IPI Valor
                                </p>
                                <asp:TextBox ID="txtIPIV" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="panelItem" style="border-style: solid; border-width: 1px;">

                            <div class="panelItem">
                                <p>
                                    CFOP
                                </p>
                                <asp:TextBox ID="txtCodigo_operacao_item" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtCodigo_operacao_item" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    NCM
                                </p>
                                <asp:TextBox ID="txtNCM" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtNCM" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    Item NF
                                </p>
                                <asp:TextBox ID="txtNum_item" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                            </div>

                            <div class="panelItem">
                                <p>
                                    Peso_liquido
                                </p>
                                <asp:TextBox ID="txtPeso_liquido" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Peso Bruto
                                </p>
                                <asp:TextBox ID="txtPeso_Bruto" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    Tributacao
                                </p>
                                <asp:TextBox ID="txtCodigo_Tributacao" runat="server" CssClass="numero" Width="50px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtCodigo_Tributacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    ICMS%
                                </p>
                                <asp:TextBox ID="txtaliquota_icms" runat="server" CssClass="numero" Width="100px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    CST Icms
                                </p>
                                <asp:TextBox ID="txtIndice_st" runat="server" Width="50px"></asp:TextBox>

                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    Redutor%
                                </p>
                                <asp:TextBox ID="txtredutor_base" runat="server" CssClass="numero" Width="70px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                                <asp:TextBox ID="txtredutor_base_iva" runat="server" CssClass="numero" Width="70px"
                                    Visible="false"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Margem IVA%
                                </p>
                                <asp:TextBox ID="txtmargem_iva" runat="server" CssClass="numero" Width="70px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    IVA
                                </p>
                                <asp:TextBox ID="txtiva" runat="server" CssClass="numero" Width="70px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">


                            <div class="panelItem">
                                <p>
                                    PIS(%)
                                </p>
                                <asp:TextBox ID="txtPisItem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Pis Vlr
                                </p>
                                <asp:TextBox ID="txtVlrPisItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    COFINS(%)
                                </p>
                                <asp:TextBox ID="txtCofinsItem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    COFINS Vlr
                                </p>
                                <asp:TextBox ID="txtVlrCofinsItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    CST Pis/Cofins
                                </p>
                                <asp:TextBox ID="txtCSTPIS" runat="server" Width="50px"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtCSTPIS" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                            <div class="panelItem">
                                <p>
                                    Base FCP
                                </p>
                                <asp:TextBox ID="txtItem_BaseFCP" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Aliq FCP
                                </p>
                                <asp:TextBox ID="txtItem_pFCP" runat="server" Width="50px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Vlr FCP
                                </p>
                                <asp:TextBox ID="txtItem_VlrFCP" runat="server" CssClass="numero" Width="100px" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                            <div class="panelItem">
                                <p>
                                    Base FCP ST
                                </p>
                                <asp:TextBox ID="txtItem_BaseFCPST" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Aliq FCP ST
                                </p>
                                <asp:TextBox ID="txtItem_pFCPST" runat="server" Width="50px" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Vlr FCP ST
                                </p>
                                <asp:TextBox ID="txtItem_VlrFCPST" runat="server" CssClass="numero" Width="100px" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem" style="float: right">
                            <p><span class="titulobtn">TOTAL </span></p>
                            <asp:TextBox ID="TxtTotalItem" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem" style="float: right">
                            <p><span class="titulobtn">CUSTO </span></p>
                            <asp:TextBox ID="txtTotalCustoItem" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                    </td>

                </tr>
                <tr>
                    <td>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    % Cred SN
                                </p>
                                <asp:TextBox ID="txtpCredSN" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Valor Cred Icms SN
                                </p>
                                <asp:TextBox ID="txtvCredicmssn" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                            </div>

                        </div>
                        <div class="panelItem">
                            <p>
                                Outras Despesas
                            </p>
                            <asp:TextBox ID="txtdespesas" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Data Validade</p>
                            <asp:TextBox ID="txtDtValidade" runat="server" Width="80px" CssClass="DATA"></asp:TextBox>
                            <asp:ImageButton ID="imgDtValidade" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtValidade"
                                TargetControlID="txtDtValidade">
                            </asp:CalendarExtender>
                        </div>
                        <div class="panelItem">
                            <p>Valor Frete</p>
                            <asp:TextBox ID="txtValorFreteItem" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged"></asp:TextBox>

                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PnExcluirItem" runat="server" CssClass="frame" Visible="false">
            <asp:ImageButton ID="ImgExcluiItem" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                OnClick="ImgExcluiItem_Click" />
            <asp:Label ID="Label6" runat="server" Text="Excluir Item"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalItens" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblError" DropShadow="true" PopupControlID="pnItens" TargetControlID="lblError">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
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
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowCommand="GridLista_RowCommand" OnRowDataBound="GridLista_RowDataBound">
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
        DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnAddPagamento" runat="server" CssClass="modalForm" Style="display: none; height: 220px;">
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
            <table style="padding: 10px">
                <tr>
                    <td>
                        <p>
                            Tipo Pagamento
                        </p>
                        <asp:TextBox ID="txtTipoPg" runat="server" Width="200px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtTipoPg" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <p>
                            Vencimento
                        </p>
                        <asp:TextBox ID="txtVencimentoPg" runat="server" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgVencimentoPg" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgVencimentoPg"
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
                <tr>
                    <td colspan="3">
                        <p>Codigo Barra</p>
                        <asp:TextBox ID="txtCodBarra" runat="server" Width="100%"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPagamentos" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroPg" DropShadow="true" PopupControlID="pnAddPagamento"
        TargetControlID="lblErroPg">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnImportar" runat="server" CssClass="modalForm" Style="display: none; height: 230px;">
        <asp:Panel ID="PnImportarFrame" runat="server" CssClass="frame" DefaultButton="btnConfirmaImportar">
            <div class="cabMenu">
                <h1>INFORMAR CHAVE DE ACESSO OU PEDIDO
                </h1>
                <asp:Label ID="lblErroImportacao" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaImportar" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaImportar_Click" />
                        <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaImportar" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaImportar_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            CHAVE DE ACESSO DA NFe
                        </p>
                        <asp:TextBox ID="txtNFE" runat="server" Width="500px" CssClass="sem"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Numero Pedido
                        </p>
                        <asp:TextBox ID="txtNumeroPedido" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalImportar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnImportar" TargetControlID="lblErroImportacao">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnConfirmaPg" runat="server" CssClass="modalForm" Style="display: none; height: 200px; width: 500px;">
        <asp:Panel ID="pnConfirmaPgFrame" runat="server" CssClass="frame" Height="150px">
            <b>
                <h2>
                    <center>
                O TOTAL DOS PAGAMENTOS NÃO BATEM COM O TOTAL DA NOTA!
                    <br />
                    GOSTARIA DE SALVAR MESMO ASSIM?
                    </center>
                </h2>
            </b>
            <hr />
            <asp:Label ID="Label7" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div class="panelItem" style="margin-left: 20%;">
                <asp:ImageButton ID="btnConfirmaPgAdd" runat="server" ImageUrl="~/img/confirm.png"
                    Width="25px" OnClick="btnConfirmaPgAdd_Click" />
                <asp:Label ID="Label12" runat="server" Text="Confirma"></asp:Label>
            </div>
            <div class="panelItem" style="margin-left: 20%;">
                <asp:ImageButton ID="btnCancelaPgAdd" runat="server" ImageUrl="~/img/cancel.png"
                    Width="25px" OnClick="btnCancelaPgAdd_Click" />
                <asp:Label ID="Label13" runat="server" Text="Cancela"></asp:Label>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaPgAdd" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaPg" TargetControlID="Label7">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label14" runat="server" Text="Confirma Cancelamento" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label14">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnCorrigiPorc" runat="server" CssClass="frameModal" Style="display: none; height: 200px;" DefaultButton="imgBtnConfirmaPorcCredito">
        <asp:Label ID="Label2" runat="server" Text="Informe o Valor do Credito" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaPorcCredito" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaPorcCredito_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaPorcCredito" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaPorcCredito_Click" />
                    <asp:Label ID="Label17" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>

        </table>
        <div class="panel" style="width: 100%;">
            <div class="panelItem" style="margin-left: 40%;">
                <asp:TextBox ID="txtValorPorcCredito" runat="server" Width="80px" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
            </div>
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCorrigiPorc" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnCorrigiPorc" TargetControlID="Label3">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnJustificativaEditar" runat="server" CssClass="frameModal" Style="display: none; padding: 5px;">
        <asp:Label ID="Label18" runat="server" Text="Justificativa Edição" CssClass="cabMenu"></asp:Label>
        <asp:Label ID="lblErroConfirmaEdicao" runat="server" Text="" ForeColor="Red"></asp:Label>
        <table class="frame">
            <tr>
                <td style="padding-right: 40px;">
                    <asp:ImageButton ID="imgBtnConfirmaEdicao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaEdicao_Click" />
                    <asp:Label ID="Label19" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td style="padding-left: 40px;">
                    <asp:ImageButton ID="imgBtnCancelaEdicao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaEdicao_Click" />
                    <asp:Label ID="Label20" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="frame">
            <tr>
                <td>
                    <p>Justificativa</p>
                    <asp:TextBox ID="txtJustificativaEdicao" TextMode="MultiLine" runat="server" Width="500" Height="100" />

                </td>
            </tr>
        </table>


    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaEdicao" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnJustificativaEditar" TargetControlID="Label18">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 100px; overflow: auto;">
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

    <asp:Panel ID="pnCadastroFornecedor" runat="server" CssClass="frameModal" Style="display: none; padding: 10px; overflow: auto; width: 110%">
        <div class="rowFlex">
            <asp:Label ID="Label21" runat="server" Text="Novo Fornecedor" CssClass="cabMenu"></asp:Label>
        </div>
        <div class="rowFlex" style="justify-content: space-around;">
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="ImgBtnConfirmaNovoFornecedor" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConfirmaNovoFornecedor_Click" />
                    <asp:Label ID="Label23" runat="server" Text="Confirma"></asp:Label>
                </div>

            </div>
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="ImgBtnCancelaNovoFornecedor" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnCancelaNovoFornecedor_Click" />
                    <asp:Label ID="Label24" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
        </div>
        <hr />
        <div class="rowFlex">
            <div class="col1">
                <p>Fornecedor</p>
                <asp:TextBox ID="txtNovoFornecedor" runat="server" Width="100%" MaxLength="19" />
            </div>
            <div class="col2">
                <p>Razão Social</p>
                <asp:TextBox ID="txtNovoRazaoSocial" runat="server" Width="100%" />
            </div>
            <div class="col2">
                <p>Nome Fantasia</p>
                <asp:TextBox ID="txtNovoFantasia" runat="server" Width="100%" />
            </div>
        </div>
        <div class="rowFlex">
            <div class="col1">
                <br />
                <asp:CheckBox ID="chknovoPessoaFisica" Text="Pessoa Fisica" runat="server" AutoPostBack="true" OnCheckedChanged="chknovoPessoaFisica_CheckedChanged" />
            </div>
            <div class="col2">
                <p>
                    <asp:Label ID="lblNovoCnpjCpf" Text="CNPJ" runat="server" /></p>
                <asp:TextBox ID="txtNovoCnpjCpf" runat="server" Width="100%" />
            </div>
            <div class="col2">
                <p>
                    <asp:Label ID="lblNovoIeRg" Text="IE" runat="server" /></p>
                <asp:TextBox ID="txtNovoIeRg" runat="server" Width="100%" />
            </div>
            <div class="col2">
                <p>Telefone</p>
                <asp:TextBox ID="txtNovoTelefone" runat="server" Width="100%" />
            </div>
        </div>
        <div class="rowFlex">
            <div class="col1">
                <p>CEP</p>
                <asp:TextBox ID="txtNovoCep" runat="server" Width="100%" />
            </div>
            <div class="col3">
                <p>Endereco</p>
                <asp:TextBox ID="txtNovoEndereco" runat="server" Width="100%" />
            </div>
            <div class="col1">
                <p>Numero</p>
                <asp:TextBox ID="txtNovoEnderecoNro" runat="server" Width="100%" />
            </div>
            <div class="col2">
                <p>Bairro</p>
                <asp:TextBox ID="txtNovoBairro" runat="server" Width="100%" />
            </div>
        </div>
        <div class="rowFlex">
            <div class="col3">
                <p>Cidade</p>
                <asp:TextBox ID="txtNovoCidade" runat="server" Width="100%" />
            </div>
            <div class="col1">
                <p>Uf</p>
                <asp:TextBox ID="txtNovoUf" runat="server" Width="100%" />
            </div>
            <div class="col2">
                <p>Cod Municipio</p>
                <asp:TextBox ID="txtNovoCodMunicipio" runat="server" Width="100%" />
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalNovoFornecedor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnCadastroFornecedor" TargetControlID="Label21">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnCadastroProdutoDetalhes" runat="server" CssClass="frameModal" Style="display: none; padding: 10px; overflow: auto; width: 80%">
        <div class="rowFlex">
            <asp:Label ID="Label22" runat="server" Text="Novo Produto" CssClass="cabMenu"></asp:Label>
            <asp:Label ID="lblIndexProdutosNovo" runat="server" Text="" Visible="false"></asp:Label>
        </div>
        <div class="rowFlex" style="justify-content: space-around;">
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="imgBtnConfirmaNovoProduto" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaNovoProduto_Click" />
                    <asp:Label ID="Label25" runat="server" Text="Confirma"></asp:Label>
                </div>

            </div>
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="imgBtnCancelarNovoProduto" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarNovoProduto_Click" />
                    <asp:Label ID="Label26" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
        </div>
        <hr />
        <div class="rowFlex">
            <div class="col1">
                <p>PLU</p>
                <asp:TextBox ID="txtNovoPlu" runat="server" Width="80%" MaxLength="19" />
                <asp:ImageButton ID="btnimg_txtNovoPlu" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="imgLista_Click" />
            </div>
            <div class="col1">
                <p>EAN</p>
                <asp:TextBox ID="txtNovoEan" runat="server" Width="100%" />
            </div>
            <div class="col1">
                <p>Ref</p>
                <asp:TextBox ID="txtNovoRef" runat="server" Width="100%" />
            </div>
            <div class="col3">
                <p>Descrição</p>
                <asp:TextBox ID="txtNovoDescricao" runat="server" Width="100%" />
            </div>
        </div>
        <div class="rowFlex">
            <div class="col1">
                <p>NCM</p>
                <asp:TextBox ID="txtNovoNCM" runat="server" Width="100%" />
            </div>
            <div class="col3">
                <p>Tributação</p>
                <asp:TextBox ID="txtNovoCodTributacao" runat="server" Width="10%" />
                <asp:TextBox ID="txtNovoDescricaoTributacao" runat="server" Width="80%" />
            </div>
            <div class="col1">
                <p>CST Pis/Cofins</p>
                <asp:TextBox ID="txtNovoCSTPisCofins" runat="server" Width="100%" />
            </div>
            <div class="col1">
                <p>Pis %</p>
                <asp:TextBox ID="txtNovoPis" runat="server" Width="100%" />
            </div>
            <div class="col1">
                <p>Cofins %</p>
                <asp:TextBox ID="txtNovoCofins" runat="server" Width="100%" />
            </div>
        </div>
        <div class="rowFlex">
            <div class="col2">
                <p>Grupo</p>
                <asp:TextBox ID="txtNovoCodGrupo" runat="server" Width="30%" />
                <asp:TextBox ID="txtNovoDescGrupo" runat="server" Width="60%" />
            </div>
            <div class="col2">
                <p>Sub Grupo</p>
                <asp:TextBox ID="txtNovoCodSubGrupo" runat="server" Width="30%" />
                <asp:TextBox ID="txtNovoDescSubGrupo" runat="server" Width="60%" />
            </div>
            <div class="col2">
                <p>Departamento</p>
                <asp:TextBox ID="txtNovoCodDepartamento" runat="server" Width="30%" />
                <asp:TextBox ID="txtNovoDescDepartamento" runat="server" Width="60%" />
            </div>
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCadastroProdutoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnCadastroProdutoDetalhes" TargetControlID="Label22">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnCadastroProdutos" runat="server" CssClass="frameModal" Style="display: none; padding: 10px; overflow: auto; width: 110%">
        <div class="rowFlex">
            <asp:Label ID="Label27" runat="server" Text="Novos Produtos" CssClass="cabMenu"></asp:Label>
        </div>
        <div class="rowFlex" style="justify-content: space-around;">
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="imgBtnConfirmaNovosProdutos" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaNovosProdutos_Click" />
                    <asp:Label ID="Label28" runat="server" Text="Confirma"></asp:Label>
                </div>

            </div>
            <div style="display: flex; flex: 1; justify-content: center;">
                <div>
                    <asp:ImageButton ID="imgBtnCancelarNovosProdutos" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarNovosProdutos_Click" />
                    <asp:Label ID="Label29" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
        </div>
        <hr />
        <div class="rowFlex" style="overflow: auto; height: 300px; border: solid 1px">
            <asp:GridView ID="gridNovosProdutosCadastrar" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                OnRowCommand="gridNovosProdutosCadastrar_RowCommand"
                AutoGenerateColumns="False" Style="width: 115%">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                        Text="Alterar">
                        <ControlStyle Height="20px" Width="20px" />
                        <ItemStyle Width="20px" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="PLU" HeaderText="PLU" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EAN" HeaderText="EAN" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="CODIGO_REFERENCIA" HeaderText="REF" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="NCM" HeaderText="NCM" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="Codigo_tributacao_novo" HeaderText="Cod" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="Descricao_tributacao_novo" HeaderText="Tributação Saida" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="Codigo_departamento_novo" HeaderText="Cod" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="Descricao_departamento_novo" HeaderText="Departamento" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>

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
    <asp:ModalPopupExtender ID="modalCadastroNovosProdutos" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnCadastroProdutos" TargetControlID="Label27">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnConsultarSefaz" runat="server" CssClass="frameModal" Style="display: none; padding: 5px;">
        <asp:Label ID="Label30" runat="server" Text="Chave não Cadstrada , consultar no SEFAZ?" CssClass="cabMenu"></asp:Label>
        <div style="padding: 50px;">

            <asp:Label ID="lblChaveConsultarSefaz" runat="server" Text="" Style="text-align: center;"></asp:Label>
        </div>
        <table class="frame">

            <tr>
                <td style="padding-right: 40px;">
                    <asp:ImageButton ID="ImgBtnConsultaChaveSefaz" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConsultaChaveSefaz_Click" />
                    <asp:Label ID="Label32" runat="server" Text="Sim"></asp:Label>
                </td>
                <td style="padding-left: 40px;">
                    <asp:ImageButton ID="ImgBntCancelaChaveSefaz" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBntCancelaChaveSefaz_Click" />
                    <asp:Label ID="Label33" runat="server" Text="Não"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConsultarSefaz" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConsultarSefaz" TargetControlID="Label30">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnMensagemAviso" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblMensagemAviso" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; width:600px; : 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkMensagemAviso" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkMensagemAviso_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMensagemAviso" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMensagemAviso" TargetControlID="lblMensagemAviso">
    </asp:ModalPopupExtender>


</asp:Content>
