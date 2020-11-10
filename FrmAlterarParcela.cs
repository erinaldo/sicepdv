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
    public partial class FrmAlterarParcela : Form
    {
        siceEntities entidade = Conexao.CriarEntidade();
        string formaPagamento = "CR";
        public static decimal totalParcelas = 0;
        int idCliente = 0;
        decimal totalPagamento = 0;

        public FrmAlterarParcela(string pagamento,decimal total,int codCliente)
        {
            formaPagamento = pagamento;
            totalParcelas = total;
            idCliente = codCliente;
            InitializeComponent();

            txtValor.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtCodBanco.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtAgencia.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtCPFCNPJch.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtNrCheque.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);

            txtValor.Leave += (objeto, evento) =>
                {                    
                        txtValor.Text = Funcoes.FormatarDecimal(txtValor.Text);
                };
            txtCPFCNPJch.Leave += (objeto, evento) =>
            {
                if (txtCPFCNPJch.Text.Length <= 11)
                {
                    if (!Funcoes.ValidarCPF(txtCPFCNPJch.Text))
                    {
                        MessageBox.Show("CNPJ ou CPF errado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtCPFCNPJch.Focus();
                    }
                }
                if (txtCPFCNPJch.Text.Length > 11)
                {
                    if (!Funcoes.ValidaCnpj(txtCPFCNPJch.Text))
                    {
                        MessageBox.Show("CNPJ ou CPF errado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtCPFCNPJch.Focus();
                    }
                }
            };

            totalPagamento = (from c in entidade.caixas
                                where c.EnderecoIP == GlbVariaveis.glb_IP
                                select c.valor).Sum();

            lblTotalOriginal.Text = "Total Venda.: "+totalPagamento.ToString("N2");
            lblTotalCalculado.Text = "Total Calculado.: "+totalPagamento.ToString("N2");
        }

        private void FrmAlterarParcela_Shown(object sender, EventArgs e)
        {
            MostrarParcelas();
        }

        private void MostrarParcelas()
        {
            var parcelas = from n in entidade.caixas
                           where n.EnderecoIP == GlbVariaveis.glb_IP
                           && n.tipopagamento == formaPagamento
                           orderby n.vencimento
                           select new
                           {
                               id = n.id,
                               vencimento = n.vencimento,
                               valor = n.valor,
                               nome = n.nome,
                               tipopagamento = n.tipopagamento,
                               parcela = n.Nrparcela
                           };

            dtgParcelas.DataSource = parcelas.AsQueryable();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            Venda.ApagarItensFormaPagamento("Pagamentos",formaPagamento);
            totalParcelas = 0;         
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            var soma = (from n in entidade.caixas
                        where n.EnderecoIP == GlbVariaveis.glb_IP
                        && n.tipopagamento == formaPagamento
                        select n.valor).Sum();
            if (soma != totalParcelas)
            {
                Venda.ApagarItensFormaPagamento("Pagamentos",formaPagamento);
                totalParcelas = 0;
            }
            Close();
        }

        private void btAlterar_Click(object sender, EventArgs e)
        {
            if (!Permissoes.alterarParcelamento)
            {                
                    FrmLogon Logon = new FrmLogon();
                    Operador.autorizado = false;
                    Logon.idCliente = idCliente;                
                    Logon.campo = "clialterarvencimento";                    
                    Logon.lblDescricao.Text = "Alterar vencimento e valores das parcelas";
                    Logon.ShowDialog();
                    if (!Operador.autorizado) return;                    
            }
            Int32 id = Convert.ToInt32(dtgParcelas.CurrentRow.Cells["id"].Value);
            pnlAlterar.Visible = true;
            pnlAlterar.Location = new Point(46, 47);
            pnlCheque.Visible = false;
            if (formaPagamento == "CH")
                pnlCheque.Visible = true;

            var dados = (from n in entidade.caixas
                        where n.id == id
                        select n).First();
            txtVencimento.Value = dados.vencimento.Value;
            txtValor.Text = string.Format("{0:N2}", dados.valor);
            txtCodBanco.Text = dados.banco;
            txtAgencia.Text = dados.agencia;
            txtNrCheque.Text = dados.cheque.ToString();
            txtNomeCH.Text = dados.NomeCheque;
            txtCPFCNPJch.Text = dados.cpfcnpjch;
            txtVencimento.Focus();

        }

        private void txtNrCheque_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConfirmarAlteracao_Click(object sender, EventArgs e)
        {
            if (formaPagamento == "CH")
            {
                if (txtCodBanco.Text == "" || txtAgencia.Text == "" || txtNomeCH.Text == "" || txtCPFCNPJch.Text == "")
                {
                    MessageBox.Show("Todos os campos devem ser preenchidos");
                    return;
                };
            }

            if (txtVencimento.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Vencimento não pode ser inferior a data atual.");
                txtVencimento.Value = DateTime.Now.Date;
                return;
            }
            
           

            Int32 id = Convert.ToInt32(dtgParcelas.CurrentRow.Cells["id"].Value);
            var dados = (from n in entidade.caixas
                         where n.id == id
                         select n).First();

            dados.vencimento = txtVencimento.Value;
            dados.valor = Convert.ToDecimal(txtValor.Text);
            dados.banco = txtCodBanco.Text;
            dados.agencia = txtAgencia.Text;
            dados.cheque = Convert.ToInt32("0"+txtNrCheque.Text);
            dados.NomeCheque = txtNomeCH.Text;
            dados.cpfcnpjch = txtCPFCNPJch.Text;            
            entidade.SaveChanges();

            //marckvaldo 2015-11-01
            if (chkSugerirParcelas.Checked == true)
            {
                var parcelas = (from p in entidade.caixas
                                where p.id != id && p.EnderecoIP == GlbVariaveis.glb_IP
                                select p).ToList();

                decimal valorParcelas = (totalPagamento - dados.valor) / parcelas.Count();

                foreach (var item in parcelas)
                {
                    item.valor = valorParcelas;
                    entidade.SaveChanges();
                }

                var novototalPagamento = (from c in entidade.caixas
                                          where c.EnderecoIP == GlbVariaveis.glb_IP
                                          select c.valor).Sum();

                var parcela = (from p in entidade.caixas
                                where p.id != id && p.EnderecoIP == GlbVariaveis.glb_IP
                                select p).FirstOrDefault();

                if (novototalPagamento != totalPagamento)
                {
                    parcela.valor = parcela.valor - (novototalPagamento - totalPagamento);

                    entidade.SaveChanges();
                }

            }

            var totalCalculado = (from c in entidade.caixas
                                  where c.EnderecoIP == GlbVariaveis.glb_IP
                                  select c.valor).Sum();

            lblTotalCalculado.Text = "Total Calculado.: " + totalCalculado.ToString("N2");

            MostrarParcelas();
            pnlAlterar.Visible = false;
        }

        private void btnCancelarAlteracao_Click(object sender, EventArgs e)
        {
            pnlAlterar.Visible = false;
        }

        private void FrmAlterarParcela_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }
    }
}
