using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class UCCartoes : UserControl
    {
        public int idCartao = 0;
        public delegate void Controle(string campo);
        public event Controle EntraControle;
        private string pagamento;

        public UCCartoes()
        {
           InitializeComponent();
            //this.ScriptManager.SetFocus(txtCodigoCartao);
            ActiveControl = txtCodigoCartao;

            txtParcelamentoCA.Enter += (objeto, evento) => EntraControle("txtParcelamentoCA");
           txtValorIndCA.Enter += (objeto, evento) => EntraControle("txtValorIndCA");
           btSairCA.Click += (objeto, evento) =>
               {
                   EntraControle("btSairCA");
                   pnlValorCA.Enabled = false;
               };
            txtParcelamentoCA.KeyPress += (objeto, evento) =>
               {
                   Funcoes.DigitarNumerosPositivos(objeto, evento);         
               };
           txtValorIndCA.KeyPress += (objeto, evento) =>
               {
                   Funcoes.DigitarNumerosPositivos(objeto, evento);        
               };
           btConfirmaCA.Click += (objeto, evento) =>
               {
                   verificaIQCard();
                   EntraControle("btConfirmaCA");                   
               };
           txtNrCartao.KeyPress += (objeto, evento) =>
               {
                   Funcoes.DigitarNumerosPositivos(objeto, evento);
               };

        }

        private void UCCartoes_Load(object sender, EventArgs e)
        {
            

            alterarSequencia();

            int x = 0;                        
            int yaltura = 0;
            

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var cartoes = from c in entidade.cartoes                         
                          orderby c.descricao
                          select c;
            
            foreach (var item in cartoes)
            {

                string visao = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT visao FROM cartoes where id = '"+item.id+"'").FirstOrDefault();

                if (visao != "1")
                {
                    if (x > 2)
                    {
                        x = 0;
                        yaltura = yaltura + 50; ;
                    }
                    Button btCartao = new Button();
                    btCartao.Location = new System.Drawing.Point(x * 120, yaltura);
                    btCartao.Name = item.id.ToString();
                    btCartao.Size = new System.Drawing.Size(118, 42);
                    btCartao.TabIndex = 0;
                    btCartao.FlatStyle = FlatStyle.Flat;
                    btCartao.BackColor = Color.FromArgb(91, 191, 223);
                    btCartao.ForeColor = System.Drawing.Color.White;
                    btCartao.Text = item.id.ToString() + " - " + item.descricao;
                    btCartao.FlatAppearance.BorderColor = Color.FromArgb(0, 65, 119);
                    btCartao.FlatAppearance.BorderSize = 3;


                    btCartao.Click += (objeto, evento) =>
                        {
                        /*
                        if (!string.IsNullOrEmpty(Venda.IQCard) && Venda.IQCard.Length == 16)
                        {
                            txtNrCartao.Text = Venda.IQCard;
                        }


                        idCartao = Convert.ToInt16(btCartao.Name);
                        if (!ConfiguracoesECF.tefDedicado)
                        {
                            var req = (from n in entidade.cartoes
                                         where n.id == idCartao
                                         select n.pathreq).FirstOrDefault();

                            var resp = (from n in entidade.cartoes
                                       where n.id == idCartao
                                       select n.pathresp).FirstOrDefault();
                            
                            TEF.PathReq = @req;
                            TEF.PathResp = @resp;
                            btConfirmaCA.Enabled = true;
                            if (!Directory.Exists(req) || !Directory.Exists(resp))
                            {
                                if (ConfiguracoesECF.tefDiscado)
                                {
                                    TEF.PathReq = @"C:\tef_dial\REQ\";
                                    TEF.PathResp = @"C:\tef_dial\RESP\";
                                    idCartao = 0;
                                }
                                MessageBox.Show(@"Diretório de resposta ou de requisição não encontrados. Por favor ir em configurações dos cartões e coloque os diretórios corretamente. Será usado os diretórios padrões. C:\tef_dial\REQ\ e C:\tef_dial\\RESP\", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                TEF.PathReq = @"C:\tef_dial\REQ\";
                                TEF.PathResp = @"C:\tef_dial\RESP\";

                                //btConfirmaCA.Enabled=false;
                            }
                        };
                        
                      
                        pnlValorCA.Enabled = true;
                        txtValorIndCA.Focus();
                        if (txtValorIndCA.Enabled == false)
                            txtParcelamentoCA.Focus();
                        btCartao.ForeColor = Color.Green;
                        TravaCartoes();   
                        */

                            clickCartao(btCartao.Name);
                        };

                    pnlCartoes.Controls.Add(btCartao);
                    //btCartao.ForeColor = Color.Green;

                    x++;
                }
            }

            txtCodigoCartao.Focus();
        }

        private void clickCartao(string idCartaoName)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            if (!string.IsNullOrEmpty(Venda.IQCard) && Venda.IQCard.Length == 16)
            {
                txtNrCartao.Text = Venda.IQCard;
            }


            idCartao = Convert.ToInt16(idCartaoName);
            if (!ConfiguracoesECF.tefDedicado)
            {
                var req = (from n in entidade.cartoes
                           where n.id == idCartao
                           select n.pathreq).FirstOrDefault();

                var resp = (from n in entidade.cartoes
                            where n.id == idCartao
                            select n.pathresp).FirstOrDefault();

                pagamento = (from n in entidade.cartoes
                                where n.id == idCartao
                                select n.tipo).FirstOrDefault();


                TEF.PathReq = @req;
                TEF.PathResp = @resp;
                btConfirmaCA.Enabled = true;
                if (!Directory.Exists(req) || !Directory.Exists(resp))
                {
                    if (ConfiguracoesECF.tefDiscado)
                    {
                        TEF.PathReq = @"C:\tef_dial\REQ\";
                        TEF.PathResp = @"C:\tef_dial\RESP\";
                        idCartao = 0;
                    }
                    MessageBox.Show(@"Diretório de resposta ou de requisição não encontrados. Por favor ir em configurações dos cartões e coloque os diretórios corretamente. Será usado os diretórios padrões. C:\tef_dial\REQ\ e C:\tef_dial\\RESP\", "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TEF.PathReq = @"C:\tef_dial\REQ\";
                    TEF.PathResp = @"C:\tef_dial\RESP\";

                    //btConfirmaCA.Enabled=false;
                }
            };


            pnlValorCA.Enabled = true;
            //txtValorIndCA.Focus();
            if (txtValorIndCA.Enabled == false)
                txtParcelamentoCA.Focus();
            CorCartoes();
            //btCartao.ForeColor = Color.Green;
            TravaCartoes();
        }

        private void TravaCartoes()
        {
            foreach (Control botao in Controls)
            {
                if (botao is Button && botao.Name != idCartao.ToString() )
                {
                    botao.Enabled = false;
                }
            }
            btSairCA.Enabled = true;

        }

        private void CorCartoes()
        {
            foreach (Control botao in pnlCartoes.Controls)
            {
                //MessageBox.Show(botao.Text + "-" + botao.Name);

                if (botao is Button && botao.Name == idCartao.ToString())
                {
                    botao.ForeColor = Color.Green;
                }
            }
            btSairCA.Enabled = true;

        }

        public void DesTravaCartoes()
        {
            foreach (Control botao in Controls)
            {     
                     if (botao is Button)
                        botao.Enabled = true;
                    if (idCartao==0)
                        botao.ForeColor = Color.Black;
            }
            btSairCA.Enabled = true;

        }

        private void txtValorIndCA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }

            sairCartoes(e);

        }

        private void txtParcelamentoCA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
            sairCartoes(e);
        }

        private void UCCartoes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();

            sairCartoes(e);   
        }

        private void txtCodigoCartao_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    idCartao = int.Parse(txtCodigoCartao.Text == "" ? "0" : txtCodigoCartao.Text);
                    clickCartao(txtCodigoCartao.Text);

                    alterarSequencia();

                    if (pagamento == "PF")
                    {
                        txtNrCartao.Focus();
                    }
                    else
                        txtValorIndCA.Focus();
                }
                catch (Exception)
                {
                    MessageBox.Show("Cartão digitado invalido", "anteção",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCodigoCartao.Focus();
                }
            }

            sairCartoes(e);
        }

        private void sairCartoes(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                
                    EntraControle("btSairCA");
                    //pnlValorCA.Enabled = false;
            }
        }

        private void txtNrCartao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }

            sairCartoes(e);
        }

        private void txtNomeCartao_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }
            sairCartoes(e);
        }

        private void btConfirmaCA_KeyDown(object sender, KeyEventArgs e)
        {
            sairCartoes(e);
        }

        private void btSairCA_KeyDown(object sender, KeyEventArgs e)
        {
            sairCartoes(e);
        }

        private void alterarSequencia()
        {
            if (pagamento == "PF")
            {
                txtNrCartao.TabIndex = 0;
                txtNomeCartao.TabIndex = 1;
                txtValorIndCA.TabIndex = 2;
                btConfirmaCA.TabIndex = 3;
            }
            else
            {
                txtNrCartao.TabIndex = 4;
                txtNomeCartao.TabIndex = 5;
                txtValorIndCA.TabIndex = 0;
                btConfirmaCA.TabIndex = 3;
                txtParcelamentoCA.TabIndex = 2;
            }
        }

        private void verificaIQCard()
        {
            if (pagamento == "PF" && !IqCard.VerificarNumeroCartao(txtNrCartao.Text))
            {
                MessageBox.Show("Nr. IQCARD não pode ser verificado");
                btConfirmaCA.Enabled = false;
                txtNrCartao.Focus();
            }
            else
            {
                btConfirmaCA.Enabled = true;
            }
        }

        private void txtNrCartao_Leave(object sender, EventArgs e)
        {
            verificaIQCard();
        }

        private void btConfirmaCA_Click(object sender, EventArgs e)
        {


        }
    }
}
