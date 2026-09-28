namespace Calculator;

public interface ICalculationLogger
{
    void Log(string operation, double firstOperand, double secondOperand, double result);
}
