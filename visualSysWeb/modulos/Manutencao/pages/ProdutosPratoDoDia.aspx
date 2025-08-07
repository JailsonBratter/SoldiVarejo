<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProdutosPratoDoDia.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ProdutosPratoDoDia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../css/ProdutosPratoDoDia.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row text-center">
        <h1>Manutenção de pratos do dia</h1>
        <hr />
    </div>
    <div class="container">
        <div class="row">

            <div class="col-md-5 ">


                <div class="panel panel-default" style="width: 100%;">

                    <div class="row">
                        <div class="col-lg-12">
                            <label>Descrição do prato:</label>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="60%" Height="35px" AutoPostBack="true" OnTextChanged="txtDescricao1_TextChanged" />
                            <asp:ImageButton ID="imgBtnPesquisa" 
                                    ImageUrl="../../../img/pesquisaM.jpg"  
                                runat="server" 
                                Width="25px" 
                                style="margin-top:0px;"
                                OnClick="imgBtnPesquisa_Click" />
                        </div>
                    </div>

                </div>


            </div>
            <div class="col-md-1 colunaCentro ">
            </div>
            <div class="col-md-5">
                <div class="panel panel-default" style="width: 100%;">

                    <div class="row">
                        <div class="col-lg-12">
                            <label>Dia da Semana:</label>
                            <asp:DropDownList ID="ddlDiaSemana" runat="server" AutoPostBack="true" OnTextChanged="ddlDiaSemana_TextChanged" Font-Size="X-Large">
                                <asp:ListItem Value="1" Text="Domingo" />
                                <asp:ListItem Value="2" Text="Segunda" />
                                <asp:ListItem Value="3" Text="Terça" />
                                <asp:ListItem Value="4" Text="Quarta" />
                                <asp:ListItem Value="5" Text="Quinta" />
                                <asp:ListItem Value="6" Text="Sexta" />
                                <asp:ListItem Value="7" Text="Sabado" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>


            </div>
        </div>
        <div class="row">
            <div class="col-lg-5" style="overflow: auto; height: 500px;">
                <div class="row">
                    <div class="col-lg-6">
                        <asp:ImageButton ID="imgBtnAddItens"
                            ImageUrl="../../../img/add.png"
                            runat="server" Width="25px"
                            OnClick="imgBtnAddItens_Click" />
                        <label>Incluir novos Produtos</label>
                    </div>
                    <div class="col-lg-6">
                        <asp:ImageButton ID="imgBtnExcluir"
                            ImageUrl="../../../img/cancel.png"
                            runat="server"
                            Width="25px"
                            OnClick="imgBtnExcluir_Click" />
                        <label>Desmarcar Selecionados</label>
                    </div>
                </div>
                <asp:GridView ID="gridProdutos1" runat="server" AutoGenerateColumns="false" CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona1_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="plu" HeaderText="PLU" />
                        <asp:BoundField DataField="ean" HeaderText="EAN" />
                        <asp:BoundField DataField="ref" HeaderText="REF" />
                        <asp:BoundField DataField="descricao" HeaderText="DESCRICAO" />
                        <asp:BoundField DataField="preco" HeaderText="PRECO"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />


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
            <div class="col-md-1 colunaCentro">

                <div class="row text-center">
                    <div class="col-md-12">

                        <asp:Button ID="btnEsquerda" runat="server" Text="←" OnClick="btn_Click" title="Retira Produtos Selecionados do dia" CssClass="btn btn-default" Font-Size="XX-Large" />

                    </div>
                </div>
                <div class="row text-center">
                    <asp:Button ID="btnDireita" runat="server" Text="→" OnClick="btn_Click" title="Inclui Produtos Selecionados no dia" CssClass="btn btn-default" Font-Size="XX-Large" />
                </div>
            </div>
            <div class="col-lg-5" style="overflow: auto; height: 500px;">
                <br />
                <br />
                <asp:GridView ID="gridProdutos2" runat="server" AutoGenerateColumns="false" CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona2_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="plu" HeaderText="PLU" />
                        <asp:BoundField DataField="ean" HeaderText="EAN" />
                        <asp:BoundField DataField="ref" HeaderText="REF" />
                        <asp:BoundField DataField="descricao" HeaderText="DESCRICAO" />
                        <asp:BoundField DataField="preco" HeaderText="PRECO"
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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

        </div>

    </div>


    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbltituloLista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row1" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row1">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CssClass="table"
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
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbltituloLista">
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



    <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="modalForm" Style="display: none; height: 700px;"
        DefaultButton="imgPesquisaMercadoria">
        <div class="frame" style="height:85%;">

            <div class="row text-center" style="margin-left:0px">
                <h1>
                    <asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto"></asp:Label></h1>
                 <hr  />
            </div>

            <div class="container" style="padding:0px; margin-top:0px;">
                
                <div class="row" style="padding:0px; margin-top:-10px; margin-bottom:0px;">
                    <div class="col-md-6" style="text-align: center; padding:0px;">
                        <asp:ImageButton ID="ImgBtnConfirmar" runat="server" ImageUrl="~/img/confirm.png"
                            Height="20px" OnClick="ImgBtnConfirmar_Click" />Confirmar
                    </div>
                    <div class="col-md-6" style="text-align: center;padding:0px;">
                        <asp:ImageButton ID="ImgBtnCancelar" runat="server" ImageUrl="~/img/cancel.png"
                            Height="20px" OnClick="ImgBtnCancelar_Click" />Cancelar
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">

               <div id="Div1" runat="server" class="col-md-5 filtro" style="height: 200px;">
                <table>
                    <tr>
                        <td colspan="3">
                            <center><b>Filtrar</b></center>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>Grupo:
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" Width="200px"
                                OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>SubGrupo:
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlSubGrupo" runat="server" AutoPostBack="True" Width="200px"
                                OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Departamento:
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Width="200px"
                                OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Linha
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlLinha" runat="server" AutoPostBack="True" Width="200px"
                                OnSelectedIndexChanged="ddlLinha_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td>Plu/EAN/Descricao:
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                                autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="20px" OnClick="ImgPesquisaMercadoria_Click" />Filtrar
                        </td>
                        <td>
                            <asp:ImageButton ID="imgLimpar" runat="server" ImageUrl="~/img/botao-apagar.png"
                                Height="20px" OnClick="imgLimpar_Click" />Limpar
                        </td>

                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="col-md-7"
                Style="height: 200px; overflow:auto; border:solid 1px;">

               
                    <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None">
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
                
            </asp:Panel>
            </div>
           
            <div class="btnImprimirDireita" style="margin-right:20px;">
                <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                    Height="30px" Width="30px" OnClick="imgBtnIncluirSelecionados_Click" />
                Incluir item
            </div>
            <br />
            <center><h3><b>Selecionados </b></h3></center>
            <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv" Style="height: 200px;">
                <div class="gridTableTelaCheia">
                    <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                        GridLines="Vertical" AutoGenerateColumns="False"
                        OnRowCommand="GridMercadoriaSelecionado_RowCommand" CssClass="table">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                            <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>

                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                Text="Alterar">
                                <ControlStyle Height="20px" Width="20px" />
                            </asp:ButtonField>
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
            </asp:Panel>

        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalIncluiItens" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMercadoriaListaFrame" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>


     <asp:Panel ID="pnConfirmaExclusao" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Tem Certeza Que gostaria de Esmarcar a opção 'Prato do dia'<br> dos Itens Selecionados ??" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <div class="container" style="padding:0px; margin-top:0px;">
                
                <div class="row" style="padding:0px; margin-top:-10px; margin-bottom:0px;">
                    <div class="col-md-6" style="text-align: center; padding:0px;">
                        <asp:ImageButton ID="ImgBtnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                            Height="20px" OnClick="ImgBtnConfirmaExclusao_Click" />Sim
                    </div>
                    <div class="col-md-6" style="text-align: center;padding:0px;">
                        <asp:ImageButton ID="ImgBtnCancelarExclusao" runat="server" ImageUrl="~/img/cancel.png"
                            Height="20px" OnClick="ImgBtnCancelarExclusao_Click" />Não
                    </div>
                </div>
            </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExclusao" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExclusao" TargetControlID="Label1">
    </asp:ModalPopupExtender>


</asp:Content>
