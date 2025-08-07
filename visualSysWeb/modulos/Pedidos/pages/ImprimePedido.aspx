<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimePedido.aspx.cs" Inherits="visualSysWeb.pedidos.ImprimePedido" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/Styles/impressao.css" rel="stylesheet" type="text/css" />
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
              <h1> <asp:Label ID="lblTitulo" runat="server" ></asp:Label></h1>
<hr />
</div>
    
     <div id="cabecalho" runat="server" class="frame" > 
     <table >
        <tr>
            <td ><p>Pedido</p>
                <asp:Label ID="txtPedido" runat="server" ></asp:Label>
           </td>
           <td ><p>Filial</p>
                <asp:Label ID="txtFilial" runat="server" ></asp:Label>
           </td>
           <td ><p>Status</p>
                <asp:Label ID="txtStatus" runat="server"  CssClass="numero" ></asp:Label>
           </td>
           
            <td ><p>funcionario</p>
                <asp:Label ID="txtfuncionario" runat="server" ></asp:Label>
            </td>
           
        </tr>
        <tr>
            <td colspan=2><p>Fornecedor</p>
                 <asp:Label ID="txtCliente_Fornec" runat="server" ></asp:Label>
                    &nbsp;
                 <asp:Label ID="txtNome" runat="server" ></asp:Label>  
            </td>
           <td ><p>Data entrega</p>
                <asp:Label ID="txtData_entrega" runat="server" ></asp:Label>
           </td>

            <td valign=top ><p>Hora Fim</p>
                <asp:Label ID="txthora_fim" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan=2>
            <span id=nomeFantasia runat=server>
             <p>Nome Fantasia</p>
                <asp:Label ID="txtFantasia" runat="server" ></asp:Label>  
            </span>
            </td>
            <td colspan="2"><p>Cfop</p>
                <asp:Label ID="txtcfop" runat="server" ></asp:Label>
            </td>
        </tr>
     </table>
     </div>
     <h2>Itens</h2>
     <hr
             <asp:GridView ID="gridItens" runat="server" CellPadding="4" ForeColor="#333333" 
                        GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                        <h2>Obs</h2>
                        <hr />
                        <asp:Label ID="txtObs" runat="server"  ></asp:Label>
               

                    <h2>Pagamentos</h2>
                    <hr />
                       <asp:GridView ID="gridPagamentos" runat="server" CellPadding="4" ForeColor="#333333" 
                        GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
        <div id="rodape" runat="server" class="frame">

        <table>
            <td ><p>Cadastro</p>
                <asp:Label ID="txtData_cadastro" runat="server" ></asp:Label>
            </td>
            <td ><p>Usuario</p>
                <asp:Label ID="txtUsuario" runat="server" ></asp:Label>
            </td>
            <td ><p>Desconto</p>
                <asp:Label ID="txtDesconto" runat="server" ></asp:Label>
            </td>
            <td ><p>Total</p>
                <asp:Label ID="txtTotal" runat="server" ></asp:Label>
            </td>
        </table>
        </div>

    </div>
    </form>
</body>
</html>
