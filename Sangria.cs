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
    public partial class Sangria : Form
    {        
        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        private string controle = "";
        private string tecla = "";
        private int devolucaoNumero = 0;
        private decimal totalDevolucao = 0;


        public Sangria()
        {
            InitializeComponent();
            teclado.TabStop = false;
            pnlTeclado.Controls.Add(teclado);
            // Enter Eventos
            txtValor.Enter += (objeto, evento) => controle = ActiveControl.Name;
            chkCredito.Enter += (objeto, evento) => controle = ActiveControl.Name;
            chkDebito.Enter += (objeto, evento) => controle = ActiveControl.Name;
            cboConta.Enter += (objeto, evento) => controle = ActiveControl.Name;
            cboSubConta.Enter += (objeto, evento) => controle = ActiveControl.Name;
            cboContaBancaria.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtCheque.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtHistorico.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtCheque.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto,evento);

            txtValor.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };
            txtValor.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                   txtValor.Text=Funcoes.FormatarDecimal(txtValor.Text);
                    if (evento.KeyCode == Keys.Escape)
                        this.Close();
                };

            txtValor.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    {
                        txtValor.Text = Funcoes.FormatarDecimal(txtValor.Text);
                        //evento.SuppressKeyPress = true;
                    }
                };

            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);
            cboConta.Enter += (objeto, evento) => PreencheConta();

            cboSubConta.Enter += (objeto, evento) => PreencheSubConta();
            this.Load += (objeto, evento) =>
            {
                txtValor.SelectAll();
                txtValor.Focus();
            };
            cboSubConta.Leave += (objeto,evento) =>
                {                                        
                    chkCredito.Checked = Despesas.ContaCredito(GlbVariaveis.glb_filial,"CR",cboConta.Text.PadLeft(5, '0').Substring(0, 5), cboSubConta.Text.PadLeft(5, '0').Substring(0, 5));
                    chkDebito.Checked = Despesas.ContaCredito(GlbVariaveis.glb_filial, "DB", cboConta.Text.PadLeft(5, '0').Substring(0, 5), cboSubConta.Text.PadLeft(5, '0').Substring(0, 5));

                };

            btnSair.Click += (objeto, evento) => this.Close();
            cboContaBancaria.Enter += (objeto, evento) =>
                {
                    PreencheContaBancaria();                    
                };
            chkCredito.Click += (objeto, evento) =>
                {
                    if (chkCredito.Checked == true) chkDebito.Checked = false;
                };
            chkDebito.Click += (objeto, evento) =>
                {
                    if (chkDebito.Checked == true) chkCredito.Checked = false;
                };
                
        }

        void DelegateTeclado(object sender, string text)
        {
            tecla = text;
            switch (tecla)
            {
                case "X":
                    break;
                case "Enter":
                    TeclaEnter();
                    break;
                case "Limpar":
                    Control[] ctls = this.Controls.Find(controle, true);
                    if (ctls[0] is TextBox)
                    {
                        TextBox txtBox = ctls[0] as TextBox;
                        txtBox.Text = "";
                        txtBox.Focus();
                    };
                    break;
                default:
                    PreencheCampo();
                    break;
            }
        }

        private void TeclaEnter()
        {
            switch (controle)
            {
                case "txtHistorico":
                   
                    break;
                default:
                    if (controle == "") return;
                    Control[] ctls = this.Controls.Find(controle, true);
                    ctls[0].Focus();
                    SendKeys.Send("{TAB}");
                    break;
            }
        }

        private void PreencheCampo()
        {
            if (controle == "") return;
            Control[] ctls = this.Controls.Find(controle, true);
            TextBox txtBox = ctls[0] as TextBox;
            if (txtBox.Enabled == false || txtBox.Text.Length>txtBox.MaxLength)
                return;
            txtBox.Text += this.tecla;
        }

        private void PreencheConta()
        {
            cboSubConta.SelectedIndex = -1;
            if (cboConta.Items.Count > 0) return;
            try
            {
                Despesas conta = new Despesas();
                cboConta.DataSource = conta.Contas(GlbVariaveis.glb_filial);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PreencheSubConta()
        {
            try
            {
                Despesas subconta = new Despesas();
                cboSubConta.DataSource = subconta.SubContas(GlbVariaveis.glb_filial, cboConta.Text.PadLeft(5, '0').Substring(0, 5), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PreencheContaBancaria()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            cboContaBancaria.DataSource = from n in entidade.contasbanco
                                          select n.conta;
            cboContaBancaria.SelectedIndex = -1;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirma sangria ?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            
            txtValor.Text = Funcoes.FormatarDecimal(txtValor.Text);

            if (chkCaixa.Checked && string.IsNullOrEmpty(cboCaixa.Text))
            {
                MessageBox.Show("Escolha um caixa de destino ou desmarque a opção de transferência de suprimento");
                return;
            }

            if (txtValor.Text == "")
            {
                erroValor.SetError(txtValor, "Digite um valor");
                txtValor.Focus();
                return;
            }
                    
            Despesas mov = new Despesas();
            FrmMsgOperador msg = new FrmMsgOperador("", "Efetuando sangria");
            try
            {
                msg.Show();
                Application.DoEvents();
                if (cboPagamento.Text == "")
                    throw new Exception("Escolha um tipo de pagamento");

                mov.Lancar(decimal.Parse(txtValor.Text), cboConta.Text.PadLeft(5, '0').Substring(0, 5), cboSubConta.Text.PadLeft(5, '0').Substring(0, 5), cboContaBancaria.Text.Trim(), Convert.ToUInt32(txtCheque.Text.PadLeft(1, '0')), chkCredito.Checked, chkDebito.Checked, txtHistorico.Text, cboPagamento.Text.Substring(0, 2), devolucaoNumero,cboCaixa.Text);

            }
            catch (Exception erro)
            {
                MessageBox.Show("Restrição: ! " + erro.Message);
                return;
            }
            finally
            {
                msg.Dispose();
            }

            btnDevolucao.BackColor = System.Drawing.SystemColors.Control;
            devolucaoNumero = 0;
            FrmDevolucao.numeroDevolucao = 0;
            totalDevolucao = 0;
            erroValor.SetError(txtValor, "");
            txtValor.Text="";
            txtCheque.Text = "";
            txtHistorico.Text = "";
            cboContaBancaria.SelectedIndex = -1;
            chkCredito.Checked = false;
            chkDebito.Checked = false;
            txtValor.Focus();
        }

        private void Sangria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
            {
                FuncoesPAFECF.ChamarMenuFiscal();
            }
            
        }

        private void ConsultarSangria(object sender, EventArgs e)
        {
            FrmConsultarSangria FrmConsulta = new FrmConsultarSangria();
            FrmConsulta.ShowDialog();
        }

        private void Sangria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }

            

        }

        private void btnDevolucao_Click(object sender, EventArgs e)
        {
            FrmDevolucao dev = new FrmDevolucao();
            dev.ShowDialog();
            if (FrmDevolucao.numeroDevolucao > 0)
            {
                btnDevolucao.BackColor = System.Drawing.Color.GreenYellow;
                devolucaoNumero = FrmDevolucao.numeroDevolucao;
                totalDevolucao = FrmDevolucao.totalDevolucao;
                txtValor.Text = string.Format("{0:N2}", totalDevolucao);
                
            }
        }

        private void Sangria_Load(object sender, EventArgs e)
        {
            cboPagamento.SelectedIndex = 0;
        }

        private void cboCaixa_Enter(object sender, EventArgs e)
        {
                   
        }

        private void chkCaixa_Click(object sender, EventArgs e)
        {
            if (chkCaixa.Checked)
            {
                grpOpcao.Enabled = false;
                grbBanco.Enabled = false;
                chkCredito.Checked = false;
                chkDebito.Checked = false;
                

                var dados = (from n in Conexao.CriarEntidade().caixa
                             orderby n.operador
                             where n.data == GlbVariaveis.Sys_Data    
                             && n.operador != GlbVariaveis.glb_Usuario
                             select n.operador).Distinct();
                cboCaixa.DataSource = dados.ToList();
                cboConta.SelectedItem = -1;
            }
            else
            {
                cboCaixa.Enabled = false;
                grpOpcao.Enabled = true;
                grbBanco.Enabled = true;
                cboCaixa.SelectedIndex = -1;

            }
        }

    }
}
