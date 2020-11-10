namespace SICEpdv
{
    partial class PainelIQCARD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPontos = new System.Windows.Forms.Label();
            this.btnRecarregar = new System.Windows.Forms.Button();
            this.menuComprar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.r100002000PONTOSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.converterMeuCréditoEmPontosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.comprarComBoletoBancárioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlFinanc = new System.Windows.Forms.Panel();
            this.pnlPontosRec = new System.Windows.Forms.Panel();
            this.lblVendasGeradas = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPontosRec = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDepositar = new System.Windows.Forms.Button();
            this.lblCreditos = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuComprar.SuspendLayout();
            this.pnlFinanc.SuspendLayout();
            this.pnlPontosRec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Sitka Small", 13F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(146, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "PONTOS IQCARD\r";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Sitka Small", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(105, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 38);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cada cliente no seu programa\r\nfidelidade é ponto para seu negócio.";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(117, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "RESTA:";
            // 
            // lblPontos
            // 
            this.lblPontos.AutoSize = true;
            this.lblPontos.BackColor = System.Drawing.Color.Transparent;
            this.lblPontos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPontos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPontos.ForeColor = System.Drawing.Color.Red;
            this.lblPontos.Location = new System.Drawing.Point(117, 96);
            this.lblPontos.Name = "lblPontos";
            this.lblPontos.Size = new System.Drawing.Size(17, 18);
            this.lblPontos.TabIndex = 0;
            this.lblPontos.Text = "#";
            this.lblPontos.Click += new System.EventHandler(this.lblPontos_Click);
            // 
            // btnRecarregar
            // 
            this.btnRecarregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRecarregar.ContextMenuStrip = this.menuComprar;
            this.btnRecarregar.ForeColor = System.Drawing.Color.White;
            this.btnRecarregar.Location = new System.Drawing.Point(237, 78);
            this.btnRecarregar.Name = "btnRecarregar";
            this.btnRecarregar.Size = new System.Drawing.Size(92, 47);
            this.btnRecarregar.TabIndex = 5;
            this.btnRecarregar.Text = "COMPRAR PONTOS";
            this.btnRecarregar.UseVisualStyleBackColor = false;
            this.btnRecarregar.Visible = false;
            this.btnRecarregar.ContextMenuStripChanged += new System.EventHandler(this.menuVenda);
            this.btnRecarregar.Click += new System.EventHandler(this.btnRecarregar_Click);
            // 
            // menuComprar
            // 
            this.menuComprar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripMenuItem3,
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem,
            this.toolStripSeparator2,
            this.comprarComBoletoBancárioToolStripMenuItem,
            this.toolStripMenuItem1,
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem,
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem,
            this.toolStripMenuItem2});
            this.menuComprar.Name = "menuComprar";
            this.menuComprar.Size = new System.Drawing.Size(382, 138);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(378, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.r100002000PONTOSToolStripMenuItem,
            this.converterMeuCréditoEmPontosToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(381, 22);
            this.toolStripMenuItem3.Text = "Recarregar com meu crédito";
            // 
            // r100002000PONTOSToolStripMenuItem
            // 
            this.r100002000PONTOSToolStripMenuItem.Name = "r100002000PONTOSToolStripMenuItem";
            this.r100002000PONTOSToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.r100002000PONTOSToolStripMenuItem.Text = "R$ 100,00 -  2000 PONTOS";
            this.r100002000PONTOSToolStripMenuItem.Click += new System.EventHandler(this.r100002000PONTOSToolStripMenuItem_Click);
            // 
            // converterMeuCréditoEmPontosToolStripMenuItem
            // 
            this.converterMeuCréditoEmPontosToolStripMenuItem.Name = "converterMeuCréditoEmPontosToolStripMenuItem";
            this.converterMeuCréditoEmPontosToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.converterMeuCréditoEmPontosToolStripMenuItem.Text = "Converter meu crédito em pontos";
            this.converterMeuCréditoEmPontosToolStripMenuItem.Click += new System.EventHandler(this.converterMeuCréditoEmPontosToolStripMenuItem_Click);
            // 
            // comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem
            // 
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem.Name = "comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem";
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem.Text = "Comprar com cartão de crédito- Liberação imediata";
            this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem.Click += new System.EventHandler(this.comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(378, 6);
            // 
            // comprarComBoletoBancárioToolStripMenuItem
            // 
            this.comprarComBoletoBancárioToolStripMenuItem.Name = "comprarComBoletoBancárioToolStripMenuItem";
            this.comprarComBoletoBancárioToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.comprarComBoletoBancárioToolStripMenuItem.Text = "2.000 PONTOS R$ 100,00 solicitar envio do boleto bancário";
            this.comprarComBoletoBancárioToolStripMenuItem.Visible = false;
            this.comprarComBoletoBancárioToolStripMenuItem.Click += new System.EventHandler(this.comprarComBoletoBancárioToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(378, 6);
            // 
            // pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem
            // 
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Name = "pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem";
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Text = "4.000 PONTOS R$ 200,00 Solicitar envio do boleto bancário";
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Visible = false;
            this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Click += new System.EventHandler(this.pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem_Click);
            // 
            // pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem
            // 
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Name = "pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem";
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Text = "6.000 PONTOS R$ 300,00 Solicitar envio do boleto bancário";
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Visible = false;
            this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem.Click += new System.EventHandler(this.pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(378, 6);
            // 
            // pnlFinanc
            // 
            this.pnlFinanc.BackColor = System.Drawing.Color.Transparent;
            this.pnlFinanc.Controls.Add(this.pnlPontosRec);
            this.pnlFinanc.Controls.Add(this.btnDepositar);
            this.pnlFinanc.Controls.Add(this.lblCreditos);
            this.pnlFinanc.Controls.Add(this.label4);
            this.pnlFinanc.Location = new System.Drawing.Point(4, 148);
            this.pnlFinanc.Name = "pnlFinanc";
            this.pnlFinanc.Size = new System.Drawing.Size(399, 67);
            this.pnlFinanc.TabIndex = 6;
            // 
            // pnlPontosRec
            // 
            this.pnlPontosRec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlPontosRec.Controls.Add(this.lblVendasGeradas);
            this.pnlPontosRec.Controls.Add(this.label7);
            this.pnlPontosRec.Controls.Add(this.lblPontosRec);
            this.pnlPontosRec.Controls.Add(this.label5);
            this.pnlPontosRec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlPontosRec.Location = new System.Drawing.Point(226, 0);
            this.pnlPontosRec.Name = "pnlPontosRec";
            this.pnlPontosRec.Size = new System.Drawing.Size(173, 64);
            this.pnlPontosRec.TabIndex = 6;
            this.pnlPontosRec.Click += new System.EventHandler(this.pnlPontosRec_Click);
            // 
            // lblVendasGeradas
            // 
            this.lblVendasGeradas.AutoSize = true;
            this.lblVendasGeradas.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendasGeradas.Location = new System.Drawing.Point(99, 35);
            this.lblVendasGeradas.Name = "lblVendasGeradas";
            this.lblVendasGeradas.Size = new System.Drawing.Size(54, 25);
            this.lblVendasGeradas.TabIndex = 7;
            this.lblVendasGeradas.Text = "0.00";
            this.lblVendasGeradas.Click += new System.EventHandler(this.lblVendasGeradas_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label7.Location = new System.Drawing.Point(91, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 26);
            this.label7.TabIndex = 6;
            this.label7.Text = "VENDAS\r\nGERADAS R$";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // lblPontosRec
            // 
            this.lblPontosRec.AutoSize = true;
            this.lblPontosRec.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPontosRec.Location = new System.Drawing.Point(23, 34);
            this.lblPontosRec.Name = "lblPontosRec";
            this.lblPontosRec.Size = new System.Drawing.Size(24, 25);
            this.lblPontosRec.TabIndex = 5;
            this.lblPontosRec.Text = "0";
            this.lblPontosRec.Click += new System.EventHandler(this.lblPontosRec_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 26);
            this.label5.TabIndex = 4;
            this.label5.Text = "PONTOS\r\nDISTRIBUÍDOS";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnDepositar
            // 
            this.btnDepositar.BackColor = System.Drawing.Color.Teal;
            this.btnDepositar.ForeColor = System.Drawing.Color.White;
            this.btnDepositar.Location = new System.Drawing.Point(115, 3);
            this.btnDepositar.Name = "btnDepositar";
            this.btnDepositar.Size = new System.Drawing.Size(105, 64);
            this.btnDepositar.TabIndex = 2;
            this.btnDepositar.Text = "SOLICITAR DEPÓSITO DO CRÉDITO";
            this.btnDepositar.UseVisualStyleBackColor = false;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);
            // 
            // lblCreditos
            // 
            this.lblCreditos.AutoSize = true;
            this.lblCreditos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditos.ForeColor = System.Drawing.Color.Blue;
            this.lblCreditos.Location = new System.Drawing.Point(12, 37);
            this.lblCreditos.Name = "lblCreditos";
            this.lblCreditos.Size = new System.Drawing.Size(19, 20);
            this.lblCreditos.TabIndex = 1;
            this.lblCreditos.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "SALDO CRÉDITOS:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.Ebene_1;
            this.pictureBox1.Location = new System.Drawing.Point(-11, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(91, 125);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // PainelIQCARD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SICEpdv.Properties.Resources.backgroundpadrao;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(403, 216);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlFinanc);
            this.Controls.Add(this.btnRecarregar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPontos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "PainelIQCARD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "APP IQCARD";
            this.Load += new System.EventHandler(this.PainelIQCARD_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PainelIQCARD_KeyPress);
            this.menuComprar.ResumeLayout(false);
            this.pnlFinanc.ResumeLayout(false);
            this.pnlFinanc.PerformLayout();
            this.pnlPontosRec.ResumeLayout(false);
            this.pnlPontosRec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPontos;
        private System.Windows.Forms.Button btnRecarregar;
        private System.Windows.Forms.Panel pnlFinanc;
        private System.Windows.Forms.Button btnDepositar;
        private System.Windows.Forms.Label lblCreditos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlPontosRec;
        private System.Windows.Forms.Label lblPontosRec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip menuComprar;
        private System.Windows.Forms.ToolStripMenuItem comprarComCartãoDeCréditoLiberaçãoImediataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comprarComBoletoBancárioToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pONOTSR20000SolicitarEnvioDoBoletoBancárioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pONTOSR30000SolicitarEnvioDoBoletoBancárioToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem r100002000PONTOSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem converterMeuCréditoEmPontosToolStripMenuItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVendasGeradas;
    }
}