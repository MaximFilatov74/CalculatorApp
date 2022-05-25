namespace CalculatorApp.Services.Interfaces;

public interface IConvertService
{
    public List<string> ConvertToPostFix(string expression);
}