<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PedidoOrcamento.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoOrcamento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Pedido de Orçamentos</h1>
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
                        Pedido</p>
                    <asp:TextBox ID="txtPedido" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Cliente</p>
                    <asp:TextBox ID="txtCliente" runat="server" Width="300px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCliente" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDataDe" runat="server"  OnTextChanged="txtDataDe_TextChanged"
                        AutoPostBack="true" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtDataAte" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                        TargetControlID="txtDataAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="0">TODOS</asp:ListItem>
                        <asp:ListItem Value="1">ABERTO</asp:ListItem>
                        <asp:ListItem Value="2">FECHADO</asp:ListItem>
                        <asp:ListItem Value="3">CANCELADO</asp:ListItem>
                        <asp:ListItem Value="4">PENDENTE ENTREGA</asp:ListItem>
                        <asp:ListItem Value="5">TRANSITO</asp:ListItem>
                    </asp:DropDownList>
                </td>
           
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" 
            OnSorting="gridPesquisa_Sorting" onrowcommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Pedido" Text="---" Visible="true" HeaderText="Pedido"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="pedido" />
                <asp:HyperLinkField DataTextField="Cliente_Fornec" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="Cliente_fornec" />
                <asp:HyperLinkField DataTextField="nome_cliente" Text="---" Visible="true" HeaderText="Cliente"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="nome_cliente" />
                <asp:HyperLinkField DataTextField="Status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="status" />
                <asp:HyperLinkField DataTextField="Data_cadastro" Text="---" Visible="true" HeaderText="Data Cadastro"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="Data_cadastro" />
                <asp:HyperLinkField DataTextField="Total" Text="---" Visible="true" HeaderText="Total"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoOrcamentoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" SortExpression="total" FooterStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:HyperLinkField>
                <asp:ButtonField ButtonType="Image" ImageUrl="../imgs/pedidoVenda.png" CommandName="EmitirPedido"
                    Text="" HeaderText="Emitir Pedido">
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
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnFechar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrlistaItem" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="LblCancelaLista">
    </asp:ModalPopupExtender>
</asp:Content>
