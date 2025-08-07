<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargaBalanca.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.CargaBalanca" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1 style="margin-left: 40px">Carga Balanças</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>   
   <div class="MenuEtiqueta">
        Modelo Etiqueta
            <asp:RadioButtonList ID="rdoBalanca" runat="server" 
              AutoPostBack="True" Height="82px" Width="932px" onselectedindexchanged="rdoBalanca_SelectedIndexChanged" 
              >
            <asp:ListItem Value="0" Selected="True">Toledo MGV /III/IV</asp:ListItem>
            <asp:ListItem Value="1">Toledo MGV V</asp:ListItem>
            <asp:ListItem Value="2">Filizola</asp:ListItem>
            <asp:ListItem Value="3">Elgin</asp:ListItem>
            <asp:ListItem Value="4">Urano</asp:ListItem>
            <asp:ListItem Value="5">Outra</asp:ListItem>
            </asp:RadioButtonList>
       </div>
        <div class ="MenuEtiquetaDireita">
            <asp:Image ID="imgBalanca" runat="server" Height="263px" 
                ImageUrl="~/modulos/Dispositivos/imgs/balanca0.jpg" Width="250px" />
        </div>           
      
</asp:Content>
