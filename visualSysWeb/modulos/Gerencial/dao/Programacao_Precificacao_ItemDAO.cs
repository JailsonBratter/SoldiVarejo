using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Gerencial.dao
{
    public class Programacao_Precificacao_ItemDAO
    {
        public int id_precificacao { get; set; }
        public int ordem { get; set; }
        public string plu { get; set; }
        public string descricao { get; set; }
        public Decimal preco { get; set; }
    }
}