<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PedidoCompra.aspx.cs" Inherits="visualSysWeb.modulos.Pedidos.pages.PedidoCompra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Pedido De Compra</h1>
        </center>
    </div>
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
                    <asp:TextBox ID="txtPedido" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="txt_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtFornecedor" runat="server" Width="300px" AutoPostBack="true"
                        OnTextChanged="txt_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDataDe" runat="server" OnTextChanged="txtDataDe_TextChanged"
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
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
              <RowStyle CssClass="rowGrid" Wrap="False" /> 
            <Columns>
                <asp:HyperLinkField DataTextField="Pedido" Text="Pedido" Visible="true" HeaderText="Pedido"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" />
                <asp:HyperLinkField DataTextField="Cliente_Fornec" Text="Cliente_Fornec" Visible="true"
                    HeaderText="Fornecedor" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" />
                <asp:HyperLinkField DataTextField="Status" Text="Status" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" />
                <asp:HyperLinkField DataTextField="Data_cadastro" Text="Data_cadastro" Visible="true"
                    HeaderText="Data cadastro" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" />
                <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" HeaderText="Total"
                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="pedido" />
                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/email.png" CommandName="Email" 
                    HeaderText="Enviar Email" Text="Email" ItemStyle-Width="50px">
                    
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
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaEmail" runat="server" CssClass="frameModal" Style="display: none">
        <h2>
            <asp:Label ID="lblPedidoEmail" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblMensagemEmail" runat="server" Text="Tem Certeza que gostaria de Enviar o email "
                CssClass="cabMenu"></asp:Label>
        </h2>
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="btnEnviarEmail" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnEnviarEmail_Click" />
                    <asp:Label ID="lblSimEmail" runat="server" Text="SIM"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="btnCancelaEmail" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancela_Click" />
                    <asp:Label ID="lblNaoEmail" runat="server" Text="NÃO"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaEmail" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaEmail" TargetControlID="lblMensagemEmail"
        >
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 200px; overflow: auto;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap;
                        justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>
</asp:Content>
