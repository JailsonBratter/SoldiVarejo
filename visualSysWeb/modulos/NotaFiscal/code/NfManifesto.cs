using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.NotaFiscal.code
{
    public class NfManifesto
    {
        public String NSU
        {
            get
            {
                if (_Nsu.Equals(""))
                    return "000000000000000";
                else
                    return _Nsu;
            }
            set
            {
                _Nsu = value;
            }
        }
        public Decimal vNF { get; set; }
        public String CNPJ { get; set; }
        public String RazaoSocial { get; set; }
        public String Chave { get; set; }
        public DateTime Emissao { get; set; }

        public String xml { get; set; }
        private String _Nsu = "";
      
    }
}