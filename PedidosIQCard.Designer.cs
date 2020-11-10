namespace SICEpdv
{
    partial class PedidosIQCard
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIQCard = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.dtPedidos = new System.Windows.Forms.DataGridView();
            this.data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.observacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RowKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66667F));
            this.tableLayoutPanel1.Controls.Add(this.lblIQCard, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNome, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTelefone, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblEndereco, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.74875F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.25125F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 50);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblIQCard
            // 
            this.lblIQCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblIQCard.AutoSize = true;
            this.lblIQCard.Location = new System.Drawing.Point(30, 5);
            this.lblIQCard.Name = "lblIQCard";
            this.lblIQCard.Size = new System.Drawing.Size(112, 13);
            this.lblIQCard.TabIndex = 1;
            this.lblIQCard.Text = "0000 0000 0000 0000";
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(175, 5);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(48, 13);
            this.lblNome.TabIndex = 2;
            this.lblNome.Text = "IQCARD";
            // 
            // lblTelefone
            // 
            this.lblTelefone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(86, 30);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(0, 13);
            this.lblTelefone.TabIndex = 3;
            // 
            // lblEndereco
            // 
            this.lblEndereco.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblEndereco.AutoSize = true;
            this.lblEndereco.Location = new System.Drawing.Point(344, 30);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(0, 13);
            this.lblEndereco.TabIndex = 4;
            // 
            // dtPedidos
            // 
            this.dtPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtPedidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.data,
            this.valor,
            this.status,
            this.processar,
            this.observacao,
            this.RowKey});
            this.dtPedidos.Location = new System.Drawing.Point(0, 82);
            this.dtPedidos.Name = "dtPedidos";
            this.dtPedidos.RowHeadersVisible = false;
            this.dtPedidos.RowHeadersWidth = 40;
            this.dtPedidos.RowTemplate.Height = 50;
            this.dtPedidos.RowTemplate.ReadOnly = true;
            this.dtPedidos.Size = new System.Drawing.Size(673, 484);
            this.dtPedidos.TabIndex = 1;
            this.dtPedidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtPedidos_CellContentClick);
            // 
            // data
            // 
            this.data.DataPropertyName = "data";
            this.data.HeaderText = "Data";
            this.data.Name = "data";
            // 
            // valor
            // 
            this.valor.DataPropertyName = "valor";
            this.valor.HeaderText = "Valor";
            this.valor.Name = "valor";
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            // 
            // processar
            // 
            this.processar.HeaderText = "";
            this.processar.Name = "processar";
            this.processar.Text = "Processar";
            this.processar.ToolTipText = "Para gerar o cupom fiscal ";
            this.processar.UseColumnTextForButtonValue = true;
            // 
            // observacao
            // 
            this.observacao.DataPropertyName = "observacao";
            this.observacao.HeaderText = "Obs.:";
            this.observacao.Name = "observacao";
            this.observacao.Width = 350;
            // 
            // RowKey
            // 
            this.RowKey.DataPropertyName = "RowKey";
            this.RowKey.HeaderText = "RowKey";
            this.RowKey.Name = "RowKey";
            this.RowKey.Visible = false;
            // 
            // PedidosIQCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 564);
            this.Controls.Add(this.dtPedidos);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PedidosIQCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PEDIDOS FEITO PELO APLICATIVO IQCARD";
            this.Load += new System.EventHandler(this.PedidosIQCard_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPedidos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblIQCard;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.DataGridView dtPedidos;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.DataGridViewTextBoxColumn data;
        private System.Windows.Forms.DataGridViewTextBoxColumn valor;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewButtonColumn processar;
        private System.Windows.Forms.DataGridViewTextBoxColumn observacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowKey;
    }
}