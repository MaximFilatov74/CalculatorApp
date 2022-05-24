namespace CalculatorApp.Services.Interfaces;

public interface ICalculationService
{
    public List<string> Calculate(string expression);

    public double ExecuteOperation(string operation, double firstNumber, double secondNumber);
}