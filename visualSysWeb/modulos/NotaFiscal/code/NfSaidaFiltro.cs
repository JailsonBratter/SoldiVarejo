using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.NotaFiscal.code
{
    public class NfSaidaFiltro
    {
        public string codigo { get; set; }
        public string  destinatario { get; set; }
        public string  cliente { get; set; }
        public string  tipoPesquisa { get; set; }
        public string  de { get; set; }
        public string  ate { get; set; }
        public string  status { get; set; }
        public bool carta_correcao { get; set; }
    }
}