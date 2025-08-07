<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPhone_Espelho.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.SmartPhone_Espelho" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        a:link {
            color: #034af3;
        }

        .container {
            width: 100%;
            ##height:100vh;
            background: white;
            display: table;
            ##flex-direction: column;
            justify-content: center;
            align-items: center;
            font-size: 30px;
            margin-left: 5px;
            margin-right: 5px;
        }

        .titulo {
            text-align: center;
            align-items: center;
            font-size: 70px;
        }

        .table {
            width: 100%;
            margin-bottom: .5em;
            table-layout: fixed;
        }

        .coluna {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div>
                <h1 class="titulo">
                    <asp:Label ID="lblTitulo" runat="server" Text="NOTAS FISCAIS"></asp:Label>
                </h1>
                <table class="table" style="width: 100%">
                    <tr class="coluna">
                        <td class="container">
                            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                PageSize="300" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Emissao" HeaderText="Emissão" ItemStyle-HorizontalAlign="left" DataFormatString="{0:d}">
                                        <HeaderStyle Width="15%" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:HyperLinkField DataTextField="Numero"  Text="-" Visible="true" HeaderText="Nro NFe" 
                                        DataNavigateUrlFormatString="~/modulos/Mobile/SmartPhone_NF_Detalhe.aspx?chave={0}"
                                        DataNavigateUrlFields="chave" />
                                    <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor" ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle Width="60%" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ValorNF" DataFormatString="{0:n2}" HeaderText="Vlr NFe" ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle Width="15%" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Chave" HeaderText="Chave" ItemStyle-HorizontalAlign="left" Visible="false">
                                        <HeaderStyle Width="15%" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
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
        </div>
    </form>
</body>
</html>
