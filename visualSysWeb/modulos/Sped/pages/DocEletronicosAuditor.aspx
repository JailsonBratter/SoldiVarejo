<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocEletronicosAuditor.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.DocEletronicosAuditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>Documentos Eletronicos</h1>
    </center>

    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <div class="btnImprimirDireita">
            Limpar Filtros
            <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
                Style="width: 20px" />
        </div>
       
        <div class="container row" style="margin-left:25px">
            <div class="panelItem">
                <p>
                    De
                </p>
                <asp:TextBox ID="txtDataDe" runat="server" AutoPostBack="true" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Ate
                </p>
                <asp:TextBox ID="txtDataAte" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                    TargetControlID="txtDataAte">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Tipo</p>
                <asp:DropDownList ID="DllTipo" runat="server" CssClass="sem">
                    <asp:ListItem Value="1">Cupom CFe</asp:ListItem>
                    <asp:ListItem Value="2">NFe Emitidas (Entradas e Saídas)</asp:ListItem>
                </asp:DropDownList>
            </div>
            
        </div>
    </div>
    <div class="row">
        <div class="panelItem" style="margin-left:25px">
            <h1>
                <asp:Label ID="lblQtdRegistros" runat="server" Text=""></asp:Label>
                Documentos Encontrados
            </h1>
        </div>
    </div>


    <div class="gridTable">
        <asp:GridView ID="gridDocumentos" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" >
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="PDV" HeaderText="PDV"></asp:BoundField>
                <asp:BoundField DataField="Emissao" HeaderText="Emissão"></asp:BoundField>
                <asp:BoundField DataField="Documento" HeaderText="Documento"></asp:BoundField>
                <asp:BoundField DataField="Chave" HeaderText="Chave" ></asp:BoundField>
                <asp:BoundField DataField="NroExtrato" HeaderText="NroExtrato"></asp:BoundField>
                <asp:BoundField DataField="Vlr" HeaderText="Vlr"></asp:BoundField>
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
