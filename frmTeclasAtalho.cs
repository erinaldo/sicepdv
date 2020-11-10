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
    public partial class frmTeclasAtalho : Form
    {
        public frmTeclasAtalho()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            FormAtualizacao objAtualizacao = new FormAtualizacao();
            objAtualizacao.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            System.Diagnostics.Process.Start("IExplore", Application.StartupPath+@"\Leiame.htm");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore","http://www.iqsistemas.com.br/ajuda");
        }

        private void frmTeclasAtalho_Load(object sender, EventArgs e)
        {
            if (GlbVariaveis.glb_atualizar == "N")
                btnSair.Enabled = false;
        }
    }
}
