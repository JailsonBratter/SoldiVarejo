<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProgramacaoPrecificacaoProdutos.aspx.cs" Inherits="visualSysWeb.modulos.Gerencial.pages.ProgramacaoPrecificacaoProdutos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Programacao de Preços</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" style="margin-left: 15px">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Id
                        </p>
                        <asp:TextBox ID="txtId" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Filial
                        </p>
                        <asp:TextBox ID="txtFilial" runat="server" Width="100px"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnFilial" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Descricao
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="300px"> </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Plu
                        </p>
                        <asp:TextBox ID="txtPlu" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnPlu" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Usuario
                        </p>
                        <asp:TextBox ID="txtUsuario" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnUsuario" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            De
                        </p>
                        <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" AutoPostBack="true"
                            OnTextChanged="txtDe_TextChanged"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ate
                        </p>
                        <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Pesquisar Por
                        </p>
                        <asp:DropDownList ID="DdlTipoPesquisa" runat="server">
                            <asp:ListItem Value="data_cadastro">CADASTRO</asp:ListItem>
                            <asp:ListItem Value="data_inicio">INICIO</asp:ListItem>

                        </asp:DropDownList>
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
                <asp:HyperLinkField DataTextField="id" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="Filial" Text="---" Visible="true" HeaderText="Filial"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="descricao" Text="----" Visible="true" HeaderText="Descricao"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="data_cadastroBr" Text="----" Visible="true" HeaderText="Cadastro"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="data_InicioBr" Text="----" Visible="true" HeaderText="Inicio"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
                    DataNavigateUrlFields="id" />
                <asp:HyperLinkField DataTextField="usuario_cadastro" Text="----" Visible="true" HeaderText="Usuario"
                    DataNavigateUrlFormatString="~/modulos/Gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?id={0}"
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
</asp:Content>
