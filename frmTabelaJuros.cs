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
    public partial class frmTabelaJuros : FormIQ
    {
        public static decimal valorFinanciamento = 0;
        public static decimal valorOriginalFinanciamento = 0;
        public static decimal valorEntrada = 0;
        public static decimal totalFinanciado = 0;
        public static int parcelamento = 0;
        public static int intervalo = 30;
        public static decimal encargos = 0;
        public static decimal desconto = 0;
        public static string classe = "0000";
        public static string tipoPagamento = "00";
        public decimal entradaObrigatoria = 0;
        public static bool financiamentoCalculado = false;
        public static bool aceiraDesconto = true;
        public static bool descontoGerencial = false;
        public static int codigoTabelaJuros = 0;
        

        public frmTabelaJuros()
        {
            InitializeComponent();
            txtEntrada.KeyPress += (objeto, evento) =>
                {
                    Funcoes.DigitarNumerosPositivos(objeto, evento);
                };

            /*txtEntrada.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return)
                    ValidarEntrada();
                };*/
        }

        private void ValidarEntrada()
        {
            if (string.IsNullOrEmpty(txtEntrada.Text))
                txtEntrada.Text = "0.00";

            if (Convert.ToDecimal(txtEntrada.Text) > (valorFinanciamento + valorEntrada))
            {
                MessageBox.Show("Entrada não pode ser maior que valor financiado");
                txtEntrada.Text = "0.00";
                txtEntrada.Focus();
            }
            valorEntrada = Convert.ToDecimal(txtEntrada.Text);
            valorFinanciamento -= valorEntrada;
            txtEntrada.Text = valorEntrada.ToString();
            //txtEntrada.Text = string.Format("{0:N2}", valorEntrada);
            //txtEntrada.Enabled = false;

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            valorFinanciamento = 0;
            totalFinanciado = 0;
            valorEntrada = 0;
            parcelamento = 0;
            encargos = 0;
            desconto = 0;
            tipoPagamento = "";
            financiamentoCalculado = false;
            this.Close();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            MostrarTabela("1");
        }

        private void MostrarTabela(string tabela)
        {
            var dados = from n in Conexao.CriarEntidade().juros
                        where n.numero.EndsWith(tabela)
                        //where n.numero == "TabSheet1"
                        && n.codigofilial == GlbVariaveis.glb_filial
                        orderby n.parcelas
                        select new {n.Codigo, n.descricao, n.classe, n.parcelas, n.intervalo, n.coeficiente, n.juros1 };
            dtgTabela.DataSource = dados.AsQueryable();
            if (dtgTabela.Rows.Count == 0)
            {
                lblValorFinanciamento.Text = string.Format("{0:N2}", 0);
                lblEntradaObrigatoria.Text = string.Format("{0:N2}", 0);
                lblEncargos.Text = string.Format("{0:N2}", 0);
                lblTotalFinanciado.Text = string.Format("{0:N2}", 0);

                lblParcela.Text = "Valor Parcela: ";
                lblValorParcela.Text = string.Format("{0:N2}", 0);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            MostrarTabela("2");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            MostrarTabela("3");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            MostrarTabela("4");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            MostrarTabela("5");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            MostrarTabela("6");
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            MostrarTabela("7");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            MostrarTabela("8");
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            MostrarTabela("9");
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            MostrarTabela("10");
        }

        private void frmTabelaJuros_Load(object sender, EventArgs e)
        {
            txtEntrada.Focus();
            MostrarTabela("1");
        }

        private void btn11_Click(object sender, EventArgs e)
        {
            MostrarTabela("11");

        }

        private void btn12_Click(object sender, EventArgs e)
        {
            MostrarTabela("12");
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            MostrarTabela("13");
        }

        private void btn14_Click(object sender, EventArgs e)
        {
            MostrarTabela("14");
        }

        private void btn15_Click(object sender, EventArgs e)
        {
            MostrarTabela("15");
        }


        private void CalcularFinanciamento()
        {
            desconto = 0;
            if (dtgTabela.Rows.Count == 0)
            {
                return;
            }
            
            int idTabela = Convert.ToInt32(dtgTabela.CurrentRow.Cells["id"].Value);

            var dadosFinanc = (from n in Conexao.CriarEntidade().juros
                              where n.Codigo == idTabela
                              select n).FirstOrDefault();

            decimal valorParcela = 0;
            decimal valorEntradaObrigatoria = 0;

            if (Configuracoes.descontoAtacado == false && dadosFinanc.juros1 < 0)
            {
                valorFinanciamento = Venda.ObterValorDesconto() - valorEntrada; 
            }
            else
            {
                valorFinanciamento = Venda.ObterValorAcrescimo() - valorEntrada; 
            }

          
            totalFinanciado = valorFinanciamento;
            lblAceitaDesconto.Text = "("+dadosFinanc.aceitadescontos+")";

            valorParcela = totalFinanciado / dadosFinanc.parcelas.Value;

            if (dadosFinanc.juros1 != 0)
            {
                totalFinanciado = valorFinanciamento * (dadosFinanc.juros1.Value/ 100) + valorFinanciamento;
                valorParcela = totalFinanciado/dadosFinanc.parcelas.Value;
               
                if (dadosFinanc.entradaobrigatoria == "S")
                {
                    valorEntradaObrigatoria = valorParcela;
                    if (dadosFinanc.percentualentrada > 0)
                    {
                        valorEntradaObrigatoria = totalFinanciado * (dadosFinanc.percentualentrada / 100);
                    }
                }
            }

            if (dadosFinanc.coeficiente != 0)
            {
                totalFinanciado = valorFinanciamento * dadosFinanc.coeficiente.Value * dadosFinanc.parcelas.Value;
                valorParcela = totalFinanciado / dadosFinanc.parcelas.Value;

                if (dadosFinanc.entradaobrigatoria == "S")
                {
                    valorEntradaObrigatoria = valorParcela;
                    if (dadosFinanc.percentualentrada > 0)
                    {
                        valorEntradaObrigatoria = totalFinanciado * (dadosFinanc.percentualentrada / 100);
                    }
                }
            }

            this.entradaObrigatoria = valorEntradaObrigatoria;
            encargos = totalFinanciado - valorFinanciamento;
            classe = dadosFinanc.classe;
            parcelamento = dadosFinanc.parcelas.Value;
            intervalo = dadosFinanc.intervalo.Value;
            aceiraDesconto = dadosFinanc.aceitadescontos == "S" ? true : false;
            codigoTabelaJuros = dadosFinanc.Codigo;

            try
            {
                tipoPagamento = dadosFinanc.tipopagamento;
            }
            catch (Exception erro)
            {
                tipoPagamento = "00";
            }


            if (encargos < 0)
            {
                desconto = Math.Round(Math.Abs(encargos),2);
                encargos = 0;           
            }
           
            lblDesconto.Text = string.Format("{0:N2}",desconto);
            lblValorFinanciamento.Text = string.Format("{0:N2}",valorFinanciamento);
            lblEntradaObrigatoria.Text = string.Format("{0:N2}",valorEntrada == 0 ? entradaObrigatoria : valorEntrada);
            lblEncargos.Text = string.Format("{0:N2}",encargos);
            lblTotalFinanciado.Text = string.Format("{0:N2}",totalFinanciado);

            lblParcela.Text = "Valor Parcela: " + dadosFinanc.parcelas.Value + " X";
            lblValorParcela.Text = string.Format("{0:N2}", valorParcela);
        }

        private void dtgTabela_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CalcularFinanciamento();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            descontoGerencial = chkDescontoGerencial.Checked;
            ConfirmarFinanciamento();
        }

        private void ConfirmarFinanciamento()
        {
            if (dtgTabela.Rows.Count == 0)
            {
                MessageBox.Show("Nenhum valor selecionado");
                return;
            }

            if (valorEntrada < entradaObrigatoria)
            {
                MessageBox.Show("Entrada obrigatório de no mínimo:" + string.Format("{0:N2}", entradaObrigatoria));
                return;
            }
            financiamentoCalculado = true;




            this.Close();
        }

        private void dtgTabela_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ConfirmarFinanciamento();
        }

        private void frmTabelaJuros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void txtEntrada_Leave(object sender, EventArgs e)
        {
            ValidarEntrada();
            dtgTabela.Focus();
        }

        private void txtEntrada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidarEntrada();
                dtgTabela.Focus();
            }
        }
    }
}
