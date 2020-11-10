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
    public partial class LancarPontosIQCARD : Form
    {
        public static int idCliente = 0;
        public LancarPontosIQCARD()
        {
            InitializeComponent();
        }

        private void btnLancar_Click(object sender, EventArgs e)
        {
            if (idCliente > 0)
            {
                if (IqCard.VerificarNumeroCartao(txtIQCARDFidelidade.Text))
                {
                    string sql = "UPDATE clientes set cartaofidelidade='" + txtIQCARDFidelidade.Text + "' WHERE codigo='" + idCliente.ToString() + "' LIMIT 1";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    Close();
                }
                else
                {
                    MessageBox.Show("IQCard inválido");
                }
            }

        }
    }
}
