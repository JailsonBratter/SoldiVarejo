using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using System.Text;

namespace visualSysWeb.dao
{
    public class mercadoria_lojaDAO
    {
        public String Filial { get; set; }
        public String PLU { get; set; }
        public String Tipo { get; set; }
        public Decimal Saldo_Atual { get; set; }
        public Decimal Preco_Compra { get; set; }
        public Decimal Preco_Custo { get; set; }
        public Decimal Margem { get; set; }
        public Decimal Preco { get; set; }
        public Decimal Preco_Promocao { get; set; }
        public Decimal Margem_Promocao { get; set; }
        public DateTime Data_Inicio { get; set; }


        public DateTime Data_Fim { get; set; }

        public Decimal Preco_Custo_1 { get; set; }
        public Decimal Preco_Custo_2 { get; set; }
        public Decimal Estoque_Minimo { get; set; }
        public int Cobertura { get; set; }
        public DateTime Ultima_Entrada { get; set; }

        public DateTime Data_Inventario { get; set; }

        public bool Promocao { get; set; }
        public bool Promocao_automatica { get; set; }
        private String vDescricao = "";
        public String codigo_familia = "";

        public String descricao
        {
            get
            {
                return vDescricao;
            }
            set
            {

                if (value.Length > 50)
                    vDescricao = value.Substring(0, 50);
                else
                    vDescricao = value;
            }
        }
        public Decimal sugerido { get; set; }
        private String vDescricao_resumida = "";
        public String descricao_resumida
        {
            get
            {
                return vDescricao_resumida;
            }
            set
            {
                if (value.Length > 25)
                    vDescricao_resumida = value.Substring(0, 25);
                else
                    vDescricao_resumida = value;
            }
        }
        public String ingredientes { get; set; }
        public String marca { get; set; }
        public Decimal validade { get; set; }
        private User usr = null;
        public Decimal qtde_atacado = 0;
        public Decimal margem_atacado = 0;
        public Decimal preco_atacado = 0;
        public String id_contrato = "";


