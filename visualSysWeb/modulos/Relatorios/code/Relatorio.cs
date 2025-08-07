using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using visualSysWeb.dao;
using visualSysWeb.code;


namespace visualSysWeb.modulos.Relatorios.code
{
    public class Relatorio
    {
        public String nomeRelatorio = "";
        public String pasta = "";
        public String enderecoImg = "";
        public String cabecalho = "";
        public ArrayList nomesColunas = new ArrayList();
        private ArrayList ArTotais = new ArrayList();
        private String cpgruptotais = "";
        public bool formataRelatorio = true;
        public String campoAgrupaTotais
        {
            get
            {
                if (cpgruptotais.IndexOf("filtro") >= 0)
                {
                    Filtro fl = (Filtro)filtros[int.Parse(cpgruptotais.Substring(6))];
                    return fl.valor;
                }
                else
                {
                    return cpgruptotais;
                }
            }
            set { cpgruptotais = value; }
        }
        public int qtdCampos;
        private ArrayList ArTotaisGrupo = new ArrayList();
        public Hashtable grupoAcumulado = new Hashtable();
        public ArrayList ArCamposSemformatacao = new ArrayList();
        public String tipoConsulta;
        public String strSqlFiltros = "";
        public String strSqlSimplificado = "";

        public bool naoOrdenar = false;

        private String strSql = "";
        private String strFilial = "MATRIZ";
        public String strfiltros = "";
        public String url = "";
        private String vPagina = "";
        public String pagina
        {
            get
            {
                String strFiltros = "";
                for (int i = 0; i < filtros.Count; i++)
                {
                    Filtro fl = (Filtro)filtros[i];
                    if (fl.valor.Trim().Length > 0)
                    {
                        if (strFiltros.Length > 0)
                            strFiltros += "&";
                        if (fl.tipo.ToUpper().Equals("DATA"))
                        {
                            strFiltros += fl.campo + "=" + DateTime.Parse(fl.valor).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            strFiltros += fl.campo + "=" + fl.valor;
                        }
                    }
                }

                bool exitPara = vPagina.Contains("?");

                return url + vPagina + (exitPara ? "&" : "?") + strFiltros;
            }
            set
            {
                vPagina = value;
            }
        }

        public String sql
        {
            get
            {
                strSqlFiltros = "";
                if (tipoConsulta.ToUpper().Equals("SQL"))
                {

                    for (int i = 0; i < filtros.Count; i++)
                    {
                        Filtro fl = (Filtro)filtros[i];
                        if (!fl.valor.Trim().Equals("") && !fl.campo.Trim().Equals("") && !fl.valor.Equals("TODOS"))
                        {
                            if (i != 0 && !strSqlFiltros.Trim().Equals(""))
                            {
                                strSqlFiltros += " and ";
                            }

                            switch (fl.tipo.ToUpper())
                            {
                                case "TEXTO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " like '%" + fl.valor + "%'";
                                    break;
                                case "LISTA":

                                    strSqlFiltros += " " + fl.campo.ToString() + " in (" + fl.valor + ")";
                                    break;
                                case "LISTATEXTO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " ='" + fl.valor + "'";
                                    break;

                                case "COMBO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " = '" + fl.valor + "'";
                                    break;
                                case "DATA":
                                    strSqlFiltros += " " + fl.campo.ToString() + " = '" + DateTime.Parse(fl.valor).ToString("yyyyMMdd") + "'";
                                    break;


                            }
                        }
                    }
                    if (!strSqlFiltros.Equals(""))
                    {
                        return strSql + " Where " + strSqlFiltros;
                    }
                    else
                    {
                        return strSql;
                    }

                }
                else
                {
                    for (int i = 0; i < filtros.Count; i++)
                    {
                        Filtro fl = (Filtro)filtros[i];
                        if (i != 0)
                        {
                            strSqlFiltros += ", ";
                        }
                        switch (fl.tipo.ToUpper())
                        {
                            case "DATA":
                                strSqlFiltros += " @" + fl.campo.ToString() + " = '" + DateTime.Parse(fl.valor).ToString("yyyyMMdd") + "'";

                                break;
                            default:
                                if (!fl.campo.Trim().Equals(""))
                                    strSqlFiltros += " @" + fl.campo.ToString() + " = '" + fl.valor + "'";

                                break;
                        }
                    }

                    return "exec " + strSql + " @Filial='" + strFilial + "'," + strSqlFiltros + " ";

                }
            }
            set { strSql = value; }
        }
        public String tituloImpSimplificado = "";

