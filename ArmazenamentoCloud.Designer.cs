namespace SICEpdv
{
    partial class ArmazenamentoCloud
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCliente = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtgArquivos = new System.Windows.Forms.DataGridView();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baixar = new System.Windows.Forms.DataGridViewImageColumn();
            this.data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apagar = new System.Windows.Forms.DataGridViewImageColumn();
            this.arquivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlEnviar = new System.Windows.Forms.Panel();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEscolher = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picEscolher = new System.Windows.Forms.PictureBox();
            this.pnlContratar = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnNao = new System.Windows.Forms.Button();
            this.btnContratar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtPesquisarDocumento = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.imgPesquisa = new System.Windows.Forms.PictureBox();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArquivos)).BeginInit();
            this.pnlEnviar.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEscolher)).BeginInit();
            this.pnlContratar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPesquisa)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblCliente);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(605, 100);
            this.panel1.TabIndex = 0;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.ForeColor = System.Drawing.Color.White;
            this.lblCliente.Location = new System.Drawing.Point(110, 31);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(0, 24);
            this.lblCliente.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.cloud_icon_white;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(101, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "UPLOAD";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dtgArquivos
            // 
            this.dtgArquivos.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dtgArquivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgArquivos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descricao,
            this.baixar,
            this.data,
            this.apagar,
            this.arquivo});
            this.dtgArquivos.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dtgArquivos.Location = new System.Drawing.Point(11, 166);
            this.dtgArquivos.Name = "dtgArquivos";
            this.dtgArquivos.RowHeadersVisible = false;
            this.dtgArquivos.RowTemplate.Height = 40;
            this.dtgArquivos.Size = new System.Drawing.Size(596, 408);
            this.dtgArquivos.TabIndex = 3;
            this.dtgArquivos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgArquivos_CellContentClick);
            // 
            // descricao
            // 
            this.descricao.DataPropertyName = "descricao";
            this.descricao.HeaderText = "Descrição";
            this.descricao.Name = "descricao";
            this.descricao.Width = 250;
            // 
            // baixar
            // 
            this.baixar.DataPropertyName = "baixar";
            this.baixar.HeaderText = "";
            this.baixar.Image = global::SICEpdv.Properties.Resources.abaixo;
            this.baixar.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.baixar.Name = "baixar";
            this.baixar.Width = 40;
            // 
            // data
            // 
            this.data.DataPropertyName = "data";
            this.data.HeaderText = "data";
            this.data.Name = "data";
            // 
            // apagar
            // 
            this.apagar.DataPropertyName = "apagar";
            this.apagar.HeaderText = "";
            this.apagar.Image = global::SICEpdv.Properties.Resources.delete;
            this.apagar.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.apagar.Name = "apagar";
            this.apagar.Width = 35;
            // 
            // arquivo
            // 
            this.arquivo.DataPropertyName = "arquivo";
            this.arquivo.HeaderText = "";
            this.arquivo.Name = "arquivo";
            this.arquivo.Visible = false;
            // 
            // pnlEnviar
            // 
            this.pnlEnviar.Controls.Add(this.btnEnviar);
            this.pnlEnviar.Controls.Add(this.btnFechar);
            this.pnlEnviar.Controls.Add(this.lblInfo);
            this.pnlEnviar.Controls.Add(this.txtDescricao);
            this.pnlEnviar.Controls.Add(this.label1);
            this.pnlEnviar.Controls.Add(this.btnEscolher);
            this.pnlEnviar.Location = new System.Drawing.Point(103, 201);
            this.pnlEnviar.Name = "pnlEnviar";
            this.pnlEnviar.Size = new System.Drawing.Size(437, 171);
            this.pnlEnviar.TabIndex = 4;
            // 
            // btnEnviar
            // 
            this.btnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnEnviar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEnviar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnEnviar.ForeColor = System.Drawing.Color.White;
            this.btnEnviar.Image = global::SICEpdv.Properties.Resources.cloud_upload;
            this.btnEnviar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnviar.Location = new System.Drawing.Point(177, 110);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(122, 44);
            this.btnEnviar.TabIndex = 5;
            this.btnEnviar.Text = " ENVIAR";
            this.btnEnviar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnviar.UseVisualStyleBackColor = false;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click_1);
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatAppearance.BorderSize = 0;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechar.ForeColor = System.Drawing.Color.White;
            this.btnFechar.Location = new System.Drawing.Point(325, 110);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(99, 44);
            this.btnFechar.TabIndex = 4;
            this.btnFechar.Text = "CANCELAR";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblInfo.Location = new System.Drawing.Point(26, 242);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 13);
            this.lblInfo.TabIndex = 3;
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(14, 41);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(410, 54);
            this.txtDescricao.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Descrição da imagem  documento / arquivo:";
            // 
            // btnEscolher
            // 
            this.btnEscolher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnEscolher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEscolher.FlatAppearance.BorderSize = 0;
            this.btnEscolher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEscolher.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEscolher.ForeColor = System.Drawing.Color.White;
            this.btnEscolher.Image = global::SICEpdv.Properties.Resources.cloud_file;
            this.btnEscolher.Location = new System.Drawing.Point(14, 109);
            this.btnEscolher.Name = "btnEscolher";
            this.btnEscolher.Size = new System.Drawing.Size(132, 45);
            this.btnEscolher.TabIndex = 0;
            this.btnEscolher.Text = "ESCOLHER ARQUIVO";
            this.btnEscolher.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEscolher.UseVisualStyleBackColor = false;
            this.btnEscolher.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.picEscolher);
            this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(479, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(128, 44);
            this.panel2.TabIndex = 5;
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // picEscolher
            // 
            this.picEscolher.Image = global::SICEpdv.Properties.Resources.cloud_upload;
            this.picEscolher.Location = new System.Drawing.Point(17, 8);
            this.picEscolher.Name = "picEscolher";
            this.picEscolher.Size = new System.Drawing.Size(33, 28);
            this.picEscolher.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEscolher.TabIndex = 1;
            this.picEscolher.TabStop = false;
            this.picEscolher.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pnlContratar
            // 
            this.pnlContratar.Controls.Add(this.label4);
            this.pnlContratar.Controls.Add(this.btnNao);
            this.pnlContratar.Controls.Add(this.btnContratar);
            this.pnlContratar.Controls.Add(this.label5);
            this.pnlContratar.Controls.Add(this.label3);
            this.pnlContratar.Controls.Add(this.pictureBox2);
            this.pnlContratar.Location = new System.Drawing.Point(579, 590);
            this.pnlContratar.Name = "pnlContratar";
            this.pnlContratar.Size = new System.Drawing.Size(605, 581);
            this.pnlContratar.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(158, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(303, 56);
            this.label4.TabIndex = 6;
            this.label4.Text = " IQS CLOUD";
            // 
            // btnNao
            // 
            this.btnNao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnNao.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNao.FlatAppearance.BorderSize = 0;
            this.btnNao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNao.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnNao.ForeColor = System.Drawing.Color.White;
            this.btnNao.Location = new System.Drawing.Point(385, 446);
            this.btnNao.Name = "btnNao";
            this.btnNao.Size = new System.Drawing.Size(127, 48);
            this.btnNao.TabIndex = 5;
            this.btnNao.Text = "CANCELAR";
            this.btnNao.UseVisualStyleBackColor = false;
            this.btnNao.Click += new System.EventHandler(this.btnNao_Click);
            // 
            // btnContratar
            // 
            this.btnContratar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnContratar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContratar.FlatAppearance.BorderSize = 0;
            this.btnContratar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContratar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContratar.ForeColor = System.Drawing.Color.White;
            this.btnContratar.Location = new System.Drawing.Point(94, 446);
            this.btnContratar.Name = "btnContratar";
            this.btnContratar.Size = new System.Drawing.Size(275, 48);
            this.btnContratar.TabIndex = 4;
            this.btnContratar.Text = "CONTRATAR E SABER MAIS";
            this.btnContratar.UseVisualStyleBackColor = false;
            this.btnContratar.Click += new System.EventHandler(this.btnContratar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(57, 303);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(499, 96);
            this.label5.TabIndex = 3;
            this.label5.Text = "Armazene os arquivos de seus clientes de forma digital.\r\nDigitalize contratos, do" +
    "cumentos, fotos e qualquer\r\noutro arquivo. Armazene-os em seu drive em nuvem, co" +
    "m\r\ncapacidade de 5GB por cliente.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(131, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(386, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "ARMAZENAMENTO EM NUVENS";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SICEpdv.Properties.Resources.cloud_icon_white;
            this.pictureBox2.Location = new System.Drawing.Point(223, 35);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(183, 130);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // txtPesquisarDocumento
            // 
            this.txtPesquisarDocumento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPesquisarDocumento.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPesquisarDocumento.Location = new System.Drawing.Point(82, 126);
            this.txtPesquisarDocumento.Name = "txtPesquisarDocumento";
            this.txtPesquisarDocumento.Size = new System.Drawing.Size(328, 15);
            this.txtPesquisarDocumento.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(280, 112);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(73, 42);
            this.panel3.TabIndex = 10;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "baixar";
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::SICEpdv.Properties.Resources.abaixo;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.DataPropertyName = "apagar";
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::SICEpdv.Properties.Resources.delete;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Width = 40;
            // 
            // imgPesquisa
            // 
            this.imgPesquisa.BackgroundImage = global::SICEpdv.Properties.Resources.input_search_products;
            this.imgPesquisa.Location = new System.Drawing.Point(12, 112);
            this.imgPesquisa.Name = "imgPesquisa";
            this.imgPesquisa.Size = new System.Drawing.Size(409, 42);
            this.imgPesquisa.TabIndex = 8;
            this.imgPesquisa.TabStop = false;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.FlatAppearance.BorderSize = 0;
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Image = global::SICEpdv.Properties.Resources.btn_search;
            this.btnPesquisar.Location = new System.Drawing.Point(428, 111);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(45, 43);
            this.btnPesquisar.TabIndex = 7;
            this.btnPesquisar.UseVisualStyleBackColor = true;
            // 
            // ArmazenamentoCloud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(621, 602);
            this.Controls.Add(this.pnlContratar);
            this.Controls.Add(this.txtPesquisarDocumento);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.imgPesquisa);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlEnviar);
            this.Controls.Add(this.dtgArquivos);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ArmazenamentoCloud";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IQS CLOUD - Armazenamento em nuvem";
            this.Load += new System.EventHandler(this.ArmazenamentoCloud_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArquivos)).EndInit();
            this.pnlEnviar.ResumeLayout(false);
            this.pnlEnviar.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEscolher)).EndInit();
            this.pnlContratar.ResumeLayout(false);
            this.pnlContratar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPesquisa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.PictureBox picEscolher;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dtgArquivos;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Panel pnlEnviar;
        private System.Windows.Forms.Button btnEscolher;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewImageColumn baixar;
        private System.Windows.Forms.DataGridViewTextBoxColumn data;
        private System.Windows.Forms.DataGridViewImageColumn apagar;
        private System.Windows.Forms.DataGridViewTextBoxColumn arquivo;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlContratar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnContratar;
        private System.Windows.Forms.Button btnNao;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.PictureBox imgPesquisa;
        private System.Windows.Forms.TextBox txtPesquisarDocumento;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
    }
}