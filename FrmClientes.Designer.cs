namespace SICEpdv
{
    partial class FrmClientes
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClientes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSair = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPesquisaAut = new System.Windows.Forms.CheckBox();
            this.btnVer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSaldoCH = new System.Windows.Forms.Label();
            this.lblDebitoCH = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblDebito = new System.Windows.Forms.Label();
            this.lblCredito = new System.Windows.Forms.Label();
            this.dtgClientes = new System.Windows.Forms.DataGridView();
            this.clientesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.rdbApelido = new System.Windows.Forms.RadioButton();
            this.rdbCodigo = new System.Windows.Forms.RadioButton();
            this.txtProcura = new System.Windows.Forms.TextBox();
            this.rdbCPF = new System.Windows.Forms.RadioButton();
            this.rdbNome = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.cloud = new System.Windows.Forms.DataGridViewImageColumn();
            this.iqcard = new System.Windows.Forms.DataGridViewImageColumn();
            this.codigoFilialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apelidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.situacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.observacaoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cidadeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endereco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saldo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debitoCH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saldoCH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgClientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientesBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.btnSair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(727, 533);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(120, 42);
            this.btnSair.TabIndex = 2;
            this.btnSair.Text = "CANCELAR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(34, 588);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(28, 24);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Procurar por:";
            this.groupBox1.Visible = false;
            // 
            // chkPesquisaAut
            // 
            this.chkPesquisaAut.AutoSize = true;
            this.chkPesquisaAut.BackColor = System.Drawing.Color.Transparent;
            this.chkPesquisaAut.ForeColor = System.Drawing.Color.White;
            this.chkPesquisaAut.Location = new System.Drawing.Point(12, 509);
            this.chkPesquisaAut.Name = "chkPesquisaAut";
            this.chkPesquisaAut.Size = new System.Drawing.Size(118, 17);
            this.chkPesquisaAut.TabIndex = 5;
            this.chkPesquisaAut.Text = "Procura automática";
            this.chkPesquisaAut.UseVisualStyleBackColor = false;
            // 
            // btnVer
            // 
            this.btnVer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(191)))), ((int)(((byte)(223)))));
            this.btnVer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVer.FlatAppearance.BorderSize = 0;
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnVer.ForeColor = System.Drawing.Color.White;
            this.btnVer.Location = new System.Drawing.Point(600, 533);
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(120, 42);
            this.btnVer.TabIndex = 3;
            this.btnVer.Text = "VER FOTO";
            this.btnVer.UseVisualStyleBackColor = false;
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(751, 500);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "F7 - Menu Fiscal";
            this.label1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(68, 590);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(27, 26);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dados Financeiros";
            this.groupBox2.Visible = false;
            // 
            // lblSaldoCH
            // 
            this.lblSaldoCH.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldoCH.Location = new System.Drawing.Point(690, 29);
            this.lblSaldoCH.Name = "lblSaldoCH";
            this.lblSaldoCH.Size = new System.Drawing.Size(127, 37);
            this.lblSaldoCH.TabIndex = 4;
            this.lblSaldoCH.Text = "0.00";
            this.lblSaldoCH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDebitoCH
            // 
            this.lblDebitoCH.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebitoCH.Location = new System.Drawing.Point(485, 29);
            this.lblDebitoCH.Name = "lblDebitoCH";
            this.lblDebitoCH.Size = new System.Drawing.Size(133, 37);
            this.lblDebitoCH.TabIndex = 3;
            this.lblDebitoCH.Text = "0.00";
            this.lblDebitoCH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSaldo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.ForeColor = System.Drawing.Color.Blue;
            this.lblSaldo.Location = new System.Drawing.Point(317, 29);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(111, 37);
            this.lblSaldo.TabIndex = 2;
            this.lblSaldo.Text = "0.00";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDebito
            // 
            this.lblDebito.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDebito.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebito.Location = new System.Drawing.Point(13, 29);
            this.lblDebito.Name = "lblDebito";
            this.lblDebito.Size = new System.Drawing.Size(110, 37);
            this.lblDebito.TabIndex = 1;
            this.lblDebito.Text = "0.00";
            this.lblDebito.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCredito
            // 
            this.lblCredito.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCredito.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredito.Location = new System.Drawing.Point(170, 29);
            this.lblCredito.Name = "lblCredito";
            this.lblCredito.Size = new System.Drawing.Size(108, 37);
            this.lblCredito.TabIndex = 0;
            this.lblCredito.Text = "0.00";
            this.lblCredito.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtgClientes
            // 
            this.dtgClientes.AutoGenerateColumns = false;
            this.dtgClientes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dtgClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtgClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cloud,
            this.iqcard,
            this.codigoFilialDataGridViewTextBoxColumn,
            this.codigo,
            this.nome,
            this.apelidoDataGridViewTextBoxColumn,
            this.situacaoDataGridViewTextBoxColumn,
            this.observacaoDataGridViewTextBoxColumn,
            this.cidadeDataGridViewTextBoxColumn,
            this.endereco,
            this.debito,
            this.credito,
            this.saldo,
            this.debitoCH,
            this.saldoCH});
            this.dtgClientes.DataSource = this.clientesBindingSource;
            this.dtgClientes.GridColor = System.Drawing.Color.Gainsboro;
            this.dtgClientes.Location = new System.Drawing.Point(12, 12);
            this.dtgClientes.MultiSelect = false;
            this.dtgClientes.Name = "dtgClientes";
            this.dtgClientes.ReadOnly = true;
            this.dtgClientes.RowHeadersVisible = false;
            this.dtgClientes.RowTemplate.Height = 35;
            this.dtgClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgClientes.Size = new System.Drawing.Size(835, 361);
            this.dtgClientes.TabIndex = 1;
            this.dtgClientes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgClientes_CellContentClick);
            // 
            // clientesBindingSource
            // 
            this.clientesBindingSource.DataSource = typeof(SICEpdv.clientes);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblDebito);
            this.panel1.Controls.Add(this.lblDebitoCH);
            this.panel1.Controls.Add(this.lblCredito);
            this.panel1.Controls.Add(this.lblSaldoCH);
            this.panel1.Controls.Add(this.lblSaldo);
            this.panel1.Location = new System.Drawing.Point(12, 396);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 82);
            this.panel1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(47, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "DÉBITO R$";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(193, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "CRÉDITO R$";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(375, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "SALDO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(486, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "DÉBITO CHEQUE R$";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(687, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "SALDO CHEQUE R$";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Olive;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 588);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(16, 28);
            this.button1.TabIndex = 13;
            this.button1.Text = "Tecl. Virtual";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            // 
            // rdbApelido
            // 
            this.rdbApelido.AutoSize = true;
            this.rdbApelido.BackColor = System.Drawing.Color.Transparent;
            this.rdbApelido.ForeColor = System.Drawing.Color.White;
            this.rdbApelido.Location = new System.Drawing.Point(379, 583);
            this.rdbApelido.Name = "rdbApelido";
            this.rdbApelido.Size = new System.Drawing.Size(60, 17);
            this.rdbApelido.TabIndex = 12;
            this.rdbApelido.Text = "Apelido";
            this.rdbApelido.UseVisualStyleBackColor = false;
            // 
            // rdbCodigo
            // 
            this.rdbCodigo.AutoSize = true;
            this.rdbCodigo.BackColor = System.Drawing.Color.Transparent;
            this.rdbCodigo.ForeColor = System.Drawing.Color.White;
            this.rdbCodigo.Location = new System.Drawing.Point(238, 583);
            this.rdbCodigo.Name = "rdbCodigo";
            this.rdbCodigo.Size = new System.Drawing.Size(58, 17);
            this.rdbCodigo.TabIndex = 11;
            this.rdbCodigo.Text = "Código";
            this.rdbCodigo.UseVisualStyleBackColor = false;
            // 
            // txtProcura
            // 
            this.txtProcura.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProcura.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcura.Location = new System.Drawing.Point(253, 542);
            this.txtProcura.Name = "txtProcura";
            this.txtProcura.Size = new System.Drawing.Size(273, 26);
            this.txtProcura.TabIndex = 7;
            // 
            // rdbCPF
            // 
            this.rdbCPF.AutoSize = true;
            this.rdbCPF.BackColor = System.Drawing.Color.Transparent;
            this.rdbCPF.ForeColor = System.Drawing.Color.White;
            this.rdbCPF.Location = new System.Drawing.Point(457, 583);
            this.rdbCPF.Name = "rdbCPF";
            this.rdbCPF.Size = new System.Drawing.Size(77, 17);
            this.rdbCPF.TabIndex = 10;
            this.rdbCPF.Text = "CPF/CNPJ";
            this.rdbCPF.UseVisualStyleBackColor = false;
            // 
            // rdbNome
            // 
            this.rdbNome.AutoSize = true;
            this.rdbNome.BackColor = System.Drawing.Color.Transparent;
            this.rdbNome.Checked = true;
            this.rdbNome.ForeColor = System.Drawing.Color.White;
            this.rdbNome.Location = new System.Drawing.Point(311, 583);
            this.rdbNome.Name = "rdbNome";
            this.rdbNome.Size = new System.Drawing.Size(53, 17);
            this.rdbNome.TabIndex = 8;
            this.rdbNome.TabStop = true;
            this.rdbNome.Text = "Nome";
            this.rdbNome.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(470, 533);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(61, 42);
            this.panel2.TabIndex = 15;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.BackgroundImage = global::SICEpdv.Properties.Resources.menu_top_user;
            this.pictureBox2.Location = new System.Drawing.Point(188, 531);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 46);
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackgroundImage = global::SICEpdv.Properties.Resources.btn_search;
            this.btnProcurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Location = new System.Drawing.Point(543, 533);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(52, 42);
            this.btnProcurar.TabIndex = 9;
            this.btnProcurar.UseVisualStyleBackColor = true;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SICEpdv.Properties.Resources.input_search_products;
            this.pictureBox1.Location = new System.Drawing.Point(186, 531);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(351, 46);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "Codigo";
            this.dataGridViewImageColumn1.HeaderText = "Cloud";
            this.dataGridViewImageColumn1.Image = global::SICEpdv.Properties.Resources.azurecloud;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // cloud
            // 
            this.cloud.DataPropertyName = "cloud";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            this.cloud.DefaultCellStyle = dataGridViewCellStyle1;
            this.cloud.HeaderText = "Cloud";
            this.cloud.Image = global::SICEpdv.Properties.Resources.azurecloud;
            this.cloud.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.cloud.Name = "cloud";
            this.cloud.ReadOnly = true;
            this.cloud.Visible = false;
            this.cloud.Width = 35;
            // 
            // iqcard
            // 
            this.iqcard.HeaderText = "Card";
            this.iqcard.Image = global::SICEpdv.Properties.Resources.favorito;
            this.iqcard.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.iqcard.Name = "iqcard";
            this.iqcard.ReadOnly = true;
            this.iqcard.Width = 35;
            // 
            // codigoFilialDataGridViewTextBoxColumn
            // 
            this.codigoFilialDataGridViewTextBoxColumn.DataPropertyName = "CodigoFilial";
            this.codigoFilialDataGridViewTextBoxColumn.HeaderText = "Filial";
            this.codigoFilialDataGridViewTextBoxColumn.Name = "codigoFilialDataGridViewTextBoxColumn";
            this.codigoFilialDataGridViewTextBoxColumn.ReadOnly = true;
            this.codigoFilialDataGridViewTextBoxColumn.Width = 40;
            // 
            // codigo
            // 
            this.codigo.DataPropertyName = "Codigo";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codigo.DefaultCellStyle = dataGridViewCellStyle2;
            this.codigo.HeaderText = "Cod.";
            this.codigo.Name = "codigo";
            this.codigo.ReadOnly = true;
            this.codigo.Width = 60;
            // 
            // nome
            // 
            this.nome.DataPropertyName = "Nome";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nome.DefaultCellStyle = dataGridViewCellStyle3;
            this.nome.HeaderText = "Nome";
            this.nome.Name = "nome";
            this.nome.ReadOnly = true;
            this.nome.Width = 250;
            // 
            // apelidoDataGridViewTextBoxColumn
            // 
            this.apelidoDataGridViewTextBoxColumn.DataPropertyName = "apelido";
            this.apelidoDataGridViewTextBoxColumn.HeaderText = "Apelido";
            this.apelidoDataGridViewTextBoxColumn.Name = "apelidoDataGridViewTextBoxColumn";
            this.apelidoDataGridViewTextBoxColumn.ReadOnly = true;
            this.apelidoDataGridViewTextBoxColumn.Width = 150;
            // 
            // situacaoDataGridViewTextBoxColumn
            // 
            this.situacaoDataGridViewTextBoxColumn.DataPropertyName = "situacao";
            this.situacaoDataGridViewTextBoxColumn.HeaderText = "Situação";
            this.situacaoDataGridViewTextBoxColumn.Name = "situacaoDataGridViewTextBoxColumn";
            this.situacaoDataGridViewTextBoxColumn.ReadOnly = true;
            this.situacaoDataGridViewTextBoxColumn.Width = 70;
            // 
            // observacaoDataGridViewTextBoxColumn
            // 
            this.observacaoDataGridViewTextBoxColumn.DataPropertyName = "observacao";
            this.observacaoDataGridViewTextBoxColumn.HeaderText = "Obs.:";
            this.observacaoDataGridViewTextBoxColumn.Name = "observacaoDataGridViewTextBoxColumn";
            this.observacaoDataGridViewTextBoxColumn.ReadOnly = true;
            this.observacaoDataGridViewTextBoxColumn.Width = 140;
            // 
            // cidadeDataGridViewTextBoxColumn
            // 
            this.cidadeDataGridViewTextBoxColumn.DataPropertyName = "cidade";
            this.cidadeDataGridViewTextBoxColumn.HeaderText = "Cidade";
            this.cidadeDataGridViewTextBoxColumn.Name = "cidadeDataGridViewTextBoxColumn";
            this.cidadeDataGridViewTextBoxColumn.ReadOnly = true;
            this.cidadeDataGridViewTextBoxColumn.Width = 120;
            // 
            // endereco
            // 
            this.endereco.DataPropertyName = "endereco";
            this.endereco.HeaderText = "Endereço";
            this.endereco.Name = "endereco";
            this.endereco.ReadOnly = true;
            this.endereco.Width = 300;
            // 
            // debito
            // 
            this.debito.DataPropertyName = "debito";
            this.debito.HeaderText = "Débito";
            this.debito.Name = "debito";
            this.debito.ReadOnly = true;
            this.debito.Visible = false;
            // 
            // credito
            // 
            this.credito.DataPropertyName = "credito";
            this.credito.HeaderText = "Crédito";
            this.credito.Name = "credito";
            this.credito.ReadOnly = true;
            this.credito.Visible = false;
            // 
            // saldo
            // 
            this.saldo.DataPropertyName = "saldo";
            this.saldo.HeaderText = "Saldo";
            this.saldo.Name = "saldo";
            this.saldo.ReadOnly = true;
            this.saldo.Visible = false;
            // 
            // debitoCH
            // 
            this.debitoCH.DataPropertyName = "debitoch";
            this.debitoCH.HeaderText = "Débito CH";
            this.debitoCH.Name = "debitoCH";
            this.debitoCH.ReadOnly = true;
            this.debitoCH.Visible = false;
            // 
            // saldoCH
            // 
            this.saldoCH.DataPropertyName = "saldoch";
            this.saldoCH.HeaderText = "Saldo c/ Cheque";
            this.saldoCH.Name = "saldoCH";
            this.saldoCH.ReadOnly = true;
            this.saldoCH.Visible = false;
            // 
            // FrmClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(859, 638);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtProcura);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnProcurar);
            this.Controls.Add(this.rdbApelido);
            this.Controls.Add(this.rdbCodigo);
            this.Controls.Add(this.rdbCPF);
            this.Controls.Add(this.rdbNome);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkPesquisaAut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVer);
            this.Controls.Add(this.dtgClientes);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "FrmClientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clientes";
            this.Load += new System.EventHandler(this.FrmClientes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgClientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientesBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.BindingSource clientesBindingSource;
        private System.Windows.Forms.Button btnVer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblDebito;
        private System.Windows.Forms.Label lblCredito;
        private System.Windows.Forms.Label lblSaldoCH;
        private System.Windows.Forms.Label lblDebitoCH;
        private System.Windows.Forms.DataGridView dtgClientes;
        private System.Windows.Forms.CheckBox chkPesquisaAut;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.RadioButton rdbApelido;
        private System.Windows.Forms.RadioButton rdbCodigo;
        private System.Windows.Forms.TextBox txtProcura;
        private System.Windows.Forms.RadioButton rdbCPF;
        private System.Windows.Forms.RadioButton rdbNome;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridViewImageColumn cloud;
        private System.Windows.Forms.DataGridViewImageColumn iqcard;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoFilialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nome;
        private System.Windows.Forms.DataGridViewTextBoxColumn apelidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn situacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn observacaoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cidadeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endereco;
        private System.Windows.Forms.DataGridViewTextBoxColumn debito;
        private System.Windows.Forms.DataGridViewTextBoxColumn credito;
        private System.Windows.Forms.DataGridViewTextBoxColumn saldo;
        private System.Windows.Forms.DataGridViewTextBoxColumn debitoCH;
        private System.Windows.Forms.DataGridViewTextBoxColumn saldoCH;
    }
}