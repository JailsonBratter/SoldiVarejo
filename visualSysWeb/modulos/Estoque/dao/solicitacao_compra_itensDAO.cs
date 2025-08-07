using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class solicitacao_compra_itensDAO
    {
        public String filial = "";
        public String codigo = "";
        public String plu = "";
        public String ean = "";
        public String ref_fornecedor = "";
        public String descricao = "";
        private String vCtr = "-";
        public String id_contrato = "";
        public String CTR 
        {
            get
            {

                if (vCtr.Equals("-"))
                {
                    String codContrato = "";
                    codContrato = Conexao.retornaUmValor("Select CF.id_contrato from Contrato_fornecedor_item as cfi " +
                    "	INNER JOIN Contrato_fornecedor AS  CF ON CFI.id_contrato =CF.id_contrato " +
                    "  inner join  Contrato_Fornecedor_Filial cff  on cfi.id_contrato = cff.id_contrato  " +
                    " where cff.Filial ='"+usr.getFilial()+"' AND CF.data_validade >= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND CFI.plu='"+plu+"'", usr);
                    if (!codContrato.Trim().Equals("") )
                    {
                        id_contrato = codContrato;
                        vCtr = "C";
                    }
                    else
                    {
                        id_contrato = "";
                        vCtr = "";
                    }
                }

                return vCtr;
            }
            set
            {
                vCtr = value;
            }
        }
            
            
        public String und = "";
        public Decimal embalagem = 0;
        public Decimal saldo = 0;
        public Decimal preco_custo = 0;
        public Decimal preco_venda = 0;
        public Decimal mes_5 = 0;
        public Decimal mes_4 = 0;
        public Decimal mes_3 = 0;
        public Decimal mes_2 = 0;
        public Decimal ult_30 = 0;
        public Decimal vda_med = 0;
        public Decimal cob_cad = 0;
        public Decimal cob_dias = 0;
        public Decimal sugestao = 0;
        public Decimal qtde_comprar = 0;
        public bool gerado = false;
        public int ordem = 0;
        public bool aceita_sug = true;
        private User usr = null;

        public solicitacao_compra_itensDAO(User usr) {
            this.usr = usr;
        }
        public solicitacao_compra_itensDAO(String codigo, String plu, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  solicitacao_compra_itens where codigo ='" + codigo + "' and plu='" + plu + "' and filial='" + usr.getFilial() + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
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

        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(filial.ToString());
            item.Add(codigo.Trim());
            item.Add(plu.Trim());
            item.Add(ean.Trim());
            item.Add(ref_fornecedor.Trim());
            item.Add(descricao.Trim());
            item.Add(CTR);
            item.Add(id_contrato);
            item.Add(und);

            item.Add(embalagem.ToString());
            item.Add(saldo.ToString());
            item.Add(preco_custo.ToString("N2"));
            item.Add(preco_venda.ToString("N2"));
            item.Add(mes_5.ToString());
            item.Add(mes_4.ToString());
            item.Add(mes_3.ToString());
            item.Add(mes_2.ToString());
            item.Add(ult_30.ToString());
            item.Add(vda_med.ToString());
            item.Add(cob_cad.ToString());
            item.Add(cob_dias.ToString());
            item.Add(sugestao.ToString());
            item.Add(qtde_comprar.ToString());
            item.Add(ordem.ToString());
            item.Add(aceita_sug ? "1" : "0");
            return item;

        }


        public void carregarDados(SqlDataReader rs)
        {
            try
            {
                if (rs.Read())
                {
                    filial = rs["filial"].ToString();
                    codigo = rs["codigo"].ToString();
                    plu = rs["plu"].ToString();
                    ean = rs["ean"].ToString();
                    ref_fornecedor = rs["ref_fornecedor"].ToString();
                    descricao = rs["descricao"].ToString();
                    
                    und = rs["und"].ToString();
                    embalagem = (Decimal)(rs["embalagem"].ToString().Equals("") ? new Decimal() : rs["embalagem"]);
                    saldo = (Decimal)(rs["saldo"].ToString().Equals("") ? new Decimal() : rs["saldo"]);
                    preco_custo = (Decimal)(rs["preco_custo"].ToString().Equals("") ? new Decimal() : rs["preco_custo"]);
                    preco_venda = (Decimal)(rs["preco_venda"].ToString().Equals("") ? new Decimal() : rs["preco_venda"]);
                    mes_5 = (Decimal)(rs["mes_5"].ToString().Equals("") ? new Decimal() : rs["mes_1"]);
                    mes_4 = (Decimal)(rs["mes_4"].ToString().Equals("") ? new Decimal() : rs["mes_2"]);
                    mes_3 = (Decimal)(rs["mes_3"].ToString().Equals("") ? new Decimal() : rs["mes_3"]);
                    mes_2 = (Decimal)(rs["mes_2"].ToString().Equals("") ? new Decimal() : rs["mes_4"]);
                    ult_30 = (Decimal)(rs["ult_30"].ToString().Equals("") ? new Decimal() : rs["ult_30"]);
                    vda_med = (Decimal)(rs["vda_med"].ToString().Equals("") ? new Decimal() : rs["vda_med"]);
                    cob_cad = (Decimal)(rs["cob_cad"].ToString().Equals("") ? new Decimal() : rs["cob_cad"]);
                    cob_dias = (Decimal)(rs["cob_dias"].ToString().Equals("") ? new Decimal() : rs["cob_dias"]);
                    sugestao = (Decimal)(rs["sugestao"].ToString().Equals("") ? new Decimal() : rs["sugestao"]);
                    qtde_comprar = (Decimal)(rs["qtde_comprar"].ToString().Equals("") ? new Decimal() : rs["qtde_comprar"]);
                    aceita_sug = rs["aceita_sug"].ToString().Equals("1");
                    int.TryParse(rs["ordem"].ToString(), out ordem);
                }
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
        private void update(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = "update  solicitacao_compra_itens set " +
                              "filial='" + filial + "'" +
                              ",codigo='" + codigo + "'" +
                              ",plu='" + plu + "'" +
                              ",ean='" + ean + "'" +
                              ",ref_fornecedor='" + ref_fornecedor + "'" +
                              ",descricao='" + descricao + "'" +
                              "ctr='"+CTR+"'"+
                              "und='"+und+"'"+
                              ",embalagem=" + embalagem.ToString().Replace(",", ".") +
                              ",saldo=" + saldo.ToString().Replace(",", ".") +
                              ",preco_custo=" + preco_custo.ToString().Replace(",", ".") +
                              ",preco_venda=" + preco_venda.ToString().Replace(",", ".") +
                              ",mes_5=" + mes_5.ToString().Replace(",", ".") +
                              ",mes_4=" + mes_4.ToString().Replace(",", ".") +
                              ",mes_3=" + mes_3.ToString().Replace(",", ".") +
                              ",mes_2=" + mes_2.ToString().Replace(",", ".") +
                              ",ult_30=" + ult_30.ToString().Replace(",", ".") +
                              ",vda_med=" + vda_med.ToString().Replace(",", ".") +
                              ",cob_cad=" + cob_cad.ToString().Replace(",", ".") +
                              ",cob_dias=" + cob_dias.ToString().Replace(",", ".") +
                              ",sugestao=" + sugestao.ToString().Replace(",", ".") +
                              ",qtde_comprar=" + qtde_comprar.ToString().Replace(",", ".") +
                              ",ordem="+ordem+
                              ",aceita_sug="+(aceita_sug?"1":"0")+
                    "  where codigo ='" + codigo + "' and plu='" + plu + "' and filial='" + filial + "'"
                        ;
                Conexao.executarSql(sql,conn,trans);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction trans)
        {
            if (novo)
            {
                insert(conn,trans);
            }
            else
            {
                update(conn,trans);
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from solicitacao_compra_itens  where codigo ='" + codigo + "' and plu='" + plu + "' and filial='" + filial + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                String sql = " insert into solicitacao_compra_itens( " +
                          "filial," +
                          "codigo," +
                          "plu," +
                          "ean," +
                          "ref_fornecedor," +
                          "descricao," +
                          "ctr,"+
                          "und,"+
                          "embalagem," +
                          "saldo," +
                          "preco_custo," +
                          "preco_venda," +
                          "mes_5," +
                          "mes_4," +
                          "mes_3," +
                          "mes_2," +
                          "ult_30," +
                          "vda_med," +
                          "cob_cad," +
                          "cob_dias," +
                          "sugestao," +
                          "qtde_comprar" +
                          ",ordem"+
                          ",aceita_sug"+
                     ") values (" +
                          "'" + filial + "'" +
                          "," + "'" + codigo + "'" +
                          "," + "'" + plu + "'" +
                          "," + "'" + ean + "'" +
                          "," + "'" + ref_fornecedor + "'" +
                          "," + "'" + descricao + "'" +
                          ",'"+CTR+"'"+
                          ",'"+und+"'"+
                          "," + embalagem.ToString().Replace(",", ".") +
                          "," + saldo.ToString().Replace(",", ".") +
                          "," + preco_custo.ToString().Replace(",", ".") +
                          "," + preco_venda.ToString().Replace(",", ".") +
                          "," + mes_5.ToString().Replace(",", ".") +
                          "," + mes_4.ToString().Replace(",", ".") +
                          "," + mes_3.ToString().Replace(",", ".") +
                          "," + mes_2.ToString().Replace(",", ".") +
                          "," + ult_30.ToString().Replace(",", ".") +
                          "," + vda_med.ToString().Replace(",", ".") +
                          "," + cob_cad.ToString().Replace(",", ".") +
                          "," + cob_dias.ToString().Replace(",", ".") +
                          "," + sugestao.ToString().Replace(",", ".") +
                          "," + qtde_comprar.ToString().Replace(",", ".") +
                          ","+ordem+
                          ","+(aceita_sug?"1":"0")+
                         ");";

                Conexao.executarSql(sql,conn,trans);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
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
                  static String sqlGrid = ""select * from solicitacao_compra_itens";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ solicitacao_compra_itensDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                        }catch (Exception err)
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
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>solicitacao_compra_itens</h1></center>
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
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo" Text="codigo" Visible="true" 
                    HeaderText="codigo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="plu" Text="plu" Visible="true" 
                    HeaderText="plu" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ean" Text="ean" Visible="true" 
                    HeaderText="ean" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ref_fornecedor" Text="ref_fornecedor" Visible="true" 
                    HeaderText="ref_fornecedor" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao" Text="descricao" Visible="true" 
                    HeaderText="descricao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="embalagem" Text="embalagem" Visible="true" 
                    HeaderText="embalagem" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="saldo" Text="saldo" Visible="true" 
                    HeaderText="saldo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="preco_custo" Text="preco_custo" Visible="true" 
                    HeaderText="preco_custo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="preco_venda" Text="preco_venda" Visible="true" 
                    HeaderText="preco_venda" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="mes_1" Text="mes_1" Visible="true" 
                    HeaderText="mes_1" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="mes_2" Text="mes_2" Visible="true" 
                    HeaderText="mes_2" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="mes_3" Text="mes_3" Visible="true" 
                    HeaderText="mes_3" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="mes_4" Text="mes_4" Visible="true" 
                    HeaderText="mes_4" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ult_30" Text="ult_30" Visible="true" 
                    HeaderText="ult_30" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="vda_med" Text="vda_med" Visible="true" 
                    HeaderText="vda_med" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cob_cad" Text="cob_cad" Visible="true" 
                    HeaderText="cob_cad" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cob_dias" Text="cob_dias" Visible="true" 
                    HeaderText="cob_dias" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sugestao" Text="sugestao" Visible="true" 
                    HeaderText="sugestao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="qtde_comprar" Text="qtde_comprar" Visible="true" 
                    HeaderText="qtde_comprar" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/solicitacao_compra_itensDetalhes.aspx?campoIndex={0}" 
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
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static solicitacao_compra_itensDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new solicitacao_compra_itensDAO();
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
                                        obj = new solicitacao_compra_itensDAO(index,usr);
                                        carregarDados();
                                    }
                                    if (status.Equals("visualizar"))
                                    {
                                         EnabledControls(conteudo, false);
                                         EnabledControls(cabecalho, false);
                                    }else{
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
                                         txtfilial.Text=obj.filial.ToString();
                                         txtcodigo.Text=obj.codigo.ToString();
                                         txtplu.Text=obj.plu.ToString();
                                         txtean.Text=obj.ean.ToString();
                                         txtref_fornecedor.Text=obj.ref_fornecedor.ToString();
                                         txtdescricao.Text=obj.descricao.ToString();
                                         txtembalagem.Text=string.Format("{0:0,0.00}",obj.embalagem);
                                         txtsaldo.Text=string.Format("{0:0,0.00}",obj.saldo);
                                         txtpreco_custo.Text=string.Format("{0:0,0.00}",obj.preco_custo);
                                         txtpreco_venda.Text=string.Format("{0:0,0.00}",obj.preco_venda);
                                         txtmes_1.Text=string.Format("{0:0,0.00}",obj.mes_1);
                                         txtmes_2.Text=string.Format("{0:0,0.00}",obj.mes_2);
                                         txtmes_3.Text=string.Format("{0:0,0.00}",obj.mes_3);
                                         txtmes_4.Text=string.Format("{0:0,0.00}",obj.mes_4);
                                         txtult_30.Text=string.Format("{0:0,0.00}",obj.ult_30);
                                         txtvda_med.Text=string.Format("{0:0,0.00}",obj.vda_med);
                                         txtcob_cad.Text=string.Format("{0:0,0.00}",obj.cob_cad);
                                         txtcob_dias.Text=string.Format("{0:0,0.00}",obj.cob_dias);
                                         txtsugestao.Text=string.Format("{0:0,0.00}",obj.sugestao);
                                         txtqtde_comprar.Text=string.Format("{0:0,0.00}",obj.qtde_comprar);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.filial=txtfilial.Text;
                                         obj.codigo=txtcodigo.Text;
                                         obj.plu=txtplu.Text;
                                         obj.ean=txtean.Text;
                                         obj.ref_fornecedor=txtref_fornecedor.Text;
                                         obj.descricao=txtdescricao.Text;
                                         obj.embalagem=Decimal.Parse(txtembalagem.Text);
                                         obj.saldo=Decimal.Parse(txtsaldo.Text);
                                         obj.preco_custo=Decimal.Parse(txtpreco_custo.Text);
                                         obj.preco_venda=Decimal.Parse(txtpreco_venda.Text);
                                         obj.mes_1=Decimal.Parse(txtmes_1.Text);
                                         obj.mes_2=Decimal.Parse(txtmes_2.Text);
                                         obj.mes_3=Decimal.Parse(txtmes_3.Text);
                                         obj.mes_4=Decimal.Parse(txtmes_4.Text);
                                         obj.ult_30=Decimal.Parse(txtult_30.Text);
                                         obj.vda_med=Decimal.Parse(txtvda_med.Text);
                                         obj.cob_cad=Decimal.Parse(txtcob_cad.Text);
                                         obj.cob_dias=Decimal.Parse(txtcob_dias.Text);
                                         obj.sugestao=Decimal.Parse(txtsugestao.Text);
                                         obj.qtde_comprar=Decimal.Parse(txtqtde_comprar.Text);
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
                       if (lista != null)
                          lista.Close();
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
       <center> <h1>Detalhes do solicitacao_compra_itens</h1></center>                  
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
                  
<div id="conteudo" runat="server" class="conteudo" >                  
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo</p>
                   <asp:TextBox ID="txtcodigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>plu</p>
                   <asp:TextBox ID="txtplu" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ean</p>
                   <asp:TextBox ID="txtean" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ref_fornecedor</p>
                   <asp:TextBox ID="txtref_fornecedor" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>descricao</p>
                   <asp:TextBox ID="txtdescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>embalagem</p>
                   <asp:TextBox ID="txtembalagem" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>saldo</p>
                   <asp:TextBox ID="txtsaldo" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>preco_custo</p>
                   <asp:TextBox ID="txtpreco_custo" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>preco_venda</p>
                   <asp:TextBox ID="txtpreco_venda" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>mes_1</p>
                   <asp:TextBox ID="txtmes_1" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>mes_2</p>
                   <asp:TextBox ID="txtmes_2" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>mes_3</p>
                   <asp:TextBox ID="txtmes_3" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>mes_4</p>
                   <asp:TextBox ID="txtmes_4" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ult_30</p>
                   <asp:TextBox ID="txtult_30" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>vda_med</p>
                   <asp:TextBox ID="txtvda_med" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>cob_cad</p>
                   <asp:TextBox ID="txtcob_cad" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>cob_dias</p>
                   <asp:TextBox ID="txtcob_dias" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sugestao</p>
                   <asp:TextBox ID="txtsugestao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>qtde_comprar</p>
                   <asp:TextBox ID="txtqtde_comprar" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
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

