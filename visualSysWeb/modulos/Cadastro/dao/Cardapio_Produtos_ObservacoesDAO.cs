using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cardapio_Produtos_ObservacoesDAO
    {
        private string Filial { get; set; }
        public string Plu { get; set; }
        public string Titulo { get; set; }
        public string PluAdd { get; set; }
        public int Obrigatorio { get; set; }
        public int ObrigatorioOrdem { get; set; }
        public string Tipo { get; set; }
        public decimal Preco { get; set; }

        public Cardapio_Produtos_ObservacoesDAO(String filial)
        {
            this.Filial = filial;
        }


        internal string sql()
        {
            string sql = "insert into mercadoria_obs (";
            string values = ")values(";

            sql += "filial";
            values += "'" + Filial + "'";

            sql += ",plu";
            values += ",'" + Plu + "'";

            sql += ",obs";
            values += ",'" + Titulo + "'";

            sql += ",plu_item_adc";
            values += ",'" + PluAdd + "'";
            
            sql += ",obrigatorio";
            values += "," + Obrigatorio.ToString();

            sql += ",obrigatorioOrdem";
            values += "," + ObrigatorioOrdem.ToString();


            sql += ",tipo";
            values += ",'" + Tipo + "'";

            sql += values + ");";

            return sql;
        }
    }
}