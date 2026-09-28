using System.Globalization;
using System.Reflection;
using Moq;

namespace Calculator.Tests;

public class CalculateTests
{
    private static readonly object ConsoleLock = new();

    [Fact]
    public void Multiply_TwoNumbers_ReturnsCorrectResult()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        double result = calculator.Multiply(2, 3);

        Assert.Equal(6, result);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Calculate(null!));
    }

    [Fact]
    public void Add_WithValidInput_PrintsResultAndLogsCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("2\n3\n", calculator.Add);

        Assert.Contains("Addition", output);
        Assert.Contains("2 + 3 = 5", output);
        VerifyLog(loggerMock, "Addition", 2, 3, 5);
    }

    [Fact]
    public void Subtract_WithValidInput_PrintsResultAndLogsCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("7\n4\n", calculator.Subtract);

        Assert.Contains("Subtraction", output);
        Assert.Contains("7 - 4 = 3", output);
        VerifyLog(loggerMock, "Subtraction", 7, 4, 3);
    }

    [Fact]
    public void MultiplyOperation_WithValidInput_PrintsResultAndLogsCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("6\n5\n", calculator.Multiply);

        Assert.Contains("Multiplication", output);
        Assert.Contains("6 * 5 = 30", output);
        VerifyLog(loggerMock, "Multiplication", 6, 5, 30);
    }

    [Fact]
    public void Divide_WithValidInput_PrintsResultAndLogsCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("9\n2\n", calculator.Divide);

        Assert.Contains("Division", output);
        Assert.Contains("9 / 2 = 4.5000", output);
        VerifyLog(loggerMock, "Division", 9, 2, 4.5);
    }

    [Fact]
    public void Divide_ByZero_PrintsErrorAndDoesNotLogCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("9\n0\n", calculator.Divide);

        Assert.Contains("Value for b (divisor) should not be zero", output);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Divide_WithInvalidInput_PrintsInputErrorAndDoesNotLogCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("abc\n2\n", calculator.Divide);

        Assert.Contains("Unable to perform operation", output);
        Assert.Contains("`abc` and `2`", output);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Add_WithInvalidInput_PrintsInputErrorAndDoesNotLogCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("abc\n2\n", calculator.Add);

        Assert.Contains("Unable to perform operation", output);
        Assert.Contains("`abc` and `2`", output);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Add_WithMissingConsoleInput_UsesFallbackValuesAndPrintsInputError()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("", calculator.Add);

        Assert.Contains("Unable to perform operation", output);
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Power_WithPositiveIntegerOperands_UsesMultiplyLoopAndLogsCalculation()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput("2\n3\n", calculator.Power);

        Assert.Contains("Exponentiation", output);
        Assert.Contains("2 ^ 3 = 8", output);
        VerifyLog(loggerMock, "Exponentiation", 2, 3, 8);
    }

    [Theory]
    [InlineData("1.5\n2\n", 1.5, 2, 2.25, "1.5 ^ 2 = 2.2500")]
    [InlineData("-2\n3\n", -2, 3, -8, "-2 ^ 3 = -8")]
    [InlineData("4\n0.5\n", 4, 0.5, 2, "4 ^ 0.5 = 2")]
    [InlineData("2\n0\n", 2, 0, 1, "2 ^ 0 = 1")]
    public void Power_WhenIntegerPositiveConditionIsFalse_UsesMathPowAndLogsCalculation(
        string input,
        double firstOperand,
        double secondOperand,
        double expectedResult,
        string expectedOutput)
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput(input, calculator.Power);

        Assert.Contains(expectedOutput, output);
        VerifyLog(loggerMock, "Exponentiation", firstOperand, secondOperand, expectedResult);
    }

    [Fact]
    public void PrintResult_WithUnknownOperator_CoversDefaultSwitchBranch()
    {
        var loggerMock = new Mock<ICalculationLogger>();
        var calculator = new Calculate(loggerMock.Object);

        string output = RunWithConsoleInput(
            "",
            () => InvokePrintResult(calculator, "Unknown", "?", "2", "3"));

        Assert.Contains("2 ? 3 = 1", output);
        VerifyLog(loggerMock, "Unknown", 2, 3, 1);
    }

    private static string RunWithConsoleInput(string input, Action action)
    {
        lock (ConsoleLock)
        {
            TextReader originalIn = Console.In;
            TextWriter originalOut = Console.Out;
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
            CultureInfo originalUiCulture = CultureInfo.CurrentUICulture;

            using StringReader reader = new(input);
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
            }
        }
    }

    private static void InvokePrintResult(
        Calculate calculator,
        string title,
        string sign,
        string firstOperand,
        string secondOperand)
    {
        MethodInfo? method = typeof(Calculate).GetMethod(
            "PrintResult",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(method);
        method.Invoke(calculator, [title, sign, firstOperand, secondOperand]);
    }

    private static void VerifyLog(
        Mock<ICalculationLogger> loggerMock,
        string operation,
        double firstOperand,
        double secondOperand,
        double result)
    {
        loggerMock.Verify(
            logger => logger.Log(
                operation,
                firstOperand,
                secondOperand,
                It.Is<double>(actual => Math.Abs(actual - result) < 0.000001)),
            Times.Once);
        loggerMock.VerifyNoOtherCalls();
    }
}
