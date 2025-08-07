<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CargaPDV.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.CargaPDV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <center>
        <h1 style="margin-left: 40px">
            Carga PDV</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="cargaPDVDireita">
        Tipo de PDV
        <asp:RadioButtonList ID="rdoPDV" runat="server" AutoPostBack="True" Height="25px"
            OnSelectedIndexChanged="rdoPDV_SelectedIndexChanged">
            <asp:ListItem Value="0" Selected="True">Soldi PDV</asp:ListItem>
            <asp:ListItem Value="1">Zanthus</asp:ListItem>
            <asp:ListItem Value="2">Busca Preço</asp:ListItem>

        </asp:RadioButtonList>
    </div>
   <asp:Panel ID="pnTipoZanthus" runat="server"   CssClass=leftCol >
         <asp:RadioButtonList ID="rdoZanthusTipoArquivo" runat="server"  AutoPostBack="True" Height="25px" 
            OnSelectedIndexChanged="rdoZanthusTipoArquivo_SelectedIndexChanged" RepeatDirection="Vertical">
            <asp:ListItem Value="0" Selected="True">Mercadoria</asp:ListItem>
            <asp:ListItem Value="1">Clientes</asp:ListItem>
        </asp:RadioButtonList>    
     </asp:Panel>
    
     <asp:Panel ID="pnPDV" runat="server" CssClass="cargaPDVDireita">
        <asp:CheckBox ID="chkGerarComCPF" runat="server" Text="Gerar CPF como Codigo Cliente"
            Checked="false" visible ="false"/>
        <asp:CheckBox ID="chkAlterados" runat="server" Text="Gerar somente os Alterados"
            Checked="true"  AutoPostBack="true" 
             oncheckedchanged="chkAlterados_CheckedChanged" />
        <br />
        <asp:CheckBox ID="chkDesmarcarAlteracoes" runat="server" Text="Desmarcar Alterações após a carga"
            Checked="false" />
        <br />
        <asp:CheckBox ID="chkGeraBuscaPreco" runat="server" Text="Gerar Busca Preço"
            Checked="false" Visible="false" Enabled="false" />
        
        <br />
        
        <br />
        <br />
        <br />
        <asp:Image ID="imgPDV" runat="server"  ImageUrl="~/modulos/Dispositivos/imgs/PDV0.jpg"
            Width="200px" />
    </asp:Panel>
    

    <div id="divPdvsCadastrados" runat="server">
        <div class="cargaPDVDireita">
        <br />


            <asp:GridView class="gridTable" ID="gridPesquisa" runat="server" 
                AutoGenerateColumns="False" GridLines="Vertical"
                CellPadding="5" ForeColor="#333333" >
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Caixa" HeaderText="PDV" />
                    <asp:BoundField DataField="Diretorio_Carga" HeaderText="Diretorio destino" ItemStyle-Width="200px"/>
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
        <div id="divArquivos" runat="server" class="cargaPDVDireita">
            <p>Arquivos pendentes de processamento</p>
            <asp:Button ID="btnReenviar" runat="server" Text="Reenviar" Width="80px" 
                onclick="btnReenviar_Click"  />
             <asp:Button ID="btnLimpar" runat="server" Text="Desprezar" Width="80px" 
                onclick="btnLimpar_Click"/>
            <asp:ListBox ID="ltbArquivosPendentes" runat="server" Height="246px" 
                Width="212px" Enabled="False">
            </asp:ListBox>
        </div>
    </div>
  
</asp:Content>
