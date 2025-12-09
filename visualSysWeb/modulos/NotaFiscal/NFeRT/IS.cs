using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    /*
     * Informações do Imposto Seletivo
     */
    public class IS
    {
        [XmlIgnore]
        public decimal _vBc;
        [XmlIgnore]
        public decimal _vBCIS;
        [XmlIgnore]
        public decimal _pIS;
        [XmlIgnore]
        public string _uTrib;
        [XmlIgnore]
        public decimal _qTrib;
        [XmlIgnore]
        public decimal _vIS;

        /// <summary>
        ///     S06 - Código de Situação Tributária da COFINS
        /// </summary>
        /// 
        [XmlElement("CSTIS")]
        public string CSTIS { get; set; }
        [XmlElement("cClassTribIS")]
        public string cClassTribIS { get; set; }
        //
        [XmlElement("vBCIS")]
        public string vBCIS
        {
            get { return _vBCIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vBCIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        //
        [XmlElement("pIS")]
        public string pIS
        {
            get { return _pIS.ToString("F4", System.Globalization.CultureInfo.InvariantCulture); }
            set { _pIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        //
        [XmlElement("uTrib")]
        public string uTrib
        {
            get { return _uTrib; }
            set { _uTrib = value; }
        }
        //
        [XmlElement("qTrib")]
        public string qTrib
        {
            get { return _qTrib.ToString("F4", System.Globalization.CultureInfo.InvariantCulture); }
            set { _qTrib = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
        //
        [XmlElement("vIS")]
        public string vIS
        {
            get { return _vIS.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); }
            set { _vIS = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }
        }
    }

}