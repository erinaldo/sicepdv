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
    public partial class PortalContabilidade : Form
    {
        public PortalContabilidade()
        {
            InitializeComponent();
        }

        private void PortalContabilidade_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = Configuracoes.emailContador;
            lblSenha.Text = Configuracoes.crccontador;
        }

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://siceweb.azurewebsites.net/Contador/Login");
        }
    }
}
