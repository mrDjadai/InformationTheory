using System.Text;

public class ColumnCipher : Cipher
{
    public override string Encrypt(string text, string key)
    {
        string filteredText = FilterRussianText(text);
        string cleanKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(filteredText))
            return text;

        if (string.IsNullOrEmpty(cleanKey))
            return text;


        string upperKey = cleanKey.ToUpper();
        var keyOrder = GetKeyOrder(upperKey);

        int cols = upperKey.Length;
        int rows = (int)Math.Ceiling((double)filteredText.Length / cols);
        char[,] table = new char[rows, cols];

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (index < filteredText.Length)
                    table[i, j] = filteredText[index++];
                else
                    table[i, j] = ' ';
            }
        }

        StringBuilder result = new StringBuilder();
        for (int k = 0; k < cols; k++)
        {
            int col = keyOrder[k];
            for (int i = 0; i < rows; i++)
            {
                if (table[i, col] != ' ')
                    result.Append(table[i, col]);
            }
        }

        return result.ToString();
    }

    public override string Decrypt(string text, string key)
    {
        string filteredText = FilterRussianText(text);
        string cleanKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(filteredText))
            return text;

        if (string.IsNullOrEmpty(cleanKey))
            return text;

        string upperKey = cleanKey.ToUpper();
        var keyOrder = GetKeyOrder(upperKey);

        int cols = upperKey.Length;
        int rows = (int)Math.Ceiling((double)filteredText.Length / cols);
        int totalCells = rows * cols;

        char[] decrypted = new char[filteredText.Length];
        int[] colLengths = new int[cols];

        int fullCols = filteredText.Length % cols;
        int baseLength = filteredText.Length / cols;

        for (int i = 0; i < cols; i++)
        {
            colLengths[i] = i < fullCols ? baseLength + 1 : baseLength;
        }

        char[,] table = new char[rows, cols];
        int textIndex = 0;

        for (int k = 0; k < cols; k++)
        {
            int col = keyOrder[k];
            for (int i = 0; i < colLengths[col]; i++)
            {
                if (textIndex < filteredText.Length)
                {
                    table[i, col] = filteredText[textIndex++];
                }
            }
        }

        StringBuilder result = new StringBuilder();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (table[i, j] != '\0')
                    result.Append(table[i, j]);
            }
        }

        return result.ToString();
    }

    private int[] GetKeyOrder(string key)
    {
        var keyChars = key.ToCharArray();
        var sorted = key.OrderBy(c => c).ToList();
        var order = new int[key.Length];

        bool[] used = new bool[key.Length];

        for (int i = 0; i < key.Length; i++)
        {
            char currentChar = sorted[i];
            for (int j = 0; j < key.Length; j++)
            {
                if (!used[j] && key[j] == currentChar)
                {
                    order[i] = j;
                    used[j] = true;
                    break;
                }
            }
        }

        return order;
    }
}