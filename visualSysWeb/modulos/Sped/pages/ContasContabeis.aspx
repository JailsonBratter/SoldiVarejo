<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContasContabeis.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.ContasContabeis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Contas Contabil</h1></center>

    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server">
        <div class="btnImprimirDireita">
            Limpar Filtros
                        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
                            Style="width: 20px" />
        </div>
        <div class="container row">
            <div class="panelItem">
                <p>
                    De
                </p>
                <asp:TextBox ID="txtDataDe" runat="server" OnTextChanged="txtDataDe_TextChanged"
                    AutoPostBack="true" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Ate
                </p>
                <asp:TextBox ID="txtDataAte" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                    TargetControlID="txtDataAte">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Codigo
                </p>
                <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
            </div>

            <div class="panelItem">
                <p>
                    Descrição
                </p>
                <asp:TextBox ID="txtDescricao" runat="server" Width="200px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    CNPJ
                </p>
                <asp:TextBox ID="txtCnpj" runat="server" Width="150px" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
            </div>

            <div class="panelItem">
                <p>
                    Tipo
                </p>
                <asp:DropDownList ID="ddlTipo" runat="server" Height="21px">

                    <asp:ListItem Value="" Text="TODOS"></asp:ListItem>
                    <asp:ListItem Value="A" Text="A-Analítica(conta)"></asp:ListItem>
                    <asp:ListItem Value="S" Text="S-Sintética(grupo e contas)"></asp:ListItem>

                </asp:DropDownList>
            </div>
            <div class="panelItem">
                <p>
                    Natureza
                </p>
                <asp:DropDownList ID="ddlNatureza" runat="server" Height="21px">
                    <asp:ListItem Value="" Text="TODAS"></asp:ListItem>
                    <asp:ListItem Value="01" Text="01-Contas de Ativo"></asp:ListItem>
                    <asp:ListItem Value="02" Text="02-Contas de Passivo"></asp:ListItem>
                    <asp:ListItem Value="03" Text="03-Patrimônio líquido"></asp:ListItem>
                    <asp:ListItem Value="04" Text="04-Contas de resultado"></asp:ListItem>
                    <asp:ListItem Value="05" Text="05-Contas de compensação"></asp:ListItem>
                    <asp:ListItem Value="09" Text="09-Outras"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="panelItem">
                <p>
                    Entrada/Saida
                </p>
                <asp:DropDownList ID="ddlEntradaSaida" runat="server" Height="21px">
                    <asp:ListItem Value="" Text="TODAS"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Entrada"></asp:ListItem>
                    <asp:ListItem Value="0" Text="Saida"></asp:ListItem>

                </asp:DropDownList>

            </div>
        </div>

    </div>
    <div class="row">
        <div class="panelItem">
            <h1>
                <asp:Label ID="lblQtdRegistros" runat="server" Text=""></asp:Label></h1>
        </div>

    </div>


    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" OnSorting="gridPesquisa_Sorting">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>

                <asp:HyperLinkField DataTextField="cod_conta" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="codigo" />
                <asp:HyperLinkField DataTextField="descricao" Text="---" Visible="true" HeaderText="Descrição"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="codigo" />
                <asp:HyperLinkField DataTextField="strTipo" Text="---" Visible="true" HeaderText="Tipo"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="strTipo" />
                <asp:HyperLinkField DataTextField="strNatureza" Text="---" Visible="true" HeaderText="Natureza"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="strNatureza" />
                <asp:HyperLinkField DataTextField="cnpj_Estabelecimento" Text="---" Visible="true" HeaderText="Cnpj"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="strNatureza" />
                <asp:HyperLinkField DataTextField="strEntradaSaida" Text="---" Visible="true" HeaderText="Entrada/Saida"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="strEntradaSaida" />
                <asp:HyperLinkField DataTextField="strData" Text="---" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="ContasContabeisDetalhes.aspx?codigo={0}&cnpj={1}"
                    DataNavigateUrlFields="cod_conta,cnpj_Estabelecimento" SortExpression="strEntradaSaida" />

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
</asp:Content>
