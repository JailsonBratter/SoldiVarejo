using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Gerencial.code
{
    public class ListaPrecificacaoPesquisa
    {
        public String fornecedor = "";
        public String grupo = "";
        public String subgrupo = "";
        public String departamento = "";
        public String plu_descricao = "";
        public String linha = "";
        public String codigo_lista = "";
       
        private User usr = null;
        public int gridInicio = 0;
        public int gridFim = 20;
        public ListaPrecificacaoPesquisa(User usr)
        {
           
            this.usr = usr;
        }
        public List<ListaPrecificacaoPesquisaItem> ItensEncontrados = new List<ListaPrecificacaoPesquisaItem>();
        public List<ListaPrecificacaoPesquisaItem> ItensSelecionados = new List<ListaPrecificacaoPesquisaItem>();

        public void atualizarPesquia()
        {
            String sqlMercadoria = " Select * into #cadDepart from W_BR_CADASTRO_DEPARTAMENTO; " +
                "Select distinct m.PLU,m.Descricao, m.UND,Departamento= wd.descricao_departamento  " +
                                             " from mercadoria as m " +
                                               " inner join #cadDepart as wd on m.Codigo_departamento = wd.codigo_departamento " +
                                    " where isnull(m.inativo,0)=0   ";
          

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

            if(!codigo_lista.Equals(""))
            {
                sqlMercadoria += " and plu in (Select plu from  LISTA_PADRAO_ITENS where id_lista='" + codigo_lista + "')";
            }


          
           
            ItensEncontrados.Clear();
            ItensSelecionados.Clear();
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sqlMercadoria + " order by m.descricao", usr, false);
                while (rs.Read())
                {
                    ListaPrecificacaoPesquisaItem item = new ListaPrecificacaoPesquisaItem();
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

        public List<ListaPrecificacaoPesquisaItem> Resultado()
        {
            List<ListaPrecificacaoPesquisaItem> resFin = new List<ListaPrecificacaoPesquisaItem>();
            for (int i = gridInicio; i < gridFim && (i < ItensEncontrados.Count); i++)
            {
                resFin.Add(ItensEncontrados[i]);
            }
            return resFin;
        }
    }
}