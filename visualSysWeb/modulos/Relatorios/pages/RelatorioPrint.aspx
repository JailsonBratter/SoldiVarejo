<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioPrint.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelatorioPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Impressao relatorio</title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
</head>

<body onload="self.print();">
    <form id="form1" runat="server" class="page">
    <div>
    
    <asp:Panel ID="pnRelatorio" runat="server"   ScrollBars="Auto">
       <div class ="cabecalhoRelatorio" >
            <asp:Image ID="imgLog" runat="server" CssClass="titleimg" />
            <asp:Label ID="lblCabecalho" runat="server" Text="Cabeçalho"></asp:Label>
        </div>
     <asp:Panel ID="Panel3" runat="server"  >
         <asp:Label ID="Label1" runat="server" Text="Ordem:"></asp:Label><asp:Label ID="lblordem" runat="server" Text=""></asp:Label>
         <asp:Label ID="Label2" runat="server" Text="Filtros:"></asp:Label><asp:Label ID="lblfiltros" runat="server" Text=""></asp:Label>
        <asp:GridView ID="Gridrelatorio" runat="server" CellPadding="4" ForeColor="Black"
                GridLines="Vertical" AllowSorting="True" Width="100%" BorderStyle="Solid" OnPageIndexChanging="Gridrelatoiro_PageIndexChanging"
                OnSorting="GridView1_Sorting" BorderWidth="1px" OnDataBound="Gridrelatorio_DataBound"
                OnRowCreated="Gridrelatorio_RowCreated" OnRowDataBound="Gridrelatorio_RowDataBound"
                BorderColor="Black" BackColor="black" ShowFooter="True">
                <AlternatingRowStyle BackColor="#D3D3D3" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Size="Medium" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" BorderColor="Black"
                    BorderStyle="Solid" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </asp:Panel>
        
        <div class="rodapeRelatorio">
         <asp:Label ID="LblRodape" runat="server" Text="rodape" ></asp:Label>
        </div>
    </asp:Panel>

    </div>
    </form>
</body>
</html>
