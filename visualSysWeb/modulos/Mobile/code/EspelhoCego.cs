using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Net;

using visualSysWeb.dao;

namespace visualSysWeb.modulos.Mobile.code
{
    public class EspelhoCego
    {
        public string codigo { get; set; }
        public string cnpj { get; set; }
        public string sdata { get; set; }
        public string fornecedor { get; set; }

        public List<EspelhoCegoItens> itens;

        public EspelhoCego()
        {
            itens = new List<EspelhoCegoItens>();
        }

        public EspelhoCego(string chave)
        {

            //if (dr.HasRows)
            //{
            //    while (dr.Read())
            //    {
            //        codigo = dr["chave"].ToString().Substring(27, 9);
            //        cnpj = dr["cnpj"].ToString();
            //        sdata = dr["emissao"].ToString();
            //        fornecedor = dr["RazaoSocial"].ToString();
            //    }
            //}

            itens = new List<EspelhoCegoItens>();
        }

    }
}