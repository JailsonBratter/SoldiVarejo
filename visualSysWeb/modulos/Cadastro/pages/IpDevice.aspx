<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IpDevice.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.IpDevice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>IpDevice</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="panel" id="filtrosPesq" runat="server">
        <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div class="row">
            <div class="panelItem">
                <p>
                    Id</p>
                <asp:TextBox ID="txtId" runat="server" Width="70px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    IP</p>
                <asp:TextBox ID="txtIp" runat="server"> </asp:TextBox>
            </div>
        </div>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" HeaderText="id" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ipDeviceDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="ip" Text="ip" Visible="true" HeaderText="ip" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ipDeviceDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="tipo" Text="tipo" Visible="true" HeaderText="tipo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ipDeviceDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="balanca_serial" Text="balanca_serial" Visible="true"
                    HeaderText="balanca_serial" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ipDeviceDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
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
