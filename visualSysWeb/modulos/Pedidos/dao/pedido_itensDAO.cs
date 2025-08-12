using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class pedido_itensDAO
    {
        public String Filial { get; set; }
        public String Pedido = "";
        public String PLU { get; set; }
        private String _ean = "";
        public String ean
        {
            get
            {
                if (_ean.Equals(""))
                {
                    _ean = Conexao.retornaUmValor("Select top 1 ean from ean where plu ='" + PLU + "'", null);
                    if (_ean.Equals(""))
                        _ean = "SEM";
                }
                if (_ean.Equals("SEM"))
                    return "";

                return _ean;
            }
            set
            {
                _ean = value;
            }
        }
        public String codCliente = "";
        public Decimal valorDesconto = 0;
        private User usr = new User();
        private TabelaPrecoDAO tbPrecoPedido = new TabelaPrecoDAO();
        public TabelaPrecoDAO TabelaPrecoPedido
        {
            get
            {
                return tbPrecoPedido;

            }
            set
            {
                tbPrecoPedido = value;

            }
        }
        public int index = -1;
        public string obs = "";
        public bool produzir = false;
        public DateTime data_hora_produzir = new DateTime();
        public string agrupamento = "";
        public preco_mercadoriaDAO tbPrecoMercadoria = new preco_mercadoriaDAO();



        public String TabelaPrecoMercadoria()
        {
            if (tbPrecoMercadoria != null && tbPrecoMercadoria.Codigo_tabela != null)
            {
                return tbPrecoMercadoria.Codigo_tabela;
            }
            else
            {
                if (!codCliente.Equals("") && PLU != null && !PLU.Equals(""))
                {

                    String tabelaCliente = Conexao.retornaUmValor("select cliente.codigo_tabela " +
                                                                   " from cliente inner join preco_mercadoria " +
                                                                      "  on cliente.codigo_tabela = preco_mercadoria.codigo_tabela " +
                                                                    " where cliente.codigo_cliente='" + codCliente + "' and preco_mercadoria.plu='" + PLU + "'", usr);
                    if (!tabelaCliente.Equals(""))
                    {
                        tbPrecoMercadoria = new preco_mercadoriaDAO(tabelaCliente, PLU, usr);

                        if (tbPrecoMercadoria != null)
                        {
                            //unitario = tbPrecoMercadoria.Preco;

                            return tbPrecoMercadoria.Codigo_tabela;
                        }
                    }
                }


                return "";
            }
        }
        public Decimal DescontoTabela()
        {
            if (Desconto == 0)
            {
                //if (tbPrecoMercadoria != null)
                //{
                //    //if (Desconto < tbPrecoPedido.porc)
                //    //{
                //    Desconto = tbPrecoMercadoria.Desconto;
                //    //}
                //}
            }
            valorDesconto = (((unitario * Embalagem) * Qtde) * Desconto) / 100;
            return Desconto;

        }


        private String _descricao = "";
        public String Descricao
        {
            get
            {
                if (PLU == null || PLU.Equals(""))
                {
                    _descricao = "";
                }
                else if(_descricao.Equals(""))
                {
                    _descricao = Conexao.retornaUmValor("select Descricao from mercadoria where plu ='" + PLU + "'", new User());
                }
                    
                return _descricao;
            }
            set
            {
                _descricao = value;
            }


        }

        public bool excluido = false;
        public bool inserido = false;
        public Decimal Qtde { get; set; }
        public Decimal Embalagem { get; set; }
        public Decimal vUnitario = 0;
        public String documento = "";
        public DateTime data_documento = new DateTime();
        public int caixa_documento = 0;

        public Decimal unitario
        {
            get
            {
              
                return vUnitario;
              
            }

            set
            {
                if (!inserido || tbPrecoMercadoria == null || tbPrecoMercadoria.Codigo_tabela == null)
                {
                    vUnitario = value;
                }

            }


        }
        public int Tipo { get; set; }
        //public natureza_operacaoDAO naturezaOperacao { get; set; }
        public Decimal vTotal = 0;
        public Decimal vUnitarioDesconto = 0;
        public Decimal total
        {
            get
            {
                TabelaPrecoMercadoria();
                Decimal vdesconto = DescontoTabela();
                vdesconto = (unitario * vdesconto) / 100;
                vUnitarioDesconto = (unitario - vdesconto);
                vTotal = (Qtde * Embalagem) * vUnitarioDesconto;
                return vTotal;
            }

            set { vTotal = value; }
        }
        public Decimal totalbruto
        {
            get
            {
                return (Qtde * Embalagem) * unitario;
            }
        }
        public Decimal id { get; set; }
        public Decimal Desconto = 0;
        public Decimal vPrecoMinimo = 0;
        public int num_item = 0;

        public Decimal precoMinimo
        {
            get
            {
                if (vPrecoMinimo == 0)
                    vPrecoMinimo = Decimal.Parse(Conexao.retornaUmValor("select isnull(mercadoria_loja.preco,0) from mercadoria_loja where plu='" + PLU + "' and FILIAL='" + usr.getFilial() + "'", null));
                return vPrecoMinimo;
            }
            set
            {
                vPrecoMinimo = value;
            }
        }

        public String CodReferencia
        {
            get
            {
                if (PLU == null || PLU.Equals(""))
                    return "";
                else
                    return Conexao.retornaUmValor("select Ref_fornecedor from mercadoria where plu ='" + PLU + "'", new User());
            }
        }

        public Decimal pesoBruto
        {
            get
            {
                if (PLU == null || PLU.Equals(""))
                    return 0;
                else
                {
                    Decimal vPesoBruto = 0;
                    Decimal.TryParse(Conexao.retornaUmValor("select ISNULL(PESO_BRUTO,0) from mercadoria where plu ='" + PLU + "'", new User()), out vPesoBruto);
                    return vPesoBruto;

                }
            }
        }



        public pedido_itensDAO(User usr)
        {
            this.usr = usr;
        }


        public pedido_itensDAO(String pedido, String plu, String tipo, User usr)
        { //colocar campo index da tabela
            this.usr = usr;

            String sql = "Select * from  pedido_itens where pedido =" + pedido + " and plu='" + plu + "' and isnull(tipo,0)=" + tipo;
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, usr, true);
                carregarDados(rs);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }

        private String dataBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }
        public bool Equals(pedido_itensDAO obj)
        {
            return (obj.Filial.Equals(Filial) && obj.PLU.Equals(PLU) && obj.Pedido.Equals(Pedido) && obj.Tipo.Equals(Tipo));
        }

        public ArrayList ArrToString(bool naoRef)
        {

            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(Pedido.ToString());
            item.Add(PLU);
            item.Add(naoRef ? "" : CodReferencia);
            item.Add(Descricao.Replace(" ", "&nbsp"));
            item.Add(Qtde.ToString("N3"));
            item.Add(Embalagem.ToString("N2"));
            item.Add(TabelaPrecoMercadoria());
            item.Add(unitario.ToString("N2"));
            item.Add(DescontoTabela().ToString("N2"));
            item.Add(((vTotal <= 0 ? total : vTotal)).ToString("N2"));
            item.Add(ean);
            item.Add(id.ToString());
            item.Add(documento);
            item.Add(caixa_documento.ToString());
            item.Add(data_documento.ToString("dd/MM/yyyy"));
            item.Add(pesoBruto.ToString());
            //if (obs.Length > 20)
            //    item.Add(obs.ToString().Substring(0, 20) + "...");
            //else
            item.Add(obs.ToString());
            //DataHoraProduzir e Agrupamento
            item.Add((produzir ? data_hora_produzir.ToString("dd/MM/yyyy HH:mm") : ""));
            item.Add(agrupamento);
            item.Add(vUnitarioDesconto.ToString("N2"));
            return item;
        }


        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                Filial = rs["Filial"].ToString();
                Pedido = rs["Pedido"].ToString();
                Tipo = (rs["Tipo"] == null ? 0 : int.Parse(rs["Tipo"].ToString()));
                PLU = rs["PLU"].ToString();
                Qtde = (Decimal)(rs["Qtde"].ToString().Equals("") ? new Decimal() : rs["Qtde"]);
                Embalagem = (Decimal)(rs["Embalagem"].ToString().Equals("") ? new Decimal() : rs["Embalagem"]);
                unitario = (Decimal)(rs["unitario"].ToString().Equals("") ? new Decimal() : rs["unitario"]);
                ean = rs["ean"].ToString();
                id = (rs["id"].ToString().Equals("") ? new Decimal() : Decimal.Parse(rs["id"].ToString()));
                Desconto = (Decimal)(rs["Desconto"].ToString().Equals("") ? new Decimal() : rs["Desconto"]);
                documento = rs["Documento"].ToString();
                data_documento = (rs["Data_documento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_documento"].ToString()));
                caixa_documento = (rs["caixa_documento"] == null ? 0 : int.Parse(rs["caixa_documento"].ToString()));
                num_item = Funcoes.intTry(rs["num_item"].ToString());
                produzir = rs["produzir"].ToString().Equals("1");
                data_hora_produzir = Funcoes.dtTry(rs["data_hora_produzir"].ToString());
                agrupamento = rs["agrupamento"].ToString();
            }

            if (rs != null)
                rs.Close();
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                if (inserido)
                {
                    insert(conn, tran);

                }
                else
                {


                    String sql = "update  pedido_itens set " +
                                  "Qtde=" + Qtde.ToString().Replace(",", ".") +
                                  ",Embalagem=" + Embalagem.ToString().Replace(",", ".") +
                                  ",unitario=" + unitario.ToString().Replace(",", ".") +
                                  ",total=" + total.ToString().Replace(",", ".") +
                                  ",ean='" + ean + "'" +
                                  ",id=" + id.ToString().Replace(",", ".") +
                                  ",desconto = " + Desconto.ToString().Replace(",", ".") +
                                  ",documento ='" + documento + "'" +
                                  ",data_documento =" + (data_documento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_documento.ToString("yyyy-MM-dd") + "'") +
                                  ",caixa_documento =" + caixa_documento +
                                  ",obs='" + obs + "'" +
                                  ",num_item=" + num_item.ToString() +
                                  ",produzir =" + (produzir ? "1" : "0") +
                                  ",data_hora_produzir =" + (data_hora_produzir.Equals(DateTime.MinValue) ? "null" : "'" + data_hora_produzir.ToString("yyyy-MM-dd HH:mm") + "'") +
                                  ",agrupamento = '" + agrupamento + "'" +
                        "  where  Filial='" + Filial + "' and Pedido=" + Pedido.ToString() + " and PLU='" + PLU + "' AND ISNULL(pedido_itens.num_item, 0) = " + num_item.ToString();

                    Conexao.executarSql(sql, conn, tran);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AtualizaSaidaEstoque(SqlConnection conn, SqlTransaction tran)
        {
            //if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            //{
            try
            {
                //Desativado processo de atualização via tabela direta e substituíto pela função
                Funcoes.atualizaSaldoPLU(Filial, PLU, ((Qtde * Embalagem) * -1), conn, tran, DateTime.Today, "SO") ;
                Funcoes.atualizaSaldoPLUDia(Filial, PLU, ((Qtde * Embalagem) * -1), conn, tran, "SO", DateTime.Today);
                //String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) -" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                //Conexao.executarSql(SqlEstoque, conn, tran);
                //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                //Conexao.executarSql(SqlMercadoria, conn, tran);
            }
            catch (Exception err)
            {

                throw new Exception("Atualização estoque FUNCOES.ATUALIZASOLDOPLU() : Erro Detalhe:" + err.Message);
            }

            //}

        }
        public void CancelaSaidaEstoque(SqlConnection conn, SqlTransaction tran)
        {
            //if (naturezaOperacao != null && naturezaOperacao.Baixa_estoque)
            //{
            try
            {
                Funcoes.atualizaSaldoPLU(Filial, PLU, (Qtde * Embalagem), conn, tran, DateTime.Today, "SO", false);
                Funcoes.atualizaSaldoPLUDia(Filial, PLU, (Qtde * Embalagem), conn, tran, "SO", DateTime.Today);
                //String SqlEstoque = " update mercadoria_loja set  saldo_atual = (isnull(saldo_atual,0) +" + (Qtde * Embalagem).ToString().Replace(',', '.') + ") where plu='" + PLU + "' and filial='" + Filial + "'";
                //Conexao.executarSql(SqlEstoque, conn, tran);
                //String SqlMercadoria = " update mercadoria  set  saldo_atual =(select sum(isnull(saldo_atual,0))from mercadoria_loja b where b.plu='" + PLU + "') where plu='" + PLU + "'";
                //Conexao.executarSql(SqlMercadoria, conn, tran);
            }
            catch (Exception err)
            {

                throw new Exception("Atualização estoque FUNCOES.ATUALIZASOLDOPLU() , mercadoria : Erro Detalhe:" + err.Message);
            }

            //}

        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "delete from pedido_itens  where  Filial='" + Filial + "' and Pedido=" + Pedido.ToString() + " and PLU='" + PLU + "' AND ISNULL(pedido_itens.num_item, 0) = " + num_item.ToString();
            ; //colocar campo index
            Conexao.executarSql(sql, conn, tran);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                if (unitario != vUnitario)
                {
                    vUnitario = unitario;
                }


                String sql = " insert into pedido_itens(" +
                          "Filial," +
                          "Pedido," +
                          "PLU," +
                          "Qtde," +
                          "Tipo," +
                          "Embalagem," +
                          "unitario," +
                          "total," +
                          "ean," +
                          "id," +
                           "desconto" +
                           ",documento" +
                           ",data_documento" +
                           ",caixa_documento" +
                           ",obs" +
                           ",num_item" +
                           ",produzir" +
                           ",data_hora_produzir" +
                           ",agrupamento" +
                ")" +
                     " values (" +
                          "'" + Filial + "'" +
                          "," + Pedido +
                          "," + "'" + PLU + "'" +
                          "," + Qtde.ToString().Replace(",", ".") +
                          "," + Tipo.ToString() +
                          "," + Embalagem.ToString().Replace(",", ".") +
                          "," + unitario.ToString().Replace(",", ".") +
                          "," + total.ToString().Replace(",", ".") +
                          "," + "'" + ean + "'" +
                          "," + id.ToString().Replace(",", ".") +
                         "," + Desconto.ToString().Replace(",", ".") +
                         ",'" + documento + "'" +
                         "," + (data_documento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_documento.ToString("yyyy-MM-dd") + "'") +
                         "," + caixa_documento +
                         ",'" + obs + "'" +
                         "," + num_item.ToString() +
                         "," + (produzir ? "1" : "0") +
                         "," + (data_hora_produzir.Equals(DateTime.MinValue) ? "null" : "'" + data_hora_produzir.ToString("yyyy-MM-dd HH:mm") + "'") +
                         ", '" + agrupamento + "'" +
                          ");";

                Conexao.executarSql(sql, conn, tran);
                inserido = false;
            }
            catch (Exception err)
            {
                throw new Exception("Item :" + err.Message);
            }
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from pedido_itens";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                       Lblindex.Text = "1/" + gridPesquisa.PageCount;
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ pedido_itensDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      String sql = "";
                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " campoPesquisa1 like '" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";     
                          }
                         sql += "campoPesquisa2 = '" + txtPESQ2.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                         try
                         {
                            User usr = (User)Session["User"];
                            if (!sql.Equals(""))
                            {
                               tb = Conexao.GetTable(sqlGrid+" where "+sql, usr);
                             }
                             else
                             {
                               tb = Conexao.GetTable(sqlGrid, usr);
                              }
                               gridPesquisa.DataSource = tb;
                               gridPesquisa.DataBind();
                               lblPesquisaErro.Text = "";
                               Lblindex.Text = "1/" + gridPesquisa.PageCount;
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
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
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>pedido_itens</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" visible="false">           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>CAMPO DE PESQUISA 1</p>   
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox></asp:TextBox>  
                </td>  
                <td>  
                   <p>CAMPO DE PESQUISA 2</p>  
                   <asp:TextBox ID="txtPESQ2" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="9"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pedido" Text="Pedido" Visible="true" 
                    HeaderText="Pedido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="PLU" Text="PLU" Visible="true" 
                    HeaderText="PLU" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Qtde" Text="Qtde" Visible="true" 
                    HeaderText="Qtde" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Embalagem" Text="Embalagem" Visible="true" 
                    HeaderText="Embalagem" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="unitario" Text="unitario" Visible="true" 
                    HeaderText="unitario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="total" Text="total" Visible="true" 
                    HeaderText="total" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ean" Text="ean" Visible="true" 
                    HeaderText="ean" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  


                 </Columns> 
                 <EditRowStyle BackColor="#999999" /> 
                 <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /> 
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> 
                 <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" /> 
                 <SortedAscendingCellStyle BackColor="#E9E7E2" /> 
                 <SortedAscendingHeaderStyle BackColor="#506C8C" /> 
                 <SortedDescendingCellStyle BackColor="#FFFDF8" /> 
                  <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
           </asp:GridView>           
           <br />       
           <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>       
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static pedido_itensDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new pedido_itensDAO();
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                      }
                      else
                      {
                           if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                           {
                              try
                              {
                                   if (!IsPostBack)
                                   {
                                        String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                        status = "visualizar";
                                        obj = new pedido_itensDAO(index,usr);
                                        carregarDados();
                                    }
                                    if (status.Equals("visualizar"))
                                    {
                                         EnabledControls(conteudo, false);
                                         EnabledControls(cabecalho, false);
                                    }else{//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                                    }
                                }
                                catch (Exception err)
                                {
                                   lblError.Text = err.Message;                 
                                }
                           }
                       }
                    carregabtn(pnBtn);
                  }
                 
                 private void limparCampos(){
                    LimparCampos(cabecalho);          
                    LimparCampos(conteudo);             
                 }
                 
                 protected bool validaCamposObrigatorios() {
                    if (validaCampos(cabecalho) && validaCampos(conteudo))
                             return true;
                    else
                             return false;
                 }
                 
                 protected override bool campoObrigatorio(Control campo)
                 {// colocar os nomes dos campos obrigarios no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {// colocar os nomes dos campos Desabilitados no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 protected override void btnIncluir_Click(object sender, EventArgs e)
                 {
                    incluir(pnBtn);
                 }
                 
                 protected override void btnEditar_Click(object sender, EventArgs e)
                 {
                    editar(pnBtn);
                    EnabledControls(cabecalho, true);
                    EnabledControls(conteudo, true);
                 }
                  
                 protected override void btnPesquisar_Click(object sender, EventArgs e)
                 {
                 Response.Redirect("nomepaginapesquisa.aspx"); //colocar o endereco da tela de pesquisa
                 }
                  
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     pnConfima.Visible = true;
                  }
                  
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     try
                     {
                       if (validaCamposObrigatorios())
                       {
                  
                             carregarDadosObj();
                             obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                             lblError.Text = "Salvo com Sucesso";
                             lblError.ForeColor = System.Drawing.Color.Blue;
                             EnabledControls(cabecalho, false);
                             EnabledControls(conteudo, false);
                             visualizar(pnBtn);
                       }
                       else
                       {
                            lblError.Text = "Campo Obrigatorio n?o preenchido";
                            lblError.ForeColor = System.Drawing.Color.Red;
                        }
                     }
                     catch (Exception err)
                     {
                         lblError.Text = err.Message;
                         lblError.ForeColor = System.Drawing.Color.Red;
                     }
                  }
                  
                  protected override void btnCancelar_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("nomepaginapesquisa.aspx");//colocar endereco pagina de pesquisa
                  }
                  protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
                  {
                      switch (e.Item.Value)
                      {
                          case "tab1":
                          MultiView1.ActiveViewIndex = 0;
                          break;
                       }
                   }
                    //--Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtPedido.Text=string.Format("{0:0,0.00}",obj.Pedido);
                                         txtPLU.Text=obj.PLU.ToString();
                                         txtQtde.Text=string.Format("{0:0,0.00}",obj.Qtde);
                                         txtEmbalagem.Text=string.Format("{0:0,0.00}",obj.Embalagem);
                                         txtunitario.Text=string.Format("{0:0,0.00}",obj.unitario);
                                         txttotal.Text=string.Format("{0:0,0.00}",obj.total);
                                         txtean.Text=obj.ean.ToString();
                                         txtid.Text=string.Format("{0:0,0.00}",obj.id);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.Pedido=Decimal.Parse(txtPedido.Text);
                                         obj.PLU=txtPLU.Text;
                                         obj.Qtde=Decimal.Parse(txtQtde.Text);
                                         obj.Embalagem=Decimal.Parse(txtEmbalagem.Text);
                                         obj.unitario=Decimal.Parse(txtunitario.Text);
                                         obj.total=Decimal.Parse(txttotal.Text);
                                         obj.ean=txtean.Text;
                                         obj.id=Decimal.Parse(txtid.Text);
   }

                  
                  protected void lista_click(object sender, ImageClickEventArgs e)
                  {
                      ImageButton btn = (ImageButton)sender;
                      pnFundo.Visible = true;
                      chkLista.Items.Clear();
                      String sqlLista = "";
                  
                      switch (btn.ID)
                      {
                          case "idBotao":
                              sqlLista = "Query de pesquisa com no minimo 2campos";
                              lbllista.Text = "Pagamentos";
                              camporeceber = "txtPagamento";
                              break;
                      }
                      User usr = (User)Session["User"];
                      SqlDataReader lista = Conexao.consulta(sqlLista, usr);
                  
                      while (lista.Read())
                      {
                          ListItem item = new ListItem();
                          item.Value = lista[0].ToString();
                          item.Text = lista[1].ToString();
                          chkLista.Items.Add(item);
                       }
                  }
                  
                  protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      try
                      {
                          obj.excluir();
                          pnConfima.Visible = false;
                          lblError.Text = "Registro Excluido com sucesso";
                          limparCampos();
                          pesquisar(pnBtn);
                       }
                       catch (Exception err)
                        {
                               lblError.Text = "N?o foi possivel Excluir o registro error:" +err.Message;
                         }
                  }
                  
                  protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      pnConfima.Visible = false;
                  }
                  
                  protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
                      txt.Text = "";
                      for (int i = 0; i < chkLista.Items.Count; i++)
                      {
                          if (chkLista.Items[i].Selected)
                          {
                              txt.Text += chkLista.Items[i].Value;
                         }
                     }
                     pnFundo.Visible = false;
                  }
                  
                  protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      pnFundo.Visible = false;
                  }
                  
                  
                  
