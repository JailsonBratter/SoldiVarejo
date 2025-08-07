using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using visualSysWeb.dao;
using System.Data;
using System.Data.SqlClient;



namespace visualSysWeb.modulos.PetShop.pages
{
    public partial class Agenda : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
               
                carregarDllhorarios();
                if (txtIntervalo.Text.Equals(""))
                {
                    txtIntervalo.Text = Conexao.retornaUmValor("Select top 1 intervalo from agenda_horarios ", null);
                }

            }

            montarAgenda();








        }


        private void montarHorarios()
        {
            DateTime Inicio = DateTime.Parse(txtHorarioInicio.Text);
            DateTime Fim = DateTime.Parse(txtHorarioFim.Text);
            int intervalo = int.Parse(txtIntervalo.Text); // minutos

            Conexao.executarSql("delete from agenda_horarios");

            for (DateTime i = Inicio; i < Fim; i = i.AddMinutes(intervalo))
            {

                String strHorario = i.ToString("HH:mm");
                Conexao.executarSql("insert agenda_horarios (horario,intervalo) values('" + strHorario + "'," + intervalo + ")");
               

            }
        }

        protected void btnMontar_Click(object sender, EventArgs e)
        {
            montarHorarios();
            montarAgenda();
            carregarDllhorarios();
        }

        protected void montarAgenda()
        {


            pnAgenda.Controls.Clear();
            lblError.Text = "";
            SqlDataReader rsAgenda = null;
            SqlDataReader rsFuncionarios = null;
            try
            {
                DateTime DtCalendario = new DateTime();
                try
                {
                    txtData.BackColor = System.Drawing.Color.White;

                    DtCalendario = DateTime.Parse(txtData.Text);

                    //Calendar1.SelectedDate = DateTime.Parse(txtData.Text);

                    //montarAgenda();
                }
                catch (Exception)
                {

                    txtData.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Data invalida");
                }



                Panel pnCabecalho = new Panel();
                pnCabecalho.Controls.Add(celula("HORARIO", "", false, false, true));
                pnCabecalho.CssClass = "row";
                ArrayList funcionarios = new ArrayList();

                rsFuncionarios = Conexao.consulta("Select NOME= UPPER(nome),Funcao,inicio,fim from Funcionario WHERE utiliza_agenda = 1;", null, false);
                String strAgendaSql = "Select HORARIO =  ah.horario";
                String colunas = "";
                while (rsFuncionarios.Read())
                {
                    String nome = rsFuncionarios["nome"].ToString();
                    if (nome.Length >= 9)
                        nome = nome.ToString().Substring(0, 9);

                    pnCabecalho.Controls.Add(celula(nome + "<br> " + rsFuncionarios["FUNCAO"].ToString().Substring(0, 3), "", false, false, false));
                    funcionarios.Add(rsFuncionarios["inicio"].ToString().PadLeft(5, '-') + "_" + rsFuncionarios["fim"].ToString().PadLeft(5, '-') + "_" + rsFuncionarios["nome"].ToString());
                    colunas += ",'" + rsFuncionarios["nome"].ToString().Trim() + "'=ISNULL((SELECT top 1 agenda.Pedido+'-'+ agenda.Nome_pet " +
                                            "				FROM Agenda " +
                                            "				WHERE Nome ='" + rsFuncionarios["nome"].ToString() + "' " +
                                            "					AND (ah.horario >= agenda.Inicio AND ah.horario < agenda.Fim) " +
                                            "					AND DATA ='" + DtCalendario.ToString("yyyyMMdd") + "'),'')";

                }
                strAgendaSql += colunas + " from agenda_horarios ah ";

                if (rsFuncionarios != null)
                    rsFuncionarios.Close();

                pnAgenda.Controls.Add(pnCabecalho);

                rsAgenda = Conexao.consulta(strAgendaSql, null, false);
                while (rsAgenda.Read())
                {
                    Panel pnRow = new Panel();
                    pnRow.CssClass = "row";
                    pnAgenda.Controls.Add(pnRow); ;
                    pnRow.Controls.Add(celula(rsAgenda["HORARIO"].ToString(), "", false, true, true));
                    foreach (String func in funcionarios)
                    {
                        DateTime dtInicioExp;
                        DateTime dtFimExp;
                        DateTime dtHora = DateTime.Parse(rsAgenda["HORARIO"].ToString());
                        String strNomeFuncionario = func.Substring(12).Trim();
                        String strInicioExp = func.Substring(0, 5);
                        String strFimExp = func.Substring(6, 5);

                        if (strInicioExp.IndexOf('-') >= 0)
                        {
                            dtInicioExp = dtHora;
                            dtFimExp = dtHora.AddDays(2);
                        }
                        else
                        {
                            dtInicioExp = DateTime.Parse(strInicioExp);
                            dtFimExp = DateTime.Parse(strFimExp);
                        }
                        if (dtHora >= dtInicioExp && dtHora < dtFimExp)
                        {
                            pnRow.Controls.Add(celula(rsAgenda[strNomeFuncionario].ToString(), rsAgenda["HORARIO"].ToString() + "_" + strNomeFuncionario, true, true, false));
                        }
                        else
                        {
                            pnRow.Controls.Add(celula("INDISPONIVEL", rsAgenda["HORARIO"].ToString() + "_" + strNomeFuncionario, true, true, false));
                        }

                    }


                }

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (rsAgenda != null)
                    rsAgenda.Close();

                if (rsFuncionarios != null)
                    rsFuncionarios.Close();
            }
        }

        protected Panel celula(String texto, String horario, bool botao, bool celula, bool celHorario)
        {
            Hashtable agendados = (Hashtable)Session["Agendado"];
            if(agendados ==null)
            {
                agendados = new Hashtable();
            }

            Panel pn = new Panel();

            if (celula)
                pn.CssClass = "AgendaCelula" + (celHorario ? "Horario" : "");
            else
                pn.CssClass = "AgendaCabecalho" + (celHorario ? "Horario" : "");

            if (botao)
            {
                Button btn = new Button();
                btn.ID = "btn" + horario.Replace(":", "");
                if (texto.Length > 0)
                {
                    if (texto.IndexOf("INDISPONIVEL") >= 0)
                    {
                        btn.Text = "";
                        btn.Enabled = false;
                        btn.CssClass = "tdIndisponivel";
                    }
                    else
                    {
                        btn.Click += btnOcupado_Click;
                        String pedido = texto.Substring(0, texto.IndexOf("-"));
                        if (agendados.Contains(btn.ID))
                        {
                            agendados[btn.ID]=pedido;
                        }
                        else
                        {
                            agendados.Add(btn.ID, pedido);
                        }
                        btn.Text = texto.Substring(texto.IndexOf("-")+1);
                        btn.CssClass = "tdOcupado";

                        Session.Remove("Agendado");
                        Session.Add("Agendado", agendados);
                    }
                }
                else
                {
                    btn.Click += btnLivre_Click;
                    btn.Text = "";
                    btn.CssClass = "tdLivre";
                }

                pn.Controls.Add(btn);

            }
            else
            {
                Label lbltexto = new Label();
                lbltexto.Text = "<center>" + texto.Substring(texto.IndexOf("-") + 1) + "</center>";
                pn.Controls.Add(lbltexto);
            }
            return pn;
        }
        protected void btnLivre_Click(object sender, EventArgs e)
        {
            divImprimir.Visible = false;
            ddlPetsCliente.BackColor = System.Drawing.Color.White;
            lblErrorAgendamento.Text = "";
            LimparCampos(pnDetalhesAgenda);
            ddlPetsCliente.Text = "";
            ddlPetsCliente.Items.Clear();

            //txtCodPedido.Text = "";
            //txtCodCliente.Text = "";
            //txtCliente.Text = "";
            //txtNomePet.Text = "";
            //txtFuncionarioEntrega.Text = "";
            //txtFuncionarioRetira.Text = "";
            //txtkmSaida.Text = "";
            //txt

            EnabledControls(pnDetalhesAgenda, true);
            divAgenda.Visible = false;
            pnDetalhesAgenda.Visible = true;
            Button btn = (Button)sender;
            txtDtAgendamento.Text = txtData.Text;
            String horario = btn.ID.Substring(3, 2) + ":" + btn.ID.Substring(5, 2);
            ddlInicio.Text = horario;
            lkMesmo.Visible = true;
            lsServicos.Items.Clear();
            try
            {
                ddlFim.Text = DateTime.Parse(ddlInicio.Text).AddMinutes(Double.Parse(txtIntervalo.Text)).ToString("HH:mm");
            }
            catch (Exception)
            {
                ddlFim.Text = ddlInicio.Text;
            }
            txtNome.Text = btn.ID.Substring(8);

        }
        protected void btnOcupado_Click(object sender, EventArgs e)
        {
            divImprimir.Visible = true;
            Hashtable agendados = (Hashtable)Session["Agendado"];
            lblErrorAgendamento.Text = "";
            Button btn = (Button)sender;
            User usr = (User)Session["User"];
            String codigo = (String)agendados[btn.ID];
            AgendaDAO ag = new AgendaDAO(codigo, usr);
            txtCodPedido.Text = ag.Pedido;
            txtNome.Text = ag.Nome;
            txtCodCliente.Text = ag.Codigo_Cliente;
            txtCliente.Text = ag.NomeCliente;
            txtusuario.Text = ag.usuario_cadastro;
            Conexao.preencherDDL1Branco(ddlPetsCliente, "select Nome_Pet from Cliente_Pet where codigo_cliente= " + ag.Codigo_Cliente, "Nome_pet", "Nome_pet", usr);
            ddlPetsCliente.Text = ag.Nome_Pet;
            txtDtAgendamento.Text = ag.DataBr();
            try
            {
                ddlInicio.Text = ag.Inicio;

            }
            catch (Exception)
            {
                DateTime tA;
                DateTime.TryParse(ag.Inicio, out tA);
                foreach (ListItem item in ddlInicio.Items)
                {
                    DateTime t;
                    DateTime.TryParse(item.Text, out t);


                    if (t > tA)
                    {
                        ddlInicio.Text = item.Text;
                        break;
                    }

                }

            }
            try
            {


                ddlFim.Text = ag.Fim;
            }
            catch (Exception)
            {

                DateTime tA;
                DateTime.TryParse(ag.Fim, out tA);
                foreach (ListItem item in ddlFim.Items)
                {
                    DateTime t;
                    DateTime.TryParse(item.Text, out t);


                    if (t > tA)
                    {
                        ddlFim.Text = item.Text;
                        break;
                    }

                }
            }
            txtHoraRetirada.Text = ag.Hora_retirada;
            chkDelivery.Checked = ag.delivery;
            txtHoraEntregaPrevista.Text = ag.Hora_entrega_prevista;
            txtEntregaReal.Text = ag.Hora_entrega_real;
            txtkmSaida.Text = ag.Saida_KM.ToString();
            txtKmChegada.Text = ag.Chegada_KM.ToString();
            txtFuncionarioEntrega.Text = ag.Funcionario_entrega;
            txtObs.Text = ag.Obs;
            txtObsVeterinario.Text=  ag.Obs_Veterinario;
            string[] arrLinha = ag.Sigla.ToString().Split(';');
            lsServicos.Items.Clear();
            foreach (string item in arrLinha)
            {
                if (!item.Trim().Equals(""))
                {
                    lsServicos.Items.Add(item);
                }
            }


            EnabledControls(pnDetalhesAgenda, true);
       
            divAgenda.Visible = false;
            pnDetalhesAgenda.Visible = true;

        }


        private void carregarDllhorarios()
        {
            Conexao.preencherDDL(ddlInicio, "select horario,intervalo from agenda_horarios", "horario", "horario", null);
            Conexao.preencherDDL(ddlFim, "select horario,intervalo from agenda_horarios", "horario", "horario", null);

            String strSql = "Select " +
                                   "fim =(select MAX(horario) from agenda_horarios)," +
                                   "intervalo = (select top 1 intervalo from agenda_horarios)";

            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(strSql, null, false);
                if (rs.Read())
                {

                    String ultimoHorario = DateTime.Parse(rs["fim"].ToString()).AddMinutes(Double.Parse(rs["intervalo"].ToString())).ToString("HH:mm");
                    ddlFim.Items.Add(ultimoHorario);
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        protected void imgConfig_Click(object sender, ImageClickEventArgs e)
        {
            String strSql = "Select inicio = (select MIN(horario)from agenda_horarios),  " +
                                      "fim =(select MAX(horario) from agenda_horarios)," +
                                      "intervalo = (select top 1 intervalo from agenda_horarios)";

            SqlDataReader rs = Conexao.consulta(strSql, null, false);
            if (rs.Read())
            {
                txtHorarioInicio.Text = rs["inicio"].ToString();
                txtIntervalo.Text = rs["intervalo"].ToString();
                if (!txtHorarioInicio.Text.Equals(""))
                {
                    txtHorarioFim.Text = DateTime.Parse(rs["fim"].ToString()).AddMinutes(Double.Parse(txtIntervalo.Text)).ToString("HH:mm");
                }
            }

            if (rs != null)
                rs.Close();
            modalConfiguracao.Show();
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
           

            montarAgenda();
        }

        protected void txtData_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtData.BackColor = System.Drawing.Color.White;

                DateTime.Parse(txtData.Text);

          
            }
            catch (Exception)
            {

                txtData.BackColor = System.Drawing.Color.Red;
            }
        }

        protected void imgBtnFechar_Click(object sender, ImageClickEventArgs e)
        {
            divAgenda.Visible = true;
            pnDetalhesAgenda.Visible = false;
        }

        //imgImprimir_Click
        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('AgendaPrint.aspx?pedido="+txtCodPedido.Text.Trim()+"','_blank');", true);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                ddlPetsCliente.BackColor = System.Drawing.Color.White;
                txtHoraRetirada.BackColor = System.Drawing.Color.White;
                txtFuncionarioRetira.BackColor = System.Drawing.Color.White;
                txtEntregaReal.BackColor = System.Drawing.Color.White;
                txtHoraEntregaPrevista.BackColor = System.Drawing.Color.White;
                txtFuncionarioEntrega.BackColor = System.Drawing.Color.White;
                ddlFim.BackColor = System.Drawing.Color.White;
                lsServicos.BackColor = System.Drawing.Color.White;
                if (txtCodCliente.Text.Equals(""))
                {
                    txtCodCliente.BackColor = System.Drawing.Color.Red;
                    ddlPetsCliente.Items.Clear();
                    throw new Exception("Escolha um Cliente");
                }
                else
                {
                    int existe;
                    int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from cliente where codigo_cliente=" + txtCodCliente.Text + "", null), out existe);
                    if (existe <= 0)
                    {
                        txtCodCliente.BackColor = System.Drawing.Color.Red;
                        ddlPetsCliente.Items.Clear();
                        throw new Exception("Cliente não cadastrado");
                    }
                }

                if (ddlPetsCliente.Text.Equals(""))
                {
                    ddlPetsCliente.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Escolha um Pet");
                }

                if (lsServicos.Items.Count==0)
                {
                    lsServicos.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Escolha um Serviço");
                }
                
                if (!txtHoraRetirada.Text.Equals("") && !chkDelivery.Checked)
                {
                    chkDelivery.Checked = true;
                }

                if (chkDelivery.Checked)
                {
                    if (txtHoraRetirada.Text.Equals(""))
                    {
                        txtHoraRetirada.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Digite a hora de Retirada");
                    }

                    if (txtFuncionarioRetira.Text.Equals(""))
                    {
                        txtFuncionarioRetira.BackColor = System.Drawing.Color.Red;
                        throw new Exception("escolha o funcionario");
                    }
                    else
                    {
                        int existe;
                        int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from funcionario where Nome='" + txtFuncionarioRetira.Text + "'", null), out existe);
                        if (existe <= 0)
                        {
                            txtFuncionarioRetira.BackColor = System.Drawing.Color.Red;
                            throw new Exception("Funcionario não cadastrado ");
                        }

                    }
                }

                if (ddlInicio.Text.Equals(ddlFim.Text) || DateTime.Parse(ddlInicio.Text) > DateTime.Parse(ddlFim.Text))
                {
                    ddlFim.BackColor = System.Drawing.Color.Red;
                    throw new Exception("Escolha o horário fim maior que o horário de início");
                }

                if (!txtHoraRetirada.Text.Equals(""))
                {
                    DateTime hrRetira;
                    DateTime hrInicio;

                    DateTime.TryParse(txtHoraRetirada.Text, out hrRetira);
                    DateTime.TryParse(ddlInicio.Text, out hrInicio);

                    if (hrRetira >= hrInicio)
                    {
                        txtHoraRetirada.BackColor = System.Drawing.Color.Red;
                        throw new Exception("O Horário da Retirada tem que ser Menor que o Horário do Agendamento");
                    }

                }

                if (!txtHoraEntregaPrevista.Text.Equals(""))
                {
                    DateTime hrEnt;
                    DateTime hrRetira;

                    DateTime.TryParse(txtHoraRetirada.Text, out hrRetira);

                    DateTime.TryParse(txtHoraEntregaPrevista.Text, out hrEnt);

                    if (hrEnt <= hrRetira)
                    {
                        txtHoraEntregaPrevista.BackColor = System.Drawing.Color.Red;
                        throw new Exception("O Horário de entrega tem que ser maior que o horário de retirada");
                    }

                    if (txtFuncionarioEntrega.Text.Equals(""))
                    {
                        txtFuncionarioEntrega.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Informe um funcionario para entrega");
                    }

                }
                if (!txtEntregaReal.Text.Equals(""))
                {
                    DateTime hrEnt;
                    DateTime hrRetira;

                    DateTime.TryParse(txtHoraRetirada.Text, out hrRetira);

                    DateTime.TryParse(txtEntregaReal.Text, out hrEnt);

                    if (hrEnt <= hrRetira)
                    {
                        txtEntregaReal.BackColor = System.Drawing.Color.Red;
                        throw new Exception("O Horário de entrega tem que ser maior que o horário de retirada");
                    }

                }

                if (!txtFuncionarioEntrega.Text.Trim().Equals(""))
                {
                    int existe;
                    int.TryParse(Conexao.retornaUmValor("Select COUNT(*) from funcionario where Nome='" + txtFuncionarioEntrega.Text + "'", null), out existe);
                    if (existe <= 0)
                    {
                        txtFuncionarioEntrega.BackColor = System.Drawing.Color.Red;
                        throw new Exception("Funcionario não cadastrado ");
                    }
                }



                User usr = (User)Session["User"];
                AgendaDAO ag;
                if (txtCodPedido.Text.Equals(""))
                {
                    ag = new AgendaDAO(usr);
                }
                else
                {
                    ag = new AgendaDAO(txtCodPedido.Text, usr);
                }
                ag.Pedido = txtCodPedido.Text;
                ag.Nome = txtNome.Text;
                ag.Codigo_Cliente = txtCodCliente.Text;
                ag.usuario_cadastro = usr.getNome();

                ag.Nome_Pet = ddlPetsCliente.Text;
                ag.Data = DateTime.Parse(txtDtAgendamento.Text);
                ag.Inicio = ddlInicio.Text;
                ag.Fim = ddlFim.Text;
                ag.Hora_retirada = txtHoraRetirada.Text;
                ag.delivery = chkDelivery.Checked;
                ag.Hora_entrega_prevista = txtHoraEntregaPrevista.Text;
                ag.Hora_entrega_real = txtEntregaReal.Text;
                ag.Saida_KM = (txtkmSaida.Text.Equals("") ? 0 : int.Parse(txtkmSaida.Text));
                ag.Chegada_KM = (txtKmChegada.Text.Equals("") ? 0 : int.Parse(txtKmChegada.Text));
                ag.Funcionario_entrega = txtFuncionarioEntrega.Text;
                ag.Funcionario_retira = txtFuncionarioRetira.Text;
                ag.Obs_Veterinario = txtObsVeterinario.Text;
                ag.Obs = txtObs.Text;
                ag.Sigla = "";
                foreach (ListItem item in lsServicos.Items)
                {
                    if (!ag.Sigla.Equals(""))
                    {
                        ag.Sigla += ";";
                    }
                    ag.Sigla += item ;
                }
                ag.salvar(txtCodPedido.Text.Equals(""));

                txtCodPedido.Text = ag.Pedido;
                


                divAgenda.Visible = true;
                pnDetalhesAgenda.Visible = false;
                montarAgenda();
                modalImprime.Show();

            }
            catch (Exception erro)
            {

                lblErrorAgendamento.Text = erro.Message;

            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = { "txtCodPedido",
                                    "txtNome", 
                                    "txtDtAgendamento", 
                                    "txtusuario",
                                     };

            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
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
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        protected void exibeLista()
        {
            String campo = (String)Session["campoLista" + urlSessao()];
            String sqlLista = "";
            User usr = (User)Session["User"];
            switch (campo)
            {
                case "imgCliente":
                    lblTitulo.Text = "Escolha um Cliente";
                    sqlLista = "select CODIGO=Codigo_Cliente,NOME=Nome_Cliente,CPF=CNPJ from cliente WHERE (CODIGO_CLIENTE+Nome_Cliente+CNPJ) LIKE '%" + TxtPesquisaLista.Text.Replace(" ", "%") + "%' order by CODIGO_CLIENTE DESC  ";



                    break;
                case "imgPetCliente":
                    lblTitulo.Text = "Escolha um Pet ";

                    sqlLista = "select Nome_Pet,Sexo,Raca,Cor,Porte,cliente.Codigo_Cliente, cliente.Nome_Cliente as Nome "+
                                  " from Cliente_Pet inner join cliente on cliente_pet.Codigo_Cliente=cliente.Codigo_Cliente where nome_pet like '%"+TxtPesquisaLista.Text+"%'";
                    break;
                case "imgFuncionarioEntrega":
                case "imgFuncionarioRetira":
                    lblTitulo.Text = "Escolha um Funcionario";
                    sqlLista = "select Nome ,Funcao from Funcionario where Nome like'" + TxtPesquisaLista.Text.Replace(" ", "%") + "%'";
                    break;
                case "imgServico":
                    lblTitulo.Text = "Escolha um Serviço";
                    sqlLista = "Select ltrim(rtrim(Sigla)) as Sigla,ltrim(rtrim(Descricao))as Descricao from servico  where sigla like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'";
                    break;

            }

            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            GridLista.DataBind();

            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }


            modalPesquisa.Show();
            TxtPesquisaLista.Focus();
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
        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").Replace(" ", "").Replace("+", "")];

            if (itemLista.Equals("imgCliente"))
            {
                txtCodCliente.Text = ListaSelecionada(1).Trim();
                txtCliente.Text = ListaSelecionada(2).Trim();
                ddlPetsCliente.Items.Clear();
                Conexao.preencherDDL1Branco(ddlPetsCliente, "select Nome_Pet from Cliente_Pet where codigo_cliente= '" + txtCodCliente.Text + "'", "Nome_pet", "Nome_pet", usr);

            }
            else if (itemLista.Equals("imgPetCliente"))
            {
                txtCodCliente.Text = ListaSelecionada(6);
                txtCliente.Text = ListaSelecionada(7);
                Conexao.preencherDDL1Branco(ddlPetsCliente, "select Nome_Pet from Cliente_Pet where codigo_cliente= '" + txtCodCliente.Text + "'", "Nome_pet", "Nome_pet", usr);

                ddlPetsCliente.Text = ListaSelecionada(1);

                //txtNomePet.Text = ListaSelecionada(1).Trim();
            }
            else if (itemLista.Equals("imgFuncionarioRetira"))
            {
                txtFuncionarioRetira.Text = ListaSelecionada(1).Trim();
            }
            else if (itemLista.Equals("imgFuncionarioEntrega"))
            {
                txtFuncionarioEntrega.Text = ListaSelecionada(1).Trim();
            }
            

            modalPesquisa.Hide();
        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPesquisa.Hide();
        }
        

        protected void btnCancelarAgendamento_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCodPedido.Text.Equals(""))
            {
                imgBtnFechar_Click(sender, e);
            }
            else
            {
                modalPnConfirma.Show();
            }

        }

        //btnConfirmaExclusao_Click
        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            AgendaDAO ag;
            if (txtCodPedido.Text.Equals(""))
            {
                ag = new AgendaDAO(usr);
            }
            else
            {
                ag = new AgendaDAO(txtCodPedido.Text, usr);
            }

            ag.excluir();
            montarAgenda();
            imgBtnFechar_Click(sender, e);

        }


        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnSelecionaServico_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            gridServicos.DataSource = Conexao.GetTable("Select ltrim(rtrim(Sigla)) as Sigla,ltrim(rtrim(Descricao))as Descricao from servico ",usr,false);
            gridServicos.DataBind();

            
            

            foreach (GridViewRow item in gridServicos.Rows)
            {
                foreach (ListItem sItem in lsServicos.Items)
                {
                    if (item.Cells[2].Text.Equals(sItem.Text))
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkServico");
                        chk.Checked = true;
                        break;
                    }
                }    
            }
            modalServicos.Show();
        }

        //btnConfirmaServicos_Click
        protected void btnConfirmaServicos_Click(object sender, ImageClickEventArgs e)
        {
            lsServicos.Items.Clear();
            foreach (GridViewRow item in gridServicos.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkServico");
                if (chk.Checked)
                {
                    lsServicos.Items.Add(item.Cells[2].Text);
                }
            }
            modalServicos.Hide();
        }

        //btnCancelaServicos_Click
        protected void btnCancelaServicos_Click(object sender, ImageClickEventArgs e)
        {
            modalServicos.Hide();
        }
 
        protected void btnImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            exibeLista(btn.ID);
        }
        private void exibeLista(String id)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), id);
            exibeLista();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetNomesClientes(string prefixText, int count)
        {
            String sql = "	Select rtrim(ltrim(codigo_cliente)) +'|'+ ltrim(rtrim(nome_cliente))  from cliente where (nome_cliente like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%' ) and isnull(inativo,0)=0";
            return Conexao.retornaArray(sql, prefixText.Length);
        }


        //GetServicos
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetServicos(string prefixText, int count)
        {
            String sql = "	Select ltrim(rtrim(Sigla))+' | '+ltrim(rtrim(Descricao)) from servico  where (Descricao like '" + (prefixText.Length > 4 ? "%" : "") + prefixText + "%' )";
            return Conexao.retornaArray(sql, prefixText.Length);
        }


        protected void txtCodCliente_TextChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (!txtCodCliente.Text.Trim().Equals(""))
            {
                txtCliente.Text = Conexao.retornaUmValor("Select ltrim(rtrim(nome_cliente)) from cliente where rtrim(ltrim(codigo_cliente))=" + txtCodCliente.Text.Trim(), usr);

                Conexao.preencherDDL1Branco(ddlPetsCliente, "select Nome_Pet from Cliente_Pet where codigo_cliente= " + txtCodCliente.Text, "Nome_pet", "Nome_pet", usr);
                ddlPetsCliente.Focus();
            }
            else
            {
                ddlPetsCliente.Items.Clear();
                txtCliente.Text = "";
            }
        }

        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtCliente.Text.IndexOf("|") >= 0)
            {
                txtCodCliente.Text = txtCliente.Text.Substring(0, txtCliente.Text.IndexOf("|")).Trim();
                txtCliente.Text = txtCliente.Text.Substring(txtCliente.Text.IndexOf("|") + 1).Trim();
                User usr = (User)Session["User"];
                Conexao.preencherDDL1Branco(ddlPetsCliente, "select Nome_Pet from Cliente_Pet where codigo_cliente= '" + txtCodCliente.Text + "'", "Nome_pet", "Nome_pet", usr);
                ddlPetsCliente.Focus();
            }
            else
            {
                txtCodCliente.Text = "";
            }

        }
        //imgBtnSimImprime_Click
        protected void imgBtnSimImprime_Click(object sender, ImageClickEventArgs e)
        {
            modalImprime.Hide();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('AgendaPrint.aspx?pedido=" + txtCodPedido.Text.Trim() + "','_blank');", true);
        }

        protected void imgBtnNaoImprime_Click(object sender, ImageClickEventArgs e)
        {
            modalImprime.Hide();
        }
       


    }

}