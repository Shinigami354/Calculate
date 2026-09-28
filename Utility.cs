namespace Calculator;

/// <summary>Shows available mathematical operations </summary>
internal enum Operation
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Power
}

/// <summary>
/// Provides utility methods for calculator operations including menu display,
/// input validation, error handling, and user interface formatting.
/// </summary>
/// <remarks>
/// <para>Author: Eddie C.</para>
/// <para>Version: 1.0</para>
/// <para>Since: 2025-11-09</para>
/// </remarks>
public static class Utility
{
    /// <summary>Displays the main operation menu with available mathematical operations</summary>
    public static void OperationMenu()
    {
        Operation[] operations = Enum.GetValues<Operation>();
        string[] symbols = ["(+)", "(-)", "(*)", "(/)", "(^)"];

        DisplayTitle("", "top");

        for (int i = 0; i < operations.Length; i++)
        {
            string tabStops = i > 0 ? new string(' ', 14) : "   Type\t";
            string centerThis = $"{tabStops}{i + 1}   - to {operations[i]}     \t{symbols[i]}    ";
            WriteLine(PrintCenteredTitle(centerThis, 29));
        }

        DisplayTitle("", "bottom");
    }

    /// <summary>Prompts user for operation choice and returns the input</summary>
    /// <returns>User input as string, defaults to "100" if input is null</returns>
    public static string OperationChoice()
    {
        Write("  Enter your choice \t: ");
        string response = ReadLine() ?? "100";
        return response;
    }

    /// <summary>Validates user input for menu selection</summary>
    /// <param name="input">The user input to validate</param>
    /// <param name="choiceStart">Minimum valid choice (inclusive)</param>
    /// <param name="choiceEnd">Maximum valid choice (inclusive)</param>
    /// <returns>Validated integer between choiceStart and choiceEnd, or 0 if invalid</returns>
    public static int ValidateInput(string input, int choiceStart, int choiceEnd)
    {
        try
        {
            int result = Convert.ToInt32(input);
            if (result is >= 1 and <= 5)
                return result;

            DisplayInputError(choiceStart.ToString(),
                choiceEnd.ToString(),
                "menu");
            return 0;
        }

        catch
        {
            DisplayInputError(choiceStart.ToString(),
                choiceEnd.ToString(),
                "menu");
            return 0;
        }
    }


    /// <summary>Prompts user to try again and validates Y/N response</summary>
    /// <returns>Validated user response as "Y" or "N"</returns>
    public static string TryAgain()
    {
        while (true)
        {
            Write("  Try Again (Y/N)\t: ");
            string response = (ReadLine() ?? "").Trim().ToUpper();

            switch (response)
            {
                case "Y":
                case "N":
                    return response;
                default:
                    DisplayInputError("", "", "tryAgain");
                    break;
            }
        }
    }

    /// <summary> Displays formatted title with decorative borders</summary>
    /// <param name="title">The title text to display</param>
    /// <param name="cover">Border type: "all", "top", or default for bottom</param>
    public static void DisplayTitle(string title, string cover)
    {
        string lineBoxTop = new('─', 45);
        const string cornerLeftTop = " ┌";
        const string cornerRightTop = "┐ ";
        const string cornerLeftBottom = " └";
        const string cornerRightBottom = "┘ ";

        string top = cornerLeftTop + lineBoxTop + cornerRightTop;
        string bottom = cornerLeftBottom + lineBoxTop + cornerRightBottom;


        switch (cover)
        {
            case "all":
                string middle = PrintCenteredTitle(title, 45);
                ApplyHighlighter(top);
                ApplyHighlighter(middle);
                ApplyHighlighter(bottom);
                break;

            case "top":
                WriteLine(top);
                break;

            default:
                WriteLine(bottom);
                break;
        }
    }

    /// <summary>Centers text within a specified width for display formatting</summary>
    /// <param name="title">Text to center</param>
    /// <param name="width">Total width for centering</param>
    /// <returns>Formatted centered string with border characters</returns>
    public static string PrintCenteredTitle(string title, int width)
    {
        int availableWidth = width;

        string centeredTitle = string.Format(" │{0,-" + availableWidth + "}│ ",
            title.PadLeft((availableWidth + title.Length) / 2).PadRight(availableWidth));
        return centeredTitle;
    }


