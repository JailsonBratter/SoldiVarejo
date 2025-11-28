using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using visualSysWeb.code;
namespace visualSysWeb.dao
{
    public class CargaDAO
    {
        public User usr = null;
        public String ean { get; set; }
        public String Filial { get; set; }
        public String plu { get; set; }
        public String Descricao { get; set; }
        public String Descricao_resumida { get; set; }
        public Decimal Preco { get; set; }
        public int pv { get; set; }
        public int pv_balanca { get; set; }
        public Decimal Preco_promocao { get; set; }
        public DateTime data_inicio { get; set; }

        public String data_inicioBr()
        {
            return dataBr(data_inicio);
        }
        public DateTime data_fim { get; set; }
        public String data_fimBr()
        {
            return dataBr(data_fim);
        }
        public bool Promocao_automatica { get; set; }
        public bool Promocao { get; set; }
        public String Tipo { get; set; }
        public String Peso_Variavel { get; set; }
        public String codigo_Grupo { get; set; }
        public String codigo_subGrupo { get; set; }
        public String Codigo_departamento { get; set; }
        public Decimal Tecla { get; set; }
        public Decimal Etiqueta { get; set; }
        public Decimal Validade { get; set; }
        public string erroCarregar = "";

        public int TipoCarga = 0;
        public int Alterados = 0;
        public int gerarBPreco = 0;
        public static String endTemp = @"C:\temp\temp_carga_";
        public String codCarga = "";
        public static void desmarcarAlterado()
        {
            Conexao.executarSql("UPDATE MERCADORIA SET estado_mercadoria=0 WHERE ESTADO_MERCADORIA =1");

        }

        public CargaDAO(User usr)
        {
            this.usr = usr;
            codCarga = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public void insereLinkServer(String enderecoLink, int Alterados, String Caixa)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" INSERT INTO  " + enderecoLink + ".[SOLDI_PDV].[dbo].[Produtos_Carga] ");
            strSql.Append(" ([ID_Carga],[ID_Produto],[CodigoBarras],[Descricao],[ID_Depto],[Preco],[Unidade],[Peso],[Pesavel]");
            strSql.Append(",[FatorConversao],[ID_Tributacao],[DataAlteracao],[DataCarga],[ICMS],[PercentualImposto],[Preco0]");
            strSql.Append(",[Preco1],[Preco2],[Preco3],[Preco4],[Preco5],[UN],[Origem],[CST_ICMS],[CFOP],[NCM],[Qtde_Tributaria]");
            strSql.Append(",[Carga_Tributaria_Municipal],[Carga_Tributaria_Estadual],[Carga_Tributaria_Federal],[CEST],[CST_PIS]");
            strSql.Append(",[CST_Cofins],[Aliquota_PIS],[Aliquota_Cofins],[Bebida_Alcoolica],[Preco_Atacado],[Margem_Atacado]");
            strSql.Append(",[Quantidade_Atacado],[Embalagem] ");
            strSql.Append(",[Promocao],[OfertaInicio],[OfertaFim],[PrecoPromocao], [ImpAux], [VendaComSenha] ,[terceiro_preco], [Inativo], [Ref_Fornecedor]");
            strSql.Append(")");

            strSql.Append("Select   '" + codCarga + "'");                                   //<ID_Carga, nvarchar(15),>
            strSql.Append(",convert(int, m.plu ) ");                                       //<ID_Produto, int,>
            strSql.Append(",isnull(ean.ean,m.plu)");                                        //--<CodigoBarras, nvarchar(17),>
            strSql.Append(", M.Descricao_resumida");                                        //<Descricao, nvarchar(50),>
            strSql.Append(",convert(int, substring(isnull(m.codigo_departamento,0),1,3))"); //<ID_Depto, int,>
            strSql.Append(" ,ml.preco ");                                                   //<Preco, float,>
            strSql.Append(",und	");                                                         //<Unidade, nvarchar(5),>
            strSql.Append(",peso");                                                         //<Peso, float,>
            strSql.Append(",pesov.codigo");                                                 //<Pesavel, int,>
            strSql.Append(", Fator_conversao ");                                            //<FatorConversao, float,>
            strSql.Append(", t.Nro_ecf ");                                                  //<ID_Tributacao, int,>
            strSql.Append(", ml.data_alteracao ");                                          //<DataAlteracao, datetime,>
            strSql.Append(", getdate()");                                                   //<DataCarga, datetime,>
            if (usr.filial.CRT.Equals("1"))
            {
                strSql.Append(",Saida_ICMS=0");                                                //<ICMS, float,>
            }
            else
            {
                strSql.Append(", t.Saida_ICMS");                                                //<ICMS, float,>
            }
            strSql.Append(", imp.aliquota_imposto ");                                       //<PercentualImposto, float,>
            strSql.Append(", ml.preco ");                                                   //<Preco0, float,>
            strSql.Append(",0 ");                                                           //<Preco1, float,>
            strSql.Append(",0 ");                                                           //<Preco2, float,>
            strSql.Append(",0 ");                                                           //<Preco3, float,>
            strSql.Append(",0 ");                                                           //<Preco4, float,>
            strSql.Append(",0 ");                                                           //<Preco5, float,>
            strSql.Append(", und");                                                         //<UN, varchar(2),>
            strSql.Append(", origem	");                                                     //<Origem, tinyint,>
            if (usr.filial.CRT.Equals("1"))
            {
                strSql.Append(", ltrim(rtrim(t.csosn)) ");                                  //<CST_ICMS, varchar(3),> CSOSN SIMPLES NACIONAL
                //strSql.Append(",case when ltrim(rtrim(t.csosn)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else 5102 end ");//<CFOP, numeric(5,0),>
                strSql.Append(",case when ltrim(rtrim(t.csosn)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else ");//<CFOP, numeric(5,0),>
                strSql.Append("CASE WHEN LEN(RTRIM(LTRIM(ISNULL(m.cfop,'')))) = 4 THEN M.CFOP ELSE CASE WHEN m.TIPO LIKE '%PRODUCAO%' THEN 5101 ELSE 5102 END END");
                strSql.Append(" end");
            }
            else
            {
                strSql.Append(", ltrim(rtrim(t.indice_st)) ");                                  //<CST_ICMS, varchar(3),>
                //strSql.Append(",case when ltrim(rtrim(t.indice_st)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else 5102 end ");//<CFOP, numeric(5,0),>
                strSql.Append(",case when ltrim(rtrim(t.indice_st)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else ");//<CFOP, numeric(5,0),>
                strSql.Append("CASE WHEN LEN(RTRIM(LTRIM(ISNULL(m.cfop,'')))) = 4 THEN M.CFOP ELSE CASE WHEN m.TIPO LIKE '%PRODUCAO%' THEN 5101 ELSE 5102 END END");
                strSql.Append(" end");
            }

            strSql.Append(", m.cf ");                                                       //<NCM, varchar(10),>
            strSql.Append(",1 ");                                                           //<Qtde_Tributaria, numeric(9,3),>
            strSql.Append(",0 ");                                                           //<Carga_Tributaria_Municipal, numeric(5,2),>
            strSql.Append(",0 ");                                                           //<Carga_Tributaria_Estadual, numeric(5,2),>
            strSql.Append(",0 ");                                                           //<Carga_Tributaria_Federal, numeric(5,2),>
            strSql.Append(", m.cest");                                                      //<CEST, numeric(7,0),>
            if (usr.filial.CRT.Equals("1"))
            {
                strSql.Append(", cst_pis= '" + usr.filial.cst_pis_cofins + "'");                                 //<CST_PIS, varchar(2),>
                strSql.Append(", cst_cofins= '" + usr.filial.cst_pis_cofins + "'");                              //<CST_Cofins, varchar(2),>
                strSql.Append(", pis_perc_saida =" + Funcoes.decimalPonto(usr.filial.pis.ToString()));        //<Aliquota_PIS, numeric(5,2),>
                strSql.Append(", cofins_perc_saida=	" + Funcoes.decimalPonto(usr.filial.cofins.ToString()));  //Aliquota_Cofins, numeric(5,2),>

            }
            else
            {
                strSql.Append(", m.cst_saida ");                                                //<CST_PIS, varchar(2),>
                strSql.Append(", m.cst_saida ");                                                //<CST_Cofins, varchar(2),>
                if (usr.filial.Reg_Federal.ToUpper().Equals("PRESUMIDO"))
                {
                    strSql.Append(", CASE WHEN M.CST_SAIDA = '01' THEN 0.65 ELSE m.pis_perc_saida END");   //<Aliquota_PIS, numeric(5,2),>
                    strSql.Append(", CASE WHEN M.CST_SAIDA = '01' THEN 3.00 ELSE m.cofins_perc_saida END");  //Aliquota_Cofins, numeric(5,2),>
                }
                else
                {
                    strSql.Append(", m.pis_perc_saida ");                                           //<Aliquota_PIS, numeric(5,2),>
                    strSql.Append(", m.cofins_perc_saida	");                                     //Aliquota_Cofins, numeric(5,2),>
                }
            }
            strSql.Append(", m.alcoolico ");                                                //<Bebida_Alcoolica, tinyint,>
            strSql.Append(", ml.preco_atacado ");                                           //<Preco_Atacado, float,>
            strSql.Append(", ml.margem_atacado ");                                          //<Margem_Atacado, float,>
            strSql.Append(", ml.qtde_atacado ");                                            //<Quantidade_Atacado, int,>
            strSql.Append(", m.embalagem ");                                                //<Embalagem, int,>
            strSql.Append(", ml.promocao ");                                                //<Promocao, tinyint,>
            strSql.Append(", ml.data_inicio ");                                             //<OfertaInicio, datetime,>
            strSql.Append(", ml.data_fim ");                                                //<OfertaFim, datetime,>
            strSql.Append(", ml.preco_promocao ");                                          //<PrecoPromocao, float,>
            strSql.Append(", m.ImpAux ");                                                   //<PrecoPromocao, float,> //Qdo item vendido no caixa, obriga o sistema a imprimir o cupom SAT
            strSql.Append(", m.Venda_Com_Senha ");                                          //<PrecoPromocao, float,> //Solicita senha antes de vender o produto
            strSql.Append(", m.terceiro_preco");                                          //<Terceiro_Preco, float,> //terceiro preco
            strSql.Append(", ISNULL(m.Inativo, 0)");
            strSql.Append(", ISNULL(m.Ref_Fornecedor, '')");

