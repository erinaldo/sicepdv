using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class FrmFornecedores : Form
    {
        siceEntities entidade = Conexao.CriarEntidade();
        public static int codigoDestinatario;
        public static string tipoFiltro;

        public FrmFornecedores()
        {
            InitializeComponent();
            txtDadosFornecedores.Focus();
           
        }

        private void txtDadosFornecedores_TextChanged(object sender, EventArgs e)
        {

            if (RdEmprezaFornecedores.Checked == true)
            {
                DgFornecedores.DataSource = (from f in entidade.fornecedores
                                             where f.razaosocial.StartsWith(txtDadosFornecedores.Text)
                                             orderby f.razaosocial
                                             select new
                                             {
                                                 Codigo = f.Codigo,
                                                 Fornecedores = f.razaosocial ?? "",
                                                 CNPJ = f.CGC ?? "",
                                                 Inscricao = f.INSCRICAO ?? "",
                                                 Situacao = f.situacao ?? "",
                                                 Fone = f.TELEFONE ??""
                                             }).Take(100);


            }
            else 
            {

                DgFornecedores.DataSource = (from f in entidade.fornecedores
                                             where f.CGC.StartsWith(txtDadosFornecedores.Text) || f.INSCRICAO.StartsWith(txtDadosFornecedores.Text)
                                             orderby f.razaosocial
                                             select new
                                             {
                                                 Codigo = f.Codigo,
                                                 Fornecedores = f.razaosocial ?? "",
                                                 CNPJ = f.CGC ?? "",
                                                 Inscricao = f.INSCRICAO ?? "",
                                                 Situacao = f.situacao ?? "",
                                                 Fone = f.TELEFONE ??""
                                             }).Take(100);
            }


            DgFornecedores.Columns[0].Width = 50;
            DgFornecedores.Columns[1].Width = 300;
            DgFornecedores.Columns[2].Width = 120;
            DgFornecedores.Columns[3].Width = 120;
            DgFornecedores.Columns[4].Width = 120;
            DgFornecedores.Columns[5].Width = 120;
           

        }


        private void DgFornecedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

           lblEmpresaFornecedores.Text ="Empresa.: "+ DgFornecedores.Rows[linha].Cells[1].Value.ToString();
           lblCNPJFornecedores.Text = "CNPJ.: " + DgFornecedores.Rows[linha].Cells[2].Value.ToString();
           lblInscricaoFornecedores.Text = "Inscrição.: " + DgFornecedores.Rows[linha].Cells[3].Value.ToString();
           lblSituacaoFornecedores.Text = "Siruacao.: " + DgFornecedores.Rows[linha].Cells[4].Value.ToString();
           lblFoneFornecedores.Text = "Fone.: " + DgFornecedores.Rows[linha].Cells[5].Value.ToString();
        }

        
        private void SairFornecedores(object sender, EventArgs e)
        {
            if (DgFornecedores.RowCount > 0)
            {
                codigoDestinatario = Convert.ToInt32("0" + DgFornecedores.CurrentRow.Cells["Codigo"].Value);
                tipoFiltro = "FN";
            }
                
            this.Close();
        }

       
    }
}
