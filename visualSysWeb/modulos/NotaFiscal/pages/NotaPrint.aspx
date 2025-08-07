<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotaPrint.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NotaPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Impressao Nota</title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
    <div class="page">
        <div class="cabMenu">
            <center>
                <h1>
                    Detalhes da Nota
                </h1>
            </center>
        </div>
        <asp:Panel ID="cabecalho" runat="server" CssClass="frame">
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo</p>
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px" ></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            CNPJ</p>
                        <asp:TextBox ID="txtFornecedor_CNPJ" runat="server" ></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Emissao</p>
                        <asp:TextBox ID="txtEmissao" runat="server" Width="80px"></asp:TextBox>
                       
                    </td>
                    <td>
                        <p>
                            Data</p>
                        <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                       
                    </td>
                    <td>
                        <asp:CheckBox ID="chknf_Canc" runat="server" Text="Cancelada" />
                    </td>
                   
                </tr>
                <tr>
                    <td colspan="3">
                        <p>
                            Fornecedor</p>
                        <asp:TextBox ID="TxtNomeFornecedor" runat="server" Width="300px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Pedido</p>
                        <asp:TextBox ID="txtPedido" runat="server" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Centro Custo</p>
                        <asp:TextBox ID="txtcentro_custo" runat="server" Width="80px"></asp:TextBox>
                      </td>
                    
                </tr>
                <tr>
                <td colspan="2">
                        <p>
                            Natureza Operacao</p>
                        <asp:TextBox ID="txtCodigo_operacao" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                     </td>
                      <td colspan="4">
                        <p>
                            Usuario</p>
                        <asp:TextBox ID="txtusuario" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <td colspan="6">
                        Chave
                        <asp:TextBox ID="txtid" runat="server" Width="500px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
            <hr />
            <h3>
                ITENS
            </h3>
            <hr />
            <div>
                <asp:GridView ID="gridItens" runat="server" ForeColor=" #333333" GridLines="Vertical"
                    AutoGenerateColumns="False" 
                    CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
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
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Embalagem" HeaderText="Emb">
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unitario" HeaderText="Preço">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Codigo_Tributacao" HeaderText="Trib">
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Desconto" HeaderText="Desc%">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Total" HeaderText="Total">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ipi" HeaderText="IPI">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ipiv" HeaderText="Ipiv">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="num_item" HeaderText="Item NF">
                            <ItemStyle Width="80px" />
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
            <hr />
            
            <h3>
                Pagamentos
            </h3>
            <hr />
            <div>
                <asp:GridView ID="gridPagamentos" runat="server" ForeColor="#333333" GridLines="Vertical"
                    AutoGenerateColumns="False"  >
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
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
                <hr />
                <h3>
                    Observação
                </h3>
                <hr />
                <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Height="150px"
                    Width="99%"></asp:TextBox>
            </div>
            <div id="rodape" runat="server" class="frame">
                <table>
                    <tr>
                        <td>
                            <p>
                                Frete</p>
                            <asp:TextBox ID="txtFrete" runat="server" CssClass="numero" Width="80px" 
                                ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Seguro</p>
                            <asp:TextBox ID="txtSeguro" runat="server" CssClass="numero" Width="80px" 
                                ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                IPI</p>
                            <asp:TextBox ID="txtIPI_Nota" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                BC ICMS</p>
                            <asp:TextBox ID="txtBase_Calculo" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                BC Subs</p>
                            <asp:TextBox ID="txtBase_Calc_Subst" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <p>
                                Desc Rateio</p>
                            <asp:TextBox ID="txtDesconto" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Despesas</p>
                            <asp:TextBox ID="txtDespesas_financeiras" runat="server" CssClass="numero" Width="80px"
                                ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Outras</p>
                            <asp:TextBox ID="txtOutras" runat="server" CssClass="numero" Width="80px" 
                                ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Desconto</p>
                            <asp:TextBox ID="txtDesconto_geral" runat="server" CssClass="numero" Width="80px"
                                ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Valor ICMS
                            </p>
                            <asp:TextBox ID="txtICMS_Nota" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Vlr ICMS Subs</p>
                            <asp:TextBox ID="txtICMS_ST" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Total Produtos</p>
                            <asp:TextBox ID="txtTotalProdutos" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Total Nota</p>
                            <asp:TextBox ID="txtTotal" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        </div>
    </form>
</body>
</html>
