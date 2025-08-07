using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Collections;
using visualSysWeb.modulos.Cadastro.dao;
using visualSysWeb.modulos.Cadastro.code;

namespace visualSysWeb.dao
{
    public class fornecedorDAO
    {
        User usr = null;
        #region PROPRIEDADES
        public String Fornecedor { get; set; }
        public String Filial { get; set; }
        public String Razao_social { get; set; }
        public String Nome_Fantasia { get; set; }
        public String CNPJ { get; set; }
        public String IE { get; set; }
        public String Cidade { get; set; }
        public String UF { get; set; }
        public String CEP { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public Decimal Desc_Coml { get; set; }
        public Decimal Desc_Finan { get; set; }
        public Decimal Adc_Finan { get; set; }
        public Decimal Adc_Mkt { get; set; }
        public Decimal Adc_Perda { get; set; }
        public Decimal Adc_Frete { get; set; }
        public DateTime Ultima_Contagem_do_Estoque { get; set; }
        public String Ultima_Contagem_do_EstoqueBr()
        {
            return dataBr(Ultima_Contagem_do_Estoque);
        }

        public Decimal Bonificacao { get; set; }
        public Decimal Prazo { get; set; }
        public String condicao_pagamento { get; set; }
        public Decimal Desc_exp { get; set; }
        public String obs { get; set; }
        public bool pessoa_fisica { get; set; }
        public String senha { get; set; }
        public String Codigo_fornecedor { get; set; }
        public String Endereco_nro { get; set; }
        public bool Formulario_proprio { get; set; }
        public DataTable contatos { get; set; }
        public DataTable meiosComunicacao { get; set; }
        public DataTable ocorrencia { get; set; }
        public bool Tipo_fornecedor { get; set; }
        public string conta_contabil_credito { get; set; }
        public string conta_contabil_debito { get; set; }
        public bool despesas_base { get; set; } = false;
        public bool ipi_base { get; set; } = false;
        public bool vDespesas_base
        {
            get
            {
                return Conexao.retornaUmValor("Select despesas_base from fornecedor where fornecedor='" + Fornecedor + "'",null).Equals("1");
            }
        }
        public bool vIpi_base
        {
            get
            {
                return Conexao.retornaUmValor("Select ipi_base from fornecedor where fornecedor='" + Fornecedor + "'",null).Equals("1");
            }
        }
        private ArrayList arrDepartamentos = new ArrayList();
        private List<FornecedorMercadoria> _Mercadorias = new List<FornecedorMercadoria>();
        public List<FornecedorMercadoria> Mercadorias
        {
            get
            {
                if (_Mercadorias.Count > 0)
                    return _Mercadorias;
                else
                {
                    List<FornecedorMercadoria> _ClearMercadorias = new List<FornecedorMercadoria>();
                    FornecedorMercadoria item = new FornecedorMercadoria();

                    _ClearMercadorias.Add(item);
                    return _ClearMercadorias;

                }
            }
        }

        public FornecedorMercadoria mercadoria(int index)
        {
            return _Mercadorias[index];
        }

        public DataTable departamentos
        {
            get
            {

                ArrayList grid = new ArrayList();
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("codigo_departamento");
                cabecalho.Add("grupo");
                cabecalho.Add("subgrupo");
                cabecalho.Add("departamento");
                grid.Add(cabecalho);

                foreach (FornecedorDepartamento item in arrDepartamentos)
                {
                    ArrayList arritem = new ArrayList();
                    arritem.Add(item.codigo_departamento);
                    arritem.Add(item.Grupo);
                    arritem.Add(item.subGrupo);
                    arritem.Add(item.Departamento);
                    grid.Add(arritem);
                }

                return Conexao.GetArryTable(grid);
            }



            set { }
        }

        public string Usuario { get; internal set; }
        public string UsuarioAlteracao { get; internal set; }

        public String codmun = "";
        public String email = "";
        public String site = "";
        public String telefone1 = "";
        public String contato1 = "";

        public String telefone2 = "";
        public String contato2 = "";

        public String telefone3 = "";
        public String contato3 = "";

        public String centro_custo = "";
        public bool produtor_rural = false;
        public int indIEDest = 1;

        public String cargo1 = "";
        public String telefone1_2 = "";
        public String telefone1_3 = "";
        public String email1 = "";

        public String cargo2 = "";
        public String telefone2_2 = "";
        public String telefone2_3 = "";
        public String email2 = "";

        public String cargo3 = "";
        public String telefone3_2 = "";
        public String telefone3_3 = "";
        public String email3 = "";
        public int inativo = 0;

        public string dataAlteracaoOcorrencia = "";
        public bool editadoOcorrencia = false; 
        public int ocorrenciasPendentes = 0;
        public DataTable ocorrenciasPendentesDT;
        public int CRT = 0;
        #endregion

        public fornecedorDAO(String fornecedor, User usr)
        {
            this.usr = usr;
            //usr.consultaTodasFiliais = true;
            String sql = "Select * from  fornecedor where fornecedor='" + fornecedor + "' or replace(replace(replace(cnpj,'.',''),'-',''),'/','') ='" + fornecedor + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, false);

            try
            {

                carregarDados(rs);
            }
            catch (Exception err)
            {

                throw new Exception("Não foi possivel carregar os dados do Fornecedor erro:" + err.Message);
            }


        }
        public fornecedorDAO(User usr)
        {
            this.usr = usr;
        }
        public bool departamentoExiste(String cod)
        {
            bool resp = false;
            foreach (FornecedorDepartamento item in arrDepartamentos)
            {
                if (item.codigo_departamento.Trim().Equals(cod.Trim()))
                {
                    resp = true;
                    break;
                }
            }
            return resp;
        }


