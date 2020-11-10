namespace SICEpdv
{
    partial class FrmProdutos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProdutos));
            this.btnTeclado = new System.Windows.Forms.Button();
            this.lblPrecoMinimo = new System.Windows.Forms.Label();
            this.lblProduto = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();
            this.txtCodPrd = new System.Windows.Forms.TextBox();
            this.rdbDescricao = new System.Windows.Forms.RadioButton();
            this.rdbBarras = new System.Windows.Forms.RadioButton();
            this.rdbCodigo = new System.Windows.Forms.RadioButton();
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.chkTipo = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnVerComposicao = new System.Windows.Forms.Button();
            this.chkProcuraAut = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTabelaAtacado = new System.Windows.Forms.Button();
            this.chkTipoPesquisa = new System.Windows.Forms.CheckBox();
            this.btnTabelaVarejo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.dtgProdutos = new System.Windows.Forms.DataGridView();
            this.anuncio = new System.Windows.Forms.DataGridViewImageColumn();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STecf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filiais = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IPPT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisponivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtdPrateleiras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeposito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precoMinimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigobarras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.localestoque = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLocalEstoque = new System.Windows.Forms.Label();
            this.btnContador = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnUpload = new System.Windows.Forms.PictureBox();
            this.fotoPrd = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnProcurar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgProdutos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTeclado
            // 
            this.btnTeclado.BackColor = System.Drawing.Color.Transparent;
            this.btnTeclado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTeclado.FlatAppearance.BorderSize = 0;
            this.btnTeclado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeclado.ForeColor = System.Drawing.Color.White;
            this.btnTeclado.Location = new System.Drawing.Point(568, 381);
            this.btnTeclado.Name = "btnTeclado";
            this.btnTeclado.Size = new System.Drawing.Size(54, 41);
            this.btnTeclado.TabIndex = 20;
            this.btnTeclado.UseVisualStyleBackColor = false;
            this.btnTeclado.Click += new System.EventHandler(this.btnTeclado_Click);
            // 
            // lblPrecoMinimo
            // 
            this.lblPrecoMinimo.AutoSize = true;
            this.lblPrecoMinimo.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecoMinimo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblPrecoMinimo.Location = new System.Drawing.Point(575, 551);
            this.lblPrecoMinimo.Name = "lblPrecoMinimo";
            this.lblPrecoMinimo.Size = new System.Drawing.Size(92, 25);
            this.lblPrecoMinimo.TabIndex = 24;
            this.lblPrecoMinimo.Text = "R$ 0,00";
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduto.ForeColor = System.Drawing.Color.White;
            this.lblProduto.Location = new System.Drawing.Point(263, 477);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(0, 29);
            this.lblProduto.TabIndex = 23;
            // 
            // lblPreco
            // 
            this.lblPreco.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreco.ForeColor = System.Drawing.Color.White;
            this.lblPreco.Location = new System.Drawing.Point(275, 517);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(294, 59);
            this.lblPreco.TabIndex = 22;
            this.lblPreco.Text = "R$ 0,00";
            this.lblPreco.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCodPrd
            // 
            this.txtCodPrd.BackColor = System.Drawing.Color.White;
            this.txtCodPrd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodPrd.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodPrd.Location = new System.Drawing.Point(338, 389);
            this.txtCodPrd.Name = "txtCodPrd";
            this.txtCodPrd.Size = new System.Drawing.Size(231, 26);
            this.txtCodPrd.TabIndex = 15;
            // 
            // rdbDescricao
            // 
            this.rdbDescricao.AutoSize = true;
            this.rdbDescricao.Checked = true;
            this.rdbDescricao.ForeColor = System.Drawing.Color.White;
            this.rdbDescricao.Location = new System.Drawing.Point(446, 435);
            this.rdbDescricao.Name = "rdbDescricao";
            this.rdbDescricao.Size = new System.Drawing.Size(73, 17);
            this.rdbDescricao.TabIndex = 19;
            this.rdbDescricao.TabStop = true;
            this.rdbDescricao.Text = "Descrição";
            this.rdbDescricao.UseVisualStyleBackColor = true;
            // 
            // rdbBarras
            // 
            this.rdbBarras.AutoSize = true;
            this.rdbBarras.ForeColor = System.Drawing.Color.White;
            this.rdbBarras.Location = new System.Drawing.Point(350, 435);
            this.rdbBarras.Name = "rdbBarras";
            this.rdbBarras.Size = new System.Drawing.Size(90, 17);
            this.rdbBarras.TabIndex = 18;
            this.rdbBarras.Text = "Código barras";
            this.rdbBarras.UseVisualStyleBackColor = true;
            // 
            // rdbCodigo
            // 
            this.rdbCodigo.AutoSize = true;
            this.rdbCodigo.ForeColor = System.Drawing.Color.White;
            this.rdbCodigo.Location = new System.Drawing.Point(286, 435);
            this.rdbCodigo.Name = "rdbCodigo";
            this.rdbCodigo.Size = new System.Drawing.Size(58, 17);
            this.rdbCodigo.TabIndex = 16;
            this.rdbCodigo.Text = "Código";
            this.rdbCodigo.UseVisualStyleBackColor = true;
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.Location = new System.Drawing.Point(7, 608);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(238, 230);
            this.pnlTeclado.TabIndex = 2;
            this.pnlTeclado.Visible = false;
            // 
            // chkTipo
            // 
            this.chkTipo.AutoSize = true;
            this.chkTipo.Checked = true;
            this.chkTipo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTipo.ForeColor = System.Drawing.Color.White;
            this.chkTipo.Location = new System.Drawing.Point(12, 403);
            this.chkTipo.Name = "chkTipo";
            this.chkTipo.Size = new System.Drawing.Size(125, 17);
            this.chkTipo.TabIndex = 12;
            this.chkTipo.Text = "Apenas: 0 - Produtos";
            this.chkTipo.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(835, 557);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Mostrando";
            this.label2.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.SteelBlue;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(699, 429);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 43);
            this.button2.TabIndex = 12;
            this.button2.Text = "(F2) VER POSIÇÕES NAS FILIAIS";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnVerComposicao
            // 
            this.btnVerComposicao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnVerComposicao.FlatAppearance.BorderSize = 0;
            this.btnVerComposicao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerComposicao.Location = new System.Drawing.Point(783, 483);
            this.btnVerComposicao.Name = "btnVerComposicao";
            this.btnVerComposicao.Size = new System.Drawing.Size(112, 48);
            this.btnVerComposicao.TabIndex = 6;
            this.btnVerComposicao.Text = "Mostrar índices\r\ndo produto";
            this.btnVerComposicao.UseVisualStyleBackColor = false;
            this.btnVerComposicao.Visible = false;
            this.btnVerComposicao.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkProcuraAut
            // 
            this.chkProcuraAut.AutoSize = true;
            this.chkProcuraAut.ForeColor = System.Drawing.Color.White;
            this.chkProcuraAut.Location = new System.Drawing.Point(12, 380);
            this.chkProcuraAut.Name = "chkProcuraAut";
            this.chkProcuraAut.Size = new System.Drawing.Size(118, 17);
            this.chkProcuraAut.TabIndex = 11;
            this.chkProcuraAut.Text = "Procura automática";
            this.chkProcuraAut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(799, 535);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "F7 Menu Fiscal";
            this.label1.Visible = false;
            // 
            // btnTabelaAtacado
            // 
            this.btnTabelaAtacado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.btnTabelaAtacado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTabelaAtacado.FlatAppearance.BorderSize = 0;
            this.btnTabelaAtacado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabelaAtacado.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnTabelaAtacado.ForeColor = System.Drawing.Color.White;
            this.btnTabelaAtacado.Location = new System.Drawing.Point(797, 381);
            this.btnTabelaAtacado.Name = "btnTabelaAtacado";
            this.btnTabelaAtacado.Size = new System.Drawing.Size(93, 42);
            this.btnTabelaAtacado.TabIndex = 9;
            this.btnTabelaAtacado.Text = "PREÇO ATACADO";
            this.btnTabelaAtacado.UseVisualStyleBackColor = false;
            this.btnTabelaAtacado.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkTipoPesquisa
            // 
            this.chkTipoPesquisa.AutoSize = true;
            this.chkTipoPesquisa.ForeColor = System.Drawing.Color.White;
            this.chkTipoPesquisa.Location = new System.Drawing.Point(12, 357);
            this.chkTipoPesquisa.Name = "chkTipoPesquisa";
            this.chkTipoPesquisa.Size = new System.Drawing.Size(141, 17);
            this.chkTipoPesquisa.TabIndex = 10;
            this.chkTipoPesquisa.Text = "&Palavra contida no texto";
            this.chkTipoPesquisa.UseVisualStyleBackColor = true;
            this.chkTipoPesquisa.Click += new System.EventHandler(this.chkTipoPesquisa_Click);
            // 
            // btnTabelaVarejo
            // 
            this.btnTabelaVarejo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(93)))));
            this.btnTabelaVarejo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTabelaVarejo.FlatAppearance.BorderSize = 0;
            this.btnTabelaVarejo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabelaVarejo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabelaVarejo.ForeColor = System.Drawing.Color.White;
            this.btnTabelaVarejo.Location = new System.Drawing.Point(698, 381);
            this.btnTabelaVarejo.Name = "btnTabelaVarejo";
            this.btnTabelaVarejo.Size = new System.Drawing.Size(93, 42);
            this.btnTabelaVarejo.TabIndex = 8;
            this.btnTabelaVarejo.Text = "PREÇO VAREJO";
            this.btnTabelaVarejo.UseVisualStyleBackColor = false;
            this.btnTabelaVarejo.Click += new System.EventHandler(this.btnTabelaVarejo_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(773, 538);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 48);
            this.button1.TabIndex = 7;
            this.button1.Text = "Produtos com índices\r\ntécnico de produção";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            // 
            // btnSair
            // 
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Location = new System.Drawing.Point(802, 579);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(93, 42);
            this.btnSair.TabIndex = 5;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Visible = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // dtgProdutos
            // 
            this.dtgProdutos.BackgroundColor = System.Drawing.Color.White;
            this.dtgProdutos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtgProdutos.ColumnHeadersHeight = 20;
            this.dtgProdutos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgProdutos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.anuncio,
            this.codigo,
            this.descricao,
            this.unidade,
            this.STecf,
            this.quantidade,
            this.preco,
            this.filiais,
            this.IAT,
            this.IPPT,
            this.colDisponivel,
            this.qtdPrateleiras,
            this.colDeposito,
            this.precoMinimo,
            this.codigobarras,
            this.localestoque});
            this.dtgProdutos.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dtgProdutos.Location = new System.Drawing.Point(7, 10);
            this.dtgProdutos.MultiSelect = false;
            this.dtgProdutos.Name = "dtgProdutos";
            this.dtgProdutos.ReadOnly = true;
            this.dtgProdutos.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dtgProdutos.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgProdutos.RowTemplate.Height = 30;
            this.dtgProdutos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgProdutos.Size = new System.Drawing.Size(892, 336);
            this.dtgProdutos.TabIndex = 0;
            this.dtgProdutos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgProdutos_CellContentClick);
            // 
            // anuncio
            // 
            this.anuncio.DataPropertyName = "anuncio";
            this.anuncio.HeaderText = "Anunciar";
            this.anuncio.Image = global::SICEpdv.Properties.Resources.compartilharAzul;
            this.anuncio.Name = "anuncio";
            this.anuncio.ReadOnly = true;
            this.anuncio.Width = 60;
            // 
            // codigo
            // 
            this.codigo.DataPropertyName = "codigo";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codigo.DefaultCellStyle = dataGridViewCellStyle1;
            this.codigo.HeaderText = "Cód";
            this.codigo.Name = "codigo";
            this.codigo.ReadOnly = true;
            this.codigo.Width = 130;
            // 
            // descricao
            // 
            this.descricao.DataPropertyName = "descricao";
            this.descricao.HeaderText = "Descrição produto";
            this.descricao.Name = "descricao";
            this.descricao.ReadOnly = true;
            this.descricao.Width = 420;
            // 
            // unidade
            // 
            this.unidade.DataPropertyName = "unidade";
            this.unidade.HeaderText = "Und";
            this.unidade.Name = "unidade";
            this.unidade.ReadOnly = true;
            this.unidade.Width = 40;
            // 
            // STecf
            // 
            this.STecf.DataPropertyName = "STecf";
            this.STecf.HeaderText = "ST";
            this.STecf.Name = "STecf";
            this.STecf.ReadOnly = true;
            this.STecf.Width = 40;
            // 
            // quantidade
            // 
            this.quantidade.DataPropertyName = "quantidade";
            this.quantidade.HeaderText = "Qtd";
            this.quantidade.Name = "quantidade";
            this.quantidade.ReadOnly = true;
            this.quantidade.Width = 70;
            // 
            // preco
            // 
            this.preco.DataPropertyName = "preco";
            this.preco.HeaderText = "Preço";
            this.preco.Name = "preco";
            this.preco.ReadOnly = true;
            this.preco.Width = 80;
            // 
            // filiais
            // 
            this.filiais.DataPropertyName = "filiais";
            this.filiais.HeaderText = "QTD F.";
            this.filiais.Name = "filiais";
            this.filiais.ReadOnly = true;
            this.filiais.Width = 60;
            // 
            // IAT
            // 
            this.IAT.DataPropertyName = "IAT";
            this.IAT.HeaderText = "IAT";
            this.IAT.Name = "IAT";
            this.IAT.ReadOnly = true;
            this.IAT.Width = 25;
            // 
            // IPPT
            // 
            this.IPPT.DataPropertyName = "IPPT";
            this.IPPT.HeaderText = "IPPT";
            this.IPPT.Name = "IPPT";
            this.IPPT.ReadOnly = true;
            this.IPPT.Width = 40;
            // 
            // colDisponivel
            // 
            this.colDisponivel.DataPropertyName = "qtdDisponivel";
            this.colDisponivel.HeaderText = "Qtd.Disponível";
            this.colDisponivel.Name = "colDisponivel";
            this.colDisponivel.ReadOnly = true;
            // 
            // qtdPrateleiras
            // 
            this.qtdPrateleiras.DataPropertyName = "qtdPrateleiras";
            this.qtdPrateleiras.HeaderText = "Qtd.Prat";
            this.qtdPrateleiras.Name = "qtdPrateleiras";
            this.qtdPrateleiras.ReadOnly = true;
            // 
            // colDeposito
            // 
            this.colDeposito.DataPropertyName = "deposito";
            this.colDeposito.HeaderText = "Déposito";
            this.colDeposito.Name = "colDeposito";
            this.colDeposito.ReadOnly = true;
            // 
            // precoMinimo
            // 
            this.precoMinimo.DataPropertyName = "precoMinimo";
            this.precoMinimo.HeaderText = "Preco Min.";
            this.precoMinimo.Name = "precoMinimo";
            this.precoMinimo.ReadOnly = true;
            // 
            // codigobarras
            // 
            this.codigobarras.DataPropertyName = "codigobarras";
            this.codigobarras.HeaderText = "Cod.Bar";
            this.codigobarras.Name = "codigobarras";
            this.codigobarras.ReadOnly = true;
            // 
            // localestoque
            // 
            this.localestoque.DataPropertyName = "localestoque";
            this.localestoque.HeaderText = "localestoque";
            this.localestoque.Name = "localestoque";
            this.localestoque.ReadOnly = true;
            // 
            // lblLocalEstoque
            // 
            this.lblLocalEstoque.AutoSize = true;
            this.lblLocalEstoque.ForeColor = System.Drawing.Color.Yellow;
            this.lblLocalEstoque.Location = new System.Drawing.Point(282, 601);
            this.lblLocalEstoque.Name = "lblLocalEstoque";
            this.lblLocalEstoque.Size = new System.Drawing.Size(35, 13);
            this.lblLocalEstoque.TabIndex = 26;
            this.lblLocalEstoque.Text = "(A-12)";
            // 
            // btnContador
            // 
            this.btnContador.BackColor = System.Drawing.Color.SteelBlue;
            this.btnContador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContador.FlatAppearance.BorderSize = 0;
            this.btnContador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContador.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnContador.ForeColor = System.Drawing.Color.White;
            this.btnContador.Location = new System.Drawing.Point(9, 559);
            this.btnContador.Name = "btnContador";
            this.btnContador.Size = new System.Drawing.Size(191, 43);
            this.btnContador.TabIndex = 27;
            this.btnContador.Text = "Enviar Informações Fiscais dos produtos a Contabilidade";
            this.btnContador.UseVisualStyleBackColor = false;
            this.btnContador.Click += new System.EventHandler(this.btnContador_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "anuncio";
            this.dataGridViewImageColumn1.HeaderText = "Anunciar";
            this.dataGridViewImageColumn1.Image = global::SICEpdv.Properties.Resources.compartilharAzul;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 60;
            // 
            // btnUpload
            // 
            this.btnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpload.Image = global::SICEpdv.Properties.Resources.upload_images;
            this.btnUpload.Location = new System.Drawing.Point(12, 422);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(30, 30);
            this.btnUpload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnUpload.TabIndex = 28;
            this.btnUpload.TabStop = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // fotoPrd
            // 
            this.fotoPrd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fotoPrd.Image = global::SICEpdv.Properties.Resources.upload_images;
            this.fotoPrd.Location = new System.Drawing.Point(14, 441);
            this.fotoPrd.Name = "fotoPrd";
            this.fotoPrd.Size = new System.Drawing.Size(116, 113);
            this.fotoPrd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fotoPrd.TabIndex = 25;
            this.fotoPrd.TabStop = false;
            this.fotoPrd.Visible = false;
            this.fotoPrd.Click += new System.EventHandler(this.fotoPrd_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SICEpdv.Properties.Resources.input_search_products;
            this.pictureBox1.Location = new System.Drawing.Point(285, 381);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 41);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackColor = System.Drawing.SystemColors.Control;
            this.btnProcurar.BackgroundImage = global::SICEpdv.Properties.Resources.btn_search;
            this.btnProcurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Location = new System.Drawing.Point(632, 380);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(52, 42);
            this.btnProcurar.TabIndex = 17;
            this.btnProcurar.UseVisualStyleBackColor = false;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // FrmProdutos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(907, 623);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnContador);
            this.Controls.Add(this.lblLocalEstoque);
            this.Controls.Add(this.fotoPrd);
            this.Controls.Add(this.btnTeclado);
            this.Controls.Add(this.lblPrecoMinimo);
            this.Controls.Add(this.lblProduto);
            this.Controls.Add(this.lblPreco);
            this.Controls.Add(this.txtCodPrd);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnProcurar);
            this.Controls.Add(this.rdbDescricao);
            this.Controls.Add(this.rdbBarras);
            this.Controls.Add(this.rdbCodigo);
            this.Controls.Add(this.pnlTeclado);
            this.Controls.Add(this.chkTipo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnVerComposicao);
            this.Controls.Add(this.chkProcuraAut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTabelaAtacado);
            this.Controls.Add(this.chkTipoPesquisa);
            this.Controls.Add(this.btnTabelaVarejo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.dtgProdutos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmProdutos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consultar Produtos";
            this.Load += new System.EventHandler(this.FrmProdutos_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmProdutos_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dtgProdutos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgProdutos;
        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnVerComposicao;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTabelaVarejo;
        private System.Windows.Forms.Button btnTabelaAtacado;
        private System.Windows.Forms.CheckBox chkTipoPesquisa;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkProcuraAut;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkTipo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCodPrd;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnTeclado;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.RadioButton rdbDescricao;
        public System.Windows.Forms.RadioButton rdbBarras;
        private System.Windows.Forms.RadioButton rdbCodigo;
        private System.Windows.Forms.Label lblPrecoMinimo;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.Label lblPreco;
        private System.Windows.Forms.PictureBox fotoPrd;
        private System.Windows.Forms.Label lblLocalEstoque;
        private System.Windows.Forms.Button btnContador;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn anuncio;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn unidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn STecf;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn preco;
        private System.Windows.Forms.DataGridViewTextBoxColumn filiais;
        private System.Windows.Forms.DataGridViewTextBoxColumn IAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPPT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisponivel;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtdPrateleiras;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeposito;
        private System.Windows.Forms.DataGridViewTextBoxColumn precoMinimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigobarras;
        private System.Windows.Forms.DataGridViewTextBoxColumn localestoque;
        private System.Windows.Forms.PictureBox btnUpload;
    }
}