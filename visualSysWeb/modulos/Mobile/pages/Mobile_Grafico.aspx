<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile_Grafico.aspx.cs" Inherits="visualSysWeb.modulos.mobile.pages.Mobile_Grafico" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GRAFICO</title>
    <link href="~/Styles/mobile.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 604px;
        }
        .style2
        {
            width: 563px;
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

    <div >
        <table>
        <tr>
        <td>
        <asp:Chart ID="Grafico1" runat="server" onload="Chart1_Load" Width="480px">
            <series>
                <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1" 
                    XValueMember="Filial" YValueMembers="Vendas">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                    <Area3DStyle Enable3D="true" Inclination="20" />
                </asp:ChartArea>
            </chartareas>
            <Legends>
                <asp:Legend Name="Legend1">
                </asp:Legend>
            </Legends>
            <Titles>
                <asp:Title Name="Title1" Font="Microsoft Sans Serif, 8pt, style=Bold" 
                    Text="Vendas">
                </asp:Title>
            </Titles>
        </asp:Chart>
        </td>
        <td class="style2">
            <asp:Chart ID="Grafico02" runat="server" Width="542px">
                <series>
                    <asp:Series Name="Series1" XValueMember="Filial" YValueMembers="Vendas">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    <Area3DStyle Enable3D="true" Inclination="10" />
                    </asp:ChartArea>
                </chartareas>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 8pt, style=Bold" Name="Title1" 
                        Text="Vendas">
                    </asp:Title>
                </Titles>
            </asp:Chart>
        </td>
        </tr>
        </table>
    </div>
    <div >
        <table>
        <tr>
        <td class="style1">
            <asp:Chart ID="Grafico03" runat="server" Width="1025px">
                <series>
                    <asp:Series Name="CP" XValueMember="Filial" YValueMembers="CP" Legend="Legend1">
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Legend="Legend1" Name="CR" 
                        XValueMember="Filial" YValueMembers="CR">
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Color="0, 192, 0" Legend="Legend1" Name="DH" 
                        XValueMember="Filial" YValueMembers="DN">
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Color="Red" Legend="Legend1" Name="Saldo" 
                        XValueMember="Filial" YValueMembers="Saldo">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 8pt, style=Bold" Name="title01" 
                        Text="Posição Financeiro">
                    </asp:Title>
                </Titles>
            </asp:Chart>
        </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
