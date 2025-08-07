using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using visualSysWeb.dao;
using System.Data;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Finalizadoras :visualSysWeb.code.PagePadrao     //inicio da classe 
{           
                static DataTable tb;
                static String sqlGrid = "select Nro_Finalizadora,Codigo_centro_custo,Finalizadora from finalizadora ";//colocar os campos no select que ser?o apresentados na tela
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
                      txtPESQ1.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
                  }
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/Cadastro/pages/FinalizadoraDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes
                  }
                  private void pesquisar()
                  {
                      String sql = "";
                      String strSqltotalFiltro = "select count(*) from finalizadora ";
                      String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " Nro_Finalizadora like'%" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }

                      if (!txtPESQ2.Text.Equals(""))
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";
                          }
                          sql += " Finalizadora Like '%" + txtPESQ2.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                      try
                      {
                          User usr = (User)Session["User"];
                          
                          if (!sql.Equals(""))
                          {
                              tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                              strSqltotalFiltro += " where " + sql;
                          }
                          else
                          {
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