/*================================HTML Pagina Detalhes==========================================
<div class="cabMenu">                  
       <center> <h1>Detalhes do pedido_itens</h1></center>                  
</div>                  
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">                  
    </asp:Panel>                  
    <br />              
     <asp:Label ID="lblError" runat="server" Text="" ForeColor=Red></asp:Label>              
                  
    <div id="cabecalho" runat="server" class="frame" >               
     <!--Coloque aqui os campos do cabe?alho    -->         
        <table>              
              <tr>    
                  <td></td>
              </tr>    
        </table>          
    </div>              
<div class="opcoes">                  
    <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal"               
                 OnMenuItemClick="tabMenu_MenuItemClick" Visible="true" > 
                  
       <Items>              
           <asp:MenuItem Text="Primeira Tab" Value="tab1" />         
       </Items>             
       <StaticMenuStyle CssClass="tab" />              
       <StaticMenuItemStyle CssClass="item" />             
       <staticselectedstyle backcolor="Beige" ForeColor="#465c71" />            
    </asp:Menu>              
</div>                  
                  
<div id="conteudo" runat="server" class="conteudo" enableviewstate="false">                  
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">              
       <asp:View ID="view1" runat="server" >              
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Pedido</p>
                   <asp:TextBox ID="txtPedido" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>PLU</p>
                   <asp:TextBox ID="txtPLU" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Qtde</p>
                   <asp:TextBox ID="txtQtde" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Embalagem</p>
                   <asp:TextBox ID="txtEmbalagem" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>unitario</p>
                   <asp:TextBox ID="txtunitario" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>total</p>
                   <asp:TextBox ID="txttotal" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ean</p>
                   <asp:TextBox ID="txtean" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
       </asp:View>              
    </asp:MultiView>                
</div>                  
        <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible =false>          
              <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>           
                    <table class="frame">
                       <tr>
                           <td>          
                             <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"                    <td>       
                              Width="25px" onclick="btnConfirmaLista_Click"   />           
                              <asp:Label ID="Label4" runat="server" Text="Seleciona" ></asp:Label>          
                           </td>           
                           <td>          
                                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png" 
                                       Width="25px" onclick="btnCancelaLista_Click"  />                   
                                    <asp:Label ID="Label5" runat="server" Text="Cancela" ></asp:Label>     
                           </td>           
                       </tr>
                     </table>
                  
                      <asp:Panel ID="Panel1" runat="server" CssClass="lista" >   
                             <asp:RadioButtonList ID="chkLista" runat="server" Height=50 Width=200>
                             </asp:RadioButtonList>
                      </asp:Panel>
         </asp:Panel>      
                  
         <asp:Panel ID="pnConfima" runat="server" CssClass="fundo" Visible =false>         
           <asp:Label ID="Label1" runat="server" Text="Confirma Exclus?o" CssClass="cabMenu"></asp:Label>         
             <table class="frame">          
                  <tr>     
                      <td>             
                             <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png" 
                                     Width="25px" onclick="btnConfirmaExclusao_Click"  /> 
                                     <asp:Label ID="Label2" runat="server" Text="Confirma" ></asp:Label>
                      </td>
                      <td>
                                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png" 
                                     Width="25px" onclick="btnCancelaExclusao_Click"  /> 
                                     <asp:Label ID="Label3" runat="server" Text="Cancela" ></asp:Label>
                      </td>
                  </tr>
              </table>     
         </asp:Panel>         
*/

