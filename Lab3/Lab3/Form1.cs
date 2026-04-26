namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void TxtP_Q_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtP.Text, out int p) &&
                int.TryParse(txtQ.Text, out int q) &&
                p > 0 && q > 0)
            {
                int r = p * q;
                int phi = (p - 1) * (q - 1);
                txtR.Text = r.ToString();
                txtEuler.Text = phi.ToString();

                // Если введен закрытый ключ, вычисляем открытый
                if (int.TryParse(txtCloseKeyInput.Text, out int closeKey) && closeKey > 0)
                {
                    try
                    {
                        int openKey = ComputeOpenKey(closeKey, phi);
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

        private void TxtCloseKeyInput_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtP.Text, out int p) &&
                int.TryParse(txtQ.Text, out int q) &&
                int.TryParse(txtCloseKeyInput.Text, out int closeKey) &&
                p > 0 && q > 0 && closeKey > 0)
            {
                int phi = (p - 1) * (q - 1);
                try
                {
                    int openKey = ComputeOpenKey(closeKey, phi);
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

        private int FastPowMod(int a, int b, int m)
        {
            if (m == 1) return 0;
            long result = 1;
            long aLong = a % m;
            long bLong = b;
            long mLong = m;

            while (bLong > 0)
            {
                if ((bLong & 1) == 1)
                    result = (result * aLong) % mLong;
                aLong = (aLong * aLong) % mLong;
                bLong >>= 1;
            }
            return (int)result;
        }

        private int ExtendedGCD(int a, int b, out int x, out int y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            int gcd = ExtendedGCD(b, a % b, out x, out y);
            int temp = x;
            x = y;
            y = temp - (a / b) * y;
            return gcd;
        }

        private bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        private int EulerFunction(int p, int q)
        {
            return (p - 1) * (q - 1);
        }


        private int ComputeOpenKey(int d, int phi)
        {
            int x, y;
            int gcd = ExtendedGCD(d, phi, out x, out y);
            if (gcd != 1)
                throw new Exception("Закрытый ключ и φ(n) не взаимно просты!");
            int e = (x % phi + phi) % phi;
            return e;
        }

        private byte[] GetBytesFromInt(int value, int byteCount)
        {
            byte[] bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                bytes[byteCount - 1 - i] = (byte)((value >> (i * 8)) & 0xFF);
            }
            return bytes;
        }

        private int GetIntFromBytes(byte[] bytes, int startIndex)
        {
            return (bytes[startIndex] << 8) | bytes[startIndex + 1];
        }

        private void EncryptFile(string inputFile, string outputFile, int openKey, int module)
        {
            byte[] fileBytes = File.ReadAllBytes(inputFile);

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (byte b in fileBytes)
                {
                    int encrypted = FastPowMod(b, openKey, module);
                    byte[] encryptedBytes16 = GetBytesFromInt(encrypted, 2);
                    ms.Write(encryptedBytes16, 0, encryptedBytes16.Length);
                }

                File.WriteAllBytes(outputFile, ms.ToArray());
            }
        }

        private void DecryptFile(string inputFile, string outputFile, int closeKey, int module)
        {
            byte[] encryptedData = File.ReadAllBytes(inputFile);

            if (encryptedData.Length % 2 != 0)
                throw new Exception("Зашифрованный файл поврежден: размер не кратен 2 байтам!");

            using (MemoryStream ms = new MemoryStream())
            {
                for (int i = 0; i < encryptedData.Length; i += 2)
                {
                    int encryptedValue = GetIntFromBytes(encryptedData, i);
                    int decrypted = FastPowMod(encryptedValue, closeKey, module);
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
                if (!int.TryParse(txtP.Text, out int p))
                    throw new Exception("p должно быть целым положительным числом!");

                if (!int.TryParse(txtQ.Text, out int q))
                    throw new Exception("q должно быть целым положительным числом!");

                if (!int.TryParse(txtCloseKeyInput.Text, out int closeKey))
                    throw new Exception("Закрытый ключ должен быть целым положительным числом!");

                if (string.IsNullOrEmpty(txtInputFile.Text) || !File.Exists(txtInputFile.Text))
                    throw new Exception("Укажите существующий входной файл!");

                if (string.IsNullOrEmpty(txtOutputFile.Text))
                    throw new Exception("Укажите путь для сохранения зашифрованного файла!");

                if (!IsPrime(p))
                    throw new Exception($"p = {p} не является простым числом!");

                if (!IsPrime(q))
                    throw new Exception($"q = {q} не является простым числом!");

                if (p == q)
                    throw new Exception("p и q должны быть различными!");

                int module = p * q;

                int phi = EulerFunction(p, q);

                int openKey = ComputeOpenKey(closeKey, phi);

                if (openKey >= phi)
                    throw new Exception($"Вычисленный открытый ключ = {openKey} больше или равен φ(n) = {phi}!");

                byte[] originalBytes = File.ReadAllBytes(txtInputFile.Text);

                EncryptFile(txtInputFile.Text, txtOutputFile.Text, openKey, module);

                byte[] encryptedBytes = File.ReadAllBytes(txtOutputFile.Text);

                rtbOutput.AppendText("=== ИНФОРМАЦИЯ О ШИФРОВАНИИ ===\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText($"Исходный файл: {Path.GetFileName(txtInputFile.Text)}\r\n");
                rtbOutput.AppendText($"Зашифрованный файл: {Path.GetFileName(txtOutputFile.Text)}\r\n");
                rtbOutput.AppendText($"p = {p}\r\n");
                rtbOutput.AppendText($"q = {q}\r\n");
                rtbOutput.AppendText($"Модуль r (n) = {module}\r\n");
                rtbOutput.AppendText($"φ(n) = {phi}\r\n");
                rtbOutput.AppendText($"Закрытый ключ KC (введен) = {closeKey}\r\n");
                rtbOutput.AppendText($"Открытый ключ KC (вычислен) = {openKey}\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText("ПАРЫ (исходный байт -> зашифрованное значение в 10-й системе):\r\n");
                rtbOutput.AppendText(new string('=', 70) + "\r\n");

                //пары исходный-зашифрованный
                for (int i = 0; i < originalBytes.Length; i++)
                {
                    if (i * 2 + 1 < encryptedBytes.Length)
                    {
                        int encryptedValue = GetIntFromBytes(encryptedBytes, i * 2);
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
                if (!int.TryParse(txtRDecrypt.Text, out int module))
                    throw new Exception("Модуль r должен быть целым положительным числом!");

                if (!int.TryParse(txtCloseKeyDecrypt.Text, out int closeKey))
                    throw new Exception("Закрытый ключ должен быть целым положительным числом!");

                if (string.IsNullOrEmpty(txtDecryptInputFile.Text) || !File.Exists(txtDecryptInputFile.Text))
                    throw new Exception("Укажите существующий зашифрованный файл!");

                if (string.IsNullOrEmpty(txtDecryptOutputFile.Text))
                    throw new Exception("Укажите путь для сохранения расшифрованного файла!");

                if (module <= 1)
                    throw new Exception("Модуль r должен быть больше 1!");

                DecryptFile(txtDecryptInputFile.Text, txtDecryptOutputFile.Text, closeKey, module);

                rtbOutput.AppendText("\r\n" + new string('=', 70) + "\r\n");
                rtbOutput.AppendText("=== ИНФОРМАЦИЯ О РАСШИФРОВАНИИ ===\r\n");
                rtbOutput.AppendText(new string('-', 70) + "\r\n");
                rtbOutput.AppendText($"Зашифрованный файл: {Path.GetFileName(txtDecryptInputFile.Text)}\r\n");
                rtbOutput.AppendText($"Расшифрованный файл: {Path.GetFileName(txtDecryptOutputFile.Text)}\r\n");
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