using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Data.SqlClient;
using System.IO;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoCompra : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select Pedido,cliente_fornec, Status = case when status=1 then 'ABERTO'else case when status=2 then 'FECHADO'else 'CANCELADO' END END,CONVERT(VARCHAR ,data_cadastro,103)as data_cadastro,total from pedido where tipo=2";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                tb = Conexao.GetTable(sqlGrid + " order by CONVERT(VARCHAR ,pedido.data_cadastro,102) desc ", usr, true);
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
            }
            pesquisar(pnBtn);
            formatarCampos();
        }


        private void formatarCampos()
        {
            txtPedido.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtDataDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txtDataAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
        }



        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/pedidos/pages/pedidoCompraDetalhes.aspx?novo=true");
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
        }

        private void pesquisa()
        {
            try
            {
                String sql = "";
                if (!txtPedido.Text.Equals("")) //colocar nome do campo de pesquisa
                {
                    sql = " and  pedido = '" + txtPedido.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                }
                if (!txtFornecedor.Text.Equals(""))
                {
                    sql += "  and cliente_fornec like '%" + txtFornecedor.Text + "%'";
                }

                if (!txtDataDe.Text.Equals(""))
                {

                    if (IsDate(txtDataDe.Text) && IsDate(txtDataAte.Text))
                    {
                        sql += "and  Data_Cadastro >= '" + DateTime.Parse(txtDataDe.Text).ToString("yyyy-MM-dd") + "' and Data_Cadastro <='" + DateTime.Parse(txtDataAte.Text).ToString("yyyy-MM-dd") + "'";
                    }
                    else
                    {
                        throw new Exception("Datas Invalidas");
                    }
                }


                User usr = (User)Session["User"];
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + sql + " order by CONVERT(VARCHAR ,pedido.data_cadastro,102) desc ", usr, false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid + " order by CONVERT(VARCHAR ,pedido.data_cadastro,102) desc ", usr, true);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
                
            }

        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        
        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";


            switch (or)//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            {
                case "Fornecedor":
                    sqlLista = "select Fornecedor, razao_social from fornecedor where Fornecedor like '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Fornecedor";
                    break;


            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr,true);
            GridLista.DataBind();
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }

            modalPnFundo.Show();
            TxtPesquisaLista.Focus();
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            switch (btn.ID)
            {
                case "imgBtnFornecedor":
                    Session.Add("camporecebe", "Fornecedor");
                    break;
            }

            exibeLista();


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
                        return item.Cells[campo].Text.Trim();
                    }
                }
            }

            return "";
        }
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
             String selecionado = ListaSelecionada(1);

             if (!selecionado.Equals("") && !selecionado.Equals("------"))
             {
                 String listaAtual = (String)Session["camporecebe"];
                 Session.Remove("camporecebe");

                 if (listaAtual.Equals("Fornecedor"))
                 {

                     txtFornecedor.Text = ListaSelecionada(1);


                 }


                 modalPnFundo.Hide();
             }
             else
             {
                 lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                 modalPnFundo.Show();
             }

        }
        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
            
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            enviarEmailPedido(lblPedidoEmail.Text);
            modalConfirmaEmail.Hide();
        }
        protected void btnCancela_Click(object sender, EventArgs e)
        {
            modalConfirmaEmail.Hide();
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }
        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            HyperLink meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[0].Controls[0];
            lblPedidoEmail.Text = meuLink.Text;

            modalConfirmaEmail.Show();

        }
        private void enviarEmailPedido(String NumeroPedido)
        {
            try
            {


                User usr = (User)Session["user"];
                pedidoDAO ped = new pedidoDAO(NumeroPedido, 2, usr);
                fornecedorDAO fornec = new fornecedorDAO(ped.Cliente_Fornec, usr);
                if (fornec.email.Trim().Equals(""))
                {
                    throw new Exception("Não foi possivel Enviar o E-mail, Fornecedor não tem o E-mail cadastrado");
                }
                
                String arquivo = Server.MapPath("~\\modulos\\Manutencao\\pages") + "\\EmailPedido.html";
                StreamReader objStreamReader = File.OpenText(arquivo);
                String conteudo = objStreamReader.ReadToEnd();
                objStreamReader.Close();

                conteudo = conteudo.Replace("<|RazaoSocial|>", usr.filial.Razao_Social);
                conteudo = conteudo.Replace("<|cnpj|>", usr.filial.CNPJ);
                conteudo = conteudo.Replace("<|Ie|>", usr.filial.IE);
                conteudo = conteudo.Replace("<|endereco|>", usr.filial.Endereco);
                conteudo = conteudo.Replace("<|numero|>", usr.filial.endereco_nro);
                conteudo = conteudo.Replace("<|complemento|>", "");
                conteudo = conteudo.Replace("<|cep|>", usr.filial.CEP);
                conteudo = conteudo.Replace("<|bairro|>", usr.filial.bairro);
                conteudo = conteudo.Replace("<|cidade|>", usr.filial.Cidade);
                conteudo = conteudo.Replace("<|uf|>", usr.filial.UF);
                conteudo = conteudo.Replace("<|Telefone|>", usr.filial.fone);

                conteudo = conteudo.Replace("<|FornecedorRazaoSocial|>", fornec.Razao_social);
                conteudo = conteudo.Replace("<|fornecedorcnpj|>", fornec.CNPJ);
                conteudo = conteudo.Replace("<|fornecedorIe|>", fornec.IE);
                conteudo = conteudo.Replace("<|Fornecedorendereco|>", fornec.Endereco);
                conteudo = conteudo.Replace("<|Fornecedornumero|>", fornec.Endereco_nro);
                conteudo = conteudo.Replace("<|Fornecedorcomplemento|>", "");
                conteudo = conteudo.Replace("<|Fornecedorcep|>", fornec.CEP);
                conteudo = conteudo.Replace("<|Fornecedorbairro|>", fornec.Bairro);
                conteudo = conteudo.Replace("<|Fornecedorcidade|>", fornec.Cidade);
                conteudo = conteudo.Replace("<|Fornecedoruf|>", fornec.UF);

                conteudo = conteudo.Replace("<|NumeroPedido|>", ped.Pedido);
                conteudo = conteudo.Replace("<|Data|>", ped.Data_cadastroBr());
                conteudo = conteudo.Replace("<|Total|>", ped.Total.ToString("N2"));

                String strItens = "";
                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    String sqlPlu = "Select top 1 m.PLU, ean.EAN, ISNULL((select top 1 codigo_referencia from Fornecedor_Mercadoria where Fornecedor='" + fornec.Fornecedor + "' AND PLU=M.PLU),'') AS CODIGO_REFERENCIA " +
                                " from mercadoria m left join EAN on m.plu = ean.PLU  " +
                                " where m.plu = '" + item.PLU + "' ";
                    SqlDataReader rsPlu = Conexao.consulta(sqlPlu, null, false);
                    String ean = "";
                    String refFornec = "";
                    if (rsPlu.Read())
                    {
                        ean = rsPlu["ean"].ToString();
                        refFornec = rsPlu["codigo_referencia"].ToString();
                    }

                    if (rsPlu != null)
                        rsPlu.Close();


                    strItens += "<tr>";
                    strItens += "<td style=\"text-align:left; \">" + item.PLU + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + ean + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + refFornec + "</td>";
                    strItens += "<td style=\"text-align:left;\">" + item.Descricao + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.Qtde + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.Embalagem + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.unitario.ToString("N2") + "</td>";
                    strItens += "<td style=\"text-align:right; \">" + item.total.ToString("N2") + "</td>";
                    strItens += "</tr>";

                }
                conteudo = conteudo.Replace("<|ITENS|>", strItens);
                String strPagamentos = "";
                foreach (pedido_pagamentoDAO item in ped.PedPg)
                {

                    strPagamentos += "<tr>";
                    strPagamentos += "<td style=\"text-align:left; \">" + item.Tipo_pagamento + "</td>";
                    strPagamentos += "<td style=\"text-align:right;\">" + item.Valor.ToString("N2") + "</td>";
                    strPagamentos += "</tr>";
                }
                conteudo = conteudo.Replace("<|PAGAMENTOS|>", strPagamentos);

                Funcoes.enviarEmail(usr, fornec.email, "", "PEDIDO DE COMPRA", conteudo);
                showMessage("Email Enviado com Sucesso!",false);
                
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
                
            }

        }
        protected void btnOkError_Click(object sender, EventArgs e)
        {

            modalError.Hide();
        }

        protected void showMessage(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            modalError.Show();
        }

    }
}