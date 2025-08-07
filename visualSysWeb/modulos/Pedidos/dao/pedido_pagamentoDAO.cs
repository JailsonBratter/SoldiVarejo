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
    public class pedido_pagamentoDAO
    {
        public DateTime Vencimento { get; set; }


        public String Filial = "";
        public String Pedido = "";
        public String Cliente_Fornec = "";
        public int Tipo = 0;
        public String Tipo_pagamento = "";
        public Decimal Valor = 0;
        public DateTime emissao = new DateTime();
        public int ordem = 0;
        public bool excluido = false;
        public String centroCusto = "";
        public User usr = null;
        public natureza_operacaoDAO naturezaOperacao { get; set; }

        public pedido_pagamentoDAO(User usr)
        {
            this.usr = usr;
        }
        public pedido_pagamentoDAO(String Pedido, int Tipo, DateTime Vencimento, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  pedido_pagamento where pedido =" + Pedido + " and Tipo =" + Tipo + " and vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }


        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                Vencimento = (rs["Vencimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Vencimento"].ToString()));
                Filial = rs["Filial"].ToString();
                Pedido = rs["Pedido"].ToString();
                Cliente_Fornec = rs["Cliente_Fornec"].ToString();
                Tipo = (rs["Tipo"].ToString().Equals("") ? 0 : int.Parse(rs["Tipo"].ToString()));
                Tipo_pagamento = rs["Tipo_pagamento"].ToString();
                Valor = (Decimal)(rs["Valor"].ToString().Equals("") ? new Decimal() : rs["Valor"]);
            }

            if (rs != null)
                rs.Close();
        }

        public bool Equals(pedido_pagamentoDAO obj)
        {
            return (obj.Filial.Equals(Filial) && obj.Tipo.Equals(Tipo) && obj.Pedido.Equals(Pedido) && obj.Vencimento.Equals(Vencimento));
        }


        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Vencimento.ToString("dd/MM/yyyy"));
            item.Add(Filial.ToString());
            item.Add(Pedido.ToString());
            item.Add(Cliente_Fornec.ToString());
            item.Add(Tipo.ToString());
            item.Add(Tipo_pagamento.ToString());
            item.Add(Valor.ToString("N2"));

            return item;


        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            excluir(conn, tran);
            insert(conn, tran);
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
            String sql = "delete from pedido_pagamento   where pedido =" + Pedido + " and Tipo =" + Tipo + " and vencimento='" + Vencimento.ToString("yyyy-MM-dd") + "'";
            Conexao.executarSql(sql, conn, tran);
            return true;
        }



        public void lancaFinanceiro(SqlConnection conn, SqlTransaction tran)
        {
            //if (naturezaOperacao != null && naturezaOperacao.Gera_apagar_receber)
            //{
            bool aVista = Conexao.retornaUmValor("Select a_Vista from tipo_pagamento where tipo_pagamento ='" + Tipo_pagamento + "'", null).Equals("1");
            bool salvaFinanceiro = true;
            bool aVistaConcluido = false;
            if (aVista)
            {
                salvaFinanceiro = !Funcoes.valorParametro("PED_AVISTA_NAO_FINANCEIRO", usr).ToUpper().Equals("TRUE");
                aVistaConcluido = !Funcoes.valorParametro("PED_AVISTA_NAO_CONCLUIDO", usr).ToUpper().Equals("TRUE");
            }

            if (salvaFinanceiro)
            {
                conta_a_receberDAO pg = new conta_a_receberDAO(usr);
                pg.Documento = "P" + Pedido.Trim() + "-" + ordem.ToString().PadLeft(2, '0');
                pg.Codigo_Cliente = Cliente_Fornec;
                pg.Filial = Filial;
                pg.Codigo_Centro_Custo = centroCusto;
                pg.id_cc = Conexao.retornaUmValor("select id_cc from centro_custo where codigo_centro_custo='" + centroCusto + "'", usr);
                pg.Valor = Valor;
                pg.Desconto = 0;
                pg.Obs = "D.G.A Pedido Saida:" + Pedido;
                pg.Emissao = emissao;
                pg.Vencimento = Vencimento;
                pg.status = (aVista ? (aVistaConcluido? 2:1) : 1);
                pg.entrada = DateTime.Now;
                pg.usuario = usr.getNome();
                pg.documento_emitido = "0";
                pg.tipo_recebimento = Tipo_pagamento;
                pg.aVista = aVista;

                pg.salvar(true, conn, tran);
            }
            //}


        }

        public void cancelaFinanceiro(SqlConnection conn, SqlTransaction tran)
        {


            conta_a_receberDAO pg = new conta_a_receberDAO("P" + Pedido.Trim() + "-" + ordem.ToString().PadLeft(2, '0'), this.Cliente_Fornec, usr);

            if (pg.status == 2)
            {
                throw new Exception("O Pedido contem titulos a receber vinculados que já estão com o Status 'CONCLUIDO' estorne os Titulos antes de cancelar o Pedido! ");
            }

            pg.excluir(conn, tran);



        }



        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into pedido_pagamento (" +
                          "Vencimento," +
                          "Filial," +
                          "Pedido," +
                          "Cliente_Fornec," +
                          "Tipo," +
                          "Tipo_pagamento," +
                          "Valor" +
                     " )values (" +
                          (Vencimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Vencimento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Filial + "'" +
                          "," + "'" + Pedido + "'" +
                          "," + "'" + Cliente_Fornec + "'" +
                          "," + Tipo.ToString() +
                          "," + "'" + Tipo_pagamento + "'" +
                          "," + Valor.ToString().Replace(",", ".") +
                         ");";

                Conexao.executarSql(sql, conn, tran);



            }
            catch (Exception err)
            {
                throw new Exception("Pagamento:" + err.Message);
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
                  static String sqlGrid = ""select * from pedido_pagamento";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ pedido_pagamentoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>pedido_pagamento</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="7"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Vencimento" Text="Vencimento" Visible="true" 
                    HeaderText="Vencimento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pedido" Text="Pedido" Visible="true" 
                    HeaderText="Pedido" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cliente_Fornec" Text="Cliente_Fornec" Visible="true" 
                    HeaderText="Cliente_Fornec" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo_pagamento" Text="Tipo_pagamento" Visible="true" 
                    HeaderText="Tipo_pagamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Valor" Text="Valor" Visible="true" 
                    HeaderText="Valor" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static pedido_pagamentoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new pedido_pagamentoDAO();
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
                                        obj = new pedido_pagamentoDAO(index,usr);
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
                 }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                 
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
                                         txtVencimento.Text=obj.VencimentoBr();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtPedido.Text=obj.Pedido.ToString();
                                         txtCliente_Fornec.Text=obj.Cliente_Fornec.ToString();
                                         chkTipo.Checked =obj.Tipo;
                                         txtTipo_pagamento.Text=obj.Tipo_pagamento.ToString();
                                         txtValor.Text=string.Format("{0:0,0.00}",obj.Valor);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Vencimento=DateTime.Parse(txtVencimento.Text);
                                         obj.Filial=txtFilial.Text;
                                         obj.Pedido=txtPedido.Text;
                                         obj.Cliente_Fornec=txtCliente_Fornec.Text;
                                         obj.Tipo=chkTipo.Checked ;
                                         obj.Tipo_pagamento=txtTipo_pagamento.Text;
                                         obj.Valor=Decimal.Parse(txtValor.Text);
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
       <center> <h1>Detalhes do pedido_pagamento</h1></center>                  
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
                                      <td >                   <p>Vencimento</p>
                   <asp:TextBox ID="txtVencimento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Pedido</p>
                   <asp:TextBox ID="txtPedido" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cliente_Fornec</p>
                   <asp:TextBox ID="txtCliente_Fornec" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <td><asp:CheckBox ID="chkTipo" runat="server" Text="Tipo"/>
                   </td>

                                      <td >                   <p>Tipo_pagamento</p>
                   <asp:TextBox ID="txtTipo_pagamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Valor</p>
                   <asp:TextBox ID="txtValor" runat="server" ></asp:TextBox>
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

