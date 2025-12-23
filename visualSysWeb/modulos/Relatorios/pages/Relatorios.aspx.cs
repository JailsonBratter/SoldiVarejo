using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.modulos.Relatorios.code;
using System.Collections;
using visualSysWeb.dao;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.IO;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class Relatorios : System.Web.UI.Page
    {
        // static  Pesq rel = null;
        // static String listaAtual = "";
        // static String btnDataAtual = "";
        String CampoAgrupado = "";
        int intSubTotalIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];

          

            if (usr == null)
                return;

            if (Request.Params["tela"] != null)
            {
                usr.tela = Request.Params["tela"].ToString();
            }

            if (!IsPostBack)
            {
                // rel = null;
                if (Request.Params["Relatorio"] != null)
                {
                    LblTitulo.Text = "Relatorios de " + Request.Params["relatorio"].ToString();
                    RelatorioIO relio = new RelatorioIO(Server.MapPath(""),usr);
                    RdoRelatorios.Items.Clear();
                    relio.preencherDDL(RdoRelatorios, Request.Params["relatorio"].ToString());
                    Session.Add("btnDataAtual", "");
                    Session.Add("listaAtual", "");
                }
            }
            if (!IsPostBack)
            {
                if (Request.Params["rel"] != null)
                {
                    String rel = Request.Params["rel"].ToString();
                    for (int i = 0; i < RdoRelatorios.Items.Count; i++)
                    {
                        if (RdoRelatorios.Items[i].Text.IndexOf(rel) >= 0)
                        {
                            RdoRelatorios.Items[i].Selected = true;
                        }
                    }
                }
            }
            criarFiltros();


        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RelatorioEditar.aspx?relatorio=" + Request.Params["relatorio"].ToString());
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            rel.ordem = "";
            visualizarRelatorio();

        }

        protected void visualizarRelatorio()
        {
            try
            {
                User usr = (User)Session["User"];

                lblError.Text = "";
                Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
                rel.grupoAcumulado.Clear();
                lblFiltros.Text = "";
                rel.strfiltros = "";
                for (int i = 0; i < rel.filtros.Count; i++)
                {
                    Filtro fl = (Filtro)rel.filtros[i];
                    fl.valor = valorFiltro(fl);

                }


                if (rel.tipoConsulta.Equals("pagina"))
                {

                    Response.Redirect(rel.pagina);
                }
                else
                {

                    imgLog.ImageUrl = rel.enderecoImg;
                    lblCabecalho.Text = rel.cabecalho;
                    LblRodape.Text = usr.filial.Fantasia  +" CNPJ:"+Funcoes.FormatCNPJ(usr.filial.CNPJ)+ " Endereço:" + usr.filial.Endereco + "," + usr.filial.endereco_nro +
                            " - " + usr.filial.bairro + " - " + usr.filial.Cidade + " - " + usr.filial.UF + " CEP:" + usr.filial.CEP +
                            " Tel/Fax:" + usr.filial.telefone;
                    imgLog.Width = 40;


                    rel.limpaTotais();

                    rel.tb = null;
                    Gridrelatorio.DataSource = rel.Reltable(usr);
                    Gridrelatorio.DataBind();


                    Gridrelatorio.AllowSorting = !rel.naoOrdenar;
                    lblOrdemTitulo.Visible = !rel.naoOrdenar;

                    //ArrayList arrTotais = rel.getTotais(usr);
                    lblFiltros.Text = rel.strfiltros;
                    /*
                    if (arrTotais.Count > 0)
                    {
                        PnTotais.Controls.Clear();
                        Label titoTotal = new Label();
                        titoTotal.Text = "TOTAIS";
                        titoTotal.CssClass = "titulos";
                        PnTotais.Controls.Add(titoTotal);
                        foreach (Total to in arrTotais)
                        {
                            Panel pn = new Panel();
                            Label lblTotal = new Label();
                            lblTotal.Text = to.titulo.ToUpper() + ":&nbsp ";
                            lblTotal.CssClass = "titulobtn";
                            pn.Controls.Add(lblTotal);

                            Label lblValorTotal = new Label();
                            lblValorTotal.Text = to.total.ToString("N2");


                            pn.Controls.Add(lblValorTotal);
                            pn.CssClass = "total";
                            PnTotais.Controls.Add(pn);
                        }


                    }
                     */


                    Pnfiltros.Visible = false;
                    PnFrameFiltrosRelatorios.Visible = false;
                    pnSelecionaNovoRelatorio.Visible = false;
                    RdoRelatorios.Visible = false;

                    pnRelatorio.Visible = true;
                    ImgBtnVoltar.Visible = true;
                    divBtnRefresh.Visible = true;
                    divImpressao.Visible = true;
                    divExcel.Visible = true;
                    LblTitulo.Visible = false;

                    //Session.Add("relatorio", rel);
                    //foreach (GridViewRow grRow in Gridrelatorio.Rows)
                    //{
                    //    //grRow = Gridrelatorio.Rows[indexRow];
                    //    for (int i = 0; i < grRow.Cells.Count; i++)
                    //    {

                    //             String cell = grRow.Cells[i].Text.Replace("||", "").Replace("|-", "").Replace("-|", "");


                    //         

                    //    }
                    //}
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;

            }


        }

        protected void Gridrelatoiro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {


                visualizarRelatorio();
                Gridrelatorio.PageIndex = e.NewPageIndex;
            }
            catch (Exception ERR)
            {

                lblError.Text = ERR.Message;
            }

        }

        private String valorFiltro(Filtro filtro)
        {
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            Control ctr = Pnfiltros.FindControl(filtro.nome());
            if (ctr != null)
            {
                if (ctr.GetType() == typeof(TextBox))
                {
                    TextBox tx = (TextBox)ctr;
                    if (!tx.Text.Trim().Equals(""))
                    {
                        rel.strfiltros += " | " + filtro.titulo + ": " + tx.Text;
                        return tx.Text;

                    }
                    if (filtro.obrigatorio)
                    {
                        tx.BackColor = System.Drawing.Color.Red;
                        throw new Exception("O FILTRO " + filtro.titulo + " É OBRIGATORIO");
                    }

                }
                else if (ctr.GetType() == typeof(DropDownList))
                {
                    DropDownList cb = (DropDownList)ctr;
                    if (!cb.Text.Trim().Equals(""))
                    {
                        rel.strfiltros += " | " + filtro.titulo + ": " + cb.Text;
                        return cb.Text;
                    }
                }

            }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.

            return "";
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {

            TextBox txt = (TextBox)sender;
            preencheCampo(txt);

        }

        protected void preencheCampo(TextBox txt)
        {

            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            int or = int.Parse(txt.ID.Substring(1, 2));
            Filtro filtro = (Filtro)rel.filtros[or];
            if (!filtro.valor.Equals(txt.Text))
            {
                filtro.valor = txt.Text;
                /*
                if (!filtro.filtroPreenche.Equals(""))
                {
                    Filtro filtroPre = (Filtro)rel.filtros[int.Parse(filtro.filtroPreenche)];
                    TextBox txtPre = (TextBox)Pnfiltros.FindControl(filtroPre.nome());

                    if (!txt.Text.Equals(""))
                    {
                        SqlDataReader rsPre = Conexao.consulta(filtro.SqlPreenche(txt.Text), null, false);
                        if (rsPre.Read())
                        {
                            txtPre.Text = rsPre[filtro.campoPreenche].ToString();
                            filtroPre.valor = rsPre[filtro.campoPreenche].ToString();
                            txtPre.Focus();
                        }
                        if (rsPre != null)
                            rsPre.Close();
                    }
                    else
                    {
                        txtPre.Text = "";
                    }
                }
                 */
            }

        }
        protected void DdlRelatorios_SelectedIndexChanged1(object sender, EventArgs e)
        {
            criarFiltros();
        }
        private void criarFiltros()
        {
            User usr = (User)Session["User"];
          
            if (!RdoRelatorios.Text.Trim().Equals(""))
            {
                Relatorio rel = (Relatorio)Session["Rel-" + RdoRelatorios.SelectedItem.Value];
                if (rel == null || !RdoRelatorios.Text.Equals(rel.nomeRelatorio + ""))
                {
                    rel = new Relatorio(Server.MapPath("") + "/RelatoriosXml/" + Request.Params["relatorio"].ToString() + "/", RdoRelatorios.Text, Request.Params["relatorio"].ToString());
                }

               
                pnRelatorio.Visible = false;
                Pnfiltros.Visible = true;
                PnFrameFiltrosRelatorios.Visible = true;
                pnBtnVisualiar.Visible = true;
                PnFrameFiltrosRelatorios.Visible = true;
                Pnfiltros.Controls.Clear();
                divImpressao.Visible = false;
                divExcel.Visible = false;
                int totalDatas = 1;

                for (int i = 0; i < rel.filtros.Count; i++)
                {


                    Panel pnfiltro = new Panel();
                    pnfiltro.CssClass = "filtrosRelatorio";
                    Filtro filtro = (Filtro)rel.filtros[i];
                    //if (filtro.campo.ToUpper().Equals("GERENTE"))
                    //{
                    //    if (usr.funcionario.Funcao.Equals("GER COM REGIONAL"))
                    //    {
                    //        filtro.valorPadrao = usr.funcionario.codigo + "-" + usr.funcionario.Nome;
                    //        filtro.habilitado = false;
                    //    }
                    //}

                    if (filtro.campo.ToUpper().Contains("FILIAL"))
                    {
                        if (filtro.tipo.ToUpper().Equals("LISTATEXTO"))
                        {
                            filtro.valorPadrao = "|" + usr.getFilial() + "|";
                        }
                        else
                        {
                            filtro.valorPadrao = usr.getFilial();
                        }
                    }

                    Label lbl = new Label();
                    lbl.Text = "<p>" + filtro.titulo + "</p>";
                    pnfiltro.Controls.Add(lbl);
                    pnfiltro.ID = i.ToString();

                    switch (filtro.tipo.ToUpper())
                    {
                        case "TEXTO":

                            TextBox tx = new TextBox();
                            tx.ID = filtro.nome();
                            tx.Text = filtro.valor;
                            if (filtro.largura.Trim().Length > 0)
                                tx.Width = int.Parse(filtro.largura);



                            pnfiltro.Controls.Add(tx);
                            //Atribuir limitação para o grupo de clientes.
                            if (!usr.grupoClientes.ToString().Trim().Equals("") && tx.ID.Trim().ToUpper().IndexOf("GRUPOEMPRESA") >= 0)
                            {
                                tx.Enabled = false;
                                tx.Text = usr.grupoClientes;
                            }
                            else
                            { 
                                tx.TextChanged += txtFiltro_TextChanged;
                                tx.Enabled = filtro.habilitado;
                                tx.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
                            }
                            break;
                        case "COMBO":
                            DropDownList cbo = new DropDownList();


                            cbo.ID = filtro.nome();
                            if (!filtro.obrigatorio)
                            {
                                cbo.Items.Add("");
                            }
                            if (filtro.largura.Trim().Length > 0)
                                cbo.Width = int.Parse(filtro.largura);
                            cbo.Height = 21;
                            if (filtro.sql.Equals(""))
                            {
                                for (int j = 0; j < filtro.lista.Count; j++)
                                {

                                    cbo.Items.Add(filtro.lista[j].ToString());

                                }
                            }
                            else
                            {
                                SqlDataReader lista = null;
                                try
                                {


                                    lista = Conexao.consulta(filtro.sql, null, false);
                                    while (lista.Read())
                                    {
                                        bool add = false;
                                        if (filtro.campo.ToUpper().Contains("FILIAL") && !lista[0].ToString().Equals("TODOS"))
                                        {
                                            //add = usr.existeFilial(lista[0].ToString());
                                        }
                                        else
                                        {
                                            add = true;
                                        }
                                        if (add)
                                        {
                                            cbo.Items.Add(lista[0].ToString());
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                finally
                                {
                                    if (lista != null)
                                        lista.Close();
                                }

                            }
                            cbo.Text = filtro.valor;
                            //cbo.Width = 200;
                            cbo.Enabled = filtro.habilitado;
                            cbo.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
                            pnfiltro.Controls.Add(cbo);
                            break;
                        case "LISTA":
                        case "LISTATEXTO":

                            TextBox txls = new TextBox();
                            ImageButton btn = new ImageButton();
                            btn.ID = "btn" + filtro.nome();
                            btn.Click += lista_click;
                            btn.Width = 15;
                            btn.ImageUrl = "~\\img\\pesquisaM.jpg";
                            txls.ID = "o" + filtro.ordem.ToString("D2") + filtro.tipo.ToUpper() + filtro.campo;
                            txls.Text = filtro.valor;
                            txls.TextChanged += txtFiltro_TextChanged;
                            txls.Enabled = filtro.habilitado;
                            btn.Visible = filtro.habilitado;
                            txls.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
                            if (filtro.largura.Trim().Length > 0)
                                txls.Width = int.Parse(filtro.largura);

                            pnfiltro.Controls.Add(txls);
                            pnfiltro.Controls.Add(btn);
                            break;
                        case "LISTAFILTRO":

                            TextBox txlFiltro = new TextBox();
                            ImageButton btnFiltro = new ImageButton();
                            btnFiltro.ID = "btn" + filtro.nome();
                            btnFiltro.Click += filtro_click;
                            btnFiltro.Width = 15;
                            btnFiltro.ImageUrl = "~\\img\\pesquisaM.jpg";
                            txlFiltro.ID = "o" + filtro.ordem.ToString("D2") + filtro.tipo.ToUpper() + filtro.campo;
                            txlFiltro.Text = filtro.valor;
                            txlFiltro.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
                            if (filtro.largura.Trim().Length > 0)
                                txlFiltro.Width = int.Parse(filtro.largura);

                            txlFiltro.Enabled = filtro.habilitado;
                            pnfiltro.Controls.Add(txlFiltro);
                            if (filtro.habilitado)
                            {
                                pnfiltro.Controls.Add(btnFiltro);
                            }
                            break;

                        case "DATA":

                            TextBox dttx = new TextBox();
                            dttx.ID = filtro.nome();

                            filtro.valorPadrao = DateTime.Now.ToString("dd/MM/yyyy");


                            dttx.MaxLength = 10;
                            dttx.Enabled = filtro.habilitado;
                            dttx.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");

                            pnfiltro.Controls.Add(dttx);
                            
                                dttx.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
                                dttx.Width = 80;
                                dttx.Text = (filtro.valor.Trim().Equals("") ? DateTime.Now.ToString("dd/MM/yyyy") : filtro.valor);
                                ImageButton btndt = new ImageButton();
                                btndt.ID = "btn" + filtro.nome();
                                // btndt.Click += data_click;
                                btndt.Width = 15;
                                btndt.ImageUrl = "~\\img\\calendar.png";
                                btndt.Visible = filtro.habilitado;
                                pnfiltro.Controls.Add(btndt);
                                CalendarExtender cln = new CalendarExtender();
                                switch (totalDatas)
                                {
                                    case 1:
                                        cln = cln1;
                                        break;
                                    case 2:
                                        cln = cln2;
                                        break;
                                    case 3:
                                        cln = cln3;
                                        break;
                                    case 4:
                                        cln = cln4;
                                        break;
                                    case 5:
                                        cln = cln5;
                                        break;
                                }
                                cln.PopupButtonID = "btn" + filtro.nome();
                                cln.TargetControlID = filtro.nome();

                                cln.DaysModeTitleFormat = "MMMM, yyyy";


                                pnfiltro.Controls.Add(cln);
                                totalDatas++;
                            
                            break;
                        case "HORA":

                            TextBox Hrtx = new TextBox();
                            Hrtx.ID = filtro.nome();
                            Hrtx.Text = filtro.valor;
                            //if (filtro.largura.Trim().Length > 0)
                            //    Hrtx.Width = int.Parse(filtro.largura);
                            Hrtx.CssClass = "hora";

                            Hrtx.MaxLength = 5;
                            Hrtx.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
                            Hrtx.Attributes.Add("type", "time");
                            pnfiltro.Controls.Add(Hrtx);
                            // Hrtx.TextChanged += txtFiltro_TextChanged;

                            Hrtx.Enabled = filtro.habilitado;
                            break;


                    }


                    Pnfiltros.Controls.Add(pnfiltro);
                    Session.Remove("Rel-" + RdoRelatorios.SelectedItem.Value);
                    Session.Add("Rel-"+ RdoRelatorios.SelectedItem.Value, rel);

                }
            }
        }

        protected void data_click(object sender, ImageClickEventArgs e)
        {
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];


            ImageButton btn = (ImageButton)sender;
            int or = int.Parse(btn.ID.Substring(4, 2));
            Filtro filtro = (Filtro)rel.filtros[or];
            Session.Add("btnDataAtual", filtro.nome());


        }

        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            ImageButton btn = (ImageButton)sender;





            //chkLista.Items.Clear();
            int or = int.Parse(btn.ID.Substring(4, 2));
            Filtro filtro = (Filtro)rel.filtros[or];
            lbllista.Text = filtro.titulo;
            Session.Add("listaAtual", filtro.nome());
            if (filtro.sql.Equals(""))
            {

                //for (int j = 0; j < filtro.lista.Count; j++)
                //{
                //    ListItem item = new ListItem();
                //    item.Text = filtro.lista[j].ToString();
                //    chkLista.Items.Add(item);

                //}
            }
            else
            {
                String sqlLista = "";
                if (!filtro.filtroPai.Equals(""))
                {
                    Filtro filtropai = (Filtro)rel.filtros[int.Parse(filtro.filtroPai)];
                    TextBox txpai = (TextBox)Pnfiltros.FindControl(filtropai.nome());
                    sqlLista = filtro.SqlPai(txpai.Text);
                }
                else
                {
                    sqlLista = filtro.sql;
                }
                User usr = (User)Session["User"];

                gridListaChk.DataSource = Conexao.GetTable(sqlLista, null, false);
                gridListaChk.DataBind();

                //SqlDataReader lista = Conexao.consulta(sqlLista, null, false);


                //while (lista.Read())
                //{
                //    ListItem item = new ListItem();
                //    item.Value = lista[0].ToString();
                //    item.Text = lista[1].ToString();
                //    chkLista.Items.Add(item);
                //}

                //if (lista != null)
                //    lista.Close();

            }
            modalPnFundo.Show();


        }


        protected void chkListaTodos_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in gridListaChk.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkLista");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {
                    chk.Checked = (sender as CheckBox).Checked;
                    chk.Enabled = !(sender as CheckBox).Checked;
                }
            }
            modalPnFundo.Show();
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            rel.ordem = e.SortExpression;
            lblordem.Text = e.SortExpression;
            visualizarRelatorio();
        }

        protected void RdoRelatorios_SelectedIndexChanged(object sender, EventArgs e)
        {
            //rel = null;

            criarFiltros();
        }
        protected void filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList ch = (CheckBoxList)sender;

            string sen = ch.Text;

            //criarFiltros();
        }


        protected void ImgBtnVoltar_Click(object sender, ImageClickEventArgs e)
        {
            Pnfiltros.Visible = true;
            PnFrameFiltrosRelatorios.Visible = true;
            RdoRelatorios.Visible = true;
            pnSelecionaNovoRelatorio.Visible = true;
            pnBtnVisualiar.Visible = true;
            pnRelatorio.Visible = false;
            ImgBtnVoltar.Visible = false;
            divBtnRefresh.Visible = false;
            divImpressao.Visible = false;
            divExcel.Visible = false;
            LblTitulo.Visible = true;
        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String listaAtual = (String)Session["listaAtual"];
            TextBox txt = (TextBox)Pnfiltros.FindControl(listaAtual);
            txt.Text = "";
            CheckBox chkTodos = (CheckBox)gridListaChk.HeaderRow.FindControl("chkListaTodos");

            if (chkTodos != null && chkTodos.Checked)
            {
                txt.Text = "TODOS";
            }
            else
            {
                foreach (GridViewRow item in gridListaChk.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkLista");
                    if (chk != null && chk.Checked)
                    {
                        if (!txt.Text.Equals(""))
                        {
                            txt.Text += ",";
                        }
                        txt.Text += "|" + item.Cells[1].Text + "|";
                    }
                }
            }

            modalPnFundo.Hide();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void Gridrelatorio_DataBound(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            ArrayList arrTotais = rel.getTotais(usr);
            if (arrTotais.Count > 0)
            {
                GridViewRow footer = Gridrelatorio.FooterRow;
                if (footer != null)
                {
                    footer.Cells[0].Text = "TOTAIS";
                    footer.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                    foreach (Total to in arrTotais)
                    {
                        try
                        {
                            if ((!rel.semFormatacao(to.index)))
                            {
                                if (to.inteiro)
                                {
                                    footer.Cells[to.index].Text = to.total.ToString("N0");
                                }
                                else
                                {
                                    footer.Cells[to.index].Text = to.total.ToString("N2");
                                }
                                if (footer.Cells[to.index].Text.Equals("-1,00") || footer.Cells[to.index].Text.Equals("-1"))
                                    footer.Cells[to.index].Text = "";
                            }
                            else
                            {
                                footer.Cells[to.index].Text = to.total.ToString("N0");
                            }
                            footer.Cells[to.index].HorizontalAlign = HorizontalAlign.Right;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (!rel.campoAgrupaTotais.Trim().Equals(""))
                {
                    Gridrelatorio.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    Gridrelatorio.AlternatingRowStyle.BackColor = System.Drawing.Color.LightGray;
                }

            }


        }

        protected void Gridrelatorio_RowCreated(object sender, GridViewRowEventArgs e)
        {


            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            bool AddNovoTotal = false;

            if (!rel.campoAgrupaTotais.Trim().Equals("") && (DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais) != null))
            {
                if ((!CampoAgrupado.Trim().Equals("")))

                    if (!CampoAgrupado.Equals(DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais).ToString()))
                    {
                        AddNovoTotal = true;
                    }

            }
            if ((!CampoAgrupado.Trim().Equals("")) && (DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais) == null))
            {
                AddNovoTotal = true;
                intSubTotalIndex = 0;
            }

            if (AddNovoTotal)
            {

                GridView grdViewProducts = (GridView)sender;

                // Creating a Row
                GridViewRow SubTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                for (int i = 0; rel.qtdCampos > i; i++)
                {
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.CssClass = "subtotal";
                    if (i == 0)
                    {
                        HeaderCell.Text = "Sub Total";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Left;

                    }
                    else
                    {
                        HeaderCell.Text = rel.getTotalGrupo(i);
                        HeaderCell.HorizontalAlign = HorizontalAlign.Right;

                    }
                    SubTotalRow.Cells.Add(HeaderCell);

                }
                SubTotalRow.BackColor = System.Drawing.Color.LightGray;
                SubTotalRow.ForeColor = System.Drawing.Color.Black;
                grdViewProducts.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow);

                intSubTotalIndex++;
                rel.zerarTotaisGrupo();

            }

            //Session.Remove("Rel");
            // Session.Add("Rel", rel);



        }

        private bool isnumero(String numero)
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

        protected void Gridrelatorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
                if (rel.formataRelatorio)
                {
                    if (e.Row.DataItem != null && e.Row.RowType == DataControlRowType.DataRow)
                    {


                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            bool semForm = false;
                            if (!rel.semFormatacao(i))
                            {


                                String cell = e.Row.Cells[i].Text.Replace("|-NI-|", "").Replace("NUI_", "").Replace("||", "").Replace("|-", "").Replace("-|", "");
                                if (cell.ToUpper().IndexOf("PLU") >= 0)
                                {
                                    semForm = true;
                                    if (e.Row.Cells[i].Text.Contains("|-"))
                                    {
                                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-", "<b>")
                                                                                 .Replace("-|", "</b>")
                                                                                 .Replace("PLU", "");

                                    }
                                    else
                                    {
                                        e.Row.Cells[i].Text = cell.ToUpper().Replace("PLU", ""); // CAMPO PLU SEM FORMATACAO
                                    }
                                }
                                if (cell.ToUpper().IndexOf("SFT_") >= 0) // CAMPO SEM FORMATACAO
                                {
                                    if (e.Row.Cells[i].Text.Contains("|-"))
                                    {
                                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-", "<b>")
                                                                                 .Replace("-|", "</b>")
                                                                                 .Replace("SFT_", "");
                                    }
                                    else
                                    {
                                        e.Row.Cells[i].Text = cell.ToUpper().Replace("SFT_", "");
                                    }
                                }
                                else if (cell.IndexOf('%') >= 0)
                                {
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                }
                                else if (cell.IndexOf("SFD_") >= 0) //CAMPO SEM FORMATACAO ALIANHADO A DIREIRA
                                {
                                    if (e.Row.Cells[i].Text.Contains("|-"))
                                    {
                                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-", "<b>")
                                                                                 .Replace("-|", "</b>")
                                                                                 .Replace("SFD_", "");
                                    }
                                    else
                                    {
                                        e.Row.Cells[i].Text = cell.ToUpper().Replace("SFD_", "");
                                    }
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                }

                                if (e.Row.Cells[i].Text.Contains("|-NI-|") || e.Row.Cells[i].Text.Contains("NUI_")) //CAMPO INTEIRO COM ALINHADO A DIREITA
                                {
                                    semForm = true;
                                    String strCel = e.Row.Cells[i].Text.Replace("|-NI-|", "");
                                    bool bNegrito = strCel.IndexOf("|-") >= 0;

                                    Decimal vlr = 0;
                                    String strVlr = cell.ToUpper()
                                                    .Replace("|-NI-|", "")
                                                    .Replace("NUI_", "")
                                                    .Replace(".", ",")
                                                    .Replace("|-", "")
                                                    .Replace("-|", "")
                                                    ;


                                    Decimal.TryParse(strVlr, out vlr);
                                    e.Row.Cells[i].Text = (bNegrito ? "<b>" : "") + vlr.ToString("N0") + (bNegrito ? "</b>" : "");
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                }


                                if (e.Row.Cells[i].Text.IndexOf("|-TITULO-|") >= 0)
                                {
                                    if (e.Row.Cells[i].Text.Contains("|CONCAT|"))
                                    {
                                        e.Row.Cells[i].ColumnSpan = e.Row.Cells.Count;
                                        for (int ci = e.Row.Cells.Count - 1; ci > 0; ci--)
                                        {
                                            e.Row.Cells.RemoveAt(ci);
                                        }
                                    }
                                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-TITULO-|", "").Replace("|CONCAT|", "");
                                    e.Row.CssClass = "tituloGrid";
                                }
                                else if (e.Row.Cells[i].Text.IndexOf("|-SUB-|") >= 0)
                                {
                                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-SUB-|", "");
                                    cell = cell.Replace("|-SUB-|", "");
                                    e.Row.CssClass = "subtotal";
                                }
                                cell = cell.Replace("NTOTAL_", "");

                                if (isnumero(cell) && !semForm)
                                {

                                    bool negrito = e.Row.Cells[i].Text.Contains("|-");
                                    Decimal vNum = 0;
                                    Decimal.TryParse(cell, out vNum);
                                    e.Row.Cells[i].Text = (negrito ? "<b>" : "") + vNum.ToString("N2") + (negrito ? "</b>" : "");
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                }
                                else if (cell.IndexOf('%') >= 0)
                                {
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                }
                                else if (cell.Contains("|chave|"))
                                {
                                    String chave = cell.Replace("|chave|", "").Replace("CFe","").Replace("NFe","");
                                    String text = chave.Substring(0, 10) + "...";
                                    
                                    e.Row.Cells[i].Text = "<input type='button' value='"+text+ "' onclick='javascript: MostrarChave(\""+chave+"\")'>";
                                }
                                //else if (cell.IndexOf("SFD_") >= 0) //CAMPO SEM FORMATACAO ALIANHADO A DIREIRA
                                //{
                                //    bool negrito = e.Row.Cells[i].Text.Contains("|-");
                                //    e.Row.Cells[i].Text = (negrito ? "<b>" : "") + cell.ToUpper().Replace("SFD_", "") + (negrito ? "</b>" : ""); ;
                                //    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                //}

                            }

                            if (e.Row.Cells[i].Text.Contains("|-") ||
                                e.Row.Cells[i].Text.Contains("|table|") ||
                                e.Row.Cells[i].Text.Contains("||")
                                )
                            {
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|table|", "<table style = \"Width:100%\">");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-table|", "</table>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|tr|", "<tr>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-tr|", "</tr>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|th|", "<th>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-th|", "</th>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|td|", "<td>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-td|", "</td>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|hr|", "<hr>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-", "<b>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("-|", "</b>");
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("||", "<br>");

                            }

                        }


                        int indexRow = e.Row.RowIndex;



                        //Relatorio rel = (Relatorio)Session["Rel"];
                        if (!rel.campoAgrupaTotais.Equals(""))
                        {
                            CampoAgrupado = DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais).ToString();
                            ArrayList arrTotais = rel.getTotalGrupo();
                            foreach (GrupoSubtotal item in arrTotais)
                            {
                                Decimal vlr = Funcoes.decTry(DataBinder.Eval(e.Row.DataItem, item.Campo).ToString());
                                if (item.acumular)
                                    rel.setTotalGrupo(item.Campo,vlr);
                                else
                                    item.Valor +=vlr;
                                //rel.setTotalGrupo(item.Campo, );


                                rel.setTotal(item.posicao, DataBinder.Eval(e.Row.DataItem, item.Campo).ToString());

                            }

                        }
                        Session.Remove("Rel-"+RdoRelatorios.SelectedItem.Value);
                        Session.Add("Rel-"+RdoRelatorios.SelectedItem.Value, rel);

                    }
                }
            }
            catch (Exception)
            {


            }

        }

        protected void calData_SelectionChanged(object sender, EventArgs e)
        {
            String btnDataAtual = (String)Session["btnDataAtual"];
            TextBox txt = (TextBox)Pnfiltros.FindControl(btnDataAtual);

        }

        protected void imgCancelaCalendar_Click(object sender, ImageClickEventArgs e)
        {

        }


        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void btnFecharFiltro_Click(object sender, ImageClickEventArgs e)
        {
            lblErroPesquisa.Text = "";

            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                ImageButton btn = (ImageButton)sender;
                Filtro listaAtual = (Filtro)Session["FiltroAtual"];
                TextBox txt = (TextBox)Pnfiltros.FindControl(listaAtual.nome());
                txt.Text = "";
                txt.Text = selecionado;
                preencheCampo(txt);

                if (listaAtual.filtrosPreenche.Count > 0)
                {

                    Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
                    foreach (ArrayList item in listaAtual.filtrosPreenche)
                    {
                        Filtro filtroPreenche = (Filtro)rel.filtros[int.Parse(item[0].ToString())];
                        TextBox txtPr = (TextBox)Pnfiltros.FindControl(filtroPreenche.nome());
                        txtPr.Text = ListaSelecionada(int.Parse(item[1].ToString()));
                        preencheCampo(txtPr);
                    }
                }



                modalPnFiltro.Hide();
                Session.Remove("btnFiltro");
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFiltro.Show();
            }
        }

        protected void btnCancelaFiltro_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFiltro.Hide();
            Session.Remove("btnFiltro");
        }


        protected String ListaSelecionada(int campo)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[campo].Text.Replace("&nbsp;", "");
                    }
                }
            }

            return "";
        }

        private void exibeLista()
        {
            lblErroPesquisa.Text = "";
            Relatorio rel = (Relatorio)Session["Rel-"+RdoRelatorios.SelectedItem.Value];
            ImageButton btn = (ImageButton)Session["btnFiltro"];
            modalPnFiltro.Show();



            int or = int.Parse(btn.ID.Substring(4, 2));
            Filtro filtro = (Filtro)rel.filtros[or];
            lblTituloFiltro.Text = filtro.titulo;
            Session.Add("FiltroAtual", filtro);

            if (!filtro.sql.Equals(""))
            {

                String sqlLista = "";
                if (!filtro.filtroPai.Equals(""))
                {
                    Filtro filtropai = (Filtro)rel.filtros[int.Parse(filtro.filtroPai)];

                    var txpai = Pnfiltros.FindControl(filtropai.nome());
                    String strPai = "";
                    if (txpai is TextBox)
                    {
                        strPai = ((TextBox)txpai).Text;

                    }
                    else if (txpai is DropDownList)
                    {
                        strPai = ((DropDownList)txpai).Text;

                    }

                    sqlLista = filtro.SqlPai(strPai);
                    if (!TxtPesquisaLista.Text.Equals(""))
                    {
                        if (filtro.camposFiltro.Count > 0)
                        {

                            String sqlFiltros = "";
                            foreach (String item in filtro.camposFiltro)
                            {
                                if (!sqlFiltros.Equals(""))
                                    sqlFiltros += " or ";
                                if (item.Contains('='))
                                    sqlFiltros += "(" + item + "'" + TxtPesquisaLista.Text + "') ";
                                else
                                    sqlFiltros += "( " + item + " Like '%" + TxtPesquisaLista.Text + "%')";
                            }

                            if (sqlFiltros.Length > 0)
                            {
                                sqlLista += (sqlLista.ToUpper().IndexOf("WHERE") >= 0 ? " and" : " WHERE ") + " (" + sqlFiltros + ")";
                            }
                        }
                    }
                }
                else
                {

                    sqlLista = filtro.sql;
                    if (filtro.camposFiltro.Count > 0)
                    {
                        sqlLista += " where ";

                        String sqlFiltros = "";
                        foreach (String item in filtro.camposFiltro)
                        {
                            if (!sqlFiltros.Equals(""))
                                sqlFiltros += " or ";
                            if (item.Contains('='))
                                sqlFiltros += "(" + item + "'" + TxtPesquisaLista.Text + "') ";
                            else
                                sqlFiltros += "( " + item + " Like '%" + TxtPesquisaLista.Text + "%')";
                        }
                        sqlLista += sqlFiltros;
                    }
                }
                User usr = (User)Session["User"];
                GridLista.DataSource = Conexao.GetTable(sqlLista, usr, false);
                GridLista.DataBind();

                if (filtro.campo.ToUpper().Contains("FILIAL"))
                {

                }
                if (GridLista.Rows.Count == 1)
                {
                    if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                    {
                        RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                        rdo.Checked = true;
                    }
                }
                TxtPesquisaLista.Focus();
            }
        }
        protected void filtro_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            Session.Add("btnFiltro", btn);
            exibeLista();


        }
        protected void imgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            Relatorio rel = (Relatorio)Session["Rel-"+ RdoRelatorios.SelectedItem.Value];
            foreach (Filtro filtro in rel.filtros)
            {
                Control ctr = Pnfiltros.FindControl(filtro.nome());
                if (ctr != null)
                {
                    if (ctr.GetType() == typeof(TextBox))
                    {
                        TextBox tx = (TextBox)ctr;
                        //Atribuir limitação para o grupo de clientes.
                        if (!usr.grupoClientes.ToString().Trim().Equals("") && tx.ID.Trim().ToUpper().IndexOf("GRUPOEMPRESA") >= 0)
                        {
                            tx.Enabled = false;
                            tx.Text = usr.grupoClientes;
                        }
                        else
                        {
                            tx.Text = filtro.valorPadrao;
                        }
                    }
                    else if (ctr.GetType() == typeof(DropDownList))
                    {
                        DropDownList cb = (DropDownList)ctr;

                        cb.Text = filtro.valorPadrao;

                    }

                }

            }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }


       

    }
}