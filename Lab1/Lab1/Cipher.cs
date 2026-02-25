using System.Text;

public abstract class Cipher
{
    protected const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    protected const string RussianAlphabetLower = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    public abstract string Encrypt(string text, string key);
    public abstract string Decrypt(string text, string key);

    public string FilterRussianText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var result = new StringBuilder();
        foreach (char c in text)
        {
            if (RussianAlphabet.Contains(c) || RussianAlphabetLower.Contains(c))
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }

    protected int GetCharIndex(char c)
    {
        if (RussianAlphabet.Contains(c))
            return RussianAlphabet.IndexOf(c);
        if (RussianAlphabetLower.Contains(c))
            return RussianAlphabetLower.IndexOf(c);
        return -1;
    }

    protected char GetCharByIndex(int index, bool isUpper)
    {
        if (isUpper)
            return RussianAlphabet[index % RussianAlphabet.Length];
        else
            return RussianAlphabetLower[index % RussianAlphabetLower.Length];
    }
}