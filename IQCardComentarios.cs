using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class IQCardComentarios : Form
    {
        public IQCardComentarios()
        {
            InitializeComponent();
        }

        private void IQCardComentarios_Load(object sender, EventArgs e)
        {

            IqCard iqcard = new IqCard();
            
            var dados = iqcard.ListagemComentarios();
            dtgComentarios.AutoGenerateColumns = false;

            dtgComentarios.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dtgComentarios.DataSource = (from c in dados
                                         orderby c.data
                                         select new
                                         {
                                             nome = c.nomeCartao+"( "+Funcoes.FormatarCartao(c.idCartao)+" )",
                                             comentario = c.comentario,
                                             data = c.data.ToShortDateString()
                                         }).ToList();
        }
    }
}
