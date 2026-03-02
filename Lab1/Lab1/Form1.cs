using System.Text;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private TableLayoutPanel mainPanel;

        private GroupBox columnGroup;
        private TextBox columnInputText;
        private TextBox columnKeyText;
        private TextBox columnOutputText;
        private Button columnEncryptBtn;
        private Button columnDecryptBtn;
        private Button columnLoadFileBtn;
        private Button columnSaveFileBtn;

        private GroupBox vigenereGroup;
        private TextBox vigenereInputText;
        private TextBox vigenereKeyText;
        private TextBox vigenereOutputText;
        private Button vigenereEncryptBtn;
        private Button vigenereDecryptBtn;
        private Button vigenereLoadFileBtn;
        private Button vigenereSaveFileBtn;

        private ColumnCipher columnCipher = new ColumnCipher();
        private VigenereCipher vigenereCipher = new VigenereCipher();
        private Font buttonFont = new Font("Arial", 8);

        public Form1()
        {
            InitializeComponent();
            this.Text = "Shifrator";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.ColumnCount = 2;
            mainPanel.RowCount = 1;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            InitializeColumnPanel();

            InitializeVigenerePanel();

            this.Controls.Add(mainPanel);
        }

        private void InitializeColumnPanel()
        {
            columnGroup = new GroupBox();
            columnGroup.Text = "Столбцовый метод";
            columnGroup.Dock = DockStyle.Fill;
            columnGroup.Padding = new Padding(10);

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 7;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Заголовок
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));  // Входной текст
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Метка ключа
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Поле ключа
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Кнопки
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));  // Выходной текст
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Кнопки файлов

            var inputLabel = new Label();
            inputLabel.Text = "Входной текст:";
            inputLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(inputLabel, 0, 0);

            columnInputText = new TextBox();
            columnInputText.Multiline = true;
            columnInputText.ScrollBars = ScrollBars.Vertical;
            columnInputText.Dock = DockStyle.Fill;
            layout.Controls.Add(columnInputText, 0, 1);

            // Ключ
            var keyLabel = new Label();
            keyLabel.Text = "Ключ:";
            keyLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(keyLabel, 0, 2);

            columnKeyText = new TextBox();
            columnKeyText.Dock = DockStyle.Fill;
            layout.Controls.Add(columnKeyText, 0, 3);

            // Кнопки шифрования
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;

            columnEncryptBtn = new Button();
            columnEncryptBtn.Text = "Зашифровать";
            columnEncryptBtn.Font = buttonFont;

            columnEncryptBtn.Width = 120;
            columnEncryptBtn.Click += ColumnEncryptBtn_Click;

            columnDecryptBtn = new Button();
            columnDecryptBtn.Text = "Расшифровать";
            columnDecryptBtn.Font = buttonFont;

            columnDecryptBtn.Width = 120;
            columnDecryptBtn.Click += ColumnDecryptBtn_Click;

            buttonPanel.Controls.Add(columnEncryptBtn);
            buttonPanel.Controls.Add(columnDecryptBtn);
            layout.Controls.Add(buttonPanel, 0, 4);

            // Выходной текст
            var outputLabel = new Label();
            outputLabel.Text = "Выходной текст:";
            outputLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(outputLabel, 0, 5);

            columnOutputText = new TextBox();
            columnOutputText.Multiline = true;
            columnOutputText.ScrollBars = ScrollBars.Vertical;
            columnOutputText.ReadOnly = true;
            columnOutputText.Dock = DockStyle.Fill;
            layout.Controls.Add(columnOutputText, 0, 6);

            // Кнопки работы с файлами
            var fileButtonPanel = new FlowLayoutPanel();
            fileButtonPanel.Dock = DockStyle.Fill;
            fileButtonPanel.FlowDirection = FlowDirection.LeftToRight;

            columnLoadFileBtn = new Button();
            columnLoadFileBtn.Text = "Загрузить из файла";
            columnLoadFileBtn.Font = buttonFont;

            columnLoadFileBtn.Width = 150;
            columnLoadFileBtn.Click += ColumnLoadFileBtn_Click;

            columnSaveFileBtn = new Button();
            columnSaveFileBtn.Text = "Сохранить в файл";
            columnSaveFileBtn.Font = buttonFont;

            columnSaveFileBtn.Width = 150;
            columnSaveFileBtn.Click += ColumnSaveFileBtn_Click;

            fileButtonPanel.Controls.Add(columnLoadFileBtn);
            fileButtonPanel.Controls.Add(columnSaveFileBtn);
            layout.Controls.Add(fileButtonPanel, 0, 7);

            columnGroup.Controls.Add(layout);
            mainPanel.Controls.Add(columnGroup, 0, 0);
        }

        private void InitializeVigenerePanel()
        {
            vigenereGroup = new GroupBox();
            vigenereGroup.Text = "Шифр Виженера (самогенерирующийся ключ)";
            vigenereGroup.Dock = DockStyle.Fill;
            vigenereGroup.Padding = new Padding(10);

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 7;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Заголовок
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));  // Входной текст
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Метка ключа
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Поле ключа
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Кнопки
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); 
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));  // Выходной текст
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Кнопки файлов

            // Входной текст
            var inputLabel = new Label();
            inputLabel.Text = "Входной текст:";
            inputLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(inputLabel, 0, 0);

            vigenereInputText = new TextBox();
            vigenereInputText.Multiline = true;
            vigenereInputText.ScrollBars = ScrollBars.Vertical;
            vigenereInputText.Dock = DockStyle.Fill;
            layout.Controls.Add(vigenereInputText, 0, 1);

            // Ключ
            var keyLabel = new Label();
            keyLabel.Text = "Ключевое слово:";
            keyLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(keyLabel, 0, 2);

            vigenereKeyText = new TextBox();
            vigenereKeyText.Dock = DockStyle.Fill;
            layout.Controls.Add(vigenereKeyText, 0, 3);

            // Кнопки шифрования
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;

            vigenereEncryptBtn = new Button();
            vigenereEncryptBtn.Text = "Зашифровать";
            vigenereEncryptBtn.Font = buttonFont;
            vigenereEncryptBtn.Width = 120;
            vigenereEncryptBtn.Click += VigenereEncryptBtn_Click;

            vigenereDecryptBtn = new Button();
            vigenereDecryptBtn.Text = "Расшифровать";
            vigenereDecryptBtn.Font = buttonFont;
            vigenereDecryptBtn.Width = 120;
            vigenereDecryptBtn.Click += VigenereDecryptBtn_Click;

            buttonPanel.Controls.Add(vigenereEncryptBtn);
            buttonPanel.Controls.Add(vigenereDecryptBtn);
            layout.Controls.Add(buttonPanel, 0, 4);

            // Выходной текст
            var outputLabel = new Label();
            outputLabel.Text = "Выходной текст:";
            outputLabel.Dock = DockStyle.Fill;
            layout.Controls.Add(outputLabel, 0, 5);

            vigenereOutputText = new TextBox();
            vigenereOutputText.Multiline = true;
            vigenereOutputText.ScrollBars = ScrollBars.Vertical;
            vigenereOutputText.ReadOnly = true;
            vigenereOutputText.Dock = DockStyle.Fill;
            layout.Controls.Add(vigenereOutputText, 0, 6);

            // Кнопки работы с файлами
            var fileButtonPanel = new FlowLayoutPanel();
            fileButtonPanel.Dock = DockStyle.Fill;
            fileButtonPanel.FlowDirection = FlowDirection.LeftToRight;

            vigenereLoadFileBtn = new Button();
            vigenereLoadFileBtn.Text = "Загрузить из файла";
            vigenereLoadFileBtn.Font = buttonFont;
            vigenereLoadFileBtn.Width = 150;
            vigenereLoadFileBtn.Click += VigenereLoadFileBtn_Click;

            vigenereSaveFileBtn = new Button();
            vigenereSaveFileBtn.Text = "Сохранить в файл";
            vigenereSaveFileBtn.Font = buttonFont;
            vigenereSaveFileBtn.Width = 150;
            vigenereSaveFileBtn.Click += VigenereSaveFileBtn_Click;

            fileButtonPanel.Controls.Add(vigenereLoadFileBtn);
            fileButtonPanel.Controls.Add(vigenereSaveFileBtn);
            layout.Controls.Add(fileButtonPanel, 0, 7);

            vigenereGroup.Controls.Add(layout);
            mainPanel.Controls.Add(vigenereGroup, 1, 0);
        }

        private void ShowKeyError()
        {
            MessageBox.Show("Ключ должен содержать хотя бы одну букву русского алфавита!",
    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string ClearSpaces(string s)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ')
                {
                    builder.Append(s[i]);
                }
            }
            return builder.ToString();
        }

        private void ColumnEncryptBtn_Click(object sender, EventArgs e)
        {
            string filteredKey = columnCipher.FilterRussianText(columnKeyText.Text);
            if (string.IsNullOrEmpty(filteredKey))
            {
                ShowKeyError();
                return;
            }

            string text = columnInputText.Text; // ClearSpaces(columnInputText.Text);

            columnOutputText.Text = columnCipher.Encrypt(text, columnKeyText.Text);
        }

        private void ColumnDecryptBtn_Click(object sender, EventArgs e)
        {
            string filteredKey = columnCipher.FilterRussianText(columnKeyText.Text);
            if (string.IsNullOrEmpty(filteredKey))
            {
                ShowKeyError();
                return;
            }
            string text = columnInputText.Text; //ClearSpaces(columnInputText.Text);

            columnOutputText.Text = columnCipher.Decrypt(text, columnKeyText.Text);
        }

        private void ColumnLoadFileBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        columnInputText.Text = content;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ColumnSaveFileBtn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, columnOutputText.Text, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void VigenereEncryptBtn_Click(object sender, EventArgs e)
        {
            string filteredKey = vigenereCipher.FilterRussianText(vigenereKeyText.Text);
            if (string.IsNullOrEmpty(filteredKey))
            {
                ShowKeyError();
                return;
            }
            string text = vigenereInputText.Text; // ClearSpaces(vigenereInputText.Text);

            vigenereOutputText.Text = vigenereCipher.Encrypt(text, vigenereKeyText.Text);
        }

        private void VigenereDecryptBtn_Click(object sender, EventArgs e)
        {
            string filteredKey = vigenereCipher.FilterRussianText(vigenereKeyText.Text);
            if (string.IsNullOrEmpty(filteredKey))
            {
                ShowKeyError();
                return;
            }
            string text = vigenereInputText.Text; // ClearSpaces(vigenereInputText.Text);

            vigenereOutputText.Text = vigenereCipher.Decrypt(text, vigenereKeyText.Text);
        }

        private void VigenereLoadFileBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        vigenereInputText.Text = content;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void VigenereSaveFileBtn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, vigenereOutputText.Text, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
