using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Sped.dao
{
    public class DocumentoEletronicoDAO
    {
        public string Documento { get; set; }
        public string Caixa { get; set; }
        public DateTime Data { get; set; }
        public string DataStr { 
            get {
                return Data.ToString("dd/MM/yyyy");
            } 
        }
        public string Nro_Serie_Equipamento { get; set; }
        public string Nro_extrato_Sat { get; set; }
        public string ID_Chave { get; set; }
        public string ID_Chave_Cancelamento { get; set; }
        public string CFe_XML { get; set; }
        public string CFe_XML_Cancelamento { get; set; }



    }
}