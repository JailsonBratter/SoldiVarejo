<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroPromocao.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CadastroPromocao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Cadastro Promoção SCANTECH</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Tipo</p>
                        <asp:DropDownList ID="ddlTipo" runat="server">
                            <asp:ListItem Value="0" Text="TODOS" />
                            <asp:ListItem Value="1" Text="Desconto" />
                            <asp:ListItem Value="2" Text="Leve X pague Y" />
                            <asp:ListItem Value="3" Text="Brinde" />
                        </asp:DropDownList>
                    </div>

                    <div class="panelItem">
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="150px"> </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            De
                        </p>
                        <asp:TextBox ID="txtDataDe" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                        <asp:ImageButton ID="imgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtDe"
                            TargetControlID="txtDataDe">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Até
                        </p>
                        <asp:TextBox ID="txtDataAte" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                        <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                            TargetControlID="txtDataAte">
                        </asp:CalendarExtender>
                    </div>
                  
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"
            CellPadding="6" ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="codigo" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CadastroPromocaoDetalhes.aspx?codigo={0}&tela=C031"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="strTipo" Text="----" Visible="true" HeaderText="Tipo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CadastroPromocaoDetalhes.aspx?codigo={0}&tela=C031"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="Descricao" Text="----" Visible="true" HeaderText="Descrição"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CadastroPromocaoDetalhes.aspx?codigo={0}&tela=C031"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="Inicio" Text="----" Visible="true" HeaderText="Inicio"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CadastroPromocaoDetalhes.aspx?codigo={0}&tela=C031"
                    DataNavigateUrlFields="codigo"
                    DataTextFormatString="{0:dd/MM/yyyy}"/>
                <asp:HyperLinkField DataTextField="Fim" Text="----" Visible="true" HeaderText="Fim"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CadastroPromocaoDetalhes.aspx?codigo={0}&tela=C031"
                    DataNavigateUrlFields="codigo" DataTextFormatString="{0:dd/MM/yyyy}" />
                
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
