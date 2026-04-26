namespace Lab3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
                private TextBox txtP, txtQ, txtCloseKeyInput, txtInputFile, txtOutputFile, txtR, txtEuler, txtOpenKeyOutput;
        private TextBox txtRDecrypt, txtCloseKeyDecrypt, txtDecryptInputFile, txtDecryptOutputFile;
        private Button btnEncrypt, btnDecrypt, btnBrowseInput, btnBrowseOutput, btnBrowseDecryptInput, btnBrowseDecryptOutput;
        private RichTextBox rtbOutput;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private void InitializeComponent()
        {
            this.Text = "RSA";
            this.Size = new System.Drawing.Size(1570, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1200, 700);

            // ============ ЛЕВАЯ ПАНЕЛЬ (ввод параметров) ============
            Panel leftPanel = new Panel()
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(790, 790),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            // ============ ПАНЕЛЬ ШИФРОВАНИЯ ============
            GroupBox gbEncrypt = new GroupBox()
            {
                Text = "ШИФРОВАНИЕ",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(770, 380),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold)
            };

            // Параметры шифрования
            Label lblP = new Label() { Text = "p (простое число):", Location = new System.Drawing.Point(15, 35), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtP = new TextBox() { Location = new System.Drawing.Point(180, 35), Size = new System.Drawing.Size(180, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtP.TextChanged += TxtP_Q_TextChanged;

            Label lblQ = new Label() { Text = "q (простое число):", Location = new System.Drawing.Point(15, 75), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtQ = new TextBox() { Location = new System.Drawing.Point(180, 75), Size = new System.Drawing.Size(180, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtQ.TextChanged += TxtP_Q_TextChanged;

            Label lblCloseKeyInput = new Label() { Text = "Закрытый ключ KC:", Location = new System.Drawing.Point(15, 115), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtCloseKeyInput = new TextBox() { Location = new System.Drawing.Point(180, 115), Size = new System.Drawing.Size(180, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtCloseKeyInput.TextChanged += TxtCloseKeyInput_TextChanged;

            // Вычисляемые параметры
            GroupBox gbCalculated = new GroupBox()
            {
                Text = "Вычисляемые параметры",
                Location = new System.Drawing.Point(570, 25),
                Size = new System.Drawing.Size(180, 250),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular)
            };

            Label lblR = new Label() { Text = "r = p*q:", Location = new System.Drawing.Point(8, 35), Size = new System.Drawing.Size(70, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtR = new TextBox() { Location = new System.Drawing.Point(8, 55), Size = new System.Drawing.Size(160, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightYellow };

            Label lblEuler = new Label() { Text = "φ(r):", Location = new System.Drawing.Point(8, 85), Size = new System.Drawing.Size(70, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtEuler = new TextBox() { Location = new System.Drawing.Point(8, 105), Size = new System.Drawing.Size(160, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightYellow };

            Label lblOpenKeyOutput = new Label() { Text = "Открытый ключ:", Location = new System.Drawing.Point(8, 135), Size = new System.Drawing.Size(150, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtOpenKeyOutput = new TextBox() { Location = new System.Drawing.Point(8, 155), Size = new System.Drawing.Size(160, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightGreen };

            gbCalculated.Controls.AddRange(new Control[] { lblR, txtR, lblEuler, txtEuler, lblOpenKeyOutput, txtOpenKeyOutput });

            Label lblInputFile = new Label() { Text = "Входной файл:", Location = new System.Drawing.Point(15, 165), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtInputFile = new TextBox() { Location = new System.Drawing.Point(180, 165), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseInput = new Button() { Text = "Обзор...", Location = new System.Drawing.Point(485, 165), Size = new System.Drawing.Size(70, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseInput.Click += BtnBrowseInput_Click;

            Label lblOutputFile = new Label() { Text = "Выходной файл:", Location = new System.Drawing.Point(15, 205), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtOutputFile = new TextBox() { Location = new System.Drawing.Point(180, 205), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseOutput = new Button() { Text = "Сохранить...", Location = new System.Drawing.Point(485, 205), Size = new System.Drawing.Size(70, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseOutput.Click += BtnBrowseOutput_Click;

            btnEncrypt = new Button()
            {
                Text = "ЗАШИФРОВАТЬ",
                Location = new System.Drawing.Point(180, 260),
                Size = new System.Drawing.Size(250, 50),
                BackColor = System.Drawing.Color.LightGreen,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 11, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnEncrypt.Click += BtnEncrypt_Click;

            gbEncrypt.Controls.AddRange(new Control[] { lblP, txtP, lblQ, txtQ, lblCloseKeyInput, txtCloseKeyInput,
                                                         gbCalculated, lblInputFile, txtInputFile, btnBrowseInput,
                                                         lblOutputFile, txtOutputFile, btnBrowseOutput, btnEncrypt });

            // ============ ПАНЕЛЬ РАСШИФРОВАНИЯ ============
            GroupBox gbDecrypt = new GroupBox()
            {
                Text = "РАСШИФРОВАНИЕ",
                Location = new System.Drawing.Point(10, 400),
                Size = new System.Drawing.Size(770, 280),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold)
            };

            Label lblRDecrypt = new Label() { Text = "Модуль r (n = p*q):", Location = new System.Drawing.Point(15, 35), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtRDecrypt = new TextBox() { Location = new System.Drawing.Point(180, 35), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };

            Label lblCloseKeyDecrypt = new Label() { Text = "Закрытый ключ KC:", Location = new System.Drawing.Point(15, 75), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtCloseKeyDecrypt = new TextBox() { Location = new System.Drawing.Point(180, 75), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };

            Label lblDecryptInput = new Label() { Text = "Зашифрованный файл:", Location = new System.Drawing.Point(15, 115), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtDecryptInputFile = new TextBox() { Location = new System.Drawing.Point(180, 115), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseDecryptInput = new Button() { Text = "Обзор...", Location = new System.Drawing.Point(485, 115), Size = new System.Drawing.Size(70, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseDecryptInput.Click += BtnBrowseDecryptInput_Click;

            Label lblDecryptOutput = new Label() { Text = "Выходной файл:", Location = new System.Drawing.Point(15, 155), Size = new System.Drawing.Size(150, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            txtDecryptOutputFile = new TextBox() { Location = new System.Drawing.Point(180, 155), Size = new System.Drawing.Size(300, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseDecryptOutput = new Button() { Text = "Сохранить...", Location = new System.Drawing.Point(485, 155), Size = new System.Drawing.Size(70, 25), Font = new System.Drawing.Font("Microsoft Sans Serif", 9) };
            btnBrowseDecryptOutput.Click += BtnBrowseDecryptOutput_Click;

            btnDecrypt = new Button()
            {
                Text = "РАСШИФРОВАТЬ",
                Location = new System.Drawing.Point(180, 210),
                Size = new System.Drawing.Size(250, 50),
                BackColor = System.Drawing.Color.LightCoral,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 11, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnDecrypt.Click += BtnDecrypt_Click;

            gbDecrypt.Controls.AddRange(new Control[] { lblRDecrypt, txtRDecrypt, lblCloseKeyDecrypt, txtCloseKeyDecrypt,
                                                         lblDecryptInput, txtDecryptInputFile, btnBrowseDecryptInput,
                                                         lblDecryptOutput, txtDecryptOutputFile, btnBrowseDecryptOutput, btnDecrypt });

            leftPanel.Controls.AddRange(new Control[] { gbEncrypt, gbDecrypt });

            // ============ ПРАВАЯ ПАНЕЛЬ (вывод результатов) ============
            Panel rightPanel = new Panel()
            {
                Location = new System.Drawing.Point(810, 10),
                Size = new System.Drawing.Size(740, 790),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblOutput = new Label()
            {
                Text = "РЕЗУЛЬТАТЫ РАБОТЫ (пары: исходный байт -> зашифрованное значение)",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(720, 30),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            rtbOutput = new RichTextBox()
            {
                Location = new System.Drawing.Point(10, 50),
                Size = new System.Drawing.Size(715, 725),
                Font = new System.Drawing.Font("Consolas", 9),
                WordWrap = true,
                ReadOnly = true,
                BackColor = System.Drawing.Color.White
            };

            rightPanel.Controls.AddRange(new Control[] { lblOutput, rtbOutput });

            this.Controls.AddRange(new Control[] { leftPanel, rightPanel });

            openFileDialog = new OpenFileDialog() { Filter = "All files (*.*)|*.*" };
            saveFileDialog = new SaveFileDialog() { Filter = "All files (*.*)|*.*" };
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>


        #endregion
    }
}
