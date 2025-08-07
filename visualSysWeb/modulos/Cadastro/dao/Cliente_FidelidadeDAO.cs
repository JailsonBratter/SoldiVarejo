using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cliente_FidelidadeDAO
    {
        public string Codigo_cliente { get; set; }
        public DateTime Data_Venda { get; set; }
        public int Caixa_saida { get; set; }
        public string Documento { get; set; }
        public string PLU { get; set; }
        private string _descricao = "";
        public string Descricao
        {
            get
            {
                if (_descricao.Equals(""))
                {
                    _descricao = Conexao.retornaUmValor("Select Descricao from mercadoria where plu ='" + PLU + "'",null);
                }
                return _descricao;
            }
            set { _descricao = value; }
        }
        public decimal Qtde_pontos { get; set; }
    }
}