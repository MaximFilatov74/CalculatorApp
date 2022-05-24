namespace CalculatorApp.Services.Interfaces;

public interface IVerificationService
{
    public bool IsOpeningBracket(char symbol);

    public bool IsClosingBracket(char symbol);

    public bool IsPointOrComa(char symbol);

    public bool IsUnaryOperation(string symbol);
}