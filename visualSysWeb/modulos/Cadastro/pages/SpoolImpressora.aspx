<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpoolImpressora.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.SpoolImpressora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center><h1>Spool_impressoras</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />
       <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
                      
       <div class="filter" id="filtrosPesq" runat="server">           
         <table>           
            <tr>           
                <td>           
                <p>id_trm</p>   
                <asp:TextBox ID="txtIdTrm" runat="server" Width="80px" ></asp:TextBox>
                </td>  
                <td>  
                   <p>Descrição</p>  
                   <asp:TextBox ID="txtDescricao" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 ForeColor="#333333" GridLines="Vertical"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                 <Columns> 
                    <asp:HyperLinkField DataTextField="id_trm" Text="id_trm" Visible="true" 
                    HeaderText="id_trm" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/SpoolImpressoraDetalhes.aspx?idTrm={0}" 
                        DataNavigateUrlFields="id_trm" />  

                    <asp:HyperLinkField DataTextField="Descricao" Text="Descricao" Visible="true" 
                    HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/SpoolImpressoraDetalhes.aspx?idTrm={0}" 
                        DataNavigateUrlFields="id_trm" /> 

                    <asp:HyperLinkField DataTextField="impressora_remota" Text="impressora_remota" Visible="true" 
                    HeaderText="impressora_remota" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/SpoolImpressoraDetalhes.aspx?idTrm={0}" 
                        DataNavigateUrlFields="id_trm" />  
 

                    <asp:HyperLinkField DataTextField="Ativo" Text="Ativo" Visible="true" 
                    HeaderText="Ativo" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/SpoolImpressoraDetalhes.aspx?idTrm={0}" 
                        DataNavigateUrlFields="id_trm" />  


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
