using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using visualSysWeb.code;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class IBSCBSTot
    {
        private decimal _vBCIBSCBS;

        [XmlElement("vBCIBSCBS")]
        public string vBCIBSCBS
        {
            get => _vBCIBSCBS.ToString("F2", CultureInfo.InvariantCulture);
            set => _vBCIBSCBS = Funcoes.ConvertstrToDecimalCulture(value.ToString());
        }

        [XmlElement("gIBS")]
        public TotgIBS gIBS { get; set; }

        [XmlElement("gCBS")]
        public TotgCBS gCBS { get; set; }
    }

    public class TotgIBS
    {
        [XmlElement("gIBSUF")]
        public TotgIBSUF gIBSUF { get; set; }

        [XmlElement("gIBSMun")]
        public TotgIBSMun gIBSMun { get; set; }

        private decimal _vIBS;
        [XmlElement("vIBS")]
        public string vIBS
        {
            get => _vIBS.ToString("F2", CultureInfo.InvariantCulture);
            set => _vIBS = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vCredPres;
        [XmlElement("vCredPres")]
        public string vCredPres
        {
            get => _vCredPres.ToString("F2", CultureInfo.InvariantCulture);
            set => _vCredPres = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vCredPresCondSus;
        [XmlElement("vCredPresCondSus")]
        public string vCredPresCondSus
        {
            get => _vCredPresCondSus.ToString("F2", CultureInfo.InvariantCulture);
            set => _vCredPresCondSus = decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }

    public class TotgIBSUF
    {
        private decimal _vDif;
        [XmlElement("vDif")]
        public string vDif
        {
            get => _vDif.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDif = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vDevTrib;
        [XmlElement("vDevTrib")]
        public string vDevTrib
        {
            get => _vDevTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDevTrib = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vIBSUF;
        [XmlElement("vIBSUF")]
        public string vIBSUF
        {
            get => _vIBSUF.ToString("F2", CultureInfo.InvariantCulture);
            set => _vIBSUF = decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }

    public class TotgIBSMun
    {
        private decimal _vDif;
        [XmlElement("vDif")]
        public string vDif
        {
            get => _vDif.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDif = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vDevTrib;
        [XmlElement("vDevTrib")]
        public string vDevTrib
        {
            get => _vDevTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDevTrib = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vIBSMun;
        [XmlElement("vIBSMun")]
        public string vIBSMun
        {
            get => _vIBSMun.ToString("F2", CultureInfo.InvariantCulture);
            set => _vIBSMun = decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }

    public class TotgCBS
    {
        private decimal _vDif;
        [XmlElement("vDif")]
        public string vDif
        {
            get => _vDif.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDif = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vDevTrib;
        [XmlElement("vDevTrib")]
        public string vDevTrib
        {
            get => _vDevTrib.ToString("F2", CultureInfo.InvariantCulture);
            set => _vDevTrib = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vCBS;
        [XmlElement("vCBS")]
        public string vCBS
        {
            get => _vCBS.ToString("F2", CultureInfo.InvariantCulture);
            set => _vCBS = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vCredPres;
        [XmlElement("vCredPres")]
        public string vCredPres
        {
            get => _vCredPres.ToString("F2", CultureInfo.InvariantCulture);
            set => _vCredPres = decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private decimal _vCredPresCondSus;
        [XmlElement("vCredPresCondSus")]
        public string vCredPresCondSus
        {
            get => _vCredPresCondSus.ToString("F2", CultureInfo.InvariantCulture);
            set => _vCredPresCondSus = decimal.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
