<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TabelasPet.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.TabelasPet" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="text-align:center;">
        <h1>Tabelas PET</h1>
        <hr />
    </div>
    <div class="row" style="width: 90%; margin-left: 20px;">
        <div class="panelItem"  style="width:15%;">
            <div class="row">
                <h3>Especie</h3>
                <hr />
            </div>
            <div class="row">
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnAddEspecie" runat="server" ImageUrl="~/img/add.png" Height="20px"
                        Width="20px" OnClick="ImgBtnAddEspecie_Click" />
                    Incluir
                </div>
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnExcluirEspecie" runat="server" ImageUrl="~/img/cancel.png" Height="20px"
                        Width="20px" OnClick="ImgBtnExcluirEspecie_Click" />
                    Excluir
                </div>


            </div>
            <div class="row">
                <asp:Panel ID="pnGridEspecie" runat="server" CssClass="panelItem" style="height:300px; overflow:auto;">
                    <asp:GridView ID="gridEspecie" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="grid_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoEspecie" runat="server" GroupName="GrEspecie" OnCheckedChanged="RdoEspecie_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Especie" HeaderText="Especie" />
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


        </div>
        <div class="panelItem" style="width:40%; margin-left:30px;">
            <div class="row">
                <h3>Raca</h3>
                <hr />
            </div>
            <div class="row">
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnAddRaca" runat="server" ImageUrl="~/img/add.png" Height="20px"
                        Width="20px" OnClick="ImgBtnAddRaca_Click" />
                    Incluir
                </div>
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnEditarRaca" runat="server" ImageUrl="~/img/Edit.png" Height="20px"
                        Width="20px" OnClick="ImgBtnEditarRaca_Click" />
                    Editar
                </div>

                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnExcluirRaca" runat="server" ImageUrl="~/img/cancel.png" Height="20px"
                        Width="20px" OnClick="ImgBtnExcluirRaca_Click" />
                    Excluir
                </div>
            </div>
            <div class="row">
                <asp:Panel ID="Panel1" runat="server" CssClass="panelItem" style="height:300px; overflow:auto;">
                    <asp:GridView ID="gridRaca" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridRaca_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoRaca" runat="server" GroupName="GrRaca" OnCheckedChanged="RdoRaca_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Raca" HeaderText="Raca" />
                            <asp:BoundField DataField="Especie" HeaderText="Especie" />
                            <asp:BoundField DataField="Frequencia_cio" HeaderText="Frequencia_cio" />
                            <asp:BoundField DataField="Duracao_cio" HeaderText="Duracao_cio" />
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

        </div>
        <div class="panelItem" style="margin-left:30px;">
            <div class="row">
                <h3>Pelagem</h3>
                <hr />
            </div>
            <div class="row">
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnAddPelagem" runat="server" ImageUrl="~/img/add.png" Height="20px"
                        Width="20px" OnClick="ImgBtnPelagem_Click" />
                    Incluir
                </div>
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnExcluirPelagem" runat="server" ImageUrl="~/img/cancel.png" Height="20px"
                        Width="20px" OnClick="ImgBtnExcluirPelagem_Click" />
                    Excluir
                </div>

            </div>
            <div class="row">
                <asp:Panel ID="Panel2" runat="server" CssClass="panelItem" style="height:300px; overflow:auto;">
                    <asp:GridView ID="GridPelagem" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" Width="100%" OnRowDataBound="GridPelagem_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoPelagem" runat="server" GroupName="Grpelagem" OnCheckedChanged="RdoPelagem_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Pelagem" HeaderText="Pelagem" />
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
        </div>
    </div>

    <asp:Panel ID="PnDetalhesEspecie" runat="server" CssClass="modalForm" Style="display: none; width:500px; height: 200px">
        <asp:Panel ID="PnDetalhesGrupoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Especie</h1>
                <asp:Label ID="lblErroEspecieDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaDetalhesEspecie" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaDetalhesEspecie_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaDetalhesEspecie" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaDetalhesEspecie_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Especie
                        </p>
                        <asp:TextBox ID="txtEspecie" runat="server" Width="200px" MaxLength="20" onchange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEspecieDetalhe" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnDetalhesEspecie"
        TargetControlID="lblErroEspecieDetalhes">
    </asp:ModalPopupExtender>

     <asp:Panel ID="pnRacaDetalhes" runat="server" CssClass="modalForm" Style="display: none; height: 200px">
        <asp:Panel ID="Panel4" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Raca</h1>
                <asp:Label ID="lblErroRaca" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaDetalhesRaca" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaDetalhesRaca_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaDetalhesRaca" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaDetalhesRaca_Click" />
                        <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Raca
                        </p>
                        <asp:TextBox ID="txtRaca" runat="server" Width="200px" MaxLength="20" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Especie
                        </p>
                        <asp:TextBox ID="txtRacaEspecie" runat="server" Width="200px" MaxLength="20" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Frequencia cio
                        </p>
                        <asp:TextBox ID="txtFrequenciaCio" runat="server" Width="80px" CssClass="numero" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Duração cio
                        </p>
                        <asp:TextBox ID="txtDuracaoCio" runat="server" Width="80px" CssClass="numero" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalRaca" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnRacaDetalhes"
        TargetControlID="lblErroRaca">
    </asp:ModalPopupExtender>

     <asp:Panel ID="panelPelagem" runat="server" CssClass="modalForm" Style="display: none; width:500px; height: 200px">
        <asp:Panel ID="Panel6" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Pelagem</h1>
                <asp:Label ID="lblErroPelagem" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaDetalhesPelagem" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaDetalhesPelagem_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelarDetalhesPelagem" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelarDetalhesPelagem_Click" />
                        <asp:Label ID="Label6" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Pelagem
                        </p>
                        <asp:TextBox ID="txtPelagem" runat="server" Width="200px" MaxLength="20" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPelagemDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="panelPelagem"
        TargetControlID="lblErroPelagem">
    </asp:ModalPopupExtender>

      <asp:Panel ID="pnExcluirEspecie" runat="server" CssClass="modalForm" Style="display: none; width:350px; height:130px;" >
        <asp:Panel ID="Panel5" runat="server" CssClass="frame" >
            <div class="cabMenu">
                <h1>Confirma Excluir a Especie</h1>
                
            </div>
            <table  style="width:80%">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaExcluirEspecie" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaExcluirEspecie_Click" />
                        <asp:Label ID="Label4Especie" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelarExcluirEspecie" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelarExcluirEspecie_Click" />
                        <asp:Label ID="Label7Especie" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirEspecie" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirEspecie"
        TargetControlID="Label4Especie">
    </asp:ModalPopupExtender>

      <asp:Panel ID="pnExcluirRaca" runat="server" CssClass="modalForm" Style="display: none;  width:350px; height:130px; ">
        <asp:Panel ID="Panel5Raca" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Confirma Excluir a Raca</h1>
                
            </div>
             <table  style="width:80%">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaExcluirRaca" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaExcluirRaca_Click" />
                        <asp:Label ID="Label4Raca" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelarExcluirRaca" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelarExcluirRaca_Click" />
                        <asp:Label ID="Label7Raca" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirRaca" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirRaca"
        TargetControlID="Label4Raca">
    </asp:ModalPopupExtender>

      <asp:Panel ID="pnExcluirPelagem" runat="server" CssClass="modalForm" Style="display: none;   width:350px; height:130px; ">
        <asp:Panel ID="Panel5Pelagem" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Confirma Excluir a Pelagem</h1>
                
            </div>
             <table  style="width:80%">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaExcluirPelagem" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaExcluirPelagem_Click" />
                        <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelarExcluirPelagem" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelarExcluirPelagem_Click" />
                        <asp:Label ID="Label7" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirPelagem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirPelagem"
        TargetControlID="Label4">
    </asp:ModalPopupExtender>
    
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
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
</asp:Content>
