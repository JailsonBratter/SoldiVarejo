<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Agenda.aspx.cs" Inherits="visualSysWeb.modulos.PetShop.pages.Agenda" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/Agenda.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divAgenda" runat="server" style="width: 1000px; height: 900px; margin: auto;
        overflow: auto;">
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        <div class="row">
            <asp:ImageButton ID="imgConfig" CssClass="btnImprimirDireita" runat="server" ImageUrl="../img/Config.png"
                OnClick="imgConfig_Click" />
        </div>
        <div style="width: 300px; height: 90%; float: left;">
            <asp:TextBox ID="txtData" runat="server" OnTextChanged="txtData_TextChanged" OnKeyPress="javascript:return formataData(this,event);"
                AutoPostBack="true" Width="100px"></asp:TextBox>
            <asp:ImageButton ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server"
                Height="15px" />
            <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="imgDeCalendario"
                TargetControlID="txtData">
            </asp:CalendarExtender>
            <br />
            <br />
          
            <br />
            <br />
            <br />
            <div class="tdLivre" style="width: 80px; height: 20px; text-align: center">
                Livre
            </div>
            <br />
            <div class="tdOcupado" style="width: 80px; height: 20px; text-align: center">
                Ocupado
            </div>
            <br />
            <div class="tdOcupadoCliente" style="width: 80px; height: 20px; text-align: center">
                Ag-Cliente
            </div>
        </div>
        <asp:Panel ID="pnAgenda" runat="server" CssClass="Agenda">
        </asp:Panel>
        <asp:Panel ID="PnConfiguraCoes" runat="server" CssClass="modalForm" Style="display: none;
            height: 200px;">
            <div class="frame">
                <div class="btnImprimirDireita">
                    <asp:ImageButton ID="imgFechar" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />Fechar
                </div>
                <div class="divFrame">
                    <h3>
                        <asp:Label ID="Label18" runat="server" Text="Configurações Agenda" CssClass="cabMenu"></asp:Label>
                    </h3>
                    Inicio:
                    <asp:TextBox ID="txtHorarioInicio" runat="server" CssClass="hora" type="time"></asp:TextBox>
                    Fim:<asp:TextBox ID="txtHorarioFim" runat="server" CssClass="hora" type="time"></asp:TextBox>
                    Intervalo<asp:TextBox ID="txtIntervalo" runat="server" Width="80px"></asp:TextBox>
                    <asp:Button ID="btnMontar" runat="server" Text="Montar" OnClick="btnMontar_Click" />
                </div>
            </div>
        </asp:Panel>
        <asp:ModalPopupExtender ID="modalConfiguracao" runat="server" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupControlID="PnConfiguraCoes" TargetControlID="Label18">
        </asp:ModalPopupExtender>
    </div>
    <div class="gridTable">
        <asp:Panel ID="pnDetalhesAgenda" runat="server" Visible="false" CssClass="modalFormTelaInteiraConfirma">
            <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
                <div style="width: 600px; float: left;">
                    <h2>
                        Detalhes do Agendamento</h2>
                    <asp:Label ID="lblErrorAgendamento" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <div id="divImprimir" runat="server" class="Paneldireita" style="float: left;">
                    <asp:ImageButton ID="imgImprimir" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                        Width="25px" OnClick="imgImprimir_Click" />Imprimir
                </div>
                <div class="Paneldireita">
                    <asp:ImageButton ID="imgBtnFechar" ImageUrl="~/img/cancel.png" runat="server" Width="20px"
                        OnClick="imgBtnFechar_Click" />
                    Fechar
                </div>
            </div>
            <br />
            <div class="panelDefault">
                <div class="panelDefault" style="width: 55%; margin-right: 5px;">
                    <div class="row">
                        <div class="panelItem">
                            <asp:ImageButton ID="btnConfirmaAgendamento" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmar_Click" />
                            Confirmar
                        </div>
                        <div class="panelItem">
                            <asp:ImageButton ID="btnCancelarAgendamento" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelarAgendamento_Click" />
                            Cancelar Agendamento
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Pedido</p>
                            <asp:TextBox ID="txtCodPedido" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Usuario</p>
                            <asp:TextBox ID="txtusuario" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Nome</p>
                            <asp:TextBox ID="txtNome" runat="server" Width="430px" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Cod</p>
                            <asp:TextBox ID="txtCodCliente" runat="server" Width="80px" CssClass="numero" AutoPostBack="true"
                                OnTextChanged="txtCodCliente_TextChanged"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Cliente</p>
                            <asp:TextBox ID="txtCliente" runat="server" Width="340px" AutoPostBack="true" OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="true"
                                TargetControlID="txtCliente" ServiceMethod="GetNomesClientes" ServicePath="Agenda.aspx"
                                MinimumPrefixLength="1" CompletionInterval="0" CompletionSetCount="20" EnableCaching="false"
                                BehaviorID="AutoCompleteExCli" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                <Animations>
                                <OnShow>
                                    <Sequence>
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoCompleteExCli');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".0">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExCli')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".0">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteExCli')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                                </Animations>
                            </asp:AutoCompleteExtender>
                            <asp:ImageButton ID="imgCliente" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Style="width: 15px" OnClick="btnImg_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Nome Pet</p>
                            <asp:DropDownList ID="ddlPetsCliente" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="imgPetCliente" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" Style="width: 15px" OnClick="btnImg_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Data</p>
                            <asp:TextBox ID="txtDtAgendamento" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Inicio</p>
                            <asp:DropDownList ID="ddlInicio" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <p>
                                Fim</p>
                            <asp:DropDownList ID="ddlFim" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <br />
                            <asp:CheckBox ID="chkDelivery" runat="server" Text="Delivery" />
                        </div>
                    </div>
                    <div class="row">
                        <h3>
                            Retirada
                        </h3>
                        <div class="panelItem">
                            <p>
                                Hora
                            </p>
                            <asp:TextBox ID="txtHoraRetirada" runat="server" CssClass="hora" MaxLength="5"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Funcionario</p>
                            <asp:TextBox ID="txtFuncionarioRetira" runat="server" Width="200px"></asp:TextBox>
                            <asp:ImageButton ID="imgFuncionarioRetira" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" Style="width: 15px" OnClick="btnImg_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <h3>
                            Entrega
                        </h3>
                        <div class="panelItem">
                            <p>
                                Prevista</p>
                            <asp:TextBox ID="txtHoraEntregaPrevista" runat="server" CssClass="hora" MaxLength="5"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Real</p>
                            <asp:TextBox ID="txtEntregaReal" runat="server" CssClass="hora" MaxLength="5"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Funcionario</p>
                            <asp:TextBox ID="txtFuncionarioEntrega" runat="server" Width="150px"></asp:TextBox>
                            <asp:ImageButton ID="imgFuncionarioEntrega" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" Style="width: 15px" OnClick="btnImg_Click" />
                            <a id="lkMesmo" runat="server" href="#" onclick="javascript:mesmoFuncionario()">Mesmo
                                da Retirada</a>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Km Saida</p>
                            <asp:TextBox ID="txtkmSaida" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Km Chegada</p>
                            <asp:TextBox ID="txtKmChegada" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="panelDefault" style="width: 30%; margin-left: 0px;">
                    <div class="row">
                        <div class="panelItem" style="width: 430px;">
                            <asp:Button ID="btnSelecionaCurso" runat="server" Text="Escolher Serviço" OnClick="btnSelecionaServico_Click"
                                Width="100%" />
                            <asp:ListBox ID="lsServicos" runat="server" Width="100%" Height="85px"></asp:ListBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Observações</p>
                            <asp:TextBox ID="txtObs" runat="server" Width="430px" Height="200px" CssClass="sem" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Observações Veterinario</p>
                            <asp:TextBox ID="txtObsVeterinario" runat="server" Width="430px" CssClass="sem" Height="200px" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Panel ID="PnPesquisa" runat="server" CssClass="modalForm" DefaultButton="ImgPesquisaLista"
                Style="display: none; top: 10px;">
                <div class="frame" style="width: 95%;">
                    <center><h1>
                            <asp:Label ID="lblTitulo" runat="server" Text="" CssClass="cabMenu"></asp:Label>
                            </h1>
                        </center>
                    <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                        Filtrar
                        <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                        <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="ImgPesquisaLista_Click" />
                    </div>
                    <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                        <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                            <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaLista_Click" />
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
                </div>
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalPesquisa" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="PnPesquisa" TargetControlID="lblTitulo">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
                <asp:Label ID="Label3" runat="server" Text="Confirma o Cancelamento do Agendamento?"
                    CssClass="cabMenu"></asp:Label>
                <table class="frame" style="border: none;">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaExclusao_Click" />
                            <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaExclusao_Click" />
                            <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label3">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnServico" runat="server" CssClass="modalForm" Style="display: none;
                top: 10px;">
                <div class="frame" style="width: 95%;">
                    <br />
                    <div class="row" style="width: 70%; float: left; margin-left: 20px;">
                        <center>
                <h1>
                            <asp:Label ID="lblTituloServico" runat="server" Text="Selecione os Serviços" CssClass="cabMenu"></asp:Label>
                            </h1>
                        </center>
                    </div>
                    <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;
                        margin-left: 30px;">
                        <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                            <asp:ImageButton ID="btnConfirmaServicos" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaServicos_Click" />
                            <asp:Label ID="Label7" runat="server" Text="Confirma"></asp:Label>
                        </div>
                        <div class="row">
                            <asp:ImageButton ID="btnCancelaServicos" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaServicos_Click" />
                            <asp:Label ID="Label8" runat="server" Text="Cancela"></asp:Label>
                        </div>
                    </div>
                    <asp:Panel ID="Panel3" runat="server" CssClass="lista" Style="height: 350px;">
                        <asp:GridView ID="gridServicos" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkServico" runat="server" />
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
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalServicos" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="pnServico" TargetControlID="lblTituloServico">
            </asp:ModalPopupExtender>
            
        </asp:Panel>
    </div>
    <asp:Panel ID="pnConfirmaImprimir" runat="server" CssClass="frameModal" Style="display: none">
                <asp:Label ID="Label6" runat="server" Text="Gostaria de Imprimir o Agendamento?"
                    CssClass="cabMenu"></asp:Label>
                <table class="frame" style="border: none;">
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgBtnSimImprime" runat="server" ImageUrl="~/img/confirm.png" Width="25px"
                                OnClick="imgBtnSimImprime_Click" />
                            <asp:Label ID="Label9" runat="server" Text="Sim"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgBtnNaoImprime" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                                OnClick="imgBtnNaoImprime_Click" />
                            <asp:Label ID="Label10" runat="server" Text="Não"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalImprime" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="pnConfirmaImprimir" TargetControlID="Label6">
            </asp:ModalPopupExtender>

</asp:Content>
