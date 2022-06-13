using System;
using System.Globalization;

public static class ArgumentsExtensions {
    /// <summary>
    /// Gets a value indicating whether an argument was specified within the input text.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter to compare.</param>
    /// <param name="argument">A string value to compare to.</param>
    /// <returns><see langword="true"/> if the input argument was specified; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, string argument) {
        if (index >= 0 && index < input.Length)
            return string.Equals(input[index].Trim(), argument, StringComparison.OrdinalIgnoreCase);

        return false;
    }
    
    /// <summary>
    /// Gets a value indicating whether an argument was specified within the input text.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="argument">A string value to search for.</param>
    /// <returns><see langword="true"/> if the input argument was specified; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, string argument) {
        return GetArgument(input, argument, out _);
    }
    
    /// <summary>
    /// Gets a value indicating whether an argument was specified within the input text.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="argument">A string value to search for.</param>
    /// <param name="index">Set to index of the string argument.</param>
    /// <returns><see langword="true"/> if the input argument was specified; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, string argument, out int index) {
        for (index = 0; index < input.Length; index++) {
            if (string.Equals(input[index].Trim(), argument, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        index = -1;
        return false;
    }

    #region Integral types
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.SByte"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.SByte"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out sbyte value, sbyte defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (sbyte.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;

        Default:
        value = defaultValue;
        return false;
    }

    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Byte"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.Byte"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out byte value, byte defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (byte.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;

        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Int16"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to an <see cref="T:System.Int16"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out short value, short defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (short.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.UInt16"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.UInt16"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out ushort value, ushort defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (ushort.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Int32"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to an <see cref="T:System.Int32"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out int value, int defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (int.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.UInt32"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.UInt32"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out uint value, uint defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (uint.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Int64"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to an <see cref="T:System.Int64"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out long value, long defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (long.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }
    
    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.UInt64"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <param name="style">A number style to use when parsing the parameter.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.UInt64"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out ulong value, ulong defaultValue = default, NumberStyles style = NumberStyles.Integer) {
        if (index < 0 || index >= input.Length)
            goto Default;

        var argument = input[index];
        if (argument.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
            argument = argument.Substring(2);
            style = NumberStyles.HexNumber;
        }

        if (ulong.TryParse(argument, style, NumberFormatInfo.CurrentInfo, out value))
            return true;
        
        Default:
        value = defaultValue;
        return false;
    }

    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Single"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.Single"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out float value, float defaultValue = default) {
        if (index >= 0 && index < input.Length && float.TryParse(input[index], out value))
            return true;

        value = defaultValue;
        return false;
    }

    /// <summary>
    /// Attempts to parse and convert user input text into a strongly typed <see cref="T:System.Double"/> value.
    /// </summary>
    /// <param name="input">An array of input arguments.</param>
    /// <param name="index">The index of the parameter.</param>
    /// <param name="value">The output value which will be successfully parsed from a string; otherwise <paramref name="defaultValue"/>.</param>
    /// <param name="defaultValue">A default value to assign to <paramref name="value"/> if the index was out of range or if the provided text could not be parsed.</param>
    /// <returns><see langword="true"/> if the input argument at the specified index was successfully converted to a <see cref="T:System.Double"/>; otherwise <see langword="false"/>.</returns>
    public static bool GetArgument(this string[] input, int index, out double value, double defaultValue = default) {
        if (index >= 0 && index < input.Length && double.TryParse(input[index], out value))
            return true;

        value = defaultValue;
        return false;
    }
    #endregion
}
