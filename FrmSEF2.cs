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
    public partial class FrmSEF2 : Form
    {
        public FrmSEF2()
        {
            InitializeComponent();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            string arquivoDestino = @"c:\iqsistemas\sice.net\exportacao\SEF\SEF2.txt";
            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando EFD");
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

                SEF2 EFD = new SEF2();
                EFD.dataInicial = dataInicial.Value;
                EFD.dataFinal = dataFinal.Value;
                EFD.modeloDocFiscal = docExportacao.ToList();
                EFD.numeroInventario = Convert.ToInt32(txtNrInventario.Value);
                EFD.anoInventario = Convert.ToInt16(txtAnoInv.Text);

                var conteudo = EFD.GerarSEF2();

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
    }
}
