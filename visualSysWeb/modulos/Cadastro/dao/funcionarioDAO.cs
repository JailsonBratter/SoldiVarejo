using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.dao
{
    public class funcionarioDAO
    {
        public String Nome { get; set; }
        public String Filial { get; set; }
        public String _funcao = "";
        public String Funcao
        {
            get
            {
                return _funcao;
            }
            set
            {
                _funcao = value;
            }
        }
        public String Inicio { get; set; }
        public String Fim { get; set; }
        public Decimal Comissao { get; set; }
        public bool usa_palm { get; set; }
        public bool cancela_item { get; set; }
        public String sp_mercadoria_plu { get; set; }
        public String sp_mercadoria_dpto { get; set; }
        public String sp_mercadoria_descricao { get; set; }
        public String sp_grupo { get; set; }
        public String sp_sgrupo { get; set; }
        public String sp_departamento { get; set; }
        public String senha { get; set; }
        public String codigo { get; set; }
        public String praca { get; set; }
        public String Nome2 { get; set; }
        public String Endereco { get; set; }
        public String CONTA { get; set; }
        public String banco { get; set; }
        public String Agencia { get; set; }
        public String Nome_correntista { get; set; }
        public String cidade { get; set; }
        public String estado { get; set; }
        public String telefone { get; set; }
        public DateTime data_nascimento { get; set; }
        public String data_nascimentoBr()
        {
            return dataBr(data_nascimento);
        }

        public String sobrenome { get; set; }
        public String cep { get; set; }
        public String celular { get; set; }
        public String bairro { get; set; }
        public User usr = null;
        public bool utiliza_agenda = false;
        public bool Usa_Terminal = false;
        public List<Funcionario_metasDAO> metas = new List<Funcionario_metasDAO>();

        public funcionarioDAO(User usr)
        {
            this.usr = usr;
            this.Filial = usr.getFilial();
        }
        public funcionarioDAO(String Nome, String codigo, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            String sql = "Select * from  funcionario where nome ='" + Nome + "' and codigo='" + codigo + "'";
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
            try
            {


                if (rs.Read())
                {
                    Nome = rs["Nome"].ToString();
                    Filial = rs["Filial"].ToString();
                    Funcao = rs["Funcao"].ToString();
                    Inicio = rs["Inicio"].ToString();
                    Fim = rs["Fim"].ToString();
                    Comissao = (Decimal)(rs["Comissao"].ToString().Equals("") ? new Decimal() : rs["Comissao"]);
                    usa_palm = (rs["usa_palm"].ToString().Equals("1") ? true : false);
                    cancela_item = (rs["cancela_item"].ToString().Equals("1") ? true : false);
                    sp_mercadoria_plu = rs["sp_mercadoria_plu"].ToString();
                    sp_mercadoria_dpto = rs["sp_mercadoria_dpto"].ToString();
                    sp_mercadoria_descricao = rs["sp_mercadoria_descricao"].ToString();
                    sp_grupo = rs["sp_grupo"].ToString();
                    sp_sgrupo = rs["sp_sgrupo"].ToString();
                    sp_departamento = rs["sp_departamento"].ToString();
                    senha = rs["senha"].ToString();
                    codigo = rs["codigo"].ToString();
                    praca = rs["praca"].ToString();
                    Nome2 = rs["Nome2"].ToString();
                    Endereco = rs["Endereco"].ToString();
                    CONTA = rs["CONTA"].ToString();
                    banco = rs["banco"].ToString();
                    Agencia = rs["Agencia"].ToString();
                    Nome_correntista = rs["Nome_correntista"].ToString();
                    cidade = rs["cidade"].ToString();
                    estado = rs["estado"].ToString();
                    telefone = rs["telefone"].ToString();
                    data_nascimento = (rs["data_nascimento"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_nascimento"].ToString()));
                    sobrenome = rs["sobrenome"].ToString();
                    cep = rs["cep"].ToString();
                    celular = rs["celular"].ToString();
                    bairro = rs["bairro"].ToString();
                    utiliza_agenda = (rs["utiliza_agenda"].ToString().Equals("1") ? true : false);
                    Usa_Terminal = (rs["Usa_Terminal"].ToString().Equals("1") ? true : false);
                    carregarMetas();
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

        public void carregarMetas()
        {
            String sql = "Select m.*,d.descricao_departamento from Funcionario_metas as m " +
                "   inner join departamento as d on m.codigo_departamento = d.codigo_departamento " +
                "  where codigo_funcionario = '" + codigo + "' and m.filial='"+Filial+"'";
            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta(sql, null, false);
                metas.Clear();
                while (rs.Read())
                {
                    Funcionario_metasDAO meta = new Funcionario_metasDAO();
                    meta.Codigo_funcionario = codigo;
                    meta.Filial = Filial;
                    meta.Codigo_departamento = rs["codigo_departamento"].ToString();
                    meta.Descricao_departamento = rs["descricao_departamento"].ToString();
                    meta.Meta = Funcoes.decTry(rs["meta"].ToString());
                    metas.Add(meta);
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
                String sql = "update  funcionario set " +

                              "Filial='" + Filial + "'" +
                              ",Funcao='" + Funcao + "'" +
                              ",Inicio='" + Inicio + "'" +
                              ",Fim='" + Fim + "'" +
                              ",Comissao=" + Comissao.ToString().Replace(",", ".") +
                              ",usa_palm=" + (usa_palm ? "1" : "0") +
                              ",cancela_item=" + (cancela_item ? "1" : "0") +
                              ",sp_mercadoria_plu='" + sp_mercadoria_plu + "'" +
                              ",sp_mercadoria_dpto='" + sp_mercadoria_dpto + "'" +
                              ",sp_mercadoria_descricao='" + sp_mercadoria_descricao + "'" +
                              ",sp_grupo='" + sp_grupo + "'" +
                              ",sp_sgrupo='" + sp_sgrupo + "'" +
                              ",sp_departamento='" + sp_departamento + "'" +
                              (senha.Equals("") ? "" : ",senha='" + senha + "'") +
                              ",praca='" + praca + "'" +
                              ",Nome2='" + Nome2 + "'" +
                              ",Endereco='" + Endereco + "'" +
                              ",CONTA='" + CONTA + "'" +
                              ",banco='" + banco + "'" +
                              ",Agencia='" + Agencia + "'" +
                              ",Nome_correntista='" + Nome_correntista + "'" +
                              ",cidade='" + cidade + "'" +
                              ",estado='" + estado + "'" +
                              ",telefone='" + telefone + "'" +
                              ",data_nascimento=" + (data_nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_nascimento.ToString("yyyy-MM-dd") + "'") +
                              ",sobrenome='" + sobrenome + "'" +
                              ",cep='" + cep + "'" +
                              ",celular='" + celular + "'" +
                              ",bairro='" + bairro + "'" +
                              ",utiliza_agenda=" + (utiliza_agenda ? "1" : "0") +
                              ",Usa_Terminal=" + (Usa_Terminal ? "1" : "0") +
                              ",loja=" + usr.filial.loja.ToString() +
                    "  where  Nome='" + Nome + "' and  codigo='" + codigo + "'";

                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            if (!senha.Equals(""))
            {
                int existSenha = int.Parse(Conexao.retornaUmValor("Select count(senha) from funcionario where senha='" + senha + "' and nome<>'" + Nome + "' and codigo <>'" + codigo + "'", usr));
                if (existSenha > 0)
                    throw new Exception("Esta senha não é permitida Tende informar uma outra Senha");
            }

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                if (novo)
                {
                    insert(conn, tran);
                }
                else
                {
                    update(conn, tran);
                }

                salvarMetas(conn, tran);

                tran.Commit();

            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }



            return true;
        }

        private void salvarMetas(SqlConnection conn, SqlTransaction tran)
        {
            String sql = " delete from Funcionario_metas where codigo_funcionario='" + codigo + "' and filial = '" + Filial + "'; ";
            foreach (Funcionario_metasDAO meta in metas)
            {
                meta.Codigo_funcionario = codigo;
                meta.Filial = Filial;
                sql += meta.sql();
            }

            Conexao.executarSql(sql, conn, tran);


        }

        public void addMeta(Funcionario_metasDAO meta)
        {
            if (metas.Count(i => i.Codigo_departamento.Equals(meta.Codigo_departamento)) == 0)
            {
                meta.Codigo_funcionario = codigo;
                meta.Filial = Filial;
                metas.Add(meta);
            }

        }

        public bool excluir()
        {

            int ped = int.Parse(Conexao.retornaUmValor("Select COUNT (Funcionario) from pedido where funcionario ='" + Nome + "' ", usr));
            int comanda = int.Parse(Conexao.retornaUmValor("Select COUNT (usuario) from comanda where usuario='" + codigo + "'", usr));
            if (ped > 0 || comanda > 0)
            {
                Conexao.executarSql("UPDATE Funcionario SET Inativo = 1 WHERE Nome = '" + Nome + "'");
                //throw new Exception("Funcionario já esta sendo utilizada");
            }
            else
            {
                String sql = "delete from funcionario  where  Nome='" + Nome + "' and  codigo='" + codigo + "'";
                Conexao.executarSql(sql);
            }
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {

                codigo = Funcoes.sequencia("FUNCIONARIO.CODIGO", usr);
                String sql = " insert into funcionario (" +
                          "Nome," +
                          "Filial," +
                          "Funcao," +
                          "Inicio," +
                          "Fim," +
                          "Comissao," +
                          "usa_palm," +
                          "cancela_item," +
                          "sp_mercadoria_plu," +
                          "sp_mercadoria_dpto," +
                          "sp_mercadoria_descricao," +
                          "sp_grupo," +
                          "sp_sgrupo," +
                          "sp_departamento," +
                          "senha," +
                          "codigo," +
                          "praca," +
                          "Nome2," +
                          "Endereco," +
                          "CONTA," +
                          "banco," +
                          "Agencia," +
                          "Nome_correntista," +
                          "cidade," +
                          "estado," +
                          "telefone," +
                          "data_nascimento," +
                          "sobrenome," +
                          "cep," +
                          "celular," +
                          "bairro," +
                          "utiliza_agenda" +
                          ",Usa_Terminal" +
                          ",loja" +
                     " )values (" +
                          "'" + Nome + "'" +
                          "," + "'" + usr.getFilial() + "'" +
                          "," + "'" + Funcao + "'" +
                          "," + "'" + Inicio + "'" +
                          "," + "'" + Fim + "'" +
                          "," + Comissao.ToString().Replace(",", ".") +
                          "," + (usa_palm ? 1 : 0) +
                          "," + (cancela_item ? 1 : 0) +
                          "," + "'" + sp_mercadoria_plu + "'" +
                          "," + "'" + sp_mercadoria_dpto + "'" +
                          "," + "'" + sp_mercadoria_descricao + "'" +
                          "," + "'" + sp_grupo + "'" +
                          "," + "'" + sp_sgrupo + "'" +
                          "," + "'" + sp_departamento + "'" +
                          "," + "'" + senha + "'" +
                          "," + "'" + codigo + "'" +
                          "," + "'" + praca + "'" +
                          "," + "'" + Nome2 + "'" +
                          "," + "'" + Endereco + "'" +
                          "," + "'" + CONTA + "'" +
                          "," + "'" + banco + "'" +
                          "," + "'" + Agencia + "'" +
                          "," + "'" + Nome_correntista + "'" +
                          "," + "'" + cidade + "'" +
                          "," + "'" + estado + "'" +
                          "," + "'" + telefone + "'" +
                          "," + (data_nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_nascimento.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + sobrenome + "'" +
                          "," + "'" + cep + "'" +
                          "," + "'" + celular + "'" +
                          "," + "'" + bairro + "'" +
                          "," + (utiliza_agenda ? "1" : "0") +
                          "," + (Usa_Terminal ? "1" : "0") +
                          "," + usr.filial.loja.ToString() +
                         ");";

                Conexao.executarSql(sql, conn, tran);
                Funcoes.salvaProximaSequencia("FUNCIONARIO.CODIGO", usr, conn, tran);
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
                  static String sqlGrid = ""select * from funcionario";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ funcionarioDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>funcionario</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="31"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Nome" Text="Nome" Visible="true" 
                    HeaderText="Nome" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Funcao" Text="Funcao" Visible="true" 
                    HeaderText="Funcao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Inicio" Text="Inicio" Visible="true" 
                    HeaderText="Inicio" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Fim" Text="Fim" Visible="true" 
                    HeaderText="Fim" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Comissao" Text="Comissao" Visible="true" 
                    HeaderText="Comissao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usa_palm" Text="usa_palm" Visible="true" 
                    HeaderText="usa_palm" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cancela_item" Text="cancela_item" Visible="true" 
                    HeaderText="cancela_item" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_mercadoria_plu" Text="sp_mercadoria_plu" Visible="true" 
                    HeaderText="sp_mercadoria_plu" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_mercadoria_dpto" Text="sp_mercadoria_dpto" Visible="true" 
                    HeaderText="sp_mercadoria_dpto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_mercadoria_descricao" Text="sp_mercadoria_descricao" Visible="true" 
                    HeaderText="sp_mercadoria_descricao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_grupo" Text="sp_grupo" Visible="true" 
                    HeaderText="sp_grupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_sgrupo" Text="sp_sgrupo" Visible="true" 
                    HeaderText="sp_sgrupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sp_departamento" Text="sp_departamento" Visible="true" 
                    HeaderText="sp_departamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="senha" Text="senha" Visible="true" 
                    HeaderText="senha" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="codigo" Text="codigo" Visible="true" 
                    HeaderText="codigo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="praca" Text="praca" Visible="true" 
                    HeaderText="praca" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nome2" Text="Nome2" Visible="true" 
                    HeaderText="Nome2" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Endereco" Text="Endereco" Visible="true" 
                    HeaderText="Endereco" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="CONTA" Text="CONTA" Visible="true" 
                    HeaderText="CONTA" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="banco" Text="banco" Visible="true" 
                    HeaderText="banco" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Agencia" Text="Agencia" Visible="true" 
                    HeaderText="Agencia" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nome_correntista" Text="Nome_correntista" Visible="true" 
                    HeaderText="Nome_correntista" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cidade" Text="cidade" Visible="true" 
                    HeaderText="cidade" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="estado" Text="estado" Visible="true" 
                    HeaderText="estado" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="telefone" Text="telefone" Visible="true" 
                    HeaderText="telefone" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="data_nascimento" Text="data_nascimento" Visible="true" 
                    HeaderText="data_nascimento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="sobrenome" Text="sobrenome" Visible="true" 
                    HeaderText="sobrenome" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="cep" Text="cep" Visible="true" 
                    HeaderText="cep" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="celular" Text="celular" Visible="true" 
                    HeaderText="celular" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="bairro" Text="bairro" Visible="true" 
                    HeaderText="bairro" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static funcionarioDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new funcionarioDAO();
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
                                        obj = new funcionarioDAO(index,usr);
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
                                         txtNome.Text=obj.Nome.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtFuncao.Text=obj.Funcao.ToString();
                                         txtInicio.Text=obj.Inicio.ToString();
                                         txtFim.Text=obj.Fim.ToString();
                                         txtComissao.Text=string.Format("{0:0,0.00}",obj.Comissao);
                                         chkusa_palm.Checked =obj.usa_palm;
                                         chkcancela_item.Checked =obj.cancela_item;
                                         txtsp_mercadoria_plu.Text=obj.sp_mercadoria_plu.ToString();
                                         txtsp_mercadoria_dpto.Text=obj.sp_mercadoria_dpto.ToString();
                                         txtsp_mercadoria_descricao.Text=obj.sp_mercadoria_descricao.ToString();
                                         txtsp_grupo.Text=obj.sp_grupo.ToString();
                                         txtsp_sgrupo.Text=obj.sp_sgrupo.ToString();
                                         txtsp_departamento.Text=obj.sp_departamento.ToString();
                                         txtsenha.Text=obj.senha.ToString();
                                         txtcodigo.Text=obj.codigo.ToString();
                                         txtpraca.Text=obj.praca.ToString();
                                         txtNome2.Text=obj.Nome2.ToString();
                                         txtEndereco.Text=obj.Endereco.ToString();
                                         txtCONTA.Text=obj.CONTA.ToString();
                                         txtbanco.Text=obj.banco.ToString();
                                         txtAgencia.Text=obj.Agencia.ToString();
                                         txtNome_correntista.Text=obj.Nome_correntista.ToString();
                                         txtcidade.Text=obj.cidade.ToString();
                                         txtestado.Text=obj.estado.ToString();
                                         txttelefone.Text=obj.telefone.ToString();
                                         txtdata_nascimento.Text=obj.data_nascimentoBr();
                                         txtsobrenome.Text=obj.sobrenome.ToString();
                                         txtcep.Text=obj.cep.ToString();
                                         txtcelular.Text=obj.celular.ToString();
                                         txtbairro.Text=obj.bairro.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Nome=txtNome.Text;
                                         obj.Filial=txtFilial.Text;
                                         obj.Funcao=txtFuncao.Text;
                                         obj.Inicio=txtInicio.Text;
                                         obj.Fim=txtFim.Text;
                                         obj.Comissao=Decimal.Parse(txtComissao.Text);
                                         obj.usa_palm=chkusa_palm.Checked ;
                                         obj.cancela_item=chkcancela_item.Checked ;
                                         obj.sp_mercadoria_plu=txtsp_mercadoria_plu.Text;
                                         obj.sp_mercadoria_dpto=txtsp_mercadoria_dpto.Text;
                                         obj.sp_mercadoria_descricao=txtsp_mercadoria_descricao.Text;
                                         obj.sp_grupo=txtsp_grupo.Text;
                                         obj.sp_sgrupo=txtsp_sgrupo.Text;
                                         obj.sp_departamento=txtsp_departamento.Text;
                                         obj.senha=txtsenha.Text;
                                         obj.codigo=txtcodigo.Text;
                                         obj.praca=txtpraca.Text;
                                         obj.Nome2=txtNome2.Text;
                                         obj.Endereco=txtEndereco.Text;
                                         obj.CONTA=txtCONTA.Text;
                                         obj.banco=txtbanco.Text;
                                         obj.Agencia=txtAgencia.Text;
                                         obj.Nome_correntista=txtNome_correntista.Text;
                                         obj.cidade=txtcidade.Text;
                                         obj.estado=txtestado.Text;
                                         obj.telefone=txttelefone.Text;
                                         obj.data_nascimento=DateTime.Parse(txtdata_nascimento.Text);
                                         obj.sobrenome=txtsobrenome.Text;
                                         obj.cep=txtcep.Text;
                                         obj.celular=txtcelular.Text;
                                         obj.bairro=txtbairro.Text;
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
       <center> <h1>Detalhes do funcionario</h1></center>                  
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
                                      <td >                   <p>Nome</p>
                   <asp:TextBox ID="txtNome" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Funcao</p>
                   <asp:TextBox ID="txtFuncao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Inicio</p>
                   <asp:TextBox ID="txtInicio" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Fim</p>
                   <asp:TextBox ID="txtFim" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Comissao</p>
                   <asp:TextBox ID="txtComissao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>usa_palm</p>
                   <td><asp:CheckBox ID="chkusa_palm" runat="server" Text="usa_palm"/>
                   </td>

                                      <td >                   <p>cancela_item</p>
                   <td><asp:CheckBox ID="chkcancela_item" runat="server" Text="cancela_item"/>
                   </td>

                                      <td >                   <p>sp_mercadoria_plu</p>
                   <asp:TextBox ID="txtsp_mercadoria_plu" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sp_mercadoria_dpto</p>
                   <asp:TextBox ID="txtsp_mercadoria_dpto" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sp_mercadoria_descricao</p>
                   <asp:TextBox ID="txtsp_mercadoria_descricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sp_grupo</p>
                   <asp:TextBox ID="txtsp_grupo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sp_sgrupo</p>
                   <asp:TextBox ID="txtsp_sgrupo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sp_departamento</p>
                   <asp:TextBox ID="txtsp_departamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>senha</p>
                   <asp:TextBox ID="txtsenha" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>codigo</p>
                   <asp:TextBox ID="txtcodigo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>praca</p>
                   <asp:TextBox ID="txtpraca" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Nome2</p>
                   <asp:TextBox ID="txtNome2" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Endereco</p>
                   <asp:TextBox ID="txtEndereco" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>CONTA</p>
                   <asp:TextBox ID="txtCONTA" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>banco</p>
                   <asp:TextBox ID="txtbanco" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Agencia</p>
                   <asp:TextBox ID="txtAgencia" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Nome_correntista</p>
                   <asp:TextBox ID="txtNome_correntista" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>cidade</p>
                   <asp:TextBox ID="txtcidade" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>estado</p>
                   <asp:TextBox ID="txtestado" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>telefone</p>
                   <asp:TextBox ID="txttelefone" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>data_nascimento</p>
                   <asp:TextBox ID="txtdata_nascimento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>sobrenome</p>
                   <asp:TextBox ID="txtsobrenome" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>cep</p>
                   <asp:TextBox ID="txtcep" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>celular</p>
                   <asp:TextBox ID="txtcelular" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>bairro</p>
                   <asp:TextBox ID="txtbairro" runat="server" ></asp:TextBox>
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

