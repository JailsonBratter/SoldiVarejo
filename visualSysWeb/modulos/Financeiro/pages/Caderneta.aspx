<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Caderneta.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.Caderneta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center><h1>CADERNETA</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" >           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>Cliente</p>   
                <asp:TextBox ID="txtCliente" runat="server" ></asp:TextBox> 
                </td> 
                <td>
                <p>CNPJ/CPF</p>   
                <asp:TextBox ID="txtCNPJ" runat="server"  ></asp:TextBox> 
                
                </td>
                <td><p>Situação</p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="">TODOS</asp:ListItem>
                        <asp:ListItem Value="OK">OK</asp:ListItem>
                        <asp:ListItem Value="ATRASO">ATRASO</asp:ListItem>
                        <asp:ListItem Value="BLOQUEADO">BLOQUEADO</asp:ListItem>
                        <asp:ListItem Value="CANCELADO">CANCELADO</asp:ListItem>

                    </asp:DropDownList>
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
                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="---" Visible="true" 
                    HeaderText="Codigo Cliente" DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente" />  

                     <asp:HyperLinkField DataTextField="CNPJ" Text="---" Visible="true" 
                    HeaderText="CNPJ/CPF" DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente" />  


                    <asp:HyperLinkField DataTextField="Nome_Cliente" Text="---" Visible="true" 
                    HeaderText="Nome Cliente"  DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente"/>  

                    <asp:HyperLinkField DataTextField="Situacao" Text="---" Visible="true" 
                    HeaderText="Situação"  DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente" />  

                    <asp:HyperLinkField DataTextField="Limite_Credito" Text="---" Visible="true" 
                    HeaderText="Limite Credito"  DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente" />  

                    <asp:HyperLinkField DataTextField="Utilizado" Text="---" Visible="true" 
                    HeaderText="Utilizado"  DataNavigateUrlFormatString="~/modulos/financeiro/pages/CadernetaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="Codigo_cliente" />  


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
