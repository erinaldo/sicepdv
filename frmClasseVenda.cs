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
    public partial class frmClasseVenda : Form
    {
        public static string descricaoClasse = "";
        public static string codigoClasse = "0000";
        public frmClasseVenda()
        {
            InitializeComponent();
            dtgClasse.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyValue == 13)
                        PegarClasseVenda();
                };
            this.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Escape)
                        Sair();
                };
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Sair();
        }

        private void Sair()
        {
            descricaoClasse = "";
            codigoClasse = "0000";
            this.Close();
        }

        private void frmClasseVenda_Load(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            dtgClasse.DataSource = from n in entidade.classe
                                   orderby n.codigo
                                   where n.codigofilial == GlbVariaveis.glb_filial
                                   select new {codigo = n.codigo, descricao = n.descricao };
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            PegarClasseVenda();
        }

        private void PegarClasseVenda()
        {
            if (dtgClasse.RowCount > 0)
            {
                descricaoClasse = (dtgClasse.CurrentRow.Cells["descricao"].Value).ToString();
                codigoClasse = (dtgClasse.CurrentRow.Cells["codigo"].Value).ToString();
            }
            this.Close();
        }

        private void frmClasseVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}
