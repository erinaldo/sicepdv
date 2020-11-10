using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class FormAtualizacao : Form
    {
        public FormAtualizacao()
        {
            InitializeComponent();
        }

        private void FormAtualizacao_Shown(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt"))
                File.Delete(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt");

            timer1.Enabled = true;

            Funcoes.escreveArquivo(@"C:\iqsistemas\IQSync\Local\atualizarAgora.txt", "atualizarahora!");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\iqsistemas\IQSync\Todo\aplicarAtualizacao.txt"))
            {
                timer1.Enabled = false;
                MessageBox.Show("Atualização concluída, por favor, encerre o SICEpdv!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
    }
}
