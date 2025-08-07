<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile_Default.aspx.cs" Inherits="visualSysWeb.modulos.mobile.mobile_default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/mobile.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="div">
        <table>
            <tr>
                <td class="style1">
                    <asp:Image ID="Image1" runat="server" Height="65px" ImageUrl="~/Img/Logo.jpg"
                        Width="71px" />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Bratter - Automação Comercial" CssClass="label" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
        <table>
            <tr>
                <td><center><p>P.Financeira</p></center><hr />
                    <asp:ImageButton ID="imgAtalho01" runat="server" Height="300px" Width="250px" 
                        ImageUrl="~/img/posicaofinanceira.jpg" onclick="imgAtalho01_Click" />
                </td>
                <td><center><p>Vendas</p></center><hr />
                    <asp:ImageButton ID="imgAtalho02" runat="server" Height="300px" Width="250px" 
                        ImageUrl="~/img/Vendas.jpg" onclick="imgAtalho02_Click" />
                </td>
                <td><center><p>Grafico Vdas/P.Fin</p></center><hr />
                    <asp:ImageButton ID="imgAtalho03" runat="server" Height="300px" Width="250px" 
                        ImageUrl="~/img/Grafico.jpg" onclick="imgAtalho03_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan=3>
                      
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx?desktop=true" >Versão Desktop</asp:HyperLink>
                </td>
            </tr>
        </table>
     <div>
    
    </div>
    </form>
</body>
</html>
