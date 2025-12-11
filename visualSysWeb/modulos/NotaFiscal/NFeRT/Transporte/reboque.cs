using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT.Transporte
{
    public class reboque
    {
        /// <summary>
        ///     X23 - Placa do Veículo
        /// </summary>
        public string placa { get; set; }

        /// <summary>
        ///     X24 - Sigla da UF
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        ///     X25 - Registro Nacional de Transportador de Carga (ANTT)
        /// </summary>
        public string RNTC { get; set; }
    }
}