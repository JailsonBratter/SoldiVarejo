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

    public class ItensInventarioOrdem : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            //bool resultado = string.Compare(a, b, true) == 0;
            String strX = ((inventario_itensDAO)x).Descricao;
            String strY= ((inventario_itensDAO)y).Descricao;
            if (strX.Length > 10)
            {
                strX = strX.Substring(0,10);
            }
            if(strY.Length > 10)
            {
                strY = strY.Substring(0, 10);
            }

            return ((string.Compare(strX, strY)));
        }
    }

    public class ItensInventarioOrdemDepartamento : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            inventario_itensDAO obX = (inventario_itensDAO) x;
            inventario_itensDAO obY = (inventario_itensDAO) y;

            return ((new CaseInsensitiveComparer()).Compare(obX.departamento+obX.Descricao,obY.departamento+obX.Descricao));
        }
    }

    public class inventarioDAO
    {
        public String Filial { get; set; }
        public String Codigo_inventario = "";
        public String Descricao_inventario = "";
        private String vtipoMovimentacao = "";
        public bool quebraDepartamento = false;
        public int gridInicio = 0;
        public int gridFim = 100;

        public String tipoMovimentacao
        {
            get
            {
                return vtipoMovimentacao;
            }
            set
            {
                vtipoMovimentacao = value;
                String strSaidaEntrada = Conexao.retornaUmValor("select saida from tipo_movimentacao where Movimentacao ='" + vtipoMovimentacao + "'", null);
                if (strSaidaEntrada.Trim().Equals(""))
                    tSaidaEntrada = -1;
                else
                    tSaidaEntrada = int.Parse(strSaidaEntrada);

            }
        }
        public int tSaidaEntrada = -1;
        public DateTime Data { get; set; }
        public String DataBr()
        {
            return dataBr(Data);
        }

        public String Usuario = "";
        public String status = "";
        private User usr = null;
        private ArrayList arrItensExcluidos = new ArrayList();
        private ArrayList arrItensAdicionado = new ArrayList();
        public ArrayList arrItens = new ArrayList();
        public bool inventario_completo = false;

        public DateTime dataInclusao;
        public DateTime dataEncerramento;

        public DataTable Itens
        {
            get
            {
                ArrayList conteudo = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("ID");
                cabecalho.Add("PLU");
                cabecalho.Add("EAN");
                cabecalho.Add("REFERENCIA");
                cabecalho.Add("DESCRICAO");
                cabecalho.Add("CODIGO_INVENTARIO");
                cabecalho.Add("SALDO_ATUAL");
                cabecalho.Add("CONTADA");
                cabecalho.Add("CUSTO");
                cabecalho.Add("QTDE");
                cabecalho.Add("DIFERENCA");
                cabecalho.Add("TOTAL");
                conteudo.Add(cabecalho);


                if (arrItens.Count > 0)
                {
                    //foreach (inventario_itensDAO item in arrItens)
                    for (int i = gridInicio; (i < gridFim )&& (i<arrItens.Count); i++)
                    {
                        inventario_itensDAO item = (inventario_itensDAO) arrItens[i];
                        item.INDEX = i+1;
                        item.tipoMovimentacao = this.tipoMovimentacao;
                        conteudo.Add(item.ArrToString());
                    }
                }
                return Conexao.GetArryTable(conteudo);
            }
        }
        public inventarioDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }

        

        public inventarioDAO(String codigo_inventario, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.Filial = usr.getFilial();
            this.Codigo_inventario = codigo_inventario;
            String sql = "Select * from  inventario where codigo_inventario ='" + Codigo_inventario.Trim() + "' and filial='" + Filial + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, false);
            carregarDados(rs);
            CarregarItens();
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
                Descricao_inventario = rs["Descricao_inventario"].ToString();
                tipoMovimentacao = rs["tipoMovimentacao"].ToString();
                Data = (rs["Data"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data"].ToString()));
                Usuario = rs["Usuario"].ToString();
                status = rs["status"].ToString();
                inventario_completo = rs["inventario_completo"].ToString().Equals("1");
                quebraDepartamento = rs["quebra_departamento"].ToString().Equals("1");
                dataInclusao = (rs["DataHora_Inclusao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["DataHora_Inclusao"].ToString()));
                dataEncerramento = (rs["DataHora_Encerramento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["DataHora_Encerramento"].ToString()));
            }
            if (rs != null)
                rs.Close();
        }

        

        public void addItens(inventario_itensDAO item)
        {
            item.Codigo_inventario = Codigo_inventario;
            item.tipoMovimentacao = this.tipoMovimentacao;
            item.tSaidaEntrada = this.tSaidaEntrada;
           
            int i = 0;
            Boolean encontrado = false;
             
            if (arrItens.Contains(item))
            {
                encontrado = true;
             
                foreach (inventario_itensDAO nitem in arrItens)
                {
                    if (nitem.PLU.Equals(item.PLU))
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            if (encontrado)
            {
               
                inventario_itensDAO it = (inventario_itensDAO)arrItens[i];
                it.Contada += item.Contada;
            }
            else
            {
                arrItens.Add(item);
                arrItensAdicionado.Add(item);

                
            }

            

        }

        public void removeItens(int index)
        {
            arrItensExcluidos.Add(arrItens[index]);
            arrItens.RemoveAt(index);
            
        }

        private void update(SqlConnection conn,SqlTransaction trans, Action<int> progressoCallback )
        {
            try
            {
                String sql = "update  inventario set " +
                              "Descricao_inventario='" + Descricao_inventario + "'" +
                              ",tipoMovimentacao='" + tipoMovimentacao + "'" +
                              ",Data=" + (Data.Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                              ",Usuario='" + Usuario + "'" +
                              ",status='" + status + "'" +
                              ",inventario_completo =" + (inventario_completo ? "1" : "0") +
                              ",quebra_departamento =" + (quebraDepartamento ? "1" : "0") +
                              ",DataHora_Encerramento = '" + (status.Equals("ENCERRADO") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : "1900-01-01") + "'" +
                    "  where  Codigo_inventario='" + Codigo_inventario.Trim() + "' and  filial='" + Filial + "'";
                        
                Conexao.executarSql(sql);


                foreach (inventario_itensDAO item in arrItensExcluidos)
                {
                    item.excluir(conn,trans);
                }

                foreach (inventario_itensDAO item in arrItensAdicionado)
                {

                    item.Codigo_inventario = Codigo_inventario;
                    item.tipoMovimentacao = tipoMovimentacao;
                    item.tSaidaEntrada = this.tSaidaEntrada;
                    item.salvar(true, conn, trans);

                    

                }
                foreach (inventario_itensDAO item in arrItens)
                {
                    item.Codigo_inventario = Codigo_inventario;
                    item.tipoMovimentacao = tipoMovimentacao;
                    item.tSaidaEntrada = this.tSaidaEntrada;
                   if (item.alterado)
                     item.salvar(false, conn, trans);

                    
                }
                encerra(conn, trans, progressoCallback);
                
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }

        private void encerra(SqlConnection conn, SqlTransaction trans, Action<int> progressoCallback)
        {
            if (status.Equals("ENCERRADO"))
            {
                bool contagemEInventario = (tSaidaEntrada == 2 ? true : false);

                int qtdeRegistros = arrItens.Count; //Qtde de registros a serem processados.
                int qtdeRegistrosProcessados = 0;

                foreach (inventario_itensDAO item in arrItens)
                {
                    qtdeRegistrosProcessados++;
                    
                    Decimal saldoValor = 0;
                    switch(tSaidaEntrada)
                    {
                        case 2:
                            //saldoValor = item.Diferenca;
                            saldoValor = item.Contada;
                            break;
                        case 1:
                            saldoValor = -item.Contada;
                            break;
                        case 0:
                            saldoValor = item.Contada;
                            break;

                    }

                    Funcoes.atualizaSaldoPLU(Filial, item.PLU, saldoValor, conn, trans, contagemEInventario);

                    string tipoMovimentacaoSaldoDiario = "EO";
                    //Checa se trata-se de inventário, o sistema vai assumir a divergência
                    if (tSaidaEntrada == 2)
                    {
                        saldoValor = item.Diferenca;
                    }

                    //Caso o valor seja negativo, será transformado em positivo
                    if (saldoValor < 0)
                    {
                        tipoMovimentacaoSaldoDiario = "SO";
                        saldoValor = saldoValor * -1;
                    }

                    //Chama função para efetuar inclusão do item na tabela de saldo diário.
                    Funcoes.atualizaSaldoPLUDia(Filial, item.PLU, saldoValor, conn, trans, tipoMovimentacaoSaldoDiario, DateTime.Today);

                    //Retorno para progressBar
                    int percentual = (qtdeRegistrosProcessados * 100) / qtdeRegistros;
                    progressoCallback?.Invoke(percentual);
                }
            }
            else
            {
                progressoCallback?.Invoke(100);
            }
        }

        public void gerarInventarioCompleto()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
           {
               Codigo_inventario = Funcoes.sequencia("INVENTARIO.CODIGO_INVENTARIO", usr);
          
                 String sql = " insert into inventario (" +
                          "Filial," +
                          "Codigo_inventario," +
                          "tipoMovimentacao,"+
                          "Descricao_inventario," +
                          "Data," +
                          "Usuario," +
                          "status,"+
                          "inventario_completo,"+
                          "quebra_departamento"+
                     " )values (" +
                          "'" + Filial + "'" +
                          "," + "'" + Codigo_inventario.Trim() + "'" +
                          ",'"+tipoMovimentacao+"'"+
                          "," + "'" + Descricao_inventario + "'" +
                          "," + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Usuario + "'" +
                          "," + "'" + status + "'" +
                          ","+(inventario_completo?"1":"0")+
                          "," + (quebraDepartamento ? "1" : "0") +
                         ");";

                 Conexao.executarSql(sql, conn, tran);

                 String sqlItens = " insert into inventario_itens (" +
                           "Filial," +
                           "PLU," +
                           "Codigo_inventario," +
                           "Saldo_atual," +
                           "Contada," +
                           "custo," +
                           "venda," +
                           "EAN" +
                      ") Select '" + Filial + "', l.PLU,'" + Codigo_inventario.Trim() + "',l.Saldo_Atual,0, l.PRECO_CUSTO,l.PRECO,'' FROM mercadoria inner join Mercadoria_Loja as l  on mercadoria.PLU=l.PLU  WHERE l.filial='" + Filial + "' and  ISNULL(Inativo,0)=0 ;";

                 Conexao.executarSql(sqlItens, conn, tran);


                 tran.Commit();
                 Funcoes.salvaProximaSequencia("INVENTARIO.CODIGO_INVENTARIO", usr);
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


        public bool salvar(bool novo, Action<int> progressoCallback)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            try
            {
                

                if (novo)
                {
                    insert(conn, tran, progressoCallback);
                }
                else
                {
                    update(conn, tran, progressoCallback);
                }
                arrItensAdicionado.Clear();
                arrItensExcluidos.Clear();


                if (inventario_completo && status.Equals("ENCERRADO"))
                {
                    SqlDataReader rsItensRestantes = null;
                    try
                    {
                        rsItensRestantes = Conexao.consulta("Select  mercadoria.PLU, isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo,Contada=0 from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='" + usr.getFilial() + "' and  (mercadoria.plu NOT IN  (Select PLU from Inventario_itens where Codigo_inventario = " + Codigo_inventario + ")) ", usr, false, conn, tran);
                        while (rsItensRestantes.Read())
                        {
                            inventario_itensDAO item = new inventario_itensDAO(usr);
                            item.PLU = rsItensRestantes["PLU"].ToString();
                            item.Saldo_atual = Decimal.Parse(rsItensRestantes["saldo_atual"].ToString());
                            item.Contada = Decimal.Parse(rsItensRestantes["contada"].ToString());
                            item.Custo = Decimal.Parse(rsItensRestantes["custo"].ToString());
                            item.encerrado = true;
                            //item.salvar(novo, conn, tran);
                            addItens(item);
                        }
                        // ordernarItens();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsItensRestantes != null)
                            rsItensRestantes.Close();
                    }

                    foreach (inventario_itensDAO item in arrItensAdicionado)
                    {
                        item.salvar(true, conn, tran);
                    }

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
            String sql = "delete from inventario  where Codigo_inventario= '" + Codigo_inventario.Trim() + "' and Filial='" + Filial + "'";
            Conexao.executarSql(sql,conn,tran);
            foreach (inventario_itensDAO item in arrItens)
            {
                item.excluir(conn, tran);

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

        private void insert(SqlConnection conn, SqlTransaction trans, Action<int> progressoCallback)
        {
            try
            {
                Codigo_inventario = Funcoes.sequencia("INVENTARIO.CODIGO_INVENTARIO", usr);
               String sql = " insert into inventario (" +
                          "Filial," +
                          "Codigo_inventario," +
                          "tipoMovimentacao,"+
                          "Descricao_inventario," +
                          "Data," +
                          "Usuario," +
                          "status,"+
                          "inventario_completo,"+
                          "quebra_departamento"+
                          ", DataHora_Inclusao"+
                          ", DataHora_Encerramento"+
                     " )values (" +
                          "'" + Filial + "'" +
                          "," + "'" + Codigo_inventario.Trim() + "'" +
                          ",'"+tipoMovimentacao+"'"+
                          "," + "'" + Descricao_inventario + "'" +
                          "," + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Usuario + "'" +
                          "," + "'" + status + "'" +
                          ","+(inventario_completo?"1":"0")+
                          "," + (quebraDepartamento ? "1" : "0") +
                          ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"+
                          ", '" + (status.Equals("ENCERRADO") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : "1900-01-01") + "'" +
                         ");";

                Conexao.executarSql(sql,conn,trans);
                Funcoes.salvaProximaSequencia("INVENTARIO.CODIGO_INVENTARIO", usr);

                foreach (inventario_itensDAO item in arrItens)
                {
                    item.Codigo_inventario = Codigo_inventario;
                    item.tipoMovimentacao = tipoMovimentacao;
                    item.tSaidaEntrada = this.tSaidaEntrada;
                    //item.encerrado = status.Equals("ENCERRADO");
                    item.salvar(true, conn, trans);
                }

                encerra(conn, trans, progressoCallback);


            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }


        public void ordernarItens()
        {
            IComparer cp  ;
            if (quebraDepartamento)
            {
               cp = new ItensInventarioOrdemDepartamento();
            }
            else
            {
                cp = new ItensInventarioOrdem();
            }
            arrItens.Sort(cp);
        }

        public void CarregarItens()
        {
            arrItens.Clear();
            
            String sqlItens = " Select Inventario_itens.*, ISNULL((SELECT TOP 1 EAN.EAN FROM EAN WHERE EAN.PLU = Mercadoria.PLU), '') AS EAN, dp.descricao_departamento ," + 
                              "ISNULL(mercadoria.Ref_Fornecedor, '') AS Referencia, mercadoria.descricao from inventario_itens inner join mercadoria on mercadoria.PLU= Inventario_itens.PLU"+
                               " INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON MERCADORIA.Codigo_departamento = DP.codigo_departamento " +
                                " where codigo_inventario='" + Codigo_inventario.Trim() + "' order by " + (quebraDepartamento ? "dp.Descricao_departamento," : "") + "  mercadoria.descricao  ";
            SqlDataReader rsItens = Conexao.consulta(sqlItens, usr, false);

            while (rsItens.Read())
            {
                inventario_itensDAO item = new inventario_itensDAO(usr);
                item.Codigo_inventario = this.Codigo_inventario;
                item.PLU = rsItens["plu"].ToString();
                item.EAN = rsItens["EAN"].ToString();
                item.departamento = rsItens["descricao_departamento"].ToString();
                item.Referencia = rsItens["Referencia"].ToString();
                item.Descricao = rsItens["descricao"].ToString();
                item.Saldo_atual = rsItens["saldo_atual"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["saldo_atual"].ToString());
                item.vContada = rsItens["contada"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["contada"].ToString());
                item.Custo = rsItens["custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["custo"].ToString());
                item.tipoMovimentacao = tipoMovimentacao;
                item.tSaidaEntrada = this.tSaidaEntrada;
                
                arrItens.Add(item);
            }

            if (rsItens != null)
                rsItens.Close();

        }

        public DataTable itensImprimir()
        {
            String SqlItens = " Select ROW_NUMBER() over(order by " + (quebraDepartamento ? "Departamento.Descricao_departamento," : "") + " mercadoria.descricao ) as RANK, Inventario_itens.*,(Inventario_itens.SALDO_ATUAL-CONTADA)AS Diferenca, mercadoria.descricao,mercadoria.ref_fornecedor,Departamento.descricao_departamento, CONVERT(DECIMAL(12,2), (Inventario_Itens.Contada * Inventario_itens.Custo)) AS Total,cast(((Inventario_itens.SALDO_ATUAL-CONTADA)*Inventario_itens.Custo) as numeric(18,2)) as totalDivergencia ,'' as itemCont, cast(((Inventario_itens.Saldo_atual+Inventario_itens.Contada)*Inventario_itens.Custo )as numeric(18,2)) as totalFinal  from inventario_itens inner join mercadoria on mercadoria.PLU= Inventario_itens.PLU " +
                                " inner join Departamento on mercadoria.Codigo_departamento = Departamento.Codigo_departamento "+
                                " where codigo_inventario='" + Codigo_inventario.Trim() + "' order by " + (quebraDepartamento ? "Departamento.Descricao_departamento," : "") + " mercadoria.descricao ";
            return Conexao.GetTable(SqlItens, usr, false);
        }

    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from inventario";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ inventarioDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>inventario</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="6"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_inventario" Text="Codigo_inventario" Visible="true" 
                    HeaderText="Codigo_inventario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_inventario" Text="Descricao_inventario" Visible="true" 
                    HeaderText="Descricao_inventario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" 
                    HeaderText="Data" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Usuario" Text="Usuario" Visible="true" 
                    HeaderText="Usuario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="status" Text="status" Visible="true" 
                    HeaderText="status" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static inventarioDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new inventarioDAO();
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
                                        obj = new inventarioDAO(index,usr);
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
                                         txtCodigo_inventario.Text=obj.Codigo_inventario.ToString();
                                         txtDescricao_inventario.Text=obj.Descricao_inventario.ToString();
                                         txtData.Text=obj.DataBr();
                                         txtUsuario.Text=obj.Usuario.ToString();
                                         txtstatus.Text=obj.status.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.Codigo_inventario=txtCodigo_inventario.Text;
                                         obj.Descricao_inventario=txtDescricao_inventario.Text;
                                         obj.Data=DateTime.Parse(txtData.Text);
                                         obj.Usuario=txtUsuario.Text;
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
       <center> <h1>Detalhes do inventario</h1></center>                  
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

                                      <td >                   <p>Codigo_inventario</p>
                   <asp:TextBox ID="txtCodigo_inventario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao_inventario</p>
                   <asp:TextBox ID="txtDescricao_inventario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data</p>
                   <asp:TextBox ID="txtData" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Usuario</p>
                   <asp:TextBox ID="txtUsuario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>status</p>
                   <asp:TextBox ID="txtstatus" runat="server" ></asp:TextBox>
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

