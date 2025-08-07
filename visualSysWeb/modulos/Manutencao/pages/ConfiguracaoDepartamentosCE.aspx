<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfiguracaoDepartamentosCE.aspx.cs" Inherits="visualSysWeb.modulos.Manutencao.pages.ConfiguracaoDepartamentosCE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Departamento Grafico CE</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />

    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical" OnRowCommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                    Text="Alterar">
                    <ControlStyle Height="20px" Width="20px" />
                </asp:ButtonField>
                <asp:BoundField DataField="Grupo_Grafico" HeaderText="Grupo Grafico" />
                <asp:BoundField DataField="Descricao" HeaderText="Descricao" />
                <asp:BoundField DataField="Codigo_Departamento" HeaderText="Departamento" />
                <asp:BoundField DataField="Dep_Ativa_CE" HeaderText="Ativa CE" />

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
        <br />
    </div>
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


    <asp:Panel ID="pnDetalhesGrupo" runat="server" CssClass="frameModal" Style="display: none;height:300px; width:500px;">
        <div class="cabMenu" style="height: 50px;">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                Detalhes do grupo
            </h2>
        </div>
        <table style="width:100%; ">
            <tr>

                <td>


                    <div class="frame" style="font-size: 20px; width: 100%;float:left; ">
                        <div class="panelItem" style="margin-top: 0; margin-bottom: 10px;">
                            <asp:ImageButton ID="btnConfirmaDepartamentoCE" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaDepartamentoCE_Click" />
                            <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                        </div>
                        <div class="panelItem">
                            <asp:ImageButton ID="btnCancelaDep" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaDep_Click" />
                            <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                        </div>
                        <div id="divExcluir" runat="server" class=" btnImprimirDireita">
                            <asp:ImageButton ID="imgBtnExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="imgBtnExcluir_Click" />
                            <asp:Label ID="Label2" runat="server" Text="Excluir"></asp:Label>
                        </div>
                    </div>
                    <hr style="border:solid 1px;" />

                    <div class="frame" style="height: 300px; width: 100%; padding:20px;">
                       
                        <div class="panelItem">
                            <p>Grupo Grafico</p>
                            <asp:TextBox ID="txtGrupoGrafico" runat="server" Width="80px" MaxLength="9" />
                             <asp:ImageButton ID="imgBtnGrupoGrafico" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="imgBtnGrupoGrafico_Click" />
                        </div>
                        <div class="panelItem">
                            <p>Descricao</p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="300px"  MaxLength="30" />
                        </div>

                        <div class="panelItem">
                            <p>Departamento</p>
                            <asp:TextBox ID="txtCodDepartamento" runat="server" Width="80px" MaxLength="9" />
                             <asp:ImageButton ID="imgBtnDepartamento" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="imgBtnDepartamento_Click" />
                        </div>
                        <div class="panelItem">
                            <br />
                            <asp:CheckBox ID="chkDepAtivaCE" Text="Dep Ativa CE" runat="server" />
                        </div>

                    </div>

                </td>
            </tr>
        </table>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalDetalhesDepartamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnDetalhesGrupo" TargetControlID="Label1">
    </asp:ModalPopupExtender>


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
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Cancela"></asp:Label>
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
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
</asp:Content>
