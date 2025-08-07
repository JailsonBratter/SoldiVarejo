<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OperadoresDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.OperadoresDetalhes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do Operador</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                 <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtID_Operador" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="ImgOperador" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="ImgOperador_Click" Style="width: 15px" />
                </td>
               <td>
                    <p>
                        Nome</p>
                    <asp:TextBox ID="txtNome" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                </td>
               <td>
                    <p>
                        Senha</p>
                    <asp:TextBox TextMode="Password" ID="txtSenha" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <table>
            <tr>
                
                
                <td>
                    <p>
                        Nivel de Acesso</p>
                    <asp:DropDownList ID="ddlNivel" runat="server">
                        <asp:ListItem Value="1">Operador</asp:ListItem>
                        <asp:ListItem Value="3">Gerente</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Cargo</p>
                    <asp:TextBox ID="txtCargo" runat="server"  Width="150px"  MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkOpCaixa" runat="server"  Text = "Operador de Caixa"/>
                    
                </td>
                
            </tr>
        </table>
    </div>

         <asp:Panel ID="pnConfirma" runat="server"  CssClass="frameModal" Style="display:none">         
           <asp:Label ID="Label1" runat="server" Text="Confirma Exclusão" CssClass="cabMenu"></asp:Label>         
             <table class="frame">          
                  <tr>     
                      <td>             
                             <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png" 
                                     Width="25px" onclick="btnConfirmaExclusao_Click"  /> 
                                     <asp:Label ID="Label2" runat="server" Text="Confirma" ></asp:Label>
                      </td>
                      <td>
                                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png" 
                                     Width="25px" onclick="btnCancelaExclusao_Click"  /> 
                                     <asp:Label ID="Label3" runat="server" Text="Cancela" ></asp:Label>
                      </td>
                  </tr>
              </table>     
         </asp:Panel>       
         <asp:ModalPopupExtender ID="modalPnConfirma" runat="server"
                    BackgroundCssClass="modalBackground"
                    DropShadow="true"
                    PopupControlID="pnConfirma"
                    TargetControlID="Label1"

                >
    
                </asp:ModalPopupExtender>  

</asp:Content>
