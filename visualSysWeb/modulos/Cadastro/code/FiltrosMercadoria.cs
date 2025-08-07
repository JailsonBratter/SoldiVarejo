using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class FiltrosMercadoria
    {
        public String plu = "";
        public String ean = "";
        public String mercadoria = "";
        public String campo1 = "";
        public String onde = "";
        public String campo2 = "";
        public String Ref = "";
        public String Ncm = "";
        public bool promocao = false;
        public bool inativo = false;
        public bool pendente = false;
        public bool atacado = false;
        public String grupo = "";
        public String subgrupo = "";
        public String departamento = "";
        public String familia = "";
        public String data = "";
        public String dtDe = "";
        public String dtAte = "";
        public String tribSaida = "";

        public int qtdeFiltro = 0;
        public int qtdeCadastro = 0;
        public int iniGrid = 0;
        public int fimGrid = 100;
        public String ultimaOrdem = "";
        public String ordemFiltro = "";
        public String tipo = "";

        public string CstPisCofins { get; internal set; }
        public string PLURelacionado = "";

        public String SqlFinal(User usr, String ordem)
        {
            this.ordemFiltro = ordem;
            String strOrdem = "";
            if (ordem.Equals(""))
            {
                strOrdem = " order by convert(varchar,l.Data_Alteracao,102) ";
            }
            else
            {
                if (ordem.Equals("PLU"))
                {
                    strOrdem = " order by convert(int,a.plu) ";
                }
                else if (ordem.Equals("Data_alteracao"))
                {
                    strOrdem = " order by convert(varchar,a.Data_alteracao,102) ";
                }
                else
                {
                    strOrdem = " order By " + ordem;
                }
            }

            if (ordem.Equals(ultimaOrdem))
            {
                strOrdem += " Desc ";
                ultimaOrdem = "";
            }
            else
            {
                ultimaOrdem = ordem;
            }

            String sql = "Select a.plu as PLU,isnull(b.EAN,'---') EAN, isnull(a.Ref_Fornecedor,'---') AS Refer, Replace(a.descricao,' ','&nbsp')  AS Mercadoria, l.Preco_Custo," +
                               "l.preco, preco_promocao = case when l.promocao=1then l.preco_promocao else 0 end, ISNULL(a.Qtde_Atacado, 0) As Qtde_Atacado, ISNULL(a.preco_atacado, 0) as Preco_Atacado, convert(varchar,l.Data_Alteracao,103)Data_Alteracao,ICMS_SAIDA = right(replicate('0',3)+isnull(ltrim(rtrim(t.Indice_ST)),'00'),3)+'-'+RIGHT( REPLICATE('0',4) + CONVERT(VARCHAR(10),CONVERT(MONEY,isnull(t.ICMS_Efetivo,0))), 5)  , a.cst_saida as PISCofins, isnull(a.cf,'') ncm  " +
                               " ,Saldo_atual = case when isnull((Select permite_item from tipo where tipo=a.tipo ),0)=1 then 0 else l.Saldo_atual end " +
                               ",l.margem,a.peso_bruto, a.inativo, ISNULL(Convert(varchar, l.Data_Inventario, 103), '1900.01.01') AS DataInventario  " +
                               ",ROW_NUMBER() OVER(" + strOrdem + ") numeroLinha" +
                               "   from mercadoria a  LEFT join ean b on a.plu=b.plu  inner join mercadoria_loja l on l.plu=a.plu	inner join Tributacao t on a.Codigo_Tributacao = t.Codigo_Tributacao  " +
                                "  left join Familia f on a.Codigo_familia = f.Codigo_familia";



            String sqlFinal = "";







            String strWhere = "";
            if (!plu.Equals(""))
            {
                strWhere += " and  a.plu = '" + plu + "'";

            }
            else if (!mercadoria.Equals(""))
            {
                strWhere += " and a.descricao like '%" + mercadoria + "%'";
            }
            else if (!ean.Equals(""))
            {
                strWhere += " and b.ean like '" + ean + "%'";

            }

            if (!campo1.Equals("") && !campo2.Equals("") && !onde.Equals(""))
            {

                strWhere += " and " + campo1 + " " + onde + " " + campo2;

            }
            if (promocao)
                strWhere += " and a.promocao = 1 ";

            if (!Ncm.Equals(""))
            {
                strWhere += " and cf LIKE '" + Ncm + "%' ";
            }
            string codigoGrupo = "";
            string codigoSubGrupo = "";
            string codigoDepartamento = "";

            if (!grupo.Equals(""))
            {
                codigoGrupo = Conexao.retornaUmValor("Select codigo_grupo from grupo where descricao_GRUPO ='" + grupo + "'", null);
                strWhere += " and a.codigo_departamento  like '" + codigoGrupo.PadLeft(3, '0') + "%' ";
            }
            if (!subgrupo.Equals(""))
            {
                codigoSubGrupo = Conexao.retornaUmValor("Select codigo_subgrupo from subgrupo where descricao_subgrupo ='" + subgrupo + "'"

                    , null);
                strWhere += " and a.codigo_departamento  like '" + codigoSubGrupo + "%'";
            }
            if (!departamento.Equals(""))
            {
                codigoDepartamento = Conexao.retornaUmValor("Select codigo_departamento from departamento where descricao_departamento ='" + departamento + "'"
                    , null);
                strWhere += " and a.codigo_departamento='" + codigoDepartamento + "'";
            }

            if (!familia.Equals(""))
            {
                strWhere += " and f.descricao_familia ='" + familia + "'";
            }

            if (!data.Equals("---") && !data.Equals(""))
            {
                strWhere += " and  " + data + " between '" + DateTime.Parse(dtDe).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(dtAte).ToString("yyyy-MM-dd") + "'";
            }

            if (!Ref.Equals(""))
            {
                strWhere += " and  a.Ref_Fornecedor LIKE '" + Ref + "%'";
            }

            if (!tribSaida.Equals(""))
            {
                strWhere += " and a.codigo_tributacao =" + tribSaida;
            }

            if(!CstPisCofins.Equals(""))
            {
                strWhere += " and a.cst_saida='" + CstPisCofins + "'";
            }

            if (!tipo.Equals(""))
            {
                strWhere += " and a.Tipo = '" + tipo + "'";
            }

            if (!PLURelacionado.Equals(""))
            {
                if (tipo.Equals("RELACIONADO"))
                {
                    strWhere += " and A.Tipo = 'RELACIONADO' AND a.PLU_Vinculado = '" + PLURelacionado + "'";
                }
                else if (!tipo.Equals(""))
                {
                    strWhere += " AND EXISTS(SELECT * FROM Item WHERE Item.PLU = a.PLU AND Item.PLU_Item = '" + PLURelacionado + "')";
                }
                else
                {
                    strWhere += " and ( A.Tipo = 'RELACIONADO' AND a.PLU_Vinculado = '" + PLURelacionado + "'";
                    strWhere += " OR EXISTS(SELECT * FROM Item WHERE Item.PLU = a.PLU AND Item.PLU_Item = '" + PLURelacionado + "') )";

                }
            }

            if (inativo)
            {
                sqlFinal = sql + " where isnull(inativo,0) = 1 ";
            }
            else if (pendente)
            {
                sqlFinal = sql + " where isnull(inativo,0) = 3 ";
            }
            else if (atacado)
            {
                sqlFinal = sql + " where (l.preco_atacado > 0 or a.preco_atacado > 0)";
            }
            else
            {
                sqlFinal = sql + " where  isnull(inativo,0) <> 1 ";
            }

            if (!strWhere.Equals(""))
            {
                
              sqlFinal += strWhere;
                
            }
         

            sqlFinal += " and l.filial='" + usr.getFilial() + "'";

            sqlFinal = " with merc_prod as (" + sqlFinal + ")  ";
            qtdeFiltro = Funcoes.intTry(Conexao.retornaUmValor(sqlFinal + " Select count(*) from merc_prod ", null));
            qtdeCadastro = Funcoes.intTry(Conexao.retornaUmValor(" Select count(*) from mercadoria as a LEFT join ean b on a.plu=b.plu ", null));
            return sqlFinal + " Select * from merc_prod where numeroLinha between " + (iniGrid + 1) + " and " + fimGrid + " order by numeroLinha"; ;
        }

    }
}