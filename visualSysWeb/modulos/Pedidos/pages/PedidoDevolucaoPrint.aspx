<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PedidoDevolucaoPrint.aspx.cs"
    Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoDevolucaoPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
   
    <div id="divPage" runat="server">
     <h1>
        PEDIDO DE DEVOLUÇÃO</h1>
    <hr />
        <div style="width: 50%; float: left;">
            Pedido:<asp:Label ID="lblPedido" runat="server" Text="000"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Status:<asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Vendedor:<asp:Label ID="lblVendedor" runat="server" Text="Vendedor"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Data:<asp:Label ID="lblData" runat="server" Text="99/99/9999"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Data Entrega:<asp:Label ID="lblDataEntrega" runat="server" Text="99/99/9999"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Hora:<asp:Label ID="lblHora" runat="server" Text="99/99/9999"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Usuario:<asp:Label ID="lblUsuario" runat="server" Text="USUARIO"></asp:Label>
        </div>
        <div style="width: 50%; float: left;">
            Natureza Operacao:<asp:Label ID="lblNaturezaOp" runat="server" Text="0000"></asp:Label>
        </div>
        <div style="width: 100%; float: left; margin-top: 10px;">
            Cliente:<asp:Label ID="lblCodigoCliente" runat="server" Text="0000"></asp:Label>-
            <asp:Label ID="lblNomeCliente" runat="server" Text="NOME CLIENTE"></asp:Label>
        </div>
        <div style="width: 100%; float: left;">
            CPF/CNPJ:<asp:Label ID="lblCnpj" runat="server" Text="000.000.000-00"></asp:Label>
        </div>
        
        <hr />

        <div style="width: 50%; float: left;" ><h2>ITENS</h2></div>
         <div style="width: 50%; float: right; text-align:right;">
            <h3>Valor:<asp:Label ID="lblValor" runat="server" Text="999,99"></asp:Label></h3>
        </div>
        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" 
            AutoGenerateColumns="False"  Width="100%">
            <Columns>
                <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                <asp:BoundField DataField="CodReferencia" HeaderText="REF"></asp:BoundField>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                <asp:BoundField DataField="Documento" HeaderText="Cupom"></asp:BoundField>
                <asp:BoundField DataField="caixa_documento" HeaderText="Pdv"></asp:BoundField>
                <asp:BoundField DataField="data_Documento" HeaderText="Data"></asp:BoundField>
                <asp:BoundField DataField="qtde" HeaderText="Qtde">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Embalagem" HeaderText="Emb">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Unitario" HeaderText="Preço">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Total" HeaderText="Total">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                
            </Columns>
        </asp:GridView>
        <div id="divObs" runat="server">
            <hr />
            <h2>
                OBSERVAÇÕES</h2>
            <div style="border-style:solid; border-width:1px;"> 
            <asp:Label ID="lblObservacao" runat="server" Text="Observacao"></asp:Label>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
