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
    public partial class FrmKitVenda : FormIQ
    {
        public static int kitNumero = 0;
        UcKitVenda mostraKitVenda = new UcKitVenda();  

        public FrmKitVenda()
        {
            InitializeComponent();
        }

        private void FrmKitVenda_Load(object sender, EventArgs e)
        {
            this.Controls.Add(mostraKitVenda);
            this.mostraKitVenda.clickBotao += new UcKitVenda.ClicarNoCurrentBotao(Fechar);
            this.Size = new Size(mostraKitVenda.Size.Width + 5, mostraKitVenda.Size.Height + 25);
        }

        void Fechar(object sender)
        {
            this.Close();
        }

        private void FrmKitVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}
