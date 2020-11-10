using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class FuncoesImpressao
    {

        static public string conteudo = "";

        /*
        public void PrintLoad()
        {

            crediario();
            this.rpvImpressao.RefreshReport();
            this.rpvImpressao.SetDisplayMode(DisplayMode.PrintLayout);

        }

        public void PrintReport()
        {

            this.rpvImpressao.PrintDialog();

        }

        private void crediario()
        {

            var filial = (from f in Conexao.CriarEntidade().filiais
                          where f.CodigoFilial == GlbVariaveis.glb_filial
                          select f).FirstOrDefault();

            StringBuilder descricaoTitulo = new StringBuilder();
            descricaoTitulo.AppendLine(filial.fantasia);
            descricaoTitulo.AppendLine(filial.empresa);
            descricaoTitulo.AppendLine(filial.endereco + ", " + filial.numero + ", " + filial.bairro);
            descricaoTitulo.AppendLine(filial.cidade + " - " + filial.estado + " CEP.:" + filial.cep);
            StringBuilder descricaoCabecalho = new StringBuilder();
            descricaoCabecalho.AppendLine("===================================");
            descricaoCabecalho.AppendLine("CNPJ.: "+filial.cnpj+" IE.: "+filial.inscricao);
            descricaoCabecalho.AppendLine(DateTime.Now.Date.ToShortDateString() + " : " + DateTime.Now.Date.ToShortTimeString());
            descricaoCabecalho.AppendLine("===================================");

            StringBuilder msgRodapeCupom = new StringBuilder();
            msgRodapeCupom.AppendLine("Estou ciente do débito abaixo relacionado. ");
            msgRodapeCupom.AppendLine("Pelo qual pagarei no vencimento");

            var dados = (from n in Conexao.CriarEntidade().crmovclientes
                        orderby n.vencimento
                        where n.documento == documento && n.codigo == codCLiente
                        select n).ToList();

            string nomeCliente = "";

            if(dados != null)
                nomeCliente = dados.First().nome;

            decimal totalDebito = 0;
            foreach (var item in dados)
            {
                msgRodapeCupom.AppendLine(string.Format("{0:dd/MM/yyyy}", item.datacompra) + " " + string.Format("{0:dd/MM/yyy}", item.vencimento) + " " + string.Format("{0:C2}", item.valoratual));
                totalDebito += item.valoratual;
            }
            msgRodapeCupom.AppendLine("===================================");
            msgRodapeCupom.AppendLine("TOTAL R$:" + string.Format("{0:N2}", totalDebito));
            msgRodapeCupom.AppendLine("" + Environment.NewLine);
            msgRodapeCupom.AppendLine("Ass:___________________________________");
            msgRodapeCupom.AppendLine(codCLiente.ToString() + " " + nomeCliente);

            StringBuilder msgRodaPe = new StringBuilder();
            msgRodaPe.AppendLine("===================================");
            msgRodaPe.AppendLine("Usuario.:" + GlbVariaveis.glb_Usuario);
            msgRodaPe.AppendLine("SICEpdv Versão " + GlbVariaveis.glb_Versao+".");


            ReportParameter titulo = new ReportParameter("titulo", descricaoTitulo.ToString());
            ReportParameter cabecalho = new ReportParameter("cabecalho", descricaoCabecalho.ToString());
            ReportParameter conteudo = new ReportParameter("conteudo", msgRodapeCupom.ToString());
            ReportParameter rodaPe = new ReportParameter("rodaPe", msgRodaPe.ToString());

            rpvImpressao.LocalReport.SetParameters(new ReportParameter[] { titulo, cabecalho, conteudo, rodaPe });
            this.rpvImpressao.RefreshReport();
        }

        private void rpvImpressao_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            this.rpvImpressao.PrintDialog();
            this.Close();
            
        }

*/

        public static int contagemLinhas()
        {
            string[] conteudoArray = conteudo.ToString().Split('\n');
            int i = 95;
            foreach (var lihna in conteudoArray)
            {
                i = i + 15;
            }

            return i + 45;

        }
        
        //impressaoDialog
        public static void impressaoDialogSerial()
        {
            
            //printDoc.DefaultPageSettings.PaperSize =
            //printDoc.PrinterSettings.PaperSizes[comboPaperSize.SelectedIndex];

            
            PrintDocument objPrintDoc = new PrintDocument();            
            objPrintDoc.PrintPage +=   new PrintPageEventHandler(impressaoDocumento);
            //PaperSize papel = new PaperSize("IQ sistemas", 305, contagemLinhas());
            //objPrintDoc.DefaultPageSettings.PaperSize = papel;
            //objPrintDoc.PrinterSettings.DefaultPageSettings.PaperSize = papel;
            objPrintDoc.Print();
            conteudo = "";
 
        }


        public static void impressaoDialog()
        {
            try
            {

                SerialPort SerialPorta = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
                //SerialPorta.Encoding = System.Text.Encoding.UTF32;
                SerialPorta.Encoding = Encoding.GetEncoding(1251);

               
                

                if (!(SerialPorta.IsOpen))
                {
                    SerialPorta.Open();

                    var filial = (from f in Conexao.CriarEntidade().filiais
                                  where f.CodigoFilial == GlbVariaveis.glb_filial
                                  select f).FirstOrDefault();

                    SerialPorta.Write(filial.fantasia + "\r\n");
                    SerialPorta.Write(filial.empresa + "\r\n");
                    SerialPorta.Write(filial.endereco + ", " + filial.numero + ", " + filial.bairro + "\r\n");
                    SerialPorta.Write(filial.cidade + " - " + filial.estado + " CEP.:" + filial.cep + "\r\n");
                    SerialPorta.Write("===================================" + "\r\n");
                    SerialPorta.Write("CNPJ.: " + filial.cnpj + "   IE.: " + filial.inscricao + "\r\n");
                    SerialPorta.Write(DateTime.Now.Date.ToShortDateString() + " : " + DateTime.Now.Date.ToShortTimeString() + "\r\n");
                    SerialPorta.Write("===================================" + "\r\n");

                    string[] conteudoArray = conteudo.ToString().Split('\n');

                    foreach (var lihna in conteudoArray)
                    {
                        SerialPorta.Write(lihna.ToString() + "\r\n");
                        
                    }

                    SerialPorta.Write("===================================" + "\r\n");
                    SerialPorta.Write("Usuario.:" + GlbVariaveis.glb_Usuario + "\r\n");
                    SerialPorta.Write("SICEpdv Versão " + GlbVariaveis.glb_Versao + "\r\n");

                    SerialPorta.Close();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("erro" + erro.Message);
            }
            
        }


        public static void impressaoDocumento(object sender, PrintPageEventArgs e)
        {

            var filial = (from f in Conexao.CriarEntidade().filiais
                          where f.CodigoFilial == GlbVariaveis.glb_filial
                          select f).FirstOrDefault();

            e.Graphics.DrawString(filial.fantasia, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 10);
            e.Graphics.DrawString(filial.empresa, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 25);
            e.Graphics.DrawString(filial.endereco + ", " + filial.numero + ", " + filial.bairro, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 35);
            e.Graphics.DrawString(filial.cidade + " - " + filial.estado + " CEP.:" + filial.cep, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 45);
            e.Graphics.DrawString("===================================", new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 55);
            e.Graphics.DrawString("CNPJ.: " + filial.cnpj + "   IE.: " + filial.inscricao, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 65);
            e.Graphics.DrawString(DateTime.Now.Date.ToShortDateString() + " : " + DateTime.Now.Date.ToShortTimeString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 75);
            e.Graphics.DrawString("===================================", new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, 85);
           

            string[] conteudoArray = conteudo.ToString().Split('\n');
            int i = 95;

            foreach (var lihna in conteudoArray)
            {
                e.Graphics.DrawString(lihna.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, i);
                i = i + 15;
            }

            e.Graphics.DrawString("===================================", new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, i);
            e.Graphics.DrawString("Usuario.:" + GlbVariaveis.glb_Usuario, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, i+15);
            e.Graphics.DrawString("SICEpdv Versão " + GlbVariaveis.glb_Versao + ".", new Font("Arial", 9, FontStyle.Regular), Brushes.Black, 10, i+30);

            
        }

        
    }

   
}