        public mercadoria_lojaDAO(User usr) 
        {
            this.usr = usr;
        }
        public mercadoria_lojaDAO(String plu, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  mercadoria_loja where plu =" + plu;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }


        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(PLU);
            item.Add(Tipo);
            item.Add(Saldo_Atual.ToString("N2"));
            item.Add(Preco_Compra.ToString("N2"));
            item.Add(Preco_Custo.ToString("N2"));
            item.Add(Margem.ToString("N2"));
            item.Add(Preco.ToString("N2"));
            item.Add(Preco_Promocao.ToString("N2"));
            item.Add(Margem_Promocao.ToString("N2"));
            item.Add(Data_Inicio.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" : Data_Inicio.ToString("dd/MM/yyyy"));
            item.Add(Data_Fim.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" :Data_Fim.ToString("dd/MM/yyyy"));
            item.Add(Preco_Custo_1.ToString("N2"));
            item.Add(Preco_Custo_2.ToString("N2"));
            item.Add(Estoque_Minimo.ToString());
            item.Add(Cobertura.ToString());
            item.Add(Ultima_Entrada.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" :Ultima_Entrada.ToString("dd/MM/yyyy"));
            item.Add(Data_Inventario.ToString("dd/MM/yyyy").Equals("01/01/0001") ? "" : Data_Inventario.ToString("dd/MM/yyyy"));
            item.Add((Promocao?"SIM":"NÃO"));
            item.Add((Promocao_automatica?"SIM":"NÃO"));
            item.Add(descricao);
            item.Add(sugerido.ToString("N2"));
            item.Add(descricao_resumida);
            item.Add(ingredientes);
            item.Add(marca);
            item.Add(validade.ToString());
            item.Add(qtde_atacado.ToString());
            item.Add(margem_atacado.ToString("N2"));
            item.Add(preco_atacado.ToString("N2"));
            item.Add(id_contrato);


            return item;
        }

        public void carregarDados(SqlDataReader rs)
        {
            try
            {
                if (rs.Read())
                {
                    Filial = rs["Filial"].ToString();
                    PLU = rs["PLU"].ToString();
                    Tipo = rs["Tipo"].ToString();
                    Saldo_Atual = (Decimal)(rs["Saldo_Atual"].ToString().Equals("") ? new Decimal() : rs["Saldo_Atual"]);
                    Preco_Compra = (Decimal)(rs["Preco_Compra"].ToString().Equals("") ? new Decimal() : rs["Preco_Compra"]);
                    Preco_Custo = (Decimal)(rs["Preco_Custo"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo"]);
                    Margem = (Decimal)(rs["Margem"].ToString().Equals("") ? new Decimal() : rs["Margem"]);
                    Preco = (Decimal)(rs["Preco"].ToString().Equals("") ? new Decimal() : rs["Preco"]);
                    Preco_Promocao = (Decimal)(rs["Preco_Promocao"].ToString().Equals("") ? new Decimal() : rs["Preco_Promocao"]);
                    Margem_Promocao = (Decimal)(rs["Margem_Promocao"].ToString().Equals("") ? new Decimal() : rs["Margem_Promocao"]);
                    Data_Inicio = (rs["Data_Inicio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Inicio"].ToString()));
                    Data_Fim = (rs["Data_Fim"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Fim"].ToString()));
                    Preco_Custo_1 = (Decimal)(rs["Preco_Custo_1"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_1"]);
                    Preco_Custo_2 = (Decimal)(rs["Preco_Custo_2"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_2"]);
                    Estoque_Minimo = (Decimal)(rs["Estoque_Minimo"].ToString().Equals("") ? new Decimal() : rs["Estoque_Minimo"]);
                    Cobertura = (rs["Cobertura"] == null ? 0 : int.Parse(rs["Cobertura"].ToString()));
                    Ultima_Entrada = (rs["Ultima_Entrada"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Ultima_Entrada"].ToString()));
                    Data_Inventario = (rs["Data_Inventario"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Inventario"].ToString()));
                    Promocao = (rs["Promocao"].ToString().Equals("1") ? true : false);
                    Promocao_automatica = (rs["Promocao_automatica"].ToString().Equals("1") ? true : false);
                    descricao = rs["descricao"].ToString();
                    sugerido = (Decimal)(rs["sugerido"].ToString().Equals("") ? new Decimal() : rs["sugerido"]);
                    descricao_resumida = rs["descricao_resumida"].ToString();
                    ingredientes = rs["ingredientes"].ToString();
                    marca = rs["marca"].ToString();
                    validade = (Decimal)(rs["validade"].ToString().Equals("") ? new Decimal() : rs["validade"]);

                    Decimal.TryParse(rs["qtde_atacado"].ToString(), out qtde_atacado);
                    Decimal.TryParse(rs["margem_atacado"].ToString(), out margem_atacado);
                    Decimal.TryParse(rs["preco_atacado"].ToString(), out preco_atacado);
                    id_contrato = rs["id_contrato"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null) //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    rs.Close();
            }
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                StringBuilder sqls = new StringBuilder(); 
                String strVericaAlteracao = "select isnull(preco,0) from mercadoria_loja where PLU= '" + PLU + "' and Filial='" + Filial + "'";
                Decimal vPrecoAnt = Decimal.Parse(Conexao.retornaUmValor(strVericaAlteracao, null,conn,tran));

                strVericaAlteracao = "select isnull(preco_Custo,0) from mercadoria_loja where PLU= '" + PLU + "' and Filial='" + Filial + "'";
                Decimal vPrecoCustoAnt = Decimal.Parse(Conexao.retornaUmValor(strVericaAlteracao, null, conn, tran));

                if (!Preco.Equals(vPrecoAnt) || !Preco_Custo.Equals(vPrecoCustoAnt))
                {
                    sqls.AppendLine("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial, custo_old, custo_new) "+
                        " values ('" + PLU + "','Mercadoria Preco',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + Preco.ToString().Replace(",", ".") + ",'"+Filial+"', " + (vPrecoCustoAnt != Preco_Custo ? vPrecoCustoAnt.ToString().Replace(",", ".") : Preco_Custo.ToString().Replace(",", ".")) +", " + Preco_Custo.ToString().Replace(",",".") + ");");
                    if (codigo_familia.Trim().Length > 0 && Funcoes.isnumeroint(codigo_familia) && int.Parse(codigo_familia) > 0)
                    {
                        sqls.AppendLine("insert into log_preco (plu,descricao,data,usuario,preco_old,preco_new,filial) " +
                        " select plu,'Alteração item da Familia " + codigo_familia + " ',GETDATE(),'" + usr.getNome() + "'," + vPrecoAnt.ToString().Replace(",", ".") + "," + Preco.ToString().Replace(",", ".") + ",'" + Filial + "' from mercadoria where  Codigo_familia='"+codigo_familia+"' and PLU<>'"+PLU+"' ;");
                    
                    }
                    String sqlTb = "update Preco_Mercadoria " +
                              "set preco ="+ Funcoes.decimalPonto(Preco.ToString())+",preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(Preco.ToString()) + "  else " + Funcoes.decimalPonto(Preco.ToString()) + "- ((" + Funcoes.decimalPonto(Preco.ToString()) + " * desconto)/100) end " +
                              " where plu ='" + PLU + "' and filial = '" + usr.getFilial() + "';";

                    //sqls.AppendLine(sqlTb);

                }
                if (!usr.filial.inibe_marcacao_familia)
                {

                    if (codigo_familia.Trim().Length > 0 && Funcoes.isnumeroint(codigo_familia) && int.Parse(codigo_familia) > 0)
                    {
                        SqlDataReader rsFmLOjas = null;
                        rsFmLOjas = Conexao.consulta("select a.PLU,isnull(b.Preco_Custo,0)preco_custo from mercadoria a inner join Mercadoria_Loja b on a.PLU=b.PLU    where a.Codigo_familia='" + codigo_familia + "' AND a.PLU <> '" + PLU + "'  and b.FILIAL='" + Filial + "'", null, false, conn, tran);
                        //ArrayList sqlsFiliais = new ArrayList();
                        while (rsFmLOjas.Read())
                        {
                            Decimal fmCustoLoja = Decimal.Parse(rsFmLOjas["preco_custo"].ToString());
                            Decimal fmMargemLoja = Funcoes.verificamargem(fmCustoLoja, Preco, 0, 0);

                            String sqlFamilia = "update Mercadoria_Loja set " +
                                      "Margem=" + fmMargemLoja.ToString().Replace(",", ".") +
                                      ",Preco=" + Preco.ToString().Replace(",", ".") +
                                      ",Preco_Promocao=" + Preco_Promocao.ToString().Replace(",", ".") +
                                      ",Margem_Promocao=" + Margem_Promocao.ToString().Replace(",", ".") +
                                      ",Data_Inicio=" + (Data_Inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Inicio.ToString("yyyy-MM-dd") + "'") +
                                      ",Data_Fim=" + (Data_Fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Fim.ToString("yyyy-MM-dd") + "'") +
                                      ",Promocao=" + (Promocao ? "1" : "0") +
                                      ",Promocao_automatica=" + (Promocao_automatica ? "1" : "0") +
                                      ",Data_Alteracao= getdate() "+ //'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                      ",qtde_atacado=" + qtde_atacado.ToString().Replace(",", ".") +
                                      ",margem_atacado=" + margem_atacado.ToString().Replace(",", ".") +
                                      ",preco_atacado=" + preco_atacado.ToString().Replace(",", ".") +
                                      " where PLU ='" + rsFmLOjas["PLU"].ToString() + "' and FILIAL='" + Filial + "' ; ";
                                        
                            sqls.AppendLine(sqlFamilia);

                            String sqlTb = "update Preco_Mercadoria " +
                             "set preco="+ Funcoes.decimalPonto(Preco.ToString()) + ",preco_promocao =case when desconto=0 then " + Funcoes.decimalPonto(Preco.ToString()) + "  else " + Funcoes.decimalPonto(Preco.ToString()) + "- ((" + Funcoes.decimalPonto(Preco.ToString()) + " * desconto)/100) end " +
                             " where plu ='" + rsFmLOjas["PLU"].ToString() + "' and filial = '" + Filial + "';";

                            sqls.AppendLine(sqlTb);

                        }
                        try
                        {

                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        finally
                        {
                            if (rsFmLOjas != null)
                                rsFmLOjas.Close();

                        }


                    }
                }


                String sql = "update  mercadoria_loja set " +
                              "Preco_Compra=" + Preco_Compra.ToString().Replace(",", ".") +
                              ",Preco_Custo=" + Preco_Custo.ToString().Replace(",", ".") +
                              (vPrecoCustoAnt != Preco_Custo ? ", Preco_Custo_1=" + vPrecoCustoAnt.ToString().Replace(",",".") : "") +
                              ",Margem=" + Margem.ToString().Replace(",", ".") +
                              ",Preco=" + Preco.ToString().Replace(",", ".") +
                              ",Preco_Promocao=" + Preco_Promocao.ToString().Replace(",", ".") +
                              ",Margem_Promocao=" + Margem_Promocao.ToString().Replace(",", ".") +
                              ",Data_Inicio=" + (Data_Inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Inicio.ToString("yyyy-MM-dd") + "'") +
                              ",Data_Fim=" + (Data_Fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Fim.ToString("yyyy-MM-dd") + "'") + 
                              ",Promocao=" + (Promocao ? "1" : "0") +
                              ",Promocao_automatica=" + (Promocao_automatica ? "1" : "0") +
                              ",Data_Alteracao = getdate() "+ //'" + DateTime.Now.ToString("yyyy-MM-dd") + "'"+
                              ",qtde_atacado=" + qtde_atacado.ToString().Replace(",", ".") +
                              ",margem_atacado=" + margem_atacado.ToString().Replace(",", ".") +
                              ",preco_atacado=" + preco_atacado.ToString().Replace(",", ".") +
                              "  where PLU= '" + PLU + "' and Filial='" + Filial + "';";
                sqls.AppendLine(sql);

                Conexao.executarSql(sqls.ToString(), conn, tran);


               


            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert( conn,  tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "delete from mercadoria_loja  where PLU= '" + PLU + "' and Filial='" + Filial + "'";
            Conexao.executarSql(sql, conn,  tran);
            return true;
        }





        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into mercadoria_loja (" +
                          "Filial," +
                          "PLU," +
                          "Tipo," +
                          "Saldo_Atual," +
                          "Preco_Compra," +
                          "Preco_Custo," +
                          "Margem," +
                          "Preco," +
                          "Preco_Promocao," +
                          "Margem_Promocao," +
                          "Data_Inicio," +
                          "Data_Fim," +
                          "Preco_Custo_1," +
                          "Preco_Custo_2," +
                          "Estoque_Minimo," +
                          "Cobertura," +
                          "Ultima_Entrada," +
                          "Data_Inventario," +
                          "Promocao," +
                          "Promocao_automatica," +
                          "descricao," +
                          "sugerido," +
                          "descricao_resumida," +
                          "ingredientes," +
                          "marca," +
                          "validade," +
                          "Data_Alteracao"+
                          ",qtde_atacado"+
                          ",margem_atacado"+
                          ",preco_atacado"+
                     ") values (" +
                          "'" + Filial + "'" +
                          "," + "'" + PLU + "'" +
                          "," + "'" + Tipo + "'" +
                          "," + Saldo_Atual.ToString().Replace(",", ".") +
                          "," + Preco_Compra.ToString().Replace(",", ".") +
                          "," + Preco_Custo.ToString().Replace(",", ".") +
                          "," + Margem.ToString().Replace(",", ".") +
                          "," + Preco.ToString().Replace(",", ".") +
                          "," + Preco_Promocao.ToString().Replace(",", ".") +
                          "," + Margem_Promocao.ToString().Replace(",", ".") +
                          "," + (Data_Inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Inicio.ToString("yyyy-MM-dd") + "'") +
                          "," + (Data_Fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Fim.ToString("yyyy-MM-dd") + "'") +
                          "," + Preco_Custo_1.ToString().Replace(",", ".") +
                          "," + Preco_Custo_2.ToString().Replace(",", ".") +
                          "," + Estoque_Minimo.ToString().Replace(",", ".") +
                          "," + Cobertura +
                          "," + (Ultima_Entrada.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Ultima_Entrada.ToString("yyyy-MM-dd") + "'") +
                          "," + (Data_Inventario.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Inventario.ToString("yyyy-MM-dd") + "'") +
                          "," + (Promocao ? 1 : 0) +
                          "," + (Promocao_automatica ? 1 : 0) +
                          "," + "'" + descricao + "'" +
                          "," + sugerido.ToString().Replace(",", ".") +
                          "," + "'" + descricao_resumida + "'" +
                          "," + "'" + ingredientes + "'" +
                          "," + "'" + marca + "'" +
                          "," + validade.ToString().Replace(",", ".") +
                          ",getdate()"+
                          //",'" + DateTime.Now.ToString("yyyy-MM-dd") + "'"+
                          ","+qtde_atacado.ToString().Replace(",",".")+
                          "," + margem_atacado.ToString().Replace(",", ".") +
                          "," + preco_atacado.ToString().Replace(",", ".") +
                         ");";

                Conexao.executarSql(sql,conn,  tran);
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
                  static String sqlGrid = ""select * from mercadoria_loja";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                       Lblindex.Text = "1/" + gridPesquisa.PageCount;
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ mercadoria_lojaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                               Lblindex.Text = "1/" + gridPesquisa.PageCount;
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
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
                  
   <center><h1>mercadoria_loja</h1></center>
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
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="26"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="PLU" Text="PLU" Visible="true" 
                    HeaderText="PLU" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Saldo_Atual" Text="Saldo_Atual" Visible="true" 
                    HeaderText="Saldo_Atual" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco_Compra" Text="Preco_Compra" Visible="true" 
                    HeaderText="Preco_Compra" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco_Custo" Text="Preco_Custo" Visible="true" 
                    HeaderText="Preco_Custo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Margem" Text="Margem" Visible="true" 
                    HeaderText="Margem" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco" Text="Preco" Visible="true" 
                    HeaderText="Preco" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco_Promocao" Text="Preco_Promocao" Visible="true" 
                    HeaderText="Preco_Promocao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Margem_Promocao" Text="Margem_Promocao" Visible="true" 
                    HeaderText="Margem_Promocao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Inicio" Text="Data_Inicio" Visible="true" 
                    HeaderText="Data_Inicio" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Fim" Text="Data_Fim" Visible="true" 
                    HeaderText="Data_Fim" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco_Custo_1" Text="Preco_Custo_1" Visible="true" 
                    HeaderText="Preco_Custo_1" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Preco_Custo_2" Text="Preco_Custo_2" Visible="true" 
                    HeaderText="Preco_Custo_2" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Estoque_Minimo" Text="Estoque_Minimo" Visible="true" 
                    HeaderText="Estoque_Minimo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cobertura" Text="Cobertura" Visible="true" 
                    HeaderText="Cobertura" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Ultima_Entrada" Text="Ultima_Entrada" Visible="true" 
                    HeaderText="Ultima_Entrada" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Inventario" Text="Data_Inventario" Visible="true" 
                    HeaderText="Data_Inventario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Promocao" Text="Promocao" Visible="true" 
                    HeaderText="Promocao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Promocao_automatica" Text="Promocao_automatica" Visible="true" 
                    HeaderText="Promocao_automatica" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao" Text="descricao" Visible="true" 
                    HeaderText="descricao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sugerido" Text="sugerido" Visible="true" 
                    HeaderText="sugerido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao_resumida" Text="descricao_resumida" Visible="true" 
                    HeaderText="descricao_resumida" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ingredientes" Text="ingredientes" Visible="true" 
                    HeaderText="ingredientes" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="marca" Text="marca" Visible="true" 
                    HeaderText="marca" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="validade" Text="validade" Visible="true" 
                    HeaderText="validade" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
           <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>       
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static mercadoria_lojaDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new mercadoria_lojaDAO();
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
                                        obj = new mercadoria_lojaDAO(index,usr);
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
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtPLU.Text=obj.PLU.ToString();
                                         txtTipo.Text=obj.Tipo.ToString();
                                         txtSaldo_Atual.Text=string.Format("{0:0,0.00}",obj.Saldo_Atual);
                                         txtPreco_Compra.Text=string.Format("{0:0,0.00}",obj.Preco_Compra);
                                         txtPreco_Custo.Text=string.Format("{0:0,0.00}",obj.Preco_Custo);
                                         txtMargem.Text=string.Format("{0:0,0.00}",obj.Margem);
                                         txtPreco.Text=string.Format("{0:0,0.00}",obj.Preco);
                                         txtPreco_Promocao.Text=string.Format("{0:0,0.00}",obj.Preco_Promocao);
                                         txtMargem_Promocao.Text=string.Format("{0:0,0.00}",obj.Margem_Promocao);
                                         txtData_Inicio.Text=obj.Data_InicioBr();
                                         txtData_Fim.Text=obj.Data_FimBr();
                                         txtPreco_Custo_1.Text=string.Format("{0:0,0.00}",obj.Preco_Custo_1);
                                         txtPreco_Custo_2.Text=string.Format("{0:0,0.00}",obj.Preco_Custo_2);
                                         txtEstoque_Minimo.Text=obj.Estoque_Minimo.ToString();
                                         txtCobertura.Text=obj.Cobertura.ToString();
                                         txtUltima_Entrada.Text=obj.Ultima_EntradaBr();
                                         txtData_Inventario.Text=obj.Data_InventarioBr();
                                         chkPromocao.Checked =obj.Promocao;
                                         chkPromocao_automatica.Checked =obj.Promocao_automatica;
                                         txtdescricao.Text=obj.descricao.ToString();
                                         txtsugerido.Text=string.Format("{0:0,0.00}",obj.sugerido);
                                         txtdescricao_resumida.Text=obj.descricao_resumida.ToString();
                                         txtingredientes.Text=obj.ingredientes.ToString();
                                         txtmarca.Text=obj.marca.ToString();
                                         txtvalidade.Text=string.Format("{0:0,0.00}",obj.validade);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.PLU=txtPLU.Text;
                                         obj.Tipo=txtTipo.Text;
                                         obj.Saldo_Atual=Decimal.Parse(txtSaldo_Atual.Text);
                                         obj.Preco_Compra=Decimal.Parse(txtPreco_Compra.Text);
                                         obj.Preco_Custo=Decimal.Parse(txtPreco_Custo.Text);
                                         obj.Margem=Decimal.Parse(txtMargem.Text);
                                         obj.Preco=Decimal.Parse(txtPreco.Text);
                                         obj.Preco_Promocao=Decimal.Parse(txtPreco_Promocao.Text);
                                         obj.Margem_Promocao=Decimal.Parse(txtMargem_Promocao.Text);
                                         obj.Data_Inicio=DateTime.Parse(txtData_Inicio.Text);
                                         obj.Data_Fim=DateTime.Parse(txtData_Fim.Text);
                                         obj.Preco_Custo_1=Decimal.Parse(txtPreco_Custo_1.Text);
                                         obj.Preco_Custo_2=Decimal.Parse(txtPreco_Custo_2.Text);
                                         obj.Estoque_Minimo=int.Parse(txtEstoque_Minimo.Text);
                                         obj.Cobertura=int.Parse(txtCobertura.Text);
                                         obj.Ultima_Entrada=DateTime.Parse(txtUltima_Entrada.Text);
                                         obj.Data_Inventario=DateTime.Parse(txtData_Inventario.Text);
                                         obj.Promocao=chkPromocao.Checked ;
                                         obj.Promocao_automatica=chkPromocao_automatica.Checked ;
                                         obj.descricao=txtdescricao.Text;
                                         obj.sugerido=Decimal.Parse(txtsugerido.Text);
                                         obj.descricao_resumida=txtdescricao_resumida.Text;
                                         obj.ingredientes=txtingredientes.Text;
                                         obj.marca=txtmarca.Text;
                                         obj.validade=Decimal.Parse(txtvalidade.Text);
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
       <center> <h1>Detalhes do mercadoria_loja</h1></center>                  
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
<div class="opcoes">                  
    <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal"               
                 OnMenuItemClick="tabMenu_MenuItemClick" Visible="true" > 
                  
       <Items>              
           <asp:MenuItem Text="Primeira Tab" Value="tab1" />         
       </Items>             
       <StaticMenuStyle CssClass="tab" />              
       <StaticMenuItemStyle CssClass="item" />             
       <staticselectedstyle backcolor="Beige" ForeColor="#465c71" />            
    </asp:Menu>              
</div>                  
                  
<div id="conteudo" runat="server" class="conteudo" enableviewstate="false">                  
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">              
       <asp:View ID="view1" runat="server" >              
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>PLU</p>
                   <asp:TextBox ID="txtPLU" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <asp:TextBox ID="txtTipo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Saldo_Atual</p>
                   <asp:TextBox ID="txtSaldo_Atual" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco_Compra</p>
                   <asp:TextBox ID="txtPreco_Compra" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco_Custo</p>
                   <asp:TextBox ID="txtPreco_Custo" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Margem</p>
                   <asp:TextBox ID="txtMargem" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco</p>
                   <asp:TextBox ID="txtPreco" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco_Promocao</p>
                   <asp:TextBox ID="txtPreco_Promocao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Margem_Promocao</p>
                   <asp:TextBox ID="txtMargem_Promocao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Inicio</p>
                   <asp:TextBox ID="txtData_Inicio" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Fim</p>
                   <asp:TextBox ID="txtData_Fim" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco_Custo_1</p>
                   <asp:TextBox ID="txtPreco_Custo_1" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Preco_Custo_2</p>
                   <asp:TextBox ID="txtPreco_Custo_2" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Estoque_Minimo</p>
                   <asp:TextBox ID="txtEstoque_Minimo" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cobertura</p>
                   <asp:TextBox ID="txtCobertura" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Ultima_Entrada</p>
                   <asp:TextBox ID="txtUltima_Entrada" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Inventario</p>
                   <asp:TextBox ID="txtData_Inventario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Promocao</p>
                   <td><asp:CheckBox ID="chkPromocao" runat="server" Text="Promocao"/>
                   </td>

                                      <td >                   <p>Promocao_automatica</p>
                   <td><asp:CheckBox ID="chkPromocao_automatica" runat="server" Text="Promocao_automatica"/>
                   </td>

                                      <td >                   <p>descricao</p>
                   <asp:TextBox ID="txtdescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sugerido</p>
                   <asp:TextBox ID="txtsugerido" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>descricao_resumida</p>
                   <asp:TextBox ID="txtdescricao_resumida" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ingredientes</p>
                   <asp:TextBox ID="txtingredientes" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>marca</p>
                   <asp:TextBox ID="txtmarca" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>validade</p>
                   <asp:TextBox ID="txtvalidade" runat="server" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
       </asp:View>              
    </asp:MultiView>                
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