        public String sqlSimplificado
        {
            get
            {
                strSqlFiltros = "";
                if (tipoConsulta.ToUpper().Equals("SQL"))
                {

                    for (int i = 0; i < filtros.Count; i++)
                    {
                        Filtro fl = (Filtro)filtros[i];
                        if (!fl.valor.Trim().Equals("") && !fl.campo.Trim().Equals("") && !fl.valor.Equals("TODOS"))
                        {
                            if (i != 0 && !strSqlFiltros.Trim().Equals(""))
                            {
                                strSqlFiltros += " and ";
                            }

                            switch (fl.tipo.ToUpper())
                            {
                                case "TEXTO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " like '%" + fl.valor + "%'";
                                    break;
                                case "LISTA":

                                    strSqlFiltros += " " + fl.campo.ToString() + " in (" + fl.valor + ")";
                                    break;
                                case "LISTATEXTO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " ='" + fl.valor + "'";
                                    break;

                                case "COMBO":
                                    strSqlFiltros += " " + fl.campo.ToString() + " = '" + fl.valor + "'";
                                    break;
                                case "DATA":
                                    strSqlFiltros += " " + fl.campo.ToString() + " = '" + DateTime.Parse(fl.valor).ToString("yyyyMMdd") + "'";
                                    break;


                            }
                        }
                    }
                    if (!strSqlFiltros.Equals(""))
                    {
                        return strSqlSimplificado + " Where " + strSqlFiltros;
                    }
                    else
                    {
                        return strSqlSimplificado;
                    }

                }
                else
                {
                    for (int i = 0; i < filtros.Count; i++)
                    {
                        Filtro fl = (Filtro)filtros[i];
                        if (i != 0)
                        {
                            strSqlFiltros += ", ";
                        }
                        switch (fl.tipo.ToUpper())
                        {
                            case "DATA":
                                strSqlFiltros += " @" + fl.campo.ToString() + " = '" + DateTime.Parse(fl.valor).ToString("yyyyMMdd") + "'";

                                break;
                            default:
                                if (!fl.campo.Trim().Equals(""))
                                    strSqlFiltros += " @" + fl.campo.ToString() + " = '" + fl.valor + "'";

                                break;
                        }
                    }

                    return "exec " + strSqlSimplificado + " @Filial='" + strFilial + "'," + strSqlFiltros + " ";

                }
            }
            set { strSqlSimplificado = value; }
        }

        private static String strOrdem = "";
        public String ordem
        {
            get
            {
                return strOrdem;
            }
            set
            {
                if (!value.Equals("") && strOrdem.Equals(value))
                {
                    strOrdem = value + " desc";

                }
                else
                {
                    strOrdem = value;
                }
            }
        }

        public String rodape = "";
        public ArrayList filtros = new ArrayList();

        public Relatorio(String pasta)
        {
            this.pasta = pasta;
        }
        public Relatorio(String pasta, String relatorio, String url)
        {
            this.pasta = pasta;
            RelatorioIO reIO = new RelatorioIO(pasta);
            this.url = "~/modulos/Relatorios/pages/RelatoriosXml/" + url + "/page/";

            if (reIO.verificaRelatorio(relatorio))
            {
                carregarRelatorio(relatorio);
            }
        }