            strSql.Append(" from mercadoria as m ");
            strSql.Append("  inner join mercadoria_loja as ml on m.plu = ml.plu ");
            strSql.Append("  left join ean on m.plu = ean.plu ");
            strSql.Append("  inner join tributacao as t on m.filial = t.filial and m.codigo_tributacao = t.codigo_tributacao");
            strSql.Append("  inner join peso_variavel as pesov on rtrim(ltrim(m.peso_variavel)) = pesov.peso_variavel");
            strSql.Append("  inner join tipo  on Tipo.Tipo = m.Tipo");

            strSql.Append("  left join imposto_nota as imp on m.cf = imp.ncm ");
            strSql.Append(" where ");

            if (Alterados == 1)
            {
                strSql.Append("    isnull(estado_mercadoria,0) = " + Alterados + " and ");
            }

            //strSql.Append("   inativo = 0 ");
            strSql.Append("  ml.filial = '" + usr.getFilial() + "'");
            strSql.Append("  and tipo.Gera_carga =1 ");
            strSql.Append("  and ml.Preco >0 ");
            strSql.Append("  and ltrim(rtrim(m.descricao)) <>'' ");
            strSql.Append("  and ltrim(rtrim(m.Descricao_resumida)) <>'' ");


            String strSqlCaixa = "update Controle_Filial_PDV set Data_ult_atualizacao = getdate() where caixa = " + Caixa + " and Filial ='" + usr.getFilial() + "'";


            String strCargaPromocao = " insert into  " + enderecoLink + ".[SOLDI_PDV].[dbo].[promocao_carga](id_carga,codigo,tipo,inicio,fim,descricao,param_base,param_brinde) " +
                "Select id_carga = '" + codCarga + "',codigo,tipo,inicio,fim,descricao,param_base,param_brinde from promocao where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";

            strCargaPromocao += " insert into  " + enderecoLink + ".[SOLDI_PDV].[dbo].[promocao_base_carga](id_carga,codigo_promo,plu ) " +
                "Select id_carga = '" + codCarga + "',codigo_promo,plu from promocao_base  inner join promocao on promocao.codigo = codigo_promo  where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";

            strCargaPromocao += " insert into  " + enderecoLink + ".[SOLDI_PDV].[dbo].[promocao_brinde_carga](id_carga,codigo_promo,plu ) " +
                "Select id_carga = '" + codCarga + "',codigo_promo,plu from promocao_brinde  inner join promocao on promocao.codigo = codigo_promo where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";


            strCargaPromocao += " insert into  " + enderecoLink + ".[SOLDI_PDV].[dbo].[Tabela_Preco_carga](id_carga,Codigo_tabela, Filial, Nro_tabela, porc, PLU) " +
                "Select id_carga = '" + codCarga + "',Tabela_Preco.Codigo_tabela,Tabela_Preco.Filial,Tabela_Preco.Nro_tabela, Preco_Mercadoria.Desconto, Preco_Mercadoria.PLU FROM Tabela_Preco INNER JOIN " +
                "Preco_Mercadoria ON Tabela_Preco.Filial = Preco_Mercadoria.Filial AND Tabela_Preco.Codigo_tabela = Preco_Mercadoria.Codigo_tabela " +
            " INNER JOIN Mercadoria M ON m.PLU = Preco_Mercadoria.PLU " +
            " WHERE ISNULL(M.ESTADO_MERCADORIA, 0) = 1;";


            String strCargaConcluida = "insert into  " + enderecoLink + ".[SOLDI_PDV].[dbo].[carga] (Id_Carga) values('" + codCarga + "');";



            try
            {
                Conexao.executarSql(strSql.ToString());
                Conexao.executarSql(strSqlCaixa);
                Conexao.executarSql(strCargaPromocao);
                Conexao.executarSql(strCargaConcluida);
                Conexao.executarSql("insert into  carga_pdv (Id_Carga,caixa,status) values('" + codCarga + "'," + Caixa + ",1);");
            }
            catch (Exception err)
            {
                Conexao.executarSql("insert into  carga_pdv (Id_Carga,caixa,status) values('" + codCarga + "'," + Caixa + ",2);");
                throw err;
            }




        }

