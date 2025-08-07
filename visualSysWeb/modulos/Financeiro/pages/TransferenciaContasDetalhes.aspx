<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TransferenciaContasDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.TransferenciaContasDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Movimentação Bancaria</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table style="width: 910px;">
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtId" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data
                        </p>
                        <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtData" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtData"
                            TargetControlID="txtData">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Tipo
                        </p>
                        <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_Changed">
                            <asp:ListItem>TRANSFERENCIA</asp:ListItem>
                            <asp:ListItem>SAQUE</asp:ListItem>
                            <asp:ListItem>DEPOSITO</asp:ListItem>
                            <asp:ListItem>LANCAMENTO CREDITO</asp:ListItem>
                            <asp:ListItem>LANCAMENTO DEBITO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Status
                        </p>
                        <asp:TextBox ID="txtStatus" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Usuário
                        </p>
                        <asp:TextBox ID="txtusuario" runat="server" Width="80px"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" >
        <table>
            <tr>
                <td>
                    <div id="divOrigem" runat="server" class="panelItem">
                        <p>
                            Origem
                        </p>
                        <asp:TextBox ID="txtOrigem" runat="server" Width="200px" MaxLength="20" AutoPostBack="true"
                            OnTextChanged="txtOrigem_Changed"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnOrigem" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                        <br />
                        <div id="divSaldoOrigem" runat="server">
                            Saldo:
                            <asp:Label ID="lblSaldoOrigem" runat="server" Text=""></asp:Label>
                        </div>
                        <div id="divCentroCustoOrigem" runat="server" class="panelItem">
                            <p>
                                Centro de Custo Origem
                            </p>
                            <asp:TextBox ID="txtCodigoCentroCustoOrigem" runat="server"  MaxLength="20"> </asp:TextBox>
                            <asp:Label ID="lblCentroCustoOrigem" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                    <div id="divDestino" runat="server" class="panelItem">
                        <p>
                            Destino
                        </p>
                        <asp:TextBox ID="txtDestino" runat="server" Width="200px" MaxLength="20" AutoPostBack="true"
                            OnTextChanged="txtDestino_Changed"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnDestino" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                        <br />
                        <div id="divSaldoDestino" runat="server">
                            Saldo:
                            <asp:Label ID="lblSaldoDestino" runat="server" Text=""></asp:Label>
                        </div>

                        <div id="divCentroCustoDestino" runat="server" class="panelItem" >
                            <p>
                                Centro de Custo Destino
                            </p>
                            <asp:TextBox ID="txtCodigoCentroCustoDestino" runat="server"  MaxLength="20"> </asp:TextBox>
                            <asp:Label ID="lblCentroCustoDestino" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                    <div id="divCentroCusto" runat="server"  class="panelItem" >
                        <p>
                            Centro de Custo 
                        </p>
                        <asp:TextBox ID="txtCodigoCentroCusto" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                   
                        <asp:Label ID="lblCentroCusto" runat="server" Text="Label"></asp:Label>
                    </div>
                    <br>
                    <br>
                    <br>
                    <br>
                    <div class="panelItem" style="float: right;">
                        <p style="margin-left: 15px; font-size: 23px;">
                            Valor
                        </p>
                        R$<asp:TextBox ID="txtValor" runat="server" Width="180px" CssClass="numero" Style="font-size: 20px;"> </asp:TextBox>
                    </div>
                </td>
            </tr>
            
            <tr>
                <td>
                   
                  
                </td>
            </tr>
            <tr>
                <td>
                   
               
                </td>
            </tr>

            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Observação
                        </p>
                        <asp:TextBox ID="txtObservacao" runat="server" Width="650px" Height="100px" TextMode="MultiLine"
                            CssClass="sem"> </asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
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
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="lblConfirmaTexto" runat="server" Text="Confirma Excluir" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmacao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmacao_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaConfirmacao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaConfirmacao_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="lblConfirmaTexto">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblpnError" runat="server" Text=""  ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnConfirmaError" runat="server" Text="OK" Width="200px" Height="100%" Font-Size="Larger" />

                    </div>

                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblpnError">
    </asp:ModalPopupExtender>
</asp:Content>
