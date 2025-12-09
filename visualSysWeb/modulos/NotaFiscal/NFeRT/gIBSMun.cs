using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class gIBSMun
    {
        [XmlIgnore]
        public decimal _pIBSMun;
        [XmlIgnore]
        public decimal _vIBSMun;

        [XmlElement]
        public string pIBSMun
        {
            get { return _pIBSMun.ToString("F4", System.Globalization.CultureInfo.InvariantCulture); }
            set { _pIBSMun = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        [XmlElement]
        public string vIBSMun
        {
            get { return _vIBSMun.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vIBSMun = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }

    }
}
