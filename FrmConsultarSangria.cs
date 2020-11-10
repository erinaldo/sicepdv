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
    public partial class FrmConsultarSangria : Form
    {
        public FrmConsultarSangria()
        {
            InitializeComponent();
        }

        private void MostrarLancamentos()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            dtgSangria.DataSource = from n in entidade.movdespesas
                                    where n.operador == GlbVariaveis.glb_Usuario
                                    && n.data == GlbVariaveis.Sys_Data
                                    && n.sangria == "S"
                                    select new 
                                    { 
                                        n.id_inc,
                                        n.conta, 
                                        n.descricaoconta,
                                        n.subconta,
                                        n.descricaosubconta,
                                        n.valor,
                                        n.historico,
                                        n.hora
                                    };            
        }

        private void FrmConsultarSangria_Load(object sender, EventArgs e)
        {
            MostrarLancamentos();
        }

        private void Sair(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btEstornar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estornar valor ?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            Despesas estorno = new Despesas();

            try
            {
                estorno.Estornar(Convert.ToInt32(dtgSangria.CurrentRow.Cells["idincDataGridViewTextBoxColumn"].Value), Convert.ToDecimal(dtgSangria.CurrentRow.Cells["valorDataGridViewTextBoxColumn"].Value));
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            finally
            {
                MostrarLancamentos();
            }
        }
    }
}
