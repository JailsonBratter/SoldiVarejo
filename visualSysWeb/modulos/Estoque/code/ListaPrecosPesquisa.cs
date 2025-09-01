using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.code
{
    public class ListaPrecosPesquisa
    {
        public String fornecedor = "";
        public String grupo = "";
        public String subgrupo = "";
        public String departamento = "";
        public String plu_descricao = "";
        public String linha = "";
        public String codigo_lista = "";
        public String cozinha = "";
        public String agrupamento = "";
        public String tipoProducao = "";
        public String tipoPesquisa = "";
        public String vendasHa = "";
        public String comprasHa = "";
        private User usr = null;
        public int gridInicio = 0;
        public int gridFim = 20;
        public ListaPrecosPesquisa(User usr, String tipoPesquisa)
        {
            this.tipoPesquisa = tipoPesquisa;
            this.usr = usr;
        }
        public List<ListaPrecosPesquisaItem> ItensEncontrados = new List<ListaPrecosPesquisaItem>();
        public List<ListaPrecosPesquisaItem> ItensSelecionados = new List<ListaPrecosPesquisaItem>();

        public void atualizarPesquia()
        {
            String sqlMercadoria = " Select * into #cadDepart from W_BR_CADASTRO_DEPARTAMENTO; " +
                "Select distinct m.PLU,m.Descricao, m.UND,Departamento= wd.descricao_departamento  " +
                                             " from mercadoria as m " +
                                               " inner join #cadDepart as wd on m.Codigo_departamento = wd.codigo_departamento " +
                                    " where isnull(m.inativo,0)=0 and  ";
            if (tipoPesquisa.Equals("COMPRA"))
                sqlMercadoria += " (m.tipo not in('PRODUCAO')) ";
            else if (tipoPesquisa.Equals("PRODUCAO"))
                sqlMercadoria += " m.tipo='PRODUCAO' " +
                    " AND ISNULL((Select m3.filial_produzido from mercadoria as m3 where  m3.plu = m.plu_receita),'') <>''";
            else
                sqlMercadoria += " m.descricao <> ''";

            if (Funcoes.isnumero(plu_descricao))
            {
                if (plu_descricao.Length <= 6)
                {
                    sqlMercadoria += " and m.plu = '" + plu_descricao + "' ";
                }
                else
                {
                    sqlMercadoria += " and (m.plu in  (Select plu from ean where ean like '%" + plu_descricao + "%'))";
                }
            }
            else
            {
                if (plu_descricao.Length > 0)
                {

                    sqlMercadoria += " and (m.descricao like '%" + plu_descricao + "%' or m.Ref_fornecedor like '%" + plu_descricao + "%')";
                }


                if (!grupo.Equals(""))
                {
                    sqlMercadoria += " and wd.Descricao_grupo='" + grupo + "' ";
                }
                if (!subgrupo.Equals(""))
                {
                    sqlMercadoria += " and wd.Descricao_subgrupo ='" + subgrupo + "' ";

                }
                if (!departamento.Equals(""))
                {
                    sqlMercadoria += " and m.Descricao_departamento ='" + departamento + "' ";
                }
            }

            if (!linha.Equals(""))
            {
                sqlMercadoria += " and convert(varchar(3),isnull(Cod_Linha,''))+CONVERT(varchar(3),isnull(Cod_Cor_Linha,'')) ='" + linha + "'";
            }

            if (!codigo_lista.Equals(""))
            {
                sqlMercadoria += " and plu in (Select plu from  LISTA_PADRAO_ITENS where id_lista='" + codigo_lista + "')";
            }


            if (cozinha.Length > 0)
            {
                sqlMercadoria += " and (Select m3.filial_produzido from mercadoria as m3 where  m3.plu = m.plu_receita)='" + cozinha + "'";
            }

            if (tipoProducao.Replace("'", "").Length > 0)
            {
                sqlMercadoria += " and m.tipo_Producao in(" + tipoProducao + ")";
            }

            if (agrupamento.Length > 0)
            {
                sqlMercadoria += " and m.Agrupamento_producao='" + agrupamento + "'";
            }
            //Especiais
            if (Funcoes.isnumero(vendasHa))
            {
                sqlMercadoria += " and m.plu IN(SELECT s.PLU FROM Saida_Estoque S WITH (INDEX=IX_SAIDA_ESTOQUE, NOLOCK) WHERE S.Filial = '" + usr.filial.Filial + "'";
                sqlMercadoria += " AND s.Data_Movimento >= DATEADD(MONTH, -" + vendasHa + ", GETDATE())" + ")";
            }
            if (Funcoes.isnumero(comprasHa))
            {
                sqlMercadoria += " AND M.PLU IN(SELECT I.PLU ";
                sqlMercadoria += " FROM NF WITH(INDEX = IX_NF_01, NOLOCK) INNER JOIN NF_ITEM I WITH(INDEX = IX_NF_ITEM_01, NOLOCK) ON NF.Filial = I.Filial AND NF.Cliente_Fornecedor = I.Cliente_Fornecedor ";
                sqlMercadoria += " AND NF.Codigo = I.Codigo AND NF.Tipo_NF = I.Tipo_NF AND NF.serie = I.serie WHERE NF.Data >= DATEADD(MONTH, -" + comprasHa + ", GETDATE()))";
            }

            ItensEncontrados.Clear();
            ItensSelecionados.Clear();
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sqlMercadoria + " order by m.descricao", usr, false);
                while (rs.Read())
                {
                    ListaPrecosPesquisaItem item = new ListaPrecosPesquisaItem();
                    item.PLU = rs["PLU"].ToString();
                    item.Descricao = rs["Descricao"].ToString();
                    item.Und = rs["Und"].ToString();
                    item.Departamento = rs["Departamento"].ToString();
                    ItensEncontrados.Add(item);
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

        public List<ListaPrecosPesquisaItem> Resultado()
        {
            List<ListaPrecosPesquisaItem> resFin = new List<ListaPrecosPesquisaItem>();
            for (int i = gridInicio; i < gridFim && (i < ItensEncontrados.Count); i++)
            {
                resFin.Add(ItensEncontrados[i]);
            }
            return resFin;
        }
    }
}