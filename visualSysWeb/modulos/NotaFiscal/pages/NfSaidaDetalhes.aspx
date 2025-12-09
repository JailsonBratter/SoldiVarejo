<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfSaidaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfSaidaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/NfSaidaDetalhes.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Nota
            </h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <asp:Button ID="btnXml" runat="server" Text="XML" OnClick="btnXml_Click" />
        <asp:Button ID="btnFaturaPedido" runat="server" Text="Fatura Pedido" OnClick="btnFaturaPedido_Click"
            Visible="false" />
        <table>
            <tr>
                <td>
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ
                    </p>
                    <asp:TextBox ID="txtCliente_CNPJ" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtCliente_CNPJ" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
                <td>
                    <p>
                        Emissao
                    </p>
                    <asp:TextBox ID="txtEmissao" runat="server" Width="100px"></asp:TextBox>
                    <asp:Image ID="ImgDtEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataEmissao" runat="server" PopupButtonID="imgDtEmissao"
                        TargetControlID="txtEmissao">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Data
                    </p>
                    <asp:TextBox ID="txtData" runat="server" Width="100px"></asp:TextBox>
                    <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtData">
                    </asp:CalendarExtender>
                </td>
                <td>

                    <p>
                        Status
                    </p>
                    <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
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
                        Destinatario
                    </p>
                    <asp:TextBox ID="txtCliente_Fornecedor" runat="server" Width="100px"></asp:TextBox>
                    <asp:TextBox ID="TxtNomeCliente" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Pedido
                    </p>
                    <asp:TextBox ID="txtPedido" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Centro Custo
                    </p>
                    <asp:TextBox ID="txtcentro_custo" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtcentro_custo" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
                <td>
                    <p>
                        Natureza Operacao
                    </p>
                    <asp:TextBox ID="txtCodigo_operacao" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="btnimg_txtCodigo_operacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgLista_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="panelItem">
                        <p>
                            Entrega
                        </p>
                        <asp:TextBox ID="txtEntrega" runat="server" Width="100px"></asp:TextBox>

                    </div>
                    <div class="panelItem">
                        <p>
                            Ordem de compra
                        </p>
                        <asp:TextBox ID="txtOrdemCompra" runat="server" Width="100px"></asp:TextBox>

                    </div>

                </td>
                <td>
                    <p>
                        UF Cliente
                    </p>
                    <asp:TextBox ID="txtUfCliente" runat="server" Width="70px"></asp:TextBox>
                </td>
                <td colspan="2">
                    <p>
                        Cliente no estabelecimento
                    </p>
                    <asp:DropDownList ID="ddlindPres" runat="server">
                        <asp:ListItem Value="0" Selected="True">Não se aplica</asp:ListItem>
                        <asp:ListItem Value="1">Operação presencial;</asp:ListItem>
                        <asp:ListItem Value="2">Operação não presencial, pela Internet</asp:ListItem>
                        <asp:ListItem Value="3">Operação não presencial, Teleatendimento</asp:ListItem>
                        <asp:ListItem Value="4">NFC-e em operação com entrega a domicílio</asp:ListItem>
                        <asp:ListItem Value="5">Operação presencial, fora do estabelecimento</asp:ListItem>
                        <asp:ListItem Value="9">Operação não presencial, outros</asp:ListItem>
                    </asp:DropDownList>
                    <!-- 
                    0=Não se aplica (por exemplo, Nota Fiscal complementar
                //                 ou de ajuste);
                //                 1=Operação presencial;
                //                 2=Operação não presencial, pela Internet;
                //                 3=Operação não presencial, Teleatendimento;
                //                 4=NFC-e em operação com entrega a domicílio;
                //                 9=Operação não presencial, outros.
                    -->
                </td>
                <td>
                    <div class="panelItem">
                        <p>
                            Finalidade Emissao
                        </p>
                        <asp:DropDownList ID="ddlFinalidade" runat="server">
                            <asp:ListItem Value="1">Normal</asp:ListItem>
                            <asp:ListItem Value="2">Complementar</asp:ListItem>
                            <asp:ListItem Value="4">Devolução</asp:ListItem>
                            <asp:ListItem Value="5">Nota de Crédito</asp:ListItem>
                            <asp:ListItem Value="4">Nota de Débito</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Consumidor final
                        </p>
                        <asp:DropDownList ID="ddlindFinal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlindFinal_SelectedIndexChanged">
                            <asp:ListItem Value="0">Não</asp:ListItem>
                            <asp:ListItem Value="1">Sim</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="min-height: 400px">
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
                    <asp:Panel ID="AddNovoCupom" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="ImgAddNovoCupom" runat="server" ImageUrl="~/img/add.png" Height="20px"
                            Width="20px" OnClick="ImgAddNovoCupom_Click" />
                        Importar
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
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição">
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
                                <asp:BoundField DataField="CEST" HeaderText="CEST">
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Margem_iva" HeaderText="M.IVA%">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="iva" HeaderText="IVA">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aliquota_Iva" HeaderText="ICMS iva">
                                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="indice_St" HeaderText="CST ICMS">
                                    <ItemStyle Width="80px" />
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
                            <table style="width: 50%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkBoletoRecebido" runat="server" Text="Boleto Recebido" />
                                    </td>

                                    <td style="text-align: right">Pagamentos:
                                        <asp:Label ID="lblTotalPagamentos" runat="server" Text="0,00"></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="2">

                                        <h3 style="text-align: center">Forma de Pagamento</h3>
                                        <hr />
                                        <div style="display: flex; justify-content: center; flex-direction: row;">
                                            <asp:DropDownList ID="ddltPag" runat="server">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="01">Dinheiro</asp:ListItem>
                                                <asp:ListItem Value="02">Cheque</asp:ListItem>
                                                <asp:ListItem Value="03">Cartão de Crédito</asp:ListItem>
                                                <asp:ListItem Value="04">Cartão de Débito</asp:ListItem>
                                                <asp:ListItem Value="05">Crédito Loja</asp:ListItem>
                                                <asp:ListItem Value="10">Vale Alimentação</asp:ListItem>
                                                <asp:ListItem Value="11">Vale Refeição</asp:ListItem>
                                                <asp:ListItem Value="12">Vale Presente</asp:ListItem>
                                                <asp:ListItem Value="13">Vale Combustível</asp:ListItem>
                                                <asp:ListItem Value="14">Duplicata Mercantil</asp:ListItem>
                                                <asp:ListItem Value="15" Selected="true">Boleto Bancário</asp:ListItem>
                                                <asp:ListItem Value="16">Depósito Bancário</asp:ListItem>
                                                <asp:ListItem Value="17">Pagamento Instantâneo (PIX)</asp:ListItem>
                                                <asp:ListItem Value="18">Transferência bancária, Carteira Digital</asp:ListItem>
                                                <asp:ListItem Value="19">Programa de fidelidade, Cashback, Crédito Virtual</asp:ListItem>
                                                <asp:ListItem Value="90">Sem Pagamento</asp:ListItem>
                                                <asp:ListItem Value="99">Outros</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>

                            </table>
                            <div class="row">
                                <h3 style="text-align: center">Intermediador (MarketPlace)</h3>
                            </div>
                            <hr />
                            <div style="display: flex; justify-content: center; flex-direction: row; width: 80%; margin: auto">
                                <div style="flex: 2">
                                    <p>
                                        Operação 
                                    </p>
                                    <asp:DropDownList ID="ddlIntermediador" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlIntermediador_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Operação sem intermediador</asp:ListItem>
                                        <asp:ListItem Value="1">Operação em site ou plataforma de terceiros</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="flex: 1">
                                    <p>CNPJ do Intermediador</p>
                                    <asp:TextBox ID="txtIntermedCnpj" Style="width: 90%" runat="server" />
                                </div>
                                <div style="flex: 1">
                                    <p>Indentificador </p>
                                    <asp:TextBox ID="txtIdCadIntTran" Style="width: 90%" runat="server" />
                                </div>
                                <div style="flex: 1">
                                    <p>CNPJ Pagamento</p>
                                    <asp:TextBox ID="txtCnpjPagamento" Style="width: 90%" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                <HeaderTemplate>
                    Observação
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="gridnf" style="height: 390px;">
                        <table width="100%">
                            <tr>
                                <td colspan="4">
                                    <p>
                                        Observacao
                                    </p>
                                    <asp:TextBox ID="txtObservacao" runat="server" CssClass="sem" TextMode="MultiLine" Height="150px"
                                        Width="99%"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        Observações Padrão
                                    </p>
                                    <asp:ImageButton ID="imgBtnObservacoesPadrao" runat="server" ImageUrl="../imgs/Observacao.ico"
                                        Height="100px" Width="100px" OnClick="imgBtnObservacoesPadrao_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <p>
                                        Referencia
                                    </p>
                                    <asp:DropDownList ID="DDlRefECF" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDlRefECF_SelectedIndexChanged">
                                        <asp:ListItem>NFE</asp:ListItem>
                                        <asp:ListItem>ECF</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    <div id="DivReferenciaNota" runat="server">
                                        <p>
                                            Referencia de nota
                                        </p>
                                        <asp:TextBox ID="txtReferenciaNota" runat="server" Width="500px" MaxLength="50"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_txtReferenciaNota" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="imgLista_Click" />
                                        Adiciona Referencia!
                                        <asp:ImageButton ID="imgAddReferencia" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            OnClick="imgAddReferencia_Click" />
                                    </div>
                                    <div id="DivECF" runat="server">
                                        ECF:<asp:TextBox ID="txtECF" runat="server" Width="80px" MaxLength="3"></asp:TextBox>
                                        COO:<asp:TextBox ID="txtCOO" runat="server" Width="80px" MaxLength="6"></asp:TextBox>
                                        Data:<asp:TextBox ID="txtDataDocumento" runat="server" Width="80px" CssClass="data"></asp:TextBox>
                                        <asp:Image ID="imgDtDocumento" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgDtDocumento"
                                            TargetControlID="txtDataDocumento">
                                        </asp:CalendarExtender>
                                        Adiciona Referencia!
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            OnClick="imgAddReferencia_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gridNfReferencia" runat="server" ForeColor="#333333" GridLines="Vertical"
                                        OnRowCommand="gridNfReferencia_RowCommand">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Alterar"
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
                                </td>
                                <td colspan="2">
                                    <asp:GridView ID="GridPedidos" runat="server" ForeColor="#333333" GridLines="Vertical"
                                        AutoGenerateColumns="False" Width="50px">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Codigo_pedido" HeaderText="Pedidos Importados">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
                <HeaderTemplate>
                    Transportadora
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Transportadora
                                </p>
                                <asp:TextBox ID="txtTransportadora" runat="server" Width="300px" MaxLength="20"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtTransportadora" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </td>
                            <td>
                                <p>
                                    Qtde
                                </p>
                                <asp:TextBox ID="txtQuantidade" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Espécie
                                </p>
                                <asp:TextBox ID="txtEspecie" runat="server" Width="100px" MaxLength="30"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Marca
                                </p>
                                <asp:TextBox ID="txtMarca" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Número
                                </p>
                                <asp:TextBox ID="txtNumero" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Peso Bruto
                                </p>
                                <asp:TextBox ID="txtPesoBrutoTransporte" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Peso Liquido
                                </p>
                                <asp:TextBox ID="txtPesoLiquidoTransporte" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Tipo Frete
                                </p>
                                <asp:DropDownList ID="ddlTipoFrete" runat="server">
                                    <asp:ListItem Value="0">Por conta do emitente</asp:ListItem>
                                    <asp:ListItem Value="1">Por conta do destinatário/remetente</asp:ListItem>
                                    <asp:ListItem Value="2">Por conta de terceiros</asp:ListItem>
                                    <asp:ListItem Value="3">Próprio por conta do Remetente</asp:ListItem>
                                    <asp:ListItem Value="4">Próprio por conta do Destinatário</asp:ListItem>
                                    <asp:ListItem Value="9">Sem frete</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <p>
                                    Placa
                                </p>
                                <asp:TextBox ID="txtPlaca" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
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
                    <asp:TextBox ID="txtDesconto" runat="server" CssClass="numero" Width="100px"
                        AutoPostBack="true" OnTextChanged="txtDesconto_TextChanged"></asp:TextBox>
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
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none; height: 450px;" DefaultButton="btnConfirmaItens">
        <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Dados do Item</h1>
                <asp:Label ID="LblErroItem" runat="server" Text="" ForeColor="Red"></asp:Label>
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
                            <asp:TextBox ID="txtPLU" runat="server" Width="50px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtPLU" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                            <asp:Label ID="lblInativo" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div class="panelItem">

                            <p>
                                Cod Cliente
                            </p>
                            <asp:TextBox ID="txtCODIGO_REFERENCIA" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Descricao
                            </p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="440px" onclick="select()"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Und
                            </p>
                            <asp:TextBox ID="txtUnd" runat="server" Width="30px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Qtde
                            </p>
                            <asp:TextBox ID="txtQtde" runat="server" CssClass="numero" Width="75px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onclick="select()" onkeydown="autoTab(this,event)"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Embalagem
                            </p>
                            <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onclick="select()" onkeydown="autoTab(this,event)"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Preço
                            </p>
                            <asp:TextBox ID="txtUnitario" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                OnTextChanged="txt_TextChanged" onclick="select()" onkeydown="autoTab(this,event)"></asp:TextBox>
                            <asp:TextBox ID="txtUnitarioOriginal" runat="server" CssClass="numero" Visible="false" />
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
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Desconto Valor
                                </p>
                                <asp:TextBox ID="txtDescValorItem" runat="server" CssClass="numero" Width="100px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    IPI%
                                </p>
                                <asp:TextBox ID="txtIPI" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    IPI Valor
                                </p>
                                <asp:TextBox ID="txtIPIV" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>

                        </div>

                        <div class="panelItem" style="border-style: solid; border-width: 1px;">

                            <div class="panelItem">
                                <p>
                                    CFOP
                                </p>
                                <asp:TextBox ID="txtCodigo_operacao_item" runat="server" CssClass="numero" Width="50px" onclick="select()"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtCodigo_operacao_item" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    CEST
                                </p>
                                <asp:TextBox ID="txtCEST" runat="server" CssClass="numero" Width="80px" onclick="select()"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    NCM
                                </p>
                                <asp:TextBox ID="txtNCM" runat="server" CssClass="numero" Width="100px" onclick="select()"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtNCM" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px;">
                            <div class="panelItem">
                                <p>
                                    Item NF
                                </p>
                                <asp:TextBox ID="txtNum_item" runat="server" CssClass="numero" Width="40px"></asp:TextBox>
                            </div>

                            <div class="panelItem">
                                <p>
                                    Peso_liquido
                                </p>
                                <asp:TextBox ID="txtPeso_liquido" runat="server" CssClass="numero" Width="60px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Peso Bruto
                                </p>
                                <asp:TextBox ID="txtPeso_Bruto" runat="server" CssClass="numero" Width="60px"></asp:TextBox>
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
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtCodigo_Tributacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    ICMS%
                                </p>
                                <asp:TextBox ID="txtaliquota_icms" runat="server" CssClass="numero" Width="50px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    CST Icms
                                </p>
                                <asp:TextBox ID="txtindice_st" runat="server" Width="40px"></asp:TextBox>

                            </div>
                            <div class="panelItem">
                                <p>
                                    ICMS% D
                                </p>
                                <asp:TextBox ID="txtICMSDestino" runat="server" Width="40px"></asp:TextBox>

                            </div>
                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px; margin-left: 10px;">
                            <div class="panelItem">
                                <p>
                                    Redutor%
                                </p>
                                <asp:TextBox ID="txtredutor_base" runat="server" CssClass="numero" Width="70px"
                                    AutoPostBack="true" OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Margem IVA%
                                </p>
                                <asp:TextBox ID="txtmargem_iva" runat="server" CssClass="numero" Width="70px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Aliq IVA
                                </p>
                                <asp:TextBox ID="txtAliquota_iva" runat="server" CssClass="numero" Width="40px"></asp:TextBox>
                            </div>

                            <div class="panelItem">
                                <p>
                                    IVA
                                </p>
                                <asp:TextBox ID="txtiva" runat="server" CssClass="numero" Width="70px" AutoPostBack="true"
                                    OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                            </div>

                        </div>
                        <div class="panelItem" style="border-style: solid; border-width: 1px; margin-left: 10px;">


                            <div class="panelItem">
                                <p>
                                    PIS
                                </p>
                                <asp:TextBox ID="txtPisItem" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                                <asp:TextBox ID="txtVlrPisItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    COFINS
                                </p>
                                <asp:TextBox ID="txtCofinsItem" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
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
                        <div class="panelItem" style="width: 85%;">
                            <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        % Cred SN
                                    </p>
                                    <asp:TextBox ID="txtpCredSN" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Vlr Cred Icms SN
                                    </p>
                                    <asp:TextBox ID="txtvCredicmssn" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Outras Despesas
                                    </p>
                                    <asp:TextBox ID="txtdespesas" runat="server" CssClass="numero" Width="80px" AutoPostBack="true"
                                        OnTextChanged="txt_TextChanged" onkeydown="autoTab(this,event)"></asp:TextBox>
                                </div>
                            </div>

                            <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        Esc Relev
                                    </p>
                                    <asp:DropDownList ID="ddlEscalaRelevante" runat="server">
                                        <asp:ListItem Value="S">Sim</asp:ListItem>
                                        <asp:ListItem Value="N">Não</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        CNPJ Fab
                                    </p>
                                    <asp:TextBox ID="txtCNPJFab" runat="server" Width="100px" CssClass="CNPJ" MaxLength="18"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Cod Benef
                                    </p>
                                    <asp:TextBox ID="txt_cBenef" runat="server" CssClass="numero" Width="60px" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        Origem Mercadoria
                                    </p>
                                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="200px">
                                        <asp:ListItem Value="0">Nacional</asp:ListItem>
                                        <asp:ListItem Value="1">Estrangeira – Importação direta</asp:ListItem>
                                        <asp:ListItem Value="2">Estrangeira – Adquirida no mercado interno</asp:ListItem>
                                        <asp:ListItem Value="3">Nacional - Mercadoria/bem Imp sup 40%</asp:ListItem>
                                        <asp:ListItem Value="4">Nacional - Produção Decreto-Lei nº 288/67</asp:ListItem>
                                        <asp:ListItem Value="5">Nacional - Mercadoria/bem Imp inf ou igual 40%</asp:ListItem>
                                        <asp:ListItem Value="6">Estrangeira - Importação direta, sem similar nacional</asp:ListItem>
                                        <asp:ListItem Value="7">Estrangeira - Adquirida no mercado interno, sem similar nacional</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
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
                            <div class="panelItem" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        Numero do Pedido
                                    </p>
                                    <asp:TextBox ID="txtPedidoItemNumero" runat="server" Width="100px" MaxLength="15" CssClass="numero"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Seq Item Pedido
                                    </p>
                                    <asp:TextBox ID="txtPedidoItemSequencia" runat="server" Width="50px" MaxLength="3" CssClass="numero"></asp:TextBox>
                                </div>
                            </div>
                            <div class="panelItem" runat="server" id="divDevolucao" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        %Devolvida
                                    </p>
                                    <asp:TextBox ID="txtItem_pDevolv" runat="server" Width="50px" CssClass="numero"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        IPI Devolvido
                                    </p>
                                    <asp:TextBox ID="txtItem_vIPIDevol" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Frete
                                    </p>
                                    <asp:TextBox ID="txtFreteItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="panelItem" runat="server" id="divMed" style="border-style: solid; border-width: 1px; height: 50px;">
                                <div class="panelItem">
                                    <p>
                                        Cod.Prod. na ANVISA
                                    </p>
                                    <asp:TextBox ID="txtCodigoProdutoAnvisa" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Motivo da isenção na ANVISA
                                    </p>
                                    <asp:TextBox ID="txtMotivoIsencaoAnvisa" runat="server" Width="300px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        PMC ANVISA
                                    </p>
                                    <asp:TextBox ID="txtPrecoMaximoAnvisa" runat="server" CssClass="numero" Width="70px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtCodigoEmisaoNFe" runat="server" CssClass="numero" Width="70px" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="panelItem" style="float: right; margin-right: 10px;">
                            <p><span class="titulobtn">TOTAL </span></p>
                            <asp:TextBox ID="TxtTotalItem" runat="server" Font-Size="Medium" CssClass="numero" Width="100px"></asp:TextBox>
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
            <center><h1><asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                <asp:DropDownList ID="ddlTipoDestinatario" runat="server" AutoPostBack="true" OnTextChanged="ddlTipoDestinatario_TextChanged">
                    <asp:ListItem>CLIENTE</asp:ListItem>
                    <asp:ListItem>FORNECEDOR</asp:ListItem>
                </asp:DropDownList>
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="300px"></asp:TextBox>
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
                    <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
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
        DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnAddPagamento" runat="server" CssClass="modalForm" Style="display: none; height: 190px;">
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
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPagamentos" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroPg" DropShadow="true" PopupControlID="pnAddPagamento"
        TargetControlID="lblErroPg">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnImportar" runat="server" CssClass="modalForm" Style="display: none; overflow: auto; height: 340px;">
        <asp:Panel ID="PnImportarFrame" runat="server" CssClass="frame" DefaultButton="btnConfirmaImportar">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblImportacao" runat="server" Text="INFORMAR O NUMERO DO PEDIDO OU CUPOM OU CHAVE DE XML"></asp:Label>
                </h1>
                <asp:Label ID="lblErrorPedido" runat="server" Text="" ForeColor="Red"></asp:Label>
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
                            <asp:Label ID="lblNumeroPedido" runat="server" Text="Numero Pedido"></asp:Label>
                        </p>
                        <asp:TextBox ID="txtNumeroPedido" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <p>
                            Tipo Pedido
                        </p>
                        <asp:DropDownList ID="ddlTipoPedido" runat="server">
                            <asp:ListItem Value="1">VENDA</asp:ListItem>
                            <asp:ListItem Value="3">DEVOLUÇÃO CLIENTE</asp:ListItem>
                            <%--<asp:ListItem Value="4">DEVOLUCAO FORNECEDOR</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td>

                        <div id="div3" runat="server" class="panelItem">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/arquivo-download.png"
                                Width="25px" OnClick="imgBtnImportarVariosPedidos_Click" />
                            (+1 Pedido)
                        </div>
                        <div class="panelItem">
                            <asp:ImageButton ID="btnAbrirColetor" runat="server" ImageUrl="~/img/arquivo-download.png"
                                Width="25px" OnClick="btnAbrirColetor_Click" />
                            Coletor
                        </div>

                        <div id="divXmlDevolucaoImporta" runat="server" class="panelItem">
                            <asp:ImageButton ID="btnAbrirXmsDevolucao" runat="server" ImageUrl="~/img/arquivo-download.png"
                                Width="25px" OnClick="btnAbrirXmsDevolucao_Click" />
                            Devolução
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Numero CUPOM
                        </p>
                        <asp:TextBox ID="txtCupom" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Caixa
                        </p>
                        <asp:TextBox ID="txtCaixa" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Data
                        </p>
                        <asp:TextBox ID="txtDataCupom" runat="server" Width="100px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                        <asp:Image ID="ImgDataCupom" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImgDataCupom"
                            TargetControlID="txtDataCupom">
                        </asp:CalendarExtender>
                    </td>
                    <td>

                        <div id="div1" runat="server" class="panelItem">
                            <asp:ImageButton ID="imgBtnImportarVariosCupons" runat="server" ImageUrl="~/img/arquivo-download.png"
                                Width="25px" OnClick="imgBtnImportarVariosCupons_Click" />
                            (+1 cupom)
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <p>
                            CHAVE XML(PRODUTOR RURAL)
                        </p>
                        <asp:TextBox ID="txtChaveXml" runat="server" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p>
                            Outras Movimentações
                        </p>
                        <asp:TextBox ID="txtDescOutrasMovimentacoes" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            De
                        </p>
                        <asp:TextBox ID="txtDtDeMov" runat="server" Width="100px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                        <asp:Image ID="imgDtDeMov" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgDtDeMov"
                            TargetControlID="txtDtDeMov">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Ate
                        </p>
                        <asp:TextBox ID="txtDtAteMov" runat="server" Width="100px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                        <asp:Image ID="imgDtAteMov" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgDtAteMov"
                            TargetControlID="txtDtAteMov">
                        </asp:CalendarExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalImportar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnImportar" TargetControlID="lblErrorPedido">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnXml" runat="server" CssClass="modalForm" Style="display: none; height: 450px;">
        <asp:Label ID="lblErroXml" runat="server" Text="" ForeColor="Red"></asp:Label>
        <asp:Panel ID="pnXmlFrame" runat="server" CssClass="frame" DefaultButton="btnConfirmaImportar">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="cabMenu">
                        <h1>Validação e Transmissão de NFE</h1>
                    </div>
                    <table class="cabMenu">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnProximo" runat="server" ImageUrl="~/img/confirm.png" Width="25px"
                                    OnClick="btnXmlprocessar_Click" />
                                <asp:Label ID="lblProximoXml" runat="server" Text="Validar"></asp:Label>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnFechar" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                                    OnClick="btnCancelarXml_Click" />
                                <asp:Label ID="Label13" runat="server" Text="Fechar"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="5">
                                <p>
                                    Resultado
                                </p>
                                <asp:Label ID="lblResultadoXML" runat="server" Text="" Width="700px" Height="150px"
                                    CssClass="gridTable"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ImgBtnCancelarNota" runat="server" ImageUrl="~/img/cancel.png"
                                    Width="25px" OnClick="btnCancelarNota_Click" />
                                <asp:Label ID="lblCancelarNota" runat="server" Text="Cancelar Nota"></asp:Label>
                                <asp:ImageButton ID="ImgBtnCartaDeCorrecao" runat="server" ImageUrl="~/img/edit.png"
                                    Width="25px" OnClick="ImgBtnCartaDeCorrecao_Click" />
                                <asp:Label ID="lblCartaCorrecaoText" runat="server" Text="Carta de Correção"></asp:Label>
                            </td>
                            <td>
                                <div id="divSituacao" runat="server">
                                    <asp:ImageButton ID="imgBtnConsultaSituacao" runat="server" ImageUrl="../imgs/consultaSituacao1.png"
                                        Width="35px" OnClick="imgBtnConsultaSituacao_Click" />
                                    <asp:Label ID="Label22" runat="server" Text="Consulta situacao"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <div id="divVisualizaXML" runat="server">
                                    <asp:Button ID="btnVisualizarCorrecao" runat="server" Text="Imprimir Carta Correção" OnClick="btnCorrecao_Click" />
                                    <asp:Button ID="btnVisualizaXML" runat="server" Text="Visualiza Xml" OnClick="btnVisualizaXML_Click" />

                                </div>
                            </td>
                            <td>
                                <p>
                                    <asp:Label ID="lblJustificativa" runat="server" Text="Justificativa" Visible="false"></asp:Label>
                                </p>
                                <asp:TextBox ID="txtJustificativa" runat="server" Width="300px" Height="200px" Visible="false"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgBtnConfirmaCancelamento" runat="server" ImageUrl="~/img/confirm.png"
                                    Width="25px" OnClick="imgBtnConfirmaCancelamento_Click" Visible="false" />
                                <asp:Label ID="lblConfirmaCancelamento" runat="server" Text="Confirma Cancelamento de Nota"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="msgCorrecao" runat="server" visible="false" style="color: Green">
                                    A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15
                                    de dezembro de 1970 e pode ser utilizada para regularização de erro ocorrido na
                                    emissão de documento fiscal, desde que o erro não esteja relacionado com: I - as
                                    variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota,
                                    diferença de preço, quantidade, valor da operação ou da prestação; II - a correção
                                    de dados cadastrais que implique mudança do remetente ou do destinatário; III -
                                    a data de emissão ou de saída.”
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <p>
                                    <asp:Label ID="lblCorrecao" runat="server" Text="Correcao" Visible="false"></asp:Label>
                                </p>
                                <asp:TextBox ID="txtCorrecao" runat="server" Width="700px" Height="200px" Visible="false"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgBtnConfirmaCorrecao" runat="server" ImageUrl="~/img/confirm.png"
                                    Width="25px" OnClick="imgBtnConfirmaCorrecao_Click" Visible="false" />
                                <asp:Label ID="lblConfirmaCorrecao" runat="server" Text="Confirma Correção" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Timer ID="TimerXml" runat="server" OnTick="TimerXml_Tick" Enabled="false" Interval="30000">
                    </asp:Timer>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnProximo" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalXml" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnXml" TargetControlID="lblErroXml">
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
    
    <asp:Panel ID="pnConfirmaPg" runat="server" CssClass="frameModal" Style="display: none; height: 180px; width: 500px;">
        <b>
            <h2>
                <center>
                    O TOTAL DOS PAGAMENTOS É DIFERENTE DO TOTAL DA NOTA!
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
            <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaPgAdd" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaPg" TargetControlID="Label7">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnObservacoesPadrao" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnObservaoFrame" runat="server" CssClass="frame" DefaultButton="imgBtnConfirmaObservacoes">
            <div class="cabMenu">
                <h1>Observações Padrão</h1>
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
                                <asp:CheckBox ID="chkSelecionado" runat="server" />
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
    <asp:Panel ID="pnConfirmaPedidoFechado" runat="server" CssClass="frameModal" Style="display: none; height: 180px; width: 500px;">
        <b>
            <h2>
                <center>
                    O PEDIDO <asp:Label ID="lblConfirmaPedidoFechado" runat="server" Text="" ></asp:Label> ESTA COM O STATUS  DE FECHADO !
                    <br />
                    GOSTARIA DE IMPORTAR MESMO ASSIM?
                    </center>
            </h2>
        </b>
        <hr />
        <asp:Label ID="Label19" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgBtnConfirmaPedidoFechado" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="imgBtnConfirmaPedidoFechado_Click" />
            <asp:Label ID="Label20" runat="server" Text="Confirma"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgCancelaPedidoFechado" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="imgCancelaPedidoFechado_Click" />
            <asp:Label ID="Label21" runat="server" Text="Cancela"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaPedidoFechado" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaPedidoFechado" TargetControlID="lblConfirmaPedidoFechado">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnOutrasMovimentacoes" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px; height: 480px;">
            <center><h1>Escolha a Movimentação</h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtro:
                <asp:Label ID="lblFiltro" runat="server" Text=""></asp:Label>
            </div>
            <div style="border-style: solid; border-width: 1px; width: 75%; float: left; background: #FFFACD; margin-left: 20px; margin-top: 10px; margin-bottom: 10px; margin-right: 10px;">
                <div id="divMsgImpMovimento" runat="server">
                    <b>ATENÇÃO!</b> Só serão importadas as movimentações ENCERRADAS, caso não encontre
                    alguma movimentação verifique o <a href="../../../Modulos/estoque/pages/OutrasMovimentacoes.aspx"
                        target="_blank">status </a>
                </div>
                <asp:Label ID="lblErroMovimentacao" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-left: 5px; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaMov" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaMov_Click" />
                    <asp:Label ID="Label24" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaMov" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="btnCancelaMov_Click" />
                    <asp:Label ID="Label25" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel4" runat="server" CssClass="lista" Height="280px">
                <asp:GridView ID="gridMovimentacoes" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="false" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkMov" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItemMov" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Codigo_inventario" HeaderText="Cod"></asp:BoundField>
                        <asp:BoundField DataField="tipoMovimentacao" HeaderText="Tipo"></asp:BoundField>
                        <asp:BoundField DataField="Descricao_inventario" HeaderText="Descricao"></asp:BoundField>
                        <asp:BoundField DataField="data" HeaderText="Data"></asp:BoundField>
                        <asp:BoundField DataField="status" HeaderText="Status"></asp:BoundField>
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
    <asp:ModalPopupExtender ID="modalOutrasMovimentacoes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnOutrasMovimentacoes" TargetControlID="lblFiltro">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnColetor" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="Panel5" runat="server" CssClass="frame" DefaultButton="imgBuscaArquivo"
            Style="font-size: 20px; height: 450px;">
            <center><h1>Importar Arquivo Coletor</h1></center>
            <hr />
            <asp:Label ID="lblErroColetor" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div style="border-style: solid; border-width: 1px; width: 70%; float: left; background: #FFFACD; margin-left: 20px; margin-top: 30px;">
                <b>ATENÇÃO!</b> Para a utilização do coletor é necessário a configuração previa
                do arquivo  <a href="javascript:abrePopupCofigColetor();">Configurar Agora</a><br />
                Duvidas entrar em contato com a Bratter.
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: right; margin-top: -10px;">
                <%--<div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaColetor" runat="server" ImageUrl="~/img/confirm.png" Width="25px"
                        OnClick="btnConfirmaColetor_Click" />
                    <asp:Label ID="Label26" runat="server" Text="Confirma"></asp:Label>
                </div>
                --%>
                <div class="row" style="float: right;">
                    <asp:ImageButton ID="btnCancelaColetor" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaColetor_Click" />
                    <asp:Label ID="Label27" runat="server" Text="Cancela"></asp:Label>
                </div>
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
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalImportColetor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnColetor" TargetControlID="lblErroColetor">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnItemComplementar" runat="server" CssClass="modalForm" Style="display: none; height: 440px;">
        <asp:Panel ID="Panel7" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Dados do Item</h1>
                <asp:Label ID="lblErroComplementar" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaItensComplementar" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaItensComplementar_Click" />
                        <asp:Label ID="Label26" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaItemComplementar" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaItemComplementar_Click" />
                        <asp:Label ID="Label28" runat="server" Text="Cancela"></asp:Label>
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
                            <asp:TextBox ID="txtCompPlu" runat="server" Width="50px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCompPlu" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                            <asp:Label ID="Label29" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div class="panelItem">
                            <p>
                                Referencia
                            </p>
                            <asp:TextBox ID="txtCompReferencia" runat="server" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                NCM
                            </p>
                            <asp:TextBox ID="txtCompNCM" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCompNCM" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                        </div>
                        <div class="panelItem">
                            <p>
                                Descricao
                            </p>
                            <asp:TextBox ID="txtCompDescricao" runat="server" Width="450px"></asp:TextBox>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>

                        <div class="panelItem">
                            <p>
                                Base IPI
                            </p>
                            <asp:TextBox ID="txtCompBaseIpi" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>

                        <div class="panelItem">
                            <p>
                                IPI%
                            </p>
                            <asp:TextBox ID="txtCompIpi" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                IPI Valor
                            </p>
                            <asp:TextBox ID="txtCompVlrIp" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Base IVA
                            </p>
                            <asp:TextBox ID="txtCompBaseIva" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                ICMS% IVA
                            </p>
                            <asp:TextBox ID="txtCompIcmsIvaPorc" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>

                        <div class="panelItem">
                            <p>
                                Margem IVA%
                            </p>
                            <asp:TextBox ID="txtCompMargemIva" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                IVA
                            </p>
                            <asp:TextBox ID="txtCompIva" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>
                                Base ICMS
                            </p>
                            <asp:TextBox ID="txtCompBaseIcms" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                ICMS%
                            </p>
                            <asp:TextBox ID="txtCompIcmsPorc" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Valor ICMS
                            </p>
                            <asp:TextBox ID="txtCompValorIcms" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>

                        <div class="panelItem">
                            <p>
                                CST ICMS
                            </p>
                            <asp:TextBox ID="txtCompCSTIcms" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Redutor%
                            </p>
                            <asp:TextBox ID="txtCompRedutor" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Item NF
                            </p>
                            <asp:TextBox ID="txtCompItemNf" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                CFOP
                            </p>
                            <asp:TextBox ID="txtCompCfop" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCompCfop" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>

                        <div class="panelItem">
                            <p>
                                PIS
                            </p>
                            <asp:TextBox ID="txtCompPis" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                CST PIS
                            </p>
                            <asp:TextBox ID="txtCompCstPis" runat="server" Width="100px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCompCstPis" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                        </div>

                        <div class="panelItem">
                            <p>
                                COFINS
                            </p>
                            <asp:TextBox ID="txtCompCofins" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                CST COFINS
                            </p>
                            <asp:TextBox ID="txtCompCstCofins" runat="server" Width="100px"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCompCstCofins" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="imgLista_Click" />
                        </div>

                        <div class="panelItem">
                            <br />
                            <span class="titulobtn">TOTAL :</span>
                            <asp:TextBox ID="txtCompTotal" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>
                                % Cred SN
                            </p>
                            <asp:TextBox ID="txtCompCredSN" runat="server"
                                CssClass="numero" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Valor Cred Icms SN
                            </p>
                            <asp:TextBox ID="txtCompValorCredIcmsSN" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </div>


                        <div class="panelItem">
                            <p>
                                Origem Mercadoria
                            </p>
                            <asp:DropDownList ID="ddlCompOrigemMercadoria" runat="server">
                                <asp:ListItem Value="0">Nacional</asp:ListItem>
                                <asp:ListItem Value="1">Estrangeira – Importação direta</asp:ListItem>
                                <asp:ListItem Value="2">Estrangeira – Adquirida no mercado interno</asp:ListItem>
                                <asp:ListItem Value="3">Nacional - Mercadoria/bem Imp sup 40%</asp:ListItem>
                                <asp:ListItem Value="4">Nacional - Produção Decreto-Lei nº 288/67</asp:ListItem>
                                <asp:ListItem Value="5">Nacional - Mercadoria/bem Imp inf ou igual 40%</asp:ListItem>
                                <asp:ListItem Value="6">Estrangeira - Importação direta, sem similar nacional</asp:ListItem>
                                <asp:ListItem Value="7">Estrangeira - Adquirida no mercado interno, sem similar nacional</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnCompExcluirItem" runat="server" CssClass="frame" Visible="false">
            <asp:ImageButton ID="imgBtnExcluirComplemento" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="ImgExcluiItemComplemento_Click" />
            <asp:Label ID="Label30" runat="server" Text="Excluir Item"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalComplementar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnItemComplementar" TargetControlID="lblErroComplementar">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 150px; overflow: auto;">
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

    <asp:Panel ID="pnXmlImportaDevolucao" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="Panel8" runat="server" CssClass="frame" DefaultButton="imgBtnPesquisaImportaXmlDev"
            Style="font-size: 20px; height: 480px;">
            <center><h1>Informe a Chave para Importação</h1></center>
            <hr />
            <div class="row" style="margin-left: 20px;">
                Tipo  de Importação: 
                <asp:DropDownList ID="ddlTipoImportacaoDev" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoImportacaoDev_SelectedIndexChanged">
                    <asp:ListItem Text="CHAVE" />
                    <asp:ListItem Text="NOTA DE SAIDA" />
                </asp:DropDownList>
            </div>
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                <asp:Label ID="lblTipoImportacaoDev" Text="Chave" runat="server" />:
                <asp:TextBox ID="txtChaveDevImporta" runat="server" Width="500px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnPesquisaImportaXmlDev" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="imgBtnPesquisaImportaXmlDev_Click" />
            </div>

            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-left: 5px; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="imgBtnConfirmaXmlImportaDev" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaXmlImportaDev_Click" />
                    <asp:Label ID="Label32" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="imgBtnCancelaXmlImportaDev" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="imgBtnCancelaXmlImportaDev_Click" />
                    <asp:Label ID="Label33" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>

            <asp:Panel ID="Panel9" runat="server" CssClass="lista" Height="280px">
                <div id="divSemItens" runat="server" visible="false">
                    <h5>SEM ITENS</h5>
                </div>
                <asp:GridView ID="gridItensXmlImportaDev" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="false" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkTodosItensDev" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelTodosItensDev_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItemDev" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="plu" HeaderText="Plu"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                        <asp:BoundField DataField="qtde" HeaderText="Qtde"></asp:BoundField>
                        <asp:BoundField DataField="unitario" HeaderText="Valor"></asp:BoundField>
                        <asp:BoundField DataField="total" HeaderText="Total"></asp:BoundField>
                        <asp:BoundField DataField="item" HeaderText="Item"></asp:BoundField>
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
    <asp:ModalPopupExtender ID="modalXmlImportaDevolucao" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnXmlImportaDevolucao" TargetControlID="Label32">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnImportarVariosCupons" runat="server" CssClass="modalForm" Style="display: none; height: 480px;">
        <asp:Panel ID="Panel10" runat="server" CssClass="frame" DefaultButton="imgBtnPesquisaCupomVarios"
            Style="font-size: 20px; height: 450px;">
            <center><h1>Importação de cupons</h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                <div class="panelItem">
                    De:
                        
                <asp:TextBox ID="txtDataDeCupomVarios" runat="server" Width="80px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                    <asp:Image ID="imgDataDeCupomVarios" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="imgDataDeCupomVarios"
                        TargetControlID="txtDataDeCupomVarios">
                    </asp:CalendarExtender>
                    Ate:
                <asp:TextBox ID="txtDataAteCupomVarios" runat="server" Width="80px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                    <asp:Image ID="imgDataAteCupomVarios" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="imgDataAteCupomVarios"
                        TargetControlID="txtDataAteCupomVarios">
                    </asp:CalendarExtender>


                    <br />
                    Cliente:
                <asp:TextBox ID="txtFiltroCupomVarios" runat="server" Width="300px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnPesqClienteCupom" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgBtnPesqClienteCupomVarios_Click" />
                </div>
                <div class="panelItem" style="margin-left: 60px; margin-top: 10px;">
                    <asp:ImageButton ID="imgBtnPesquisaCupomVarios" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="25px" OnClick="imgBtnPesquisaCupomVarios_Click" />
                    Filtrar

                    <br />
                    <asp:Label ID="lblQtdeSelecionados" runat="server" Text=""></asp:Label>
                </div>
            </div>


            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-left: 5px; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="imgBtnConfirmaCupomVarios" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaCupomVarios_Click" />
                    <asp:Label ID="Label23" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="imgBtnCancelaCupomVarios_Click" />
                    <asp:Label ID="Label31" runat="server" Text="Cancela"></asp:Label>
                </div>

            </div>

            <asp:Panel ID="Panel11" runat="server" CssClass="lista" Height="280px">
                <div id="divSemCupons" runat="server" visible="false">
                    <h5>SEM ITENS</h5>
                </div>
                <asp:GridView ID="gridCuponsVarios" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="false" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkTodosItensCupom" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelTodosCupomVarios_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItemCupom" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelItemCupomVarios_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="cliente" HeaderText="Cliente"></asp:BoundField>
                        <asp:BoundField DataField="documento" HeaderText="Documento"></asp:BoundField>
                        <asp:BoundField DataField="Caixa" HeaderText="Caixa"></asp:BoundField>
                        <asp:BoundField DataField="dtMovimento" HeaderText="Data"></asp:BoundField>
                        <asp:BoundField DataField="valor" HeaderText="Valor">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCupomVarios" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnImportarVariosCupons" TargetControlID="Label23">
    </asp:ModalPopupExtender>



    <asp:Panel ID="pnImportarVariosPedidos" runat="server" CssClass="modalForm" Style="display: none; height: 480px;">
        <asp:Panel ID="Panel12" runat="server" CssClass="frame" DefaultButton="imgBtnPesquisaPedidoVarios"
            Style="font-size: 20px; height: 450px;">
            <center><h1>Importação de Pedidos</h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                <div class="panelItem">

                    Data:
                    <asp:DropDownList ID="DllDataPesquisa" runat="server" CssClass="sem">
                        <asp:ListItem Value="p.Data_Entrega">Entrega</asp:ListItem>
                        <asp:ListItem Value="p.Data_Cadastro">Cadastro</asp:ListItem>
                    </asp:DropDownList>


                    De:
                        
                <asp:TextBox ID="txtDataDePedidosVarios" runat="server" Width="80px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                    <asp:Image ID="imgDataDePedidosVarios" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender8" runat="server" PopupButtonID="imgDataDePedidosVarios"
                        TargetControlID="txtDataDePedidosVarios">
                    </asp:CalendarExtender>
                    Ate:
                <asp:TextBox ID="txtDataAtePedidosVarios" runat="server" Width="80px" CssClass="DATA" OnKeyPress="javascript:return formataData(this,event);"></asp:TextBox>
                    <asp:Image ID="imgDataAtePedidosVarios" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender9" runat="server" PopupButtonID="imgDataAtePedidosVarios"
                        TargetControlID="txtDataAtePedidosVarios">
                    </asp:CalendarExtender>


                    <br />
                    Cliente:
                <asp:TextBox ID="txtClienteVariosPedidos" runat="server" Width="300px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnPesqClientePedidosVarios" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgBtnPesqClientePedidosVarios_Click" />
                </div>
                <div class="panelItem" style="margin-left: 60px; margin-top: 10px;">
                    <asp:ImageButton ID="imgBtnPesquisaPedidoVarios" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="25px" OnClick="imgBtnPesquisaPedidoVarios_Click" />
                    Filtrar
                </div>
            </div>

            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-left: 5px; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="imgBtnConfirmaPedidosVarios" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaPedidosVarios_Click" />
                    <asp:Label ID="Label34" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="imgBtnCancelaPedidoVarios" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="imgBtnCancelaPedidoVarios_Click" />
                    <asp:Label ID="Label35" runat="server" Text="Cancela"></asp:Label>
                </div>

            </div>

            <asp:Panel ID="Panel13" runat="server" CssClass="lista" Height="280px">
                <div id="div2" runat="server" visible="false">
                    <h5>SEM ITENS</h5>
                </div>
                <asp:GridView ID="gridVariosPedidos" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="false" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkTodosItensPedido" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelTodosPedidosVarios_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItemPedido" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="cliente" HeaderText="Cliente"></asp:BoundField>
                        <asp:BoundField DataField="pedido" HeaderText="Pedido"></asp:BoundField>
                        <asp:BoundField DataField="data_cadastro" HeaderText="Data"></asp:BoundField>
                        <asp:BoundField DataField="total" HeaderText="Valor">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalImportarVariosPedidos" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnImportarVariosPedidos" TargetControlID="Label35">
    </asp:ModalPopupExtender>

</asp:Content>
