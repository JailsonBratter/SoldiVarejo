<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfPedidoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfPedidoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/NfPedidosDetalhes.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Emissão de Pedidos
            </h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <asp:Button ID="btnXml" runat="server" Text="AUTORIZAR" OnClick="btnXml_Click" />
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
                        UF Cliente
                    </p>
                    <asp:TextBox ID="txtUfCliente" runat="server" Width="70px"></asp:TextBox>
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
                <div id="divNt" runat="server" visible="false">
                    <td>

                        <p>
                            Natureza Operacao
                        </p>
                        <asp:TextBox ID="txtCodigo_operacao" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        <%--<asp:ImageButton ID="btnimg_txtCodigo_operacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />--%>
                        
                    </td>
                </div>
            </tr>
            <tr>
                <td colspan="2">
                    <p>
                        Entrega
                    </p>
                    <asp:TextBox ID="txtEntrega" runat="server" Width="200px"></asp:TextBox>
                </td>

                <div id="div1" runat="server" visible="false">
                    <td colspan="2">
                        <p>
                            Cliente no estabelecimento
                        </p>
                        <asp:DropDownList ID="ddlindPres" runat="server">
                            <asp:ListItem Value="0">Não se aplica</asp:ListItem>
                            <asp:ListItem Value="1">Operação presencial;</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">Operação não presencial, pela Internet</asp:ListItem>
                            <asp:ListItem Value="3">Operação não presencial, Teleatendimento</asp:ListItem>
                            <asp:ListItem Value="4">NFC-e em operação com entrega a domicílio</asp:ListItem>
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
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <p>
                                Consumidor final
                            </p>
                            <asp:DropDownList ID="ddlindFinal" runat="server">
                                <asp:ListItem Value="0">Não</asp:ListItem>
                                <asp:ListItem Value="1">Sim</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </div>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
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
                        OnTextChanged="txtCalcula_TextChanged"></asp:TextBox>
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
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none; height: 440px;">
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
                        <p>
                            PLU
                        </p>
                        <asp:TextBox ID="txtPLU" runat="server" Width="50px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtPLU" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                        <asp:Label ID="lblInativo" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <p>
                            Cod Cliente
                        </p>
                        <asp:TextBox ID="txtCODIGO_REFERENCIA" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <p>
                            Descricao
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px" MaxLength="60"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Qtde
                        </p>
                        <asp:TextBox ID="txtQtde" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Embalagem
                        </p>
                        <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="50px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Preço
                        </p>
                        <asp:TextBox ID="txtUnitario" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Tributacao
                        </p>
                        <asp:TextBox ID="txtCodigo_Tributacao" runat="server" CssClass="numero" Width="100px"
                            AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtCodigo_Tributacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <p>
                            Desconto %
                        </p>
                        <asp:TextBox ID="txtDescontoItem" runat="server" CssClass="numero" Width="100px"
                            AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Despesas
                        </p>
                        <asp:TextBox ID="txtdespesas" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            IPI%
                        </p>
                        <asp:TextBox ID="txtIPI" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            IPI Valor
                        </p>
                        <asp:TextBox ID="txtIPIV" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Margem IVA%
                        </p>
                        <asp:TextBox ID="txtmargem_iva" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            IVA
                        </p>
                        <asp:TextBox ID="txtiva" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            ICMS%
                        </p>
                        <asp:TextBox ID="txtaliquota_icms" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Redutor%
                        </p>
                        <asp:TextBox ID="txtredutor_base" runat="server" CssClass="numero" Width="100px"
                            AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            PIS
                        </p>
                        <asp:TextBox ID="txtPisItem" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            COFINS
                        </p>
                        <asp:TextBox ID="txtCofinsItem" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Item NF
                        </p>
                        <asp:TextBox ID="txtNum_item" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            CFOP
                        </p>
                        <asp:TextBox ID="txtCodigo_operacao_item" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtCodigo_operacao_item" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <p>
                            NCM
                        </p>
                        <asp:TextBox ID="txtNCM" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtNCM" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Und
                        </p>
                        <asp:TextBox ID="txtUnd" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Peso_liquido
                        </p>
                        <asp:TextBox ID="txtPeso_liquido" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Peso_Bruto
                        </p>
                        <asp:TextBox ID="txtPeso_Bruto" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            CST PIS
                        </p>
                        <asp:TextBox ID="txtCSTPIS" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtCSTPIS" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <p>
                            CST COFINS
                        </p>
                        <asp:TextBox ID="txtCSTCOFINS" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtCSTCOFINS" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <p>
                            CEST
                        </p>
                        <asp:TextBox ID="txtCEST" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="btnimg_txtCEST" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgLista_Click" />
                    </td>
                    <td>
                        <br />
                        <br />
                        <span class="titulobtn">TOTAL :</span>
                        <asp:TextBox ID="TxtTotalItem" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            % Cred SN
                        </p>
                        <asp:TextBox ID="txtpCredSN" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                            CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Valor Cred Icms SN
                        </p>
                        <asp:TextBox ID="txtvCredicmssn" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            ICMS% IVA
                        </p>
                        <asp:TextBox ID="txtAliquota_iva" runat="server" CssClass="numero" Width="100px"
                            AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            CST ICMS
                        </p>
                        <asp:TextBox ID="txtindice_st" runat="server" CssClass="numero" Width="100px" AutoPostBack="true"
                            OnTextChanged="txt_TextChanged"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <p>
                            Origem Mercadoria
                        </p>
                        <asp:DropDownList ID="ddlOrigem" runat="server">
                            <asp:ListItem Value="0">Nacional</asp:ListItem>
                            <asp:ListItem Value="1">Estrangeira – Importação direta</asp:ListItem>
                            <asp:ListItem Value="2">Estrangeira – Adquirida no mercado interno</asp:ListItem>
                            <asp:ListItem Value="3">Nacional - Mercadoria/bem Imp sup 40%</asp:ListItem>
                            <asp:ListItem Value="4">Nacional - Produção Decreto-Lei nº 288/67</asp:ListItem>
                            <asp:ListItem Value="5">Nacional - Mercadoria/bem Imp inf ou igual 40%</asp:ListItem>
                            <asp:ListItem Value="6">Estrangeira - Importação direta, sem similar nacional</asp:ListItem>
                            <asp:ListItem Value="7">Estrangeira - Adquirida no mercado interno, sem similar nacional</asp:ListItem>
                        </asp:DropDownList>
                    </td>
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
    <asp:Panel ID="PnImportar" runat="server" CssClass="modalForm" Style="display: none; height: 600px">
        <asp:Panel ID="PnImportarFrame" runat="server" CssClass="frame" DefaultButton="imgPesquisaImporta">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblImportacao" runat="server" Text="INFORMAR O NUMERO OS PEDIDOS PARA IMPORTAÇÃO"></asp:Label>
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
                        <div class="panelItem">
                            <p>
                                Data de
                            </p>
                            <asp:TextBox ID="txtDtDeImp" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgDtDeImp" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtDeImp"
                                TargetControlID="txtDtDeImp">
                            </asp:CalendarExtender>
                        </div>
                        <div class="panelItem">
                            <p>
                                Ate
                            </p>
                            <asp:TextBox ID="txtDtAteImp" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgDtAteImp" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgDtAteImp"
                                TargetControlID="txtDtAteImp">
                            </asp:CalendarExtender>
                        </div>


                        <div class="panelItem">
                            <p>
                                <asp:Label ID="lblNumeroPedido" runat="server" Text="Numero Pedido"></asp:Label>
                            </p>
                            <asp:TextBox ID="txtNumeroPedido" runat="server" Width="100px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Cod / Nome / CNPJ Cliente</p>
                            <asp:TextBox ID="txtImportaCliente" runat="server" Width="400px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <br />
                            <asp:ImageButton ID="imgPesquisaImporta" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="15px" OnClick="imgPesquisaImporta_Click" />
                            Pesquisar
                        </div>
                    </td>

                </tr>
                <tr>
                    <td>
                        <div class="btnImprimirDireita">
                            <h1>Valor Selecionado
                            <asp:Label ID="lblValorSelecionado" runat="server" Text="0,00"></asp:Label>
                            </h1>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" CssClass="lista" Style="width: 100%;">
                            <asp:GridView ID="gridPedidosImportar" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="false" GridLines="None" Width="100%">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkTodos" runat="server" onclick="javascript:selecionarPedidos(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkItem" runat="server" onclick="javascript:vlrTotalSelecionado();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Pedido" HeaderText="Pedido"></asp:BoundField>
                                    <asp:BoundField DataField="data" HeaderText="Data"></asp:BoundField>
                                    <asp:BoundField DataField="Cliente" HeaderText="Cliente"></asp:BoundField>
                                    <asp:BoundField DataField="CNPJ" HeaderText="CNPJ"></asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
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

                    </td>

                </tr>

            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalImportar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnImportar" TargetControlID="lblErrorPedido">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnXml" runat="server" CssClass="frameModal" Style="display: none; height: 150px;">
        <asp:Label ID="lblErroXml" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <div class="cabMenu">
                        <h1>Confirmação de Autorização de Nota de Pedidos</h1>
                    </div>
                    <table class="frame">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnProximo" runat="server" ImageUrl="~/img/confirm.png" Width="25px"
                                    OnClick="btnXmlprocessar_Click" />
                                <asp:Label ID="lblProximoXml" runat="server" Text="Confirmar"></asp:Label>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnFechar" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                                    OnClick="btnCancelarXml_Click" />
                                <asp:Label ID="Label13" runat="server" Text="Fechar"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    
                
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
                    alguma movimentação verifique o <a href="../../../Modulos/Manutencao/pages/OutrasMovimentacoes.aspx"
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
                                <asp:CheckBox ID="chkMov" runat="server" AutoPostBack="True" />
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
                do arquivo Web.config<br />
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
        <div class="cabMenu" style="height: 200px; overflow: auto;">
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
</asp:Content>
