using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class TransferenciaContasDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            transferencias_contasDAO obj = null;
            if (usr != null)
            {

                if (!IsPostBack)
                {
                    if (Request.Params["novo"] != null)
                    {
                        obj = new transferencias_contasDAO(usr);
                        status = "incluir";
                        obj.data = DateTime.Now;
                        obj.codigo_centro_custo = "049001004";
                        obj.usuario = usr.getUsuario();
                    }
                    else
                    {
                        String strId = Request.Params["id"].ToString();
                        status = "visualizar";
                        obj = new transferencias_contasDAO(strId, usr);
                    }

                    Session.Remove("objTransferencia" + urlSessao());
                    Session.Add("objTransferencia" + urlSessao(), obj);
                    carregarDados();

                }
                if (status.Equals("visualizar"))
                {
                    habilitar(false);
                }
                else
                {
                    habilitar(true);
                }

                carregaBtn();
            }
        }
        private void carregaBtn()
        {
            if (status.Equals("visualizar"))
            {
                divSaldoDestino.Visible = false;
                divSaldoOrigem.Visible = false;
            }

            carregabtn(pnBtn, null, null, "FALSE", null, (txtStatus.Text.Equals("ESTORNADA")?"FALSE":"Estornar"), null);
        }
        private void carregarDados()
        {
            transferencias_contasDAO obj = (transferencias_contasDAO)Session["objTransferencia" + urlSessao()];
            txtId.Text = obj.id.ToString().Equals("0") ? "" : obj.id.ToString();
            txtData.Text = obj.dataBr();
            txtDescricao.Text = obj.descricao;
            txtusuario.Text = obj.usuario;
            txtOrigem.Text = obj.conta_origem;
            txtDestino.Text = obj.conta_destino;
            txtObservacao.Text = obj.obs;
            lblSaldoOrigem.Text = "R$" + obj.saldo_ant_origem.ToString("N2");
            lblSaldoDestino.Text = "R$" + obj.saldo_ant_destino.ToString("N2");
            txtValor.Text = obj.valor.ToString("N2");
            txtStatus.Text = obj.status;
            ddlTipo.Text = obj.tipo;
            txtCodigoCentroCusto.Text = obj.codigo_centro_custo;
            lblCentroCusto.Text = obj.descricao_centro_custo;
            txtCodigoCentroCustoOrigem.Text = obj.codigo_centro_custoOrigem;
            lblCentroCustoOrigem.Text = obj.descricao_centro_custoOrigem;
            txtCodigoCentroCustoDestino.Text = obj.codigo_centro_custoDestino;
            lblCentroCustoDestino.Text = obj.descricao_centro_custoDestino;
        }

        private void carregarDadosObj()
        {
            transferencias_contasDAO obj = (transferencias_contasDAO)Session["objTransferencia" + urlSessao()];
            int.TryParse(txtId.Text, out obj.id);
            DateTime.TryParse(txtData.Text, out obj.data);

            obj.descricao = txtDescricao.Text;
            obj.usuario = txtusuario.Text;
            obj.conta_origem = txtOrigem.Text;
            obj.conta_destino = txtDestino.Text;
            obj.obs = txtObservacao.Text;
            Decimal.TryParse(lblSaldoOrigem.Text.Replace("R$", "").Trim(), out obj.saldo_ant_origem);
            Decimal.TryParse(lblSaldoDestino.Text.Replace("R$", "").Trim(), out obj.saldo_ant_destino);
            Decimal.TryParse(txtValor.Text, out obj.valor);
            obj.status = txtStatus.Text;
            obj.tipo = ddlTipo.Text;
            obj.codigo_centro_custo = txtCodigoCentroCusto.Text;
            obj.codigo_centro_custoOrigem = txtCodigoCentroCustoOrigem.Text;
            obj.codigo_centro_custoDestino = txtCodigoCentroCustoDestino.Text;
            Session.Remove("objTransferencia" + urlSessao());
            Session.Add("objTransferencia" + urlSessao(), obj);

        }


        private void habilitar(bool enable)
        {


            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
          
                if (ddlTipo.Text.Equals("SAQUE") || ddlTipo.Text.Equals("LANCAMENTO DEBITO"))
                {
                    divDestino.Visible = false;
                    txtDestino.Text = "";

                    divOrigem.Visible = true;

                }
                else if (ddlTipo.Text.Equals("DEPOSITO") || ddlTipo.Text.Equals("LANCAMENTO CREDITO"))
                {
                    divOrigem.Visible = false;
                    txtOrigem.Text = "";
                    divDestino.Visible = true;

                }
                else
                {
                    divCentroCusto.Visible = false;
                }
            


                
            
        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransferenciaContasDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            status = "editar";
            carregabtn(pnBtn, null, null, null, null, "Estornar", null);
            habilitar(true);
        }



        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("transferenciaContas.aspx");
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            Session.Remove("SalvaTransf");
            lblConfirmaTexto.Text = "Confirmar Estorno " + ddlTipo.Text + " ?";
            modalPnConfirma.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

                if (validaCamposObrigatorios())
                {
                    Session.Remove("SalvaTransf");
                    lblConfirmaTexto.Text = "Confirmar "+ddlTipo.Text+" ?";
                    modalPnConfirma.Show();

                }
            }
            catch (Exception err)
            {
                
                lblpnError.Text = err.Message;
                modalError.Show();
            }

        }

        protected bool validaCamposObrigatorios()
        {
            txtCodigoCentroCusto.BackColor = System.Drawing.Color.White;
            txtValor.BackColor = System.Drawing.Color.White;

            Decimal vVlr = 0 ;
            Decimal.TryParse(txtValor.Text, out vVlr);
            if (vVlr <= 0)
            {
                txtValor.BackColor = System.Drawing.Color.Red;
                throw new Exception("Valor não Pode estar Zerado");
            }
            //int centroCusto = 0;

            //int.TryParse(Conexao.retornaUmValor("Select count(*) from centro_custo where ltrim(rtrim(codigo_centro_custo)) = '"+txtCodigoCentroCusto.Text.Trim()+"'",null), out centroCusto);
            //if (centroCusto < 1)
            //{
            //    txtCodigoCentroCusto.BackColor = System.Drawing.Color.Red;
            //    throw new Exception("Codigo Centro de custo não existe");
            //}

            bool retorno = false;
            if (validaCampos(cabecalho) && validaCampos(conteudo))
                retorno = true;
            else
                retorno = false;
            return retorno;

        }


        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransferenciaContas.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {


            String[] campos = {     "txtId"  
                                    //,"txtData"  
                                    ,"txtusuario"
                                    ,"txtStatus"
                                    ,"txtCodigoCentroCustoOrigem"
                                    ,"txtCodigoCentroCustoDestino"
                               };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = {     (ddlTipo.Text.Equals("TRANSFERENCIA")||ddlTipo.Text.Equals("SAQUE")||ddlTipo.Text.Equals("LANCAMENTO DEBITO")?"txtOrigem":""),
                                    (ddlTipo.Text.Equals("TRANSFERENCIA")||ddlTipo.Text.Equals("DEPOSITO")||ddlTipo.Text.Equals("LANCAMENTO CREDITO")? "txtDestino":""),
                                    "txtValor",
                                    "txtDescricao", 
                                    "txtCodigoCentroCusto"
                               };
            return existeNoArray(campos, campo.ID + "");
        }




        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";


            switch (or)
            {
                case "Destino":
                case "Origem":
                    sqlLista = "Select conta_corrente.id_cc," +
                                       " Agencia= Agencia  , " +
                                       " Conta= conta  " +
                                       ", Saldo" +
                                       ",[Centro custo]= conta_corrente.codigo_centro_custo " +
                                       ",[Descricao]=centro_custo.descricao_centro_custo" +
                               " from conta_corrente" +
                               "   left join centro_custo on conta_corrente.codigo_centro_custo=centro_custo.codigo_centro_custo and centro_custo.filial=conta_corrente.filial " +
                               " where ( conta_corrente.id_cc like '%" + TxtPesquisaLista.Text + "%' or Agencia like '%" + TxtPesquisaLista.Text + "%' or conta like '%" + TxtPesquisaLista.Text + "')" ;
                    lbllista.Text = "Conta";
                    break;
                case "CentroCusto":

                    usr.consultaTodasFiliais = true;
                    sqlLista = "select Codigo = Codigo_centro_custo,Grupo=descricao_grupo, SubGrupo= Descricao_subgrupo, [Centro custo] = Descricao_centro_custo "+ 
                               " from centro_custo " +
                                   " inner join subgrupo_cc on subgrupo_cc.codigo_subgrupo = centro_custo.codigo_subgrupo "+
                                   " inner join grupo_cc on grupo_cc.codigo_grupo = subgrupo_cc.codigo_grupo "+

                               " where ( codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%' or Descricao_centro_custo like '"+TxtPesquisaLista.Text +"%' or Descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "')";
                    if (ddlTipo.Text.Equals("LANCAMENTO DEBITO"))
                    {
                        sqlLista += " and isnull(contabil,0) =1 ";
                    }
                        
                        lbllista.Text = "Centro de custo ";
                    
                    break;


            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            GridLista.DataBind();
            usr.consultaTodasFiliais = false;
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }

            TxtPesquisaLista.Focus();
            modalPnFundo.Show();
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            Session.Remove("camporecebe");

            switch (btn.ID)
            {
                case "imgBtnOrigem":
                    Session.Add("camporecebe", "Origem");
                    break;
                case "imgBtnDestino":
                    Session.Add("camporecebe", "Destino");
                    break;
                case "imgBtnCentroCusto":
                    Session.Add("camporecebe", "CentroCusto");
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
                        return item.Cells[campo].Text;
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

                if (listaAtual.Equals("Origem"))
                {

                    txtOrigem.Text = ListaSelecionada(1);
                    lblSaldoOrigem.Text = "R$ " + ListaSelecionada(4);
                    txtCodigoCentroCustoOrigem.Text = ListaSelecionada(5);
                    lblCentroCustoOrigem.Text = ListaSelecionada(6);


                }
                if (listaAtual.Equals("Destino"))
                {
                    txtDestino.Text = ListaSelecionada(1);
                    lblSaldoDestino.Text = "R$ " + ListaSelecionada(4);
                    txtCodigoCentroCustoDestino.Text = ListaSelecionada(5);
                    lblCentroCustoDestino.Text = ListaSelecionada(6);
                }
                if (listaAtual.Equals("CentroCusto"))
                {
                    txtCodigoCentroCusto.Text = ListaSelecionada(1);
                    lblCentroCusto.Text = ListaSelecionada(4);
                    if (ddlTipo.Text.Equals("SAQUE") || ddlTipo.Text.Equals("LANCAMENTO DEBITO"))
                    {
                        txtCodigoCentroCustoDestino.Text = txtCodigoCentroCusto.Text;
                        
                    }
                    else if (ddlTipo.Text.Equals("DEPOSITO") || ddlTipo.Text.Equals("LANCAMENTO CREDITO"))
                    {
                        divOrigem.Visible = false;
                        txtCodigoCentroCustoOrigem.Text = txtCodigoCentroCusto.Text;
                    }

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
            modalPnFundo.Hide();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void txtOrigem_Changed(object sender, EventArgs e)
        {
            centroCustoConta(txtOrigem.Text, false);

        }
        protected void txtDestino_Changed(object sender, EventArgs e)
        {
            centroCustoConta(txtDestino.Text, true);
        }


        private void centroCustoConta(String id_cc,bool destino)
        {
            User usr = (User)Session["User"];

            SqlDataReader rs = null;
            try
            {
               rs =  Conexao.consulta("Select saldo" +
                                   ",conta_corrente.codigo_centro_custo" +
                                   ",centro_custo.descricao_centro_custo" +
               " from conta_corrente " +
               "   inner join centro_custo on conta_corrente.codigo_centro_custo = centro_custo.codigo_centro_custo" +
               " where conta_corrente.id_cc='" + id_cc + "'", usr,false);

                if (rs.Read())
                {
                    if(destino)
                    {
                        lblSaldoDestino.Text = "R$" + Funcoes.decTry(rs["saldo"].ToString()).ToString("N2");
                        txtCodigoCentroCustoDestino.Text = rs["codigo_centro_custo"].ToString();
                        lblCentroCustoDestino.Text = rs["descricao_centro_custo"].ToString();
                    }
                    else
                    {
                        lblSaldoOrigem.Text = "R$" + Funcoes.decTry(rs["saldo"].ToString()).ToString("N2");
                        txtCodigoCentroCustoOrigem.Text = rs["codigo_centro_custo"].ToString();
                        lblCentroCustoOrigem.Text = rs["descricao_centro_custo"].ToString();
                    }

                }




            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();

            }
           
        }
        protected void btnConfirmacao_Click(object sender, EventArgs e)
        {
            try
            {
                string salvar =(String) Session["SalvaTransf"];
                if(salvar != null)
                {

                    status = "visualizar";
                    carregarDados();
                    habilitar(false);
                    modalPnConfirma.Hide();
                    carregaBtn();
                    return;
                }

                Session.Add("SalvaTransf", "true");
                User usr = (User)Session["User"];
                carregarDadosObj();
                transferencias_contasDAO obj = (transferencias_contasDAO)Session["objTransferencia" + urlSessao()];
                
                if (lblConfirmaTexto.Text.Equals("Confirmar "+ddlTipo.Text+" ?"))
                {
                    obj.obs += "\n\nCONFIRMADO " + obj.tipo + " DATA " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " User:" + usr.getUsuario();
                    if (!obj.conta_origem.Equals(""))
                    {
                        obj.obs += "\nOrigem:" + obj.conta_origem +
                                  "\n Saldo Ant: R$" + obj.saldo_ant_origem.ToString("N2") +
                                  "\n Saldo Novo: R$" + (obj.saldo_ant_origem - obj.valor).ToString("N2");
                    }
                    if (!obj.conta_destino.Equals(""))
                    {
                        obj.obs +=
                              "\nDestino:" + obj.conta_destino +
                              "\n Saldo Ant: R$" + obj.saldo_ant_destino.ToString("N2") +
                              "\n Saldo Novo: R$" + (obj.saldo_ant_destino + obj.valor).ToString("N2");
                    }
                    obj.status = "CONCLUIDA";
                    obj.salvar(true);        
                }
                else
                {
                    if (lblConfirmaTexto.Text.Equals("Confirmar Estorno " + ddlTipo.Text + " ?"))
                    {

                        Decimal vlrSaldoOrigem = 0;
                        Decimal.TryParse(Conexao.retornaUmValor("Select saldo from conta_corrente where id_cc='" + txtOrigem.Text + "'", usr), out vlrSaldoOrigem);

                        Decimal vlrSaldoDestino = 0;
                        Decimal.TryParse(Conexao.retornaUmValor("Select saldo from conta_corrente where id_cc='" + txtDestino.Text + "'", usr), out vlrSaldoDestino);

                        obj.obs += "\n\nESTORNO DATA " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " User:" + usr.getUsuario();
                            
                        if (!obj.conta_origem.Equals(""))
                        {
                            obj.obs += "\nOrigem:" + obj.conta_origem +
                            "\n Saldo Ant: R$" + vlrSaldoOrigem.ToString("N2") +
                            "\n Saldo Novo: R$" + (vlrSaldoOrigem + obj.valor).ToString("N2");
                        }

                        if (!obj.conta_destino.Equals(""))
                        {
                        obj.obs += 
                            "\nDestino:" + obj.conta_destino +
                            "\n Saldo Ant: R$" + vlrSaldoDestino.ToString("N2") +
                            "\n Saldo Novo: R$" + (vlrSaldoDestino - obj.valor).ToString("N2");
                        }

                        obj.excluir();
                    }
                }
                
                
                status = "visualizar";
                carregarDados();
                habilitar(false);
                modalPnConfirma.Hide();
                carregaBtn();

                
            }
            catch (Exception err)
            {
                lblpnError.Text = err.Message;
                modalError.Show();
            }

        }

        protected void btnCancelaConfirmacao_Click(object sender, EventArgs e)
        {
            modalPnConfirma.Hide();
        }

        protected void ddlTipo_Changed(object sender, EventArgs e)
        {
            txtDestino.Text = "";
            txtOrigem.Text = "";
            lblSaldoDestino.Text = "";
            lblSaldoOrigem.Text = "";
            txtCodigoCentroCustoDestino.Text = "";
            txtCodigoCentroCustoOrigem.Text = "";
            lblCentroCustoDestino.Text = "";
            lblCentroCustoOrigem.Text = "";
            txtCodigoCentroCusto.Text = "";
            lblCentroCusto.Text = "";
            divCentroCusto.Visible = true;
            User usr = (User)Session["User"];
            if (ddlTipo.Text.Equals("SAQUE"))
            {
                txtOrigem.Text = Conexao.retornaUmValor("SELECT ID_CC FROM CONTA_CORRENTE WHERE CONTA_CAIXA =1", usr);
                divOrigem.Visible = true;
                divDestino.Visible = false;
            }
            else if (ddlTipo.Text.Equals("DEPOSITO") || ddlTipo.Text.Equals("LANCAMENTO CREDITO"))
            {
                divDestino.Visible = true;
                divOrigem.Visible = false;
            }
            else if (ddlTipo.Text.Equals("LANCAMENTO DEBITO"))
            {
                divOrigem.Visible = true;
                divDestino.Visible = false;
                
            }
            else
            {
                divDestino.Visible = true;
                divOrigem.Visible = true;
                divCentroCusto.Visible = false;
                txtCodigoCentroCusto.Text = "049001004";
            }

        }
    }
}