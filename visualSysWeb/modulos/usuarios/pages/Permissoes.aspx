<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Permissoes.aspx.cs" Inherits="visualSysWeb.modulos.usuarios.pages.Permissoes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<center><h1>PERMISSÕES</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" >           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>Usuario</p>   
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox></asp:TextBox>  
                </td>  
                <td>  
                   <p>Nome</p>  
                   <asp:TextBox ID="txtPESQ2" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="6"  
                 ForeColor="#333333" GridLines="Vertical"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/usuarios/pages/PermissoesDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="id" />  

                    <asp:HyperLinkField DataTextField="usuario" Text="usuario" Visible="true" 
                    HeaderText="usuario" DataNavigateUrlFormatString="~/modulos/usuarios/pages/PermissoesDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="id" />  

                    <asp:HyperLinkField DataTextField="nome" Text="nome" Visible="true" 
                    HeaderText="nome" DataNavigateUrlFormatString="~/modulos/usuarios/pages/PermissoesDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="id" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/usuarios/pages/PermissoesDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="id" />  

                    <asp:HyperLinkField DataTextField="adm" Text="adm" Visible="true" 
                    HeaderText="adm" DataNavigateUrlFormatString="~/modulos/usuarios/pages/PermissoesDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="id" />  


 



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
           <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>       
        </div>        

</asp:Content>
