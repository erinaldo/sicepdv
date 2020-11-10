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
    public partial class FrmCOF : Form
    {

        public static int idCOF;
        public FrmCOF()
        {
            InitializeComponent();
        }

        private void FrmCOF_Load(object sender, EventArgs e)
        {
            var dados = from n in Conexao.CriarEntidade().cof
                        select n;            
            dtgCOF.DataSource = dados.AsQueryable();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            idCOF = 0;
            this.Close();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            idCOF = Convert.ToInt16(dtgCOF.CurrentRow.Cells["id"].Value);           
            this.Close();
        }
        
    }
}
