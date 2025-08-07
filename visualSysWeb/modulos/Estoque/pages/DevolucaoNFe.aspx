<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DevolucaoNFe.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.DevolucaoNFe" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Devolução NFe</h1>
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
                        Status</p>
                    <asp:DropDownList ID="DllStatus" runat="server" CssClass="sem">
                        <asp:ListItem Value="0">PENDENTE </asp:ListItem>
                        <asp:ListItem Value="1">EMITIDA</asp:ListItem>
                        <asp:ListItem Value="2">TODOS</asp:ListItem>
                    </asp:DropDownList>

                </td>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="100px"></asp:TextBox>
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
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" 
            OnSorting="gridPesquisa_Sorting" onrowcommand="gridPesquisa_RowCommand" >
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFields="Codigo" SortExpression="Codigo" />
                <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="---" Visible="true" HeaderText="CodigoCliente"
                    DataNavigateUrlFields="Codigo" SortExpression="Cliente_fornec" />
                <asp:HyperLinkField DataTextField="nome_cliente" Text="---" Visible="true" HeaderText="Cliente"
                    DataNavigateUrlFields="Codigo" SortExpression="nome_cliente" />
                <asp:HyperLinkField DataTextField="Status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFields="Codigo" SortExpression="status" />
                <asp:HyperLinkField DataTextField="Data_cadastro" Text="---" Visible="true" HeaderText="Data Cadastro"
                    DataNavigateUrlFields="Codigo" SortExpression="Data_cadastro" />
                <asp:HyperLinkField DataTextField="Total" Text="---" Visible="true" HeaderText="Total"
                    DataNavigateUrlFields="Codigo" SortExpression="total" FooterStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:HyperLinkField>
                <asp:ButtonField ButtonType="Image" ImageUrl="../../NotaFiscal/imgs/nfmenu.jpg" CommandName="EmitirNf"
                    Text="" HeaderText="Emitir NFE">
                    <ControlStyle Height="20px" Width="20px" />
                </asp:ButtonField>
              <%--  <asp:ButtonField ButtonType="Image"  ImageUrl="../../../img/ico-doc-danfe.png" CommandName="Danfe"
                                    Text="" HeaderText="Pré     Danfe">
                                    <ControlStyle Height="20px" Width="20px" />
                 </asp:ButtonField>--%>
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
    </div></asp:Content>
