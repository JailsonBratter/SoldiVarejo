using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.Cadastro;

namespace visualSysWeb.dao
{
    public class pedidoDAO
    {
        public String Filial { get; set; }
        public String Pedido { get; set; }
        public int Status = 0;
        public bool entrega = false;
        public String getStatus
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "ABERTO";

                    case 2:
                        return "FECHADO";

                    case 3:
                        return "CANCELADO";

                    default:
                        return "";
                }

            }

        }
        public int Tipo { get; set; }
        public String Cliente_Fornec = "";
        public bool pedido_simples = false;
        private TabelaPrecoDAO tbPreco = new TabelaPrecoDAO();
        public String TabelaPreco
        {
            get
            {
                if (tbPreco != null && tbPreco.codigo_tabela != null)
                {
                    return tbPreco.codigo_tabela;

                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (!value.Equals(""))
                {
                    tbPreco = new TabelaPrecoDAO(value, usr);
                }
            }
        }
        public String NomeCliente
        {
            get
            {
                if(cliente == null || !Cliente_Fornec.Equals(cliente.Codigo_Cliente))
                    cliente = new ClienteDAO(Cliente_Fornec, usr);

                return cliente.Nome_Cliente;
            }
            set { }
        }
        public String NomeFornecedor
        {
            get
            {
                return Conexao.retornaUmValor("select Razao_social from fornecedor where fornecedor='" + Cliente_Fornec + "'", new User());
            }
            set { }
        }

        public String centro_custo = "";
        public DateTime Data_cadastro { get; set; }
        public String Data_cadastroBr()
        {
            return dataBr(Data_cadastro);
        }

        public DateTime Data_entrega { get; set; }
        public String Data_entregaBr()
        {
            return dataBr(Data_entrega);
        }

        public String hora = "";
        public Decimal Desconto = 0;
        public Decimal vTotal = 0;
        public Decimal Total
        {
            get
            {
                vTotal = 0;
                if (tbPreco.porc != 0)
                {
                    if (Tipo == 1)
                    {
                        Desconto = 0;
                    }
                    foreach (pedido_itensDAO item in PedItens)
                    {
                        if (!item.excluido)
                        {
                           
                            vTotal += item.total;
                            if (Tipo == 1)
                            {
                               
                                Desconto += item.valorDesconto;
                               
                            }
                        }
                    }
                }
                else
                {
                    foreach (pedido_itensDAO item in PedItens)
                    {
                        if (!item.excluido)
                        {
                            vTotal += item.total;
                        }
                    }
                }
                vTotal += Frete;
                return vTotal;
            }
            set { vTotal = value; }
        }

        public Decimal totalBruto
        {
            get
            {
                Decimal vTotal = 0;
                foreach (pedido_itensDAO item in PedItens)
                {
                    if (!item.excluido)
                    {

                        vTotal += item.totalbruto;
                    }
                }
                vTotal += Frete;
                return vTotal;
            }
        }

        public String Usuario = "";
        public String Obs = "";
        public Decimal CFOP = 0;
        public String orcamento = "";
        public String funcionario = "";
        public String codigo_solicitacao = "";
        public String hora_fim = "";
        public bool ID { get; set; }
        public int cotacao { get; set; }
        public bool impresso { get; set; }
        public ArrayList PedItens = new ArrayList();
        public User usr = null;
        public ArrayList PedPg = new ArrayList();

        public int indIntermed;
        public string intermedCnpj = "";
        public string idCadIntTran = "";
        public string CNPJPagamento = "";

        public string hora_cadastro = "";
        public Decimal Frete { get; set; }
        public Decimal Despesas { get; set; }
        public ClienteDAO cliente { get; set; }

        public string historico { get; set; }

        public pedidoDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }
        public pedidoDAO(String pedido, int tipo, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  pedido where pedido =" + pedido + " and tipo =" + tipo + " and Filial='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public void addItens(pedido_itensDAO ped, bool naoAtribuirNum_Item = false)
        {
            ped.Filial = Filial;
            ped.Tipo = Tipo;
            ped.codCliente = this.Cliente_Fornec;
            ped.TabelaPrecoPedido = tbPreco;
            ped.index = PedItens.Count;
            if (!naoAtribuirNum_Item) //Caso esteja setado como true (não atribuir sequência)
            ped.num_item = PedItens.Count + 1;
            //Se o pedido for igual a 1 (venda)
            if (this.Tipo == 1)
            {
                if (cliente.ativa_terceiro_preco)
                {
                    decimal vTerceiroPreco = Funcoes.decTry(
                        Conexao.retornaUmValor("Select terceiro_preco from mercadoria where plu ='" + ped.PLU + "'", null)
                       );
                    if (vTerceiroPreco > 0)
                        ped.vUnitario = vTerceiroPreco;
                }
            }

            PedItens.Add(ped);
            
        }
        public void aplicarDesconto(Decimal valorDesconto)
        {
            Decimal vTotalItensDesconto = 0;
            Desconto = valorDesconto;
            Decimal vlrItemDesconto = 0;
            if (valorDesconto > 0 && totalBruto >0)
            {
                vlrItemDesconto = Decimal.Round((valorDesconto / totalBruto) * 100, 4);
                //vlrItemDesconto = Decimal.Round((valorDesconto / totalBruto) * 100, 2);

            }

            foreach (pedido_itensDAO item in PedItens)
            {
                if (!item.excluido)
                {
                    item.Desconto = vlrItemDesconto;
                    vTotalItensDesconto += Decimal.Round(item.total, 2);
                }
            }
            /*
            Decimal dif = Decimal.Round(Total,2) - vTotalItensDesconto;
            if (!dif.ToString("N2").Equals("0,00"))
            {

                pedido_itensDAO item = (pedido_itensDAO)PedItens[PedItens.Count - 1];
                if (item.excluido || item.totalbruto< dif)
                {
                    for (int i = PedItens.Count - 2; i >= 0; i--)
                    {
                        item = (pedido_itensDAO)PedItens[i];
                        if (!item.excluido && item.totalbruto > dif)
                            break;
                    }
                }
                item.valorDesconto += dif;
                item.Desconto = (Decimal.Round(item.valorDesconto,2) / item.totalbruto) * 100;
            }
            */


        }

        public void atualizaitem(pedido_itensDAO pedItem)
        {
            PedItens[pedItem.index] = pedItem;
            calcularDesconto();
            /*
            int i = 0;
            foreach (pedido_itensDAO item in PedItens)
            {
                pedItem.Pedido = Pedido;
                if (item.PLU.Equals(pedItem.PLU) && !item.excluido)
                {
                    pedItem.TabelaPrecoPedido = tbPreco;
                    PedItens[i] = pedItem;
                    break;
                }
                i++;
            }
             */
        }
        public void calcularDesconto()
        {
            Desconto = 0;
            foreach (pedido_itensDAO item in PedItens)
            {
                Desconto += item.valorDesconto; 
            }

        }



        public pedido_itensDAO item(int item)
        {
            int itensAtivos = -1;
            foreach (pedido_itensDAO pditem in PedItens)
            {
                if (!pditem.excluido)
                {
                    itensAtivos++;
                }
                if (itensAtivos.Equals(item))
                {
                    return pditem;
                }

            }
            return null;
        }

        public Decimal qtdeAdicionadaItem(String plu, Decimal preco)
        {
            Decimal vQtde = 0;

            foreach (pedido_itensDAO pditem in PedItens)
            {
                if (!pditem.excluido && (pditem.PLU).Equals(plu) && pditem.unitario.Equals(preco))
                {
                    vQtde += pditem.Qtde;
                }
            }

            return vQtde;
        }
        public pedido_itensDAO item(String plu)
        {
            foreach (pedido_itensDAO pditem in PedItens)
            {
                if (!pditem.excluido && pditem.PLU.Equals(plu))
                {
                    return pditem;

                }
            }
            return null;
        }
        public void aproveitaItens()
        {
            foreach (pedido_itensDAO item in PedItens)
            {
                item.inserido = true;
                Decimal vUnit = Decimal.Parse(Conexao.retornaUmValor("select ISNULL(preco,0) from Mercadoria_Loja where Filial ='" + Filial + "' AND PLU ='" + item.PLU + "'", usr));
                item.unitario = vUnit;
            }
        }

        public void removeItem(pedido_itensDAO pedItem)
        {
            pedItem.Tipo = Tipo;
            pedItem.excluido = true;
            PedItens[pedItem.index] = pedItem;

            /*
            foreach (pedido_itensDAO item in PedItens)
            {
                if(item.Equals(pedItem))
                //if (item.PLU.Equals(pedItem.PLU))
                {
                    item.excluido = true;
                    //atualizaitem(item);
                    break;
                }
            }
             */
        }

        public void carregarItens()
        {
            String Sql = "Select pedido_itens.*,mercadoria.descricao from pedido_itens inner join mercadoria on mercadoria.plu = pedido_itens.plu where pedido = " + Pedido + " and pedido_itens.Tipo=" + Tipo + " order by " + (Tipo == 1 ? "num_item" : "mercadoria.descricao");
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(Sql, usr, false);

                while (rs.Read())
                {
                    pedido_itensDAO item = new pedido_itensDAO(usr);
                    item.codCliente = Cliente_Fornec;
                    item.TabelaPrecoPedido = tbPreco;
                    item.TabelaPrecoMercadoria();
                    item.Filial = rs["Filial"].ToString();
                    item.Pedido = rs["Pedido"].ToString();
                    item.Tipo = (rs["Tipo"] == null ? 0 : int.Parse(rs["Tipo"].ToString()));
                    item.PLU = rs["PLU"].ToString();
                    item.Descricao = rs["Descricao"].ToString();
                    item.Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                    item.Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                    item.unitario = (Decimal)(rs["unitario"].ToString().Equals("") ? new Decimal() : rs["unitario"]);
                    item.ean = rs["ean"].ToString();
                    item.id = (rs["id"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["id"].ToString()));
                    item.Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    item.documento = rs["Documento"].ToString();
                    item.caixa_documento = rs["caixa_documento"].ToString().Equals("") ? 0 : int.Parse(rs["caixa_documento"].ToString());
                    item.data_documento = rs["data_documento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_documento"].ToString());
                    item.obs = rs["obs"].ToString();
                    item.num_item = Funcoes.intTry(rs["num_item"].ToString());
                    item.produzir = rs["produzir"].ToString().Equals("1");
                    item.data_hora_produzir = Funcoes.dtTry(rs["data_hora_produzir"].ToString());
                    item.agrupamento = rs["agrupamento"].ToString();
                    addItens(item, true);
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
        public DataTable PdItens()
        {
            return PdItens(false);
        }

        public DataTable PdItens(bool naoRef)
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Filial");
            cabecalho.Add("Pedido");
            cabecalho.Add("PLU");
            cabecalho.Add("CodReferencia");
            cabecalho.Add("Descricao");
            cabecalho.Add("Qtde");
            cabecalho.Add("Embalagem");

            cabecalho.Add("TabDesc");
            cabecalho.Add("unitario");
            cabecalho.Add("Desc");


            cabecalho.Add("total");
            cabecalho.Add("ean");
            cabecalho.Add("id");
            cabecalho.Add("Documento");
            cabecalho.Add("caixa_documento");
            cabecalho.Add("data_Documento");
            cabecalho.Add("peso_Bruto");
            cabecalho.Add("obs");

            cabecalho.Add("DataHoraProduzir");
            cabecalho.Add("Agrupamento");
            cabecalho.Add("UnitarioDesconto");

            itens.Add(cabecalho);
            if (PedItens != null && PedItens.Count > 0)
            {
                foreach (pedido_itensDAO item in PedItens)
                {
                    if (!item.excluido)
                    {
                        if (!item.codCliente.Equals(Cliente_Fornec))
                        {
                            item.codCliente = Cliente_Fornec;
                            item.tbPrecoMercadoria = null;
                        }
                        item.TabelaPrecoPedido = tbPreco;
                        itens.Add(item.ArrToString(naoRef));
                    }
                }
            }
            return Conexao.GetArryTable(itens);
        }

        //Pagamentos

        public void addPagamentos(pedido_pagamentoDAO pg)
        {
            pg.Filial = Filial;
            pg.Tipo = Tipo;
            pg.ordem = (PedPg.Count + 1);
            PedPg.Add(pg);
        }

        public Decimal totalPagamentos()
        {
            if (PedPg.Count < 1)
            {
                return 0;
            }
            else
            {
                Decimal pgTotal = 0;
                foreach (pedido_pagamentoDAO pg in PedPg)
                {
                    if (!pg.excluido)
                    {
                        pgTotal += pg.Valor;
                    }
                }
                return pgTotal;
            }
        }

        public void atualizaPg(pedido_pagamentoDAO pedpg)
        {
            int i = 0;
            foreach (pedido_pagamentoDAO pg in PedPg)
            {

                if (pg.Equals(pedpg))
                {
                    PedPg[i] = pedpg;
                    break;
                }
                i++;
            }
        }



        public void calcularParcelaPG()
        {
            Decimal vlrParcela = (Total / PedPg.Count);

            foreach (pedido_pagamentoDAO pg in PedPg)
            {
                pg.Valor = vlrParcela;
            }
        }

        public pedido_pagamentoDAO pagamento(int pg)
        {
            return (pedido_pagamentoDAO)PedPg[pg];
        }

        public void removePG(pedido_pagamentoDAO pedPg)
        {
            foreach (pedido_pagamentoDAO pg in PedPg)
            {

                if (pg.Equals(pedPg))
                {
                    PedPg.Remove(pg);

                    break;
                }
            }
        }

        public void carregarPagamentos()
        {
            String Sql = "Select * from pedido_pagamento where pedido = " + Pedido + " and Tipo=" + Tipo;
            SqlDataReader rs = Conexao.consulta(Sql, usr, false);

            while (rs.Read())
            {
                pedido_pagamentoDAO pg = new pedido_pagamentoDAO(usr);
                pg.Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                pg.Filial = rs["Filial"].ToString();
                pg.Pedido = rs["Pedido"].ToString();
                pg.Cliente_Fornec = rs["Cliente_Fornec"].ToString();
                pg.Tipo = (rs["Tipo"].ToString().Equals("") ? 0 : int.Parse(rs["Tipo"].ToString()));
                pg.Tipo_pagamento = rs["Tipo_pagamento"].ToString();
                pg.Valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);

                addPagamentos(pg);
            }

            if (rs != null)
                rs.Close();
        }




        public DataTable PdPag()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Vencimento");
            cabecalho.Add("Filial");
            cabecalho.Add("Pedido");
            cabecalho.Add("Cliente_Fornec");
            cabecalho.Add("Tipo");
            cabecalho.Add("Tipo_pagamento");
            cabecalho.Add("Valor");

            itens.Add(cabecalho);
            if (PedPg != null && PedPg.Count > 0)
            {
                foreach (pedido_pagamentoDAO pg in PedPg)
                {
                    if (!pg.excluido)
                    {
                        itens.Add(pg.ArrToString());
                    }
                }
            }
            return Conexao.GetArryTable(itens);
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
                    Filial = rs["Filial"].ToString();
                    Pedido = rs["Pedido"].ToString();
                    Status = (rs["Status"] == null ? 0 : int.Parse(rs["Status"].ToString()));
                    Tipo = (rs["Tipo"] == null ? 0 : int.Parse(rs["Tipo"].ToString()));
                    Cliente_Fornec = rs["Cliente_Fornec"].ToString();
                    if (Tipo == 1)
                        cliente = new ClienteDAO(Cliente_Fornec, usr);

                    Data_cadastro = (rs["Data_cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_cadastro"].ToString()));
                    Data_entrega = (rs["Data_entrega"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_entrega"].ToString()));
                    hora = rs["hora"].ToString();
                    Usuario = rs["Usuario"].ToString();
                    Obs = rs["Obs"].ToString();
                    CFOP = (Decimal)(rs["CFOP"].ToString().Equals("") ? new Decimal() : rs["CFOP"]);
                    orcamento = rs["orcamento"].ToString();
                    funcionario = rs["funcionario"].ToString();

                    hora_fim = rs["hora_fim"].ToString();
                    ID = (rs["ID"].ToString().Equals("1") ? true : false);
                    cotacao = (rs["cotacao"].ToString().Equals("") ? 0 : int.Parse(rs["cotacao"].ToString()));
                    //    impresso = (rs["impresso"].ToString().Equals("1")?true:false);
                    TabelaPreco = rs["tabela_desconto"].ToString();
                    pedido_simples = (rs["pedido_simples"].ToString().Equals("1") ? true : false);
                    centro_custo = rs["centro_custo"].ToString();

                    carregarItens();
                    carregarPagamentos();

                    Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                    Total = (Decimal)(rs["Total"].ToString().Equals("") ? new Decimal() : rs["Total"]);

                    entrega = rs["entrega"].ToString().Equals("1");
                    hora_cadastro = rs["hora_cadastro"].ToString();

                    Frete = (Decimal)(rs["Frete"].ToString().Equals("") ? new Decimal() : rs["Frete"]);
                    Despesas = Funcoes.decTry(rs["despesas"].ToString());
                    indIntermed = Funcoes.intTry(rs["indIntermed"].ToString());
                    intermedCnpj = rs["intermedCnpj"].ToString();
                    idCadIntTran = rs["idCadIntTran"].ToString();
                    CNPJPagamento = rs["CNPJPagamento"].ToString();
                    historico = rs["historico"].ToString();

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
                String sql = "update  pedido set " +
                              "Status=" + Status +

                              ",Cliente_Fornec='" + Cliente_Fornec + "'" +
                              ",Data_cadastro=" + (Data_cadastro.Equals("0001-01-01") ? "null" : "'" + Data_cadastro.ToString("yyyy-MM-dd") + "'") +
                              ",Data_entrega=" + (Data_entrega.Equals("0001-01-01") ? "null" : "'" + Data_entrega.ToString("yyyy-MM-dd") + "'") +
                              ",hora='" + hora + "'" +
                              ",Desconto=" + Funcoes.decimalPonto(Desconto.ToString()) +
                              ",Total=" + Total.ToString().Replace(",", ".") +
                              ",Usuario='" + Usuario + "'" +
                              ",Obs='" + Obs + "'" +
                              ",CFOP=" + CFOP.ToString().Replace(",", ".") +
                              ",orcamento='" + orcamento + "'" +
                              ",funcionario='" + funcionario + "'" +
                              ",hora_fim='" + hora_fim + "'" +
                              ",ID=" + (ID ? "1" : "0") +
                              ",cotacao=" + cotacao +
                              ",impresso=" + (impresso ? "1" : "0") +
                              ",tabela_desconto='" + TabelaPreco + "'" +
                              ",pedido_simples=" + (pedido_simples ? "1" : "0") +
                              ",centro_custo='" + centro_custo + "'" +
                              ",entrega =" + (entrega ? "1" : "0") +
                              ",hora_cadastro='" + hora_cadastro + "'" +
                              ",Frete=" + Funcoes.decimalPonto(Frete.ToString()) +
                              ",despesas = " + Funcoes.decimalPonto(Despesas.ToString()) +
                              ",indIntermed=" + indIntermed.ToString() +
                              ",intermedCnpj='" + intermedCnpj.Trim() + "'" +
                              ",idCadIntTran='" + idCadIntTran.Trim() + "'" +
                              ",CNPJPagamento='" + CNPJPagamento + "'" +
                              ",Historico = '" + historico + "'" +
                    "  where  Pedido='" + Pedido + "' and Tipo=" + Tipo + " and Filial = '" + Filial + "'";
                Conexao.executarSql(sql, conn, tran);

                foreach (pedido_itensDAO item in PedItens)
                {
                    item.Pedido = Pedido;
                    item.Filial = Filial;
                    item.Tipo = Tipo;
                    if (item.excluido)
                        item.excluir(conn, tran);
                    else
                        item.salvar(false, conn, tran);
                }

                Conexao.executarSql("delete from pedido_pagamento where filial='" + Filial + "' and pedido = '" + Pedido + "' and tipo =" + Tipo, conn, tran);


                foreach (pedido_pagamentoDAO pg in PedPg)
                {
                    pg.Pedido = Pedido;
                    pg.Filial = Filial;
                    pg.Tipo = Tipo;
                    pg.salvar(true, conn, tran);
                }


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

            if (Conexao.countSql("select pedido from nf where pedido = '" + Pedido + "' AND FILIAL ='" + Filial + "'", usr) > 0)
            {
                throw new Exception("Pedido não pode ser Cancelado por estar vinculado a uma nota fiscal");
            }


            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                String sql = "update pedido set status=3 where Pedido='" + Pedido + "' and Tipo=" + Tipo + " and Filial ='" + Filial + "'"; //colocar campo index

                Conexao.executarSql(sql, conn, tran);
                /* foreach (pedido_itensDAO item in PedItens)
                 {
                     item.excluir(conn, tran);
                 }
                 */
                String baixaEstoque = Funcoes.valorParametro("BAIXA_ESTOQUE_PED_VENDA", null);
                if ((baixaEstoque.ToUpper().Equals("TRUE") || pedido_simples) && Tipo != 8)
                {
                    //natureza_operacaoDAO op = new natureza_operacaoDAO(CFOP.ToString(), null);
                    foreach (pedido_itensDAO item in PedItens)
                    {
                        //item.naturezaOperacao = op;
                        item.CancelaSaidaEstoque(conn, tran);
                    }

                }
                //Laço para cancelar pagamentos.
                foreach (pedido_pagamentoDAO pg in PedPg)
                {
                    pg.cancelaFinanceiro(conn, tran);

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
                String strSequencia = "";
                bool bAtualizaSeq = false;
                if (Pedido == null || Pedido.Equals(""))
                {


                    if (Tipo == 1)
                    {
                        strSequencia = "VENDAS.PEDIDO";
                    }
                    else if (Tipo == 2)
                    {
                        strSequencia = "COMPRAS.PEDIDO";
                    }
                    else if (Tipo == 3)
                    {
                        strSequencia = "DEVOLUCAO.PEDIDO";
                    }
                    else if (Tipo == 4)
                    {
                        strSequencia = "DEVFORNECEDOR.PEDIDO";
                    }
                    else if (Tipo == 5)
                    {
                        strSequencia = "PRODUCAO.PEDIDO";
                    }
                    else if (Tipo == 6)
                    {
                        strSequencia = "ENCOMENDA.PEDIDO";
                    }
                    else if (Tipo == 7)
                    {
                        strSequencia = "VENDA_EXTERNA.PEDIDO";
                    }
                    else if (Tipo == 8)
                    {
                        strSequencia = "ORCAMENTO.PEDIDO";
                    }

                    bAtualizaSeq = true;
                    Pedido = Funcoes.sequencia(strSequencia, usr);
                }

                String sql = " insert into pedido (" +
                        "Filial," +
                        "Pedido," +
                        "Status," +
                        "Tipo," +
                        "Cliente_Fornec," +
                        "Data_cadastro," +
                        "Data_entrega," +
                        "hora," +
                        "Desconto," +
                        "Total," +
                        "Usuario," +
                        "Obs," +
                        "CFOP," +
                        "orcamento," +
                        "funcionario," +
                        "hora_fim," +
                        "ID," +
                        "cotacao," +
                        "impresso," +
                        "tabela_desconto," +
                        "pedido_simples," +
                        "centro_custo" +
                        ",entrega" +
                        ",hora_cadastro" +
                        ",Frete" +
                        ",despesas" +
                        ",indIntermed " +
                        ",intermedCnpj " +
                        ",idCadIntTran" +
                         ",CNPJPagamento" +
                     ") values (" +
                        "'" + Filial + "'" +
                        "," + "'" + Pedido + "'" +
                        "," + Status +
                        "," + Tipo +
                        "," + "'" + Cliente_Fornec + "'" +
                        "," + (Data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_cadastro.ToString("yyyy-MM-dd") + "'") +
                        "," + (Data_entrega.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_entrega.ToString("yyyy-MM-dd") + "'") +
                        "," + "'" + hora + "'" +
                        "," + Desconto.ToString().Replace(",", ".") +
                        "," + Total.ToString().Replace(",", ".") +
                        "," + "'" + Usuario + "'" +
                        "," + "'" + Obs + "'" +
                        "," + CFOP.ToString().Replace(",", ".") +
                        "," + "'" + orcamento + "'" +
                        "," + "'" + funcionario + "'" +
                        "," + "'" + hora_fim + "'" +
                        "," + (ID ? 1 : 0) +
                        "," + cotacao +
                        "," + (impresso ? 1 : 0) +
                        ",'" + TabelaPreco + "'" +
                        "," + (pedido_simples ? 1 : 0) +
                        ",'" + centro_custo + "'" +
                        "," + (entrega ? "1" : "0") +
                        ",'" + hora_cadastro + "'" +
                        "," + Frete.ToString().Replace(",", ".") +
                        "," + Funcoes.decimalPonto(Despesas.ToString()) +
                        "," + indIntermed.ToString() +
                        ",'" + intermedCnpj.Trim() + "'" +
                        ",'" + idCadIntTran.Trim() + "'" +
                        ",'" + CNPJPagamento + "'" +
                 ")";
                Conexao.executarSql(sql, conn, tran);
                if (bAtualizaSeq)
                {
                    Funcoes.salvaProximaSequencia(strSequencia, usr);
                }
                foreach (pedido_itensDAO item in PedItens)
                {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    if (!item.excluido)
                    {
                        item.Pedido = Pedido;
                        item.Filial = Filial;
                        item.Tipo = Tipo;
                        item.salvar(true, conn, tran);
                    }
                }

                foreach (pedido_pagamentoDAO pg in PedPg)
                {
                    if (!pg.excluido)
                    {
                        pg.Pedido = Pedido;
                        pg.Filial = Filial;
                        pg.Tipo = Tipo;
                        pg.Cliente_Fornec = Cliente_Fornec;
                        pg.salvar(true, conn, tran);
                    }
                }

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
                  static String sqlGrid = ""select * from pedido";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ pedidoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>pedido</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="20"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pedido" Text="Pedido" Visible="true" 
                    HeaderText="Pedido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Status" Text="Status" Visible="true" 
                    HeaderText="Status" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cliente_Fornec" Text="Cliente_Fornec" Visible="true" 
                    HeaderText="Cliente_Fornec" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_cadastro" Text="Data_cadastro" Visible="true" 
                    HeaderText="Data_cadastro" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_entrega" Text="Data_entrega" Visible="true" 
                    HeaderText="Data_entrega" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="hora" Text="hora" Visible="true" 
                    HeaderText="hora" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Desconto" Text="Desconto" Visible="true" 
                    HeaderText="Desconto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" 
                    HeaderText="Total" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Usuario" Text="Usuario" Visible="true" 
                    HeaderText="Usuario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Obs" Text="Obs" Visible="true" 
                    HeaderText="Obs" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CFOP" Text="CFOP" Visible="true" 
                    HeaderText="CFOP" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="orcamento" Text="orcamento" Visible="true" 
                    HeaderText="orcamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="funcionario" Text="funcionario" Visible="true" 
                    HeaderText="funcionario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo_solicitacao" Text="codigo_solicitacao" Visible="true" 
                    HeaderText="codigo_solicitacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="hora_fim" Text="hora_fim" Visible="true" 
                    HeaderText="hora_fim" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ID" Text="ID" Visible="true" 
                    HeaderText="ID" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cotacao" Text="cotacao" Visible="true" 
                    HeaderText="cotacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="impresso" Text="impresso" Visible="true" 
                    HeaderText="impresso" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static pedidoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new pedidoDAO();
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
                                        obj = new pedidoDAO(index,usr);
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
                                         txtPedido.Text=obj.Pedido.ToString();
                                         txtStatus.Text=obj.Status.ToString();
                                         txtTipo.Text=obj.Tipo.ToString();
                                         txtCliente_Fornec.Text=obj.Cliente_Fornec.ToString();
                                         txtData_cadastro.Text=obj.Data_cadastroBr();
                                         txtData_entrega.Text=obj.Data_entregaBr();
                                         txthora.Text=obj.hora.ToString();
                                         txtDesconto.Text=string.Format("{0:0,0.00}",obj.Desconto);
                                         txtTotal.Text=string.Format("{0:0,0.00}",obj.Total);
                                         txtUsuario.Text=obj.Usuario.ToString();
                                         txtObs.Text=obj.Obs.ToString();
                                         txtCFOP.Text=string.Format("{0:0,0.00}",obj.CFOP);
                                         txtorcamento.Text=obj.orcamento.ToString();
                                         txtfuncionario.Text=obj.funcionario.ToString();
                                         txtcodigo_solicitacao.Text=obj.codigo_solicitacao.ToString();
                                         txthora_fim.Text=obj.hora_fim.ToString();
                                         chkID.Checked =obj.ID;
                                         txtcotacao.Text=obj.cotacao.ToString();
                                         chkimpresso.Checked =obj.impresso;
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.Pedido=txtPedido.Text;
                                         obj.Status=int.Parse(txtStatus.Text);
                                         obj.Tipo=int.Parse(txtTipo.Text);
                                         obj.Cliente_Fornec=txtCliente_Fornec.Text;
                                         obj.Data_cadastro=DateTime.Parse(txtData_cadastro.Text);
                                         obj.Data_entrega=DateTime.Parse(txtData_entrega.Text);
                                         obj.hora=txthora.Text;
                                         obj.Desconto=Decimal.Parse(txtDesconto.Text);
                                         obj.Total=Decimal.Parse(txtTotal.Text);
                                         obj.Usuario=txtUsuario.Text;
                                         obj.Obs=txtObs.Text;
                                         obj.CFOP=Decimal.Parse(txtCFOP.Text);
                                         obj.orcamento=txtorcamento.Text;
                                         obj.funcionario=txtfuncionario.Text;
                                         obj.codigo_solicitacao=txtcodigo_solicitacao.Text;
                                         obj.hora_fim=txthora_fim.Text;
                                         obj.ID=chkID.Checked ;
                                         obj.cotacao=int.Parse(txtcotacao.Text);
                                         obj.impresso=chkimpresso.Checked ;
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
       <center> <h1>Detalhes do pedido</h1></center>                  
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

                                      <td >                   <p>Pedido</p>
                   <asp:TextBox ID="txtPedido" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Status</p>
                   <asp:TextBox ID="txtStatus" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <asp:TextBox ID="txtTipo" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cliente_Fornec</p>
                   <asp:TextBox ID="txtCliente_Fornec" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_cadastro</p>
                   <asp:TextBox ID="txtData_cadastro" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_entrega</p>
                   <asp:TextBox ID="txtData_entrega" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>hora</p>
                   <asp:TextBox ID="txthora" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Desconto</p>
                   <asp:TextBox ID="txtDesconto" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total</p>
                   <asp:TextBox ID="txtTotal" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Usuario</p>
                   <asp:TextBox ID="txtUsuario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Obs</p>
                   <asp:TextBox ID="txtObs" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CFOP</p>
                   <asp:TextBox ID="txtCFOP" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>orcamento</p>
                   <asp:TextBox ID="txtorcamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>funcionario</p>
                   <asp:TextBox ID="txtfuncionario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo_solicitacao</p>
                   <asp:TextBox ID="txtcodigo_solicitacao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>hora_fim</p>
                   <asp:TextBox ID="txthora_fim" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ID</p>
                   <td><asp:CheckBox ID="chkID" runat="server" Text="ID"/>
                   </td>

                                      <td >                   <p>cotacao</p>
                   <asp:TextBox ID="txtcotacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>impresso</p>
                   <td><asp:CheckBox ID="chkimpresso" runat="server" Text="impresso"/>
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

