namespace SICEpdv
{
    partial class ucClientePdv
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblIdCliente = new System.Windows.Forms.Label();
            this.txtIdCliente = new System.Windows.Forms.TextBox();
            this.pnlParcelamentoCR = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDiasVenc = new System.Windows.Forms.TextBox();
            this.chkAltCR = new System.Windows.Forms.CheckBox();
            this.btConfirmarCR = new System.Windows.Forms.Button();
            this.txtIntervaloCR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtParcelamentoCR = new System.Windows.Forms.TextBox();
            this.txtParcelas = new System.Windows.Forms.Label();
            this.vencimentoCR = new System.Windows.Forms.DateTimePicker();
            this.lblVencimento = new System.Windows.Forms.Label();
            this.layoutConsumidor = new System.Windows.Forms.TableLayoutPanel();
            this.txtConsumidor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndConsumidor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btSairCR = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboRedeConsulta = new System.Windows.Forms.ComboBox();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.pnlCheque = new System.Windows.Forms.Panel();
            this.txtConta = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.chkAltCH = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTelefoneCH = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCPFCNPJch = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtNomeCH = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtNrCheque = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAgencia = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCodBanco = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtValorIndCH = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btConfirmarCH = new System.Windows.Forms.Button();
            this.txtIntervaloCH = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtParcelamentoCH = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.vencimentoCH = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExtrato = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.chkVerDependentes = new System.Windows.Forms.CheckBox();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblCidade = new System.Windows.Forms.Label();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.lblCpfCnpj = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlParcelamentoCR.SuspendLayout();
            this.layoutConsumidor.SuspendLayout();
            this.pnlCheque.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIdCliente
            // 
            this.lblIdCliente.AutoSize = true;
            this.lblIdCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdCliente.ForeColor = System.Drawing.Color.White;
            this.lblIdCliente.Location = new System.Drawing.Point(35, 74);
            this.lblIdCliente.Name = "lblIdCliente";
            this.lblIdCliente.Size = new System.Drawing.Size(120, 16);
            this.lblIdCliente.TabIndex = 0;
            this.lblIdCliente.Tag = "";
            this.lblIdCliente.Text = "CPF/CNPJ Cliente:";
            this.lblIdCliente.Click += new System.EventHandler(this.lblIdCliente_Click);
            // 
            // txtIdCliente
            // 
            this.txtIdCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdCliente.Location = new System.Drawing.Point(157, 68);
            this.txtIdCliente.MaxLength = 18;
            this.txtIdCliente.Name = "txtIdCliente";
            this.txtIdCliente.Size = new System.Drawing.Size(171, 26);
            this.txtIdCliente.TabIndex = 0;
            this.txtIdCliente.Tag = "";
            this.toolTip1.SetToolTip(this.txtIdCliente, "Código, CPF ou CNPJ");
            this.txtIdCliente.TextChanged += new System.EventHandler(this.txtIdCliente_TextChanged);
            this.txtIdCliente.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIdCliente_KeyDown);
            // 
            // pnlParcelamentoCR
            // 
            this.pnlParcelamentoCR.Controls.Add(this.label15);
            this.pnlParcelamentoCR.Controls.Add(this.txtDiasVenc);
            this.pnlParcelamentoCR.Controls.Add(this.chkAltCR);
            this.pnlParcelamentoCR.Controls.Add(this.btConfirmarCR);
            this.pnlParcelamentoCR.Controls.Add(this.txtIntervaloCR);
            this.pnlParcelamentoCR.Controls.Add(this.label3);
            this.pnlParcelamentoCR.Controls.Add(this.txtParcelamentoCR);
            this.pnlParcelamentoCR.Controls.Add(this.txtParcelas);
            this.pnlParcelamentoCR.Controls.Add(this.vencimentoCR);
            this.pnlParcelamentoCR.Controls.Add(this.lblVencimento);
            this.pnlParcelamentoCR.Location = new System.Drawing.Point(39, 235);
            this.pnlParcelamentoCR.Name = "pnlParcelamentoCR";
            this.pnlParcelamentoCR.Size = new System.Drawing.Size(352, 54);
            this.pnlParcelamentoCR.TabIndex = 2;
            this.pnlParcelamentoCR.Visible = false;
            this.pnlParcelamentoCR.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlParcelamentoCR_Paint);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(9, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Dias";
            // 
            // txtDiasVenc
            // 
            this.txtDiasVenc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiasVenc.Location = new System.Drawing.Point(6, 17);
            this.txtDiasVenc.Name = "txtDiasVenc";
            this.txtDiasVenc.Size = new System.Drawing.Size(36, 26);
            this.txtDiasVenc.TabIndex = 0;
            this.txtDiasVenc.Text = "30";
            // 
            // chkAltCR
            // 
            this.chkAltCR.AutoSize = true;
            this.chkAltCR.ForeColor = System.Drawing.Color.White;
            this.chkAltCR.Location = new System.Drawing.Point(223, 15);
            this.chkAltCR.Name = "chkAltCR";
            this.chkAltCR.Size = new System.Drawing.Size(51, 30);
            this.chkAltCR.TabIndex = 5;
            this.chkAltCR.Text = "Alt.\r\nParc.";
            this.chkAltCR.UseVisualStyleBackColor = true;
            // 
            // btConfirmarCR
            // 
            this.btConfirmarCR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btConfirmarCR.FlatAppearance.BorderSize = 0;
            this.btConfirmarCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConfirmarCR.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConfirmarCR.ForeColor = System.Drawing.Color.White;
            this.btConfirmarCR.Location = new System.Drawing.Point(276, 13);
            this.btConfirmarCR.Name = "btConfirmarCR";
            this.btConfirmarCR.Size = new System.Drawing.Size(73, 30);
            this.btConfirmarCR.TabIndex = 4;
            this.btConfirmarCR.Text = "&Confirmar";
            this.btConfirmarCR.UseVisualStyleBackColor = false;
            // 
            // txtIntervaloCR
            // 
            this.txtIntervaloCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIntervaloCR.Location = new System.Drawing.Point(190, 19);
            this.txtIntervaloCR.MaxLength = 3;
            this.txtIntervaloCR.Name = "txtIntervaloCR";
            this.txtIntervaloCR.Size = new System.Drawing.Size(30, 26);
            this.txtIntervaloCR.TabIndex = 3;
            this.txtIntervaloCR.Text = "30";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(189, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Intervalo";
            // 
            // txtParcelamentoCR
            // 
            this.txtParcelamentoCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParcelamentoCR.Location = new System.Drawing.Point(159, 19);
            this.txtParcelamentoCR.MaxLength = 2;
            this.txtParcelamentoCR.Name = "txtParcelamentoCR";
            this.txtParcelamentoCR.Size = new System.Drawing.Size(29, 26);
            this.txtParcelamentoCR.TabIndex = 2;
            this.txtParcelamentoCR.Text = "1";
            // 
            // txtParcelas
            // 
            this.txtParcelas.AutoSize = true;
            this.txtParcelas.ForeColor = System.Drawing.Color.White;
            this.txtParcelas.Location = new System.Drawing.Point(156, 3);
            this.txtParcelas.Name = "txtParcelas";
            this.txtParcelas.Size = new System.Drawing.Size(32, 13);
            this.txtParcelas.TabIndex = 2;
            this.txtParcelas.Text = "Parc.";
            // 
            // vencimentoCR
            // 
            this.vencimentoCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vencimentoCR.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.vencimentoCR.Location = new System.Drawing.Point(48, 18);
            this.vencimentoCR.Name = "vencimentoCR";
            this.vencimentoCR.Size = new System.Drawing.Size(105, 26);
            this.vencimentoCR.TabIndex = 1;
            this.vencimentoCR.Value = new System.DateTime(2009, 11, 24, 0, 0, 0, 0);
            this.vencimentoCR.ValueChanged += new System.EventHandler(this.vencimentoCR_ValueChanged);
            // 
            // lblVencimento
            // 
            this.lblVencimento.AutoSize = true;
            this.lblVencimento.ForeColor = System.Drawing.Color.White;
            this.lblVencimento.Location = new System.Drawing.Point(46, 3);
            this.lblVencimento.Name = "lblVencimento";
            this.lblVencimento.Size = new System.Drawing.Size(73, 13);
            this.lblVencimento.TabIndex = 0;
            this.lblVencimento.Text = "1ºVencimento";
            // 
            // layoutConsumidor
            // 
            this.layoutConsumidor.ColumnCount = 2;
            this.layoutConsumidor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.09941F));
            this.layoutConsumidor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.90058F));
            this.layoutConsumidor.Controls.Add(this.txtConsumidor, 1, 0);
            this.layoutConsumidor.Controls.Add(this.label2, 0, 1);
            this.layoutConsumidor.Controls.Add(this.txtEndConsumidor, 1, 1);
            this.layoutConsumidor.Controls.Add(this.label1, 0, 0);
            this.layoutConsumidor.Location = new System.Drawing.Point(39, 124);
            this.layoutConsumidor.Name = "layoutConsumidor";
            this.layoutConsumidor.RowCount = 2;
            this.layoutConsumidor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutConsumidor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutConsumidor.Size = new System.Drawing.Size(350, 74);
            this.layoutConsumidor.TabIndex = 1;
            this.layoutConsumidor.Visible = false;
            this.layoutConsumidor.Paint += new System.Windows.Forms.PaintEventHandler(this.layoutConsumidor_Paint);
            // 
            // txtConsumidor
            // 
            this.txtConsumidor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsumidor.Location = new System.Drawing.Point(83, 3);
            this.txtConsumidor.MaxLength = 30;
            this.txtConsumidor.Name = "txtConsumidor";
            this.txtConsumidor.Size = new System.Drawing.Size(260, 29);
            this.txtConsumidor.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Endereço:";
            // 
            // txtEndConsumidor
            // 
            this.txtEndConsumidor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndConsumidor.Location = new System.Drawing.Point(83, 40);
            this.txtEndConsumidor.MaxLength = 60;
            this.txtEndConsumidor.Name = "txtEndConsumidor";
            this.txtEndConsumidor.Size = new System.Drawing.Size(260, 29);
            this.txtEndConsumidor.TabIndex = 2;
            this.txtEndConsumidor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEndConsumidor_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Consumidor:";
            // 
            // btSairCR
            // 
            this.btSairCR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btSairCR.FlatAppearance.BorderSize = 0;
            this.btSairCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSairCR.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSairCR.ForeColor = System.Drawing.Color.White;
            this.btSairCR.Location = new System.Drawing.Point(288, 292);
            this.btSairCR.Name = "btSairCR";
            this.btSairCR.Size = new System.Drawing.Size(100, 35);
            this.btSairCR.TabIndex = 4;
            this.btSairCR.Text = "CONFIRMAR";
            this.btSairCR.UseVisualStyleBackColor = false;
            this.btSairCR.Click += new System.EventHandler(this.btSairCR_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 40;
            this.toolTip1.Tag = "Código do Cliente, CPF ou CNPJ";
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // cboRedeConsulta
            // 
            this.cboRedeConsulta.DisplayMember = "1";
            this.cboRedeConsulta.FormattingEnabled = true;
            this.cboRedeConsulta.Location = new System.Drawing.Point(225, 19);
            this.cboRedeConsulta.Name = "cboRedeConsulta";
            this.cboRedeConsulta.Size = new System.Drawing.Size(117, 21);
            this.cboRedeConsulta.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cboRedeConsulta, "Rede como TECBAN, SERASA, Check check. \r\nPara apresentar as rede aqui eles devem " +
        "estarem\r\n cadastrar em cartões no SICE.net tipo de transação CHQ.");
            this.cboRedeConsulta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.cboRedeConsulta.Leave += new System.EventHandler(this.cboRedeConsulta_Leave);
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackgroundImage = global::SICEpdv.Properties.Resources.btn_search;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Location = new System.Drawing.Point(336, 61);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(52, 42);
            this.btnProcurar.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnProcurar, "Mostrar todos os clientes");
            this.btnProcurar.UseVisualStyleBackColor = true;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // pnlCheque
            // 
            this.pnlCheque.AutoSize = true;
            this.pnlCheque.Controls.Add(this.txtConta);
            this.pnlCheque.Controls.Add(this.label16);
            this.pnlCheque.Controls.Add(this.chkAltCH);
            this.pnlCheque.Controls.Add(this.cboRedeConsulta);
            this.pnlCheque.Controls.Add(this.label4);
            this.pnlCheque.Controls.Add(this.txtTelefoneCH);
            this.pnlCheque.Controls.Add(this.label14);
            this.pnlCheque.Controls.Add(this.txtCPFCNPJch);
            this.pnlCheque.Controls.Add(this.label13);
            this.pnlCheque.Controls.Add(this.txtNomeCH);
            this.pnlCheque.Controls.Add(this.label12);
            this.pnlCheque.Controls.Add(this.txtNrCheque);
            this.pnlCheque.Controls.Add(this.label11);
            this.pnlCheque.Controls.Add(this.txtAgencia);
            this.pnlCheque.Controls.Add(this.label10);
            this.pnlCheque.Controls.Add(this.txtCodBanco);
            this.pnlCheque.Controls.Add(this.label9);
            this.pnlCheque.Controls.Add(this.panel1);
            this.pnlCheque.Location = new System.Drawing.Point(36, 362);
            this.pnlCheque.Name = "pnlCheque";
            this.pnlCheque.Size = new System.Drawing.Size(353, 167);
            this.pnlCheque.TabIndex = 5;
            this.pnlCheque.Visible = false;
            this.pnlCheque.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCheque_Paint);
            // 
            // txtConta
            // 
            this.txtConta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConta.Location = new System.Drawing.Point(66, 43);
            this.txtConta.MaxLength = 10;
            this.txtConta.Name = "txtConta";
            this.txtConta.Size = new System.Drawing.Size(65, 22);
            this.txtConta.TabIndex = 4;
            this.txtConta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.txtConta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitarNumeros);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(24, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Conta:";
            // 
            // chkAltCH
            // 
            this.chkAltCH.AutoSize = true;
            this.chkAltCH.ForeColor = System.Drawing.Color.White;
            this.chkAltCH.Location = new System.Drawing.Point(262, 92);
            this.chkAltCH.Name = "chkAltCH";
            this.chkAltCH.Size = new System.Drawing.Size(82, 17);
            this.chkAltCH.TabIndex = 17;
            this.chkAltCH.Text = "Alt.Parcelas";
            this.chkAltCH.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(232, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Rede Consulta";
            // 
            // txtTelefoneCH
            // 
            this.txtTelefoneCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefoneCH.Location = new System.Drawing.Point(255, 69);
            this.txtTelefoneCH.Name = "txtTelefoneCH";
            this.txtTelefoneCH.Size = new System.Drawing.Size(87, 21);
            this.txtTelefoneCH.TabIndex = 6;
            this.txtTelefoneCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.txtTelefoneCH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitarNumeros);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(252, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Telefone:";
            // 
            // txtCPFCNPJch
            // 
            this.txtCPFCNPJch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCPFCNPJch.Location = new System.Drawing.Point(95, 92);
            this.txtCPFCNPJch.MaxLength = 14;
            this.txtCPFCNPJch.Name = "txtCPFCNPJch";
            this.txtCPFCNPJch.Size = new System.Drawing.Size(143, 21);
            this.txtCPFCNPJch.TabIndex = 7;
            this.txtCPFCNPJch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.txtCPFCNPJch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitarNumeros);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(11, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "CPF/CNPJ CH.";
            // 
            // txtNomeCH
            // 
            this.txtNomeCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCH.Location = new System.Drawing.Point(66, 69);
            this.txtNomeCH.Name = "txtNomeCH";
            this.txtNomeCH.Size = new System.Drawing.Size(173, 21);
            this.txtNomeCH.TabIndex = 5;
            this.txtNomeCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(6, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Nome CH:";
            // 
            // txtNrCheque
            // 
            this.txtNrCheque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNrCheque.Location = new System.Drawing.Point(137, 19);
            this.txtNrCheque.MaxLength = 10;
            this.txtNrCheque.Name = "txtNrCheque";
            this.txtNrCheque.Size = new System.Drawing.Size(86, 22);
            this.txtNrCheque.TabIndex = 2;
            this.txtNrCheque.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.txtNrCheque.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitarNumeros);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(134, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Cheque Nr.:";
            // 
            // txtAgencia
            // 
            this.txtAgencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAgencia.Location = new System.Drawing.Point(66, 19);
            this.txtAgencia.MaxLength = 8;
            this.txtAgencia.Name = "txtAgencia";
            this.txtAgencia.Size = new System.Drawing.Size(65, 22);
            this.txtAgencia.TabIndex = 1;
            this.txtAgencia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(64, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Agência:";
            // 
            // txtCodBanco
            // 
            this.txtCodBanco.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodBanco.Location = new System.Drawing.Point(8, 19);
            this.txtCodBanco.MaxLength = 10;
            this.txtCodBanco.Name = "txtCodBanco";
            this.txtCodBanco.Size = new System.Drawing.Size(51, 22);
            this.txtCodBanco.TabIndex = 0;
            this.txtCodBanco.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            this.txtCodBanco.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitarNumeros);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(6, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Banco";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtValorIndCH);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.btConfirmarCH);
            this.panel1.Controls.Add(this.txtIntervaloCH);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtParcelamentoCH);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.vencimentoCH);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(3, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 48);
            this.panel1.TabIndex = 6;
            // 
            // txtValorIndCH
            // 
            this.txtValorIndCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorIndCH.Location = new System.Drawing.Point(6, 19);
            this.txtValorIndCH.MaxLength = 7;
            this.txtValorIndCH.Name = "txtValorIndCH";
            this.txtValorIndCH.Size = new System.Drawing.Size(70, 26);
            this.txtValorIndCH.TabIndex = 0;
            this.txtValorIndCH.Text = "0,00";
            this.txtValorIndCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Valor";
            // 
            // btConfirmarCH
            // 
            this.btConfirmarCH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btConfirmarCH.FlatAppearance.BorderSize = 0;
            this.btConfirmarCH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConfirmarCH.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConfirmarCH.ForeColor = System.Drawing.Color.White;
            this.btConfirmarCH.Location = new System.Drawing.Point(270, 3);
            this.btConfirmarCH.Name = "btConfirmarCH";
            this.btConfirmarCH.Size = new System.Drawing.Size(71, 42);
            this.btConfirmarCH.TabIndex = 4;
            this.btConfirmarCH.Text = "&Confirmar";
            this.btConfirmarCH.UseVisualStyleBackColor = false;
            // 
            // txtIntervaloCH
            // 
            this.txtIntervaloCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIntervaloCH.Location = new System.Drawing.Point(232, 19);
            this.txtIntervaloCH.MaxLength = 3;
            this.txtIntervaloCH.Name = "txtIntervaloCH";
            this.txtIntervaloCH.Size = new System.Drawing.Size(28, 26);
            this.txtIntervaloCH.TabIndex = 3;
            this.txtIntervaloCH.Text = "30";
            this.txtIntervaloCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(229, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Int.";
            // 
            // txtParcelamentoCH
            // 
            this.txtParcelamentoCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParcelamentoCH.Location = new System.Drawing.Point(204, 19);
            this.txtParcelamentoCH.MaxLength = 2;
            this.txtParcelamentoCH.Name = "txtParcelamentoCH";
            this.txtParcelamentoCH.Size = new System.Drawing.Size(25, 26);
            this.txtParcelamentoCH.TabIndex = 2;
            this.txtParcelamentoCH.Text = "1";
            this.txtParcelamentoCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(204, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Parc.";
            // 
            // vencimentoCH
            // 
            this.vencimentoCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vencimentoCH.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.vencimentoCH.Location = new System.Drawing.Point(82, 19);
            this.vencimentoCH.Name = "vencimentoCH";
            this.vencimentoCH.Size = new System.Drawing.Size(119, 26);
            this.vencimentoCH.TabIndex = 1;
            this.vencimentoCH.Value = new System.DateTime(2009, 11, 24, 0, 0, 0, 0);
            this.vencimentoCH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProximoControle);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(79, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Vencimento";
            // 
            // btnExtrato
            // 
            this.btnExtrato.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnExtrato.Enabled = false;
            this.btnExtrato.FlatAppearance.BorderSize = 0;
            this.btnExtrato.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtrato.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtrato.ForeColor = System.Drawing.Color.White;
            this.btnExtrato.Location = new System.Drawing.Point(38, 292);
            this.btnExtrato.Name = "btnExtrato";
            this.btnExtrato.Size = new System.Drawing.Size(129, 35);
            this.btnExtrato.TabIndex = 7;
            this.btnExtrato.Text = "EXTRATO/RECEBER";
            this.btnExtrato.UseVisualStyleBackColor = false;
            this.btnExtrato.Click += new System.EventHandler(this.btnExtrato_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(172, 292);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 35);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // chkVerDependentes
            // 
            this.chkVerDependentes.AutoSize = true;
            this.chkVerDependentes.ForeColor = System.Drawing.Color.White;
            this.chkVerDependentes.Location = new System.Drawing.Point(157, 101);
            this.chkVerDependentes.Name = "chkVerDependentes";
            this.chkVerDependentes.Size = new System.Drawing.Size(109, 17);
            this.chkVerDependentes.TabIndex = 9;
            this.chkVerDependentes.Text = "Ver Dependentes";
            this.chkVerDependentes.UseVisualStyleBackColor = true;
            this.chkVerDependentes.CheckedChanged += new System.EventHandler(this.chkVerDependentes_CheckedChanged);
            // 
            // lblSaldo
            // 
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblSaldo.Location = new System.Drawing.Point(3, 44);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(0, 15);
            this.lblSaldo.TabIndex = 4;
            // 
            // lblCidade
            // 
            this.lblCidade.AutoSize = true;
            this.lblCidade.Location = new System.Drawing.Point(203, 22);
            this.lblCidade.Name = "lblCidade";
            this.lblCidade.Size = new System.Drawing.Size(0, 15);
            this.lblCidade.TabIndex = 3;
            // 
            // lblEndereco
            // 
            this.lblEndereco.AutoSize = true;
            this.lblEndereco.Location = new System.Drawing.Point(3, 22);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(0, 15);
            this.lblEndereco.TabIndex = 2;
            // 
            // lblCpfCnpj
            // 
            this.lblCpfCnpj.AutoSize = true;
            this.lblCpfCnpj.Location = new System.Drawing.Point(203, 0);
            this.lblCpfCnpj.Name = "lblCpfCnpj";
            this.lblCpfCnpj.Size = new System.Drawing.Size(0, 15);
            this.lblCpfCnpj.TabIndex = 1;
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.Location = new System.Drawing.Point(3, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(0, 15);
            this.lblNome.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.14286F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714F));
            this.tableLayoutPanel1.Controls.Add(this.lblNome, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCpfCnpj, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblEndereco, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCidade, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSaldo, 0, 2);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(39, 157);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(351, 66);
            this.tableLayoutPanel1.TabIndex = 2;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Controls.Add(this.lblTitulo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(436, 50);
            this.panel2.TabIndex = 11;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitulo.Location = new System.Drawing.Point(98, 13);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(230, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Informações do cliente";
            // 
            // ucClientePdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chkVerDependentes);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnExtrato);
            this.Controls.Add(this.btnProcurar);
            this.Controls.Add(this.pnlCheque);
            this.Controls.Add(this.btSairCR);
            this.Controls.Add(this.layoutConsumidor);
            this.Controls.Add(this.pnlParcelamentoCR);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtIdCliente);
            this.Controls.Add(this.lblIdCliente);
            this.Name = "ucClientePdv";
            this.Size = new System.Drawing.Size(436, 560);
            this.Load += new System.EventHandler(this.ucClientePdv_Load);
            this.pnlParcelamentoCR.ResumeLayout(false);
            this.pnlParcelamentoCR.PerformLayout();
            this.layoutConsumidor.ResumeLayout(false);
            this.layoutConsumidor.PerformLayout();
            this.pnlCheque.ResumeLayout(false);
            this.pnlCheque.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIdCliente;
        public System.Windows.Forms.TextBox txtIdCliente;
        private System.Windows.Forms.Label lblVencimento;
        private System.Windows.Forms.Label txtParcelas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtConsumidor;
        public System.Windows.Forms.TextBox txtEndConsumidor;
        public System.Windows.Forms.TableLayoutPanel layoutConsumidor;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Panel pnlParcelamentoCR;
        public System.Windows.Forms.DateTimePicker vencimentoCR;
        public System.Windows.Forms.TextBox txtParcelamentoCR;
        public System.Windows.Forms.TextBox txtIntervaloCR;
        private System.Windows.Forms.Button btSairCR;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btConfirmarCR;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btConfirmarCH;
        public System.Windows.Forms.TextBox txtIntervaloCH;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtParcelamentoCH;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.DateTimePicker vencimentoCH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.Panel pnlCheque;
        public System.Windows.Forms.TextBox txtCodBanco;
        public System.Windows.Forms.TextBox txtValorIndCH;
        public System.Windows.Forms.TextBox txtNrCheque;
        public System.Windows.Forms.TextBox txtAgencia;
        public System.Windows.Forms.TextBox txtCPFCNPJch;
        public System.Windows.Forms.TextBox txtNomeCH;
        public System.Windows.Forms.TextBox txtTelefoneCH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboRedeConsulta;
        private System.Windows.Forms.Button btnProcurar;
        public System.Windows.Forms.CheckBox chkAltCR;
        public System.Windows.Forms.CheckBox chkAltCH;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDiasVenc;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.CheckBox chkVerDependentes;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtConta;
        public System.Windows.Forms.Button btnExtrato;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblCidade;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.Label lblCpfCnpj;
        private System.Windows.Forms.Label lblNome;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTitulo;
    }
}
