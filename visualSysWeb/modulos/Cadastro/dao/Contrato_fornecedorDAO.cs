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
    public class Contrato_fornecedorDAO
    {
        public String id_contrato = "";
        public String fornecedor = "";
        public String descricao = "";
        
        public DateTime data_Cadastro = new DateTime();
        public String data_CadastroBr()
        {
            return dataBr(data_Cadastro);
        }
        public DateTime data_inicio = new DateTime();
        public String data_inicioBr()
        {
            return dataBr(data_inicio);
        }

        public DateTime data_validade = new DateTime();
        public String data_validadeBr()
        {
            return dataBr(data_validade);
        }

        public DateTime data_base_reajuste = new DateTime();
        public String data_base_reajusteBr()
        {
            return dataBr(data_base_reajuste);
        }
        public String forma_reajuste = "";
        public int prazo_entrega = 0;
        public String prazo = "";
        public Decimal qtde_minima = 0;
        public String tipo_reajuste = "";
        public int  dia_mes_reajuste = 0;

        public ArrayList arrItens = new ArrayList();
        public ArrayList arrFilias = new ArrayList();

        public int gridInicio = 0;
        public int gridFim = 100;

        public bool itensDigitados = false;
        public int qtdeItensDigitados
        {
            get
            {
                int qtde = 0;
                foreach (Contrato_fornecedor_itemDAO item in arrItens)
                {
                    if (item.vlr > 0)
                        qtde++;
                }
                return qtde;
            }
        }
        public Contrato_fornecedorDAO() { }
        public Contrato_fornecedorDAO(String id_contrato, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  Contrato_fornecedor where id_contrato='" + id_contrato + "'";
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
        public DataTable tbFiliais()
        {
            ArrayList tb = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("id_contrato");
            cabecalho.Add("filial");
            cabecalho.Add("plu");
            cabecalho.Add("cnpj");
            cabecalho.Add("razao_social");
            tb.Add(cabecalho);
            foreach (Contrato_Fornecedor_FilialDAO item in arrFilias)
            {
                tb.Add(item.arrToString());
            }
            return Conexao.GetArryTable(tb);
        }
        public void addFilia(String filial,String Cnpj , String razaoSocial)
        {
            bool incluido = false;
            foreach (Contrato_Fornecedor_FilialDAO item in arrFilias)
            {
                if (item.Filial.Equals(filial))
                {
                    incluido = true;
                    break;
                }
            }
            if (!incluido)
            {
                Contrato_Fornecedor_FilialDAO objFilial = new Contrato_Fornecedor_FilialDAO();
                objFilial.Filial = filial;
                objFilial.cnpj = Cnpj;
                objFilial.Razao_social = razaoSocial;
                arrFilias.Add(objFilial);
            }
        }
        public void removeFilial(String filial)
        {
            foreach (Contrato_Fornecedor_FilialDAO item in arrFilias)
            {
                if (item.Filial.Equals(filial))
                {
                    arrFilias.Remove(item);
                    break;
                }
            }
        }

        public void carregarFiliais()
        {
            SqlDataReader rs = null;

            try
            {
                arrFilias.Clear();
                rs = Conexao.consulta("Select cff.*, filial.cnpj,filial.Razao_social from Contrato_Fornecedor_filial as cff inner join filial on cff.filial = filial.filial  where cff.id_contrato ='" + id_contrato + "' ", null, false);
                while (rs.Read())
                {
                    Contrato_Fornecedor_FilialDAO item = new Contrato_Fornecedor_FilialDAO();
                    item.id_contrato = id_contrato;
                    item.cnpj = rs["cnpj"].ToString();
                    item.Filial = rs["filial"].ToString();
                    item.Razao_social = rs["razao_social"].ToString();
                    arrFilias.Add(item);

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();

                }
            }


        }
        public void addItem(String plu, String Descricao, Decimal vlr,String und)
        {

            Contrato_fornecedor_itemDAO item = new Contrato_fornecedor_itemDAO();
            item.id_contrato = this.id_contrato;
            item.plu = plu;
            item.descricao = Descricao;
            item.vlr = vlr;
            item.und = und;
            arrItens.Add(item);

        }

        public void removeItem(String plu)
        {
            foreach (Contrato_fornecedor_itemDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    arrItens.Remove(item);
                    break;
                }
            }
        }
        public Contrato_fornecedor_itemDAO objItem(String plu)
        {
            Contrato_fornecedor_itemDAO oItem = null;
            foreach (Contrato_fornecedor_itemDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    oItem = item;
                    break;
                }
            }
            return oItem;
        }


        public bool itemIncluido(String plu)
        {
            bool oItem = false;
            foreach (Contrato_fornecedor_itemDAO item in arrItens)
            {
                if (item.plu.Equals(plu))
                {
                    oItem = true;
                    break;
                }
            }
            return oItem;
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
                id_contrato = rs["id_contrato"].ToString();
                fornecedor = rs["fornecedor"].ToString();
                data_Cadastro = (rs["data_Cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_Cadastro"].ToString()));
                data_validade = (rs["data_validade"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_validade"].ToString()));
                prazo = rs["prazo"].ToString();
                Decimal.TryParse(rs["qtde_minima"].ToString(), out qtde_minima);
                descricao = rs["descricao"].ToString();
                DateTime.TryParse(rs["data_inicio"].ToString(), out data_inicio);
                DateTime.TryParse(rs["data_base_reajuste"].ToString(), out data_base_reajuste);
                forma_reajuste = rs["forma_reajuste"].ToString();
                int.TryParse(rs["prazo_entrega"].ToString(), out prazo_entrega);
                tipo_reajuste = rs["tipo_reajuste"].ToString();
                int.TryParse(rs["dia_mes_reajuste"].ToString(), out dia_mes_reajuste);



                carregarItens();
                carregarFiliais();
            }

        }

        public DataTable tbItens()
        {
            ArrayList tb = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("id_contrato");
            cabecalho.Add("plu");
            cabecalho.Add("Descricao");
            cabecalho.Add("vlr");
            cabecalho.Add("und");
            tb.Add(cabecalho);
            foreach (Contrato_fornecedor_itemDAO item in arrItens)
            {
                tb.Add(item.arrToString());
            }
            return Conexao.GetArryTable(tb);
        }


        private void carregarItens()
        {
            SqlDataReader rs = null;

            try
            {
                arrItens.Clear();
                rs = Conexao.consulta("Select cfi.*,m.descricao from Contrato_Fornecedor_item as cfi inner join mercadoria as m on cfi.plu=m.plu where cfi.id_contrato ='" + id_contrato + "'", null, false);
                while (rs.Read())
                {
                    Contrato_fornecedor_itemDAO item = new Contrato_fornecedor_itemDAO();
                    item.id_contrato = id_contrato;
                    item.plu = rs["plu"].ToString();
                    item.descricao = rs["descricao"].ToString();
                    Decimal.TryParse(rs["vlr"].ToString(), out item.vlr);
                    item.und = rs["und"].ToString();
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
                {
                    rs.Close();

                }
            }


        }

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  Contrato_fornecedor set " +
                              "fornecedor='" + fornecedor + "'" +
                              ",data_Cadastro=" + (data_Cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_Cadastro.ToString("yyyy-MM-dd") + "'") + 
                              ",data_validade=" + (data_validade.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_validade.ToString("yyyy-MM-dd") + "'") + 
                              ",prazo='" + prazo + "'" +
                              ",Qtde_minima="+Funcoes.decimalPonto(qtde_minima.ToString())+
                              ",descricao='"+descricao+"'"+
                              ",data_inicio=" + (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") +
                              ",data_base_reajuste=" + (data_base_reajuste.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_base_reajuste.ToString("yyyy-MM-dd") + "'") + 
                              ",forma_reajuste='"+forma_reajuste+"'"+
                              ",prazo_entrega="+prazo_entrega.ToString()+
                              ",tipo_reajuste='"+tipo_reajuste+"'"+
                              ",dia_mes_reajuste="+dia_mes_reajuste+
                    "  where id_contrato='" + id_contrato + "'";
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

                Conexao.executarSql("delete from Contrato_fornecedor_item where id_contrato ='" + id_contrato + "'", conn, tran);
                Conexao.executarSql("update mercadoria_loja set id_contrato = '' where id_contrato ='" + id_contrato + "' and Filial in (Select Filial from contrato_fornecedor_filial where id_contrato='"+id_contrato+"')", conn, tran);
                
                
                Conexao.executarSql("delete from contrato_Fornecedor_filial where id_contrato='" + id_contrato + "' " , conn, tran);

                String strFiliais = "";
                
                foreach (Contrato_Fornecedor_FilialDAO item in arrFilias)
                {
                    item.id_contrato = id_contrato;
                    item.salvar(true, conn, tran);
                    if (strFiliais.Length > 0)
                        strFiliais += ",";

                    strFiliais += "'" + item.Filial + "'";
                }

                foreach (Contrato_fornecedor_itemDAO item in arrItens)
                {
                    
                    item.id_contrato = this.id_contrato;
                    item.salvar(true,conn, tran);
                    Conexao.executarSql("update mercadoria_loja set id_contrato = '" + id_contrato + "' where plu ='" + item.plu + "' and Filial in (" + strFiliais + ")", conn, tran);
                
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
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                String sql = "delete from Contrato_fornecedor  where id_contrato='" + id_contrato + "'";
                Conexao.executarSql(sql,conn,tran);
                Conexao.executarSql("delete from Contrato_fornecedor_item where id_contrato ='" + id_contrato + "'", conn, tran);
                Conexao.executarSql("delete from contrato_Fornecedor_filial where id_contrato='" + id_contrato + "' ", conn, tran);
                
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

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                id_contrato = Funcoes.sequencia("CONTRATO_FORNECEDOR.ID_CONTRATO", null);


                String sql = " insert into Contrato_fornecedor (" +
                          "id_contrato" +
                          ",fornecedor" +
                          ",data_Cadastro" +
                          ",data_validade" +
                          ",prazo" +
                          ",Qtde_minima" +
                          ",Descricao"+
                          ",data_inicio"+
                          ",data_base_reajuste"+
                          ",forma_reajuste"+
                          ",prazo_entrega"+
                          ",tipo_reajuste"+
                          ",dia_mes_reajuste"+
                     " )values( " +
                          "'" + id_contrato + "'" +
                          "," + "'" + fornecedor + "'" +
                          "," + (data_Cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_Cadastro.ToString("yyyy-MM-dd") + "'") +
                          "," + (data_validade.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_validade.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + prazo + "'" +
                          "," + Funcoes.decimalPonto(qtde_minima.ToString())+
                          ",'"+ descricao+"'"+
                          "," + (data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inicio.ToString("yyyy-MM-dd") + "'") +
                          "," + (data_base_reajuste.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_base_reajuste.ToString("yyyy-MM-dd") + "'") +
                          ",'"+forma_reajuste+"'"+
                          ","+prazo_entrega.ToString()+
                          ",'"+tipo_reajuste+"'"+
                          ","+dia_mes_reajuste+
                         ");";

                Conexao.executarSql(sql, conn, tran);
                Funcoes.salvaProximaSequencia("CONTRATO_FORNECEDOR.ID_CONTRATO", null);
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
                  static String sqlGrid = ""select * from Contrato_fornecedor";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ Contrato_fornecedorDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>Contrato_fornecedor</h1></center>
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
                    <asp:HyperLinkField DataTextField="id_contrato" Text="id_contrato" Visible="true" 
                    HeaderText="id_contrato" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/Contrato_fornecedorDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="fornecedor" Text="fornecedor" Visible="true" 
                    HeaderText="fornecedor" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/Contrato_fornecedorDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data_Cadastro" Text="data_Cadastro" Visible="true" 
                    HeaderText="data_Cadastro" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/Contrato_fornecedorDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data_validade" Text="data_validade" Visible="true" 
                    HeaderText="data_validade" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/Contrato_fornecedorDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="prazo" Text="prazo" Visible="true" 
                    HeaderText="prazo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/Contrato_fornecedorDetalhes.aspx?campoIndex={0}" 
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
                 protected static Contrato_fornecedorDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new Contrato_fornecedorDAO();
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
                                        obj = new Contrato_fornecedorDAO(index,usr);
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
                    //--Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtid_contrato.Text=obj.id_contrato.ToString();
                                         txtfornecedor.Text=obj.fornecedor.ToString();
                                         txtdata_Cadastro.Text=obj.data_CadastroBr();
                                         txtdata_validade.Text=obj.data_validadeBr();
                                         txtprazo.Text=obj.prazo.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.id_contrato=txtid_contrato.Text;
                                         obj.fornecedor=txtfornecedor.Text;
                                         obj.data_Cadastro=(txtdata_Cadastro.Text.Equals("")?new DateTime():DateTime.Parse(txtdata_Cadastro.Text));
                                         obj.data_validade=(txtdata_validade.Text.Equals("")?new DateTime():DateTime.Parse(txtdata_validade.Text));
                                         obj.prazo=txtprazo.Text;
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
       <center> <h1>Detalhes do Contrato_fornecedor</h1></center>                  
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
                                      <td >                   <p>id_contrato</p>
                   <asp:TextBox ID="txtid_contrato" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>fornecedor</p>
                   <asp:TextBox ID="txtfornecedor" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data_Cadastro</p>
                   <asp:TextBox ID="txtdata_Cadastro" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data_validade</p>
                   <asp:TextBox ID="txtdata_validade" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>prazo</p>
                   <asp:TextBox ID="txtprazo" runat="server" ></asp:TextBox>
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

