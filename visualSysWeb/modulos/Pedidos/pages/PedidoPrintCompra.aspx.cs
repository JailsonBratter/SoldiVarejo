using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.IO;
using System.Text;
using visualSysWeb.code;
using System.Collections;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoPrintCompra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                User usr = (User)Session["User"];
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : "\\\\" + usr.filial.ip);
                pedidoDAO obj = (pedidoDAO)Session["PedidoPrintCompra"];
                bool imp40 = Funcoes.valorParametro("PEDIDO_IMPRIMIR_40", usr).ToUpper().Equals("TRUE");
                String tipoRelatorio = Request.Params["simples"];
                if (tipoRelatorio != null)
                {
                    if (Funcoes.existePasta(endereco + "\\imprimePedido"))
                    {

                        ArrayList arr = (ArrayList)Session["titulosImprimir"];

                        if (arr != null)
                        {
                            StreamWriter ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido9999.txt", false, Encoding.ASCII);
                            ArqImprime.Write(GerarArquivo(arr));
                            ArqImprime.Close();
                            Session.Remove("titulosImprimir");
                        }
                        else if (obj != null)
                        {
                            StreamWriter ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido" + obj.Pedido.Trim() + ".txt", false, Encoding.ASCII);

                            if (imp40)
                            {
                                ArqImprime.Write(GerarArquivo40(obj));
                            }
                            else
                            {
                                ArqImprime.Write(GerarArquivo(obj, false));
                            }

                            ArqImprime.Close();

                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.close();", true);

                    }
                    else
                    {
                        divPage.InnerHtml = GerarArquivo(obj, true);
                    }
                }
                else
                {
                    carregarDados();
                }


                // lblObservacoes.Text = endereco + "\\imprimePedido";
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void carregarDados()
        {
            pedidoDAO obj = (pedidoDAO)Session["PedidoPrintCompra"];
            lblPedido.Text = (obj.Pedido == null ? "" : obj.Pedido.ToString());
            if (obj.Tipo == 1)
            {
                lblTituloPedido.Text = "PEDIDO DE VENDA";
            }
            else
            {
                lblTituloPedido.Text = "PEDIDO DE COMPRA";
            }



            switch (obj.Status)
            {
                case 1:
                    lblStatus.Text = "ABERTO";
                    break;
                case 2:
                    lblStatus.Text = "FECHADO";
                    break;
                case 3:
                    lblStatus.Text = "CANCELADO";
                    lblTituloPedido.Text += " CANCELADO!!";
                    break;

            }


            lblCliente_Fornec.Text = obj.Cliente_Fornec.ToString();
            if (obj.Tipo == 1)
                lblNomeClie_Fornec.Text = obj.NomeCliente;
            else
                lblNomeClie_Fornec.Visible = false;

            lblDataCadastro.Text = obj.Data_cadastroBr();
            lblDataEntrega.Text = obj.Data_entregaBr();
            lblHora.Text = obj.hora.ToString();
            lblTabelaDesconto.Text = obj.TabelaPreco;
            lblTotal.Text = string.Format("{0:0,0.00}", obj.Total);
            lblDesconto.Text = string.Format("{0:0,0.00}", obj.Desconto);
            lblUsuario.Text = obj.Usuario.ToString();
            lblObservacoes.Text = obj.Obs.ToString();
            lblNaturezaOperacao.Text = obj.CFOP.ToString();
            lblFuncionario.Text = obj.funcionario.ToString();
            lblPedidoSimplificado.Visible = obj.pedido_simples;
            lblCentroCusto.Text = obj.centro_custo;
            gridItens.DataSource = obj.PdItens();
            gridItens.DataBind();

            gridPagamentos.DataSource = obj.PdPag();
            gridPagamentos.DataBind();
        }


        protected String GerarArquivo(ArrayList titulos)
        {


            StringBuilder strLinhaReg = new StringBuilder();
            User usr = (User)Session["User"];
            bool cabecalho = false;
            Decimal vtotal = 0;
            strLinhaReg.AppendLine("--------------------------------TITULOS EM ABERTO------------------------------");

            foreach (ArrayList linha in titulos)
            {

                String cel1 = linha[0].ToString();
                if (cel1.Equals("CLIENTE"))
                {
                    strLinhaReg.AppendLine("CLIENTE:" + linha[1] + "-" + linha[2]);
                }
                else
                {
                    if (!cabecalho)
                    {
                        strLinhaReg.AppendLine("===============================================================================");
                        strLinhaReg.AppendLine("DOCUMENTO  TIPO            EMISSAO      VENCIMENTO      VALOR        DIAS  ");
                        strLinhaReg.AppendLine("===============================================================================");
                        cabecalho = true;
                    }
                    strLinhaReg.AppendLine(linha[0].ToString().PadRight(11, ' ') +
                                            linha[1].ToString().Replace("&nbsp;", "").PadRight(16, ' ') +
                                            linha[2].ToString().PadRight(13, ' ') +
                                            linha[3].ToString().PadLeft(10, ' ') +
                                            linha[4].ToString().PadLeft(13, ' ') +
                                            linha[5].ToString().PadLeft(10, ' ')
                                            );
                    strLinhaReg.AppendLine("-------------------------------------------------------------------------------");
                    if (Funcoes.isnumero(linha[3].ToString()))
                    {
                        vtotal += Decimal.Parse(linha[3].ToString());
                    }
                }
            }

            strLinhaReg.AppendLine("===============================================================================");
            strLinhaReg.AppendLine("".PadRight(30, ' ') + "TOTAL:         " + vtotal.ToString("N2").PadLeft(15, ' '));


            return strLinhaReg.ToString();
        }

        protected String GerarArquivo(pedidoDAO ped, bool html)
        {

            int intLinhasImpPosPedido = 0;
            String strLinhaReg = "";
            User usr = (User)Session["User"];
            // Variavel para inserir linhas após término da impressão do pedido (apenas arquivo gerado)
            String strLinhaPosPedido = Funcoes.valorParametro("LINHAS_IMP_POS_PEDIDO", usr);

            if (strLinhaPosPedido.Equals(""))
            {
                intLinhasImpPosPedido = 0;
            }
            else
            {
                intLinhasImpPosPedido = int.Parse(strLinhaPosPedido);
            }


            string strSpace = "";
            ClienteDAO Cliente = new ClienteDAO(ped.Cliente_Fornec, usr);


            if (!html)
            {

                strLinhaReg = "Orcamento: " + ped.Pedido.ToString().Trim().PadLeft(10, '0');
                strLinhaReg = strLinhaReg + (ped.pedido_simples ? "***" : "") + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Data: " + ped.Data_cadastro.ToString("dd/MM/yyyy") + " ";
                strLinhaReg = strLinhaReg + "Atendente: " + ped.funcionario.ToString() + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Cliente:" + Cliente.Codigo_Cliente + "-" + Cliente.Nome_Cliente + " Telefone: " + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Ende: " + Cliente.Endereco + " " + Cliente.endereco_nro.ToString() + " - " + Cliente.complemento_end.ToString().Trim() + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Bairro: " + Cliente.Bairro + " Cidade:" + Cliente.Cidade + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "CEP: " + Cliente.CEP + " UF: " + Cliente.UF + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "CNPJ/CPF: " + Cliente.CNPJ.Trim() + " Insc.Estadual: " + Cliente.IE + (char)13 + (char)10;
                //Incluir o telefone
                if (Cliente.meiosComunicacao.Rows.Count > 0)
                {
                    strLinhaReg = strLinhaReg + "Meios de Contato:" + (char)13 + (char)10;
                    for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                    {
                        strLinhaReg = strLinhaReg + "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ";
                        strLinhaReg = strLinhaReg + "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1] + (char)13 + (char)10;
                    }
                }

                strLinhaReg = strLinhaReg + "===============================================================================" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Qtde  Uni Cod            Descricao                            Unit  Total Item" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "===============================================================================" + (char)13 + (char)10;
                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    if (!item.excluido)
                    {
                        strLinhaReg = strLinhaReg + item.Qtde.ToString("N2").PadLeft(5, ' ') + " ";
                        strLinhaReg = strLinhaReg + Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null);
                        strLinhaReg = strLinhaReg + item.CodReferencia.PadRight(7, ' ') + "-";
                        strLinhaReg = strLinhaReg + item.PLU.ToString().PadLeft(6, '0') + " ";
                        if (item.Descricao.ToString().Length > 35)
                        {
                            strLinhaReg = strLinhaReg + Funcoes.RemoverAcentos(item.Descricao).Substring(0, 35).PadRight(35, ' ');
                        }
                        else
                        {
                            strLinhaReg = strLinhaReg + Funcoes.RemoverAcentos(item.Descricao).PadRight(35, ' ');
                        }
                        strLinhaReg = strLinhaReg + (item.total / item.Qtde).ToString("N2").PadLeft(8, ' ');
                        //strLinhaReg = strLinhaReg + item.unitario.ToString("N2").PadLeft(12, ' ');
                        strLinhaReg = strLinhaReg + item.total.ToString("N2").PadLeft(10, ' ');
                        strLinhaReg = strLinhaReg + (char)13 + (char)10;
                        strLinhaReg = strLinhaReg + "-------------------------------------------------------------------------------";
                        if (html)
                        {
                            strLinhaReg = strLinhaReg + "-------------------------------------------------";
                        }

                        strLinhaReg = strLinhaReg + (char)13 + (char)10;
                    }
                }
                strLinhaReg = strLinhaReg + strSpace.PadLeft(50, ' ') + "Vlr do Pedido:" + ped.Total.ToString("N2").PadLeft(10, ' ') + (char)13 + (char)10; ;

                if (ped.PdPag().Rows.Count > 0)
                {
                    strLinhaReg = strLinhaReg + "FORMA DE PAGAMENTO" + (char)13 + (char)10;
                    strLinhaReg = strLinhaReg + (char)13 + (char)10;
                    for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                    {
                        strLinhaReg = strLinhaReg + "Tipo: " + ped.PdPag().Rows[i].ItemArray[5] + " ";
                        strLinhaReg = strLinhaReg + "Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + " " + (char)13 + (char)10;

                        //strLinhaReg = strLinhaReg + "Vlr: " + ped.PdPag().Rows[i].ItemArray[6] 
                        strLinhaReg = strLinhaReg + "-------------------------------------------------------------------------------" + (char)13 + (char)10; ;
                    }
                }

                strLinhaReg = strLinhaReg + "Observacoes:" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + ped.Obs + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Imp.:" + DateTime.Now.ToString() + (char)13 + (char)10;

                for (int y = 1; y <= intLinhasImpPosPedido; y++)
                {
                    strLinhaReg = strLinhaReg + (char)13 + (char)10;
                }
                return strLinhaReg;
            }
            else
            {

                strLinhaReg = "Orcamento: " + ped.Pedido.ToString().Trim().PadLeft(10, '0');
                strLinhaReg += (ped.pedido_simples ? "***" : "") + "</br></br>";
                strLinhaReg += "Data: " + ped.Data_cadastro.ToString("dd/MM/yyyy") + " ";
                strLinhaReg += "Atendente: " + ped.funcionario.ToString() + "</br>";
                strLinhaReg += "Cliente:" + Cliente.Codigo_Cliente.Trim() + "-" + Cliente.Nome_Cliente + "<br>";
                strLinhaReg += "Ende: " + Cliente.Endereco + " " + Cliente.endereco_nro.ToString() + " - " + Cliente.complemento_end.ToString().Trim() + "</br>";
                strLinhaReg += "Bairro: " + Cliente.Bairro + " Cidade:" + Cliente.Cidade + "</br>";
                strLinhaReg += "CEP: " + Cliente.CEP + " UF: " + Cliente.UF + "</br>";
                strLinhaReg += "CNPJ/CPF: " + Cliente.CNPJ.Trim() + " Insc.Estadual: " + Cliente.IE + "</br></br>";
                //Incluir o telefone
                if (Cliente.meiosComunicacao.Rows.Count > 0)
                {
                    strLinhaReg += "Meios de Contato:" + "</br>";
                    for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                    {
                        strLinhaReg += "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ";
                        strLinhaReg += "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1] + "</br>";
                    }
                }

                strLinhaReg += " </br></br>";

                strLinhaReg += "<table class=\"tableSimplesPedido\">";
                strLinhaReg += "<tr>";
                strLinhaReg += "<th>Qtde</th>";
                strLinhaReg += "<th>Uni</th>";
                strLinhaReg += "<th>Cod </th>";
                strLinhaReg += "<th>Descricao </th>";
                strLinhaReg += "<th class=\"thNumero\">Unit</th>";
                strLinhaReg += "<th class=\"thNumero\">Total Item</th>";
                strLinhaReg += "</tr>";

                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    if (!item.excluido)
                    {
                        strLinhaReg += "<tr >";

                        strLinhaReg += "<td >" + item.Qtde.ToString("N2") + "</td> ";
                        strLinhaReg += "<td >" + Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null) + "</td> ";
                        strLinhaReg += "<td >" + item.CodReferencia + "-" + item.PLU.ToString() + " " + "</td> ";
                        if (item.Descricao.ToString().Length > 35)
                        {
                            strLinhaReg += "<td >" + Funcoes.RemoverAcentos(item.Descricao).Substring(0, 35) + "</td> ";
                        }
                        else
                        {
                            strLinhaReg += "<td >" + Funcoes.RemoverAcentos(item.Descricao) + "</td> ";
                        }
                        strLinhaReg += "<td class=\"tdNumero\">" + (item.total / item.Qtde).ToString("N2") + "</td> ";
                        //strLinhaReg = strLinhaReg + item.unitario.ToString("N2").PadLeft(12, ' ');
                        strLinhaReg += "<td class=\"tdNumero\">" + item.total.ToString("N2").PadLeft(11, ' ') + "</td> ";

                        strLinhaReg += "</tr>";


                    }
                }
                strLinhaReg += "<tr>";
                strLinhaReg += "<td colspan=\"4\"></td>";
                strLinhaReg += "<td> Vlr do Pedido:</td>";
                strLinhaReg += "<td class=\"tdNumero\"> " + ped.Total.ToString("N2").PadLeft(10, ' ') + "</td>";
                strLinhaReg += "</tr>";

                if (ped.PdPag().Rows.Count > 0)
                {
                    strLinhaReg += "<tr>";

                    strLinhaReg += "<td class=\"semBorda\" colspan=\"6\">FORMA DE PAGAMENTO </td>";

                    strLinhaReg += "</tr>";

                    for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                    {
                        strLinhaReg += "<tr>";
                        strLinhaReg += "<td colspan=\"3\" > Tipo: " + ped.PdPag().Rows[i].ItemArray[5] + "</td> ";
                        strLinhaReg += "<td > Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + "</td> ";

                        //strLinhaReg = strLinhaReg + "Vlr: " + ped.PdPag().Rows[i].ItemArray[6] 
                        strLinhaReg += "</tr>";
                    }


                }
                strLinhaReg += "<tr>";
                strLinhaReg += "<td class=\"semBorda\" colspan=\"6\"> Observacoes:</td>";
                strLinhaReg += "</tr>";
                strLinhaReg += "<tr>";
                strLinhaReg += "<td class=\"semBorda\" colspan=\"6\">";
                strLinhaReg += ped.Obs.Replace("" + (char)10, "</br>") + "</br>";
                strLinhaReg += "Imp.:" + DateTime.Now.ToString();
                strLinhaReg += "</tr>";



                string quebra = ("" + (char)13 + (char)10).ToString();
                return strLinhaReg.Replace(quebra, "</br>").Replace("  ", "&nbsp;&nbsp;");

            }
        }


        protected String GerarArquivo40(pedidoDAO ped)
        {

            int intLinhasImpPosPedido = 0;
            StringBuilder strLinhaReg = new StringBuilder();
            User usr = (User)Session["User"];
            // Variavel para inserir linhas após término da impressão do pedido (apenas arquivo gerado)
            String strLinhaPosPedido = Funcoes.valorParametro("LINHAS_IMP_POS_PEDIDO", usr);

            if (strLinhaPosPedido.Equals(""))
            {
                intLinhasImpPosPedido = 0;
            }
            else
            {
                intLinhasImpPosPedido = int.Parse(strLinhaPosPedido);
            }


            string strSpace = "";
            ClienteDAO Cliente = new ClienteDAO(ped.Cliente_Fornec, usr);

            strLinhaReg.AppendLine((char)15+"------------------------------------------------");
            strLinhaReg.AppendLine(usr.filial.Fantasia);
            strLinhaReg.AppendLine("PEDIDO: " + ped.Pedido.ToString().Trim().PadLeft(10, '0'));
            strLinhaReg.AppendLine("------------------------------------------------");
            
            strLinhaReg.AppendLine("DATA ENTREGA: " + ped.Data_entrega.ToString("dd/MM/yyyy") + "        HORA:"+ped.hora);
            strLinhaReg.AppendLine("");
            //strLinhaReg.AppendLine("Atendente: " + ped.funcionario.ToString());
            strLinhaReg.AppendLine("Cliente:" + Cliente.Codigo_Cliente.Trim() + "-" + Cliente.Nome_Cliente.Trim());
            if(!Cliente.nome_fantasia.Trim().Equals(""))
            {
                strLinhaReg.AppendLine("Nome Fantasia:" + Cliente.nome_fantasia.Trim());
            }
            strLinhaReg.AppendLine("End: " + Cliente.Endereco.Trim() + " " + Cliente.endereco_nro.ToString().Trim() + " - " + Cliente.complemento_end.ToString().Trim());
            strLinhaReg.AppendLine("Bairro: " + Cliente.Bairro.Trim()) ;
            //Incluir o telefone
            if (Cliente.meiosComunicacao.Rows.Count > 0)
            {
                strLinhaReg.AppendLine("Meios de Contato:" );
                for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                {
                    strLinhaReg.Append("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ");
                    strLinhaReg.AppendLine("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1]);
                }
            }

            strLinhaReg.AppendLine("================================================" );
            strLinhaReg.AppendLine("PRODUTO    QTDE  UND      VALOR            TOTAL" );
            strLinhaReg.AppendLine("================================================");
            foreach (pedido_itensDAO item in ped.PedItens)
            {
                if (!item.excluido)
                {
                    strLinhaReg.Append(item.PLU.ToString().PadLeft(6, '0') + " ");
                    if (item.Descricao.ToString().Length > 35)
                    {
                        strLinhaReg.AppendLine(Funcoes.RemoverAcentos(item.Descricao).Substring(0, 35).PadRight(35, ' '));
                    }
                    else
                    {
                        strLinhaReg.AppendLine(Funcoes.RemoverAcentos(item.Descricao).PadRight(35, ' '));
                    }
                    strLinhaReg.Append(item.Qtde.ToString("N3").PadLeft(15, ' ') + " x " );
                    strLinhaReg.Append(Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null));
                    // strLinhaReg.Append(item.CodReferencia.PadRight(7, ' ') + "-");

                    strLinhaReg.Append((item.unitario).ToString("N2").PadLeft(10, ' ')+"=");
                    //strLinhaReg = strLinhaReg + item.unitario.ToString("N2").PadLeft(12, ' ');
                    strLinhaReg.AppendLine(item.total.ToString("N2").PadLeft(15, ' '));

                }
            }

            strLinhaReg.AppendLine("------------------------------------------------");
            strLinhaReg.AppendLine("".PadLeft(15, ' ') + "VALOR:" + ped.Total.ToString("N2").PadLeft(15, ' '));
            strLinhaReg.AppendLine("------------------------------------------------");
            strLinhaReg.AppendLine("");
            if (ped.PedPg.Count > 0)
            {
                strLinhaReg.AppendLine("FORMA DE PAGAMENTO");
                
                for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                {
                    strLinhaReg.Append("Tipo: " + ped.PdPag().Rows[i].ItemArray[5] + " ");
                    strLinhaReg.AppendLine("Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + " ");

                    //strLinhaReg = strLinhaReg + "Vlr: " + ped.PdPag().Rows[i].ItemArray[6] 
                    strLinhaReg.AppendLine("------------------------------------------------");
                }
            }
            if (ped.Obs.Length > 0)
            {
                strLinhaReg.AppendLine("Observacoes:");
                strLinhaReg.AppendLine(ped.Obs);
            }
         
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("----------------------------------");
            strLinhaReg.AppendLine("VISTO DE RECEBIMENTO");
            //strLinhaReg.AppendLine("------------------------------------------------");
            //strLinhaReg.AppendLine("Imp.:" + DateTime.Now.ToString() + (char)13 + (char)10); tirado por causa do Portal do Padeiro
            strLinhaReg.AppendLine("------------------------------------------------");
            for (int y = 1; y <= intLinhasImpPosPedido; y++)
            {
                strLinhaReg.AppendLine("");
            }
            //strLinhaReg.Append();
            return strLinhaReg.ToString() + (char)29 + (char)86 + (char)66 + (char)0;

        }



    }

}