<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContaCorrenteDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ContaCorrenteDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes da Conta Corrente</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu" Height="30px">
    </asp:Panel>
    <br />
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="conteudo" style="float: left; width: 100%">
        <div id="cabecalho" runat="server" class="frame" style="width: 90%; margin-left: 30%;
            float: left;">
            <!--Coloque aqui os campos do cabe?alho    -->
            <div class="panel" style="border-style: none;">
                <div class="row">
                    <div class="panelItem">
                        <asp:Label ID="lblContaCaixa" runat="server" Text="" Style="font-size: 30px;"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="panelItem">
                        <p>
                            ID CC</p>
                        <asp:TextBox ID="txtid_cc" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Banco</p>
                        <asp:TextBox ID="txtBanco" runat="server" AutoPostBack="true" OnTextChanged="txtBanco_TextChanged"
                            Width="80px"></asp:TextBox>
                        <asp:TextBox ID="txtNomeBanco" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtBanco" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />
                    </div>
                </div>
            </div>
        </div>
        <div id="conteudo" runat="server">
            <div class="panel" style="width: 90%; margin-left: 30%; float: left;">
                <div class="panelItem">
                    <p>
                        Agencia</p>
                    <asp:TextBox ID="txtAgencia" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Conta</p>
                    <asp:TextBox ID="txtConta" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        saldo</p>
                    <asp:TextBox ID="txtsaldo" runat="server" CssClass="numero"></asp:TextBox>
                </div>
                <div class="row">
                    <div class="panelItem">
                        <p>
                            Centro de Custo
                        </p>
                        <asp:TextBox ID="txtCentroDeCusto" runat="server" AutoPostBack="true" OnTextChanged="txtBanco_TextChanged"
                            Width="80px"></asp:TextBox>
                        <asp:TextBox ID="txtNomeCentro" runat="server" Width="300px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtCentroDeCusto" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />
                 </div>

                </div>
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
            <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            <hr />
            <div id="divPagPesquisa" runat="server">
                <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                    Filtrar
                    <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                    <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="ImgPesquisaLista_Click" />
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
                        <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                    </div>
                </div>
                <asp:Panel ID="Panel2" runat="server" CssClass="lista" Height="280px">
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
            </div>
            <div id="divAddBanco" runat="server" visible="false" style="margin-left: 10px;">
                <asp:ImageButton ID="btnImgAddNovoBanco" runat="server" ImageUrl="~/img/add.png"
                    Width="25px" OnClick="btnImgAddNovoBanco_Click" />
                Adicionar novo Banco
            </div>
            <div id="divAddDetalhesBanco" runat="server" visible="false">
                <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                    Numero
                    <asp:TextBox ID="txtAddNumeroBanco" runat="server" Width="80px" MaxLength="3" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
                    Nome:
                    <asp:TextBox ID="txtAddNomeBanco" runat="server" Width="200px" MaxLength="20" OnChange="javascript:this.value = this.value.toUpperCase();"> </asp:TextBox>
                </div>
                <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                    <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                        <asp:ImageButton ID="btnImgConfirmaNovoBanco" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnImgConfirmaNovoBanco_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </div>
                    <div class="row">
                        <asp:ImageButton ID="btnImgCancelaNovoBanco" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnImgCancelaNovoBanco_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label3" runat="server" Text="Incluir nova conta caixa?" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaContaCaixa" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaContaCaixa_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaContaCaixa" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaContaCaixa_Click" />
                    <asp:Label ID="Label7" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="Label3">
    </asp:ModalPopupExtender>
</asp:Content>
