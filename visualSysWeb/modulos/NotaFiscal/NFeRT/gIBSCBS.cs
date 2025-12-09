using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class gIBSCBS
    {
        [XmlIgnore]
        public decimal _vBC;
        [XmlIgnore]
        public decimal _vIBS;
        [XmlElement]
        public string vBC
        {
            get { return _vBC.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vBC = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        [XmlElement]
        public gIBSUF gIBSUF { get; set; }
        [XmlElement]
        public gIBSMun gIBSMun { get; set; }
        [XmlElement]
        public string vIBS
        {
            get { return _vIBS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vIBS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }

        [XmlElement]
        public gCBS gCBS { get; set; }
    }
}
