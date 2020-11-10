namespace SICEpdv
{
    partial class FrmConsultarSangria
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
            this.dtgSangria = new System.Windows.Forms.DataGridView();
            this.idincDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricaocontaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subcontaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricaosubcontaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historicoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.horaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.movdespesasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btSair = new System.Windows.Forms.Button();
            this.btEstornar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSangria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.movdespesasBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgSangria
            // 
            this.dtgSangria.AutoGenerateColumns = false;
            this.dtgSangria.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idincDataGridViewTextBoxColumn,
            this.contaDataGridViewTextBoxColumn,
            this.descricaocontaDataGridViewTextBoxColumn,
            this.subcontaDataGridViewTextBoxColumn,
            this.descricaosubcontaDataGridViewTextBoxColumn,
            this.historicoDataGridViewTextBoxColumn,
            this.valorDataGridViewTextBoxColumn,
            this.horaDataGridViewTextBoxColumn});
            this.dtgSangria.DataSource = this.movdespesasBindingSource;
            this.dtgSangria.Location = new System.Drawing.Point(-10, 3);
            this.dtgSangria.Name = "dtgSangria";
            this.dtgSangria.RowTemplate.Height = 35;
            this.dtgSangria.Size = new System.Drawing.Size(795, 513);
            this.dtgSangria.TabIndex = 0;
            // 
            // idincDataGridViewTextBoxColumn
            // 
            this.idincDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.idincDataGridViewTextBoxColumn.DataPropertyName = "id_inc";
            this.idincDataGridViewTextBoxColumn.HeaderText = "ID";
            this.idincDataGridViewTextBoxColumn.Name = "idincDataGridViewTextBoxColumn";
            this.idincDataGridViewTextBoxColumn.Width = 30;
            // 
            // contaDataGridViewTextBoxColumn
            // 
            this.contaDataGridViewTextBoxColumn.DataPropertyName = "conta";
            this.contaDataGridViewTextBoxColumn.HeaderText = "Conta";
            this.contaDataGridViewTextBoxColumn.Name = "contaDataGridViewTextBoxColumn";
            this.contaDataGridViewTextBoxColumn.Width = 50;
            // 
            // descricaocontaDataGridViewTextBoxColumn
            // 
            this.descricaocontaDataGridViewTextBoxColumn.DataPropertyName = "descricaoconta";
            this.descricaocontaDataGridViewTextBoxColumn.HeaderText = "Descrição";
            this.descricaocontaDataGridViewTextBoxColumn.Name = "descricaocontaDataGridViewTextBoxColumn";
            this.descricaocontaDataGridViewTextBoxColumn.Width = 140;
            // 
            // subcontaDataGridViewTextBoxColumn
            // 
            this.subcontaDataGridViewTextBoxColumn.DataPropertyName = "subconta";
            this.subcontaDataGridViewTextBoxColumn.HeaderText = "Sub-Conta";
            this.subcontaDataGridViewTextBoxColumn.Name = "subcontaDataGridViewTextBoxColumn";
            this.subcontaDataGridViewTextBoxColumn.Width = 50;
            // 
            // descricaosubcontaDataGridViewTextBoxColumn
            // 
            this.descricaosubcontaDataGridViewTextBoxColumn.DataPropertyName = "descricaosubconta";
            this.descricaosubcontaDataGridViewTextBoxColumn.HeaderText = "Descrição";
            this.descricaosubcontaDataGridViewTextBoxColumn.Name = "descricaosubcontaDataGridViewTextBoxColumn";
            this.descricaosubcontaDataGridViewTextBoxColumn.Width = 140;
            // 
            // historicoDataGridViewTextBoxColumn
            // 
            this.historicoDataGridViewTextBoxColumn.DataPropertyName = "historico";
            this.historicoDataGridViewTextBoxColumn.HeaderText = "Histórico";
            this.historicoDataGridViewTextBoxColumn.Name = "historicoDataGridViewTextBoxColumn";
            this.historicoDataGridViewTextBoxColumn.Width = 200;
            // 
            // valorDataGridViewTextBoxColumn
            // 
            this.valorDataGridViewTextBoxColumn.DataPropertyName = "valor";
            this.valorDataGridViewTextBoxColumn.HeaderText = "Valor";
            this.valorDataGridViewTextBoxColumn.Name = "valorDataGridViewTextBoxColumn";
            this.valorDataGridViewTextBoxColumn.Width = 50;
            // 
            // horaDataGridViewTextBoxColumn
            // 
            this.horaDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.horaDataGridViewTextBoxColumn.DataPropertyName = "hora";
            this.horaDataGridViewTextBoxColumn.HeaderText = "Hora";
            this.horaDataGridViewTextBoxColumn.Name = "horaDataGridViewTextBoxColumn";
            // 
            // movdespesasBindingSource
            // 
            this.movdespesasBindingSource.DataSource = typeof(SICEpdv.movdespesas);
            // 
            // btSair
            // 
            this.btSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btSair.Location = new System.Drawing.Point(690, 522);
            this.btSair.Name = "btSair";
            this.btSair.Size = new System.Drawing.Size(95, 40);
            this.btSair.TabIndex = 1;
            this.btSair.Text = "&Sair";
            this.btSair.UseVisualStyleBackColor = false;
            this.btSair.Click += new System.EventHandler(this.Sair);
            // 
            // btEstornar
            // 
            this.btEstornar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btEstornar.Location = new System.Drawing.Point(538, 522);
            this.btEstornar.Name = "btEstornar";
            this.btEstornar.Size = new System.Drawing.Size(95, 40);
            this.btEstornar.TabIndex = 2;
            this.btEstornar.Text = "Estornar";
            this.btEstornar.UseVisualStyleBackColor = false;
            this.btEstornar.Click += new System.EventHandler(this.btEstornar_Click);
            // 
            // FrmConsultarSangria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this.btEstornar);
            this.Controls.Add(this.btSair);
            this.Controls.Add(this.dtgSangria);
            this.Name = "FrmConsultarSangria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Consulta de Sangria";
            this.Load += new System.EventHandler(this.FrmConsultarSangria_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSangria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.movdespesasBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgSangria;
        private System.Windows.Forms.BindingSource movdespesasBindingSource;
        private System.Windows.Forms.Button btSair;
        private System.Windows.Forms.DataGridViewTextBoxColumn idincDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricaocontaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subcontaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricaosubcontaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn historicoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn horaDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btEstornar;
    }
}