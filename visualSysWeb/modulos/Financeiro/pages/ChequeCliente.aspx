<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChequeCliente.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ChequeCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Cheque Clientes</h1></center>
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
                        Cliente</p>
                    <asp:TextBox ID="txtCliente" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ/CPF</p>
                    <asp:TextBox ID="txtCnpjCpf" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Tipo
                    </p>
                      <asp:DropDownList ID="ddlTipoPesquisa" runat="server">
                            <asp:ListItem Value="Emissao_cheque">EMISSAO</asp:ListItem>
                            <asp:ListItem Value="deposito_cheque">DEPOSITO</asp:ListItem>
                            <asp:ListItem Value="data_cadastro">DATA</asp:ListItem>
                        </asp:DropDownList>
                </td>

                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" 
                        AutoPostBack="true" ontextchanged="txtDe_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" 
            ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="---" Visible="true" HeaderText="Codigo Cliente"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="Nome_Cliente" Text="---" Visible="true" HeaderText="Nome Cliente"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="CNPJ" Text="---" Visible="true" HeaderText="CNPJ/CPF"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="QtdCheques" Text="---" Visible="true" HeaderText="Qtde Cheques"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="UltimoCheque" Text="---" Visible="true" HeaderText="Ultimo Cheque"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="Emissao" Text="---" Visible="true" HeaderText="Emissao"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ChequeClienteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Codigo_cliente" />
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
