<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OutrasMovimentacoes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.OutrasMovimentacoes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Outras Movimentações</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>Tipo</p>
                    
                    <asp:DropDownList ID="ddTipo" runat="server">
                    </asp:DropDownList>
                </td>
                
                <td>
                    <p>
                        Descrição Inventario</p>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="200px"> </asp:TextBox>
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDataDe" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Até</p>
                    <asp:TextBox ID="txtDataAte" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                    <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                        TargetControlID="txtDataAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        PLU</p>
                    <asp:TextBox ID="txtPLU" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" 
            CellPadding="6" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridPesquisa_RowCommand" OnRowDataBound="gridPesquisa_RowDataBound">
             <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo_inventario" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:HyperLinkField DataTextField="TipoMovimentacao" Text="---" Visible="true"
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                
                <asp:HyperLinkField DataTextField="Descricao_inventario" Text="---" Visible="true"
                    HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:HyperLinkField DataTextField="Usuario" Text="Usuario" Visible="true" HeaderText="Usuario"
                    DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:HyperLinkField DataTextField="status" Text="status" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:HyperLinkField DataTextField="DataHora_Encerramento" Text="status" Visible="true" HeaderText="Data e Hora Encerrado"
                    DataNavigateUrlFormatString="~/modulos/estoque/pages/OutrasMovimentacoesDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_inventario" />
                <asp:ButtonField ButtonType="Image" ImageUrl="../../NotaFiscal/imgs/nfmenu.jpg" CommandName="EmitirNf"
                    Text="" HeaderText="Emitir NFE">
                    <ControlStyle Height="20px" Width="20px" />
                </asp:ButtonField>
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
        <br />
    </div>
</asp:Content>
