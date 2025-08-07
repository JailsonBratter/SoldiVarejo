using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using visualSysWeb.code;
using visualSysWeb.dao;

using visualSysWeb.modulos.Mobile.code;
using visualSysWeb.modulos.Mobile.dao;

namespace visualSysWeb.modulos.Mobile
{
    public partial class SmartPhone_NF_Detalhe : System.Web.UI.Page
    {
        String chave = "";
        int indexPublico = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                Response.Redirect("Mobile_Login.aspx");



            chave = Request.Params["chave"].ToString();// colocar o campo index da tabela

            NotaEspelho nota = null;
            try
            {
                nota = (NotaEspelho)Session["objNota" + chave];
            }
            catch (Exception)
            {

                Session.Remove("objNota" + chave);
                nota = null;
            }

            //Nota nota;
            //NotaEspelho nota = new NotaEspelho();

            if (Request.Params["chave"] != null)  // colocar o campo index da tabela
            {
                try
                {
                    if (!IsPostBack)
                    {
                        //String chave = Request.Params["chave"].ToString();// colocar o campo index da tabela


                        using (WebClient client = new WebClient())
                        {
                            var jsonResponse = client.DownloadString("http://breeds.bratter.com.br:19745/soldiapicloud/api/notas?chave=" + chave);
                            ////var jsonResponse = client.DownloadString("http://localhost:10291/api/Notas?chave=" + chave);
                            if (nota == null)
                            {
                                nota = new NotaEspelho();
                            }

                            nota.nota = JsonConvert.DeserializeObject<Nota>(jsonResponse);


                            Session.Remove("objNota" + chave);
                            Session.Add("objNota" + chave, nota);

                            User usr = (User)Session["User"];

                            lblCodigo.Text = nota.nota.numero;
                            lblEmissao.Text = DateTime.Parse(nota.nota.emissao.ToString()).ToString("dd/MM/yyyy");
                            lblCliente_Fornecedor.Text = nota.nota.fornecedor;
                            lblFornecedor_CNPJ.Text = nota.nota.chave.Substring(6, 14);

                            fornecedorDAO forn = new fornecedorDAO(lblFornecedor_CNPJ.Text, usr);
                            nota.nota.fornecedor = forn.Fornecedor;
                            forn = null;

                            //DataTable dt = CollectionHelper.ConvertTo<ItensNFe>(nota.nota.itens);
                            //gridItens.DataSource = dt; // Conexao.GetTable(sql, null, false);

                            gridItens.DataSource = nota.nota.itens; // Conexao.GetTable(sql, null, false);
                            gridItens.DataBind();


                        }

                        ddlAno.Items.Clear();
                        ddlAno.Items.Add(new ListItem("", "0000"));
                        for (int i = 0; i < 4; i++)
                        {
                            int intAno = DateTime.Now.Year;
                            intAno += i;
                            ddlAno.Items.Add(new ListItem(intAno.ToString(), intAno.ToString()));
                        }

                        ddlDia.Items.Clear();
                        ddlDia.Items.Add(new ListItem("", "00"));
                        for (int i = 1; i <= 31; i++)
                        {
                            ddlDia.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
                        }

                        //espelho = new EspelhoCego(chave);
                    }
                    else
                    {

                        //espelho = new EspelhoCego();
                    }
                }
                catch (Exception er)
                {
                    //espelho = new EspelhoCego(); //TRATAR EXCESSÃO AQUI NO FUTURO
                }
            }
            else
            {
                //VOLTAR AQUI E NÃO ABRIR 
                //RETORNAR FALSO
                //espelho = new EspelhoCego();
            }

            //espelho.itens.Add(new EspelhoCegoItens() { plu = "17", descricao = "PRODUTO CODIGO 17", qtde = 12, emb = 1, qtdetotal= 12 });
            //espelho.itens.Add(new EspelhoCegoItens() { plu = "18", descricao = "PRODUTO CODIGO 18", qtde = 1, emb = 1, qtdetotal = 1 });
            //espelho.itens.Add(new EspelhoCegoItens() { plu = "19", descricao = "PRODUTO CODIGO 19", qtde = 6, emb = 1 , qtdetotal = 6});
            //espelho.itens.Add(new EspelhoCegoItens() { plu = "20", descricao = "PRODUTO CODIGO 20", qtde = 24, emb = 1, qtdetotal = 24 });


            //gridItens.DataSource = espelho.itens;
            //gridItens.DataBind();
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("<script>window.open('SmartPhone_EspelhoCego.aspx?chave=" + chave + "','_blank'); </script>");
            //Response.Redirect("SmartPhone_EspelhoCego.aspx");
        }

