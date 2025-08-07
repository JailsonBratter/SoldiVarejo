using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class transferencias_contasDAO
    {
        public int id = 0;
        public DateTime data = new DateTime();
        public String dataBr()
        {
            return dataBr(data);
        }

        public String filial = "";
        public String usuario = "";
        public String tipo = "";
        public String conta_origem = "";
        public String conta_destino = "";
        public Decimal valor = 0;
        public String descricao = "";
        public String obs = "";
        public Decimal saldo_ant_origem = 0;
        public Decimal saldo_ant_destino = 0;
        private User usr;
        public String status = "";
        public String codigo_centro_custo = "";
        private String strDescCentro = "";
        public String descricao_centro_custo
        {
            get
            {
                if (strDescCentro.Equals("") && !codigo_centro_custo.Equals(""))
                {
                    strDescCentro = Conexao.retornaUmValor("select descricao_centro_custo from centro_custo where codigo_centro_custo ='" + codigo_centro_custo + "'", null);
                }

                return strDescCentro;
            }
        }
        public String codigo_centro_custoOrigem = "";
        private String strDescCentroOrigem = "";
        public String descricao_centro_custoOrigem
        {
            get
            {
                if (strDescCentroOrigem.Equals("") && !codigo_centro_custoOrigem.Equals(""))
                {
                    strDescCentroOrigem = Conexao.retornaUmValor("select descricao_centro_custo from centro_custo where codigo_centro_custo ='" + codigo_centro_custoOrigem + "'", null);
                }

                return strDescCentroOrigem;
            }
        }
         public String codigo_centro_custoDestino = "";
        private String strDescCentroDestino = "";
        public String descricao_centro_custoDestino
        {
            get
            {
                if (strDescCentroDestino.Equals("") && !codigo_centro_custoDestino.Equals(""))
                {
                    strDescCentroDestino = Conexao.retornaUmValor("select descricao_centro_custo from centro_custo where codigo_centro_custo ='" + codigo_centro_custoDestino + "'", null);
                }

                return strDescCentroDestino;
            }
        }


        public transferencias_contasDAO(User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
        }
        public transferencias_contasDAO(String id, User usr)
        {
            this.usr = usr;
            this.filial = usr.getFilial();
            String sql = "Select * from  transferencias_contas where id =" + id;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            try
            {
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
                int.TryParse(rs["id"].ToString(), out id);

                DateTime.TryParse(rs["data"].ToString(), out data);
                filial = rs["filial"].ToString();
                usuario = rs["usuario"].ToString();
                tipo = rs["tipo"].ToString();
                conta_origem = rs["conta_origem"].ToString();
                conta_destino = rs["conta_destino"].ToString();
                Decimal.TryParse(rs["valor"].ToString(), out valor);
                descricao = rs["descricao"].ToString();
                obs = rs["obs"].ToString();
                Decimal.TryParse(rs["saldo_ant_origem"].ToString(), out saldo_ant_origem);
                Decimal.TryParse(rs["saldo_ant_destino"].ToString(), out saldo_ant_destino);
                status = rs["status"].ToString();
                codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                codigo_centro_custoOrigem = rs["codigo_centro_custo_origem"].ToString();
                codigo_centro_custoDestino = rs["codigo_centro_custo_destino"].ToString();
            }
            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                //String sql = "update  transferencias_contas set " +
                //              "id=" + id +
                //              ",data=" + (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") + "," +
                //              ",filial='" + filial + "'" +
                //              ",usuario='" + usuario + "'" +
                //              ",tipo='" + tipo + "'" +
                //              ",conta_origem='" + conta_origem + "'" +
                //              ",conta_destino='" + conta_destino + "'" +
                //              ",valor=" + valor.ToString().Replace(",", ".") +
                //              ",descricao='" + descricao + "'" +
                //              ",obs='" + obs + "'" +
                //              ",saldo_ant_origem=" + Funcoes.decimalPonto(saldo_ant_origem.ToString("N2")) +
                //              ",saldo_ant_destino=" + Funcoes.decimalPonto(saldo_ant_destino.ToString("N2")) +

                //    "  where id = " + id
                //        ;
                //Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                if (novo)
                {
                    insert(cnn, trans);
                }
                else
                {
                    update();
                }
                trans.Commit();

            }
            catch (Exception err)
            {
                trans.Rollback();
                throw err;
            }
            finally
            {
                if (cnn != null)
                    cnn.Close();
            }


            return true;
        }

        public bool excluir()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                status = "ESTORNADA";
                String sql = "UPDATE  transferencias_contas set status ='ESTORNADA' WHERE ID =" + id;
                Conexao.executarSql(sql, conn, tran);
                String strhist = "";
                String sqlUpConta = "";

               

                if (!conta_origem.Equals(""))
                {
                    String sqlEstornoMov = "UPDATE HISTORICO_MOV_CONTA SET ESTORNADO = 1 " +
                                          " where " +
                                          " FILIAL = '"+usr.getFilial()+"'"+
                                          " and pagamento= " + (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'")  +
                                          " and id_cc ='" + conta_origem + "'" +
                                          " and origem = 'TRANSFERENCIA'" +
                                          " and documento = '" + id.ToString() + "'" +
                                          " and forma_pg='DEBITO'" +
                                          " AND OPERACAO = '"+tipo+"'" +
                                          " and vlr = " + Funcoes.decimalPonto(valor.ToString("N2")) +
                                          " AND ISNULL(ESTORNADO ,0) = 0 ;";

                    Conexao.executarSql(sqlEstornoMov, conn, tran);


                    strhist = "insert into Historico_mov_conta " +
                                        " values(" +
                                              "'" + usr.getFilial() + "'," +
                                             " GETDATE()," +
                                              (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") + "," +
                                             "'" + conta_origem + "'," +
                                             "'TRANSFERENCIA'," +
                                             "'" + (tipo.Equals("SAQUE") || tipo.Equals("LANCAMENTO DEBITO") ? descricao : "DESTINO:" + conta_destino) + "'," +
                                             "'" + id.ToString() + "'," +
                                             "'CREDITO'," +
                                             "'ESTORNO " + tipo + "'," +
                                              Funcoes.decimalPonto(valor.ToString("N2")) + "," +
                                             "'" + usr.getUsuario() + "'" +
                                             ",1"+
                                         ")";

                    Conexao.executarSql(strhist, conn, tran);
                    String filialConta = Conexao.retornaUmValor("Select Filial from conta_corrente where id_cc ='" + conta_origem + "'", null);

                    sqlUpConta = "update conta_corrente set saldo= isnull(saldo,0)+" + Funcoes.decimalPonto(valor.ToString()) +
                                        " where id_cc='" + conta_origem + "' and filial ='" +filialConta+ "'";
                    Conexao.executarSql(sqlUpConta, conn, tran);

                    
                    String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + filialConta + "' , '" + conta_origem + "' ,'" + data.ToString("yyyy-MM-dd") + "'";
                    Conexao.executarSql(strCorrigHistDia, conn, tran);
                    
                }
                if (!conta_destino.Equals(""))
                {
                    String sqlEstornoMov = "UPDATE HISTORICO_MOV_CONTA SET ESTORNADO = 1 " +
                                         " where " +
                                         " FILIAL = '" + usr.getFilial() + "'" +
                                         " and pagamento= " + (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") +
                                         " and id_cc ='" + conta_destino + "'" +
                                         " and origem = 'TRANSFERENCIA'" +
                                         " and documento = '" + id.ToString() + "'" +
                                         " and forma_pg='CREDITO'" +
                                         " AND OPERACAO = '"+tipo+"'" +
                                         " and vlr = " + Funcoes.decimalPonto(valor.ToString("N2")) +
                                         " AND ISNULL(ESTORNADO ,0) = 0 ;";

                    Conexao.executarSql(sqlEstornoMov, conn, tran);

                    strhist = "insert into Historico_mov_conta " +
                                      " values(" +
                                            "'" + usr.getFilial() + "'," +
                                           " GETDATE()," +
                                            (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") + "," +
                                           "'" + conta_destino + "'," +
                                           "'TRANSFERENCIA'," +
                                            "'" + (tipo.Equals("DEPOSITO") || tipo.Equals("LANCAMENTO CREDITO") ? descricao : "ORIGEM:" + conta_origem) + "'," +
                                           "'" + id.ToString() + "'," +
                                           "'DEBITO'," +
                                           "'ESTORNO " + tipo + "'," +
                                            Funcoes.decimalPonto(valor.ToString("N2")) + "," +
                                           "'" + usr.getUsuario() + "'" +
                                           ",1"+
                                       ")";

                    Conexao.executarSql(strhist, conn, tran);
                    String filialConta = Conexao.retornaUmValor("Select Filial from conta_corrente where id_cc ='" + conta_destino + "'", null);
                    sqlUpConta = "update conta_corrente set saldo= isnull(saldo,0)-" + Funcoes.decimalPonto(valor.ToString()) +
                                 " where id_cc='" + conta_destino + "' and filial ='" + filialConta + "'";
                    Conexao.executarSql(sqlUpConta, conn, tran);

                   
                    String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + filialConta + "' , '" + conta_destino + "' ,'" + data.ToString("yyyy-MM-dd") + "'";
                    Conexao.executarSql(strCorrigHistDia, conn, tran);

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

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                int.TryParse(Funcoes.sequencia("TRANSFERENCIAS_CONTAS.ID", usr), out id);

                String sql = " insert into transferencias_contas (" +
                          "id," +
                          "data," +
                          "filial," +
                          "usuario," +
                          "tipo," +
                          "conta_origem," +
                          "conta_destino," +
                          "valor" +
                          ",descricao" +
                          ",obs" +
                          ",saldo_ant_origem" +
                          ",saldo_ant_destino" +
                          ",status " +
                          ",codigo_centro_custo"+
                          ",codigo_centro_custo_origem"+
                          ",codigo_centro_custo_destino"+
                     " )values (" +
                          id +
                          "," + (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + filial + "'" +
                          "," + "'" + usuario + "'" +
                          "," + "'" + tipo + "'" +
                          "," + "'" + conta_origem + "'" +
                          "," + "'" + conta_destino + "'" +
                          "," + valor.ToString().Replace(",", ".") +
                          ",'" + descricao + "'" +
                          ",'" + obs + "'" +
                          "," + Funcoes.decimalPonto(saldo_ant_origem.ToString()) +
                          "," + Funcoes.decimalPonto(saldo_ant_destino.ToString()) +
                          ",'" + status + "'" +
                          ",'"+codigo_centro_custo+"'"+
                          ",'"+codigo_centro_custoOrigem+"'"+
                          ",'"+codigo_centro_custoDestino+"'"+
                        ");";

                Conexao.executarSql(sql, conn, tran);
                String strhist = "";
                String sqlUpConta = "";
                if (!conta_origem.Equals(""))
                {
                    strhist = "insert into Historico_mov_conta " +
                                      " values(" +
                                            "'" + usr.getFilial() + "'," +
                                           " GETDATE()," +
                                            (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") + "," +
                                           "'" + conta_origem + "'," +
                                           "'TRANSFERENCIA'," +
                                           "'" + (tipo.Equals("SAQUE") || tipo.Equals("LANCAMENTO DEBITO") ? descricao : "DESTINO:" + conta_destino) + "'," +
                                           "'" + id.ToString() + "'," +
                                           "'DEBITO'," +
                                           "'" + tipo + "'," +
                                            Funcoes.decimalPonto(valor.ToString("N2")) + "," +
                                           "'" + usr.getUsuario() + "'" +
                                           ",0"+
                                       ")";

                    Conexao.executarSql(strhist, conn, tran);
                    String filialConta = Conexao.retornaUmValor("Select Filial from conta_corrente where id_cc ='" + conta_origem + "'", null);

                    sqlUpConta = "update conta_corrente set saldo= isnull(saldo,0)-" + Funcoes.decimalPonto(valor.ToString()) +
                                      " where id_cc='" + conta_origem + "' ";
                    Conexao.executarSql(sqlUpConta, conn, tran);

                    String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + filialConta + "' , '" + conta_origem + "' ,'" + data.ToString("yyyy-MM-dd") + "'";
                    Conexao.executarSql(strCorrigHistDia, conn, tran);

                }

                if (!conta_destino.Equals(""))
                {

                    strhist = "insert into Historico_mov_conta " +
                                      " values(" +
                                            "'" + usr.getFilial() + "'," +
                                           " GETDATE()," +
                                            (data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data.ToString("yyyy-MM-dd") + "'") + "," +
                                           "'" + conta_destino + "'," +
                                           "'TRANSFERENCIA'," +
                                           "'"+(tipo.Equals("DEPOSITO")||tipo.Equals("LANCAMENTO CREDITO") ? descricao : "ORIGEM:" + conta_origem)+"'," +
                                           "'" + id.ToString() + "'," +
                                           "'CREDITO'," +
                                           "'" + tipo + "'," +
                                            Funcoes.decimalPonto(valor.ToString("N2")) + "," +
                                           "'" + usr.getUsuario() + "'" +
                                           ",0"+
                                       ")";

                    Conexao.executarSql(strhist, conn, tran);

                    sqlUpConta = "update conta_corrente set saldo= isnull(saldo,0)+" + Funcoes.decimalPonto(valor.ToString()) +
                                        " where id_cc='" + conta_destino + "'";
                    Conexao.executarSql(sqlUpConta, conn, tran);

                    String filialConta = Conexao.retornaUmValor("Select Filial from conta_corrente where id_cc ='" + conta_destino + "'", null);

                    String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + filialConta+ "' , '" + conta_destino + "' ,'" + data.ToString("yyyy-MM-dd") + "'";
                    Conexao.executarSql(strCorrigHistDia, conn, tran);
                }



              
                



                Funcoes.salvaProximaSequencia("TRANSFERENCIAS_CONTAS.ID", usr);

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
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
                  static String sqlGrid = ""select * from transferencias_contas";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ transferencias_contasDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>transferencias_contas</h1></center>
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
                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data" Text="data" Visible="true" 
                    HeaderText="data" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usuario" Text="usuario" Visible="true" 
                    HeaderText="usuario" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="tipo" Text="tipo" Visible="true" 
                    HeaderText="tipo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="conta_origem" Text="conta_origem" Visible="true" 
                    HeaderText="conta_origem" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="conta_destino" Text="conta_destino" Visible="true" 
                    HeaderText="conta_destino" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="valor" Text="valor" Visible="true" 
                    HeaderText="valor" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/transferencias_contasDetalhes.aspx?campoIndex={0}" 
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
                 protected static transferencias_contasDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new transferencias_contasDAO();
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
                                        obj = new transferencias_contasDAO(index,usr);
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
                                         txtid.Text=obj.id.ToString();
                                         txtdata.Text=obj.dataBr();
                                         txtfilial.Text=obj.filial.ToString();
                                         txtusuario.Text=obj.usuario.ToString();
                                         txttipo.Text=obj.tipo.ToString();
                                         txtconta_origem.Text=obj.conta_origem.ToString();
                                         txtconta_destino.Text=obj.conta_destino.ToString();
                                         txtvalor.Text=string.Format("{0:0,0.00}",obj.valor);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.id=int.Parse(txtid.Text);
                                         obj.data=(txtdata.Text.Equals("")?new DateTime():DateTime.Parse(txtdata.Text));
                                         obj.filial=txtfilial.Text;
                                         obj.usuario=txtusuario.Text;
                                         obj.tipo=txttipo.Text;
                                         obj.conta_origem=txtconta_origem.Text;
                                         obj.conta_destino=txtconta_destino.Text;
                                         obj.valor=Decimal.Parse(txtvalor.Text);
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
       <center> <h1>Detalhes do transferencias_contas</h1></center>                  
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
                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data</p>
                   <asp:TextBox ID="txtdata" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>usuario</p>
                   <asp:TextBox ID="txtusuario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>tipo</p>
                   <asp:TextBox ID="txttipo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>conta_origem</p>
                   <asp:TextBox ID="txtconta_origem" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>conta_destino</p>
                   <asp:TextBox ID="txtconta_destino" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>valor</p>
                   <asp:TextBox ID="txtvalor" runat="server"  CssClass="numero" ></asp:TextBox>
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

