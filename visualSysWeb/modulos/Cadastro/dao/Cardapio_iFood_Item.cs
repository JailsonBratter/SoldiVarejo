using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cardapio_iFood_Item
    {
        public string status { get; set; }
        public Price price { get; set; }
        public string externalCode { get; set; }
        public int index { get; set; }
        public List<string> shifts { get; set; }

        public class Price
        {
            public decimal value { get; set; }
            public decimal originalValue { get; set; }

            public Price()
            {

            }
        }
    }
}