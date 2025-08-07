<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RelComercial.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelComercial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h2>
            Relatorio Comercial
        </h2>
    </center>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" CssClass="filter" BorderStyle="Solid" BorderWidth="1px">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Descrição do Produto</p>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="327px"></asp:TextBox>

                </td>
                <td>
                    <p>
                        Grupo<p />
                        <asp:TextBox ID="txtDescricaoGrupo" runat="server" Width="239px"></asp:TextBox>
                    </p>
                </td>
                <td>
                    <p>
                        SubGrupo<p />
                        <asp:TextBox ID="txtDescricaoSubGrupo" runat="server" Width="236px"></asp:TextBox>
                    </p>
                </td>
                <td>
                    <p>
                        Departamento<p />
                        <asp:TextBox ID="txtDescricaoDepto" runat="server" Width="257px"></asp:TextBox>
                    </p>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <asp:Panel ID="Panel5" runat="server" Height="650px" BorderStyle="Solid" BorderWidth="1px">
        <div class="vendaMercadoriaComercial">
            <asp:Label ID="Label1" runat="server" Text="Venda Qtde/Vlr Ultimos 13 meses" Font-Bold="True"></asp:Label>
            <asp:GridView class="gridTable" ID="GridVendas" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" CssClass="FrameDivisaoTela" AutoGenerateColumns="False" ShowFooter="true">
                <Columns>
                    <asp:BoundField DataField="AnoMes" HeaderText="Ano/Mes" />
                    <asp:BoundField DataField="Qtde" HeaderText="Qtde" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="vlr" HeaderText="Vlr" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="Lucro" HeaderText="Lucro" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                </Columns>
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
        <div class="vendaGraficoComercial">
            <asp:Chart ID="Grafico01" runat="server" Width="700px">
                <Series>
                    <asp:Series ChartArea="ChartArea1" Legend="Legend1" Name="Qtde" XValueMember="AnoMes"
                        YValueMembers="Qtde">
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Color="red" Legend="Legend1" Name="Vlr" XValueMember="AnoMes"
                        YValueMembers="Vlr">
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Color="Green" Legend="Legend1" Name="Lucro" XValueMember="AnoMes"
                        YValueMembers="Lucro">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 8pt, style=Bold" Name="title01" Text="Qtde/Vlr ultimos 13 meses">
                    </asp:Title>
                </Titles>
            </asp:Chart>
            <asp:Chart ID="Grafico02" runat="server" OnLoad="Chart1_Load" Width="649px">
                <Series>
                    <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1" XValueMember="GRUPO_DESCRICAO"
                        YValueMembers="Vlr">
                        <EmptyPointStyle IsVisibleInLegend="False" />
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                        <Area3DStyle Enable3D="true" Inclination="20" />
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Name="Title1" Font="Microsoft Sans Serif, 8pt, style=Bold" Text="Vendas por Grupo">
                    </asp:Title>
                </Titles>
            </asp:Chart>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" Height="841px" BorderStyle="Solid" BorderWidth="1px">
        <div class="vendaMercadoriaComercialItens">
            <center>
                <asp:Label ID="Label2" runat="server" Text="30 Produtos de maior valor" Font-Bold="True"></asp:Label>
            </center>
            <asp:GridView ID="Grid30Mais" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" CssClass="FrameDivisaoTela" AutoGenerateColumns="False">
                <Columns>
                <asp:HyperLinkField DataTextField="PLU" HeaderText="PLU" Text="PLU" 
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}" 
                        DataNavigateUrlFields="PLU" />
                    <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                    <asp:BoundField DataField="DEPARTAMENTO" HeaderText="Cod.Depto" />
                    <asp:BoundField DataField="DEPARTAMENTO_DESCRICAO" HeaderText="Nome.Depto" />
                    <asp:BoundField DataField="Qtde" HeaderText="Qtde" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="vlr" HeaderText="Vlr" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="PrcMD" HeaderText="MD" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                </Columns>
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
    </asp:Panel>
</asp:Content>
