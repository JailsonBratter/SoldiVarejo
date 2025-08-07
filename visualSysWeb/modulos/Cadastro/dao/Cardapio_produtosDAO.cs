using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cardapio_produtosDAO
    {
        public string Plu { get; set; }
        public int IdCategoria { get; set; }
        public int idCardapio { get; set; }
        public bool Ativo { get; set; }
        public decimal Preco { get; set; }
        public int PrecoPorObs { get; set; }
        public string Descricao { get; set; }
        public string DescricaoComercial { get; set; }
        public int obs
        {
            get
            {
                return Observacoes.Count;
            }
        }

        public List<Cardapio_Produtos_ObservacoesDAO> Observacoes { get; set; } = new List<Cardapio_Produtos_ObservacoesDAO>();
        private string Filial { get; }
        public string codigoUUID { get; set; }
        public int Numero_Maximo_Obs { get; set; }

        public Cardapio_produtosDAO(String filial, int IDCardapio)
        {
            this.Filial = filial;
            this.idCardapio = IDCardapio;
        }
        internal void CarregarObs()
        {
            SqlDataReader rs = null;
            string sql = "Select mo.*, m.preco from mercadoria_obs as mo left join mercadoria_loja m on " +
                "mo.plu_item_adc= m.plu  and m.filial='" +this.Filial+"' "+
                "where mo.filial ='" + this.Filial + "' and mo.plu ='"+this.Plu+"'";
            try
            {
                rs = Conexao.consulta(sql, null, false);
                this.Observacoes.Clear();
                while (rs.Read())
                {
                    Cardapio_Produtos_ObservacoesDAO obs = new Cardapio_Produtos_ObservacoesDAO(this.Filial);
                    obs.Plu = this.Plu;
                    obs.Titulo = rs["obs"].ToString();
                    obs.PluAdd = rs["plu_item_adc"].ToString();
                    obs.Tipo = rs["tipo"].ToString();
                    obs.Preco = Funcoes.decTry(rs["preco"].ToString());
                    obs.ObrigatorioOrdem = Funcoes.intTry(rs["ObrigatorioOrdem"].ToString());
                    this.Observacoes.Add(obs);
                }
            }
            catch (Exception err)
            {
                throw new Exception("Observacao - " + err.Message);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        internal string sql()
        {
            string sql = "insert into cardapio_produtos (";
            string values = ")values(";

            sql += "filial";
            values += "'" + Filial + "'";

            sql += ",plu";
            values += ",'" + Plu + "'";

            sql += ",idCategoria";
            values += "," + IdCategoria;

            sql += ",ativo";
            values += "," + (Ativo ? "1" : "0");

            sql += ",precoPorObs";
            values += "," + PrecoPorObs.ToString();

            sql += ",IDCardapio";
            values += ", " + idCardapio.ToString();

            sql += values + ");";

            sql += "delete from mercadoria_obs where plu ='" + Plu + "'; ";

            foreach (Cardapio_Produtos_ObservacoesDAO obs in Observacoes)
            {
                obs.Plu = this.Plu;
                sql += obs.sql();
            }

            return sql;

        }
    }
}