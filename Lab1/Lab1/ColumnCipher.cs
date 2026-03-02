using System.Text;

public class ColumnCipher : Cipher
{
    public override string Encrypt(string text, string key)
    {
        string cleanKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(text))
            return text;

        if (string.IsNullOrEmpty(cleanKey))
            return text;

        List<char> russianChars = new List<char>();
        int[] russianPositions = new int[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            if (IsRussianChar(text[i]))
            {
                russianPositions[i] = russianChars.Count;
                russianChars.Add(text[i]);
            }
            else
            {
                russianPositions[i] = -1;
            }
        }

        if (russianChars.Count == 0)
            return text;

        string russianText = new string(russianChars.ToArray());
        string upperKey = cleanKey.ToUpper();
        var keyOrder = GetKeyOrder(upperKey);

        int cols = upperKey.Length;
        int rows = (int)Math.Ceiling((double)russianText.Length / cols);
        char[,] table = new char[rows, cols];

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (index < russianText.Length)
                    table[i, j] = russianText[index++];
                else
                    table[i, j] = '\0';
            }
        }

        StringBuilder encryptedRussian = new StringBuilder();
        for (int k = 0; k < cols; k++)
        {
            int col = keyOrder[k];
            for (int i = 0; i < rows; i++)
            {
                if (table[i, col] != '\0')
                    encryptedRussian.Append(table[i, col]);
            }
        }

        StringBuilder result = new StringBuilder(text);
        int russianIndex = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (russianPositions[i] != -1)
            {
                result[i] = encryptedRussian[russianIndex++];
            }
        }

        return result.ToString();
    }

    public override string Decrypt(string text, string key)
    {
        string cleanKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(text))
            return text;

        if (string.IsNullOrEmpty(cleanKey))
            return text;

        List<char> russianChars = new List<char>();
        int[] russianPositions = new int[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            if (IsRussianChar(text[i]))
            {
                russianPositions[i] = russianChars.Count;
                russianChars.Add(text[i]);
            }
            else
            {
                russianPositions[i] = -1;
            }
        }
        if (russianChars.Count == 0)
            return text;

        string russianText = new string(russianChars.ToArray());
        string upperKey = cleanKey.ToUpper();
        var keyOrder = GetKeyOrder(upperKey);

        int cols = upperKey.Length;
        int rows = (int)Math.Ceiling((double)russianText.Length / cols);
        int totalCells = rows * cols;

        int[] colLengths = new int[cols];

        int fullCols = russianText.Length % cols;
        int baseLength = russianText.Length / cols;

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
                if (textIndex < russianText.Length)
                {
                    table[i, col] = russianText[textIndex++];
                }
            }
        }

        StringBuilder decryptedRussian = new StringBuilder();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (table[i, j] != '\0')
                    decryptedRussian.Append(table[i, j]);
            }
        }

        StringBuilder result = new StringBuilder(text);
        int russianIndex = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (russianPositions[i] != -1)
            {
                result[i] = decryptedRussian[russianIndex++];
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