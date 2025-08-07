using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Sped.code;
using visualSysWeb.modulos.Sped.dao;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class DocumentosEletronicos : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DocumentosFiltros filtros = (DocumentosFiltros)Session["filtro" + urlSessao()];
                if(filtros == null)
                {
                    txtDataDe.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
                    txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtPDV.Text = filtros.pdv;
                    txtDocumento.Text = filtros.documento;
                    txtDataDe.Text = filtros.dataDe;
                    txtDataAte.Text = filtros.dataAte;
                }
                pesquisar();
            }

            sopesquisa(pnBtn);
        }
        protected void pesquisar()
        {
            DocumentosFiltros filtros = new DocumentosFiltros()
            {
                pdv = txtPDV.Text ,
                documento = txtDocumento.Text ,
                dataDe= txtDataDe.Text ,
                dataAte = txtDataAte.Text 
            };

            Session.Remove("filtro" + urlSessao());
            Session.Add("filtro" + urlSessao(), filtros);

            User usr = (User)Session["User"];
            String sql = "";

            if (!DllTipo.Text.Equals("3"))
            {

                sql = "Select *,case when tipo = 1 then SUBSTRING(id_chave,35,6) else SUBSTRING(id_chave, 26, 9) END as Nro_extrato_Sat from documento_eletronico ";
                sql += " where Filial = '" + usr.getFilial() + "' AND data between '" + Funcoes.dtTry(txtDataDe.Text).ToString("yyyy-MM-dd") + "'" +
                        " AND '" + Funcoes.dtTry(txtDataAte.Text).ToString("yyyy-MM-dd") + "' AND documento_eletronico.Tipo = " + DllTipo.Text;
                if (DllTipo.Text.Equals("1"))
                {
                    if (!txtPDV.Text.Equals(""))
                    {
                        sql += " and caixa ='" + txtPDV.Text + "'";
                    }
                    if (!txtSerieEquipamento.Text.Equals(""))
                    {
                        sql += " and cast(nro_serie_equipamento as int) =" + txtSerieEquipamento.Text;
                    }
                    if (!txtDocumento.Text.Equals(""))
                    {
                        sql += " and documento like '" + txtDocumento.Text + "%'";
                    }

                }
                if (!txtExtrato.Text.Equals(""))
                {
                    if (DllTipo.Text.Equals("1"))
                    {
                        sql += " and SUBSTRING(id_chave,35,6)  = '" + txtExtrato.Text + "'";
                    }
                    else
                    {
                        sql += " and CONVERT(INT, SUBSTRING(id_chave,26,9))  = " + txtExtrato.Text;
                    }
                }
            }
            else
            {
                sql = "select nf.codigo AS Documento, nf.codigo as Nro_extrato_sat, '' as Nro_Serie_Equipamento, 0 AS Caixa,";
                sql += " nf.data, nf.ID as Id_Chave, '' as ID_Chave_Cancelamento,";
                sql += " nfm.nfexml as Cfe_XML, '' As CFE_XML_Cancelamento";
                sql += " FROM NF ";
                sql += " INNER JOIN Nf_manifestar nfm on NF.FILIAL = nfm.filial AND nf.id = nfm.Chave";
                sql += " WHERE nf.filial = '" + usr.getFilial() + "'";
                sql += " AND NF.Data BETWEEN '" + Funcoes.dtTry(txtDataDe.Text).ToString("yyyy-MM-dd") + "' AND '" + Funcoes.dtTry(txtDataAte.Text).ToString("yyyy-MM-dd") + "'";

                if (!txtExtrato.Text.Equals(""))
                {
                    sql += " AND SUBSTRING(NF.ID, 26, 9) = '" + txtExtrato.Text.Trim().PadLeft(9, '0') + "'";
                }

                sql += " UNION ALL";
                sql += " SELECT DOC.Documento, SUBSTRING(Doc.Id_Chave, 26, 9) as Nro_extrato_sat,";
                sql += " ISNULL(doc.Nro_Serie_Equipamento, '') as Nro_Serie_Equipamento, Doc.Caixa AS Caixa,";
                sql += " Doc.data, Doc.ID_Chave, Doc.ID_Chave_Cancelamento, Doc.Cfe_XML, Doc.CFE_XML_Cancelamento";
                sql += " FROM Documento_Eletronico doc";
                sql += " WHERE doc.Filial = '" + usr.getFilial() + "'";
                sql += " AND DATA BETWEEN '" + Funcoes.dtTry(txtDataDe.Text).ToString("yyyy-MM-dd") + "' AND '" + Funcoes.dtTry(txtDataAte.Text).ToString("yyyy-MM-dd") + "'";
                sql += " AND DOC.Tipo = 3";

            }

            SqlDataReader rs = null;

            List<DocumentoEletronicoDAO> Documentos = new List<DocumentoEletronicoDAO>();
            
            
            try
            {
                rs = Conexao.consulta(sql, usr,false);
                while (rs.Read())
                {
                    DocumentoEletronicoDAO doc = new DocumentoEletronicoDAO()
                    {
                        Documento = rs["Documento"].ToString(),
                        Nro_extrato_Sat = rs["Nro_extrato_Sat"].ToString(),
                        Nro_Serie_Equipamento = rs["Nro_Serie_Equipamento"].ToString(),
                        Caixa = rs["Caixa"].ToString(),
                        Data = Funcoes.dtTry(rs["Data"].ToString()),
                        ID_Chave = rs["ID_Chave"].ToString(),
                        ID_Chave_Cancelamento = rs["ID_Chave_Cancelamento"].ToString(),
                        CFe_XML = rs["CFe_XML"].ToString(),
                        CFe_XML_Cancelamento = rs["CFe_XML_Cancelamento"].ToString()
                    };
                    Documentos.Add(doc);
                    
                }
                if (Documentos.Count > 100)
                {
                    lblQtdRegistros.Text = "100 de "+ Documentos.Count.ToString();

                }
                else
                {
                    lblQtdRegistros.Text = Documentos.Count.ToString();
                }
                Session.Remove("Documentos" + urlSessao());
                Session.Add("Documentos" + urlSessao(), Documentos);
                if (Documentos.Count > 0 && Documentos.Count > 100)
                {
                    List<DocumentoEletronicoDAO> arr = Documentos.GetRange(0,100);
                    
                    gridDocumentos.DataSource =  arr;
                    gridDocumentos.DataBind();
                }
                else
                {
                    gridDocumentos.DataSource = Documentos;
                    gridDocumentos.DataBind();
                }
            }
            catch (Exception erro)
            {

                msgShow(erro.Message, true);
            }
            finally
            {
                if (rs != null)
                    rs.Close();

            }
        }

        
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void msgShow(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            btnOkError.Focus();
            modalError.Show();
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }
        
        protected void imgBtnLimpar_Click(object sender, EventArgs e)
        {
            Session.Remove("filtro" + urlSessao());
            Response.Redirect("DocumentosEletronicos.aspx");
        }

        protected void btnBaixarArquivos_Click(object sender, EventArgs e)
        {
            try
            {

                TimerGerar.Interval = 450;
                TimerGerar.Enabled = true;
                Session.Remove("resultadoGerarDocs");
                Session.Remove("erroGerarDocs");
                Session.Remove("abortaGerarDocs");

                System.Threading.Thread th = new System.Threading.Thread(arquivoZipDocs);
                th.Start();


            }
            catch (Exception err)
            {
                msgShow(err.Message, true);
            }


        }
        protected void TimerGerar_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["resultadoGerarDocs"];
            String error = (String)Session["erroGerarDocs"];
            String aborta = (String)Session["abortaGerarDocs"];

            btnFecharXml.Enabled = false;
            btnFecharXml.Text = "Aguarde...";

            if (resultado != null)
            {
                lblMsgXml.Text = resultado;

                if (lblMsgXml.Text.Contains("ARQUIVOS GERADOS COM SUCESSO"))
                {
                    btnFecharXml.Enabled = true;
                    btnFecharXml.Text = "OK";
                    TimerGerar.Enabled = false;

                    lblMsgXml.ForeColor = System.Drawing.Color.Blue;

                    Session.Remove("resultadoGerarDocs");

                    String strEnd = (String)Session["endArquivo"];

                    RedirectNovaAba(strEnd);
                    Session.Remove("endArquivo");
                }


            }


            if (error != null)
            {


                TimerGerar.Enabled = false;
                Session.Remove("erroGerarDocs");
                modalGerar.Hide();
                msgShow(error, true);

            }

            modalGerar.Show();

            if (aborta != null)
            {
                TimerGerar.Enabled = false;
                modalGerar.Hide();
                Session.Remove("abortaGerarDocs");


            }
        }


        protected void arquivoZipDocs()
        {
            try
            {

                DateTime dtDe = Funcoes.dtTry(txtDataDe.Text);
                DateTime dtAte = Funcoes.dtTry(txtDataAte.Text);

                User usr = (User)Session["User"];
                String strPastaZip = "";
                List<DocumentoEletronicoDAO> Documentos = (List<DocumentoEletronicoDAO>)Session["Documentos" + urlSessao()];


                int nArq = 0;
                foreach (DocumentoEletronicoDAO doc in Documentos)
                {
                    try
                    {
                        if (strPastaZip.Equals(""))
                        {
                            switch (DllTipo.Text)
                            {
                                case "1":
                                    strPastaZip = Server.MapPath("~/modulos/Sped/pages/" + DateTime.Now.ToString("yyyyMMdd") + "-" + usr.getFilial());
                                    break;
                                case "2":
                                    strPastaZip = Server.MapPath("~/modulos/Sped/pages/" + DateTime.Now.ToString("yyyyMMdd") + "-Saida-" + usr.getFilial());
                                    break;
                                case "3":
                                    strPastaZip = Server.MapPath("~/modulos/Sped/pages/" + DateTime.Now.ToString("yyyyMMdd") + "-Entrada-" + usr.getFilial());
                                    break;
                            }
                        }

                        if (!Directory.Exists(strPastaZip))
                        {
                            Directory.CreateDirectory(strPastaZip);
                        }
                        string strNomeArquivo = strPastaZip + "/" + doc.ID_Chave + "_" + doc.Caixa + "_" + doc.Data.ToString("yyyyMMdd") + ".xml";
                        nArq++;
                        StreamWriter xml = new StreamWriter(strNomeArquivo, true, Encoding.ASCII);
                        xml.Write(doc.CFe_XML);
                        xml.Close();
                        if (doc.CFe_XML_Cancelamento.Length > 0)
                        {
                            string strNomeArquivoCanc = strPastaZip + "/" + doc.ID_Chave_Cancelamento + "_" + doc.Caixa + "_" + doc.Data.ToString("yyyyMMdd") + "_Canc.xml";
                            nArq++;
                            StreamWriter xmlCanc = new StreamWriter(strNomeArquivoCanc, true, Encoding.ASCII);
                            xmlCanc.Write(doc.CFe_XML_Cancelamento);
                            xmlCanc.Close();
                        }

                        Session.Remove("resultadoGerarDocs");
                        String msg = "";
                        if (nArq > 0)
                        {
                            msg = "LOCALIZANDO ARQUIVOS...  " + nArq.ToString() + " ARQUIVOS ENCONTRADOS";
                        }
                        else
                        {
                            msg = "AGUARDE LOCALIZANDO ARQUIVOS";
                        }
                        Session.Add("resultadoGerarDocs", msg);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (nArq == 0)
                {
                    throw new Exception("Nenhuma arquivo foi encontrado");
                }

                using (ZipFile zip = new ZipFile())
                {
                    Session.Add("resultadoGerarDocs", "AGUARDE COMPACTANDO " + nArq.ToString() + " ARQUIVOS ENCONTRADOS" );
                    // percorre todos os arquivos da lista
                    // se o item é uma pasta
                    if (Directory.Exists(strPastaZip))
                    {
                        try
                        {
                            // Adiciona a pasta no arquivo zip com o nome da pasta 
                            zip.AddDirectory(strPastaZip, new DirectoryInfo(strPastaZip).Name);
                        }
                        catch
                        {
                            throw;
                        }
                    }

                    // Salva o arquivo zip para o destino
                    try
                    {
                        zip.Save(strPastaZip + ".zip");

                        Directory.Delete(strPastaZip, true);
                        Session.Remove("resultadoGerarDocs");
                        Session.Add("resultadoGerarDocs", nArq.ToString() + " ARQUIVOS GERADOS COM SUCESSO");
                        Session.Remove("endArquivo");
                        Session.Add("endArquivo", "~\\modulos\\financeiro\\pages\\BaixarArquivo.aspx?endereco=" + strPastaZip.Replace("\\", "/") + ".zip");

                    }
                    catch
                    {
                        throw;
                    }
                }

            }
            catch (Exception err)
            {
                modalGerar.Hide();
                Session.Add("erroGerarDocs", err.Message);
            }



        }
        protected void btnFecharXml_Click(object sender, EventArgs e)
        {
            modalGerar.Hide();
        }
    }
}