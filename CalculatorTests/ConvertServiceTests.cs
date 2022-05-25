using System.Collections.Generic;
using System.Linq;
using CalculatorApp.Services;
using Xunit;

namespace CalculatorTests;

public class ConvertServiceTests
{
    [Fact]
    public void ConvertToPostFix_CorrectPostfixForm()
    {
        const string expression = "2 + 2";
        var convertService = new ConvertService();

        var result = convertService.ConvertToPostFix(expression);
        
        Assert.True(result.SequenceEqual(PostfixExpressionList()));
    }
    
    private static List<string> PostfixExpressionList() => new() { "2", "2", "+" };
}