<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AlterarAliquota.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.AlterarAliquota" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Atualização de Aliquota de Saida</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <br />
    <div class="row">
        <div class="filter">
            <div class="panelItem">
                <p>
                    Codigo Tributacao</p>
                <asp:DropDownList ID="ddlTributacao" runat="server">
                </asp:DropDownList>
            </div>
            <div class="panelItem">
                <p>
                    Ncm</p>
                <asp:TextBox ID="txtNcm" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnNcm" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                    OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    CEST</p>
                <asp:TextBox ID="txtCest" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnCest" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                    OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    CST PIS/COFINS</p>
                <asp:TextBox ID="txtCstPisConfins" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnCstPisConfins" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    PIS%</p>
                <asp:TextBox ID="txtPis" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    COFINS%</p>
                <asp:TextBox ID="txtCofins" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    IPI%</p>
                <asp:TextBox ID="txtIpi" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    IVA%</p>
                <asp:TextBox ID="txtIva" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <br />
                <asp:Button ID="btnAplicarAlteracoes" runat="server" Text="Aplicar Alterações" Height="35px"
                    OnClick="btnAplicarAlteracoes_Click" />
            </div>
            <div class="panelItem">
                <p>
                    Origem</p>
                <asp:DropDownList ID="ddlOrigem" runat="server">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="0">0-Nacional</asp:ListItem>
                    <asp:ListItem Value="1">1-Estrangeira – Importação direta</asp:ListItem>
                    <asp:ListItem Value="2">2-Estrangeira – Adquirida no mercado interno</asp:ListItem>
                    <asp:ListItem Value="3">3-Nacional - Mercadoria/bem Imp sup 40%</asp:ListItem>
                    <asp:ListItem Value="4">4-Nacional - Produção Decreto-Lei nº 288/67</asp:ListItem>
                    <asp:ListItem Value="5">5-Nacional - Mercadoria/bem Imp inf ou igual 40%</asp:ListItem>
                    <asp:ListItem Value="6">6-Estrangeira - Importação direta, sem similar nacional</asp:ListItem>
                    <asp:ListItem Value="7">7-Estrangeira - Adquirida no mercado interno, sem similar nacional</asp:ListItem>
                </asp:DropDownList>
            </div>
            
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="filter">
            <div class="row">
                <h3>
                    Filtros:</h3>
                <hr />
            </div>
            <div class="panelItem">
                <p>
                    PLU</p>
                <asp:TextBox ID="txtPlu" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Descricao</p>
                <asp:TextBox ID="txtDescricao" runat="server" Width="150px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Ncm de</p>
                <asp:TextBox ID="txtNcmDe" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Ncm ate</p>
                <asp:TextBox ID="txtNcmAte" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Grupo</p>
                <asp:TextBox ID="txtGrupo" runat="server" Width="120px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                    OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    SubGrupo</p>
                <asp:TextBox ID="txtSubGrupo" runat="server" Width="120px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    Departamento</p>
                <asp:TextBox ID="txtDepartamento" runat="server" Width="120px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="Img_Click" />
            </div>
            <div class="panelItem">
                <p>
                    Familia</p>
                <asp:TextBox ID="txtFamilia" runat="server" Width="120px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnFamilia" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="Img_Click" />
            </div>
        </div>
    </div>
    <div class="gridTable">
        <asp:GridView ID="GridItens" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" />
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
    <asp:Panel ID="PnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria Alterar as Tributações dos itens Selecionados?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarEncerrar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaEncerrar_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarEncerrar_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEncerrar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirma" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
