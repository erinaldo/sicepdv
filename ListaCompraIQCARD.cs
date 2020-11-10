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
    public partial class ListaCompraIQCARD : Form
    {
        List<ServiceReference1.ListaCompraCompartilhamento> listagem = new List<ServiceReference1.ListaCompraCompartilhamento>();
        public ListaCompraIQCARD()
        {
            InitializeComponent();
        }

        private void ListaCompraIQCARD_Load(object sender, EventArgs e)
        {
            try
            {
                IqCard iqcard = new IqCard();
                listagem = iqcard.ListaCompras(1);
                dtgLista.DataSource = (from n in listagem select new { idCartao = n.idCartao, nome = n.nomeCartao }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void dtgLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == 0)
            {
                var idCartao = dtgLista.CurrentRow.Cells["idCartao"].Value.ToString();
                var nome = dtgLista.CurrentRow.Cells["nome"].Value.ToString();
                ListaCompraItens.idCartao = idCartao;
                ListaCompraItens.nome = nome;
                ListaCompraItens lista = new ListaCompraItens();
                lista.ShowDialog();
            }
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {         
            if(txtProcura.Text=="")
            {
                dtgLista.DataSource = (from n in listagem select new { idCartao = n.idCartao, nome = n.nomeCartao }).ToList();
                return;
            }
            var listagemProcura = (from n in listagem where n.nomeCartao.Contains(txtProcura.Text.ToUpper()) select n).ToList();
            dtgLista.DataSource = (from n in listagemProcura select new { idCartao = n.idCartao, nome = n.nomeCartao }).ToList();
        }
    }
}
