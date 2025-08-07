using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Financeiro.code
{
    public class ContasReceberFiltro
    {
        public String documento = "";
        public String codCliente = "";
        public String nomeCliente = "";
        public String de = "";
        public String ate = "";
        public String pesquisaPor = "";
        public String status = "";
        public String orderBy = "";
        public String tipoPagamento = "";
        public String centroCusto = "";
        public String cartao = "";
        public String cnpj = "";
        public int iniGrid = 0;
        public int fimGrid = 100;
        public String sqlFinal = "";
        public int totalfiltro = 0;
        public Decimal totalValor = 0;
        public int totalCadastrado = 0;
        public String valores = "";
    }
}