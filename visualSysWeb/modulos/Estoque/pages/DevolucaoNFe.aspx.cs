using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using visualSysWeb.dao;
using visualSysWeb.code;
using visualSysWeb.modulos.Estoque.dao;
using visualSysWeb.modulos.Cadastro.code;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class DevolucaoNFe :visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "SELECT DNFe.Codigo, Cliente.Nome_Cliente, Status = CASE WHEN ISNULL(DNFe.Status, 0) = 0 THEN 'PENDENTE' ELSE 'EMITIDO' END, DNFe.codigo_Cliente, DNFe.Data_Cadastro, DNFe.Total FROM Devolucao_NFe DNFe INNER JOIN Cliente ON DNFe.Codigo_Cliente = Cliente.Codigo_Cliente WHERE DNFe.Status IN (0)";
        static String UltimaOrdem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (!IsPostBack)
            {
                tb = Conexao.GetTable(sqlGrid + " order by DNFe.Codigo DESC", usr, true);
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();

            }
            pesquisar(pnBtn);

            if (!IsPostBack)
            {
                txtDataAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDataAte.MaxLength = 10;
                txtDataDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDataDe.MaxLength = 10;
            }
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/pedidos/pages/pedidoVendaDetalhes.aspx?novo=true");
        }


        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            pesquisar(e.SortExpression);

        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            UltimaOrdem = "";
            User usr = (User)Session["User"];
            DataTable tb;
            String sqlGrid = "SELECT DNFe.Codigo, Cliente.Nome_Cliente, Status = CASE WHEN ISNULL(DNFe.Status, 0) = 0 THEN 'PENDENTE' ELSE 'EMITIDO' END, DNFe.codigo_Cliente, DNFe.Data_Cadastro, DNFe.Total FROM Devolucao_NFe DNFe INNER JOIN Cliente ON DNFe.Codigo_Cliente = Cliente.Codigo_Cliente ";
            sqlGrid += " WHERE DNFe.Codigo > 0";

            //Checa o status selecionado
            if (DllStatus.Text.Equals("0"))
            {
                sqlGrid += " AND DNFe.Status = 0";
            }
            else if (DllStatus.Text.Equals("1"))
            {
                sqlGrid += " AND DNFe.Status = 1";
            }

            if (!txtDataDe.Text.Equals(""))
            {
                sqlGrid += " AND DNFe.Data_Cadastro BETWEEN '" + DateTime.Parse(txtDataDe.Text).ToString("yyyy-MM-dd") + "' AND '" + DateTime.Parse(txtDataAte.Text).ToString("yyyy-MM-dd") + " 23:59:59.000'";
            }

            if (!txtPLU.Text.Equals(""))
            {
                sqlGrid += " AND DNFe.Codigo IN(SELECT DNFei.Codigo FROM Devolucao_NFe_Item DNFei WHERE DNFei.plu = '" + txtPLU.Text + "')";
            }

            gridPesquisa.DataSource = null;
            
            tb = Conexao.GetTable(sqlGrid + " order by DNFe.Codigo DESC", usr, true);
            gridPesquisa.DataSource = tb;
            gridPesquisa.DataBind();


        }
        protected void pesquisar()
        {

            pesquisar(UltimaOrdem);

        }

        protected void pesquisar(string ordem)
        {
            String strOrdem = "";

            DataTable tb;
            String sqlGrid = "SELECT DNFe.Codigo, Cliente.Nome_Cliente, Status = CASE WHEN ISNULL(DNFe.Status, 0) = 0 THEN 'PENDENTE' ELSE 'EMITIDO' END, DNFe.codigo_Cliente, DNFe.Data_Cadastro, DNFe.Total FROM Devolucao_NFe DNFe INNER JOIN Cliente ON DNFe.Codigo_Cliente = Cliente.Codigo_Cliente WHERE DNFe.Status IN (1, 0)";


            try
            {
                lblPesquisaErro.Text = "";

            }

            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;

            }

        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }



        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }

        protected void exibeLista()
        {
        }

        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            if (int.TryParse(e.CommandArgument.ToString(), out index))
            {
                if (e.CommandName.Equals("EmitirNf"))
                {
                    HyperLink codigoCliente = (HyperLink)gridPesquisa.Rows[index].Cells[1].Controls[0];
                    string codigoClienteNF = codigoCliente.Text;
                    ClienteDAO clienteNFe = new ClienteDAO(codigoClienteNF.ToString(), null);
                    //if (clienteNFe.CEP.Equals("") || clienteNFe.endereco_nro.Equals("") || clienteNFe.email().Equals(""))
                    if (clienteNFe.CEP.Equals("") || clienteNFe.email().Equals(""))
                    {
                        lblPesquisaErro.Text = "Dados do clientes incompleto.";
                        lblPesquisaErro.Visible = true;
                        return;
                    }
                    else
                    {
                        if (clienteNFe.Endereco.Equals(""))
                        {
                            WSCEP cep = new WSCEP(clienteNFe.CEP);
                            //logradouro
                            if (cep.logradouro.Trim().Length > 100)
                            {
                                clienteNFe.Endereco = Funcoes.RemoverAcentos(cep.logradouro.ToUpper().Trim().Substring(0, 100));
                            }
                            else
                            {
                                clienteNFe.Endereco = Funcoes.RemoverAcentos(cep.logradouro.ToUpper().Trim());
                            }
                            //Bairro
                            if (cep.bairro.Trim().Length > 100)
                            {
                                clienteNFe.Bairro = Funcoes.RemoverAcentos(cep.bairro.ToUpper().Trim().Substring(0, 100));
                            }
                            else
                            {
                                clienteNFe.Bairro = Funcoes.RemoverAcentos(cep.bairro.ToUpper().Trim());
                            }
                            //Cidade
                            if (cep.localidade.Trim().Length > 100)
                            {
                                clienteNFe.Cidade = Funcoes.RemoverAcentos(cep.localidade.ToUpper().Trim().Substring(0, 100));
                            }
                            else
                            {
                                clienteNFe.Cidade = Funcoes.RemoverAcentos(cep.localidade.ToUpper().Trim());
                            }

                            if (clienteNFe.endereco_ent_nro.Equals(""))
                            {
                                clienteNFe.endereco_ent_nro = "100";
                            }

                            if (!clienteNFe.salvarEnderecoNFe())
                            {
                                lblPesquisaErro.Text = "Erro ao tentar atualizar dados do cliente.";
                                lblPesquisaErro.Visible = true;
                                return;
                            }
                        }
                    }



                    HyperLink mPed = (HyperLink)gridPesquisa.Rows[index].Cells[0].Controls[0];
                    String strPed = mPed.Text;
                    if (!strPed.Equals(""))
                    {
                        DevolucaoNFeDAO obj = new DevolucaoNFeDAO(int.Parse(strPed));
                        Session.Remove("objDevolucao" + urlSessao());
                        Session.Add("objDevolucao" + urlSessao(), obj);

                        Response.Redirect("~/modulos/notafiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&novo=true&DevolucaoImporta=" + strPed);
                    }
                }
            }

        }

    }
}