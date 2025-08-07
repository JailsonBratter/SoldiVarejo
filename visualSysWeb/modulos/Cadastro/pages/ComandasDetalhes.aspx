<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ComandasDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ComandasDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 138px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Comanda</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="conteudo" runat="server" style="height: 600px;">
        <div style="width:200px;  margin-left:15%; margin-right: auto">
            <table>
                <tr>
                    <td>
                        <p>
                            Comanda</p>
                        <asp:TextBox ID="txtComanda" runat="server" CssClass="numero" ></asp:TextBox>
                    </td>
                    <td class="style1">
                        <p>
                            Status</p>
                        <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="129px">
                            <asp:ListItem Value="0">LIVRE</asp:ListItem>
                            <asp:ListItem Value="2">ABERTA</asp:ListItem>
                            <asp:ListItem Value="4">BLOQUEADA</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td >
                        <p>
                            Observação</p>
                        <asp:TextBox ID="txtOBSERVACAO" runat="server"  Width="499px"
                           ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; margin-left: 2px; margin-right: 5px;">
            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Height="400px"
                >
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                    <HeaderTemplate>
                        Itens Adicionados
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div style="height:350px; width:100%; overflow:auto;  ">
                            
                            
                            <asp:GridView ID="gridItensComanda" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" GridLines="None" ShowFooter="True" Width="100%"
                                ondatabound="gridItensComanda_DataBound" >
                                <Columns>
                                   
                                    <asp:BoundField DataField="Plu" HeaderText="PLU" />
                                    <asp:BoundField DataField="Descricao" HeaderText="DESCRICAO" />
                                    <asp:BoundField DataField="origem" HeaderText="ORIG" />
                                    <asp:BoundField DataField="usuario" HeaderText="USUARIO" />
                                    <asp:BoundField DataField="data" HeaderText="DATA" />
                                    <asp:BoundField DataField="hora" HeaderText="HORA" />
                                    <asp:BoundField DataField="qtde" HeaderText="QTDE" >
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                     </asp:BoundField>
                                    <asp:BoundField DataField="unitario" HeaderText="UNITARIO" >
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                     </asp:BoundField>
                                    <asp:BoundField DataField="total" HeaderText="TOTAL">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                     </asp:BoundField>
                                </Columns>
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
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
               
            </asp:TabContainer>
        </div>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible="false">
        <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click"></asp:ImageButton>
                    <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" CssClass="lista">
            <asp:RadioButtonList ID="chkLista" runat="server" Height="50" Width="200">
            </asp:RadioButtonList>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="PnExcluirComanda" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Bloquear a COMANDA?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarInativar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExluirComanda" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirComanda" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
