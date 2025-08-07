using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao

{
  public class pedidoDAO
   {
              public User usr = null;
              public String Filial { get; set; }
              public String Pedido { get; set; }
              public int Status { get; set; }
              public  String getStatus() {
                    switch (Status){
                        case 1:
                             return "ABERTO";
                        case 2:
                             return "CONCLUIDO";
                        case 3:
                             return "CANCELADO";
                        case 4 :
                             return "LIBERADO";
                        case 5 :
                             return "CARREGADO";
                        default:
                             return "";

                    }
                }   

              public int Tipo { get; set; }
              public String Cliente_Fornec { get; set; }
              public String nome { get; set; }
              public String fantasia { get; set; }
              public DateTime Data_cadastro { get; set; }
                 public String Data_cadastroBr() 
                 {
                     return dataBr(Data_cadastro);
                  }

              public DateTime Data_entrega { get; set; }
                 public String Data_entregaBr() 
                 {
                     return dataBr(Data_entrega);
                  }

              public String hora { get; set; }
              public Decimal Desconto { get; set; }
              public Decimal Total { get; set; }
              public String Usuario { get; set; }
              public String Obs { get; set; }
              public Decimal cfop { get; set; }
              public String orcamento { get; set; }
              public String funcionario { get; set; }
              public bool id { get; set; }
              public int cotacao { get; set; }
              public String hora_fim { get; set; }
              public bool impresso { get; set; }
              public DataTable itens { get; set; }
              public DataTable pagamentos { get; set; }

         public pedidoDAO(String pedido,User usr){
             this.usr = usr;
             String sql="Select * from  pedido where pedido ='"+pedido+"'";
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
              Filial = rs["Filial"].ToString();
              Pedido = rs["pedido"].ToString();
              Status = (rs["Status"]==null?0:int.Parse(rs["Status"].ToString()));
              Tipo = (rs["Tipo"]==null?0:int.Parse(rs["Tipo"].ToString()));
              Cliente_Fornec = rs["Cliente_Fornec"].ToString();
              if (Tipo == 1)
              {
                  nome = Conexao.retornaUmValor("Select nome_cliente from cliente where codigo_cliente ='"+Cliente_Fornec+"'",usr);
                  fantasia = Conexao.retornaUmValor("Select nome_fantasia from cliente where codigo_cliente ='" + Cliente_Fornec + "'",usr); 
              }
              else {
                  nome = Conexao.retornaUmValor("Select Razao_social from fornecedor where fornecedor='"+Cliente_Fornec+"'",usr);
              }
                  
              Data_cadastro = (rs["Data_cadastro"].ToString().Equals("")? new DateTime():DateTime.Parse(rs["Data_cadastro"].ToString()));
              Data_entrega = (rs["Data_entrega"].ToString().Equals("")? new DateTime():DateTime.Parse(rs["Data_entrega"].ToString()));
              hora = rs["hora"].ToString();
              Desconto = (Decimal)(rs["Desconto"].ToString().Equals("")? new Decimal():rs["Desconto"]);
              Total = (Decimal)(rs["Total"].ToString().Equals("")? new Decimal():rs["Total"]);
              Usuario = rs["Usuario"].ToString();
              Obs = rs["Obs"].ToString();
              cfop = (Decimal)(rs["cfop"].ToString().Equals("")? new Decimal():rs["cfop"]);
              orcamento = rs["orcamento"].ToString();
              funcionario = rs["funcionario"].ToString();
              id = (rs["id"].ToString().Equals("1")?true:false);
              cotacao = (rs["cotacao"].ToString().Equals("")? 0 : int.Parse(rs["cotacao"].ToString()));
              hora_fim = rs["hora_fim"].ToString();
              impresso = (rs["impresso"].ToString().Equals("1")?true:false);
              itens = Conexao.GetTable("select a.plu,b.Descricao, a.qtde,a.embalagem , b.preco, a.unitario,total "+
                                       " from pedido_Itens a "+
	                                       " inner join mercadoria b on a.plu = b.plu "+
                                        " where pedido='"+Pedido+"'",usr);
              pagamentos = Conexao.GetTable("select Tipo_pagamento,vencimento, valor from pedido_pagamento where pedido = '"+Pedido+"'",usr);


              }
          }
        private void update(){
        try{
         String sql= "update set pedido("+
                       "Filial='"+Filial+"',"+
                       "Pedido='"+Pedido+"',"+
                       "Status="+Status+","+
                       "Tipo="+Tipo+","+
                       "Cliente_Fornec='"+Cliente_Fornec+"',"+
                       "Data_cadastro='"+Data_cadastro.ToString("yyyy-MM-dd")+"',"+
                       "Data_entrega='"+Data_entrega.ToString("yyyy-MM-dd")+"',"+
                       "hora='"+hora+"',"+
                       "Desconto="+string.Format("{0:0,0.00}",Desconto)+","+
                       "Total="+string.Format("{0:0,0.00}",Total)+","+
                       "Usuario='"+Usuario+"',"+
                       "Obs='"+Obs+"',"+
                       "cfop="+string.Format("{0:0,0.00}",cfop)+","+
                       "orcamento='"+orcamento+"'"+
                       "funcionario='"+funcionario+"',"+
                       "id="+(id?"1":"0")+","+
                       "cotacao="+cotacao+","+
                       "hora_fim='"+hora_fim+"',"+
                       "impresso="+(impresso?"1":"0")+","+
             " )where pedido='"+Pedido+"'";
             Conexao.executarSql(sql);
             }catch (Exception err){
                 throw new Exception("nao foi possivel Atualizar os valores erro:"+err.Message );
             }
         }
     private void insert(){
         try{
             String sql = " insert into pedido("+
                       "Filial,"+
                       "Pedido,"+
                       "Status,"+
                       "Tipo,"+
                       "Cliente_Fornec,"+
                       "Data_cadastro,"+
                       "Data_entrega,"+
                       "hora,"+
                       "Desconto,"+
                       "Total,"+
                       "Usuario,"+
                       "Obs,"+
                       "cfop,"+
                       "orcamento,"+
                       "funcionario,"+
                       "id,"+
                       "cotacao,"+
                       "hora_fim,"+
                       "impresso,"+
                  " )values ("+
                       "'"+Filial+"',"+
                        "'"+Pedido+"',"+
                       Status+","+
                       Tipo+","+
                       "'"+Cliente_Fornec+"',"+
                       "'"+Data_cadastro.ToString("yyyy-MM-dd")+"',"+
                       "'"+Data_entrega.ToString("yyyy-MM-dd")+"',"+
                       "'"+hora+"',"+
                       string.Format("{0:0,0.00}",Desconto)+","+
                       string.Format("{0:0,0.00}",Total)+","+
                       "'"+Usuario+"',"+
                       "'"+Obs+"',"+
                       string.Format("{0:0,0.00}",cfop)+","+
                        "'"+orcamento+"'"+
                       "'"+funcionario+"',"+
                       (id?1:0)+","+
                       cotacao+","+
                       "'"+hora_fim+"',"+
                       (impresso?1:0)+","+

             " )";
         Conexao.executarSql(sql);
         }catch (Exception err){
                 throw new Exception("nao foi possivel Inserir os valores erro:"+err.Message );
         }
      }
    }
}/* --Atualizar DaoForm 
txtFilial.Text=obj.Filial.ToString();
txtPedido.Text = obj.Pedido.toString();
txtStatus.Text=obj.Status.ToString();
txtTipo.Text=obj.Tipo.ToString();
txtCliente_Fornec.Text=obj.Cliente_Fornec.ToString();
txtData_cadastro.Text=obj.Data_cadastroBr();
txtData_entrega.Text=obj.Data_entregaBr();
txthora.Text=obj.hora.ToString();
txtDesconto.Text=string.Format("{0:0,0.00}",obj.Desconto);
txtTotal.Text=string.Format("{0:0,0.00}",obj.Total);
txtUsuario.Text=obj.Usuario.ToString();
txtObs.Text=obj.Obs.ToString();
txtcfop.Text=string.Format("{0:0,0.00}",obj.cfop);
--------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
txtfuncionario.Text=obj.funcionario.ToString();
chkid.Checked =obj.id;
txtcotacao.Text=obj.cotacao.ToString();
txthora_fim.Text=obj.hora_fim.ToString();
chkimpresso.Checked =obj.impresso;
*/ 
/* --Atualizar FormDao 
obj.Filial=txtFilial.Text;
--------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
obj.Status=int.Parse(txtStatus.Text);
obj.Tipo=int.Parse(txtTipo.Text);
obj.Cliente_Fornec=txtCliente_Fornec.Text;
obj.Data_cadastro=DateTime.Parse(txtData_cadastro.Text);
obj.Data_entrega=DateTime.Parse(txtData_entrega.Text);
obj.hora=txthora.Text;
obj.Desconto=Decimal.Parse(txtDesconto.Text);
obj.Total=Decimal.Parse(txtTotal.Text);
obj.Usuario=txtUsuario.Text;
obj.Obs=txtObs.Text;
obj.cfop=Decimal.Parse(txtcfop.Text);
--------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
obj.funcionario=txtfuncionario.Text;
obj.id=chkid.Checked ;
obj.cotacao=int.Parse(txtcotacao.Text);
obj.hora_fim=txthora_fim.Text;
obj.impresso=chkimpresso.Checked ;
*/ 
/*--Campos Form
<td ><p>Filial</p>
<asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
 </td>

<td ><p>Pedido</p>
<asp:TextBox ID="txtPedido" runat="server" ></asp:TextBox>
 </td>

<td ><p>Status</p>
<asp:TextBox ID="txtStatus" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Tipo</p>
<asp:TextBox ID="txtTipo" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>Cliente_Fornec</p>
<asp:TextBox ID="txtCliente_Fornec" runat="server" ></asp:TextBox>
 </td>

<td ><p>Data_cadastro</p>
<asp:TextBox ID="txtData_cadastro" runat="server" ></asp:TextBox>
 </td>

<td ><p>Data_entrega</p>
<asp:TextBox ID="txtData_entrega" runat="server" ></asp:TextBox>
 </td>

<td ><p>hora</p>
<asp:TextBox ID="txthora" runat="server" ></asp:TextBox>
 </td>

<td ><p>Desconto</p>
<asp:TextBox ID="txtDesconto" runat="server" ></asp:TextBox>
 </td>

<td ><p>Total</p>
<asp:TextBox ID="txtTotal" runat="server" ></asp:TextBox>
 </td>

<td ><p>Usuario</p>
<asp:TextBox ID="txtUsuario" runat="server" ></asp:TextBox>
 </td>

<td ><p>Obs</p>
<asp:TextBox ID="txtObs" runat="server" ></asp:TextBox>
 </td>

<td ><p>cfop</p>
<asp:TextBox ID="txtcfop" runat="server" ></asp:TextBox>
 </td>

<td ><p>orcamento</p>
<asp:TextBox ID="txtorcamento" runat="server" ></asp:TextBox>
 </td>

<td ><p>funcionario</p>
<asp:TextBox ID="txtfuncionario" runat="server" ></asp:TextBox>
 </td>

<td ><p>id</p>
<td><asp:CheckBox ID="chkid" runat="server" Text="id"/></td>
 </td>

<td ><p>cotacao</p>
<asp:TextBox ID="txtcotacao" runat="server"  CssClass="numero" ></asp:TextBox>
 </td>

<td ><p>hora_fim</p>
<asp:TextBox ID="txthora_fim" runat="server" ></asp:TextBox>
 </td>

<td ><p>impresso</p>
<td><asp:CheckBox ID="chkimpresso" runat="server" Text="impresso"/></td>
 </td>

*/

