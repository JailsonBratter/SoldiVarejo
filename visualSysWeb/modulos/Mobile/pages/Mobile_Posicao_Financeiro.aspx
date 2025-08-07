<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile_Posicao_Financeiro.aspx.cs"
    Inherits="visualSysWeb.modulos.mobile.pages.Mobile_Posicao_Financeiro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>POSIÇÃO FINANCEIRO</title>
    <link href="~/Styles/mobile.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        a:link, a:visited
        {
            color: #034af3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="div">
        <table>
            <tr>
                <td class="style1">
                    <asp:Image ID="Image1" runat="server" Height="65px" ImageUrl="~/Img/Logo.jpg" Width="71px" />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Bratter - Automação Comercial" CssClass="label"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblData" runat="server" Text="Data:"></asp:Label>
                 </td>            
            </tr>
        </table>               
    </div>
    <div>
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            PageSize="20" CellPadding="6"
            ForeColor="#333333" GridLines="Vertical" 
            onrowdatabound="gridPesquisa_RowDataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Filial" HeaderText="Filial"/>
                <asp:BoundField DataField="CP" HeaderText="Pagar" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="CR" HeaderText="Receber" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="DN" HeaderText="Dinheiro" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"/>
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
