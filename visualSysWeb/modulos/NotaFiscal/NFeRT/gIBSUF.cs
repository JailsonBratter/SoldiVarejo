using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class gIBSUF
    {
        [XmlIgnore]
        public decimal _pIBSUF;
        [XmlIgnore] 
        public decimal _vIBSUF;

        [XmlElement]
        public string pIBSUF
        {
            get { return _pIBSUF.ToString("F4", System.Globalization.CultureInfo.InvariantCulture); }
            set { _pIBSUF = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        [XmlElement]
        public string vIBSUF
        {
            get { return _vIBSUF.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vIBSUF = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
    }
}
