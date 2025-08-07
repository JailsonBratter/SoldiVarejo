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
    public partial class Linhas  :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = "select * from linha";
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {
                         pesquisar();
                      }
                      pesquisar(pnBtn);
                      camposnumericos();
                  }

                  private void camposnumericos()
                  {
                      txtCodigo.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
                  }
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/cadastro/pages/linhasDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  private void pesquisar()
                  {
                      String sql = "";
                      String strSqltotalFiltro = "select count(*) from linha ";
                      String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);
                      if (!txtCodigo.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " codigo_linha like '%" + txtCodigo.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtLinha.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";
                          }
                          sql += "descricao_linha = '" + txtLinha.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                      try
                      {
                          User usr = (User)Session["User"];
                          if (!sql.Equals(""))
                          {
                              tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                              strSqltotalFiltro += " Where " + sql;
                          }
                          else
                          {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                              tb = Conexao.GetTable(sqlGrid, usr, true);
                          }
                          gridPesquisa.DataSource = tb;
                          gridPesquisa.DataBind();
                          lblPesquisaErro.Text = "";
                          String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                          lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
                      }
                      catch (Exception err)
                      {
                          lblPesquisaErro.Text = err.Message;
                      }
                  }
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      pesquisar();
                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                  protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
                  {
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.PageIndex = e.NewPageIndex;
                    Lblindex.Text = (e.NewPageIndex+1)+"/" + gridPesquisa.PageCount;
                    gridPesquisa.DataBind();
                  }
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