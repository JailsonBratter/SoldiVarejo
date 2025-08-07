using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.dao
{
    public class contabil_eventosDAO
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string conta_contabil { get; set; }
        public int despesa { get; set; }

        public contabil_eventosDAO()
        {

        }
    }
}