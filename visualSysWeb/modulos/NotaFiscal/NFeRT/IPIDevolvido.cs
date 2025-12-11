using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class IPIDevolvido
    {
        private decimal _vIpiDevol;

        /// <summary>
        ///     UA04 - Valor do IPI devolvido
        /// </summary>
        public decimal vIPIDevol
        {
            get { return _vIpiDevol; }
            set { _vIpiDevol = value.Arredondar(2); }
        }
    }
}