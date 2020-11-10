using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SICEpdv
{
    public partial class FrmSPEDPisCofins : Form
    {
        public FrmSPEDPisCofins()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            string arquivoDestino = @"c:\iqsistemas\sice.net\exportacao\SEF\sped_PIS_COFINS.txt";
            FrmMsgOperador msg = new FrmMsgOperador("", "Pode demorar vários minutos. Gerando EFD .");
            msg.Show();
            Application.DoEvents();
            try
            {
                ///Somente CFOPS 1.2
                /// PRA AS NFE DE ENTRADAS e SAIDAS EXPORTADOS APENAS os CFOPS
 
                //1.102 -  entradas de dentro do estado
                //2.102 - entradas de fora do estado
 
                //______________________________________
 
                //PARA AS NFE DE SAIDAS E CUPOM FISCAL
 
                //5.102 - SAIDA PRA DENTRO DO ESTADO  (CFOP UTILIZADO PRA SAIDA COM CUPOM FISCAL)
                //6.102 - SAIDA PRA FORA DO ESTADO


                //List<ModeloDocFiscal> lstModelo = new List<ModeloDocFiscal>();
                //ModeloDocFiscal docModelo = new ModeloDocFiscal();

                //docModelo.modeloDocFiscal = "02";
                //docModelo.tipo = "Saida";
                //lstModelo.Add(docModelo);

                var docExportacao = new[]                 
                    {
                        new ModeloDocFiscal{modeloDocFiscal="02",tipo="Saida"}, //Nota Fiscal de venda ao consumidor, modelo 2
                        new ModeloDocFiscal{modeloDocFiscal="2D",tipo="Saida"},   // Cupom Fiscal emitido por ECF                       
                        new ModeloDocFiscal{modeloDocFiscal="55",tipo="Entrada"},
                        new ModeloDocFiscal{modeloDocFiscal="01",tipo="Saida"},
                        new ModeloDocFiscal{modeloDocFiscal="55",tipo="Saida"}
                    };

                SPEDPisCofins EFD = new SPEDPisCofins();
                EFD.dataInicial = dataInicial.Value;
                EFD.dataFinal = dataFinal.Value;
                EFD.modeloDocFiscal = docExportacao.ToList();
                EFD.gerarRegC100Entrada = rdEntradas.Checked;
                EFD.gerarRegC100Saida = rdSaidas.Checked;
                EFD.gerarRegC400 = rdMapa.Checked;
                EFD.gerarRegC400 = false;
                EFD.gerarRegC490 = true;
               
                if (chkTodos.Checked)
                {
                    EFD.gerarRegC100Entrada = true;
                    EFD.gerarRegC100Saida = true;
                    EFD.gerarRegC400 = true;

                }

                if (chkGerarC400.Checked == true)
                {
                    EFD.gerarRegC400 = true;
                    EFD.gerarRegC490 = false;
                }
                else
                {
                    EFD.gerarRegC400 = false;
                    EFD.gerarRegC490 = true;
                }


                foreach (var item in chkLstFiliais.CheckedItems)
                {
                    if (item.ToString().Substring(0,5) != GlbVariaveis.glb_filial)
                    EFD.filiaisEFD.Add(item.ToString().Substring(0,5));
                }

                // Aqui para colocar o movimento da matriz sempre na última posição
                // senão dar erro no validor 
                EFD.filiaisEFD.Add(GlbVariaveis.glb_filial);

                var conteudo = EFD.GerarEFD();

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

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
            {
                rdEntradas.Checked = true;
                rdSaidas.Checked = true;
                rdMapa.Checked = true;
            }
        }

        private void FrmSPEDPisCofins_Load(object sender, EventArgs e)
        {
            dataInicial.Value = Convert.ToDateTime("01/" + DateTime.Now.Date.ToString("MM/yyyy"));
            dataFinal.Value = dataInicial.Value.AddMonths(1).AddDays(-1);

            var dadosFiliais = (from n in Conexao.CriarEntidade().filiais
                              where n.ativa == "S"
                              select n.CodigoFilial+" "+n.descricao).ToList();
            chkLstFiliais.Items.Clear();
            foreach (var item in dadosFiliais)
            {                
                chkLstFiliais.Items.Add(item);   
                  
            }

            for (int i = 0; i < chkLstFiliais.Items.Count; i++)
            {
                chkLstFiliais.SetItemChecked(i, true);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\iqsistemas\sice.net\exportacao\sef"); 
        }
    }
}
