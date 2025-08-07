using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class CadernetaPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (Request["codCliente"] != null)
                {
                    User usr = (User)Session["User"];
                    String codigo_cliente = Request["codCliente"];
                    String de = Request["de"];
                    String ate = Request["ate"];
                    String tipo = Request["tipo"];
                    lblDe.Text = de;
                    lblAte.Text = ate;
                    lblTipo.Text =(tipo == null ? "TODOS" : tipo);
                    lblFantasia.Text = usr.filial.Fantasia;
                    String Sql = "Select Emissao =CONVERT(varchar,emissao_caderneta,103),Tipo,Documento=Documento_Caderneta,valor=Total_Caderneta from Caderneta where codigo_cliente ='" + codigo_cliente.Trim() + "'";
                    String sqlWhere = "";
                    if (tipo != null)
                    {
                        sqlWhere += " and tipo ='" + tipo + "' ";
                    }
                    if (de != null && ate != null)
                    {
                        sqlWhere += " and convert(dateTime ,convert(varchar,emissao_caderneta,102)) between '" + DateTime.Parse(de).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(ate).ToString("yyyy-MM-dd") + "'";
                    }
                    gridItens.DataSource = Conexao.GetTable(Sql + sqlWhere + " order by CONVERT(varchar,emissao_caderneta,102) desc", usr, false);
                    gridItens.DataBind();
                    ClienteDAO cliente = new ClienteDAO(codigo_cliente, usr);
                    lblCliente.Text = cliente.Codigo_Cliente + "-" + cliente.Nome_Cliente;
                    lblSaldoDevedor.Text = cliente.Utilizado.ToString("N2");

                    Decimal total = 0;

                    foreach (GridViewRow item in gridItens.Rows)
                    {
                        if (item.Cells[0].Text.Trim().ToString().ToUpper().Equals("DEBITO"))
                            total += Decimal.Parse(item.Cells[3].Text);
                        else
                            total -= Decimal.Parse(item.Cells[3].Text);
                    }
                    lblTotal.Text = total.ToString("N2");
                }
            }
            catch (Exception)
            {

                form1.Controls.Clear();
                form1.InnerHtml = "Erro ao montar a impressão!!";
            }
        }
    }
}