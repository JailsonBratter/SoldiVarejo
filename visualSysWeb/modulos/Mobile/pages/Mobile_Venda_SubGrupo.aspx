<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile_Venda_SubGrupo.aspx.cs" Inherits="visualSysWeb.modulos.mobile.pages.Mobile_Venda_SubGrupo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
                    <asp:Label ID="lblFilial" runat="server" Text="Filial:"></asp:Label>
                </td>
                <td>-</td>
                <td>
                    <asp:Label ID="lblData" runat="server" Text="Data:"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            PageSize="20" CellPadding="10" ForeColor="#333333" GridLines="Vertical" 
            onrowdatabound="gridPesquisa_RowDataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo_SubGrupo" HeaderText="SubGrupo" Text="SubGrupo" DataNavigateUrlFormatString="~/modulos/mobile/pages/Mobile_Venda_Departamento.aspx?codigo_SubGrupo={0}"
                    DataNavigateUrlFields="Codigo_SubGrupo" />
                <asp:HyperLinkField DataTextField="Descricao_SubGrupo" HeaderText="Descricao" Text="Descricao" DataNavigateUrlFormatString="~/modulos/mobile/pages/Mobile_Venda_Departamento.aspx?Codigo_SubGrupo={0}"
                    DataNavigateUrlFields="Codigo_SubGrupo" />
                <asp:BoundField DataField="Vlr" HeaderText="Vendas" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Qtde" Visible="false" DataFormatString="{0:0,0.000}" HeaderText="Qtde" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="Custo" DataFormatString="{0:n}" HeaderText="Custo" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="Lucro" DataFormatString="{0:n}" HeaderText="Lucro" ItemStyle-HorizontalAlign="Right"/>
                <asp:TemplateField HeaderText="Mrg" ItemStyle-HorizontalAlign="Right"></asp:TemplateField>
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
