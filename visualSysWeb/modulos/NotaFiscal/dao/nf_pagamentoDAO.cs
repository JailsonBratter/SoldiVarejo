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
    public class nf_pagamentoDAO
    {
        public DateTime Vencimento { get; set; }
        public String VencimentoBr()
        {
            return dataBr(Vencimento);
        }

        public DateTime emissao = new DateTime();
        public String Filial { get; set; }
        public String Codigo { get; set; }
        public int serie { get; set; }
        public String Cliente_Fornecedor { get; set; }
        public String centroCusto = "";
        public String Tipo_NF { get; set; }
        public String Tipo_pagamento { get; set; }
        public Decimal Valor { get; set; }
        public natureza_operacaoDAO NaturezaOperacao { get; set; }
        public int ordem = 0;
        public bool boleto_recebido { get; set; }
        private User usr = null;

        public String vCodigoNotaProdutor = "";
        public string cod_barras = "";
        public DateTime entrada = new DateTime();

        public String codigoDuplicata
        {
            get
            {
                return Codigo + "-" + ordem.ToString().PadLeft(2, '0');
            }
            set { }
        }


        public nf_pagamentoDAO(User usr)
        {
            this.usr = usr;
        }
        public nf_pagamentoDAO(DateTime Vencimento, String codigo, String tipoNf, String clienteFornecedor, User usr)
        {
            this.usr = usr;
            String sql = "Select * from  nf_pagamento where vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "' and codigo ='" + codigo + "' and tipo_nf = " + tipoNf + " and cliente_fornecedor = '" + clienteFornecedor + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public static ArrayList pagamentos(String codigo, String tipoNf,int serie, String clienteFornecedor, User usr)
        {
            ArrayList ArrPagamentos = new ArrayList();
            String sql = "Select * from  nf_pagamento where codigo ='" + codigo + "' and tipo_nf = " + tipoNf + " and cliente_fornecedor = '" + clienteFornecedor + "' and serie ="+serie.ToString();
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(sql, usr, true);
                while (rs.Read())
                {
                    nf_pagamentoDAO nfItem = new nf_pagamentoDAO(usr);
                    nfItem.Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                    nfItem.Filial = rs["Filial"].ToString();
                    nfItem.Codigo = rs["Codigo"].ToString();
                    nfItem.serie = Funcoes.intTry(rs["serie"].ToString());
                    nfItem.Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                    nfItem.Tipo_NF = rs["Tipo_NF"].ToString();
                    nfItem.Tipo_pagamento = rs["Tipo_pagamento"].ToString();
                    nfItem.Valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);
                    nfItem.boleto_recebido = (rs["boleto_recebido"].ToString().Equals("1") ? true : false);
                    nfItem.cod_barras = rs["cod_barras"].ToString();
                    nfItem.ordem = ArrPagamentos.Count + 1;
                    ArrPagamentos.Add(nfItem);

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
            return ArrPagamentos;
        }

        public ArrayList ArrToString()
        {
            ArrayList pagamento = new ArrayList();
            pagamento.Add(VencimentoBr());
            pagamento.Add(Filial);
            pagamento.Add(Codigo);
            pagamento.Add(Cliente_Fornecedor);
            pagamento.Add(Tipo_NF.ToString());
            pagamento.Add(Tipo_pagamento);
            pagamento.Add(Valor.ToString("N2"));
            pagamento.Add((boleto_recebido ? "1" : "0"));
            pagamento.Add(cod_barras.ToString());

            return pagamento;
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
                    Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                    Filial = rs["Filial"].ToString();
                    Codigo = rs["Codigo"].ToString();
                    Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                    Tipo_NF = rs["Tipo_NF"].ToString();
                    Tipo_pagamento = rs["Tipo_pagamento"].ToString();
                    Valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);
                    boleto_recebido = (rs["boleto_recebido"].ToString().Equals("1") ? true : false);
                    cod_barras = rs["cod_barras"].ToString();
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
                String sql = "update  nf_pagamento set " +
                              "Vencimento=" + (Vencimento.Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                              ",Filial='" + Filial + "'" +
                              ",Codigo='" + Codigo + "'" +
                              ",Cliente_Fornecedor='" + Cliente_Fornecedor + "'" +
                              ",Tipo_NF=" + Tipo_NF +
                              ",Tipo_pagamento='" + Tipo_pagamento + "'" +
                              ",Valor=" + Valor.ToString().Replace(",", ".") +
                              ",boleto_recebido=" + (boleto_recebido ? "1" : "0") +
                              ",serie ="+serie.ToString()+
                              ",cod_barras ='"+cod_barras+"'"+
                    "  where vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "' and codigo =" + Codigo + " and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'";
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
            return true;
        }
        public void atualizaFinanceiro(SqlConnection conn, SqlTransaction tran)
        {
            if (NaturezaOperacao != null && NaturezaOperacao.Gera_apagar_receber)
            {
                bool aVista = Conexao.retornaUmValor("Select a_Vista from tipo_pagamento where tipo_pagamento ='" + Tipo_pagamento + "'", null).Equals("1");
                String id_cc = Conexao.retornaUmValor("select id_cc from centro_custo where codigo_centro_custo='" + centroCusto + "'", null);
                if (emissao == null)
                    emissao = DateTime.Now;
                if (Tipo_NF.Equals("2"))
                {
                    conta_a_pagarDAO pg = new conta_a_pagarDAO(usr);
                    pg.Documento = Codigo + serie + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0');
                    pg.Fornecedor = Cliente_Fornecedor;
                    pg.Filial = Filial;
                    pg.Codigo_Centro_Custo = centroCusto;
                    pg.Valor = Valor;
                    pg.Desconto = 0;
                    pg.serie = this.serie;
                    pg.obs = "D.G.A NF " + (Tipo_NF.Equals("1") ? "SAIDA" : "ENTRADA") + ":" + Codigo;


                    pg.emissao = emissao;
                    pg.Vencimento = Vencimento;
                    pg.Tipo_Pagamento = Tipo_pagamento;
                    pg.id_cc = id_cc;

                    if (aVista)
                    {
                        pg.Pagamento = emissao;
                        pg.Valor_Pago = Valor;

                        pg.status = "2";
                    }
                    else
                    {
                        pg.status = "1";
                    }
                    pg.entrada = DateTime.Now;
                    pg.usuario = usr.getNome();
                    pg.documento_emitido = "0";
                    // ALTERACÃO SOLICITADA PELO RAFAEL NO DIA 08/12/2015 
                    pg.Duplicata = boleto_recebido;
                    pg.BOLETO_RECEBIDO = boleto_recebido;
                    pg.cod_barras = cod_barras;
                    //====================================================
                    pg.salvar(true, conn, tran);

                }
                else
                {

                    conta_a_receberDAO pg = new conta_a_receberDAO(usr);

                    pg.Documento = Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0');
                    pg.Codigo_Cliente = Cliente_Fornecedor;
                    pg.Filial = Filial;
                    pg.Codigo_Centro_Custo = centroCusto;
                    pg.Valor = Valor;
                    pg.Desconto = 0;
                    pg.Obs = "D.G.A NF " + (Tipo_NF.Equals("1") ? "SAIDA" : "ENTRADA") + ":" + Codigo;
                    pg.Emissao = emissao;
                    pg.Vencimento = Vencimento;
                    pg.id_cc = id_cc;
                    pg.aVista = aVista;
                    if (aVista)
                    {
                        pg.Pagamento = emissao;
                        pg.Valor_Pago = Valor;
                        pg.status = 2;
                    }
                    else
                    {
                        pg.status = 1;
                    }
                    pg.entrada = DateTime.Now;
                    pg.usuario = usr.getNome();
                    pg.documento_emitido = "0";

                    pg.salvar(true, conn, tran);
                }
            }
        }
        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            if (NaturezaOperacao != null && NaturezaOperacao.Gera_apagar_receber)
            {
                if (Tipo_NF.Equals("2") && !NaturezaOperacao.Imprime_NF)
                {
                    //conta_a_pagarDAO ctaaPg = new conta_a_pagarDAO(Codigo + "-" + ordem.ToString().PadLeft(2, '0'), Cliente_Fornecedor, usr);
                    String ctaPGstatus = Conexao.retornaUmValor("select status from conta_a_pagar where Documento='" + Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0') + "'  and  fornecedor ='" + Cliente_Fornecedor + "' and serie ="+serie, usr, conn, tran);
                    if (ctaPGstatus.Equals("2"))
                    {

                        throw new Exception("A Nota contem titulos a pagar vinculados que já estão com o Status 'CONCLUIDO' estorne os Titulos antes de excluir a Nota! ");
                    }
                    else
                    {
                        Conexao.executarSql("delete from conta_a_pagar where documento='" + Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0') + "' and fornecedor = '" + Cliente_Fornecedor + "' and filial='" + Filial + "' and serie =" + serie, conn, tran);
                    }
                }
                else
                {
                    if (Tipo_NF.Equals("2"))
                    {
                        Conexao.executarSql("update Conta_a_pagar set status =3 , documento = documento+'C' where documento='" + Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0') + "' and fornecedor = '" + Cliente_Fornecedor + "' and filial='" + Filial + "' and serie =" + serie, conn, tran);
                    }
                    else
                    {
                        Conexao.executarSql("update Conta_a_receber set status =3 where documento='" + Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0') + "' and codigo_cliente = '" + Cliente_Fornecedor + "' and filial='" + Filial + "' ", conn, tran);
                    }
                }

            }

            String sql = "delete from nf_pagamento  where filial='" + Filial + "' and  vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "' and codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and serie =" + serie;
            Conexao.executarSql(sql, conn, tran);


            return true;
        }

        private void    insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {


                String sql = " insert into nf_pagamento( " +
                          "Vencimento," +
                          "Filial," +
                          "Codigo," +
                          "Cliente_Fornecedor," +
                          "Tipo_NF," +
                          "Tipo_pagamento," +
                          "Valor," +
                          "boleto_recebido" +
                          ",serie"+
                          ",cod_barras"+
                     " )values( " +
                          (Vencimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Filial + "'" +
                          "," + "'" + Codigo + "'" +
                          "," + "'" + Cliente_Fornecedor + "'" +
                          "," + Tipo_NF +
                          "," + "'" + Tipo_pagamento + "'" +
                          "," + Valor.ToString().Replace(",", ".") +
                          "," + (boleto_recebido ? 1 : 0) +
                          ","+serie.ToString()+
                          ",'"+cod_barras+"'"+
                         ");";

                Conexao.executarSql(sql, conn, tran);

                if (NaturezaOperacao != null && NaturezaOperacao.Gera_apagar_receber)
                {
                    if (emissao == null)
                        emissao = DateTime.Now;
                    if (Tipo_NF.Equals("2") && !NaturezaOperacao.Imprime_NF)
                    {
                        bool aVista = Conexao.retornaUmValor("Select a_Vista from tipo_pagamento where tipo_pagamento ='" + Tipo_pagamento + "'", null).Equals("1");
                        String id_cc = Conexao.retornaUmValor("select id_cc from centro_custo where codigo_centro_custo='" + centroCusto + "'", null);
                        if (Tipo_NF.Equals("2"))
                        {
                            //Alteracao para permitir o lançamento com destinação para outra FILIAL
                            //este processo terá um serviço na loja que vai ficar responsável em gravar os títulos
                            //no contas a receber da nuvem.
                            if (!usr.FilialFinanceiro.Equals(""))
                            {
                                conta_a_pagar_outra_filialDAO pg = new conta_a_pagar_outra_filialDAO();
                                pg.filial = usr.FilialFinanceiro;
                                pg.fornecedor = Cliente_Fornecedor;
                                pg.documento = Codigo.Trim() + serie + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0');
                                pg.numero_nf = Codigo.Trim();
                                pg.serie = this.serie;
                                pg.emissao = emissao;
                                pg.vencimento = Vencimento;
                                pg.codigo_centro_custo = centroCusto;
                                pg.valor = Valor;
                                pg.usuario = usr.getUsuario().ToString();
                                pg.insert(conn, tran);
                            }
                            else
                            {
                                conta_a_pagarDAO pg = new conta_a_pagarDAO(usr);
                                pg.Documento = Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0');
                                pg.Fornecedor = Cliente_Fornecedor;
                                pg.serie = this.serie;
                                pg.Filial = Filial;
                                pg.Codigo_Centro_Custo = centroCusto;
                                pg.Valor = Valor;
                                pg.Desconto = 0;
                                pg.obs = "D.G.A NF " + (Tipo_NF.Equals("1") ? "SAIDA" : "ENTRADA") + ":" + Codigo;
                                pg.emissao = emissao;
                                pg.Vencimento = Vencimento;
                                pg.Tipo_Pagamento = Tipo_pagamento;
                                pg.id_cc = id_cc;

                                if (aVista)
                                {
                                    pg.Pagamento = emissao;
                                    pg.Valor_Pago = Valor;

                                    pg.status = "2";
                                }
                                else
                                {
                                    pg.status = "1";
                                }
                                //pg.entrada = DateTime.Now;
                                pg.entrada = entrada;
                                pg.usuario = usr.getNome();
                                pg.documento_emitido = "0";
                                // ALTERACÃO SOLICITADA PELO RAFAEL NO DIA 08/12/2015 
                                pg.Duplicata = boleto_recebido;
                                pg.BOLETO_RECEBIDO = boleto_recebido;
                                pg.cod_barras = cod_barras;
                                //====================================================
                                pg.salvar(true, conn, tran);
                            }
                        }
                        else
                        {

                            conta_a_receberDAO pg = new conta_a_receberDAO(usr);

                            pg.Documento = Codigo + vCodigoNotaProdutor + "-" + ordem.ToString().PadLeft(2, '0');
                            pg.Codigo_Cliente = Cliente_Fornecedor;
                            pg.Filial = Filial;
                            pg.Codigo_Centro_Custo = centroCusto;
                            pg.Valor = Valor;
                            pg.Desconto = 0;
                            pg.Obs = "D.G.A NF " + (Tipo_NF.Equals("1") ? "SAIDA" : "ENTRADA") + ":" + Codigo;
                            pg.Emissao = emissao;
                            pg.Vencimento = Vencimento;
                            pg.id_cc = id_cc;
                            if (aVista)
                            {
                                pg.Pagamento = emissao;
                                pg.Valor_Pago = Valor;
                                pg.status = 2;
                            }
                            else
                            {
                                pg.status = 1;
                            }
                            pg.entrada = DateTime.Now;
                            pg.usuario = usr.getNome();
                            pg.documento_emitido = "0";

                            pg.salvar(true, conn, tran);
                        }
                    }

                }
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}