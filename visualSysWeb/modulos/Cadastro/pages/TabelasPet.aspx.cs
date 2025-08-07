using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class TabelasPet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregarGrids();

                carregarRaca();

                carregarPelagem();

            }
        }


        private void carregarGrids()
        {
            String sqlEspecie = "Select Especie from Especie ";
            gridEspecie.DataSource = Conexao.GetTable(sqlEspecie, null, false);
            gridEspecie.DataBind();
            if (gridEspecie.Rows.Count> 0 && ((RadioButton)gridEspecie.Rows[0].FindControl("RdoEspecie")) != null)
                ((RadioButton)gridEspecie.Rows[0].FindControl("RdoEspecie")).Checked = true;





        }

        private string EspecieSelecionada()
        {
            foreach (GridViewRow row in gridEspecie.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("RdoEspecie");
                if (rdo.Checked)
                    return row.Cells[1].Text;
            }
            return "";
        }

        private void carregarPelagem()
        {
            String sqlPelagem = "Select * from Pelagem";
            GridPelagem.DataSource = Conexao.GetTable(sqlPelagem, null, false);
            GridPelagem.DataBind();
            if (GridPelagem.Rows.Count>0 && ((RadioButton)GridPelagem.Rows[0].FindControl("RdoPelagem")) != null)
                ((RadioButton)GridPelagem.Rows[0].FindControl("RdoPelagem")).Checked = true;
        }
        private void carregarRaca()
        {

            String sqlRaca = "Select * from Raca  where Especie= '" + EspecieSelecionada() + "'";
            gridRaca.DataSource = Conexao.GetTable(sqlRaca, null, false);
            gridRaca.DataBind();
            if (gridRaca.Rows.Count>0 && ((RadioButton)gridRaca.Rows[0].FindControl("RdoRaca")) != null)
                ((RadioButton)gridRaca.Rows[0].FindControl("RdoRaca")).Checked = true;
        }

        protected void ImgBtnAddEspecie_Click(object sender, ImageClickEventArgs e)
        {
            modalEspecieDetalhe.Show();
            txtEspecie.Text = "";

        }

        protected void ImgBtnExcluirEspecie_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirEspecie.Show();
        }

        protected void ImgBtnAddRaca_Click(object sender, ImageClickEventArgs e)
        {
            modalRaca.Show();
            txtRaca.Text = "";
            txtRaca.Enabled = true;
            txtRacaEspecie.Text = EspecieSelecionada();
            txtRacaEspecie.Enabled = false;
            txtFrequenciaCio.Text = "";
            txtDuracaoCio.Text = "";
        }

        protected void ImgBtnExcluirRaca_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirRaca.Show();
        }

        protected void ImgBtnEditarRaca_Click(object sender, ImageClickEventArgs e)
        {
            txtRaca.Text = RacaSelecionada(1);
            txtRaca.Enabled = false;
            txtRacaEspecie.Text = EspecieSelecionada();
            txtRacaEspecie.Enabled = false;
            txtDuracaoCio.Text = RacaSelecionada(3);
            txtFrequenciaCio.Text = RacaSelecionada(4);
            modalRaca.Show();
        }

        private string RacaSelecionada(int v)
        {
            foreach (GridViewRow row in gridRaca.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("RdoRaca");
                if (rdo.Checked)
                    return row.Cells[v].Text;
            }
            return "";
        }

        protected void ImgBtnPelagem_Click(object sender, ImageClickEventArgs e)
        {
            modalPelagemDetalhes.Show();
            txtPelagem.Text = "";
        }

        protected void ImgBtnExcluirPelagem_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirPelagem.Show();
        }

        protected void RdoEspecie_CheckedChanged(object sender, EventArgs e)
        {
            carregarRaca();
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoEspecie");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridEspecie.*GrEspecie',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void RdoRaca_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void GridPelagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoPelagem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridPelagem.*Grpelagem',this)";
            rdo.Attributes.Add("onclick", script);

        }

        protected void gridRaca_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoRaca");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('gridRaca.*GrRaca',this)";
            rdo.Attributes.Add("onclick", script);


        }

        protected void RdoPelagem_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnConfirmaDetalhesEspecie_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (txtEspecie.Text.Equals(""))
                {
                    lblErroEspecieDetalhes.Text = "Digite Uma Especie ";
                    modalEspecieDetalhe.Show();
                }
                else
                {
                    String sql = "Insert into Especie values ('" + txtEspecie.Text + "');";
                    Conexao.executarSql(sql);
                    carregarGrids();

                }
            }
            catch (Exception err)
            {
                showMsg(err.Message, true);
            }


        }

        protected void btnCancelaDetalhesEspecie_Click(object sender, ImageClickEventArgs e)
        {
            modalEspecieDetalhe.Hide();
        }

        protected void ImgBtnConfirmaDetalhesRaca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                String sql = "";
                if (txtRaca.Enabled)
                {
                    sql = "Insert into Raca (Raca,Especie,frequencia_cio,Duracao_cio) values(" +
                        "'" + txtRaca.Text + "'" +
                        ",'" + txtRacaEspecie.Text + "'" +
                        "," + Funcoes.decimalPonto(Funcoes.intTry(txtFrequenciaCio.Text).ToString()) +
                        "," + Funcoes.decimalPonto(Funcoes.intTry(txtDuracaoCio.Text).ToString()) + ");";
                }
                else
                {
                    sql = "update Raca set " +
                        "frequencia_cio =" + Funcoes.decimalPonto(Funcoes.intTry(txtFrequenciaCio.Text).ToString()) +
                        ",Duracao_cio=" + Funcoes.decimalPonto(Funcoes.intTry(txtDuracaoCio.Text).ToString()) +
                        " where Raca='" + txtRaca.Text + "' and Especie='" + txtRacaEspecie.Text + "'";
                }

                Conexao.executarSql(sql);
                carregarRaca();
            }
            catch (Exception err)
            {
                showMsg(err.Message, true);
            }

        }

        protected void ImgBtnCancelaDetalhesRaca_Click(object sender, ImageClickEventArgs e)
        {
            modalRaca.Hide();
        }

        protected void ImgBtnConfirmaDetalhesPelagem_Click(object sender, ImageClickEventArgs e)
        {
            if (txtPelagem.Text.Equals(""))
            {
                lblErroPelagem.Text = "Digite Uma Pelagem";
                modalPelagemDetalhes.Show();
            }
            else
            {
                try
                {
                    String sql = "Insert into Pelagem values ('" + txtPelagem.Text + "');";
                    Conexao.executarSql(sql);
                    carregarPelagem();
                }
                catch (Exception err)
                {
                    showMsg(err.Message, true);
                }
            }
        }

        protected void ImgBtnCancelarDetalhesPelagem_Click(object sender, ImageClickEventArgs e)
        {
            modalPelagemDetalhes.Hide();
        }

        protected void ImgBtnConfirmaExcluirPelagem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "Delete from pelagem where pelagem ='" + pelagemSeleciona() + "'";
                Conexao.executarSql(sql);
                carregarPelagem();

            }
            catch (Exception err)
            {

                showMsg(err.Message, true);
            }
        }

        private string pelagemSeleciona()
        {
            foreach (GridViewRow row in GridPelagem.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("RdoPelagem");
                if (rdo.Checked)
                    return row.Cells[1].Text;
            }
            return "";
        }

        protected void ImgBtnCancelarExcluirPelagem_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirPelagem.Hide();
        }

        protected void ImgBtnConfirmaExcluirRaca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "Delete from raca where raca ='" + RacaSelecionada(1) + "' and especie='" + EspecieSelecionada() + "'";
                Conexao.executarSql(sql);
                carregarRaca();
            }
            catch (Exception err)
            {

                showMsg(err.Message, true);
            }
        }

        protected void ImgBtnCancelarExcluirRaca_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirRaca.Hide();
        }

        protected void ImgBtnConfirmaExcluirEspecie_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "Delete from especie where Especie ='" + EspecieSelecionada() + "'";
                Conexao.executarSql(sql);
                carregarGrids();
                carregarRaca();
            }
            catch (Exception err)
            {

                showMsg(err.Message, true);
            }

        }

        protected void ImgBtnCancelarExcluirEspecie_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirEspecie.Hide();
        }
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void showMsg(String msg, bool err)
        {
            lblErroPanel.Text = msg;
            if (err)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
        }
    }
}