<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CartaoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CartaoDetalhes" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do cartao</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="conteudo" style="margin-top:100px; height:200px;"   >
        <div class="panelDefault" style="width: 60%; margin-left: 20%; float: left; ">
            <div class="row">
                <div class="panelItem">
                    <p>
                        Nro Finalizadora</p>
                    <asp:TextBox ID="txtnro_Finalizadora" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Id Cartao</p>
                    <asp:TextBox ID="txtid_cartao" runat="server" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Dias</p>
                    <asp:TextBox ID="txtdias" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Taxa</p>
                    <asp:TextBox ID="txttaxa" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Data</p>
                    <asp:TextBox ID="txtdata" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    
                </div>
                <div class="panelItem">
                    <p>
                        Centro Custo</p>
                    <asp:TextBox ID="txtcentro_custo" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="btnCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Despesa cc</p>
                    <asp:TextBox ID="txtdespesa_cc" runat="server" Width="100px"></asp:TextBox>
                    
                                <asp:ImageButton ID="btnDespesasCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtfornecedor" runat="server" Width="120px"></asp:TextBox>
                </div>
               <%-- <div class="panelItem">
                    <p>
                        Ajuste cc</p>
                    <asp:TextBox ID="txtajuste_cc" runat="server" Width="80px"></asp:TextBox>
                </div>--%>
                <div class="panelItem">
                    <p>
                        Bandeira</p>
                    <asp:TextBox ID="txtbandeira" runat="server" Width="100px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Acrecimo</p>
                    <asp:TextBox ID="txtacrecimo" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Id Bandeira</p>
                    <asp:TextBox ID="txtid_Bandeira" runat="server" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Id Rede</p>
                    <asp:TextBox ID="txtid_Rede" runat="server" Width="80px"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <asp:CheckBox ID="chkInativo" runat="server" Text="Inativo" Checked="false" />

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
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound" OnSelectedIndexChanged="GridLista_SelectedIndexChanged">
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
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
</asp:Content>
