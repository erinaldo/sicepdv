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
    public partial class ListaCompraItens : Form
    {
        public static string idCartao;
        public static string nome;
        public ListaCompraItens()
        {
            InitializeComponent();
        }

        private void ListaCompraItens_Load(object sender, EventArgs e)
        {
            FrmMsgOperador msg3 = new FrmMsgOperador("", "Buscando lista");
            try
            {
                msg3.Show();
                Application.DoEvents();
                IqCard iqcard = new IqCard();
                var itens = iqcard.ListaComprasItens(idCartao,1);
                dtgItens.AutoGenerateColumns = false;
                dtgItens.DataSource = itens.ToList();
                lblIQCARD.Text = Funcoes.FormatarCartao(idCartao);
                lblNome.Text = nome;
                lblTotal.Text = string.Format("{0:N2}", (from n in itens select n.quantidade * n.preco).Sum()).ToString();
            }
            catch (Exception ex)
            {
                msg3.Dispose();
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimirPreco_Click(object sender, EventArgs e)
        {

        }
    }
}
