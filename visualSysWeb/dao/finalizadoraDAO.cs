using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao

{
  public class finalizadoraDAO
   {
              public int Nro_Finalizadora { get; set; }
              public String filial { get; set; }
              public String Codigo_centro_custo { get; set; }
              public String Pagamento { get; set; }
              public String Troco { get; set; }
              public String Finalizadora { get; set; }
              public int Tecla { get; set; }
              public int Ecf { get; set; }
         public finalizadoraDAO(User usr){
             String sql="Select * from  finalizadora";
             SqlDataReader rs = Conexao.consulta(sql,usr);
             carregarDados(rs);
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
              Nro_Finalizadora = (rs["Nro_Finalizadora"]==null?0:int.Parse(rs["Nro_Finalizadora"].ToString()));
              filial = rs["filial"].ToString();
              Codigo_centro_custo = rs["Codigo_centro_custo"].ToString();
              Pagamento = rs["Pagamento"].ToString();
              Troco = rs["Troco"].ToString();
              Finalizadora = rs["Finalizadora"].ToString();
              Tecla = (rs["Tecla"]==null?0:int.Parse(rs["Tecla"].ToString()));
              Ecf = (rs["Ecf"]==null?0:int.Parse(rs["Ecf"].ToString()));
              }
          }
        private void update(){
        try{
         String sql= "update set finalizadora("+
                       "Nro_Finalizadora="+Nro_Finalizadora+","+
                       "filial='"+filial+"',"+
                       "Codigo_centro_custo='"+Codigo_centro_custo+"',"+
                       "Pagamento='"+Pagamento+"',"+
                       "Troco='"+Troco+"',"+
                       "Finalizadora='"+Finalizadora+"',"+
                       "Tecla="+Tecla+","+
                       "Ecf="+Ecf+","+
             " )where = "
                 ;
             Conexao.executarSql(sql);
             }catch (Exception err){
                 throw new Exception("nao foi possivel Atualizar os valores erro:"+err.message );
             }
         }
     private void insert(){
         try{
             String sql = " insert into finalizadora("+
                       "Nro_Finalizadora,"+
                       "filial,"+
                       "Codigo_centro_custo,"+
                       "Pagamento,"+
                       "Troco,"+
                       "Finalizadora,"+
                       "Tecla,"+
                       "Ecf,"+
                  " )values ("+
                       Nro_Finalizadora+","+
                       "'"+filial+"',"+
                       "'"+Codigo_centro_custo+"',"+
                       "'"+Pagamento+"',"+
                       "'"+Troco+"',"+
                       "'"+Finalizadora+"',"+
                       Tecla+","+
                       Ecf+","+

             " )";
         Conexao.executarSql(sql);
         }catch (Exception err){
                 throw new Exception("nao foi possivel Inserir os valores erro:"+err.Message );
         }
      }
    }
}/*/* --Atualizar DaoForm 
txtNro_Finalizadora.Text=obj.Nro_Finalizadora.ToString();
txtfilial.Text=obj.filial.ToString();
txtCodigo_centro_custo.Text=obj.Codigo_centro_custo.ToString();
txtPagamento.Text=obj.Pagamento.ToString();
txtTroco.Text=obj.Troco.ToString();
txtFinalizadora.Text=obj.Finalizadora.ToString();
txtTecla.Text=obj.Tecla.ToString();
txtEcf.Text=obj.Ecf.ToString();
*/ 
/* --Atualizar FormDao 
obj.Nro_Finalizadora=int.Parse(txtNro_Finalizadora.Text);
obj.filial=txtfilial.Text;
obj.Codigo_centro_custo=txtCodigo_centro_custo.Text;
obj.Pagamento=txtPagamento.Text;
obj.Troco=txtTroco.Text;
obj.Finalizadora=txtFinalizadora.Text;
obj.Tecla=int.Parse(txtTecla.Text);
obj.Ecf=int.Parse(txtEcf.Text);
*/ 
/*--Campos Form
<td ><p>Nro_Finalizadora</p>
<asp:TextBox ID="txtNro_Finalizadora" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>filial</p>
<asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
 </td>

<td ><p>Codigo_centro_custo</p>
<asp:TextBox ID="txtCodigo_centro_custo" runat="server" ></asp:TextBox>
 </td>

<td ><p>Pagamento</p>
<asp:TextBox ID="txtPagamento" runat="server" ></asp:TextBox>
 </td>

<td ><p>Troco</p>
<asp:TextBox ID="txtTroco" runat="server" ></asp:TextBox>
 </td>

<td ><p>Finalizadora</p>
<asp:TextBox ID="txtFinalizadora" runat="server" ></asp:TextBox>
 </td>

<td ><p>Tecla</p>
<asp:TextBox ID="txtTecla" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Ecf</p>
<asp:TextBox ID="txtEcf" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

*/
/*================================Metodos Botoes==========================================
  
                 protected override void btnIncluir_Click(object sender, EventArgs e)
                 {
                    incluir(pnBtn);
                 }
                 
                 protected override void btnEditar_Click(object sender, EventArgs e)
                 {
                    editar(pnBtn);
                 }
                 protected override void btnPesquisar_Click(object sender, EventArgs e)
                 {
                 Response.Redirect("nomepaginapesquisa");
                 }
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     lblError.Text = "n?o ? possivel excluir o registro";
                     lblError.ForeColor = System.Drawing.Color.Red;
                  }
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     carregarDadosObj();
                     obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                     lblError.Text = "Salvo com Sucesso";
                     lblError.ForeColor = System.Drawing.Color.Blue;
                     desabilitaControles(cabecalho);
                     desabilitaControles(conteudo);
                     visualizar(pnBtn);
                  }
                  protected override void btnCancelar_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("nomepaginapesquisa");
                  }
*/ 
*/
