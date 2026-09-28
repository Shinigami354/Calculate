using System.Globalization;

namespace Calculator.Tests;

internal static class ConsoleTestHelper
{
    private static readonly object ConsoleLock = new();

    public static string Run(string input, Action action)
    {
        return Run(new StringReader(input), action);
    }

    public static string Run(TextReader reader, Action action)
    {
        lock (ConsoleLock)
        {
            TextReader originalIn = Console.In;
            TextWriter originalOut = Console.Out;
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
            CultureInfo originalUiCulture = CultureInfo.CurrentUICulture;

            using StringWriter writer = new(CultureInfo.InvariantCulture);

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
                Console.SetIn(reader);
                Console.SetOut(writer);

                action();

                return writer.ToString();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUiCulture;
                reader.Dispose();
            }
        }
    }
}
