using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Mobile.dao
{
    public class NFDAORec
    {
        public string Codigo { get; set; } = "";
        public string Cliente_Fornecedor { get; set; } = "";
        public int Tipo_NF { get; set; } = 0;
        public DateTime Data { get; set; }
        public decimal Codigo_operacao { get; set; } = 0;
        public decimal codigo_operacao1 { get; set; } = 0;
        public DateTime Emissao { get; set; }
        public string Filial { get; set; } = "";
        public decimal Total { get; set; } = 0;
        public decimal Desconto { get; set; } = 0;
        public decimal Frete { get; set; } = 0;
        public decimal Seguro { get; set; } = 0;
        public decimal IPI_Nota { get; set; } = 0;
        public decimal Outras { get; set; } = 0;
        public decimal ICMS_Nota { get; set; } = 0;
        public int Estado { get; set; } = 0;
        public decimal Base_Calculo { get; set; } = 0;
        public decimal Despesas_financeiras { get; set; } = 0;
        public string Pedido { get; set; } = "";
        public decimal Base_Calc_Subst { get; set; } = 0;
        public string Observacao { get; set; } = "";
        public int nf_Canc { get; set; } = 0;
        public char formulario { get; set; }
        public string nome_transportadora { get; set; } = "";
        public decimal qtde { get; set; } = 0;
        public string especie { get; set; } = "";
        public string marca { get; set; } = "";
        public decimal numero { get; set; } = 0;
        public decimal peso_bruto { get; set; } = 0;
        public decimal peso_liquido { get; set; } = 0;
        public int tipo_frete { get; set; } = 0;
        public string funcionario { get; set; } = "";
        public string centro_custo { get; set; } = "";
        public decimal encargo_financeiro { get; set; }
        public decimal ICMS_ST { get; set; } = 0;
        public string Pedido_cliente { get; set; } = "";
        public string Fornecedor_CNPJ { get; set; } = "";
        public string Placa { get; set; } = "";
        public string Endereco_Entrega { get; set; } = "";
        public decimal Desconto_geral { get; set; } = 0;
        public string Nome_fantasia { get; set; } = "";
        public int boleto_recebido { get; set; } = 0;
        public string usuario { get; set; } = "";
        public int xml { get; set; } = 0;
        public int nfe { get; set; } = 0;
        public string ID { get; set; } = "";
        public string Status { get; set; } = "";
        public string Usuario_precificacao { get; set; } = "";
        public DateTime Data_precificacao { get; set; }
        public int precoTodasFiliais { get; set; } = 0;
        public int serie { get; set; } = 0;
        public string Nota_referencia { get; set; } = "";
        public int indPres { get; set; } = 0;
        public int indFinal { get; set; } = 0;
        public int Dest_Fornec { get; set; } = 0;
        public int Ref_ECF { get; set; } = 0;
        public decimal base_pis { get; set; } = 0;
        public decimal base_cofins { get; set; } = 0;
        public decimal pisv { get; set; } = 0;
        public decimal cofinsv { get; set; } = 0;
        public decimal total_produto { get; set; } = 0;
        public string CodigoNotaProdutor { get; set; } = "";
        public string numero_protocolo { get; set; } = "";
        public int finNFe { get; set; } = 0;
        public string IDBarra { get; set; } = "";
        public int Producao_NFe { get; set; } = 0;
        public decimal vTotTrib { get; set; } = 0;
        public string emissao_hora { get; set; } = "";
        public string data_hora { get; set; } = "";
        public decimal vFCP { get; set; } = 0;
        public decimal vFCPST { get; set; } = 0;
        public string tPag { get; set; } = "";
        public decimal vIPIDevol { get; set; } = 0;
        public string usuario_alteracao { get; set; } = "";
        public decimal vCredicmssn { get; set; } = 0;
        public string ordem_compra { get; set; } = "";
        public int Integrado { get; set; } = 0;
        public int indIntermed { get; set; } = 0;
        public string intermedCnpj { get; set; } = "";
        public string idCadIntTran { get; set; } = "";
        public string CNPJPagamento { get; set; } = "";
        public DateTime DataHora_Lancamento { get; set; }
        public string Validacao_Fiscal { get; set; } = "";
        public decimal Valor_Difal { get; set; } = 0;
        public List<NFItemDAORec> Itens;

        public NFDAORec(string chave, User usr)
        {
            natureza_operacaoDAO natureza = new natureza_operacaoDAO("7002", usr);
            SqlDataReader dr = Conexao.consulta("Exec sp_NFe_Total 'NFE" + chave + "'", usr, true);

            dr.Read();
            String Cnpj_Cpf = Conexao.retornaUmValor("select  emit_CNPJ = isnull(emit_CNPJ,emit_CPF) FROM NFe_XML where id LIKE'%NFe" + chave + "' and((emit_CNPJ is not null and emit_CNPJ <>'' ) or emit_CPF is Not null) group by emit_CNPJ,emit_CPF", usr);
            int tFornecedor = Conexao.countSql("SELECT cnpj FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr);

            if (tFornecedor <= 0)
            {
                throw new Exception("Fornecedor não Cadastrado");
            }
            //Dados NFe (Cabeçalho)
            Filial = usr.getFilial();
            Codigo = dr["ide_nNF"].ToString();
            Emissao = (dr["ide_dEmi"].ToString().Equals("") ? new DateTime() : DateTime.Parse(dr["ide_dEmi"].ToString()));
            Tipo_NF = 2;
            serie = Funcoes.intTry(chave.Substring(22, 3));
            usuario = usr.getNome();
            Data = DateTime.Now.Date;
            ID = chave;
            Codigo_operacao = natureza.Codigo_operacao;
            codigo_operacao1 = 0;
            centro_custo = "000000000";
            usuario = usr.getUsuario();

            SqlDataReader rsFornecedor = Conexao.consulta("SELECT * FROM Fornecedor WHERE replace(replace(replace(CNPJ,'.',''),'/',''),'-','')='" + Cnpj_Cpf + "'", usr, true);
            if (rsFornecedor.Read())
            {
                Fornecedor_CNPJ = rsFornecedor["CNPJ"].ToString();
                Cliente_Fornecedor = rsFornecedor["Fornecedor"].ToString();
            }

            //NFE Itens
            Itens = new List<NFItemDAORec>();
            String strSqlItens = "Exec sp_NFe_det 'NFE" + chave + "'";
            int tItens = Conexao.countSql(strSqlItens, usr);
            if (tItens <= 0)
            {
                throw new Exception("NFe foi importada com problemas, favor entrar em contato com o suporte.");
            }
            SqlDataReader rsItens = Conexao.consulta(strSqlItens, usr, true);
            
            while (rsItens.Read())
            {
                NFItemDAORec item = new NFItemDAORec();
                item.Filial = Filial;
                item.Cliente_Fornecedor = Cliente_Fornecedor;
                item.Codigo = Codigo;
                item.serie = serie;
                item.Tipo_NF = Tipo_NF;
                item.PLU = rsItens["det_prod_cProd"].ToString();
                item.codigo_referencia = item.PLU;
                item.EAN = rsItens["det_prod_cEANTrib"].ToString();
                item.Codigo_Tributacao = 0;
                item.Qtde = (rsItens["det_prod_qCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_qCom"].ToString()));
                item.Unitario = (rsItens["det_prod_vUnCom"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vUnCom"].ToString()));
                item.Embalagem = 1;
                item.Unitario = item.Unitario;
                item.total_produto = Funcoes.decTry(rsItens["det_prod_vProd"].ToString());
                item.Total = item.total_produto;
                item.Desconto = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                item.desconto_valor = (rsItens["det_prod_vDesc"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vDesc"].ToString()));
                item.total_faturado = item.Total;
                item.indEscala = "N";
                item.Und = "UN";
                item.NCM = rsItens["det_prod_NCM"].ToString();

                if (item.Desconto > 0)
                {
                    item.Desconto = ((item.Desconto / Decimal.Parse(rsItens["det_prod_vProd"].ToString())) * 100);
                }
                item.Despesas = (rsItens["det_prod_vOutro"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_prod_vOutro"].ToString()));
                item.Preco_custo = item.total_produto;
                item.Descricao = rsItens["det_prod_xProd"].ToString();
                item.IVA = 0;
                item.BASE_IVA = 0;
                item.MARGEM_IVA = 0;
                item.Num_Item = (rsItens["det_nItem"].ToString().Equals("") ? 0 : int.Parse(rsItens["det_nItem"].ToString()));
                item.Origem = (rsItens["det_icms_orig"].ToString().Equals("") ? 0 : int.Parse(rsItens["det_icms_orig"].ToString()));
                //ICMS
                if (natureza.Incide_ICMS)
                {
                    item.base_icms = (rsItens["det_icms_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vBC"].ToString()));
                    item.aliquota_icms = (rsItens["det_icms_pICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_pICMS"].ToString()));
                    item.icmsv = (rsItens["det_icms_vICMS"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_icms_vICMS"].ToString()));
                    item.cst_icms = rsItens["det_icms_CST"].ToString();
                    item.Indice_st = item.cst_icms;
                }
                else
                {
                    if (!natureza.cst_ICMS.Equals(""))
                    {
                        item.Indice_st = natureza.cst_ICMS;
                        item.cst_icms = natureza.cst_ICMS;
                    }
                    else
                    {
                        item.Indice_st = rsItens["det_icms_CST"].ToString();
                        item.cst_icms = rsItens["det_icms_CST"].ToString();
                    }
                    item.aliquota_icms = 0;
                    item.base_icms = 0;
                    item.icmsv = 0;
                }
                //CFOP
                switch (item.cst_icms)
                {
                    case "60":
                    case "10":
                    case "70":
                    case "30":
                        item.codigo_operacao = Decimal.Parse(natureza.cfop_st);
                        break;
                    default:
                        item.codigo_operacao = Decimal.Parse(natureza.cfop);
                        break;
                }
                item.Cfop = item.codigo_operacao;
                //IPI
                if (natureza.Incide_IPI)
                {
                    item.base_ipi = (rsItens["det_ipi_vBC"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_vBC"].ToString()));
                    item.IPI = (rsItens["det_ipi_pIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_pIPI"].ToString()));
                    item.IPIV = (rsItens["det_ipi_vIPI"].ToString().Equals("") ? 0 : Decimal.Parse(rsItens["det_ipi_vIPI"].ToString()));
                }
                else
                {
                    item.base_ipi = 0;
                    item.IPI = 0;
                    item.IPIV = 0;
                }
                //PIS e COFINS
                if (natureza.incide_PisCofins)
                {
                    if (rsItens["det_pis_CST"].ToString().Equals("01") || rsItens["det_pis_CST"].ToString().Equals("02") || rsItens["det_pis_CST"].ToString().Equals("03"))
                    {
                        item.cstcofins = "50";
                        item.cstpis = "50";
                        item.base_cofins = item.total_produto - item.icmsv;
                        item.base_pis = item.total_produto - item.icmsv;
                        item.aliquota_pis = 1.65m;
                        item.aliq_pis = 1.65m;
                        item.aliquota_cofins = 7.60m;
                        item.aliq_cofins = 7.60m;
                        item.CofinsV = Math.Round(item.base_cofins * (item.aliquota_cofins / 100), 2);
                        item.PISV = Math.Round(item.base_cofins * (item.aliquota_pis / 100), 2);
                    }
                    else
                    {
                        item.cstcofins = "73";
                        item.cstpis = "73";
                        item.base_cofins = 0;
                        item.base_pis = 0;
                        item.aliquota_pis = 0;
                        item.aliq_pis = 0;
                        item.aliquota_cofins = 0;
                        item.aliq_cofins =0;
                        item.CofinsV = 0;
                        item.PISV = 0;
                    }
                }
                else
                {
                    if (!natureza.cst_pis_cofins.Equals(""))
                    {
                        item.cstcofins = natureza.cst_pis_cofins;
                        item.cstpis = natureza.cst_pis_cofins;
                    }
                    else
                    {
                        item.cstcofins = natureza.cst_pis_cofins;
                        item.cstpis = natureza.cst_pis_cofins;
                    }
                    item.base_cofins = 0;
                    item.base_pis = 0;
                    item.CofinsV = 0;
                    item.PISV = 0;
                    item.aliquota_pis = 0;
                    item.aliq_pis = 0;
                    item.aliquota_cofins = 0;
                    item.aliq_cofins = 0;
                }
                Itens.Add(item);
            }
            //Valores padrão
            Estado = 1;
            tPag = "90";
            finNFe = 1;
            Dest_Fornec = 1;
            Usuario_precificacao = usr.getUsuario();
            Data_precificacao = DateTime.Now.Date;

            //Processar totalizadores
            Total = 0;
            total_produto = 0;
            Desconto = 0;
            Frete = 0;
            Seguro = 0;
            IPI_Nota = 0;
            Outras = 0;
            ICMS_Nota = 0;
            Despesas_financeiras = 0;
            base_cofins = 0;
            base_pis = 0;
            pisv = 0;
            cofinsv = 0;
            foreach (NFItemDAORec item in Itens)
            {
                Total += item.Total;
                total_produto += item.total_produto;
                Desconto += item.desconto_valor;
                Frete += item.Frete;
                Seguro = 0; 
                IPI_Nota += item.IPIV;
                Outras = 0; 
                ICMS_Nota += item.icmsv;
                Despesas_financeiras += item.Despesas;
                base_cofins += item.base_cofins;
                base_pis += item.base_pis;
                pisv += item.PISV;
                cofinsv += item.CofinsV;
            }
        }

        public bool insert()
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            try
            {

                string query = @"INSERT INTO NF (Codigo, Cliente_Fornecedor, Tipo_NF, Data, Codigo_operacao, codigo_operacao1, Emissao, Filial, Total, Desconto, Frete, Seguro, IPI_Nota, Outras, ICMS_Nota, Estado, Base_Calculo, Despesas_financeiras, Pedido, Base_Calc_Subst, Observacao, nf_Canc, formulario, nome_transportadora, qtde, especie, marca, numero, peso_bruto, peso_liquido, tipo_frete, funcionario, centro_custo, encargo_financeiro, ICMS_ST, Pedido_cliente, Fornecedor_CNPJ, Placa, Endereco_Entrega, Desconto_geral, Nome_fantasia, boleto_recebido, usuario, xml, nfe, ID, Status, Usuario_precificacao, Data_precificacao, precoTodasFiliais, serie, Nota_referencia, indPres, indFinal, Dest_Fornec, Ref_ECF, base_pis, base_cofins, pisv, cofinsv, total_produto, CodigoNotaProdutor, numero_protocolo, finNFe, IDBarra, Producao_NFe, vTotTrib, emissao_hora, data_hora, vFCP, vFCPST, tPag, vIPIDevol, usuario_alteracao, vCredicmssn, ordem_compra, Integrado, indIntermed, intermedCnpj, idCadIntTran, CNPJPagamento, DataHora_Lancamento, Validacao_Fiscal)
                            VALUES (@Codigo, @Cliente_Fornecedor, @Tipo_NF, @Data, @Codigo_operacao, @codigo_operacao1, @Emissao, @Filial, @Total, @Desconto, @Frete, @Seguro, @IPI_Nota, @Outras, @ICMS_Nota, @Estado, @Base_Calculo, @Despesas_financeiras, @Pedido, @Base_Calc_Subst, @Observacao, @nf_Canc, @formulario, @nome_transportadora, @qtde, @especie, @marca, @numero, @peso_bruto, @peso_liquido, @tipo_frete, @funcionario, @centro_custo, @encargo_financeiro, @ICMS_ST, @Pedido_cliente, @Fornecedor_CNPJ, @Placa, @Endereco_Entrega, @Desconto_geral, @Nome_fantasia, @boleto_recebido, @usuario, @xml, @nfe, @ID, @Status, @Usuario_precificacao, @Data_precificacao, @precoTodasFiliais, @serie, @Nota_referencia, @indPres, @indFinal, @Dest_Fornec, @Ref_ECF, @base_pis, @base_cofins, @pisv, @cofinsv, @total_produto, @CodigoNotaProdutor, @numero_protocolo, @finNFe, @IDBarra, @Producao_NFe, @vTotTrib, @emissao_hora, @data_hora, @vFCP, @vFCPST, @tPag, @vIPIDevol, @usuario_alteracao, @vCredicmssn, @ordem_compra, @Integrado, @indIntermed, @intermedCnpj, @idCadIntTran, @CNPJPagamento, @DataHora_Lancamento, @Validacao_Fiscal)";

                using (SqlCommand command = new SqlCommand(query, conn, trans))
                {
                    command.Parameters.AddWithValue("@Codigo", Codigo);
                    command.Parameters.AddWithValue("@Cliente_Fornecedor", Cliente_Fornecedor);
                    command.Parameters.AddWithValue("@Tipo_NF", Tipo_NF);
                    command.Parameters.AddWithValue("@Data", Data);
                    command.Parameters.AddWithValue("@Codigo_operacao", Codigo_operacao);
                    command.Parameters.AddWithValue("@codigo_operacao1", Codigo_operacao);
                    command.Parameters.AddWithValue("@Emissao", Emissao);
                    command.Parameters.AddWithValue("@Filial", Filial);
                    command.Parameters.AddWithValue("@Total", Total);
                    command.Parameters.AddWithValue("@Desconto", Desconto);
                    command.Parameters.AddWithValue("@Frete", Frete);
                    command.Parameters.AddWithValue("@Seguro", Seguro);
                    command.Parameters.AddWithValue("@IPI_Nota", IPI_Nota);
                    command.Parameters.AddWithValue("@Outras", Outras);
                    command.Parameters.AddWithValue("@ICMS_Nota", ICMS_Nota);
                    command.Parameters.AddWithValue("@Estado", Estado);
                    command.Parameters.AddWithValue("@Base_Calculo", Base_Calculo);
                    command.Parameters.AddWithValue("@Despesas_financeiras", Despesas_financeiras);
                    command.Parameters.AddWithValue("@Pedido", Pedido);
                    command.Parameters.AddWithValue("@Base_Calc_Subst", Base_Calc_Subst);
                    command.Parameters.AddWithValue("@Observacao", Observacao);
                    command.Parameters.AddWithValue("@nf_Canc", nf_Canc);
                    command.Parameters.AddWithValue("@formulario", System.Data.SqlTypes.SqlBinary.Null);
                    command.Parameters.AddWithValue("@nome_transportadora", nome_transportadora);
                    command.Parameters.AddWithValue("@qtde", qtde);
                    command.Parameters.AddWithValue("@especie", especie);
                    command.Parameters.AddWithValue("@marca", marca);
                    command.Parameters.AddWithValue("@numero", numero);
                    command.Parameters.AddWithValue("@peso_bruto", peso_bruto);
                    command.Parameters.AddWithValue("@peso_liquido", peso_liquido);
                    command.Parameters.AddWithValue("@tipo_frete", tipo_frete);
                    command.Parameters.AddWithValue("@funcionario", funcionario);
                    command.Parameters.AddWithValue("@centro_custo", centro_custo);
                    command.Parameters.AddWithValue("@encargo_financeiro", encargo_financeiro);
                    command.Parameters.AddWithValue("@ICMS_ST", ICMS_ST);
                    command.Parameters.AddWithValue("@Pedido_cliente", Pedido_cliente);
                    command.Parameters.AddWithValue("@Fornecedor_CNPJ", Fornecedor_CNPJ);
                    command.Parameters.AddWithValue("@Placa", Placa);
                    command.Parameters.AddWithValue("@Endereco_Entrega", Endereco_Entrega);
                    command.Parameters.AddWithValue("@Desconto_geral", Desconto_geral);
                    command.Parameters.AddWithValue("@Nome_fantasia", Nome_fantasia);
                    command.Parameters.AddWithValue("@boleto_recebido", boleto_recebido);
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@xml", xml);
                    command.Parameters.AddWithValue("@nfe", nfe);
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@Usuario_precificacao", Usuario_precificacao);
                    command.Parameters.AddWithValue("@Data_precificacao", Data_precificacao);
                    command.Parameters.AddWithValue("@precoTodasFiliais", precoTodasFiliais);
                    command.Parameters.AddWithValue("@serie", serie);
                    command.Parameters.AddWithValue("@Nota_referencia", Nota_referencia);
                    command.Parameters.AddWithValue("@indPres", indPres);
                    command.Parameters.AddWithValue("@indFinal", indFinal);
                    command.Parameters.AddWithValue("@Dest_Fornec", Dest_Fornec);
                    command.Parameters.AddWithValue("@Ref_ECF", Ref_ECF);
                    command.Parameters.AddWithValue("@base_pis", base_pis);
                    command.Parameters.AddWithValue("@base_cofins", base_cofins);
                    command.Parameters.AddWithValue("@pisv", pisv);
                    command.Parameters.AddWithValue("@cofinsv", cofinsv);
                    command.Parameters.AddWithValue("@total_produto", total_produto);
                    command.Parameters.AddWithValue("@CodigoNotaProdutor", CodigoNotaProdutor);
                    command.Parameters.AddWithValue("@numero_protocolo", numero_protocolo);
                    command.Parameters.AddWithValue("@finNFe", finNFe);
                    command.Parameters.AddWithValue("@IDBarra", System.Data.SqlTypes.SqlBinary.Null);
                    command.Parameters.AddWithValue("@Producao_NFe", Producao_NFe);
                    command.Parameters.AddWithValue("@vTotTrib", vTotTrib);
                    command.Parameters.AddWithValue("@emissao_hora", emissao_hora);
                    command.Parameters.AddWithValue("@data_hora", data_hora);
                    command.Parameters.AddWithValue("@vFCP", vFCP);
                    command.Parameters.AddWithValue("@vFCPST", vFCPST);
                    command.Parameters.AddWithValue("@tPag", tPag);
                    command.Parameters.AddWithValue("@vIPIDevol", vIPIDevol);
                    command.Parameters.AddWithValue("@usuario_alteracao", usuario_alteracao);
                    command.Parameters.AddWithValue("@vCredicmssn", vCredicmssn);
                    command.Parameters.AddWithValue("@ordem_compra", ordem_compra);
                    command.Parameters.AddWithValue("@Integrado", Integrado);
                    command.Parameters.AddWithValue("@indIntermed", indIntermed);
                    command.Parameters.AddWithValue("@intermedCnpj", intermedCnpj);
                    command.Parameters.AddWithValue("@idCadIntTran", idCadIntTran);
                    command.Parameters.AddWithValue("@CNPJPagamento", CNPJPagamento);
                    command.Parameters.AddWithValue("@DataHora_Lancamento", DateTime.Now);
                    command.Parameters.AddWithValue("@Validacao_Fiscal", Validacao_Fiscal);
                    command.ExecuteNonQuery();

                    foreach (NFItemDAORec item in Itens)
                    {
                        item.insert(conn, trans);
                        Funcoes.atualizaSaldoPLU(item.Filial, item.PLU, (item.Qtde * item.Embalagem), conn, trans, DateTime.Today, "EN");
                        Funcoes.atualizaSaldoPLUDia(item.Filial, item.PLU, (item.Qtde * item.Embalagem), conn, trans, "EN", DateTime.Today);
                    }

                    trans.Commit();

                }
                return true;
            }
            catch (Exception err)
            {
                trans.Rollback();
                throw err;
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