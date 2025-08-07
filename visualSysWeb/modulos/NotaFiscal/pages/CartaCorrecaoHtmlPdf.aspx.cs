using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region Prop
        String clienteFornecedor = "";
        int tipoNf = 1;
        String numero = "";
        bool enviaEmail = false;
        String strId = "";
        DateTime dtEmissao = new DateTime();
        String Dest_Fornec = "";
        filialDAO fili = new filialDAO();
        #endregion

        /// <summary>
        /// Metodo para pegar o logo que será impresso
        /// </summary>
        private void PegarLogo()
        {

            SqlDataReader rs = null;
            try
            {
                rs = Conexao.consulta("Select logo from filial where filial = 'MATRIZ'", null, false);
                if (rs.Read())
                {
                    byte[] bytes = (byte[])rs["logo"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string url = "data:image/png;base64," + base64String;
                    ImgLogo.ImageUrl = url;

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

        /// <summary>
        /// Metodo para pegar os parametros na Url
        /// </summary>
        private void ParametrosGet()
        {
            try
            {
                if (Request.Params["cliente_Fornecedor"] != null)
                {//destinatario
                    clienteFornecedor = Request.Params["cliente_Fornecedor"].ToString();
                }
                if (Request.Params["numero"] != null)
                {//numero da nota
                    LblNumNFE.Text = String.Format("{0:000.000.000}", Request.Params["numero"]);
                    numero = Request.Params["numero"].ToString();
                }
                if (Request.Params["tipoNf"] != null)
                {//se é entrada ou saida
                    int.TryParse(Request.Params["tipoNf"].ToString(), out tipoNf);
                }

                if (Request.Params["enviaEmail"] != null)
                {
                    enviaEmail = Request.Params["enviaEmail"].ToString().ToUpper().Equals("TRUE");
                }

                if (Request.Params["Filial"] != null)
                {
                    fili = new filialDAO(Request.Params["Filial"]);
                }



            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Metodo para trazer e preencher os dados necessarios para a CCe
        /// </summary>
        private void PreencherDados()
        {
            try
            {
                PegarLogo();
                ParametrosGet();

                #region Nota
                SqlDataReader rsNF = null;
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    Sql.Append("Select id, Emissao,Dest_Fornec,IDBarra from nf where codigo='" + numero);
                    Sql.Append("' and tipo_nf=" + tipoNf);
                    Sql.Append(" and cliente_Fornecedor='" + clienteFornecedor + "'");
                    rsNF = Conexao.consulta(Sql.ToString(), null, false);
                    if (rsNF.Read())
                    {
                        strId = rsNF["id"].ToString();
                        DateTime.TryParse(rsNF["emissao"].ToString(), out dtEmissao);
                        Dest_Fornec = rsNF["Dest_Fornec"].ToString();
                        try
                        {


                            byte[] bytes = (byte[])rsNF["IDBARRA"];
                            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                            string url = "data:image/png;base64," + base64String;

                            ImgCodBarras.ImageUrl = url;
                        }
                        catch (Exception)
                        {


                        }
                        LblChave.Text = strId;
                        LblDTEmissao.Text = dtEmissao.ToShortDateString();

                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsNF != null)
                        rsNF.Close();

                }
                #endregion

                #region Emitente
                if (fili != null)
                {
                    LblRazaoSEmit.Text = fili.Razao_Social;
                    StringBuilder EndEmit = new StringBuilder();
                    EndEmit.Append(fili.Endereco + ", " + fili.endereco_nro);
                    EndEmit.Append(" - " + fili.bairro + " \n" + fili.CEP);
                    EndEmit.Append(" " + fili.Cidade + " - " + fili.UF);
                    LblEnderecoEmit.Text = EndEmit.ToString();
                    long Tel = 0;
                    long.TryParse(fili.telefone, out Tel);
                    LblTelEmit.Text = String.Format("{0:#0000-0000}", Tel);
                    LblIEEmit.Text = fili.IE;
                    long CNPJ = 0;
                    long.TryParse(fili.CNPJ, out CNPJ);
                    LblCNPJEmit.Text = String.Format("{0:00 000 000/0000-00}", CNPJ).Replace(" ", ".");
                    LblSerie.Text = fili.serie_nfe.ToString();
                }
                #endregion

                SqlDataReader rs = null;
                #region Correcao
                try
                {
                    string SQL = "Select * from nfe_correcao where codigo = " + numero + "order by seq desc";
                    rs = Conexao.consulta(SQL, null, false);
                    if (rs.Read())
                    {
                        LblCarta.Text = rs["correcao"].ToString();
                        LblSequencia.Text = rs["seq"].ToString();
                        LblDtRegistro.Text = rs["data"].ToString();
                        LblProtocolo.Text = rs["protocolo"].ToString();
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
                #endregion]

                #region Destinatario
                try
                {
                    ClienteDAO ObjCli = new ClienteDAO(clienteFornecedor, null);
                    fornecedorDAO ObjFor = new fornecedorDAO(clienteFornecedor, null);
                    if (Dest_Fornec == "1")
                    {
                        LblRazaoSDest.Text = ObjFor.Razao_social;
                        LblEnderecoDest.Text = ObjFor.Endereco;
                        if (!string.IsNullOrEmpty(ObjFor.Endereco_nro))
                            LblEnderecoDest.Text += ", " + ObjFor.Endereco_nro;
                        LblBairroDest.Text = ObjFor.Bairro;
                        LblUFDest.Text = ObjFor.UF;
                        LblMunicipioDest.Text = ObjFor.Cidade;
                        LblCEPDest.Text = ObjFor.CEP;
                        LblCNPJDest.Text = ObjFor.CNPJ;
                        LblIEDest.Text = ObjFor.IE;
                        LblTelDest.Text = ObjFor.telefone1;
                    }
                    else
                    {
                        LblRazaoSDest.Text = ObjCli.Nome_Cliente;
                        LblEnderecoDest.Text = ObjCli.Endereco;
                        if (!string.IsNullOrEmpty(ObjCli.endereco_nro))
                            LblEnderecoDest.Text += ", " + ObjCli.endereco_nro;
                        if (!string.IsNullOrEmpty(ObjCli.complemento_end))
                            LblEnderecoDest.Text += " - " + ObjCli.complemento_end;
                        LblBairroDest.Text = ObjCli.Bairro;
                        LblUFDest.Text = ObjCli.UF;
                        LblMunicipioDest.Text = ObjCli.Cidade;
                        LblCEPDest.Text = ObjCli.CEP;
                        LblCNPJDest.Text = ObjCli.CNPJ;
                        LblIEDest.Text = ObjCli.IE;
                        LblTelDest.Text = ObjCli.primeiroMeioComunicacao();
                    }


                }
                catch (Exception)
                {

                    throw;
                }

                #endregion]
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PreencherDados();


                if(Request.Params["imprimir"] !=null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "print", "self.print();", true);
                }
            }
            catch (Exception err)
            {

                LblRazaoSEmit.Text = err.Message;

            }
            
        }


      
    }
}