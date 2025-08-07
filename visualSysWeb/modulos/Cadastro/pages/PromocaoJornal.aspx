<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PromocaoJornal.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PromocaoJornal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Promoções</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Status
                    </p>
                    <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Descrição
                    </p>
                    <asp:TextBox ID="txtDescricao" runat="server"> </asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Inicio
                    </p>
                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="DATA" MaxLength="10" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDeInicio" ImageUrl="~/img/calendar.png" runat="server"
                        Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImgDeInicio"
                        TargetControlID="txtDataInicio">
                    </asp:CalendarExtender>
                </div>
                <div class="panelItem">
                    <p>
                        Fim
                    </p>
                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="DATA" MaxLength="10" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDeFim" ImageUrl="~/img/calendar.png" runat="server"
                        Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImgDeFim"
                        TargetControlID="txtDataFim">
                    </asp:CalendarExtender>
                </div>
            </div>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="status" Text="----" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PromocaoJornalDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="codigo" Text="----" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PromocaoJornalDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="Descricao" Text="----" Visible="true" HeaderText="Descrição"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PromocaoJornalDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="Data_Inicio" Text="----" Visible="true" HeaderText="Inicio"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PromocaoJornalDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="Data_Fim" Text="----" Visible="true" HeaderText="Fim"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PromocaoJornalDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo" />
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
