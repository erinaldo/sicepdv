using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class PagamentoOnLine : Form
    {
        public static bool cartaoPresente = false;
        public static int diasRestante { get; set; }
        public static string linkPagSeguro = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.idCliente.PadLeft(5, '0') + "sicenet?valor=0";
        public static string linkPayPal = "https://iqcard.com.br/Empresa/Comprar/" + GlbVariaveis.idCliente.PadLeft(5, '0') + "sicenetpaypal?valor=0";
        public static string produto = "mensalidade";
        public static double total = 0;
        public static int quantidade = 0;

        public PagamentoOnLine()
        {
            InitializeComponent();
            this.Text = "A licença se expira em " + diasRestante.ToString() + " dias.";
           
        }

        private void gerarBoleto_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://siceweb.dyndns.info:88/boletoIQ/app_listar_debitos/app_listar_debitos.php?p_codigo=" + 501);
        }

        private void pagSeguro_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkPagSeguro);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkPayPal);
        }

        private void PagamentoOnLine_Load(object sender, EventArgs e)
        {
            if (cartaoPresente)
            {
                gerarBoleto.Visible = false;
                pnlInfo.Visible = false;
            }

            if (produto == "recarga")
            {

                gerarBoleto.Visible = false;
                NumberFormatInfo format = new NumberFormatInfo();
                // Set the 'splitter' for thousands
                format.NumberGroupSeparator = ",";
                // Set the decimal seperator
                format.NumberDecimalSeparator = ".";
                this.Text = "RECARGA DE PONTOS " + quantidade + " TOTAL R$: " + string.Format("{0:N2}", total);

                linkPagSeguro = "https://iqcard.com.br/Eticket/PagSeguroCompraTicket?idEmpresa=" + GlbVariaveis.glb_chaveIQCard + "&produto=RECARGA PONTOS IQCARD&preco=" + string.Format("{0:N2}", total * 100, format) + "&quantidade=" + quantidade + "&pontos=25";
                linkPayPal = "https://iqcard.com.br/Eticket/PayPalCompraTicket?idEmpresa=" + GlbVariaveis.glb_chaveIQCard + "&produto=RECARGA PONTOS IQCARD&preco=" + total.ToString() + "&quantidade=" + quantidade + "&pontos=25";
            }

        }
    }
}
