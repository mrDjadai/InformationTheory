using System.Text;

public static class LFSR
{
    private const int registerLength = 36;

    private static bool[] register;

    public static bool Initialize(string initialState)
    {
        if (initialState.Length != registerLength)
            return false;

        register = new bool[registerLength];
        for (int i = 0; i < registerLength; i++)
        {
            register[registerLength - 1 -i] = initialState[i] == '1';
        }
        return true;
    }

    private static int NextKeyBit()
    {
        int outputBit = register[registerLength - 1] ? 1 : 0;

        bool feedbackBit = register[35] ^ register[10];

        for (int i = registerLength - 1; i > 0; i--)
        {
            register[i] = register[i - 1];
        }

        register[0] = feedbackBit;

        return outputBit;
    }

    public static bool GenerateKeyStream(int length, out string result)
    {
        result = "";
        if (register == null)
            return false;

        StringBuilder keyStream = new StringBuilder(length + (length / 8));
        for (int i = 0; i < length; i++)
        {
            keyStream.Append(NextKeyBit());

            if ((i + 1) % 8 == 0 && i != length - 1)
            {
                keyStream.Append(' ');
            }
        }

        result = keyStream.ToString();
        return true;
    }

    public static bool ProcessData(byte[] inputData, out byte[] result)
    {
        result = null;
        if (register == null)
            return false;

        bool[] savedRegister = (bool[])register.Clone();

        byte[] outputData = new byte[inputData.Length];

        for (int i = 0; i < inputData.Length; i++)
        {
            byte inputByte = inputData[i];
            byte outputByte = 0;

            for (int bitPos = 0; bitPos < 8; bitPos++)
            {
                int keyBit = NextKeyBit();

                int inputBit = (inputByte >> (7 - bitPos)) & 1;

                int resultBit = inputBit ^ keyBit;

                outputByte = (byte)((outputByte << 1) | (byte)resultBit);
            }

            outputData[i] = outputByte;
        }

        register = savedRegister;

        result = outputData;
        return true;
    }

    public static string ByteToBinaryString(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }

    public static string BytesToBinaryString(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(ByteToBinaryString(bytes[i]));
            if (i < bytes.Length - 1)
                sb.Append(" ");
        }


        return sb.ToString();
    }
}