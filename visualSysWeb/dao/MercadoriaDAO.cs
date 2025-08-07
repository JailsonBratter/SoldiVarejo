using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
namespace visualSysWeb.dao
{
    public class MercadoriaDAO
    {
        public User usr = null;
        public String ean { get; set; }
        public String Filial { get; set; }
        public String PLU { get; set; }
        public String Tipo { get; set; }
        public String Peso_Variavel { get; set; }
        public String Codigo_Portaria { get; set; }
        public Decimal Codigo_Tributacao { get; set; }
        public String descricaoTributacao{ get; set; }
        public Decimal Codigo_Tributacao_ent { get; set; }
        public String descricaoTributacaoEnt { get; set; }
        public String Codigo_departamento { get; set; }
        public String Codigo_familia { get; set; }
        public String Descricao_departamento { get; set; }

        public String codigo_subGrupo{ get; set; }
        public String descricao_subGrupo{ get; set; }
        
        public String codigo_Grupo{ get; set; }
        public String descricao_Grupo{ get; set; }


        public String Descricao { get; set; }
        public String Descricao_resumida { get; set; }
        public String Descricao_familia { get; set; }
        public Decimal Tecla { get; set; }
        public Decimal Margem { get; set; }
        public Decimal Estoque_minimo { get; set; }
        public Decimal Etiqueta { get; set; }
        public Decimal Validade { get; set; }
        public Decimal Preco { get; set; }
        public Decimal Preco_promocao { get; set; }
        public DateTime data_inicio { get; set; }
        public String data_inicioBr() {
            return dataBr(data_inicio);
        } 
        public DateTime data_fim { get; set; }
        public String data_fimBr()
        {
            return dataBr(data_fim);
        } 
        public bool Promocao_automatica { get; set; }
        public bool Promocao { get; set; }
        public Decimal Preco_Custo { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public String Data_CadastroBr()
        {
            return dataBr(Data_Cadastro);
        } 
        public DateTime Data_Alteracao { get; set; }
        public String Data_AlteracaoBr()
        {
            return dataBr(Data_Alteracao);
        }
        public Decimal IPI { get; set; }
        public bool Incide_Pis { get; set; }
        public Decimal Embalagem { get; set; }
        public String ultimo_fornecedor { get; set; }
        public Decimal Fator_conversao { get; set; }
        public Decimal Tecla_balanca { get; set; }
        public String Localizacao { get; set; }
        public bool Inativo { get; set; }
        public bool Imprime_etiqueta { get; set; }
        public bool Estado_Mercadoria { get; set; }
        public Decimal saldo_atual { get; set; }
        public Decimal Preco_Custo_1 { get; set; }
        public Decimal Preco_Custo_2 { get; set; }
        public String Ref_fornecedor { get; set; }
        public DateTime data_inventario { get; set; }
        public String data_inventarioBr()
        {
            return dataBr(data_inventario);
        }
        public Decimal saldo_inicial { get; set; }
        public Decimal peso { get; set; }
        public String receita { get; set; }
        public Decimal qtde_receita { get; set; }
        public Decimal preco_compra { get; set; }
        public Decimal frete { get; set; }
        public Decimal seguro { get; set; }
        public Decimal outras_despesas { get; set; }
        public Decimal valor_ipi { get; set; }
        public String codigo_centro_custo { get; set; }
        public String venda_fracionaria { get; set; }
        public Decimal pis { get; set; }
        public Decimal cofins { get; set; }
        public String cf { get; set; }
        public String und { get; set; }
        public bool gera_inativo { get; set; }
        public String inventario { get; set; }
        public String tipo_produto_origem { get; set; }
        public String tipo_produto_destino { get; set; }
        public bool curva_a { get; set; }
        public bool curva_b { get; set; }
        public bool curva_c { get; set; }
        public bool estoque_aviso { get; set; }
        public String artigo { get; set; }
        public int estoque_margem { get; set; }
        public int estoque_meses { get; set; }
        public int cobertura { get; set; }
        public DateTime sazonal1 { get; set; }
        public DataTable precosPromocionais { get; set; }
        public DataTable itens { get; set; }


        public String sazonal1Br()
        {
            return dataBr(sazonal1);
        }
        public DateTime sazonal2 { get; set; }
        public String sazonal2Br()
        {
            return dataBr(sazonal2);
        }
        public DateTime sazonal3 { get; set; }
        public String sazonal3Br()
        {
            return dataBr(sazonal3);
        }
        public DateTime sazonal4 { get; set; }
        public String sazonal4Br()
        {
            return dataBr(sazonal4);
        }
        public DateTime sazonal5 { get; set; }
        public String sazonal5Br()
        {
            return dataBr(sazonal4);
        }
        public DateTime sazonal6 { get; set; }
        public String sazonal6Br()
        {
            return dataBr(sazonal6);
        }
        public Decimal peso_liquido { get; set; }
        public Decimal peso_bruto { get; set; }
        public Decimal margem_iva { get; set; }
        public String Marca { get; set; }
        public String descricao_pisc { get; set; }
        public String mercadoria { get; set; }


        public MercadoriaDAO(String plu,User usr)
        {
            this.usr = usr;
            String sql = "Select c.ean,b.codigo_subgrupo, b.descricao_subgrupo,b.codigo_grupo ,b.descricao_grupo, a.* "+
                         " from  Mercadoria a inner join W_BR_CADASTRO_DEPARTAMENTO b on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial)"+
		                 " inner join ean c on (a.plu = c.plu and a.filial= c.filial	)"+
                         " where a.plu ='" + plu + "'";
            SqlDataReader rs = Conexao.consulta(sql,usr);
            carregarDados(rs);
        }

        private String dataBr(DateTime dt) {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else {
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
                    PLU = rs["PLU"].ToString();
                    Tipo = rs["Tipo"].ToString();
                    Peso_Variavel = rs["Peso_Variavel"].ToString();
                    Codigo_Portaria = rs["Codigo_Portaria"].ToString();
                    Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                    Codigo_Tributacao_ent = (Decimal)(rs["Codigo_Tributacao_ent"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao_ent"]);
                    Codigo_departamento = rs["Codigo_departamento"].ToString();
                    Codigo_familia = rs["Codigo_familia"].ToString();
                    Descricao_departamento = rs["Descricao_departamento"].ToString();
                    Descricao = rs["Descricao"].ToString();
                    Descricao_resumida = rs["Descricao_resumida"].ToString();
                    Descricao_familia = rs["Descricao_familia"].ToString();
                    Tecla = (Decimal)(rs["Tecla"].ToString().Equals("") ? new Decimal() : rs["Tecla"]);
                    Margem = (Decimal)(rs["Margem"].ToString().Equals("") ? new Decimal() : rs["Margem"]);
                    Estoque_minimo = (Decimal)(rs["Estoque_minimo"].ToString().Equals("") ? new Decimal() : rs["Estoque_minimo"]);
                    Etiqueta = (Decimal)(rs["Etiqueta"].ToString().Equals("") ? new Decimal() : rs["Etiqueta"]);
                    Validade = (Decimal)(rs["Validade"].ToString().Equals("") ? new Decimal() : rs["Validade"]);
                    Preco = (Decimal)(rs["Preco"].ToString().Equals("") ? new Decimal() : rs["Preco"]);
                    Preco_promocao = (Decimal)(rs["Preco_promocao"].ToString().Equals("") ? new Decimal() : rs["Preco_promocao"]);

                    data_inicio = (rs["data_inicio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_inicio"].ToString()));
                    data_fim = (rs["data_fim"].ToString().Equals("") ? data_fim : DateTime.Parse(rs["data_fim"].ToString()));
                    Promocao_automatica = (rs["Promocao_automatica"].ToString().Equals("1") ? true : false);
                    Promocao = (rs["Promocao"].ToString().Equals("1") ? true : false);
                    Preco_Custo = (Decimal)(rs["Preco_Custo"].ToString().Equals("") ? 0 : rs["Preco_Custo"]);
                    Data_Cadastro = (rs["Data_Cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Cadastro"].ToString()));
                    Data_Alteracao = (rs["Data_Alteracao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Alteracao"].ToString()));
                    IPI = (Decimal)(rs["IPI"].ToString().Equals("") ? new Decimal() : rs["IPI"]);
                    Incide_Pis = (rs["Incide_Pis"].ToString().Equals("1") ? true : false);
                    Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                    ultimo_fornecedor = rs["ultimo_fornecedor"].ToString();
                    Fator_conversao = (Decimal)(rs["Fator_conversao"].ToString().Equals("") ? new Decimal() : rs["Fator_conversao"]);
                    Tecla_balanca = (Decimal)(rs["Tecla_balanca"].ToString().Equals("") ? new Decimal() : rs["Tecla_balanca"]);
                    Localizacao = rs["Localizacao"].ToString();
                    Inativo = (rs["Inativo"].ToString().Equals("1") ? true : false);
                    Imprime_etiqueta = (rs["Imprime_etiqueta"].ToString().Equals("1") ? true : false);
                    Estado_Mercadoria = (rs["Estado_Mercadoria"].ToString().Equals("1") ? true : false);
                    saldo_atual = (Decimal)(rs["saldo_atual"].ToString().Equals("") ? new Decimal() : rs["saldo_atual"]);

                    Preco_Custo_1 = (Decimal)(rs["Preco_Custo_1"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_1"]);
                    Preco_Custo_2 = (Decimal)(rs["Preco_Custo_2"].ToString().Equals("") ? new Decimal() : rs["Preco_Custo_2"]);
                    Ref_fornecedor = rs["Ref_fornecedor"].ToString();
                    data_inventario = (rs["data_inventario"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_inventario"].ToString()));
                    saldo_inicial = (Decimal)(rs["saldo_inicial"].ToString().Equals("") ? new Decimal() : rs["saldo_inicial"]);
                    peso = (Decimal)(rs["peso"].ToString().Equals("") ? new Decimal() : rs["peso"]);
                    receita = rs["receita"].ToString();
                    qtde_receita = (Decimal)(rs["qtde_receita"].ToString().Equals("") ? new Decimal() : rs["qtde_receita"]);
                    preco_compra = (Decimal)(rs["preco_compra"].ToString().Equals("") ? new Decimal() : rs["preco_compra"]);
                    frete = (Decimal)(rs["frete"].ToString().Equals("") ? new Decimal() : rs["frete"]);
                    seguro = (Decimal)(rs["seguro"].ToString().Equals("") ? new Decimal() : rs["seguro"]);
                    outras_despesas = (Decimal)(rs["outras_despesas"].ToString().Equals("") ? new Decimal() : rs["outras_despesas"]);
                    valor_ipi = (Decimal)(rs["valor_ipi"].ToString().Equals("") ? new Decimal() : rs["valor_ipi"]);
                    codigo_centro_custo = rs["codigo_centro_custo"].ToString();
                    venda_fracionaria = rs["venda_fracionaria"].ToString();
                    pis = (Decimal)(rs["pis"].ToString().Equals("") ? new Decimal() : rs["pis"]);
                    cofins = (Decimal)(rs["cofins"].ToString().Equals("") ? new Decimal() : rs["cofins"]);
                    cf = rs["cf"].ToString();
                    und = rs["und"].ToString();
                    gera_inativo = (rs["gera_inativo"].ToString().Equals("1") ? true : false);
                    inventario = rs["inventario"].ToString();
                    tipo_produto_origem = rs["tipo_produto_origem"].ToString();
                    tipo_produto_destino = rs["tipo_produto_destino"].ToString();
                    curva_a = (rs["curva_a"].ToString().Equals("1") ? true : false);
                    curva_b = (rs["curva_b"].ToString().Equals("1") ? true : false);
                    curva_c = (rs["curva_c"].ToString().Equals("1") ? true : false);
                    estoque_aviso = (rs["estoque_aviso"].ToString().Equals("1") ? true : false);
                    artigo = rs["artigo"].ToString();
                    estoque_margem = (rs["estoque_margem"].ToString().Equals("") ? 0 : int.Parse(rs["estoque_margem"].ToString()));
                    estoque_meses = (rs["estoque_meses"].ToString().Equals("") ? 0 : int.Parse(rs["estoque_meses"].ToString()));
                    cobertura = (rs["cobertura"].ToString().Equals("") ? 0 : int.Parse(rs["cobertura"].ToString()));
                    sazonal1 = (rs["sazonal1"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal1"].ToString()));
                    sazonal2 = (rs["sazonal2"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal2"].ToString()));
                    sazonal3 = (rs["sazonal3"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal3"].ToString()));
                    sazonal4 = (rs["sazonal4"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal4"].ToString()));
                    sazonal5 = (rs["sazonal5"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal5"].ToString()));
                    sazonal6 = (rs["sazonal6"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["sazonal6"].ToString()));
                    peso_liquido = (Decimal)(rs["peso_liquido"].ToString().Equals("") ? new Decimal() : rs["peso_liquido"]);
                    peso_bruto = (Decimal)(rs["peso_bruto"].ToString().Equals("") ? new Decimal() : rs["peso_bruto"]);
                    margem_iva = (Decimal)(rs["margem_iva"].ToString().Equals("") ? new Decimal() : rs["margem_iva"]);
                    Marca = rs["Marca"].ToString();
                    descricao_pisc = rs["descricao_pisc"].ToString();
                    mercadoria = rs["mercadoria"].ToString();

                    ean = rs["ean"].ToString();
                    codigo_subGrupo = rs["codigo_subGrupo"].ToString();
                    descricao_subGrupo = rs["descricao_subGrupo"].ToString();
                    codigo_Grupo = rs["codigo_grupo"].ToString();
                    descricao_Grupo = rs["descricao_grupo"].ToString();


                    String sqlpreco = "SELECT Codigo_tabela Tabela, Desconto , Preco, Desconto_promocao Desconto_promocao, Preco_promocao "+
                                       " FROM dbo.Preco_Mercadoria Preco_mercadoria "+
                                       " WHERE  Preco_mercadoria.filial = '"+Filial+"' AND  Preco_mercadoria.plu = '"+PLU+"'";
                    precosPromocionais = Conexao.GetTable(sqlpreco,usr);

                    String sqlitens = "SELECT Item.Plu_item, Mercadoria.Descricao , Mercadoria.Preco_Custo , Item.fator_conversao" +
                                        " FROM  ( dbo.Item Item  LEFT OUTER JOIN dbo.Mercadoria Mercadoria  ON  Item.Filial = Mercadoria.Filial AND  Item.Plu_item = Mercadoria.PLU) " +
                                        " WHERE  Item.Filial = '" + Filial + "' AND  Item.PLU = '" + PLU + "'";
                    itens = Conexao.GetTable(sqlitens,usr);

                    descricaoTributacao = Conexao.retornaUmValor("select Descricao_tributacao from tributacao where codigo_tributacao="+Codigo_Tributacao,usr);
                    descricaoTributacaoEnt = Conexao.retornaUmValor("select Descricao_tributacao from tributacao where codigo_tributacao=" + Codigo_Tributacao_ent,usr);

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }
        }
    }
}

/*
        private void update()
        {
            try
            {
                String sql = "update set Mercadoria(" +
                   "Filial='" + Filial + "'," +
                   "PLU='" + PLU + "'," +
                   "Tipo='" + Tipo + "'," +
                   "Peso_Variavel='" + Peso_Variavel + "'," +
                   "Codigo_Portaria='" + Codigo_Portaria + "'," +
                   "Codigo_Tributacao=" + string.Format("{0:0,0.00}", Codigo_Tributacao) + "," +
                   "Codigo_Tributacao_ent=" + string.Format("{0:0,0.00}", Codigo_Tributacao_ent) + "," +
                   "Codigo_departamento='" + Codigo_departamento + "'," +
                   "Codigo_familia='" + Codigo_familia + "'," +
                   "Descricao_departamento='" + Descricao_departamento + "'," +
                   "Descricao='" + Descricao + "'," +
                   "Descricao_resumida='" + Descricao_resumida + "'," +
                   "Descricao_familia='" + Descricao_familia + "'," +
                   "Tecla=" + string.Format("{0:0,0.00}", Tecla) + "," +
                   "Margem=" + string.Format("{0:0,0.00}", Margem) + "," +
                   "Estoque_minimo=" + string.Format("{0:0,0.00}", Estoque_minimo) + "," +
                   "Etiqueta=" + string.Format("{0:0,0.00}", Etiqueta) + "," +
                   "Validade=" + string.Format("{0:0,0.00}", Validade) + "," +
                   "Preco=" + string.Format("{0:0,0.00}", Preco) + "," +
                   "Preco_promocao=" + string.Format("{0:0,0.00}", Preco_promocao) + "," +
                   "data_inicio='" + data_inicio.ToString("yyyy-MM-dd") + "'," +
                   "data_fim='" + data_fim.ToString("yyyy-MM-dd") + "'," +
                   "Promocao_automatica=" + (Promocao_automatica ? 1 : 0) + "," +
                   "Promocao=" + (Promocao ? 1 : 0) + "," +
                   "Preco_Custo=" + string.Format("{0:0,0.00}", Preco_Custo) + "," +
                   "Data_Cadastro='" + Data_Cadastro.ToString("yyyy-MM-dd") + "'," +
                   "Data_Alteracao='" + Data_Alteracao.ToString("yyyy-MM-dd") + "'," +
                   "IPI=" + string.Format("{0:0,0.00}", IPI) + "," +
                   "Incide_Pis=" + (Incide_Pis ? 1 : 0) + "," +
                   "Embalagem=" + string.Format("{0:0,0.00}", Embalagem) + "," +
                   "ultimo_fornecedor='" + ultimo_fornecedor + "'," +
                   "Fator_conversao=" + string.Format("{0:0,0.00}", Fator_conversao) + "," +
                   "Tecla_balanca=" + string.Format("{0:0,0.00}", Tecla_balanca) + "," +
                   "Localizacao='" + Localizacao + "'," +
                   "Inativo=" + (Inativo ? 1 : 0) + "," +
                   "Imprime_etiqueta=" + (Imprime_etiqueta ? 1 : 0) + "," +
                   "Estado_Mercadoria=" + (Estado_Mercadoria ? 1 : 0) + "," +
                   "saldo_atual=" + string.Format("{0:0,0.00}", saldo_atual) + "," +
                   "Preco_Custo_1=" + string.Format("{0:0,0.00}", Preco_Custo_1) + "," +
                   "Preco_Custo_2=" + string.Format("{0:0,0.00}", Preco_Custo_2) + "," +
                   "Ref_fornecedor='" + Ref_fornecedor + "'," +
                   "data_inventario='" + data_inventario.ToString("yyyy-MM-dd") + "'," +
                   "saldo_inicial=" + string.Format("{0:0,0.00}", saldo_inicial) + "," +
                   "peso=" + string.Format("{0:0,0.00}", peso) + "," +
                   "receita='" + receita + "'," +
                   "qtde_receita=" + string.Format("{0:0,0.00}", qtde_receita) + "," +
                   "preco_compra=" + string.Format("{0:0,0.00}", preco_compra) + "," +
                   "frete=" + string.Format("{0:0,0.00}", frete) + "," +
                   "seguro=" + string.Format("{0:0,0.00}", seguro) + "," +
                   "outras_despesas=" + string.Format("{0:0,0.00}", outras_despesas) + "," +
                   "valor_ipi=" + string.Format("{0:0,0.00}", valor_ipi) + "," +
                   "codigo_centro_custo='" + codigo_centro_custo + "'," +
                   "venda_fracionaria='" + venda_fracionaria + "'," +
                   "pis=" + string.Format("{0:0,0.00}", pis) + "," +
                   "cofins=" + string.Format("{0:0,0.00}", cofins) + "," +
                   "cf='" + cf + "'," +
                   "und='" + und + "'," +
                   "gera_inativo=" + (gera_inativo ? 1 : 0) + "," +
                   "inventario='" + inventario + "'," +
                   "tipo_produto_origem='" + tipo_produto_origem + "'," +
                   "tipo_produto_destino='" + tipo_produto_destino + "'," +
                   "curva_a=" + (curva_a ? 1 : 0) + "," +
                   "curva_b=" + (curva_b ? 1 : 0) + "," +
                   "curva_c=" + (curva_c ? 1 : 0) + "," +
                   "estoque_aviso=" + (estoque_aviso ? 1 : 0) + "," +
                   "artigo='" + artigo + "'," +
                   "estoque_margem=" + estoque_margem + "," +
                   "estoque_meses=" + estoque_meses + "," +
                   "cobertura=" + cobertura + "," +
                   "sazonal1='" + sazonal1.ToString("yyyy-MM-dd") + "'," +
                   "sazonal2='" + sazonal2.ToString("yyyy-MM-dd") + "'," +
                   "sazonal3='" + sazonal3.ToString("yyyy-MM-dd") + "'," +
                   "sazonal4='" + sazonal4.ToString("yyyy-MM-dd") + "'," +
                   "sazonal5='" + sazonal5.ToString("yyyy-MM-dd") + "'," +
                   "sazonal6='" + sazonal6.ToString("yyyy-MM-dd") + "'," +
                   "peso_liquido=" + string.Format("{0:0,0.00}", peso_liquido) + "," +
                   "peso_bruto=" + string.Format("{0:0,0.00}", peso_bruto) + "," +
                   "margem_iva=" + string.Format("{0:0,0.00}", margem_iva) + "," +
                   "Marca='" + Marca + "'," +
                   "descricao_pisc='" + descricao_pisc + "'," +
                   "mercadoria='" + mercadoria + "')" +

                   "where plu='" + plu + "'";

                Conexao.executar(sql);
            }
            catch (Exception)
            { }
        }
    }
}

      /*
  private void insert(){
      "Filial,"+
      "PLU,"+
      "Tipo,"+
      "Peso_Variavel,"+
      "Codigo_Portaria,"+
      "Codigo_Tributacao,"+
      "Codigo_Tributacao_ent,"+
      "Codigo_departamento,"+
      "Codigo_familia,"+
      "Descricao_departamento,"+
      "Descricao,"+
      "Descricao_resumida,"+
      "Descricao_familia,"+
      "Tecla,"+
      "Margem,"+
      "Estoque_minimo,"+
      "Etiqueta,"+
      "Validade,"+
      "Preco,"+
      "Preco_promocao,"+
      "data_inicio,"+
      "data_fim,"+
      "Promocao_automatica,"+
      "Promocao,"+
      "Preco_Custo,"+
      "Data_Cadastro,"+
      "Data_Alteracao,"+
      "IPI,"+
      "Incide_Pis,"+
      "Embalagem,"+
      "ultimo_fornecedor,"+
      "Fator_conversao,"+
      "Tecla_balanca,"+
      "Localizacao,"+
      "Inativo,"+
      "Imprime_etiqueta,"+
      "Estado_Mercadoria,"+
      "saldo_atual,"+
      "Preco_Custo_1,"+
      "Preco_Custo_2,"+
      "Ref_fornecedor,"+
      "data_inventario,"+
      "saldo_inicial,"+
      "peso,"+
      "receita,"+
      "qtde_receita,"+
      "preco_compra,"+
      "frete,"+
      "seguro,"+
      "outras_despesas,"+
      "valor_ipi,"+
      "codigo_centro_custo,"+
      "venda_fracionaria,"+
      "pis,"+
      "cofins,"+
      "cf,"+
      "und,"+
      "gera_inativo,"+
      "inventario,"+
      "tipo_produto_origem,"+
      "tipo_produto_destino,"+
      "curva_a,"+
      "curva_b,"+
      "curva_c,"+
      "estoque_aviso,"+
      "artigo,"+
      "estoque_margem,"+
      "estoque_meses,"+
      "cobertura,"+
      "sazonal1,"+
      "sazonal2,"+
      "sazonal3,"+
      "sazonal4,"+
      "sazonal5,"+
      "sazonal6,"+
      "peso_liquido,"+
      "peso_bruto,"+
      "margem_iva,"+
      "Marca,"+
      "descricao_pisc,"+
      "mercadoria,"+
        "'"+Filial+"',"+
        "'"+PLU+"',"+
        "'"+Tipo+"',"+
        "'"+Peso_Variavel+"',"+
        "'"+Codigo_Portaria+"',"+
        string.Format("{0:0,0.00}",Codigo_Tributacao)+","+
        string.Format("{0:0,0.00}",Codigo_Tributacao_ent)+","+
        "'"+Codigo_departamento+"',"+
        "'"+Codigo_familia+"',"+
        "'"+Descricao_departamento+"',"+
        "'"+Descricao+"',"+
        "'"+Descricao_resumida+"',"+
        "'"+Descricao_familia+"',"+
        string.Format("{0:0,0.00}",Tecla)+","+
        string.Format("{0:0,0.00}",Margem)+","+
        string.Format("{0:0,0.00}",Estoque_minimo)+","+
        string.Format("{0:0,0.00}",Etiqueta)+","+
        string.Format("{0:0,0.00}",Validade)+","+
        string.Format("{0:0,0.00}",Preco)+","+
        string.Format("{0:0,0.00}",Preco_promocao)+","+
        "'"+data_inicio.ToString("yyyy-MM-dd")+"',"+
        "'"+data_fim.ToString("yyyy-MM-dd")+"',"+
        (Promocao_automatica?1:0)+","+
        (Promocao?1:0)+","+
        string.Format("{0:0,0.00}",Preco_Custo)+","+
        "'"+Data_Cadastro.ToString("yyyy-MM-dd")+"',"+
        "'"+Data_Alteracao.ToString("yyyy-MM-dd")+"',"+
        string.Format("{0:0,0.00}",IPI)+","+
        (Incide_Pis?"1":"0")+","+
        string.Format("{0:0,0.00}",Embalagem)+","+
        "'"+ultimo_fornecedor+"',"+
        string.Format("{0:0,0.00}",Fator_conversao)+","+
        string.Format("{0:0,0.00}",Tecla_balanca)+","+
        "'"+Localizacao+"',"+
        (Inativo?1:0)+","+
        (Imprime_etiqueta?1:0)+","+
        (Estado_Mercadoria?1:0)+","+
        string.Format("{0:0,0.00}",saldo_atual)+","+
        string.Format("{0:0,0.00}",Preco_Custo_1)+","+
        string.Format("{0:0,0.00}",Preco_Custo_2)+","+
        "'"+Ref_fornecedor+"',"+
        "'"+data_inventario.ToString("yyyy-MM-dd")+"',"+
        string.Format("{0:0,0.00}",saldo_inicial)+","+
        string.Format("{0:0,0.00}",peso)+","+
        "'"+receita+"',"+
        string.Format("{0:0,0.00}",qtde_receita)+","+
        string.Format("{0:0,0.00}",preco_compra)+","+
        string.Format("{0:0,0.00}",frete)+","+
        string.Format("{0:0,0.00}",seguro)+","+
        string.Format("{0:0,0.00}",outras_despesas)+","+
        string.Format("{0:0,0.00}",valor_ipi)+","+
        "'"+codigo_centro_custo+"',"+
        "'"+venda_fracionaria+"',"+
        string.Format("{0:0,0.00}",pis)+","+
        string.Format("{0:0,0.00}",cofins)+","+
        "'"+cf+"',"+
        "'"+und+"',"+
        (gera_inativo?1:0)+","+
        "'"+inventario+"',"+
        "'"+tipo_produto_origem+"',"+
        "'"+tipo_produto_destino+"',"+
        (curva_a?1:0)+","+
        (curva_b?1:0)+","+
        (curva_c?1:0)+","+
        (estoque_aviso?1:0)+","+
        "'"+artigo+"',"+
        estoque_margem+","+
        estoque_meses+","+
        cobertura+","+
        "'"+sazonal1.ToString("yyyy-MM-dd")+"',"+
        "'"+sazonal2.ToString("yyyy-MM-dd")+"',"+
        "'"+sazonal3.ToString("yyyy-MM-dd")+"',"+
        "'"+sazonal4.ToString("yyyy-MM-dd")+"',"+
        "'"+sazonal5.ToString("yyyy-MM-dd")+"',"+
        "'"+sazonal6.ToString("yyyy-MM-dd")+"',"+
        string.Format("{0:0,0.00}",peso_liquido)+","+
        string.Format("{0:0,0.00}",peso_bruto)+","+
        string.Format("{0:0,0.00}",margem_iva)+","+
        "'"+Marca+"',"+
        "'"+descricao_pisc+"',"+
        "'"+mercadoria+"',"+

  }
}/* --Atualizar DaoForm 
txt_Filial.Text=Filial.ToString();
txt_PLU.Text=PLU.ToString();
txt_Tipo.Text=Tipo.ToString();
txt_Peso_Variavel.Text=Peso_Variavel.ToString();
txt_Codigo_Portaria.Text=Codigo_Portaria.ToString();
txt_Codigo_Tributacao.Text=Codigo_Tributacao.ToString();
txt_Codigo_Tributacao_ent.Text=Codigo_Tributacao_ent.ToString();
txt_Codigo_departamento.Text=Codigo_departamento.ToString();
txt_Codigo_familia.Text=Codigo_familia.ToString();
txt_Descricao_departamento.Text=Descricao_departamento.ToString();
txt_Descricao.Text=Descricao.ToString();
txt_Descricao_resumida.Text=Descricao_resumida.ToString();
txt_Descricao_familia.Text=Descricao_familia.ToString();
txt_Tecla.Text=Tecla.ToString();
txt_Margem.Text=Margem.ToString();
txt_Estoque_minimo.Text=Estoque_minimo.ToString();
txt_Etiqueta.Text=Etiqueta.ToString();
txt_Validade.Text=Validade.ToString();
txt_Preco.Text=Preco.ToString();
txt_Preco_promocao.Text=Preco_promocao.ToString();
txt_data_inicio.Text=data_inicio.ToString();
txt_data_fim.Text=data_fim.ToString();
txt_Promocao_automatica.Text=Promocao_automatica.ToString();
txt_Promocao.Text=Promocao.ToString();
txt_Preco_Custo.Text=Preco_Custo.ToString();
txt_Data_Cadastro.Text=Data_Cadastro.ToString();
txt_Data_Alteracao.Text=Data_Alteracao.ToString();
txt_IPI.Text=IPI.ToString();
txt_Incide_Pis.Text=Incide_Pis.ToString();
txt_Embalagem.Text=Embalagem.ToString();
txt_ultimo_fornecedor.Text=ultimo_fornecedor.ToString();
txt_Fator_conversao.Text=Fator_conversao.ToString();
txt_Tecla_balanca.Text=Tecla_balanca.ToString();
txt_Localizacao.Text=Localizacao.ToString();
txt_Inativo.Text=Inativo.ToString();
txt_Imprime_etiqueta.Text=Imprime_etiqueta.ToString();
txt_Estado_Mercadoria.Text=Estado_Mercadoria.ToString();
txt_saldo_atual.Text=saldo_atual.ToString();
txt_Preco_Custo_1.Text=Preco_Custo_1.ToString();
txt_Preco_Custo_2.Text=Preco_Custo_2.ToString();
txt_Ref_fornecedor.Text=Ref_fornecedor.ToString();
txt_data_inventario.Text=data_inventario.ToString();
txt_saldo_inicial.Text=saldo_inicial.ToString();
txt_peso.Text=peso.ToString();
txt_receita.Text=receita.ToString();
txt_qtde_receita.Text=qtde_receita.ToString();
txt_preco_compra.Text=preco_compra.ToString();
txt_frete.Text=frete.ToString();
txt_seguro.Text=seguro.ToString();
txt_outras_despesas.Text=outras_despesas.ToString();
txt_valor_ipi.Text=valor_ipi.ToString();
txt_codigo_centro_custo.Text=codigo_centro_custo.ToString();
txt_venda_fracionaria.Text=venda_fracionaria.ToString();
txt_pis.Text=pis.ToString();
txt_cofins.Text=cofins.ToString();
txt_cf.Text=cf.ToString();
txt_und.Text=und.ToString();
txt_gera_inativo.Text=gera_inativo.ToString();
txt_inventario.Text=inventario.ToString();
txt_tipo_produto_origem.Text=tipo_produto_origem.ToString();
txt_tipo_produto_destino.Text=tipo_produto_destino.ToString();
txt_curva_a.Text=curva_a.ToString();
txt_curva_b.Text=curva_b.ToString();
txt_curva_c.Text=curva_c.ToString();
txt_estoque_aviso.Text=estoque_aviso.ToString();
txt_artigo.Text=artigo.ToString();
txt_estoque_margem.Text=estoque_margem.ToString();
txt_estoque_meses.Text=estoque_meses.ToString();
txt_cobertura.Text=cobertura.ToString();
txt_sazonal1.Text=sazonal1.ToString();
txt_sazonal2.Text=sazonal2.ToString();
txt_sazonal3.Text=sazonal3.ToString();
txt_sazonal4.Text=sazonal4.ToString();
txt_sazonal5.Text=sazonal5.ToString();
txt_sazonal6.Text=sazonal6.ToString();
txt_peso_liquido.Text=peso_liquido.ToString();
txt_peso_bruto.Text=peso_bruto.ToString();
txt_margem_iva.Text=margem_iva.ToString();
txt_Marca.Text=Marca.ToString();
txt_descricao_pisc.Text=descricao_pisc.ToString();
txt_mercadoria.Text=mercadoria.ToString();
*/ 
/* --Atualizar FormDao 
Filial=txtFilial.Text;
PLU=txtPLU.Text;
Tipo=txtTipo.Text;
Peso_Variavel=txtPeso_Variavel.Text;
Codigo_Portaria=txtCodigo_Portaria.Text;
Codigo_Tributacao=txtCodigo_Tributacao.Text;
Codigo_Tributacao_ent=txtCodigo_Tributacao_ent.Text;
Codigo_departamento=txtCodigo_departamento.Text;
Codigo_familia=txtCodigo_familia.Text;
Descricao_departamento=txtDescricao_departamento.Text;
Descricao=txtDescricao.Text;
Descricao_resumida=txtDescricao_resumida.Text;
Descricao_familia=txtDescricao_familia.Text;
Tecla=txtTecla.Text;
Margem=txtMargem.Text;
Estoque_minimo=txtEstoque_minimo.Text;
Etiqueta=txtEtiqueta.Text;
Validade=txtValidade.Text;
Preco=txtPreco.Text;
Preco_promocao=txtPreco_promocao.Text;
data_inicio=txtdata_inicio.Text;
data_fim=txtdata_fim.Text;
Promocao_automatica=txtPromocao_automatica.Text;
Promocao=txtPromocao.Text;
Preco_Custo=txtPreco_Custo.Text;
Data_Cadastro=txtData_Cadastro.Text;
Data_Alteracao=txtData_Alteracao.Text;
IPI=txtIPI.Text;
Incide_Pis=txtIncide_Pis.Text;
Embalagem=txtEmbalagem.Text;
ultimo_fornecedor=txtultimo_fornecedor.Text;
Fator_conversao=txtFator_conversao.Text;
Tecla_balanca=txtTecla_balanca.Text;
Localizacao=txtLocalizacao.Text;
Inativo=txtInativo.Text;
Imprime_etiqueta=txtImprime_etiqueta.Text;
Estado_Mercadoria=txtEstado_Mercadoria.Text;
saldo_atual=txtsaldo_atual.Text;
Preco_Custo_1=txtPreco_Custo_1.Text;
Preco_Custo_2=txtPreco_Custo_2.Text;
Ref_fornecedor=txtRef_fornecedor.Text;
data_inventario=txtdata_inventario.Text;
saldo_inicial=txtsaldo_inicial.Text;
peso=txtpeso.Text;
receita=txtreceita.Text;
qtde_receita=txtqtde_receita.Text;
preco_compra=txtpreco_compra.Text;
frete=txtfrete.Text;
seguro=txtseguro.Text;
outras_despesas=txtoutras_despesas.Text;
valor_ipi=txtvalor_ipi.Text;
codigo_centro_custo=txtcodigo_centro_custo.Text;
venda_fracionaria=txtvenda_fracionaria.Text;
pis=txtpis.Text;
cofins=txtcofins.Text;
cf=txtcf.Text;
und=txtund.Text;
gera_inativo=txtgera_inativo.Text;
inventario=txtinventario.Text;
tipo_produto_origem=txttipo_produto_origem.Text;
tipo_produto_destino=txttipo_produto_destino.Text;
curva_a=txtcurva_a.Text;
curva_b=txtcurva_b.Text;
curva_c=txtcurva_c.Text;
estoque_aviso=txtestoque_aviso.Text;
artigo=txtartigo.Text;
estoque_margem=txtestoque_margem.Text;
estoque_meses=txtestoque_meses.Text;
cobertura=txtcobertura.Text;
sazonal1=txtsazonal1.Text;
sazonal2=txtsazonal2.Text;
sazonal3=txtsazonal3.Text;
sazonal4=txtsazonal4.Text;
sazonal5=txtsazonal5.Text;
sazonal6=txtsazonal6.Text;
peso_liquido=txtpeso_liquido.Text;
peso_bruto=txtpeso_bruto.Text;
margem_iva=txtmargem_iva.Text;
Marca=txtMarca.Text;
descricao_pisc=txtdescricao_pisc.Text;
mercadoria=txtmercadoria.Text;
*/ 

