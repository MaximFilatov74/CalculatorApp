namespace CalculatorApp.Services.Interfaces;

public interface IParseService
{
    public List<string> ConvertToPostFix(string expression);
}