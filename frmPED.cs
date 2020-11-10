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
    public partial class frmPED : Form
    {
        UcAdicionarItens itens = new UcAdicionarItens();
        
        public frmPED()
        {
            InitializeComponent();
            pnlItens.Controls.Add(itens);
            this.itens.clickBotao += new UcAdicionarItens.ClicarNoBotao(DelegateItens);
            txtNF.KeyPress += (objeto, evento) =>
                Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtNF.Enter += (objento, evento) =>
                {
                    ObeterNumeroDocumento();
                };
        }

        private void ObeterNumeroDocumento()
        {
            int? nrDoc = ((from n in Conexao.CriarEntidade().contdocs
                           where n.modeloDOCFiscal == "02"
                           && n.serienf==txtSerie.Text
                           select (int?)n.nrnotafiscal).Max() + 1);
            txtNF.Text = nrDoc.GetValueOrDefault().ToString();
        }

        void DelegateItens(object sender, string acao)
        {
            switch (acao)
            {
                case "incluir":
                    SelecionarItens();
                    break;
            }
        }

        private void SelecionarItens()
        {
            Venda itens = new Venda();
            dtgItens.DataSource = itens.SelectionaItensVenda();
            if (dtgItens.Rows.Count > 0)
            dtgItens.CurrentCell = dtgItens.Rows[dtgItens.RowCount - 1].Cells[0];
            lblTotal.Text = String.Format("{0:n2}",itens.SomaItens());
        }

        private void frmPED_Load(object sender, EventArgs e)
        {
            SelecionarItens();
        }

        private void frmPED_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dtgItens.Rows.Count == 0)
                return;

            try
            {
                Venda venda = new Venda();
                venda.ExcluirItem(Convert.ToInt32(dtgItens.CurrentRow.Cells["nrcontrole"].Value), Convert.ToInt32(dtgItens.CurrentRow.Cells["id"].Value));
                SelecionarItens();
                // dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível excluir o item :" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }      
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btEncerrar_Click(object sender, EventArgs e)
        {
           
            if (txtNF.Text == "" || txtSerie.Text == "" || txtSubSerie.Text == "")
            {
                MessageBox.Show("Preencha o campo Doc. Fiscal série e sub-série");
                txtNF.Focus();
                return;
            }

            FrmMsgOperador msg = new FrmMsgOperador("", "Encerrando lançamento");
            msg.Show();
            Application.DoEvents();
            try
            {
                ConfiguracoesECF.pdv = true;
                ConfiguracoesECF.davporImpressoraNaoFiscal = false;
                ConfiguracoesECF.prevenda = false;
                siceEntities entidade = Conexao.CriarEntidade();
                caixas cx = new caixas();
                cx.tipopagamento = "DH";
                cx.valor = Convert.ToDecimal(lblTotal.Text);
                cx.dpfinanceiro = "Venda";
                cx.vendedor = "000";
                cx.historico = "*";
                cx.data = GlbVariaveis.Sys_Data;
                cx.EnderecoIP = GlbVariaveis.glb_IP;
                cx.caixa = 0;
                cx.operador = GlbVariaveis.glb_Usuario;
                cx.filialorigem = GlbVariaveis.glb_filial;
                cx.CodigoFilial = GlbVariaveis.glb_filial;
                entidade.AddTocaixas(cx);
                entidade.SaveChanges();
            }
            catch (Exception erro)
            {
                msg.Dispose();
                MessageBox.Show(erro.Message);
            }

            try
            {
                Venda venda = new Venda();
                venda.vendaFinalizada = false;
                venda.dpFinanceiro = "Venda";
                venda.valorBruto = Convert.ToDecimal(lblTotal.Text);
                venda.desconto = 0;
                venda.idCliente = 0; // clientePDV.idCliente;

                Venda.dadosConsumidor =
                new DadosConsumidorCupom
                {
                    cpfCnpjConsumidor = "",
                    nomeConsumidor = "",
                    endConsumidor = "",
                    endNumero = "",
                    endBairro = "",
                    endCEP = "",
                    endCidade = "",
                    endEstado = "",
                    idConsumidor = ""
                };
                

                venda.valorLiquido = Convert.ToDecimal(lblTotal.Text);
                venda.Finalizar(false, false, true);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            finally
            {
                msg.Dispose();
                SelecionarItens();
            }

            siceEntities container = Conexao.CriarEntidade();

            var ultDoc = (from n in container.contdocs
                          where n.ip == GlbVariaveis.glb_IP
                          select n.documento).Max();

            contdocs dados = (from n in container.contdocs
                             where n.documento == ultDoc
                             select n).First();
            dados.nrnotafiscal = Convert.ToInt64(txtNF.Text);
            dados.serienf = txtSerie.Text;
            dados.subserienf = txtSubSerie.Text;
            dados.modeloDOCFiscal = "02";

            //var dadosVenda = from n in container.venda
            //                   where n.documento == ultDoc
            //                   select n;

            //foreach (var item in dadosVenda)
            //{
            //    venda upVenda = (from n in container.venda
            //                     where n.inc == item.inc
            //                     select n).First();

            //    upVenda.serieNF = txtSerie.Text;
            //    upVenda.subserienf = txtSubSerie.Text;
            //    upVenda.modelodocfiscal = "02";
            //}
            msg.Dispose();
            container.SaveChanges();
            txtNF.Focus();

        }

        private void frmPED_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }
    }
}
