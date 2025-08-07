using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.dao
{
    public class cliente_petDAO
    {
        public String Nome_Pet = "";
        public String Sexo = "";
        public String Codigo_Cliente = "";
        public String Cor = "";
        public DateTime Data_Nascimento = new DateTime();
        public String Data_NascimentoBr()
        {
            return dataBr(Data_Nascimento);
        }

        public String Pelagem = "";
        public String Porte = "";
        public String Raca = "";
        public DateTime Ultimo_Cio = new DateTime();
        public String Ultimo_CioBr()
        {
            return dataBr(Ultimo_Cio);
        }

        public String Especie = "";
        public String pedigree = "";
        public String codigo_pet = "";
        public int index = 0;
        public User usr = null;
        public ArrayList arrVacinas = new ArrayList();
        public List<ClienteObservacao_veterinarioDAO> ObservacoesVet = new List<ClienteObservacao_veterinarioDAO>();
        public List<ClientePetImagens> Imagens = new List<ClientePetImagens>();
        public string statusObs = "";

        public cliente_petDAO(User usr)
        {
            this.usr = usr;
        }
        public cliente_petDAO(String codigo_pet, String codigo_cliente, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  cliente_pet where codigo_pet ='" + codigo_pet + "' and codigo_cliente='" + codigo_cliente + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }




        public void addVacina(Cliente_Pet_VacinaDAO item)
        {
            item.codigo_pet = this.codigo_pet;
            item.Codigo_Cliente = this.Codigo_Cliente;
            item.Nome_Pet = this.Nome_Pet;
            arrVacinas.Add(item);

        }

        public DataTable tbVacinas()
        {
            ArrayList itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            cabecalho.Add("Codigo_pet");
            cabecalho.Add("Nome_pet");
            cabecalho.Add("Codigo_cliente");
            cabecalho.Add("Vacina");
            cabecalho.Add("Data_Ultima_Vacina");

            itens.Add(cabecalho);
            if (arrVacinas != null && arrVacinas.Count > 0)
            {
                foreach (Cliente_Pet_VacinaDAO item in arrVacinas)
                {
                    itens.Add(item.ArrToString());

                }
            }
            return Conexao.GetArryTable(itens);
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
            item.Add(codigo_pet.ToString());
            item.Add(Nome_Pet.ToString());
            item.Add(Sexo.ToString());
            item.Add(Codigo_Cliente.ToString());
            item.Add(Cor.ToString());
            item.Add(Data_NascimentoBr());
            item.Add(Pelagem.ToString());
            item.Add(Porte.ToString());
            item.Add(Raca.ToString());
            item.Add(Ultimo_CioBr());
            item.Add(Especie.ToString());
            item.Add(pedigree.ToString());
            return item;
        }

        public void carregarDados(SqlDataReader rs)
        {
            try
            {


                if (rs.Read())
                {
                    Nome_Pet = rs["Nome_Pet"].ToString();
                    Sexo = rs["Sexo"].ToString();
                    Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                    Cor = rs["Cor"].ToString();
                    Data_Nascimento = (rs["Data_Nascimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Nascimento"].ToString()));
                    Pelagem = rs["Pelagem"].ToString();
                    Porte = rs["Porte"].ToString();
                    Raca = rs["Raca"].ToString();
                    Ultimo_Cio = (rs["Ultimo_Cio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Ultimo_Cio"].ToString()));
                    Especie = rs["Especie"].ToString();
                    pedigree = rs["pedigree"].ToString();
                    codigo_pet = rs["codigo_pet"].ToString();
                    carregarVacinas();
                    carregarObservacoes();
                    carregarImagens();
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

        public void carregarImagens()
        {

            String strSql = "Select * from cliente_pet_Imagem where codigo_pet='" + this.codigo_pet + "' and codigo_cliente='" + Codigo_Cliente + "' ";
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(strSql, usr, false);
                Imagens.Clear();
                while (rs.Read())
                {
                    ClientePetImagens img = new ClientePetImagens()
                    {
                        Codigo_Cliente = this.Codigo_Cliente,
                        Codigo_pet = this.codigo_pet,
                        Imagem = rs["Imagem"].ToString(),
                        url = rs["urlstr"].ToString()
                    };
                    Imagens.Add(img);
                }
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
           
        }

        public void carregarVacinas()
        {
            String strSql = "Select * from cliente_pet_vacina where codigo_pet='" + this.codigo_pet + "' and codigo_cliente='" + Codigo_Cliente + "' order by convert(varchar,data_ultima_vacinacao,102) ";
            SqlDataReader rs = Conexao.consulta(strSql, usr, false);
            arrVacinas.Clear();
            while (rs.Read())
            {
                Cliente_Pet_VacinaDAO vacina = new Cliente_Pet_VacinaDAO();
                vacina.Codigo_Cliente = this.Codigo_Cliente;
                vacina.codigo_pet = this.codigo_pet;
                vacina.Nome_Pet = rs["nome_pet"].ToString();
                vacina.Vacina = rs["vacina"].ToString();
                DateTime.TryParse(rs["data_ultima_vacinacao"].ToString(), out vacina.Data_ultima_vacinacao);
                arrVacinas.Add(vacina);
            }
        }
        public void carregarObservacoes()
        {
            String sql = "Select * from Observacao_Veterinario where nome_pet ='" + Nome_Pet + "' and codigo_cliente ='" + Codigo_Cliente + "' Order by Data ";
            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, usr, false);
                ObservacoesVet.Clear();
                while (rs.Read())
                {
                    ClienteObservacao_veterinarioDAO obs = new ClienteObservacao_veterinarioDAO();
                    obs.Codigo_cliente = Codigo_Cliente;
                    obs.Nome_pet = Nome_Pet;
                    obs.Data = Funcoes.dtTry(rs["data"].ToString());
                    obs.Usuario = rs["usuario"].ToString();
                    obs.Observacao = rs["Observacao"].ToString();
                    obs.Filial = usr.getFilial();
                    ObservacoesVet.Add(obs);

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

        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  cliente_pet set " +
                              "Nome_Pet='" + Nome_Pet + "'" +
                              ",Sexo='" + Sexo + "'" +
                              ",Cor='" + Cor + "'" +
                              ",Data_Nascimento=" + (Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Nascimento.ToString("yyyy-MM-dd") + "'") + "," +
                              ",Pelagem='" + Pelagem + "'" +
                              ",Porte='" + Porte + "'" +
                              ",Raca='" + Raca + "'" +
                              ",Ultimo_Cio=" + (Ultimo_Cio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Ultimo_Cio.ToString("yyyy-MM-dd") + "'") + "," +
                              ",Especie='" + Especie + "'" +
                              ",pedigree='" + pedigree + "'" +

                    "  where codigo_pet='" + codigo_pet + "' and Codigo_Cliente='" + Codigo_Cliente + "'";
                ;
                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
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

            Conexao.executarSql("delete from Cliente_pet_vacina where codigo_cliente ='" + this.Codigo_Cliente + "' and codigo_pet ='" + this.codigo_pet + "'", conn, tran);
            foreach (Cliente_Pet_VacinaDAO item in arrVacinas)
            {
                item.codigo_pet = this.codigo_pet;
                item.Codigo_Cliente = this.Codigo_Cliente;
                item.salvar(true, conn, tran);
            }

            Conexao.executarSql("delete from Observacao_veterinario where codigo_cliente ='" + this.Codigo_Cliente + "' and nome_pet ='" + this.Nome_Pet + "'", conn, tran);
            foreach (ClienteObservacao_veterinarioDAO item in ObservacoesVet)
            {
                item.Nome_pet = this.Nome_Pet;
                item.Codigo_cliente = this.Codigo_Cliente;
                item.Filial = usr.getFilial();
                item.salvar(conn, tran);
            }

            Conexao.executarSql("delete from cliente_pet_Imagem where codigo_cliente ='" + this.Codigo_Cliente + "' and codigo_pet='"+this.codigo_pet+"'", conn, tran);
            foreach (ClientePetImagens item in Imagens)
            {
                item.Codigo_Cliente = this.Codigo_Cliente;
                item.Codigo_pet = this.codigo_pet;
                item.Salvar(conn,tran);

            }




            return true;
        }

        public bool excluir()
        {
            String sql = "delete from cliente_pet  where codigo_pet='" + codigo_pet + "' and Codigo_Cliente='" + Codigo_Cliente + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into cliente_pet (" +
                          "Nome_Pet," +
                          "Sexo," +
                          "Codigo_Cliente," +
                          "Cor," +
                          "Data_Nascimento," +
                          "Pelagem," +
                          "Porte," +
                          "Raca," +
                          "Ultimo_Cio," +
                          "Especie," +
                          "pedigree," +
                          "codigo_pet)" +
                     " values (" +
                          "'" + Nome_Pet + "'" +
                          "," + "'" + Sexo + "'" +
                          "," + "'" + Codigo_Cliente + "'" +
                          "," + "'" + Cor + "'" +
                          "," + (Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_Nascimento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Pelagem + "'" +
                          "," + "'" + Porte + "'" +
                          "," + "'" + Raca + "'" +
                          "," + (Ultimo_Cio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Ultimo_Cio.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Especie + "'" +
                          "," + "'" + pedigree + "'" +
                          "," + "'" + codigo_pet + "'" +
                         ");";

                Conexao.executarSql(sql, conn, tran);


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
                  static String sqlGrid = ""select * from cliente_pet";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ cliente_petDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>cliente_pet</h1></center>
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
                    <asp:HyperLinkField DataTextField="Nome_Pet" Text="Nome_Pet" Visible="true" 
                    HeaderText="Nome_Pet" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Sexo" Text="Sexo" Visible="true" 
                    HeaderText="Sexo" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="Codigo_Cliente" Visible="true" 
                    HeaderText="Codigo_Cliente" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cor" Text="Cor" Visible="true" 
                    HeaderText="Cor" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_Nascimento" Text="Data_Nascimento" Visible="true" 
                    HeaderText="Data_Nascimento" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pelagem" Text="Pelagem" Visible="true" 
                    HeaderText="Pelagem" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Porte" Text="Porte" Visible="true" 
                    HeaderText="Porte" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Raca" Text="Raca" Visible="true" 
                    HeaderText="Raca" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Ultimo_Cio" Text="Ultimo_Cio" Visible="true" 
                    HeaderText="Ultimo_Cio" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Especie" Text="Especie" Visible="true" 
                    HeaderText="Especie" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="pedigree" Text="pedigree" Visible="true" 
                    HeaderText="pedigree" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo_pet" Text="codigo_pet" Visible="true" 
                    HeaderText="codigo_pet" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/cliente_petDetalhes.aspx?campoIndex={0}" 
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
                 protected static cliente_petDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new cliente_petDAO();
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
                                        obj = new cliente_petDAO(index,usr);
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
                                         txtNome_Pet.Text=obj.Nome_Pet.ToString();
                                         txtSexo.Text=obj.Sexo.ToString();
                                         txtCodigo_Cliente.Text=obj.Codigo_Cliente.ToString();
                                         txtCor.Text=obj.Cor.ToString();
                                         txtData_Nascimento.Text=obj.Data_NascimentoBr();
                                         txtPelagem.Text=obj.Pelagem.ToString();
                                         txtPorte.Text=obj.Porte.ToString();
                                         txtRaca.Text=obj.Raca.ToString();
                                         txtUltimo_Cio.Text=obj.Ultimo_CioBr();
                                         txtEspecie.Text=obj.Especie.ToString();
                                         txtpedigree.Text=obj.pedigree.ToString();
                                         txtcodigo_pet.Text=obj.codigo_pet.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Nome_Pet=txtNome_Pet.Text;
                                         obj.Sexo=txtSexo.Text;
                                         obj.Codigo_Cliente=txtCodigo_Cliente.Text;
                                         obj.Cor=txtCor.Text;
                                         obj.Data_Nascimento=(txtData_Nascimento.Text.Equals("")?new DateTime():DateTime.Parse(txtData_Nascimento.Text));
                                         obj.Pelagem=txtPelagem.Text;
                                         obj.Porte=txtPorte.Text;
                                         obj.Raca=txtRaca.Text;
                                         obj.Ultimo_Cio=(txtUltimo_Cio.Text.Equals("")?new DateTime():DateTime.Parse(txtUltimo_Cio.Text));
                                         obj.Especie=txtEspecie.Text;
                                         obj.pedigree=txtpedigree.Text;
                                         obj.codigo_pet=txtcodigo_pet.Text;
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
       <center> <h1>Detalhes do cliente_pet</h1></center>                  
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
                                      <td >                   <p>Nome_Pet</p>
                   <asp:TextBox ID="txtNome_Pet" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Sexo</p>
                   <asp:TextBox ID="txtSexo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Cliente</p>
                   <asp:TextBox ID="txtCodigo_Cliente" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cor</p>
                   <asp:TextBox ID="txtCor" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_Nascimento</p>
                   <asp:TextBox ID="txtData_Nascimento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Pelagem</p>
                   <asp:TextBox ID="txtPelagem" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Porte</p>
                   <asp:TextBox ID="txtPorte" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Raca</p>
                   <asp:TextBox ID="txtRaca" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Ultimo_Cio</p>
                   <asp:TextBox ID="txtUltimo_Cio" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Especie</p>
                   <asp:TextBox ID="txtEspecie" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>pedigree</p>
                   <asp:TextBox ID="txtpedigree" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo_pet</p>
                   <asp:TextBox ID="txtcodigo_pet" runat="server" ></asp:TextBox>
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

