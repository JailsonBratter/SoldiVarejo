using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class PrecificacaoItensOdemPlu : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            precificacao_itensDAO obX = (precificacao_itensDAO)x;
            precificacao_itensDAO obY = (precificacao_itensDAO)y;

            int inx = 0;
            int.TryParse(obX.plu, out inx);
            int inY = 0 ;
            int.TryParse(obY.plu, out inY);
            return ((new CaseInsensitiveComparer()).Compare(inx, inY));
        }
    }
    public class PrecificacaoItensOdemDescricao : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            precificacao_itensDAO obX = (precificacao_itensDAO)x;
            precificacao_itensDAO obY = (precificacao_itensDAO)y;

            return ((new CaseInsensitiveComparer()).Compare(obX.vDescricao.Trim(), obY.vDescricao.Trim()));
        }
    }

    public class precificacaoDAO
    {
        public String filial = "";
        public String codigo = "";
        public String descricao = "";
        public DateTime data_cadastro = new DateTime();
        public String data_cadastroBr()
        {
            return dataBr(data_cadastro);
        }

        public int gridInicio = 0;
        public int gridFim = 100;
        public String usuario { get; set; }
        public String status { get; set; }
        public ArrayList arrItens = new ArrayList();
        private User usr;
        public Decimal vlr_porc = 0;

        public bool todas_filiais = false;
        public precificacaoDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();

        }
        public precificacaoDAO(String codigo, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.filial = usr.getFilial();
            String sql = "Select * from  precificacao where codigo ='" + codigo + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }


        public void addItem(precificacao_itensDAO item)
        {
            item.index = arrItens.Count;
            item.codigo = this.codigo;
            arrItens.Add(item);
        }

        public void removeItem(int index)
        {
            arrItens.RemoveAt(index + gridInicio);
        }

        public DataTable precificacaoItens()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("Codigo");
            cabecalho.Add("plu");
            cabecalho.Add("descricao");
            cabecalho.Add("custo");
            cabecalho.Add("margem");
            cabecalho.Add("preco_anterior");
            cabecalho.Add("preco_novo");
            cabecalho.Add("codigo_familia");


            itens.Add(cabecalho);

            if (arrItens != null && arrItens.Count > 0)
            {
                 for (int i = gridInicio; (i < gridFim )&& (i<arrItens.Count); i++)
                    {
                        precificacao_itensDAO item =(precificacao_itensDAO)arrItens[i];
                        itens.Add(item.ArrToString());
                     }
            }
            return Conexao.GetArryTable(itens);
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
                    filial = rs["filial"].ToString();
                    codigo = rs["codigo"].ToString();
                    descricao = rs["descricao"].ToString();
                    data_cadastro = (rs["data_cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_cadastro"].ToString()));
                    usuario = rs["usuario"].ToString();
                    status = rs["status"].ToString();
                    todas_filiais = rs["todas_filiais"].ToString().Equals("1");
                    Decimal.TryParse(rs["vlr_porc"].ToString(), out vlr_porc);
                }
                carregarItens();
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

        public void ordernarItens(String Ordem)
        {
            IComparer cp;
            if (Ordem.ToUpper().Equals("1"))
            {
                cp = new PrecificacaoItensOdemPlu();
                
            }
            else
            {
                cp = new PrecificacaoItensOdemDescricao();
                
            }
            arrItens.Sort(cp);
        }

        public void carregarItens()
        {
            SqlDataReader rs = null;

            try
            {
                arrItens.Clear();
                rs = Conexao.consulta("Select pi.plu, mercadoria.descricao,pi.custo,pi.margem,pi.preco_anterior,pi.preco_novo,mercadoria.codigo_familia from precificacao_itens as pi inner join mercadoria on pi.plu = mercadoria.plu where codigo='" + this.codigo + "' ", usr, false);

                while (rs.Read())
                {
                    precificacao_itensDAO item = new precificacao_itensDAO(usr);
                    item.plu = rs["plu"].ToString();
                    item.descricao = rs["descricao"].ToString();
                    Decimal.TryParse(rs["custo"].ToString(), out item.custo);
                    Decimal.TryParse(rs["margem"].ToString(), out item.margem);
                    Decimal.TryParse(rs["preco_anterior"].ToString(), out item.preco_anterior);
                    Decimal.TryParse(rs["preco_novo"].ToString(), out item.preco_novo);
                    item.Codigo_Familia = rs["codigo_familia"].ToString();
                    addItem(item);
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

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  precificacao set " +

                              "descricao='" + descricao + "'" +
                              ",data_cadastro=" + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'")  +
                              ",usuario='" + usuario + "'" +
                              ",status='" + status + "'" +
                              ",todas_filiais="+(todas_filiais?"1":"0")+
                              ",vlr_porc ="+vlr_porc.ToString().Replace(",",".")+
                    "  where filial='" + filial + "' and  codigo= '" + codigo + "'";
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                if (novo)
                {
                    insert(conn, tran);
                }
                else
                {
                    update(conn, tran);
                }
                atualizarItens(conn, tran);


                tran.Commit();
                return true;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public void atualizarItens(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from precificacao_itens where filial='" + filial + "' and codigo='" + codigo + "'", conn, tran);
            foreach (precificacao_itensDAO item in arrItens)
            {
                item.codigo = this.codigo;
                item.salvar(true, conn, tran);
            }
        }
        public void aplicaPorc(Decimal vlr)
        {
            
                foreach (precificacao_itensDAO item in arrItens)
                {
                    Decimal nVlr;
                    if (vlr > 0)
                    {
                        nVlr = item.preco_anterior + ((item.preco_anterior * vlr) / 100);
                    }
                    else
                    {
                        nVlr = item.preco_anterior;
                    }
                    item.preco_novo = nVlr;
                    item.margem = Funcoes.verificamargem(item.custo, item.preco_novo, 0, 0);

                }
            
            
        }
        public bool excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                String sql = "delete from precificacao  where filial='" + filial + "' and codigo= '" + codigo + "'"; //colocar campo index
                Conexao.executarSql(sql);

                Conexao.executarSql("delete from precificacao_itens where filial='" + filial + "' and codigo='" + codigo + "'", conn, tran);



                tran.Commit();
                return true;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                this.codigo = Funcoes.sequencia("precificacao.codigo", usr);


                String sql = " insert into precificacao (" +
                          "filial," +
                          "codigo," +
                          "descricao," +
                          "data_cadastro," +
                          "usuario," +
                          "status" +
                          ",todas_filiais"+
                          ",vlr_porc"+
                     " )values (" +
                          "'" + filial + "'," +
                          "'" + codigo + "'" +
                          "," + "'" + descricao + "'" +
                          "," + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + usuario + "'" +
                          "," + "'" + status + "'" +
                          ","+(todas_filiais?"1":"0")+
                          ","+vlr_porc.ToString().Replace(",",".")+
                         ");";

                Conexao.executarSql(sql, conn, tran);

                Funcoes.salvaProximaSequencia("precificacao.codigo", usr);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from precificacao";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ precificacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      String sql = "";
                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " campoPesquisa1 like '" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";     
                          }
                         sql += "campoPesquisa2 = '" + txtPESQ2.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                         try
                         {
                            User usr = (User)Session["User"];
                            if (!sql.Equals(""))
                            {
                               tb = Conexao.GetTable(sqlGrid+" where "+sql, usr);
                             }
                             else
                             {
                               tb = Conexao.GetTable(sqlGrid, usr);
                              }
                               gridPesquisa.DataSource = tb;
                               gridPesquisa.DataBind();
                               lblPesquisaErro.Text = "";
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                 protected override bool campoObrigatorio(Control campo)
                 { 
                       return false;
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {
                       return false;
                 }
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>precificacao</h1></center>
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
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox></asp:TextBox>  
                </td>  
                <td>  
                   <p>CAMPO DE PESQUISA 2</p>  
                   <asp:TextBox ID="txtPESQ2" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="codigo" Text="codigo" Visible="true" 
                    HeaderText="codigo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/precificacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao" Text="descricao" Visible="true" 
                    HeaderText="descricao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/precificacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data_cadastro" Text="data_cadastro" Visible="true" 
                    HeaderText="data_cadastro" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/precificacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usuario" Text="usuario" Visible="true" 
                    HeaderText="usuario" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/precificacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="status" Text="status" Visible="true" 
                    HeaderText="status" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/precificacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  


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
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static precificacaoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new precificacaoDAO();
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
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
                                        obj = new precificacaoDAO(index,usr);
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
                 
                 protected bool validaCamposObrigatorios() {
                    if (validaCampos(cabecalho) && validaCampos(conteudo))
                             return true;
                    else
                             return false;
                 }
                 
                 protected override bool campoObrigatorio(Control campo)
                 {// colocar os nomes dos campos obrigarios no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {// colocar os nomes dos campos Desabilitados no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 protected override void btnIncluir_Click(object sender, EventArgs e)
                 {
                    incluir(pnBtn);
                 }
                 
                 protected override void btnEditar_Click(object sender, EventArgs e)
                 {
                    editar(pnBtn);
                    EnabledControls(cabecalho, true);
                    EnabledControls(conteudo, true);
                 }
                  
                 protected override void btnPesquisar_Click(object sender, EventArgs e)
                 {
                 Response.Redirect("nomepaginapesquisa.aspx"); //colocar o endereco da tela de pesquisa
                 }
                  
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     pnConfima.Visible = true;
                  }
                  
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     try
                     {
                       if (validaCamposObrigatorios())
                       {
                  
                             carregarDadosObj();
                             obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                             lblError.Text = "Salvo com Sucesso";
                             lblError.ForeColor = System.Drawing.Color.Blue;
                             EnabledControls(cabecalho, false);
                             EnabledControls(conteudo, false);
                             visualizar(pnBtn);
                       }
                       else
                       {
                            lblError.Text = "Campo Obrigatorio n?o preenchido";
                            lblError.ForeColor = System.Drawing.Color.Red;
                        }
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
                       }
                   }
                    //--Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtcodigo.Text=obj.codigo.ToString();
                                         txtdescricao.Text=obj.descricao.ToString();
                                         txtdata_cadastro.Text=obj.data_cadastroBr();
                                         txtusuario.Text=obj.usuario.ToString();
                                         txtstatus.Text=obj.status.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.codigo=txtcodigo.Text;
                                         obj.descricao=txtdescricao.Text;
                                         obj.data_cadastro=(txtdata_cadastro.Text.Equals("")?new DateTime():DateTime.Parse(txtdata_cadastro.Text));
                                         obj.usuario=txtusuario.Text;
                                         obj.status=txtstatus.Text;
   }

                  
                  protected void lista_click(object sender, ImageClickEventArgs e)
                  {
                      ImageButton btn = (ImageButton)sender;
                      pnFundo.Visible = true;
                      chkLista.Items.Clear();
                      String sqlLista = "";
                  
                      switch (btn.ID)
                      {
                          case "idBotao":
                              sqlLista = "Query de pesquisa com no minimo 2campos";
                              lbllista.Text = "Pagamentos";
                              camporeceber = "txtPagamento";
                              break;
                      }
                      User usr = (User)Session["User"];
                      SqlDataReader lista = Conexao.consulta(sqlLista, usr);
                  
                      while (lista.Read())
                      {
                          ListItem item = new ListItem();
                          item.Value = lista[0].ToString();
                          item.Text = lista[1].ToString();
                          chkLista.Items.Add(item);
                       }
                       if (lista != null)
                          lista.Close();
                  }
                  
                  protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      try
                      {
                          obj.excluir();
                          pnConfima.Visible = false;
                          lblError.Text = "Registro Excluido com sucesso";
                          limparCampos();
                          pesquisar(pnBtn);
                       }
                       catch (Exception err)
                        {
                               lblError.Text = "N?o foi possivel Excluir o registro error:" +err.Message;
                         }
                  }
                  
                  protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      pnConfima.Visible = false;
                  }
                  
                  protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
                      txt.Text = "";
                      for (int i = 0; i < chkLista.Items.Count; i++)
                      {
                          if (chkLista.Items[i].Selected)
                          {
                              txt.Text += chkLista.Items[i].Value;
                         }
                     }
                     pnFundo.Visible = false;
                  }
                  
                  protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      pnFundo.Visible = false;
                  }
                  
                  
                  
