using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class TesourariaDetalhesDAO
    {
        public DateTime Emissao { get; set; }
        public int Pdv { get; set; }
        public string Operador { get; set; }
        public int Finalizadora { get; set; }
        public string Cupom { get; set; }
        public decimal Total { get; set; }
        public string IdFinalizadora { get; set; }
        public string IdCartao { get; set; }
        public string Autorizacao { get; set; }
        public string IdBandeira { get; set; }
        public string RedeCartao { get; set; }
        public string Filial { get; set; }
        public string HoraVenda { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Taxa { get; set; }
        public long IdFechamento { get; set; }
        public long Sequencia { get; set; }


        public bool insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}