using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT.Cobranca
{
    public class fat
    {
        private decimal? _vOrig;
        private decimal? _vDesc;
        private decimal? _vLiq;

        private decimal? RoundNullable(decimal? v, int casas)
            => v.HasValue ? Math.Round(v.Value, casas) : (decimal?)null;

        /// <summary>
        /// Y03 - Número da Fatura
        /// </summary>
        [XmlElement(Order = 1)]
        public string nFat { get; set; }

        /// <summary>
        /// Y04 - Valor Original da Fatura
        /// </summary>
        [XmlElement(Order = 2)]
        public decimal? vOrig
        {
            get => RoundNullable(_vOrig, 2);
            set => _vOrig = RoundNullable(value, 2);
        }

        /// <summary>
        /// Y05 - Valor do Desconto
        /// </summary>
        [XmlElement(Order = 3)]
        public decimal? vDesc
        {
            get => RoundNullable(_vDesc, 2);
            set => _vDesc = RoundNullable(value, 2);
        }

        /// <summary>
        /// Y06 - Valor Líquido da Fatura
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal? vLiq
        {
            get => RoundNullable(_vLiq, 2);
            set => _vLiq = RoundNullable(value, 2);
        }

        public bool ShouldSerializevOrig() => vOrig.HasValue;
        public bool ShouldSerializevDesc() => vDesc.HasValue;
        public bool ShouldSerializevLiq() => vLiq.HasValue;
    }
}