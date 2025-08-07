using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class cartaoDAO
    {
        public int nro_Finalizadora { get; set; }
        public String filial { get; set; }
        public String id_cartao { get; set; }
        public int dias { get; set; }
        public Decimal taxa { get; set; }
        public int data { get; set; }
        public String centro_custo { get; set; }
        public String despesa_cc { get; set; }
        public String fornecedor { get; set; }
        public String ajuste_cc { get; set; }
        public String bandeira { get; set; }
        public Decimal acrecimo { get; set; }
        public String id_Bandeira { get; set; }
        public String id_Rede { get; set; }
        public cartaoDAO(User usr) {
            this.filial = usr.getFilial();
        }
        public cartaoDAO(String nro_Finalizadora,String id_cartao, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  cartao  where    nro_Finalizadora=" + nro_Finalizadora + " and id_cartao='" + id_cartao + "'";
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
        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                nro_Finalizadora = (rs["nro_Finalizadora"] == null ? 0 : int.Parse(rs["nro_Finalizadora"].ToString()));
                filial = rs["filial"].ToString();
                id_cartao = rs["id_cartao"].ToString();
                dias = (rs["dias"] == null ? 0 : int.Parse(rs["dias"].ToString()));
                taxa = (Decimal)(rs["taxa"].ToString().Equals("") ? new Decimal() : rs["taxa"]);
                data = (rs["data"] == null ? 0 : int.Parse(rs["data"].ToString()));
                centro_custo = rs["centro_custo"].ToString();
                despesa_cc = rs["despesa_cc"].ToString();
                fornecedor = rs["fornecedor"].ToString();
                ajuste_cc = rs["ajuste_cc"].ToString();
                bandeira = rs["bandeira"].ToString();
                acrecimo = (Decimal)(rs["acrecimo"].ToString().Equals("") ? new Decimal() : rs["acrecimo"]);
                id_Bandeira = rs["id_Bandeira"].ToString();
                id_Rede = rs["id_Rede"].ToString();
            }
            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                String sql = "update  cartao set " +

                              "filial='" + filial + "'" +
                              ",dias=" + dias +
                              ",taxa=" + taxa.ToString().Replace(",", ".") +
                              ",data=" + data +
                              ",centro_custo='" + centro_custo + "'" +
                              ",despesa_cc='" + despesa_cc + "'" +
                              ",fornecedor='" + fornecedor + "'" +
                              ",ajuste_cc='" + ajuste_cc + "'" +
                              ",bandeira='" + bandeira + "'" +
                              ",acrecimo=" + acrecimo.ToString().Replace(",", ".") +
                              ",id_Bandeira='" + id_Bandeira + "'" +
                              ",id_Rede='" + id_Rede + "'" +
                    "  where    nro_Finalizadora=" + nro_Finalizadora + " and id_cartao='" + id_cartao + "'";
                ;
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            if (novo)
            {
                insert();
            }
            else
            {
                update();
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from cartao    where    nro_Finalizadora=" + nro_Finalizadora + " and id_cartao='" + id_cartao + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into cartao " +
                          "(nro_Finalizadora," +
                          "filial," +
                          "id_cartao," +
                          "dias," +
                          "taxa," +
                          "data," +
                          "centro_custo," +
                          "despesa_cc," +
                          "fornecedor," +
                          "ajuste_cc," +
                          "bandeira," +
                          "acrecimo," +
                          "id_Bandeira," +
                          "id_Rede" +
                     " )values (" +
                          nro_Finalizadora +
                          "," + "'" + filial + "'" +
                          "," + "'" + id_cartao + "'" +
                          "," + dias +
                          "," + taxa.ToString().Replace(",", ".") +
                          "," + data +
                          "," + "'" + centro_custo + "'" +
                          "," + "'" + despesa_cc + "'" +
                          "," + "'" + fornecedor + "'" +
                          "," + "'" + ajuste_cc + "'" +
                          "," + "'" + bandeira + "'" +
                          "," + acrecimo.ToString().Replace(",", ".") +
                          "," + "'" + id_Bandeira + "'" +
                          "," + "'" + id_Rede + "'" +
                         ");";

                Conexao.executarSql(sql);
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
                  static String sqlGrid = ""select * from cartao";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ cartaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>cartao</h1></center>
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
                    <asp:HyperLinkField DataTextField="nro_Finalizadora" Text="nro_Finalizadora" Visible="true" 
                    HeaderText="nro_Finalizadora" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_cartao" Text="id_cartao" Visible="true" 
                    HeaderText="id_cartao" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="dias" Text="dias" Visible="true" 
                    HeaderText="dias" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="taxa" Text="taxa" Visible="true" 
                    HeaderText="taxa" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data" Text="data" Visible="true" 
                    HeaderText="data" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="centro_custo" Text="centro_custo" Visible="true" 
                    HeaderText="centro_custo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="despesa_cc" Text="despesa_cc" Visible="true" 
                    HeaderText="despesa_cc" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="fornecedor" Text="fornecedor" Visible="true" 
                    HeaderText="fornecedor" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ajuste_cc" Text="ajuste_cc" Visible="true" 
                    HeaderText="ajuste_cc" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="bandeira" Text="bandeira" Visible="true" 
                    HeaderText="bandeira" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="acrecimo" Text="acrecimo" Visible="true" 
                    HeaderText="acrecimo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_Bandeira" Text="id_Bandeira" Visible="true" 
                    HeaderText="id_Bandeira" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_Rede" Text="id_Rede" Visible="true" 
                    HeaderText="id_Rede" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cartaoDetalhes.aspx?campoIndex={0}" 
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
                 protected static cartaoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new cartaoDAO();
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
                                        obj = new cartaoDAO(index,usr);
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
                                         txtnro_Finalizadora.Text=obj.nro_Finalizadora.ToString();
                                         txtfilial.Text=obj.filial.ToString();
                                         txtid_cartao.Text=obj.id_cartao.ToString();
                                         txtdias.Text=obj.dias.ToString();
                                         txttaxa.Text=string.Format("{0:0,0.00}",obj.taxa);
                                         txtdata.Text=obj.data.ToString();
                                         txtcentro_custo.Text=obj.centro_custo.ToString();
                                         txtdespesa_cc.Text=obj.despesa_cc.ToString();
                                         txtfornecedor.Text=obj.fornecedor.ToString();
                                         txtajuste_cc.Text=obj.ajuste_cc.ToString();
                                         txtbandeira.Text=obj.bandeira.ToString();
                                         txtacrecimo.Text=string.Format("{0:0,0.00}",obj.acrecimo);
                                         txtid_Bandeira.Text=obj.id_Bandeira.ToString();
                                         txtid_Rede.Text=obj.id_Rede.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.nro_Finalizadora=int.Parse(txtnro_Finalizadora.Text);
                                         obj.filial=txtfilial.Text;
                                         obj.id_cartao=txtid_cartao.Text;
                                         obj.dias=int.Parse(txtdias.Text);
                                         obj.taxa=Decimal.Parse(txttaxa.Text);
                                         obj.data=int.Parse(txtdata.Text);
                                         obj.centro_custo=txtcentro_custo.Text;
                                         obj.despesa_cc=txtdespesa_cc.Text;
                                         obj.fornecedor=txtfornecedor.Text;
                                         obj.ajuste_cc=txtajuste_cc.Text;
                                         obj.bandeira=txtbandeira.Text;
                                         obj.acrecimo=(txtacrecimo.Text.Equals("")?0:Decimal.Parse(txtacrecimo.Text));
                                         obj.id_Bandeira=txtid_Bandeira.Text;
                                         obj.id_Rede=txtid_Rede.Text;
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
       <center> <h1>Detalhes do cartao</h1></center>                  
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
                                      <td >                   <p>nro_Finalizadora</p>
                   <asp:TextBox ID="txtnro_Finalizadora" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id_cartao</p>
                   <asp:TextBox ID="txtid_cartao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>dias</p>
                   <asp:TextBox ID="txtdias" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>taxa</p>
                   <asp:TextBox ID="txttaxa" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data</p>
                   <asp:TextBox ID="txtdata" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>centro_custo</p>
                   <asp:TextBox ID="txtcentro_custo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>despesa_cc</p>
                   <asp:TextBox ID="txtdespesa_cc" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>fornecedor</p>
                   <asp:TextBox ID="txtfornecedor" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ajuste_cc</p>
                   <asp:TextBox ID="txtajuste_cc" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>bandeira</p>
                   <asp:TextBox ID="txtbandeira" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>acrecimo</p>
                   <asp:TextBox ID="txtacrecimo" runat="server" CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id_Bandeira</p>
                   <asp:TextBox ID="txtid_Bandeira" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id_Rede</p>
                   <asp:TextBox ID="txtid_Rede" runat="server" ></asp:TextBox>
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

