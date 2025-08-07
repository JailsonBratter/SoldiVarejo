<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargaFTP.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.Carga_FTP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1 >
            Impressão de Etiquetas</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnConfirmaPgFrame" runat="server" CssClass="frame">
            <asp:Label ID="lbllista" runat="server" Text="Escolha As etiquetas que serão impressas"
                CssClass="cabMenu"></asp:Label>
            <table class="frame">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                    Checked="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="false" />
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
    <asp:ModalPopupExtender ID="modalEtiquetas" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <div class="conteudo">
        <div class="frameConteudo" style="height: 600px;">
            <div class="MenuEtiqueta" style="border:solid 1px; padding:20px 30px; width:15%; margin-left:5px;">
                <div class="row">
                    <h2 style="text-align:center">LOGIN FTP</h2>    
                </div>
                <div class="row">
                        <p>Usuario</p>
                        <asp:TextBox ID="txtUsuarioFtp" runat="server" Width="100%"></asp:TextBox>

                </div>
                <div class="row">
                    
                        <p>Senha</p>
                        <asp:TextBox ID="txtSenhaFtp" runat="server" Width="100%" ></asp:TextBox>
                   

                </div>
                <div class="row">
                    
                        <p>Endereço</p>
                        <asp:TextBox ID="txtEnderecoFtp" runat="server" Width="100%" ></asp:TextBox>
                   

                </div>


            </div>

            <div class="MenuEtiquetaDireita" style="padding:10px;">
                <div class="filter" id="filtrosPesq" runat="server">
                    <div class="panel" style="width: 100%">
                        <div class="row">
                            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:Label ID="lblPesquisaFiltro" runat="server" Text="" ForeColor="Blue"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="panelItem" style="width: 20%">
                                <p>
                                    Plu / EAN
                                </p>
                                <asp:TextBox ID="txtPluEan" runat="server" Width="100%"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="width: 10%; margin-left: 10px;">
                                <p>
                                    Referencia
                                </p>
                                <asp:TextBox ID="txtReferencia" runat="server" Width="100%"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="width: 58%; margin-left: 10px;">
                                <p>
                                    Descrição
                                </p>
                                <asp:TextBox ID="txtDescricao" runat="server" Width="100%"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">

                            <div class="panelItem">
                                <p>
                                    NF
                                </p>
                                <asp:TextBox ID="txtNF" runat="server" Width="100px"> </asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    FORNECEDOR
                                </p>
                                <asp:TextBox ID="txtFornecedor" runat="server" Width="200px"> </asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>

                            <div class="panelItem">
                                <p>
                                    FAMILIA
                                </p>
                                <asp:TextBox ID="txtfamilia" runat="server" Width="180px"> </asp:TextBox>
                                <asp:ImageButton ID="imgBtnFamilia" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem" style="margin-left: 20px;">
                                <p>Data Cadastro</p>
                                <asp:TextBox ID="txtDtCadastro" runat="server" Width="80px"> </asp:TextBox>
                                <asp:ImageButton ID="ImgDtCadastro" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="clnDataCadastro" runat="server" PopupButtonID="ImgDtCadastro"
                                    TargetControlID="txtDtCadastro" Enabled="True">
                                </asp:CalendarExtender>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Data Alteração
                                </p>
                                <asp:TextBox ID="txtDtalteracao" runat="server" Width="80px"> </asp:TextBox>
                                <asp:ImageButton ID="ImgDtAlteracao" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImgDtAlteracao"
                                    TargetControlID="txtDtalteracao" Enabled="True">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="row">
                            <div class="panelItem">
                                <p>Grupo</p>
                                <asp:TextBox ID="txtGrupo" runat="server" />
                                <asp:ImageButton ID="ImgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>SubGrupo</p>
                                <asp:TextBox ID="txtSubGrupo" runat="server" />
                                <asp:ImageButton ID="ImgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                            <div class="panelItem">
                                <p>Departamento</p>
                                <asp:TextBox ID="txtDepartamento" runat="server" />
                                <asp:ImageButton ID="ImgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="panelItem">
                                <br />
                                <asp:CheckBox ID="chkImprimeEtiqueta" Text="Só Alterados " runat="server" Checked="false" />
                            </div>

                            <div class="panelItem" style="margin-left: 50px; margin-bottom: 30px;">
                                <br />
                                <asp:CheckBox ID="chkColetor" Text="Coletor" runat="server" Checked="false" AutoPostBack="true"
                                    OnCheckedChanged="chkColetor_CheckedChanged" />
                            </div>
                            <div id="divArquivos" runat="server" visible="false" class="panelItem" style="line-height: 55px;">

                                <asp:ImageButton ID="imgBuscaArquivo" runat="server" Height="25px" ImageUrl="~/img/arquivo-txt.png"
                                    Width="27px" OnClick="imgBuscaArquivo_Click" />

                                <asp:Label ID="lblIdentificarArquivo" runat="server" Text="1º Identificar Arquivo"></asp:Label>
                                <asp:DropDownList ID="ddlArquivos" runat="server" Visible="false">
                                </asp:DropDownList>

                                <asp:ImageButton ID="imgLeArquivo" runat="server" Height="25px" ImageUrl="~/img/arquivo-download.png"
                                    Width="27px" OnClick="imgLeArquivo_Click" />
                                2º Ler Arquivo Selec
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnLimpar" runat="server" Visible="true" CssClass="titulobtn">
                    <asp:ImageButton ID="ImgBtnLimpar" runat="server" Height="25px" ImageUrl="~/img/botao-apagar.png"
                        Width="27px" OnClick="ImgBtnLimpar_Click" />
                    Limpar
                </asp:Panel>
                <asp:Panel ID="pnImprimir" runat="server" Visible="true" CssClass="titulobtn">
                    <asp:ImageButton ID="imgBtnImprimir" runat="server" Height="25px" ImageUrl="../../../img/arquivo-upload.png"
                        Width="27px" OnClick="imgBtnImprimir_Click" />
                    Enviar
                </asp:Panel>
                <div class="gridTableEtiqueta" style="width: 100%;">
                    <asp:GridView ID="gridImpressao" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gridPesquisa_PageIndexChanging"
                        CellPadding="6" ForeColor="#333333" GridLines="Vertical" Width="100%" OnRowCommand="gridImpressao_RowCommand">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:HyperLinkField DataTextField="plu" Text="PLU" Visible="true" HeaderText="PLU" />
                            <asp:HyperLinkField DataTextField="EAN" Text="EAN" Visible="true" HeaderText="EAN" />
                            <asp:HyperLinkField DataTextField="descricao" Text="Descrição" Visible="true" HeaderText="Descrição" />
                            <asp:HyperLinkField DataTextField="preco" Text="Descrição" Visible="true" HeaderText="Preço">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataTextField="und" Text="---" Visible="true" HeaderText="UND" />

                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="excluir"
                                Text="X">
                                <ControlStyle ForeColor="Red" Height="20px" Width="20px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
                    <br />
                    <center>
                <asp:Label ID="lblImpressao" runat="server" Text=""></asp:Label></center>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnFundoFornecedor" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbltituloLista" runat="server" Text="ESCOLHA O FORNECEDOR"></asp:Label></h1></center>
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
                    <asp:ImageButton ID="btnConfirmaFornecedor" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaFornecedor_Click" />
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaFornecedor" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaFornecedor_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel3" runat="server" CssClass="lista">
                <asp:GridView ID="GridFornecedor" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoFornecedorItem" runat="server" GroupName="GrFornecedorItem" />
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
    <asp:ModalPopupExtender ID="modalListafornecedor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundoFornecedor" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <br />
    <br />
    <br />


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
