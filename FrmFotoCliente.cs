using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SICEpdv
{
    public partial class FrmFotoCliente : Form
    {
        public static int codCli = 0;
        public FrmFotoCliente()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmFotoCliente_Load(object sender, EventArgs e)
        {
            try
            {
            string codigoCliente = codCli.ToString();
            byte[] foto = (from n in Conexao.CriarEntidade().clientesfoto
                           where n.codcli == codigoCliente
                           select n.fotocli).First();

                MemoryStream ms = new MemoryStream(foto);
                pictureBox1.Image = Image.FromStream(ms);
            }
            catch
            {
                return;
            }
            
        }
    }
}
