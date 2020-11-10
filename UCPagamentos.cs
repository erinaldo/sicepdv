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
    public partial class UCPagamentos : UserControl
    {        
        public delegate void Controle(string campo);
        public event Controle EntraControle;


        public UCPagamentos()
        {
            InitializeComponent();

            txtDesconto.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtDinheiro.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtCrediario.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtCheque.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);
            txtCartao.Enter += (objeto, evento) => EntraControle(ActiveControl.Name);

            txtDesconto.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
                
            };
            txtDesconto.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyValue == 13)
                {
                    txtDesconto.Text = Funcoes.FormatarDecimal(txtDesconto.Text);              
                    evento.SuppressKeyPress = true;
                }
            };
        }
    }
}
