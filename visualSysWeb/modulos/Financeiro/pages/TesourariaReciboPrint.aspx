<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TesourariaReciboPrint.aspx.cs"
    Inherits="visualSysWeb.modulos.Financeiro.pages.TesourariaReciboPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
    <div style="width: 100%">
        <div style="width: 40%; float: left;">
            Id Movimento:<asp:Label ID="lblIdMovimento" runat="server" Text=""></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Operador:<asp:Label ID="lblOperador" runat="server" Text="Operador"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            PDV:<asp:Label ID="lblpdv" runat="server" Text="pdv"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Data:<asp:Label ID="lblData" runat="server" Text="99/99/9999"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Usuario:<asp:Label ID="lblUsuario" runat="server" Text="USUARIO"></asp:Label>
        </div>
    </div>
    <br />
    <br />
    <br />
    <div style="width: 100%; font-size: 13px;">
        <h2>
            VALORES</h2>
        <asp:GridView ID="gridFinalizadoras" runat="server" ForeColor="#333333" AutoGenerateColumns="False"
            Width="100%" ShowFooter="true" OnDataBound="gridFinalizadoras_DataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="COD_FINALIZADORA" HeaderText="ID">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FINALIZADORA" HeaderText="FINALIZADORA">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="REGISTRADO" HeaderText="REGISTRADO">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="VALOR" HeaderText="VALOR">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="DIFERENCA" HeaderText="DIFERENCA">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <div>
        <table style="width: 100%">
            <tr>
                <td style="width: 40%; border-bottom-style: solid; border-bottom-width: 1px;">
                </td>
                <td>
                </td>
                <td style="width: 40%; border-bottom-style: solid; border-bottom-width: 1px;">
                </td>
            </tr>
            <tr>
                <td>
                    <center>Fiscal de Caixa</center>
                </td>
                <td>
                </td>
                <td>
                    <center>Operador de Caixa</center>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
