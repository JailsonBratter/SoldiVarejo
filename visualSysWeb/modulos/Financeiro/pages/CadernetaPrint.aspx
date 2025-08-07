<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadernetaPrint.aspx.cs"
    Inherits="visualSysWeb.modulos.Financeiro.pages.CadernetaPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IMPRESSÃO CADERNETA</title>
    <style type="text/css">
        .gridTable
        {
           
            margin-top: 30px;
            margin-bottom: 30px;
            width: 100%;
            color: Black;
            overflow: auto;
        }
        .gridTable table
        {
            margin: auto;
            text-align: left;
            width: 95%;
           
        }
        .direita
        {
            margin: auto;
            text-align: right;
        }
    </style>
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
    <div>
        <div class="cabMenu">
            <center>
                <asp:Label ID="lblTitulo" runat="server" Text="Titulos Pagos" Font-Size="X-Large"></asp:Label>
                <br />
                <hr />
            </center>
        </div>
    </div>
    <div>
        <b>DE:</b><asp:Label ID="lblDe" runat="server" Text=""></asp:Label>
        <b>&nbsp;&nbsp;&nbsp; ATÉ:</b><asp:Label ID="lblAte" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp; Tipo:<asp:Label ID="lblTipo" runat="server" Text=""></asp:Label>
        <div class="direita">
            SALDO DEVEDOR:<asp:Label ID="lblSaldoDevedor" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <hr />
        <b>Cliente:</b>
        <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridItens" runat="server" AutoGenerateColumns="False" CssClass="table"
            >
            <Columns>
                <asp:BoundField DataField="Tipo" HeaderText="TIPO"></asp:BoundField>
                <asp:BoundField DataField="Documento" HeaderText="DOCUMENTO"></asp:BoundField>
                <asp:BoundField DataField="Emissao" HeaderText="EMISSAO"></asp:BoundField>
                <asp:BoundField DataField="Valor" HeaderText="VALOR">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        ____________________________________&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;____________________________________
        <br />
        Ass. Cliente&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Ass. </b><asp:Label ID="lblFantasia" runat="server" Text=""></asp:Label>
    </div>
    <br />
    <span style="font-size:20px;">
    <b>TOTAL DEBITO :&nbsp;&nbsp;&nbsp;</b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>

    </span>
    </form>
</body>
</html>
