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
    public partial class FrmDadosOS : Form
    {
        public FrmDadosOS()
        {
            InitializeComponent();

            txtNF.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };

            txtNF.Focus();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (txtPlaca.Text.Substring(0, 1) != " ")
            {
                var dadosDAVOS = new DadosDAVOS();
                dadosDAVOS.nrfabricacao = txtNrFabricacao.Text;
                dadosDAVOS.anoFabricacao = Convert.ToInt16("0" + txtAno.Text);
                dadosDAVOS.marca = txtMarca.Text;
                dadosDAVOS.modelo = txtModelo.Text;
                dadosDAVOS.placa = txtPlaca.Text;
                dadosDAVOS.renavam = txtRENAVAM.Text;
                Venda.dadosDAVOS = dadosDAVOS;
                //dadosEntrega.observacao += "PLACA :" + txtPlaca.Text + " MARCA: " + txtMarca.Text + " ANO: " + txtAno.Text + " RENAVAM: " + txtRENAVAM.Text;
            };

            if (!string.IsNullOrEmpty(txtNrFabricacao.Text))
            {
                var dadosDAVOS = new DadosDAVOS();
                dadosDAVOS.nrfabricacao = txtNrFabricacao.Text;
                Venda.dadosDAVOS = dadosDAVOS;
            }
            if (string.IsNullOrEmpty(txtNF.Text))
            {
                txtNF.Text = "0";
            }

            Venda.numeroPED = Convert.ToInt32(txtNF.Text);

            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Venda.dadosDAVOS = new DadosDAVOS();
            Venda.numeroPED = 0;
            
            Close();
        }

        private void FrmDadosOS_Load(object sender, EventArgs e)
        {
            if (GlbVariaveis.glb_TipoPAF != GlbVariaveis.tipoPAF.Serviço.ToString())
            {
                tbPanel.Visible = false;
                this.Text = "Entre com os dados";
            }

            txtAno.Text = Venda.dadosDAVOS.anoFabricacao.ToString();
            txtMarca.Text = Venda.dadosDAVOS.marca ?? "";
            txtModelo.Text = Venda.dadosDAVOS.modelo ?? "";
            txtNrFabricacao.Text = Venda.dadosDAVOS.nrfabricacao ?? "";
            txtPlaca.Text = Venda.dadosDAVOS.placa ?? "";
            txtRENAVAM.Text = Venda.dadosDAVOS.renavam ?? "";            
        }

        private void FrmDadosOS_Shown(object sender, EventArgs e)
        {            
            txtNF.Focus();
        }
    }
}
