using System.Text;

public class VigenereCipher : Cipher
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

        string generatedKey = GenerateKeyForEncrypt(cleanKey, russianText);

        StringBuilder encryptedRussian = new StringBuilder();
        int keyIndex = 0;

        for (int i = 0; i < russianText.Length; i++)
        {
            char currentChar = russianText[i];
            char keyChar = generatedKey[keyIndex];
            keyIndex++;

            bool isUpper = RussianAlphabet.Contains(currentChar);
            int textIndex = GetCharIndex(currentChar);
            int keyCharIndex = GetCharIndex(keyChar);

            int encryptedIndex = (textIndex + keyCharIndex) % RussianAlphabet.Length;
            encryptedRussian.Append(GetCharByIndex(encryptedIndex, isUpper));
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
        string filteredKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(text))
            return text;

        if (string.IsNullOrEmpty(filteredKey))
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


        List<char> keyChars = new List<char>();
        foreach (char k in filteredKey)
        {
            keyChars.Add(k);
        }

        StringBuilder decryptedRussian = new StringBuilder();
        int keyIndex = 0;

        for (int i = 0; i < russianText.Length; i++)
        {
            char currentChar = russianText[i];
            char keyChar = keyChars[keyIndex];
            keyIndex++;

            bool isUpper = RussianAlphabet.Contains(currentChar);
            int textIndex = GetCharIndex(currentChar);
            int keyCharIndex = GetCharIndex(keyChar);

            int decryptedIndex = (textIndex - keyCharIndex + RussianAlphabet.Length) % RussianAlphabet.Length;
            char decryptedChar = GetCharByIndex(decryptedIndex, isUpper);
            decryptedRussian.Append(decryptedChar);

            keyChars.Add(decryptedChar);
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

    private string GenerateKeyForEncrypt(string key, string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        StringBuilder generatedKey = new StringBuilder();

        generatedKey.Append(key);

        int keyLength = key.Length;
        for (int i = 0; i < plainText.Length - key.Length; i++)
        {
            generatedKey.Append(plainText[i]);
        }

        return generatedKey.ToString();
    }
}