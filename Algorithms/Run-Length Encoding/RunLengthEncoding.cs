public static class RunLengthEncoding {
    public static string Encode(string input) {
        if (string.IsNullOrEmpty(input)) return "";
        var result = new System.Text.StringBuilder();
        char currentChar = input[0];
        int count = 1;
        for (int i = 1; i < input.Length; i++) {
            if (input[i] == currentChar) {
                count++;
            } else {
                result.Append(currentChar).Append(count);
                currentChar = input[i];
                count = 1;
            }
        }
        result.Append(currentChar).Append(count);
        return result.ToString();
    }

    public static string Decode(string encodedInput) {
        if (string.IsNullOrEmpty(encodedInput)) return "";
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < encodedInput.Length; i++) {
            char currentChar = encodedInput[i];
            i++;
            int count = 0;
            while (i < encodedInput.Length && char.IsDigit(encodedInput[i])) {
                count = count * 10 + (encodedInput[i] - '0');
                i++;
            }
            result.Append(currentChar, count);
            i--;
        }
        return result.ToString();
    }
}