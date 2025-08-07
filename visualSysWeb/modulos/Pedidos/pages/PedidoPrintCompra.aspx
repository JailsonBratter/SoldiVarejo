<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PedidoPrintCompra.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoPrintCompra" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IMPRESSÃO DE PEDIDO</title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
    <div id="divPage" runat="server" class="page" style="border-style:hidden;">
        <div class="cabMenu">
            <center>
                    <asp:Label ID="lblTituloPedido" runat="server" Text="Pedido" Font-Size="X-Large"></asp:Label>
            </center>
        </div>
        <div class="framePrint">
            <table  width="100%" style="width:20px;">
                <tr >
                    <td colspan="8">
                        <asp:Panel ID="pnSimples" runat="server">
                            <asp:Label ID="lblPedidoSimplificado" runat="server" Text="Pedido Simplificado" Font-Size="X-Large"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Pedido</p>
                        <asp:Label ID="lblPedido" runat="server" Text="" ></asp:Label>
                    </td>
                    <td>
                        <p>
                            Status</p>
                        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Vendedor</p>
                        <asp:Label ID="lblFuncionario" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Usuario</p>
                        <asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Data cadastro</p>
                        <asp:Label ID="lblDataCadastro" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Data entrega</p>
                        <asp:Label ID="lblDataEntrega" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Hora</p>
                        <asp:Label ID="lblHora" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Natureza Operacção </p>
                        <asp:Label ID="lblNaturezaOperacao" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <p>
                            <asp:Label ID="LbltituloCliente_Fornec" runat="server" Text="Nome Cliente"></asp:Label>
                        </p>
                        <asp:Label ID="lblCliente_Fornec" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblNomeClie_Fornec" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Tabela Desconto</p>
                        <asp:Label ID="lblTabelaDesconto" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Centro Custo</p>
                        <asp:Label ID="lblCentroCusto" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Desconto</p>
                        <asp:Label ID="lblDesconto" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <p>
                            Total</p>
                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="conteudo" runat="server" class="conteudo">
           <asp:Label ID="Label1" runat="server" Text="Itens" Font-Size="X-Large"> </asp:Label>
            <hr />
            <div class="gridTable">
                <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Both"
                    AutoGenerateColumns="False"  CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                        <asp:BoundField DataField="CodReferencia" HeaderText="REFERENCIA"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false"></asp:BoundField>
                        <asp:BoundField DataField="qtde" HeaderText="Qtde"></asp:BoundField>
                        <asp:BoundField DataField="Embalagem" HeaderText="Emb"></asp:BoundField>
                        <asp:BoundField DataField="TabDesc" HeaderText="Tab Desc"></asp:BoundField>
                        <asp:BoundField DataField="Desc" HeaderText="Desc%"></asp:BoundField>
                        <asp:BoundField DataField="Unitario" HeaderText="Preço"></asp:BoundField>
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
            </div>
            <hr />
             <asp:Label ID="Label2" runat="server" Text="Observações" Font-Size="X-Large"> </asp:Label>
          
            
            <hr />
            <asp:Label ID="lblObservacoes" runat="server" Text="" ></asp:Label>
            
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
                        <asp:BoundField DataField="Valor" HeaderText="VALOR"></asp:BoundField>
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