        private void addTotaisGrupo(bool acumular, String campo, String posicao)
        {
            GrupoSubtotal total = new GrupoSubtotal();
            total.acumular = acumular;
            total.Campo = campo;
            total.posicao = int.Parse(posicao);
            ArTotaisGrupo.Add(total);
        }

        public void setTotalGrupo(String campo, Decimal valor)
        {

            if (grupoAcumulado.Contains(campo))
            {
                Decimal vlt = (Decimal)grupoAcumulado[campo];
                vlt += valor;
                grupoAcumulado[campo] = vlt;

            }
            else
            {
                grupoAcumulado.Add(campo, valor);
            }

        }

        public void zerarTotaisGrupo()
        {
            foreach (GrupoSubtotal sub in ArTotaisGrupo)
            {
                sub.Valor = 0;

            }
        }

        public String getTotalGrupo(int campo)
        {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.

            foreach (GrupoSubtotal sub in ArTotaisGrupo)
            {

                if (sub.posicao.Equals(campo))
                {
                    if (sub.acumular)
                    {
                        try
                        {


                            Decimal vlr = (Decimal)grupoAcumulado[sub.Campo];
                            return vlr.ToString("N2");
                        }
                        catch (Exception)
                        {

                            return "0,00";
                        }
                    }
                    else

                        return sub.Valor.ToString("N2");

                }
            }
            return "";
        }
        public ArrayList getTotalGrupo()
        {
            return ArTotaisGrupo;
        }


        public void AddnomeColuna(String nome)
        {
            nomesColunas.Add(nome);
        }

        public void addFiltros(Filtro filtro)
        {
            filtros.Add(filtro);
        }

        public void addTotal(Total sbt)
        {
            ArTotais.Add(sbt);
        }

        public ArrayList getTotaisSimples(User usr)
        {

            foreach (Total sub in ArTotais)
            {
                sub.total = 0;
                DataTable tb = ReltableSimples(usr);
                if (tb.Rows.Count >= 1)
                {
                    foreach (DataRow drw in tb.Rows)
                    {
                        try
                        {

                            String strTd = drw[sub.index].ToString().Replace("-", "");
                            if (!strTd.Trim().Equals("") || strTd.ToUpper().IndexOf("PLU") < 0)
                            {
                                sub.total += decimal.Parse(Convert.ToString(drw[sub.index].ToString().Replace("-", "").Replace("|", "")));
                            }
                        }
                        catch (Exception)
                        {
                            sub.total = -1;
                            break;
                        }
                    }
                }

                //    sub.total = decimal.Parse(Conexao.retornaUmValor("select Sum(" + sub.campo + ") FROM (" + sql + " ) aTOTAL", usr));
            }




            return ArTotais;
        }

