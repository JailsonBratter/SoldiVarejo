using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.dao
{
    public class Contas_a_pagar_grupoDAO
    {
        public String Filial { get; set; }
        public String Cod_grupo { get; set; }
        public String Documento { get; set; }
        public String Fornecedor { get; set; }
        public DateTime Vencimento { get; set; }
        public String StrVencimento
        {
            get
            {
                return Vencimento.ToString("dd/MM/yyyy");
            }
            set { }
        }
        public Decimal Valor { get; set; }
        public String Status { get; set; }

    }
}