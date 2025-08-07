using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Relatorios.pages.RelatoriosXml.venda.page
{
    public partial class VendasHoraMedia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (!IsPostBack)
            {
                if (usr != null)
                {

                    carregarRelatorio();
                }
            }
        }

        private void carregarRelatorio()
        {
            User usr = (User)Session["User"];
            String filial = usr.getFilial();
            String dtDe = Request.Params["DataDe"].ToString().Trim();
            String dtAte = Request.Params["DataAte"].ToString().Trim();
            String ini_periodo = Request.Params["ini_periodo"].ToString().Trim();
            String fim_periodo = Request.Params["fim_periodo"].ToString().Trim();
            String plu = Request.Params["plu"];
            String descricao = Request.Params["descricao"];
            String grupo = Request.Params["grupo"];
            String subgrupo = Request.Params["subgrupo"];
            String departamento = Request.Params["departamento"];
            String relatorio = Request.Params["relatorio"];
            String pdv = Request.Params["pdv"];
            String diasSemana = Request.Params["diasSemana"];

            if (plu == null)
                plu = "";

            if (descricao == null)
                descricao = "";

            if(grupo ==null)
                grupo = "";

            if(subgrupo == null)
                subgrupo ="";
            if (departamento == null)
                departamento = "";

            if (relatorio == null)
                relatorio = "";

            if (pdv == null)
                pdv = "TODOS";

            if (diasSemana == null || diasSemana.Equals(""))
                diasSemana = "TODOS";

            Decimal vTotVendaTT = 0;
            Decimal vTotVendaMD = 0;
            Decimal vTotQtdeTT = 0;
            Decimal vTotQtdeMD = 0;
            Decimal vTotClientesTT = 0;
            Decimal vTotClientesMD = 0;
            Decimal vTotTicketMd = 0;

            
            lblFiltros.Text = "Filial:" + filial;

           
            DateTime dataDe = new DateTime();
            DateTime dataAte = new DateTime();

            DateTime.TryParse(dtDe, out dataDe);
            DateTime.TryParse(dtAte, out dataAte);
            int qtdeDias = ((dataAte - dataDe).Days)+1;

          


            lblFiltros.Text += " De : " + dataDe.ToString("dd/MM/yyyy") + " ate " + dataAte.ToString("dd/MM/yyyy") +
                               " total de "+ qtdeDias.ToString()+ " Dias"+
                               " Periodo: "+ini_periodo+" as "+ fim_periodo;
            if (fim_periodo.Substring(3, 2).Equals("00"))
                fim_periodo = fim_periodo.Substring(0, 3) + "59";

            if (plu.Length > 0)
                lblFiltros.Text += "| PLU:" + plu;

            if(descricao.Length>0)
                    lblFiltros.Text += "| Descrição:" + descricao;

            if (grupo.Length > 0)
                lblFiltros.Text += "| Grupo:" + grupo;

            if (subgrupo.Length > 0)
                lblFiltros.Text += "| SubGrupo:" + subgrupo;

            if (departamento.Length > 0)
                lblFiltros.Text += "| Departamento:" + departamento;

            if (relatorio.Length > 0)
                lblFiltros.Text += "| Relatorio:" + relatorio;

            if (relatorio.Length > 0)
                lblFiltros.Text += "Dias Semana:" + diasSemana;


            if (pdv.Length > 0)
                lblFiltros.Text += "|PDV:" + pdv;


            imgLog.ImageUrl = "~/img/logo.jpg";

            String styleLinha = "style=\"border-right:solid 1px  black;\"";



            String sql = " EXEC sp_Rel_Resumo_Vendas_hora_media " +
                                "'" + filial + "'" +
                                ",'" + dataDe.ToString("yyyyMMdd") + "'" +
                                ",'" + dataAte.ToString("yyyyMMdd") + "'" +
                                ",'" + ini_periodo + "'" +
                                ",'" + fim_periodo + "'" +
                                ",'" + plu + "'" +
                                ",'" + descricao + "'" +
                                ",'" + grupo + "'" +
                                ",'" + subgrupo + "'" +
                                ",'" + departamento + "'" +
                                ",'" + relatorio + "'" +
                                ",'" + pdv + "'" +
                                ",'" + diasSemana + "'";

           


            String htmlTable = "<table style = \" float:left; color:Black;border-color:Black;border-width:1px;border-style:Solid;width:100%;border-collapse:collapse;\" >" +
                                    "<tr style=\"color:White;background-color:#5D7B9D;font-weight:bold;\" >" +
                                       "<th " + styleLinha + ">Hora</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Venda TT</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Venda MD</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Qtde TT</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Qtde MD</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Clientes TT</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Clientes MD</th>" +
                                       "<th align=\"right\" " + styleLinha + ">Ticket MD</th>" +
                                   "</tr>";
            ArrayList table = new ArrayList();
            SqlDataReader rs = null;

            try
            {
                bool linha = true;
                rs = Conexao.consulta(sql, null, false);

                while (rs.Read())
                {

                    ArrayList row = new ArrayList();

                    Decimal VendaTT = Funcoes.decTry(rs["Venda TT"].ToString());
                    Decimal VendaMD = Funcoes.decTry(rs["Venda MD"].ToString());
                    Decimal QtdeTT = Funcoes.decTry(rs["Qtde TT"].ToString());
                    Decimal QtdeMD = Funcoes.decTry(rs["Qtde MD"].ToString());
                    Decimal ClientesTT = Funcoes.decTry(rs["Clientes TT"].ToString());
                    Decimal ClientesMD = Funcoes.decTry(rs["Clientes MD"].ToString());

                    Decimal ticketMd = 0;
                    if(VendaTT >0 &&  ClientesTT>0)
                        ticketMd =(VendaTT / ClientesTT);

                    row.Add(rs["hora"].ToString());
                    row.Add(VendaMD.ToString());
                    row.Add(QtdeMD.ToString());
                    row.Add(ClientesMD.ToString());

                    table.Add(row);
                    String strcolor = "";
                    if (linha)
                    {
                        strcolor = "style=\"color:#333333;background-color:#F7F6F3;\"";
                    }
                    else
                    {
                        strcolor = "style=\"color:Black;background-color:LightGrey;\"";
                    }
                    linha = !linha;



                    htmlTable += "<tr " + strcolor + ">" +
                                            "<td " + styleLinha + " > "+rs["hora"].ToString()+"</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + VendaTT.ToString("N2") + "</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + VendaMD.ToString("N2") + "</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + QtdeTT.ToString("N2") + "</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + QtdeMD.ToString("N2") + "</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + ClientesTT.ToString("N0") + " </td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + ClientesMD.ToString("N0") + "</td>" +
                                            "<td align=\"right\" " + styleLinha + ">" + ticketMd.ToString("N2") + "</td>" +

                                 "</tr>";

                    vTotVendaTT += VendaTT;
                    vTotQtdeTT += QtdeTT;
                    vTotClientesTT += ClientesTT;
                   


                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;

            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }


            vTotVendaMD = (vTotVendaTT / qtdeDias);
            vTotQtdeMD = (vTotQtdeTT / qtdeDias);
            vTotClientesMD = (vTotClientesTT / qtdeDias);
            if (vTotVendaTT > 0 && vTotClientesTT > 0)
                vTotTicketMd = (vTotVendaTT / vTotClientesTT);



            styleLinha = "style=\"border:solid 1px  black;\"";
            htmlTable += "<tr>" +
                          "<td  style =\"font-weight: bold;font-size:20px; border-top:solid 1px black; border-bottom: solid 1px  black;\" >TOTAL</td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotVendaTT.ToString("N2") + " </b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotVendaMD.ToString("N2") + " </b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotQtdeTT.ToString("N2") + "</b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotQtdeMD.ToString("N2") + "</b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotClientesTT.ToString("N2") + "</b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotClientesMD.ToString("N2") + "</b></td>" +
                          "<td align=\"right\" " + styleLinha + "><b>" + vTotTicketMd.ToString("N2") + "</b></td>" +
                          
                  "</tr>";

            htmlTable += "</table>";


            atualizarGrafico(table);



            divRelatorio.InnerHtml = htmlTable;
        }

        protected void ImgBtnVoltar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/modulos/Relatorios/pages/Relatorios.aspx?relatorio=venda&tela=R002&rel=23");
        }
        protected void btnVisualizar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Request.RawUrl);

        }

        private void atualizarGrafico(ArrayList table)
        {
            StringBuilder opt = new StringBuilder("");
            StringBuilder vlrs = new StringBuilder("");
            StringBuilder vlrs2 = new StringBuilder("");
            StringBuilder vlrs3 = new StringBuilder("");

            opt.Append("[");
            vlrs.Append("[[");
            vlrs2.Append("[");
            vlrs3.Append("[");


            for (int i = 0; i < table.Count; i++)
            {
                ArrayList rw = (ArrayList) table[i];
                opt.Append("'" + rw[0].ToString().Substring(0,2) + "',");
                vlrs.Append("" + Funcoes.decimalPonto(rw[1].ToString()) + ",");
                vlrs2.Append("" + Funcoes.decimalPonto(rw[2].ToString()) + ",");
                vlrs3.Append("" + Funcoes.decimalPonto(rw[3].ToString()) + ",");

            }

            opt.Append("]");
            vlrs2.Append("]");
            vlrs3.Append("]]");
            vlrs.Append("],"+vlrs2.ToString()+","+vlrs3.ToString());
            




            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Grafico", "carregarGraficos('#grafico1'," + opt.ToString() + "," + vlrs.ToString() + ");", true);

            
        }
    }
}