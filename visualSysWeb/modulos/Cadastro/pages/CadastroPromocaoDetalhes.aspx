<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroPromocaoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CadastroPromocaoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Cadastro de Promoção SCANTECH Detalhes</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />

    <div id="cabecalho" runat="server" class="frame" style=" width:75%; height:120px; margin:auto;">
        <div class="panelItem">
            <p>Codigo</p>
            <asp:TextBox ID="txtCodigo" runat="server" Width="80" />
        </div>
          <div class="panelItem">
            <p>Descrição</p>
            <asp:TextBox ID="txtDescricao" runat="server" Width="250" MaxLength="50" />
        </div>
        <div class="panelItem">
            <p>Tipo</p>
            <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                <asp:ListItem Value="1" Text="Desconto Percentual" />
                <asp:ListItem Value="2" Text="Leve X pague Y" />
                <asp:ListItem Value="3" Text="Brinde" />
                <asp:ListItem Value="3" Text="Preço Fixo" />
            </asp:DropDownList>
        </div>
        <div class="panelItem">
            <p>Inicio</p>
            <asp:TextBox ID="txtDtInicio" runat="server" Width="70" />
            <asp:ImageButton ID="imgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtDe"
                TargetControlID="txtDtInicio">
            </asp:CalendarExtender>
        </div>
        <div class="panelItem">
            <p>Fim</p>
            <asp:TextBox ID="txtDtFim" runat="server" Width="70"  />
            <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                TargetControlID="txtDtFim">
            </asp:CalendarExtender>
        </div>
      
        <div class="panelItem">
            <p>
                <asp:Label ID="lblParam_Base" Text="Quantidade" runat="server" />
            </p>
            <asp:TextBox ID="txtParam_Base" runat="server" Width="80" CssClass="numero"/>
        </div>
        <div id="divParamBrinde" class="panelItem">
            <p>
                <asp:Label ID="lblParam_Brinde" Text="Desconto" runat="server"  />
            </p>
            <asp:TextBox ID="txtParam_Brinde" runat="server" Width="80" CssClass="numero" />
        </div>
    </div>
    <div id="conteudo" runat="server" class="conteudo">

        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabItens">
                <HeaderTemplate>
                    Itens
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItensDig" runat="server" CssClass="filter" DefaultButton="imgPlu">
                        <table>
                            <tr>
                                <td>
                                    <div class="panelItem">
                                        <p>
                                            PLU
                                        </p>
                                        <asp:TextBox ID="txtPlu" runat="server" Width="80px"></asp:TextBox><asp:ImageButton
                                            ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>
                                            Descrição
                                        </p>
                                        <asp:TextBox ID="txtDescricaoItem" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <br />
                                        <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            Width="20px" OnClick="ImgBtnAddItens_Click" />Incluir item
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:GridView ID="gridItens" runat="server" CellPadding="4" ForeColor="#333333"
                        OnRowCommand="gridItens_RowCommand"
                        AutoGenerateColumns="false"
                        GridLines="Vertical"
                        Width="50%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU"  ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="descricao" HeaderText="Descrição" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabBrindes">
                <HeaderTemplate>
                    Brindes
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItensDigBrinde" runat="server" CssClass="filter" DefaultButton="imgBtnPluBrinde">
                        <table>
                            <tr>
                                <td>
                                    <div class="panelItem">

                                        <p>
                                            PLU
                                        </p>
                                        <asp:TextBox ID="txtPluBrinde" runat="server" Width="80px"></asp:TextBox><asp:ImageButton
                                            ID="imgBtnPluBrinde" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>
                                            Descrição
                                        </p>
                                        <asp:TextBox ID="txtDescricaoBrinde" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <br />
                                        <asp:ImageButton ID="imgBtnAddBrinde" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            Width="20px" OnClick="ImgBtnAddItens_Click" />Incluir item
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:GridView ID="gridBrindes" runat="server" CellPadding="4" ForeColor="#333333"
                        OnRowCommand="gridBrindes_RowCommand"
                        AutoGenerateColumns="false"
                        GridLines="Vertical"
                         Width="50%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="descricao" HeaderText="Descrição" />
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
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label>
                </h1>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            Filtrar
            <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="600px" CssClass="SEM"></asp:TextBox>
            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaLista_Click" />
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
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label2" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style=" overflow: auto; min-height:100px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>


     <asp:Panel ID="pnConfirmaExcluirItem" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label6" runat="server" Text="Tem Certeza que gostaria de Excluir o Item?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <asp:Label ID="lblPluExcluirItem" runat="server" Text="" Visible="false"></asp:Label> 
         <asp:Label ID="lblPluExcluirBrinde" runat="server" Text="" Visible="false"></asp:Label>
    
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExcluirItem" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExcluirItem_Click" />
                    <asp:Label ID="Label7" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExcluirItem" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExcluirItem_Click" />
                    <asp:Label ID="Label8" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirItem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirItem" TargetControlID="Label6">
    </asp:ModalPopupExtender>

     <asp:Panel ID="pnExcluirPromocao" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label3" runat="server" Text="Tem Certeza que gostaria de Excluir a Promocao?"
                CssClass="cabMenu"></asp:Label>
        </h3>
       
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBntnExcluirPromocao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBntnExcluirPromocao_Click" />
                    <asp:Label ID="Label9" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarExcluir_Click" />
                    <asp:Label ID="Label10" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirPromocao" TargetControlID="Label3">
    </asp:ModalPopupExtender>
</asp:Content>
