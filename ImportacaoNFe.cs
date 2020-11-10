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
    public partial class ImportacaoNFe : Form
    {        
        public static int numero = 0;
        public static string tipo;
        public ImportacaoNFe()
        {
            InitializeComponent();
            txtNumero.KeyPress += (objeto, evento) =>
            {
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            };
        }

        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        public enum TipoImportacao
        {
            entrada,
            entradaDevolucao,
            entradaTransf,
            saidaDevolucao,
            saidaTransf,
            saidaPerdas
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (tipo==TipoImportacao.entrada.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().moventradas
                             where n.numero == numero
                             && n.Codigofilial == GlbVariaveis.glb_filial
                             select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault()==0)
                {
                    MessageBox.Show("Número da entrada não foi encontrado.");
                        numero=0;
                        return;
                }

                this.Close();
            }

            if (tipo == TipoImportacao.entradaDevolucao.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().contdevolucao
                              where n.numero == numero
                              && n.finalizada=="S"                              
                              select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault() == 0)
                {
                    MessageBox.Show("Número da devolução não foi encontrado ou devolução não foi encerrada.");
                    numero = 0;
                    return;
                }                

                this.Close();
            }


            if (tipo == TipoImportacao.entradaTransf.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().conttransf
                              where n.numero == numero
                              && n.FilialDestino == GlbVariaveis.glb_filial          
                              && (n.lancada =="S" || n.lancada=="X")
                              select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault() == 0)
                {
                    MessageBox.Show("Número da transferência não foi encontrado ou não foi encerrada.");
                    numero = 0;
                    return;
                }

                this.Close();
            }


            if (tipo == TipoImportacao.saidaTransf.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().conttransf
                              where n.numero == numero
                              && n.filialorigem == GlbVariaveis.glb_filial
                              && (n.lancada == "S" || n.lancada == "X")
                              select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault() == 0)
                {
                    MessageBox.Show("Número da transferência não foi encontrado ou não foi encerrada.");
                    numero = 0;
                    return;
                }

                this.Close();
            }


            if (tipo == TipoImportacao.saidaDevolucao.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().contvencidos
                              where n.numero == numero                              
                              select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault() == 0)
                {
                    MessageBox.Show("Número da devolução não foi encontrado.");
                    numero = 0;
                    return;
                }

                this.Close();
            }


            if (tipo == TipoImportacao.saidaPerdas.ToString())
            {
                numero = Convert.ToInt32(txtNumero.Text);
                int? dados = (from n in Conexao.CriarEntidade().contperdas
                              where n.numero == numero
                              && (n.encerrada=="S" || n.encerrada=="X")
                              select (int?)n.numero).FirstOrDefault();

                if (dados.GetValueOrDefault() == 0)
                {
                    MessageBox.Show("Número de controle não foi encontrado.");
                    numero = 0;
                    return;
                }

                this.Close();
            }

            numero =Convert.ToInt32(txtNumero.Text);
            this.Close();
        }
    }

   
    
}
