using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT.Intermediador
{
    public class infIntermed
    {
        /// <summary>
        ///     YB02 - CNPJ do Intermediador da Transação (agenciador, plataforma de delivery, marketplace e similar) de serviços e de negócios.
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        ///     YB03 - Identificador cadastrado no intermediador
        /// </summary>
        public string idCadIntTran { get; set; }
    }
}