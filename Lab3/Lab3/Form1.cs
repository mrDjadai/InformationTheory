using System.Numerics;

namespace Lab3
{
    public partial class Form1 : Form
    {
        private TextBox txtP, txtQ, txtCloseKeyInput, txtInputFile, txtOutputFile, txtR, txtEuler, txtOpenKeyOutput;
        private TextBox txtRDecrypt, txtCloseKeyDecrypt, txtDecryptInputFile, txtDecryptOutputFile;
        private Button btnEncrypt, btnDecrypt, btnBrowseInput, btnBrowseOutput, btnBrowseDecryptInput, btnBrowseDecryptOutput;
        private RichTextBox rtbOutput;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "RSA File Cipher - Lab3";
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
                Size = new System.Drawing.Size(175, 250),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular)
            };

            Label lblR = new Label() { Text = "r = p*q:", Location = new System.Drawing.Point(8, 35), Size = new System.Drawing.Size(70, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtR = new TextBox() { Location = new System.Drawing.Point(8, 55), Size = new System.Drawing.Size(155, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightYellow };

            Label lblEuler = new Label() { Text = "φ(r):", Location = new System.Drawing.Point(8, 85), Size = new System.Drawing.Size(70, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtEuler = new TextBox() { Location = new System.Drawing.Point(8, 105), Size = new System.Drawing.Size(155, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightYellow };

            Label lblOpenKeyOutput = new Label() { Text = "Открытый ключ:", Location = new System.Drawing.Point(8, 135), Size = new System.Drawing.Size(150, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8) };
            txtOpenKeyOutput = new TextBox() { Location = new System.Drawing.Point(8, 155), Size = new System.Drawing.Size(155, 20), Font = new System.Drawing.Font("Microsoft Sans Serif", 8), ReadOnly = true, BackColor = System.Drawing.Color.LightGreen };

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
                Location = new System.Drawing.Point(800, 10),
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

        private void TxtP_Q_TextChanged(object sender, EventArgs e)
        {
            if (BigInteger.TryParse(txtP.Text, out BigInteger p) &&
                BigInteger.TryParse(txtQ.Text, out BigInteger q) &&
                p > 0 && q > 0)
            {
                BigInteger r = p * q;
                BigInteger phi = (p - 1) * (q - 1);
                txtR.Text = r.ToString();
                txtEuler.Text = phi.ToString();

                // Если введен закрытый ключ, вычисляем открытый
                if (BigInteger.TryParse(txtCloseKeyInput.Text, out BigInteger closeKey) && closeKey > 0)
                {
                    try
                    {
                        BigInteger openKey = ComputeOpenKey(closeKey, phi);
                        txtOpenKeyOutput.Text = openKey.ToString();
                    }
                    catch
                    {
                        txtOpenKeyOutput.Text = "Ошибка";
                    }
                }
            }
            else
            {
                txtR.Text = "";
                txtEuler.Text = "";
                txtOpenKeyOutput.Text = "";
            }
        }

        // Обработчик изменения закрытого ключа
        private void TxtCloseKeyInput_TextChanged(object sender, EventArgs e)
        {
            if (BigInteger.TryParse(txtP.Text, out BigInteger p) &&
                BigInteger.TryParse(txtQ.Text, out BigInteger q) &&
                BigInteger.TryParse(txtCloseKeyInput.Text, out BigInteger closeKey) &&
                p > 0 && q > 0 && closeKey > 0)
            {
                BigInteger phi = (p - 1) * (q - 1);
                try
                {
                    BigInteger openKey = ComputeOpenKey(closeKey, phi);
                    txtOpenKeyOutput.Text = openKey.ToString();
                }
                catch
                {
                    txtOpenKeyOutput.Text = "Ошибка";
                }
            }
            else
            {
                txtOpenKeyOutput.Text = "";
            }
        }

        private void BtnBrowseInput_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog.FileName;
                if (string.IsNullOrEmpty(txtOutputFile.Text))
                {
                    string extension = Path.GetExtension(openFileDialog.FileName);
                    txtOutputFile.Text = Path.GetDirectoryName(openFileDialog.FileName) + "\\" +
                                        Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "_encrypted" + extension;
                }
            }
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = Path.GetFileName(txtOutputFile.Text);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputFile.Text = saveFileDialog.FileName;
            }
        }

        private void BtnBrowseDecryptInput_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtDecryptInputFile.Text = openFileDialog.FileName;
                if (string.IsNullOrEmpty(txtDecryptOutputFile.Text))
                {
                    string extension = Path.GetExtension(openFileDialog.FileName);
                    txtDecryptOutputFile.Text = Path.GetDirectoryName(openFileDialog.FileName) + "\\" +
                                               Path.GetFileNameWithoutExtension(openFileDialog.FileName).Replace("_encrypted", "") + "_decrypted" + extension;
                }
            }
        }

        private void BtnBrowseDecryptOutput_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = Path.GetFileName(txtDecryptOutputFile.Text);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtDecryptOutputFile.Text = saveFileDialog.FileName;
            }
        }

        // Алгоритм быстрого возведения в степень (a^b mod m)
        private BigInteger FastPowMod(BigInteger a, BigInteger b, BigInteger m)
        {
            if (m == 1) return 0;
            BigInteger result = 1;
            a = a % m;
            while (b > 0)
            {
                if ((b & 1) == 1)
                    result = (result * a) % m;
                a = (a * a) % m;
                b >>= 1;
            }
            return result;
        }

        // Расширенный алгоритм Евклида для нахождения обратного элемента
        private BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            BigInteger gcd = ExtendedGCD(b, a % b, out x, out y);
            BigInteger temp = x;
            x = y;
            y = temp - (a / b) * y;
            return gcd;
        }

        // Проверка числа на простоту (упрощенная)
        private bool IsPrime(BigInteger n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            for (BigInteger i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        // Вычисление функции Эйлера
        private BigInteger EulerFunction(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }

        // Вычисление открытого ключа по закрытому
        private BigInteger ComputeOpenKey(BigInteger d, BigInteger phi)
        {
            BigInteger x, y;
            BigInteger gcd = ExtendedGCD(d, phi, out x, out y);
            if (gcd != 1)
                throw new Exception("Закрытый ключ и φ(n) не взаимно просты!");
            BigInteger e = (x % phi + phi) % phi;
            return e;
        }

        // Вычисление закрытого ключа по открытому
        private BigInteger ComputePrivateKey(BigInteger e, BigInteger phi)
        {
            BigInteger x, y;
            BigInteger gcd = ExtendedGCD(e, phi, out x, out y);
            if (gcd != 1)
                throw new Exception("Открытый ключ и φ(n) не взаимно просты!");
            return (x % phi + phi) % phi;
        }

        // Преобразование BigInteger в массив байтов фиксированной длины
        private byte[] GetBytesFromBigInteger(BigInteger value, int byteCount)
        {
            byte[] bytes = new byte[byteCount];
            byte[] temp = value.ToByteArray();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);

            int copyLength = Math.Min(temp.Length, byteCount);
            Array.Copy(temp, Math.Max(0, temp.Length - copyLength), bytes, byteCount - copyLength, copyLength);

            return bytes;
        }

        // Преобразование 2 байтов в BigInteger
        private BigInteger GetBigIntegerFromBytes(byte[] bytes, int startIndex)
        {
            byte[] temp = new byte[2];
            Array.Copy(bytes, startIndex, temp, 0, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(temp);
            return new BigInteger(temp);
        }

        // Шифрование файла
        private void EncryptFile(string inputFile, string outputFile, BigInteger openKey, BigInteger module)
        {
            byte[] fileBytes = File.ReadAllBytes(inputFile);

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (byte b in fileBytes)
                {
                    BigInteger encrypted = FastPowMod(b, openKey, module);
                    byte[] encryptedBytes16 = GetBytesFromBigInteger(encrypted, 2);
                    ms.Write(encryptedBytes16, 0, encryptedBytes16.Length);
                }

                File.WriteAllBytes(outputFile, ms.ToArray());
            }
        }

        // Расшифрование файла
        private void DecryptFile(string inputFile, string outputFile, BigInteger closeKey, BigInteger module)
        {
            byte[] encryptedData = File.ReadAllBytes(inputFile);

            if (encryptedData.Length % 2 != 0)
                throw new Exception("Зашифрованный файл поврежден: размер не кратен 2 байтам!");

            using (MemoryStream ms = new MemoryStream())
            {
                for (int i = 0; i < encryptedData.Length; i += 2)
                {
                    BigInteger encryptedValue = GetBigIntegerFromBytes(encryptedData, i);
                    BigInteger decrypted = FastPowMod(encryptedValue, closeKey, module);
                    byte originalByte = (byte)(decrypted % 256);
                    ms.WriteByte(originalByte);
                }

                File.WriteAllBytes(outputFile, ms.ToArray());
            }
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка ввода
                if (!BigInteger.TryParse(txtP.Text, out BigInteger p))
                    throw new Exception("p должно быть целым положительным числом!");

                if (!BigInteger.TryParse(txtQ.Text, out BigInteger q))
                    throw new Exception("q должно быть целым положительным числом!");

                if (!BigInteger.TryParse(txtCloseKeyInput.Text, out BigInteger closeKey))
                    throw new Exception("Закрытый ключ должен быть целым положительным числом!");

                if (string.IsNullOrEmpty(txtInputFile.Text) || !File.Exists(txtInputFile.Text))
                    throw new Exception("Укажите существующий входной файл!");

                if (string.IsNullOrEmpty(txtOutputFile.Text))
                    throw new Exception("Укажите путь для сохранения зашифрованного файла!");

                // Проверка, что p и q - простые числа
                if (!IsPrime(p))
                    throw new Exception($"p = {p} не является простым числом!");

                if (!IsPrime(q))
                    throw new Exception($"q = {q} не является простым числом!");

                if (p == q)
                    throw new Exception("p и q должны быть различными!");

                // Вычисление модуля
                BigInteger module = p * q;

                // Вычисление функции Эйлера
                BigInteger phi = EulerFunction(p, q);

                // Вычисляем открытый ключ из закрытого
                BigInteger openKey = ComputeOpenKey(closeKey, phi);

                // Проверка, что открытый ключ меньше φ(n)
                if (openKey >= phi)
                    throw new Exception($"Вычисленный открытый ключ = {openKey} больше или равен φ(n) = {phi}!");

                // Читаем исходный файл для вывода пар
                byte[] originalBytes = File.ReadAllBytes(txtInputFile.Text);

                // Шифрование
                EncryptFile(txtInputFile.Text, txtOutputFile.Text, openKey, module);

                // Читаем зашифрованный файл для вывода
                byte[] encryptedBytes = File.ReadAllBytes(txtOutputFile.Text);

                // Вывод информации и пар "исходный -> зашифрованный"
                rtbOutput.Clear();
                rtbOutput.AppendText("=== ИНФОРМАЦИЯ О ШИФРОВАНИИ ===\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText($"Исходный файл: {System.IO.Path.GetFileName(txtInputFile.Text)}\r\n");
                rtbOutput.AppendText($"Зашифрованный файл: {System.IO.Path.GetFileName(txtOutputFile.Text)}\r\n");
                rtbOutput.AppendText($"p = {p}\r\n");
                rtbOutput.AppendText($"q = {q}\r\n");
                rtbOutput.AppendText($"Модуль r (n) = {module}\r\n");
                rtbOutput.AppendText($"φ(n) = {phi}\r\n");
                rtbOutput.AppendText($"Закрытый ключ KC (введен) = {closeKey}\r\n");
                rtbOutput.AppendText($"Открытый ключ KC (вычислен) = {openKey}\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText("ПАРЫ (исходный байт -> зашифрованное значение в 10-й системе):\r\n");
                rtbOutput.AppendText(new string('=', 70) + "\r\n");

                // Выводим пары исходный-зашифрованный
                for (int i = 0; i < originalBytes.Length; i++)
                {
                    if (i * 2 + 1 < encryptedBytes.Length)
                    {
                        ushort encryptedValue = (ushort)((encryptedBytes[i * 2] << 8) | encryptedBytes[i * 2 + 1]);
                        rtbOutput.AppendText($"{originalBytes[i],6} -> {encryptedValue,8}");

                        if ((i + 1) % 3 == 0)
                            rtbOutput.AppendText("\r\n");
                        else
                            rtbOutput.AppendText("    ");
                    }
                }

                rtbOutput.AppendText("\r\n" + new string('=', 70) + "\r\n");
                rtbOutput.AppendText($"Файл сохранен: {txtOutputFile.Text}\r\n");

                MessageBox.Show($"Шифрование успешно завершено!\n\nОткрытый ключ для передачи: {openKey}\nМодуль r: {module}",
                                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при шифровании:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка ввода
                if (!BigInteger.TryParse(txtRDecrypt.Text, out BigInteger module))
                    throw new Exception("Модуль r должен быть целым положительным числом!");

                if (!BigInteger.TryParse(txtCloseKeyDecrypt.Text, out BigInteger closeKey))
                    throw new Exception("Закрытый ключ должен быть целым положительным числом!");

                if (string.IsNullOrEmpty(txtDecryptInputFile.Text) || !File.Exists(txtDecryptInputFile.Text))
                    throw new Exception("Укажите существующий зашифрованный файл!");

                if (string.IsNullOrEmpty(txtDecryptOutputFile.Text))
                    throw new Exception("Укажите путь для сохранения расшифрованного файла!");

                if (module <= 1)
                    throw new Exception("Модуль r должен быть больше 1!");

                // Расшифрование
                DecryptFile(txtDecryptInputFile.Text, txtDecryptOutputFile.Text, closeKey, module);

                rtbOutput.AppendText("\r\n" + new string('=', 70) + "\r\n");
                rtbOutput.AppendText("=== ИНФОРМАЦИЯ О РАСШИФРОВАНИИ ===\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText($"Зашифрованный файл: {System.IO.Path.GetFileName(txtDecryptInputFile.Text)}\r\n");
                rtbOutput.AppendText($"Расшифрованный файл: {System.IO.Path.GetFileName(txtDecryptOutputFile.Text)}\r\n");
                rtbOutput.AppendText($"Модуль r (n) = {module}\r\n");
                rtbOutput.AppendText($"Закрытый ключ KC = {closeKey}\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText($"Файл сохранен: {txtDecryptOutputFile.Text}\r\n");

                MessageBox.Show("Расшифрование успешно завершено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расшифровании:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}