        protected void imgValida_Click(object sender, ImageClickEventArgs e)
        {
            lblCritica.Text = "";
            DateTime dataValidade = DateTime.MinValue;
            if (!chkSemValidade.Checked)
            {
                try
                {
                    dataValidade = DateTime.Parse(ddlAno.Text + '-' + ddlMes.Text + "-" + ddlDia.Text);
                    if (dataValidade < DateTime.Now)
                    {
                        lblCritica.Text = "Data de validade não pode ser inferior a data atual!";
                        lblCritica.Visible = true;
                        lblCritica.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else if ((dataValidade - DateTime.Now).Days < 180)
                    {
                        lblCritica.Text = "Data de validade inferior a seis meses.";
                        lblCritica.Visible = true;
                        lblCritica.ForeColor = System.Drawing.Color.Orange;
                    }
                }
                catch
                {
                    lblCritica.Text = "Data Inválida!!";
                    lblCritica.Visible = true;
                    lblCritica.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            else
            {
                dataValidade = DateTime.Parse("1900-01-01");
            }
            int.TryParse(lblIndex.Text, out indexPublico);

            NotaEspelho nota = (NotaEspelho)Session["objNota" + chave];

            gridItens.Rows[indexPublico].Cells[3].Text = nota.nota.itens[indexPublico].qtde.ToString();
            gridItens.Rows[indexPublico].Cells[4].Text = nota.nota.itens[indexPublico].emb.ToString();

            gridItens.Rows[indexPublico].Cells[7].Text = dataValidade.ToString("yyyy-MM-dd");

            if (txtPlu.Text.Equals("") || txtDescricao.Text.Equals("") || txtRecebido.Text.Equals(""))
            {
                lblCritica.Text += "Dados inválidos! Necessário selecionar um item.";
                lblCritica.Visible = true;
                lblCritica.ForeColor = System.Drawing.Color.Red;
                return;
                //throw new Exception("Dados inválidos! Necessário selecionar um item.");
            }

            gridItens.Rows[indexPublico].Cells[6].Text = txtRecebido.Text;

            ItensNFeTmp tmp = new ItensNFeTmp();
            tmp.plu = nota.nota.itens[indexPublico].plu;
            tmp.dataValidade = dataValidade;
            tmp.qtde = Funcoes.intTry(txtRecebido.Text);
            tmp.salvar(chave);

            txtPlu.Text = "";
            txtDescricao.Text = "";
            txtRecebido.Text = "";

            ddlAno.SelectedIndex = 0;
            ddlMes.SelectedIndex = 0;
            ddlDia.SelectedIndex = 0;

        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblIndex.Text = index.ToString();
            string plu = gridItens.Rows[index].Cells[1].Text;
            string descricao = gridItens.Rows[index].Cells[2].Text;

            txtPlu.Text = plu;
            txtDescricao.Text = descricao;
            lblCritica.Text = "";

            chkSemValidade.Checked = false;

            ddlAno.SelectedIndex = 0;
            ddlMes.SelectedIndex = 0;
            ddlDia.SelectedIndex = 0;


            ddlAno.Enabled = true;
            ddlMes.Enabled = true;
            ddlDia.Enabled = true;

            txtRecebido.Focus();
        }

        protected void btnReceber_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];
            List<ItensNFe> itens = new List<ItensNFe>();
            foreach (GridViewRow row in gridItens.Rows)
            {
                if (row.Cells[5].Text != row.Cells[6].Text)
                {
                    row.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                ItensNFe item = new ItensNFe();
                item.plu = row.Cells[1].Text;
                item.emb = Funcoes.intTry(row.Cells[4].Text);
                item.qtde = Funcoes.intTry(row.Cells[3].Text);
                item.qtdetotal = Funcoes.intTry(row.Cells[5].Text);
                item.qtdeRecebida = Funcoes.intTry(row.Cells[6].Text);
                item.dataValidade = Funcoes.dtTry(row.Cells[7].Text);
                itens.Add(item);
            }
            NotaEspelho nota = (NotaEspelho)Session["objNota" + chave];
            //nota.itens.Clear();
            nota.nota.itens = itens;

            Session.Remove("objNota" + chave);
            Session.Add("objNota" + chave, nota);

            if (!nota.nota.salvar(usr))
            {
                lblError.Text += "Erro ao tentar salvar NFe";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                //Processo de recebimento
                //Se for uma NFe de transferencia será inclusa direto no DB
                receberNFe(chave, nota.nota.itens);
                ItensNFeTmp tmp = new ItensNFeTmp();
                tmp.excluir(chave);
                Response.Redirect("SmartPhone_Espelho.aspx");
            }

        }

        protected void chkSemValidade_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSemValidade.Checked)
            {
                ddlAno.Enabled = false;
                ddlMes.Enabled = false;
                ddlDia.Enabled = false;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SmartPhone_Espelho.aspx");
        }

        private void receberNFe(string chave, List<ItensNFe> itemNF)
        {
            //Processo para entrada direta da NFe.
            User usr = (User)Session["user"];
            //Checa se trata-se de uma NFe de transferência.
            try
            {
                if (chave.Substring(6, 8) == usr.filial.CNPJ.Substring(0, 8))
                {
                    NFDAORec nfREC = new NFDAORec(chave, usr);
                    foreach (NFItemDAORec item in nfREC.Itens)
                    {
                        var obj = itemNF.Find(x => x.plu == item.PLU);
                        item.Tipo = "";
                        item.Qtde = obj.qtde;
                        item.data_validade = obj.dataValidade;
                        item.Qtde_Devolver = (obj.qtde - obj.qtdeRecebida);
                    }
                    if (nfREC.insert())
                    {
                        Nota nota = new Nota();
                        nota.atualizaStatus(chave);
                    }
                }
            }
            catch
            {

            }
            //Atualiza NFe na Nuvem
            using (WebClient client = new WebClient())
            {
                var jsonResponse = client.UploadString("http://breeds.bratter.com.br:19745/soldiapicloud/api/notas?chave=" + chave, "PUT", "");
            }
        }

        protected void txtPlu_TextChanged(object sender, EventArgs e)
        {
            if (txtPlu.Text.Trim().Length > 6)
            {
                for (int i = 0; i < gridItens.Rows.Count; i++)
                {
                    if (gridItens.Rows[i].Cells[2].Text.IndexOf(txtPlu.Text) >= 0)
                    {
                        lblIndex.Text = i.ToString();
                        string plu = gridItens.Rows[i].Cells[1].Text;
                        string descricao = gridItens.Rows[i].Cells[2].Text;

                        txtPlu.Text = plu;
                        txtDescricao.Text = descricao;
                        lblCritica.Text = "";

                        chkSemValidade.Checked = false;

                        ddlAno.SelectedIndex = 0;
                        ddlMes.SelectedIndex = 0;
                        ddlDia.SelectedIndex = 0;


                        ddlAno.Enabled = true;
                        ddlMes.Enabled = true;
                        ddlDia.Enabled = true;

                        txtRecebido.Focus();

                        return;
                    }
                }
            }
        }

        protected void btnForcarRecebimento_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];
            List<ItensNFe> itens = new List<ItensNFe>();
            foreach (GridViewRow row in gridItens.Rows)
            {
                ItensNFe item = new ItensNFe();
                item.plu = row.Cells[1].Text;
                item.emb = Funcoes.intTry(row.Cells[4].Text);
                item.qtde = Funcoes.intTry(row.Cells[3].Text);
                item.qtdetotal = Funcoes.intTry(row.Cells[5].Text);
                item.qtdeRecebida = Funcoes.intTry(row.Cells[6].Text);
                item.dataValidade = Funcoes.dtTry(row.Cells[7].Text);
                itens.Add(item);
            }
            NotaEspelho nota = (NotaEspelho)Session["objNota" + chave];
            //nota.itens.Clear();
            nota.nota.itens = itens;

            Session.Remove("objNota" + chave);
            Session.Add("objNota" + chave, nota);

            if (!nota.nota.salvar(usr))
            {
                lblError.Text += "Erro ao tentar salvar NFe";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                receberNFe(chave, nota.nota.itens);
                ItensNFeTmp tmp = new ItensNFeTmp();
                tmp.excluir(chave);
                Response.Redirect("SmartPhone_Espelho.aspx");
            }
        }

        protected void btnRecuperarContagem_Click(object sender, EventArgs e)
        {
            ItensNFeTmp tmp = new ItensNFeTmp(chave);
            if (tmp.itens.Count > 0)
            {
                foreach (GridViewRow row in gridItens.Rows)
                {
                    ItensNFeTmp obj = tmp.itens.Find(x => x.plu == row.Cells[1].Text.ToString());
                    try
                    {
                        if (obj.plu != null)
                        {
                            row.Cells[7].Text = obj.dataValidade.ToString("yyyy-MM-dd");
                            row.Cells[6].Text = obj.qtde.ToString();
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}