        public ArrayList getTotais(User usr)
        {


            foreach (Total sub in ArTotais)
            {
                sub.total = 0;
                DataTable tb = Reltable(usr);
                if (tb.Rows.Count >= 1)
                {
                    foreach (DataRow drw in tb.Rows)
                    {
                        try
                        {
                            if (!drw[sub.index].ToString().Contains("|-SUB-|"))
                            {
                                String strTd = drw[sub.index].ToString().Replace("|-NI-|", "").Replace("-", "").Replace("NUI_", "").Replace(".", ",");
                                if (!strTd.Trim().Equals("") || strTd.ToUpper().IndexOf("PLU") < 0)
                                {
                                    if (!strTd.Contains("NTOTAL_"))
                                    {
                                        if (drw[sub.index].ToString().Contains("|-NI-|") || drw[sub.index].ToString().Contains("NUI_"))
                                        {
                                            Decimal vlr = 0;
                                            Decimal.TryParse(strTd, out vlr);
                                            sub.total += vlr;
                                            sub.inteiro = true;
                                        }
                                        else
                                        {



                                            Decimal vlr = 0;
                                            String str = Convert.ToString(drw[sub.index].ToString().Replace("|", ""));
                                            if (!Funcoes.isnumero(str))
                                            {
                                                throw new Exception();
                                            }
                                            Decimal.TryParse(str, out vlr);


                                            sub.total += Decimal.Round(vlr, 2);
                                        }
                                    }
                                    else
                                    {


                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            sub.total = -1;
                            break;
                        }
                    }
                }

                //    sub.total = decimal.Parse(Conexao.retornaUmValor("select Sum(" + sub.campo + ") FROM (" + sql + " ) aTOTAL", usr));
            }




            return ArTotais;
        }
        public void setTotal(int index, String valor)
        {
            foreach (Total sub in ArTotais)
            {
                if (sub.index.Equals(index))
                {
                    sub.total += decimal.Parse(valor);
                }
            }

        }


        public void limpaTotais()
        {
            foreach (Total sub in ArTotais)
            {
                sub.total = 0;

            }

        }

        public DataTable ReltableSimples(User usr)
        {
            strFilial = usr.getFilial();
            DataTable tb = Conexao.GetTable(sqlSimplificado, usr, false);

            DataView dv = tb.DefaultView;


            dv.Sort = ordem;

            tb = dv.ToTable();

            return tb;
        }

        public DataTable tb = null;
        public DataTable Reltable(User usr)
        {


            strFilial = usr.getFilial();

            if (tb == null)
            {
                tb = Conexao.GetTable(sql, usr, false);
            }
            DataView dv = tb.DefaultView;


            dv.Sort = ordem;

            tb = dv.ToTable();

            return tb;
        }

        public DataTable ReltableEstrutura(User usr)
        {
            DataTable tb = Conexao.GetTable("select top 0 " + sql.TrimStart().Substring(7), usr, false);

            DataView dv = tb.DefaultView;
            dv.Sort = ordem;

            tb = dv.ToTable();

            return tb;
        }


        public DataTable RelSubtotais(User usr)
        {
            String sqlSub = ""; ;




            return Conexao.GetTable(sqlSub, usr, true);

        }


        private void carregarRelatorio(String relatorio)
        {
            XPathNavigator rs;
            XPathDocument docNav;
            nomeRelatorio = relatorio;
            docNav = new XPathDocument(pasta + relatorio.Replace(" ", "_") + ".xml");

            rs = docNav.CreateNavigator();
            enderecoImg = retornaValorNo(rs, "enderecoImg");
            cabecalho = retornaValorNo(rs, "cabecalho");
            naoOrdenar = retornaValorNo(rs, "naoOrdenar").ToUpper().Equals("TRUE");
            tipoConsulta = retornaValorNo(rs, "tipoConsulta");
            formataRelatorio = !retornaValorNo(rs, "formatacao").ToUpper().Equals("FALSE");
            pagina = retornaValorNo(rs, "pagina");
            sql = retornaValorNo(rs, "sql");
            sqlSimplificado = retornaValorNo(rs, "sqlImpressaoSimples");
            tituloImpSimplificado = retornaValorNo(rs, "tituloImpSimples");

            ordem = retornaValorNo(rs, "order");
            rs.MoveToRoot();
            rs.MoveToFirstChild();
            rs.MoveToChild("colunas", "");
            rs.MoveToFirstChild();
            while (rs.MoveToNext())
            {
                AddnomeColuna(rs.Value);
            }



            rs.MoveToRoot();
            rs.MoveToFirstChild();

            rs.MoveToChild("filtros", "");
            // string t = "filtros" + filtros.Count;
            filtros.Clear();
            while (rs.MoveToChild("filtro" + filtros.Count, ""))
            {
                Filtro filtro = new Filtro();

                filtro.tipo = retornaValorNoAtual(rs, "tipo");
                filtro.titulo = retornaValorNoAtual(rs, "titulo");
                filtro.valor = retornaValorNoAtual(rs, "valorPadrao");
                filtro.valorPadrao = filtro.valor;
                filtro.obrigatorio = retornaValorNoAtual(rs, "obrigatorio").ToUpper().Equals("TRUE");
                if (rs.MoveToChild("sql", ""))
                {
                    filtro.sql = rs.Value;
                    rs.MoveToParent();
                }
                else
                {
                    if (rs.MoveToChild("lista", ""))
                    {
                        rs.MoveToFirstChild();
                        filtro.lista.Add(rs.Value);
                        while (rs.MoveToNext())
                        {
                            filtro.lista.Add(rs.Value);
                        }
                        rs.MoveToParent();
                        rs.MoveToParent();
                    }
                }


                if (rs.MoveToChild("camposFiltros", ""))
                {
                    rs.MoveToFirstChild();
                    filtro.camposFiltro.Add(rs.Value);
                    while (rs.MoveToNext())
                    {
                        filtro.camposFiltro.Add(rs.Value);
                    }
                    rs.MoveToParent();
                    rs.MoveToParent();
                }

                filtro.campo = retornaValorNoAtual(rs, "campo");
                filtro.campofiltroPai = retornaValorNoAtual(rs, "campofiltroPai");
                filtro.filtroPai = retornaValorNoAtual(rs, "filtropai");

                filtro.largura = retornaValorNoAtual(rs, "largura");

                String hab = retornaValorNoAtual(rs, "habilitado");
                if (!hab.Equals(""))
                    filtro.habilitado = hab.ToUpper().Equals("TRUE");

                if (rs.MoveToChild("preencher", ""))
                {
                    rs.MoveToFirstChild();
                    ArrayList lsPrenche = new ArrayList();
                    rs.MoveToFirstChild();
                    lsPrenche.Add(rs.Value);
                    rs.MoveToNext();
                    lsPrenche.Add(rs.Value);
                    filtro.filtrosPreenche.Add(lsPrenche);
                    rs.MoveToParent();
                    while (rs.MoveToNext())
                    {
                        lsPrenche = new ArrayList();
                        rs.MoveToFirstChild();
                        lsPrenche.Add(rs.Value);
                        rs.MoveToNext();
                        lsPrenche.Add(rs.Value);
                        filtro.filtrosPreenche.Add(lsPrenche);
                        rs.MoveToParent();
                    }
                    rs.MoveToParent();
                    rs.MoveToParent();
                }

                // filtro.filtroPreenche = retornaValorNoAtual(rs, "filtroPreenche");
                // filtro.campoFiltroPreenche = retornaValorNoAtual(rs, "campofiltroPreenche");


                //  filtro.campoPreenche = retornaValorNoAtual(rs, "campoPreenche");

                // filtro.autocompleta = (retornaValorNoAtual(rs, "sqlAutocompleta").Trim().ToUpper().Equals("TRUE"));

                rs.MoveToParent();
                filtro.ordem = filtros.Count;
                addFiltros(filtro);
            }
            rs.MoveToRoot();
            rs.MoveToFirstChild();
            bool teste = rs.MoveToChild("subtotais", "");
            while (rs.MoveToChild("sub" + ArTotais.Count, ""))
            {
                Total sub = new Total();
                try
                {
                    sub.index = int.Parse(retornaValorNoAtual(rs, "index"));
                }
                catch (Exception e)
                {
                    throw new Exception("Index do Subtotal " + sub.titulo + " Invalido ");
                }

                sub.titulo = retornaValorNoAtual(rs, "titulo");
                sub.campo = retornaValorNoAtual(rs, "campo");

                addTotal(sub);
                rs.MoveToParent();
            }

            rs.MoveToRoot();
            rs.MoveToFirstChild();
            rs.MoveToChild("grupoTotais", "");
            campoAgrupaTotais = retornaValorNoAtual(rs, "agrupar");
            String qtCampos = retornaValorNoAtual(rs, "totalCampos");
            qtdCampos = (qtCampos.Equals("") ? 0 : int.Parse(qtCampos));

            rs.MoveToRoot();
            rs.MoveToFirstChild();
            rs.MoveToChild("grupoTotais", "");
            while (rs.MoveToChild("somar" + ArTotaisGrupo.Count, ""))
            {
                bool acumular = retornaValorNoAtual(rs, "acumular").ToUpper().Equals("TRUE");
                String strcampo = retornaValorNoAtual(rs, "campo");
                String strposicao = retornaValorNoAtual(rs, "posicao");
                addTotaisGrupo(acumular, strcampo, strposicao);
                rs.MoveToParent();
            }

            rs.MoveToRoot();
            rs.MoveToFirstChild();
            if (rs.MoveToChild("camposSemformatacao", ""))
            {
                rs.MoveToFirstChild();
                ArCamposSemformatacao.Add(rs.Value);
                while (rs.MoveToNext())
                {
                    ArCamposSemformatacao.Add(rs.Value);
                }
            }

            rodape = retornaValorNo(rs, "rodape");




        }

        public bool semFormatacao(int coluna)
        {
            foreach (String item in ArCamposSemformatacao)
            {
                if (item.Equals(coluna.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private String retornaValorNo(XPathNavigator rs, String no)
        {
            try
            {
                rs.MoveToRoot();
                rs.MoveToFirstChild();
                if (rs.MoveToChild(no, ""))
                    return rs.Value;
                else
                    return "";
            }
            catch (Exception)
            {

                return "";
            }

        }
        private String retornaValorNoAtual(XPathNavigator rs, String no)
        {
            string valor = "";
            if (rs.MoveToChild(no, ""))
            {
                valor = rs.Value;
                rs.MoveToParent();
            }
            return valor;
        }
        private void Criar()
        {

            // Instancia o objeto ‘doc’ como XML
            XmlDocument doc = new XmlDocument();

            // Define o header do XML
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            // Adiciona o header ao XML
            doc.AppendChild(docNode);

            XmlNode Relatorio = doc.CreateElement("Relatorio");
            doc.AppendChild(Relatorio);



            XmlNode xmlEnderecoImg = doc.CreateElement("enderecoImg");
            xmlEnderecoImg.Value = enderecoImg;
            Relatorio.AppendChild(xmlEnderecoImg);

            XmlNode xmlCabecalho = doc.CreateElement("cabecalho");
            xmlCabecalho.Value = cabecalho;
            Relatorio.AppendChild(xmlCabecalho);

            XmlNode xmlColunas = doc.CreateElement("colunas");
            Relatorio.AppendChild(xmlColunas);

            for (int i = 0; i < nomesColunas.Count; i++)
            {
                XmlNode xmlNomeColuna = doc.CreateElement("nome");
                xmlNomeColuna.Value = nomesColunas[i].ToString();
                xmlColunas.AppendChild(xmlNomeColuna);
            }




            XmlNode xmlFiltros = doc.CreateElement("filtros");
            Relatorio.AppendChild(xmlFiltros);

            for (int i = 0; i < filtros.Count; i++)
            {
                XmlNode xmlfiltro = doc.CreateElement("filtro");
                Filtro filtro = (Filtro)filtros[i];

                XmlNode xmltipo = doc.CreateElement("tipo");
                xmltipo.Value = filtro.tipo;
                xmlfiltro.AppendChild(xmltipo);


                XmlNode xmlTitulo = doc.CreateElement("titulo");
                xmlTitulo.Value = filtro.titulo;
                xmlfiltro.AppendChild(xmlTitulo);

                XmlNode xmlValorPadrao = doc.CreateElement("valorPadrao");
                xmltipo.Value = filtro.tipo;
                xmlfiltro.AppendChild(xmltipo);

                XmlNode xmlCampo = doc.CreateElement("campo");
                xmltipo.Value = filtro.tipo;
                xmlfiltro.AppendChild(xmltipo);

                xmlFiltros.AppendChild(xmlfiltro);
            }

            XmlNode xmlRodape = doc.CreateElement("rodape");
            xmlRodape.Value = rodape;
            Relatorio.AppendChild(xmlRodape);

            // Salva o XML no PATH informado
            doc.Save(pasta + "\\" + nomeRelatorio + ".xml");
        }

    }
}