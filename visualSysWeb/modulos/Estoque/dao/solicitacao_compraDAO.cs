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
    public class solicitacao_compraDAO
    {
        public String filial = "";
        public String codigo = "";
        public String descricao = "";
        public DateTime data_cadastro = new DateTime();
        public String data_cadastroBr()
        {
            return dataBr(data_cadastro);
        }

        public String usuario_cadastro = "";
        public String status = "";
        public String tipo_solicitacao = "";
        public ArrayList arrItens = new ArrayList();
        public int gridInicio = 0;
        public int gridFim = 100;
        public User usr;
        public ArrayList arrPedidos = new ArrayList();
        public ArrayList arrCotacoes = new ArrayList();
        public bool itensDigitados = false;
        public int qtdeItensDigitados
        {
            get
            {
                int qtde = 0;
                foreach (solicitacao_compra_itensDAO item in arrItens)
                {
                    if (item.qtde_comprar > 0)
                        qtde++;
                }
                return qtde;
            }
        }
        public solicitacao_compra_itensDAO item(String plu )
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (plu.Equals(item.plu))
                    return item;
            }
            return null;
        }


        public solicitacao_compraDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
        }
        public solicitacao_compraDAO(String strCodigo, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.filial = usr.getFilial();
            String sql = "Select * from  solicitacao_compra where codigo ='" + strCodigo + "' and filial='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
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

        public void itemgerado(String plu)
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    item.gerado = true;
                }
            }
        }

        public bool itensCotacao()
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (!item.gerado)
                {
                    return true;
                }
            }
            return false;
        }

        public bool itemIncluido(String plu)
        {
            bool oItem = false;
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    oItem = true;
                    break;
                }
            }
            return oItem;
        }

        public void excluirItem(String plu)
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    arrItens.Remove(item);
                    break;
                }
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
                    DateTime.TryParse(rs["data_cadastro"].ToString(), out data_cadastro);
                    usuario_cadastro = rs["usuario_cadastro"].ToString();
                    status = rs["status"].ToString();
                    tipo_solicitacao = rs["tipo_solicitacao"].ToString();
                }

                carregarItens();
                carregarCotacaoPedidos();
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

        private void carregarCotacaoPedidos()
        {

            String strSqlCotacao = "Select codigo_cotacao from solicitacao_cotacao where codigo_solicitacao='" + codigo + "'";
            
            SqlDataReader rsCotacao = null;
            try
            {
                rsCotacao = Conexao.consulta(strSqlCotacao, usr, false);
                arrCotacoes.Clear();
                while (rsCotacao.Read())
                {
                    cotacaoDAO cotacao = new cotacaoDAO(rsCotacao["codigo_cotacao"].ToString(), usr);
                    arrCotacoes.Add(cotacao);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsCotacao != null)
                {
                    rsCotacao.Close();
                }
            }

            String strSqlPedidos = "Select pedido from solicitacao_pedidos where codigo_solicitacao ='" + codigo + "'";
            SqlDataReader rsPedido = null;
            try
            {
                rsPedido = Conexao.consulta(strSqlPedidos, usr, false);
                arrPedidos.Clear();
                while (rsPedido.Read())
                {
                    pedidoDAO ped = new pedidoDAO(rsPedido["pedido"].ToString(),2,usr);
                    arrPedidos.Add(ped);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsPedido != null)
                {
                    rsPedido.Close();
                }
            }
        }

        public DataTable tbPedidos()
        {
            ArrayList arrTb = new ArrayList();
            ArrayList Cabecalho = new ArrayList();
           
            
            Cabecalho.Add("Pedido");
            Cabecalho.Add("Fornecedor");
            Cabecalho.Add("Status");
            Cabecalho.Add("Data_cadastro");
            Cabecalho.Add("Total");

            arrTb.Add(Cabecalho);
            foreach (pedidoDAO item in arrPedidos)
            {
                ArrayList rw = new ArrayList();
                rw.Add(item.Pedido);
                rw.Add(item.NomeFornecedor);
                rw.Add(item.getStatus);
                rw.Add(item.Data_cadastroBr());
                rw.Add(item.Total.ToString("N2"));

                arrTb.Add(rw);
            }
            return Conexao.GetArryTable(arrTb);
        }
        public DataTable tbCotacao()
        {
            ArrayList arrTb = new ArrayList();
            ArrayList Cabecalho = new ArrayList();
            Cabecalho.Add("cotacao");
            Cabecalho.Add("descricao");
            Cabecalho.Add("data");
            Cabecalho.Add("status");


            arrTb.Add(Cabecalho);
            foreach (cotacaoDAO item in arrCotacoes)
            {
                 ArrayList rw = new ArrayList();
                 rw.Add(item.Cotacao.ToString());
                 rw.Add(item.descricao.ToString());
                 rw.Add(item.DataBr());
                 rw.Add(item.Status.ToString());
                 arrTb.Add(rw);
            }
            return Conexao.GetArryTable(arrTb);
        }

        private void carregarItens()
        {
            arrItens.Clear();
            SqlDataReader rs = null;
            try
            {


                String sql = "Select * from  solicitacao_compra_itens where codigo ='" + codigo + "'  and filial='" + filial + "' order by ordem";
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    solicitacao_compra_itensDAO item = new solicitacao_compra_itensDAO(usr);
                    item.filial = rs["filial"].ToString();
                    item.codigo = rs["codigo"].ToString();
                    item.plu = rs["plu"].ToString();
                    item.ean = rs["ean"].ToString();
                    item.ref_fornecedor = rs["ref_fornecedor"].ToString();
                    item.descricao = rs["descricao"].ToString();

                    item.und = rs["und"].ToString();
                    item.embalagem = (Decimal)(rs["embalagem"].ToString().Equals("") ? new Decimal() : rs["embalagem"]);
                    item.saldo = (Decimal)(rs["saldo"].ToString().Equals("") ? new Decimal() : rs["saldo"]);
                    item.preco_custo = (Decimal)(rs["preco_custo"].ToString().Equals("") ? new Decimal() : rs["preco_custo"]);
                    item.preco_venda = (Decimal)(rs["preco_venda"].ToString().Equals("") ? new Decimal() : rs["preco_venda"]);
                    item.mes_5 = (Decimal)(rs["mes_5"].ToString().Equals("") ? new Decimal() : rs["mes_5"]);
                    item.mes_4 = (Decimal)(rs["mes_4"].ToString().Equals("") ? new Decimal() : rs["mes_4"]);
                    item.mes_3 = (Decimal)(rs["mes_3"].ToString().Equals("") ? new Decimal() : rs["mes_3"]);
                    item.mes_2 = (Decimal)(rs["mes_2"].ToString().Equals("") ? new Decimal() : rs["mes_2"]);
                    item.ult_30 = (Decimal)(rs["ult_30"].ToString().Equals("") ? new Decimal() : rs["ult_30"]);
                    item.vda_med = (Decimal)(rs["vda_med"].ToString().Equals("") ? new Decimal() : rs["vda_med"]);
                    item.cob_cad = (Decimal)(rs["cob_cad"].ToString().Equals("") ? new Decimal() : rs["cob_cad"]);
                    item.cob_dias = (Decimal)(rs["cob_dias"].ToString().Equals("") ? new Decimal() : rs["cob_dias"]);
                    item.sugestao = (Decimal)(rs["sugestao"].ToString().Equals("") ? new Decimal() : rs["sugestao"]);
                    item.qtde_comprar = (Decimal)(rs["qtde_comprar"].ToString().Equals("") ? new Decimal() : rs["qtde_comprar"]);
                    int.TryParse(rs["ordem"].ToString(), out item.ordem);
                    item.aceita_sug = rs["aceita_sug"].ToString().Equals("1");
                    arrItens.Add(item);
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


        public bool existItemContrato()
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (item.CTR.Equals("C"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool existItemZerado()
        {
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                if (item.qtde_comprar<=0)
                {
                    return true;
                }
            }
            return false;
        }


        public void ordemItens()
        {
            int i = 1;
            foreach (solicitacao_compra_itensDAO item in arrItens)
            {
                item.ordem = i;
                i++;
            }
        }

        public DataTable Itens
        {
            get
            {
                ArrayList conteudo = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("FILIAL");
                cabecalho.Add("CODIGO");
                cabecalho.Add("PLU");
                cabecalho.Add("EAN");
                cabecalho.Add("REF_FORNECEDOR");
                cabecalho.Add("DESCRICAO");
                cabecalho.Add("CTR");
                cabecalho.Add("id_contrato");
                cabecalho.Add("UND");
                cabecalho.Add("EMBALAGEM");
                cabecalho.Add("SALDO");
                cabecalho.Add("PRECO_CUSTO");
                cabecalho.Add("PRECO_VENDA");
                cabecalho.Add("MES_1");
                cabecalho.Add("MES_2");
                cabecalho.Add("MES_3");
                cabecalho.Add("MES_4");
                cabecalho.Add("ULT_30");
                cabecalho.Add("VDA_MED");
                cabecalho.Add("COB_CAD");
                cabecalho.Add("COB_DIAS");
                cabecalho.Add("SUGESTAO");
                cabecalho.Add("QTDE_COMPRAR");
                cabecalho.Add("ORDEM");
                cabecalho.Add("ACEITA_SUG");
                conteudo.Add(cabecalho);


                if (arrItens.Count > 0)
                {

                    for (int i = gridInicio; (i < gridFim) && (i < arrItens.Count); i++)
                    {
                        solicitacao_compra_itensDAO item = (solicitacao_compra_itensDAO)arrItens[i];
                        item.ordem = i + 1;


                        if (itensDigitados)
                        {
                            if (item.qtde_comprar > 0)
                                conteudo.Add(item.ArrToString());
                        }
                        else
                        {
                            conteudo.Add(item.ArrToString());
                        }
                    }
                }
                return Conexao.GetArryTable(conteudo);
            }
        }


        public DataTable ItensContratos
        {
            get
            {
                ArrayList conteudo = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("FILIAL");
                cabecalho.Add("CODIGO");
                cabecalho.Add("PLU");
                cabecalho.Add("EAN");
                cabecalho.Add("REF_FORNECEDOR");
                cabecalho.Add("DESCRICAO");
                cabecalho.Add("CTR");
                cabecalho.Add("id_contrato");

                cabecalho.Add("UND");
                cabecalho.Add("EMBALAGEM");
                cabecalho.Add("SALDO");
                cabecalho.Add("PRECO_CUSTO");
                cabecalho.Add("PRECO_VENDA");
                cabecalho.Add("MES_1");
                cabecalho.Add("MES_2");
                cabecalho.Add("MES_3");
                cabecalho.Add("MES_4");
                cabecalho.Add("ULT_30");
                cabecalho.Add("VDA_MED");
                cabecalho.Add("COB_CAD");
                cabecalho.Add("COB_DIAS");
                cabecalho.Add("SUGESTAO");
                cabecalho.Add("QTDE_COMPRAR");
                cabecalho.Add("ORDEM");
                cabecalho.Add("ACEITA_SUG");
                conteudo.Add(cabecalho);


                if (arrItens.Count > 0)
                {

                    foreach (solicitacao_compra_itensDAO item in arrItens)
                    {
                        if (item.CTR.Equals("C"))
                        {
                            conteudo.Add(item.ArrToString());
                        }
                    }

                }
                return Conexao.GetArryTable(conteudo);
            }
        }



        private void update(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = "update  solicitacao_compra set " +
                              "descricao='" + descricao + "'" +
                              ",data_cadastro=" + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") +
                              ",usuario_cadastro='" + usuario_cadastro + "'" +
                              ",status='" + status + "'" +
                              ",tipo_solicitacao= '" + tipo_solicitacao + "'" +
                    "  where codigo='" + codigo + "' and filial='" + filial + "'";
                Conexao.executarSql(sql, conn, trans);

                Conexao.executarSql("delete from solicitacao_compra_itens where codigo ='" + codigo + "'  and filial='" + filial + "'", conn, trans);
                foreach (solicitacao_compra_itensDAO item in arrItens)
                {

                    item.codigo = codigo;
                    item.filial = filial;
                    item.salvar(true, conn, trans);
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

                Conexao.executarSql("Delete from solicitacao_pedidos where codigo_solicitacao ='" + codigo + "' and filial='"+usr.getFilial()+"'", conn, tran);
                Conexao.executarSql("Delete from solicitacao_cotacao where codigo_solicitacao ='" + codigo + "' and filial='"+usr.getFilial()+"'", conn, tran);

                foreach (pedidoDAO item in arrPedidos)
                {
                    Conexao.executarSql("insert into solicitacao_pedidos (filial,codigo_solicitacao,pedido) values('" + usr.getFilial() + "','" + codigo + "','" + item.Pedido + "')", conn, tran);
                }

                foreach (cotacaoDAO item in arrCotacoes)
                {
                    Conexao.executarSql("insert into solicitacao_cotacao (filial,codigo_solicitacao,codigo_cotacao) values('" + usr.getFilial() + "','" + codigo + "'," + item.Cotacao.ToString() + ")", conn, tran);
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
            String sql = "delete from solicitacao_compra  where campoIndex= "; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                codigo = Funcoes.sequencia("SOLICITACAO_COMPRA.CODIGO", usr);

                filial = usr.getFilial();

                String sql = " insert into solicitacao_compra (" +
                          "filial," +
                          "codigo," +
                          "descricao," +
                          "data_cadastro," +
                          "usuario_cadastro," +
                          "status" +
                          ",tipo_solicitacao" +
                     ") values (" +
                          "'" + filial + "'" +
                          ",'" + codigo + "'" +
                          ",'" + descricao + "'" +
                          "," + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") +
                          ",'" + usuario_cadastro + "'" +
                          ",'" + status + "'" +
                          ",'" + tipo_solicitacao + "'" +
                         ");";

                Conexao.executarSql(sql, conn, trans);

                foreach (solicitacao_compra_itensDAO item in arrItens)
                {
                    item.codigo = codigo;
                    item.filial = filial;
                    item.salvar(true, conn, trans);
                }


                Funcoes.salvaProximaSequencia("SOLICITACAO_COMPRA.CODIGO", usr);

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
                  static String sqlGrid = ""select * from solicitacao_compra";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ solicitacao_compraDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>solicitacao_compra</h1></center>
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
                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compraDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo" Text="codigo" Visible="true" 
                    HeaderText="codigo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compraDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao" Text="descricao" Visible="true" 
                    HeaderText="descricao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compraDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data_cadastro" Text="data_cadastro" Visible="true" 
                    HeaderText="data_cadastro" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compraDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usuario_cadastro" Text="usuario_cadastro" Visible="true" 
                    HeaderText="usuario_cadastro" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compraDetalhes.aspx?campoIndex={0}" 
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
                 protected static solicitacao_compraDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new solicitacao_compraDAO();
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
                                        obj = new solicitacao_compraDAO(index,usr);
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
                                         txtfilial.Text=obj.filial.ToString();
                                         txtcodigo.Text=obj.codigo.ToString();
                                         txtdescricao.Text=obj.descricao.ToString();
                                         txtdata_cadastro.Text=obj.data_cadastroBr();
                                         txtusuario_cadastro.Text=obj.usuario_cadastro.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.filial=txtfilial.Text;
                                         obj.codigo=txtcodigo.Text;
                                         obj.descricao=txtdescricao.Text;
                                         obj.data_cadastro=(txtdata_cadastro.Text.Equals("")?new DateTime():DateTime.Parse(txtdata_cadastro.Text));
                                         obj.usuario_cadastro=txtusuario_cadastro.Text;
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
       <center> <h1>Detalhes do solicitacao_compra</h1></center>                  
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
                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo</p>
                   <asp:TextBox ID="txtcodigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>descricao</p>
                   <asp:TextBox ID="txtdescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data_cadastro</p>
                   <asp:TextBox ID="txtdata_cadastro" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>usuario_cadastro</p>
                   <asp:TextBox ID="txtusuario_cadastro" runat="server" ></asp:TextBox>
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

