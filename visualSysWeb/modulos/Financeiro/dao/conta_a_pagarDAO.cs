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
    public class conta_a_pagarDAO
    {
        private User usr = null;
        public String Documento { get; set; }
        public String Fornecedor { get; set; }
        public int serie { get; set; }
        public String Filial { get; set; }
        private String CentroCusto = "";
        public bool conferido = false;
        public bool VlrAlterado = false;
        public bool VencAlterado = false;
        public String codigo_nf { get
            {
                return Documento.Substring(0, Documento.IndexOf("-"));
            } }

        public List<Contas_a_pagar_grupoDAO> Grupo_pagamento = new List<Contas_a_pagar_grupoDAO>();
        public String cod_grupo_pagamento = "";

        public String Codigo_Centro_Custo
        {
            get
            {
                if (DescCentroCusto.Equals(""))
                    DescCentroCusto = Conexao.retornaUmValor("Select top 1 descricao_centro_custo from centro_custo where codigo_centro_custo='" + CentroCusto + "'", null);


                return CentroCusto;

            }
            set
            {
                if (!CentroCusto.Equals(value))
                    VlrAlterado = true;


                CentroCusto = value;


            }
        }

        public String DescCentroCusto = "";
        private Decimal _valor = 0;
        public Decimal Valor
        {
            get { return _valor; }
            set
            {
                if (_valor != value)
                    VlrAlterado = true;
                _valor = value;
            }
        }

        public Decimal _desconto = 0;
        public Decimal Desconto
        {
            get { return _desconto; }
            set
            {
                if (_desconto != value)
                    VlrAlterado = true;
                _desconto = value;
            }
        }
        public String obs { get; set; }
        private Decimal _acrescimo = 0;

        public Decimal acrescimo
        {
            get
            {
                return _acrescimo;
            }
            set
            {
                if (_acrescimo != value)
                    VlrAlterado = true;

                _acrescimo = value;
            }
        }
        public DateTime emissao { get; set; }
        public String emissaoBr()
        {
            return dataBr(emissao);
        }

        public DateTime Pagamento { get; set; }
        public String PagamentoBr()
        {
            return dataBr(Pagamento);
        }
        private String _tipoPagamento = "";
        public String Tipo_Pagamento { get {
                return _tipoPagamento;
            }
            set
            {
                if (!_tipoPagamento.Equals(value))
                    VlrAlterado = true;


                _tipoPagamento = value;
            }
        }
        public bool Duplicata { get; set; }
        public String Numero_cheque { get; set; }
        public Decimal Valor_Pago = 0;
        private DateTime _Vencimento = new DateTime();
        public DateTime Vencimento
        {
            get
            {
                return _Vencimento;
            }
            set
            {
                if (_Vencimento != value)
                {
                    VlrAlterado = true;
                    VencAlterado = true;
                }
                    
                _Vencimento = value;
            }
        }
        public String VencimentoBr()
        {
            return dataBr(Vencimento);
        }

        private String strIdCC;
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
                    rs = Conexao.consulta("Select banco,agencia,conta from conta_corrente where id_cc='" + strIdCC + "'", usr, true);

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

        public String banco = "";
        public String agencia = "";
        public String conta = "";

        public bool Baixa_Automatica { get; set; }
        public String usuario { get; set; }
        public String status { get; set; }
        public String documento_emitido { get; set; }
        public DateTime entrada { get; set; }

        public Decimal ValorPagar
        {
            get
            {
                return ((Valor - Desconto) + acrescimo);
            }
        }
        public String entradaBr()
        {
            return dataBr(entrada);
        }

        public Decimal Parcial { get; set; }

        public bool BOLETO_RECEBIDO { get; set; }
        public string cod_barras { get; internal set; }

        public bool lancamento_simples = false;
        public string codigo_grupo = "";
        public int qtdeParcelas = 1;
        public String tipoParcela = "Mes";
        public int qtdeDias = 30;
        public bool forcaDiaUtilVencimento = false;

        public string descEventoContabil = "";

        //Evento Contábil.
        public bool origemCodigoContabilEvento = false; //Controla se o código contábil foi definido pelo FORNECEDOR ou pelo Evento Contábil
        public contabil_eventosDAO contabil_evento = new contabil_eventosDAO();
        private int eventoContabilCodigo = 0;
        public int eventoContabil
        {
            get
            {
                if (descEventoContabil.Equals("") && origemCodigoContabilEvento)
                    descEventoContabil = Conexao.retornaUmValor("SELECT TOP 1 Descricao FROM Contabil_Eventos WHERE Codigo =" + eventoContabilCodigo.ToString(), null);

                return eventoContabilCodigo;

            }
            set
            {
                //if (origemCodigoContabilEvento)
                //{ 
                    eventoContabilCodigo = value;
                
                    SqlDataReader dreventos = Conexao.consulta("SELECT * FROM Contabil_Eventos WHERE Codigo =" + eventoContabilCodigo.ToString(), null, false);
                    if (dreventos.HasRows)
                    {
                        while (dreventos.Read())
                        {
                            contabil_evento.codigo = int.Parse(dreventos["codigo"].ToString());
                            contabil_evento.descricao = dreventos["descricao"].ToString();
                            contabil_evento.conta_contabil = dreventos["conta_contabil"].ToString();
                            contabil_evento.despesa = int.Parse(dreventos["despesa"].ToString());
                        }
                    }
                    else
                    {
                        contabil_evento.codigo = 0;
                        contabil_evento.descricao = "";
                        contabil_evento.conta_contabil = "";
                        contabil_evento.despesa = 0;
                    }
                //}
                //else
                //{
                //    contabil_evento.conta_contabil = value.ToString();
                //    contabil_evento.descricao = "CONTA CONTABIL DO FORNECEDOR";
                //    eventoContabilCodigo = 0;
                //}

            }
        }
        


        public conta_a_pagarDAO(User usr)
        {
            if (usr != null)
            {
                this.Filial = usr.getFilial();
                this.usr = usr;
            }
        }
        public conta_a_pagarDAO(String documento, String fornecedor, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  conta_a_pagar where ltrim(rtrim(Documento)) ='" + documento + "' and fornecedor='" + fornecedor + "' and filial='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, false);
            carregarDados(rs);
        }


        public void carregarContasPagarGrupo()
        {
            Grupo_pagamento.Clear();
            if (cod_grupo_pagamento.Equals(""))
                return;


            String sql = "Select cp.filial " +
                               " ,cp.grupo_pagamento " +
                               " ,cp.Fornecedor " +
                               " ,CP.Documento " +
                               " ,CP.Vencimento " +
                               " ,Valor=(isnull(CP.Acrescimo,0)+isnull(CP.Valor,0))-isnull(CP.Desconto,0)" +
                               " ,CP.Status" +
                         " from conta_a_pagar as cp " +
                         " where  cp.filial ='" + Filial + "' and cp.grupo_pagamento = '" + cod_grupo_pagamento + "'";
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, usr, false);

                while (rs.Read())
                {
                    Contas_a_pagar_grupoDAO pg = new Contas_a_pagar_grupoDAO();
                    pg.Filial = rs["Filial"].ToString();
                    pg.Cod_grupo = cod_grupo_pagamento;
                    pg.Fornecedor = rs["Fornecedor"].ToString();
                    pg.Documento = rs["Documento"].ToString();
                    pg.Vencimento = Funcoes.dtTry(rs["Vencimento"].ToString());
                    pg.Valor = Funcoes.decTry(rs["Valor"].ToString());

                    if (rs["Status"].ToString().Equals("1"))
                    {
                        pg.Status = "ABERTO";
                    }
                    else if (rs["Status"].ToString().Equals("2"))
                    {
                        pg.Status = "CONCLUIDO";
                    }
                    else if (rs["Status"].ToString().Equals("3"))
                    {
                        pg.Status = "CANCELADO";
                    }

                    Grupo_pagamento.Add(pg);
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
                Documento = rs["Documento"].ToString();
                Fornecedor = rs["Fornecedor"].ToString();
                serie = Funcoes.intTry(rs["serie"].ToString());
                Filial = rs["Filial"].ToString();
                CentroCusto = rs["Codigo_Centro_Custo"].ToString();
                _valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);
                _desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                obs = rs["obs"].ToString();
                emissao = (rs["emissao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["emissao"].ToString()));
                Pagamento = (rs["Pagamento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Pagamento"].ToString()));
                _tipoPagamento = rs["Tipo_Pagamento"].ToString();
                Duplicata = rs["Duplicata"].ToString().Equals("1") ? true : false;
                Numero_cheque = rs["Numero_cheque"].ToString();
                Valor_Pago = (Decimal)(rs["Valor_Pago"].ToString().Equals("") ? new Decimal() : rs["Valor_Pago"]);
                _Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                id_cc = rs["id_cc"].ToString();
                Baixa_Automatica = (rs["Baixa_Automatica"].ToString().Equals("1") ? true : false);
                usuario = rs["usuario"].ToString();
                status = rs["status"].ToString();
                documento_emitido = rs["documento_emitido"].ToString();
                entrada = (rs["entrada"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["entrada"].ToString()));
                Parcial = (Decimal)(rs["Parcial"].ToString().Equals("") ? new Decimal() : rs["Parcial"]);
                _acrescimo = (Decimal)(rs["acrescimo"].ToString().Equals("") ? new Decimal() : rs["acrescimo"]);
                BOLETO_RECEBIDO = (rs["BOLETO_RECEBIDO"].ToString().Equals("1") ? true : false);
                conferido = (rs["conferido"].ToString().Equals("1") ? true : false);
                lancamento_simples = (rs["lancamento_simples"].ToString().Equals("1") ? true : false);
                cod_grupo_pagamento = rs["Grupo_pagamento"].ToString();
                qtdeParcelas = Funcoes.intTry(rs["qtde_parcelas"].ToString());
                tipoParcela = rs["tipoParcela"].ToString();
                qtdeDias = Funcoes.intTry(rs["qtde_dias"].ToString());
                forcaDiaUtilVencimento = rs["forcaDiaUltiVencimento"].ToString().Equals("1");
                cod_barras = rs["Cod_barras"].ToString();

                try
                {
                    if (Funcoes.intTry(rs["evento_contabil"].ToString()) > 0)
                    {
                        eventoContabil = Funcoes.intTry(rs["evento_contabil"].ToString());
                        origemCodigoContabilEvento = true;
                    }
                    else
                    {
                        origemCodigoContabilEvento = false;
                        fornecedorDAO fornContabil = new fornecedorDAO(Fornecedor, usr);
                        eventoContabil = int.Parse(fornContabil.conta_contabil_credito);
                    }
                }
                catch
                {
                    origemCodigoContabilEvento = false;
                }

                carregarContasPagarGrupo();
            }
            if (rs != null)
                rs.Close();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.

        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  conta_a_pagar set " +
                              "Codigo_Centro_Custo='" + Codigo_Centro_Custo + "'" +
                              ",Valor=" + Valor.ToString().Replace(",", ".") +
                              ",Desconto=" + Desconto.ToString().Replace(",", ".") +
                              ",obs='" + obs + "'" +
                              ",emissao='" + emissao.ToString("yyyy-MM-dd") + "'" +
                              ",Pagamento=" + (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Pagamento.ToString("yyyy-MM-dd") + "'") +
                              ",Tipo_Pagamento='" + Tipo_Pagamento + "'" +
                              ",Duplicata=" + (Duplicata ? "1" : "0") +
                              ",Numero_cheque='" + Numero_cheque + "'" +
                              ",Valor_Pago=" + Valor_Pago.ToString().Replace(",", ".") +
                              ",Vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "'" +
                              ",id_cc='" + id_cc + "'" +
                              ",Baixa_Automatica=" + (Baixa_Automatica ? "1" : "0") +
                              ",usuario='" + usuario + "'" +
                              ",status=" + status +
                              ",entrada='" + entrada.ToString("yyyy-MM-dd") + "'" +
                              ",Parcial=" + Parcial.ToString().Replace(",", ".") +
                              ",conta='" + conta + "'" +
                              ",BOLETO_RECEBIDO=" + (BOLETO_RECEBIDO ? "1" : "0") +
                              ",acrescimo=" + acrescimo.ToString().Replace(",", ".") +
                              ",conferido=" + (conferido ? "1" : "0") +
                              ",lancamento_simples=" + (lancamento_simples ? "1" : "0") +
                              ",Grupo_pagamento='" + cod_grupo_pagamento + "'" +
                              ",serie ="+ serie+
                              ",cod_barras='"+cod_barras+"'"+
                              ",evento_contabil = " + eventoContabil.ToString() + 
                              ",Integrado = 0" +
                    " where documento= '" + Documento + "' and fornecedor = '" + Fornecedor + "'  and documento_emitido='" + documento_emitido + "' and filial='" + Filial + "' and serie ="+serie;
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

                if (qtdeParcelas > 1)
                {
                    if (!Documento.Contains("-01"))
                        Documento += "-01";

                    String documInicial = Documento;
                    DateTime VencIni = Vencimento;
                    int nParc = 1;
                    cod_grupo_pagamento = Funcoes.sequencia("GRUPO_DE_PAGAMENTOS.COD_GRUPO", usr, conn, tran);

                    Conexao.executarSql("insert into grupo_de_pagamentos " +
                                         " values('" + usr.getFilial() + "', '" + cod_grupo_pagamento + "', '" + Documento + "', '" + Fornecedor + "')", conn, tran);

                    Funcoes.salvaProximaSequencia("GRUPO_DE_PAGAMENTOS.COD_GRUPO", usr, conn, tran);
                    while (nParc <= qtdeParcelas)
                    {
                        if (!Documento.Contains("-" + nParc.ToString().PadLeft(2, '0')))
                            Documento += "-" + nParc.ToString().PadLeft(2, '0');


                        insert(conn, tran);
                        if (tipoParcela.Equals("Mês"))
                            Vencimento = VencIni.AddMonths(nParc);
                        else
                            Vencimento = VencIni.AddDays((qtdeDias * nParc));
                       
                        if(forcaDiaUtilVencimento)
                        {
                            Vencimento = Funcoes.DiaUtil(Vencimento);
                        }

                        Documento = Documento.Replace("-" + nParc.ToString().PadLeft(2, '0'), "");
                        nParc++;
                    }

                    Documento = documInicial;
                    Vencimento = Vencimento;

                }
                else
                {

                    insert(conn, tran);
                }
            }
            else
            {
                update(conn, tran);
            }
            if (status.Equals("2") && !id_cc.Equals(""))
            {
                Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) -" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial='" + usr.getFilial() + "'", conn, tran);
                String strhist = "insert into Historico_mov_conta " +
                                    " values(" +
                                         "'" + usr.getFilial() + "'," +
                                         " GETDATE()," +
                                         "'" + Pagamento.ToString("yyyy-MM-dd") + "'," +
                                         "'" + id_cc + "'," +
                                         "'CONTA_A_PAGAR'," +
                                         "'" + Fornecedor + "'," +
                                         "'" + Documento + "'," +
                                         "'" + Tipo_Pagamento + "'," +
                                         "'BAIXA'," +
                                          Funcoes.decimalPonto(ValorPagar.ToString("N2")) + "," +
                                         "'" + usr.getUsuario() + "'" +
                                         ",0" +
                                     ")";

                Conexao.executarSql(strhist, conn, tran);

                String strCorrigHistDia = " exec SP_CORRIGI_HIST_BANCARIO '" + usr.getFilial() + "' , '" + id_cc + "' ,'" + Pagamento.ToString("yyyy-MM-dd") + "'";
                Conexao.executarSql(strCorrigHistDia, conn, tran);




                //Rotina para lançamento contábil.
                if (Funcoes.valorParametro("EVENTOS_CONTABEIS", null).ToUpper().Equals("TRUE"))
                {

                    if (!contabil_evento.conta_contabil.Equals(""))
                    {
                        try
                        {
                            conta_correnteDAO contaCorrente = new conta_correnteDAO(id_cc, usr);
                            fornecedorDAO fornecedorContabil = new fornecedorDAO(Fornecedor, usr);

                            lancamento_contabilDAO lctoContabil = new lancamento_contabilDAO();
                            lctoContabil.filial = usr.getFilial();
                            lctoContabil.clienteFornecedor = Fornecedor;
                            lctoContabil.numeroDocumento = Documento;
                            lctoContabil.idlancamento = 0;
                            lctoContabil.data = Pagamento;
                            if (!fornecedorContabil.Tipo_fornecedor || contabil_evento.codigo > 0)
                            {
                                lctoContabil.contaCredito = contaCorrente.conta_contabil;
                                lctoContabil.contaDebito = contabil_evento.conta_contabil;
                            }
                            else
                            {
                                lctoContabil.contaDebito = contaCorrente.conta_contabil;
                                lctoContabil.contaCredito = contabil_evento.conta_contabil;
                            }
                            lctoContabil.complemento = Fornecedor + " " + this.obs;
                            lctoContabil.valor = Valor_Pago;

                            lctoContabil.insert(conn, tran);

                        }
                        catch
                        {
                            //Sem tratamento
                        }
                    }
                }
            }

            if (cod_grupo_pagamento.Length > 0)
            {
                carregarContasPagarGrupo();
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
                /*
                 if (status.Equals("2") && !id_cc.Equals(""))
                 {
                     Conexao.executarSql("update conta_corrente set salto = saldo -" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc + "'",cnn,trans);
                 }
                 */
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

        public bool estornar()
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                status = "1";
                if (!id_cc.Equals(""))
                {

                    String sqlEstornoMov = "UPDATE HISTORICO_MOV_CONTA SET ESTORNADO = 1 " +
                                          " where " +
                                          " FILIAL = '" + usr.getFilial() + "'" +
                                          " and pagamento= '" + Pagamento.ToString("yyyy-MM-dd") + "'" +
                                          " and id_cc ='" + id_cc + "'" +
                                          " and origem = 'CONTA_A_PAGAR'" +
                                          " and cliente_fornecedor='" + Fornecedor + "'" +
                                          " and documento = '" + Documento + "'" +
                                          " and forma_pg='" + Tipo_Pagamento + "'" +
                                          " AND OPERACAO = 'BAIXA'" +
                                          " and vlr = " + Funcoes.decimalPonto(Valor_Pago.ToString("N2")) +
                                          " AND ISNULL(ESTORNADO ,0) = 0 ;";

                    Conexao.executarSql(sqlEstornoMov, cnn, trans);


                    Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) +" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial ='" + usr.getFilial() + "'", cnn, trans);
                    String strhist = "insert into Historico_mov_conta " +
                                     " values(" +
                                            "'" + usr.getFilial() + "'," +
                                         " GETDATE()," +
                                         "'" + Pagamento.ToString("yyyy-MM-dd") + "'," +
                                          "'" + id_cc + "'," +
                                          "'CONTA_A_PAGAR'," +
                                          "'" + Fornecedor + "'," +
                                          "'" + Documento + "'," +
                                          "'" + Tipo_Pagamento + "'," +
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


                //Jailson 04.02.2022
                if (Funcoes.valorParametro("EVENTOS_CONTABEIS", null).ToUpper().Equals("TRUE"))
                {
                    lancamento_contabilDAO lctoContabil = new lancamento_contabilDAO(usr.getFilial(), Fornecedor, Documento);
                    if (!lctoContabil.numeroDocumento.Equals(""))
                    {
                        lctoContabil.delete(cnn, trans);
                    }
                }

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
        public bool excluir()
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                status = "3";
                String sql = "update conta_a_pagar set status=3 , documento=documento+'C' where  documento='" + Documento + "' and fornecedor='" + Fornecedor + "'"; //colocar campo index
                Conexao.executarSql(sql, cnn, trans);

                if (!id_cc.Equals(""))
                {
                    Conexao.executarSql("update conta_corrente set saldo = isnull(saldo,0) -" + Valor_Pago.ToString("N2").Replace(".", "").Replace(",", ".") + " where id_cc='" + id_cc.Trim() + "' and filial='" + usr.getFilial() + "'", cnn, trans);
                }

                //Conexao.executarSql("Delete from conta_a_pagar  where documento= '" + Documento + "' and fornecedor = '" + Fornecedor + "'  and documento_emitido='" + documento_emitido + "' and filial='" + Filial + "' ", cnn, trans);
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
                bool AtSequencia = false;
                if (Documento.Trim().Equals(""))
                {
                    AtSequencia = true;
                    Documento = DateTime.Now.ToString("yyyyMM") + '-' + Funcoes.sequencia("CONTA_A_PAGAR.DOCUMENTO", usr);
                }

                String sqlVerDub = "Select count(*) from conta_a_pagar where documento='" + Documento + "' and fornecedor ='" + Fornecedor + "' and filial ='" + Filial + "' and serie="+serie;
                bool bExiste = int.Parse(Conexao.retornaUmValor(sqlVerDub, usr)) > 0;
                if (bExiste)
                {
                    throw new Exception("Já Existe um Documento:" + Documento + " Para o Fornecedor: " + Fornecedor);
                }

                String sql = " insert into conta_a_pagar (" +
                          "Documento," +
                          "Fornecedor," +
                          "Filial," +
                          "Codigo_Centro_Custo," +
                          "Valor," +
                          "Desconto," +
                          "obs," +
                          "emissao," +
                          "Pagamento," +
                          "Tipo_Pagamento," +
                          "Duplicata," +
                          "Numero_cheque," +
                          "Valor_Pago," +
                          "Vencimento," +
                          "id_cc," +
                          "Baixa_Automatica," +
                          "usuario," +
                          "status," +
                          "documento_emitido," +
                          "entrada," +
                          "Parcial," +
                          "conta," +
                          "BOLETO_RECEBIDO," +
                          "acrescimo " +
                          ",conferido" +
                          ",lancamento_simples" +
                          ",grupo_pagamento" +
                          ",qtde_parcelas" +
                          ",tipoParcela"+
                          ",qtde_Dias"+
                          ",forcaDiaUltiVencimento" +
                          ",serie"+
                          ",cod_barras"+
                          ",evento_contabil"+
                     " )values( " +
                          "'" + Documento + "'" +
                          "," + "'" + Fornecedor + "'" +
                          "," + "'" + Filial + "'" +
                          "," + "'" + Codigo_Centro_Custo + "'" +
                          "," + Valor.ToString().Replace(",", ".") +
                          "," + Desconto.ToString().Replace(",", ".") +
                          "," + "'" + obs + "'" +
                          "," + (emissao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? DateTime.Now.ToString("yyyy-MM-dd") : "'" + emissao.ToString("yyyy-MM-dd") + "'") +
                          "," + (Pagamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Pagamento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Tipo_Pagamento + "'" +
                          "," + (Duplicata ? "1" : "0") +
                          "," + "'" + Numero_cheque + "'" +
                          "," + Valor_Pago.ToString().Replace(",", ".") +
                          "," + (Vencimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + id_cc + "'" +
                          "," + (Baixa_Automatica ? 1 : 0) +
                          "," + "'" + usuario + "'" +
                          "," + status +
                          "," + "'" + documento_emitido + "'" +
                          "," + (entrada.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + entrada.ToString("yyyy-MM-dd") + "'") +
                          "," + Parcial.ToString().Replace(",", ".") +
                          "," + "'" + conta + "'" +
                          "," + (BOLETO_RECEBIDO ? 1 : 0) +
                          "," + acrescimo.ToString().Replace(",", ".") +
                          "," + (conferido ? 1 : 0) +
                          "," + (lancamento_simples ? "1" : "0") +
                          ",'" + cod_grupo_pagamento + "'" +
                          ","+qtdeParcelas.ToString()+
                          ",'"+tipoParcela+"'" +
                          ","+ qtdeDias.ToString()+
                          ","+(forcaDiaUtilVencimento?"1":"0")+
                          ","+serie+
                          ",'"+cod_barras+"'"+
                          ", " +eventoContabil.ToString()+ 
                         ");";

                Conexao.executarSql(sql, conn, tran);

                if (AtSequencia)
                {
                    Funcoes.salvaProximaSequencia("CONTA_A_PAGAR.DOCUMENTO", usr);
                }
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }

        public static void atualizarDoc(String filial, String nDoc, String Fornecedor, Decimal vlr, Decimal acrescimo, Decimal desc, DateTime dt,String codCentroCusto,String tipoPagamento)
        {
            String sql = "Update conta_a_pagar set valor=" + Funcoes.decimalPonto(vlr.ToString("N2"));

            if (!dt.Equals(DateTime.MinValue))
                sql += ",vencimento='" + dt.ToString("yyyyMMdd") + "'";

                sql += ",acrescimo =" + Funcoes.decimalPonto(acrescimo.ToString("N2")) +
                        ",desconto =" + Funcoes.decimalPonto(desc.ToString("N2")) +
                        ",codigo_centro_custo='"+codCentroCusto+"'"+
                        ",Tipo_Pagamento ='"+tipoPagamento+"'"+
                " where filial='" + filial + "' and documento='" + nDoc + "' and fornecedor ='" + Fornecedor + "'";
            try
            {
                Conexao.executarSql(sql);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

