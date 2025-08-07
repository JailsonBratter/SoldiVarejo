using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Collections;

namespace visualSysWeb.dao
{
    public class AlteracaoFinalizadoraDAO
    {
        public string finalizadora { get; set; }
        public string autorizadora { get; set; }
        public string cartao { get; set; }
        public string codigoAutorizacao { get; set; }
        public decimal valor { get; set; }

        public AlteracaoFinalizadoraDAO()
        {

        }

    }
}