    /// <summary>Applies ANSI color highlighting to text for console display</summary>
    /// <param name="text">Text to apply highlighting to</param>
    public static void ApplyHighlighter(string text)
    {
        text = AnsiColorCodes.Background + AnsiColorCodes.Foreground + text + AnsiColorCodes.Reset;
        WriteLine(text);
    }

    public static bool IsDouble(string input)
    {
        double result;
        return double.TryParse(input, out result);
    }

    /// <summary>Displays formatted error messages based on error type</summary>
    /// <param name="number1">First number/parameter for error context</param>
    /// <param name="number2">Second number/parameter for error context</param>
    /// <param name="errorType">Type of error: "menu", "tryAgain", "operate", or default</param>
    public static void DisplayInputError(string number1, string number2, string errorType)
    {
        switch (errorType)
        {
            case "menu":
            {
                WriteLine($"{AnsiColorCodes.Error} Please enter a valid choice between " +
                          $"{number1} and {number2}. {AnsiColorCodes.Reset}\n");
                break;
            }

            case "tryAgain":
            {
                WriteLine(
                    $"{AnsiColorCodes.Error} Error: Enter choice between 'Y' and 'N'  " +
                    $"{AnsiColorCodes.Reset}\n");
                break;
            }

            case "operate":
            {
                const string errorLine1 =
                    "Error: Unable to perform operation with the given inputs: ";
                const string errorLine2 =
                    "Expected: Both inputs should be of the same type - numbers.";

                WriteLine($"\n{AnsiColorCodes.Error} {errorLine1} `{number1}` and `{number2}`. " +
                          $"{AnsiColorCodes.Reset}");
                WriteLine($"{AnsiColorCodes.Error} {errorLine2} {AnsiColorCodes.Reset}\n");
                break;
            }

            default:
            {
                break;
            }
        }
    }

    /// <summary>Displays operation title and formula with decorative formatting</summary>
    /// <param name="title">Operation title to display</param>
    /// <param name="formula">Mathematical formula to display</param>
    public static void DisplayTitleAndFormula(string title, string formula)
    {
        WriteLine();
        DisplayTitle("", "top");
        WriteLine(PrintCenteredTitle(title, 45));
        WriteLine(PrintCenteredTitle("", 45));
        WriteLine(PrintCenteredTitle("", 45));
        WriteLine(PrintCenteredTitle(formula, 45));
        DisplayTitle("", "bottom");
    }

    /// <summary>Formats numeric results with appropriate precision</summary>
    /// <param name="result">Numeric value to format</param>
    /// <returns> Whole number as integer string if whole number, otherwise formatted
    /// to 4 decimal places </returns>
    public static string FormatNumber(double result)
    {
        if (result % 1 == 0)
            return result.ToString("0");
        else
            return result.ToString("0.0000");
    }


    /// <summary>Handles mathematical operation selection with input validation and retry
    /// logic</summary>
    /// <param name="isValidInput">Initial validation state</param>
    /// <param name="attemptCounter">Current attempt count</param>
    /// <param name="maxAttempt">Maximum number of allowed attempts</param>
    /// <returns>Valid operation code (1-5) if successful, 0 if maximum attempts reached</returns>
    public static int MathOperation(bool isValidInput, int attemptCounter, int maxAttempt)
    {
        while (!isValidInput)
        {
            string choice = OperationChoice();
            int operation = ValidateInput(choice, 1, 5);
            attemptCounter++;

            if (operation != 0)
            {
                isValidInput = true;
                return operation;
            }

            if (attemptCounter >= maxAttempt)
            {
                isValidInput = true;
                //SetCursorPosition(29, CursorTop);
                WriteLine(
                    $"{AnsiColorCodes.Error} Maximum number of attempts has been reached. " +
                    $"{attemptCounter} / {maxAttempt} {AnsiColorCodes.Reset}");
            }
        }

        return 0;
    }
}