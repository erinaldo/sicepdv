using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SICEpdv
{
    public partial class FrmSPED : Form
    {
        public FrmSPED()
        {
            InitializeComponent();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            string arquivoDestino = @"c:\iqsistemas\sice.net\exportacao\SEF\sped_fiscal.txt";
            FrmMsgOperador msg = new FrmMsgOperador("", "Pode demorar vários minutos. Gerando SPED Fiscal.");
            msg.Show();
            Application.DoEvents();
            try
            {
                //List<ModeloDocFiscal> lstModelo = new List<ModeloDocFiscal>();
                //ModeloDocFiscal docModelo = new ModeloDocFiscal();
                
                //docModelo.modeloDocFiscal = "02";
                //docModelo.tipo = "Saida";
                //lstModelo.Add(docModelo);

                var docExportacao = new[]                 
                    {
                        new ModeloDocFiscal{modeloDocFiscal="02",tipo="Saida"}, //Nota Fiscal de venda ao consumidor, modelo 2
                        new ModeloDocFiscal{modeloDocFiscal="2D",tipo="Saida"},   // Cupom Fiscal emitido por ECF
                        new ModeloDocFiscal{modeloDocFiscal="Inventario",tipo="Inventario"},
                        new ModeloDocFiscal{modeloDocFiscal="55",tipo="Entrada"},
                        new ModeloDocFiscal{modeloDocFiscal="01",tipo="Saida"},
                        new ModeloDocFiscal{modeloDocFiscal="55",tipo="Saida"}
                    };

                SPEDFiscal EFD = new SPEDFiscal();
                EFD.dataInicial = dataInicial.Value;
                EFD.dataFinal = dataFinal.Value;
                EFD.modeloDocFiscal = docExportacao.ToList();
                EFD.numeroInventario = Convert.ToInt32(txtNrInventario.Value);
                EFD.anoInventario = Convert.ToInt16(txtAnoInv.Text);
                EFD.codigoLayout = cboEstrutura.Text.Trim();
                EFD.gerarRegC100Entrada = rdEntradas.Checked;
                EFD.gerarRegC100Saida = rdSaidas.Checked;
                EFD.gerarRegC400 = rdMapa.Checked;

                if (chkTodos.Checked)
                {
                    EFD.gerarRegC100Entrada = true;
                    EFD.gerarRegC100Saida = true;
                    EFD.gerarRegC400 = true;
                }


                var conteudo = EFD.GerarSPEDFiscal();

                using (FileStream fs = File.Create(@arquivoDestino))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                MessageBox.Show("Exportação realizada. Destino: " + arquivoDestino);
                
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível gerar o arquivo EFD agora: " + erro.Message, "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msg.Dispose();
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmSPED_Load(object sender, EventArgs e)
        {
            dataInicial.Value = Convert.ToDateTime("01/" + DateTime.Now.Date.ToString("MM/yyyy"));
            dataFinal.Value = dataInicial.Value.AddMonths(1).AddDays(-1);
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
            {
                rdEntradas.Checked = true;
                rdSaidas.Checked = true;
                rdMapa.Checked = true;
            }
        }
        
    }
}
