using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
namespace visualSysWeb.dao
{
public class MercadoriaDao
   {
     public String Filial { get; set; }
     public String PLU { get; set; }
     public String Tipo { get; set; }
     public String Peso_Variavel { get; set; }
     public String Codigo_Portaria { get; set; }
     public Decimal Codigo_Tributacao { get; set; }
     public Decimal Codigo_Tributacao_ent { get; set; }
     public String Codigo_departamento { get; set; }
     public String Codigo_familia { get; set; }
     public String Descricao_departamento { get; set; }
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
     public DateTime data_fim { get; set; }
     public bool Promocao_automatica { get; set; }
     public bool Promocao { get; set; }
     public Decimal Preco_Custo { get; set; }
     public DateTime Data_Cadastro { get; set; }
     public DateTime Data_Alteracao { get; set; }
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
     public DateTime sazonal2 { get; set; }
     public DateTime sazonal3 { get; set; }
     public DateTime sazonal4 { get; set; }
     public DateTime sazonal5 { get; set; }
     public DateTime sazonal6 { get; set; }
     public Decimal peso_liquido { get; set; }
     public Decimal peso_bruto { get; set; }
     public Decimal margem_iva { get; set; }
     public String Marca { get; set; }
     public String descricao_pisc { get; set; }
     public String mercadoria { get; set; }
   public MercadoriaDao(String plu){
       String sql="Select * from  Mercadoria where plu='"+plu+"'";
       SqlDataReader rs = Conexao.consulta(sql);
       carregarDados(rs);
   }
 
  public void carregarDados(SqlDataReader rs){
      Filial = rs["Filial"].ToString();
      PLU = rs["PLU"].ToString();
      Tipo = rs["Tipo"].ToString();
      Peso_Variavel = rs["Peso_Variavel"].ToString();
      Codigo_Portaria = rs["Codigo_Portaria"].ToString();
      Codigo_Tributacao = (Decimal)rs["Codigo_Tributacao"];
      Codigo_Tributacao_ent = (Decimal)rs["Codigo_Tributacao_ent"];
      Codigo_departamento = rs["Codigo_departamento"].ToString();
      Codigo_familia = rs["Codigo_familia"].ToString();
      Descricao_departamento = rs["Descricao_departamento"].ToString();
      Descricao = rs["Descricao"].ToString();
      Descricao_resumida = rs["Descricao_resumida"].ToString();
      Descricao_familia = rs["Descricao_familia"].ToString();
      Tecla = (Decimal)rs["Tecla"];
      Margem = (Decimal)rs["Margem"];
      Estoque_minimo = (Decimal)rs["Estoque_minimo"];
      Etiqueta = (Decimal)rs["Etiqueta"];
      Validade = (Decimal)rs["Validade"];
      Preco = (Decimal)rs["Preco"];
      Preco_promocao = (Decimal)rs["Preco_promocao"];
      data_inicio = DateTime.Parse(rs["data_inicio"].ToString());
      data_fim = DateTime.Parse(rs["data_fim"].ToString());
      Promocao_automatica = (rs["Promocao_automatica"].ToString().Equals("1")?true:false);
      Promocao = (rs["Promocao"].ToString().Equals("1")?true:false);
      Preco_Custo = (Decimal)rs["Preco_Custo"];
      Data_Cadastro = DateTime.Parse(rs["Data_Cadastro"].ToString());
      Data_Alteracao = DateTime.Parse(rs["Data_Alteracao"].ToString());
      IPI = (Decimal)rs["IPI"];
      Incide_Pis = (rs["Incide_Pis"].ToString().Equals("1")?true:false);
      Embalagem = (Decimal)rs["Embalagem"];
      ultimo_fornecedor = rs["ultimo_fornecedor"].ToString();
      Fator_conversao = (Decimal)rs["Fator_conversao"];
      Tecla_balanca = (Decimal)rs["Tecla_balanca"];
      Localizacao = rs["Localizacao"].ToString();
      Inativo = (rs["Inativo"].ToString().Equals("1")?true:false);
      Imprime_etiqueta = (rs["Imprime_etiqueta"].ToString().Equals("1")?true:false);
      Estado_Mercadoria = (rs["Estado_Mercadoria"].ToString().Equals("1")?true:false);
      saldo_atual = (Decimal)rs["saldo_atual"];
      Preco_Custo_1 = (Decimal)rs["Preco_Custo_1"];
      Preco_Custo_2 = (Decimal)rs["Preco_Custo_2"];
      Ref_fornecedor = rs["Ref_fornecedor"].ToString();
      data_inventario = DateTime.Parse(rs["data_inventario"].ToString());
      saldo_inicial = (Decimal)rs["saldo_inicial"];
      peso = (Decimal)rs["peso"];
      receita = rs["receita"].ToString();
      qtde_receita = (Decimal)rs["qtde_receita"];
      preco_compra = (Decimal)rs["preco_compra"];
      frete = (Decimal)rs["frete"];
      seguro = (Decimal)rs["seguro"];
      outras_despesas = (Decimal)rs["outras_despesas"];
      valor_ipi = (Decimal)rs["valor_ipi"];
      codigo_centro_custo = rs["codigo_centro_custo"].ToString();
      venda_fracionaria = rs["venda_fracionaria"].ToString();
      pis = (Decimal)rs["pis"];
      cofins = (Decimal)rs["cofins"];
      cf = rs["cf"].ToString();
      und = rs["und"].ToString();
      gera_inativo = (rs["gera_inativo"].ToString().Equals("1")?true:false);
      inventario = rs["inventario"].ToString();
      tipo_produto_origem = rs["tipo_produto_origem"].ToString();
      tipo_produto_destino = rs["tipo_produto_destino"].ToString();
      curva_a = (rs["curva_a"].ToString().Equals("1")?true:false);
      curva_b = (rs["curva_b"].ToString().Equals("1")?true:false);
      curva_c = (rs["curva_c"].ToString().Equals("1")?true:false);
      estoque_aviso = (rs["estoque_aviso"].ToString().Equals("1")?true:false);
      artigo = rs["artigo"].ToString();
      estoque_margem = int.Parse(rs["estoque_margem"].ToString());
      estoque_meses = int.Parse(rs["estoque_meses"].ToString());
      cobertura = int.Parse(rs["cobertura"].ToString());
      sazonal1 = DateTime.Parse(rs["sazonal1"].ToString());
      sazonal2 = DateTime.Parse(rs["sazonal2"].ToString());
      sazonal3 = DateTime.Parse(rs["sazonal3"].ToString());
      sazonal4 = DateTime.Parse(rs["sazonal4"].ToString());
      sazonal5 = DateTime.Parse(rs["sazonal5"].ToString());
      sazonal6 = DateTime.Parse(rs["sazonal6"].ToString());
      peso_liquido = (Decimal)rs["peso_liquido"];
      peso_bruto = (Decimal)rs["peso_bruto"];
      margem_iva = (Decimal)rs["margem_iva"];
      Marca = rs["Marca"].ToString();
      descricao_pisc = rs["descricao_pisc"].ToString();
      mercadoria = rs["mercadoria"].ToString();
       }
   }
}
