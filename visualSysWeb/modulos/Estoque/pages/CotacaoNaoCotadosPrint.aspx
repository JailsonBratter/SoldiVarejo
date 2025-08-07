<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CotacaoNaoCotadosPrint.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.CotacaoNaoCotadosPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../../../Styles/Site.css" rel="stylesheet" />
    <title>Cotação-Itens Não Cotados</title>
    <style>
        .panelItem{
            margin-left:10px;
            margin-right:20px;
        }

    </style>
</head>
<body onload="self.print();" >
    <form id="form1" runat="server" style="background-color:white;margin-bottom:15px;">
        <h1 style="text-align:center">Itens não Cotados</h1>
        <hr />
        <div style="padding:20px; font-size:19px; margin-left:12px;">
            <div class="panelItem">
                <b>Cotação:</b> <asp:Label ID="lblCotacao" Text="" runat="server" />
            </div>
            <div class="panelItem">
                <b>Data:</b> <asp:Label ID="lblData" Text="" runat="server" />
            </div> 
            <div class="panelItem">
                <b>Usuario: </b> <asp:Label ID="lblUsuario" Text="" runat="server" />
            </div> 
            

            <div class="panelItem">
                <b>Descrição:</b> <asp:Label ID="lblDescricao" Text="" runat="server" />
            </div> 
            
        </div>
        <div class="gridTable" style="padding:5px;">
                        <asp:GridView ID="gridItensNaoCotados" runat="server" 
                            AutoGenerateColumns="False" ForeColor="#333333"
                            GridLines="Both"
                            OnRowDataBound="gridItensNaoCotados_RowDataBound"
                            >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Mercadoria" HeaderText="Plu" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="Embalagem" HeaderText="Embalagem" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
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
    </form>
</body>
</html>