        public bool excluirDepartamento(String cod)
        {
            bool resp = false;
            foreach (FornecedorDepartamento item in arrDepartamentos)
            {
                if (item.codigo_departamento.Equals(cod))
                {
                    resp = true;
                    arrDepartamentos.Remove(item);
                    break;
                }
            }
            return resp;
        }

        public void addDepartamento(String cod)
        {
            String sql = "Select Wd.codigo_departamento, " +
                                         " Grupo= Wd.Descricao_grupo," +
                                         " SubGrupo = Wd.descricao_subgrupo," +
                                         " Departamento=Wd.descricao_departamento " +

                                     " from W_BR_CADASTRO_DEPARTAMENTO as Wd " +
                                     " Where Wd.codigo_departamento ='" + cod + "'";

            SqlDataReader rsDep = null;

            try
            {

                rsDep = Conexao.consulta(sql, null, false);
                if (rsDep.Read())
                {
                    FornecedorDepartamento dep = new FornecedorDepartamento();
                    dep.codigo_departamento = rsDep["codigo_departamento"].ToString();
                    dep.Grupo = rsDep["Grupo"].ToString();
                    dep.subGrupo = rsDep["subGrupo"].ToString();
                    dep.Departamento = rsDep["Departamento"].ToString();
                    arrDepartamentos.Add(dep);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsDep != null)
                {
                    rsDep.Close();
                }
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

        public void addMeioComunicacao(String meio_comunicacao, String id_meio_comunicacao, String contato)
        {
            if (id_meio_comunicacao.Trim().Equals(""))
                throw new Exception("Id de Meio de comunicação não informado!");

            DataRow drRow;
            if (meiosComunicacao == null)
            {
                meiosComunicacao = Conexao.GetTable("select '' as meio_comunicacao,'' as id_meio_comunicacao,''as contato", usr, false);
            }
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
                if (!Fornecedor.Equals("") && meiosComunicacao != null)
                {
                    Conexao.executarSql("delete from Fornecedor_meio_comunicacao where fornecedor='" + Fornecedor + "'");
                    for (int i = 0; i < meiosComunicacao.Rows.Count; i++)
                    {

                        DataRow drRow = meiosComunicacao.Rows[i];
                        if (!drRow[0].ToString().Equals("------"))
                        {
                            String sql = "insert into Fornecedor_meio_comunicacao (filial, meio_comunicacao, id_comunicacao, contatao,fornecedor " +
                                     " )values (' '," +
                                "'" + drRow[0] + "'," +
                                "'" + drRow[1] + "'," +
                                "'" + drRow[2] + "'," +
                                "'" + Fornecedor + "')";


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

        public void addOcorrencia(String texto_ocorrencia)
        {
            if (texto_ocorrencia.Trim().Equals(""))
                throw new Exception("Ocorrência não informada!");

            DataRow drRow;
            if (ocorrencia == null)
            {
                ocorrencia = Conexao.GetTable("select '' as Data,'' as Ocorrencia, '' as Status, ''as Usuario", usr, false);
            }
            if (ocorrencia.Rows.Count >= 1)
            {
                drRow = ocorrencia.Rows[0]; 

                if (!drRow[0].Equals("------") && !drRow[0].ToString().Equals(""))
                {
                    drRow = ocorrencia.NewRow();
                    drRow[0] = DateTime.Now;
                    drRow[1] = texto_ocorrencia;
                    drRow[2] = "PENDENTE";
                    drRow[3] = usr.getNome().ToString();
                    ocorrencia.Rows.Add(drRow);
                }
                else
                {
                    ocorrencia.Rows.Remove(drRow);
                    drRow = ocorrencia.NewRow();
                    drRow[0] = DateTime.Now;
                    drRow[1] = texto_ocorrencia;
                    drRow[2] = "PENDENTE";
                    drRow[3] = usr.getNome().ToString();
                    ocorrencia.Rows.Add(drRow);
                }

            }
            else
            {
                drRow = ocorrencia.NewRow();
                drRow[0] = DateTime.Now;
                drRow[1] = texto_ocorrencia;
                drRow[2] = "PENDENTE";
                drRow[3] = usr.getNome().ToString();
                ocorrencia.Rows.Add(drRow);
            }
        }
        public void atualizarOcorrenciaDT(string ocorrenciaValor, string ocorrenciaStatus)
        {
            foreach (DataRow row in ocorrencia.Rows)
            {
                if (row[0].ToString() == dataAlteracaoOcorrencia)
                {
                    row[1] = ocorrenciaValor;
                    row[2] = ocorrenciaStatus;
                    break;
                }
            }
        }


        private void atualizarOcorrencia(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                if (!Fornecedor.Equals("") && ocorrencia != null)
                {
                    Conexao.executarSql("delete from Fornecedor_Ocorrencia where fornecedor='" + Fornecedor + "'");
                    for (int i = 0; i < ocorrencia.Rows.Count; i++)
                    {

                        DataRow drRow = ocorrencia.Rows[i];
                        if (!drRow[0].ToString().Equals("------"))
                        {
                            String sql = "insert into Fornecedor_Ocorrencia (Fornecedor, Data, Ocorrencia, Status,Usuario " +
                                     " )values ("  +
                                "'" + Fornecedor + "'" +
                                ", '" + DateTime.Parse(drRow[0].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "'" +
                                ", '" + drRow[1] + "'" +
                                ", '" + drRow[2] + "'" +
                                ", '" + usr.getNome().ToString() + "')";


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


        public void carregarDados(SqlDataReader rs)
        {
            try
            {


                if (rs.Read())
                {
                    Fornecedor = rs["Fornecedor"].ToString();
                    Filial = rs["Filial"].ToString();
                    Razao_social = rs["Razao_social"].ToString();
                    Nome_Fantasia = rs["Nome_Fantasia"].ToString();
                    CNPJ = rs["CNPJ"].ToString();
                    IE = rs["IE"].ToString();
                    Cidade = rs["Cidade"].ToString();
                    UF = rs["UF"].ToString();
                    CEP = rs["CEP"].ToString();
                    Endereco = rs["Endereco"].ToString();
                    Bairro = rs["Bairro"].ToString();
                    Desc_Coml = (Decimal)(rs["Desc_Coml"].ToString().Equals("") ? new Decimal() : rs["Desc_Coml"]);
                    Desc_Finan = (Decimal)(rs["Desc_Finan"].ToString().Equals("") ? new Decimal() : rs["Desc_Finan"]);
                    Adc_Finan = (Decimal)(rs["Adc_Finan"].ToString().Equals("") ? new Decimal() : rs["Adc_Finan"]);
                    Adc_Mkt = (Decimal)(rs["Adc_Mkt"].ToString().Equals("") ? new Decimal() : rs["Adc_Mkt"]);
                    Adc_Perda = (Decimal)(rs["Adc_Perda"].ToString().Equals("") ? new Decimal() : rs["Adc_Perda"]);
                    Adc_Frete = (Decimal)(rs["Adc_Frete"].ToString().Equals("") ? new Decimal() : rs["Adc_Frete"]);
                    Ultima_Contagem_do_Estoque = (rs["Ultima_Contagem_do_Estoque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Ultima_Contagem_do_Estoque"].ToString()));
                    Bonificacao = (Decimal)(rs["Bonificacao"].ToString().Equals("") ? new Decimal() : rs["Bonificacao"]);
                    Prazo = (Decimal)(rs["Prazo"].ToString().Equals("") ? new Decimal() : rs["Prazo"]);
                    condicao_pagamento = rs["condicao_pagamento"].ToString();
                    Desc_exp = (Decimal)(rs["Desc_exp"].ToString().Equals("") ? new Decimal() : rs["Desc_exp"]);
                    obs = rs["obs"].ToString();
                    pessoa_fisica = (rs["pessoa_fisica"].ToString().Equals("1") ? true : false);
                    senha = rs["senha"].ToString();
                    Codigo_fornecedor = rs["Codigo_fornecedor"].ToString();
                    Endereco_nro = rs["Endereco_nro"].ToString();
                    Formulario_proprio = (rs["Formulario_proprio"].ToString().Equals("1") ? true : false);



                    meiosComunicacao = Conexao.GetTable("SELECT Meio_comunicacao, id_comunicacao,contatao FROM Fornecedor_meio_comunicacao where fornecedor='" + Fornecedor.Trim() + "'", null, false);
                    codmun = rs["codmun"].ToString();
                    email = rs["email"].ToString();
                    site = rs["site"].ToString();
                    telefone1 = rs["telefone1"].ToString();
                    contato1 = rs["contato1"].ToString();

                    telefone2 = rs["telefone2"].ToString();
                    contato2 = rs["contato2"].ToString();

                    telefone3 = rs["telefone3"].ToString();
                    contato3 = rs["contato3"].ToString();
                    produtor_rural = (rs["produtor_rural"].ToString().Equals("1") ? true : false);

                    centro_custo = rs["centro_custo"].ToString();
                    int.TryParse(rs["indIEDest"].ToString(), out indIEDest);



                    carregarDepartamentos();
                    carregarMercadorias();

                    cargo1 = rs["cargo1"].ToString();
                    telefone1_2 = rs["telefone1_2"].ToString();
                    telefone1_3 = rs["telefone1_3"].ToString();
                    email1 = rs["email1"].ToString();

                    cargo2 = rs["cargo2"].ToString();
                    telefone2_2 = rs["telefone2_2"].ToString();
                    telefone2_3 = rs["telefone2_3"].ToString();
                    email2 = rs["email2"].ToString();

                    cargo3 = rs["cargo3"].ToString();
                    telefone3_2 = rs["telefone3_2"].ToString();
                    telefone3_3 = rs["telefone3_3"].ToString();
                    email3 = rs["email3"].ToString();

                    Tipo_fornecedor = rs["tipo_fornecedor"].ToString().Equals("1");
                    Usuario = rs["usuario"].ToString();
                    UsuarioAlteracao = rs["usuarioAlteracao"].ToString();
                    conta_contabil_credito = rs["conta_contabil_credito"].ToString();
                    conta_contabil_debito = rs["conta_contabil_debito"].ToString();
                    ipi_base = rs["ipi_base"].ToString().Equals("1");
                    despesas_base = rs["despesas_base"].ToString().Equals("1");

                    inativo = Funcoes.intTry(rs["Inativo"].ToString());

                    ocorrencia = Conexao.GetTable("SELECT Data, Ocorrencia,Status, Usuario FROM Fornecedor_Ocorrencia where fornecedor='" + Fornecedor.Trim() + "'", null, false);
                    ocorrenciasPendentesDT = Conexao.GetTable("SELECT Data, Ocorrencia, Status, Usuario FROM Fornecedor_Ocorrencia where fornecedor='" + Fornecedor.Trim() + "' AND Status = 'PENDENTE'", null, false);
                    CRT = Funcoes.intTry(rs["CRT"].ToString());

                    ocorrenciasPendentes = 0;
                    for (int i = 0; i < ocorrencia.Rows.Count; i++)
                    {
                        if (ocorrencia.Rows[i][2].Equals("PENDENTE"))
                        {
                            ocorrenciasPendentes++;
                        }
                    }

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

        private void carregarMercadorias()
        {

            String sql = "SELECT fm.*,DescricaoMercadoria =m.descricao FROM fornecedor_mercadoria as fm " +
                           " inner join mercadoria as m  on m.plu = fm.plu " +
                          " where fornecedor='" + Fornecedor.Trim() + "' order by convert(varchar,data,102) DESC, m.descricao";

            SqlDataReader rs = null;

            try
            {
                _Mercadorias.Clear();
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    FornecedorMercadoria m = new FornecedorMercadoria();
                    m.filial = rs["Filial"].ToString();
                    m.fornecedor = Fornecedor.Trim();
                    m.plu = rs["plu"].ToString();
                    m.plu_ant = m.plu;
                    m.Descricao_NF = rs["descricao"].ToString();
                    m.descricao_NF_ant = m.Descricao_NF;
                    m.Descricao = rs["DescricaoMercadoria"].ToString();
                    DateTime.TryParse(rs["data"].ToString(), out m.data);
                    m.preco_compra = Funcoes.decTry(rs["preco_compra"].ToString());
                    m.ean = rs["ean"].ToString();
                    m.codigo_referencia = rs["codigo_referencia"].ToString();
                    m.codigo_referencia_antigo = m.codigo_referencia;
                    m.preco_Custo = Funcoes.decTry(rs["preco_custo"].ToString());
                    m.embalagem = Funcoes.intTry(rs["embalagem"].ToString());
                    m.importado_nf = rs["importado_nf"].ToString().Equals("1");
                    _Mercadorias.Add(m);
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
        private void carregarDepartamentos()
        {
            String sql = "Select Wd.codigo_departamento, " +
                                         " Grupo= Wd.Descricao_grupo," +
                                         " SubGrupo = Wd.descricao_subgrupo," +
                                         " Departamento=Wd.descricao_departamento " +

                                     " from W_BR_CADASTRO_DEPARTAMENTO as Wd " +
                                     " inner join fornecedor_departamento  as fd  on fd.codigo_departamento = wd.codigo_departamento " +
                                     " Where fd.fornecedor ='" + Fornecedor.Trim() + "'";

            SqlDataReader rsDep = null;

            try
            {
                arrDepartamentos.Clear();
                rsDep = Conexao.consulta(sql, null, false);
                while (rsDep.Read())
                {
                    FornecedorDepartamento dep = new FornecedorDepartamento();
                    dep.codigo_departamento = rsDep["codigo_departamento"].ToString();
                    dep.Grupo = rsDep["Grupo"].ToString();
                    dep.subGrupo = rsDep["SubGrupo"].ToString();
                    dep.Departamento = rsDep["departamento"].ToString();
                    arrDepartamentos.Add(dep);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsDep != null)
                {
                    rsDep.Close();
                }
            }
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update fornecedor set " +
                              "Razao_social='" + Funcoes.RemoverAcentos(Razao_social) + "'," +
                              "Nome_Fantasia='" + Funcoes.RemoverAcentos(Nome_Fantasia) + "'," +
                              "CNPJ='" + CNPJ + "'," +
                              "IE='" + IE + "'," +
                              "Cidade='" + Funcoes.RemoverAcentos(Cidade) + "'," +
                              "UF='" + UF + "'," +
                              "CEP='" + CEP + "'," +
                              "Endereco='" + Endereco + "'," +
                              "Bairro='" + Funcoes.RemoverAcentos(Bairro) + "'," +
                              "Desc_Coml=" + Desc_Coml.ToString().Replace(",", ".") + "," +
                              "Desc_Finan=" + Desc_Finan.ToString().Replace(",", ".") + "," +
                              "Adc_Finan=" + Adc_Finan.ToString().Replace(",", ".") + "," +
                              "Adc_Mkt=" + Adc_Mkt.ToString().Replace(",", ".") + "," +
                              "Adc_Perda=" + Adc_Perda.ToString().Replace(",", ".") + "," +
                              "Adc_Frete=" + Adc_Frete.ToString().Replace(",", ".") + "," +
                              "Ultima_Contagem_do_Estoque='" + (Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd")) + "'," +
                              "Bonificacao=" + Bonificacao.ToString().Replace(",", ".") + "," +
                              "Prazo=" + Prazo.ToString().Replace(",", ".") + "," +
                              "condicao_pagamento='" + condicao_pagamento + "'," +
                              "Desc_exp=" + Desc_exp.ToString().Replace(",", ".") + "," +
                              "obs='" + obs + "'," +
                              "pessoa_fisica=" + (pessoa_fisica ? "1" : "0") + "," +

                              (senha.Trim().Length > 0 ? "senha='" + senha + "'," : "") +
                              "Codigo_fornecedor='" + Codigo_fornecedor + "'," +
                              "Endereco_nro='" + Endereco_nro + "'," +
                              "Formulario_proprio=" + (Formulario_proprio ? "1" : "0") + "," +
                              "codmun='" + codmun + "'," +
                              "email='" + email + "'," +
                              "site='" + site + "'," +
                              "telefone1='" + telefone1 + "'," +
                              "contato1='" + Funcoes.RemoverAcentos(contato1) + "'," +
                              "telefone2='" + telefone2 + "'," +
                              "contato2='" + Funcoes.RemoverAcentos(contato2) + "'," +
                              "telefone3='" + telefone3 + "'," +
                              "contato3='" + Funcoes.RemoverAcentos(contato3) + "'," +
                                "Produtor_rural=" + (produtor_rural ? "1" : "0") +
                              ",centro_custo='" + centro_custo + "'" +
                              ",indIEDest=" + indIEDest +
                              ",cargo1='" + cargo1 + "'" +
                              ",telefone1_2='" + telefone1_2 + "'" +
                              ",telefone1_3='" + telefone1_3 + "'" +
                              ",email1='" + email1 + "'" +
                              ",cargo2='" + cargo2 + "'" +
                              ",telefone2_2='" + telefone2_2 + "'" +
                              ",telefone2_3='" + telefone2_3 + "'" +
                              ",email2='" + email2 + "'" +
                              ",cargo3='" + cargo3 + "'" +
                              ",telefone3_2='" + telefone3_2 + "'" +
                              ",telefone3_3='" + telefone3_3 + "'" +
                              ",email3='" + email3 + "'" +
                              ",tipo_fornecedor=" + (Tipo_fornecedor ? "1" : "0") +
                              ",usuario='" + Usuario + "'" +
                              ",usuarioAlteracao ='" + UsuarioAlteracao + "'" +
                              ",conta_contabil_credito='" + conta_contabil_credito + "'" +
                              ",conta_contabil_debito='" + conta_contabil_debito + "'" +
                              ",despesas_base=" + (despesas_base ? "1" : "0") +
                              ",ipi_base=" + (ipi_base ? "1" : "0") +
                              ", inativo = " + inativo.ToString() + 
                              ", CRT = " + CRT.ToString() +
                    "  where  Fornecedor='" + Fornecedor + "'";

                Conexao.executarSql(sql, conn, tran);
                atualizarMeioComunicacao(conn, tran);
                atualizarOcorrencia(conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            if (!CNPJ.Equals(""))
            {
                if (Conexao.countSql("select cnpj from fornecedor where cnpj='" + CNPJ + "'", null) > 0)
                    throw new Exception("CNPJ JÁ ESTA CADASTRADO");
            }

            Usuario = usr.getUsuario();
            UsuarioAlteracao = usr.getUsuario();


            String sql = " insert into fornecedor(" +
                      "Filial," +
                      "Fornecedor," +
                      "Razao_social," +
                      "Nome_Fantasia," +
                      "CNPJ," +
                      "IE," +
                      "Cidade," +
                      "UF," +
                      "CEP," +
                      "Endereco," +
                      "Bairro," +
                      "Desc_Coml," +
                      "Desc_Finan," +
                      "Adc_Finan," +
                      "Adc_Mkt," +
                      "Adc_Perda," +
                      "Adc_Frete," +
                      "Ultima_Contagem_do_Estoque," +
                      "Bonificacao," +
                      "Prazo," +
                      "condicao_pagamento," +
                      "Desc_exp," +
                      "obs," +
                      "pessoa_fisica," +
                      "senha," +
                      "Codigo_fornecedor," +
                      "Endereco_nro," +
                      "Formulario_proprio," +
                      "codmun," +
                      "email," +
                      "site," +
                      "telefone1," +
                      "contato1," +
                      "telefone2," +
                      "contato2," +
                      "telefone3," +
                      "contato3," +
                      "produtor_rural," +
                      "centro_custo" +
                      ",indIEDest" +
                      ",cargo1" +
                      ",telefone1_2" +
                      ",telefone1_3" +
                      ",email1" +
                      ",cargo2" +
                      ",telefone2_2" +
                      ",telefone2_3" +
                      ",email2" +
                      ",cargo3" +
                      ",telefone3_2" +
                      ",telefone3_3" +
                      ",email3" +
                      ",Tipo_fornecedor" +
                      ",usuario" +
                      ",usuarioAlteracao" +
                      ",conta_contabil_credito" +
                      ",conta_contabil_debito" +
                      ",despesas_base" +
                      ",ipi_base" +
                      ", inativo" + 
                      ", CRT " +
                 " )values (" +
                      "'" + usr.getFilial() + "'," +
                      "'" + Funcoes.RemoverAcentos(Fornecedor.Trim()) + "'," +
                      "'" + Funcoes.RemoverAcentos(Razao_social) + "'," +
                      "'" + Funcoes.RemoverAcentos(Nome_Fantasia) + "'," +
                      "'" + CNPJ + "'," +
                      "'" + IE + "'," +
                      "'" + Funcoes.RemoverAcentos(Cidade) + "'," +
                      "'" + UF + "'," +
                      "'" + CEP + "'," +
                      "'" + Funcoes.RemoverAcentos(Endereco) + "'," +
                      "'" + Funcoes.RemoverAcentos(Bairro) + "'," +
                      Desc_Coml.ToString().Replace(",", ".") + "," +
                      Desc_Finan.ToString().Replace(",", ".") + "," +
                      Adc_Finan.ToString().Replace(",", ".") + "," +
                      Adc_Mkt.ToString().Replace(",", ".") + "," +
                      Adc_Perda.ToString().Replace(",", ".") + "," +
                      Adc_Frete.ToString().Replace(",", ".") + "," +
                      "'" + (Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd")) + "'," +
                      Bonificacao.ToString().Replace(",", ".") + "," +
                      Prazo.ToString().Replace(",", ".") + "," +
                      "'" + condicao_pagamento + "'," +
                      Desc_exp.ToString().Replace(",", ".") + "," + //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                      "'" + obs + "'," +
                      (pessoa_fisica ? 1 : 0) + "," +
                      "'" + senha + "'," +
                      "'" + Codigo_fornecedor + "'," +
                      "'" + Endereco_nro + "'," +
                      (Formulario_proprio ? 1 : 0) + "," +
                      "'" + codmun + "'," +
                      "'" + email + "'," +
                      "'" + site + "'," +
                      "'" + telefone1 + "'," +
                      "'" + Funcoes.RemoverAcentos(contato1) + "'," +
                      "'" + telefone2 + "'," +
                      "'" + Funcoes.RemoverAcentos(contato2) + "'," +
                      "'" + telefone3 + "'," +
                      "'" + Funcoes.RemoverAcentos(contato3) + "'," +
                      (produtor_rural ? 1 : 0) +
                      ",'" + centro_custo + "'" +
                      "," + indIEDest +
                      ",'" + cargo1 + "'" +
                      ",'" + telefone1_2 + "'" +
                      ",'" + telefone1_3 + "'" +
                      ",'" + email1 + "'" +
                      ",'" + cargo2 + "'" +
                      ",'" + telefone2_2 + "'" +
                      ",'" + telefone2_3 + "'" +
                      ",'" + email2 + "'" +
                      ",'" + cargo3 + "'" +
                      ",'" + telefone3_2 + "'" +
                      ",'" + telefone3_3 + "'" +
                      ",'" + email3 + "'" +
                      "," + (Tipo_fornecedor ? "1" : "0") +
                      ",'" + Usuario + "'" +
                      ",'" + UsuarioAlteracao + "'" +
                      ",'" + conta_contabil_credito + "'" +
                      ",'" + conta_contabil_debito + "'" +
                      "," + (despesas_base ? "1" : "0") +
                      "," + (ipi_base ? "1" : "0") +
                      "," + inativo.ToString() +
                      "," + CRT.ToString()+

            " )";
            Conexao.executarSql(sql, conn, tran);
            atualizarMeioComunicacao(conn, tran);
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


                atualizarDepartamentos(conn, tran);

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
        public void atualizarDepartamentos(SqlConnection conn, SqlTransaction tran)
        {
            Conexao.executarSql("Delete from fornecedor_departamento where fornecedor ='" + Fornecedor.Trim() + "'", conn, tran);

            foreach (FornecedorDepartamento item in arrDepartamentos)
            {
                Conexao.executarSql("insert into fornecedor_departamento values('" + Fornecedor.Trim() + "','" + item.codigo_departamento.Trim() + "')", conn, tran);
            }
        }
        public void excluir()
        {
            int notas = Conexao.countSql("select codigo from nf where cliente_fornecedor='" + Fornecedor + "'", null);
            if (notas > 0)
            {
                throw new Exception("Fornecedor contem informações fiscais registradas");
            }
            else
            {

                Conexao.executarSql("delete from fornecedor where fornecedor='" + Fornecedor + "'");
                Conexao.executarSql("Delete from fornecedor_departamento where fornecedor ='" + Fornecedor.Trim() + "'");

            }
        }

        public void excluiProduto(FornecedorMercadoria prod)
        {


            String sql = "Delete " +
                         "from fornecedor_mercadoria " +
                         "where plu ='" + prod.plu_ant + "' " +
                         "  and codigo_referencia='" + prod.codigo_referencia_antigo + "'" +
                         "  and descricao='" + prod.descricao_NF_ant + "'" +
                         "  and fornecedor ='" + Fornecedor + "'";
            Conexao.executarSql(sql);
            carregarMercadorias();

        }

        public void AddProduto(FornecedorMercadoria prod)
        {
            String strSqlVerifica = "Select count(*) from mercadoria where plu ='" + prod.plu + "'";

            int exist = Funcoes.intTry(Conexao.retornaUmValor(strSqlVerifica, null));
            if (exist == 0)
                throw new Exception("PLU não Foi Cadastrado");

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {



                String sql = "Delete " +
                         "from fornecedor_mercadoria " +
                         "where plu ='" + prod.plu_ant + "' " +
                         "  and codigo_referencia ='" + prod.codigo_referencia_antigo + "'" +
                         "  and descricao='" + prod.descricao_NF_ant + "'" +
                         "  and fornecedor ='" + Fornecedor + "'";
                Conexao.executarSql(sql, conn, tran);


                sql = "Insert into fornecedor_mercadoria values( " +
                                " '" + Filial + "'" +
                                ",'" + Fornecedor + "'" +
                                ",'" + prod.plu + "'" +
                                ",'" + prod.Descricao_NF + "'" +
                                ",'" + prod.data.ToString("yyyyMMdd") + "'" +
                                "," + Funcoes.decimalPonto(prod.preco_compra.ToString()) +
                                ",'" + prod.ean + "'" +
                                ",'" + prod.codigo_referencia + "'" +
                                "," + Funcoes.decimalPonto(prod.preco_Custo.ToString()) +
                                "," + prod.embalagem.ToString() +
                                ",null" +
                                ",0" +
                            ")";
                Conexao.executarSql(sql, conn, tran);
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
            carregarMercadorias();


        }

    }
}