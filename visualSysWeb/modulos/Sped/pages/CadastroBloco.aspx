<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CadastroBloco.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.CadastroBloco" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Cadastro de Blocos SPED</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="filter" id="filtrosPesq" runat="server" style="margin-bottom: 0px;">
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div class="panelDefault" style="margin-top: 0px;">
                <div class="row">
                    <div class="panelItem" style="margin-left:30px; ">
                        <p>
                            Nome Bloco</p>
                        <asp:TextBox ID="txtNomeBloco" runat="server" Width="200px" ></asp:TextBox>
                    </div>
                </div>
            </div>
     </div>
 
     <asp:Panel ID="pnBlocos" runat="server"   CssClass="BlocoPag" >

    </asp:Panel>
    

</asp:Content>
