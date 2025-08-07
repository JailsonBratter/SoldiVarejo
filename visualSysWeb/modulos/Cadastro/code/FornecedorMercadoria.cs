using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class FornecedorMercadoria
    {
        public String filial = "";
        public String fornecedor = "";
        public String plu_ant = "";
        public String plu { get; set; }
        public String Descricao  { get; set; }
        public String Descricao_NF { get; set; }
        public String descricao_NF_ant = "";
        public DateTime data = new DateTime();
        public String dataBr
        {
            get
            {
                return datafBr(data);
            }
        }
        

        public Decimal preco_compra { get; set; }
        public String ean { get; set; }
        public String codigo_referencia_antigo = "";
        public String codigo_referencia { get; set; }
        public Decimal preco_Custo { get; set; }
        public int embalagem { get; set; }
        public DateTime prazo = new DateTime();
        public String prazoBr
        {
            get
            {
                return datafBr(prazo);
            }
        }

        public bool importado_nf { get; set; }
        private String datafBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }




    }
}