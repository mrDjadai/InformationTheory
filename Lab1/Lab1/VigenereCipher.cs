using System.Text;

public class VigenereCipher : Cipher
{
    public override string Encrypt(string text, string key)
    {
        string filteredText = FilterRussianText(text);
        string cleanKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(filteredText))
            return text;

        if (string.IsNullOrEmpty(cleanKey))
            return text;

        StringBuilder result = new StringBuilder();

        string generatedKey = GenerateKeyForEncrypt(cleanKey, filteredText);

        for (int i = 0; i < filteredText.Length; i++)
        {
            char currentChar = filteredText[i];
            char keyChar = generatedKey[i];

            bool isUpper = RussianAlphabet.Contains(currentChar);
            int textIndex = GetCharIndex(currentChar);
            int keyIndex = GetCharIndex(keyChar);

            int encryptedIndex = (textIndex + keyIndex) % RussianAlphabet.Length;
            result.Append(GetCharByIndex(encryptedIndex, isUpper));
        }

        return result.ToString();
    }

    public override string Decrypt(string text, string key)
    {
        string filteredText = FilterRussianText(text);
        string filteredKey = FilterRussianText(key);

        if (string.IsNullOrEmpty(filteredText))
            return text;

        if (string.IsNullOrEmpty(filteredKey))
            return text;

        StringBuilder result = new StringBuilder();

        string generatedKey = GenerateKeyForDecrypt(filteredKey, filteredText, result);

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

    private string GenerateKeyForDecrypt(string key, string cipherText, StringBuilder result)
    {
        StringBuilder generatedKey = new StringBuilder();

        generatedKey.Append(key);

        for (int i = 0; i < Math.Min(key.Length, cipherText.Length); i++)
        {
            char cipherChar = cipherText[i];
            char keyChar = generatedKey[i];

            bool isUpper = RussianAlphabet.Contains(cipherChar);
            int cipherIndex = GetCharIndex(cipherChar);
            int keyIndex = GetCharIndex(keyChar);

            int decryptedIndex = (cipherIndex - keyIndex + RussianAlphabet.Length) % RussianAlphabet.Length;
            char decryptedChar = GetCharByIndex(decryptedIndex, isUpper);

            result.Append(decryptedChar);
        }

        // Ключ из текста
        for (int i = key.Length; i < cipherText.Length; i++)
        {
            generatedKey.Append(result[i - key.Length]);

            char cipherChar = cipherText[i];
            char keyChar = generatedKey[i];

            bool isUpper = RussianAlphabet.Contains(cipherChar);
            int cipherIndex = GetCharIndex(cipherChar);
            int keyIndex = GetCharIndex(keyChar);

            int decryptedIndex = (cipherIndex - keyIndex + RussianAlphabet.Length) % RussianAlphabet.Length;
            char decryptedChar = GetCharByIndex(decryptedIndex, isUpper);

            result.Append(decryptedChar);
        }

        return generatedKey.ToString();
    }
}