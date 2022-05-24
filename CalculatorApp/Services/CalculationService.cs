using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class CalculationService : ICalculationService
{
    private readonly IParseService _parseService;
    private readonly IVerificationService _verificationService;
    private readonly IPriorityService _priorityService;

    public CalculationService(
        IParseService parseService, 
        IVerificationService verificationService, 
        IPriorityService priorityService)
    {
        _parseService = parseService;
        _verificationService = verificationService;
        _priorityService = priorityService;
    }

    public List<string> Calculate(string expression)
    {
        var postfixList = _parseService.ConvertToPostFix(expression);
        var calculations = new Stack<double>();
        var results = new List<string>();

        foreach (var token in postfixList)
        {
            if (double.TryParse(token, out var number))
            {
                calculations.Push(number);
            } 
            else if (_priorityService.ContainsOperation(token))
            {
                if (_verificationService.IsUnaryOperation(token))
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
}