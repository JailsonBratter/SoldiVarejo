using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class gCBS
    {
        [XmlIgnore]
        public decimal _pCBS;
        [XmlIgnore]
        public decimal _vCBS;

        [XmlElement]
        public string pCBS
        {
            get { return _pCBS.ToString("F4", CultureInfo.InvariantCulture); }
            set { _pCBS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        [XmlElement]
        public string vCBS
        {
            get { return _vCBS.ToString("F2", CultureInfo.InvariantCulture); }
            set { _vCBS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
    }
}