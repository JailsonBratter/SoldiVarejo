<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FinalizadoraDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.FinalizadoraDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da finalizadora</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Nro Finalizadora</p>
                    <asp:TextBox ID="txtNro_Finalizadora" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Finalizadora</p>
                    <asp:TextBox ID="txtFinalizadora" runat="server" Width="200px"></asp:TextBox>
                </td>
        </table>
    </div>
    <div class="opcoes">
        <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabMenu_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Detalhes" Value="tab1" />
            </Items>
            <StaticMenuStyle CssClass="tab" />
            <StaticMenuItemStyle CssClass="item" />
            <StaticSelectedStyle BackColor="Beige" ForeColor="#465c71" />
        </asp:Menu>
    </div>
    <div id="conteudo" runat="server" class="conteudo" >
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="view1" runat="server">
                <table>
                    <tr>
                        <td>
                            <div class="panelItem">
                                <p>
                                    Codigo_centro_custo</p>
                                <asp:TextBox ID="txtCodigo_centro_custo" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="btnCentroCusto" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" Width="15px" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    Pagamento</p>
                                <asp:TextBox ID="txtPagamento" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="bntPagamento" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" Width="15px" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    Troco</p>
                                <asp:TextBox ID="txtTroco" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="panelItem">
                                <p>
                                    Tecla</p>
                                <asp:TextBox ID="txtTecla" runat="server" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Ecf</p>
                                <asp:TextBox ID="txtEcf" runat="server" CssClass="numero"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Autorizadora
                                </p>
                                <asp:DropDownList ID="ddlAutorizadora" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="panelItem">
                                <asp:CheckBox ID="chkNaoComputa" runat="server" Text="Não Computa Venda" />
                            </div>
                          
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center>
                <h1>
                    <asp:Label ID="lbllista" runat="server" Text=""></asp:Label></h1>
            </center>
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
    <asp:Panel ID="pnConfirma" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Label ID="Label1" runat="server" Text="Confirma Exclusão" CssClass="cabMenu"></asp:Label>
        <table class="frame">
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
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label1">
    </asp:ModalPopupExtender>
</asp:Content>