        public void insereServico(int Alterados, String Caixa)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                bool cargaIncluida = (Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from Produtos_Carga where ID_Carga='" + codCarga + "';", null)) > 0);
                if (!cargaIncluida)
                {


                    strSql.Append(" INSERT INTO [Produtos_Carga] ");
                    strSql.Append(" ([ID_Carga],[ID_Produto],[CodigoBarras],[Descricao],[ID_Depto],[Preco],[Unidade],[Peso],[Pesavel]");
                    strSql.Append(",[FatorConversao],[ID_Tributacao],[DataAlteracao],[DataCarga],[ICMS],[PercentualImposto],[Preco0]");
                    strSql.Append(",[Preco1],[Preco2],[Preco3],[Preco4],[Preco5],[UN],[Origem],[CST_ICMS],[CFOP],[NCM],[Qtde_Tributaria]");
                    strSql.Append(",[Carga_Tributaria_Municipal],[Carga_Tributaria_Estadual],[Carga_Tributaria_Federal],[CEST],[CST_PIS]");
                    strSql.Append(",[CST_Cofins],[Aliquota_PIS],[Aliquota_Cofins],[Bebida_Alcoolica],[Preco_Atacado],[Margem_Atacado]");
                    strSql.Append(",[Quantidade_Atacado],[Embalagem] ");
                    strSql.Append(",[Promocao],[OfertaInicio],[OfertaFim],[PrecoPromocao], [ImpAux], [VendaComSenha],[terceiro_preco], [Inativo], [Ref_Fornecedor] ");
                    strSql.Append(")");

                    strSql.Append("Select   '" + codCarga + "'");                                   //<ID_Carga, nvarchar(15),>
                    strSql.Append(",convert(int, m.plu ) ");                                       //<ID_Produto, int,>
                    strSql.Append(",isnull(ean.ean,m.plu)");                                        //--<CodigoBarras, nvarchar(17),>
                    strSql.Append(", M.Descricao_resumida");                                        //<Descricao, nvarchar(50),>
                    strSql.Append(",convert(int, substring(isnull(m.codigo_departamento,0),1,3))"); //<ID_Depto, int,>
                    strSql.Append(" ,ml.preco ");                                                   //<Preco, float,>
                    strSql.Append(",und	");                                                         //<Unidade, nvarchar(5),>
                    strSql.Append(",peso");                                                         //<Peso, float,>
                    strSql.Append(",pesov.codigo");                                                 //<Pesavel, int,>
                    strSql.Append(", Fator_conversao ");                                            //<FatorConversao, float,>
                    strSql.Append(", t.Nro_ecf ");                                                  //<ID_Tributacao, int,>
                    strSql.Append(", ml.data_alteracao ");                                          //<DataAlteracao, datetime,>
                    strSql.Append(", getdate()");                                                   //<DataCarga, datetime,>
                    if (usr.filial.CRT.Equals("1"))
                    {
                        strSql.Append(",Saida_ICMS=0");                                                //<ICMS, float,>
                    }
                    else
                    {
                        strSql.Append(", t.Saida_ICMS");                                                //<ICMS, float,>
                    }
                    strSql.Append(", imp.aliquota_imposto ");                                       //<PercentualImposto, float,>
                    strSql.Append(", ml.preco ");                                                   //<Preco0, float,>
                    strSql.Append(",0 ");                                                           //<Preco1, float,>
                    strSql.Append(",0 ");                                                           //<Preco2, float,>
                    strSql.Append(",0 ");                                                           //<Preco3, float,>
                    strSql.Append(",0 ");                                                           //<Preco4, float,>
                    strSql.Append(",0 ");                                                           //<Preco5, float,>
                    strSql.Append(", und");                                                         //<UN, varchar(2),>
                    strSql.Append(", origem	");                                                     //<Origem, tinyint,>
                    if (usr.filial.CRT.Equals("1"))
                    {
                        strSql.Append(", ltrim(rtrim(t.csosn)) ");                                  //<CST_ICMS, varchar(3),> CSOSN SIMPLES NACIONAL
                                                                                                    //strSql.Append(",case when ltrim(rtrim(t.csosn)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else 5102 end ");//<CFOP, numeric(5,0),>
                        strSql.Append(",case when ltrim(rtrim(t.csosn)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else ");//<CFOP, numeric(5,0),>
                        strSql.Append("CASE WHEN LEN(RTRIM(LTRIM(ISNULL(m.cfop,'')))) = 4 THEN M.CFOP ELSE CASE WHEN m.TIPO LIKE '%PRODUCAO%' THEN 5101 ELSE 5102 END END");
                        strSql.Append(" end");
                    }
                    else
                    {
                        strSql.Append(", ltrim(rtrim(t.indice_st)) ");                                  //<CST_ICMS, varchar(3),>
                                                                                                        //strSql.Append(",case when ltrim(rtrim(t.indice_st)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else 5102 end ");//<CFOP, numeric(5,0),>
                        strSql.Append(",case when ltrim(rtrim(t.csosn)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else ");//<CFOP, numeric(5,0),>
                        strSql.Append("CASE WHEN LEN(RTRIM(LTRIM(ISNULL(m.cfop,'')))) = 4 THEN M.CFOP ELSE CASE WHEN m.TIPO LIKE '%PRODUCAO%' THEN 5101 ELSE 5102 END END");
                        strSql.Append(" end");
                    }

                    strSql.Append(", m.cf ");                                                       //<NCM, varchar(10),>
                    strSql.Append(",1 ");                                                           //<Qtde_Tributaria, numeric(9,3),>
                    strSql.Append(",0 ");                                                           //<Carga_Tributaria_Municipal, numeric(5,2),>
                    strSql.Append(",0 ");                                                           //<Carga_Tributaria_Estadual, numeric(5,2),>
                    strSql.Append(",0 ");                                                           //<Carga_Tributaria_Federal, numeric(5,2),>
                    strSql.Append(", m.cest");                                                      //<CEST, numeric(7,0),>
                    if (usr.filial.CRT.Equals("1"))
                    {
                        strSql.Append(", cst_pis= '" + usr.filial.cst_pis_cofins + "'");                                 //<CST_PIS, varchar(2),>
                        strSql.Append(", cst_cofins= '" + usr.filial.cst_pis_cofins + "'");                              //<CST_Cofins, varchar(2),>
                        strSql.Append(", pis_perc_saida =" + Funcoes.decimalPonto(usr.filial.pis.ToString()));        //<Aliquota_PIS, numeric(5,2),>
                        strSql.Append(", cofins_perc_saida=	" + Funcoes.decimalPonto(usr.filial.cofins.ToString()));  //Aliquota_Cofins, numeric(5,2),>

                    }
                    else
                    {
                        strSql.Append(", m.cst_saida ");                                                //<CST_PIS, varchar(2),>
                        strSql.Append(", m.cst_saida ");                                                //<CST_Cofins, varchar(2),>
                        if (usr.filial.Reg_Federal.ToUpper().Equals("PRESUMIDO"))
                        {
                            strSql.Append(", CASE WHEN M.CST_SAIDA = '01' THEN 0.65 ELSE m.pis_perc_saida END");   //<Aliquota_PIS, numeric(5,2),>
                            strSql.Append(", CASE WHEN M.CST_SAIDA = '01' THEN 3.00 ELSE m.cofins_perc_saida END");  //Aliquota_Cofins, numeric(5,2),>
                        }
                        else
                        {
                            strSql.Append(", m.pis_perc_saida ");                                           //<Aliquota_PIS, numeric(5,2),>
                            strSql.Append(", m.cofins_perc_saida");                                     //Aliquota_Cofins, numeric(5,2),>
                        }
                    }
                    strSql.Append(", m.alcoolico ");                                                //<Bebida_Alcoolica, tinyint,>
                    strSql.Append(", ml.preco_atacado ");                                           //<Preco_Atacado, float,>
                    strSql.Append(", ml.margem_atacado ");                                          //<Margem_Atacado, float,>
                    strSql.Append(", ml.qtde_atacado ");                                            //<Quantidade_Atacado, int,>
                    strSql.Append(", m.embalagem ");                                                //<Embalagem, int,>
                    strSql.Append(", ml.promocao ");                                                //<Promocao, tinyint,>
                    strSql.Append(", ml.data_inicio ");                                             //<OfertaInicio, datetime,>
                    strSql.Append(", ml.data_fim ");                                                //<OfertaFim, datetime,>
                    strSql.Append(", ml.preco_promocao ");                                          //<PrecoPromocao, float,>
                    strSql.Append(", m.ImpAux ");                                                   //<PrecoPromocao, float,> //Qdo item vendido no caixa, obriga o sistema a imprimir o cupom SAT
                    strSql.Append(", m.Venda_Com_Senha ");                                          //<PrecoPromocao, float,> //Solicita senha antes de vender o produto
                    strSql.Append(", m.terceiro_preco");                                          //<Terceiro_Preco, float,> //terceiro preco
                    strSql.Append(", ISNULL(m.Inativo, 0)");
                    strSql.Append(", ISNULL(m.Ref_Fornecedor, '')");

                    strSql.Append(" from mercadoria as m ");
                    strSql.Append("  inner join mercadoria_loja as ml on m.plu = ml.plu ");
                    strSql.Append("  left join ean on m.plu = ean.plu ");
                    strSql.Append("  inner join tributacao as t on ML.FILIAL = t.filial AND  m.codigo_tributacao = t.codigo_tributacao");
                    strSql.Append("  inner join peso_variavel as pesov on rtrim(ltrim(m.peso_variavel)) = pesov.peso_variavel");
                    strSql.Append("  inner join tipo  on Tipo.Tipo = m.Tipo");

                    strSql.Append("  left join imposto_nota as imp on m.cf = imp.ncm ");
                    strSql.Append(" where ");

                    if (Alterados == 1)
                    {
                        strSql.Append("    isnull(estado_mercadoria,0) = " + Alterados + " and ");
                    }

                    //strSql.Append("   inativo = 0 ");
                    strSql.Append("  ml.filial = '" + usr.getFilial() + "'");
                    strSql.Append("  and tipo.Gera_carga =1 ");
                    strSql.Append("  and ml.Preco >0 ");
                    strSql.Append("  and ltrim(rtrim(m.descricao)) <>'' ");
                    strSql.Append("  and ltrim(rtrim(m.Descricao_resumida)) <>'' ");

                    String strCargaPromocao = " insert into  [promocao_carga](id_carga,codigo,tipo,inicio,fim,descricao,param_base,param_brinde) " +
                  "Select id_carga = '" + codCarga + "',codigo,tipo,inicio,fim,descricao,param_base,param_brinde from promocao where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";

                    strCargaPromocao += " insert into [promocao_base_carga](id_carga,codigo_promo,plu ) " +
                        "Select id_carga = '" + codCarga + "',codigo_promo,plu from promocao_base  inner join promocao on promocao.codigo = codigo_promo  where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";

                    strCargaPromocao += " insert into  [promocao_brinde_carga](id_carga,codigo_promo,plu ) " +
                        "Select id_carga = '" + codCarga + "',codigo_promo,plu from promocao_brinde  inner join promocao on promocao.codigo = codigo_promo where fim >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "';";


                    strCargaPromocao += " insert into  [Tabela_Preco_carga](id_carga,Codigo_tabela, Filial, Nro_tabela, porc, PLU) " +
                        "Select id_carga = '" + codCarga + "',Tabela_Preco.Codigo_tabela,Tabela_Preco.Filial,Tabela_Preco.Nro_tabela, Preco_Mercadoria.Desconto, Preco_Mercadoria.PLU FROM Tabela_Preco INNER JOIN " +
                        "Preco_Mercadoria ON Tabela_Preco.Filial = Preco_Mercadoria.Filial AND Tabela_Preco.Codigo_tabela = Preco_Mercadoria.Codigo_tabela " +
                    " INNER JOIN Mercadoria M ON m.PLU = Preco_Mercadoria.PLU " +
                    " WHERE ISNULL(M.ESTADO_MERCADORIA, 0) = 1;";

                    Conexao.executarSql(strCargaPromocao);
                    Conexao.executarSql(strSql.ToString());
                }

                String strSqlCaixa = "update Controle_Filial_PDV set Data_ult_atualizacao = getdate() where caixa = " + Caixa + " and Filial ='" + usr.getFilial() + "'";
                String strCargaConcluida = "insert into carga_pdv (Id_Carga,caixa,status) values('" + codCarga + "'," + Caixa + ",0);";


                Conexao.executarSql(strSqlCaixa);
                Conexao.executarSql(strCargaConcluida);
            }
            catch (Exception err)
            {
                Conexao.executarSql("insert into  carga_pdv (Id_Carga,caixa,status) values('" + codCarga + "'," + Caixa + ",2);");
                throw err;
            }
        }

        public CargaDAO(User usr, int TipoCarga, int Alterados)
        {

            this.usr = usr;
            this.TipoCarga = TipoCarga;
            this.Alterados = Alterados;

            /******************************************************************
             * Tipo de Arquivos Gerados na Carga Parametro TipoCarga
             * ****************************************************************
             * ESPECIFICOS PARA PDV´S
             * *****************************************************************
             *  0 - Arquivo 6.SDF Carga de PDV´s (Visual Shop 3.0 / Zanthus)
             * 10 - 10.SDF Cadastro de Clientes
             * ?? - Operadores/Gerentes de Caixa
             * ?? - Departamento
             * ?? - Finalizadoras
             * ?? - Tributação
             * *****************************************************************
             * BALANÇA ELETRÔNICA
             * *****************************************************************
             * 101 - Arquivo Toledo MGVIV
             * 102 - Arquivo Toledo MGV V
             * 103 - Arquivo Filizola Platina 15
             * 107 - Arquivo Toledo MGV+ Nutrição
             * 108 - Arquivo Sunnyvale 5600
             */
            SqlDataReader rs = null;
            Conexao.executarSql("execute sp_RetiraOferta");
            String sql = "";
            if (TipoCarga == 10)
            {
                sql = "Select * From w_br_cadastro_cliente_gera order by cast(codigo_cliente as decimal(16))";
            }
            else
            {
                sql = "EXECUTE sp_Cons_Cadastro_Mercadoria '" + usr.getFilial() + "', " + this.TipoCarga.ToString() + ", " + this.Alterados.ToString();
            }

            rs = Conexao.consulta(sql, usr, false);

            geraArquivo(rs);

        }

        public CargaDAO(User usr, int TipoCarga, int Alterados, bool desmarcarAlteracoes)
            : this(usr, TipoCarga, Alterados)
        {

            if (desmarcarAlteracoes)
            {
                Conexao.executarSql("UPDATE MERCADORIA SET estado_mercadoria=0 WHERE ESTADO_MERCADORIA =1");
            }
        }

        public CargaDAO(User usr, int TipoCarga, int Alterados, bool desmarcarAlteracoes, bool geraBPreco)
            : this(usr, TipoCarga, Alterados, desmarcarAlteracoes)
        {
            if (this.TipoCarga == 1 && this.Alterados != 1 && geraBPreco)
            {
                if (!System.IO.Directory.Exists(usr.filial.gerar_bpreco))
                {
                    System.IO.Directory.CreateDirectory(usr.filial.gerar_bpreco);
                }


                String endFinal = endTemp + usr.filial.diretorio_gera.Replace("\\", "_").Replace(":", "") + "_6.sdf";

                System.IO.File.Copy(endFinal, usr.filial.gerar_bpreco + "\\6.sdf", true);

            }



        }

        public CargaDAO(User usr, int TipoCarga, int Alterados, bool desmarcarAlteracoes, bool geraBPreco, bool gerarOperadores)
            : this(usr, TipoCarga, Alterados, desmarcarAlteracoes, geraBPreco)
        {

            if (gerarOperadores)
            {
                gerarArquivoOperadores(usr);
            }


        }
        public CargaDAO(User usr, int TipoCarga, int Alterados, bool desmarcarAlteracoes, bool geraBPreco, bool gerarOperadores, bool gerarTabelaPreco)
            : this(usr, TipoCarga, Alterados, desmarcarAlteracoes, geraBPreco, gerarOperadores)
        {

            if (gerarTabelaPreco)
            {
                gerarArquivoTabPreco((Alterados == 1));
            }


            limpaTemp();
        }


        public static void limpaTemp()
        {
            string[] txtList = Directory.GetFiles(@"C:\temp\", "*temp_carga*");
            foreach (string item in txtList)
            {
                File.Delete(item);
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

        public static void gerarArquivoOperadores(User usr)
        {
            SqlDataReader rsOp = null;
            StringBuilder strOP = new StringBuilder();
            try
            {
                String sql = "Select * from operadores ";

                rsOp = Conexao.consulta(sql, usr, false);
                while (rsOp.Read())
                {
                    strOP.Append(rsOp["id_operador"].ToString().PadLeft(4, '0'));  //P01  Tam 04 - Id Operador  
                    strOP.Append(rsOp["Nome"].ToString().PadRight(40, ' '));      //P05  Tam 40 - Nome Operador 
                    strOP.Append(rsOp["Senha"].ToString().PadRight(40, ' '));     //P45  Tam 40 - Senha 
                    strOP.Append(rsOp["ID_NivelAcesso"].ToString());              //P85  Tam 01 - Nivel de Acesso
                    strOP.Append(rsOp["Cargo"].ToString().PadRight(60, ' '));     //P86  Tam 60 - Cargo
                    strOP.Append(rsOp["OpCaixa"].ToString().Equals("1") ? "1" : "0");//P146 Tam 01 - OpCaixa
                    strOP.Append(rsOp["inativo"].ToString().Equals("1") ? "1" : "0"); //P147 Tam 01 - Inativo

                    strOP.AppendLine();
                }

                if (strOP.Length > 0)
                {

                    String endereco = usr.filial.diretorio_gera + "\\OP.TXT";
                    String endFinal = endTemp + endereco.Replace("\\", "_").Replace(":", "");


                    StreamWriter valor = new StreamWriter(endFinal, false, Encoding.ASCII);
                    valor.Write(strOP.ToString());
                    valor.Close();

                    System.IO.File.Copy(endFinal, endereco, true);

                }



            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsOp != null)
                    rsOp.Close();

            }
        }

        private void gerarArquivoTabPreco(bool alterados)
        {
            SqlDataReader rsPreco = null;
            StringBuilder strPreco = new StringBuilder();
            try
            {
                String sql = "Select tp.Nro_tabela,pm.PLU,pm.Desconto from Preco_Mercadoria as pm " +
                               " inner join tabela_preco as tp on pm.Codigo_tabela = tp.Codigo_tabela " +
                               " inner join mercadoria as m on pm.plu=m.plu" +
                             " where isnull(m.Inativo,0) =0 ";

                if (alterados)
                {
                    sql += " and  m.ESTADO_MERCADORIA =1 ";
                }

                rsPreco = Conexao.consulta(sql, usr, false);
                while (rsPreco.Read())
                {
                    strPreco.Append(rsPreco["Nro_tabela"].ToString().PadLeft(4, '0'));      //P01  Tam 04 - Nro Tabela
                    strPreco.Append(rsPreco["PLU"].ToString().PadLeft(13, '0'));            //P05  Tam 13 - PLU 
                    strPreco.Append(rsPreco["DESCONTO"].ToString()
                                            .Replace(",", "")
                                            .Replace(".", "").PadLeft(10, '0'));             //P18  Tam 10 - DESCONTO

                    strPreco.AppendLine();
                }

                if (strPreco.Length > 0)
                {

                    String endereco = usr.filial.diretorio_gera + "\\TBPRECO.TXT";
                    String endFinal = endTemp + endereco.Replace("\\", "_").Replace(":", "");


                    StreamWriter valor = new StreamWriter(endFinal, false, Encoding.ASCII);
                    valor.Write(strPreco.ToString());
                    valor.Close();

                    System.IO.File.Copy(endFinal, endereco, true);

                }



            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsPreco != null)
                    rsPreco.Close();

            }
        }



        private void gerarArquivoInformacaoNutricional()
        {
            SqlDataReader rsNutri = null;
            StringBuilder strNutri = new StringBuilder();
            try
            {
                String sql = "Select * from mercadoria where porcao >0";

                rsNutri = Conexao.consulta(sql, usr, false);
                while (rsNutri.Read())
                {

                    strNutri.Append("N");                                                           //Tam 1 - Indicador nova Informação nutricional, sempre 'N'  
                    strNutri.Append(rsNutri["PLU"].ToString().PadLeft(6, '0'));                     //Tam 6 - Código da Informação Nutricional 
                    strNutri.Append("0");                                                           //Tam 1 - Reservado 

                    Decimal vPorc = 0;
                    Decimal.TryParse(rsNutri["Porcao"].ToString(), out vPorc);
                    strNutri.Append(vPorc.ToString("N0").PadLeft(3, '0'));                           //Tam 3 - Quantidade 

                    strNutri.Append(rsNutri["Porcao_medida"].ToString());                           //Tam 1 - Unidade da Porção 

                    Decimal vPorcNumero = 0;
                    Decimal.TryParse(rsNutri["Porcao_Numero"].ToString(), out vPorcNumero);
                    strNutri.Append(vPorcNumero.ToString("N0").PadLeft(2, '0'));                     //Tam 2 - Parte Inteira da Medida Caseira 

                    strNutri.Append(rsNutri["Porcao_div"].ToString());                              //Tam 1 - Parte Decimal da Medida Caseira
                    strNutri.Append(rsNutri["Porcao_detalhe"].ToString().Trim().PadLeft(2, '0'));    //Tam 2 - Medida Caseira Utilizada

                    Decimal vVlrEnerg = 0;
                    Decimal.TryParse(rsNutri["vlr_energ_qtde"].ToString(), out vVlrEnerg);
                    strNutri.Append(vVlrEnerg.ToString("N0").PadLeft(4, '0'));                      //Tam 4 - Valor Energético 


                    Decimal vCarboidratos = 0;
                    Decimal.TryParse(rsNutri["carboidratos_qtde"].ToString(), out vCarboidratos);
                    strNutri.Append(vCarboidratos.ToString("N1").Replace(",", "").PadLeft(4, '0')); //Tam 4 - Carboidratos 

                    Decimal vProteinas = 0;
                    Decimal.TryParse(rsNutri["proteinas_qtde"].ToString(), out vProteinas);
                    strNutri.Append(vProteinas.ToString("N1").Replace(",", "").PadLeft(3, '0'));    //Tam 3 - Proteinas

                    Decimal vGordTotais = 0;
                    Decimal.TryParse(rsNutri["gorduras_totais_qtde"].ToString(), out vGordTotais);
                    strNutri.Append(vGordTotais.ToString("N1").Replace(",", "").PadLeft(3, '0'));   //Tam 3 - Gorduras Totais

                    Decimal vGordSatu = 0;
                    Decimal.TryParse(rsNutri["gorduras_satu_qtde"].ToString(), out vGordSatu);
                    strNutri.Append(vGordSatu.ToString("N1").Replace(",", "").PadLeft(3, '0'));     //Tam 3 - Gorduras Saturadas

                    Decimal vGordTrans = 0;
                    Decimal.TryParse(rsNutri["gorduras_trans_qtde"].ToString(), out vGordTrans);
                    strNutri.Append(vGordTrans.ToString("N1").Replace(",", "").PadLeft(3, '0'));    //Tam 3 - Gorduras Trans

                    Decimal vFibra = 0;
                    Decimal.TryParse(rsNutri["fibra_alimen_qtde"].ToString(), out vFibra);
                    strNutri.Append(vFibra.ToString("N1").Replace(",", "").PadLeft(3, '0'));        //Tam 3 - Fibra Alimentar


                    Decimal vSodio = 0;
                    Decimal.TryParse(rsNutri["sodio_qtde"].ToString(), out vSodio);
                    strNutri.Append(vSodio.ToString("N1").Replace(",", "").PadLeft(5, '0'));        //Tam 5 - Sodio
                    strNutri.AppendLine("");

                }




                if (strNutri.Length > 0)
                {
                    String endereco = usr.filial.diretorio_balanca + "\\Infnutri.txt";

                    String endFinal = endTemp + endereco.Replace("\\", "_").Replace(":", "");

                    StreamWriter valor = new StreamWriter(endFinal, false, Encoding.ASCII);
                    valor.Write(strNutri.ToString());
                    valor.Close();

                    System.IO.File.Copy(endFinal, endereco, true);

                }



            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsNutri != null)
                    rsNutri.Close();

            }
        }

        public void geraArquivo(SqlDataReader rs)
        {

            StringBuilder strLinhaReg = new StringBuilder();

            StringBuilder strLinhaReg2 = new StringBuilder();

            string endereco = "";
            string endereco2 = "";

            if (!Directory.Exists(@"C:\temp"))
            {
                try
                {
                    Directory.CreateDirectory(@"C:\temp");

                }
                catch (Exception)
                {

                    throw new Exception("Não Foi encontrado a Pasta C:/temp e o sistema não tem permissão para criar, Crie a pasta e execute novamente a operação!");
                }

            }



            if (this.TipoCarga == 101) //Balança Toledo MGVIV
            {



                endereco = usr.filial.diretorio_balanca + "\\itens.txt";


                String tipo = "";
                while (rs.Read())
                {

                    strLinhaReg.Append("01");
                    strLinhaReg.Append("00");
                    strLinhaReg.Append(rs["PV_Balanca"].ToString().PadLeft(2, '0'));
                    strLinhaReg.Append(rs["PLU"].ToString().PadLeft(13, '0'));
                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));

                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                        {
                            strLinhaReg.Append(rs["Preco_promocao"].ToString().Replace(",", "").PadLeft(6, '0'));
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));

                        }
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));
                    }
                    strLinhaReg.Append(rs["Validade"].ToString().PadLeft(3, '0'));
                    strLinhaReg.Append(Funcoes.RemoverAcentos(rs["Descricao"].ToString()).PadRight(25, ' '));

                    Decimal vPorc = 0;

                    Decimal.TryParse(rs["Porcao"].ToString(), out vPorc);
                    if (vPorc > 0)
                    {

                        strLinhaReg.Append("".ToString().PadRight(25, ' '));
                        strLinhaReg.Append("".ToString().PadRight(10, '0'));
                        strLinhaReg.Append(rs["Cod_plu"].ToString().PadLeft(6, '0'));
                        strLinhaReg.AppendLine("".ToString().PadRight(199, ' '));

                    }
                    else
                    {
                        strLinhaReg.AppendLine("".ToString().PadRight(250, ' '));
                    }
                }
            }
            else if (this.TipoCarga == 102) //Balança Toledo MGV V
            {

                endereco = usr.filial.diretorio_balanca + "\\txitens.txt";

                while (rs.Read())
                {

                    strLinhaReg.Append("01");
                    strLinhaReg.Append("00");
                    int pv = int.Parse(rs["PV_Balanca"].ToString()) - 1;
                    strLinhaReg.Append(pv.ToString());
                    strLinhaReg.Append(rs["PLU"].ToString().PadLeft(6, '0'));
                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));

                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                        {
                            strLinhaReg.Append(rs["Preco_promocao"].ToString().Replace(",", "").PadLeft(6, '0'));
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));

                        }
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));
                    }
                    strLinhaReg.Append(rs["Validade"].ToString().PadLeft(3, '0'));

                    String strDescricao = Funcoes.RemoverAcentos(rs["Descricao"].ToString());

                    if (strDescricao.Length > 25)
                    {
                        strLinhaReg.Append(strDescricao.Substring(0, 25));
                    }
                    else
                    {
                        strLinhaReg.Append(strDescricao.PadRight(25, ' '));
                    }




                    strLinhaReg.AppendLine("".ToString().PadRight(234, ' '));



                }
            }
            else if (this.TipoCarga == 107)
            {
                gerarArquivoInformacaoNutricional();
                endereco = usr.filial.diretorio_balanca + "\\Itensmgv.txt";
                String tipo = "";
                while (rs.Read())
                {

                    strLinhaReg.Append("01");
                    //strLinhaReg.Append("00");
                    int pv = int.Parse(rs["PV_Balanca"].ToString()) - 1;
                    strLinhaReg.Append(pv.ToString());
                    strLinhaReg.Append(rs["PLU"].ToString().PadLeft(6, '0'));
                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));

                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                        {
                            strLinhaReg.Append(rs["Preco_promocao"].ToString().Replace(",", "").PadLeft(6, '0'));
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));

                        }
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(6, '0'));
                    }
                    strLinhaReg.Append(rs["Validade"].ToString().PadLeft(3, '0'));

                    String strDescricao = Funcoes.RemoverAcentos(rs["Descricao"].ToString());

                    if (strDescricao.Length > 25)
                    {
                        strLinhaReg.Append(strDescricao.Substring(0, 25));
                    }
                    else
                    {
                        strLinhaReg.Append(strDescricao.PadRight(25, ' '));
                    }

                    strLinhaReg.Append("".ToString().PadRight(25, ' '));
                    strLinhaReg.Append("".ToString().PadRight(10, '0'));


                    Decimal vPorc = 0;

                    Decimal.TryParse(rs["Porcao"].ToString(), out vPorc);

                    if (vPorc > 0)
                    {


                        strLinhaReg.Append(rs["Cod_plu"].ToString().PadLeft(6, '0'));

                    }
                    else
                    {
                        strLinhaReg.Append("0".PadLeft(6, '0'));
                    }
                    strLinhaReg.Append("1"); //Impressão data Validade
                    strLinhaReg.Append("1"); //impressao data Embalagem
                    strLinhaReg.Append("0000"); //Cod Fornecedor
                    strLinhaReg.Append("0".PadLeft(12, '0')); //Lote
                    strLinhaReg.Append("".ToString().PadRight(12, ' '));
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod do Som
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Tab Pre determinada
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod do Fracionador
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Extra 1
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Extra 2
                    strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Conservação
                    strLinhaReg.Append("".ToString().PadRight(12, ' '));
                    strLinhaReg.Append("0".PadLeft(6, '0')); //	Percentual de Glaciamento


                    strLinhaReg.AppendLine("".ToString().PadRight(86, ' '));

                }

            }
            else if (this.TipoCarga == 108) //Sunnyvale 5600
            {
                endereco = usr.filial.diretorio_balanca + "\\PLU0DALL.CSV";
                String tipo = "";
                while (rs.Read())
                {
                    strLinhaReg.Append(DateTime.Now.ToString("yyyyMMdd") + ","); //Data
                    strLinhaReg.Append(",0,"); //vazio e 0
                    strLinhaReg.Append(rs["PLU"].ToString() + ","); //Código PLU
                    strLinhaReg.Append("0,");
                    strLinhaReg.Append("2" + int.Parse(rs["PLU"].ToString()).ToString().PadLeft(4, '0') + "00000009" + ",");
                    strLinhaReg.Append("6,22,"); //Ainda não definido
                    String strDescricao = Funcoes.RemoverAcentos(rs["Descricao"].ToString());
                    if (strDescricao.Length > 25)
                    {
                        strLinhaReg.Append(strDescricao.Substring(0, 25) + ",");
                    }
                    else
                    {
                        strLinhaReg.Append(strDescricao + ",");
                    }
                    strLinhaReg.Append("0,,0,,0,,0,,0,"); //Valores fixos

                    //strLinhaReg.Append("00");
                    int pv = int.Parse(rs["PV_Balanca"].ToString()) - 1;
                    strLinhaReg.Append(pv.ToString() + ","); //Peso ou unidade
                    strLinhaReg.Append("0,");
                    //Preço
                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));

                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                        {
                            strLinhaReg.Append(rs["Preco_promocao"].ToString().Replace(",", "").Replace(".", "") + ",");
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").Replace(".", "") + ",");

                        }
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").Replace(".", "") + ",");
                    }
                    strLinhaReg.Append("0,0,0,0,0,1,,0,0,"); //Colunas 22 23 24 25 26 27
                    strLinhaReg.Append((Funcoes.intTry(rs["Validade"].ToString()) > 0 ? 1 : 0).ToString() + ","); //31
                    strLinhaReg.Append(Funcoes.intTry(rs["Validade"].ToString()).ToString() + ","); //32
                    strLinhaReg.Append("0,"); //33
                    strLinhaReg.Append(","); //34
                    strLinhaReg.Append(","); //35
                    strLinhaReg.Append("0,"); //36
                    strLinhaReg.Append("1,"); //37
                    strLinhaReg.Append("0,"); //38
                    strLinhaReg.Append("0,"); //39
                    strLinhaReg.Append("0,"); //40
                    strLinhaReg.Append("0,"); //41
                    strLinhaReg.Append("0,"); //42
                    strLinhaReg.Append("0,"); //43
                    strLinhaReg.Append("0,"); //44
                    strLinhaReg.Append("0,"); //45
                    strLinhaReg.Append("0,"); //46
                    strLinhaReg.Append("0,"); //47
                    string codPrecfixo = "";
                    if (rs["pv_balanca"].ToString().Equals("2"))
                        codPrecfixo = "1";
                    else
                        codPrecfixo = "8";

                    strLinhaReg.Append(codPrecfixo + ","); //48  preco fixo
                    strLinhaReg.Append("0,"); //49
                    strLinhaReg.Append("0,"); //50
                    strLinhaReg.Append(rs["bandeja"].ToString() + ","); //51 **confirmar
                    strLinhaReg.Append("0,"); //52
                    strLinhaReg.Append("0,"); //53
                    strLinhaReg.Append(",".PadRight(3, ',')); //54 a 56
                    strLinhaReg.Append("0,"); //57
                    strLinhaReg.Append("0,"); //58
                    strLinhaReg.Append("0,"); //59
                    strLinhaReg.Append(","); //60
                    strLinhaReg.Append("1,"); //61
                    strLinhaReg.Append(",".PadRight(5, ',')); //62 a 66
                    strLinhaReg.Append("0,"); //67
                    strLinhaReg.Append(",".PadRight(4, ',')); //68 a 71
                    strLinhaReg.Append("0,"); //72
                    strLinhaReg.Append("0,"); //73
                    strLinhaReg.Append("0,"); //74
                    strLinhaReg.Append("0,"); //75
                    strLinhaReg.Append(",".PadRight(11, ',')); //76 a 86

                    strLinhaReg.Append("0,"); //87
                    strLinhaReg.Append("0,"); //88
                    strLinhaReg.Append("0,"); //89
                    strLinhaReg.Append("0,"); //90
                    strLinhaReg.Append("0,"); //91
                    strLinhaReg.Append("0,"); //92
                    strLinhaReg.Append(","); //93
                    strLinhaReg.Append(","); //94
                    strLinhaReg.Append("0,"); //95
                    strLinhaReg.Append("0,"); //96
                    strLinhaReg.Append("0,"); //97
                    strLinhaReg.Append("0,"); //98
                    strLinhaReg.Append("0,"); //99
                    strLinhaReg.Append(",".PadRight(6, ',')); //100 a 105
                    strLinhaReg.Append("0,"); //106
                    strLinhaReg.Append(","); //107
                    strLinhaReg.Append("0,"); //108
                    strLinhaReg.Append("0,"); //109
                    strLinhaReg.Append(",".PadRight(7, ',')); //110 a 116

                    strLinhaReg.Append("0,"); //117
                    strLinhaReg.Append("0,"); //118
                    strLinhaReg.Append("0,"); //119
                    strLinhaReg.Append("0,"); //120
                    strLinhaReg.Append("0,"); //121
                    strLinhaReg.Append("0,"); //122
                    strLinhaReg.Append(",".PadRight(9, ',')); //123 a 131
                    strLinhaReg.Append("0,"); //132
                    strLinhaReg.Append("0,"); //133
                    strLinhaReg.Append("0,"); //134
                    strLinhaReg.Append("0,"); //135
                    strLinhaReg.Append(",".PadRight(3, ',')); //136 a 138

                    strLinhaReg.Append("0,"); //139
                    strLinhaReg.Append("0,"); //140
                    strLinhaReg.Append(","); //141
                    strLinhaReg.Append("0,"); //142
                    strLinhaReg.Append(","); //143
                    strLinhaReg.Append("0,"); //144
                    strLinhaReg.Append(","); //145
                    strLinhaReg.Append("0,"); //146
                    strLinhaReg.Append(","); //147
                    strLinhaReg.Append("0,"); //148
                    strLinhaReg.Append(",".PadRight(4, ',')); //149 a 152
                    strLinhaReg.Append("0,"); //153
                    strLinhaReg.Append(",".PadRight(6, ',')); //154 a 159
                    strLinhaReg.Append("0,"); //160
                    strLinhaReg.Append(",".PadRight(5, ',')); //161 a 165
                    strLinhaReg.Append("0,"); //166
                    strLinhaReg.Append("0,"); //167
                    strLinhaReg.Append(",".PadRight(5, ',')); //168 a 172
                    strLinhaReg.Append("0,"); //173
                    strLinhaReg.Append("0,"); //174
                    strLinhaReg.Append("0,"); //175
                    strLinhaReg.Append("0,"); //176
                    strLinhaReg.Append("0,"); //177
                    strLinhaReg.Append("0,"); //178
                    strLinhaReg.Append("0,"); //179
                    strLinhaReg.Append("0,"); //180
                    strLinhaReg.Append("0,"); //181
                    strLinhaReg.Append("0,"); //182
                    strLinhaReg.Append(",".PadRight(10, ',')); //183 a 192
                    strLinhaReg.Append("0,"); //193
                    strLinhaReg.Append("0,"); //194
                    strLinhaReg.Append(",".PadRight(4, ',')); //195 a 198
                    strLinhaReg.Append("0,"); //199
                    strLinhaReg.Append(","); //200
                    strLinhaReg.Append(","); //201
                    strLinhaReg.Append("0,"); //202
                    strLinhaReg.Append(","); //203
                    strLinhaReg.Append(","); //204
                    strLinhaReg.Append(","); //205
                    strLinhaReg.Append("0,"); //206
                    strLinhaReg.Append("0,"); //207
                    strLinhaReg.Append("0,"); //208
                    strLinhaReg.Append(","); //209
                    strLinhaReg.Append("0,"); //210
                    strLinhaReg.Append(","); //211
                    strLinhaReg.Append("0,"); //212
                    strLinhaReg.Append("0,"); //213
                    strLinhaReg.Append("0,"); //214
                    strLinhaReg.Append("0,"); //215
                    strLinhaReg.Append(","); //216
                    strLinhaReg.Append(","); //217
                    strLinhaReg.Append("0,"); //218
                    strLinhaReg.Append("0,"); //219
                    strLinhaReg.Append("0,"); //220
                    strLinhaReg.Append(","); //221
                    strLinhaReg.Append("51,"); //222
                    strLinhaReg.Append(",".PadRight(11, ',')); //223 a 233
                    strLinhaReg.Append("0,"); //234
                    strLinhaReg.Append("0,"); //235
                    strLinhaReg.Append("0,"); //236
                    strLinhaReg.Append("0,"); //237
                    strLinhaReg.Append("0,"); //238
                    strLinhaReg.Append(",".PadRight(154, ',')); //239 a 392
                    strLinhaReg.Append("0,"); //393
                    strLinhaReg.Append("0,"); //394
                    strLinhaReg.Append(","); //395
                    strLinhaReg.Append("0,"); //396
                    strLinhaReg.Append(","); //397
                    strLinhaReg.Append("0,"); //398
                    strLinhaReg.Append("0,"); //399
                    strLinhaReg.Append("0,"); //400
                    strLinhaReg.Append("0,"); //401
                    strLinhaReg.Append("0,"); //402
                    strLinhaReg.Append(""); //403








                    //strLinhaReg.Append("".ToString().PadRight(25, ' '));
                    //strLinhaReg.Append("".ToString().PadRight(10, '0'));


                    //Decimal vPorc = 0;

                    //Decimal.TryParse(rs["Porcao"].ToString(), out vPorc);

                    //if (vPorc > 0)
                    //{


                    //    strLinhaReg.Append(rs["Cod_plu"].ToString().PadLeft(6, '0'));

                    //}
                    //else
                    //{
                    //    strLinhaReg.Append("0".PadLeft(6, '0'));
                    //}
                    //strLinhaReg.Append("1"); //Impressão data Validade
                    //strLinhaReg.Append("1"); //impressao data Embalagem
                    //strLinhaReg.Append("0000"); //Cod Fornecedor
                    //strLinhaReg.Append("0".PadLeft(12, '0')); //Lote
                    //strLinhaReg.Append("".ToString().PadRight(12, ' '));
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod do Som
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Tab Pre determinada
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod do Fracionador
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Extra 1
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Extra 2
                    //strLinhaReg.Append("0".PadLeft(4, '0')); //Cod Conservação
                    //strLinhaReg.Append("".ToString().PadRight(12, ' '));
                    //strLinhaReg.Append("0".PadLeft(6, '0')); //	Percentual de Glaciamento


                    strLinhaReg.AppendLine("".ToString().PadRight(86, ' '));

                }

            }
            else if (this.TipoCarga == 103)
            {
                endereco = usr.filial.diretorio_balanca + "\\CADTXT.txt";
                endereco2 = usr.filial.diretorio_balanca + "\\SETORTXT.txt";
                String strCodGrupo = "";
                String strDescricaoGrupo = "";
                int contitens = 1;


                while (rs.Read())
                {
                    //================CADTXT.txt
                    strLinhaReg.Append(rs["plu"].ToString().PadLeft(6, '0'));
                    strLinhaReg.Append(rs["pv_balanca"].ToString().Equals("1") ? "P" : "U");
                    String descricao = Funcoes.RemoverAcentos(rs["descricao"].ToString());

                    if (descricao.Length > 22)
                        strLinhaReg.Append(descricao.Substring(0, 22));
                    else
                        strLinhaReg.Append(descricao.PadRight(22, ' '));



                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));
                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                        {
                            strLinhaReg.Append(rs["Preco_promocao"].ToString().Replace(",", "").PadLeft(7, '0'));
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(7, '0'));

                        }

                    }
                    else
                    {

                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(7, '0'));

                    }


                    if (rs["validade"].ToString().Trim().Equals(""))
                    {
                        strLinhaReg.Append("000");
                    }
                    else
                    {
                        strLinhaReg.Append(rs["validade"].ToString().Trim().PadLeft(3, '0'));
                    }
                    strLinhaReg.AppendLine();

                    // ========== SETORTXT.txt";


                    if (!strCodGrupo.Equals(rs["codigo_grupo"].ToString()))
                    {
                        strCodGrupo = rs["codigo_grupo"].ToString();
                        strDescricaoGrupo = Conexao.retornaUmValor("select descricao_grupo from grupo where codigo_grupo =" + strCodGrupo, new User());
                    }
                    if (strDescricaoGrupo.Length > 12)
                        strDescricaoGrupo = strDescricaoGrupo.Substring(0, 12);


                    strLinhaReg2.Append(Funcoes.RemoverAcentos(strDescricaoGrupo).PadRight(12, ' '));
                    strLinhaReg2.Append(rs["plu"].ToString().PadLeft(6, '0'));
                    strLinhaReg2.Append(contitens.ToString().ToString().PadLeft(4, '0'));
                    strLinhaReg2.Append(""); // tecla

                    contitens++;
                    strLinhaReg2.AppendLine();

                }





            }
            else if (this.TipoCarga == 0 || this.TipoCarga == 1) //PDV [Visual Shop ou PDV Zanthus]
            {
                if (Alterados == 0)
                    endereco = usr.filial.diretorio_gera + "\\6.SDF";
                else
                    endereco = usr.filial.diretorio_gera + "\\ALTERA6.SDF";




                while (rs.Read())
                {
                    strLinhaReg.Append(DateTime.Today.ToString("ddMMyy"));//1
                    strLinhaReg.Append("000");//7
                    strLinhaReg.Append(rs["PLU"].ToString().PadLeft(17, '0'));//10
                    bool bCEST = Funcoes.valorParametro("CEST_DESC_PDV", usr).ToUpper().Equals("TRUE");

                    String strDescricao = "";
                    if (bCEST)
                    {
                        strDescricao = rs["CEST"].ToString().Trim();
                        if (strDescricao.Length > 0 || rs["NCM"].ToString().Trim().Length > 0)
                            strDescricao += "#" + rs["NCM"].ToString().Trim() + " ";


                        strDescricao += Funcoes.RemoverAcentos(rs["Descricao_Resumida"].ToString().Trim());
                    }
                    else
                    {
                        strDescricao = Funcoes.RemoverAcentos(rs["Descricao_Resumida"].ToString());
                    }


                    if (strDescricao.Length > 44)
                        strDescricao = strDescricao.Substring(0, 43);

                    strLinhaReg.Append(strDescricao.PadRight(44, ' '));//27 
                    if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                    {
                        TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                        TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));
                        //if (DateTime.Parse(rs["Data_Inicio"].ToString()).ToString("yyyyMMdd") <= DateTime.Now.ToString("yyyyMMdd"))
                        if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                        // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                        {
                            strLinhaReg.Append(rs["Preco_Promocao"].ToString().Replace(",", "").PadLeft(11, '0'));//71
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(11, '0'));//71
                        }
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "").PadLeft(11, '0'));//71
                    }

                    strLinhaReg.Append(rs["Codigo_Grupo"].ToString().PadLeft(3, '0'));//82
                    strLinhaReg.Append("0");//85
                    strLinhaReg.Append(rs["Nro_ECF"].ToString().PadLeft(2, '0'));//86
                    strLinhaReg.Append("".ToString().PadLeft(36, '0'));//88
                    strLinhaReg.Append((rs["pv"].ToString().Equals("1") ? "0" : "1")); //124 Venda Fracionaria
                    strLinhaReg.Append("000"); //125 Inativo 
                    strLinhaReg.Append("00000");//128
                    strLinhaReg.Append(rs["PV"].ToString());//133
                    if (TipoCarga == 0)
                    {
                        strLinhaReg.Append(rs["Preco_atacado"].ToString().Replace(",", "").PadLeft(12, '0'));//134
                        strLinhaReg.Append(rs["qtde_atacado"].ToString().Replace(",", "").PadLeft(9, '0'));//146
                        strLinhaReg.Append(rs["embalagem"].ToString().Replace(",", "").PadLeft(5, '0'));//155
                        strLinhaReg.Append("".ToString().PadLeft(138, '0'));//160

                    }
                    else
                    {

                        strLinhaReg.Append("".ToString().PadLeft(164, '0'));//134
                    }

                    strLinhaReg.Append("".PadLeft(11, '0'));//298
                    strLinhaReg.Append("".PadLeft(11, '0'));//309
                    strLinhaReg.Append("".PadLeft(11, '0'));//320
                    strLinhaReg.Append("01");//331
                    strLinhaReg.Append("".ToString().PadLeft(110, '0'));//333
                    if (rs["NCM"].ToString().Trim().Length > 9)
                    {
                        strLinhaReg.Append("".PadLeft(9, '0'));//443 NCM 
                    }
                    else
                    {
                        strLinhaReg.Append(rs["NCM"].ToString().Trim().PadLeft(9, '0'));//443 NCM 
                    }

                    strLinhaReg.Append("".ToString().PadLeft(40, '0'));//452

                    strLinhaReg.Append(rs["Saida_ICMS"].ToString().Replace(",", "").PadLeft(5, '0'));//492
                    strLinhaReg.Append("".PadLeft(11, '0'));//497
                    if (TipoCarga == 0)
                    {
                        strLinhaReg.Append(rs["Cod_PLU"].ToString().PadLeft(6, '0'));//508
                        strLinhaReg.Append(rs["CST_PIS_COFINS"].ToString().PadLeft(2, '0'));//514
                        strLinhaReg.Append(rs["pis_perc_saida"].ToString().Replace(",", "").Replace(".", "").PadLeft(5, '0'));//516
                        strLinhaReg.Append(rs["cofins_perc_saida"].ToString().Replace(",", "").Replace(".", "").PadLeft(5, '0'));//521

                    }
                    else
                    {
                        strLinhaReg.Append("".ToString().PadLeft(18, '0'));//508
                    }

                    strLinhaReg.Append("".ToString().PadLeft(130, '0'));//526

                    String strCST = rs["CST"].ToString().Trim();

                    if (
                        strCST.Equals("10") ||
                        strCST.Equals("30") ||
                        strCST.Equals("70") ||
                        strCST.Equals("60") ||
                        strCST.Equals("203") ||
                        strCST.Equals("201") ||
                        strCST.Equals("202") ||
                        strCST.Equals("500")
                        )
                    {
                        strLinhaReg.Append("5405".PadLeft(7, '0'));//656  CFOP
                    }
                    else
                    {
                        strLinhaReg.Append("5102".PadLeft(7, '0'));//656  CFOP
                    }

                    strLinhaReg.Append("".ToString().PadLeft(35, '0'));//663

                    strLinhaReg.Append(rs["CEST"].ToString().Trim().PadLeft(7, '0'));//698  CEST

                    strLinhaReg.Append("".ToString().PadLeft(80, '0'));//705

                    strLinhaReg.Append(rs["origem"].ToString().Trim().PadLeft(1, '0'));//785  ORIGEM
                    strLinhaReg.Append(rs["CST"].ToString().Trim().PadLeft(3, '0'));//786  CST

                    strLinhaReg.Append(rs["UND"].ToString().Trim().PadLeft(2, '0'));//789  Unidade

                    strLinhaReg.Append("".ToString().PadLeft(99, '0'));//791


                    strLinhaReg.Append((rs["alcoolico"].ToString().Equals("1") ? "1" : "0")); //890 Bebida Alcoolica
                    strLinhaReg.Append("".ToString().PadLeft(10, '0'));//891
                    strLinhaReg.Append(rs["Imposto"].ToString().Replace(",", "").PadLeft(5, '0'));//901
                    strLinhaReg.Append("".ToString().PadRight(17, '0'));//906

                    //CAMPO PARA O SAT
                    strLinhaReg.Append(rs["Saida_ICMS"].ToString().Replace(",", "").PadLeft(5, '0'));//923 Aliq Intern icms
                    strLinhaReg.Append("".ToString().PadLeft(5, '0'));//928 perc reduz b calculo pis
                    strLinhaReg.Append("".ToString().PadLeft(5, '0'));//933 perc reduz b calculo cofins
                    strLinhaReg.Append(rs["pis_perc_saida"].ToString().Replace(",", "").Replace(".", "").PadLeft(5, '0'));//938 aliquota pis
                    strLinhaReg.Append(rs["cofins_perc_saida"].ToString().Replace(",", "").Replace(".", "").PadLeft(5, '0'));//943 aliquota cofins
                    strLinhaReg.Append("".ToString().PadLeft(5, '0'));//948 perc subst ICMS
                    strLinhaReg.Append("".ToString().PadLeft(5, '0'));//953 perc subst PIS
                    strLinhaReg.Append("".ToString().PadLeft(5, '0'));//958 perc subst COFINS
                    strLinhaReg.Append("".ToString().PadLeft(12, '0'));//963 aliquota valor pis
                    strLinhaReg.Append("".ToString().PadLeft(12, '0'));//975 aliquota valor cofins
                    strLinhaReg.Append(rs["CST_PIS_COFINS"].ToString().PadLeft(3, '0'));//987 cod sit trib pis
                    strLinhaReg.Append(rs["CST_PIS_COFINS"].ToString().PadLeft(3, '0'));//990 cod sit trib pis pvvm
                    strLinhaReg.Append(rs["CST_PIS_COFINS"].ToString().PadLeft(3, '0'));//993 cod sit trib cofins
                    strLinhaReg.Append(rs["CST_PIS_COFINS"].ToString().PadLeft(3, '0'));//996 cod sit trib cofins pvvm
                    strLinhaReg.Append("".ToString().PadLeft(165, '0'));//999 PVVM
                    strLinhaReg.AppendLine();

                }
            }
            else if (this.TipoCarga == 2)
            {
                //Novo busca preco
                if (usr.filial.mBusca_preco == "SWEDA")
                {
                    endereco = usr.filial.diretorio_busca_preco + "\\PRICETAB.CSV";
                    while (rs.Read())
                    {
                        strLinhaReg.Append(rs["PLU"].ToString().Trim() + ";");
                        strLinhaReg.Append(Funcoes.RemoverAcentos(rs["Descricao_Resumida"].ToString()).Trim().PadRight(20, ' ').Substring(0, 20) + ";");
                        if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                        {
                            TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                            TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));
                            //if (DateTime.Parse(rs["Data_Inicio"].ToString()).ToString("yyyyMMdd") <= DateTime.Now.ToString("yyyyMMdd"))
                            if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                            // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                            {
                                strLinhaReg.Append(rs["Preco_Promocao"].ToString().Replace(",", "") + ";");
                                strLinhaReg.Append(rs["Preco_Promocao"].ToString().Replace(",", "") + ";");
                            }
                            else
                            {
                                strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "") + ";");
                                strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "") + ";");
                            }
                        }
                        else
                        {
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "") + ";");
                            strLinhaReg.Append(rs["Preco"].ToString().Replace(",", "") + ";");
                        }
                        strLinhaReg.AppendLine("0;0");
                    }

                }
                else
                {
                    endereco = usr.filial.diretorio_busca_preco + "\\PRICETAB.txt";

                    while (rs.Read())
                    {

                        strLinhaReg.Append(rs["PLU"].ToString().Trim() + "|");
                        strLinhaReg.Append(Funcoes.RemoverAcentos(rs["Descricao_Resumida"].ToString()).Trim().PadRight(20, ' ').Substring(0, 20) + "|");
                        if (!rs["Data_Inicio"].ToString().Equals("") && !rs["Data_Fim"].ToString().Equals(""))
                        {
                            TimeSpan intervalo = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Inicio"].ToString()));
                            TimeSpan intervalo2 = DateTime.Now.Subtract(DateTime.Parse(rs["Data_Fim"].ToString()));
                            //if (DateTime.Parse(rs["Data_Inicio"].ToString()).ToString("yyyyMMdd") <= DateTime.Now.ToString("yyyyMMdd"))
                            if (intervalo.Days >= 0 && intervalo.Hours >= 0 && intervalo2.Days <= 0)
                            // && (DateTime)rs["Data_Fim"] >= DateTime.Today)
                            {
                                strLinhaReg.AppendLine(rs["Preco_Promocao"].ToString().Replace(",", ".") + "|");
                            }
                            else
                            {
                                strLinhaReg.AppendLine(rs["Preco"].ToString().Replace(",", ".") + "|");
                            }
                        }
                        else
                        {
                            strLinhaReg.AppendLine(rs["Preco"].ToString().Replace(",", ".") + "|");
                        }
                    }
                }

            }
            else if (this.TipoCarga == 10)
            {
                endereco = usr.filial.diretorio_gera + "\\10.sdf";

                while (rs.Read())
                {
                    strLinhaReg.Append(DateTime.Now.ToString("ddMMyy"));
                    strLinhaReg.Append("000");
                    if (Alterados == 1)
                    {
                        strLinhaReg.Append(Regex.Replace(rs["CNPJ"].ToString().Trim(), @"[,.\-/]", "").PadLeft(16, '0'));
                    }
                    else
                    {
                        strLinhaReg.Append(rs["Codigo_cliente"].ToString().Trim().PadLeft(16, '0'));
                    }
                    strLinhaReg.Append(rs["numero_situacao"].ToString().Trim().PadLeft(2, '0'));
                    Decimal limite_credito = rs["limite_credito"].ToString().Trim().Equals("") ? 0 : Decimal.Parse(rs["limite_credito"].ToString());
                    strLinhaReg.Append(limite_credito.ToString("N2").Replace(".", "").Replace(",", "").PadLeft(12, '0'));
                    Decimal Utilizado = rs["utilizado"].ToString().Trim().Equals("") ? 0 : Decimal.Parse(rs["utilizado"].ToString());
                    strLinhaReg.Append(Utilizado.ToString("N2").Replace(".", "").Replace(",", "").PadLeft(12, '0'));
                    strLinhaReg.Append(rs["Nome_cliente"].ToString().Trim().PadRight(44, ' ').Substring(0, 44));
                    strLinhaReg.Append(rs["Endereco"].ToString().Trim().PadRight(60, ' ').Substring(0, 60));
                    String cnpj = Regex.Replace(rs["CNPJ"].ToString().Trim(), @"[,.\-/]", "");
                    strLinhaReg.Append(Regex.Replace(rs["CNPJ"].ToString().Trim(), @"[,.\-/]", "").PadLeft(15, '0'));
                    strLinhaReg.Append(Regex.Replace(rs["IE"].ToString().Trim(), @"[,.\-/]", "").PadLeft(14, '0'));
                    strLinhaReg.Append("00");
                    strLinhaReg.Append(rs["Bairro"].ToString().Trim().PadRight(32, ' ').Substring(0, 32));
                    strLinhaReg.Append(rs["CEP"].ToString().Replace("-", "").Trim().PadLeft(10, '0'));
                    strLinhaReg.Append(rs["Cidade"].ToString().Trim().PadRight(44, ' '));
                    strLinhaReg.Append(rs["UF"].ToString().Trim().PadRight(2, ' '));
                    strLinhaReg.Append(new String(' ', 20));
                    strLinhaReg.AppendLine("02222");

                }

            }



            //Gerar o Arquivo TXT conforme parametros.
            if (strLinhaReg.Length <= 0)
            {
                if (rs != null)
                {
                    rs.Close();
                }
                String strMsg = "Não foi encontrado nenhum produto ";
                if (Alterados == 1)
                {
                    strMsg += "Alterado";
                }
                throw new Exception(strMsg);

            }


            String endFinal = endTemp + endereco.Replace("\\", "_").Replace(":", "");

            StreamWriter valor = new StreamWriter(endFinal, false, Encoding.ASCII);
            valor.Write(strLinhaReg);
            valor.Close();


            System.IO.File.Copy(endFinal, endereco, true);



            if (!endereco2.Trim().Equals(""))
            {
                endFinal = endTemp + endereco2.Replace("\\", "_").Replace(":", "");
                StreamWriter valor2 = new StreamWriter(endFinal, false, Encoding.ASCII);
                valor2.Write(strLinhaReg2);
                valor2.Close();


                System.IO.File.Copy(endFinal, endereco2, true);
            }


            if (rs != null)
            {
                rs.Close();
            }
        }


    }
}

