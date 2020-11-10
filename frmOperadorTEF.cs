using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SICEpdv
{
    /// <summary>
    /// Summary description for frmMsgTEF.
    /// </summary>
    public class frmOperadorTEF : System.Windows.Forms.Form
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        private System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Label lblMensagem;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private string mensagem = "";
        private Panel pnlIQCARD;
        private Button btnEnviar;
        private TextBox txtIQCard;
        private Label label2;
        private bool adm = false;
        public static bool EnviarIQCARD = false;
        public bool mostarIQCARD = false;
        private PictureBox pictureBox1;
        private Label label1;
        public static long davNumero = 0;


        public frmOperadorTEF(string mensagem, bool tipo)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.mensagem = mensagem;
            this.adm = tipo;
            btnOk.Focus();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.pnlIQCARD = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtIQCard = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlIQCARD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnOk.Location = new System.Drawing.Point(246, 144);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(112, 36);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblMensagem
            // 
            this.lblMensagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblMensagem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMensagem.Location = new System.Drawing.Point(12, 24);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(280, 96);
            this.lblMensagem.TabIndex = 2;
            this.lblMensagem.Text = "lblMensagem";
            // 
            // pnlIQCARD
            // 
            this.pnlIQCARD.Controls.Add(this.label1);
            this.pnlIQCARD.Controls.Add(this.pictureBox1);
            this.pnlIQCARD.Controls.Add(this.btnEnviar);
            this.pnlIQCARD.Controls.Add(this.txtIQCard);
            this.pnlIQCARD.Controls.Add(this.label2);
            this.pnlIQCARD.Location = new System.Drawing.Point(15, 45);
            this.pnlIQCARD.Name = "pnlIQCARD";
            this.pnlIQCARD.Size = new System.Drawing.Size(346, 93);
            this.pnlIQCARD.TabIndex = 6;
            this.pnlIQCARD.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.mascote;
            this.pictureBox1.Location = new System.Drawing.Point(15, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(273, 34);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(58, 39);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = false;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtIQCard
            // 
            this.txtIQCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIQCard.ForeColor = System.Drawing.Color.Black;
            this.txtIQCard.Location = new System.Drawing.Point(90, 40);
            this.txtIQCard.Name = "txtIQCard";
            this.txtIQCard.Size = new System.Drawing.Size(169, 26);
            this.txtIQCard.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Enviar para o aplicativo IQCARD:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Orçamento no smartphone do cliente";
            // 
            // frmOperadorTEF
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(370, 186);
            this.ControlBox = false;
            this.Controls.Add(this.pnlIQCARD);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOperadorTEF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmMsgTEF";
            this.Activated += new System.EventHandler(this.frmMsgTEF_Activated);
            this.Load += new System.EventHandler(this.frmMsgTEF_Load);
            this.pnlIQCARD.ResumeLayout(false);
            this.pnlIQCARD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frmMsgTEF_Load(object sender, System.EventArgs e)
        {
            lblMensagem.Text = mensagem;
            this.Focus();
            btnOk.Focus();
            // FindWindow(null, "SICEpdv.exe");
            Application.DoEvents();
            if (adm)
            {                
                SendKeys.Send("{TAB}");
            }
            pnlIQCARD.Visible = false;

            if (EnviarIQCARD && mostarIQCARD)
            {                
                pnlIQCARD.Visible = true;
            }

            if (!string.IsNullOrEmpty(Venda.IQCard) && Venda.IQCard.Length==16)
                txtIQCard.Text = Venda.IQCard;

        }

        private void frmMsgTEF_Activated(object sender, System.EventArgs e)
        {
            this.Focus();
            this.Refresh();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if(!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem conexao com a internet");
                return;
            };

           

            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                ServiceReference2.WSClientesClient suporte = new ServiceReference2.WSClientesClient();
                var dados = suporte.DadosCliente(Convert.ToInt16(GlbVariaveis.idCliente));
                GlbVariaveis.glb_chaveIQCard = dados.iqcard;
                string sql = "UPDATE filiais SET tokeniqcard='" + dados.iqcard + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);               
            }

            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
            {
                MessageBox.Show("É necessário ter uma conta IQCARD. Solicite ao suporte IQ Sistemas");
                return;
            }


            if (!IqCard.VerificarNumeroCartao(txtIQCard.Text) || txtIQCard.Text.Length!=16)
            {
                MessageBox.Show("IQCARD Inválido");
            }

            try
            {
                IqCard iqcard = new IqCard();
                iqcard.EnviarDAVIQCARD(txtIQCard.Text, davNumero);
                MessageBox.Show("Orçamento(DAV) enviado para o aplicativo com sucesso!");
                btnEnviar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
    }
}
