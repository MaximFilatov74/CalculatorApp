using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class VerificationService : IVerificationService
{
    public bool IsOpeningBracket(char symbol) => symbol.Equals('(');
    
    public bool IsClosingBracket(char symbol) => symbol.Equals(')');

    public bool IsPointOrComa(char symbol) => symbol.Equals('.') || symbol.Equals(',');

    public bool IsUnaryOperation(string symbol) => symbol.Equals("~");
}