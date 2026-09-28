namespace Calculator;

/// <summary>ANSI escape codes for console coloring</summary>
/// <remarks>
/// <para>Author: Eddie C.</para>
/// <para>Version: 1.0</para>
/// <para>Since: 2025-11-09</para>
/// </remarks>

internal static class AnsiColorCodes
{
    /// <summary>Reset colors to default</summary>
    public const string Reset = "\e[0m";

    /// <summary> Red font and white background for error messages</summary>
    public const string Error = "\e[48;2;255;255;255;38;2;255;0;0m";

    /// <summary>Green font and white background for results </summary>
    public const string Result = "\e[48;2;255;255;255;38;2;0;128;0m";

    /// <summary> Light blue background color </summary>
    public const string Background = "\e[48;2;26;132;184m";

    /// <summary> White foreground color </summary>
    public const string Foreground = "\e[37m";
}