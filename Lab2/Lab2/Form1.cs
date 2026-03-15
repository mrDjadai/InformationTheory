namespace Lab2
{
    public partial class Form1 : Form
    {
        private TextBox txtInitialState;
        private Button btnSelectFile;
        private Button btnEncrypt;
        private Button btnDecrypt;
        private Button btnSave;
        private TextBox txtFilePath;
        private TextBox txtKeyStream;
        private TextBox txtOriginalBinary;
        private TextBox txtEncryptedBinary;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        private byte[] originalFileData;
        private byte[] processedFileData;
        private string currentFilePath;
        private string generatedKeyStream;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "LFSR Шифрование (x^36 + x^11 + 1)";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblInitState = new Label
            {
                Text = "Начальное состояние регистра (36 бит):",
                Location = new Point(20, 20),
                Size = new Size(200, 20)
            };

            txtInitialState = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(400, 25),
                MaxLength = 36,
                BackColor = Color.LightPink
            };
            txtInitialState.KeyPress += TxtInitialState_KeyPress;
            txtInitialState.TextChanged += TxtInitialState_TextChanged;

            Label lblInitExample = new Label
            {
                Text = "Только 0 и 1, длина 36 символов",
                Location = new Point(430, 45),
                Size = new Size(200, 20),
                ForeColor = Color.Gray
            };

            Label lblFile = new Label
            {
                Text = "Выберите файл:",
                Location = new Point(20, 80),
                Size = new Size(100, 20)
            };

            txtFilePath = new TextBox
            {
                Location = new Point(20, 110),
                Size = new Size(550, 25),
                ReadOnly = true
            };

            btnSelectFile = new Button
            {
                Text = "Обзор...",
                Location = new Point(580, 110),
                Size = new Size(80, 30)
            };
            btnSelectFile.Click += BtnSelectFile_Click;

            btnEncrypt = new Button
            {
                Text = "Зашифровать",
                Location = new Point(20, 140),
                Size = new Size(120, 35),
                BackColor = Color.LightBlue,
                Enabled = false
            };
            btnEncrypt.Click += BtnEncrypt_Click;

            btnDecrypt = new Button
            {
                Text = "Расшифровать",
                Location = new Point(150, 140),
                Size = new Size(120, 35),
                BackColor = Color.LightGreen,
                Enabled = false
            };
            btnDecrypt.Click += BtnDecrypt_Click;

            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(280, 140),
                Size = new Size(150, 35),
                BackColor = Color.LightYellow,
                Enabled = false
            };
            btnSave.Click += BtnSave_Click;

            Label lblKey = new Label
            {
                Text = "Сгенерированный ключ:",
                Location = new Point(20, 235),
                Size = new Size(250, 20)
            };

            txtKeyStream = new TextBox
            {
                Location = new Point(20, 260),
                Size = new Size(640, 70),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Courier New", 9),
                BackColor = Color.LightGray,
                ScrollBars = ScrollBars.Vertical
            };

            Label lblOriginal = new Label
            {
                Text = "Исходный файл (двоичный вид):",
                Location = new Point(20, 330),
                Size = new Size(200, 20)
            };

            txtOriginalBinary = new TextBox
            {
                Location = new Point(20, 355),
                Size = new Size(640, 70),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Courier New", 9),
                BackColor = Color.LightGray,
                ScrollBars = ScrollBars.Vertical
            };

            Label lblEncrypted = new Label
            {
                Text = "Зашифрованный/расшифрованный файл (двоичный вид):",
                Location = new Point(20, 435),
                Size = new Size(300, 20)
            };

            txtEncryptedBinary = new TextBox
            {
                Location = new Point(20, 460),
                Size = new Size(640, 70),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Courier New", 9),
                BackColor = Color.LightGray,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.AddRange(new Control[] {
                lblInitState, txtInitialState, lblInitExample,
                lblFile, txtFilePath, btnSelectFile,
                btnEncrypt, btnDecrypt, btnSave,
                lblKey, txtKeyStream,
                lblOriginal, txtOriginalBinary,
                lblEncrypted, txtEncryptedBinary
            });

            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
        }

        private void TxtInitialState_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || e.KeyChar == '0' || e.KeyChar == '1'))
            {
                e.Handled = true;
            }
        }

        private void TxtInitialState_TextChanged(object sender, EventArgs e)
        {
            string originalText = txtInitialState.Text;
            string filteredText = new string(originalText.Where(c => c == '0' || c == '1').ToArray());

            if (filteredText.Length > 36)
                filteredText = filteredText.Substring(0, 36);

            if (originalText != filteredText)
            {
                int selectionStart = txtInitialState.SelectionStart;
                txtInitialState.Text = filteredText;
                txtInitialState.SelectionStart = Math.Min(selectionStart, filteredText.Length);
            }
            if (txtInitialState.Text.Length == 36)
            {
                txtInitialState.BackColor = Color.White;
                btnEncrypt.Enabled = !string.IsNullOrEmpty(txtFilePath.Text);
                btnDecrypt.Enabled = !string.IsNullOrEmpty(txtFilePath.Text);
            }
            else
            {
                txtInitialState.BackColor = Color.LightPink;
                btnEncrypt.Enabled = false;
                btnDecrypt.Enabled = false;
            }
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                txtFilePath.Text = currentFilePath;

                try
                {
                    originalFileData = File.ReadAllBytes(currentFilePath);
                    txtOriginalBinary.Text = LFSR.BytesToBinaryString(originalFileData);

                    if (txtInitialState.Text.Length == 36)
                    {
                        btnEncrypt.Enabled = true;
                        btnDecrypt.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            ProcessFile("Зашифрован");
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            ProcessFile("Расшифрован");
        }

        private void ProcessFile(string operationName)
        {
            try
            {

                if (!LFSR.Initialize(txtInitialState.Text))
                {
                    MessageBox.Show($"Ошибка инициализации регистра. Убедитесь, что введено 36 бит.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!LFSR.GenerateKeyStream(originalFileData.Length * 8, out generatedKeyStream))
                {
                    MessageBox.Show("Ошибка генерации ключа.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtKeyStream.Text = generatedKeyStream;

                if (!LFSR.ProcessData(originalFileData, out processedFileData))
                {
                    MessageBox.Show("Ошибка обработки данных.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtEncryptedBinary.Text = LFSR.BytesToBinaryString(processedFileData);

                MessageBox.Show("Операция завершена", operationName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (processedFileData == null)
                return;

            saveFileDialog.FileName = "processed_" + Path.GetFileName(currentFilePath);
            saveFileDialog.Filter = "All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, processedFileData);
                    MessageBox.Show("Файл успешно сохранен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
