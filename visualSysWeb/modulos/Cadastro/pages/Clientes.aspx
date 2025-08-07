<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Clientes.aspx.cs" Inherits="visualSysWeb.Cadastro.Clientes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Clientes</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtns" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo
                    </p>

                    <asp:TextBox ID="txtCod_cliente" runat="server" MaxLength="11" CssClass="numero" Width="70px"></asp:TextBox>

                </td>
                <td>
                    <p>
                        Cliente
                    </p>
                    <input type="text" id="txtcliente" runat="server" size="30" />
                </td>
                <td>
                    <p>
                        CNPJ / CPF
                    </p>
                    <input type="text" id="txtCnpj" runat="server" onkeypress="javascript:return numeros(this,event);" size="15" />
                </td>
                <td>
                    <p>
                        Data Cadastro
                    </p>
                    <asp:TextBox ID="txtDataCadastro" runat="server" CssClass="DATA" MaxLength="10" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server"
                        Height="15px" />
                    <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtDataCadastro">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Cidade
                    </p>
                    <asp:TextBox ID="txtCidade" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CEP
                    </p>
                    <asp:TextBox ID="txtCEP" runat="server" Width="80px" OnKeyPress="javascript:return numeros(this,event);" MaxLength="8"></asp:TextBox>
                </td>
                <td>
                    <div class="panelItem">
                        <p>Tabela Preço</p>
                        <asp:TextBox ID="txtTabPreco" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnTabPreco" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg" OnClick="ImgBtnTabPreco_Click" />
                    </div>
                </td>
                <td>
                    <div class="panelItem" cssclass="sem">
                        <br />
                        <asp:CheckBox ID="chkInativo" runat="server" Text="Inativo" AutoPostBack="true" ForeColor="Red"
                            Font-Bold="True" OnCheckedChanged="chk_CheckedChanged" />
                    </div>
                </td>
                <td>
                    <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkContaAssinada" Text="Conta Assinada" runat="server" />

                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivPet" runat="server">
                        <p>
                            Nome Pet
                        </p>
                        <asp:TextBox ID="txtNomePet" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    
                </td>
            </tr>

        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <div class="panelItem" style="margin-bottom: 0px;">
            <h1>
                <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
        </div>
        <div class="panelItem" style="margin-top: 0px; margin-left: 10px;">
            <asp:ImageButton ID="imgBtnSalvarInativos" runat="server" Visible="false" ImageUrl="../../../img/confirm.png"
                Width="30px" OnClick="imgBtnSalvarInativos_Click" />
            <asp:Label ID="lblSalvarInativos" runat="server" Text="Ativar Clientes Selecionados"
                Visible="false" Font-Size="Medium"></asp:Label>
        </div>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridClientes" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="True" OnSorting="gridPesquisa_Sorting">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField Visible="false">
                    <HeaderTemplate>
                        <%--<asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                Checked="true" />--%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataTextField="codigo_cliente" Text="codigo" Visible="true" HeaderText="Codigo Cliente"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="codigo_cliente" />
                <asp:HyperLinkField DataTextField="Nome_cliente" Text="Nome Cliente" Visible="true"
                    HeaderText="Nome Cliente" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="Nome_cliente" />
                <asp:HyperLinkField DataTextField="CNPJ" Text="CNPJ" Visible="true" HeaderText="CNPJ"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="CNPJ" />
                <asp:HyperLinkField DataTextField="Data_Cadastro" Text="-----" Visible="true" HeaderText="Data Cadastro"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="DataCadOrdem" />
                <asp:HyperLinkField DataTextField="CIDADE" Text="-----" Visible="true" HeaderText="Cidade"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="CIDADE" />
                <asp:HyperLinkField DataTextField="CEP" Text="-----" Visible="true" HeaderText="CEP"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="CEP" />

                <asp:HyperLinkField DataTextField="NRO" Text="-----" Visible="true" HeaderText="Nro"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="nro" />

                <asp:HyperLinkField DataTextField="DtVencimento" Text="-----" Visible="true" HeaderText="Vencimento"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="DtVencimentoOrdem" />
                <asp:HyperLinkField DataTextField="Dias" Text="-----" Visible="true" HeaderText="Dias"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ClienteDetalhes.aspx?cod={0}"
                    DataNavigateUrlFields="codigo_cliente" SortExpression="DiasOrdem" />
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
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
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
    <asp:ModalPopupExtender ID="ModalFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
</asp:Content>
