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
    public partial class MesclarItens : Form
    {
        public int davNumero = 0;
        public MesclarItens(int numeroDAV)
        {
            davNumero = numeroDAV;            
            InitializeComponent();
            dgItens.DataSource = (from n in Conexao.CriarEntidade().vendadav where n.documento == numeroDAV select n).AsQueryable();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgItens_DoubleClick(object sender, EventArgs e)
        {
            string sql = "";
            try
            {
                string id = Convert.ToString(dgItens.CurrentRow.Cells["inc"].Value);
                string total = Funcoes.FormatarDecimal(dgItens.CurrentRow.Cells["total"].Value.ToString());
                int doc = Convert.ToInt32(dgItens.CurrentRow.Cells["documento"].Value);

                sql = "UPDATE vendadav SET itemDAV=if(itemDAV='S','N','S') WHERE inc='" + id + "'";
                var numerodav = Conexao.CriarEntidade().ExecuteStoreCommand(sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" "+sql);
                MessageBox.Show(ex.InnerException.ToString());
            }
           dgItens.DataSource = (from n in Conexao.CriarEntidade().vendadav where n.documento == davNumero select n).AsQueryable();
        }

    }
}
