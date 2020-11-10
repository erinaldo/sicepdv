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
    public partial class frmIndiceTecnicoProducao : Form
    {
        public string codigoPrd;        
        public frmIndiceTecnicoProducao(string filtrarCodigo)
        {
            codigoPrd = filtrarCodigo;
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmIndiceTecnicoProducao_Load(object sender, EventArgs e)
        {
            MostrarIndices(); 
        }

        private void MostrarIndices()
        {
            var dados = from n in Conexao.CriarEntidade().produtoscomposicao
                        where n.codigo == codigoPrd
                        select n;
            if (dados.Count() == 0)
                return;
            else
            {
                dtgProdutos.DataSource = from n in dados select new { n.codigomateria, n.descricaomateria, n.quantidade, n.unidade };
                lblProduto.Text = dados.First().codigo + " " + dados.First().descricao;
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmLogon Logon = new FrmLogon();
            Operador.autorizado = false;
            Logon.campo = "gerente";
            Logon.lblDescricao.Text = "Incluir Índice Técnico de produção";
            Logon.ShowDialog();
            if (!Operador.autorizado) return;           

            
            if (codigoPrd == "")
            {
                return;
            }
            frmCadastroIndiceTecProducao frmCadastro = new frmCadastroIndiceTecProducao(codigoPrd);
            frmCadastro.ShowDialog();
            MostrarIndices();
        }

        private void btAdicionar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Excluir o índice ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                Produtos.ApagarComposicao(codigoPrd, Convert.ToString(dtgProdutos.CurrentRow.Cells["codigo"].Value));
                MostrarIndices();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível exclir: "+erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                throw;
            }
        }
    }
}
