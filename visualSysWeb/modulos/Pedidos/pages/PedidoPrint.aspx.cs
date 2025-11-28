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
using System.Data;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoPrint : System.Web.UI.Page
    {
        bool ocultaVlrPG = Funcoes.valorParametro("OCULTA_PG_PEDIDO", null).ToUpper().Equals("TRUE");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                User usr = (User)Session["User"];
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : "\\\\" + usr.filial.ip);
                pedidoDAO obj = (pedidoDAO)Session["PedidoPrint"];
                bool imp40 = Funcoes.valorParametro("PEDIDO_IMPRIMIR_40", usr).ToUpper().Equals("TRUE");
                bool imp40Delivery = Funcoes.valorParametro("PEDIDO_IMP_40_DELIVERY", usr).ToUpper().Equals("TRUE");
                String endImpressoraRemota = Funcoes.valorParametro("IMP_REMOTA_PEDIDO", usr);
                String tipoRelatorio = Request.Params["simples"];
                int qtdeImp = Funcoes.intTry(Funcoes.valorParametro("QTDE_IMPRESSAO_PEDIDO", usr));
                if (qtdeImp == 0)
                    qtdeImp = 1;



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
                            int qtdeArquivos = 0;

                            if (endImpressoraRemota.Length > 0)
                            {
                                qtdeArquivos = 1;
                            }
                            else
                            {
                                qtdeArquivos = qtdeImp;
                            }

                            for (int i = 0; i < qtdeArquivos; i++)
                            {

                           
                                StreamWriter ArqImprime = null;
                                try
                                {


                                    ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido" + obj.Pedido.Trim()+i + ".txt", false, Encoding.ASCII);
                                    if (imp40Delivery)
                                    {
                                        ArqImprime.Write(GerarArquivo40Delivery(obj));
                                    }
                                    else if (imp40)
                                    {
                                        ArqImprime.Write(GerarArquivo40(obj));
                                    }
                                    else
                                    {
                                        ArqImprime.Write(GerarArquivo(obj, false));
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                finally
                                {
                                    if (ArqImprime != null)
                                        ArqImprime.Close();
                                }
                            }
                        }
                       
                       
                        if (endImpressoraRemota.Length>0)
                        {

                            for (int i = 0; i < qtdeImp; i++)
                            {
                                File.Copy(endereco + "\\imprimePedido\\Pedido" + obj.Pedido.Trim() + "0.txt", @endImpressoraRemota);
                            }


                            File.Delete(endereco + "\\imprimePedido\\Pedido" + obj.Pedido.Trim() + "0.txt");

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
            pedidoDAO obj = (pedidoDAO)Session["PedidoPrint"];
            lblPedido.Text = (obj.Pedido == null ? "" : obj.Pedido.ToString());
            divFilial.Visible = false;
            lblTipoPedido.Text = "";
            if (obj.Tipo == 1)
            {
                lblTituloPedido.Text = "PEDIDO DE VENDA";
            }
            else if (obj.Tipo == 2)
            {
                lblTituloPedido.Text = "PEDIDO DE COMPRA";
            }
            else if (obj.Tipo == 8 )
            {
                lblTituloPedido.Text = "ORÇAMENTO";
                //lblOrcamentoObservacoes.Text = Funcoes.valorParametro("OBS_ORCAMENTO", null);
                //divFilial.Visible = true;
                //filialDAO filial = new filialDAO(obj.Filial);
                //lblFilialCnpj.Text = filial.CNPJ;
                //lblFilialRazaoSocial.Text = filial.Razao_Social;
                //lblFilialEndereco.Text = filial.Endereco + "," + filial.endereco_nro + " " + filial.bairro + " " +filial.Cidade +"-" +filial.UF;

            }
            divFilial.Visible = true;
            filialDAO filial = new filialDAO(obj.Filial);
            lblFilialCnpj.Text = filial.CNPJ;
            lblFilialIE.Text = filial.IE;
            lblFilialRazaoSocial.Text = filial.Razao_Social + " - " + filial.Fantasia;
            lblFilialEndereco.Text = filial.Endereco + "," + filial.endereco_nro + " " + filial.bairro + " " + filial.Cidade + " - " + filial.UF + "- CEP: " + filial.CEP;
            lblFilialContato.Text = filial.fone;

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
            if (obj.Tipo == 1 || obj.Tipo ==8)
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
            lblTipoPedido.Text = (obj.entrega ? "ENTREGA" : "RETIRA");

            //Dados do cliente
            lblEndereco_Completo.Text = (((obj.cliente.Endereco + " " + obj.cliente.endereco_nro).Trim() + " "
                + obj.cliente.complemento_end).Trim() + ", "
                + obj.cliente.Bairro).Trim() + ", "
                + (obj.cliente.Cidade + " UF:" + obj.cliente.UF + " CEP:" + obj.cliente.CEP).Trim();

            bool _naoImpRef = Funcoes.valorParametro("N_IMP_REF_PED", obj.usr).ToUpper().Equals("TRUE");

            int intLinhaImpressao = 0;

            gridItens.DataSource = obj.PdItens(_naoImpRef);
            gridItens.DataBind();

            int linhasAdd = 2;
            foreach (GridViewRow item in gridItens.Rows)
            {
                //if(!item.Cells[10].Text.Trim().Replace("&nbsp;", "").Equals(""))
                pedido_itensDAO itemImpressao = (pedido_itensDAO)obj.PedItens[intLinhaImpressao];
                if (!itemImpressao.obs.Equals("") || itemImpressao.produzir)
                {
                    GridViewRow newRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    TableCell rowCell = new TableCell();
                    rowCell.ColumnSpan= item.Cells.Count;
                    rowCell.Text = (!itemImpressao.obs.Equals("") ? "OBS: " + itemImpressao.obs + "." : "") + (itemImpressao.produzir ? " Dt.Hr.Produção: " + itemImpressao.data_hora_produzir.ToString("dd/MM/yyy HH:mm") + " " + itemImpressao.agrupamento : "");
                    newRow.Cells.Add(rowCell);
                    gridItens.Controls[0].Controls.AddAt(item.RowIndex+linhasAdd, newRow);
                    linhasAdd++;
                }
                intLinhaImpressao++;
            }
            //gridItens.Columns[9].Visible = false;


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


            if (!html)
            {

                strLinhaReg.Append( "Orcamento: " + ped.Pedido.ToString().Trim().PadLeft(10, '0'));
                strLinhaReg.AppendLine((ped.pedido_simples ? "***" : ""));
                strLinhaReg.Append("Data: " + ped.Data_cadastro.ToString("dd/MM/yyyy") + " Hora:" + ped.hora);
                strLinhaReg.AppendLine("Atendente: " + ped.funcionario.ToString());
                strLinhaReg.AppendLine("Cliente:" + Cliente.Codigo_Cliente + "-" + Cliente.Nome_Cliente + " Telefone: ");
                strLinhaReg.AppendLine("Ende: " + Cliente.Endereco + " " + Cliente.endereco_nro.ToString() + " - " + Cliente.complemento_end.ToString().Trim());
                strLinhaReg.AppendLine("Bairro: " + Cliente.Bairro + " Cidade:" + Cliente.Cidade );
                strLinhaReg.AppendLine("CEP: " + Cliente.CEP + " UF: " + Cliente.UF );
                strLinhaReg.AppendLine("CNPJ/CPF: " + Cliente.CNPJ.Trim() + " Insc.Estadual: " + Cliente.IE);
                strLinhaReg.AppendLine("TIPO: "+(ped.entrega ? "ENTREGA" : "RETIRA"));

                //Incluir o telefone
                if (Cliente.meiosComunicacao.Rows.Count > 0)
                {
                    strLinhaReg.AppendLine("Meios de Contato:");
                    for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                    {
                        strLinhaReg.Append("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ");
                        strLinhaReg.AppendLine("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1]);
                    }
                }

                strLinhaReg.AppendLine("===============================================================================" );
                strLinhaReg.AppendLine(  "Qtde  Uni Cod            Descricao                            Unit  Total Item" );
                strLinhaReg.AppendLine( "===============================================================================" );
                bool _naoImpRef = Funcoes.valorParametro("N_IMP_REF_PED", usr).ToUpper().Equals("TRUE");
                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    if (!item.excluido)
                    {
                        strLinhaReg.Append(item.Qtde.ToString("N2").PadLeft(5, ' ') + " ");
                        strLinhaReg.Append(Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null));
                        String codRef = "";
                        if (!_naoImpRef)
                            codRef = item.CodReferencia;

                        strLinhaReg.Append(codRef.PadRight(7, ' ') + "-");
                        strLinhaReg.Append(item.PLU.ToString().PadLeft(6, '0') + " ");
                        if (item.Descricao.ToString().Length > 35)
                        {
                            strLinhaReg.Append(Funcoes.RemoverAcentos(item.Descricao).Substring(0, 35).PadRight(35, ' '));
                        }
                        else
                        {
                            strLinhaReg.Append(Funcoes.RemoverAcentos(item.Descricao).PadRight(35, ' '));
                        }
                        strLinhaReg.Append((item.total / item.Qtde).ToString("N2").PadLeft(8, ' '));
                        //strLinhaReg += item.unitario.ToString("N2").PadLeft(12, ' ');
                        strLinhaReg.Append( item.total.ToString("N2").PadLeft(10, ' '));
                        strLinhaReg.AppendLine();
                        if (item.obs.Length>0)
                        {
                            strLinhaReg.AppendLine("OBS:"+ item.obs);
                            
                        }
                        strLinhaReg.AppendLine( "-------------------------------------------------------------------------------");

                        
                    }
                }
                strLinhaReg.AppendLine(strSpace.PadLeft(50, ' ') + "Vlr do Pedido:" + ped.Total.ToString("N2").PadLeft(10, ' '));
               
                if (ped.PdPag().Rows.Count > 0)
                {
                    strLinhaReg.AppendLine("FORMA DE PAGAMENTO" );
                    strLinhaReg.AppendLine();
                    for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                    {
                        strLinhaReg.Append("Tipo: " + ped.PdPag().Rows[i].ItemArray[5] + " ");
                        strLinhaReg.Append("Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + " ");

                        if(!ocultaVlrPG)
                            strLinhaReg.AppendLine(  "Vlr: " + ped.PdPag().Rows[i].ItemArray[6] +" ");

                        strLinhaReg.AppendLine( "-------------------------------------------------------------------------------") ;
                    }
                }

                strLinhaReg.AppendLine( "Observacoes:" );
                strLinhaReg.AppendLine(ped.Obs );
                strLinhaReg.AppendLine("Imp.:" + DateTime.Now.ToString() );

                for (int y = 1; y <= intLinhasImpPosPedido; y++)
                {
                    strLinhaReg.AppendLine();
                }
                return strLinhaReg.ToString();
            }
            else
            {

                strLinhaReg.Append( "Orcamento: " + ped.Pedido.ToString().Trim().PadLeft(10, '0'));
                strLinhaReg.Append( (ped.pedido_simples ? "***" : "") + "</br></br>");
                strLinhaReg.Append( "Data: " + ped.Data_cadastro.ToString("dd/MM/yyyy") + " " + ped.hora);
                strLinhaReg.Append( "Atendente: " + ped.funcionario.ToString() + "</br>");
                strLinhaReg.Append( "Cliente:" + Cliente.Codigo_Cliente.Trim() + "-" + Cliente.Nome_Cliente + "<br>");
                strLinhaReg.Append( "Ende: " + Cliente.Endereco + " " + Cliente.endereco_nro.ToString() + " - " + Cliente.complemento_end.ToString().Trim() + "</br>");
                strLinhaReg.Append( "Bairro: " + Cliente.Bairro + " Cidade:" + Cliente.Cidade + "</br>");
                strLinhaReg.Append( "CEP: " + Cliente.CEP + " UF: " + Cliente.UF + "</br>");
                strLinhaReg.Append( "CNPJ/CPF: " + Cliente.CNPJ.Trim() + " Insc.Estadual: " + Cliente.IE + "</br>");
                strLinhaReg.Append( "TIPO: " + (ped.entrega ? "ENTREGA" : "RETIRA") + "</br></br>");
                //Incluir o telefone
                if (Cliente.meiosComunicacao.Rows.Count > 0)
                {
                    strLinhaReg.Append( "Meios de Contato:" + "</br>");
                    for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                    {
                        strLinhaReg.Append( "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ");
                        strLinhaReg.Append( "     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1] + "</br>");
                    }
                }

                strLinhaReg.Append( " </br></br>");

                strLinhaReg.Append( "<table class=\"tableSimplesPedido\">");
                strLinhaReg.Append( "<tr>");
                strLinhaReg.Append( "<th>Qtde</th>");
                strLinhaReg.Append( "<th>Uni</th>");
                strLinhaReg.Append( "<th>Cod </th>");
                strLinhaReg.Append( "<th>Descricao </th>");
                strLinhaReg.Append( "<th class=\"thNumero\">Unit</th>");
                strLinhaReg.Append( "<th class=\"thNumero\">Total Item</th>");
                strLinhaReg.Append( "</tr>");
                bool _naoImpRef = Funcoes.valorParametro("N_IMP_REF_PED", usr).ToUpper().Equals("TRUE");

                foreach (pedido_itensDAO item in ped.PedItens)
                {
                    if (!item.excluido)
                    {
                        strLinhaReg.Append( "<tr >");

                        strLinhaReg.Append( "<td >" + item.Qtde.ToString("N2") + "</td> ");
                        strLinhaReg.Append( "<td >" + Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null) + "</td> ");
                        String codRef = "";
                        if (!_naoImpRef)
                            codRef = item.CodReferencia;

                        strLinhaReg.Append( "<td >" +codRef  + "-" + item.PLU.ToString() + " " + "</td> ");
                        if (item.Descricao.ToString().Length > 35)
                        {
                            strLinhaReg.Append( "<td >" + Funcoes.RemoverAcentos(item.Descricao).Substring(0, 35) + "</td> ");
                        }
                        else
                        {
                            strLinhaReg.Append( "<td >" + Funcoes.RemoverAcentos(item.Descricao) + "</td> ");
                        }
                        strLinhaReg.Append( "<td class=\"tdNumero\">" + (item.total / item.Qtde).ToString("N2") + "</td> ");
                        //strLinhaReg.Append( item.unitario.ToString("N2").PadLeft(12, ' ');
                        strLinhaReg.Append( "<td class=\"tdNumero\">" + item.total.ToString("N2").PadLeft(11, ' ') + "</td> ");

                        strLinhaReg.Append( "</tr>");
                        if(item.obs.Length>0)
                        {
                            strLinhaReg.Append( "<tr style=\"border-top:hidden\">");
                            strLinhaReg.Append( "<td colspan=\"7\" style=\"border-top:hidden\">");
                            strLinhaReg.Append( "OBS:"+item.obs);
                            strLinhaReg.Append( "</td>");
                            strLinhaReg.Append( "</tr>");


                        }

                    }
                }
                strLinhaReg.Append( "<tr>");
                strLinhaReg.Append( "<td colspan=\"4\"></td>");
                strLinhaReg.Append( "<td> Vlr do Pedido:</td>");
                strLinhaReg.Append( "<td class=\"tdNumero\"> " + ped.Total.ToString("N2").PadLeft(10, ' ') + "</td>");
                strLinhaReg.Append( "</tr>");

                if (ped.PdPag().Rows.Count > 0)
                {
                    strLinhaReg.Append( "<tr>");

                    strLinhaReg.Append( "<td class=\"semBorda\" colspan=\"6\">FORMA DE PAGAMENTO </td>");

                    strLinhaReg.Append( "</tr>");
                   
                    for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                    {
                        strLinhaReg.Append( "<tr>");
                        strLinhaReg.Append( "<td colspan=\"3\" > Tipo: " + ped.PdPag().Rows[i].ItemArray[5] + "</td> ");
                        strLinhaReg.Append( "<td > Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + "</td> ");

                        if (!ocultaVlrPG)
                            strLinhaReg.Append( "<td >  Vlr: " + ped.PdPag().Rows[i].ItemArray[6] + "</td> ");


                        strLinhaReg.Append( "</tr>");
                    }
                  

                }
                strLinhaReg.Append( "<tr>");
                strLinhaReg.Append( "<td class=\"semBorda\" colspan=\"6\"> Observacoes:</td>");
                strLinhaReg.Append( "</tr>");
                strLinhaReg.Append( "<tr>");
                strLinhaReg.Append( "<td class=\"semBorda\" colspan=\"6\">");
                strLinhaReg.Append( ped.Obs.Replace("" + (char)10, "</br>") + "</br>");
                strLinhaReg.Append( "Imp.:" + DateTime.Now.ToString());
                strLinhaReg.Append( "</tr>");



                string quebra = ("" + (char)13 + (char)10).ToString();
                return strLinhaReg.ToString().Replace(quebra, "</br>").Replace("  ", "&nbsp;&nbsp;");

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

            strLinhaReg.AppendLine((char)15 + "------------------------------------------------");
            strLinhaReg.AppendLine(usr.filial.Fantasia);
            strLinhaReg.AppendLine("PEDIDO: " + ped.Pedido.ToString().Trim().PadLeft(10, '0'));
            strLinhaReg.AppendLine("------------------------------------------------");

            strLinhaReg.AppendLine("DATA ENTREGA: " + ped.Data_entrega.ToString("dd/MM/yyyy") + "        HORA:" + ped.hora);
            strLinhaReg.AppendLine("");
            //strLinhaReg.AppendLine("Atendente: " + ped.funcionario.ToString());
            strLinhaReg.AppendLine("Cliente:" + Cliente.Codigo_Cliente.Trim() + "-" + Cliente.Nome_Cliente.Trim());
            if (!Cliente.nome_fantasia.Trim().Equals(""))
            {
                strLinhaReg.AppendLine("Nome Fantasia:" + Cliente.nome_fantasia.Trim());
            }
            strLinhaReg.AppendLine("End: " + Cliente.Endereco.Trim() + " " + Cliente.endereco_nro.ToString().Trim() + " - " + Cliente.complemento_end.ToString().Trim());
            strLinhaReg.AppendLine("Bairro: " + Cliente.Bairro.Trim());
            //Incluir o telefone
            if (Cliente.meiosComunicacao.Rows.Count > 0)
            {
                strLinhaReg.AppendLine("Meios de Contato:");
                for (int i = 0; i < Cliente.meiosComunicacao.Rows.Count; i++)
                {
                    strLinhaReg.Append("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[0] + ": ");
                    strLinhaReg.AppendLine("     " + Cliente.meiosComunicacao.Rows[i].ItemArray[1]);
                }
            }

            strLinhaReg.AppendLine("================================================");
            strLinhaReg.AppendLine("PRODUTO    QTDE  UND      VALOR            TOTAL");
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
                    strLinhaReg.Append(item.Qtde.ToString("N3").PadLeft(15, ' ') + " x ");
                    strLinhaReg.Append(Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null));
                    // strLinhaReg.Append(item.CodReferencia.PadRight(7, ' ') + "-");

                    strLinhaReg.Append((item.unitario).ToString("N2").PadLeft(10, ' ') + "=");
                    //strLinhaReg += item.unitario.ToString("N2").PadLeft(12, ' ');
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
                    strLinhaReg.Append("Vcto: " + ped.PdPag().Rows[i].ItemArray[0] + " ");
                    if(!ocultaVlrPG)
                        strLinhaReg.AppendLine("Vlr: " + ped.PdPag().Rows[i].ItemArray[6]);
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


        protected String GerarArquivo40Delivery(pedidoDAO ped)
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


        
            ClienteDAO Cliente = new ClienteDAO(ped.Cliente_Fornec, usr);
            strLinhaReg.AppendLine((char)15 + ("PEDIDO: " + ped.Pedido.ToString().Trim()).PadLeft(40, ' '));
            strLinhaReg.AppendLine(usr.filial.Fantasia);
            strLinhaReg.AppendLine("-".PadLeft(40,'-'));
            if (ped.entrega)
                strLinhaReg.Append("".PadLeft(5, ' ') + "*** E N T R E G A ***" );
            else
                strLinhaReg.Append("".PadLeft(8, ' ') + "*** R E T I R A ***");
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("-".PadLeft(40, '-'));
            strLinhaReg.AppendLine("Atendente:"+usr.getUsuario() + " "+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " " );
            strLinhaReg.AppendLine("-".PadLeft(40, '-'));
            //Configuração para impressão
            //strLinhaReg.Append((char)29 + (char)33 + (char)1 + (char)2);
            string ESC = "\u001B";
            string GS = "\u001D";
            string BoldON = ESC + "E" + "\u0001";
            string BoldOFF = ESC + "E" + "\0";
            string DoubleON = GS + "!" + "\u0011";
            string DoubleOFF = GS + "!" + "\0";

            //strLinhaReg.Append("\u001D\u0021\u0001\u0002");
            strLinhaReg.AppendLine(DoubleON + BoldON + "PEDIDO:  "+ped.Pedido +  BoldOFF + DoubleOFF );
            
            //strLinhaReg.Append("\u001D\u0021\u0000\u0002\u001B\u0061" + "0");

            //strLinhaReg.Append((char)29 + (char)33 + (char)0 + (char)2 + (char)27 + (char)97 + "0");
            strLinhaReg.AppendLine("Data do Pedido:" + ped.Data_cadastro.ToString("dd-MM-yyyy") +" " +ped.hora_cadastro);
            strLinhaReg.AppendLine("Agendado para:" + ped.Data_entrega.ToString("dd-MM-yyyy")+ " " + ped.hora);


            strLinhaReg.AppendLine("Dados de Entrega".PadRight(40, '-'));
            strLinhaReg.AppendLine("");
            strLinhaReg.AppendLine("Nome:");
            strLinhaReg.AppendLine(DoubleON + BoldON + Cliente.Nome_Cliente.Trim()+ BoldOFF + DoubleOFF);
            strLinhaReg.AppendLine("Fone:"+ Cliente.primeiroMeioComunicacao().Trim());

            String ender = "";
            String comp = "";
            String bairro = "";
            String cidade = "";
            String cep = "";
            if (Cliente.Endereco_ent.Trim().Length>0)
            {
                ender = Cliente.Endereco_ent.Trim() + " " + Cliente.endereco_ent_nro.ToString().Trim() ;
                comp = Cliente.complemento_ent.Trim();
                bairro = Cliente.Bairro_ent.Trim();
                cidade = Cliente.Cidade_ent.Trim();
                cep = Cliente.Cep_ent.Trim();
            }
            else
            {
                ender = Cliente.Endereco.Trim() + " " + Cliente.endereco_nro.ToString().Trim() ;
                comp = Cliente.complemento_end;
                bairro = Cliente.Bairro.Trim();
                cidade = Cliente.Cidade.Trim();
                cep = Cliente.CEP.Trim();
            }
           
            strLinhaReg.Append(linha40("Ender: " +ender));
            strLinhaReg.AppendLine("Comp.: " +comp);
            strLinhaReg.AppendLine("Bairro: " +bairro);
            strLinhaReg.AppendLine("Cidade: " +cidade + " CEP: "+cep );

            strLinhaReg.AppendLine("-".PadLeft(40, '-'));
            strLinhaReg.AppendLine("Observacoes:");
            strLinhaReg.AppendLine(linha40(Funcoes.RemoverAcentos(ped.Obs)));
            strLinhaReg.AppendLine("=".PadLeft(40, '='));
            strLinhaReg.AppendLine("Item Codigo       Descricao");
            strLinhaReg.AppendLine("   qtde         Prc Unitario  Total Item");
            strLinhaReg.AppendLine("=".PadLeft(40, '='));

            foreach (pedido_itensDAO item in ped.PedItens)
            {
                if (!item.excluido)
                {
                    strLinhaReg.AppendLine("");
                    strLinhaReg.Append(item.num_item.ToString().PadLeft(3, '0'));
                    strLinhaReg.Append(" "+item.PLU.ToString().PadLeft(7, '0') + " ");
                    if (item.Descricao.ToString().Length > 27)
                    {
                        strLinhaReg.Append(Funcoes.RemoverAcentos(item.Descricao).Substring(0, 27));
                    }
                    else
                    {
                        strLinhaReg.Append(Funcoes.RemoverAcentos(item.Descricao).PadRight(27, ' '));
                    }
                    strLinhaReg.AppendLine("");
                    strLinhaReg.Append(item.Qtde.ToString("N3").PadLeft(9,' ')+ " X ");
                    strLinhaReg.Append(Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + item.PLU + "'", null).Trim());
                    strLinhaReg.Append((item.unitario).ToString("N2").PadLeft(10, ' ') + "=");
                    strLinhaReg.AppendLine(item.total.ToString("N2").PadLeft(15, ' '));
                    if (item.obs.Length > 0)
                        strLinhaReg.Append(linha40("Obs:"+Funcoes.RemoverAcentos(item.obs)));


                    if (item.produzir)
                        strLinhaReg.AppendLine("PRODUZIR: "+item.data_hora_produzir.ToString("dd/MM/yyyy HH:mm"));

                    strLinhaReg.AppendLine("-".PadLeft(40, '-'));

                }
            }

            //strLinhaReg.AppendLine("-".PadLeft(40, '-'));
            strLinhaReg.AppendLine( ("Total R$ " + DoubleON + BoldON + ped.Total.ToString("N2").PadLeft(12, ' ')).PadLeft(30,' ')+ BoldOFF + DoubleOFF);
            strLinhaReg.AppendLine("-".PadLeft(40, '-'));
            strLinhaReg.AppendLine("");
            if (ped.PedPg.Count > 0)
            {
                Decimal totalPg = 0;

                for (int i = 0; i < ped.PdPag().Rows.Count; i++)
                {
                    totalPg += Funcoes.decTry(ped.PdPag().Rows[i].ItemArray[6].ToString());
                    strLinhaReg.AppendLine((ped.PdPag().Rows[i].ItemArray[5] + " " 
                                          +(ocultaVlrPG?"": ped.PdPag().Rows[i].ItemArray[6].ToString()).PadLeft(12)
                                          ).PadLeft(40)); 
                }

                if(totalPg> ped.Total)
                {
                    strLinhaReg.AppendLine(("Troco: " + (totalPg - ped.Total ).ToString("N2").PadLeft(12)).PadLeft(40, ' '));
                }
            }
           
            strLinhaReg.AppendLine("=".PadLeft(40,'='));
            for (int y = 1; y <= intLinhasImpPosPedido; y++)
            {
                strLinhaReg.AppendLine("");
            }
            //strLinhaReg.Append();
            return strLinhaReg.ToString() + (char)29 + (char)86 + (char)66 + (char)0;

        }


        private String linha40(String obs)
        {

            StringBuilder obsFinal = new StringBuilder();
            if (obs.Length > 0)
            {
                int qtdLinha = obs.Length / 40;
                int ini = 0;
                for(int i = 0; i<= qtdLinha; i++)
                {
                    if (obs.Substring(ini).Length > 40)
                        obsFinal.AppendLine(obs.Substring(ini, 40));
                    else
                        obsFinal.AppendLine(obs.Substring(ini));
                    ini += 40;
                }
                
            }
            return obsFinal.ToString();
        }
    }

}