using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class fornecedorDAO
    {
        User usr = null;
        public String Fornecedor { get; set; }
        public String Filial { get; set; }
        public String Razao_social { get; set; }
        public String Nome_Fantasia { get; set; }
        public String CNPJ { get; set; }
        public String IE { get; set; }
        public String Cidade { get; set; }
        public String UF { get; set; }
        public String CEP { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public Decimal Desc_Coml { get; set; }
        public Decimal Desc_Finan { get; set; }
        public Decimal Adc_Finan { get; set; }
        public Decimal Adc_Mkt { get; set; }
        public Decimal Adc_Perda { get; set; }
        public Decimal Adc_Frete { get; set; }
        public DateTime Ultima_Contagem_do_Estoque { get; set; }
        public String Ultima_Contagem_do_EstoqueBr()
        {
            return dataBr(Ultima_Contagem_do_Estoque);
        }

        public Decimal Bonificacao { get; set; }
        public Decimal Prazo { get; set; }
        public String condicao_pagamento { get; set; }
        public Decimal Desc_exp { get; set; }
        public String obs { get; set; }
        public bool pessoa_fisica { get; set; }
        public String senha { get; set; }
        public String Codigo_fornecedor { get; set; }
        public String Endereco_nro { get; set; }
        public bool Formulario_proprio { get; set; }
        public DataTable contatos { get; set; }
        public DataTable mercadorias { get; set; }   
        


        public fornecedorDAO(String fornecedor,User usr)
        {
            this.usr = usr;
            String sql = "Select * from  fornecedor where fornecedor='"+fornecedor+"'";
            SqlDataReader rs = Conexao.consulta(sql,usr);

            try
            {

                carregarDados(rs);
            }
            catch (Exception err)
            {
                
                throw new Exception("Não foi possivel carregar os dados do Fornecedor erro:"+err.Message);
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
        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                Fornecedor = rs["Fornecedor"].ToString();
                Filial = rs["Filial"].ToString();
                Razao_social = rs["Razao_social"].ToString();
                Nome_Fantasia = rs["Nome_Fantasia"].ToString();
                CNPJ = rs["CNPJ"].ToString();
                IE = rs["IE"].ToString();
                Cidade = rs["Cidade"].ToString();
                UF = rs["UF"].ToString();
                CEP = rs["CEP"].ToString();
                Endereco = rs["Endereco"].ToString();
                Bairro = rs["Bairro"].ToString();
                Desc_Coml = (Decimal)(rs["Desc_Coml"].ToString().Equals("") ? new Decimal() : rs["Desc_Coml"]);
                Desc_Finan = (Decimal)(rs["Desc_Finan"].ToString().Equals("") ? new Decimal() : rs["Desc_Finan"]);
                Adc_Finan = (Decimal)(rs["Adc_Finan"].ToString().Equals("") ? new Decimal() : rs["Adc_Finan"]);
                Adc_Mkt = (Decimal)(rs["Adc_Mkt"].ToString().Equals("") ? new Decimal() : rs["Adc_Mkt"]);
                Adc_Perda = (Decimal)(rs["Adc_Perda"].ToString().Equals("") ? new Decimal() : rs["Adc_Perda"]);
                Adc_Frete = (Decimal)(rs["Adc_Frete"].ToString().Equals("") ? new Decimal() : rs["Adc_Frete"]);
                Ultima_Contagem_do_Estoque = (rs["Ultima_Contagem_do_Estoque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Ultima_Contagem_do_Estoque"].ToString()));
                Bonificacao = (Decimal)(rs["Bonificacao"].ToString().Equals("") ? new Decimal() : rs["Bonificacao"]);
                Prazo = (Decimal)(rs["Prazo"].ToString().Equals("") ? new Decimal() : rs["Prazo"]);
                condicao_pagamento = rs["condicao_pagamento"].ToString();
                Desc_exp = (Decimal)(rs["Desc_exp"].ToString().Equals("") ? new Decimal() : rs["Desc_exp"]);
                obs = rs["obs"].ToString();
                pessoa_fisica = (rs["pessoa_fisica"].ToString().Equals("1") ? true : false);
                senha = rs["senha"].ToString();
                Codigo_fornecedor = rs["Codigo_fornecedor"].ToString();
                Endereco_nro = rs["Endereco_nro"].ToString();
                Formulario_proprio = (rs["Formulario_proprio"].ToString().Equals("1") ? true : false);
                mercadorias = Conexao.GetTable("SELECT plu,ean,descricao,data,preco_compra FROM fornecedor_mercadoria where fornecedor='"+Fornecedor+"'",usr);
                contatos = Conexao.GetTable("SELECT Meio_comunicacao, id_comunicacao,contatao FROM Fornecedor_meio_comunicacao where fornecedor='"+Fornecedor+"'",usr);
            
            
            }
        }
        private void update()
        {
            try
            {
                String sql = "update set fornecedor(" +
                              "Fornecedor='" + Fornecedor + "'," +
                              "Filial='" + Filial + "'," +
                              "Razao_social='" + Razao_social + "'," +
                              "Nome_Fantasia='" + Nome_Fantasia + "'," +
                              "CNPJ='" + CNPJ + "'," +
                              "IE='" + IE + "'," +
                              "Cidade='" + Cidade + "'," +
                              "UF='" + UF + "'," +
                              "CEP='" + CEP + "'," +
                              "Endereco='" + Endereco + "'," +
                              "Bairro='" + Bairro + "'," +
                              "Desc_Coml=" + string.Format("{0:0,0.00}", Desc_Coml) + "," +
                              "Desc_Finan=" + string.Format("{0:0,0.00}", Desc_Finan) + "," +
                              "Adc_Finan=" + string.Format("{0:0,0.00}", Adc_Finan) + "," +
                              "Adc_Mkt=" + string.Format("{0:0,0.00}", Adc_Mkt) + "," +
                              "Adc_Perda=" + string.Format("{0:0,0.00}", Adc_Perda) + "," +
                              "Adc_Frete=" + string.Format("{0:0,0.00}", Adc_Frete) + "," +
                              "Ultima_Contagem_do_Estoque='" + Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd") + "'," +
                              "Bonificacao=" + string.Format("{0:0,0.00}", Bonificacao) + "," +
                              "Prazo=" + string.Format("{0:0,0.00}", Prazo) + "," +
                              "condicao_pagamento='" + condicao_pagamento + "'," +
                              "Desc_exp=" + string.Format("{0:0,0.00}", Desc_exp) + "," +
                              "obs='" + obs + "'," +
                              "pessoa_fisica=" + (pessoa_fisica ? "1" : "0") + "," +
                              "senha='" + senha + "'," +
                              "Codigo_fornecedor='" + Codigo_fornecedor + "'," +
                              "Endereco_nro='" + Endereco_nro + "'," +
                              "Formulario_proprio=" + (Formulario_proprio ? "1" : "0") + "," +
                    " )where = "
                        ;
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        private void insert()
        {
            try
            {
                String sql = " insert into fornecedor(" +
                          "Fornecedor," +
                          "Filial," +
                          "Razao_social," +
                          "Nome_Fantasia," +
                          "CNPJ," +
                          "IE," +
                          "Cidade," +
                          "UF," +
                          "CEP," +
                          "Endereco," +
                          "Bairro," +
                          "Desc_Coml," +
                          "Desc_Finan," +
                          "Adc_Finan," +
                          "Adc_Mkt," +
                          "Adc_Perda," +
                          "Adc_Frete," +
                          "Ultima_Contagem_do_Estoque," +
                          "Bonificacao," +
                          "Prazo," +
                          "condicao_pagamento," +
                          "Desc_exp," +
                          "obs," +
                          "pessoa_fisica," +
                          "senha," +
                          "Codigo_fornecedor," +
                          "Endereco_nro," +
                          "Formulario_proprio" +
                     " )values (" +
                          "'" + Fornecedor + "'," +
                          "'" + Filial + "'," +
                          "'" + Razao_social + "'," +
                          "'" + Nome_Fantasia + "'," +
                          "'" + CNPJ + "'," +
                          "'" + IE + "'," +
                          "'" + Cidade + "'," +
                          "'" + UF + "'," +
                          "'" + CEP + "'," +
                          "'" + Endereco + "'," +
                          "'" + Bairro + "'," +
                          string.Format("{0:0,0.00}", Desc_Coml) + "," +
                          string.Format("{0:0,0.00}", Desc_Finan) + "," +
                          string.Format("{0:0,0.00}", Adc_Finan) + "," +
                          string.Format("{0:0,0.00}", Adc_Mkt) + "," +
                          string.Format("{0:0,0.00}", Adc_Perda) + "," +
                          string.Format("{0:0,0.00}", Adc_Frete) + "," +
                          "'" + Ultima_Contagem_do_Estoque.ToString("yyyy-MM-dd") + "'," +
                          string.Format("{0:0,0.00}", Bonificacao) + "," +
                          string.Format("{0:0,0.00}", Prazo) + "," +
                          "'" + condicao_pagamento + "'," +
                          string.Format("{0:0,0.00}", Desc_exp) + "," +
                          "'" + obs + "'," +
                          (pessoa_fisica ? 1 : 0) + "," +
                          "'" + senha + "'," +
                          "'" + Codigo_fornecedor + "'," +
                          "'" + Endereco_nro + "'," +
                          (Formulario_proprio ? 1 : 0) +

                " )";
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/* --Atualizar DaoForm 
txtFornecedor.Text=obj.Fornecedor.ToString();
txtFilial.Text=obj.Filial.ToString();
txtRazao_social.Text=obj.Razao_social.ToString();
txtNome_Fantasia.Text=obj.Nome_Fantasia.ToString();
txtCNPJ.Text=obj.CNPJ.ToString();
txtIE.Text=obj.IE.ToString();
txtCidade.Text=obj.Cidade.ToString();
txtUF.Text=obj.UF.ToString();
txtCEP.Text=obj.CEP.ToString();
txtEndereco.Text=obj.Endereco.ToString();
txtBairro.Text=obj.Bairro.ToString();
txtDesc_Coml.Text=string.Format("{0:0,0.00}",obj.Desc_Coml);
txtDesc_Finan.Text=string.Format("{0:0,0.00}",obj.Desc_Finan);
txtAdc_Finan.Text=string.Format("{0:0,0.00}",obj.Adc_Finan);
txtAdc_Mkt.Text=string.Format("{0:0,0.00}",obj.Adc_Mkt);
txtAdc_Perda.Text=string.Format("{0:0,0.00}",obj.Adc_Perda);
txtAdc_Frete.Text=string.Format("{0:0,0.00}",obj.Adc_Frete);
txtUltima_Contagem_do_Estoque.Text=obj.Ultima_Contagem_do_EstoqueBr();
txtBonificacao.Text=string.Format("{0:0,0.00}",obj.Bonificacao);
txtPrazo.Text=string.Format("{0:0,0.00}",obj.Prazo);
txtcondicao_pagamento.Text=obj.condicao_pagamento.ToString();
txtDesc_exp.Text=string.Format("{0:0,0.00}",obj.Desc_exp);
txtobs.Text=obj.obs.ToString();
chkpessoa_fisica.Checked =obj.pessoa_fisica;
txtsenha.Text=obj.senha.ToString();
txtCodigo_fornecedor.Text=obj.Codigo_fornecedor.ToString();
txtEndereco_nro.Text=obj.Endereco_nro.ToString();
chkFormulario_proprio.Checked =obj.Formulario_proprio;
*/ 
/* --Atualizar FormDao 
obj.Fornecedor=txtFornecedor.Text;
obj.Filial=txtFilial.Text;
obj.Razao_social=txtRazao_social.Text;
obj.Nome_Fantasia=txtNome_Fantasia.Text;
obj.CNPJ=txtCNPJ.Text;
obj.IE=txtIE.Text;
obj.Cidade=txtCidade.Text;
obj.UF=txtUF.Text;
obj.CEP=txtCEP.Text;
obj.Endereco=txtEndereco.Text;
obj.Bairro=txtBairro.Text;
obj.Desc_Coml=Decimal.Parse(txtDesc_Coml.Text);
obj.Desc_Finan=Decimal.Parse(txtDesc_Finan.Text);
obj.Adc_Finan=Decimal.Parse(txtAdc_Finan.Text);
obj.Adc_Mkt=Decimal.Parse(txtAdc_Mkt.Text);
obj.Adc_Perda=Decimal.Parse(txtAdc_Perda.Text);
obj.Adc_Frete=Decimal.Parse(txtAdc_Frete.Text);
obj.Ultima_Contagem_do_Estoque=DateTime.Parse(txtUltima_Contagem_do_Estoque.Text);
obj.Bonificacao=Decimal.Parse(txtBonificacao.Text);
obj.Prazo=Decimal.Parse(txtPrazo.Text);
obj.condicao_pagamento=txtcondicao_pagamento.Text;
obj.Desc_exp=Decimal.Parse(txtDesc_exp.Text);
obj.obs=txtobs.Text;
obj.pessoa_fisica=chkpessoa_fisica.Checked ;
obj.senha=txtsenha.Text;
obj.Codigo_fornecedor=txtCodigo_fornecedor.Text;
obj.Endereco_nro=txtEndereco_nro.Text;
obj.Formulario_proprio=chkFormulario_proprio.Checked ;
*/ 
/*--Campos Form
<td ><p>Fornecedor</p>
<asp:TextBox ID="txtFornecedor" runat="server" ></asp:TextBox>
 </td>

<td ><p>Filial</p>
<asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
 </td>

<td ><p>Razao_social</p>
<asp:TextBox ID="txtRazao_social" runat="server" ></asp:TextBox>
 </td>

<td ><p>Nome_Fantasia</p>
<asp:TextBox ID="txtNome_Fantasia" runat="server" ></asp:TextBox>
 </td>

<td ><p>CNPJ</p>
<asp:TextBox ID="txtCNPJ" runat="server" ></asp:TextBox>
 </td>

<td ><p>IE</p>
<asp:TextBox ID="txtIE" runat="server" ></asp:TextBox>
 </td>

<td ><p>Cidade</p>
<asp:TextBox ID="txtCidade" runat="server" ></asp:TextBox>
 </td>

<td ><p>UF</p>
<asp:TextBox ID="txtUF" runat="server" ></asp:TextBox>
 </td>

<td ><p>CEP</p>
<asp:TextBox ID="txtCEP" runat="server" ></asp:TextBox>
 </td>

<td ><p>Endereco</p>
<asp:TextBox ID="txtEndereco" runat="server" ></asp:TextBox>
 </td>

<td ><p>Bairro</p>
<asp:TextBox ID="txtBairro" runat="server" ></asp:TextBox>
 </td>

<td ><p>Desc_Coml</p>
<asp:TextBox ID="txtDesc_Coml" runat="server" ></asp:TextBox>
 </td>

<td ><p>Desc_Finan</p>
<asp:TextBox ID="txtDesc_Finan" runat="server" ></asp:TextBox>
 </td>

<td ><p>Adc_Finan</p>
<asp:TextBox ID="txtAdc_Finan" runat="server" ></asp:TextBox>
 </td>

<td ><p>Adc_Mkt</p>
<asp:TextBox ID="txtAdc_Mkt" runat="server" ></asp:TextBox>
 </td>

<td ><p>Adc_Perda</p>
<asp:TextBox ID="txtAdc_Perda" runat="server" ></asp:TextBox>
 </td>

<td ><p>Adc_Frete</p>
<asp:TextBox ID="txtAdc_Frete" runat="server" ></asp:TextBox>
 </td>

<td ><p>Ultima_Contagem_do_Estoque</p>
<asp:TextBox ID="txtUltima_Contagem_do_Estoque" runat="server" ></asp:TextBox>
 </td>

<td ><p>Bonificacao</p>
<asp:TextBox ID="txtBonificacao" runat="server" ></asp:TextBox>
 </td>

<td ><p>Prazo</p>
<asp:TextBox ID="txtPrazo" runat="server" ></asp:TextBox>
 </td>

<td ><p>condicao_pagamento</p>
<asp:TextBox ID="txtcondicao_pagamento" runat="server" ></asp:TextBox>
 </td>

<td ><p>Desc_exp</p>
<asp:TextBox ID="txtDesc_exp" runat="server" ></asp:TextBox>
 </td>

<td ><p>obs</p>
<asp:TextBox ID="txtobs" runat="server" ></asp:TextBox>
 </td>

<td ><p>pessoa_fisica</p>
<td><asp:CheckBox ID="chkpessoa_fisica" runat="server" Text="pessoa_fisica"/></td>
 </td>

<td ><p>senha</p>
<asp:TextBox ID="txtsenha" runat="server" ></asp:TextBox>
 </td>

<td ><p>Codigo_fornecedor</p>
<asp:TextBox ID="txtCodigo_fornecedor" runat="server" ></asp:TextBox>
 </td>

<td ><p>Endereco_nro</p>
<asp:TextBox ID="txtEndereco_nro" runat="server" ></asp:TextBox>
 </td>



 </td>

*/

