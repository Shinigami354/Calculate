using System.Reflection;
using Moq;

namespace Calculator.Tests;

public class ProgramTests
{
    [Fact]
    public void Main_WhenMaximumMenuAttemptsReached_ReturnsBeforeCalculation()
    {
        string output = ConsoleTestHelper.Run("bad\nbad\nbad\nbad\nbad\n", () => Program.Main([]));

        Assert.Contains("Calculator", output);
        Assert.Contains("Maximum number of attempts has been reached", output);
    }

    [Fact]
    public void Main_WithValidOperationAndNoTryAgain_ReturnsAfterOneCalculation()
    {
        string output = ConsoleTestHelper.Run("1\n2\n3\nN\n", () => Program.Main([]));

        Assert.Contains("2 + 3 = 5", output);
        Assert.Contains("Try Again", output);
    }

    [Fact]
    public void Main_WithYesTryAgain_ContinuesToNextMenu()
    {
        string output = ConsoleTestHelper.Run(
            "2\n7\n4\nY\nbad\nbad\nbad\nbad\nbad\n",
            () => Program.Main([]));

        Assert.Contains("7 - 4 = 3", output);
        Assert.Contains("Maximum number of attempts has been reached", output);
    }

    [Theory]
    [InlineData(1, "2\n3\n", "Addition", 5)]
    [InlineData(2, "7\n4\n", "Subtraction", 3)]
    [InlineData(3, "6\n5\n", "Multiplication", 30)]
    [InlineData(4, "9\n2\n", "Division", 4.5)]
    [InlineData(5, "2\n3\n", "Exponentiation", 8)]
    public void CallCalculate_WithKnownOperation_RunsSelectedCalculationAndLogsResult(
        int operation,
        string input,
        string expectedTitle,
        double expectedResult)
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = ConsoleTestHelper.Run(input, () => InvokeCallCalculate(operation, calculator));

        Assert.Contains(expectedTitle, output);
        loggerMock.Verify(
            logger => logger.Log(
                expectedTitle,
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.Is<double>(actual => Math.Abs(actual - expectedResult) < 0.000001)),
            Times.Once);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void CallCalculate_WithUnknownOperation_DoesNothing()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = ConsoleTestHelper.Run("", () => InvokeCallCalculate(99, calculator));

        Assert.Equal(string.Empty, output);
        loggerMock.VerifyNoOtherCalls();
    }

    private static void InvokeCallCalculate(int operation, Calculate calculator)
    {
        MethodInfo? method = typeof(Program).GetMethod(
            "CallCalculate",
            BindingFlags.Static | BindingFlags.NonPublic);

        Assert.NotNull(method);
        method.Invoke(null, [operation, calculator]);
    }
}
