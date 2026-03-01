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

        StringBuilder result = new StringBuilder();

        string generatedKey = GenerateKeyForEncrypt(cleanKey, text);
        int keyIndex = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            bool isRussianChar = IsRussianChar(currentChar);

            if (!isRussianChar)
            {
                result.Append(currentChar);
            }
            else
            {
                char keyChar = generatedKey[keyIndex];
                keyIndex++;

                bool isUpper = RussianAlphabet.Contains(currentChar);
                int textIndex = GetCharIndex(currentChar);
                int keyCharIndex = GetCharIndex(keyChar);

                int encryptedIndex = (textIndex + keyCharIndex) % RussianAlphabet.Length;
                result.Append(GetCharByIndex(encryptedIndex, isUpper));
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

        StringBuilder result = new StringBuilder();

        // Собираем ключевые символы по мере обработки
        List<char> keyChars = new List<char>();
        foreach (char k in filteredKey)
        {
            keyChars.Add(k);
        }

        int keyIndex = 0;
        int russianCharCount = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            bool isRussianChar = IsRussianChar(currentChar);

            if (!isRussianChar)
            {
                result.Append(currentChar);
            }
            else
            {
                char keyChar = keyChars[keyIndex];

                bool isUpper = RussianAlphabet.Contains(currentChar);
                int textIndex = GetCharIndex(currentChar);
                int keyCharIndex = GetCharIndex(keyChar);

                int decryptedIndex = (textIndex - keyCharIndex + RussianAlphabet.Length) % RussianAlphabet.Length;
                char decryptedChar = GetCharByIndex(decryptedIndex, isUpper);
                result.Append(decryptedChar);

                keyChars.Add(decryptedChar);

                keyIndex++;
                russianCharCount++;
            }
        }

        return result.ToString();
    }

    private string GenerateKeyForEncrypt(string key, string plainText)
    {
        StringBuilder generatedKey = new StringBuilder();

        generatedKey.Append(key);

        int keyLength = key.Length;
        for (int i = 0; i < plainText.Length - key.Length; i++)
        {
            if (i < plainText.Length)
            {
                generatedKey.Append(plainText[i]);
            }
        }

        return generatedKey.ToString().Substring(0, plainText.Length);
    }

}