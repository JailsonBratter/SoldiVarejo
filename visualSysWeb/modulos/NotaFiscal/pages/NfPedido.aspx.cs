using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NfPedido :visualSysWeb.code.PagePadrao     
{ 
                  static DataTable tb;
                  static String ultimaOrdem = "";
                  static String sqlGrid = "select ltrim(rtrim(Codigo)) as Codigo, cliente_fornecedor,isnull(Nome_cliente,fornecedor.razao_social) cliente,convert(varchar,data,103) as Data,convert(varchar,emissao,103) as Emissao,total, Status from nf  left join cliente on nf.cliente_fornecedor = cliente.codigo_cliente left join Fornecedor on nf.cliente_fornecedor = fornecedor.fornecedor  INNER JOIN Natureza_operacao ON NF.Codigo_operacao= Natureza_operacao.Codigo_operacao where natureza_operacao.filial='MATRIZ' and TIPO_NF =3 ";
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       pesquisar("");
                      }
                      pesquisar(pnBtn);
                      formataCampos();
                  }



                  private void formataCampos()
                  {
                      txtCodigo.Attributes.Add("OnKeyPress", "javascript:return numeros(this,event);");
                      txtDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                      txtAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                  }


                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/NotaFiscal/pages/NfPedidoDetalhes.aspx?novo=true"); 
                  }


                  protected void pesquisar(String ordem)
                  {
                      try
                      {

                          User usr = (User)Session["User"];
                          String strOrdem = "";
                          String strSqltotalFiltro = "select count(*) from nf  left join cliente on nf.cliente_fornecedor = cliente.codigo_cliente left join Fornecedor on nf.cliente_fornecedor = fornecedor.fornecedor  INNER JOIN Natureza_operacao ON NF.Codigo_operacao= Natureza_operacao.Codigo_operacao where natureza_operacao.filial='MATRIZ' and TIPO_NF =3  and nf.filial='" + usr.getFilial() + "'";
                          String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                          if (ordem.Equals(""))
                          {
                              strOrdem = " order by convert(varchar,data,102) desc, convert(int,Codigo) ";
                          }
                          else
                          {
                              if (ordem.Equals("Codigo"))
                              {
                                  strOrdem = " order by convert(int,Codigo) ";
                              }
                              else if (ordem.Equals("Data"))
                              {
                                  strOrdem = " order by convert(varchar,data,102) ";
                              }
                              else if (ordem.Equals("Emissao"))
                              {
                                  strOrdem = " order by convert(varchar,emissao,102) ";
                              }
                              else
                              {
                                  strOrdem = " order By " + ordem;
                              }
                          }

                          if (ordem.Equals(ultimaOrdem))
                          {
                              strOrdem += " Desc ";
                              ultimaOrdem = "";
                          }
                          else
                          {
                              ultimaOrdem = ordem;
                          }

                          String sql = "";
                          if (!txtCodigo.Text.Equals("")) 
                          {
                              sql = " and Codigo like '" + txtCodigo.Text + "%'"; 
                          }
                          if (!txtCliente.Text.Equals("")) 
                          {

                              sql += " and ((cliente_Fornecedor like'" + txtCliente.Text + "%' )or(Nome_cliente like'" + txtCliente.Text + "%' )or(fornecedor.razao_social like'" + txtCliente.Text + "%' ))";
                          }
                          if (!DllTipoPesquisa.SelectedValue.Equals(""))
                          {
                              if (!txtDe.Text.Equals(""))
                              {
                                 
                                  sql += " and " +DllTipoPesquisa.SelectedValue + " between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                              }


                          }

                          //if (!ddlStatus.SelectedValue.Equals(""))
                          //{
                          //    sql += " and status = '" + ddlStatus.SelectedValue + "'";
                          //}

                          if (!sql.Equals(""))
                          {
                              tb = Conexao.GetTable(sqlGrid + sql + strOrdem, usr, true);
                              strSqltotalFiltro += sql;
                          }
                          else
                          {
                              tb = Conexao.GetTable(sqlGrid + strOrdem, usr, true);
                          }

                          String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                          lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
                          gridPesquisa.DataSource = tb;
                          gridPesquisa.DataBind();
                          lblPesquisaErro.Text = "";
                          
                      }
                      catch (FormatException)
                      {
                          lblPesquisaErro.Text = "Digite uma Data Valida";
                      }

                      catch (Exception err)
                      {
                          lblPesquisaErro.Text = err.Message;
                      }
                  }
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      pesquisar(ultimaOrdem);
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

                 protected void imgCliente_Click(object sender, ImageClickEventArgs e)
                 {
                     Session.Remove("campoLista");
                     Session.Add("campoLista", "txtCliente");
                     carregaLista();
                                              
                 }
                 //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                 protected void carregaLista()
                 {
                     
                        
                        String campo = (String) Session["campoLista"];
                        String sqlLista = "";
                        switch (Ddlcli_fornec.SelectedValue)
                        {
              
                            case "CLIENTE":
                                lbltituloLista.Text = "Escolha um Cliente";
                                sqlLista = "select codigo_cliente Codigo,Nome_cliente Cliente  from cliente where CNPJ like '%" + TxtPesquisaLista.Text + "%' or codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%'";

                                break;
                            case "FORNECEDOR":
                                lbltituloLista.Text = "Escolha um Fornecedor";
                                sqlLista = "SELECT FORNECEDOR,[RAZAO SOCIAL]=Razao_social FROM Fornecedor where FORNECEDOR like '%" + TxtPesquisaLista.Text + "%' or RAZAO_SOCIAL like '%" + TxtPesquisaLista.Text + "%' ";

                                break;

               

                        }
                        User usr = (User)Session["User"];
                        GridLista.DataSource = Conexao.GetTable(sqlLista, usr,true);
                        GridLista.DataBind();

                        modalLista.Show();
                    
                 }

                 protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
                 {
                     txtCliente.Text =ListaSelecionada() ;
                     Session.Remove("campoLista");
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
                 protected String ListaSelecionada()
                 {
                     foreach (GridViewRow item in GridLista.Rows)
                     {
                         RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                         if (rdo != null)
                         {
                             if (rdo.Checked)
                             {
                                 return item.Cells[1].Text;
                             }
                         }
                     }

                     return "";
                 }
                 protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
                 {

                 }

                 protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
                 {
                     modalLista.Hide();
                     Session.Remove("campoLista");
                    
                 }
                 protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
                 {
                     carregaLista();
                 }

                 protected void TxtPesquisaLista_TextChanged(object sender, EventArgs e)
                 {
                     carregaLista();
                 }

                 protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
                 {
                     pesquisar(e.SortExpression);
                 }

                 protected void txtCodigo_TextChanged(object sender, EventArgs e)
                 {
                     pesquisar(ultimaOrdem);
                 }
                 protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
                 {
                     int index = Convert.ToInt32(e.CommandArgument);


                     if (e.CommandName.Equals("Danfe"))
                     {
                         User usr = (User)Session["User"];
                         HyperLink meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[0].Controls[0];
                         String numero = meuLink.Text;

                         meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[1].Controls[0];
                         String clienteFornecedor = meuLink.Text;

                         nfDAO nf = new nfDAO(numero, "3", clienteFornecedor, usr);

                         RedirectNovaAba("~/modulos/notafiscal/pages/DanfeReport.aspx?cliente_Fornecedor=" + nf.Cliente_Fornecedor + "&" +
                                                                                    "numero=" + nf.Codigo+ "&" +
                                                                                    "tipoNf="+nf.Tipo_NF+" &" +
                                                                                    "tipoOrigem=N");
                     }
                     else
                     {
                         //HyperLink meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[0].Controls[0];
                         //String numero = meuLink.Text;
                         //HyperLink situacaoLink = (HyperLink)gridPesquisa.Rows[index].Cells[6].Controls[0];
                         //String situacao = situacaoLink.Text;

                         //meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[1].Controls[0];
                         //String clienteFornecedor = meuLink.Text;

                         //if (!situacao.Equals("AUTORIZADO"))
                         //{
                         //    lblPesquisaErro.Text = "Não é permitido o envio de e-mail para notas que não foram AUTORIZADAS!";
                         //    lblPesquisaErro.ForeColor = System.Drawing.Color.Red;
                         //}
                         //else
                         //{
                         //    meuLink = (HyperLink)gridPesquisa.Rows[index].Cells[2].Controls[0];
                         //    String nomeFornecedor = meuLink.Text;

                         //    lblCodigoNota.Text = numero;
                         //    lblDestinatario.Text = clienteFornecedor;
                         //    lblNomeDestinatario.Text = nomeFornecedor;
                         //    modalConfirmaEnvioEmail.Show();

                         //}
                     }
                 }

                 protected void imgBtnConfirmaEnvioEmail_Click(object sender, ImageClickEventArgs e)
                 {


                     RedirectNovaAba("~/modulos/notafiscal/pages/DanfeReport.aspx?cliente_Fornecedor=" + lblDestinatario.Text + "&" +
                                                                           "numero=" + lblCodigoNota.Text + "&" +
                                                                           "tipoNf=1 &" +
                                                                           "tipoOrigem=N&" +
                                                                           "soEmail=true"
                                                                           );
                     modalConfirmaEnvioEmail.Hide();
                 }

                 protected void imgBtnCancelaEnvioEmail_Click(object sender, ImageClickEventArgs e)
                 {
                     modalConfirmaEnvioEmail.Hide();
                 }

    }
}