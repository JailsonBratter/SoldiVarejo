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
    public class status_pdvDAO
    {
        public int id_fechamento = 0;
        public int Id_Operador = 0;
        public String Filial { get; set; }
        public DateTime Data_Abertura = new DateTime();
        public DateTime data_fechamento_movimento = new DateTime();
        public String Data_AberturaBr()
        {
            return dataBr(Data_Abertura);
        }

        public int Pdv = 0;
        public String Usuario_Fechamento { get; set; }
        public String Status = "OPERANDO";
        public DateTime Data_Fechamento { get; set; }
        public Decimal vDiferenca = 0;
        User usr = null;
        public String Data_FechamentoBr()
        {
            return dataBr(Data_Fechamento);
        }

        public ArrayList arrItensTesouraria = new ArrayList();


        public status_pdvDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }
        public void addItem(tesourariaDAO tesouraria)
        {
            tesouraria.PDV = this.Pdv;
            tesouraria.DATA_ABERTURA = this.Data_Abertura;
            tesouraria.ID_OPERADOR = this.Id_Operador;
            tesouraria.FILIAL = this.Filial;
            vDiferenca += (tesouraria.Total_Entregue - tesouraria.Total_Sistema);
            arrItensTesouraria.Add(tesouraria);
        }

        public status_pdvDAO(int id_operador, int pdv, int idfecha, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.Id_Operador = id_operador;
            this.Pdv = pdv;
       
            this.Filial = usr.getFilial();
            this.id_fechamento = idfecha;
            String sql = "Select * from  status_pdv  where Id_Operador=" + Id_Operador +
                                      " and Filial='" + Filial + "'" +
                                      " and Pdv=" + Pdv+
                                      " and id_Fechamento="+idfecha;
            
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public DataTable tbCancelados()
        {
            String sql = " EXEC sp_tesouraria_cancelados " +
                            "@filial='"+usr.getFilial()+"'" +
                            ",@id_movimento="+id_fechamento+
                            ",@pdv="+Pdv+
                            ",@id_funcionario="+Id_Operador;

            return Conexao.GetTable(sql, usr, false);
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
                    Id_Operador = (rs["Id_Operador"] == null ? 0 : int.Parse(rs["Id_Operador"].ToString()));
                    Filial = rs["Filial"].ToString();
                    Data_Abertura = Funcoes.dtTry(rs["Data_Abertura"].ToString());
                    Pdv = (rs["Pdv"] == null ? 0 : int.Parse(rs["Pdv"].ToString()));
                    Usuario_Fechamento = rs["Usuario_Fechamento"].ToString();
                    Status = rs["Status"].ToString().Equals("") ? "ABERTO" : rs["Status"].ToString();
                    Data_Fechamento = (rs["Data_Fechamento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Fechamento"].ToString()));
                    int.TryParse(rs["id_fechamento"].ToString(), out id_fechamento);
                    data_fechamento_movimento = Funcoes.dtTry(rs["data_fechamento_movimento"].ToString());
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
                //String sql = "update  status_pdv set " +
                //                  "Usuario_Fechamento='" + Usuario_Fechamento + "'" +
                //                  ",Status='" + Status + "'" +
                //                  ",Data_Fechamento=" + (Data_Fechamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Fechamento.ToString("yyyy-MM-dd") + "'") + "," +
                //            "  where Id_Operador=" + Id_Operador +
                //                      " and Filial='" + Filial + "'" +
                //                      " and Data_Abertura=" + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd") + "'") +
                //                      " and Pdv=" + Pdv;

                //Conexao.executarSql(sql, conn, tran);
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
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "delete from status_pdv    where Id_Operador=" + Id_Operador +
                              " and Filial='" + Filial + "'" +
                              //" and Data_Abertura=" + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd") + "'") +
                              " and Pdv=" + Pdv+
                              " and id_fechamento ="+id_fechamento+"";

            Conexao.executarSql(sql, conn, tran);

            String sqlDeteleItens = "delete from tesouraria where Id_Operador=" + Id_Operador +
                              " and Filial='" + Filial + "'" +
                              //" and Data_Abertura=" + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd") + "'") +
                              " and Pdv=" + Pdv+
                              " and id_fechamento =" + id_fechamento + "";

            Conexao.executarSql(sqlDeteleItens, conn, tran);

            return true;
        }

        public void limparDetalhes()
        {
            Conexao.executarSql("Delete from tesouraria_detalhes where id_fechamento ="+id_fechamento+" and filial='"+Filial+"' and pdv="+Pdv+" and operador="+Id_Operador);
        }

        public void inserirDetalhes()
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                inserirDetalhes(cnn, trans);

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

        }
        private void reabrirFechamento(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "UPDATE  status_pdv SET " +
                                  "Status_Pdv.Status='" + Status + "'" +
                                  ", Status_Pdv.Data_Fechamento = NULL" +
                            "  WHERE Id_Operador=" + Id_Operador +
                                      " and Filial='" + Filial + "'" +
                                      " AND Id_Fechamento = " + this.id_fechamento.ToString() +
                                      " AND id_Operador = " + this.Id_Operador.ToString() +
                                      " AND PDV = " + this.Pdv.ToString() +
                                      " AND Data_Abertura=" + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd HH:mm:ss") + "'");

                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel executar reabertura da tesouraria:" + err.Message);
            }
        }




        private void inserirDetalhes(SqlConnection conn, SqlTransaction tran)
        {
            String sqlDetalhes = " insert into tesouraria_detalhes (filial,emissao,pdv,cupom,operador,total,finalizadora,id_finalizadora,id_cartao,autorizacao,id_bandeira,rede_cartao,hora_venda,vencimento,taxa,id_fechamento, Sequencia)" +
                                                " Select lf.filial, lf.Emissao, lf.pdv,lf.cupom,lf.operador , lf.total,lf.finalizadora,lf.id_finalizadora, cartao.id_cartao,lf.autorizacao," +
                                                "lf.id_Bandeira," + 
                                                "case when isnull(lf.autorizacao,'')in ('','0') then '' else  lf.rede_cartao   end" +  //autorizacao não preenchida não é operação pelo tef
                                                ",(Select max(hora_venda) from Saida_Estoque as se where  se.Filial=lf.filial and se.Data_movimento= lf.Emissao and se.Caixa_Saida=lf.pdv and se.Documento=lf.cupom )as hora " +
                                                ", lf.vencimento, lf.taxa, lf.id_movimento, lf.Sequencia "+ 
                                                "    from Lista_finalizadora lf   " +
                                                "     left join  Cartao on cartao.nro_Finalizadora=lf.finalizadora and  " +
                                                "             convert( int ,lf.rede_cartao)= convert(int,Cartao.id_rede) and " +
                                                "             convert(int,lf.id_Bandeira)= convert(int ,Cartao.id_bandeira) " +
                                                "     left join tesouraria_detalhes td  on " +
                                                "         lf.cupom = td.cupom and " +
                                                "         lf.emissao = td.emissao and  " +
                                                "         lf.pdv = td.pdv  and" +
                                                "         lf.filial = td.filial and" +
                                                "         td.cupom is null		" +
                                                " where	(" +
                                                "        lf.Emissao >=" + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd") + "'") + " and  " +
                                                "         lf.pdv=" + Pdv + " and " +
                                                "         lf.operador = " + Id_Operador + " and " +
                                                "         lf.filial='" + this.Filial + "' and " +
                                                "         lf.id_movimento = " + this.id_fechamento + " AND " +
                                                "         isnull(lf.Cancelado,0) <>1" +
                                                "             ) " +
                                                " order by cartao.id_cartao ";

            Conexao.executarSql(sqlDetalhes, conn, tran);
        }

        public void encerrar(User usr)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                SqlDataReader rsTes = null;
                String sql = "";
                if (Funcoes.valorParametro("CARTAO_SEMTEF", usr).ToUpper().Equals("TRUE"))
                {
                    sql = " SELECT A.* FROM ";
                    sql += " (SELECT T.DATA_ABERTURA AS Emissao, Vencimento = master.dbo.F_BR_PROX_DIA_UTIL(t.Data_Abertura + isnull(Cartao.dias, 0)),";
                    sql += " T.Finalizadora, Id_Finalizadora = F.Finalizadora, Rede_Cartao = ISNULL(Cartao.id_Rede, ''), ID_Cartao = ISNULL(Cartao.id_cartao, ''),";
                    sql += " ID_Bandeira = ISNULL(cartao.id_Bandeira, ''), Total = T.Total_Sistema, Taxa = CASE WHEN ISNULL(Cartao.Taxa, 0) > 0 THEN ";
                    sql += " CONVERT(DECIMAL(12, 2), ((T.TOTAL_Entregue * Cartao.taxa) / 100)) ELSE 0 END,";
                    sql += " Centro_Custo = ISNULL(Cartao.centro_custo, F.Codigo_centro_custo), dif = (T.Total_Entregue - T.Total_Sistema)";
                    sql += " FROM TESOURARIA T INNER JOIN FinaliZadora F ON T.Finalizadora = F.Nro_Finalizadora ";
                    sql += " LEFT OUTER JOIN Cartao ON T.FINALIZADORA = Cartao.Nro_Finalizadora";
                    sql += " WHERE t.DATA_ABERTURA = " + Funcoes.dateSql(Data_Abertura) + " and T.id_fechamento = " + id_fechamento + " AND T.pdv =  " + Pdv + " AND ";
                    sql += " T.ID_OPERADOR = " + Id_Operador.ToString() + ") AS A WHERE A.Total + A.dif > 0";
                }
                else
                {
                    sql = "Select emissao,vencimento, finalizadora ,id_finalizadora, rede_cartao,td.id_cartao,td.id_bandeira, sum(total) as total, sum(td.taxa) as taxa ,c.centro_custo " +
                                 ",dif = (Select top 1 ( Total_Entregue - Total_Sistema ) as Dif from tesouraria where  id_fechamento  = td.id_fechamento and pdv = td.pdv and ID_OPERADOR = td.operador and id_finalizadora= td.id_finalizadora)" +
                                 " from tesouraria_detalhes as td left join cartao as c on td.id_cartao = c.id_cartao and c.id_Bandeira = td.id_bandeira and td.rede_cartao = c.id_Rede" +
                                 " where emissao = " + Funcoes.dateSql(Data_Abertura) + " and id_fechamento = " + id_fechamento + " and td.pdv = " + Pdv + " and td.operador=  " + Id_Operador +
                                 " group by emissao, vencimento,td.id_fechamento, td.pdv,td.operador, td.finalizadora,td.id_finalizadora,rede_cartao,td.id_bandeira,td.id_cartao, c.centro_custo";
                }

                try
                {
                    Hashtable finaDif = new Hashtable();
                    rsTes = Conexao.consulta(sql, usr, false);
                    while(rsTes.Read())
                    {
                        String numeroDoc = Data_Abertura.ToString("ddMMyy") +
                                           rsTes["rede_cartao"].ToString().PadLeft(3, '0') +
                                           rsTes["id_bandeira"].ToString().PadLeft(5, '0') +
                                           "-" + rsTes["finalizadora"].ToString().PadLeft(2, '0');


                        conta_a_receberDAO rec = new conta_a_receberDAO(usr,numeroDoc);
                        bool novo = (rec.Documento == null);
                        rec.Documento = numeroDoc;
                        rec.finalizadora = Funcoes.intTry( rsTes["finalizadora"].ToString());
                        rec.id_finalizadora = rsTes["id_finalizadora"].ToString();
                        rec.id_bandeira = rsTes["id_bandeira"].ToString();
                        rec.rede_cartao = rsTes["rede_cartao"].ToString();
                        rec.Emissao = Data_Abertura;
                        rec.entrada = Data_Abertura;
                        rec.Vencimento = Funcoes.dtTry(rsTes["vencimento"].ToString());
                        rec.taxa += Funcoes.decTry(rsTes["taxa"].ToString());
                        rec.Valor += Funcoes.decTry(rsTes["total"].ToString()) + Funcoes.decTry(rsTes["dif"].ToString());
                        decimal dif = Funcoes.decTry(rsTes["dif"].ToString());
                        
                        rec.status = 1;
                        rec.documento_emitido = "9999999999";
                        rec.usuario = usr.getUsuario();
                        rec.Codigo_Centro_Custo = rsTes["centro_custo"].ToString();
                        if (rec.Codigo_Centro_Custo.Equals(""))
                        {
                            rec.Codigo_Centro_Custo = Funcoes.valorParametro("CENTRO_CUSTO_REC", usr);
                        }
                        rec.Obs += (novo ? "Lançamento Automatico tesouraria \n" : "\n") +
                        "Operador:" + this.Id_Operador.ToString().PadRight(5, ' ') +
                        " PDV:" + this.Pdv.ToString().PadRight(5, ' ') +
                        " Taxa:" + Funcoes.decTry(rsTes["taxa"].ToString()).ToString("N2").PadLeft(10, ' ') +
                        " Total:" + (Funcoes.decTry(rsTes["total"].ToString()) + Funcoes.decTry(rsTes["dif"].ToString())).ToString("N2").PadLeft(10, ' ');
                        if (dif < 0 && !finaDif.ContainsKey(rec.id_finalizadora))
                        {
                            finaDif.Add(rec.id_finalizadora, dif);
                            //rec.Valor += dif;
                            rec.Obs += " *Quebra: " + dif.ToString("N2");
                        }

                        rec.salvar(novo, cnn,trans);
                        

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsTes != null)
                        rsTes.Close();
                }
              

                this.Status = "FECHADO";
                this.Usuario_Fechamento = usr.getNome();
                this.Data_Fechamento = DateTime.Now;
                salvar(true, cnn, trans);


                if (vDiferenca != 0)
                {
                    String strDocumento = Data_Abertura.ToString("ddMMyy") + id_fechamento.ToString().PadLeft(4, '0') + Pdv.ToString().PadLeft(2, '0') + Id_Operador.ToString().PadLeft(2, '0');
                    String strSqlCr = "select count(*) from conta_a_receber where documento ='" + strDocumento + "'";
                    int bExists = 0;
                    int.TryParse(Conexao.retornaUmValor(strSqlCr, usr, cnn, trans), out bExists);

                    if (bExists >= 1)
                    {
                        Conexao.executarSql("delete from conta_a_receber where filial ='" + usr.getFilial() + "' and documento ='" + strDocumento + "'", cnn, trans);
                    }


                    conta_a_receberDAO rec = new conta_a_receberDAO(usr);
                    rec.Documento = strDocumento;
                    rec.operador = Id_Operador.ToString();
                    rec.pdv = Pdv;
                    rec.entrada = DateTime.Now;
                    rec.Emissao = Data_Abertura;
                    rec.Vencimento = Data_Abertura;
                    rec.usuario = usr.getUsuario();
                    if (vDiferenca < 0)
                    {
                        rec.Valor = (vDiferenca * -1);
                    }
                    else
                    {
                        rec.Valor = vDiferenca;
                    }
                    rec.status = 1;

                    if (vDiferenca < 0)
                    {
                        rec.Obs = "Recibo de Quebra de caixa do Operador:" + Id_Operador + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + Id_Operador, usr); ;
                    }
                    else if (vDiferenca > 0)
                    {
                        rec.Obs = "Recibo de Sobra de caixa do Operador:" + Id_Operador + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + Id_Operador, usr); ;
                    }
                    rec.Baixa_Automatica = true;
                    rec.id_movimento = id_fechamento.ToString();
                    rec.documento_emitido = "9999999999";
                    rec.Codigo_Centro_Custo = Funcoes.valorParametro("CENTRO_CUSTO_REC", usr);
                    rec.salvar(true, cnn, trans);

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


        }


        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                excluir(conn, tran);
              

                String sql = " insert into status_pdv (" +
                          "Id_Operador," +
                          "Filial," +
                          "Data_Abertura," +
                          "Pdv," +
                          "Usuario_Fechamento," +
                          "Status," +
                          "Data_Fechamento" +
                          ",Data_fechamento_movimento"+
                          ",id_fechamento"+
                     " )values (" +
                           Id_Operador +
                          "," + "'" + Filial + "'" +
                          "," + (Data_Abertura.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Abertura.ToString("yyyy-MM-dd HH:mm") + "'") +
                          "," + Pdv +
                          "," + "'" + Usuario_Fechamento + "'" +
                          "," + "'" + Status + "'" +
                          "," + (Data_Fechamento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Fechamento.ToString("yyyy-MM-dd HH:mm") + "'") +
                          "," + (data_fechamento_movimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_fechamento_movimento.ToString("yyyy-MM-dd HH:mm") + "'") +
                          ","+id_fechamento.ToString()+
                         ");";

                Conexao.executarSql(sql, conn, tran);

                foreach (tesourariaDAO item in arrItensTesouraria)
                {
                    item.id_fechamento = this.id_fechamento;
                    item.salvar(true, conn, tran);
                }
             

                //inserirDetalhes(this.id_fechamento.ToString(), conn, tran);

            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }

        public void estornarFechamento(User usr)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                SqlDataReader rsTes = null;
                String sql = "";
                if (Funcoes.valorParametro("CARTAO_SEMTEF", usr).ToUpper().Equals("TRUE"))
                {
                    sql = " SELECT A.* FROM ";
                    sql += " (SELECT T.DATA_ABERTURA AS Emissao, Vencimento = master.dbo.F_BR_PROX_DIA_UTIL(t.Data_Abertura + isnull(Cartao.dias, 0)),";
                    sql += " T.Finalizadora, Id_Finalizadora = F.Finalizadora, Rede_Cartao = ISNULL(Cartao.id_Rede, ''), ID_Cartao = ISNULL(Cartao.id_cartao, ''),";
                    sql += " ID_Bandeira = ISNULL(cartao.id_Bandeira, ''), Total = T.Total_Sistema, Taxa = CASE WHEN ISNULL(Cartao.Taxa, 0) > 0 THEN ";
                    sql += " CONVERT(DECIMAL(12, 2), ((T.TOTAL_Entregue * Cartao.taxa) / 100)) ELSE 0 END,";
                    sql += " Centro_Custo = ISNULL(Cartao.centro_custo, F.Codigo_centro_custo), dif = (T.Total_Entregue - T.Total_Sistema)";
                    sql += " FROM TESOURARIA T INNER JOIN FinaliZadora F ON T.Finalizadora = F.Nro_Finalizadora ";
                    sql += " LEFT OUTER JOIN Cartao ON T.FINALIZADORA = Cartao.Nro_Finalizadora";
                    sql += " WHERE t.DATA_ABERTURA = " + Funcoes.dateSql(Data_Abertura) + " and T.id_fechamento = " + id_fechamento + " AND T.pdv =  " + Pdv + " AND ";
                    sql += " T.ID_OPERADOR = 1) AS A WHERE A.Total + A.dif > 0";
                }
                else
                {
                    sql = "Select emissao,vencimento, finalizadora ,id_finalizadora, rede_cartao,td.id_cartao,td.id_bandeira, sum(total) as total, sum(td.taxa) as taxa ,c.centro_custo " +
                            ",dif = (Select top 1 ( Total_Entregue - Total_Sistema ) as Dif from tesouraria where  id_fechamento  = td.id_fechamento and pdv = td.pdv and ID_OPERADOR = td.operador and id_finalizadora= td.id_finalizadora)" +
                            " from tesouraria_detalhes as td left join cartao as c on td.id_cartao = c.id_cartao and c.id_Bandeira = td.id_bandeira and td.rede_cartao = c.id_Rede" +
                            " where emissao = " + Funcoes.dateSql(Data_Abertura) + " and id_fechamento = " + id_fechamento + " and td.pdv = " + Pdv + " and td.operador=  " + Id_Operador +
                            " group by emissao, vencimento,td.id_fechamento, td.pdv,td.operador, td.finalizadora,td.id_finalizadora,rede_cartao,td.id_bandeira,td.id_cartao, c.centro_custo";
                }

                try
                {
                    Hashtable finaDif = new Hashtable();
                    rsTes = Conexao.consulta(sql, usr, false);
                    while (rsTes.Read())
                    {
                        String numeroDoc = Data_Abertura.ToString("ddMMyy") +
                                           rsTes["rede_cartao"].ToString().PadLeft(3, '0') +
                                           rsTes["id_bandeira"].ToString().PadLeft(5, '0') +
                                           "-" + rsTes["finalizadora"].ToString().PadLeft(2, '0');

                        conta_a_receberDAO rec = new conta_a_receberDAO(usr, numeroDoc);
                        bool existeLancamento = (rec.Documento == null ? false : true);
                        if (rec.status != null)
                        {
                            if (!rec.status.ToString().Equals("1"))
                            {
                                throw new Exception("Esta rotina só será executada se os lançamentos do contas a receber originados destes lançamentos estiverem com STATUS de ABERTO.");
                            }
                        }
                        else
                        {
                            throw new Exception("Esta rotina só será executada se os lançamentos do contas a receber originados destes lançamentos existirem.");
                        }

                        if (!existeLancamento)
                        {
                            throw new Exception("Esta rotina só será executada se os lançamentos do contas a receber originados destes lançamentos existirem.");
                        }

                        rec.Documento = numeroDoc;
                        rec.finalizadora = Funcoes.intTry(rsTes["finalizadora"].ToString());
                        rec.id_finalizadora = rsTes["id_finalizadora"].ToString();
                        rec.id_bandeira = rsTes["id_bandeira"].ToString();
                        rec.rede_cartao = rsTes["rede_cartao"].ToString();
                        rec.Emissao = Data_Abertura;
                        rec.entrada = Data_Abertura;
                        rec.Vencimento = Funcoes.dtTry(rsTes["vencimento"].ToString());
                        rec.taxa -= Funcoes.decTry(rsTes["taxa"].ToString());
                        rec.Valor -= Funcoes.decTry(rsTes["total"].ToString()) + Funcoes.decTry(rsTes["dif"].ToString());
                        decimal dif = Funcoes.decTry(rsTes["dif"].ToString());

                        rec.status = 1;
                        rec.documento_emitido = "9999999999";
                        rec.usuario = usr.getUsuario();
                        rec.Codigo_Centro_Custo = rsTes["centro_custo"].ToString();
                        if (rec.Codigo_Centro_Custo.Equals(""))
                        {
                            rec.Codigo_Centro_Custo = Funcoes.valorParametro("CENTRO_CUSTO_REC", usr);
                        }
                        rec.Obs += "\nESTORNO de Lançamento da tesouraria Usuário: " + usr.getUsuario() +"\n"  +
                        "Operador:" + this.Id_Operador.ToString().PadRight(5, ' ') +
                        " PDV:" + this.Pdv.ToString().PadRight(5, ' ') +
                        " Taxa:" + (Funcoes.decTry(rsTes["taxa"].ToString()) * -1).ToString("N2").PadLeft(10, ' ') +
                        " Total:" + (Funcoes.decTry(rsTes["total"].ToString()) * -1).ToString("N2").PadLeft(10, ' ');
                        if (dif < 0 && !finaDif.ContainsKey(rec.id_finalizadora))
                        {
                            finaDif.Add(rec.id_finalizadora, dif);
                            //rec.Valor -= dif;
                            rec.Obs += " *Quebra: " + dif.ToString("N2");
                        }
                        rec.salvar(false, cnn, trans);


                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsTes != null)
                        rsTes.Close();
                }

                this.Status = "ABERTO";
                this.Data_Fechamento = DateTime.MinValue;
                reabrirFechamento(cnn, trans);


                if (vDiferenca != 0)
                {
                    String strDocumento = Data_Abertura.ToString("ddMMyy") + id_fechamento.ToString().PadLeft(4, '0') + Pdv.ToString().PadLeft(2, '0') + Id_Operador.ToString().PadLeft(2, '0');
                    String strSqlCr = "select count(*) from conta_a_receber where documento ='" + strDocumento + "'";
                    int bExists = 0;
                    int.TryParse(Conexao.retornaUmValor(strSqlCr, usr, cnn, trans), out bExists);

                    if (bExists >= 1)
                    {
                        Conexao.executarSql("delete from conta_a_receber where filial ='" + usr.getFilial() + "' and documento ='" + strDocumento + "'", cnn, trans);
                    }

                    //conta_a_receberDAO rec = new conta_a_receberDAO(usr);
                    //rec.Documento = strDocumento;
                    //rec.operador = Id_Operador.ToString();
                    //rec.pdv = Pdv;
                    //rec.entrada = DateTime.Now;
                    //rec.Emissao = Data_Abertura;
                    //rec.Vencimento = Data_Abertura;
                    //rec.usuario = usr.getUsuario();
                    //if (vDiferenca < 0)
                    //{
                    //    rec.Valor = (vDiferenca * -1);
                    //}
                    //else
                    //{
                    //    rec.Valor = vDiferenca;
                    //}
                    //rec.status = 1;

                    //if (vDiferenca < 0)
                    //{
                    //    rec.Obs = "Recibo de Quebra de caixa do Operador:" + Id_Operador + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + Id_Operador, usr); ;
                    //}
                    //else if (vDiferenca > 0)
                    //{
                    //    rec.Obs = "Recibo de Sobra de caixa do Operador:" + Id_Operador + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + Id_Operador, usr); ;
                    //}
                    //rec.Baixa_Automatica = true;
                    //rec.id_movimento = id_fechamento.ToString();
                    //rec.documento_emitido = "9999999999";
                    //rec.Codigo_Centro_Custo = Funcoes.valorParametro("CENTRO_CUSTO_REC", usr);
                    //rec.salvar(true, cnn, trans);

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

        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from status_pdv";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ status_pdvDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>status_pdv</h1></center>
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
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Id_Operador" Text="Id_Operador" Visible="true" 
                    HeaderText="Id_Operador" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Abertura" Text="Data_Abertura" Visible="true" 
                    HeaderText="Data_Abertura" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pdv" Text="Pdv" Visible="true" 
                    HeaderText="Pdv" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Usuario_Fechamento" Text="Usuario_Fechamento" Visible="true" 
                    HeaderText="Usuario_Fechamento" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Status" Text="Status" Visible="true" 
                    HeaderText="Status" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Fechamento" Text="Data_Fechamento" Visible="true" 
                    HeaderText="Data_Fechamento" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/status_pdvDetalhes.aspx?campoIndex={0}" 
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
                 protected static status_pdvDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new status_pdvDAO();
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
                                        obj = new status_pdvDAO(index,usr);
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
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         txtid.Text=obj.id.ToString();
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         txtId_Operador.Text=obj.Id_Operador.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtData_Abertura.Text=obj.Data_AberturaBr();
                                         txtPdv.Text=obj.Pdv.ToString();
                                         txtUsuario_Fechamento.Text=obj.Usuario_Fechamento.ToString();
                                         txtStatus.Text=obj.Status.ToString();
                                         txtData_Fechamento.Text=obj.Data_FechamentoBr();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         obj.id=txtid.Text;
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         obj.Id_Operador=int.Parse(txtId_Operador.Text);
                                         obj.Filial=txtFilial.Text;
                                         obj.Data_Abertura=(txtData_Abertura.Text.Equals("")?new DateTime():DateTime.Parse(txtData_Abertura.Text));
                                         obj.Pdv=int.Parse(txtPdv.Text);
                                         obj.Usuario_Fechamento=txtUsuario_Fechamento.Text;
                                         obj.Status=txtStatus.Text;
                                         obj.Data_Fechamento=(txtData_Fechamento.Text.Equals("")?new DateTime():DateTime.Parse(txtData_Fechamento.Text));
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
       <center> <h1>Detalhes do status_pdv</h1></center>                  
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
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Id_Operador</p>
                   <asp:TextBox ID="txtId_Operador" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Abertura</p>
                   <asp:TextBox ID="txtData_Abertura" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Pdv</p>
                   <asp:TextBox ID="txtPdv" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Usuario_Fechamento</p>
                   <asp:TextBox ID="txtUsuario_Fechamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Status</p>
                   <asp:TextBox ID="txtStatus" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Fechamento</p>
                   <asp:TextBox ID="txtData_Fechamento" runat="server" ></asp:TextBox>
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

