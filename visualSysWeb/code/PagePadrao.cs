using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using visualSysWeb.dao;
using AjaxControlToolkit;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace visualSysWeb.code
{
    public abstract class PagePadrao : System.Web.UI.Page
    {


        private bool sessaoatu = true;
        private static String ultimostatus = "visualizar";
        protected String status
        {
            get
            {
                try
                {

                
                    User usr = (User)Session["User"];
                    if (usr == null)
                        return "";
                    String url = Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                    String sts = (String)Session["status" + url];
                
                    if (sts == null)
                    {
                        ultimostatus = "visualizar";
                        return "visualizar";
                    }
                    else
                    {
                        ultimostatus = sts;
                        return sts;
                    }
                }
                catch (Exception )
                {

                    return ultimostatus;
                }

            }
            set
            {
                User usr = (User)Session["User"];
                if (usr != null)
                {
                    String url = Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                    Session.Remove("status" + url);
                    Session.Add("status" + url, value);
                }
            }
        }


        protected void carregabtn(Panel pn)
        {
            switch (status)
            {
                case "visualizar":
                    visualizar(pn);
                    break;

                case "incluir":
                    incluir(pn);
                    break;
                case "editar":
                    editar(pn);
                    break;
                case "pesquisar":
                    pesquisar(pn);
                    break;
                case "sopesquisa":
                    sopesquisa(pn);
                    break;


            }

            CriarLista(pn);


            controleSessaoTela();

        }

        protected void carregabtn(Panel pn, String btnIncluir, String btnConfirmar, String btnEditar, String btnCancelar, String btnExcluir, String btnPesquisar)
        {
            switch (status)
            {
                case "visualizar":
                    visualizar(pn, btnIncluir, btnEditar, btnExcluir, btnPesquisar);
                    break;

                case "incluir":
                    incluir(pn, btnConfirmar, btnCancelar);
                    break;
                case "editar":
                    editar(pn, btnConfirmar, btnCancelar);
                    break;
                case "pesquisar":
                    pesquisar(pn, btnPesquisar, btnIncluir);
                    break;
                case "sopesquisa":
                    sopesquisa(pn, btnPesquisar);
                    break;


            }

            CriarLista(pn);


            controleSessaoTela();

        }



        public void CriarLista(Panel pn)
        {

        }

        protected void carregabtn(Panel pn, bool semExcluir)
        {
            switch (status)
            {
                case "visualizar":
                    visualizar(pn);
                    break;
                case "incluir":
                    incluir(pn);
                    break;
                case "editar":
                    editar(pn);
                    break;
                case "pesquisar":
                    pesquisar(pn);
                    break;
                case "sopesquisa":
                    sopesquisa(pn);
                    break;


            }

            if (semExcluir)
            {
                Panel btn = (Panel)pn.FindControl("PnEXCLUIR");
                if (btn != null)
                {
                    btn.Visible = false;
                }
            }


        }
        private static int numeropagiansAberta = 0;

        private String ultimaUrl = ""; 
        public String urlSessao()
        {
            try
            {
                ultimaUrl = Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "");
                return ultimaUrl;
            }
            catch (Exception)
            {

                return ultimaUrl;
            }
        }

        public void incluir(Panel pn)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "incluir";
            pnBotoes.Controls.Add(criaBotao("Confirmar"));
            pnBotoes.Controls.Add(criaBotao("Cancelar"));
            pn.Controls.Add(pnBotoes);

        }

        public void incluir(Panel pn, String btnConfirma, String btnCancela)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "incluir";
            if (btnConfirma != null && !btnConfirma.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Confirmar", btnConfirma));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Confirmar"));
            }

            if (btnCancela != null && !btnCancela.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Cancelar", btnCancela));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Cancelar"));
            }
            pn.Controls.Add(pnBotoes);
        }

        protected bool isnumero(String numero)
        {
            try
            {
                decimal number3 = 0;
                bool resultado = Decimal.TryParse(numero, out number3);
                return resultado;
            
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected bool isnumeroint(String numero)
        {
            try
            {
                int number3 = 0;
                bool resultado = int.TryParse(numero, out number3);
                return resultado;
            
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void editar(Panel pn)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "editar";
            pnBotoes.Controls.Add(criaBotao("Confirmar"));
            pnBotoes.Controls.Add(criaBotao("Cancelar"));

            pn.Controls.Add(pnBotoes);

        }
        public void editar(Panel pn, String btnConfirma, String btnCancela)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "editar";
            if (btnConfirma != null && !btnConfirma.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Confirmar", btnConfirma));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Confirmar"));
            }

            if (btnCancela != null && !btnCancela.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Cancelar", btnCancela));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Cancelar"));
            }

            pn.Controls.Add(pnBotoes);

        }

        public void visualizar(Panel pn)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "visualizar";
            pnBotoes.Controls.Add(criaBotao("Incluir"));
            pnBotoes.Controls.Add(criaBotao("Editar"));
            pnBotoes.Controls.Add(criaBotao("Excluir"));
            pnBotoes.Controls.Add(criaBotao("Pesquisar"));

            pn.Controls.Add(pnBotoes);

        }

        public void visualizar(Panel pn, String btnIncluir, String btnEditar, String btnExcluir, String btnPesquisar)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();
            status = "visualizar";
            if (btnIncluir != null && !btnIncluir.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Incluir", btnIncluir));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Incluir"));
            }

            if (btnEditar != null && !btnEditar.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Editar", btnEditar));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Editar"));
            }
            if (btnExcluir != null && !btnExcluir.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Excluir", btnExcluir));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Excluir"));
            }

            if (btnPesquisar != null && !btnPesquisar.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar", btnPesquisar));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar"));
            }

            pn.Controls.Add(pnBotoes);

        }



        public void pesquisar(Panel pn)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();

            pnBotoes.Controls.Add(criaBotao("Pesquisar"));
            pnBotoes.Controls.Add(criaBotao("Incluir"));
            status = "pesquisar";
            pn.Controls.Add(pnBotoes);
        }

        public void pesquisar(Panel pn, String btnPesquisa, String btnIncluir)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();

            if (btnPesquisa != null && !btnPesquisa.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar", btnPesquisa));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar"));
            }
            if (btnIncluir != null && !btnIncluir.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Incluir", btnIncluir));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Incluir"));
            }

            status = "pesquisar";
            pn.Controls.Add(pnBotoes);
        }



        public void sopesquisa(Panel pn)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();


            pnBotoes.Controls.Add(criaBotao("Pesquisar"));
            status = "sopesquisa";
            pn.Controls.Add(pnBotoes);


        }
        public void sopesquisa(Panel pn, String btnPesquisa)
        {
            pn.Controls.Clear();
            Panel pnBotoes = new Panel();

            if (btnPesquisa != null && !btnPesquisa.Equals(""))
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar", btnPesquisa));
            }
            else
            {
                pnBotoes.Controls.Add(criaBotao("Pesquisar"));
            }
            status = "sopesquisa";
            pn.Controls.Add(pnBotoes);


        }

        private Panel criaBotao(String botao, String titulo)
        {
            try
            {
                if(titulo.ToUpper().Equals("NAO"))
                     throw new Exception("Sem Acesso");

                User usr = (User)Session["user"];
                if (Request.Params["tela"] != null)
                {
                    usr.tela = Request.Params["tela"].ToString();
                }

                Panel pnBtn = new Panel();
                pnBtn.ID = "pn" + botao.ToUpper();
                pnBtn.CssClass = "titulobtn";
                ImageButton btn = new ImageButton();
                btn.ID = "btn" + botao;
                String codigoTela = usr.tela;

                switch (botao.ToUpper())
                {
                    case "INCLUIR":
                        if (!usr.incluir(codigoTela))
                        {
                            throw new Exception("Sem Acesso");
                        }
                        btn.Click += btnIncluir_Click;
                        btn.ImageUrl = "~\\img\\add.png";


                        break;
                    case "EDITAR":
                        if (!usr.editar(codigoTela))
                        {
                            throw new Exception("Sem Acesso");
                        }

                        btn.Click += btnEditar_Click;
                        btn.ImageUrl = "~\\img\\edit.png";
                        break;
                    case "EXCLUIR":
                        if (!usr.excluir(codigoTela))
                        {
                            throw new Exception("Sem Acesso");
                        }

                        btn.Click += btnExcluir_Click;
                        btn.ImageUrl = "~\\img\\cancel.png";
                        break;
                    case "PESQUISAR":
                        if (!usr.acesso(codigoTela))
                        {
                            throw new Exception("Sem Acesso");
                        }

                        btn.Click += btnPesquisar_Click;
                        btn.ImageUrl = "~\\img\\pesquisaM.png";
                        break;
                    case "CONFIRMAR":
                        btn.Click += btnConfirmar_Click;
                        btn.ImageUrl = "~\\img\\confirm.png";
                        break;
                    case "CANCELAR":
                        btn.Click += btnCancelar_Click;
                        btn.ImageUrl = "~\\img\\cancel.png";
                        break;

                }
                btn.Width = 20;
                pnBtn.Controls.Add(btn);

                Label lbl = new Label();
                lbl.Text = titulo;
                pnBtn.Controls.Add(lbl);
                return pnBtn;
            }
            catch (Exception)
            {
                return new Panel();
            }

        }
        private Panel criaBotao(String botao)
        {
            return criaBotao(botao, botao);
        }

        protected abstract void btnIncluir_Click(object sender, EventArgs e);
        protected abstract void btnEditar_Click(object sender, EventArgs e);
        protected abstract void btnPesquisar_Click(object sender, EventArgs e);
        protected abstract void btnExcluir_Click(object sender, EventArgs e);
        protected abstract void btnConfirmar_Click(object sender, EventArgs e);
        protected abstract void btnCancelar_Click(object sender, EventArgs e);
        protected abstract bool campoDesabilitado(Control campo);
        protected abstract bool campoObrigatorio(Control campo);



        public void controleSessaoTela()
        {

            if (!IsPostBack && sessaoatu)
            {
                sessaoatu = false;
                String ntela = (String)Session["ntela" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                if (ntela != null && numeropagiansAberta < 50)
                {
                    int tela = 0;
                    String strUrl = Request.Url.ToString();
                    while (ntela != null)
                    {
                        if (strUrl.IndexOf("ntela") > 0)
                        {
                            strUrl = strUrl.ToString().Substring(0, strUrl.ToString().IndexOf("&ntela"));
                            tela++;

                        }
                        strUrl = strUrl + "&ntela=" + tela;
                        ntela = (String)Session["ntela" + strUrl.Replace(" ", "").Replace("+", "").Replace("%", "")];
                    }
                    numeropagiansAberta = tela;
                    Response.Redirect(strUrl);
                }
                else
                {
                    Session.Remove("ntela" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("ntela" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), "0");

                }


            }

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void metodo(String url)
        {
            String str = url;
            HttpContext.Current.Session.Remove("ntela" + url.Replace(" ", "").Replace("+", "").Replace("%20", ""));

        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Decimal verificapreco(Decimal margem, Decimal custoliq)
        {
            return Funcoes.verificapreco(margem, custoliq);
        }
        //Funcoes.verificamargem(dblCusto, dblPrecoNovo, 0, 0);
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static Decimal verificamargem(Decimal custoliq, Decimal venda, Decimal aliqicms, Decimal PISS)
        {
            return Funcoes.verificamargem(custoliq, venda, aliqicms, PISS);
        }



        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static String statusPagina(String url)
        {
            return "visualiza";
            /*String sts = null;

            try
            {

                sts = (String)HttpContext.Current.Session["status" + url.Replace(" ", "").Replace("+", "").Replace("%20", "")];
            }
            catch (Exception)
            {
                return "visualizar";
            }
            if (sts == null)
            {
                return "visualizar";
            }
            else
            {
                return sts;
            }
             */
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static String GetNomeCliente(string prefixText)
        {
            String sql = "	Select ltrim(rtrim(nome_cliente))  from cliente where (codigo_cliente = '" + prefixText + "' ) and isnull(inativo,0)=0";
            return Conexao.retornaUmValor(sql, null);
        }

        public void EnabledButtons(Control parent, bool campo)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ImageButton)
                {
                    ((ImageButton)(c)).Visible = campo;
                }

                EnabledButtons(c, campo);
            }
        }
        public void FormataCamposInteiros(ArrayList campos, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    if (campos.Contains(((TextBox)(c)).ID))
                        ((TextBox)(c)).Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
                }

                FormataCamposInteiros(campos, c);
            }


        }

        public void FormataCamposNumericos(ArrayList campos, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    if (campos.Contains(((TextBox)(c)).ID))
                        ((TextBox)(c)).Attributes.Add("OnKeyPress", "javascript:return formataDouble(this,event);");
                }

                FormataCamposNumericos(campos, c);
            }


        }

        public void FormataCamposDatas(ArrayList campos, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    if (campos.Contains(((TextBox)(c)).ID))
                    {
                        ((TextBox)(c)).Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                        ((TextBox)(c)).MaxLength = 10;
                    }
                }

                FormataCamposDatas(campos, c);
            }

            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }
        public void FormataCamposHora(ArrayList campos, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    //if (campos.Contains(((TextBox)(c)).ID))
                    //    //((TextBox)(c)).Attributes.Add("OnKeyPress", "javascript:return formataHora(this,event);");
                }

                FormataCamposHora(campos, c);
            }


        }





        /*
                protected void DesabilitaBotoes()
                {
                    DisableButtons(botoes);
                    DisableButtons(botoes1);
                }
                protected void habilitaBotoes()
                {
                    EnabledButtons(botoes1);
                    EnabledButtons(botoes);
                }


                */

        public void DisableButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ImageButton)
                {
                    ((ImageButton)(c)).Visible = false;
                }

                DisableButtons(c);
            }
        }


        private void habilitarControl(Control controle, bool habilitado)
        {
            if (controle is TextBox)
            {
                ((TextBox)(controle)).Enabled = habilitado;
                ((TextBox)(controle)).Attributes.Add("onkeydown", "javascript:return autoTab(this,event);");

                switch (((TextBox)(controle)).CssClass.ToUpper())
                {
                    case string a when a.Contains("NUMERO"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
                        break;
                    case string a when a.Contains("DATA"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                        ((TextBox)(controle)).MaxLength = 10;
                        break;
                    case string a when a.Contains("HORA"):
                        ((TextBox)(controle)).MaxLength = 5;
                        ((TextBox)(controle)).Attributes.Add("type", "time");
                        
                        break;
                    case string a when a.Contains("INTEIRO"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
                        break;
                    case string a when a.Contains("TELEFONE"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataTelefone(this,event);");
                        break;
                    case string a when a.Contains("CPF"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataCPF(this,event);");
                        break;
                    case string a when a.Contains("CNPJ"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataCNPJ(this,event);");
                        break;
                    case string a when a.Contains("CEP"):
                        ((TextBox)(controle)).Attributes.Add("OnKeyPress", "javascript:return formataCEP(this,event);");
                        break;
                    case string a when a.Contains("SEM"):
                    case string b when b.Contains("TEXTO-ECOMMERCE-COMERCIAL"):
                        ((TextBox)(controle)).Attributes.Remove("onkeydown");
                        break;

                    default:
                        ((TextBox)(controle)).Attributes.Add("OnChange", "javascript:this.value = this.value.toUpperCase();");
                       

                        break;



                }


                if (!((TextBox)(controle)).Enabled)
                {
                    ((TextBox)(controle)).BackColor = Color.FromArgb(0xDCDCDC);

                }
                else
                {
                    ((TextBox)(controle)).BackColor = Color.White;
                }
                if (campoObrigatorio(controle))
                {
                    if (((TextBox)(controle)).CssClass.ToUpper().Contains("NUMERO"))
                    {
                        ((TextBox)(controle)).CssClass += " numeroObrigatorio";
                    }
                    else
                    {
                        ((TextBox)(controle)).CssClass += " campoObrigatorio";
                    }
                }
            }

            else if (controle is ImageButton)
            {


                ImageButton btn = ((ImageButton)(controle));
                ((ImageButton)(controle)).Visible = habilitado;
                if (btn.ID != null && btn.ID.ToUpper().IndexOf("DT") < 0)
                {
                    PostBackOptions optionsSubmit = new PostBackOptions(btn);
                    btn.OnClientClick = "disableButtonOnClick(this, 'Aguarde...'); ";
                    btn.OnClientClick += ClientScript.GetPostBackEventReference(optionsSubmit);
                }

            }
            else if (controle is DropDownList)
            {
                ((DropDownList)(controle)).Enabled = habilitado;
                if (!((DropDownList)(controle)).Enabled)
                {
                    ((DropDownList)(controle)).BackColor = Color.FromArgb(0xDCDCDC);

                }
                else
                {
                    ((DropDownList)(controle)).BackColor = Color.White;
                }
                if (campoObrigatorio(controle))
                {

                    ((DropDownList)(controle)).CssClass += " campoObrigatorio";
                }
            }
            else if (controle is CheckBoxList)
            {
                ((CheckBoxList)(controle)).Enabled = habilitado;
                if (campoObrigatorio(controle))
                {

                    ((CheckBoxList)(controle)).CssClass += " campoObrigatorio";
                }
            }
            else if (controle is CheckBox)
            {
                ((CheckBox)(controle)).Enabled = habilitado;
                if (campoObrigatorio(controle))
                {

                    ((CheckBox)(controle)).CssClass += " campoObrigatorio";
                }
            }






        }


        public void EnabledControls(Control parent, bool campo)
        {
            foreach (Control c in parent.Controls)
            {
                if ((c is TextBox) || (c is ImageButton) || (c is DropDownList) || (c is CheckBox) || (c is CheckBoxList))
                {
                    if (!campo || !campoDesabilitado(c))
                    {
                        habilitarControl(c, campo);

                    }
                    else
                    {
                        habilitarControl(c, false);

                    }
                }


                EnabledControls(c, campo);
            }

        }
        public void LimparCampos(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)(c)).Text = "";

                }
                LimparCampos(c);
            }

        }


        public bool validaCampos(Control parent)
        {
            bool valor = true;
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    if (campoObrigatorio(c))
                    {
                        if (((TextBox)(c)).Text.Trim().Equals(""))
                        {
                            ((TextBox)(c)).BackColor = Color.Red;
                            valor = false;
                            String erro = " Erro: O Campo " + ((TextBox)(c)).ID.ToUpper().Replace("TXT", "") + " é Obrigatorio!";
                            throw new Exception(erro);
                        }
                        else
                        {
                            ((TextBox)(c)).BackColor = Color.White;
                        }
                    }
                }
                if (c is DropDownList)
                {
                    if (campoObrigatorio(c))
                    {
                        if (((DropDownList)(c)).Text.Trim().Equals(""))
                        {
                            ((DropDownList)(c)).BackColor = Color.Red;
                            valor = false;
                            String erro = " Erro: O Campo " + ((DropDownList)(c)).ID.ToUpper().Replace("DDL", "").Replace("DLL","") + " é Obrigatorio!";
                            throw new Exception(erro);
                        }
                        else
                        {
                            ((DropDownList)(c)).BackColor = Color.White;
                        }
                    }
                }
                if (!valor)
                    return false;



                valor = validaCampos(c);
            }
            return valor;
        }

        public static bool IsDate(string date)
        {

            return IsDate(date, "dd/MM/yyyy");

        }

        public static bool IsDate(string date, string format)
        {

            DateTime parsedDate;

            bool isValidDate;



            isValidDate = DateTime.TryParseExact(

            date,

            format,

            System.Globalization.CultureInfo.InvariantCulture,

            System.Globalization.DateTimeStyles.None,

            out parsedDate);



            return isValidDate;

        }


        public bool existeNoArray(String[] campos, String campo)
        {
            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.Equals(campos[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static void RedirectNovaAba(string url)
        {
            string target = "_blank";
            string windowFeatures = "";
            HttpContext context = HttpContext.Current;
            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
            String.IsNullOrEmpty(windowFeatures))
            {
                context.Response.Redirect(url);
            }
            else
            {
                var page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);
                string script = !String.IsNullOrEmpty(windowFeatures) ? @"window.open(""{0}"", ""{1}"", ""{2}"");" : @"window.open(""{0}"", ""{1}"");";
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
        public void javascript(String comando, String strChave)
        {
            HttpContext context = HttpContext.Current;

            var page = (Page)context.Handler;
            ScriptManager.RegisterStartupScript(page, typeof(Page), strChave, comando, true);
        }


    }
}