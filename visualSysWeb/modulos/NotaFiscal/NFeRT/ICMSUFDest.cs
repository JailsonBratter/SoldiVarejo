using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class ICMSUFDest
    {
        private decimal _vBcufDest;
        private decimal? _pFcpufDest;
        private decimal _pIcmsufDest;
        private decimal _pIcmsInter;
        private decimal _pIcmsInterPart;
        private decimal? _vFcpufDest;
        private decimal _vIcmsufDest;
        private decimal _vIcmsufRemet;
        private decimal? _vBcfcpufDest;

        private decimal? RoundNullable(decimal? v, int casas)
            => v.HasValue ? Math.Round(v.Value, casas) : (decimal?)null;

        private decimal Round(decimal v, int casas)
            => Math.Round(v, casas);

        // ---------------- NA03 ----------------
        [XmlElement(Order = 1)]
        public decimal vBCUFDest
        {
            get => _vBcufDest;
            set => _vBcufDest = Round(value, 2);
        }

        // ---------------- NA04 ----------------
        [XmlElement(Order = 2)]
        public decimal? vBCFCPUFDest
        {
            get => RoundNullable(_vBcfcpufDest, 2);
            set => _vBcfcpufDest = RoundNullable(value, 2);
        }

        public bool vBCFCPUFDestSpecified => vBCFCPUFDest.HasValue;

        // ---------------- NA05 ----------------
        [XmlElement(Order = 3)]
        public decimal? pFCPUFDest
        {
            get => _pFcpufDest;
            set => _pFcpufDest = RoundNullable(value, 4);
        }

        public bool pFCPUFDestSpecified => pFCPUFDest.HasValue;

        // ---------------- NA07 ----------------
        [XmlElement(Order = 4)]
        public decimal pICMSUFDest
        {
            get => _pIcmsufDest;
            set => _pIcmsufDest = Round(value, 4);
        }

        // ---------------- NA09 ----------------
        [XmlElement(Order = 5)]
        public decimal pICMSInter
        {
            get => _pIcmsInter;
            set => _pIcmsInter = Round(value, 2);
        }

        // ---------------- NA11 ----------------
        [XmlElement(Order = 6)]
        public decimal pICMSInterPart
        {
            get => _pIcmsInterPart;
            set => _pIcmsInterPart = Round(value, 4);
        }

        // ---------------- NA13 ----------------
        [XmlElement(Order = 7)]
        public decimal? vFCPUFDest
        {
            get => _vFcpufDest;
            set => _vFcpufDest = RoundNullable(value, 2);
        }

        public bool vFCPUFDestSpecified => vFCPUFDest.HasValue;

        // ---------------- NA15 ----------------
        [XmlElement(Order = 8)]
        public decimal vICMSUFDest
        {
            get => _vIcmsufDest;
            set => _vIcmsufDest = Round(value, 2);
        }

        // ---------------- NA17 ----------------
        [XmlElement(Order = 9)]
        public decimal vICMSUFRemet
        {
            get => _vIcmsufRemet;
            set => _vIcmsufRemet = Round(value, 2);
        }
    }
}