/*================================HTML Pagina Detalhes==========================================
<div class="cabMenu">                  
       <center> <h1>Detalhes do precificacao</h1></center>                  
</div>                  
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">                  
    </asp:Panel>                  
    <br />              
     <asp:Label ID="lblError" runat="server" Text="" ForeColor=Red></asp:Label>              
                  
    <div id="cabecalho" runat="server" class="frame" >               
     <!--Coloque aqui os campos do cabe?alho    -->         
        <table>              
              <tr>    
                  <td></td>
              </tr>    
        </table>          
    </div>              
                  
<div id="conteudo" runat="server" class="conteudo" >                  
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>codigo</p>
                   <asp:TextBox ID="txtcodigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>descricao</p>
                   <asp:TextBox ID="txtdescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data_cadastro</p>
                   <asp:TextBox ID="txtdata_cadastro" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>usuario</p>
                   <asp:TextBox ID="txtusuario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>status</p>
                   <asp:TextBox ID="txtstatus" runat="server" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
</div>                  
        <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible =false>          
              <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>           
                    <table class="frame">
                       <tr>
                           <td>          
                             <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"                    <td>       
                              Width="25px" onclick="btnConfirmaLista_Click"   />           
                              <asp:Label ID="Label4" runat="server" Text="Seleciona" ></asp:Label>          
                           </td>           
                           <td>          
                                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png" 
                                       Width="25px" onclick="btnCancelaLista_Click"  />                   
                                    <asp:Label ID="Label5" runat="server" Text="Cancela" ></asp:Label>     
                           </td>           
                       </tr>
                     </table>
                  
                      <asp:Panel ID="Panel1" runat="server" CssClass="lista" >   
                             <asp:RadioButtonList ID="chkLista" runat="server" Height=50 Width=200>
                             </asp:RadioButtonList>
                      </asp:Panel>
         </asp:Panel>      
                  
         <asp:Panel ID="pnConfima" runat="server" CssClass="fundo" Visible =false>         
           <asp:Label ID="Label1" runat="server" Text="Confirma Exclus?o" CssClass="cabMenu"></asp:Label>         
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
*/

