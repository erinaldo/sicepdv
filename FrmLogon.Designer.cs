namespace SICEpdv
{
    partial class FrmLogon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogon));
            this.pnlTeclado = new System.Windows.Forms.Panel();
            this.txtOperador = new System.Windows.Forms.TextBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.lblIDCliente = new System.Windows.Forms.Label();
            this.txtEmpresa = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelTeclado1 = new System.Windows.Forms.Panel();
            this.btnTecladoLogin = new System.Windows.Forms.Button();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.panelTeclado2 = new System.Windows.Forms.Panel();
            this.btnTecladoSenha = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtVersao = new System.Windows.Forms.Label();
            this.logoPdv = new System.Windows.Forms.PictureBox();
            this.pnlQR = new System.Windows.Forms.Panel();
            this.lblWhats = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlAvaliacao = new System.Windows.Forms.Panel();
            this.rating4 = new System.Windows.Forms.Panel();
            this.rating5 = new System.Windows.Forms.Panel();
            this.rating3 = new System.Windows.Forms.Panel();
            this.rating1 = new System.Windows.Forms.Panel();
            this.rating2 = new System.Windows.Forms.Panel();
            this.btnMostrarEstrelas = new System.Windows.Forms.PictureBox();
            this.btnOcultarAvaliacao2 = new System.Windows.Forms.Panel();
            this.btnOcultarAvaliacao = new System.Windows.Forms.PictureBox();
            this.txtAvaliacao = new System.Windows.Forms.Label();
            this.txtTecnicoIQCard = new System.Windows.Forms.Label();
            this.imgQrCodeSuporte = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblAguarde = new System.Windows.Forms.Label();
            this.timeVerificaRest = new System.Windows.Forms.Timer(this.components);
            this.pnlIQCard = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlLogin.SuspendLayout();
            this.panelTeclado1.SuspendLayout();
            this.panelTeclado2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPdv)).BeginInit();
            this.pnlQR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlAvaliacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMostrarEstrelas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOcultarAvaliacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgQrCodeSuporte)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlIQCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTeclado
            // 
            this.pnlTeclado.AutoSize = true;
            this.pnlTeclado.BackColor = System.Drawing.Color.Transparent;
            this.pnlTeclado.Location = new System.Drawing.Point(32, 37);
            this.pnlTeclado.Name = "pnlTeclado";
            this.pnlTeclado.Size = new System.Drawing.Size(219, 200);
            this.pnlTeclado.TabIndex = 1;
            this.pnlTeclado.Visible = false;
            // 
            // txtOperador
            // 
            this.txtOperador.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperador.Font = new System.Drawing.Font("Corbel", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperador.Location = new System.Drawing.Point(120, 109);
            this.txtOperador.MaxLength = 16;
            this.txtOperador.Name = "txtOperador";
            this.txtOperador.Size = new System.Drawing.Size(321, 30);
            this.txtOperador.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtOperador, "Digite o nome do operador ou o número");
            // 
            // txtSenha
            // 
            this.txtSenha.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSenha.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.Location = new System.Drawing.Point(120, 184);
            this.txtSenha.MaxLength = 20;
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(319, 28);
            this.txtSenha.TabIndex = 5;
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogin.Controls.Add(this.lblIDCliente);
            this.pnlLogin.Controls.Add(this.txtEmpresa);
            this.pnlLogin.Controls.Add(this.label3);
            this.pnlLogin.Controls.Add(this.panelTeclado1);
            this.pnlLogin.Controls.Add(this.lblDescricao);
            this.pnlLogin.Controls.Add(this.panelTeclado2);
            this.pnlLogin.Controls.Add(this.button1);
            this.pnlLogin.Controls.Add(this.txtVersao);
            this.pnlLogin.Controls.Add(this.txtSenha);
            this.pnlLogin.Controls.Add(this.txtOperador);
            this.pnlLogin.Controls.Add(this.logoPdv);
            this.pnlLogin.Controls.Add(this.pnlQR);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(791, 333);
            this.pnlLogin.TabIndex = 13;
            this.pnlLogin.Visible = false;
            // 
            // lblIDCliente
            // 
            this.lblIDCliente.AutoSize = true;
            this.lblIDCliente.ForeColor = System.Drawing.Color.White;
            this.lblIDCliente.Location = new System.Drawing.Point(30, 254);
            this.lblIDCliente.Name = "lblIDCliente";
            this.lblIDCliente.Size = new System.Drawing.Size(45, 13);
            this.lblIDCliente.TabIndex = 25;
            this.lblIDCliente.Text = "ID 0000";
            // 
            // txtEmpresa
            // 
            this.txtEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpresa.ForeColor = System.Drawing.Color.White;
            this.txtEmpresa.Location = new System.Drawing.Point(30, 266);
            this.txtEmpresa.Name = "txtEmpresa";
            this.txtEmpresa.Size = new System.Drawing.Size(355, 23);
            this.txtEmpresa.TabIndex = 24;
            this.txtEmpresa.Text = "label4";
            this.txtEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(291, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Todos os direitos reservados © 1997 - 2019";
            // 
            // panelTeclado1
            // 
            this.panelTeclado1.Controls.Add(this.btnTecladoLogin);
            this.panelTeclado1.Location = new System.Drawing.Point(426, 106);
            this.panelTeclado1.Name = "panelTeclado1";
            this.panelTeclado1.Size = new System.Drawing.Size(60, 30);
            this.panelTeclado1.TabIndex = 22;
            this.panelTeclado1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTeclado1_Paint);
            // 
            // btnTecladoLogin
            // 
            this.btnTecladoLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTecladoLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTecladoLogin.FlatAppearance.BorderSize = 0;
            this.btnTecladoLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTecladoLogin.ForeColor = System.Drawing.Color.Transparent;
            this.btnTecladoLogin.Location = new System.Drawing.Point(1, -6);
            this.btnTecladoLogin.Name = "btnTecladoLogin";
            this.btnTecladoLogin.Size = new System.Drawing.Size(54, 41);
            this.btnTecladoLogin.TabIndex = 0;
            this.btnTecladoLogin.UseVisualStyleBackColor = true;
            this.btnTecladoLogin.Click += new System.EventHandler(this.btnTecladoLogin_Click);
            // 
            // lblDescricao
            // 
            this.lblDescricao.BackColor = System.Drawing.Color.Transparent;
            this.lblDescricao.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(84)))), ((int)(((byte)(79)))));
            this.lblDescricao.Location = new System.Drawing.Point(16, 58);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(489, 32);
            this.lblDescricao.TabIndex = 7;
            this.lblDescricao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTeclado2
            // 
            this.panelTeclado2.Controls.Add(this.btnTecladoSenha);
            this.panelTeclado2.Location = new System.Drawing.Point(426, 182);
            this.panelTeclado2.Name = "panelTeclado2";
            this.panelTeclado2.Size = new System.Drawing.Size(55, 28);
            this.panelTeclado2.TabIndex = 21;
            // 
            // btnTecladoSenha
            // 
            this.btnTecladoSenha.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTecladoSenha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTecladoSenha.FlatAppearance.BorderSize = 0;
            this.btnTecladoSenha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTecladoSenha.ForeColor = System.Drawing.Color.Transparent;
            this.btnTecladoSenha.Location = new System.Drawing.Point(1, -8);
            this.btnTecladoSenha.Name = "btnTecladoSenha";
            this.btnTecladoSenha.Size = new System.Drawing.Size(54, 41);
            this.btnTecladoSenha.TabIndex = 1;
            this.btnTecladoSenha.UseVisualStyleBackColor = true;
            this.btnTecladoSenha.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(193)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(409, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 15;
            this.button1.Text = "ACESSAR";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtVersao
            // 
            this.txtVersao.AutoSize = true;
            this.txtVersao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(193)))));
            this.txtVersao.Location = new System.Drawing.Point(410, 15);
            this.txtVersao.Name = "txtVersao";
            this.txtVersao.Size = new System.Drawing.Size(35, 13);
            this.txtVersao.TabIndex = 14;
            this.txtVersao.Text = "label1";
            // 
            // logoPdv
            // 
            this.logoPdv.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.logoPdv.BackColor = System.Drawing.Color.Transparent;
            this.logoPdv.BackgroundImage = global::SICEpdv.Properties.Resources.logo_pdv;
            this.logoPdv.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logoPdv.Location = new System.Drawing.Point(194, 9);
            this.logoPdv.Name = "logoPdv";
            this.logoPdv.Size = new System.Drawing.Size(100, 65);
            this.logoPdv.TabIndex = 13;
            this.logoPdv.TabStop = false;
            // 
            // pnlQR
            // 
            this.pnlQR.Controls.Add(this.lblWhats);
            this.pnlQR.Controls.Add(this.pictureBox1);
            this.pnlQR.Controls.Add(this.pnlAvaliacao);
            this.pnlQR.Controls.Add(this.btnMostrarEstrelas);
            this.pnlQR.Controls.Add(this.btnOcultarAvaliacao2);
            this.pnlQR.Controls.Add(this.btnOcultarAvaliacao);
            this.pnlQR.Controls.Add(this.txtAvaliacao);
            this.pnlQR.Controls.Add(this.txtTecnicoIQCard);
            this.pnlQR.Controls.Add(this.pnlTeclado);
            this.pnlQR.Controls.Add(this.imgQrCodeSuporte);
            this.pnlQR.Controls.Add(this.label10);
            this.pnlQR.Controls.Add(this.label7);
            this.pnlQR.Controls.Add(this.label1);
            this.pnlQR.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlQR.Location = new System.Drawing.Point(515, 0);
            this.pnlQR.Name = "pnlQR";
            this.pnlQR.Size = new System.Drawing.Size(276, 333);
            this.pnlQR.TabIndex = 20;
            // 
            // lblWhats
            // 
            this.lblWhats.BackColor = System.Drawing.Color.Transparent;
            this.lblWhats.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhats.ForeColor = System.Drawing.Color.White;
            this.lblWhats.Location = new System.Drawing.Point(76, 262);
            this.lblWhats.Name = "lblWhats";
            this.lblWhats.Size = new System.Drawing.Size(162, 19);
            this.lblWhats.TabIndex = 41;
            this.lblWhats.Text = "(87) 9 9981 0000";
            this.lblWhats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SICEpdv.Properties.Resources.whatsapp;
            this.pictureBox1.Location = new System.Drawing.Point(39, 256);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            // 
            // pnlAvaliacao
            // 
            this.pnlAvaliacao.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlAvaliacao.Controls.Add(this.rating4);
            this.pnlAvaliacao.Controls.Add(this.rating5);
            this.pnlAvaliacao.Controls.Add(this.rating3);
            this.pnlAvaliacao.Controls.Add(this.rating1);
            this.pnlAvaliacao.Controls.Add(this.rating2);
            this.pnlAvaliacao.Location = new System.Drawing.Point(40, 284);
            this.pnlAvaliacao.Name = "pnlAvaliacao";
            this.pnlAvaliacao.Size = new System.Drawing.Size(246, 29);
            this.pnlAvaliacao.TabIndex = 0;
            // 
            // rating4
            // 
            this.rating4.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gray;
            this.rating4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rating4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rating4.Location = new System.Drawing.Point(133, 5);
            this.rating4.Name = "rating4";
            this.rating4.Size = new System.Drawing.Size(20, 20);
            this.rating4.TabIndex = 30;
            this.rating4.Click += new System.EventHandler(this.rating4_Click);
            // 
            // rating5
            // 
            this.rating5.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gray;
            this.rating5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rating5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rating5.Location = new System.Drawing.Point(163, 5);
            this.rating5.Name = "rating5";
            this.rating5.Size = new System.Drawing.Size(20, 20);
            this.rating5.TabIndex = 31;
            this.rating5.Click += new System.EventHandler(this.rating5_Click);
            // 
            // rating3
            // 
            this.rating3.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gray;
            this.rating3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rating3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rating3.Location = new System.Drawing.Point(104, 5);
            this.rating3.Name = "rating3";
            this.rating3.Size = new System.Drawing.Size(20, 20);
            this.rating3.TabIndex = 29;
            this.rating3.Click += new System.EventHandler(this.rating3_Click);
            // 
            // rating1
            // 
            this.rating1.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gray;
            this.rating1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rating1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rating1.Location = new System.Drawing.Point(51, 5);
            this.rating1.Name = "rating1";
            this.rating1.Size = new System.Drawing.Size(20, 20);
            this.rating1.TabIndex = 27;
            this.rating1.Click += new System.EventHandler(this.rating1_Click);
            // 
            // rating2
            // 
            this.rating2.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gray;
            this.rating2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rating2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rating2.Location = new System.Drawing.Point(77, 5);
            this.rating2.Name = "rating2";
            this.rating2.Size = new System.Drawing.Size(20, 20);
            this.rating2.TabIndex = 28;
            this.rating2.Click += new System.EventHandler(this.rating2_Click);
            // 
            // btnMostrarEstrelas
            // 
            this.btnMostrarEstrelas.BackgroundImage = global::SICEpdv.Properties.Resources.rating_gold;
            this.btnMostrarEstrelas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMostrarEstrelas.Location = new System.Drawing.Point(224, 311);
            this.btnMostrarEstrelas.Name = "btnMostrarEstrelas";
            this.btnMostrarEstrelas.Size = new System.Drawing.Size(15, 15);
            this.btnMostrarEstrelas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMostrarEstrelas.TabIndex = 35;
            this.btnMostrarEstrelas.TabStop = false;
            this.btnMostrarEstrelas.Click += new System.EventHandler(this.btnMostrarEstrelas_Click);
            // 
            // btnOcultarAvaliacao2
            // 
            this.btnOcultarAvaliacao2.Location = new System.Drawing.Point(256, 210);
            this.btnOcultarAvaliacao2.Name = "btnOcultarAvaliacao2";
            this.btnOcultarAvaliacao2.Size = new System.Drawing.Size(15, 15);
            this.btnOcultarAvaliacao2.TabIndex = 34;
            this.btnOcultarAvaliacao2.Visible = false;
            this.btnOcultarAvaliacao2.Click += new System.EventHandler(this.btnOcultarAvaliacao_Click_1);
            // 
            // btnOcultarAvaliacao
            // 
            this.btnOcultarAvaliacao.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOcultarAvaliacao.Image = global::SICEpdv.Properties.Resources.btn_times;
            this.btnOcultarAvaliacao.Location = new System.Drawing.Point(250, 311);
            this.btnOcultarAvaliacao.Name = "btnOcultarAvaliacao";
            this.btnOcultarAvaliacao.Size = new System.Drawing.Size(15, 15);
            this.btnOcultarAvaliacao.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnOcultarAvaliacao.TabIndex = 33;
            this.btnOcultarAvaliacao.TabStop = false;
            this.btnOcultarAvaliacao.Click += new System.EventHandler(this.btnOcultarAvaliacao_Click);
            // 
            // txtAvaliacao
            // 
            this.txtAvaliacao.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.txtAvaliacao.ForeColor = System.Drawing.Color.White;
            this.txtAvaliacao.Location = new System.Drawing.Point(29, 305);
            this.txtAvaliacao.Name = "txtAvaliacao";
            this.txtAvaliacao.Size = new System.Drawing.Size(239, 22);
            this.txtAvaliacao.TabIndex = 32;
            this.txtAvaliacao.Text = "Ocultar Avaliação";
            this.txtAvaliacao.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.txtAvaliacao.Click += new System.EventHandler(this.btnMostrarEstrelas_Click);
            // 
            // txtTecnicoIQCard
            // 
            this.txtTecnicoIQCard.BackColor = System.Drawing.Color.Transparent;
            this.txtTecnicoIQCard.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTecnicoIQCard.ForeColor = System.Drawing.Color.White;
            this.txtTecnicoIQCard.Location = new System.Drawing.Point(36, 241);
            this.txtTecnicoIQCard.Name = "txtTecnicoIQCard";
            this.txtTecnicoIQCard.Size = new System.Drawing.Size(209, 19);
            this.txtTecnicoIQCard.TabIndex = 24;
            this.txtTecnicoIQCard.Text = "TÉCNICO";
            this.txtTecnicoIQCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imgQrCodeSuporte
            // 
            this.imgQrCodeSuporte.BackColor = System.Drawing.Color.White;
            this.imgQrCodeSuporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgQrCodeSuporte.Location = new System.Drawing.Point(57, 58);
            this.imgQrCodeSuporte.Name = "imgQrCodeSuporte";
            this.imgQrCodeSuporte.Size = new System.Drawing.Size(182, 181);
            this.imgQrCodeSuporte.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imgQrCodeSuporte.TabIndex = 26;
            this.imgQrCodeSuporte.TabStop = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(3, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(273, 19);
            this.label10.TabIndex = 23;
            this.label10.Text = "IQCARD E LEIA O QRCODE";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(128, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "BAIXE O APLICATIVO";
            this.label7.Click += new System.EventHandler(this.label7_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(154)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "PARA SUPORTE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(58, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "www.iqsistemas.com.br";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 70);
            this.panel1.TabIndex = 6;
            this.panel1.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Olive;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(20, 33);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 29);
            this.button3.TabIndex = 16;
            this.button3.TabStop = false;
            this.button3.Text = "Tecl. Virtual";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtDescricao
            // 
            this.txtDescricao.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtDescricao.BackColor = System.Drawing.SystemColors.Info;
            this.txtDescricao.Location = new System.Drawing.Point(217, -14);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.ReadOnly = true;
            this.txtDescricao.Size = new System.Drawing.Size(30, 28);
            this.txtDescricao.TabIndex = 0;
            this.txtDescricao.TabStop = false;
            this.txtDescricao.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 20;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            // 
            // lblAguarde
            // 
            this.lblAguarde.AutoSize = true;
            this.lblAguarde.Location = new System.Drawing.Point(201, 179);
            this.lblAguarde.Name = "lblAguarde";
            this.lblAguarde.Size = new System.Drawing.Size(0, 13);
            this.lblAguarde.TabIndex = 15;
            // 
            // timeVerificaRest
            // 
            this.timeVerificaRest.Interval = 6000;
            this.timeVerificaRest.Tick += new System.EventHandler(this.timeVerificaRest_Tick);
            // 
            // pnlIQCard
            // 
            this.pnlIQCard.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pnlIQCard.Controls.Add(this.label11);
            this.pnlIQCard.Controls.Add(this.label5);
            this.pnlIQCard.Controls.Add(this.label6);
            this.pnlIQCard.Controls.Add(this.pictureBox2);
            this.pnlIQCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlIQCard.Location = new System.Drawing.Point(5, 201);
            this.pnlIQCard.Name = "pnlIQCard";
            this.pnlIQCard.Size = new System.Drawing.Size(17, 77);
            this.pnlIQCard.TabIndex = 18;
            this.pnlIQCard.Visible = false;
            this.pnlIQCard.Click += new System.EventHandler(this.pnlIQCard_Click);
            this.pnlIQCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlIQCard_Paint);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(215, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "BEM-VINDO!";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(417, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "CLIENTES.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(214, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(247, 30);
            this.label6.TabIndex = 19;
            this.label6.Text = "                          CRIE UMA CONTA PARA SUA\r\nEMPRESA E COMECE A FIDELIZAR";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(78, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(130, 39);
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // FrmLogon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(791, 333);
            this.ControlBox = false;
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.lblAguarde);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlIQCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "®SICEpdv.net versão 1.0";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogon_FormClosed);
            this.Load += new System.EventHandler(this.FrmLogon_Load);
            this.Shown += new System.EventHandler(this.FrmLogon_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogon_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmLogon_KeyPress);
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.panelTeclado1.ResumeLayout(false);
            this.panelTeclado2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPdv)).EndInit();
            this.pnlQR.ResumeLayout(false);
            this.pnlQR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlAvaliacao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnMostrarEstrelas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOcultarAvaliacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgQrCodeSuporte)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlIQCard.ResumeLayout(false);
            this.pnlIQCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTeclado;
        private System.Windows.Forms.TextBox txtOperador;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox logoPdv;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox txtDescricao;
        public System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblAguarde;
        private System.Windows.Forms.Timer timeVerificaRest;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel pnlIQCard;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label txtVersao;
        private System.Windows.Forms.Panel pnlQR;
        private System.Windows.Forms.PictureBox imgQrCodeSuporte;
        private System.Windows.Forms.Label txtTecnicoIQCard;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelTeclado1;
        private System.Windows.Forms.Panel panelTeclado2;
        private System.Windows.Forms.Button btnTecladoLogin;
        private System.Windows.Forms.Button btnTecladoSenha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel rating1;
        private System.Windows.Forms.Panel rating5;
        private System.Windows.Forms.Panel rating4;
        private System.Windows.Forms.Panel rating3;
        private System.Windows.Forms.Panel rating2;
        private System.Windows.Forms.Label txtAvaliacao;
        private System.Windows.Forms.PictureBox btnOcultarAvaliacao;
        private System.Windows.Forms.Panel btnOcultarAvaliacao2;
        private System.Windows.Forms.PictureBox btnMostrarEstrelas;
        private System.Windows.Forms.Panel pnlAvaliacao;
        private System.Windows.Forms.Label txtEmpresa;
        private System.Windows.Forms.Label lblIDCliente;
        private System.Windows.Forms.Label lblWhats;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}