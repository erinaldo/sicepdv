namespace SICEpdv
{
    partial class frmPreVenda
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
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.btnSair = new System.Windows.Forms.Button();
            this.pnlPreVenda = new System.Windows.Forms.Panel();
            this.btnMesclar = new System.Windows.Forms.Button();
            this.btnFinalizarMescladas = new System.Windows.Forms.Button();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMesclarItem = new System.Windows.Forms.Button();
            this.chkManter = new System.Windows.Forms.CheckBox();
            this.bntEditarDAV = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAppStore = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.Location = new System.Drawing.Point(414, 80);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(238, 225);
            this.pnlTeclado.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(411, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Número:";
            // 
            // txtNumero
            // 
            this.txtNumero.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNumero.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumero.Location = new System.Drawing.Point(9, 10);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(161, 24);
            this.txtNumero.TabIndex = 3;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(601, 331);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(66, 37);
            this.btnSair.TabIndex = 5;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // pnlPreVenda
            // 
            this.pnlPreVenda.AutoScroll = true;
            this.pnlPreVenda.BackColor = System.Drawing.Color.Azure;
            this.pnlPreVenda.Location = new System.Drawing.Point(12, 15);
            this.pnlPreVenda.Name = "pnlPreVenda";
            this.pnlPreVenda.Size = new System.Drawing.Size(393, 287);
            this.pnlPreVenda.TabIndex = 6;
            // 
            // btnMesclar
            // 
            this.btnMesclar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnMesclar.FlatAppearance.BorderSize = 0;
            this.btnMesclar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesclar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesclar.ForeColor = System.Drawing.Color.White;
            this.btnMesclar.Location = new System.Drawing.Point(12, 330);
            this.btnMesclar.Name = "btnMesclar";
            this.btnMesclar.Size = new System.Drawing.Size(78, 37);
            this.btnMesclar.TabIndex = 7;
            this.btnMesclar.Text = "Mesclar";
            this.btnMesclar.UseVisualStyleBackColor = false;
            this.btnMesclar.Click += new System.EventHandler(this.btnMesclar_Click);
            // 
            // btnFinalizarMescladas
            // 
            this.btnFinalizarMescladas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnFinalizarMescladas.FlatAppearance.BorderSize = 0;
            this.btnFinalizarMescladas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizarMescladas.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinalizarMescladas.ForeColor = System.Drawing.Color.White;
            this.btnFinalizarMescladas.Location = new System.Drawing.Point(413, 331);
            this.btnFinalizarMescladas.Name = "btnFinalizarMescladas";
            this.btnFinalizarMescladas.Size = new System.Drawing.Size(84, 37);
            this.btnFinalizarMescladas.TabIndex = 8;
            this.btnFinalizarMescladas.Text = "FINALIZAR";
            this.btnFinalizarMescladas.UseVisualStyleBackColor = false;
            this.btnFinalizarMescladas.Click += new System.EventHandler(this.btnFinalizarMescladas_Click);
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackgroundImage = global::SICEpdv.Properties.Resources.btn_search;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Location = new System.Drawing.Point(615, 34);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(52, 42);
            this.btnProcurar.TabIndex = 10;
            this.btnProcurar.UseVisualStyleBackColor = true;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(580, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "F7 Menu Fiscal";
            this.label3.Visible = false;
            // 
            // btnMesclarItem
            // 
            this.btnMesclarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnMesclarItem.FlatAppearance.BorderSize = 0;
            this.btnMesclarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesclarItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesclarItem.ForeColor = System.Drawing.Color.White;
            this.btnMesclarItem.Location = new System.Drawing.Point(96, 330);
            this.btnMesclarItem.Name = "btnMesclarItem";
            this.btnMesclarItem.Size = new System.Drawing.Size(78, 37);
            this.btnMesclarItem.TabIndex = 12;
            this.btnMesclarItem.Text = "Mesclar Itens";
            this.btnMesclarItem.UseVisualStyleBackColor = false;
            this.btnMesclarItem.Click += new System.EventHandler(this.btnMesclarItem_Click);
            // 
            // chkManter
            // 
            this.chkManter.AutoSize = true;
            this.chkManter.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkManter.ForeColor = System.Drawing.Color.White;
            this.chkManter.Location = new System.Drawing.Point(20, 306);
            this.chkManter.Name = "chkManter";
            this.chkManter.Size = new System.Drawing.Size(242, 18);
            this.chkManter.TabIndex = 13;
            this.chkManter.Text = "Manter vendedor original do DAV ou PV";
            this.chkManter.UseVisualStyleBackColor = true;
            this.chkManter.Visible = false;
            // 
            // bntEditarDAV
            // 
            this.bntEditarDAV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.bntEditarDAV.FlatAppearance.BorderSize = 0;
            this.bntEditarDAV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntEditarDAV.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntEditarDAV.ForeColor = System.Drawing.Color.White;
            this.bntEditarDAV.Location = new System.Drawing.Point(503, 331);
            this.bntEditarDAV.Name = "bntEditarDAV";
            this.bntEditarDAV.Size = new System.Drawing.Size(92, 37);
            this.bntEditarDAV.TabIndex = 14;
            this.bntEditarDAV.Text = "EDITAR DAV";
            this.bntEditarDAV.UseVisualStyleBackColor = false;
            this.bntEditarDAV.Click += new System.EventHandler(this.bntEditarDAV_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtNumero);
            this.panel1.Location = new System.Drawing.Point(414, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 45);
            this.panel1.TabIndex = 15;
            // 
            // btnAppStore
            // 
            this.btnAppStore.BackColor = System.Drawing.Color.Cyan;
            this.btnAppStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppStore.Location = new System.Drawing.Point(299, 330);
            this.btnAppStore.Name = "btnAppStore";
            this.btnAppStore.Size = new System.Drawing.Size(106, 38);
            this.btnAppStore.TabIndex = 16;
            this.btnAppStore.Text = "PEDIDO APP STORE";
            this.btnAppStore.UseVisualStyleBackColor = false;
            this.btnAppStore.Click += new System.EventHandler(this.btnAppStore_Click);
            // 
            // frmPreVenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(692, 379);
            this.Controls.Add(this.btnAppStore);
            this.Controls.Add(this.btnProcurar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bntEditarDAV);
            this.Controls.Add(this.chkManter);
            this.Controls.Add(this.btnMesclarItem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFinalizarMescladas);
            this.Controls.Add(this.btnMesclar);
            this.Controls.Add(this.pnlPreVenda);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlTeclado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frmPreVenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Escolha o número";
            this.Load += new System.EventHandler(this.PreVenda_Load);
            this.Shown += new System.EventHandler(this.frmPreVenda_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPreVenda_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmPreVenda_KeyPress);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Panel pnlPreVenda;
        private System.Windows.Forms.Button btnMesclar;
        private System.Windows.Forms.Button btnFinalizarMescladas;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMesclarItem;
        private System.Windows.Forms.CheckBox chkManter;
        private System.Windows.Forms.Button bntEditarDAV;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAppStore;
    }
}