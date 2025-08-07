using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Cartao :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = "select * from cartao";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {
                         Pesquisa();
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/Cadastro/pages/cartaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      Pesquisa();
                  }

                  private void Pesquisa()
                  {
                      String sql = "";
                      if (!txtIdCartao.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " id_cartao = '" + txtIdCartao.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtIdFinalizadora.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";
                          }
                          sql += "id_finalizadora = '" + txtIdFinalizadora.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                      if (!txtBandeira.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";
                          }
                          sql += "bandeira= '" + txtIdFinalizadora.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                      try
                      {
                          String totalRegistro = Conexao.retornaUmValor("select count(*) from cartao ", null);
                          String strSqltotalFiltro = "Select count(*) from cartao ";
                          User usr = (User)Session["User"];
                          if (!sql.Equals(""))
                          {
                              strSqltotalFiltro += " where " + sql;
                              tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                          }
                          else
                          {
                              tb = Conexao.GetTable(sqlGrid, usr, false);
                          }
                          gridPesquisa.DataSource = tb;
                          gridPesquisa.DataBind();

                          String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                          lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";

                          lblPesquisaErro.Text = "";
                      }
                      catch (Exception err)
                      {
                          lblPesquisaErro.Text = err.Message;
                      }
                  }

                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                 protected override bool campoObrigatorio(Control campo)
                 { 
                       return false;
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {
                       return false;
                 }
    }
}