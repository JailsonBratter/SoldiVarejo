<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CEP.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CEP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <center>
        <h1>
            Consulta CEP</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtns" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br />
<div class="frameConteudo"  style=" height:600px;">
<div class="panelDefault" >
        <div>Pesquisa: 
                <asp:TextBox ID="txtPesquisa" runat="server" Width="400px"></asp:TextBox></div>
</div>

<div id="divResultados" class="gridTable" runat="server" style="width:100%; height:500px; overflow:auto;">
         <asp:GridView ID="gridCEP" runat="server" ForeColor=" #333333" GridLines="Vertical"
                AutoGenerateColumns="False" >
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="CEP" HeaderText="CEP" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>

                    <asp:BoundField DataField="logradouro" HeaderText="Logradouro" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Bairro" HeaderText="Bairro" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Cidade" HeaderText="Cidade" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="UF" HeaderText="Uf" HeaderStyle-HorizontalAlign="Center">
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
</div>
</asp:Content>
