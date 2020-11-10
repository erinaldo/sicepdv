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
    public partial class frmCadastroIndiceTecProducao : Form
    {
        string codigo;
        public frmCadastroIndiceTecProducao(string codProduto)
        {
            InitializeComponent();
            codigo = codProduto;
            lblProduto.Text = codigo;
            txtQtd.KeyPress += (objeto,evento) => Funcoes.DigitarNumerosPositivos(objeto,evento);
            txtQtd.Leave += (objeto, evento) =>
                {
                    txtQtd.Text = Funcoes.FormatarDecimal(txtQtd.Text,3);
                };
            txtCodigo.KeyDown += (objeto, evento) =>
                {
                    if (txtCodigo.Text == codProduto)
                    {
                        MessageBox.Show("Código não pode ser o mesmo do produto");
                        txtCodigo.Focus();
                        evento.SuppressKeyPress = false;                        
                    }
                    if (evento.KeyValue == 13)
                    {
                        lblDescricaoMateria.Text = (from n in Conexao.CriarEntidade().produtos
                                                   where n.codigo == txtCodigo.Text
                                                   select n.descricao).FirstOrDefault();
                    }
                };

        }

        private void frmCadastroIndiceTecProducao_Load(object sender, EventArgs e)
        {

            if (GlbVariaveis.glb_filial == "00001")
            {
                var dadosPrd = from n in Conexao.CriarEntidade().produtos
                               where n.codigo == codigo
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select new { n.codigo, n.descricao };

                lblProduto.Text = dadosPrd.First().codigo + " - " + dadosPrd.First().descricao;

                var dados = Produtos.Unidades();
                foreach (var item in dados)
                {
                    cboUnidade.Items.Add(item.unidade + " - " + item.descricao);
                }
            }

            if (GlbVariaveis.glb_filial != "00001")
            {
                var dadosPrd = from n in Conexao.CriarEntidade().produtosfilial
                               where n.codigo == codigo
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select new { n.codigo, n.descricao };

                lblProduto.Text = dadosPrd.First().codigo + " - " + dadosPrd.First().descricao;

                var dados = Produtos.Unidades();
                foreach (var item in dados)
                {
                    cboUnidade.Items.Add(item.unidade + " - " + item.descricao);
                }
            }


        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCadastroIndiceTecProducao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Produtos.GravarComposicao(codigo, txtCodigo.Text, Convert.ToDecimal(txtQtd.Text), cboUnidade.Text))
                {
                    MessageBox.Show("Índice de produção gravado ");
                    Close();
                }
                else
                    MessageBox.Show("Não foi possível incluir", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Exceção na inclusão "+erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void frmCadastroIndiceTecProducao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}
