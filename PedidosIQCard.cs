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
    public partial class PedidosIQCard : Form
    {

        public PedidosIQCard()
        {
            InitializeComponent();
        }

        private void PedidosIQCard_Load(object sender, EventArgs e)
        {
            var dados = IqCard.PedidosIQCard(Venda.IQCard);
            lblIQCard.Text = Venda.IQCard;
            lblNome.Text = IqCard.dadosCartao.nome;
            lblTelefone.Text = Funcoes.FormatarTelefone(IqCard.dadosCartao.telefone);
            lblEndereco.Text = IqCard.dadosCartao.endereco + " " + IqCard.dadosCartao.numero + " " + IqCard.dadosCartao.bairro + " " + IqCard.dadosCartao.cidade + " " + IqCard.dadosCartao.estado;

            dtPedidos.DataSource = null;
            dtPedidos.DataSource = (from n in dados select new { data = n.data, valor = n.totalOrcamento, observacao = n.observacao, RowKey = n.RowKey,status=n.status }).ToList();
            ItensPedidoIQCard.gerarVenda += ItensPedidoIQCard_gerarVenda;
        }

        private void ItensPedidoIQCard_gerarVenda(object sender, EventArgs e)
        {
            Close();
        }

        private void dtPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == 0)
            {
                string idPedido = dtPedidos.CurrentRow.Cells["RowKey"].Value.ToString();
                IqCard.DadosPedido.RowKey = idPedido;

                ItensPedidoIQCard itens = new ItensPedidoIQCard();
                itens.ShowDialog();
            }
        }
    }
}
