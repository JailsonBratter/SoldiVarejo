<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="LinhasDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.LinhasDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do linha</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table>
            <tr>
                <td>
                    <p>
                        Codigo Linha</p>
                    <asp:TextBox ID="txtcodigo_linha" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição Linha</p>
                    <asp:TextBox ID="txtdescricao_linha" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <center> <h1>Cores</h1></center>
        <table>
            <tr>
                <td>
                    <p>
                        Codigo Cor</p>
                    <asp:TextBox ID="txtcodigo_cor" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descricao Cor</p>
                    <asp:TextBox ID="txtdescricao_cor" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <asp:ImageButton ID="btnAddCor" runat="server" ImageUrl="~/img/add.png" Width="25px"
                        OnClick="btnAddCor_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Adicionar"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gridCores" runat="server" ForeColor="#333333" GridLines="Vertical"
            AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridCores_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                    Text="Alterar">
                    <ControlStyle Height="20px" Width="20px" />
                    <ItemStyle Width="20px" />
                </asp:ButtonField>
                <asp:BoundField DataField="codigo_cor" HeaderText="Codigo">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="Descricao_cor" HeaderText="Descrição">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
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
