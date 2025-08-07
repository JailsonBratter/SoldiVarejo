using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao

{
  public class ClienteDAO
   {
      public User usr = null;
              public String Codigo_Cliente { get; set; }
              public String Nome_Cliente { get; set; }
              public String Codigo_Portaria { get; set; }
              public String Situacao { get; set; }
              public String Endereco { get; set; }
              public String Estado_civil { get; set; }
              public String CEP { get; set; }
              public String Bairro { get; set; }
              public String Cidade { get; set; }
              public String UF { get; set; }
              public String CNPJ { get; set; }
              public String IE { get; set; }
              public DateTime Data_Nascimento { get; set; }
                 public String Data_NascimentoBr() 
                 {
                     return dataBr(Data_Nascimento);
                  }

              public String Naturalidade { get; set; }
              public String Nome_conjuge { get; set; }
              public String Contato { get; set; }
              public Decimal Renda_Mensal { get; set; }
              public bool Pessoa_Juridica { get; set; }
              public Decimal Limite_Credito { get; set; }
              public Decimal Utilizado { get; set; }
              public bool ICM_Isento { get; set; }
              public String Historico { get; set; }
              public DateTime data_cadastro { get; set; }
                 public String data_cadastroBr() 
                 {
                     return dataBr(data_cadastro);
                  }

              public bool estado_cliente { get; set; }
              public String Codigo_tabela { get; set; }
              public String vendedor { get; set; }
              public String nome_fantasia { get; set; }
              public String Endereco_ent { get; set; }
              public String Cep_ent { get; set; }
              public String Bairro_ent { get; set; }
              public String Cidade_ent { get; set; }
              public String Uf_ent { get; set; }
              public String endereco_nro { get; set; }
              public String complemento_end { get; set; }
              public String endereco_ent_nro { get; set; }
              public DataTable meiosComunicacao { get; set; }
              public DataTable clienteBanco { get; set; }
              public DataTable localEntrega { get; set; }
              public DataTable mercadorias { get; set; }
              public DataTable entregacozinha { get; set; }

         public ClienteDAO(String codCliente,User usr){
             this.usr = usr;
             String sql="Select * from  Cliente where codigo_cliente='"+codCliente+"'";
             SqlDataReader rs = Conexao.consulta(sql,usr);
             carregarDados(rs);
         }

         public ClienteDAO(User usr) {
             this.usr = usr;
         }
          private String dataBr(DateTime dt) {
              if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001")){
                  return "";
              }else{
                  return dt.ToString("dd/MM/yyyy");
              }
          }
          public void carregarDados(SqlDataReader rs){
              if(rs.Read()){
              Codigo_Cliente = rs["Codigo_Cliente"].ToString();
              Nome_Cliente = rs["Nome_Cliente"].ToString();
              Codigo_Portaria = rs["Codigo_Portaria"].ToString();
              Situacao = rs["Situacao"].ToString();
              Endereco = rs["Endereco"].ToString();
              Estado_civil = rs["Estado_civil"].ToString();
              CEP = rs["CEP"].ToString();
              Bairro = rs["Bairro"].ToString();
              Cidade = rs["Cidade"].ToString();
              UF = rs["UF"].ToString();
              CNPJ = rs["CNPJ"].ToString();
              IE = rs["IE"].ToString();
              Data_Nascimento = (rs["Data_Nascimento"].ToString().Equals("")? new DateTime():DateTime.Parse(rs["Data_Nascimento"].ToString()));
              Naturalidade = rs["Naturalidade"].ToString();
              Nome_conjuge = rs["Nome_conjuge"].ToString();
              Contato = rs["Contato"].ToString();
              Renda_Mensal = (Decimal)(rs["Renda_Mensal"].ToString().Equals("")? new Decimal():rs["Renda_Mensal"]);
              Pessoa_Juridica = (rs["Pessoa_Juridica"].ToString().Equals("1")?true:false);
              Limite_Credito = (Decimal)(rs["Limite_Credito"].ToString().Equals("")? new Decimal():rs["Limite_Credito"]);
              Utilizado = (Decimal)(rs["Utilizado"].ToString().Equals("")? new Decimal():rs["Utilizado"]);
              ICM_Isento = (rs["ICM_Isento"].ToString().Equals("1")?true:false);
              Historico = rs["Historico"].ToString();
              data_cadastro = (rs["data_cadastro"].ToString().Equals("")? new DateTime():DateTime.Parse(rs["data_cadastro"].ToString()));
              estado_cliente = (rs["estado_cliente"].ToString().Equals("1")?true:false);
              Codigo_tabela = rs["Codigo_tabela"].ToString();
              vendedor = rs["vendedor"].ToString();
              nome_fantasia = rs["nome_fantasia"].ToString();
              Endereco_ent = rs["Endereco_ent"].ToString();
              Cep_ent = rs["Cep_ent"].ToString();
              Bairro_ent = rs["Bairro_ent"].ToString();
              Cidade_ent = rs["Cidade_ent"].ToString();
              Uf_ent = rs["Uf_ent"].ToString();
              endereco_nro = rs["endereco_nro"].ToString();
              complemento_end = rs["complemento_end"].ToString();
              endereco_ent_nro = rs["endereco_ent_nro"].ToString();
              meiosComunicacao = Conexao.GetTable("select meio_comunicacao,id_meio_comunicacao,contato from cliente_contato where codigo_cliente='"+Codigo_Cliente+"'",usr);
              clienteBanco = Conexao.GetTable("select a.Numero_banco,b.nome_banco, a.Agencia,a.Conta,a.telefone,a.contato  "+
                                                   " from cliente_banco a inner join banco b on a.numero_banco=b.numero_banco "+
                                                   " where Codigo_cliente= '"+Codigo_Cliente+"'",usr);
              localEntrega = Conexao.GetTable("select LUGAR,ENDERECO,UF,CEP,CIDADE from cliente_local_entrega where codigo_cliente='"+Codigo_Cliente+"'",usr);
              entregacozinha = Conexao.GetTable("SELECT ENDERECO,ENDERECO_NRO NUMERO,BAIRRO,CIDADE,UF,CEP FROM ENTREGA_COZINHA where codigo_cliente='" + Codigo_Cliente + "'",usr);
                mercadorias = Conexao.GetTable("SELECT PLU,DESCRICAO,DATA FROM CLIENTE_MERCADORIA where codigo_cliente='" + Codigo_Cliente + "'",usr );
              }
          }
        private void update(){
        try{
            String sql = "update  Cliente set " +
                       "Nome_Cliente='"+Nome_Cliente+"',"+
                       "Codigo_Portaria='"+Codigo_Portaria+"',"+
                       "Situacao='"+Situacao+"',"+
                       "Endereco='"+Endereco+"',"+
                       "Estado_civil='"+Estado_civil+"',"+
                       "CEP='"+CEP+"',"+
                       "Bairro='"+Bairro+"',"+
                       "Cidade='"+Cidade+"',"+
                       "UF='"+UF+"',"+
                       "CNPJ='"+CNPJ+"',"+
                       "IE='"+IE+"',"+
                       "Data_Nascimento="+(Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01")?"null": "'"+Data_Nascimento.ToString("yyyy-MM-dd")+"'")+","+
                       "Naturalidade='"+Naturalidade+"',"+
                       "Nome_conjuge='"+Nome_conjuge+"',"+
                       "Contato='"+Contato+"',"+
                       "Renda_Mensal="+Renda_Mensal.ToString().Replace(",",".")+","+
                       "Pessoa_Juridica="+(Pessoa_Juridica?"1":"0")+","+
                       "Limite_Credito=" + Limite_Credito.ToString().Replace(",", ".") + "," +
                       "Utilizado=" + Utilizado.ToString().Replace(",", ".") + "," +
                       "ICM_Isento="+(ICM_Isento?"1":"0")+","+
                       "Historico='"+Historico+"',"+
                       "data_cadastro="+(data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01")?"null":"'"+data_cadastro.ToString("yyyy-MM-dd")+"'")+","+
                       "estado_cliente="+(estado_cliente?"1":"0")+","+
                       "Codigo_tabela='"+Codigo_tabela+"',"+
                       "vendedor='"+vendedor+"',"+
                       "nome_fantasia='"+nome_fantasia+"',"+
                       "Endereco_ent='"+Endereco_ent+"',"+
                       "Cep_ent='"+Cep_ent+"',"+
                       "Bairro_ent='"+Bairro_ent+"',"+
                       "Cidade_ent='"+Cidade_ent+"',"+
                       "Uf_ent='"+Uf_ent+"',"+
                       "endereco_nro='"+endereco_nro+"',"+
                       "complemento_end='"+complemento_end+"',"+
                       "endereco_ent_nro='"+endereco_ent_nro+"'"+
             " where  Codigo_Cliente='"+Codigo_Cliente+"'";
             Conexao.executarSql(sql);
             }catch (Exception err){
                 throw new Exception("nao foi possivel Atualizar os valores erro:"+err.Message );
             }
         }


        public bool salvar(bool novo) {
            try
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
            catch (Exception)
            {

                throw;
            }
            

        }

     private void insert(){
         try{
             Codigo_Cliente = Conexao.retornaUmValor("select sequencia from sequenciais where tabela_coluna = 'CLIENTE.CODIGO_CLIENTE    '",usr);
             Codigo_Cliente = (long.Parse(Codigo_Cliente) + 1).ToString().PadLeft(3, '0');
             data_cadastro = DateTime.Now;
             String[,] parametros = new String[35,2];

             parametros[0, 0] = "@Codigo_Cliente";
             parametros[0, 1] = "'" + Codigo_Cliente + "'";

             parametros[1, 0] = "@Nome_Cliente";
             parametros[1, 1] = "'" + Nome_Cliente + "'";

             parametros[1, 0] = "@Nome_Cliente";
             parametros[1, 1] = "'" + Nome_Cliente + "'";

             Conexao.executaProcedure("sp_I_cliente", parametros);

             String sql = " insert into Cliente("+
                       "Codigo_Cliente,"+
                       "Nome_Cliente,"+
                       "Codigo_Portaria,"+
                       "Situacao,"+
                       "Endereco,"+
                       "Estado_civil,"+
                       "CEP,"+
                       "Bairro,"+
                       "Cidade,"+
                       "UF,"+
                       "CNPJ,"+
                       "IE,"+
                       "Data_Nascimento,"+
                       "Naturalidade,"+
                       "Nome_conjuge,"+
                       "Contato,"+
                       "Renda_Mensal,"+
                       "Pessoa_Juridica,"+
                       "Limite_Credito,"+
                       "Utilizado,"+
                       "ICM_Isento,"+
                       "Historico,"+
                       "data_cadastro,"+
                       "estado_cliente,"+
                       "Codigo_tabela,"+
                       "vendedor,"+
                       "nome_fantasia,"+
                       "Endereco_ent,"+
                       "Cep_ent,"+
                       "Bairro_ent,"+
                       "Cidade_ent,"+
                       "Uf_ent,"+
                       "endereco_nro,"+
                       "complemento_end,"+
                       "endereco_ent_nro"+
                  " )values ("+
                       "'"+Codigo_Cliente+"',"+
                       "'"+Nome_Cliente+"',"+
                       "'"+Codigo_Portaria+"',"+
                       "'"+Situacao+"',"+
                       "'"+Endereco+"',"+
                       "'"+Estado_civil+"',"+
                       "'"+CEP+"',"+
                       "'"+Bairro+"',"+
                       "'"+Cidade+"',"+
                       "'"+UF+"',"+
                       "'"+CNPJ+"',"+
                       "'"+IE+"',"+
                       (Data_Nascimento.ToString("yyyy-MM-dd").Equals("0001-01-01")?"null": "'"+Data_Nascimento.ToString("yyyy-MM-dd")+"'")+","+
                       "'"+Naturalidade+"',"+
                       "'"+Nome_conjuge+"',"+
                       "'"+Contato+"',"+
                       Renda_Mensal.ToString().Replace(",",".")+","+
                       (Pessoa_Juridica?1:0)+","+
                       Limite_Credito.ToString().Replace(",",".")+","+
                       Utilizado.ToString().Replace(",",".")+","+
                       (ICM_Isento?1:0)+","+
                       "'"+Historico+"',"+
                       "'"+data_cadastro.ToString("yyyy-MM-dd")+"',"+
                       (estado_cliente?1:0)+","+
                       "'"+Codigo_tabela+"',"+
                       "'"+vendedor+"',"+
                       "'"+nome_fantasia+"',"+
                       "'"+Endereco_ent+"',"+
                       "'"+Cep_ent+"',"+
                       "'"+Bairro_ent+"',"+
                       "'"+Cidade_ent+"',"+
                       "'"+Uf_ent+"',"+
                       "'"+endereco_nro+"',"+
                       "'"+complemento_end+"',"+
                       "'"+endereco_ent_nro+"'"+

             " )";
         Conexao.executarSql(sql);
         Conexao.executarSql("update sequencias set (sequencia='" + Codigo_Cliente + "')tabela_coluna = 'CLIENTE.CODIGO_CLIENTE    '");
         }catch (Exception err){
                 throw new Exception("nao foi possivel Inserir o novo cliente erro:"+err.Message );
         }
      }
    }
}/* --Atualizar DaoForm 
txtCodigo_Cliente.Text=obj.Codigo_Cliente.ToString();
txtNome_Cliente.Text=obj.Nome_Cliente.ToString();
txtCodigo_Portaria.Text=obj.Codigo_Portaria.ToString();
txtSituacao.Text=obj.Situacao.ToString();
txtEndereco.Text=obj.Endereco.ToString();
txtEstado_civil.Text=obj.Estado_civil.ToString();
txtCEP.Text=obj.CEP.ToString();
txtBairro.Text=obj.Bairro.ToString();
txtCidade.Text=obj.Cidade.ToString();
txtUF.Text=obj.UF.ToString();
txtCNPJ.Text=obj.CNPJ.ToString();
txtIE.Text=obj.IE.ToString();
txtData_Nascimento.Text=obj.Data_NascimentoBr();
txtNaturalidade.Text=obj.Naturalidade.ToString();
txtNome_conjuge.Text=obj.Nome_conjuge.ToString();
txtContato.Text=obj.Contato.ToString();
txtRenda_Mensal.Text=string.Format("{0:0,0.00}",obj.Renda_Mensal);
chkPessoa_Juridica.Checked =obj.Pessoa_Juridica;
txtLimite_Credito.Text=string.Format("{0:0,0.00}",obj.Limite_Credito);
txtUtilizado.Text=string.Format("{0:0,0.00}",obj.Utilizado);
chkICM_Isento.Checked =obj.ICM_Isento;
txtHistorico.Text=obj.Historico.ToString();
txtdata_cadastro.Text=obj.data_cadastroBr();
chkestado_cliente.Checked =obj.estado_cliente;
txtCodigo_tabela.Text=obj.Codigo_tabela.ToString();
txtvendedor.Text=obj.vendedor.ToString();
txtnome_fantasia.Text=obj.nome_fantasia.ToString();
txtEndereco_ent.Text=obj.Endereco_ent.ToString();
txtCep_ent.Text=obj.Cep_ent.ToString();
txtBairro_ent.Text=obj.Bairro_ent.ToString();
txtCidade_ent.Text=obj.Cidade_ent.ToString();
txtUf_ent.Text=obj.Uf_ent.ToString();
txtendereco_nro.Text=obj.endereco_nro.ToString();
txtcomplemento_end.Text=obj.complemento_end.ToString();
txtendereco_ent_nro.Text=obj.endereco_ent_nro.ToString();
*/ 
/* --Atualizar FormDao 
obj.Codigo_Cliente=txtCodigo_Cliente.Text;
obj.Nome_Cliente=txtNome_Cliente.Text;
obj.Codigo_Portaria=txtCodigo_Portaria.Text;
obj.Situacao=txtSituacao.Text;
obj.Endereco=txtEndereco.Text;
obj.Estado_civil=txtEstado_civil.Text;
obj.CEP=txtCEP.Text;
obj.Bairro=txtBairro.Text;
obj.Cidade=txtCidade.Text;
obj.UF=txtUF.Text;
obj.CNPJ=txtCNPJ.Text;
obj.IE=txtIE.Text;
obj.Data_Nascimento=DateTime.Parse(txtData_Nascimento.Text);
obj.Naturalidade=txtNaturalidade.Text;
obj.Nome_conjuge=txtNome_conjuge.Text;
obj.Contato=txtContato.Text;
obj.Renda_Mensal=Decimal.Parse(txtRenda_Mensal.Text);
obj.Pessoa_Juridica=chkPessoa_Juridica.Checked ;
obj.Limite_Credito=Decimal.Parse(txtLimite_Credito.Text);
obj.Utilizado=Decimal.Parse(txtUtilizado.Text);
obj.ICM_Isento=chkICM_Isento.Checked ;
obj.Historico=txtHistorico.Text;
obj.data_cadastro=DateTime.Parse(txtdata_cadastro.Text);
obj.estado_cliente=chkestado_cliente.Checked ;
obj.Codigo_tabela=txtCodigo_tabela.Text;
obj.vendedor=txtvendedor.Text;
obj.nome_fantasia=txtnome_fantasia.Text;
obj.Endereco_ent=txtEndereco_ent.Text;
obj.Cep_ent=txtCep_ent.Text;
obj.Bairro_ent=txtBairro_ent.Text;
obj.Cidade_ent=txtCidade_ent.Text;
obj.Uf_ent=txtUf_ent.Text;
obj.endereco_nro=txtendereco_nro.Text;
obj.complemento_end=txtcomplemento_end.Text;
obj.endereco_ent_nro=txtendereco_ent_nro.Text;
*/ 
/*--Campos Form
<td ><p>Codigo_Cliente</p>
<asp:TextBox ID="txtCodigo_Cliente" runat="server" ></asp:TextBox>
 </td>

<td ><p>Nome_Cliente</p>
<asp:TextBox ID="txtNome_Cliente" runat="server" ></asp:TextBox>
 </td>

<td ><p>Codigo_Portaria</p>
<asp:TextBox ID="txtCodigo_Portaria" runat="server" ></asp:TextBox>
 </td>

<td ><p>Situacao</p>
<asp:TextBox ID="txtSituacao" runat="server" ></asp:TextBox>
 </td>

<td ><p>Endereco</p>
<asp:TextBox ID="txtEndereco" runat="server" ></asp:TextBox>
 </td>

<td ><p>Estado_civil</p>
<asp:TextBox ID="txtEstado_civil" runat="server" ></asp:TextBox>
 </td>

<td ><p>CEP</p>
<asp:TextBox ID="txtCEP" runat="server" ></asp:TextBox>
 </td>

<td ><p>Bairro</p>
<asp:TextBox ID="txtBairro" runat="server" ></asp:TextBox>
 </td>

<td ><p>Cidade</p>
<asp:TextBox ID="txtCidade" runat="server" ></asp:TextBox>
 </td>

<td ><p>UF</p>
<asp:TextBox ID="txtUF" runat="server" ></asp:TextBox>
 </td>

<td ><p>CNPJ</p>
<asp:TextBox ID="txtCNPJ" runat="server" ></asp:TextBox>
 </td>

<td ><p>IE</p>
<asp:TextBox ID="txtIE" runat="server" ></asp:TextBox>
 </td>

<td ><p>Data_Nascimento</p>
<asp:TextBox ID="txtData_Nascimento" runat="server" ></asp:TextBox>
 </td>

<td ><p>Naturalidade</p>
<asp:TextBox ID="txtNaturalidade" runat="server" ></asp:TextBox>
 </td>

<td ><p>Nome_conjuge</p>
<asp:TextBox ID="txtNome_conjuge" runat="server" ></asp:TextBox>
 </td>

<td ><p>Contato</p>
<asp:TextBox ID="txtContato" runat="server" ></asp:TextBox>
 </td>

<td ><p>Renda_Mensal</p>
<asp:TextBox ID="txtRenda_Mensal" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Pessoa_Juridica</p>
<td><asp:CheckBox ID="chkPessoa_Juridica" runat="server" Text="Pessoa_Juridica"/></td>
 </td>

<td ><p>Limite_Credito</p>
<asp:TextBox ID="txtLimite_Credito" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Utilizado</p>
<asp:TextBox ID="txtUtilizado" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>ICM_Isento</p>
<td><asp:CheckBox ID="chkICM_Isento" runat="server" Text="ICM_Isento"/></td>
 </td>

<td ><p>Historico</p>
<asp:TextBox ID="txtHistorico" runat="server" ></asp:TextBox>
 </td>

<td ><p>data_cadastro</p>
<asp:TextBox ID="txtdata_cadastro" runat="server" ></asp:TextBox>
 </td>

<td ><p>estado_cliente</p>
<td><asp:CheckBox ID="chkestado_cliente" runat="server" Text="estado_cliente"/></td>
 </td>

<td ><p>Codigo_tabela</p>
<asp:TextBox ID="txtCodigo_tabela" runat="server" ></asp:TextBox>
 </td>

<td ><p>vendedor</p>
<asp:TextBox ID="txtvendedor" runat="server" ></asp:TextBox>
 </td>

<td ><p>nome_fantasia</p>
<asp:TextBox ID="txtnome_fantasia" runat="server" ></asp:TextBox>
 </td>

<td ><p>Endereco_ent</p>
<asp:TextBox ID="txtEndereco_ent" runat="server" ></asp:TextBox>
 </td>

<td ><p>Cep_ent</p>
<asp:TextBox ID="txtCep_ent" runat="server" ></asp:TextBox>
 </td>

<td ><p>Bairro_ent</p>
<asp:TextBox ID="txtBairro_ent" runat="server" ></asp:TextBox>
 </td>

<td ><p>Cidade_ent</p>
<asp:TextBox ID="txtCidade_ent" runat="server" ></asp:TextBox>
 </td>

<td ><p>Uf_ent</p>
<asp:TextBox ID="txtUf_ent" runat="server" ></asp:TextBox>
 </td>

<td ><p>endereco_nro</p>
<asp:TextBox ID="txtendereco_nro" runat="server" ></asp:TextBox>
 </td>

<td ><p>complemento_end</p>
<asp:TextBox ID="txtcomplemento_end" runat="server" ></asp:TextBox>
 </td>

<td ><p>endereco_ent_nro</p>
<asp:TextBox ID="txtendereco_ent_nro" runat="server" ></asp:TextBox>
 </td>

*/
