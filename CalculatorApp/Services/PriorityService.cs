using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class PriorityService : IPriorityService
{
    private readonly Dictionary<string, int> _operationPriority = new() {
        {"(", 0},
        {"+", 1},
        {"-", 1},
        {"*", 2},
        {"/", 2},
        {"~", 3}
    };
    
    public bool OperationPriorityCheck(char topOfTheStack, char token) => 
        _operationPriority[topOfTheStack.ToString()] >= _operationPriority[token.ToString()];

    public bool ContainsOperation(string token) => _operationPriority.ContainsKey(token);
}