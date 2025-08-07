using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Dispositivos.dao
{
    public class FtpItem
    {
        public String PLU{ get; set; }
        public String EAN { get; set; }
        public String Descricao { get; set; }
        public Decimal Preco { get; set; }
        public String Und { get; set; }
    }
}