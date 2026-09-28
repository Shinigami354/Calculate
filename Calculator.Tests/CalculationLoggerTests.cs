using System.Diagnostics;

namespace Calculator.Tests;

public class CalculationLoggerTests
{
    [Fact]
    public void Log_WritesCalculationToTrace()
    {
        using StringWriter writer = new();
        using TextWriterTraceListener listener = new(writer);
        bool originalAutoFlush = Trace.AutoFlush;

        try
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(listener);

            new CalculationLogger().Log("Addition", 2, 3, 5);
            Trace.Flush();

            Assert.Contains("Addition: 2 and 3 = 5", writer.ToString());
        }
        finally
        {
            Trace.Listeners.Remove(listener);
            Trace.AutoFlush = originalAutoFlush;
        }
    }
}
