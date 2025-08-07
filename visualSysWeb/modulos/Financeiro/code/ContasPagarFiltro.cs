using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Financeiro.code
{
    public class ContasPagarFiltro
    {
      public  String fornecedor = "";
      public String Documento = "";
     
      public String de = "";
      public String ate = "";
      public String PesquisaPor = "";
      public String Status = "";
      public String orderBy = "";
      public String tipoPagamento = "";
      public bool conferido = false;
        public String codigo_centro_custo = "";
    }
}