<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IpDeviceDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.IpDeviceDetalhes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do ipDevice</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="frame">
    </div>
    <div id="conteudo" runat="server" class="conteudo" style="width:100%; height:200px; margin-top:80px; ">
        <div class="panel" style="  margin-left: 20%; width:50%; float:left;">
            <div class="row">
                <div class="panelItem">
                    <p>
                        id</p>
                    <asp:TextBox ID="txtid" runat="server" CssClass="numero" Width="70px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        ip</p>
                    <asp:TextBox ID="txtip" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        tipo</p>
                    <asp:TextBox ID="txttipo" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Balanca_serial</p>
                    <asp:TextBox ID="txtBalanca_Serial" runat="server"></asp:TextBox>
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>
