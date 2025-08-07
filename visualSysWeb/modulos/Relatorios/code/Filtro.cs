using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace visualSysWeb.modulos.Relatorios.code
{
    public class Filtro
    {
        public int ordem = 0;
        public String tipo = "";
        public String titulo = "";

        public String campo = "";
        public String valor = "";
        public String valorPadrao = "";
        public ArrayList lista = new ArrayList();
        public String sql = "";
        public String filtroPai = "";
        //public String campoFiltroPreenche="";
        public bool obrigatorio = false;
        public String campofiltroPai = "";
        public ArrayList camposFiltro = new ArrayList();
        public ArrayList filtrosPreenche = new ArrayList();

        public String sqlDetalhe = "";


        public String SqlPai(String campo)
        {

            if (!campo.Equals(""))
            {
                if (campo.Contains("|"))
                    return sql + " where '" + campo + "' like '%|'+" + campofiltroPai + "+'|%' ";
                else
                    return sql + " where " + campofiltroPai + " = '" + campo + "' ";
            }
            else
            {
                return sql;
            }

        }


        public String largura = "";
        //public String filtroPreenche = "";
        //public String campoPreenche = "";
        public bool autocompleta = false;
        public bool habilitado = true;

        public String nome()
        {
            String nome = "";
            switch (tipo.ToUpper())
            {
                case "TEXTO":

                    return "o" + ordem.ToString("D2") + "TEXTO" + campo;
                case "COMBO":
                    return "o" + ordem + "COMBO" + campo;
                case "LISTA":
                    return "o" + ordem.ToString("D2") + "LISTA" + campo;
                case "LISTATEXTO":
                    return "o" + ordem.ToString("D2") + "LISTATEXTO" + campo;
                case "LISTAFILTRO":
                    return "o" + ordem.ToString("D2") + "LISTAFILTRO" + campo;

                case "DATA":
                    return "o" + ordem.ToString("D2") + "DATAtext" + campo;
                case "HORA":
                    return "o" + ordem.ToString("D2") + "HORAtext" + campo;

                    break;
                    //Sinto Muito Me Perdoe Agradeço Eu Te Amo.


            }
            return nome;
        }
    }
}