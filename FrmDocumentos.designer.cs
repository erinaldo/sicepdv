namespace SICEpdv
{
    partial class FrmDocumentos
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
            this.DgDocumentos = new System.Windows.Forms.DataGridView();
            this.bntFiltro = new System.Windows.Forms.Button();
            this.lblNomeCliente = new System.Windows.Forms.Label();
            this.lblRNomeCliente = new System.Windows.Forms.Label();
            this.lblNrNF = new System.Windows.Forms.Label();
            this.lblRNrNFe = new System.Windows.Forms.Label();
            this.bntCancelar = new System.Windows.Forms.Button();
            this.lblTotalVenda = new System.Windows.Forms.Label();
            this.lblRTotalDoc = new System.Windows.Forms.Label();
            this.bntNFCe = new System.Windows.Forms.Button();
            this.bntImpressao = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblNFCPendente = new System.Windows.Forms.Label();
            this.pnlIdentificacaoCliente = new System.Windows.Forms.Panel();
            this.grbCliente = new System.Windows.Forms.GroupBox();
            this.chkFisica = new System.Windows.Forms.CheckBox();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnAlterar = new System.Windows.Forms.Button();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCPF = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnExportarXML = new System.Windows.Forms.Button();
            this.btnAbrirPnlInutilizarNFCe = new System.Windows.Forms.Button();
            this.pnlInutilizarNfce = new System.Windows.Forms.Panel();
            this.txtJustificativa = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSairInutilizarNfe = new System.Windows.Forms.Button();
            this.btnInutilizarNfce = new System.Windows.Forms.Button();
            this.txtSerieNFCe = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtNumeroFinalNFCe = new System.Windows.Forms.TextBox();
            this.txtNumeroInicialNFCe = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkImpressao = new System.Windows.Forms.CheckBox();
            this.chktodos = new System.Windows.Forms.CheckBox();
            this.chkPendentes = new System.Windows.Forms.CheckBox();
            this.cbFiliais = new System.Windows.Forms.ComboBox();
            this.lblCodigoFIlial = new System.Windows.Forms.Label();
            this.cbModeloCupom = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCupom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbOperadores = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtDataFinal = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDataInicio = new System.Windows.Forms.DateTimePicker();
            this.bntConsultar = new System.Windows.Forms.Button();
            this.lblStatusNF012 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgDocumentos)).BeginInit();
            this.pnlIdentificacaoCliente.SuspendLayout();
            this.grbCliente.SuspendLayout();
            this.pnlInutilizarNfce.SuspendLayout();
            this.SuspendLayout();
            // 
            // DgDocumentos
            // 
            this.DgDocumentos.AllowUserToAddRows = false;
            this.DgDocumentos.AllowUserToDeleteRows = false;
            this.DgDocumentos.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DgDocumentos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgDocumentos.ColumnHeadersHeight = 20;
            this.DgDocumentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DgDocumentos.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.DgDocumentos.Location = new System.Drawing.Point(13, 25);
            this.DgDocumentos.Name = "DgDocumentos";
            this.DgDocumentos.ReadOnly = true;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.DgDocumentos.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DgDocumentos.RowTemplate.Height = 30;
            this.DgDocumentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgDocumentos.Size = new System.Drawing.Size(980, 336);
            this.DgDocumentos.TabIndex = 0;
            this.DgDocumentos.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgDocumentos_CellEnter);
            this.DgDocumentos.DoubleClick += new System.EventHandler(this.DgDocumentos_DoubleClick);
            this.DgDocumentos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgDocumentos_KeyDown);
            // 
            // bntFiltro
            // 
            this.bntFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.bntFiltro.FlatAppearance.BorderSize = 0;
            this.bntFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntFiltro.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntFiltro.Location = new System.Drawing.Point(428, 370);
            this.bntFiltro.Name = "bntFiltro";
            this.bntFiltro.Size = new System.Drawing.Size(84, 54);
            this.bntFiltro.TabIndex = 6;
            this.bntFiltro.Text = "FILTRAR";
            this.bntFiltro.UseVisualStyleBackColor = false;
            this.bntFiltro.Click += new System.EventHandler(this.Filtro);
            // 
            // lblNomeCliente
            // 
            this.lblNomeCliente.AutoSize = true;
            this.lblNomeCliente.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNomeCliente.Location = new System.Drawing.Point(701, 373);
            this.lblNomeCliente.Name = "lblNomeCliente";
            this.lblNomeCliente.Size = new System.Drawing.Size(49, 14);
            this.lblNomeCliente.TabIndex = 4;
            this.lblNomeCliente.Text = "Cliente:";
            // 
            // lblRNomeCliente
            // 
            this.lblRNomeCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRNomeCliente.ForeColor = System.Drawing.Color.White;
            this.lblRNomeCliente.Location = new System.Drawing.Point(701, 394);
            this.lblRNomeCliente.Name = "lblRNomeCliente";
            this.lblRNomeCliente.Size = new System.Drawing.Size(292, 40);
            this.lblRNomeCliente.TabIndex = 5;
            this.lblRNomeCliente.Text = "_";
            // 
            // lblNrNF
            // 
            this.lblNrNF.AutoSize = true;
            this.lblNrNF.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNrNF.Location = new System.Drawing.Point(701, 493);
            this.lblNrNF.Name = "lblNrNF";
            this.lblNrNF.Size = new System.Drawing.Size(85, 14);
            this.lblNrNF.TabIndex = 8;
            this.lblNrNF.Text = "Número NFCe:";
            // 
            // lblRNrNFe
            // 
            this.lblRNrNFe.AutoSize = true;
            this.lblRNrNFe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRNrNFe.ForeColor = System.Drawing.Color.White;
            this.lblRNrNFe.Location = new System.Drawing.Point(701, 512);
            this.lblRNrNFe.Name = "lblRNrNFe";
            this.lblRNrNFe.Size = new System.Drawing.Size(18, 20);
            this.lblRNrNFe.TabIndex = 9;
            this.lblRNrNFe.Text = "_";
            // 
            // bntCancelar
            // 
            this.bntCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.bntCancelar.FlatAppearance.BorderSize = 0;
            this.bntCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntCancelar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.bntCancelar.Location = new System.Drawing.Point(606, 428);
            this.bntCancelar.Name = "bntCancelar";
            this.bntCancelar.Size = new System.Drawing.Size(84, 54);
            this.bntCancelar.TabIndex = 10;
            this.bntCancelar.Text = "CANCELAR DOCUMENTO";
            this.bntCancelar.UseVisualStyleBackColor = false;
            this.bntCancelar.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblTotalVenda
            // 
            this.lblTotalVenda.AutoSize = true;
            this.lblTotalVenda.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotalVenda.Location = new System.Drawing.Point(701, 441);
            this.lblTotalVenda.Name = "lblTotalVenda";
            this.lblTotalVenda.Size = new System.Drawing.Size(102, 14);
            this.lblTotalVenda.TabIndex = 6;
            this.lblTotalVenda.Text = "Total Documento:";
            // 
            // lblRTotalDoc
            // 
            this.lblRTotalDoc.AutoSize = true;
            this.lblRTotalDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRTotalDoc.ForeColor = System.Drawing.Color.White;
            this.lblRTotalDoc.Location = new System.Drawing.Point(701, 462);
            this.lblRTotalDoc.Name = "lblRTotalDoc";
            this.lblRTotalDoc.Size = new System.Drawing.Size(18, 20);
            this.lblRTotalDoc.TabIndex = 7;
            this.lblRTotalDoc.Text = "_";
            // 
            // bntNFCe
            // 
            this.bntNFCe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.bntNFCe.FlatAppearance.BorderSize = 0;
            this.bntNFCe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntNFCe.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntNFCe.Location = new System.Drawing.Point(517, 370);
            this.bntNFCe.Name = "bntNFCe";
            this.bntNFCe.Size = new System.Drawing.Size(84, 54);
            this.bntNFCe.TabIndex = 11;
            this.bntNFCe.Text = "REENVIAR CUPOM";
            this.bntNFCe.UseVisualStyleBackColor = false;
            this.bntNFCe.Click += new System.EventHandler(this.button2_Click);
            // 
            // bntImpressao
            // 
            this.bntImpressao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.bntImpressao.FlatAppearance.BorderSize = 0;
            this.bntImpressao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntImpressao.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.bntImpressao.Location = new System.Drawing.Point(428, 428);
            this.bntImpressao.Name = "bntImpressao";
            this.bntImpressao.Size = new System.Drawing.Size(84, 54);
            this.bntImpressao.TabIndex = 12;
            this.bntImpressao.Text = "DANFE";
            this.bntImpressao.UseVisualStyleBackColor = false;
            this.bntImpressao.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(517, 428);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 54);
            this.button1.TabIndex = 13;
            this.button1.Text = "GERAR NFCe";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // lblNFCPendente
            // 
            this.lblNFCPendente.AutoSize = true;
            this.lblNFCPendente.Location = new System.Drawing.Point(13, 6);
            this.lblNFCPendente.Name = "lblNFCPendente";
            this.lblNFCPendente.Size = new System.Drawing.Size(13, 13);
            this.lblNFCPendente.TabIndex = 14;
            this.lblNFCPendente.Text = "_";
            // 
            // pnlIdentificacaoCliente
            // 
            this.pnlIdentificacaoCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.pnlIdentificacaoCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlIdentificacaoCliente.Controls.Add(this.grbCliente);
            this.pnlIdentificacaoCliente.ForeColor = System.Drawing.Color.Black;
            this.pnlIdentificacaoCliente.Location = new System.Drawing.Point(310, 72);
            this.pnlIdentificacaoCliente.Name = "pnlIdentificacaoCliente";
            this.pnlIdentificacaoCliente.Size = new System.Drawing.Size(367, 208);
            this.pnlIdentificacaoCliente.TabIndex = 15;
            // 
            // grbCliente
            // 
            this.grbCliente.Controls.Add(this.chkFisica);
            this.grbCliente.Controls.Add(this.btnSair);
            this.grbCliente.Controls.Add(this.btnAlterar);
            this.grbCliente.Controls.Add(this.txtNome);
            this.grbCliente.Controls.Add(this.label9);
            this.grbCliente.Controls.Add(this.txtCPF);
            this.grbCliente.Controls.Add(this.label8);
            this.grbCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grbCliente.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.grbCliente.ForeColor = System.Drawing.Color.White;
            this.grbCliente.Location = new System.Drawing.Point(3, 17);
            this.grbCliente.Name = "grbCliente";
            this.grbCliente.Size = new System.Drawing.Size(360, 186);
            this.grbCliente.TabIndex = 0;
            this.grbCliente.TabStop = false;
            this.grbCliente.Text = "Identificação do Cliente";
            // 
            // chkFisica
            // 
            this.chkFisica.AutoSize = true;
            this.chkFisica.Checked = true;
            this.chkFisica.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFisica.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkFisica.Location = new System.Drawing.Point(252, 43);
            this.chkFisica.Name = "chkFisica";
            this.chkFisica.Size = new System.Drawing.Size(101, 18);
            this.chkFisica.TabIndex = 10;
            this.chkFisica.Text = "Pessoa Fisica";
            this.chkFisica.UseVisualStyleBackColor = true;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSair.Location = new System.Drawing.Point(279, 117);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(75, 51);
            this.btnSair.TabIndex = 7;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnAlterar
            // 
            this.btnAlterar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnAlterar.FlatAppearance.BorderSize = 0;
            this.btnAlterar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlterar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAlterar.Location = new System.Drawing.Point(194, 117);
            this.btnAlterar.Name = "btnAlterar";
            this.btnAlterar.Size = new System.Drawing.Size(79, 51);
            this.btnAlterar.TabIndex = 6;
            this.btnAlterar.Text = "ALTERAR";
            this.btnAlterar.UseVisualStyleBackColor = false;
            this.btnAlterar.Click += new System.EventHandler(this.btnAlterar_Click);
            // 
            // txtNome
            // 
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(90, 72);
            this.txtNome.MaxLength = 60;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(264, 23);
            this.txtNome.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(19, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 14);
            this.label9.TabIndex = 2;
            this.label9.Text = "Nome.:";
            // 
            // txtCPF
            // 
            this.txtCPF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCPF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCPF.Location = new System.Drawing.Point(90, 43);
            this.txtCPF.Name = "txtCPF";
            this.txtCPF.Size = new System.Drawing.Size(156, 23);
            this.txtCPF.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(19, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 14);
            this.label8.TabIndex = 0;
            this.label8.Text = "CPF Cliente.:";
            // 
            // btnExportarXML
            // 
            this.btnExportarXML.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnExportarXML.FlatAppearance.BorderSize = 0;
            this.btnExportarXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarXML.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExportarXML.ForeColor = System.Drawing.Color.White;
            this.btnExportarXML.Location = new System.Drawing.Point(517, 487);
            this.btnExportarXML.Name = "btnExportarXML";
            this.btnExportarXML.Size = new System.Drawing.Size(173, 54);
            this.btnExportarXML.TabIndex = 16;
            this.btnExportarXML.Text = "EXPORTAR XML";
            this.btnExportarXML.UseVisualStyleBackColor = false;
            this.btnExportarXML.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnAbrirPnlInutilizarNFCe
            // 
            this.btnAbrirPnlInutilizarNFCe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.btnAbrirPnlInutilizarNFCe.FlatAppearance.BorderSize = 0;
            this.btnAbrirPnlInutilizarNFCe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirPnlInutilizarNFCe.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAbrirPnlInutilizarNFCe.Location = new System.Drawing.Point(606, 370);
            this.btnAbrirPnlInutilizarNFCe.Name = "btnAbrirPnlInutilizarNFCe";
            this.btnAbrirPnlInutilizarNFCe.Size = new System.Drawing.Size(84, 54);
            this.btnAbrirPnlInutilizarNFCe.TabIndex = 19;
            this.btnAbrirPnlInutilizarNFCe.Text = "INUTILIZAR NFCe";
            this.btnAbrirPnlInutilizarNFCe.UseVisualStyleBackColor = false;
            this.btnAbrirPnlInutilizarNFCe.Click += new System.EventHandler(this.btnAbrirPnlInutilizarNFCe_Click);
            // 
            // pnlInutilizarNfce
            // 
            this.pnlInutilizarNfce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.pnlInutilizarNfce.Controls.Add(this.txtJustificativa);
            this.pnlInutilizarNfce.Controls.Add(this.label14);
            this.pnlInutilizarNfce.Controls.Add(this.btnSairInutilizarNfe);
            this.pnlInutilizarNfce.Controls.Add(this.btnInutilizarNfce);
            this.pnlInutilizarNfce.Controls.Add(this.txtSerieNFCe);
            this.pnlInutilizarNfce.Controls.Add(this.label13);
            this.pnlInutilizarNfce.Controls.Add(this.txtNumeroFinalNFCe);
            this.pnlInutilizarNfce.Controls.Add(this.txtNumeroInicialNFCe);
            this.pnlInutilizarNfce.Controls.Add(this.label12);
            this.pnlInutilizarNfce.Controls.Add(this.label11);
            this.pnlInutilizarNfce.Controls.Add(this.label10);
            this.pnlInutilizarNfce.Location = new System.Drawing.Point(336, 34);
            this.pnlInutilizarNfce.Name = "pnlInutilizarNfce";
            this.pnlInutilizarNfce.Size = new System.Drawing.Size(309, 297);
            this.pnlInutilizarNfce.TabIndex = 20;
            // 
            // txtJustificativa
            // 
            this.txtJustificativa.Location = new System.Drawing.Point(17, 163);
            this.txtJustificativa.MaxLength = 60;
            this.txtJustificativa.Name = "txtJustificativa";
            this.txtJustificativa.Size = new System.Drawing.Size(273, 20);
            this.txtJustificativa.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(14, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 14);
            this.label14.TabIndex = 11;
            this.label14.Text = "Justificativa";
            // 
            // btnSairInutilizarNfe
            // 
            this.btnSairInutilizarNfe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSairInutilizarNfe.FlatAppearance.BorderSize = 0;
            this.btnSairInutilizarNfe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSairInutilizarNfe.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSairInutilizarNfe.Location = new System.Drawing.Point(211, 229);
            this.btnSairInutilizarNfe.Name = "btnSairInutilizarNfe";
            this.btnSairInutilizarNfe.Size = new System.Drawing.Size(79, 51);
            this.btnSairInutilizarNfe.TabIndex = 10;
            this.btnSairInutilizarNfe.Text = "SAIR";
            this.btnSairInutilizarNfe.UseVisualStyleBackColor = false;
            this.btnSairInutilizarNfe.Click += new System.EventHandler(this.btnSairInutilizarNfe_Click);
            // 
            // btnInutilizarNfce
            // 
            this.btnInutilizarNfce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnInutilizarNfce.FlatAppearance.BorderSize = 0;
            this.btnInutilizarNfce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInutilizarNfce.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnInutilizarNfce.Location = new System.Drawing.Point(115, 229);
            this.btnInutilizarNfce.Name = "btnInutilizarNfce";
            this.btnInutilizarNfce.Size = new System.Drawing.Size(94, 51);
            this.btnInutilizarNfce.TabIndex = 9;
            this.btnInutilizarNfce.Text = "INUTILIZAR";
            this.btnInutilizarNfce.UseVisualStyleBackColor = false;
            this.btnInutilizarNfce.Click += new System.EventHandler(this.btnInutilizarNfce_Click);
            // 
            // txtSerieNFCe
            // 
            this.txtSerieNFCe.Enabled = false;
            this.txtSerieNFCe.Location = new System.Drawing.Point(17, 112);
            this.txtSerieNFCe.Name = "txtSerieNFCe";
            this.txtSerieNFCe.Size = new System.Drawing.Size(100, 20);
            this.txtSerieNFCe.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(14, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 14);
            this.label13.TabIndex = 6;
            this.label13.Text = "Série";
            // 
            // txtNumeroFinalNFCe
            // 
            this.txtNumeroFinalNFCe.Location = new System.Drawing.Point(132, 63);
            this.txtNumeroFinalNFCe.Name = "txtNumeroFinalNFCe";
            this.txtNumeroFinalNFCe.Size = new System.Drawing.Size(100, 20);
            this.txtNumeroFinalNFCe.TabIndex = 4;
            this.txtNumeroFinalNFCe.Leave += new System.EventHandler(this.txtNumeroFinalNFCe_Leave);
            // 
            // txtNumeroInicialNFCe
            // 
            this.txtNumeroInicialNFCe.Location = new System.Drawing.Point(17, 63);
            this.txtNumeroInicialNFCe.Name = "txtNumeroInicialNFCe";
            this.txtNumeroInicialNFCe.Size = new System.Drawing.Size(100, 20);
            this.txtNumeroInicialNFCe.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(129, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 14);
            this.label12.TabIndex = 3;
            this.label12.Text = "Número Final";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(14, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 14);
            this.label11.TabIndex = 2;
            this.label11.Text = "Número Inicial";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(14, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 14);
            this.label10.TabIndex = 20;
            this.label10.Text = "Inutilização de NFC-e";
            // 
            // chkImpressao
            // 
            this.chkImpressao.AutoSize = true;
            this.chkImpressao.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkImpressao.Location = new System.Drawing.Point(295, 480);
            this.chkImpressao.Name = "chkImpressao";
            this.chkImpressao.Size = new System.Drawing.Size(81, 18);
            this.chkImpressao.TabIndex = 37;
            this.chkImpressao.Text = "Imp. DANF";
            this.chkImpressao.UseVisualStyleBackColor = true;
            this.chkImpressao.Visible = false;
            // 
            // chktodos
            // 
            this.chktodos.AutoSize = true;
            this.chktodos.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chktodos.Location = new System.Drawing.Point(295, 456);
            this.chktodos.Name = "chktodos";
            this.chktodos.Size = new System.Drawing.Size(84, 18);
            this.chktodos.TabIndex = 36;
            this.chktodos.Text = "Env. Todos";
            this.chktodos.UseVisualStyleBackColor = true;
            this.chktodos.Click += new System.EventHandler(this.chktodos_Click);
            // 
            // chkPendentes
            // 
            this.chkPendentes.AutoSize = true;
            this.chkPendentes.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkPendentes.Location = new System.Drawing.Point(295, 433);
            this.chkPendentes.Name = "chkPendentes";
            this.chkPendentes.Size = new System.Drawing.Size(117, 18);
            this.chkPendentes.TabIndex = 35;
            this.chkPendentes.Text = "Pendentes Proc.";
            this.chkPendentes.UseVisualStyleBackColor = true;
            // 
            // cbFiliais
            // 
            this.cbFiliais.FormattingEnabled = true;
            this.cbFiliais.Location = new System.Drawing.Point(292, 393);
            this.cbFiliais.Name = "cbFiliais";
            this.cbFiliais.Size = new System.Drawing.Size(121, 21);
            this.cbFiliais.TabIndex = 34;
            // 
            // lblCodigoFIlial
            // 
            this.lblCodigoFIlial.AutoSize = true;
            this.lblCodigoFIlial.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCodigoFIlial.Location = new System.Drawing.Point(289, 376);
            this.lblCodigoFIlial.Name = "lblCodigoFIlial";
            this.lblCodigoFIlial.Size = new System.Drawing.Size(79, 14);
            this.lblCodigoFIlial.TabIndex = 33;
            this.lblCodigoFIlial.Text = "Codigo Filial.:";
            // 
            // cbModeloCupom
            // 
            this.cbModeloCupom.FormattingEnabled = true;
            this.cbModeloCupom.Items.AddRange(new object[] {
            "(2D) ECF",
            "(65) NFC",
            "(02) Venda"});
            this.cbModeloCupom.Location = new System.Drawing.Point(155, 473);
            this.cbModeloCupom.Name = "cbModeloCupom";
            this.cbModeloCupom.Size = new System.Drawing.Size(123, 21);
            this.cbModeloCupom.TabIndex = 32;
            this.cbModeloCupom.Text = "(65) NFC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(152, 457);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 14);
            this.label6.TabIndex = 31;
            this.label6.Text = "Modelo";
            // 
            // txtCupom
            // 
            this.txtCupom.Location = new System.Drawing.Point(155, 433);
            this.txtCupom.Name = "txtCupom";
            this.txtCupom.Size = new System.Drawing.Size(123, 20);
            this.txtCupom.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(13, 457);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 14);
            this.label5.TabIndex = 29;
            this.label5.Text = "Documento.:";
            // 
            // txtDocumento
            // 
            this.txtDocumento.Location = new System.Drawing.Point(16, 473);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(123, 20);
            this.txtDocumento.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(152, 417);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 14);
            this.label4.TabIndex = 27;
            this.label4.Text = "COO Cupom.:";
            // 
            // cbOperadores
            // 
            this.cbOperadores.FormattingEnabled = true;
            this.cbOperadores.Location = new System.Drawing.Point(16, 433);
            this.cbOperadores.Name = "cbOperadores";
            this.cbOperadores.Size = new System.Drawing.Size(123, 21);
            this.cbOperadores.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(13, 417);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 14);
            this.label3.TabIndex = 25;
            this.label3.Text = "Operadores.:";
            // 
            // dtDataFinal
            // 
            this.dtDataFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDataFinal.Location = new System.Drawing.Point(155, 394);
            this.dtDataFinal.Name = "dtDataFinal";
            this.dtDataFinal.Size = new System.Drawing.Size(123, 20);
            this.dtDataFinal.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(152, 376);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 14);
            this.label2.TabIndex = 23;
            this.label2.Text = "Data Final.:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(13, 376);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "Data Inical.:";
            // 
            // dtDataInicio
            // 
            this.dtDataInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDataInicio.Location = new System.Drawing.Point(16, 394);
            this.dtDataInicio.Name = "dtDataInicio";
            this.dtDataInicio.Size = new System.Drawing.Size(123, 20);
            this.dtDataInicio.TabIndex = 21;
            // 
            // bntConsultar
            // 
            this.bntConsultar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.bntConsultar.FlatAppearance.BorderSize = 0;
            this.bntConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntConsultar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.bntConsultar.Location = new System.Drawing.Point(428, 487);
            this.bntConsultar.Name = "bntConsultar";
            this.bntConsultar.Size = new System.Drawing.Size(84, 54);
            this.bntConsultar.TabIndex = 38;
            this.bntConsultar.Text = "CONSULTAR NFCe";
            this.bntConsultar.UseVisualStyleBackColor = false;
            this.bntConsultar.Click += new System.EventHandler(this.bntConsultar_Click);
            // 
            // lblStatusNF012
            // 
            this.lblStatusNF012.AutoSize = true;
            this.lblStatusNF012.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusNF012.ForeColor = System.Drawing.Color.White;
            this.lblStatusNF012.Location = new System.Drawing.Point(12, 512);
            this.lblStatusNF012.Name = "lblStatusNF012";
            this.lblStatusNF012.Size = new System.Drawing.Size(18, 20);
            this.lblStatusNF012.TabIndex = 39;
            this.lblStatusNF012.Text = "_";
            // 
            // FrmDocumentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(1005, 555);
            this.Controls.Add(this.lblStatusNF012);
            this.Controls.Add(this.bntConsultar);
            this.Controls.Add(this.chkImpressao);
            this.Controls.Add(this.pnlInutilizarNfce);
            this.Controls.Add(this.chktodos);
            this.Controls.Add(this.btnAbrirPnlInutilizarNFCe);
            this.Controls.Add(this.chkPendentes);
            this.Controls.Add(this.btnExportarXML);
            this.Controls.Add(this.cbFiliais);
            this.Controls.Add(this.pnlIdentificacaoCliente);
            this.Controls.Add(this.lblCodigoFIlial);
            this.Controls.Add(this.lblNFCPendente);
            this.Controls.Add(this.cbModeloCupom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bntImpressao);
            this.Controls.Add(this.txtCupom);
            this.Controls.Add(this.bntNFCe);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bntCancelar);
            this.Controls.Add(this.txtDocumento);
            this.Controls.Add(this.lblRNrNFe);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNrNF);
            this.Controls.Add(this.cbOperadores);
            this.Controls.Add(this.lblRTotalDoc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalVenda);
            this.Controls.Add(this.dtDataFinal);
            this.Controls.Add(this.bntFiltro);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRNomeCliente);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNomeCliente);
            this.Controls.Add(this.dtDataInicio);
            this.Controls.Add(this.DgDocumentos);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDocumentos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relação de Documentos";
            this.Load += new System.EventHandler(this.FrmDocumentos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgDocumentos)).EndInit();
            this.pnlIdentificacaoCliente.ResumeLayout(false);
            this.grbCliente.ResumeLayout(false);
            this.grbCliente.PerformLayout();
            this.pnlInutilizarNfce.ResumeLayout(false);
            this.pnlInutilizarNfce.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgDocumentos;
        private System.Windows.Forms.Button bntFiltro;
        private System.Windows.Forms.Label lblNomeCliente;
        private System.Windows.Forms.Label lblRNomeCliente;
        private System.Windows.Forms.Label lblNrNF;
        private System.Windows.Forms.Label lblRNrNFe;
        private System.Windows.Forms.Button bntCancelar;
        private System.Windows.Forms.Label lblTotalVenda;
        private System.Windows.Forms.Label lblRTotalDoc;
        private System.Windows.Forms.Button bntNFCe;
        private System.Windows.Forms.Button bntImpressao;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblNFCPendente;
        private System.Windows.Forms.Panel pnlIdentificacaoCliente;
        private System.Windows.Forms.GroupBox grbCliente;
        private System.Windows.Forms.MaskedTextBox txtCPF;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnAlterar;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnExportarXML;
        private System.Windows.Forms.CheckBox chkFisica;
        private System.Windows.Forms.Button btnAbrirPnlInutilizarNFCe;
        private System.Windows.Forms.Panel pnlInutilizarNfce;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSerieNFCe;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtNumeroFinalNFCe;
        private System.Windows.Forms.TextBox txtNumeroInicialNFCe;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnInutilizarNfce;
        private System.Windows.Forms.Button btnSairInutilizarNfe;
        private System.Windows.Forms.TextBox txtJustificativa;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkImpressao;
        private System.Windows.Forms.CheckBox chktodos;
        private System.Windows.Forms.CheckBox chkPendentes;
        private System.Windows.Forms.ComboBox cbFiliais;
        private System.Windows.Forms.Label lblCodigoFIlial;
        private System.Windows.Forms.ComboBox cbModeloCupom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCupom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbOperadores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtDataFinal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtDataInicio;
        private System.Windows.Forms.Button bntConsultar;
        private System.Windows.Forms.Label lblStatusNF012;
    }
}