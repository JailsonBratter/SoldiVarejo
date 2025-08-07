<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutrasMovimentacoesPrint.aspx.cs"
    Inherits="visualSysWeb.modulos.Estoque.pages.OutrasMovimentacoesPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="self.print();">
    <form id="form1" runat="server">
    <div style="width: 100%">
        <div style="width: 40%; float: left;">
            Codigo:<asp:Label ID="lblCodigo" runat="server" Text="000"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Tipo Movimentação:<asp:Label ID="lblTipoMovimentacao" runat="server" Text="TIPO MOVIMENTACAO"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Descrição:<asp:Label ID="lblDescricao" runat="server" Text="TIPO MOVIMENTACAO"></asp:Label>
        </div>

        <div style="width: 40%; float: left;">
            Data:<asp:Label ID="lblData" runat="server" Text="99/99/9999"></asp:Label>
        </div>
        <div style="width: 40%; float: left;">
            Status:<asp:Label ID="lblStatus" runat="server" Text="STATUS"></asp:Label>
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
            ITENS</h2>
        <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
            AutoGenerateColumns="False" Width="100%" ShowFooter="true" 
            ondatabinding="gridMercadoriasSelecionadas_DataBinding" 
            ondatabound="gridMercadoriasSelecionadas_DataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="RANK" HeaderText="Ordem">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                
                <asp:BoundField DataField="PLU" HeaderText="PLU">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="EAN" HeaderText="EAN">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ref_Fornecedor" HeaderText="Ref">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="descricao_departamento" HeaderText="Departamento">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Saldo_Atual" HeaderText="Saldo">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="contada" HeaderText="Contado">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Diferenca" HeaderText="Divergencia">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="itemCont" HeaderText="Contado">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Custo" HeaderText="Custo">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Total" HeaderText="Total Estoque">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="totalDivergencia" HeaderText="Total Divergencia">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="totalFinal" HeaderText="Total Final">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
