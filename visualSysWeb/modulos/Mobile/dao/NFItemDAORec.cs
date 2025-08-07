using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Mobile.dao
{
    public class NFItemDAORec
    {
        public string Filial { get; set; }
        public string Codigo { get; set; }
        public string Cliente_Fornecedor { get; set; }
        public int Tipo_NF { get; set; }
        public string PLU { get; set; }
        public decimal Codigo_Tributacao { get; set; }
        public decimal Qtde { get; set; }
        public decimal Embalagem { get; set; }
        public decimal Unitario { get; set; }
        public decimal Total { get; set; }
        public decimal Preco_custo { get; set; }
        public int Num_Item { get; set; }
        public decimal PISV { get; set; } 
        public decimal CofinsV { get; set; }
        public string NCM { get; set; }
        public string Und { get; set; }
        public string Artigo { get; set; } = "";
        public decimal Peso_Liquido { get; set; }
        public decimal Peso_Bruto { get; set; }
        public string Tipo { get; set; }
        public string CF { get; set; } = "";
        public string cstpis { get; set; }
        public string cstcofins { get; set; }
        public decimal aliquota_icms { get; set; }
        public decimal redutor_base { get; set; }
        public decimal codigo_operacao { get; set; }
        public string Indice_st { get; set; }
        public decimal Cfop { get; set; }
        public int Origem { get; set; }
        public int inativo { get; set; }
        public decimal base_pis { get; set; }
        public decimal base_cofins { get; set; }
        public decimal aliquota_pis { get; set; }
        public decimal aliquota_cofins { get; set; }
        public string cst_icms { get; set; }
        public decimal base_icms { get; set; }
        public decimal icmsv { get; set; }
        public string CEST { get; set; } = "";
        public int finNFe { get; set; }
        public decimal total_faturado { get; set; }
        public decimal vTotTrib { get; set; }
        public string indEscala { get; set; }
        public DateTime data_validade { get; set; }
        public int serie { get; set; }
        public decimal total_produto { get; set; }
        public decimal aliq_pis { get; set; }
        public decimal aliq_cofins { get; set; }
        public string Codigo_Emissao_NFe { get; set; } = "";
        public string Pedido_Numero { get; set; } = "";
        public string Pedido_Sequencia { get; set; } = "";
        public decimal Aliquota_ICMS_Destino { get; set; } = 0;
        public decimal Valor_Difal { get; set; } = 0;
        public decimal IPI { get; set; } = 0;
        public string Descricao { get; set; } = "";
        public decimal IPIV { get; set; } = 0;
        public decimal IVA { get; set; } = 0;
        public decimal BASE_IVA { get; set; } = 0;
        public decimal MARGEM_IVA { get; set; } = 0;
        public decimal Despesas { get; set; } = 0;
        public decimal Desconto { get; set; } = 0;
        public string codigo_referencia { get; set; } = "";
        public string EAN { get; set; } = "";
        public decimal pCredSN { get; set; } = 0;
        public decimal vCredICMSSN { get; set; } = 0;
        public int Nf_canc { get; set; } = 0;
        public decimal Aliquota_iva { get; set; } = 0;
        public string CST_IPI { get; set; } = "";
        public decimal base_ipi { get; set; } = 0;
        public string cnpj_Fabricante { get; set; } = "";
        public string cBenef { get; set; } = "";
        public decimal vBCFCP { get; set; } = 0;
        public decimal pFCP { get; set; } = 0;
        public decimal vFCP { get; set; } = 0;
        public decimal vBCFCPST { get; set; } = 0;
        public decimal pFCPST { get; set; } = 0;
        public decimal vFCPST { get; set; } = 0;
        public decimal vIPIDevol { get; set; } = 0;
        public decimal pDevol { get; set; } = 0;
        public decimal desconto_valor { get; set; } = 0;
        public string codigo_centro_custo { get; set; } = "";
        public string ordem_compra { get; set; } = "";
        public string codigo_devolucao { get; set; } = "";
        public string Codigo_Produto_ANVISA { get; set; } = "";
        public string Motivo_Isencao_ANVISA { get; set; } = "";
        public decimal Preco_Maximo_ANVISA { get; set; } = 0;
        public decimal Redutor_base_Iva { get; set; } = 0;
        public decimal Frete { get; set; } = 0;
        public decimal Qtde_Devolver { get; set; } = 0;

        public bool insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string query = @"INSERT INTO NF_Item (Filial, Codigo, Cliente_Fornecedor, Tipo_NF, PLU, Codigo_Tributacao, Qtde, Embalagem, Unitario, Desconto, Total, IPI, Descricao, IPIV, Preco_custo, IVA, BASE_IVA, MARGEM_IVA, EAN, Despesas, Num_Item, PISV, CofinsV, NCM, Und, Artigo, Peso_Liquido, Peso_Bruto, Tipo, CF, cstpis, cstcofins, codigo_referencia, aliquota_icms, redutor_base, codigo_operacao, pCredSN, vCredICMSSN, Nf_canc, Aliquota_iva, Indice_st, Cfop, Frete, Origem, inativo, base_pis, base_cofins, aliquota_pis, aliquota_cofins, cst_icms, base_icms, icmsv, CEST, CST_IPI, base_ipi, finNFe, total_faturado, vTotTrib, indEscala, cnpj_Fabricante, cBenef, vBCFCP, pFCP, vFCP, vBCFCPST, pFCPST, vFCPST, vIPIDevol, pDevol, data_validade, serie, desconto_valor, total_produto, codigo_centro_custo, ordem_compra, aliq_pis, aliq_cofins, Codigo_Devolucao, Codigo_Produto_ANVISA, Motivo_Isencao_ANVISA, Preco_Maximo_ANVISA, Redutor_base_Iva, Codigo_Emissao_NFe, Pedido_Numero, Pedido_Sequencia, Qtde_Devolver)
                            VALUES (@Filial, @Codigo, @Cliente_Fornecedor, @Tipo_NF, @PLU, @Codigo_Tributacao, @Qtde, @Embalagem, @Unitario, @Desconto, @Total, @IPI, @Descricao, @IPIV, @Preco_custo, @IVA, @BASE_IVA, @MARGEM_IVA, @EAN, @Despesas, @Num_Item, @PISV, @CofinsV, @NCM, @Und, @Artigo, @Peso_Liquido, @Peso_Bruto, @Tipo, @CF, @cstpis, @cstcofins, @codigo_referencia, @aliquota_icms, @redutor_base, @codigo_operacao, @pCredSN, @vCredICMSSN, @Nf_canc, @Aliquota_iva, @Indice_st, @Cfop, @Frete, @Origem, @inativo, @base_pis, @base_cofins, @aliquota_pis, @aliquota_cofins, @cst_icms, @base_icms, @icmsv, @CEST, @CST_IPI, @base_ipi, @finNFe, @total_faturado, @vTotTrib, @indEscala, @cnpj_Fabricante, @cBenef, @vBCFCP, @pFCP, @vFCP, @vBCFCPST, @pFCPST, @vFCPST, @vIPIDevol, @pDevol, @data_validade, @serie, @desconto_valor, @total_produto, @codigo_centro_custo, @ordem_compra, @aliq_pis, @aliq_cofins, @Codigo_Devolucao, @Codigo_Produto_ANVISA, @Motivo_Isencao_ANVISA, @Preco_Maximo_ANVISA, @Redutor_base_Iva, @Codigo_Emissao_NFe, @Pedido_Numero, @Pedido_Sequencia, @Qtde_Devolver)";

                using (SqlCommand command = new SqlCommand(query, conn, tran))
                {
                    command.Parameters.AddWithValue("@Filial", Filial);
                    command.Parameters.AddWithValue("@Codigo", Codigo);
                    command.Parameters.AddWithValue("@Cliente_Fornecedor", Cliente_Fornecedor);
                    command.Parameters.AddWithValue("@Tipo_NF", Tipo_NF);
                    command.Parameters.AddWithValue("@PLU", PLU);
                    command.Parameters.AddWithValue("@Codigo_Tributacao", Codigo_Tributacao);
                    command.Parameters.AddWithValue("@Qtde", Qtde);
                    command.Parameters.AddWithValue("@Embalagem", Embalagem);
                    command.Parameters.AddWithValue("@Unitario", Unitario);
                    command.Parameters.AddWithValue("@Desconto", Desconto);
                    command.Parameters.AddWithValue("@Total", Total);
                    command.Parameters.AddWithValue("@IPI", IPI);
                    command.Parameters.AddWithValue("@Descricao", Descricao);
                    command.Parameters.AddWithValue("@IPIV", IPIV);
                    command.Parameters.AddWithValue("@Preco_custo", Preco_custo);
                    command.Parameters.AddWithValue("@IVA", IVA);
                    command.Parameters.AddWithValue("@BASE_IVA", BASE_IVA);
                    command.Parameters.AddWithValue("@MARGEM_IVA", MARGEM_IVA);
                    command.Parameters.AddWithValue("@EAN", EAN);
                    command.Parameters.AddWithValue("@Despesas", Despesas);
                    command.Parameters.AddWithValue("@Num_Item", Num_Item);
                    command.Parameters.AddWithValue("@PISV", PISV);
                    command.Parameters.AddWithValue("@CofinsV", CofinsV);
                    command.Parameters.AddWithValue("@NCM", NCM);
                    command.Parameters.AddWithValue("@Und", Und);
                    command.Parameters.AddWithValue("@Artigo", Artigo);
                    command.Parameters.AddWithValue("@Peso_Liquido", Peso_Liquido);
                    command.Parameters.AddWithValue("@Peso_Bruto", Peso_Bruto);
                    command.Parameters.AddWithValue("@Tipo", Tipo);
                    command.Parameters.AddWithValue("@CF", CF);
                    command.Parameters.AddWithValue("@cstpis", cstpis);
                    command.Parameters.AddWithValue("@cstcofins", cstcofins);
                    command.Parameters.AddWithValue("@codigo_referencia", codigo_referencia);
                    command.Parameters.AddWithValue("@aliquota_icms", aliquota_icms);
                    command.Parameters.AddWithValue("@redutor_base", redutor_base);
                    command.Parameters.AddWithValue("@codigo_operacao", codigo_operacao);
                    command.Parameters.AddWithValue("@pCredSN", pCredSN);
                    command.Parameters.AddWithValue("@vCredICMSSN", vCredICMSSN);
                    command.Parameters.AddWithValue("@Nf_canc", Nf_canc);
                    command.Parameters.AddWithValue("@Aliquota_iva", Aliquota_iva);
                    command.Parameters.AddWithValue("@Indice_st", Indice_st);
                    command.Parameters.AddWithValue("@Cfop", Cfop);
                    command.Parameters.AddWithValue("@Frete", Frete);
                    command.Parameters.AddWithValue("@Origem", Origem);
                    command.Parameters.AddWithValue("@inativo", inativo);
                    command.Parameters.AddWithValue("@base_pis", base_pis);
                    command.Parameters.AddWithValue("@base_cofins", base_cofins);
                    command.Parameters.AddWithValue("@aliquota_pis", aliquota_pis);
                    command.Parameters.AddWithValue("@aliquota_cofins", aliquota_cofins);
                    command.Parameters.AddWithValue("@cst_icms", cst_icms);
                    command.Parameters.AddWithValue("@base_icms", base_icms);
                    command.Parameters.AddWithValue("@icmsv", icmsv);
                    command.Parameters.AddWithValue("@CEST", CEST);
                    command.Parameters.AddWithValue("@CST_IPI", CST_IPI);
                    command.Parameters.AddWithValue("@base_ipi", base_ipi);
                    command.Parameters.AddWithValue("@finNFe", finNFe);
                    command.Parameters.AddWithValue("@total_faturado", total_faturado);
                    command.Parameters.AddWithValue("@vTotTrib", vTotTrib);
                    command.Parameters.AddWithValue("@indEscala", indEscala);
                    command.Parameters.AddWithValue("@cnpj_Fabricante", cnpj_Fabricante);
                    command.Parameters.AddWithValue("@cBenef", cBenef);
                    command.Parameters.AddWithValue("@vBCFCP", vBCFCP);
                    command.Parameters.AddWithValue("@pFCP", pFCP);
                    command.Parameters.AddWithValue("@vFCP", vFCP);
                    command.Parameters.AddWithValue("@vBCFCPST", vBCFCPST);
                    command.Parameters.AddWithValue("@pFCPST", pFCPST);
                    command.Parameters.AddWithValue("@vFCPST", vFCPST);
                    command.Parameters.AddWithValue("@vIPIDevol", vIPIDevol);
                    command.Parameters.AddWithValue("@pDevol", pDevol);
                    command.Parameters.AddWithValue("@data_validade", data_validade);
                    command.Parameters.AddWithValue("@serie", serie);
                    command.Parameters.AddWithValue("@desconto_valor", desconto_valor);
                    command.Parameters.AddWithValue("@total_produto", total_produto);
                    command.Parameters.AddWithValue("@codigo_centro_custo", codigo_centro_custo);
                    command.Parameters.AddWithValue("@ordem_compra", ordem_compra);
                    command.Parameters.AddWithValue("@aliq_pis", aliq_pis);
                    command.Parameters.AddWithValue("@aliq_cofins", aliq_cofins);
                    command.Parameters.AddWithValue("@Codigo_Devolucao", codigo_devolucao);
                    command.Parameters.AddWithValue("@Codigo_Produto_ANVISA", Codigo_Produto_ANVISA);
                    command.Parameters.AddWithValue("@Motivo_Isencao_ANVISA", Motivo_Isencao_ANVISA);
                    command.Parameters.AddWithValue("@Preco_Maximo_ANVISA", Preco_Maximo_ANVISA);
                    command.Parameters.AddWithValue("@Redutor_base_Iva", Redutor_base_Iva);
                    command.Parameters.AddWithValue("@Codigo_Emissao_NFe", Codigo_Emissao_NFe);
                    command.Parameters.AddWithValue("@Pedido_Numero", Pedido_Numero);
                    command.Parameters.AddWithValue("@Pedido_Sequencia", Pedido_Sequencia);
                    command.Parameters.AddWithValue("@Qtde_Devolver", Qtde_Devolver);

                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}