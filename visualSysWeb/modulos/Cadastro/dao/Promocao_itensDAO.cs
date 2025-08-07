using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Promocao_itensDAO
    {
        public int Codigo_promo { get; set; }
        public int Plu { get; set; }
        public string Descricao { get; set; }
        public decimal preco_promocao { get; set; }
    }
}