using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace SICEpdv
{
    public partial class FrmNFCePendentes : Form
    {

        public List<documentosXML> listNFCePendentes = new List<documentosXML>();
        public DateTime dataFinal;
        public DateTime dataIncio;
        public string codigoFilial;

        public FrmNFCePendentes()
        {
            InitializeComponent();
        }

        private void FrmNFCePendentes_Load(object sender, EventArgs e)
        {
            ReportParameter Titulo = new ReportParameter("Titulo", "RELATÓRIO DE NFC-e PENDENTES");
            ReportParameter Empresa = new ReportParameter("Empresa", Configuracoes.razaoSocial + "," + Configuracoes.endereco + ", " + Configuracoes.bairro + ", " + Configuracoes.cidade + " - " + Configuracoes.estado);
            ReportParameter CodigoFilial = new ReportParameter("CodigoFilial", codigoFilial);
            ReportParameter Periodo = new ReportParameter("Periodo", "Período: " + dataIncio.ToString("dd/MM/yyyy") + " a " + dataFinal.ToString("dd/MM/yyyy"));
            ReportParameter DataRelatorio = new ReportParameter("DataRelatorio", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            ReportParameter Versao = new ReportParameter("Versao", GlbVariaveis.glb_Versao);
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { Titulo, Empresa, CodigoFilial, Periodo, DataRelatorio, Versao });
            this.documentosXMLBindingSource.DataSource = listNFCePendentes;
            this.reportViewer1.RefreshReport();
        }
    }
}
