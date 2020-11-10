namespace SICEpdv
{
    partial class FrmFinalizarPreVenda
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFinalizarPreVenda));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNumero = new System.Windows.Forms.Label();
            this.btnSair = new System.Windows.Forms.Button();
            this.vendasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lstPagamento = new System.Windows.Forms.ListBox();
            this.btConcluir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnPagDH = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.pnlAjuste = new System.Windows.Forms.Panel();
            this.chkAjusteDiferenca = new System.Windows.Forms.CheckBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.btnCancelarAjuste = new System.Windows.Forms.Button();
            this.btnConfirmarAjuste = new System.Windows.Forms.Button();
            this.chkPagamentos = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtgItens = new System.Windows.Forms.DataGridView();
            this.codigoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.produtoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidadeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalitem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtdatual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtdatualizada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlItens = new System.Windows.Forms.Panel();
            this.btnEncerrar = new System.Windows.Forms.Button();
            this.pnlDados = new System.Windows.Forms.Panel();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblVendedor = new System.Windows.Forms.Label();
            this.btnAlterarPgt = new System.Windows.Forms.Button();
            this.btSair = new System.Windows.Forms.Button();
            this.lblDesconto = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.pnStatusPedido = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.lblStatusDAV = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStatusPagamento = new System.Windows.Forms.Label();
            this.pnlStatusPagamento = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.vendasBindingSource)).BeginInit();
            this.pnlAjuste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).BeginInit();
            this.pnlItens.SuspendLayout();
            this.pnlDados.SuspendLayout();
            this.pnStatusPedido.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.Location = new System.Drawing.Point(517, 69);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(238, 234);
            this.pnlTeclado.TabIndex = 2;
            this.pnlTeclado.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Número(s):";
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumero.ForeColor = System.Drawing.Color.White;
            this.lblNumero.Location = new System.Drawing.Point(69, 4);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(15, 16);
            this.lblNumero.TabIndex = 4;
            this.lblNumero.Text = "0";
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnSair.Location = new System.Drawing.Point(-235, 396);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(70, 40);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            // 
            // vendasBindingSource
            // 
            this.vendasBindingSource.AllowNew = true;
            this.vendasBindingSource.DataSource = typeof(SICEpdv.vendas);
            this.vendasBindingSource.Sort = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 507);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(124, 505);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(45, 24);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 402);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Descrição Pagamento:";
            // 
            // lstPagamento
            // 
            this.lstPagamento.BackColor = System.Drawing.Color.White;
            this.lstPagamento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPagamento.FormattingEnabled = true;
            this.lstPagamento.ItemHeight = 16;
            this.lstPagamento.Location = new System.Drawing.Point(12, 420);
            this.lstPagamento.Name = "lstPagamento";
            this.lstPagamento.Size = new System.Drawing.Size(157, 82);
            this.lstPagamento.TabIndex = 14;
            // 
            // btConcluir
            // 
            this.btConcluir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btConcluir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btConcluir.FlatAppearance.BorderSize = 0;
            this.btConcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConcluir.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConcluir.ForeColor = System.Drawing.Color.White;
            this.btConcluir.Location = new System.Drawing.Point(335, 490);
            this.btConcluir.Name = "btConcluir";
            this.btConcluir.Size = new System.Drawing.Size(85, 40);
            this.btConcluir.TabIndex = 0;
            this.btConcluir.Text = "FINALIZAR";
            this.btConcluir.UseVisualStyleBackColor = false;
            this.btConcluir.Click += new System.EventHandler(this.btConcluir_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(426, 442);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 40);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnPagDH
            // 
            this.btnPagDH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.btnPagDH.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPagDH.FlatAppearance.BorderSize = 0;
            this.btnPagDH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagDH.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagDH.ForeColor = System.Drawing.Color.White;
            this.btnPagDH.Location = new System.Drawing.Point(335, 441);
            this.btnPagDH.Name = "btnPagDH";
            this.btnPagDH.Size = new System.Drawing.Size(85, 40);
            this.btnPagDH.TabIndex = 2;
            this.btnPagDH.Text = "PAGAMENTO À VISTA";
            this.btnPagDH.UseVisualStyleBackColor = false;
            this.btnPagDH.Click += new System.EventHandler(this.btnPagDH_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExcluir.BackgroundImage")));
            this.btnExcluir.Location = new System.Drawing.Point(53, 333);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(37, 32);
            this.btnExcluir.TabIndex = 24;
            this.btnExcluir.TabStop = false;
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdicionar.BackgroundImage")));
            this.btnAdicionar.Location = new System.Drawing.Point(4, 333);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(43, 32);
            this.btnAdicionar.TabIndex = 25;
            this.btnAdicionar.TabStop = false;
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // pnlAjuste
            // 
            this.pnlAjuste.Controls.Add(this.chkAjusteDiferenca);
            this.pnlAjuste.Controls.Add(this.lblValor);
            this.pnlAjuste.Controls.Add(this.btnCancelarAjuste);
            this.pnlAjuste.Controls.Add(this.btnConfirmarAjuste);
            this.pnlAjuste.Controls.Add(this.chkPagamentos);
            this.pnlAjuste.Controls.Add(this.label4);
            this.pnlAjuste.Location = new System.Drawing.Point(73, 104);
            this.pnlAjuste.Name = "pnlAjuste";
            this.pnlAjuste.Size = new System.Drawing.Size(266, 199);
            this.pnlAjuste.TabIndex = 26;
            this.pnlAjuste.Visible = false;
            // 
            // chkAjusteDiferenca
            // 
            this.chkAjusteDiferenca.AutoSize = true;
            this.chkAjusteDiferenca.ForeColor = System.Drawing.Color.White;
            this.chkAjusteDiferenca.Location = new System.Drawing.Point(11, 118);
            this.chkAjusteDiferenca.Name = "chkAjusteDiferenca";
            this.chkAjusteDiferenca.Size = new System.Drawing.Size(107, 17);
            this.chkAjusteDiferenca.TabIndex = 5;
            this.chkAjusteDiferenca.Text = "Ajustar Diferença";
            this.chkAjusteDiferenca.UseVisualStyleBackColor = true;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.ForeColor = System.Drawing.Color.White;
            this.lblValor.Location = new System.Drawing.Point(141, 8);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(32, 13);
            this.lblValor.TabIndex = 4;
            this.lblValor.Text = "0.00";
            // 
            // btnCancelarAjuste
            // 
            this.btnCancelarAjuste.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancelarAjuste.FlatAppearance.BorderSize = 0;
            this.btnCancelarAjuste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarAjuste.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarAjuste.ForeColor = System.Drawing.Color.White;
            this.btnCancelarAjuste.Location = new System.Drawing.Point(177, 155);
            this.btnCancelarAjuste.Name = "btnCancelarAjuste";
            this.btnCancelarAjuste.Size = new System.Drawing.Size(82, 38);
            this.btnCancelarAjuste.TabIndex = 3;
            this.btnCancelarAjuste.Text = "CANCELAR";
            this.btnCancelarAjuste.UseVisualStyleBackColor = false;
            this.btnCancelarAjuste.Click += new System.EventHandler(this.btnCancelarAjuste_Click);
            // 
            // btnConfirmarAjuste
            // 
            this.btnConfirmarAjuste.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnConfirmarAjuste.FlatAppearance.BorderSize = 0;
            this.btnConfirmarAjuste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarAjuste.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmarAjuste.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarAjuste.Location = new System.Drawing.Point(88, 155);
            this.btnConfirmarAjuste.Name = "btnConfirmarAjuste";
            this.btnConfirmarAjuste.Size = new System.Drawing.Size(83, 38);
            this.btnConfirmarAjuste.TabIndex = 2;
            this.btnConfirmarAjuste.Text = "CONFIRMAR";
            this.btnConfirmarAjuste.UseVisualStyleBackColor = false;
            this.btnConfirmarAjuste.Click += new System.EventHandler(this.btnConfirmarAjuste_Click);
            // 
            // chkPagamentos
            // 
            this.chkPagamentos.CheckOnClick = true;
            this.chkPagamentos.FormattingEnabled = true;
            this.chkPagamentos.Location = new System.Drawing.Point(11, 48);
            this.chkPagamentos.Name = "chkPagamentos";
            this.chkPagamentos.Size = new System.Drawing.Size(127, 64);
            this.chkPagamentos.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Selecione o pag.\r\npara ajuste:";
            // 
            // dtgItens
            // 
            this.dtgItens.AllowUserToAddRows = false;
            this.dtgItens.AllowUserToDeleteRows = false;
            this.dtgItens.AllowUserToOrderColumns = true;
            this.dtgItens.AutoGenerateColumns = false;
            this.dtgItens.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dtgItens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtgItens.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dtgItens.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dtgItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigoDataGridViewTextBoxColumn,
            this.produtoDataGridViewTextBoxColumn,
            this.precoDataGridViewTextBoxColumn,
            this.quantidadeDataGridViewTextBoxColumn,
            this.totalitem,
            this.qtdatual,
            this.qtdatualizada,
            this.inc});
            this.dtgItens.DataSource = this.vendasBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgItens.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtgItens.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dtgItens.Location = new System.Drawing.Point(7, 69);
            this.dtgItens.MultiSelect = false;
            this.dtgItens.Name = "dtgItens";
            this.dtgItens.ReadOnly = true;
            this.dtgItens.RowHeadersVisible = false;
            this.dtgItens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgItens.Size = new System.Drawing.Size(504, 258);
            this.dtgItens.TabIndex = 10;
            this.dtgItens.TabStop = false;
            this.dtgItens.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgItens_CellContentClick);
            this.dtgItens.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtgItens_CellFormatting);
            // 
            // codigoDataGridViewTextBoxColumn
            // 
            this.codigoDataGridViewTextBoxColumn.DataPropertyName = "codigo";
            this.codigoDataGridViewTextBoxColumn.HeaderText = "Cód:";
            this.codigoDataGridViewTextBoxColumn.Name = "codigoDataGridViewTextBoxColumn";
            this.codigoDataGridViewTextBoxColumn.ReadOnly = true;
            this.codigoDataGridViewTextBoxColumn.Width = 60;
            // 
            // produtoDataGridViewTextBoxColumn
            // 
            this.produtoDataGridViewTextBoxColumn.DataPropertyName = "produto";
            this.produtoDataGridViewTextBoxColumn.HeaderText = "Descrição";
            this.produtoDataGridViewTextBoxColumn.Name = "produtoDataGridViewTextBoxColumn";
            this.produtoDataGridViewTextBoxColumn.ReadOnly = true;
            this.produtoDataGridViewTextBoxColumn.Width = 150;
            // 
            // precoDataGridViewTextBoxColumn
            // 
            this.precoDataGridViewTextBoxColumn.DataPropertyName = "preco";
            this.precoDataGridViewTextBoxColumn.HeaderText = "Preço";
            this.precoDataGridViewTextBoxColumn.Name = "precoDataGridViewTextBoxColumn";
            this.precoDataGridViewTextBoxColumn.ReadOnly = true;
            this.precoDataGridViewTextBoxColumn.Width = 60;
            // 
            // quantidadeDataGridViewTextBoxColumn
            // 
            this.quantidadeDataGridViewTextBoxColumn.DataPropertyName = "quantidade";
            this.quantidadeDataGridViewTextBoxColumn.HeaderText = "Qtd.";
            this.quantidadeDataGridViewTextBoxColumn.Name = "quantidadeDataGridViewTextBoxColumn";
            this.quantidadeDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantidadeDataGridViewTextBoxColumn.Width = 50;
            // 
            // totalitem
            // 
            this.totalitem.DataPropertyName = "total";
            this.totalitem.HeaderText = "Total";
            this.totalitem.Name = "totalitem";
            this.totalitem.ReadOnly = true;
            this.totalitem.Width = 60;
            // 
            // qtdatual
            // 
            this.qtdatual.DataPropertyName = "quantidadeanterior";
            this.qtdatual.HeaderText = "Qtd.Atual";
            this.qtdatual.Name = "qtdatual";
            this.qtdatual.ReadOnly = true;
            this.qtdatual.Width = 50;
            // 
            // qtdatualizada
            // 
            this.qtdatualizada.DataPropertyName = "quantidadeatualizada";
            this.qtdatualizada.HeaderText = "Q.Apos V.";
            this.qtdatualizada.Name = "qtdatualizada";
            this.qtdatualizada.ReadOnly = true;
            this.qtdatualizada.Width = 50;
            // 
            // inc
            // 
            this.inc.DataPropertyName = "inc";
            this.inc.HeaderText = "id";
            this.inc.Name = "inc";
            this.inc.ReadOnly = true;
            // 
            // pnlItens
            // 
            this.pnlItens.Controls.Add(this.btnEncerrar);
            this.pnlItens.Location = new System.Drawing.Point(15, 141);
            this.pnlItens.Name = "pnlItens";
            this.pnlItens.Size = new System.Drawing.Size(485, 134);
            this.pnlItens.TabIndex = 27;
            this.pnlItens.Visible = false;
            // 
            // btnEncerrar
            // 
            this.btnEncerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnEncerrar.FlatAppearance.BorderSize = 0;
            this.btnEncerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEncerrar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEncerrar.ForeColor = System.Drawing.Color.White;
            this.btnEncerrar.Location = new System.Drawing.Point(397, 90);
            this.btnEncerrar.Name = "btnEncerrar";
            this.btnEncerrar.Size = new System.Drawing.Size(82, 41);
            this.btnEncerrar.TabIndex = 0;
            this.btnEncerrar.Text = "ENCERRAR";
            this.btnEncerrar.UseVisualStyleBackColor = false;
            this.btnEncerrar.Click += new System.EventHandler(this.btnEncerrar_Click);
            // 
            // pnlDados
            // 
            this.pnlDados.Controls.Add(this.lblCliente);
            this.pnlDados.Location = new System.Drawing.Point(5, 36);
            this.pnlDados.Name = "pnlDados";
            this.pnlDados.Size = new System.Drawing.Size(506, 22);
            this.pnlDados.TabIndex = 28;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.ForeColor = System.Drawing.Color.White;
            this.lblCliente.Location = new System.Drawing.Point(3, 4);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(39, 13);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente";
            // 
            // lblVendedor
            // 
            this.lblVendedor.AutoSize = true;
            this.lblVendedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendedor.ForeColor = System.Drawing.Color.White;
            this.lblVendedor.Location = new System.Drawing.Point(4, 19);
            this.lblVendedor.Name = "lblVendedor";
            this.lblVendedor.Size = new System.Drawing.Size(41, 13);
            this.lblVendedor.TabIndex = 1;
            this.lblVendedor.Text = "label5";
            // 
            // btnAlterarPgt
            // 
            this.btnAlterarPgt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.btnAlterarPgt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlterarPgt.FlatAppearance.BorderSize = 0;
            this.btnAlterarPgt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlterarPgt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlterarPgt.ForeColor = System.Drawing.Color.White;
            this.btnAlterarPgt.Location = new System.Drawing.Point(335, 395);
            this.btnAlterarPgt.Name = "btnAlterarPgt";
            this.btnAlterarPgt.Size = new System.Drawing.Size(176, 40);
            this.btnAlterarPgt.TabIndex = 29;
            this.btnAlterarPgt.Text = "FINALIZAR COM ALTERAÇÃO DE PAGAMENTO";
            this.btnAlterarPgt.UseVisualStyleBackColor = false;
            this.btnAlterarPgt.Click += new System.EventHandler(this.btnAlterarPgt_Click);
            // 
            // btSair
            // 
            this.btSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btSair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSair.FlatAppearance.BorderSize = 0;
            this.btSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSair.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSair.ForeColor = System.Drawing.Color.White;
            this.btSair.Location = new System.Drawing.Point(426, 490);
            this.btSair.Name = "btSair";
            this.btSair.Size = new System.Drawing.Size(85, 40);
            this.btSair.TabIndex = 30;
            this.btSair.Text = "SAIR";
            this.btSair.UseVisualStyleBackColor = false;
            this.btSair.Click += new System.EventHandler(this.btSair_Click);
            // 
            // lblDesconto
            // 
            this.lblDesconto.AutoSize = true;
            this.lblDesconto.ForeColor = System.Drawing.Color.Red;
            this.lblDesconto.Location = new System.Drawing.Point(10, 383);
            this.lblDesconto.Name = "lblDesconto";
            this.lblDesconto.Size = new System.Drawing.Size(80, 13);
            this.lblDesconto.TabIndex = 31;
            this.lblDesconto.Text = "Desconto: 0,00";
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.btnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImprimir.FlatAppearance.BorderSize = 0;
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(335, 333);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(176, 41);
            this.btnImprimir.TabIndex = 32;
            this.btnImprimir.Text = "IMPRIMIR";
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // pnStatusPedido
            // 
            this.pnStatusPedido.Controls.Add(this.label6);
            this.pnStatusPedido.Controls.Add(this.lblStatusDAV);
            this.pnStatusPedido.Controls.Add(this.label5);
            this.pnStatusPedido.Controls.Add(this.lblStatusPagamento);
            this.pnStatusPedido.Controls.Add(this.pnlStatusPagamento);
            this.pnStatusPedido.Location = new System.Drawing.Point(175, 339);
            this.pnStatusPedido.Name = "pnStatusPedido";
            this.pnStatusPedido.Size = new System.Drawing.Size(154, 163);
            this.pnStatusPedido.TabIndex = 33;
            this.pnStatusPedido.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Status Pagamento";
            // 
            // lblStatusDAV
            // 
            this.lblStatusDAV.AutoSize = true;
            this.lblStatusDAV.ForeColor = System.Drawing.Color.White;
            this.lblStatusDAV.Location = new System.Drawing.Point(87, 63);
            this.lblStatusDAV.Name = "lblStatusDAV";
            this.lblStatusDAV.Size = new System.Drawing.Size(16, 13);
            this.lblStatusDAV.TabIndex = 3;
            this.lblStatusDAV.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Status DAV.:";
            // 
            // lblStatusPagamento
            // 
            this.lblStatusPagamento.AutoSize = true;
            this.lblStatusPagamento.ForeColor = System.Drawing.Color.White;
            this.lblStatusPagamento.Location = new System.Drawing.Point(44, 44);
            this.lblStatusPagamento.Name = "lblStatusPagamento";
            this.lblStatusPagamento.Size = new System.Drawing.Size(16, 13);
            this.lblStatusPagamento.TabIndex = 1;
            this.lblStatusPagamento.Text = "...";
            // 
            // pnlStatusPagamento
            // 
            this.pnlStatusPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlStatusPagamento.Location = new System.Drawing.Point(16, 32);
            this.pnlStatusPagamento.Name = "pnlStatusPagamento";
            this.pnlStatusPagamento.Size = new System.Drawing.Size(22, 25);
            this.pnlStatusPagamento.TabIndex = 0;
            // 
            // FrmFinalizarPreVenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(523, 542);
            this.Controls.Add(this.pnStatusPedido);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.pnlItens);
            this.Controls.Add(this.pnlAjuste);
            this.Controls.Add(this.lblVendedor);
            this.Controls.Add(this.lblDesconto);
            this.Controls.Add(this.btSair);
            this.Controls.Add(this.btnAlterarPgt);
            this.Controls.Add(this.pnlDados);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.btnPagDH);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btConcluir);
            this.Controls.Add(this.lstPagamento);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.lblNumero);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlTeclado);
            this.Controls.Add(this.dtgItens);
            this.Controls.Add(this.lblTotal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "FrmFinalizarPreVenda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Finalizar - F7 menu Fiscal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFinalizarPreVenda_FormClosed);
            this.Load += new System.EventHandler(this.FrmFinalizarPreVenda_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFinalizarPreVenda_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmFinalizarPreVenda_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.vendasBindingSource)).EndInit();
            this.pnlAjuste.ResumeLayout(false);
            this.pnlAjuste.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgItens)).EndInit();
            this.pnlItens.ResumeLayout(false);
            this.pnlDados.ResumeLayout(false);
            this.pnlDados.PerformLayout();
            this.pnStatusPedido.ResumeLayout(false);
            this.pnStatusPedido.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.BindingSource vendasBindingSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstPagamento;
        private System.Windows.Forms.Button btConcluir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnPagDH;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Panel pnlAjuste;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnConfirmarAjuste;
        private System.Windows.Forms.CheckedListBox chkPagamentos;
        private System.Windows.Forms.Button btnCancelarAjuste;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.DataGridView dtgItens;
        private System.Windows.Forms.Panel pnlItens;
        private System.Windows.Forms.Button btnEncerrar;
        private System.Windows.Forms.Panel pnlDados;
        private System.Windows.Forms.Label lblVendedor;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Button btnAlterarPgt;
        private System.Windows.Forms.Button btSair;
        private System.Windows.Forms.Label lblDesconto;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn produtoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidadeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalitem;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtdatual;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtdatualizada;
        private System.Windows.Forms.DataGridViewTextBoxColumn inc;
        private System.Windows.Forms.CheckBox chkAjusteDiferenca;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Panel pnStatusPedido;
        private System.Windows.Forms.Label lblStatusDAV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStatusPagamento;
        private System.Windows.Forms.Panel pnlStatusPagamento;
        private System.Windows.Forms.Label label6;
    }
}