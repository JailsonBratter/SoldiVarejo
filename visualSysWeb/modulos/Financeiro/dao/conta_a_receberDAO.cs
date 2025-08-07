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
    public class conta_a_receberDAO
    {

        public String Documento { get; set; }
        public String Codigo_Cliente { get; set; }
        private User usr = null;
        public String nomeCliente
        {
            get
            {
                if (Codigo_Cliente.Trim().Equals(""))
                    return "";
                else
                {
                    return Conexao.retornaUmValor("Select nome_cliente from cliente where codigo_cliente ='" + Codigo_Cliente + "'", new User());
                }
            }
        }
        public String Filial { get; set; }
        public String Codigo_Centro_Custo { get; set; }
        public String Centro_custo_Descricao
        {
            get
            {
                if (Codigo_Centro_Custo != null && !Codigo_Centro_Custo.Equals(""))
                {
                    return Conexao.retornaUmValor("Select descricao_centro_custo from centro_custo where codigo_centro_custo ='" + Codigo_Centro_Custo + "'", null);
                }
                else
                {
                    return "";


                }

            }
        }

        public Decimal Valor = 0;
        public Decimal ValorPagar
        {
            get
            {
                return ((Valor - Desconto) + acrescimo) - taxa;
            }
        }
        public Decimal Desconto = 0;
        public String Obs { get; set; } = "";
        public DateTime Emissao = new DateTime();
        public String EmissaoBr()
        {
            return dataBr(Emissao);
        }

        public DateTime Vencimento { get; set; }
        public String VencimentoBr()
        {
            return dataBr(Vencimento);
        }

        public DateTime entrada { get; set; }
        public String entradaBr()
        {
            return dataBr(entrada);
        }

        public DateTime Pagamento { get; set; }
        public String PagamentoBr()
        {
            return dataBr(Pagamento);
        }

        public Decimal Valor_Pago = 0;
        private String strIdCC = "";
        public String id_cc
        {
            get
            {
                return strIdCC;

            }
            set
            {
                strIdCC = value;
                SqlDataReader rs = null;
                try
                {
                    rs = Conexao.consulta("Select banco,agencia,conta from conta_corrente where id_cc='" + strIdCC.Trim() + "'", usr, true);

                    if (rs.Read())
                    {
                        banco = rs["banco"].ToString();
                        agencia = rs["agencia"].ToString();
                        conta = rs["conta"].ToString();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    rs.Close();
                }
            }
        }

        public String cheque { get; set; }
        public String agencia { get; set; }
        public String conta { get; set; }
        public String banco { get; set; }
        public bool Baixa_Automatica { get; set; }
        public String usuario { get; set; }
        public int status { get; set; }
        public String operador { get; set; }
        public int pdv { get; set; }
        public int finalizadora { get; set; }
        public String id_finalizadora { get; set; }
        public String documento_emitido { get; set; }
        public Decimal taxa = 0;
        public Decimal parcial { get; set; }
        public Decimal acrescimo = 0;
        public String rede_cartao = "";
        public String id_bandeira = "";
        public DataTable cupons
        {

            get
            {
                try
                {
                    return Conexao.GetTable("select  Documento,convert(varchar,Emissao,103)Emissao,Valor,taxa,(total-isnull(taxa,0))total,rede_cartao,id_Bandeira from Lista_finalizadora where finalizadora=" + finalizadora + " and Emissao ='" + Emissao.ToString("yyyyMMdd") + "' and rede_cartao='" + rede_cartao + "' and id_Bandeira='" + id_bandeira + "' and pdv='" + pdv + "' ", usr, false);

                }
                catch (Exception)
                {
                    ArrayList cabecalho = new ArrayList();
                    cabecalho.Add("Documento");
                    cabecalho.Add("Emissao");
                    cabecalho.Add("Valor");
                    cabecalho.Add("taxa");
                    cabecalho.Add("Total");
                    cabecalho.Add("rede_cartao");
                    cabecalho.Add("id_Bandeira");
                    ArrayList item = new ArrayList();
                    item.Add(cabecalho);
                    return Conexao.GetArryTable(item);


                }

            }
        }
        public String tipoCartao
        {
            get
            {
                try
                {
                    return Conexao.retornaUmValor("select id_cartao from Cartao where ID_BANDEIRA ='" + id_bandeira.PadLeft(2, '0') + "' AND nro_Finalizadora='" + finalizadora + "' AND ID_REDE='" + rede_cartao + "'", null);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public String tipo_recebimento = "";
        public bool nota_servico = false;
        public String id_movimento = "";
        public bool aVista = false;


        public conta_a_receberDAO(User usr)
        {
            this.usr = usr;
            Filial = usr.getFilial(); ;
        }
        public conta_a_receberDAO(User usr, String documento)
        {            
            String sql = "Select * from  conta_a_receber where documento ='" + documento + "' and  FILIAL='" + usr.getFilial() + "'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);
                Filial = usr.getFilial(); 
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
        public conta_a_receberDAO(String documento, String codCliente, User usr)
        {
            this.usr = usr;
            this.Documento = documento;
            this.Codigo_Cliente = codCliente;

            String sql = "Select * from  conta_a_receber where documento ='" + this.Documento + "' AND codigo_cliente='" + this.Codigo_Cliente + "'  and  FILIAL='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            Filial = usr.getFilial(); ;
            carregarDados(rs);
        }
        public conta_a_receberDAO(String documento, String codCliente, String Emissao, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.Documento = documento;
            this.Codigo_Cliente = codCliente;
            DateTime.TryParse(Emissao, out this.Emissao);
            String sql = "Select * from  conta_a_receber where documento ='" + this.Documento + "' AND codigo_cliente='" + this.Codigo_Cliente + "' and emissao ='" + this.Emissao.ToString("yyyy-MM-dd") + "' and  FILIAL='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            Filial = usr.getFilial(); ;
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
                    Documento = rs["Documento"].ToString();
                    Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                    Filial = rs["Filial"].ToString();
                    Codigo_Centro_Custo = rs["Codigo_Centro_Custo"].ToString();
                    Valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);
                    Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    Obs = rs["Obs"].ToString();
                    Emissao = (rs["Emissao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Emissao"].ToString()));
                    Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                    entrada = (rs["entrada"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["entrada"].ToString()));
                    Pagamento = (rs["Pagamento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Pagamento"].ToString()));
                    Valor_Pago = (Decimal)(rs["Valor_Pago"].ToString().Equals("") ? new Decimal() : rs["Valor_Pago"]);
                    id_cc = rs["id_cc"].ToString();
                    cheque = rs["cheque"].ToString();
                    agencia = rs["agencia"].ToString();
                    conta = rs["conta"].ToString();
                    banco = rs["banco"].ToString();
                    Baixa_Automatica = (rs["Baixa_Automatica"].ToString().Equals("1") ? true : false);
                    usuario = rs["usuario"].ToString();
                    status = (rs["status"].ToString().Equals("") ? 0 : int.Parse(rs["status"].ToString()));
                    operador = rs["operador"].ToString();
                    pdv = (rs["pdv"] == null ? 0 : int.Parse(rs["pdv"].ToString()));
                    finalizadora = (rs["finalizadora"] == null ? 0 : int.Parse(rs["finalizadora"].ToString()));
                    id_finalizadora = rs["id_finalizadora"].ToString();
                    documento_emitido = rs["documento_emitido"].ToString();
                    taxa = (Decimal)(rs["taxa"].ToString().Equals("") ? new Decimal() : rs["taxa"]);
                    parcial = (Decimal)(rs["parcial"].ToString().Equals("") ? new Decimal() : rs["parcial"]);
                    rede_cartao = rs["rede_cartao"].ToString();
                    id_bandeira = rs["id_bandeira"].ToString();
                    acrescimo = (Decimal)(rs["acrescimo"].ToString().Equals("") ? new Decimal() : rs["acrescimo"]);
                    tipo_recebimento = rs["tipo_recebimento"].ToString();
                    nota_servico = (rs["nota_servico"].ToString().Equals("1") ? true : false);
                    id_movimento = rs["id_movimento"].ToString();

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
                if (status == 1 && !aVista)
                {

                    Decimal vlrAtual = Funcoes.decTry(Conexao.retornaUmValor("Select ((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0) " +
                        " from conta_a_receber " +
                        " where documento= '" + Documento + "'  and isnull(codigo_cliente,'')='" + Codigo_Cliente + "' and filial='" + Filial + "' "
                        , usr));

                    Decimal vlrNovo = ((Valor - Desconto) + acrescimo) - taxa;

                    if (vlrAtual != vlrNovo)
                    {

                        String sqlCliente = "Update cliente set utilizado= isnull(utilizado,0)-" + Funcoes.decimalPonto(vlrAtual.ToString()) +
                                 " where codigo_cliente = '" + Codigo_Cliente + "';";

                        sqlCliente += "Update cliente set utilizado= isnull(utilizado,0)+" + Funcoes.decimalPonto(vlrNovo.ToString()) +
                                 " where codigo_cliente = '" + Codigo_Cliente + "';";



                        Conexao.executarSql(sqlCliente, conn, tran);

                    }
                }

                String sql = "update  conta_a_receber set " +

                              "Codigo_Cliente='" + Codigo_Cliente + "'" +
                              ",Filial='" + Filial + "'" +
                              ",Codigo_Centro_Custo='" + Codigo_Centro_Custo + "'" +
                              ",Valor=" + Valor.ToString().Replace(",", ".") +
                              ",Desconto=" + Desconto.ToString().Replace(",", ".") +
                              ",Obs='" + Obs + "'" +
                              ",Emissao=" + (Emissao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" : "'" + Emissao.ToString("yyyy-MM-dd") + "'") +
                              ",Vencimento=" + (Vencimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                              ",entrada=" + (entrada.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + entrada.ToString("yyyy-MM-dd") + "'") +
                              ",Pagamento=" + (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Pagamento.ToString("yyyy-MM-dd") + "'") +
                              ",Valor_Pago=" + Valor_Pago.ToString().Replace(",", ".") +
                              ",id_cc='" + id_cc + "'" +
                              ",cheque='" + cheque + "'" +
                              ",agencia='" + agencia + "'" +
                              ",conta='" + conta + "'" +
                              ",banco='" + banco + "'" +
                              ",Baixa_Automatica=" + (Baixa_Automatica ? "1" : "0") +
                              ",usuario='" + usuario + "'" +
                              ",status=" + status +
                              ",operador='" + operador + "'" +
                              ",pdv=" + pdv +
                              ",finalizadora=" + finalizadora +
                              ",id_finalizadora='" + id_finalizadora + "'" +
                              ",documento_emitido='" + documento_emitido + "'" +
                              ",taxa=" + taxa.ToString().Replace(",", ".") +
                              ",parcial=" + parcial.ToString().Replace(",", ".") +
                              ",rede_cartao='" + rede_cartao + "'" +
                              ",acrescimo=" + acrescimo.ToString().Replace(",", ".") +
                              ",tipo_recebimento='" + tipo_recebimento + "'" +
                              ",nota_servico=" + (nota_servico ? "1" : "0") +
                              ",id_movimento='" + id_movimento + "'" +

                    " where documento= '" + Documento + "'  and isnull(codigo_cliente,'')='" + Codigo_Cliente + "' and filial='" + Filial + "' " //ARRUMAR CAMPO INDEX
                        ;
                Conexao.executarSql(sql, conn, tran);




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
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            if (status == 2)
            {
                if (!aVista)
                {

                    Decimal vlrAtual = ((Valor - Desconto) + acrescimo) - taxa;
                    String sqlCliente = "Update cliente set utilizado= utilizado-" + Funcoes.decimalPonto(vlrAtual.ToString()) +
                                    " where codigo_cliente = '" + Codigo_Cliente + "';";

                    Conexao.executarSql(sqlCliente, conn, tran);
                    if (!id_cc.Equals(""))
                    {
                        Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) +" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where rtrim(ltrim(id_cc))='" + id_cc.Trim() + "' and filial='" + usr.getFilial() + "'", conn, tran);
                        String strhist = "insert into Historico_mov_conta " +
                                            " values(" +
                                                  "'" + usr.getFilial() + "'," +
                                                 " GETDATE()," +
                                                  (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Pagamento.ToString("yyyy-MM-dd") + "'") + "," +
                                                 "'" + id_cc + "'," +
                                                 "'CONTA_A_RECEBER'," +
                                                 "'" + Codigo_Cliente + "'," +
                                                 "'" + Documento + "'," +
                                                 "'" + tipo_recebimento + "'," +
                                                 "'BAIXA'," +
                                                  Funcoes.decimalPonto(Valor_Pago.ToString("N2")) + "," +
                                                 "'" + usr.getUsuario() + "'" +
                                                 ",0" +
                                             ")";


                        Conexao.executarSql(strhist, conn, tran);

                        String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + usr.getFilial() + "' , '" + id_cc + "' ,'" + Pagamento.ToString("yyyy-MM-dd") + "'";
                        Conexao.executarSql(strCorrigHistDia, conn, tran);
                    }
                }
            }
            return true;
        }


        public bool salvar(bool novo)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                salvar(novo, cnn, trans);

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

        public bool reativaTitulo()
        {
            if (Documento.Contains("C"))
            {
                string docum = Documento.Replace("C", "");
                String sql = "update conta_a_receber set status = 1, documento = " + docum + " where documento = '" + Documento + "' and filial ='" + Filial + "'";
                Conexao.executarSql(sql);
            }
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            status = 3;
            String sql = "update conta_a_receber set status=3 , documento=documento+'C' where  documento='" + Documento + "' and codigo_cliente='" + Codigo_Cliente + "'"; //colocar campo index
            Conexao.executarSql(sql, conn, tran);

            if (!id_cc.Equals(""))
            {
                Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) +" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial ='" + usr.getFilial() + "'", conn, tran);
            }
            return true;


        }

        public bool excluir()
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                status = 3;
                String sql = "update conta_a_receber set status=3 , documento=documento+'C' where  documento='" + Documento + "' and codigo_cliente='" + Codigo_Cliente + "'"; //colocar campo index
                Conexao.executarSql(sql, cnn, trans);
                if (!aVista)
                {
                    String sqlCliente = "Update cliente set utilizado= isnull(utilizado,0)-" + Funcoes.decimalPonto(ValorPagar.ToString()) +
                    " where codigo_cliente = '" + Codigo_Cliente + "';";

                    Conexao.executarSql(sqlCliente, cnn, trans);
                }

                if (!id_cc.Equals(""))
                {
                    Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) -" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial ='" + usr.getFilial() + "'", cnn, trans);
                }

                //Conexao.executarSql("Delete from conta_a_receber  where  documento='" + Documento + "' and codigo_cliente='" + Codigo_Cliente + "' and Filial='" + usr.getFilial() + "'"); //colocar campo index
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

        public bool Estornar()
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                status = 1;
                if (!id_cc.Equals(""))
                {

                    Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) -" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial='" + usr.getFilial() + "'", cnn, trans);
                    String dtPg = (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? DateTime.Now.ToString("yyyy-MM-dd") : Pagamento.ToString("yyyy-MM-dd"));


                    String sqlEstornoMov = "UPDATE HISTORICO_MOV_CONTA SET ESTORNADO = 1 " +
                                          " where " +
                                          " FILIAL = '" + usr.getFilial() + "'" +
                                          " and pagamento= '" + dtPg + "'" +
                                          " and id_cc ='" + id_cc + "'" +
                                          " and origem = 'CONTA_A_RECEBER'" +
                                          " and cliente_fornecedor='" + Codigo_Cliente + "'" +
                                          " and documento = '" + Documento + "'" +
                                          " and forma_pg='" + tipo_recebimento + "'" +
                                          " AND OPERACAO = 'BAIXA'" +
                                          " and vlr = " + Funcoes.decimalPonto(Valor_Pago.ToString("N2")) +
                                          " AND ISNULL(ESTORNADO ,0) = 0 ;";

                    Conexao.executarSql(sqlEstornoMov, cnn, trans);


                    String strhist = "insert into Historico_mov_conta " +
                                   " values(" +
                                         "'" + usr.getFilial() + "'," +
                                         " GETDATE()," +
                                         "'" + dtPg + "'," +
                                        "'" + id_cc + "'," +
                                        "'CONTA_A_RECEBER'," +
                                        "'" + Codigo_Cliente + "'," +
                                        "'" + Documento + "'," +
                                        "'" + tipo_recebimento + "'," +
                                        "'ESTORNO'," +
                                         Funcoes.decimalPonto(Valor_Pago.ToString("N2")) + "," +
                                        "'" + usr.getUsuario() + "'" +
                                        ",1" +
                                    ")";

                    Conexao.executarSql(strhist, cnn, trans);

                    String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + usr.getFilial() + "' , '" + id_cc + "' ,'" + Pagamento.ToString("yyyy-MM-dd") + "'";
                    Conexao.executarSql(strCorrigHistDia, cnn, trans);
                }
                Valor_Pago = 0;
                acrescimo = 0;
                Desconto = 0;
                Pagamento = new DateTime();
                update(cnn, trans);



                trans.Commit();

            }
            catch (Exception err)
            {
                trans.Rollback();
                throw err;
            }
            return true;
        }


        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into conta_a_receber ( " +
                          "Documento," +
                          "Codigo_Cliente," +
                          "Filial," +
                          "Codigo_Centro_Custo," +
                          "Valor," +
                          "Desconto," +
                          "Obs," +
                          "Emissao," +
                          "Vencimento," +
                          "entrada," +
                          "Pagamento," +
                          "Valor_Pago," +
                          "id_cc," +
                          "cheque," +
                          "agencia," +
                          "conta," +
                          "banco," +
                          "Baixa_Automatica," +
                          "usuario," +
                          "status," +
                          "operador," +
                          "pdv," +
                          "finalizadora," +
                          "id_finalizadora," +
                          "documento_emitido," +
                          "taxa," +
                          "parcial," +
                          "rede_cartao," +
                          "acrescimo," +
                          "tipo_recebimento," +
                          "nota_servico" +
                          ",id_movimento" +
                          ",id_bandeira"+
                     " )values( " +
                          "'" + Documento + "'" +
                          "," + "'" + Codigo_Cliente + "'" +
                          "," + "'" + Filial + "'" +
                          "," + "'" + Codigo_Centro_Custo + "'" +
                          "," + Valor.ToString().Replace(",", ".") +
                          "," + Desconto.ToString().Replace(",", ".") +
                          "," + "'" + Obs + "'" +
                          "," + (Emissao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" : "'" + Emissao.ToString("yyyy-MM-dd") + "'") +
                          "," + (Vencimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                          "," + (entrada.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + entrada.ToString("yyyy-MM-dd") + "'") +
                          "," + (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Pagamento.ToString("yyyy-MM-dd") + "'") +
                          "," + Valor_Pago.ToString().Replace(",", ".") +
                          "," + "'" + id_cc + "'" +
                          "," + "'" + cheque + "'" +
                          "," + "'" + agencia + "'" +
                          "," + "'" + conta + "'" +
                          "," + "'" + banco + "'" +
                          "," + (Baixa_Automatica ? 1 : 0) +
                          "," + "'" + usuario + "'" +
                          "," + status +
                          "," + "'" + operador + "'" +
                          "," + pdv +
                          "," + finalizadora +
                          "," + "'" + id_finalizadora + "'" +
                          "," + "'" + documento_emitido + "'" +
                          "," + taxa.ToString().Replace(",", ".") +
                          "," + parcial.ToString().Replace(",", ".") +
                          ",'" + rede_cartao + "'" +
                          "," + acrescimo.ToString().Replace(",", ".") +
                          ",'" + tipo_recebimento + "'" +
                          "," + (nota_servico ? 1 : 0) +
                          ",'" + id_movimento + "'" +
                          ",'"+id_bandeira+"'"+
                         ");";

                Conexao.executarSql(sql, conn, tran);
                if (!aVista)
                {
                    String sqlCliente = "Update cliente set utilizado= isnull(utilizado,0)+" + Funcoes.decimalPonto((((Valor - Desconto) + acrescimo) - taxa).ToString()) +
                             " where codigo_cliente = '" + Codigo_Cliente + "'";
                    Conexao.executarSql(sqlCliente, conn, tran);
                }
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/*/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from conta_a_receber";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ conta_a_receberDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                 
                  
/*================================html tela de Pesquisa==========================================
                  
   <center><h1>conta_a_receber</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="28"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Documento" Text="Documento" Visible="true" 
                    HeaderText="Documento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="Codigo_Cliente" Visible="true" 
                    HeaderText="Codigo_Cliente" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Centro_Custo" Text="Codigo_Centro_Custo" Visible="true" 
                    HeaderText="Codigo_Centro_Custo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Valor" Text="Valor" Visible="true" 
                    HeaderText="Valor" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Desconto" Text="Desconto" Visible="true" 
                    HeaderText="Desconto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Obs" Text="Obs" Visible="true" 
                    HeaderText="Obs" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Emissao" Text="Emissao" Visible="true" 
                    HeaderText="Emissao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Vencimento" Text="Vencimento" Visible="true" 
                    HeaderText="Vencimento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="entrada" Text="entrada" Visible="true" 
                    HeaderText="entrada" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pagamento" Text="Pagamento" Visible="true" 
                    HeaderText="Pagamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Valor_Pago" Text="Valor_Pago" Visible="true" 
                    HeaderText="Valor_Pago" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_cc" Text="id_cc" Visible="true" 
                    HeaderText="id_cc" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cheque" Text="cheque" Visible="true" 
                    HeaderText="cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="agencia" Text="agencia" Visible="true" 
                    HeaderText="agencia" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="conta" Text="conta" Visible="true" 
                    HeaderText="conta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="banco" Text="banco" Visible="true" 
                    HeaderText="banco" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Baixa_Automatica" Text="Baixa_Automatica" Visible="true" 
                    HeaderText="Baixa_Automatica" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usuario" Text="usuario" Visible="true" 
                    HeaderText="usuario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="status" Text="status" Visible="true" 
                    HeaderText="status" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="operador" Text="operador" Visible="true" 
                    HeaderText="operador" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="pdv" Text="pdv" Visible="true" 
                    HeaderText="pdv" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="finalizadora" Text="finalizadora" Visible="true" 
                    HeaderText="finalizadora" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_finalizadora" Text="id_finalizadora" Visible="true" 
                    HeaderText="id_finalizadora" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="documento_emitido" Text="documento_emitido" Visible="true" 
                    HeaderText="documento_emitido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="taxa" Text="taxa" Visible="true" 
                    HeaderText="taxa" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="parcial" Text="parcial" Visible="true" 
                    HeaderText="parcial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="rede_cartao" Text="rede_cartao" Visible="true" 
                    HeaderText="rede_cartao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static conta_a_receberDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new conta_a_receberDAO();
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
                                        obj = new conta_a_receberDAO(index,usr);
                                        carregarDados();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
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
                   /* --Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtDocumento.Text=obj.Documento.ToString();
                                         txtCodigo_Cliente.Text=obj.Codigo_Cliente.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtCodigo_Centro_Custo.Text=obj.Codigo_Centro_Custo.ToString();
                                         txtValor.Text=string.Format("{0:0,0.00}",obj.Valor);
                                         txtDesconto.Text=string.Format("{0:0,0.00}",obj.Desconto);
                                         txtObs.Text=obj.Obs.ToString();
                                         txtEmissao.Text=obj.EmissaoBr();
                                         txtVencimento.Text=obj.VencimentoBr();
                                         txtentrada.Text=obj.entradaBr();
                                         txtPagamento.Text=obj.PagamentoBr();
                                         txtValor_Pago.Text=string.Format("{0:0,0.00}",obj.Valor_Pago);
                                         txtid_cc.Text=obj.id_cc.ToString();
                                         txtcheque.Text=obj.cheque.ToString();
                                         txtagencia.Text=obj.agencia.ToString();
                                         txtconta.Text=obj.conta.ToString();
                                         txtbanco.Text=obj.banco.ToString();
                                         chkBaixa_Automatica.Checked =obj.Baixa_Automatica;
                                         txtusuario.Text=obj.usuario.ToString();
                                         chkstatus.Checked =obj.status;
                                         txtoperador.Text=obj.operador.ToString();
                                         txtpdv.Text=obj.pdv.ToString();
                                         txtfinalizadora.Text=obj.finalizadora.ToString();
                                         txtid_finalizadora.Text=obj.id_finalizadora.ToString();
                                         txtdocumento_emitido.Text=obj.documento_emitido.ToString();
                                         txttaxa.Text=string.Format("{0:0,0.00}",obj.taxa);
                                         txtparcial.Text=string.Format("{0:0,0.00}",obj.parcial);
                                         txtrede_cartao.Text=obj.rede_cartao.ToString();
   }
*/

/* --Atualizar FormDao 
private void carregarDadosObj()
{
                      obj.Documento=txtDocumento.Text;
                      obj.Codigo_Cliente=txtCodigo_Cliente.Text;
                      obj.Filial=txtFilial.Text;
                      obj.Codigo_Centro_Custo=txtCodigo_Centro_Custo.Text;
                      obj.Valor=Decimal.Parse(txtValor.Text);
                      obj.Desconto=Decimal.Parse(txtDesconto.Text);
                      obj.Obs=txtObs.Text;
                      obj.Emissao=DateTime.Parse(txtEmissao.Text);
                      obj.Vencimento=DateTime.Parse(txtVencimento.Text);
                      obj.entrada=DateTime.Parse(txtentrada.Text);
                      obj.Pagamento=DateTime.Parse(txtPagamento.Text);
                      obj.Valor_Pago=Decimal.Parse(txtValor_Pago.Text);
                      obj.id_cc=txtid_cc.Text;
                      obj.cheque=txtcheque.Text;
                      obj.agencia=txtagencia.Text;
                      obj.conta=txtconta.Text;
                      obj.banco=txtbanco.Text;
                      obj.Baixa_Automatica=chkBaixa_Automatica.Checked ;
                      obj.usuario=txtusuario.Text;
                      obj.status=chkstatus.Checked ;
                      obj.operador=txtoperador.Text;
                      obj.pdv=int.Parse(txtpdv.Text);
                      obj.finalizadora=int.Parse(txtfinalizadora.Text);
                      obj.id_finalizadora=txtid_finalizadora.Text;
                      obj.documento_emitido=txtdocumento_emitido.Text;
                      obj.taxa=Decimal.Parse(txttaxa.Text);
                      obj.parcial=Decimal.Parse(txtparcial.Text);
                      obj.rede_cartao=int.Parse(txtrede_cartao.Text);
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
<center> <h1>Detalhes do conta_a_receber</h1></center>                  
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
                   <td >                   <p>Documento</p>
<asp:TextBox ID="txtDocumento" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Codigo_Cliente</p>
<asp:TextBox ID="txtCodigo_Cliente" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Filial</p>
<asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Codigo_Centro_Custo</p>
<asp:TextBox ID="txtCodigo_Centro_Custo" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Valor</p>
<asp:TextBox ID="txtValor" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>Desconto</p>
<asp:TextBox ID="txtDesconto" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>Obs</p>
<asp:TextBox ID="txtObs" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Emissao</p>
<asp:TextBox ID="txtEmissao" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Vencimento</p>
<asp:TextBox ID="txtVencimento" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>entrada</p>
<asp:TextBox ID="txtentrada" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Pagamento</p>
<asp:TextBox ID="txtPagamento" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Valor_Pago</p>
<asp:TextBox ID="txtValor_Pago" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>id_cc</p>
<asp:TextBox ID="txtid_cc" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>cheque</p>
<asp:TextBox ID="txtcheque" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>agencia</p>
<asp:TextBox ID="txtagencia" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>conta</p>
<asp:TextBox ID="txtconta" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>banco</p>
<asp:TextBox ID="txtbanco" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>Baixa_Automatica</p>
<td><asp:CheckBox ID="chkBaixa_Automatica" runat="server" Text="Baixa_Automatica"/>
</td>

                   <td >                   <p>usuario</p>
<asp:TextBox ID="txtusuario" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>status</p>
<td><asp:CheckBox ID="chkstatus" runat="server" Text="status"/>
</td>

                   <td >                   <p>operador</p>
<asp:TextBox ID="txtoperador" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>pdv</p>
<asp:TextBox ID="txtpdv" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>finalizadora</p>
<asp:TextBox ID="txtfinalizadora" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>id_finalizadora</p>
<asp:TextBox ID="txtid_finalizadora" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>documento_emitido</p>
<asp:TextBox ID="txtdocumento_emitido" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>taxa</p>
<asp:TextBox ID="txttaxa" runat="server"  CssClass="numero" ></asp:TextBox>
</td>

                   <td >                   <p>parcial</p>
<asp:TextBox ID="txtparcial" runat="server" ></asp:TextBox>
</td>

                   <td >                   <p>rede_cartao</p>
<asp:TextBox ID="txtrede_cartao" runat="server"  CssClass="numero" ></asp:TextBox>
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

