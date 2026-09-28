namespace Calculator.Tests;

public class UtilityTests
{
    [Fact]
    public void OperationMenu_PrintsAllOperations()
    {
        string output = ConsoleTestHelper.Run("", Utility.OperationMenu);

        Assert.Contains("to Add", output);
        Assert.Contains("to Subtract", output);
        Assert.Contains("to Multiply", output);
        Assert.Contains("to Divide", output);
        Assert.Contains("to Power", output);
    }

    [Fact]
    public void OperationChoice_WithInput_ReturnsUserResponse()
    {
        string? result = null;

        string output = ConsoleTestHelper.Run("4\n", () => result = Utility.OperationChoice());

        Assert.Equal("4", result);
        Assert.Contains("Enter your choice", output);
    }

    [Fact]
    public void OperationChoice_WhenReadLineReturnsNull_ReturnsDefaultChoice()
    {
        string? result = null;

        ConsoleTestHelper.Run("", () => result = Utility.OperationChoice());

        Assert.Equal("100", result);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("5", 5)]
    public void ValidateInput_WithValidMenuChoice_ReturnsChoice(string input, int expected)
    {
        int result = Utility.ValidateInput(input, 1, 5);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("6")]
    public void ValidateInput_WithOutOfRangeChoice_PrintsMenuErrorAndReturnsZero(string input)
    {
        int result = -1;

        string output = ConsoleTestHelper.Run("", () => result = Utility.ValidateInput(input, 1, 5));

        Assert.Equal(0, result);
        Assert.Contains("Please enter a valid choice between 1 and 5", output);
    }

    [Fact]
    public void ValidateInput_WithNonIntegerChoice_PrintsMenuErrorAndReturnsZero()
    {
        int result = -1;

        string output = ConsoleTestHelper.Run("", () => result = Utility.ValidateInput("abc", 1, 5));

        Assert.Equal(0, result);
        Assert.Contains("Please enter a valid choice between 1 and 5", output);
    }

    [Fact]
    public void TryAgain_WithYesInput_ReturnsY()
    {
        string? result = null;

        string output = ConsoleTestHelper.Run(" y \n", () => result = Utility.TryAgain());

        Assert.Equal("Y", result);
        Assert.Contains("Try Again", output);
    }

    [Fact]
    public void TryAgain_WithInvalidThenNoInput_PrintsErrorAndReturnsN()
    {
        string? result = null;

        string output = ConsoleTestHelper.Run("maybe\nN\n", () => result = Utility.TryAgain());

        Assert.Equal("N", result);
        Assert.Contains("Enter choice between 'Y' and 'N'", output);
    }

    [Fact]
    public void TryAgain_WhenReadLineReturnsNullThenNo_PrintsErrorAndReturnsN()
    {
        string? result = null;
        using SequenceTextReader reader = new(null, "N");

        string output = ConsoleTestHelper.Run(reader, () => result = Utility.TryAgain());

        Assert.Equal("N", result);
        Assert.Contains("Enter choice between 'Y' and 'N'", output);
    }

    [Theory]
    [InlineData("all", "Calculator")]
    [InlineData("top", "┌")]
    [InlineData("bottom", "└")]
    public void DisplayTitle_PrintsRequestedCover(string cover, string expectedOutput)
    {
        string output = ConsoleTestHelper.Run("", () => Utility.DisplayTitle("Calculator", cover));

        Assert.Contains(expectedOutput, output);
    }

    [Fact]
    public void PrintCenteredTitle_ReturnsCenteredTitleWithinBox()
    {
        string result = Utility.PrintCenteredTitle("Hi", 10);

        Assert.StartsWith(" │", result);
        Assert.Contains("Hi", result);
        Assert.EndsWith("│ ", result);
    }

    [Fact]
    public void ApplyHighlighter_WritesHighlightedText()
    {
        string output = ConsoleTestHelper.Run("", () => Utility.ApplyHighlighter("hello"));

        Assert.Contains("hello", output);
        Assert.Contains("\u001b[", output);
    }

    [Theory]
    [InlineData("10", true)]
    [InlineData("abc", false)]
    public void IsDouble_ReturnsWhetherInputCanBeParsedAsDouble(string input, bool expected)
    {
        Assert.Equal(expected, Utility.IsDouble(input));
    }

    [Theory]
    [InlineData("menu", "Please enter a valid choice between 1 and 5")]
    [InlineData("tryAgain", "Enter choice between 'Y' and 'N'")]
    [InlineData("operate", "Unable to perform operation")]
    public void DisplayInputError_PrintsExpectedErrorMessage(string errorType, string expectedOutput)
    {
        string output = ConsoleTestHelper.Run(
            "",
            () => Utility.DisplayInputError("1", "5", errorType));

        Assert.Contains(expectedOutput, output);
    }

    [Fact]
    public void DisplayInputError_WithUnknownErrorType_DoesNotWriteOutput()
    {
        string output = ConsoleTestHelper.Run("", () => Utility.DisplayInputError("1", "5", "unknown"));

        Assert.Equal(string.Empty, output);
    }

    [Fact]
    public void DisplayTitleAndFormula_PrintsTitleAndFormula()
    {
        string output = ConsoleTestHelper.Run(
            "",
            () => Utility.DisplayTitleAndFormula("Addition", "Formula: sum = a + b"));

        Assert.Contains("Addition", output);
        Assert.Contains("Formula: sum = a + b", output);
    }

    [Fact]
    public void FormatNumber_WithWholeNumber_ReturnsIntegerText()
    {
        Assert.Equal("5", Utility.FormatNumber(5));
    }

    [Fact]
    public void FormatNumber_WithFractionalNumber_ReturnsFourDecimalPlaces()
    {
        Assert.Equal(2.5.ToString("0.0000"), Utility.FormatNumber(2.5));
    }

    [Fact]
    public void MathOperation_WhenAlreadyValidInput_SkipsPromptAndReturnsZero()
    {
        int result = Utility.MathOperation(true, 0, 5);

        Assert.Equal(0, result);
    }

    [Fact]
    public void MathOperation_WithValidChoice_ReturnsOperation()
    {
        int result = -1;

        ConsoleTestHelper.Run("4\n", () => result = Utility.MathOperation(false, 0, 5));

        Assert.Equal(4, result);
    }

    [Fact]
    public void MathOperation_WithInvalidThenValidChoice_RetriesAndReturnsOperation()
    {
        int result = -1;

        string output = ConsoleTestHelper.Run("bad\n2\n", () => result = Utility.MathOperation(false, 0, 5));

        Assert.Equal(2, result);
        Assert.Contains("Please enter a valid choice between 1 and 5", output);
    }

    [Fact]
    public void MathOperation_WhenMaximumAttemptsReached_PrintsErrorAndReturnsZero()
    {
        int result = -1;

        string output = ConsoleTestHelper.Run("bad\nbad\n", () => result = Utility.MathOperation(false, 0, 2));

        Assert.Equal(0, result);
        Assert.Contains("Maximum number of attempts has been reached", output);
        Assert.Contains("2 / 2", output);
    }
}
