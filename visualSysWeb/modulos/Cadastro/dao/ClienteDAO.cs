using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Collections;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.dao
{
    public class ClienteDAO
    {
        public User usr = null;
        public String Codigo_Cliente = "";
        public String Nome_Cliente = "";
        public String Codigo_Portaria = "null";
        public String Situacao = "";
        public String Endereco = "";
        public String Estado_civil = "";
        public String CEP = "";
        public String Bairro = "";
        public String Cidade = "";
        public String UF = "";
        public String CNPJ = "";
        public String IE = "";
        public String Codigo_tabela = "";
        public bool habilita_f9 = false;
        public DateTime Data_Nascimento { get; set; }
        public String Data_NascimentoBr()
        {
            return dataBr(Data_Nascimento);
        }

        public String Naturalidade { get; set; }
        public String Nome_conjuge { get; set; }
        public String Contato { get; set; }
        public Decimal Renda_Mensal { get; set; }
        public bool Pessoa_Juridica { get; set; }
        public Decimal Limite_Credito { get; set; }
        public Decimal Utilizado { get; set; }
        public bool ICM_Isento { get; set; }
        public String Historico { get; set; }
        public DateTime data_cadastro { get; set; }
        public String data_cadastroBr()
        {
            return dataBr(data_cadastro);
        }

        public bool estado_cliente { get; set; }
        public String vendedor { get; set; }
        public String nome_fantasia { get; set; }
        public String Endereco_ent { get; set; }
        public String Cep_ent { get; set; }
        public String Bairro_ent { get; set; }
        public String Cidade_ent { get; set; }
        public String Uf_ent { get; set; }
        public String endereco_nro { get; set; }
        public String complemento_end = "";
        public String complemento_ent { get; set; }

        public String endereco_ent_nro { get; set; }
        public DataTable meiosComunicacao { get; set; }
        public DataTable clienteBanco { get; set; }
        public DataTable localEntrega { get; set; }
        public DataTable mercadorias { get; set; }
        public DataTable entregacozinha { get; set; }
        public bool Iva_descricao = false;
        public bool inativo = false;
        public bool opt_Simples_Nac = false;
        public int indIEDest = 1;
        public bool conta_assinada = false;
        public int grupoEmpresa = 0;
        private string _nomeGrupoEmpresa = "";
        public string nomeGrupoEmpresa
        {
            get
            {
                if (!grupoEmpresa.Equals("") && _nomeGrupoEmpresa.Equals(""))
                {
                    _nomeGrupoEmpresa = Conexao.retornaUmValor("Select grupo  from cliente_grupo where id = " + grupoEmpresa, null);
                }
                return _nomeGrupoEmpresa;
            }
            set
            {
                _nomeGrupoEmpresa = value;
            }
        }

        public string conta_contabil_credito { get; set; }
        public string conta_contabil_debito { get; set; }

        public string Usuario { get; internal set; }
        public string UsuarioAlteracao { get; internal set; }

        public ArrayList arrPets = new ArrayList();
        public List<Cliente_FidelidadeDAO> ArrFidelidade = new List<Cliente_FidelidadeDAO>();
        public bool ativa_terceiro_preco { get; set; } = false;

        public ClienteDAO(String codCliente, User usr)
        {
            this.usr = usr;
            String sql = "Select * from  Cliente where codigo_cliente='" + codCliente.Trim() + "' or cnpj ='" + codCliente.Trim() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public ClienteDAO(User usr)
        {
            this.usr = usr;
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
                    Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                    Nome_Cliente = rs["Nome_Cliente"].ToString();
                    Codigo_Portaria = rs["Codigo_Portaria"].ToString();
                    Situacao = rs["Situacao"].ToString();
                    Endereco = rs["Endereco"].ToString();
                    Estado_civil = rs["Estado_civil"].ToString();
                    CEP = rs["CEP"].ToString();
                    Bairro = rs["Bairro"].ToString();
                    Cidade = rs["Cidade"].ToString();
                    UF = rs["UF"].ToString();
                    CNPJ = rs["CNPJ"].ToString();
                    IE = rs["IE"].ToString();
                    Data_Nascimento = (rs["Data_Nascimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Nascimento"].ToString()));
                    Naturalidade = rs["Naturalidade"].ToString();
                    Nome_conjuge = rs["Nome_conjuge"].ToString();
                    Contato = rs["Contato"].ToString();
                    Renda_Mensal = (Decimal)(rs["Renda_Mensal"].ToString().Equals("") ? new Decimal() : rs["Renda_Mensal"]);
                    Pessoa_Juridica = (rs["Pessoa_Juridica"].ToString().Equals("1") ? true : false);
                    Limite_Credito = (Decimal)(rs["Limite_Credito"].ToString().Equals("") ? new Decimal() : rs["Limite_Credito"]);
                    Utilizado = (Decimal)(rs["Utilizado"].ToString().Equals("") ? new Decimal() : rs["Utilizado"]);
                    ICM_Isento = (rs["ICM_Isento"].ToString().Equals("1") ? true : false);
                    Historico = rs["Historico"].ToString();
                    data_cadastro = (rs["data_cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_cadastro"].ToString()));
                    estado_cliente = (rs["estado_cliente"].ToString().Equals("1") ? true : false);
                    Codigo_tabela = rs["Codigo_tabela"].ToString();
                    vendedor = rs["vendedor"].ToString();
                    nome_fantasia = rs["nome_fantasia"].ToString();
                    Endereco_ent = rs["Endereco_ent"].ToString();
                    Cep_ent = rs["Cep_ent"].ToString();
                    Bairro_ent = rs["Bairro_ent"].ToString();
                    Cidade_ent = rs["Cidade_ent"].ToString();
                    Uf_ent = rs["Uf_ent"].ToString();
                    endereco_nro = rs["endereco_nro"].ToString();
                    complemento_end = rs["complemento_end"].ToString();
                    endereco_ent_nro = rs["endereco_ent_nro"].ToString();
                    meiosComunicacao = Conexao.GetTable("select meio_comunicacao,id_meio_comunicacao,contato from cliente_contato where codigo_cliente='" + Codigo_Cliente + "'", usr, false);
                    clienteBanco = Conexao.GetTable("select a.Numero_banco,b.nome_banco, a.Agencia,a.Conta,a.telefone,a.contato  " +
                                                         " from cliente_banco a inner join banco b on a.numero_banco=b.numero_banco " +
                                                         " where Codigo_cliente= '" + Codigo_Cliente + "'", usr, false);
                    localEntrega = Conexao.GetTable("select LUGAR,ENDERECO,UF,CEP,CIDADE from cliente_local_entrega where codigo_cliente='" + Codigo_Cliente + "'", usr, false);
                    entregacozinha = Conexao.GetTable("SELECT ENDERECO,ENDERECO_NRO NUMERO,BAIRRO,CIDADE,UF,CEP FROM ENTREGA_COZINHA where codigo_cliente='" + Codigo_Cliente + "'", usr, false);
                    mercadorias = Conexao.GetTable("SELECT PLU,DESCRICAO,DATA FROM CLIENTE_MERCADORIA where codigo_cliente='" + Codigo_Cliente + "'", usr, false);
                    Iva_descricao = (rs["iva_descricao"].ToString().Equals("1") ? true : false);
                    inativo = rs["inativo"].ToString().Equals("1");
                    int.TryParse(rs["indIEDest"].ToString(), out indIEDest);
                    habilita_f9 = rs["habilita_f9"].ToString().Equals("1");
                    opt_Simples_Nac = rs["Opt_Simples_nac"].ToString().Equals("1");
                    complemento_ent = rs["complemento_ent"].ToString();
                    conta_assinada = rs["conta_assinada"].ToString().Equals("1");
                    grupoEmpresa = Funcoes.intTry(rs["grupo_empresa"].ToString());
                    conta_contabil_credito = rs["conta_contabil_credito"].ToString();
                    conta_contabil_debito = rs["conta_contabil_debito"].ToString();
                    ativa_terceiro_preco = (Funcoes.intTry(rs["ativa_terceiro_preco"].ToString()) > 0 ?  true : false) ;

                    CarregarPets();
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

            carregarFidelidade();
        }

        private void carregarFidelidade()
        {
            String sql = "Select cliente_fidelidade.*, m.descricao from Cliente_Fidelidade" +
                " inner join mercadoria  as m on m.plu = cliente_fidelidade.plu " +
                " where codigo_cliente ='" + Codigo_Cliente + "' order by data_venda desc;";

            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, null, false);
                ArrFidelidade.Clear();
                while (rs.Read())
                {
                    Cliente_FidelidadeDAO ponto = new Cliente_FidelidadeDAO()
                    {
                        Codigo_cliente = rs["Codigo_cliente"].ToString(),
                        Data_Venda = Funcoes.dtTry(rs["Data_venda"].ToString()),
                        Caixa_saida = Funcoes.intTry(rs["Caixa_saida"].ToString()),
                        Documento = rs["Documento"].ToString(),
                        PLU = rs["PLU"].ToString(),
                        Descricao = rs["Descricao"].ToString(),
                        Qtde_pontos = Funcoes.decTry(rs["Qtde_pontos"].ToString())

                    };

                    ArrFidelidade.Add(ponto);
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

        public DataTable getPagamentos(String status, int dias)
        {
            DateTime dtInicio = DateTime.Now.AddMonths(-dias);
            String sqlWhere = "";

            if (!status.Equals("PREVISTO"))
            {
                if (status.Equals("A VENCER"))
                {
                    sqlWhere += " AND STATUS =1 AND vencimento >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
                else if (status.Equals("CONCLUIDO"))
                {
                    sqlWhere += " AND STATUS =2 ";
                }
                else if (status.Equals("VENCIDOS"))
                {
                    sqlWhere += " AND STATUS =1 AND vencimento < '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
            }
            String sql = "select Documento, " +
                           "conta_a_receber.Codigo_Cliente," +
                           " Pedido = conta_a_receber.documento_emitido," +
                           " convert(varchar,Entrada,103)As Entrada," +
                           " convert(varchar,pagamento,103)As Pagamento  ," +
                           "  ((isnull(Valor,0)-(isnull(desconto,0)))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0) Valor," +
                           " valor_multa = 0," +
                           " CONVERT (VARCHAR ,Emissao,103) as Emissao," +
                           " CONVERT (VARCHAR,vencimento,103) as vencimento," +
                           " Status = case Status when 1  then CASE WHEN Vencimento < convert(date,GETDATE()) then 'VENCIDO' else 'ABERTO' end when 2 then 'CONCLUIDO' when 3 then 'CANCELADO' when 4 then 'LANCADO' END ," +
                           " codigo_centro_custo,id_cartao=isnull(id_cartao,id_finalizadora),id_cc " +
                       " from conta_a_receber left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente left join Cartao on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora and Conta_a_receber.Rede_Cartao =cartao.id_rede " +
                       " where " +
                       " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "' and " +
                        " (Emissao between '" + dtInicio.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "')" +
                        sqlWhere;



            return Conexao.GetTable(sql + " order by convert(varchar,vencimento,102) desc", usr, false);
        }

        public DataTable getPedidos(String statusPedido, int dias)
        {
            DateTime dt = DateTime.Now.AddMonths(-dias);
            String sql = "select pedido.filial,	pedido.pedido, " +
                               " simples= case when pedido_simples=1 " +
                                          " then 'SIM' " +
                                          " ELSE 'NAO' " +
                                        " END, " +
                              " pedido.cliente_fornec,cliente.nome_cliente, " +
                              " Status = case   when pedido.status=0 then 'ABERTO'" +
                                              " when pedido.status=1 then 'PENDENCIA FINANCEIRA'" +
                                              " when pedido.status=2 then 'SUSPENSÃO COMERCIAL' " +
                                              " when pedido.status=3 then 'CANCELADO'  " +
                                              " when pedido.status=4 then 'PENDENCIA COMERCIAL' " +
                                              " when pedido.status=5 then 'EM SEPARAÇÃO' " +
                                              " when pedido.status=6 then 'A FATURAR' " +
                                              " when pedido.status=7 then 'CONCLUIDO' " +
                              " END, " +
                              " Nf=cod_nota," +
                              " Status_nota = nf.status, " +
                              " convert(varchar,pedido.data_Cadastro,103) Data_Cadastro, " +
                              " pedido.Total, " +
                              " funcionario.nome" +
                              " ,uf = cliente.uf" +
                              ",natureza_operacao" +
                          " from pedido inner join cliente on " +
                                  " cliente.codigo_cliente = pedido.cliente_fornec " +
                                  " left join nf on pedido.cod_nota = nf.codigo  and nf.tipo_nf =1 " +
                                  " inner join funcionario on funcionario.codigo=pedido.funcionario " +
                          " where tipo=1 and pedido.cliente_fornec ='" + Codigo_Cliente + "'" +
                          " and pedido.data_cadastro >='" + dt.ToString("yyyy-MM-dd") + "'";

            if (!statusPedido.Equals(""))
            {
                sql += " and pedido.status =" + statusPedido;
            }


            return Conexao.GetTable(sql, null, false);
        }


        public Decimal maiorCompra()
        {
            Decimal maiorCompra = 0;
            String strSqlmax = "Select total= sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) " +
                                    " into #maxCompra from Conta_a_receber  " +
                                    " where " +
                                    " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "' group by documento; " +

                                "Select max(total) from #maxCompra; ";
            Decimal.TryParse(Conexao.retornaUmValor(strSqlmax, usr), out maiorCompra);
            return maiorCompra;
        }
        public String UltimaCompra()
        {

            String strSqlmax = "Select convert(varchar, MAX(Emissao),103)" +
                                       " from Conta_a_receber " +
                                   " where " +
                                   " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "' ";

            return Conexao.retornaUmValor(strSqlmax, usr);
        }

        public Decimal totalAbertos()
        {
            Decimal tAberto = 0;

            String sql = "select sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) Valor" +

                      " from conta_a_receber left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente left join Cartao on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora and Conta_a_receber.Rede_Cartao =cartao.id_rede " +
                      " where " +
                      " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "'  " +
                       " AND STATUS =1 AND vencimento >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            Decimal.TryParse(Conexao.retornaUmValor(sql, usr), out tAberto);

            return tAberto;

        }
        public String totalConcluido()
        {
            Decimal tConcluido = 0;
            DateTime dtInicio = DateTime.Now.AddMonths(-12);

            String sql = "select sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) Valor" +

                      " from conta_a_receber left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente left join Cartao on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora and Conta_a_receber.Rede_Cartao =cartao.id_rede " +
                      " where " +
                      " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "' and " +
                       " (Emissao between '" + dtInicio.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "')" +
                       " AND STATUS =2 ";
            Decimal.TryParse(Conexao.retornaUmValor(sql, usr), out tConcluido);
            return tConcluido.ToString("N2");

        }

        public Decimal totalUtilizado()
        {
            Decimal tAberto = 0;

            String sql = "select sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) Valor" +

                      " from conta_a_receber left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente left join Cartao on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora and Conta_a_receber.Rede_Cartao =cartao.id_rede " +
                      " where " +
                      " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "'  " +
                       " AND STATUS =1 ";

            Decimal.TryParse(Conexao.retornaUmValor(sql, usr), out tAberto);

            return tAberto;

        }

        public Decimal cadernetaSaldo()
        {
            String sql = "Select total = (sum(case when tipo ='CREDITO' THEN (-1 * TOTAL_CADERNETA) ELSE TOTAL_CADERNETA END )) " +
                            " from caderneta" +
                            " where codigo_cliente = '" + Codigo_Cliente + "'";

            return Funcoes.decTry(Conexao.retornaUmValor(sql, usr));

        }

        internal object getCaderneta(int meses)
        {
            DateTime dtDe = DateTime.Now.AddMonths(-meses);
            String Sql = "Select Emissao_Caderneta =CONVERT(varchar,emissao_caderneta,103),Tipo,Documento_Caderneta,Historico_Caderneta,Total_Caderneta,Caixa_Caderneta,lancamento,usuario,data_inclusao=CONVERT(varchar,data_inclusao,103) from Caderneta with( index(IX_ID_Caderneta))   where codigo_cliente ='" + Codigo_Cliente + "'";

            if (!dtDe.Equals(DateTime.MinValue))
            {
                Sql += " and convert(dateTime ,convert(varchar,emissao_caderneta,102)) between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }


            return Conexao.GetTable(Sql + " order by CONVERT(varchar,emissao_caderneta,102) desc", usr, false);

        }

        public Decimal totalAtrasados()
        {
            return totalAtrasados(DateTime.Now);
        }

        public Decimal totalAtrasados(DateTime dt)
        {
            Decimal tAtrasado = 0;


            String sql = "select sum(((isnull(Valor,0)-isnull(desconto,0))+isnull(acrescimo,0))-isnull(conta_a_receber.taxa,0)) Valor" +

                      " from conta_a_receber left join cliente on conta_a_receber.codigo_cliente = cliente.codigo_cliente left join Cartao on convert(int,Conta_a_receber.id_Bandeira) = convert(int,Cartao.id_bandeira) and Conta_a_receber.finalizadora= Cartao.nro_Finalizadora and Conta_a_receber.Rede_Cartao =cartao.id_rede " +
                      " where " +
                      " conta_a_receber.Codigo_Cliente ='" + Codigo_Cliente + "'" +
                       " AND STATUS =1 AND vencimento < '" + dt.ToString("yyyy-MM-dd") + "'";
            Decimal.TryParse(Conexao.retornaUmValor(sql, usr), out tAtrasado);
            return tAtrasado;
        }



        public void addPet(cliente_petDAO pet)
        {
            if (!Codigo_Cliente.Equals("") && pet.codigo_pet.Equals(""))
            {
                pet.codigo_pet = Codigo_Cliente.Trim() + (arrPets.Count + 1).ToString();
            }
            pet.Codigo_Cliente = Codigo_Cliente;
            pet.index = arrPets.Count;
            arrPets.Add(pet);
        }
        public void atualizarPet(cliente_petDAO pet)
        {
            arrPets[pet.index] = pet;
        }

        public DataTable tbPets()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Codigo_pet");
            cabecalho.Add("Nome_pet");
            cabecalho.Add("Sexo");
            cabecalho.Add("Codigo_cliente");
            cabecalho.Add("Cor");
            cabecalho.Add("Data_Nascimento");
            cabecalho.Add("Pelagem");
            cabecalho.Add("Porte");
            cabecalho.Add("Raca");
            cabecalho.Add("Ultimo_cio");
            cabecalho.Add("especie");
            cabecalho.Add("pedigree");

            itens.Add(cabecalho);
            if (arrPets != null && arrPets.Count > 0)
            {
                foreach (cliente_petDAO item in arrPets)
                {
                    itens.Add(item.ArrToString());

                }
            }
            return Conexao.GetArryTable(itens);

        }

        public void CarregarPets()
        {
            String strSql = "Select * from cliente_pet where codigo_cliente='" + Codigo_Cliente + "'";
            SqlDataReader rs = Conexao.consulta(strSql, usr, false);
            while (rs.Read())
            {
                cliente_petDAO pet = new cliente_petDAO(usr);
                pet.codigo_pet = rs["Codigo_pet"].ToString();
                pet.Sexo = rs["sexo"].ToString();
                pet.Nome_Pet = rs["Nome_pet"].ToString();
                pet.Codigo_Cliente = Codigo_Cliente;
                pet.Cor = rs["Cor"].ToString();
                DateTime.TryParse(rs["Data_Nascimento"].ToString(), out pet.Data_Nascimento);
                pet.Pelagem = rs["Pelagem"].ToString();
                pet.Porte = rs["Porte"].ToString();
                pet.Raca = rs["Raca"].ToString();
                DateTime.TryParse(rs["Ultimo_Cio"].ToString(), out pet.Ultimo_Cio);
                pet.Especie = rs["Especie"].ToString();
                pet.pedigree = rs["pedigree"].ToString();
                pet.carregarVacinas();
                pet.carregarObservacoes();
                pet.carregarImagens();
                addPet(pet);
            }

        }


        public void addClienteBanco(String numero_banco, String nome_banco, String agencia, String conta, String telefone, String contato)
        {
            if (clienteBanco == null)
            {
                clienteBanco = Conexao.GetTable("select a.Numero_banco,b.nome_banco, a.Agencia,a.Conta,a.telefone,a.contato  " +
                                                  " from cliente_banco a inner join banco b on a.numero_banco=b.numero_banco " +
                                                  " where Codigo_cliente= '" + Codigo_Cliente + "'", usr, false);
            }

            DataRow drRow;
            if (clienteBanco.Rows.Count > 0)
            {
                drRow = clienteBanco.Rows[0];

                if (!drRow[0].Equals("------"))
                {
                    drRow = clienteBanco.NewRow();

                    drRow[0] = numero_banco;
                    drRow[1] = nome_banco;
                    drRow[2] = agencia;
                    drRow[3] = conta;
                    drRow[4] = telefone;
                    drRow[5] = contato;

                    clienteBanco.Rows.Add(drRow);
                }
                else
                {

                    drRow[0] = numero_banco;
                    drRow[1] = nome_banco;
                    drRow[2] = agencia;
                    drRow[3] = conta;
                    drRow[4] = telefone;
                    drRow[5] = contato;
                }

            }
            else
            {
                drRow = clienteBanco.NewRow();

                drRow[0] = numero_banco;
                drRow[1] = nome_banco;
                drRow[2] = agencia;
                drRow[3] = conta;
                drRow[4] = telefone;
                drRow[5] = contato;

                clienteBanco.Rows.Add(drRow);

            }
        }
        public void deleteClienteBanco(String numero_banco, String agencia, String conta)
        {
            foreach (DataRow item in clienteBanco.Rows)
            {
                if (item[0].Equals(numero_banco) && item[2].Equals(agencia) && item[3].Equals(conta))
                {
                    clienteBanco.Rows.Remove(item);
                    break;
                }
            }
        }
        private void atualizarClienteBanco(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (!Codigo_Cliente.Equals("") && clienteBanco != null)
                {
                    Conexao.executarSql("delete from cliente_banco where codigo_cliente='" + Codigo_Cliente + "'", conn, tran);
                    for (int i = 0; i < clienteBanco.Rows.Count; i++)
                    {
                        DataRow drRow = clienteBanco.Rows[i];
                        if (!drRow[0].Equals("------"))
                        {
                            String sql = "insert into cliente_banco ( numero_banco,agencia , conta,telefone,contato,codigo_cliente " +
                                     " )values (" +
                                "'" + drRow[0] + "'," +
                                "'" + drRow[2] + "'," +
                                "'" + drRow[3] + "'," +
                                "'" + drRow[4] + "'," +
                                "'" + drRow[5] + "'," +
                                "'" + Codigo_Cliente + "')";


                            Conexao.executarSql(sql, conn, tran);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception("Atualizar Bancos Arquivo:ClienteDAO.cs" + e.Message);
            }
        }

        public bool existeMeioComunica(String meio, String id)
        {
            bool retKCS = false;
            try
            {
                foreach (DataRow item in meiosComunicacao.Rows)
                {
                    if (item[0].Equals(meio) && item[1].Equals(id))
                    {
                        retKCS = true;
                    }
                }
            }
            catch
            {
                retKCS = false;
            }
            return retKCS;
        }

        public void addMeioComunicacao(String meio_comunicacao, String id_meio_comunicacao, String contato)
        {




            if (id_meio_comunicacao.Trim().Equals(""))
                throw new Exception("ID do Meio de Comunicação não foi informado!!");
            if (meiosComunicacao == null)
            {
                meiosComunicacao = Conexao.GetTable("select '' as meio_comunicacao,'' as id_meio_comunicacao,''as contato", usr, false);
            }

            foreach (DataRow item in meiosComunicacao.Rows)
            {
                if (item[0].Equals(meio_comunicacao) &&
                    item[1].Equals(id_meio_comunicacao)
                    )
                {
                    throw new Exception("Meio de Comunicação Já Cadastrado!!");
                }
            }

            DataRow drRow;

            if (meiosComunicacao.Rows.Count >= 1)
            {
                drRow = meiosComunicacao.Rows[0]; ;

                if (!drRow[0].Equals("------") && !drRow[0].ToString().Equals(""))
                {
                    drRow = meiosComunicacao.NewRow();
                    drRow[0] = meio_comunicacao;
                    drRow[1] = id_meio_comunicacao;
                    drRow[2] = contato;
                    meiosComunicacao.Rows.Add(drRow);
                }
                else
                {
                    meiosComunicacao.Rows.Remove(drRow);
                    drRow = meiosComunicacao.NewRow();
                    drRow[0] = meio_comunicacao;
                    drRow[1] = id_meio_comunicacao;
                    drRow[2] = contato;
                    meiosComunicacao.Rows.Add(drRow);
                }

            }
            else
            {
                drRow = meiosComunicacao.NewRow();
                drRow[0] = meio_comunicacao;
                drRow[1] = id_meio_comunicacao;
                drRow[2] = contato;
                meiosComunicacao.Rows.Add(drRow);

            }
        }
        public String primeiroMeioComunicacao()
        {
            foreach (DataRow item in meiosComunicacao.Rows)
            {
                if ((item[0].ToString().ToUpper().IndexOf("FONE") >= 0) || (item[0].ToString().ToUpper().IndexOf("CELULAR") >= 0))
                    return item[1].ToString();

            }
            return "";
        }
        public String email()
        {
            foreach (DataRow item in meiosComunicacao.Rows)
            {
                if (item[0].ToString().ToUpper().Trim().Equals("EMAIL"))
                {
                    return item[1].ToString();
                }
            }
            return "";
        }


        public void deleteMeioComunicacao(String idMeioComunicacao)
        {
            foreach (DataRow item in meiosComunicacao.Rows)
            {
                if (item[1].Equals(idMeioComunicacao))
                {
                    meiosComunicacao.Rows.Remove(item);
                    break;
                }
            }
        }
        private void atualizarMeioComunicacao(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (!Codigo_Cliente.Equals("") && meiosComunicacao != null)
                {
                    Conexao.executarSql("delete from cliente_contato where codigo_cliente='" + Codigo_Cliente + "'", conn, tran);
                    for (int i = 0; i < meiosComunicacao.Rows.Count; i++)
                    {

                        DataRow drRow = meiosComunicacao.Rows[i];
                        if (!drRow[0].ToString().Equals("------"))
                        {
                            String sql = "insert into cliente_contato ( meio_comunicacao, id_meio_comunicacao, contato,codigo_cliente " +
                                     " )values (" +
                                "'" + drRow[0] + "'," +
                                "'" + drRow[1] + "'," +
                                "'" + drRow[2] + "'," +
                                "'" + Codigo_Cliente + "')";


                            Conexao.executarSql(sql, conn, tran);
                        }
                    }

                }
            }
            catch (Exception e)
            {

                throw new Exception("Atualizar os meios de comunicação: Arquivo:ClienteDAO.cs:" + e.Message);
            }
        }

        public void addLocalEntrega(String lugar, String endereco, String uf, String cep, String cidade)
        {
            DataRow drRow;
            if (localEntrega.Rows.Count > 0)
            {
                drRow = localEntrega.Rows[0]; ;

                if (!drRow[0].Equals("------"))
                {
                    drRow = localEntrega.NewRow();
                    drRow[0] = lugar;
                    drRow[1] = endereco;
                    drRow[2] = uf;
                    drRow[3] = cep;
                    drRow[4] = cidade;


                    localEntrega.Rows.Add(drRow);
                }
                else
                {

                    drRow[0] = lugar;
                    drRow[1] = endereco;
                    drRow[2] = uf;
                    drRow[3] = cep;
                    drRow[4] = cidade;

                }

            }
            else
            {
                drRow = localEntrega.NewRow();
                drRow[0] = lugar;
                drRow[1] = endereco;
                drRow[2] = uf;
                drRow[3] = cep;
                drRow[4] = cidade;

                localEntrega.Rows.Add(drRow);

            }
        }
        public void deletelocalEntrega(String lugar)
        {
            foreach (DataRow item in localEntrega.Rows)
            {
                if (item[1].Equals(lugar))
                {
                    localEntrega.Rows.Remove(item);
                    break;
                }
            }
        }
        private void atualizarlocalEntrega(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (!Codigo_Cliente.Equals(""))
                {
                    if (localEntrega != null && localEntrega.Rows != null)
                    {
                        Conexao.executarSql("delete from cliente_local_entrega where codigo_cliente='" + Codigo_Cliente + "'", conn, tran);

                        for (int i = 0; i < localEntrega.Rows.Count; i++)
                        {
                            DataRow drRow = localEntrega.Rows[i];
                            if (!drRow[0].Equals("------"))
                            {
                                String sql = "insert into cliente_local_entrega  ( LUGAR,ENDERECO,UF,CEP,CIDADE,codigo_cliente " +
                                         " )values (" +
                                    "'" + drRow[0] + "'," +
                                    "'" + drRow[1] + "'," +
                                    "'" + drRow[2] + "'," +
                                    "'" + drRow[3] + "'," +
                                    "'" + drRow[4] + "'," +
                                    "'" + Codigo_Cliente + "')";


                                Conexao.executarSql(sql, conn, tran);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception("Atualizar os local de Entrega: Arquivo:ClienteDAO.cs:" + e.Message);
            }
        }

        public void addEntregaCozinha(String endereco, String enderecoNro, String bairro, String cidade, String uf, String cep)
        {
            DataRow drRow;
            if (entregacozinha.Rows.Count > 0)
            {
                drRow = entregacozinha.Rows[0]; ;

                if (!drRow[0].Equals("------"))
                {
                    drRow = entregacozinha.NewRow();
                    drRow[0] = endereco;
                    drRow[1] = enderecoNro;
                    drRow[2] = bairro;
                    drRow[3] = cidade;
                    drRow[4] = uf;
                    drRow[5] = cep;

                    entregacozinha.Rows.Add(drRow);
                }
                else
                {
                    drRow[0] = endereco;
                    drRow[1] = enderecoNro;
                    drRow[2] = bairro;
                    drRow[3] = cidade;
                    drRow[4] = uf;
                    drRow[5] = cep;

                }

            }
            else
            {
                drRow = entregacozinha.NewRow();

                drRow[0] = endereco;
                drRow[1] = enderecoNro;
                drRow[2] = bairro;
                drRow[3] = cidade;
                drRow[4] = uf;
                drRow[5] = cep;

                entregacozinha.Rows.Add(drRow);

            }
        }
        public void deleteEntregaCozinha(String endereco, String enderecoNro)
        {
            foreach (DataRow item in entregacozinha.Rows)
            {
                if (item[0].Equals(endereco) && item[1].Equals(enderecoNro))
                {
                    entregacozinha.Rows.Remove(item);
                    break;
                }
            }
        }
        private void atualizarEntregaCozinha(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (!Codigo_Cliente.Equals(""))
                {
                    if (entregacozinha != null && entregacozinha.Rows != null)
                    {
                        Conexao.executarSql("delete from entrega_cozinha where codigo_cliente='" + Codigo_Cliente + "'", conn, tran);
                        for (int i = 0; i < entregacozinha.Rows.Count; i++)
                        {
                            DataRow drRow = entregacozinha.Rows[i];
                            if (!drRow[0].Equals("------"))
                            {
                                String sql = "insert into entrega_cozinha  ( endereco,endereco_nro,bairro,cidade,uf,codigo_cliente " +
                                         " )values (" +
                                    "'" + drRow[0] + "'," +
                                    "'" + drRow[1] + "'," +
                                    "'" + drRow[2] + "'," +
                                    "'" + drRow[3] + "'," +
                                    "'" + drRow[4] + "'," +
                                    "'" + Codigo_Cliente + "')";


                                Conexao.executarSql(sql, conn, tran);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception("Atualizar os local de Entrega cozinha: Arquivo:ClienteDAO.cs:" + e.Message);
            }
        }


        public void Exclui()
        {
            Conexao.executarSql("update cliente set inativo=1 where codigo_cliente=" + Codigo_Cliente);


        }

        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  Cliente set " +
                           "Nome_Cliente='" + Funcoes.RemoverAcentos(Nome_Cliente) + "'," +
                           "Codigo_Portaria=" + (Codigo_Portaria.Trim().Equals("") ? "null" : Codigo_Portaria) + "," +
                           "Situacao='" + Situacao + "'," +
                           "Endereco='" + Funcoes.RemoverAcentos(Endereco) + "'," +
                           "Estado_civil='" + (Estado_civil.Trim().Equals("") ? "Solteiro" : Estado_civil) + "'," +
                           "CEP='" + CEP + "'," +
                           "Bairro='" + Funcoes.RemoverAcentos(Bairro) + "'," +
                           "Cidade='" + Funcoes.RemoverAcentos(Cidade) + "'," +
                           "UF='" + UF + "'," +
                           "CNPJ='" + CNPJ + "'," +
                           "IE='" + IE + "'," +
                           "Data_Nascimento=" + (Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Nascimento.ToString("yyyy-MM-dd") + "'") + "," +
                           "Naturalidade='" + Naturalidade + "'," +
                           "Nome_conjuge='" + Funcoes.RemoverAcentos(Nome_conjuge) + "'," +
                           "Contato='" + Contato + "'," +
                           "Renda_Mensal=" + Renda_Mensal.ToString().Replace(",", ".") + "," +
                           "Pessoa_Juridica=" + (Pessoa_Juridica ? "1" : "0") + "," +
                           "Limite_Credito=" + Limite_Credito.ToString().Replace(",", ".") + "," +
                           "Utilizado=" + Utilizado.ToString().Replace(",", ".") + "," +
                           "ICM_Isento=" + (ICM_Isento ? "1" : "0") + "," +
                           "Historico='" + Funcoes.RemoverAcentos(Historico) + "'," +
                           "data_cadastro=" + (data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_cadastro.ToString("yyyy-MM-dd") + "'") + "," +
                           "estado_cliente=" + (estado_cliente ? "1" : "0") + "," +
                           "Codigo_tabela='" + Codigo_tabela + "'," +
                           "vendedor='" + vendedor + "'," +
                           "nome_fantasia='" + Funcoes.RemoverAcentos(nome_fantasia) + "'," +
                           "Endereco_ent='" + Funcoes.RemoverAcentos(Endereco_ent) + "'," +
                           "Cep_ent='" + Cep_ent + "'," +
                           "Bairro_ent='" + Funcoes.RemoverAcentos(Bairro_ent) + "'," +
                           "Cidade_ent='" + Funcoes.RemoverAcentos(Cidade_ent) + "'," +
                           "Uf_ent='" + Uf_ent + "'," +
                           "endereco_nro='" + Funcoes.RemoverAcentos(endereco_nro) + "'," +
                           "complemento_end='" + Funcoes.RemoverAcentos(complemento_end) + "'," +
                           "iva_descricao=" + (Iva_descricao ? "1" : "0") + "," +
                           "endereco_ent_nro='" + endereco_ent_nro + "'" +
                           ",inativo =" + (inativo ? "1" : "0") +
                           ",indIEDest=" + indIEDest +
                           ",habilita_f9=" + (habilita_f9 ? "1" : "0") +
                           ",Opt_Simples_nac=" + (opt_Simples_Nac ? "1" : "0") +
                           ",complemento_ent='" + complemento_ent + "'" +
                           ",conta_assinada=" + (conta_assinada ? "1" : "0") +
                           ",grupo_empresa=" + grupoEmpresa +
                           ",conta_contabil_credito='"+conta_contabil_credito+"'"+
                           ",conta_contabil_debito='"+conta_contabil_debito +"'"+
                           ",ativa_terceiro_preco=" +(ativa_terceiro_preco?"1":"0")+
                 " where  Codigo_Cliente='" + Codigo_Cliente + "'";
                Conexao.executarSql(sql, conn, tran);
                atualizarMeioComunicacao(conn, tran);
                atualizarClienteBanco(conn, tran);
                atualizarlocalEntrega(conn, tran);
                atualizarEntregaCozinha(conn, tran);
                atualizarPets(conn, tran);


            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:Arquivo:ClienteDAO.cs linha:303:" + err.Message);
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
                tran.Commit();
                return true;
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



        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                bool validaCnpj = Funcoes.valorParametro("NOME_FANTASIA_ENF", usr).ToUpper().Equals("TRUE");
                bool salvaSequencia = false;
                if (!validaCnpj && !CNPJ.Trim().Equals(""))
                {
                    if (Conexao.countSql("select cnpj from cliente where replace(replace(replace(cnpj,'.',''),'-',''),'/','')='" + CNPJ.Replace(".", "").Replace("-", "").Replace("/", "") + "'", null) > 0)
                        throw new Exception("CNPJ JÁ ESTA CADASTRADO");
                }

                if (Codigo_Cliente.Trim().Equals(""))
                {
                    Codigo_Cliente = Conexao.retornaUmValor("select sequencia from sequenciais where ltrim(rtrim(tabela_coluna)) = 'CLIENTE.CODIGO_CLIENTE'", usr);
                    Codigo_Cliente = (long.Parse(Codigo_Cliente) + 1).ToString();
                    salvaSequencia = true;
                }

                bool existe = (Funcoes.intTry(Conexao.retornaUmValor("select count(*) from cliente where ltrim(rtrim(codigo_cliente))='" + Codigo_Cliente.Trim() + "'", null)) > 0);
                if (existe)
                {
                    throw new Exception("Codigo de Cliente já cadastrado");
                }



                data_cadastro = DateTime.Now;

                if (Situacao.Trim().Equals(""))
                    Situacao = "OK";



                String sql = " insert into Cliente(" +
                          "Codigo_Cliente," +
                          "Nome_Cliente," +

                          "Situacao," +
                          "Endereco," +
                          "Estado_civil," +
                          "CEP," +
                          "Bairro," +
                          "Cidade," +
                          "UF," +
                          "CNPJ," +
                          "IE," +
                          "Data_Nascimento," +
                          "Naturalidade," +
                          "Nome_conjuge," +
                          "Contato," +
                          "Renda_Mensal," +
                          "Pessoa_Juridica," +
                          "Limite_Credito," +
                          "Utilizado," +
                          "ICM_Isento," +
                          "Historico," +
                          "data_cadastro," +
                          "estado_cliente," +
                          "Codigo_tabela," +
                          "vendedor," +
                          "nome_fantasia," +
                          "Endereco_ent," +
                          "Cep_ent," +
                          "Bairro_ent," +
                          "Cidade_ent," +
                          "Uf_ent," +
                          "endereco_nro," +
                          "complemento_end," +
                          "iva_descricao," +
                          "endereco_ent_nro" +
                          ",inativo" +
                          ",indIEDest" +
                          ",habilita_f9" +
                          ",Opt_Simples_nac" +
                          ",complemento_ent" +
                          ",conta_assinada" +
                          ",grupo_empresa" +
                          ",conta_contabil_credito"+
                          ",conta_contabil_debito"+
                          ",ativa_terceiro_preco"+
                     " )values (" +
                          "'" + Codigo_Cliente + "'," +
                          "'" + Funcoes.RemoverAcentos(Nome_Cliente) + "'," +

                          "'" + Situacao + "'," +
                          "'" + Funcoes.RemoverAcentos(Endereco) + "'," +
                          (Estado_civil.Equals("") ? "null" : "'" + Estado_civil + "'") + "," +
                          "'" + CEP + "'," +
                          "'" + Funcoes.RemoverAcentos(Bairro) + "'," +
                          "'" + Funcoes.RemoverAcentos(Cidade) + "'," +
                          "'" + UF + "'," +
                          "'" + CNPJ + "'," +
                          "'" + IE + "'," +
                          (Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Nascimento.ToString("yyyy-MM-dd") + "'") + "," +
                          "'" + Naturalidade + "'," +
                          "'" + Funcoes.RemoverAcentos(Nome_conjuge) + "'," +
                          "'" + Funcoes.RemoverAcentos(Contato) + "'," +
                          Renda_Mensal.ToString().Replace(",", ".") + "," +
                          (Pessoa_Juridica ? 1 : 0) + "," +
                          Limite_Credito.ToString().Replace(",", ".") + "," +
                          Utilizado.ToString().Replace(",", ".") + "," +
                          (ICM_Isento ? 1 : 0) + "," +
                          "'" + Funcoes.RemoverAcentos(Historico) + "'," +
                          "'" + data_cadastro.ToString("yyyy-MM-dd") + "'," +
                          (estado_cliente ? 1 : 0) + "," +
                          "'" + Codigo_tabela + "'," +
                          "'" + vendedor + "'," +
                          "'" + Funcoes.RemoverAcentos(nome_fantasia) + "'," +
                          "'" + Funcoes.RemoverAcentos(Endereco_ent) + "'," +
                          "'" + Cep_ent + "'," +
                          "'" + Funcoes.RemoverAcentos(Bairro_ent) + "'," +
                          "'" + Funcoes.RemoverAcentos(Cidade_ent) + "'," +
                          "'" + Uf_ent + "'," +
                          "'" + endereco_nro + "'," +
                          "'" + Funcoes.RemoverAcentos(complemento_end) + "'," +
                          (Iva_descricao ? "1" : "0") + "," +
                          "'" + endereco_ent_nro + "'" +
                          "," + (inativo ? "1" : "0") +
                          "," + indIEDest +
                          "," + (habilita_f9 ? "1" : "0") +
                          "," + (opt_Simples_Nac ? "1" : "0") +
                          ",'" + complemento_ent + "'" +
                          "," + (conta_assinada ? "1" : "0") +
                          "," + grupoEmpresa +
                          ",'"+conta_contabil_credito+"'"+
                          ",'"+conta_contabil_debito+"'"+
                          ","+ (ativa_terceiro_preco?"1":"0")+
                " )";
                Conexao.executarSql(sql, conn, tran);
                if (salvaSequencia)
                {
                    Conexao.executarSql("update sequenciais set sequencia='" + Codigo_Cliente + "' where tabela_coluna = 'CLIENTE.CODIGO_CLIENTE    '", conn, tran);
                }

                if (Codigo_Cliente.Trim().Length >= 9)
                {
                    String meio = Conexao.retornaUmValor("Select Meio_comunicacao from meio_comunicacao where meio_comunicacao like '%FONE%' ", null);
                    addMeioComunicacao(meio, Codigo_Cliente.Trim(), "");
                }

                atualizarMeioComunicacao(conn, tran);
                atualizarClienteBanco(conn, tran);
                atualizarlocalEntrega(conn, tran);
                atualizarEntregaCozinha(conn, tran);
                atualizarPets(conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir o novo cliente erro:" + err.Message);
            }
        }

        private void atualizarPets(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("delete from cliente_pet_vacina where codigo_cliente ='" + this.Codigo_Cliente + "'", conn, tran);
            Conexao.executarSql("delete from cliente_pet where codigo_cliente='" + this.Codigo_Cliente + "'", conn, tran);
            foreach (cliente_petDAO item in arrPets)
            {
                item.Codigo_Cliente = this.Codigo_Cliente;
                if (item.codigo_pet.Equals(""))
                {
                    item.codigo_pet = Codigo_Cliente + item.index + 1;
                }
                item.salvar(true, conn, tran);



            }

        }

        internal static void incluirNovoGrupoEmpresa(string grupo)
        {
            int exist = Funcoes.intTry(Conexao.retornaUmValor("select count(*) from cliente_grupo where grupo = '" + grupo.Trim() + "'", null));

            if (exist > 0)
                throw new Exception("Grupo já Existe!");


            String sql = "insert into cliente_grupo values('" + grupo.Trim() + "');";
            Conexao.executarSql(sql);
        }

        public bool salvarEnderecoNFe()
        {
            SqlConnection conn = Conexao.novaConexao();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "UPDATE Cliente SET Endereco = '" + this.Endereco + "', Cidade = '" + this.Cidade + "', Bairro = ";
                    sql += "'" + this.Bairro + "', Endereco_nro = '" + this.endereco_ent_nro + "' WHERE Codigo_Cliente = '" + this.Codigo_Cliente + "'";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                    SqlConnection.ClearPool(conn);
                }
            }
        }
    }
}