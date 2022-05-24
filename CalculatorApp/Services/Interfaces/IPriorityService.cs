namespace CalculatorApp.Services.Interfaces;

public interface IPriorityService
{
    public bool OperationPriorityCheck(char topOfTheStack, char token);

    public bool ContainsOperation(string token);
}