<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CEPDeliveryDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CEPDeliveryDetalhes" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do CEP Delivery</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <div class="panel" style="width:100%; height:200px; border-style:solid; border-width:1px;">
            <div class="panel" style="width: 670px; margin-left: 20%;">
                <div class="row" style="margin-left: 10px;">
                    <div class="panelItem">
                        <p>
                            CEP</p>
                        <asp:TextBox ID="txtCEP" Width="100px" runat="server" AutoPostBack="true" 
                            ontextchanged="txtCEP_TextChanged" ></asp:TextBox>
                            <asp:ImageButton ID="imgBtn_txtCEP" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Logradouro</p>
                        <asp:TextBox ID="txtLogradouro" Width="500px" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-left: 10px; margin-bottom: 10px;">
                    <div class="panelItem">
                        <p>
                            Bairro</p>
                        <asp:TextBox ID="txtBairro" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Cidade</p>
                        <asp:TextBox ID="txtCidade" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            UF</p>
                        <asp:TextBox ID="txtUF" Width="50px" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            N. Inicio</p>
                        <asp:TextBox ID="txtnum_inicio" Width="50px" runat="server" CssClass="numero"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            N. Fim</p>
                        <asp:TextBox ID="txtnum_fim" runat="server" Width="50px" CssClass="numero"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
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
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound" >
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
    <asp:ModalPopupExtender ID="ModalFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
         <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir o Endereço?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
