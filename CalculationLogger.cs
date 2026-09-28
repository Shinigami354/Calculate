namespace Calculator;

public class CalculationLogger : ICalculationLogger
{
    public void Log(string operation, double firstOperand, double secondOperand, double result)
    {
        System.Diagnostics.Trace.WriteLine(
            $"{operation}: {firstOperand} and {secondOperand} = {result}");
    }
}
