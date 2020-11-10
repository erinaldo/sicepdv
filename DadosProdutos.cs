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
    public partial class DadosProdutos : Form
    {
        public DadosProdutos()
        {
            InitializeComponent();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtNCM_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCodBarras_KeyDown(object sender, KeyEventArgs e)
        {
            txtCodBarras.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };
        }

        private void txtCusto_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == 13)
            {
                txtCusto.Text = Funcoes.FormatarDecimal(txtCusto.Text);
            };

        }

        private void txtCusto_KeyPress(object sender, KeyPressEventArgs e)
        {                                    
                Funcoes.DigitarNumerosPositivos(sender, e);            
        }

        private void DadosProdutos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }
    }
}