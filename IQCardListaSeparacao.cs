using Microsoft.Reporting.WinForms;
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
    public partial class IQCardListaSeparacao : Form
    {
        public IQCardListaSeparacao()
        {
            InitializeComponent();
        }

        private void IQCardListaSeparacao_Load(object sender, EventArgs e)
        {
            this.ItensPedidoBindingSource.DataSource = IqCard.itensPedidoIQCard.ToList();            
            ReportParameter idPedido = new ReportParameter("idPedido", IqCard.DadosPedido.RowKey);
            ReportParameter dataPedido = new ReportParameter("dataPedido",  IqCard.DadosPedido.data.ToString("dd/MM/yyyy hh:mm:ss"));
            ReportParameter nome = new ReportParameter("nome",IqCard.DadosPedido.nomeCartao);
            ReportParameter telefone = new ReportParameter("telefone", IqCard.dadosCartao.telefone);
            ReportParameter statusPagamento = new ReportParameter("statusPagamento", IqCard.DadosPedido.statusPagamento);
            ReportParameter totalPedido = new ReportParameter("totalPedido", string.Format("{0:N2}", IqCard.DadosPedido.totalOrcamento));
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { idPedido, dataPedido, nome, telefone, statusPagamento, totalPedido });
            this.reportViewer1.RefreshReport();            
        }
    }
}
