<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cotacao.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.Cotacao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
          
   <center><h1>Cotação</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" >           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>Cotacao</p>   
                <asp:TextBox ID="txtCotacao" runat="server" Width="80px" ></asp:TextBox> 
                </td>  
                <td>  
                   <p>Descrição</p>  
                   <asp:TextBox ID="txtDescricao" runat="server"  Width="200px"> </asp:TextBox>
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDataDe" runat="server" OnTextChanged="txtDataDe_TextChanged"
                        AutoPostBack="true" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtDataAte" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                        TargetControlID="txtDataAte">
                    </asp:CalendarExtender>
                </td>  
                <td>
                    <p>Status</p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                       <asp:ListItem></asp:ListItem>
                        <asp:ListItem>ABERTO</asp:ListItem>
                        <asp:ListItem>FECHADA</asp:ListItem>
                        <asp:ListItem>FINALIZADA</asp:ListItem>
                    </asp:DropDownList>

                </td>
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 ForeColor="#333333"  GridLines="Vertical" 
                 > 
                  <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Cotacao" Text="----" Visible="true" 
                    HeaderText="Cotacao" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="cotacao" />  
                   
                    <asp:HyperLinkField DataTextField="descricao" Text="----" Visible="true" 
                    HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="cotacao" /> 

                   <asp:HyperLinkField DataTextField="Data" Text="----" Visible="true" 
                    HeaderText="Data" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="cotacao" /> 


                    <asp:HyperLinkField DataTextField="Status" Text="----" Visible="true" 
                    HeaderText="Status" DataNavigateUrlFormatString="~/modulos/estoque/pages/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="cotacao" /> 



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
