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
    public partial class FrmEscolherGrade : Form
    {
        string codProduto;
        string codFilial;
        public string gradeProduto = "Nenhuma";


        public FrmEscolherGrade(string codigo,string filial)
        {
            codProduto = codigo;
            codFilial = filial;
            InitializeComponent();
            dtgGrade.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    {
                        Escolher();
                        evento.Handled = true;
                    };
                };
        }

        private void FrmEscolherGrade_Load(object sender, EventArgs e)
        {
            dtgGrade.DataSource = from n in Conexao.CriarEntidade().produtosgrade
                                  where n.codigo == codProduto && n.codigofilial == codFilial
                                  select new { grade = n.grade, quantidade= n.quantidade };
            dtgGrade.Focus();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Escolher();
        }

        private void Escolher()
        {
            if (dtgGrade.RowCount > 0)
                this.gradeProduto = Convert.ToString(dtgGrade.CurrentRow.Cells["grade"].Value);
            Close();
        }

        private void FrmEscolherGrade_FormClosed(object sender, FormClosedEventArgs e)
        {
            Escolher();
        }
    }
}
