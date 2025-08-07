using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Collections;

namespace visualSysWeb.dao
{
    public class ItensCotacaoOrdem : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            //bool resultado = string.Compare(a, b, true) == 0;

            return ((string.Compare(((cotacao_itemDAO)x).descricao, ((cotacao_itemDAO)y).descricao)));
        }
    }

    public class cotacaoDAO
    {
        public int Cotacao = 0;
        public String filial = "";
        public String Usuario = "";
        public DateTime Data { get; set; }
        public String DataBr()
        {
            return dataBr(Data);
        }

        public String Status = "";
        public String descricao = "";
        public String desc_cotacao = "";
        public Decimal Embalagem = 0;
        private User usr = null;
        private ArrayList arritens = new ArrayList();



        public cotacaoDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
        }
        public cotacaoDAO(String cotacao, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.filial = usr.getFilial();
            String sql = "Select * from  cotacao where cotacao =" + cotacao + " and filial='" + filial + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);
                carregarDados(rs);
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
            if (rs.Read())
            {
                Cotacao = (rs["Cotacao"] == null ? 0 : int.Parse(rs["Cotacao"].ToString()));
                filial = rs["filial"].ToString();
                Usuario = rs["Usuario"].ToString();
                Data = (rs["Data"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data"].ToString()));
                Status = rs["Status"].ToString();
                descricao = rs["descricao"].ToString();
                desc_cotacao = rs["desc_cotacao"].ToString();
                // Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);

                carregarItens();

            }
            if (rs != null)
                rs.Close();
        }

        private void carregarItens()
        {




            SqlDataReader rsItens = null;
            try
            {



                rsItens = Conexao.consulta("Select Cotacao_item.* , mercadoria.descricao , mercadoria.preco_compra from  cotacao_item inner join mercadoria on cotacao_item.mercadoria = mercadoria.plu where cotacao=" + Cotacao + " order by mercadoria.descricao", usr, false);


                arritens.Clear();
                while (rsItens.Read())
                {
                    cotacao_itemDAO item = new cotacao_itemDAO(usr);
                    item.Cotacao = Cotacao;
                    item.EAN = rsItens["EAN"].ToString();
                    item.Vencedor = rsItens["Vencedor"].ToString();
                    item.Mercadoria = rsItens["Mercadoria"].ToString();
                    item.Quantidade = (rsItens["Quantidade"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rsItens["Quantidade"].ToString()));
                    item.Filial = usr.getFilial();
                    item.embalagem = (rsItens["embalagem"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rsItens["embalagem"].ToString()));
                    item.descricao = rsItens["descricao"].ToString();
                    item.preco_compra = (Decimal)(rsItens["preco_compra"].ToString().Equals("") ? new Decimal() : rsItens["preco_compra"]);
                    addItem(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsItens != null)
                    rsItens.Close();
            }
        }

        public DataTable itensNaoCotados()
        {
            String sql = "Select i.Mercadoria,m.Descricao,i.Quantidade ,i.embalagem "+
                          "  from Cotacao_item  as i "+
                             " inner join mercadoria as m on i.Mercadoria = m.PLU "+
                             " left join  Cotacao_digitacao as d  on i.Filial = d.Filial and i.Cotacao = d.Cotacao and i.Mercadoria = d.Mercadoria "+
                            " where i.Filial = '"+filial+"' and i.Cotacao = "+Cotacao+" and d.Cotacao is null";

            return Conexao.GetTable(sql, null, false);
        }
        
        public DataTable cotacaoItens()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Vencedor");
            cabecalho.Add("Mercadoria");
            cabecalho.Add("EAN");

            cabecalho.Add("Quantidade");
            cabecalho.Add("Cotacao");
            cabecalho.Add("Filial");
            cabecalho.Add("embalagem");
            cabecalho.Add("descricao");
            cabecalho.Add("preco_compra");

            itens.Add(cabecalho);

            foreach (cotacao_itemDAO item in arritens)
            {
                if (!item.excluido)
                {
                    
                        itens.Add(item.arrString());
                    
                }
            }

            return Conexao.GetArryTable(itens);
        }

        public cotacao_itemDAO item(int item)
        {

            int itensAtivos = -1;
            foreach (cotacao_itemDAO ctitem in arritens)
            {
                if (!ctitem.excluido)
                {
                    itensAtivos++;
                }
                if (itensAtivos.Equals(item))
                {
                    return ctitem;
                }

            }
            return null;
        }

        public void removeItem(cotacao_itemDAO item)
        {
            item.excluido = true;
            arritens[item.index] = item;
        }
        public void atualizaitem(cotacao_itemDAO ctItem)
        {
            arritens[ctItem.index] = ctItem;
        }

        public void addItem(cotacao_itemDAO ctItem)
        {

            if (arritens.Contains(ctItem))
                throw new Exception("Produto " + ctItem.Mercadoria + " Já incluido na Cotacao ");

            ctItem.Cotacao = this.Cotacao;
            ctItem.index = arritens.Count;
            arritens.Add(ctItem);

        }


        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  cotacao set " +
                              "Usuario='" + Usuario + "'" +
                              ",Data=" + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                              ",Status='" + Status + "'" +
                              ",descricao='" + descricao + "'" +
                              ",desc_cotacao='" + desc_cotacao + "'" +
                    "  where Cotacao=" + Cotacao + " and filial='" + filial + "'";
                Conexao.executarSql(sql, conn, tran);
                foreach (cotacao_itemDAO item in arritens)
                {
                    if (!item.excluido)
                    {
                        item.Cotacao = this.Cotacao;
                        item.Filial = usr.getFilial();
                        item.salvar(false, conn, tran);
                    }
                    else
                    {
                        item.Cotacao = this.Cotacao;
                        item.Filial = usr.getFilial();
                        item.excluir(conn, tran);
                    }


                }

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
                tran.Commit();
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
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from cotacao  where cotacao= " + Cotacao; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                Cotacao = int.Parse(Funcoes.sequencia("COTACAO.COTACAO", usr));
                String sql = " insert into cotacao (" +
                          "Cotacao," +
                          "filial," +
                          "Usuario," +
                          "Data," +
                          "Status," +
                          "descricao," +
                          "desc_cotacao" +
                     //"Embalagem" +
                     " )values( " +
                          Cotacao +
                          ",'" + usr.getFilial() + "'" +
                          ",'" + Usuario + "'" +
                          "," + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                          ",'" + Status + "'" +
                          ",'" + descricao + "'" +
                          ",'" + desc_cotacao + "'" +
                         //"," + Embalagem.ToString().Replace(",", ".") +
                         ");";

                Conexao.executarSql(sql, conn, trans);

                foreach (cotacao_itemDAO item in arritens)
                {
                    if (!item.excluido)
                    {
                        item.Cotacao = this.Cotacao;
                        item.Filial = usr.getFilial();
                        item.salvar(true, conn, trans);
                        item.inserido = false;
                    }
                }

                Funcoes.salvaProximaSequencia("COTACAO.COTACAO", usr);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }


        public void ordernarItens()
        {
            IComparer cp;
            cp = new ItensCotacaoOrdem();
            arritens.Sort(cp);
            int i = 0;
            foreach (cotacao_itemDAO item in arritens)
            {
                item.index = i;
                i++;
            }
        }

        internal string obsFornecedor(String fornecedor)
        {
            String sql = "Select REPLACE(convert(nvarchar(max),OBS),char(10) ,'<BR>') " +
                "from COTACAO_OBS_FORNECEDOR " +
                "where fornecedor = '" + fornecedor + "' and cotacao = '" + this.Cotacao + "'";
            return Conexao.retornaUmValor(sql, usr);
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from cotacao";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ cotacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>cotacao</h1></center>
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
                    <asp:HyperLinkField DataTextField="Cotacao" Text="Cotacao" Visible="true" 
                    HeaderText="Cotacao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Usuario" Text="Usuario" Visible="true" 
                    HeaderText="Usuario" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" 
                    HeaderText="Data" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Status" Text="Status" Visible="true" 
                    HeaderText="Status" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao" Text="descricao" Visible="true" 
                    HeaderText="descricao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="desc_cotacao" Text="desc_cotacao" Visible="true" 
                    HeaderText="desc_cotacao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Embalagem" Text="Embalagem" Visible="true" 
                    HeaderText="Embalagem" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cotacaoDetalhes.aspx?campoIndex={0}" 
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
                 protected static cotacaoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new cotacaoDAO();
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
                                        obj = new cotacaoDAO(index,usr);
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
                                         txtCotacao.Text=obj.Cotacao.ToString();
                                         txtfilial.Text=obj.filial.ToString();
                                         txtUsuario.Text=obj.Usuario.ToString();
                                         txtData.Text=obj.DataBr();
                                         txtStatus.Text=obj.Status.ToString();
                                         txtdescricao.Text=obj.descricao.ToString();
                                         txtdesc_cotacao.Text=obj.desc_cotacao.ToString();
                                         txtEmbalagem.Text=string.Format("{0:0,0.00}",obj.Embalagem);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Cotacao=int.Parse(txtCotacao.Text);
                                         obj.filial=txtfilial.Text;
                                         obj.Usuario=txtUsuario.Text;
                                         obj.Data=(txtData.Text.Equals("")?new DateTime():DateTime.Parse(txtData.Text));
                                         obj.Status=txtStatus.Text;
                                         obj.descricao=txtdescricao.Text;
                                         obj.desc_cotacao=txtdesc_cotacao.Text;
                                         obj.Embalagem=(txtEmbalagem.Text.Equals("")?0:Decimal.Parse(txtEmbalagem.Text));
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
       <center> <h1>Detalhes do cotacao</h1></center>                  
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
                                      <td >                   <p>Cotacao</p>
                   <asp:TextBox ID="txtCotacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Usuario</p>
                   <asp:TextBox ID="txtUsuario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data</p>
                   <asp:TextBox ID="txtData" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Status</p>
                   <asp:TextBox ID="txtStatus" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>descricao</p>
                   <asp:TextBox ID="txtdescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>desc_cotacao</p>
                   <asp:TextBox ID="txtdesc_cotacao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Embalagem</p>
                   <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" ></asp:TextBox>
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

