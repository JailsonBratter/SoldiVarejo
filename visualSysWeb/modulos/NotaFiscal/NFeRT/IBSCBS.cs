using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class IBSCBS
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public string cClassTrib { get; set; }
        [XmlElement]
        public gIBSCBS gIBSCBS { get; set; }
    }

}