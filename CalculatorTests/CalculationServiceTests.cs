using System.Collections.Generic;
using System.Linq;
using CalculatorApp.Services;
using CalculatorApp.Services.Interfaces;
using Moq;
using Xunit;

namespace CalculatorTests;

public class CalculationServiceTests
{
    [Fact]
    public void Calculate_ReturnsWithCorrectData()
    {
        const string expression = "1 + 1";
        var convertMock = new Mock<IConvertService>();
        convertMock.Setup(x => x.ConvertToPostFix(expression)).Returns(GetPostfixList());
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.Calculate(expression);
        
        Assert.True(result.SequenceEqual(GetResult()));
    }

    [Fact]
    public void ExecuteOperation_ReturnsCorrectResultWhenPlus()
    {
        var convertMock = new Mock<IConvertService>();
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.ExecuteOperation("+", 1, 2);
        
        Assert.Equal(3, result);
    }
    
    [Fact]
    public void ExecuteOperation_ReturnsCorrectResultWhenMinus()
    {
        var convertMock = new Mock<IConvertService>();
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.ExecuteOperation("-", 3, 1);
        
        Assert.Equal(2, result);
    }
    
    [Fact]
    public void ExecuteOperation_ReturnsCorrectResultWhenUnaryMinus()
    {
        var convertMock = new Mock<IConvertService>();
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.ExecuteOperation("~", 0, 1);
        
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public void ExecuteOperation_ReturnsCorrectResultWhenMultiplication()
    {
        var convertMock = new Mock<IConvertService>();
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.ExecuteOperation("*", 2, 2);
        
        Assert.Equal(4, result);
    }
    
    [Fact]
    public void ExecuteOperation_ReturnsCorrectResultWhenDivision()
    {
        var convertMock = new Mock<IConvertService>();
        var calculationService = new CalculationService(convertMock.Object);

        var result = calculationService.ExecuteOperation("/", 2, 2);
        
        Assert.Equal(1, result);
    }

    private static List<string> GetResult() => new() { "1 + 1 = 2", "Result: 2" };

    private static List<string> GetPostfixList() => new() { "1", "1", "+" };
}