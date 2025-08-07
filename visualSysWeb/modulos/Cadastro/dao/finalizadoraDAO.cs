using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class finalizadoraDAO
    {
        public int Nro_Finalizadora { get; set; }
        public String filial { get; set; }
        public String Codigo_centro_custo { get; set; }
        public String Pagamento { get; set; }
        public String Troco { get; set; }
        public String Finalizadora { get; set; }
        public int Tecla { get; set; }
        public int Ecf { get; set; }
        public bool naoComputa = false;
        public string id_Autorizadora { get; set; } = "";

        public finalizadoraDAO() { }

        public finalizadoraDAO(String nroFinalizadora, User usr)
        {
            String sql = "Select * from  finalizadora where Nro_Finalizadora=" + nroFinalizadora;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public finalizadoraDAO(String finalizadora)
        {
            String sql = "Select * from  finalizadora where finalizadora='" + finalizadora+"'";
            SqlDataReader rs = Conexao.consulta(sql, null, true);
            carregarDados(rs);
        }

        private String dataBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }
        public void carregarDados(SqlDataReader rs)
        {
            try
            {



                if (rs.Read())
                {
                    Nro_Finalizadora = (rs["Nro_Finalizadora"] == null ? 0 : int.Parse(rs["Nro_Finalizadora"].ToString()));
                    filial = rs["filial"].ToString();
                    Codigo_centro_custo = rs["Codigo_centro_custo"].ToString();
                    Pagamento = rs["Pagamento"].ToString();
                    Troco = rs["Troco"].ToString();
                    Finalizadora = rs["Finalizadora"].ToString();
                    Tecla = (rs["Tecla"].ToString().Equals("") ? 0 : int.Parse(rs["Tecla"].ToString()));
                    Ecf = (rs["Ecf"].ToString().Equals("") ? 0 : int.Parse(rs["Ecf"].ToString()));
                    naoComputa = rs["naoComputa"].ToString().Equals("1");
                    id_Autorizadora = rs["id_autorizadora"].ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }
        private void update()
        {
            try
            {
                String sql = "update finalizadora set " +

                           "filial='" + filial + "'," +
                           "Codigo_centro_custo='" + Codigo_centro_custo + "'," +
                           "Pagamento='" + Pagamento + "'," +
                           "Troco='" + Troco + "'," +
                           "Finalizadora='" + Finalizadora + "'," +
                           "Tecla=" + Tecla + "," +
                           "Ecf=" + Ecf +
                           ",naoComputa="+(naoComputa?"1":"0")+
                           ",id_autorizadora="+id_Autorizadora+
                 " where Nro_Finalizadora=" + Nro_Finalizadora;
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        private void insert()
        {
            try
            {
                String sql = " insert into finalizadora(" +
                          "Nro_Finalizadora," +
                          "filial," +
                          "Codigo_centro_custo," +
                          "Pagamento," +
                          "Troco," +
                          "Finalizadora," +
                          "Tecla," +
                          "Ecf" +
                          ",naoComputa"+
                          "id_autorizadora"+
                     " )values (" +
                          Nro_Finalizadora + "," +
                          "'" + filial + "'," +
                          "'" + Codigo_centro_custo + "'," +
                          "'" + Pagamento + "'," +
                          "'" + Troco + "'," +
                          "'" + Finalizadora + "'," +
                          Tecla + "," +
                          Ecf +
                          ","+(naoComputa?"1":"0")+
                          ","+id_Autorizadora+
                " )";
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
        public bool excluir()
        {
            String sql = "delete from finalizadora where Nro_Finalizadora=" + Nro_Finalizadora;
            Conexao.executarSql(sql);
            return true;
        }

        public bool salvar(bool novo)
        {
            if (novo)
            {
                insert();
            }
            else
            {
                update();
            }
            return true;


        }
    }
}/*/*--Campos Form
<td ><p>Nro_Finalizadora</p>
<asp:TextBox ID="txtNro_Finalizadora" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>filial</p>
<asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
 </td>

<td ><p>Codigo_centro_custo</p>
<asp:TextBox ID="txtCodigo_centro_custo" runat="server" ></asp:TextBox>
 </td>

<td ><p>Pagamento</p>
<asp:TextBox ID="txtPagamento" runat="server" ></asp:TextBox>
 </td>

<td ><p>Troco</p>
<asp:TextBox ID="txtTroco" runat="server" ></asp:TextBox>
 </td>

<td ><p>Finalizadora</p>
<asp:TextBox ID="txtFinalizadora" runat="server" ></asp:TextBox>
 </td>

<td ><p>Tecla</p>
<asp:TextBox ID="txtTecla" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Ecf</p>
<asp:TextBox ID="txtEcf" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

*/
/*================================Metodos tela detalhes==========================================
                 : visualSysWeb.code.PagePadrao
  {
                 protected static finalizadoraDAO obj= null ;
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new finalizadoraDAO(usr);
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                        habilitaControles(conteudo); 
                        habilitaControles(cabecalho);
                      }
                      else
                      {
                           if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                           {
                              try
                              {
                                   if (!IsPostBack)
                                   {
                                        String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                        status = "visualizar";
                                        obj = new finalizadoraDAO(clie,usr);
                                        carregarDados();
                                    }
                                    if (status.Equals("visualizar"))
                                    {
                                         EnabledControls(conteudo, false);
                                         EnabledControls(cabecalho, false);
                                    }else{
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                                    }
                                }
                                catch (Exception err)
                                {
                                   lblError.Text = err.Message;                 
                                }
                           }
                       }
                    carregabtn(pnBtn);
                  }
                 
                 private void limparCampos(){
                    LimparCampos(cabecalho);          
                    LimparCampos(conteudo);             
                 }
                 
                 protected override bool campoObrigatorio(Control campo)
                 {// colocar os nomes dos campos obrigarios no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                      for (int i = 0; i < campos.Length; i++)
                      {
                        if (campo.ClientID.Equals(campos[i]))
                        {
                               return true;
                        }
                       }
                       return false;
                 
                 
                 protected override void btnIncluir_Click(object sender, EventArgs e)
                 {
                    incluir(pnBtn);
                 }
                 
                 protected override void btnEditar_Click(object sender, EventArgs e)
                 {
                    editar(pnBtn);
                 }
                 protected override void btnPesquisar_Click(object sender, EventArgs e)
                 {
                 Response.Redirect("nomepaginapesquisa.aspx"); //colocar o endereco da tela de pesquisa
                 }
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     lblError.Text = "n?o ? possivel excluir o registro";//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                     lblError.ForeColor = System.Drawing.Color.Red;
                  }
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     try
                     {
                       carregarDadosObj();
                       obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                       lblError.Text = "Salvo com Sucesso";
                       lblError.ForeColor = System.Drawing.Color.Blue;
                       desabilitaControles(cabecalho);
                       desabilitaControles(conteudo);
                       visualizar(pnBtn);
                     }
                     catch (Exception err)
                     {
                         lblError.Text = err.Message;
                         lblError.ForeColor = System.Drawing.Color.Red;
                     }
                  }
                  
                  protected override void btnCancelar_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("nomepaginapesquisa.aspx");//colocar endereco pagina de pesquisa
                  }
                  protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
                  {
                      switch (e.Item.Value)
                      {
                          case "tab1":
                          MultiView1.ActiveViewIndex = 0;
                          break;
                          case "tab2":
                          MultiView1.ActiveViewIndex = 2;
                          break;
                          case "tab3":
                          MultiView1.ActiveViewIndex = 3;
                          break;
                       }
                   }
/* --Atualizar DaoForm 
      private void carregarDados()
      {
txtNro_Finalizadora.Text=obj.Nro_Finalizadora.ToString();
txtfilial.Text=obj.filial.ToString();
txtCodigo_centro_custo.Text=obj.Codigo_centro_custo.ToString();
txtPagamento.Text=obj.Pagamento.ToString();
txtTroco.Text=obj.Troco.ToString();
txtFinalizadora.Text=obj.Finalizadora.ToString();
txtTecla.Text=obj.Tecla.ToString();
txtEcf.Text=obj.Ecf.ToString();
   }
*/

/* --Atualizar FormDao 
     private void carregarDadosObj()
     {
obj.Nro_Finalizadora=int.Parse(txtNro_Finalizadora.Text);
obj.filial=txtfilial.Text;
obj.Codigo_centro_custo=txtCodigo_centro_custo.Text;
obj.Pagamento=txtPagamento.Text;
obj.Troco=txtTroco.Text;
obj.Finalizadora=txtFinalizadora.Text;
obj.Tecla=int.Parse(txtTecla.Text);
obj.Ecf=int.Parse(txtEcf.Text);
   }
*/









/*================================HTML Pagina Detalhes==========================================
<div class="cabMenu">                  
       <center> <h1>Detalhes do finalizadora</h1></center>                  
</div>                  
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">                  
    </asp:Panel>                  
    <br />              
     <asp:Label ID="lblError" runat="server" Text="" ForeColor=Red></asp:Label>              
                  }
    <div id="cabecalho" runat="server" class="frame" >               
     Coloque aqui os campos do cabe?alho             
        <table>              
              <tr>    
                  <td></td>
              <tr>    
        </table>          
    </div>              
<div class="opcoes">                  
    <asp:Menu ID="tabOpcoes" runat="server" Orientation="Horizontal"               
                 OnMenuItemClick="tabMenu_MenuItemClick"  > 
                  
       <Items>              
           <asp:MenuItem Text="Primeira Tab" Value="tab1" />         
           <asp:MenuItem Text="Segunda Tab" Value="tab2" />         
           <asp:MenuItem Text="Terceira Tab" Value="tab3" />         
       </Items>             
       <StaticMenuStyle CssClass="tab" />              
       <StaticMenuItemStyle CssClass="item" />             
       <staticselectedstyle backcolor="Beige" ForeColor="#465c71" />            
    </asp:Menu>              
</div>                  
                  
<div id="conteudo" runat="server" class="conteudo" enableviewstate="false">                  
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">              
       <asp:View ID="view1" runat="server" >              
           Campos primeira Tab          
           <table>              
                <tr>    
                  <td></td>
                <tr>    
           </table>          
       </asp:View>              
       <asp:View ID="view2" runat="server" >              
           Campos Segunda Tab          
           <table>              
                <tr>    
                  <td></td>
                <tr>    
           </table>          
       </asp:View>              
       <asp:View ID="view3" runat="server" >              
           Campos Terceira Tab          
           <table>              
                <tr>    
                  <td></td>
                <tr>    
           </table>          
       </asp:View>              
    </asp:MultiView>                
</div>                  
*/
/* 
/*================================Metodos tela de Pesq==========================================
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{           
                static DataTable tb;
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable("select * from finalizadora ,usr); //colocar os campos no select que ser?o apresentados na tela 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                       Lblindex.Text = "1/" + gridPesquisa.PageCount;
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/EnderecoPaginaDeDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      String sql = "";
                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = "select * from finalizadora where campoPesquisa1 like '" + txtPESQ1.Value + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      else if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                         sql = "select * from finalizadora where campo2 = '" + txtPESQ2.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                      if (!sql.Equals(""))
                      {
                         try
                         {
                             User usr = (User)Session["User"];
                             tb = Conexao.GetTable(sql, usr);
                             gridPesquisa.DataSource = tb;
                             gridPesquisa.DataBind();
                             lblPesquisaErro.Text = "";
                             Lblindex.Text = "1/" + gridPesquisa.PageCount;
                         }
                         catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
                      }
                      else
                      {
                          lblPesquisaErro.Text = "Informe um valor para efetuar a pesquisa ";
                      }
                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                  protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
                  {
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.PageIndex = e.NewPageIndex;
                    Lblindex.Text = (e.NewPageIndex+1)+"/" + gridPesquisa.PageCount;
                    gridPesquisa.DataBind();
                  }
                  
/*================================html tela de Pesq==========================================
                  
   <center><h1>finalizadora</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" visible="false">           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>CAMPO DE PESQUISA 1</p>   
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox><input type="text" id="" runat="server" size=15/>  
                </td>  
                <td>  
                   <p>CAMPO DE PESQUISA 2</p>  
                   <input type="text" id="txtPESQ2" runat="server" size=50/>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="8"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
 
            <asp:HyperLinkField DataTextField="Nro_Finalizadora" Text="Nro_Finalizadora" Visible="true" 
            HeaderText="Nro_Finalizadora" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
            HeaderText="filial" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Codigo_centro_custo" Text="Codigo_centro_custo" Visible="true" 
            HeaderText="Codigo_centro_custo" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Pagamento" Text="Pagamento" Visible="true" 
            HeaderText="Pagamento" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Troco" Text="Troco" Visible="true" 
            HeaderText="Troco" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Finalizadora" Text="Finalizadora" Visible="true" 
            HeaderText="Finalizadora" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Tecla" Text="Tecla" Visible="true" 
            HeaderText="Tecla" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 

 
            <asp:HyperLinkField DataTextField="Ecf" Text="Ecf" Visible="true" 
            HeaderText="Ecf" DataNavigateUrlFormatString="~/modulos/enderecopagina.aspx?parametro={0}" // enderecoPagina=colocar endere?o da pagina de detalhes
                  DataNavigateUrlFields="campoIndex" />  //campoIndex= colocar o campo Index que fara a pesquisar
 


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
                  
                  
*/

