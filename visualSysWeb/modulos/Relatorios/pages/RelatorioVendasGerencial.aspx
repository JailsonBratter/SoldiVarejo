<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RelatorioVendasGerencial.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelatorioVendasPorAliquota" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divPage" runat="server">
        <div class="row">
            <asp:Label ID="LblTitulo" runat="server" Text=" Relatorios de vendas gerencial "
                CssClass="titulos"></asp:Label>
        </div>
        <div class="row" style="margin-top: 30px;">
            <div class="panelItem">
                <asp:RadioButtonList ID="RdoRelatorios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RdoRelatorios_SelectedIndexChanged"
                    RepeatColumns="4" Width="100%">
                    <asp:ListItem Selected="True">01-Aliquota</asp:ListItem>
                    <asp:ListItem>02-Finalizadora</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="panelItem">
                <p>
                    De</p>
                <asp:TextBox ID="txtDe" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                    TargetControlID="txtDe">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Ate</p>
                <asp:TextBox ID="txtAte" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                <asp:ImageButton ID="dt_txtAte" runat="server" ImageUrl="~/img/calendar.png" Height="15px" />
                <asp:CalendarExtender ID="ClnDataAte" runat="server" PopupButtonID="Dt_txtAte" TargetControlID="txtAte">
                </asp:CalendarExtender>
            </div>
        </div>
        <div class="row">
            <asp:Panel ID="pnBtnVisualiar" runat="server">
                <br />
                <br />
                <br />
                <br />
                <br />
                <p>
                    VISUALIZAR</p>
                <asp:ImageButton ID="ImgBtnVisualizarREL" runat="server" ImageUrl="~/img/visualizar.png"
                    Width="80px" OnClick="btnVisualizar_Click" />
                <br />
                <br />
            </asp:Panel>
        </div>
    </div>
    <div  id="divRelatorio" runat="server" visible="false" style="width:100%;" >
        <div class="row">
            <asp:ImageButton ID="ImgBtnVoltar" runat="server" Height="28px" ImageUrl="~/img/icon_voltar.jpg"
                Width="60px" OnClick="ImgBtnVoltar_Click" />
        </div>
        <div class="row">
            <center><h4><asp:Label ID="lbltituloRelatorio" runat="server" Text="" ></asp:Label></h4></center>
            <hr />
        </div>
        <asp:Label ID="lblFiltros" runat="server" Text="" CssClass="titulos"></asp:Label>
        <asp:GridView ID="GridRelatorio" runat="server" CellPadding="4" ForeColor="#333333"
            OnDataBound="Gridrelatorio_DataBound" GridLines="Vertical" Width="100%" ShowFooter="True">
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
        <div class="rodapeRelatorio">
            <asp:Label ID="LblRodape" runat="server" Text="Bratter e Bocco Automação Comercial  Av. Indianópolis, 1788 - Planalto Paulista - São Paulo - SP - 04062-002     Tel/Fax: (11) 5078-6121"></asp:Label>
        </div>
    </div>


</asp:Content>
