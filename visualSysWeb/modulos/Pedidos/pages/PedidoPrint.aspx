<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PedidoPrint.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IMPRESSÃO DE PEDIDO</title>
    <link href="../../../Styles/Site2.css" rel="stylesheet" />
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
        <div id="divPage" runat="server" class="page" style="border-style: hidden;">
            <div class="cabMenu">
                <center>
                    <asp:Label ID="lblTituloPedido" runat="server" Text="Pedido" Font-Size="X-Large"></asp:Label>
                </center>
            </div>

            <div class="framePrint">
                <div id="divFilial" runat="server">
                    <div class="row">
                        <div class="panelItem">
                            <p>CNPJ</p>
                            <asp:Label ID="lblFilialCnpj" runat="server"></asp:Label>
                        </div>
                        <div class="panelItem" style="margin-left:30%">
                            <p>Razao Social</p>
                            <asp:Label ID="lblFilialRazaoSocial" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="panelItem">
                            <p>Endereço</p>
                            <asp:Label ID="lblFilialEndereco" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <table width="100%">
                    <tr>
                        <td colspan="8">
                            <asp:Panel ID="pnSimples" runat="server">
                                <asp:Label ID="lblPedidoSimplificado" runat="server" Text="Pedido Simplificado" Font-Size="X-Large"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!--
                        <p>
                            Pedido</p> -->
                            Pedido:<br />
                            <asp:Label ID="lblPedido" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Status</p> -->
                            Status:<br />
                            <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Vendedor</p> -->
                            Vendedor:<br />
                            <asp:Label ID="lblFuncionario" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Usuario</p> -->
                            Usuário:<br />
                            <asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Cadastro</p> -->
                            Data Cadastro:<br />
                            <asp:Label ID="lblDataCadastro" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                           Entrega</p> -->
                            Data Entrega:<br />
                            <asp:Label ID="lblDataEntrega" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Hora</p> -->
                            Hora:<br />
                            <asp:Label ID="lblHora" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <!--
                        <p>
                            Nat Operação </p> -->
                            Nat. Operação:
                            <br />
                            <asp:Label ID="lblNaturezaOperacao" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <!-- 
                        <p>
                            <asp:Label ID="LbltituloCliente_Fornec" runat="server" Text="Nome Cliente"></asp:Label>
                        </p> -->
                            Nome Cliente:<br />
                            <asp:Label ID="lblCliente_Fornec" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblNomeClie_Fornec" runat="server" Text=""></asp:Label><br />
                            <asp:Label ID="lblEndereco_Completo" runat="server" Text=""></asp:Label>
                        </td>
                        <!--
                    <td>
                        <p>
                            Tab Desc</p>
                        <asp:Label ID="lblTabelaDesconto" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Cent Custo</p>
                        <asp:Label ID="lblCentroCusto" runat="server" Text=""></asp:Label>
                    </td> -->

                    </tr>
                    <tr>
                        <td colspan="5">
                            <!-- <p>Tipo</p> -->
                            <br />
                            <asp:Label ID="lblTipoPedido" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <!--
                        <p>
                            Desconto</p> -->
                            Desconto:<br />
                            <asp:Label ID="lblDesconto" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                        <td colspan="2" style="text-align: right">
                            <!--
                        <p>
                            Total</p> -->
                            Total:<br />
                            <asp:Label ID="lblTotal" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="conteudo" runat="server" class="conteudo">
                <br />
                <asp:Label ID="Label1" runat="server" Text="Itens" Font-Size="X-Large"> </asp:Label>
                <hr />
                <div class="gridTable">
                    <asp:GridView ID="gridItens" runat="server" ForeColor="#333333"
                        AutoGenerateColumns="False" CssClass="table" Font-Size="Small" Width="95%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                            <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                            <asp:BoundField DataField="CodReferencia" HeaderText="REFERENCIA" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="qtde" HeaderText="Qtde" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Embalagem" HeaderText="Emb" ItemStyle-HorizontalAlign="Right" Visible="true">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                           <%-- <asp:BoundField DataField="TabDesc" HeaderText="Tab Desc" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="Desc" HeaderText="Desc%" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="UnitarioDesconto" HeaderText="Preço" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="obs" HeaderText="obs" Visible="False"></asp:BoundField>

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
                <asp:Label ID="Label2" runat="server" Text="Observações" Font-Size="X-Large"> </asp:Label>


                <hr />
                <asp:Label ID="lblObservacoes" runat="server" Text=""></asp:Label>
                <hr />
                <asp:Label ID="lblOrcamentoObservacoes" runat="server" Text=""></asp:Label>
                <hr />
                <asp:Label ID="Label3" runat="server" Text="Pagamentos" Font-Size="X-Large"> </asp:Label>


                <hr />
                <div class="gridTable">
                    <asp:GridView ID="gridPagamentos" runat="server" ForeColor="#333333" GridLines="Both"
                        AutoGenerateColumns="False"
                        CssClass="table">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Tipo_pagamento" HeaderText="TIPO PAGAMENTO"></asp:BoundField>
                            <asp:BoundField DataField="Vencimento" HeaderText="VENCIMENTO"></asp:BoundField>
                            <asp:BoundField DataField="Valor" HeaderText="VALOR" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
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
    </form>
</body>


</html>
