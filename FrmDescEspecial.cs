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
    public partial class FrmDescEspecial : FormIQ
    {
        decimal valorVenda = 0;
        decimal percDesconto = 0;
        public FrmDescEspecial(decimal totalTransacao)
        {
            valorVenda = totalTransacao;
            InitializeComponent();
            lblDescontoPerc.Text = "";
            txtDesconto.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };
            txtDescPerc.KeyPress += (objeto, evento) =>
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtDescPerc.Leave += (objeto, evento) =>
                {
                    txtDescPerc.Text = Funcoes.FormatarDecimal(txtDescPerc.Text);
                    txtDesconto.Text = string.Format("{0:N2}", totalTransacao * Convert.ToDecimal(txtDescPerc.Text) / 100);
                    txtDesconto.Focus();
                };

            txtDesconto.Leave += (objeto, evento) =>
                {
                    try
                    {
                        txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);
                        percDesconto = Convert.ToDecimal(txtDesconto.Text) / totalTransacao * 100;
                        lblDescontoPerc.Text = "Desconto (%): " + string.Format("{0:N2}", percDesconto);
                    }
                    catch
                    {
                        txtDesconto.Text = "0,00";
                    }
                    
                };
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtDesconto.Text) == 0)
            {
                Close();
                return;
            }
            
                FrmLogon logon = new FrmLogon();
                logon.campo = "vendescontogerencial";
                logon.txtDescricao.Text ="Vlr.Venda : "+string.Format("{0:N2}",valorVenda)+" "+ lblDescontoPerc.Text+" Valor Desconto R$: "+txtDesconto.Text;
                logon.ShowDialog();
                if (Operador.autorizado)
                {
                    Close();
                }
                else
                {
                    txtDesconto.Text = "0.00";
                    return;
                }

                if (percDesconto > Configuracoes.descontoMaxGerencial && Configuracoes.descontoMaxGerencial>0)
                {
                    MessageBox.Show("% Max. desconto gerencial é de :" + Configuracoes.descontoMaxGerencial);
                    txtDesconto.Text = "0.00";
                    txtDesconto.Focus();
                    return;
                }

            Close();
        }

        private void FrmDescEspecial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtDesconto.Text = "0,00";
            Close();
        }

        private void FrmDescEspecial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}
