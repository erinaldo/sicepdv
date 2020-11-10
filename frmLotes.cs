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
    public partial class frmLotes : Form
    {
        string codigo = "";
        decimal quantidadeVenda = 0;

        public frmLotes(string codigo, decimal quantidade)
        {
            this.codigo = codigo;
            this.quantidadeVenda = quantidade;

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLotes_Load(object sender, EventArgs e)
        {
            try
            {
                var conexao = Conexao.CriarEntidade(true);

                var Lotes = conexao.ExecuteStoreQuery<produtoLote>("SELECT codigo as codigoProduto,fornecedor as codigoFornecedor,quantidade,lote,data_lote as vencimento,data_fabricacao as fabricacao, inc, " + quantidadeVenda + "  as quantidadeDigitada FROM produtos_lote WHERE codigo = '" + codigo + "'").ToList();
                dgLotes.DataSource = Lotes.ToList();
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                produtoLote l = new produtoLote();
                l.lote = dgLotes.CurrentRow.Cells["lote"].Value.ToString();
                l.fabricacao = DateTime.Parse(dgLotes.CurrentRow.Cells["fabricacao"].Value.ToString());
                l.codigoFornecedor = int.Parse(dgLotes.CurrentRow.Cells["codigoFornecedor"].Value.ToString());
                l.quantidade = decimal.Parse(dgLotes.CurrentRow.Cells["quantidade"].Value.ToString());
                l.vencimento = DateTime.Parse(dgLotes.CurrentRow.Cells["vencimento"].Value.ToString());
                l.quantidadeDigitada = quantidadeVenda;
                l.codigoProduto = codigo;


                var conexao = Conexao.CriarEntidade(true);

                int quantidadePrevenda = conexao.ExecuteStoreQuery<int>("select IFNULL(sum(quantidade),0) from vendas where lote = '"+l.lote+"' and vencimento = '"+l.vencimento.ToString("yyyy-MM-dd")+"' and datafabricacao = '"+l.fabricacao.ToString("yyyy-MM-dd") + "' and idfornecedor ='"+l.codigoFornecedor+"' and codigo = '"+l.codigoProduto+"' AND cancelado = 'N';").FirstOrDefault();

                if (l.quantidade < (quantidadeVenda + quantidadePrevenda))
                {
                    MessageBox.Show("Saldo insuficiente nos lotes!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    if (l.vencimento < DateTime.Now.Date)
                    {
                        MessageBox.Show("O lote escolhido está vencido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (l.vencimento < DateTime.Now.Date)
                {
                    MessageBox.Show("O lote escolhido está vencido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _pdv.listLotes.Add(l);
                    this.Close();
                }

            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }
           
        }

        private void dgLotes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1_Click(this, new EventArgs());
            }
            else if (e.KeyValue == 27)
            {
                this.Close();
            }
        }

        private void dgLotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dgLotes_KeyUp(object sender, KeyEventArgs e)
        {
           
        }
    }
}
