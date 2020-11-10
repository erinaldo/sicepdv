namespace SICEpdv
{
    partial class frmDependente
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgDependentes = new System.Windows.Forms.DataGridView();
            this.nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btSair = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgDependentes)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgDependentes
            // 
            this.dtgDependentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgDependentes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nome,
            this.cpf,
            this.Identidade});
            this.dtgDependentes.Location = new System.Drawing.Point(3, 12);
            this.dtgDependentes.Name = "dtgDependentes";
            this.dtgDependentes.ReadOnly = true;
            this.dtgDependentes.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgDependentes.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgDependentes.RowTemplate.Height = 40;
            this.dtgDependentes.Size = new System.Drawing.Size(508, 217);
            this.dtgDependentes.TabIndex = 0;
            // 
            // nome
            // 
            this.nome.DataPropertyName = "nome";
            this.nome.HeaderText = "Nome";
            this.nome.Name = "nome";
            this.nome.ReadOnly = true;
            this.nome.Width = 300;
            // 
            // cpf
            // 
            this.cpf.DataPropertyName = "cpf";
            this.cpf.HeaderText = "CPF";
            this.cpf.Name = "cpf";
            this.cpf.ReadOnly = true;
            // 
            // Identidade
            // 
            this.Identidade.DataPropertyName = "rg";
            this.Identidade.HeaderText = "RG";
            this.Identidade.Name = "Identidade";
            this.Identidade.ReadOnly = true;
            // 
            // btSair
            // 
            this.btSair.Location = new System.Drawing.Point(431, 235);
            this.btSair.Name = "btSair";
            this.btSair.Size = new System.Drawing.Size(80, 37);
            this.btSair.TabIndex = 2;
            this.btSair.Text = "Sair";
            this.btSair.UseVisualStyleBackColor = true;
            this.btSair.Click += new System.EventHandler(this.btSair_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(3, 235);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(105, 37);
            this.btnConfirmar.TabIndex = 1;
            this.btnConfirmar.Text = "&Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // frmDependente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 275);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.btSair);
            this.Controls.Add(this.dtgDependentes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frmDependente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dependentes do Cliente";
            this.Shown += new System.EventHandler(this.frmDependente_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDependente_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmDependente_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dtgDependentes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgDependentes;
        private System.Windows.Forms.Button btSair;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.DataGridViewTextBoxColumn nome;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Identidade;
    }
}