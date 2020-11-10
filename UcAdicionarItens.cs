using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class UcAdicionarItens : UserControl
    {
        Produtos dadosProduto = new Produtos();
        public delegate void ClicarNoBotao(object sender, string tecla);
        public event ClicarNoBotao clickBotao;

        public UcAdicionarItens()
        {
            InitializeComponent();
            txtQtd.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtPreco.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtDesc.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtQtd.Leave += (objeto, evento) =>
                {
                  txtQtd.Text = Funcoes.FormatarDecimal(txtQtd.Text,3);
                  txtQtd.Text = Funcoes.FormatarValorPositivo(txtQtd.Text);
                  txtDesc.Focus();
                };
            cboCFOP.KeyPress += (objeto, evento) =>
                {
                    evento.Handled = true;
                    
                };

            txtDesc.Leave += (objeto, evento) =>
            {
                txtDesc.Text = Funcoes.FormatarDecimal(txtDesc.Text);
                txtDesc.Text = Funcoes.FormatarValorPositivo(txtDesc.Text);
                if (Convert.ToDecimal(txtDesc.Text) > 0)
                {                    
                    txtPreco.Text = string.Format("{0:n2}", dadosProduto.preco * (Convert.ToDecimal(txtDesc.Text) / 100));
                    txtPreco.Text = Funcoes.FormatarValorPositivo(txtPreco.Text);
                    Lancar();
                    clickBotao(null, "incluir");
                    return;
                };
                txtPreco.Focus();
            };
            txtCodigo.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return && txtCodigo.Text.Length== 0 )
                    ChamarProdutos();
                };

            txtPreco.Leave += (objeto, evento) =>
            {
                txtPreco.Text = Funcoes.FormatarDecimal(txtPreco.Text);
                txtPreco.Text = Funcoes.FormatarValorPositivo(txtPreco.Text);
                btnIncluir.Focus();
            };
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "")
            {
                return;               
            }

            try
            {              
                if (dadosProduto.ProcurarCodigo(txtCodigo.Text, GlbVariaveis.glb_filial))
                {
                    lblDescricao.Text = dadosProduto.descricao;
                    txtPreco.Text = string.Format("{0:n2}", dadosProduto.preco);
                    txtQtd.Focus();
                }                
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                txtCodigo.SelectAll();
                txtDesc.Text = "0.00";
                txtCodigo.Focus();
            }
        }

        private void ChamarProdutos()
        {
            FrmProdutos frmprd = new FrmProdutos();
            FrmProdutos.consultarPreco = false;
            frmprd.ShowDialog();
            txtCodigo.Text = FrmProdutos.ultCodigo;
        }

        private void UcAdicionarItens_Load(object sender, EventArgs e)
        {
            var dados = (from n in Conexao.CriarEntidade().nfoperacao
                         where n.codigo.StartsWith("5") || n.codigo.StartsWith("6")
                         select new { codigo = n.codigo, descricao = n.codigo + " " + n.descricao }).ToList();

            cboCFOP.DataSource = null;            
            cboCFOP.DataSource = dados;
            cboCFOP.ValueMember = "codigo";
            cboCFOP.DisplayMember = "descricao";
            cboCFOP.Text = "5.102";
            txtCodigo.Focus();
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {            
            Lancar();
            clickBotao(sender, "incluir");
        }

        private void Lancar()
        {
            if (cboCFOP.Text == "" || cboCFOP.Text == null)
                cboCFOP.Text = "5.102";

            try
            {
                string compDescricao = "";
                string lote = "";
                string grade = "Nenhuma";
                if (dadosProduto.descricaoComplementar)
                {
                    frmComplementoItem complemento = new frmComplementoItem();
                    complemento.ShowDialog();
                    lote = complemento.txtLote.Text;
                    compDescricao = complemento.txtComplemento.Text;
                    complemento.Dispose();
                }

                if (dadosProduto.grade.ToLower() != "nenhuma")
                {
                    FrmEscolherGrade frmgrade = new FrmEscolherGrade(dadosProduto.codigo, GlbVariaveis.glb_filial);
                    frmgrade.ShowDialog();
                    grade = frmgrade.gradeProduto;
                }



                Venda item = new Venda();
                item.InserirItem(false, txtCodigo.Text, dadosProduto.descricao,compDescricao, dadosProduto.quantidadeDisponivel,dadosProduto.quantidadePrat,
                    Convert.ToDecimal(txtQtd.Text), Convert.ToDecimal(txtPreco.Text), dadosProduto.preco,
                    dadosProduto.custo, dadosProduto.unidade,dadosProduto.embalagem, Convert.ToDecimal(txtDesc.Text), 0m, Venda.vendedor,
                    Convert.ToInt16(dadosProduto.icms), dadosProduto.tributacao, 0, dadosProduto.tipo, lote, grade, dadosProduto.aceitaDesconto, cboCFOP.Text.Substring(0, 5), dadosProduto.pis, dadosProduto.cofins, 0, dadosProduto.ncm, "I", false, dadosProduto.cest);
                txtCodigo.Focus();
                txtCodigo.Text = "";
                txtQtd.Text = "0.00";
                txtDesc.Text = "0.00";
                txtPreco.Text = "0.00";

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
