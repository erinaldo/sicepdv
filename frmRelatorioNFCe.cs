using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class frmRelatorioNFCe : Form
    {
        public List<dadosNFCe> listDadosNFCe = new List<dadosNFCe>();
        public DateTime dataFinal;
        public DateTime dataIncio;

        public frmRelatorioNFCe()
        {
            InitializeComponent();
        }
        

        private void frmRelatorioNFCe_Load(object sender, EventArgs e)
        {

            var valorCancelado = (from c in listDadosNFCe.ToList()
                                  where c.cancelado == true
                                  select c.total).Sum();
            //lblValorLiquido

            var valorBruto = (from c in listDadosNFCe.ToList()
                              select c.total).Sum();

            this.dadosNFCeBindingSource.DataSource = listDadosNFCe.ToList();
            ReportParameter titulo = new ReportParameter("lblTitulo", "Relatorio NFCe Periodo "+dataIncio.ToString("dd/MM/yyyy")+" a "+dataFinal.ToString("dd/MM/yyyy"));
            ReportParameter empresa = new ReportParameter("lblDataRelatorio", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            ReportParameter data = new ReportParameter("lblEmpresa", Configuracoes.razaoSocial + "," + Configuracoes.endereco+", "+Configuracoes.bairro+", "+Configuracoes.cidade+" - "+Configuracoes.estado);
            ReportParameter versao = new ReportParameter("lblVersao", "SICEpdv Versão "+GlbVariaveis.glb_Versao);
            ReportParameter cancelado = new ReportParameter("lblvCancelado", "R$ " + valorCancelado.ToString("N2"));
            ReportParameter valorLiquido = new ReportParameter("lblvLiquido", "R$ " + (valorBruto - valorCancelado).ToString("N2"));

            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { titulo, empresa, data, versao, cancelado, valorLiquido });
            this.reportViewer1.RefreshReport();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = reportViewer1.LocalReport.Render(
            "Pdf", null, out mimeType, out encoding,
             out extension,
            out streamids, out warnings);

            FileStream fs = new FileStream(@"xmls\relatoriaNFCe.pdf",
               FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}
