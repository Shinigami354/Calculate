namespace Calculator;

/// <summary>Main calculator application entry point</summary>
/// <remarks>
/// <para>Author: Eddie C.</para>
/// <para>Version: 1.0</para>
/// <para>Since: 2025-11-09</para>
/// </remarks>
public class Program
{
    /// <summary>Starts and runs the calculator application</summary>
    /// <param name="args">Command line arguments</param>
    public static void Main(string[] args)
    {
        Calculate calculator = new(new CalculationLogger());

        while (true)
        {
            const int ATTEMPT_COUNTER_START = 0;
            const int CATCHALL_FOR_INVALID_INPUT = 0;
            const int MAX_ATTEMPTS = 5;
            Utility.DisplayTitle("Calculator", "all");
            Utility.OperationMenu();

            int operationToPerform = Utility.MathOperation(false,
                ATTEMPT_COUNTER_START,
                MAX_ATTEMPTS);

            if (operationToPerform == CATCHALL_FOR_INVALID_INPUT) return;

            CallCalculate(operationToPerform, calculator);

            if (Utility.TryAgain() == "N") return;
        }
    }

    /// <summary>Calls specific calculation based on user selection</summary>
    /// <param name="operationToPerform">Operation number to execute</param>
    private static void CallCalculate(int operationToPerform, Calculate calculator)
    {
        switch (operationToPerform)
        {
            case 1:
                calculator.Add();
                break;
            case 2:
                calculator.Subtract();
                break;
            case 3:
                calculator.Multiply();
                break;
            case 4:
                calculator.Divide();
                break;
            case 5:
                calculator.Power();
                break;
            default:
                break;
        }
    }
}
