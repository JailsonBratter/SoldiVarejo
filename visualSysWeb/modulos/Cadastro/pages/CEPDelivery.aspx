<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CEPDelivery.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CEPDelivery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center><h1>CEP Delivery</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server">           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>CEP</p>   
                <asp:TextBox ID="txtCEP" runat="server" Width="100px" ></asp:TextBox>
                </td>  
                 <td>           
                <p>Logradouro</p>   
                <asp:TextBox ID="txtLogradouro" runat="server" Width="300px" ></asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="CEP" Text="CEP" Visible="true" 
                    HeaderText="CEP" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    <asp:HyperLinkField DataTextField="Logradouro" Text="Logradouro" Visible="true" 
                    HeaderText="Logradouro" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    <asp:HyperLinkField DataTextField="Bairro" Text="Bairro" Visible="true" 
                    HeaderText="Bairro" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    <asp:HyperLinkField DataTextField="Cidade" Text="Cidade" Visible="true" 
                    HeaderText="Cidade" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    <asp:HyperLinkField DataTextField="UF" Text="UF" Visible="true" 
                    HeaderText="UF" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    
                    <asp:HyperLinkField DataTextField="num_inicio" Text="num_inicio" Visible="true" 
                    HeaderText="Numero Inicio" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  

                    <asp:HyperLinkField DataTextField="num_fim" Text="num_fim" Visible="true" 
                    HeaderText="Numero Fim" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?cep={0}" 
                        DataNavigateUrlFields="CEP" />  


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
