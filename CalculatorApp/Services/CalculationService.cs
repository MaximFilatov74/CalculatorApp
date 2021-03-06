using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class CalculationService : ICalculationService
{
    private readonly IConvertService _convertService;
    private readonly List<string> _operationsList = new() { "+", "-", "~", "*", "/" };

    public CalculationService(IConvertService convertService)
    {
        _convertService = convertService;
    }

    public List<string> Calculate(string expression)
    {
        var postfixList = _convertService.ConvertToPostFix(expression);
        var calculations = new Stack<double>();
        var results = new List<string>();

        foreach (var token in postfixList)
        {
            if (double.TryParse(token, out var number))
            {
                calculations.Push(number);
            } 
            else if (ContainsOperation(token))
            {
                if (IsUnaryOperation(token))
                {
                    var numberWithUnaryOp = calculations.Count > 0 ? calculations.Pop() : 0;
                    calculations.Push(ExecuteOperation(token, 0, numberWithUnaryOp));
                    results.Add($"{token}{numberWithUnaryOp} = {calculations.Peek()}");
                }
                else
                {
                    var secondNumber = calculations.Count > 0 ? calculations.Pop() : 0;
                    var firstNumber = calculations.Count > 0 ? calculations.Pop() : 0;
                    
                    calculations.Push(ExecuteOperation(token, firstNumber, secondNumber));
                    results.Add($"{firstNumber} {token} {secondNumber} = {calculations.Peek()}");
                }
            }
        }
        
        results.Add(calculations.Count > 0 ? $"Result: {calculations.Pop()}" : string.Empty);
        return results;
    }

    public double ExecuteOperation(string operation, double firstNumber, double secondNumber)
    {
        return operation switch
        {
            "+" => firstNumber + secondNumber,
            "-" => firstNumber - secondNumber,
            "~" => firstNumber - secondNumber,
            "*" => firstNumber * secondNumber,
            "/" => firstNumber / secondNumber,
            _ => 0
        };
    }

    private static bool IsUnaryOperation(string symbol) => symbol.Equals("~");

    private bool ContainsOperation(string symbol) => _operationsList.Contains(symbol);
}